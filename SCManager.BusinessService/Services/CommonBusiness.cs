using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using System.Reflection;
using System.Globalization;

namespace SCManager.BusinessService.Services
{
    public class CommonBusiness: ICommonBusiness
    {

        private int getMAndatoryIndex(object myObj,string mandatoryProperties) {

            int mandIndx = -1;

            object tmp = myObj;
            var ppty = GetProperties(tmp);
            int i;
            for (i = 0; i < ppty.Length; i++)
            {

                if (ppty[i].Name == mandatoryProperties)
                {
                    mandIndx = i;
                    break;
                }

            }

            return mandIndx;


        }

        private void XML(object some_object,int mandIndx,ref string result,ref int totalRows) {

            var properties = GetProperties(some_object);
            var mand = properties[mandIndx].GetValue(some_object, null);

            if ((mand != null) && (!string.IsNullOrEmpty(mand.ToString())))
            {

                result = result + "<item ";


                foreach (var p in properties)
                {
                    string name = p.Name;
                    var value = p.GetValue(some_object, null);
                    result = result + " " + name + @"=""" + value + @""" ";

                }
                result = result + "></item>";
                totalRows = totalRows + 1;
            }
        }

        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }


        //----------------------need to make below in common -----------------------
        public string GetXMLfromForm8BDetail(List<Form8BDetail> myObj, string mandatoryProperties, UA ua)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {                
                int mandIndx = getMAndatoryIndex(myObj[0], mandatoryProperties);      

                foreach (object some_object in myObj)
                {
                    XML(some_object,mandIndx,ref result,ref totalRows );
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
            else
            {
                return "";
            }
        }
     
        public string GetXMLfromObject(List<Form8Detail> myObj, string mandatoryProperties, UA ua)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int mandIndx = getMAndatoryIndex(myObj[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in myObj)
                {
                    XML(some_object, mandIndx, ref result, ref totalRows);                    

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
            else
            {
                return "";
            }

        }

        public string GetXMLfromTCRObject(List<TCRBillEntryDetail> tcrDetailObj, string mandatoryProperties, UA ua)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int mandIndx = getMAndatoryIndex(tcrDetailObj[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in tcrDetailObj)
                {
                    XML(some_object, mandIndx, ref result, ref totalRows);

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
            else
            {
                return "";
            }

        }

        public string GetXMLfromOtherSCReceiptObject(List<ReceiveFromOtherScDetail> receiveFromOtherScDetailObj, string mandatoryProperties, UA ua)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int mandIndx = getMAndatoryIndex(receiveFromOtherScDetailObj[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in receiveFromOtherScDetailObj)
                {
                    XML(some_object, mandIndx, ref result, ref totalRows);

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
            else
            {
                return "";
            }

        }

        public string GetXMLfromIssueToOtherSCObject(List<IssueToOtherScDetail> issueToOtherScDetailObj, string mandatoryProperties, UA ua)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int mandIndx = getMAndatoryIndex(issueToOtherScDetailObj[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in issueToOtherScDetailObj)
                {
                    XML(some_object, mandIndx, ref result, ref totalRows);

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
            else
            {
                return "";
            }

        }

        public string GetXMLfromICRObject(List<ICRBillEntryDetail> icrDetailObj, string mandatoryProperties, UA ua)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int mandIndx = getMAndatoryIndex(icrDetailObj[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in icrDetailObj)
                {
                    XML(some_object, mandIndx, ref result, ref totalRows);

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
            else
            {
                return "";
            }

        }
        public string GetXMLfromOfficeObject(List<OfficeBillEntryDetail> officeDetailObj, string mandatoryProperties, UA ua)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int mandIndx = getMAndatoryIndex(officeDetailObj[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in officeDetailObj)
                {
                    XML(some_object, mandIndx, ref result, ref totalRows);

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
            else
            {
                return "";
            }

        }
        public string GetXMLfromLocalPurchaseDetail(List<LocalPurchaseDetail> myObj, string mandatoryProperties, UA ua)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int mandIndx = getMAndatoryIndex(myObj[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in myObj)
                {
                    XML(some_object, mandIndx, ref result, ref totalRows);

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
            else
            {
                return "";
            }

        }

        public string GetXMLfromOpenDetail(List<OpeningDetail> myObj, string mandatoryProperties, UA ua)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int mandIndx = getMAndatoryIndex(myObj[0], mandatoryProperties); //int mandIndx = 0;                

                foreach (object some_object in myObj)
                {
                    XML(some_object, mandIndx, ref result, ref totalRows);

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
            else
            {
                return "";
            }

        }

        //----------------------------------------------------------------------------

        public string ConvertCurrency(decimal value,int DecimalPoints=0,bool Symbol=true  ) {
            string result = value.ToString();
            string fare = result;
            decimal parsed = decimal.Parse(fare, CultureInfo.InvariantCulture);
            CultureInfo hindi = new CultureInfo("hi-IN");
            if (Symbol)
                result = string.Format(hindi, "{0:C" + DecimalPoints + "}", parsed);
            else
            {
                if (DecimalPoints == 0)
                { result = string.Format(hindi, "{0:#,#.##}", parsed); }
                else
                { result = string.Format(hindi, "{0:#,#0.00}", parsed); }
            }
            return result;  

        }

        public string GetXMLfromReturnBill(List<ReturnBillDetail> myObj, string mandatoryProperties, UA ua)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                int mandIndx = getMAndatoryIndex(myObj[0], mandatoryProperties);

                foreach (object some_object in myObj)
                {
                    XML(some_object, mandIndx, ref result, ref totalRows);
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
            else
            {
                return "";
            }
        }
    }
}