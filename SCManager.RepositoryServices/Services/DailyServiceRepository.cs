using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SCManager.RepositoryServices.Services
{
    public class DailyServiceRepository : IDailyServiceRepository
    {
        private IDatabaseFactory _databaseFactory;
        private Const constObj = new Const();
        public DailyServiceRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region GetAllServiceTypes
        public List<ServiceType> GetAllServiceTypes(string SCCode)
        {
            List<ServiceType> serviceTypeList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[GetAllServiceTypes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = SCCode;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                serviceTypeList = new List<ServiceType>();
                                while (sdr.Read())
                                {
                                    ServiceType _serviceType = new ServiceType();
                                    {
                                        _serviceType.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _serviceType.SCCode);
                                        _serviceType.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : _serviceType.Code);
                                        _serviceType.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _serviceType.Description);
                                        _serviceType.Commission = (sdr["Commission"].ToString() != "" ?decimal.Parse(sdr["Commission"].ToString()) : _serviceType.Commission);
                                        _serviceType.SubType = (sdr["SubType"].ToString() != "" ? (sdr["SubType"].ToString()) : _serviceType.SubType);
                                    }
                                    serviceTypeList.Add(_serviceType);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return serviceTypeList;
        }
        #endregion GetAllServiceTypes

        #region GetAllJobCallTypes
        public List<JobCallTypes> GetAllJobCallTypes(string SCCode)
        {
            List<JobCallTypes> JobCallTypesList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[GetAllJobCallTypes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = SCCode;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                JobCallTypesList = new List<JobCallTypes>();
                                while (sdr.Read())
                                {
                                    JobCallTypes _jobCallTypes = new JobCallTypes();
                                    {
                                        _jobCallTypes.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _jobCallTypes.SCCode);
                                        _jobCallTypes.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : _jobCallTypes.Code);
                                        _jobCallTypes.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _jobCallTypes.Description);
                                      
                                    }
                                    JobCallTypesList.Add(_jobCallTypes);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return JobCallTypesList;
        }
        #endregion GetAllJobCallTypes

        #region GetAllDailyJobs
        public List<Job> GetAllDailyJobs(string SCCode)
        {
            List<Job> jobList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[GetAllDailyServiceEntries]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = SCCode;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                jobList = new List<Job>();
                                while (sdr.Read())
                                {
                                    Job _job = new Job();
                                    {
                                        _job.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _job.SCCode);
                                        _job.ID= (sdr["ID"].ToString() != "" ? (Guid.Parse(sdr["ID"].ToString())) : Guid.Empty);
                                        _job.Employee = new Employees()
                                        {
                                            ID= (sdr["EmpID"].ToString() != "" ? (Guid.Parse(sdr["EmpID"].ToString())) : Guid.Empty),
                                            Name= (sdr["EmployeeName"].ToString() != "" ? (sdr["EmployeeName"].ToString()) : _job.Employee.Name),
                                            Type= (sdr["EmployeeType"].ToString() != "" ? (sdr["EmployeeType"].ToString()) : _job.Employee.Type),
                                        };
                                        _job.ServiceDate = (sdr["ServiceDate"].ToString() != "" ? (sdr["ServiceDate"].ToString()) : _job.ServiceDate);
                                        _job.JobNo = (sdr["JobNo"].ToString() != "" ? (sdr["JobNo"].ToString()) : _job.JobNo);
                                        _job.CustomerName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : _job.CustomerName);
                                        _job.CustomerLocation = (sdr["CustomerLocation"].ToString() != "" ? (sdr["CustomerLocation"].ToString()) : _job.CustomerLocation);
                                        _job.ServiceType= (sdr["ServiceTypeCode"].ToString() != "" ? (sdr["ServiceTypeCode"].ToString()) : _job.ServiceType);
                                        _job.CallType = (sdr["CallTypeCode"].ToString() != "" ? (sdr["CallTypeCode"].ToString()) : _job.CallType);
                                        _job.ModelNo = (sdr["ModelNo"].ToString() != "" ? (sdr["ModelNo"].ToString()) : _job.ModelNo);
                                        _job.SerialNo = (sdr["SerialNo"].ToString() != "" ? (sdr["SerialNo"].ToString()) : _job.SerialNo);
                                     
                                        _job.CallStatusCode = (sdr["CallStatusCode"].ToString() != "" ? (sdr["CallStatusCode"].ToString()) : _job.CallStatusCode);
                                        _job.MobileNumber= (sdr["ICRNo"].ToString() != "" ? (sdr["ICRNo"].ToString()) : _job.MobileNumber);
                                        _job.TechnicianRemark = (sdr["TechnicianRemarks"].ToString() != "" ? (sdr["TechnicianRemarks"].ToString()) : _job.TechnicianRemark);
                                        _job.RepeatEmpName= (sdr["Repeat_EmpName"].ToString() != "" ? (sdr["Repeat_EmpName"].ToString()) : _job.TechnicianRemark);
                                        _job.RepeatJobNo = (sdr["Repeat_JobNo"].ToString() != "" ? (sdr["Repeat_JobNo"].ToString()) : _job.RepeatJobNo);
                                        _job.Repeat_EmpID = (sdr["Repeat_EmpID"].ToString() != "" ? (sdr["Repeat_EmpID"].ToString()) : _job.Repeat_EmpID);
                                        _job.CallStatusDescription = (sdr["CallStatusDescription"].ToString() != "" ? (sdr["CallStatusDescription"].ToString()) : _job.CallStatusDescription);
                                        _job.JobCallTypeDescription= (sdr["JobCallTypeDescription"].ToString() != "" ? (sdr["JobCallTypeDescription"].ToString()) : _job.JobCallTypeDescription);
                                        _job.ServiceTypeDescription = (sdr["ServiceTypeDescription"].ToString() != "" ? (sdr["ServiceTypeDescription"].ToString()) : _job.ServiceTypeDescription);
                                        _job.SCCommAmount= (sdr["SpecialCommission"].ToString() != "" ? decimal.Parse(sdr["SpecialCommission"].ToString()) : _job.SCCommAmount);
                                    }
                                    jobList.Add(_job);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobList;
        }
        #endregion GetAllDailyJobs


        #region GetServicefilterbyDays
        public List<Job> GetServicefilterbyDays(string SCCode,string Createddate,string Isdefault)
        {
            List<Job> jobList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[GetServicefilterbyDays]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = SCCode;
                        cmd.Parameters.Add("@IsDefault", SqlDbType.Bit).Value =bool.Parse(Isdefault);
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DateTime.Parse(Createddate);
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                jobList = new List<Job>();
                                while (sdr.Read())
                                {
                                    Job _job = new Job();
                                    {
                                        _job.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _job.SCCode);
                                        _job.ID = (sdr["ID"].ToString() != "" ? (Guid.Parse(sdr["ID"].ToString())) : Guid.Empty);
                                        _job.Employee = new Employees()
                                        {
                                            ID = (sdr["EmpID"].ToString() != "" ? (Guid.Parse(sdr["EmpID"].ToString())) : Guid.Empty),
                                            Name = (sdr["EmployeeName"].ToString() != "" ? (sdr["EmployeeName"].ToString()) : _job.Employee.Name),
                                            Type = (sdr["EmployeeType"].ToString() != "" ? (sdr["EmployeeType"].ToString()) : _job.Employee.Type),
                                        };
                                        _job.ServiceDate = (sdr["ServiceDate"].ToString() != "" ? (sdr["ServiceDate"].ToString()) : _job.ServiceDate);
                                        _job.JobNo = (sdr["JobNo"].ToString() != "" ? (sdr["JobNo"].ToString()) : _job.JobNo);
                                        _job.CustomerName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : _job.CustomerName);
                                        _job.CustomerLocation = (sdr["CustomerLocation"].ToString() != "" ? (sdr["CustomerLocation"].ToString()) : _job.CustomerLocation);
                                        _job.ServiceType = (sdr["ServiceTypeCode"].ToString() != "" ? (sdr["ServiceTypeCode"].ToString()) : _job.ServiceType);
                                        _job.CallType = (sdr["CallTypeCode"].ToString() != "" ? (sdr["CallTypeCode"].ToString()) : _job.CallType);
                                        _job.ModelNo = (sdr["ModelNo"].ToString() != "" ? (sdr["ModelNo"].ToString()) : _job.ModelNo);
                                        _job.SerialNo = (sdr["SerialNo"].ToString() != "" ? (sdr["SerialNo"].ToString()) : _job.SerialNo);

                                        _job.CallStatusCode = (sdr["CallStatusCode"].ToString() != "" ? (sdr["CallStatusCode"].ToString()) : _job.CallStatusCode);
                                        _job.MobileNumber = (sdr["ICRNo"].ToString() != "" ? (sdr["ICRNo"].ToString()) : _job.MobileNumber);
                                        _job.TechnicianRemark = (sdr["TechnicianRemarks"].ToString() != "" ? (sdr["TechnicianRemarks"].ToString()) : _job.TechnicianRemark);
                                        _job.RepeatEmpName = (sdr["Repeat_EmpName"].ToString() != "" ? (sdr["Repeat_EmpName"].ToString()) : _job.TechnicianRemark);
                                        _job.RepeatJobNo = (sdr["Repeat_JobNo"].ToString() != "" ? (sdr["Repeat_JobNo"].ToString()) : _job.RepeatJobNo);
                                        _job.CallStatusDescription = (sdr["CallStatusDescription"].ToString() != "" ? (sdr["CallStatusDescription"].ToString()) : _job.CallStatusDescription);
                                        _job.JobCallTypeDescription = (sdr["JobCallTypeDescription"].ToString() != "" ? (sdr["JobCallTypeDescription"].ToString()) : _job.JobCallTypeDescription);
                                        _job.ServiceTypeDescription = (sdr["ServiceTypeDescription"].ToString() != "" ? (sdr["ServiceTypeDescription"].ToString()) : _job.ServiceTypeDescription);
                                        _job.SCCommAmount = (sdr["SpecialCommission"].ToString() != "" ? decimal.Parse(sdr["SpecialCommission"].ToString()) : _job.SCCommAmount);
                                    }
                                    jobList.Add(_job);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobList;
        }
        #endregion GetServicefilterbyDays

        #region InsertJob
        public object InsertJob(TechnicianJob technicianJob)
        {
            SqlParameter outParameter, outParameter1 = null;
            try
            {

                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[InsertTechnicianJob]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = technicianJob.SCCode;
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = technicianJob.TechEmpID;
                        cmd.Parameters.Add("@ServiceDate", SqlDbType.DateTime).Value = DateTime.Parse(technicianJob.ServiceDate);
                       // cmd.Parameters.Add("@JobNo", SqlDbType.NVarChar, 50).Value = technicianJob.JobNo;
                        cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar, 250).Value = technicianJob.CustomerName;
                        cmd.Parameters.Add("@CustomerLocation", SqlDbType.NVarChar, 250).Value = technicianJob.CustomerLocation;
                        cmd.Parameters.Add("@ServiceTypeCode", SqlDbType.NVarChar, 5).Value = technicianJob.ServiceType;
                        cmd.Parameters.Add("@CallTypeCode", SqlDbType.NVarChar, 20).Value = technicianJob.CallType;
                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar, 50).Value = technicianJob.ModelNo;
                        cmd.Parameters.Add("@SerialNo", SqlDbType.NVarChar, 50).Value = technicianJob.SerialNo;
                        cmd.Parameters.Add("@CallStatusCode", SqlDbType.NVarChar, 5).Value = technicianJob.CallStatusCode;
                        cmd.Parameters.Add("@ICRNo", SqlDbType.NVarChar, 50).Value = technicianJob.MobileNumber; 
                        cmd.Parameters.Add("@TechnicianRemarks", SqlDbType.NVarChar, -1).Value = technicianJob.TechnicianRemark;
                        if(technicianJob.Repeat_EmpID!=Guid.Empty)
                        {
                            cmd.Parameters.Add("@Repeat_EmpID", SqlDbType.UniqueIdentifier).Value = technicianJob.Repeat_EmpID;
                        }
                       if(!string.IsNullOrEmpty(technicianJob.Repeat_JobNo))
                        {
                            cmd.Parameters.Add("@Repeat_JobNo", SqlDbType.NVarChar, 50).Value = technicianJob.Repeat_JobNo;
                        }
                        cmd.Parameters.Add("@SpecialCommission", SqlDbType.Decimal).Value = technicianJob.SCCommAmount;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = technicianJob.logDetails.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.SmallDateTime).Value = technicianJob.logDetails.CreatedDate;
                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        outParameter1 = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outParameter1.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            switch(outParameter.Value.ToString())
            {
               
                case "1":
                    return new
                    {
                        jobID = Guid.Parse(outParameter1.Value.ToString()),
                        Status = outParameter.Value.ToString(),
                        Message = constObj.InsertSuccess
                    };
               
                case "2":
                    return new
                    {
                        Status = outParameter.Value.ToString(),
                        Message = constObj.JobDuplicate
                    };
                   
                default:
                    return new
                    {
                        Status = outParameter.Value.ToString(),
                        Message = constObj.InsertFailure
                    };
                   
            }
          
            
        }
        #endregion InsertJob

        #region UpdateJob
        public object UpdateJob(TechnicianJob technicianJob)
        {
            SqlParameter outParameter=null;
            try
            {

                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[UpdateTechnicianJob]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = technicianJob.SCCode;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = technicianJob.ID;
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = technicianJob.TechEmpID;
                        cmd.Parameters.Add("@ServiceDate", SqlDbType.DateTime).Value = DateTime.Parse(technicianJob.ServiceDate);
                        cmd.Parameters.Add("@JobNo", SqlDbType.NVarChar, 50).Value = technicianJob.JobNo;
                        cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar, 250).Value = technicianJob.CustomerName;
                        cmd.Parameters.Add("@CustomerLocation", SqlDbType.NVarChar, 250).Value = technicianJob.CustomerLocation;
                        cmd.Parameters.Add("@ServiceTypeCode", SqlDbType.NVarChar, 5).Value = technicianJob.ServiceType;
                        cmd.Parameters.Add("@CallTypeCode", SqlDbType.NVarChar, 20).Value = technicianJob.CallType;
                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar, 50).Value = technicianJob.ModelNo;
                        cmd.Parameters.Add("@SerialNo", SqlDbType.NVarChar, 50).Value = technicianJob.SerialNo;
                        cmd.Parameters.Add("@CallStatusCode", SqlDbType.NVarChar, 5).Value = technicianJob.CallStatusCode;
                        cmd.Parameters.Add("@ICRNo", SqlDbType.NVarChar, 50).Value = technicianJob.MobileNumber;
                        cmd.Parameters.Add("@TechnicianRemarks", SqlDbType.NVarChar, -1).Value = technicianJob.TechnicianRemark;
                        if (technicianJob.Repeat_EmpID != Guid.Empty)
                        {
                            cmd.Parameters.Add("@Repeat_EmpID", SqlDbType.UniqueIdentifier).Value = technicianJob.Repeat_EmpID;
                        }
                        if (!string.IsNullOrEmpty(technicianJob.Repeat_JobNo))
                        {
                            cmd.Parameters.Add("@Repeat_JobNo", SqlDbType.NVarChar, 50).Value = technicianJob.Repeat_JobNo;
                        }
                        cmd.Parameters.Add("@SpecialCommission", SqlDbType.Decimal).Value = technicianJob.SCCommAmount;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = technicianJob.logDetails.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = technicianJob.logDetails.UpdatedDate;
                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                     
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (outParameter.Value.ToString() == "1")
            {
                return new
                {
                 Message = constObj.UpdateSuccess
                };
            }
            else
            {
                return new
                {
                    Message = constObj.UpdateFailure
                };
            }

        }
        #endregion UpdateJob


        #region DeleteJob
        public object DeleteJob(Job job)
        {
            SqlParameter outParameter = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[DeleteServiceRegisterEntry]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = job.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar).Value = job.SCCode;
                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
            if (outParameter.Value.ToString()=="1")
            {
                return new
                {

                 
                    Message = constObj.DeleteSuccess
                };
            }
            else
            {
                return new
                {

                    
                    Message = constObj.DeleteFailure
                };
            }
          
          
        }
        #endregion DeleteJob

        #region GetServiceRegistrySummary
        public List<ServiceRegistrySummary> GetServiceRegistrySummary(string SCCode, string serviceDate)
        {
            List<ServiceRegistrySummary> serviceregistryList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[GetServiceRegisterSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = SCCode;
                        cmd.Parameters.Add("@serviceDate", SqlDbType.DateTime).Value = serviceDate;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                serviceregistryList = new List<ServiceRegistrySummary>();
                                while (sdr.Read())
                                {
                                    ServiceRegistrySummary _serviceRegistrySummary = new ServiceRegistrySummary();
                                    {
                                        _serviceRegistrySummary.ServiceDate = (sdr["ServiceDate"].ToString() != "" ? (sdr["ServiceDate"].ToString()) : _serviceRegistrySummary.ServiceDate);
                                        _serviceRegistrySummary.Technician = (sdr["Technician"].ToString() != "" ? (sdr["Technician"].ToString()) : _serviceRegistrySummary.Technician);
                                        _serviceRegistrySummary.EmpID = (sdr["EmpID"].ToString() != "" ? (sdr["EmpID"].ToString()) : _serviceRegistrySummary.EmpID);
                                        _serviceRegistrySummary.TotalCalls = (sdr["TotalCalls"].ToString() != "" ? (int.Parse(sdr["TotalCalls"].ToString())) : _serviceRegistrySummary.TotalCalls);
                                        _serviceRegistrySummary.MinorCalls = (sdr["MinorCalls"].ToString() != "" ? (int.Parse(sdr["MinorCalls"].ToString())) : _serviceRegistrySummary.MinorCalls);
                                        _serviceRegistrySummary.MajorCalls = (sdr["MajorCalls"].ToString() != "" ? (int.Parse(sdr["MajorCalls"].ToString())) : _serviceRegistrySummary.MajorCalls);
                                        _serviceRegistrySummary.MandatoryCalls = (sdr["MandatoryCalls"].ToString() != "" ? (int.Parse(sdr["MandatoryCalls"].ToString())) : _serviceRegistrySummary.MandatoryCalls);
                                        _serviceRegistrySummary.DemoCalls = (sdr["DemoCalls"].ToString() != "" ? (int.Parse(sdr["DemoCalls"].ToString())) : _serviceRegistrySummary.DemoCalls);
                                        _serviceRegistrySummary.RepeatCalls = (sdr["RepeatCalls"].ToString() != "" ? (int.Parse(sdr["RepeatCalls"].ToString())) : _serviceRegistrySummary.RepeatCalls);
                                    }
                                    serviceregistryList.Add(_serviceRegistrySummary);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return serviceregistryList;
        }
        #endregion GetServiceRegistrySummary

        #region GetServiceRegisterSummaryFilter
        public List<ServiceRegistrySummary> GetServiceRegisterSummaryFilter(string SCCode, string CreatedDate, string Isdefault)
        {
            List<ServiceRegistrySummary> serviceregistryList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[GetServiceRegisterSummaryFilter]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = SCCode;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = CreatedDate;
                        cmd.Parameters.Add("@IsDefault", SqlDbType.Bit).Value = bool.Parse(Isdefault);
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                serviceregistryList = new List<ServiceRegistrySummary>();
                                while (sdr.Read())
                                {
                                    ServiceRegistrySummary _serviceRegistrySummary = new ServiceRegistrySummary();
                                    {
                                        _serviceRegistrySummary.ServiceDate = (sdr["ServiceDate"].ToString() != "" ? (sdr["ServiceDate"].ToString()) : _serviceRegistrySummary.ServiceDate);
                                        _serviceRegistrySummary.EmpID = (sdr["EmpID"].ToString() != "" ? (sdr["EmpID"].ToString()) : _serviceRegistrySummary.EmpID);
                                        _serviceRegistrySummary.Technician = (sdr["Technician"].ToString() != "" ? (sdr["Technician"].ToString()) : _serviceRegistrySummary.Technician);
                                        _serviceRegistrySummary.TotalCalls = (sdr["TotalCalls"].ToString() != "" ? (int.Parse(sdr["TotalCalls"].ToString())) : _serviceRegistrySummary.TotalCalls);
                                        _serviceRegistrySummary.MinorCalls = (sdr["MinorCalls"].ToString() != "" ? (int.Parse(sdr["MinorCalls"].ToString())) : _serviceRegistrySummary.MinorCalls);
                                        _serviceRegistrySummary.MajorCalls = (sdr["MajorCalls"].ToString() != "" ? (int.Parse(sdr["MajorCalls"].ToString())) : _serviceRegistrySummary.MajorCalls);
                                        _serviceRegistrySummary.MandatoryCalls = (sdr["MandatoryCalls"].ToString() != "" ? (int.Parse(sdr["MandatoryCalls"].ToString())) : _serviceRegistrySummary.MandatoryCalls);
                                        _serviceRegistrySummary.DemoCalls = (sdr["DemoCalls"].ToString() != "" ? (int.Parse(sdr["DemoCalls"].ToString())) : _serviceRegistrySummary.DemoCalls);
                                        _serviceRegistrySummary.RepeatCalls = (sdr["RepeatCalls"].ToString() != "" ? (int.Parse(sdr["RepeatCalls"].ToString())) : _serviceRegistrySummary.RepeatCalls);
                                    }
                                    serviceregistryList.Add(_serviceRegistrySummary);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return serviceregistryList;
        }
        #endregion GetServiceRegisterSummaryFilter
    }
}