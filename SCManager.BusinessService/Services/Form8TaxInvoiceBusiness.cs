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
        

        public Form8TaxInvoiceBusiness(IForm8TaxInvoiceRepository form8TaxInvoiceRepository )
        {
            _form8TaxInvoiceRepository = form8TaxInvoiceRepository;
           
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
                frm8.DetailXML=GetXMLfromObject(frm8.Form8Detail, "MaterialID",UA);

            }
            catch (Exception)
            {

                throw;
            }    

            return result;
        }

        //----------------------need to make below in common -----------------------
        public string GetXMLfromObject(List<Form8Detail> myObj, string mandatoryProperties,UA ua)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int mandIndx=0;
              
                object tmp = myObj[0];
                var ppty = GetProperties(tmp);
                int i;
                for( i=0;i<ppty.Length;i++)
                {
                    
                    if (ppty[i].Name == mandatoryProperties)
                    {
                        mandIndx = i;
                        break;
                    }

                }
                //------------------------//


                foreach (object some_object in myObj)
                {
                     var properties = GetProperties(some_object);
                     var mand= properties[mandIndx].GetValue(some_object, null);
                    if( mand !=null)
                    {

                        result = result + "<item>";


                        foreach (var p in properties)
                        {
                            string name = p.Name;
                            var value = p.GetValue(some_object, null);
                            result= result +"<" + name + ">" +value + "</" + name +">";

                        }
                        result = result + "</item>";
                        totalRows = totalRows + 1;
                    }
                   
                }

                result = result + "</Details>";


            }
            catch (Exception)
            {

                throw;
            }
            if (totalRows > 0)
            {
                return result;
            }
            else {
                return "";
            }
            
        }

        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }
        //----------------------------------------------------------------------------
    }
}