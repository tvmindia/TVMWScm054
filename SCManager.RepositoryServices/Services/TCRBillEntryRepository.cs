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
    public class TCRBillEntryRepository : ITCRBillEntryRepository
    {
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public TCRBillEntryRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region GetAllTCRBill
        public List<TCRBillEntry> GetAllTCRBillEntry(UA UA)
        {
            List<TCRBillEntry> TCRBillEntrylist = null;
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
                        cmd.CommandText = "[GetAllTCRBill]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                TCRBillEntrylist = new List<TCRBillEntry>();
                                while (sdr.Read())
                                {
                                    TCRBillEntry TCRBillEntrylistObj = new TCRBillEntry();
                                    {
                                        TCRBillEntrylistObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : TCRBillEntrylistObj.ID);
                                        TCRBillEntrylistObj.BillDate = (sdr["BillDate"].ToString() != "" ? DateTime.Parse(sdr["BillDate"].ToString()).ToString("dd-MMM-yyyy") : TCRBillEntrylistObj.BillDate);
                                        TCRBillEntrylistObj.BillNo = (sdr["BillNo"].ToString() != "" ? (sdr["BillNo"].ToString()) : TCRBillEntrylistObj.BillNo);
                                        TCRBillEntrylistObj.JobNo = (sdr["JobNo"].ToString() != "" ? (sdr["JobNo"].ToString()) : TCRBillEntrylistObj.JobNo);
                                        TCRBillEntrylistObj.CustomerName = (sdr["CustomerName"].ToString() != "" ? (sdr["CustomerName"].ToString()) : TCRBillEntrylistObj.CustomerName);
                                        TCRBillEntrylistObj.CustomerContactNo = (sdr["CustomerContactNo"].ToString() != "" ? (sdr["CustomerContactNo"].ToString()) : TCRBillEntrylistObj.CustomerContactNo);
                                        TCRBillEntrylistObj.CustomerLocation = (sdr["CustomerLocation"].ToString() != "" ? (sdr["CustomerLocation"].ToString()) : TCRBillEntrylistObj.CustomerLocation);
                                        TCRBillEntrylistObj.Technician = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : TCRBillEntrylistObj.Technician);
                                        TCRBillEntrylistObj.PaymentRefNo = (sdr["PaymentRefNo"].ToString() != "" ? (sdr["PaymentRefNo"].ToString()) : TCRBillEntrylistObj.PaymentRefNo);
                                        TCRBillEntrylistObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : TCRBillEntrylistObj.Remarks);
                                        TCRBillEntrylistObj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : TCRBillEntrylistObj.VATAmount);
                                        TCRBillEntrylistObj.ServiceCharge = (sdr["ServiceCharge"].ToString() != "" ? decimal.Parse(sdr["ServiceCharge"].ToString()) : TCRBillEntrylistObj.ServiceCharge);
                                        TCRBillEntrylistObj.Subtotal = (sdr["SubTotal"].ToString() != "" ? decimal.Parse(sdr["SubTotal"].ToString()) : TCRBillEntrylistObj.Subtotal);
                                        TCRBillEntrylistObj.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : TCRBillEntrylistObj.Discount);
                                        TCRBillEntrylistObj.CGSTAmount = (sdr["CGSTAmount"].ToString() != "" ? decimal.Parse(sdr["CGSTAmount"].ToString()) : TCRBillEntrylistObj.CGSTAmount);
                                        TCRBillEntrylistObj.SGSTAmount = (sdr["SGSTAmount"].ToString() != "" ? decimal.Parse(sdr["SGSTAmount"].ToString()) : TCRBillEntrylistObj.SGSTAmount);
                                    }

                                    TCRBillEntrylist.Add(TCRBillEntrylistObj);
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
            return TCRBillEntrylist;
        }
        #endregion  GetAllTCRBill

        #region InsertTCRBillEntry
        public TCRBillEntry InsertTCRBillEntry(TCRBillEntry tCRBillEntry, UA UA)
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
                        cmd.CommandText = "[InsertTCRBillEntry]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@BillNo", SqlDbType.NVarChar, 50).Value = tCRBillEntry.BillNo;
                        cmd.Parameters.Add("@BillDate", SqlDbType.SmallDateTime).Value = tCRBillEntry.BillDate;
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = tCRBillEntry.EmpID;
                        cmd.Parameters.Add("@JobNo", SqlDbType.NVarChar, 50).Value = tCRBillEntry.JobNo;
                        cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar,250).Value = tCRBillEntry.CustomerName;
                        cmd.Parameters.Add("@CustomerContactNo", SqlDbType.NVarChar, 50).Value = tCRBillEntry.CustomerContactNo;
                        cmd.Parameters.Add("@CustomerLocation", SqlDbType.NVarChar,50).Value = tCRBillEntry.CustomerLocation;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar,20).Value = tCRBillEntry.PaymentMode;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar,-1).Value = tCRBillEntry.Remarks;
                        cmd.Parameters.Add("@VATAmount", SqlDbType.Decimal).Value = tCRBillEntry.VATAmount;
                        cmd.Parameters.Add("@CGSTAmount", SqlDbType.Decimal).Value = tCRBillEntry.CGSTAmount;
                        cmd.Parameters.Add("@SGSTAmount", SqlDbType.Decimal).Value = tCRBillEntry.SGSTAmount;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = tCRBillEntry.Discount;
                        cmd.Parameters.Add("@ServiceCharge", SqlDbType.Decimal).Value = tCRBillEntry.ServiceCharge;
                        cmd.Parameters.Add("@ServiceChargeComm", SqlDbType.Decimal).Value = tCRBillEntry.SCCommAmount;
                        //cmd.Parameters.Add("@SpecialComm", SqlDbType.Decimal).Value = tCRBillEntry.SpecialComm;
                        cmd.Parameters.Add("@PaymentRefNo", SqlDbType.NVarChar, 50).Value = tCRBillEntry.PaymentRefNo;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = tCRBillEntry.DetailXML;

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
                        tCRBillEntry.ID = new Guid(outputID.Value.ToString());
                        tCRBillEntry.TCRBillEntryDetail = GetTCRBillDetail(tCRBillEntry.ID, UA);

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return tCRBillEntry;
        }
        #endregion InsertTCRBillEntry

        #region UpdateTCRBillEntry
        public TCRBillEntry UpdateTCRBillEntry(TCRBillEntry tCRBillEntry, UA UA)
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
                        cmd.CommandText = "[UpdateTCRBillEntry]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = tCRBillEntry.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@BillNo", SqlDbType.NVarChar, 50).Value = tCRBillEntry.BillNo;
                        cmd.Parameters.Add("@BillDate", SqlDbType.SmallDateTime).Value = tCRBillEntry.BillDate;
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = tCRBillEntry.EmpID;
                        cmd.Parameters.Add("@JobNo", SqlDbType.NVarChar, 50).Value = tCRBillEntry.JobNo;
                        cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar, 250).Value = tCRBillEntry.CustomerName;
                        cmd.Parameters.Add("@CustomerContactNo", SqlDbType.NVarChar, 50).Value = tCRBillEntry.CustomerContactNo;
                        cmd.Parameters.Add("@CustomerLocation", SqlDbType.NVarChar, 50).Value = tCRBillEntry.CustomerLocation;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar, 20).Value = tCRBillEntry.PaymentMode;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = tCRBillEntry.Remarks;
                        cmd.Parameters.Add("@VATAmount", SqlDbType.Decimal).Value = tCRBillEntry.VATAmount;
                        cmd.Parameters.Add("@CGSTAmount", SqlDbType.Decimal).Value = tCRBillEntry.CGSTAmount;
                        cmd.Parameters.Add("@SGSTAmount", SqlDbType.Decimal).Value = tCRBillEntry.SGSTAmount;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = tCRBillEntry.Discount;
                        cmd.Parameters.Add("@ServiceCharge", SqlDbType.Decimal).Value = tCRBillEntry.ServiceCharge;
                        cmd.Parameters.Add("@ServiceChargeComm", SqlDbType.Decimal).Value = tCRBillEntry.SCCommAmount;
                        //cmd.Parameters.Add("@SpecialComm", SqlDbType.Decimal).Value = tCRBillEntry.SpecialComm;
                        cmd.Parameters.Add("@PaymentRefNo", SqlDbType.NVarChar, 50).Value = tCRBillEntry.PaymentRefNo;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = tCRBillEntry.DetailXML;

                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = UA.UserName;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = UA.CurrentDatetime();

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
                        tCRBillEntry.TCRBillEntryDetail = GetTCRBillDetail(tCRBillEntry.ID, UA);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return tCRBillEntry;
        }
        #endregion UpdateTCRBillEntry

        #region GetTCRBillDetail
        public List<TCRBillEntryDetail> GetTCRBillDetail(Guid ID, UA UA)
        {
            List<TCRBillEntryDetail> TCRBillEntryDetailList = null;
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
                        cmd.CommandText = "[GetTCRBillDetailByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                TCRBillEntryDetailList = new List<TCRBillEntryDetail>();
                                while (sdr.Read())
                                {
                                    TCRBillEntryDetail _TCRBillEntryDetailObj = new TCRBillEntryDetail();
                                    {
                                        _TCRBillEntryDetailObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _TCRBillEntryDetailObj.ID);
                                        _TCRBillEntryDetailObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _TCRBillEntryDetailObj.SCCode);
                                        _TCRBillEntryDetailObj.MaterialID = (sdr["ItemID"].ToString() != "" ? Guid.Parse(sdr["ItemID"].ToString()) : _TCRBillEntryDetailObj.MaterialID);
                                        _TCRBillEntryDetailObj.Quantity = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);
                                        _TCRBillEntryDetailObj.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : 0);                                        
                                        _TCRBillEntryDetailObj.Material = (sdr["Material"].ToString() != "" ? (sdr["Material"].ToString()) : _TCRBillEntryDetailObj.Material);
                                        _TCRBillEntryDetailObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _TCRBillEntryDetailObj.Description);
                                        _TCRBillEntryDetailObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _TCRBillEntryDetailObj.UOM);
                                        _TCRBillEntryDetailObj.TradeDiscount = (sdr["TradeDiscount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscount"].ToString()) : 0);
                                        _TCRBillEntryDetailObj.CgstPercentage = (sdr["CgstPercentage"].ToString() != "" ? decimal.Parse(sdr["CgstPercentage"].ToString()) : 0);
                                        _TCRBillEntryDetailObj.SgstPercentage = (sdr["SgstPercentage"].ToString() != "" ? decimal.Parse(sdr["SgstPercentage"].ToString()) : 0);
                                       

                                    }

                                    TCRBillEntryDetailList.Add(_TCRBillEntryDetailObj);
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
            return TCRBillEntryDetailList;
        }
        #endregion GetTCRBillDetail

        #region GetTCRBillHeaderByID
        public TCRBillEntry GetTCRBillHeaderByID(Guid ID, UA ua)
        {
            TCRBillEntry TCRBillEntryHeaderList = null;
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
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value =ID;
                        cmd.CommandText = "[GetTCRBillHeaderByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                               // TCRBillEntryHeaderList = TCRBillEntry();
                                while (sdr.Read())
                                {
                                    TCRBillEntry TCRBillEntryHeaderObj = new TCRBillEntry();
                                    {
                                        TCRBillEntryHeaderObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : TCRBillEntryHeaderObj.ID);
                                        TCRBillEntryHeaderObj.EmpID = (sdr["EmpID"].ToString() != "" ? Guid.Parse(sdr["EmpID"].ToString()) : TCRBillEntryHeaderObj.EmpID);
                                        TCRBillEntryHeaderObj.BillDate = (sdr["BillDate"].ToString() != "" ? DateTime.Parse(sdr["BillDate"].ToString()).ToString("dd-MMM-yyyy") : TCRBillEntryHeaderObj.BillDate);
                                        TCRBillEntryHeaderObj.BillNo = (sdr["BillNo"].ToString() != "" ? (sdr["BillNo"].ToString()) : TCRBillEntryHeaderObj.BillNo);
                                        TCRBillEntryHeaderObj.JobNo = (sdr["JobNo"].ToString() != "" ? (sdr["JobNo"].ToString()) : TCRBillEntryHeaderObj.JobNo);
                                        TCRBillEntryHeaderObj.CustomerName = (sdr["CustomerName"].ToString() != "" ? (sdr["CustomerName"].ToString()) : TCRBillEntryHeaderObj.CustomerName);
                                        TCRBillEntryHeaderObj.CustomerContactNo = (sdr["CustomerContactNo"].ToString() != "" ? (sdr["CustomerContactNo"].ToString()) : TCRBillEntryHeaderObj.CustomerContactNo);
                                        TCRBillEntryHeaderObj.CustomerLocation = (sdr["CustomerLocation"].ToString() != "" ? (sdr["CustomerLocation"].ToString()) : TCRBillEntryHeaderObj.CustomerLocation);
                                        TCRBillEntryHeaderObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? (sdr["PaymentMode"].ToString()) : TCRBillEntryHeaderObj.PaymentMode);
                                        TCRBillEntryHeaderObj.PaymentRefNo = (sdr["PaymentRefNo"].ToString() != "" ? (sdr["PaymentRefNo"].ToString()) : TCRBillEntryHeaderObj.PaymentRefNo);
                                        TCRBillEntryHeaderObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : TCRBillEntryHeaderObj.Remarks);
                                        TCRBillEntryHeaderObj.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : TCRBillEntryHeaderObj.Discount);
                                        TCRBillEntryHeaderObj.ServiceCharge = (sdr["ServiceCharge"].ToString() != "" ? decimal.Parse(sdr["ServiceCharge"].ToString()) : TCRBillEntryHeaderObj.ServiceCharge);
                                        TCRBillEntryHeaderObj.SCCommAmount = (sdr["ServiceChargeComm"].ToString() != "" ? decimal.Parse(sdr["ServiceChargeComm"].ToString()) : TCRBillEntryHeaderObj.SCCommAmount);
                                        TCRBillEntryHeaderObj.SpecialComm = (sdr["SpecialComm"].ToString() != "" ? decimal.Parse(sdr["SpecialComm"].ToString()) : TCRBillEntryHeaderObj.SpecialComm);
                                        TCRBillEntryHeaderObj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : TCRBillEntryHeaderObj.VATAmount);
                                        TCRBillEntryHeaderObj.CGSTAmount = (sdr["CGSTAmount"].ToString() != "" ? decimal.Parse(sdr["CGSTAmount"].ToString()) : TCRBillEntryHeaderObj.CGSTAmount);
                                        TCRBillEntryHeaderObj.SGSTAmount = (sdr["SGSTAmount"].ToString() != "" ? decimal.Parse(sdr["SGSTAmount"].ToString()) : TCRBillEntryHeaderObj.SGSTAmount);
                                        TCRBillEntryHeaderObj.CgstPercentage = (sdr["CgstPercentage"].ToString() != "" ? decimal.Parse(sdr["CgstPercentage"].ToString()) : TCRBillEntryHeaderObj.CgstPercentage);
                                        TCRBillEntryHeaderObj.SgstPercentage = (sdr["SgstPercentage"].ToString() != "" ? decimal.Parse(sdr["SgstPercentage"].ToString()) : TCRBillEntryHeaderObj.SgstPercentage);
                                    }

                                    TCRBillEntryHeaderList =TCRBillEntryHeaderObj;
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
            return TCRBillEntryHeaderList;
        }
        #endregion GetTCRBillHeaderByID

        #region GetAllJobNo
        public List<TCRBillEntry> GetAllJobNo(UA UA)
        {
            List<TCRBillEntry> JobNolist = null;
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
                        cmd.CommandText = "[GetJobNo]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                JobNolist = new List<TCRBillEntry>();
                                while (sdr.Read())
                                {
                                    TCRBillEntry tCRBillEntryObj = new TCRBillEntry();

                                    {
                                        tCRBillEntryObj.jobNoID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : tCRBillEntryObj.jobNoID);
                                        tCRBillEntryObj.JobNo = (sdr["JobNo"].ToString() != "" ? sdr["JobNo"].ToString() : tCRBillEntryObj.JobNo);

                                    };

                                    JobNolist.Add(tCRBillEntryObj);
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
            return JobNolist;
        }
        #endregion GetAllJobNo

        #region DeleteTCRBillDetail
        public bool DeleteTCRBillDetail(Guid ID, Guid HeaderID, UA UA)
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
                        cmd.CommandText = "[DeleteTCRBillDetail]";
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
        #endregion DeleteTCRBillDetail

        #region DeleteTCRBillEntry
        public bool DeleteTCRBillEntry(Guid ID, UA UA)
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
                        cmd.CommandText = "[DeleteTCRBillEntry]";
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
        #endregion DeleteTCRBillEntry
    }
}