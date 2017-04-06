
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
        /// <summary>
        /// Constructor Injection:-Getting IDynamicUIBusiness implementing object
        /// </summary>
        /// <param name="dynamicUIBusiness"></param>
        public TechnicianBusiness(ITechnicianRepository technicianRepository)
        {
            _technicianRepository = technicianRepository;
        }


        public List<TechnicianSummary> GetTechnicianSummary(UA ua)
        {
            try
            {
                return _technicianRepository.GetTechnicianSummary(ua);
            }
            catch (Exception)
            {
                throw;
            }


        }


    }
}