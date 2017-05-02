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

        #region UpdateCallType
        public string UpdateCallType(CallTypes callTypesObj)
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
                        cmd.CommandText = "[UpdateCallType]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = callTypesObj.SCCode;
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 20).Value = callTypesObj.Code;
                        cmd.Parameters.Add("@MinorCommission", SqlDbType.Decimal).Value = callTypesObj.MinorCommission;
                        cmd.Parameters.Add("@MajorCommission", SqlDbType.Decimal).Value = callTypesObj.MajorCommission;
                        cmd.Parameters.Add("@MandatoryCommission", SqlDbType.Decimal).Value = callTypesObj.MandatoryCommission;
                        cmd.Parameters.Add("@RepeatCommission", SqlDbType.Decimal).Value = callTypesObj.RepeatCommission;
                        cmd.Parameters.Add("@DemoCommission", SqlDbType.Decimal).Value = callTypesObj.DemoCommission;
                        cmd.Parameters.Add("@SubType", SqlDbType.NChar,10).Value = callTypesObj.SubType;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = callTypesObj.logDetails.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = callTypesObj.logDetails.CreatedDate;

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
        #endregion UpdateCallType

        #region UpdateServiceType
        public string UpdateServiceType(ServiceTypes serviceTypesObj)
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