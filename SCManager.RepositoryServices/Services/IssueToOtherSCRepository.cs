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
    public class IssueToOtherSCRepository : IIssueToOtherSCRepository
    {
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public IssueToOtherSCRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region GetAllIssueToOtherSC
        public List<IssueToOtherSC> GetAllIssueToOtherSC(UA UA)
        {
            List<IssueToOtherSC> IssueToOtherSClist = null;
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
                        cmd.CommandText = "[GetAllIssueToOtherSC]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                IssueToOtherSClist = new List<IssueToOtherSC>();
                                while (sdr.Read())
                                {
                                    IssueToOtherSC _IssueToOtherSClistObj = new IssueToOtherSC();
                                    {
                                        _IssueToOtherSClistObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _IssueToOtherSClistObj.ID);
                                        _IssueToOtherSClistObj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()) : _IssueToOtherSClistObj.InvoiceDate);
                                        _IssueToOtherSClistObj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? (sdr["InvoiceNo"].ToString()) : _IssueToOtherSClistObj.InvoiceNo);

                                        _IssueToOtherSClistObj.ToSCName = (sdr["ToSCName"].ToString() != "" ? (sdr["ToSCName"].ToString()) : _IssueToOtherSClistObj.ToSCName);

                                        _IssueToOtherSClistObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _IssueToOtherSClistObj.Remarks);
                                        _IssueToOtherSClistObj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : _IssueToOtherSClistObj.VATAmount);

                                        _IssueToOtherSClistObj.Subtotal = (sdr["SubTotal"].ToString() != "" ? decimal.Parse(sdr["SubTotal"].ToString()) : _IssueToOtherSClistObj.Subtotal);

                                    }

                                    IssueToOtherSClist.Add(_IssueToOtherSClistObj);
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
            return IssueToOtherSClist;
        }
        #endregion  GetAllIssueToOtherSC

        #region InsertIssueToOtherSC
        public IssueToOtherSC InsertIssueToOtherSC(IssueToOtherSC issueToOtherSC, UA UA)
        {

            try
            {
                SqlParameter outputStatus, outputID = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[InsertIssueToOtherSC]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar, 20).Value = issueToOtherSC.InvoiceNo;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.SmallDateTime).Value = issueToOtherSC.InvoiceDate;
                        cmd.Parameters.Add("@ToSCName", SqlDbType.NVarChar, 250).Value = issueToOtherSC.ToSCName;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = issueToOtherSC.Remarks;
                        cmd.Parameters.Add("@VATAmount", SqlDbType.Decimal).Value = issueToOtherSC.VATAmount;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = issueToOtherSC.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = UA.UserName;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = UA.CurrentDatetime();

                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        Const Cobj = new Const();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        issueToOtherSC.ID = new Guid(outputID.Value.ToString());
                        issueToOtherSC.IssueToOtherScDetail = GetIssueToOtherScDetail(issueToOtherSC.ID, UA);

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return issueToOtherSC;
        }
        #endregion InsertIssueToOtherSC

        #region UpdateIssueToOtherSC
        public IssueToOtherSC UpdateIssueToOtherSC(IssueToOtherSC issueToOtherSC, UA UA)
        {
            try
            {
                SqlParameter outputStatus;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[UpdateIssueToOtherSC]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = issueToOtherSC.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar, 20).Value = issueToOtherSC.InvoiceNo;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.SmallDateTime).Value = issueToOtherSC.InvoiceDate;
                        cmd.Parameters.Add("@ToSCName", SqlDbType.NVarChar, 250).Value = issueToOtherSC.ToSCName;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = issueToOtherSC.Remarks;
                        cmd.Parameters.Add("@VATAmount", SqlDbType.Decimal).Value = issueToOtherSC.VATAmount;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = issueToOtherSC.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = UA.UserName;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = UA.CurrentDatetime();

                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();



                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        Const Cobj = new Const();
                        throw new Exception(Cobj.UpdateFailure);

                    case "1":
                        issueToOtherSC.IssueToOtherScDetail = GetIssueToOtherScDetail(issueToOtherSC.ID, UA);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return issueToOtherSC;
        }
        #endregion UpdateIssueToOtherSC

        #region GetIssueToOtherSCDetail
        public List<IssueToOtherScDetail> GetIssueToOtherScDetail(Guid ID, UA UA)
        {
            List<IssueToOtherScDetail> issueToOtherScDetailList = null;
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
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandText = "[GetIssueToOtherSCDetailByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                issueToOtherScDetailList = new List<IssueToOtherScDetail>();
                                while (sdr.Read())
                                {
                                    IssueToOtherScDetail _IssueToOtherScDetailObj = new IssueToOtherScDetail();
                                    {
                                        _IssueToOtherScDetailObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _IssueToOtherScDetailObj.ID);
                                        _IssueToOtherScDetailObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _IssueToOtherScDetailObj.SCCode);
                                        _IssueToOtherScDetailObj.MaterialID = (sdr["ItemID"].ToString() != "" ? Guid.Parse(sdr["ItemID"].ToString()) : _IssueToOtherScDetailObj.MaterialID);
                                        _IssueToOtherScDetailObj.Quantity = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);
                                        _IssueToOtherScDetailObj.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : 0);
                                        _IssueToOtherScDetailObj.TradeDiscount = (sdr["TradeDiscount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscount"].ToString()) : 0);
                                        _IssueToOtherScDetailObj.Material = (sdr["Material"].ToString() != "" ? (sdr["Material"].ToString()) : _IssueToOtherScDetailObj.Material);
                                        _IssueToOtherScDetailObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _IssueToOtherScDetailObj.Description);
                                        _IssueToOtherScDetailObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _IssueToOtherScDetailObj.UOM);
                                    }

                                    issueToOtherScDetailList.Add(_IssueToOtherScDetailObj);
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
            return issueToOtherScDetailList;
        }
        #endregion GetIssueToOtherScDetail

        #region GetIssueToOtherSCByID
        public IssueToOtherSC GetIssueToOtherSCByID(Guid ID, UA ua)
        {
            IssueToOtherSC IssueToOtherSCList = null;
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
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = ua.SCCode;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandText = "[GetIssueToOtherSCByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {

                                while (sdr.Read())
                                {
                                    IssueToOtherSC _IssueToOtherSCObj = new IssueToOtherSC();
                                    {
                                        _IssueToOtherSCObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _IssueToOtherSCObj.ID);
                                        _IssueToOtherSCObj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()) : _IssueToOtherSCObj.InvoiceDate);
                                        _IssueToOtherSCObj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? (sdr["InvoiceNo"].ToString()) : _IssueToOtherSCObj.InvoiceNo);
                                        _IssueToOtherSCObj.ToSCName = (sdr["ToSCName"].ToString() != "" ? (sdr["ToSCName"].ToString()) : _IssueToOtherSCObj.ToSCName);
                                        _IssueToOtherSCObj.Subtotal = (sdr["TotalValue"].ToString() != "" ? decimal.Parse(sdr["TotalValue"].ToString()) : _IssueToOtherSCObj.Subtotal);
                                        _IssueToOtherSCObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _IssueToOtherSCObj.Remarks);
                                        _IssueToOtherSCObj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : _IssueToOtherSCObj.VATAmount);

                                    }

                                    IssueToOtherSCList = _IssueToOtherSCObj;
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
            return IssueToOtherSCList;
        }
        #endregion GetIssueToOtherSCByID

        #region DeleteIssueToOtherSCDetail
        public bool DeleteIssueToOtherSCDetail(Guid ID, Guid HeaderID, UA UA)
        {
            bool result = false;
            try
            {
                SqlParameter outputStatus;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[DeleteIssueToOtherSCDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.Parameters.Add("@HeaderID", SqlDbType.UniqueIdentifier).Value = HeaderID;
                        cmd.Parameters.Add("@DeletedBy", SqlDbType.NVarChar, 250).Value = UA.UserName;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        Const Cobj = new Const();
                        throw new Exception(Cobj.DeleteFailure);
                    case "1":
                        return true;

                    default:
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        #endregion DeleteIssueToOtherSCDetail

        #region DeleteIssueToOtherSC
        public bool DeleteIssueToOtherSC(Guid ID, UA UA)
        {
            bool result = false;
            try
            {
                SqlParameter outputStatus;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[DeleteIssueToOtherSC]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.Parameters.Add("@DeletedBy", SqlDbType.NVarChar, 250).Value = UA.UserName;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        Const Cobj = new Const();
                        throw new Exception(Cobj.DeleteFailure);
                    case "1":
                        return true;

                    default:
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        #endregion DeleteIssueToOtherSC
    }
}