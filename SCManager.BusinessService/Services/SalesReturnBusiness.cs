using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class SalesReturnBusiness : ISalesReturnBusiness
    {
        private ISalesReturnRepository _iSalesReturnRepository;
        public SalesReturnBusiness(ISalesReturnRepository iSalesReturnRepository)
        {
            _iSalesReturnRepository = iSalesReturnRepository;
        }

        public List<SalesReturn> GetAllSalesReturn(UA UA)
        {
            List<SalesReturn> salesReturnList = null;
            salesReturnList = _iSalesReturnRepository.GetAllSalesReturn(UA);
            if (salesReturnList != null)
            {
                foreach (SalesReturn sr in salesReturnList)
                {

                    SalesReturn_DF(sr);
                }
            }
            return salesReturnList;
        }

        private void SalesReturn_DF(SalesReturn sr)
        {
            if (sr != null)
            {
                SCManagerSettings settings = new SCManagerSettings();

                if (sr.OpenDate != null)
                    sr.OpenDateFormatted = sr.OpenDate.GetValueOrDefault().ToString(settings.dateformat);
            }

        }

        public string SalesReturnValidation(string itemID, UA ua)
        {
            string status = null;
            try
            {
                status = _iSalesReturnRepository.SalesReturnValidation(itemID, ua);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public List<SalesReturn> GetSalesReturnByID(UA UA, string ID)
        {
            List<SalesReturn> salesReturnList = null;
            salesReturnList = _iSalesReturnRepository.GetSalesReturnByID(UA, ID);
            return salesReturnList;
        }

        public object InsertUpdateSalesReturn(SalesReturn salesReturnObj)
        {
            object result = null;
            try
            {
                salesReturnObj.ReturnStatusYN = false;
                if (salesReturnObj.ID == Guid.Empty)
                {
                    result = _iSalesReturnRepository.InsertSalesReturn(salesReturnObj);
                }
                else
                {
                    result = _iSalesReturnRepository.UpdateSalesReturn(salesReturnObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string DeleteSalesReturn(string ID, UA ua)
        {
            string status = null;
            try
            {
                status = _iSalesReturnRepository.DeleteSalesReturn(ID, ua);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public string ReturnSalesToCompany(string ID, UA ua)
        {
            string status = null;
            try
            {
                status = _iSalesReturnRepository.ReturnSalesToCompany(ID, ua);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
    }
}