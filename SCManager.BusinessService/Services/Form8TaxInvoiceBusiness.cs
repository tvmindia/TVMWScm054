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
    public class Form8TaxInvoiceBusiness : IForm8TaxInvoiceBusiness
    {
         private IForm8TaxInvoiceRepository _form8TaxInvoiceRepository;
        private ICommonBusiness _commonBusiness;

        public Form8TaxInvoiceBusiness(IForm8TaxInvoiceRepository form8TaxInvoiceRepository , ICommonBusiness commonBusiness)
        {
            _form8TaxInvoiceRepository = form8TaxInvoiceRepository;
            _commonBusiness = commonBusiness;
        }

        public List<Form8> GetAllForm8(UA UA)
        {
            List<Form8> Form8list = null;
            try
            {
               
                Form8list = _form8TaxInvoiceRepository.GetAllForm8(UA);
                foreach (Form8 F in Form8list  )      
                {

                    Form8BL(F);
                }
      
            }
            catch (Exception)
            {
                
                throw;
            }
            return Form8list;
        }    

        public Form8 InsertUpdate(Form8 frm8, UA UA) {
            Form8 result = null;
            try
            {
                frm8.DetailXML= _commonBusiness.GetXMLfromObject(frm8.Form8Detail, "MaterialID",UA);
                if (frm8.ID == null || frm8.ID == Guid.Empty) {
                    result= _form8TaxInvoiceRepository.InsertForm8(frm8, UA);
                }
                else {
                    result=_form8TaxInvoiceRepository.UpdateForm8(frm8, UA);

                }

                //--------BL works ----------------------
                if (result != null) {
                    Form8BL(result);
                    Form8DetailBL(result.Form8Detail);
                }
                   
                

            }
            catch (Exception)
            {

                throw;
            }    

            return result;
        }

        private void Form8BL(Form8 F)
        {
            if (F != null) {
                SCManagerSettings settings = new SCManagerSettings();
                F.GrandTotal = F.Subtotal + F.VATAmount - F.Discount;

                if (F.ChallanDate != null)
                    F.ChallanDateFormatted = F.ChallanDate.GetValueOrDefault().ToString(settings.dateformat);
                if (F.PODate != null)
                    F.PODateFormatted = F.PODate.GetValueOrDefault().ToString(settings.dateformat);
                if (F.InvoiceDate != null)
                    F.InvoiceDateFormatted = F.InvoiceDate.ToString(settings.dateformat);
            }
         
        }

        private void Form8DetailBL(List<Form8Detail> List)
        {
            if (List != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
                int slno = 1;
                foreach (Form8Detail F in List)
                {
                    F.SlNo = slno;
                    F.BasicAmount = F.Quantity * F.Rate;
                    F.NetAmount = F.BasicAmount - F.TradeDiscount;
                    slno = slno + 1;
                }
            }
           

            
        }

        public bool DeleteForm8Detail(Guid ID, Guid HeaderID, UA UA) {
            return _form8TaxInvoiceRepository.DeleteForm8Detail(ID, HeaderID, UA);
        }

        public bool DeleteForm8(Guid ID, UA UA) {
            return _form8TaxInvoiceRepository.DeleteForm8(ID, UA);
        }

        public Form8 GetForm8(Guid ID,UA ua) {
            try
            {
                Form8 Result = new Form8();
                Result = _form8TaxInvoiceRepository.GetForm8Header(ID, ua);
                Result.Form8Detail = _form8TaxInvoiceRepository.GetForm8Detail(ID, ua);
                Form8BL(Result);
                Form8DetailBL(Result.Form8Detail);
                return Result;
            }
            catch (Exception)
            {

                throw;
            }
         

        }




    }
}