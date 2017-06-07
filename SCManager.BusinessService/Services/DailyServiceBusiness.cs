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
                ServiceTypeList = _dailyServiceRepository.GetAllServiceTypes(ua.SCCode);
                ServiceTypeList = ServiceTypeList == null ? null : ServiceTypeList.Where(stype => stype.SCCode == ua.SCCode && stype.SubType== "SERVICE").ToList();
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

        public List<Job> GetJobs(string SCCode, Guid id, string servicedate)
        {
            List<Job> JobList = null;
            try
            {
                JobList = _dailyServiceRepository.GetAllDailyJobs(SCCode);
                JobList = JobList == null ? null : JobList.Where(stype => stype.SCCode == SCCode && stype.Employee.ID==id && DateTime.Parse(stype.ServiceDate)== DateTime.Parse(servicedate)).ToList();
                if (JobList != null)
                {
                    foreach (Job EX in JobList)
                    {
                        SCManagerSettings settings = new SCManagerSettings();

                        if (EX.ServiceDate != null)
                            EX.ServiceDateformatted = Convert.ToDateTime(EX.ServiceDate).ToString(settings.dateformat);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return JobList;
        }

        public List<Job> GetServicefilterbyDays(string SCCode,string CreatedDate ,string Isdefault)
        {
            List<Job> JobList = null;
            try
            {
                JobList = _dailyServiceRepository.GetServicefilterbyDays(SCCode,CreatedDate,Isdefault);
                if (JobList != null)
                {
                    foreach (Job EX in JobList)
                    {
                        SCManagerSettings settings = new SCManagerSettings();

                        if (EX.ServiceDate != null)
                            EX.ServiceDateformatted = Convert.ToDateTime(EX.ServiceDate).ToString(settings.dateformat);
                    }

                }

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
                JobList = JobList == null ? null : JobList.Select(x => new Job { ID = x.ID, JobNo = x.JobNo }).OrderBy(x => x.JobNo).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return JobList;
        }

        public List<Employees> GetTechniciansForRepeatedJob(string SCCode,string JobNo)
        {
            List<Job> JobList = null;
            List<Employees> EmpList = null;
            try
            {
                JobList = _dailyServiceRepository.GetAllDailyJobs(SCCode);

                EmpList = JobList == null ? null : JobList.Where(x=>x.JobNo==JobNo).Select(x => new Employees { ID = x.Employee.ID, Name = x.Employee.Name }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EmpList;
        }

        public List<JobCallTypes> GetJobCallTypes(string SCCode)
        {
            List<JobCallTypes> JobCallTypeList = null;
            try
            {
                JobCallTypeList = _dailyServiceRepository.GetAllJobCallTypes(SCCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return JobCallTypeList;
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
        public object UpdateJob(TechnicianJob technicianJob)
        {
            try
            {
                return _dailyServiceRepository.UpdateJob(technicianJob);
            }
            catch (Exception ex)
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
                JobList = JobList == null ? null : JobList.Where(j=>j.ID== Guid.Parse(JobID)).Select(c=> { c.ServiceDate = DateTime.Parse(c.ServiceDate).Date.ToString("yyyy-MM-dd");return c; }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ((JobList!=null)&&(JobList.Count>0))? JobList[0]:null;
        }

        public Job GetDailyJobByJobNo(string SCCode, string JobNo)
        {
            List<Job> JobList = null;
           
            try
            {
                JobList = _dailyServiceRepository.GetAllDailyJobs(SCCode);
                JobList = JobList == null ? null : JobList.Where(j => j.JobNo == JobNo).Select(c => { c.ServiceDate = DateTime.Parse(c.ServiceDate).Date.ToString("dd-MMM-yyyy"); return c; }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ((JobList != null) && (JobList.Count > 0)) ? JobList[0] : null;
        }

        public List<ServiceRegistrySummary> GetServiceRegistrySummary(string SCCode, string serviceDate)
        {
            List<ServiceRegistrySummary> ServiceRegistrySummaryList = null;
            try
            {
                ServiceRegistrySummaryList = _dailyServiceRepository.GetServiceRegistrySummary(SCCode, serviceDate);
                ServiceRegistrySummaryList = ServiceRegistrySummaryList == null ? null : ServiceRegistrySummaryList.Select(c => { c.ServiceDate = DateTime.Parse(c.ServiceDate).Date.ToString("yyyy-MM-dd"); return c; }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ServiceRegistrySummaryList;
        }

        public List<ServiceRegistrySummary> GetServiceRegisterSummaryFilter(string SCCode, string CreatedDate, string Isdefault)
        {
            List<ServiceRegistrySummary> ServiceRegistrySummaryList = null;
            try
            {
                ServiceRegistrySummaryList = _dailyServiceRepository.GetServiceRegisterSummaryFilter(SCCode, CreatedDate, Isdefault);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ServiceRegistrySummaryList;
        }
    }
}