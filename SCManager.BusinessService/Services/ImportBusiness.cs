﻿using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class ImportBusiness: IImportBusiness
    {
        private IImportRepository _importRepository;
        private IItemBusiness _itemBusiness;
        private IForm8TaxInvoiceBusiness _form8TaxInvoiceBusiness;
        SCManagerSettings settings = new SCManagerSettings();
        public ImportBusiness(IImportRepository importRepository, IItemBusiness itemBusiness, IForm8TaxInvoiceBusiness form8TaxInvoiceBusiness)
        {
            _importRepository = importRepository;
            _itemBusiness = itemBusiness;
            _form8TaxInvoiceBusiness = form8TaxInvoiceBusiness;
        }

        public List<UploadedFiles> GetAllUploadedFile()
        {
            return _importRepository.GetAllUploadedFile();
        }

        public UploadedFiles InsertAttachment(UploadedFiles uploadedFile)
        {
            uploadedFile.FileStatus = "Unvalidated";
            return _importRepository.InsertAttachment(uploadedFile);
        }

        public UploadedFiles ValidateImportData(UploadedFiles uploadedFile, string filePath, UA ua)
        {
            List<ImportForm8> removedData = new List<ImportForm8>();
            List<ImportForm8> importForm8List = new List<ImportForm8>();
            string extension = Path.GetExtension(filePath);
            try
            {
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        DataTable ExcelDataXls= _importRepository.GetExcelDataToTable(uploadedFile, filePath, 1);
                        importForm8List = ConvertTableToList(ExcelDataXls);
                        uploadedFile.RecordCount = importForm8List.Count;
                        uploadedFile.FileStatus = "Validated";
                        uploadedFile = _importRepository.UpdateUploadedFileDetail(uploadedFile, ua, importForm8List.Count);
                        importForm8List = FetchMaterialID(importForm8List, ua);
                        importForm8List = CheckIfInvoiceExists(importForm8List, ua);
                        removedData = (from removed in importForm8List where removed.Error != "" select removed).ToList();
                        uploadedFile.RemovedDataCount = removedData.Count;
                        uploadedFile.Form8List = (removedData.Count != 0) ? removedData : importForm8List;
                        break;
                    case ".xlsx": //Excel 07 to 12
                        DataTable ExcelDataXlsx= _importRepository.GetExcelDataToTable(uploadedFile, filePath, 2);
                        importForm8List = ConvertTableToList(ExcelDataXlsx);
                        uploadedFile.RecordCount = importForm8List.Count;
                        uploadedFile.FileStatus = "Validated";
                        uploadedFile = _importRepository.UpdateUploadedFileDetail(uploadedFile, ua, importForm8List.Count);
                        importForm8List = FetchMaterialID(importForm8List, ua);
                        importForm8List = CheckIfInvoiceExists(importForm8List, ua);
                        removedData = (from removed in importForm8List where removed.Error != "" select removed).ToList();
                        uploadedFile.RemovedDataCount = removedData.Count;
                        uploadedFile.Form8List = (removedData.Count != 0) ? removedData : importForm8List;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return uploadedFile;
        }

        #region ValidateMandatoryFields
        void ValidateMandatoryFields(DataTable excelData)
        {
            List<string> errorList = null;
            foreach (DataRow row in excelData.Rows)
            {
                errorList = new List<string>();
                if (row["InvoiceNo"].ToString() == "" || row["InvoiceNo"].ToString() == null)
                    errorList.Add("No Invoice");
                if (row["InvoiceDate"].ToString() == "" || row["InvoiceDate"].ToString() == null)
                    errorList.Add("No InvoiceDate");
                if (row["PONo"].ToString() == "" || row["PONo"].ToString() == null)
                    errorList.Add("No PONo");
                if (row["Item"].ToString() == "" || row["Item"].ToString() == null)
                    errorList.Add("No Item");
                if (row["Quantity"].ToString() == "" || row["Quantity"].ToString() == null)
                    errorList.Add("No Quantity");


                row["Error"] = (errorList.Count > 0 ? errorList[0] : "");
                for(int i=1;i<errorList.Count;i++)
                {
                    row["Error"] += ", "+errorList[i];
                }
            }
        }
        #endregion ValidateMandatoryFields

        #region ValidateFields
        void ValidateRowData(DataTable excelData)
        {
            List<string> errorList = null;
            foreach (DataRow row in excelData.Rows)
            {
                errorList = new List<string>();
                string invoiceDate = row["InvoiceDate"].ToString();
                string challanDate = row["ChallanDate"].ToString();
                string poDate = row["PODate"].ToString();
                Regex datePattern = new Regex(@"^([0-3]?[1-9]|[1][0-2]|2[0-2]|3[0-2])[./-][A-Za-z]{3}[./-]([0-9]{4}|[0-9]{2})");
               // Regex datePattern = new Regex(@"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$");
                if (!string.IsNullOrEmpty(invoiceDate))
                {
                    if (!datePattern.IsMatch(invoiceDate))
                        errorList.Add("InvoiceDate Should be in (DD/MMM/YYYY)");
                }
                
                if(!string.IsNullOrEmpty(challanDate))
                {
                    if (!datePattern.IsMatch(challanDate))
                        errorList.Add("ChallanDate Should be in (DD/MMM/YYYY)");
                }
                
                if(!string.IsNullOrEmpty(poDate))
                {
                    if (!datePattern.IsMatch(poDate))
                        errorList.Add("PODate Should be in (DD/MMM/YYYY)");

                }

                string rate = row["Rate"].ToString();
                Regex decimalPattern = new Regex(@"[\d]{1,4}([.,][\d]{1,2})?");

                if(!string.IsNullOrEmpty(rate))
                {
                    if (!decimalPattern.IsMatch(rate))
                        errorList.Add("Rate is not in decimal format");

                }

                string quantity = row["Quantity"].ToString();
                Regex numberPattern = new Regex(@"^[0 - 9] *$");

                if(!string.IsNullOrEmpty(quantity))
                {
                    if (!decimalPattern.IsMatch(quantity))
                        errorList.Add("Quantity is not in Number Format");

                }

                row["Error"] += (errorList.Count > 0 ? errorList[0] : "");
                for (int i = 1; i < errorList.Count; i++)
                {
                    row["Error"] += errorList[i];
                }
            }
        }
        #endregion ValidateFields

        #region Fetch MaterialID
        List<ImportForm8> FetchMaterialID(List<ImportForm8> importForm8List, UA ua)
        {
            try
            {
                List<Item> itemList = _itemBusiness.GetAllItems(ua,"1");
                string[] itemNameList = (from i in itemList select i.Description.Replace(" [HSN:" + i.HsnNo + "]","").Trim()).ToArray();
                string[] itemCodeList = (from i in itemList select i.ItemCode.ToString().Trim()).ToArray();
                foreach (ImportForm8 importForm8 in importForm8List)
                {
                    if (!itemNameList.Contains(importForm8.Material.Trim()) && !itemCodeList.Contains(importForm8.Material.Trim()))
                    {
                        importForm8.Error += (importForm8.Error == "" ? "Item Unavailable" : ", Item Unavailable");
                    }
                    else
                    {
                        if (itemCodeList.Contains(importForm8.Material)) {
                            importForm8.MaterialID = (from i in itemList where i.ItemCode.Trim() == importForm8.Material.Trim() select i.ID).ToArray()[0];

                        }
                        else {
                            importForm8.MaterialID = (from i in itemList where i.Description.Replace(" [HSN:" + i.HsnNo + "]", "").Trim() == importForm8.Material.Trim() select i.ID).ToArray()[0];

                        }
                    }
                }
                return importForm8List;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion Fetch MaterialID

        #region ConvertTableToList
        List<ImportForm8> ConvertTableToList(DataTable excelData)
        {
            List<ImportForm8> importForm8List = new List<ImportForm8>();
            try
            {
                DataColumn addColumn = new DataColumn();
                addColumn.DataType = System.Type.GetType("System.String");
                addColumn.ColumnName = "Error";
                addColumn.ReadOnly = false;
                addColumn.AllowDBNull = true;

                excelData.Columns.Add(addColumn);
                ValidateMandatoryFields(excelData);
                ValidateRowData(excelData);

                for (int i = 0; i < excelData.Rows.Count; i++)
                {
                    DataRow row = excelData.Rows[i];
                    ImportForm8 form8 = new ImportForm8();
                    form8.InvoiceNo = row["InvoiceNo"].ToString().Trim();
                    if (!string.IsNullOrEmpty(row["InvoiceDate"].ToString().Trim()))
                    {


                        form8.InvoiceDate = DateTime.Parse(row["InvoiceDate"].ToString().Trim()).ToString(settings.dateformat);

                    }

                   
                    form8.SalesOrderNo = (row["SalesOrderNo"].ToString().Trim()) == null?"":row["SalesOrderNo"].ToString().Trim();
                    if(!string.IsNullOrEmpty(row["ChallanDate"].ToString().Trim()))
                    {
                        form8.ChallanDate = (row["ChallanDate"].ToString().Trim()) == null ? "" : DateTime.Parse(row["ChallanDate"].ToString().Trim()).ToString(settings.dateformat);
                    }
                    
                    form8.ChallanNo = (row["ChallanNo"].ToString().Trim())== null ? "" :row["ChallanNo"].ToString().Trim();
                    form8.PONo = row["PONo"].ToString().Trim();
                    if (!string.IsNullOrEmpty(row["PODate"].ToString().Trim()))
                    {
                        form8.PODate = (row["PODate"].ToString().Trim()) == null ? "" : DateTime.Parse(row["PODate"].ToString().Trim()).ToString(settings.dateformat);
                    }
                    form8.Material = row["Item"].ToString().Trim();
                    if(!string.IsNullOrEmpty(row["Quantity"].ToString().Trim()))
                    {
                        form8.Quantity = int.Parse(row["Quantity"].ToString().Trim());
                    }

                   
                    form8.Remarks = (row["Remarks"].ToString().Trim())== null ?"": row["Remarks"].ToString().Trim();

                  //if(row["Discount"].ToString().Trim() != null && row["Discount"].ToString().Trim() != "")
                  //      form8.Discount = decimal.Parse(row["Discount"].ToString().Trim());


                    if (row["TradeDiscount"].ToString().Trim() != null && row["TradeDiscount"].ToString().Trim() != "")
                        form8.TradeDiscount =  decimal.Parse(row["TradeDiscount"].ToString().Trim());
                    if (row["CGSTPercentage"].ToString().Trim() != null && row["CGSTPercentage"].ToString().Trim() != "")
                        form8.CGSTPercentage = decimal.Parse(row["CGSTPercentage"].ToString().Trim());

                    if (row["SGSTPercentage"].ToString().Trim() != null && row["SGSTPercentage"].ToString().Trim() != "")
                        form8.SGSTPercentage = decimal.Parse(row["SGSTPercentage"].ToString().Trim());
                    if (row["Rate"].ToString().Trim() != null && row["Rate"].ToString().Trim() != "")
                        form8.Rate = decimal.Parse(row["Rate"].ToString().Trim());
                    form8.ErrorRow = i;
                    form8.Error = row["Error"].ToString().Trim();

                    importForm8List.Add(form8);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return importForm8List;
        }
        #endregion ConvertTableToList

        #region CheckIfInvoiceExists
        List<ImportForm8> CheckIfInvoiceExists(List<ImportForm8> importForm8List, UA ua)
        {
            try
            {
                List<string> invoiceList = (from form8 in _form8TaxInvoiceBusiness.GetAllForm8(ua) select form8.InvoiceNo).ToList();
                foreach (ImportForm8 importForm in importForm8List)
                {
                    if (invoiceList.Contains(importForm.InvoiceNo))
                    {
                        importForm.Error += (importForm.Error == "" ? "Existing Invoice" : ", Existing Invoice");
                    }
                }
                return importForm8List;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion CheckIfInvoiceExists

        #region ImportDataFromList
        List<Form8> ImportDataFromList(List<ImportForm8> importForm8List)
        {
            List<Form8> form8List = new List<Form8>();
            List<string> distinctInvoices = (from m in importForm8List select m.InvoiceNo).Distinct().ToList();
            for (int i = 0; i < distinctInvoices.Count; i++)
            {
                List<ImportForm8> importFormList = (from import in importForm8List where import.InvoiceNo == distinctInvoices[i] select import).ToList();
                ImportForm8 importForm8 = importFormList.First();
                Form8 form8 = new Form8
                {
                    ID = Guid.Empty,
                    InvoiceNo = importForm8.InvoiceNo,
                    InvoiceDate = importForm8.InvoiceDate,
                    ChallanNo = importForm8.ChallanNo,
                    ChallanDate = importForm8.ChallanDate,
                    SaleOrderNo = importForm8.SalesOrderNo,
                    PONo = importForm8.PONo,
                    PODate = importForm8.PODate,
                   // Discount = importForm8.Discount,
                    Remarks = importForm8.Remarks,
                    Form8Detail = new List<Form8Detail>()
                };
                foreach (ImportForm8 importForm in importFormList)
                {
                    Form8Detail form8Detail = new Form8Detail
                    {
                        ID = Guid.Empty,
                        MaterialID = importForm.MaterialID,
                        Quantity = importForm.Quantity,
                        Rate = importForm.Rate,
                        TradeDiscount = importForm.TradeDiscount,
                        CGSTPercentage = importForm.CGSTPercentage,
                        SGSTPercentage = importForm.SGSTPercentage
                    };
                    form8.Form8Detail.Add(form8Detail);
                }
                form8List.Add(form8);
            }
            return form8List;
        }
        #endregion ImportDataFromList

        #region ImportDataToDB
        public UploadedFiles ImportDataToDB(UploadedFiles uploadedFile, string filePath, UA ua)
        {
            List<Form8> detailData = new List<Form8>();
            List<ImportForm8> importForm8List = null;
            string extension = Path.GetExtension(filePath);
            try
            {
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        DataTable ExcelDataXls= _importRepository.GetExcelDataToTable(uploadedFile, filePath, 1);
                        importForm8List = ConvertTableToList(ExcelDataXls);
                        importForm8List = FetchMaterialID(importForm8List, ua);
                        detailData = ImportDataFromList(importForm8List);
                        foreach (Form8 form in detailData)
                            _form8TaxInvoiceBusiness.InsertUpdate(form, ua);
                        uploadedFile.FileStatus = "Successfully Imported";
                        uploadedFile = _importRepository.UpdateUploadedFileDetail(uploadedFile, ua, importForm8List.Count);
                        uploadedFile.RemovedDataCount = 0;
                        uploadedFile.Form8List = importForm8List;
                        break;
                    case ".xlsx": //Excel 07 to 12
                        DataTable ExcelDataXlsx= _importRepository.GetExcelDataToTable(uploadedFile, filePath, 2);
                        importForm8List = ConvertTableToList(ExcelDataXlsx);
                        importForm8List = FetchMaterialID(importForm8List, ua);
                        detailData = ImportDataFromList(importForm8List);
                        foreach (Form8 form in detailData)
                            _form8TaxInvoiceBusiness.InsertUpdate(form, ua);
                        uploadedFile.FileStatus = "Successfully Imported";
                        uploadedFile = _importRepository.UpdateUploadedFileDetail(uploadedFile, ua, importForm8List.Count);
                        uploadedFile.RemovedDataCount = 0;
                        uploadedFile.Form8List = importForm8List;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return uploadedFile;
        }
        #endregion ImportDataToDB
    }
}