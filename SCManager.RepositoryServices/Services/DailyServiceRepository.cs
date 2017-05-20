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
        public List<ServiceType> GetAllServiceTypes()
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
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = technicianJob.EmpID;
                        cmd.Parameters.Add("@SeviceDate", SqlDbType.DateTime).Value = technicianJob.ServiceDate;
                        cmd.Parameters.Add("@JobNo", SqlDbType.NVarChar, 50).Value = technicianJob.JobNo;
                        cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar, 250).Value = technicianJob.CustomerName;
                        cmd.Parameters.Add("@CustomerLocation", SqlDbType.NVarChar, 250).Value = technicianJob.CustomerLocation;
                        cmd.Parameters.Add("@ServiceTypeCode", SqlDbType.NVarChar, 5).Value = technicianJob.ServiceTypeCode;
                        cmd.Parameters.Add("@CallTypeCode", SqlDbType.NVarChar, 20).Value = technicianJob.CallTypeCode;
                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar, 50).Value = technicianJob.ModelNo;
                        cmd.Parameters.Add("@SerialNo", SqlDbType.NVarChar, 50).Value = technicianJob.SerialNo;
                        cmd.Parameters.Add("@CallStatusCode", SqlDbType.NVarChar, 5).Value = technicianJob.CallStatusCode;
                        cmd.Parameters.Add("@ICRNo", SqlDbType.NVarChar, 50).Value = technicianJob.ICRNo;
                        cmd.Parameters.Add("@TechnicianRemarks", SqlDbType.NVarChar, -1).Value = technicianJob.TechnicianRemarks;
                        cmd.Parameters.Add("@Repeat_EmpID", SqlDbType.UniqueIdentifier).Value = technicianJob.Repeat_EmpID;
                        cmd.Parameters.Add("@Repeat_JobNo", SqlDbType.NVarChar, 50).Value = technicianJob.Repeat_JobNo;
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
            return new
            {
                jobID = Guid.Parse(outParameter1.Value.ToString()),
                Status = outParameter.Value.ToString(),
                Message = constObj.InsertSuccess
            };
        }
        #endregion InsertJob

    }
}