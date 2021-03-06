﻿using SCManager.DataAccessObject.DTO;
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
        List<ServiceType> GetAllServiceTypes(string SCCode);
        List<Job> GetAllDailyJobs(string SCCode);
        List<Job> GetServicefilterbyDays(string SCCode, string FromDate, string ToDate);

        object DeleteJob(Job job);
        object UpdateJob(TechnicianJob technicianJob);
        List<ServiceRegistrySummary> GetServiceRegistrySummary(string SCCode,string serviceDate);
        List<JobCallTypes> GetAllJobCallTypes(string SCCode);
        List<ServiceRegistrySummary> GetServiceRegisterSummaryFilter(string SCCode, string CreatedDate, string Isdefault);
    }
}
