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
    public class OfficeBillEntryRepository : IOfficeBillEntryRepository
    {
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public OfficeBillEntryRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region GetAllOfficeBillHeader
        public List<OfficeBillEntry> GetAllOfficeBillHeader(UA UA)
        {
            List<OfficeBillEntry> OfficeBillEntrylist = null;
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
                        cmd.CommandText = "[GetAllOfficeBillHeader]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                OfficeBillEntrylist = new List<OfficeBillEntry>();
                                while (sdr.Read())
                                {
                                    OfficeBillEntry _OfficeBillEntrylistObj = new OfficeBillEntry();
                                    {
                                        _OfficeBillEntrylistObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _OfficeBillEntrylistObj.ID);                                      
                                        _OfficeBillEntrylistObj.BillNo = (sdr["BillNo"].ToString() != "" ? (sdr["BillNo"].ToString()) : _OfficeBillEntrylistObj.BillNo);
                                        _OfficeBillEntrylistObj.BillDate = (sdr["BillDate"].ToString() != "" ? DateTime.Parse(sdr["BillDate"].ToString()) : _OfficeBillEntrylistObj.BillDate);                                      
                                        _OfficeBillEntrylistObj.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : _OfficeBillEntrylistObj.Discount);
                                        _OfficeBillEntrylistObj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : _OfficeBillEntrylistObj.VATAmount);
                                        _OfficeBillEntrylistObj.CustomerName = (sdr["CustomerName"].ToString() != "" ? (sdr["CustomerName"].ToString()) : _OfficeBillEntrylistObj.CustomerName);
                                        _OfficeBillEntrylistObj.CustomerContactNo = (sdr["CustomerContactNo"].ToString() != "" ? (sdr["CustomerContactNo"].ToString()) : _OfficeBillEntrylistObj.CustomerContactNo);
                                        _OfficeBillEntrylistObj.CustomerLocation = (sdr["CustomerLocation"].ToString() != "" ? (sdr["CustomerLocation"].ToString()) : _OfficeBillEntrylistObj.CustomerLocation);                                       
                                        _OfficeBillEntrylistObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _OfficeBillEntrylistObj.Remarks);
                                    }

                                    OfficeBillEntrylist.Add(_OfficeBillEntrylistObj);
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
            return OfficeBillEntrylist;
        }
        #endregion  GetAllOfficeBillHeader

        #region InsertOfficeBillEntry
        public OfficeBillEntry InsertOfficeBillEntry(OfficeBillEntry officeBillEntry, UA UA)
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
                        cmd.CommandText = "[InsertOfficeBillEntry]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@BillNo", SqlDbType.NVarChar, 50).Value = officeBillEntry.BillNo;
                        cmd.Parameters.Add("@BillDate", SqlDbType.SmallDateTime).Value = officeBillEntry.BillDate;                   
                       
                        cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar, 250).Value = officeBillEntry.CustomerName;
                        cmd.Parameters.Add("@CustomerContactNo", SqlDbType.NVarChar, 50).Value = officeBillEntry.CustomerContactNo;
                        cmd.Parameters.Add("@CustomerLocation", SqlDbType.NVarChar, 50).Value = officeBillEntry.CustomerLocation;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar, 20).Value = officeBillEntry.PaymentMode;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = officeBillEntry.Remarks;
                        cmd.Parameters.Add("@VATAmount", SqlDbType.Decimal).Value = officeBillEntry.VATAmount;                    
                       
                        cmd.Parameters.Add("@SubTotalAmount", SqlDbType.Decimal).Value = officeBillEntry.Subtotal;                      
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = officeBillEntry.Discount;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = officeBillEntry.DetailXML;

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
                        officeBillEntry.ID = new Guid(outputID.Value.ToString());
                        officeBillEntry.OfficeBillEntryDetail = GetOfficeBillDetail(officeBillEntry.ID, UA);

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return officeBillEntry;
        }
        #endregion InsertOfficeBillEntry

        #region UpdateOfficeBillEntry
        public OfficeBillEntry UpdateOfficeBillEntry(OfficeBillEntry officeBillEntry, UA UA)
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
                        cmd.CommandText = "[UpdateOfficeBillEntry]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = officeBillEntry.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@BillNo", SqlDbType.NVarChar, 50).Value = officeBillEntry.BillNo;
                        cmd.Parameters.Add("@BillDate", SqlDbType.SmallDateTime).Value = officeBillEntry.BillDate;                       
                        cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar, 250).Value = officeBillEntry.CustomerName;
                        cmd.Parameters.Add("@CustomerContactNo", SqlDbType.NVarChar, 50).Value = officeBillEntry.CustomerContactNo;
                        cmd.Parameters.Add("@CustomerLocation", SqlDbType.NVarChar, 50).Value = officeBillEntry.CustomerLocation;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar, 20).Value = officeBillEntry.PaymentMode;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = officeBillEntry.Remarks;
                        cmd.Parameters.Add("@VATAmount", SqlDbType.Decimal).Value = officeBillEntry.VATAmount;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = officeBillEntry.Discount;
                        cmd.Parameters.Add("@SubTotalAmount", SqlDbType.Decimal).Value = officeBillEntry.Subtotal;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = officeBillEntry.DetailXML;

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
                        officeBillEntry.OfficeBillEntryDetail = GetOfficeBillDetail(officeBillEntry.ID, UA);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return officeBillEntry;
        }
        #endregion UpdateOfficeBillEntry

        #region GetOfficeBillDetail
        public List<OfficeBillEntryDetail> GetOfficeBillDetail(Guid? ID, UA UA)
        {
            List<OfficeBillEntryDetail> OfficeBillEntryDetailList = null;
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
                        cmd.CommandText = "[OfficeBillEntryDetailByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                OfficeBillEntryDetailList = new List<OfficeBillEntryDetail>();
                                while (sdr.Read())
                                {
                                    OfficeBillEntryDetail _OfficeBillEntryDetailObj = new OfficeBillEntryDetail();
                                    {
                                        _OfficeBillEntryDetailObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _OfficeBillEntryDetailObj.ID);
                                        _OfficeBillEntryDetailObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _OfficeBillEntryDetailObj.SCCode);
                                        _OfficeBillEntryDetailObj.Quantity = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);
                                        _OfficeBillEntryDetailObj.SlNo = (sdr["ItemNo"].ToString() != "" ? int.Parse(sdr["ItemNo"].ToString()) : 0);
                                        _OfficeBillEntryDetailObj.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : 0);
                                        _OfficeBillEntryDetailObj.Material = (sdr["Material"].ToString() != "" ? (sdr["Material"].ToString()) : _OfficeBillEntryDetailObj.Material);
                                        _OfficeBillEntryDetailObj.MaterialID = (sdr["ItemID"].ToString() != "" ? Guid.Parse(sdr["ItemID"].ToString()) : _OfficeBillEntryDetailObj.MaterialID);
                                        _OfficeBillEntryDetailObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _OfficeBillEntryDetailObj.UOM);
                                        _OfficeBillEntryDetailObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _OfficeBillEntryDetailObj.Description);
                                    }

                                    OfficeBillEntryDetailList.Add(_OfficeBillEntryDetailObj);
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
            return OfficeBillEntryDetailList;
        }
        #endregion GetOfficeBillDetail

        #region GetOfficeBillHeaderByID
        public OfficeBillEntry GetOfficeBillHeaderByID(Guid ID, UA ua)
        {
            OfficeBillEntry OfficeBillEntryHeaderList = null;
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
                        cmd.CommandText = "[GetOfficeBillHeaderByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                // TCRBillEntryHeaderList = TCRBillEntry();
                                while (sdr.Read())
                                {
                                    OfficeBillEntry _OfficeBillEntryHeaderObj = new OfficeBillEntry();
                                    {
                                        _OfficeBillEntryHeaderObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _OfficeBillEntryHeaderObj.ID);                                       
                                        _OfficeBillEntryHeaderObj.BillDate = (sdr["BillDate"].ToString() != "" ? DateTime.Parse(sdr["BillDate"].ToString()) : _OfficeBillEntryHeaderObj.BillDate);
                                        _OfficeBillEntryHeaderObj.BillNo = (sdr["BillNo"].ToString() != "" ? (sdr["BillNo"].ToString()) : _OfficeBillEntryHeaderObj.BillNo);                                       
                                        _OfficeBillEntryHeaderObj.CustomerName = (sdr["CustomerName"].ToString() != "" ? (sdr["CustomerName"].ToString()) : _OfficeBillEntryHeaderObj.CustomerName);
                                        _OfficeBillEntryHeaderObj.CustomerContactNo = (sdr["CustomerContactNo"].ToString() != "" ? (sdr["CustomerContactNo"].ToString()) : _OfficeBillEntryHeaderObj.CustomerContactNo);
                                        _OfficeBillEntryHeaderObj.CustomerLocation = (sdr["CustomerLocation"].ToString() != "" ? (sdr["CustomerLocation"].ToString()) : _OfficeBillEntryHeaderObj.CustomerLocation);
                                        _OfficeBillEntryHeaderObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? (sdr["PaymentMode"].ToString()) : _OfficeBillEntryHeaderObj.PaymentMode);
                                        _OfficeBillEntryHeaderObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _OfficeBillEntryHeaderObj.Remarks);
                                        _OfficeBillEntryHeaderObj.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : _OfficeBillEntryHeaderObj.Discount);                                       
                                        _OfficeBillEntryHeaderObj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : _OfficeBillEntryHeaderObj.VATAmount);

                                    }

                                    OfficeBillEntryHeaderList = _OfficeBillEntryHeaderObj;
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
            return OfficeBillEntryHeaderList;
        }
        #endregion GetOfficeBillHeaderByID

        #region DeleteOfficeBillEntry
        public bool DeleteOfficeBillEntry(Guid ID, UA UA)
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
                        cmd.CommandText = "[DeleteOfficeBillEntry]";
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
        #endregion DeleteOfficeBillEntry

        #region DeleteOfficeBillDetail
        public bool DeleteOfficeBillDetail(Guid ID, Guid HeaderID, UA UA)
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
                        cmd.CommandText = "[DeleteOfficeBillDetail]";
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
        #endregion DeleteOfficeBillDetail
    }
}