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
    public class CallandServiceTypesRepository : ICallandServiceTypesRepository
    {
        Const c = new Const();
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public CallandServiceTypesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region Methods

      

        #region GetServiceTypes
        public List<ServiceTypes> GetServiceTypes(UA UA)
        {
            List<ServiceTypes> serviceTypeslist = null;
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
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.CommandText = "[GetServiceTypes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                serviceTypeslist = new List<ServiceTypes>();
                                while (sdr.Read())
                                {
                                    ServiceTypes _ServiceTypesObj = new ServiceTypes();
                                    {
                                        _ServiceTypesObj.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : _ServiceTypesObj.Code);
                                        switch(_ServiceTypesObj.Code)
                                        {
                                            case "DMO":
                                                _ServiceTypesObj.DemoCommission = (sdr["Commission"].ToString() != "" ? float.Parse(sdr["Commission"].ToString()) : _ServiceTypesObj.DemoCommission);
                                                break;
                                            case "MJR":
                                                _ServiceTypesObj.MajorCommission = (sdr["Commission"].ToString() != "" ? float.Parse(sdr["Commission"].ToString()) : _ServiceTypesObj.MajorCommission);
                                                break;
                                            case "MND":
                                                _ServiceTypesObj.MandatoryCommission = (sdr["Commission"].ToString() != "" ? float.Parse(sdr["Commission"].ToString()) : _ServiceTypesObj.MandatoryCommission);
                                                break;
                                            case "MNR":
                                                _ServiceTypesObj.MinorCommission = (sdr["Commission"].ToString() != "" ? float.Parse(sdr["Commission"].ToString()) : _ServiceTypesObj.MinorCommission);
                                                break;
                                            case "RPT":
                                                _ServiceTypesObj.RepeatCommission = (sdr["Commission"].ToString() != "" ? float.Parse(sdr["Commission"].ToString()) : _ServiceTypesObj.RepeatCommission);
                                                break;
                                            case "AMC1":
                                                _ServiceTypesObj.AMC1Commission = (sdr["Commission"].ToString() != "" ? float.Parse(sdr["Commission"].ToString()) : _ServiceTypesObj.AMC1Commission);
                                                break;
                                            case "AMC2":
                                                _ServiceTypesObj.AMC2Commission = (sdr["Commission"].ToString() != "" ? float.Parse(sdr["Commission"].ToString()) : _ServiceTypesObj.AMC2Commission);
                                                break;
                                            
                                        }
                                     }

                                    serviceTypeslist.Add(_ServiceTypesObj);
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
            return serviceTypeslist;
        }
        #endregion  GetServiceTypes

      

        #region UpdateServiceType
        public Object UpdateServiceTypesAndCommission(ServiceTypes serviceTypesObj)
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
                        cmd.CommandText = "[UpdateServiceTypes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = serviceTypesObj.SCCode;
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 20).Value = serviceTypesObj.Code;
                        cmd.Parameters.Add("@MinorCommission", SqlDbType.Decimal).Value = serviceTypesObj.MinorCommission;
                        cmd.Parameters.Add("@MajorCommission", SqlDbType.Decimal).Value = serviceTypesObj.MajorCommission;
                        cmd.Parameters.Add("@MandatoryCommission", SqlDbType.Decimal).Value = serviceTypesObj.MandatoryCommission;
                        cmd.Parameters.Add("@RepeatCommission", SqlDbType.Decimal).Value = serviceTypesObj.RepeatCommission;
                        cmd.Parameters.Add("@DemoCommission", SqlDbType.Decimal).Value = serviceTypesObj.DemoCommission;
                        cmd.Parameters.Add("@AMC1Commission", SqlDbType.Decimal).Value = serviceTypesObj.AMC1Commission;
                        cmd.Parameters.Add("@AMC2Commission", SqlDbType.Decimal).Value = serviceTypesObj.AMC2Commission;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = serviceTypesObj.logDetails.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = serviceTypesObj.logDetails.CreatedDate;

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
            return outParameter.Value.ToString();
              
        }
        #endregion UpdateServiceType

        #endregion Methods
    }
}