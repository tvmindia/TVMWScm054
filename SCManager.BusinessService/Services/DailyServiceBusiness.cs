using SCManager.BusinessService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;

namespace SCManager.BusinessService.Services
{
    public class DailyServiceBusiness : IDailyServiceBusiness
    {
        IDailyServiceRepository _dailyServiceRepository;
        public DailyServiceBusiness(IDailyServiceRepository dailyServiceRepository)
        {
            _dailyServiceRepository = dailyServiceRepository;
        }
        public object InsertJob(TechnicianJob technicianJob)
        {
            try
            {
              return  _dailyServiceRepository.InsertJob(technicianJob);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}