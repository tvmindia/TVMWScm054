﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using System.Reflection;

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

            if (mand != null)
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

        //----------------------need to make below in common -----------------------
        public string GetXMLfromObject(List<Form8Detail> myObj, string mandatoryProperties, UA ua)
        {
            string result = "<Details>";
            int totalRows = 0;
            try
            {
                //-------------------------//
                int mandIndx = 0;

                object tmp = myObj[0];
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
                //------------------------//


                foreach (object some_object in myObj)
                {
                    var properties = GetProperties(some_object);
                    var mand = properties[mandIndx].GetValue(some_object, null);
                 
                    if (mand != null)
                    {

                        result = result + "<item ";


                        foreach (var p in properties)
                        {
                            string name = p.Name;
                            var value = p.GetValue(some_object, null);
                            result = result + " " + name + @"=""" + value + @""" " ;

                        }
                        result = result + "></item>";
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
            else
            {
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