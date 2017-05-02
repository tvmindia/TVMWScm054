using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class CallandServiceTypesBusiness : ICallandServiceTypesBusiness
    {
        private ICallandServiceTypesRepository _iCallandServiceTypesRepository;
        public CallandServiceTypesBusiness(ICallandServiceTypesRepository iCallandServiceTypesRepository)
        {
            _iCallandServiceTypesRepository = iCallandServiceTypesRepository;
        }
        public string UpdateCallandServiceTypes(CallTypes callTypesObj,ServiceTypes serviceTypesObj)
        {
            string result = null;
            try
            {              
                
                    result = _iCallandServiceTypesRepository.UpdateCallType(callTypesObj);
                if(result=="1")
                {
                    result = _iCallandServiceTypesRepository.UpdateServiceType(serviceTypesObj);
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}