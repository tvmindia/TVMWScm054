
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using SCManager.BusinessService.Contracts;

namespace SCManager.BusinessService.Services
{
    public class TechnicianBusiness : ITechnicianBusiness
    {

        private ITechnicianRepository _technicianRepository;
        private ICommonBusiness _commonBusiness;
        /// <summary>
        /// Constructor Injection:-Getting IDynamicUIBusiness implementing object
        /// </summary>
        /// <param name="dynamicUIBusiness"></param>
        public TechnicianBusiness(ITechnicianRepository technicianRepository, ICommonBusiness commonBusiness)
        {
            _technicianRepository = technicianRepository;
            _commonBusiness = commonBusiness;
        }


        public List<TechnicianSummary> GetTechnicianSummary(UA ua)
        {
            try
            {
                List<TechnicianSummary> Result = new List<TechnicianSummary>();
                Result= _technicianRepository.GetTechnicianSummary(ua);
                if (Result != null) {
                    foreach (TechnicianSummary T in Result)
                    {
                        T.StockValueFormatted = _commonBusiness.ConvertCurrency(T.StockValue);
                    }
                }
                return Result;
            }
            catch (Exception)
            {
                throw;
            }


        }


    }
}