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
                SCManagerSettings settings=new SCManagerSettings();
                Form8list = _form8TaxInvoiceRepository.GetAllForm8(UA);
                foreach (Form8 F in Form8list  )      
                {
                    F.Total = F.TotalItemsValue + F.VATAmount - F.Discount;
                    if (F.ChallanDate!=null)
                        F.ChallanDateFormatted = F.ChallanDate.ToString(settings.dateformat);
                    if (F.PODate != null)
                        F.PODateFormatted = F.PODate.ToString(settings.dateformat);
                    if (F.InvoiceDate != null)
                        F.InvoiceDateFormatted = F.InvoiceDate.ToString(settings.dateformat);
                
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

            }
            catch (Exception)
            {

                throw;
            }    

            return result;
        }

      
    }
}