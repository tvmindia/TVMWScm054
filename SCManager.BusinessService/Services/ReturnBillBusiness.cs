using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.RepositoryServices.Contracts;
using SCManager.BusinessService.Contracts;
using System.Reflection;

namespace SCManager.BusinessService.Services
{
    public class ReturnBillBusiness:IReturnBillBusiness
    {
        private IReturnBillRepository _returnBillRepository;
        private ICommonBusiness _commonBusiness;

        public ReturnBillBusiness(IReturnBillRepository returnBillRepository, ICommonBusiness commonBusiness)
        {
            _returnBillRepository = returnBillRepository;
            _commonBusiness = commonBusiness;
        }

        public List<ReturnBill> GetAllReturnBill(UA UA)
        {
            List<ReturnBill> ReturnBilllist = null;
            try
            {

                ReturnBilllist = _returnBillRepository.GetAllReturnBill(UA);
                if (ReturnBilllist != null)
                {
                    foreach (ReturnBill F in ReturnBilllist)
                    {

                        ReturnBillBL(F);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            return ReturnBilllist;
        }


        public List<ReturnBill> GetAllFranchiseeDetail(UA UA)
        {
            List<ReturnBill> ReturnBilllist = null;
            try
            {

                ReturnBilllist = _returnBillRepository.GetAllFranchiseeDetail(UA);
                if (ReturnBilllist != null)
                {
                    foreach (ReturnBill F in ReturnBilllist)
                    {

                        ReturnBillBL(F);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            return ReturnBilllist;
        }

        public List<ReturnBill> GetSupplierDetail(UA UA)
        {
            List<ReturnBill> ReturnBilllist = null;
            try
            {

                ReturnBilllist = _returnBillRepository.GetSupplierDetail(UA);
                if (ReturnBilllist != null)
                {
                    foreach (ReturnBill F in ReturnBilllist)
                    {

                        ReturnBillBL(F);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            return ReturnBilllist;
        }


        public List<ReturnBillDetail> GetMaterialsFromDefectiveDamaged(string TicketNo, string SCCode)
        {
            ReturnBill result = new ReturnBill();
            result.ReturnBillDetail = new List<ReturnBillDetail>();
            result.ReturnBillDetail = _returnBillRepository.GetMaterialsFromDefectiveDamaged(TicketNo, SCCode);
            if (result != null)
                ReturnBillDetailBL(result);
            return result.ReturnBillDetail;

        }


        public ReturnBill InsertUpdate(ReturnBill rtb, UA UA)
        {
            ReturnBill result = null;
            try
            {
                rtb.DetailXML = _commonBusiness.GetXMLfromReturnBill(rtb.ReturnBillDetail, "MaterialID", UA);
                if (rtb.ID == null || rtb.ID == Guid.Empty)
                {
                    result = _returnBillRepository.InsertReturnBill(rtb, UA);
                }
                else
                {
                    result = _returnBillRepository.UpdateReturnBill(rtb, UA);

                }

                //--------BL works----------------------
                if (result != null)
                {
                    ReturnBillBL(result);
                    ReturnBillDetailBL(result);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }

        private void ReturnBillBL(ReturnBill F)
        {
            if (F != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
               
                if (F.InvoiceDate != null)
                   F.InvoiceDateFormatted = F.InvoiceDate;//.ToString(settings.dateformat);
            }

        }


        private void ReturnBillDetailBL(ReturnBill RB)
        {
            List<ReturnBillDetail> List = RB.ReturnBillDetail;
            decimal t1;
            decimal t2;
            decimal net;
            RB.TotalTaxAmount = 0;
            RB.GrandTotal = 0;
            if (List != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
                int slno = 1;
                foreach (ReturnBillDetail F in List)
                {
                    F.SlNo = slno;
                    F.BasicAmount = F.Quantity * F.Rate;
                    F.NetAmount = F.BasicAmount - F.TradeDiscount;
                    F.CGSTAmount = (((F.Quantity * F.Rate) - (F.TradeDiscount)) * (F.CGSTPercentage / 100));
                    F.SGSTAmount = (((F.Quantity * F.Rate) - (F.TradeDiscount)) * (F.SGSTPercentage / 100));
                    F.TotalAmount = F.NetAmount + F.CGSTAmount + F.SGSTAmount;
                    t1 = (F.CGSTAmount ?? 0);
                    t2 = F.SGSTAmount ?? 0;
                    net = F.NetAmount ?? 0;
                    RB.TotalTaxAmount = RB.TotalTaxAmount + t1 + t2;//.Total Tax Amount calculation
                    RB.GrandTotal = RB.GrandTotal + net + t1 + t2;//. Grand Total calculation
                    slno = slno + 1;
                }
            }

        }

        public bool DeleteReturnBillDetail(Guid ID, Guid HeaderID, UA UA)
        {
            return _returnBillRepository.DeleteReturnBillDetail(ID, HeaderID, UA);
        }

        public bool DeleteReturnBill(Guid ID, UA UA)
        {
            return _returnBillRepository.DeleteReturnBill(ID, UA);
        }

        public List<ReturnBill> GetAllTicketNo(UA UA)
        {
            return _returnBillRepository.GetAllTicketNo(UA);
        }

        public ReturnBill GetReturnBill(Guid ID, UA ua)
        {
            try
            {
                ReturnBill Result = new ReturnBill();
                Result = _returnBillRepository.GetReturnBillHeader(ID, ua);
                Result.ReturnBillDetail = _returnBillRepository.GetReturnBillDetail(ID, ua);
                ReturnBillBL(Result);
                ReturnBillDetailBL(Result);
                return Result;
            }
            catch (Exception)
            {

                throw;
            }
        }



        public ReturnBill GetReturnBillHeaderByID(Guid ID, UA ua)
        {
            try
            {
                ReturnBill Result = new ReturnBill();
                Result = _returnBillRepository.GetReturnBillHeader(ID, ua);
                Result.ReturnBillDetail = _returnBillRepository.GetReturnBillDetail(ID, ua);
                ReturnBillBL(Result);
                ReturnBillDetailBL(Result);
                if (Result.ReturnBillDetail != null)
                {
                    ReturnBillBL(Result);
                }
                return Result;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public string ReturnDefectiveDamaged(string ID, UA ua,string TicketNo)
        {
            string status = null;
            try
            {
                status = _returnBillRepository.ReturnDefectiveDamaged(ID, ua,TicketNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
    }
}