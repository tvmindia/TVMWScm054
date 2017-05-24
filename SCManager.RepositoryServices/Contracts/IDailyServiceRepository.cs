using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
    public interface IDailyServiceRepository
    {
        object InsertJob(TechnicianJob technicianJob);
        List<ServiceType> GetAllServiceTypes();
        List<Job> GetAllDailyJobs(string SCCode);

        object DeleteJob(Job job);
        object UpdateJob(TechnicianJob technicianJob);
    }
}
