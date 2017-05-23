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
        ICallandServiceTypesBusiness _callandServiceTypesBusiness;
        IEmployeesBusiness _employeesBusiness;
        public DailyServiceBusiness(IDailyServiceRepository dailyServiceRepository, ICallandServiceTypesBusiness callandServiceTypesBusiness, ITechnicianBusiness technicianBusiness, IEmployeesBusiness employeesBusiness)
        {
            _dailyServiceRepository = dailyServiceRepository;
            _callandServiceTypesBusiness = callandServiceTypesBusiness;
            _employeesBusiness = employeesBusiness;
        }

        public List<ServiceType> GetAllServiceTypes(UA ua)
        {
            List<ServiceType> ServiceTypeList = null;
           try
            {
                ServiceTypeList = _dailyServiceRepository.GetAllServiceTypes();
                ServiceTypeList = ServiceTypeList == null ? null : ServiceTypeList.Where(stype => stype.SCCode == ua.SCCode).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return ServiceTypeList;
        }


        public List<Job> GetAllDailyJobs(string SCCode)
        {
            List<Job> JobList = null;
            try
            {
                JobList = _dailyServiceRepository.GetAllDailyJobs(SCCode);
              // JobList = JobList == null ? null : JobList.Where(stype => stype.SCCode == ua.SCCode).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return JobList;
        }

        public List<Job> GetAllJobNumbers(string SCCode)
        {
            List<Job> JobList = null;
            try
            {
                JobList = _dailyServiceRepository.GetAllDailyJobs(SCCode);
                JobList = JobList == null ? null : JobList.Select(x => new Job { ID = x.ID, JobNo = x.JobNo }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return JobList;
        }

        public List<Employees> GetTechniciansForRepeatedJob(string SCCode)
        {
            List<Job> JobList = null;
            List<Employees> EmpList = null;
            try
            {
                JobList = _dailyServiceRepository.GetAllDailyJobs(SCCode);

                EmpList = JobList == null ? null : JobList.Select(x => new Employees { ID = x.Employee.ID, Name = x.Employee.Name }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EmpList;
        }

        public List<CallTypes> GetCallTypes(UA ua)
        {
            List<CallTypes> CallTypeList = null;
            try
            {
                CallTypeList = _callandServiceTypesBusiness.GetCallTypes(ua);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CallTypeList;
        }

        public List<Employees> GetAllTechnicians(UA ua)
        {
            List<Employees> EmployeeList = null;
            try
            {

                EmployeeList = _employeesBusiness.GetAllTechnicians(ua);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EmployeeList;
        }

        public object DeleteJob(Job job)
        {
            try
            {
                return _dailyServiceRepository.DeleteJob(job);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public Job GetDailyJobByID(string SCCode, string JobID)
        {
            List<Job> JobList = null;
            try
            {
                JobList = _dailyServiceRepository.GetAllDailyJobs(SCCode);
                JobList = JobList == null ? null : JobList.Where(j=>j.ID== Guid.Parse(JobID)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ((JobList!=null)&&(JobList.Count>0))? JobList[0]:null;
        }
    }
}