using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.RepositoryServices.Contracts;
using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;

namespace SCManager.BusinessService.Services
{
    public class Form8BRetailInvoiceBusiness: IForm8BRetailInvoiceBusiness
    {
        private IForm8BRetailInvoiceRepository _form8BRetailInvoiceRepository;
        private ICommonBusiness _commonBusiness;

        public Form8BRetailInvoiceBusiness(IForm8BRetailInvoiceRepository form8BRetailInvoiceRepository, ICommonBusiness commonBusiness)
        {
            _form8BRetailInvoiceRepository = form8BRetailInvoiceRepository;
            _commonBusiness = commonBusiness;
        }


        public List<Form8B> GetAllForm8B(UA UA)
        {
            List<Form8B> Form8Blist = null;
            try
            {

                Form8Blist = _form8BRetailInvoiceRepository.GetAllForm8B(UA);
                if (Form8Blist != null)
                {
                    foreach (Form8B F in Form8Blist)
                    {

                        Form8B_BL(F);
                    }
                }
               

            }
            catch (Exception)
            {

                throw;
            }
            return Form8Blist;
        }

        private void Form8B_BL(Form8B F)
        {
            if (F != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
                F.GrandTotal = F.Subtotal + F.VATAmount - F.VATExpense;

                if (F.ChallanDate != null)
                    F.ChallanDateFormatted = F.ChallanDate.GetValueOrDefault().ToString(settings.dateformat);
                if (F.PODate != null)
                    F.PODateFormatted = F.PODate.GetValueOrDefault().ToString(settings.dateformat);
                if (F.InvoiceDate != null)
                    F.InvoiceDateFormatted = F.InvoiceDate.ToString(settings.dateformat);
            }

        }
        private void Form8B_DetailBL(List<Form8BDetail> List)
        {
            if (List != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
                int slno = 1;
                foreach (Form8BDetail F in List)
                {
                    F.SlNo = slno;
                    F.BasicAmount = F.Quantity * F.Rate;
                    F.NetAmount = F.BasicAmount - F.TradeDiscount;
                    slno = slno + 1;
                }
            }
        }



        public Form8B InsertUpdate(Form8B frm8B, UA UA)
        {
            Form8B result = null;
            try
            {
                frm8B.DetailXML = _commonBusiness.GetXMLfromForm8BDetail(frm8B.Form8BDetail, "MaterialID", UA);
                if (frm8B.ID == null || frm8B.ID == Guid.Empty)
                {
                    result = _form8BRetailInvoiceRepository.InsertForm8B(frm8B, UA);
                }
                else
                {
                    result = _form8BRetailInvoiceRepository.UpdateForm8B(frm8B, UA);

                }

                //--------BL works ----------------------
                if (result != null)
                {
                    Form8B_BL(result);
                    Form8B_DetailBL(result.Form8BDetail);
                }



            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }


        public bool DeleteForm8BDetail(Guid ID, Guid HeaderID, UA UA)
        {
            return _form8BRetailInvoiceRepository.DeleteForm8BDetail(ID, HeaderID, UA);
        }

        public bool DeleteForm8B(Guid ID, UA UA)
        {
            return _form8BRetailInvoiceRepository.DeleteForm8B(ID, UA);
        }

        public Form8B GetForm8B(Guid ID, UA ua)
        {
            try
            {
                Form8B Result = new Form8B();
                Result = _form8BRetailInvoiceRepository.GetForm8BHeader(ID, ua);
                Result.Form8BDetail = _form8BRetailInvoiceRepository.GetForm8BDetail(ID, ua);
                Form8B_BL(Result);
                Form8B_DetailBL(Result.Form8BDetail);
                return Result;
            }
            catch (Exception)
            {

                throw;
            }


        }


    }
}