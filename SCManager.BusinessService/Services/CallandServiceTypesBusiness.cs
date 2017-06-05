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
        public Object UpdateServiceTypesAndCommission(ServiceTypes serviceTypesObj)
        {
         try
            {
                // result = _iCallandServiceTypesRepository.UpdateCallType(callTypesObj);
                return _iCallandServiceTypesRepository.UpdateServiceTypesAndCommission(serviceTypesObj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
             
        }
      
        public ServiceTypes GetServiceTypes(UA UA)
        {
            List<ServiceTypes> serviceTypesList = null;
            ServiceTypes _serviceTypes = null;
            try
            {
                serviceTypesList = _iCallandServiceTypesRepository.GetServiceTypes(UA);
                if(serviceTypesList!=null)
                {
                    _serviceTypes = new ServiceTypes();
                    foreach (ServiceTypes st in serviceTypesList)
                    {
                        switch (st.Code)
                        {
                            case "DMO":
                                _serviceTypes.DemoCommission = st.DemoCommission;
                                break;
                            case "MJR":
                                _serviceTypes.MajorCommission = st.MajorCommission;
                                break;
                            case "MND":
                                _serviceTypes.MandatoryCommission = st.MandatoryCommission;
                                break;
                            case "MNR":
                                _serviceTypes.MinorCommission = st.MinorCommission;
                                break;
                            case "RPT":
                                _serviceTypes.RepeatCommission = st.RepeatCommission;
                                break;
                            case "AMC1":
                                _serviceTypes.AMC1Commission = st.AMC1Commission;
                                break;
                            case "AMC2":
                                _serviceTypes.AMC2Commission = st.AMC2Commission;
                                break;

                        }
                    
                }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return _serviceTypes;
        }
    }
}