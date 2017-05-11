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
    public class OtherIncomeRepository : IOtherIncomeRepository
    {
        Const c = new Const();

        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public OtherIncomeRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region Methods

        #region GetAllOtherIncome
        public List<OtherIncome> GetAllOtherIncome(UA UA, string showAllYN)
        {
            List<OtherIncome> OtherIncomelist = null;
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
                        cmd.Parameters.Add("@showAllYN", SqlDbType.NVarChar, 5).Value = showAllYN;
                        cmd.CommandText = "[GetAllOtherIncome]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                OtherIncomelist = new List<OtherIncome>();
                                while (sdr.Read())
                                {
                                    OtherIncome otherIncomeObj = new OtherIncome();

                                    {
                                        otherIncomeObj.RefDate = (sdr["RefDate"].ToString() != "" ? DateTime.Parse(sdr["RefDate"].ToString()) : otherIncomeObj.RefDate);
                                        otherIncomeObj.RefNo = (sdr["RefNo"].ToString() != "" ? (sdr["RefNo"].ToString()) : otherIncomeObj.RefNo);
                                        otherIncomeObj.IncomeTypeCode = (sdr["IncomeTypeCode"].ToString() != "" ? (sdr["IncomeTypeCode"].ToString()) : otherIncomeObj.IncomeTypeCode);
                                        otherIncomeObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : otherIncomeObj.Description);
                                        otherIncomeObj.Amount = (sdr["Amount"].ToString() != "" ? float.Parse(sdr["Amount"].ToString()) : otherIncomeObj.Amount);
                                        otherIncomeObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : otherIncomeObj.ID);

                                    };

                                    OtherIncomelist.Add(otherIncomeObj);
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
            return OtherIncomelist;
        }
        #endregion GetAllOtherIncome

        #region GetOtherIncomeByID
        public List<OtherIncome> GetOtherIncomeByID(UA UA, string ID)
        {
            List<OtherIncome> OtherIncomelist = null;
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
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                        cmd.CommandText = "[GetOtherIncomeByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                OtherIncomelist = new List<OtherIncome>();
                                while (sdr.Read())
                                {
                                    OtherIncome otherIncomeObj = new OtherIncome();

                                    {
                                        otherIncomeObj.RefDate = (sdr["RefDate"].ToString() != "" ? DateTime.Parse(sdr["RefDate"].ToString()) : otherIncomeObj.RefDate);
                                        otherIncomeObj.RefNo = (sdr["RefNo"].ToString() != "" ? (sdr["RefNo"].ToString()) : otherIncomeObj.RefNo);
                                        otherIncomeObj.IncomeTypeCode = (sdr["IncomeTypeCode"].ToString() != "" ? (sdr["IncomeTypeCode"].ToString()) : otherIncomeObj.IncomeTypeCode);
                                        otherIncomeObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : otherIncomeObj.Description);
                                        otherIncomeObj.Amount = (sdr["Amount"].ToString() != "" ? float.Parse(sdr["Amount"].ToString()) : otherIncomeObj.Amount);
                                        otherIncomeObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : otherIncomeObj.ID);
                                        otherIncomeObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? (sdr["PaymentMode"].ToString()) : otherIncomeObj.PaymentMode);
                                    };

                                    OtherIncomelist.Add(otherIncomeObj);
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
            return OtherIncomelist;
        }
        #endregion GetOtherIncomeByID

        #region InsertOtherIncome
        public object InsertOtherIncome(OtherIncome otherIncomeObj)
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
                        cmd.CommandText = "[InsertOtherIncome]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = otherIncomeObj.SCCode;
                        cmd.Parameters.Add("@EntryNo", SqlDbType.NVarChar, 20).Value = otherIncomeObj.EntryNo;
                        cmd.Parameters.Add("@IncomeTypeCode", SqlDbType.NVarChar, 5).Value = otherIncomeObj.IncomeTypeCode;
                        cmd.Parameters.Add("@RefNo", SqlDbType.NVarChar, 20).Value = otherIncomeObj.RefNo;
                        cmd.Parameters.Add("@RefDate", SqlDbType.SmallDateTime).Value = otherIncomeObj.RefDate;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar, 20).Value = otherIncomeObj.PaymentMode;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = otherIncomeObj.Amount;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = otherIncomeObj.Description;
                     
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = otherIncomeObj.logDetails.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.SmallDateTime).Value = otherIncomeObj.logDetails.CreatedDate;

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
                ID = Guid.Parse(outParameter1.Value.ToString()),
                Status = outParameter.Value.ToString(),
                Message = c.InsertSuccess
            };
        }
        #endregion InsertOtherIncome

        #region UpdateOtherIncome
        public object UpdateOtherIncome(OtherIncome otherIncomeObj)
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
                        cmd.CommandText = "[UpdateOtherIncome]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = otherIncomeObj.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = otherIncomeObj.SCCode;
                        cmd.Parameters.Add("@IncomeTypeCode", SqlDbType.NVarChar, 5).Value = otherIncomeObj.IncomeTypeCode;
                        cmd.Parameters.Add("@RefNo", SqlDbType.NVarChar, 20).Value = otherIncomeObj.RefNo;
                        cmd.Parameters.Add("@RefDate", SqlDbType.SmallDateTime).Value = otherIncomeObj.RefDate;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar, 20).Value = otherIncomeObj.PaymentMode;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = otherIncomeObj.Amount;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = otherIncomeObj.Description;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = otherIncomeObj.logDetails.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = otherIncomeObj.logDetails.CreatedDate;

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
            return new
            {
                Status = outParameter.Value.ToString(),
                Message = c.UpdateSuccess
            };
        }
        #endregion UpdateOtherIncome

        #region GetAllIncomeType
        public List<OtherIncome> GetAllIncomeType()
        {
            List<OtherIncome> IncomeTypelist = null;
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
                        cmd.CommandText = "[GetAllIncomeType]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                IncomeTypelist = new List<OtherIncome>();
                                while (sdr.Read())
                                {
                                    OtherIncome otherIncomeObj = new OtherIncome();

                                    {
                                        otherIncomeObj.IncomeTypeCode = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : otherIncomeObj.IncomeTypeCode);
                                        otherIncomeObj.IncomeTypeDescription = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : otherIncomeObj.IncomeTypeDescription);

                                    };

                                    IncomeTypelist.Add(otherIncomeObj);
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
            return IncomeTypelist;
        }
        #endregion GetAllIncomeType

        #region GetOtherIncomeBetweenDates
        public List<OtherIncome> GetOtherIncomeBetweenDates(UA UA, string fromDate, string toDate)
        {
            List<OtherIncome> OtherIncomelist = null;
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
                        if (!string.IsNullOrEmpty(fromDate))
                        {
                            cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DateTime.Parse(fromDate);
                        }
                        if (!string.IsNullOrEmpty(toDate))
                        {
                            cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = DateTime.Parse(toDate);
                        }

                        cmd.CommandText = "[GetOtherIncomeBetweenDates]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                OtherIncomelist = new List<OtherIncome>();
                                while (sdr.Read())
                                {
                                    OtherIncome otherIncomeObj = new OtherIncome();

                                    {
                                        otherIncomeObj.RefDate = (sdr["RefDate"].ToString() != "" ? DateTime.Parse(sdr["RefDate"].ToString()) : otherIncomeObj.RefDate);
                                        otherIncomeObj.RefNo = (sdr["RefNo"].ToString() != "" ? (sdr["RefNo"].ToString()) : otherIncomeObj.RefNo);
                                        otherIncomeObj.IncomeTypeCode = (sdr["IncomeTypeCode"].ToString() != "" ? (sdr["IncomeTypeCode"].ToString()) : otherIncomeObj.IncomeTypeCode);
                                        otherIncomeObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : otherIncomeObj.Description);
                                        otherIncomeObj.Amount = (sdr["Amount"].ToString() != "" ? float.Parse(sdr["Amount"].ToString()) : otherIncomeObj.Amount);
                                        otherIncomeObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : otherIncomeObj.ID);

                                    };

                                    OtherIncomelist.Add(otherIncomeObj);
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
            return OtherIncomelist;
        }
        #endregion GetOtherIncomeBetweenDates

        #region DeleteOtherIncome
        public string DeleteOtherIncome(string ID, UA ua)
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
                        cmd.CommandText = "[DeleteOtherIncome]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = ua.SCCode;

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
        #endregion DeleteOtherIncome

        #endregion Methods
    }
}