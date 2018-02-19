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
                if(Form8list != null)
                {
                    foreach (Form8 F in Form8list)
                    {

                        Form8BL(F);
                    }
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
                frm8.DetailXML = _commonBusiness.GetXMLfromObject(frm8.Form8Detail, "MaterialID", UA);
                if (frm8.ID == null || frm8.ID == Guid.Empty)
                {
                    result = _form8TaxInvoiceRepository.InsertForm8(frm8, UA);
                }
                else
                {
                    result = _form8TaxInvoiceRepository.UpdateForm8(frm8, UA);

                }

                //--------BL works ----------------------
                if (result != null)
                {
                    Form8BL(result);
                    Form8DetailBL(result);
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
            //    F.GrandTotal = F.Subtotal + F.VATAmount - F.Discount;
               

                if (F.ChallanDate != null)
                    F.ChallanDateFormatted = F.ChallanDate;//.GetValueOrDefault().ToString(settings.dateformat);
                if (F.PODate != null)
                    F.PODateFormatted = F.PODate;//.GetValueOrDefault().ToString(settings.dateformat);
                if (F.InvoiceDate != null)
                    F.InvoiceDateFormatted = F.InvoiceDate;//.ToString(settings.dateformat);
            }
         
        }

        private void Form8DetailBL(Form8 F8)
        {
            List<Form8Detail> List = F8.Form8Detail;
            decimal t1;
            decimal t2;
            decimal net;
            F8.TotalTaxAmount = 0;
            F8.GrandTotal = 0;
            if (List != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
                int slno = 1;
                foreach (Form8Detail F in List)
                {
                    F.SlNo = slno;
                    F.BasicAmount = F.Quantity * F.Rate;
                    F.NetAmount = F.BasicAmount - F.TradeDiscount;
                    F.CGSTAmount = (((F.Quantity * F.Rate) - (F.TradeDiscount)) * (F.CGSTPercentage / 100));
                    F.SGSTAmount = (((F.Quantity * F.Rate) - (F.TradeDiscount)) * (F.SGSTPercentage / 100));
                    t1 = (F.CGSTAmount??0   );
                    t2 = F.SGSTAmount ?? 0;
                    net = F.NetAmount ?? 0;
                    F8.TotalTaxAmount = F8.TotalTaxAmount + t1+t2;//.Total Tax Amount calculation
                    F8.GrandTotal = F8.GrandTotal + net + t1 + t2;//. Grand Total calculation
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
                Form8DetailBL(Result);
                return Result;
            }
            catch (Exception)
            {

                throw;
            }    


        }
    }
}