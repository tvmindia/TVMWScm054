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
    public class SalesReturnRepository : ISalesReturnRepository
    {
        Const c = new Const();
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public SalesReturnRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region Methods
        #region GetAllSalesReturn
        public List<SalesReturn> GetAllSalesReturn(UA UA)
        {
            List<SalesReturn> SalesReturnlist = null;
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
                        cmd.CommandText = "[GetAllSalesReturn]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SalesReturnlist = new List<SalesReturn>();
                                while (sdr.Read())
                                {
                                    SalesReturn _SalesReturnlistObj = new SalesReturn();
                                    {
                                        _SalesReturnlistObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _SalesReturnlistObj.ID);
                                        _SalesReturnlistObj.ItemCode = (sdr["ItemCode"].ToString() != "" ? (sdr["ItemCode"].ToString()) : _SalesReturnlistObj.ItemCode);
                                       
                                        _SalesReturnlistObj.OpenDate = (sdr["OpenDate"].ToString() != "" ? DateTime.Parse(sdr["OpenDate"].ToString()) : _SalesReturnlistObj.OpenDate);
                                        _SalesReturnlistObj.RefNo = (sdr["RefNo"].ToString() != "" ? (sdr["RefNo"].ToString()) : _SalesReturnlistObj.RefNo);
                                        _SalesReturnlistObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _SalesReturnlistObj.Description);
                                        _SalesReturnlistObj.Qty = (sdr["Qty"].ToString() != "" ? (sdr["Qty"].ToString()) : _SalesReturnlistObj.Qty);
                                        _SalesReturnlistObj.ReturnStatusYN = (sdr["ReturnStatusYN"].ToString() != "" ? bool.Parse(sdr["ReturnStatusYN"].ToString()) : _SalesReturnlistObj.ReturnStatusYN);
                                        _SalesReturnlistObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _SalesReturnlistObj.Remarks);
                                    }

                                    SalesReturnlist.Add(_SalesReturnlistObj);
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
            return SalesReturnlist;
        }
        #endregion  GetAllSalesReturn

        #region SalesReturnValidation
        public string SalesReturnValidation(string itemID, UA ua)
        {

            int result;
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
                        cmd.CommandText = "[GetItemStockByLocation]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ItemID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(itemID);
                        //cmd.Parameters.Add("@TechID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(empID);
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = ua.SCCode;
                        cmd.Parameters.Add("@LocType", SqlDbType.NVarChar, 5).Value = "OFFC";

                        result = int.Parse(cmd.ExecuteScalar().ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result.ToString();
        }
        #endregion SalesReturnValidation

        #region InsertSalesReturn
        public object InsertSalesReturn(SalesReturn SalesReturn)
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
                        cmd.CommandText = "[InsertSalesReturn]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = SalesReturn.SCCode;
                        cmd.Parameters.Add("@OpenDate", SqlDbType.DateTime).Value = SalesReturn.OpenDate;
                        cmd.Parameters.Add("@RefNo", SqlDbType.NVarChar, 50).Value = SalesReturn.RefNo;
                        cmd.Parameters.Add("@ItemID", SqlDbType.UniqueIdentifier).Value = SalesReturn.ItemID;
                        cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = SalesReturn.Qty;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = SalesReturn.Remarks;
                        cmd.Parameters.Add("@ReturnStatusYN", SqlDbType.Bit).Value = SalesReturn.ReturnStatusYN;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = SalesReturn.logDetails.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.SmallDateTime).Value = SalesReturn.logDetails.CreatedDate;

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
        #endregion InsertSalesReturn

        #region UpdateSalesReturn
        public object UpdateSalesReturn(SalesReturn salesReturn)
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
                        cmd.CommandText = "[UpdateSalesReturn]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = salesReturn.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = salesReturn.SCCode;
                        cmd.Parameters.Add("@OpenDate", SqlDbType.DateTime).Value = salesReturn.OpenDate;
                        cmd.Parameters.Add("@RefNo", SqlDbType.NVarChar, 50).Value = salesReturn.RefNo;
                        cmd.Parameters.Add("@ItemID", SqlDbType.UniqueIdentifier).Value = salesReturn.ItemID;
                        cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = salesReturn.Qty;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = salesReturn.Remarks;
                        cmd.Parameters.Add("@ReturnStatusYN", SqlDbType.Bit).Value = salesReturn.ReturnStatusYN;
                        cmd.Parameters.Add("@ReturnDate", SqlDbType.DateTime).Value = salesReturn.ReturnDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = salesReturn.logDetails.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = salesReturn.logDetails.CreatedDate;

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
        #endregion UpdateSalesReturn

        #region GetSalesReturnByID
        public List<SalesReturn> GetSalesReturnByID(UA UA, string ID)
        {
            List<SalesReturn> SalesReturnlist = null;
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
                        cmd.CommandText = "[GetSalesReturnByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SalesReturnlist = new List<SalesReturn>();
                                while (sdr.Read())
                                {
                                    SalesReturn _SalesReturnlistObj = new SalesReturn();
                                    {
                                        _SalesReturnlistObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _SalesReturnlistObj.ID);
                                        _SalesReturnlistObj.ItemID = (sdr["ItemID"].ToString() != "" ? Guid.Parse(sdr["ItemID"].ToString()) : _SalesReturnlistObj.ItemID);
                                        
                                        _SalesReturnlistObj.ItemCode = (sdr["ItemCode"].ToString() != "" ? (sdr["ItemCode"].ToString()) : _SalesReturnlistObj.ItemCode);
                                       
                                        _SalesReturnlistObj.OpenDate = (sdr["OpenDate"].ToString() != "" ? DateTime.Parse(sdr["OpenDate"].ToString()) : _SalesReturnlistObj.OpenDate);
                                        _SalesReturnlistObj.RefNo = (sdr["RefNo"].ToString() != "" ? (sdr["RefNo"].ToString()) : _SalesReturnlistObj.RefNo);
                                        _SalesReturnlistObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _SalesReturnlistObj.Description);
                                        _SalesReturnlistObj.Qty = (sdr["Qty"].ToString() != "" ? (sdr["Qty"].ToString()) : _SalesReturnlistObj.Qty);
                                        _SalesReturnlistObj.ReturnStatusYN = (sdr["ReturnStatusYN"].ToString() != "" ? bool.Parse(sdr["ReturnStatusYN"].ToString()) : _SalesReturnlistObj.ReturnStatusYN);
                                        _SalesReturnlistObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _SalesReturnlistObj.Remarks);
                                    }

                                    SalesReturnlist.Add(_SalesReturnlistObj);
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
            return SalesReturnlist;
        }
        #endregion  GetSalesReturnByID

        #region DeleteSalesReturn
        public string DeleteSalesReturn(string ID, UA ua)
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
                        cmd.CommandText = "[DeleteSalesReturn]";
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
        #endregion DeleteSalesReturn

        #region ReturnSalesToCompany
        public string ReturnSalesToCompany(string ID, UA ua)
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
                        cmd.CommandText = "[ReturnSalesToCompany]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = ua.SCCode;
                        cmd.Parameters.Add("@ReturnDate", SqlDbType.DateTime).Value = ua.CurrentDatetime();
                        cmd.Parameters.Add("@ReturnStatusYN", SqlDbType.Bit).Value = true;
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
        #endregion ReturnSalesToCompany

        #endregion Methods
    }
}