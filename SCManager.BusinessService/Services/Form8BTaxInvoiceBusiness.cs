using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.RepositoryServices.Contracts;
using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;

namespace SCManager.BusinessService.Services
{
    public class Form8BTaxInvoiceBusiness: IForm8BTaxInvoiceBusiness
    {
        private IForm8BTaxInvoiceRepository _form8BTaxInvoiceRepository;
        private ICommonBusiness _commonBusiness;

        public Form8BTaxInvoiceBusiness(IForm8BTaxInvoiceRepository form8BTaxInvoiceRepository, ICommonBusiness commonBusiness)
        {
            _form8BTaxInvoiceRepository = form8BTaxInvoiceRepository;
            _commonBusiness = commonBusiness;
        }


        public List<Form8B> GetAllForm8B(UA UA)
        {
            List<Form8B> Form8Blist = null;
            try
            {

                Form8Blist = _form8BTaxInvoiceRepository.GetAllForm8B(UA);
                foreach (Form8B F in Form8Blist)
                {

                    Form8B_BL(F);
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
    }
}