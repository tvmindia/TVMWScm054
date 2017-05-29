using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
    public interface IDailyServiceBusiness
    {
        object InsertJob(TechnicianJob technicianJob);
        List<ServiceType> GetAllServiceTypes(UA ua);
        List<Job> GetAllDailyJobs(string SCCode);
        List<Job> GetAllJobNumbers(string SCCode);
        List<Employees> GetTechniciansForRepeatedJob(string SCCode);
        List<CallTypes> GetCallTypes(UA ua);
        List<Employees> GetAllTechnicians(UA ua);
        object DeleteJob(Job job);
        Job GetDailyJobByID(string SCCode, string JobID);
        Job GetDailyJobByJobNo(string SCCode, string JobNo);
        List<Job> GetJobs(string SCCode,Guid id,string servicedate);
        object UpdateJob(TechnicianJob technicianJob);
        List<ServiceRegistrySummary> GetServiceRegistrySummary(string SCCode, string serviceDate);
    }
}
