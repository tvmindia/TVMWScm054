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
    public class ICRBillEntryRepository : IICRBillEntryRepository
    {
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ICRBillEntryRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region GetAllICRBill
        public List<ICRBillEntry> GetAllICRBillEntry(UA UA)
        {
            List<ICRBillEntry> ICRBillEntrylist = null;
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
                        cmd.CommandText = "[GetAllICRBillHeader]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ICRBillEntrylist = new List<ICRBillEntry>();
                                while (sdr.Read())
                                {
                                    ICRBillEntry _ICRBillEntrylistObj = new ICRBillEntry();
                                    {
                                        _ICRBillEntrylistObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ICRBillEntrylistObj.ID);
                                        _ICRBillEntrylistObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _ICRBillEntrylistObj.SCCode);
                                        _ICRBillEntrylistObj.ICRNo = (sdr["ICRNo"].ToString() != "" ? (sdr["ICRNo"].ToString()) : _ICRBillEntrylistObj.ICRNo);
                                        _ICRBillEntrylistObj.ICRDate = (sdr["ICRDate"].ToString() != "" ? DateTime.Parse(sdr["ICRDate"].ToString()).ToString("dd-MMM-yyyy") : _ICRBillEntrylistObj.ICRDate);
                                        _ICRBillEntrylistObj.ModelNo = (sdr["ModelNo"].ToString() != "" ? (sdr["ModelNo"].ToString()) : _ICRBillEntrylistObj.ModelNo);
                                        _ICRBillEntrylistObj.SerialNo = (sdr["SerialNo"].ToString() != "" ? (sdr["SerialNo"].ToString()) : _ICRBillEntrylistObj.SerialNo);
                                        _ICRBillEntrylistObj.STAmount = (sdr["STAmount"].ToString() != "" ?decimal.Parse(sdr["STAmount"].ToString()) : _ICRBillEntrylistObj.STAmount);
                                        _ICRBillEntrylistObj.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : _ICRBillEntrylistObj.Discount);
                                        _ICRBillEntrylistObj.TotalServiceTaxAmt = (sdr["TotalServiceTaxAmount"].ToString() != "" ? decimal.Parse(sdr["TotalServiceTaxAmount"].ToString()) : _ICRBillEntrylistObj.TotalServiceTaxAmt);
                                        _ICRBillEntrylistObj.JobNo = (sdr["JobNo"].ToString() != "" ? (sdr["JobNo"].ToString()) : _ICRBillEntrylistObj.JobNo);
                                        _ICRBillEntrylistObj.CustomerName = (sdr["CustomerName"].ToString() != "" ? (sdr["CustomerName"].ToString()) : _ICRBillEntrylistObj.CustomerName);
                                        _ICRBillEntrylistObj.CustomerContactNo = (sdr["CustomerContactNo"].ToString() != "" ? (sdr["CustomerContactNo"].ToString()) : _ICRBillEntrylistObj.CustomerContactNo);
                                        _ICRBillEntrylistObj.CustomerLocation = (sdr["CustomerLocation"].ToString() != "" ? (sdr["CustomerLocation"].ToString()) : _ICRBillEntrylistObj.CustomerLocation);
                                        _ICRBillEntrylistObj.Technician = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : _ICRBillEntrylistObj.Technician);
                                        _ICRBillEntrylistObj.AMCValidFromDate = (sdr["AMCValidFromDate"].ToString() != "" ? DateTime.Parse(sdr["AMCValidFromDate"].ToString()).ToString("dd-MMM-yyyy") : _ICRBillEntrylistObj.AMCValidFromDate);
                                        _ICRBillEntrylistObj.AMCValidToDate = (sdr["AMCValidToDate"].ToString() != "" ? DateTime.Parse(sdr["AMCValidToDate"].ToString()).ToString("dd-MMM-yyyy") : _ICRBillEntrylistObj.AMCValidToDate);
                                        _ICRBillEntrylistObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _ICRBillEntrylistObj.Remarks);
                                        _ICRBillEntrylistObj.AMCNO = (sdr["AMCNO"].ToString() != "" ? (sdr["AMCNO"].ToString()) : _ICRBillEntrylistObj.AMCNO);
                                    }

                                    ICRBillEntrylist.Add(_ICRBillEntrylistObj);
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
            return ICRBillEntrylist;
        }
        #endregion  GetAllICRBill

        #region InsertICRBillEntry
        public ICRBillEntry InsertICRBillEntry(ICRBillEntry iCRBillEntry, UA UA)
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
                        cmd.CommandText = "[InsertICRBillEntry]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@ICRNo", SqlDbType.NVarChar, 50).Value = iCRBillEntry.ICRNo;
                        cmd.Parameters.Add("@ICRDate", SqlDbType.SmallDateTime).Value = iCRBillEntry.ICRDate;
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = iCRBillEntry.EmpID;
                        cmd.Parameters.Add("@JobNo", SqlDbType.NVarChar, 50).Value = iCRBillEntry.JobNo;
                        cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar, 250).Value = iCRBillEntry.CustomerName;
                        cmd.Parameters.Add("@CustomerContactNo", SqlDbType.NVarChar, 50).Value = iCRBillEntry.CustomerContactNo;
                        cmd.Parameters.Add("@CustomerLocation", SqlDbType.NVarChar, 50).Value = iCRBillEntry.CustomerLocation;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar, 20).Value = iCRBillEntry.PaymentMode;
                        cmd.Parameters.Add("@PaymentRefNo", SqlDbType.NVarChar, 50).Value = iCRBillEntry.PaymentRefNo;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = iCRBillEntry.Remarks;
                        cmd.Parameters.Add("@STAmount", SqlDbType.Decimal).Value = iCRBillEntry.STAmount;
                        cmd.Parameters.Add("@AMCValidFromDate", SqlDbType.SmallDateTime).Value = iCRBillEntry.AMCValidFromDate;
                        cmd.Parameters.Add("@AMCValidToDate", SqlDbType.SmallDateTime).Value = iCRBillEntry.AMCValidToDate;
                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar,20).Value = iCRBillEntry.ModelNo;
                        cmd.Parameters.Add("@TotalServiceTaxAmount", SqlDbType.Decimal).Value =iCRBillEntry.TotalServiceTaxAmt;
                        cmd.Parameters.Add("@SerialNo", SqlDbType.NVarChar,20).Value = iCRBillEntry.SerialNo;
                        cmd.Parameters.Add("@AMCNO", SqlDbType.NVarChar, 50).Value = iCRBillEntry.AMCNO;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = iCRBillEntry.Discount;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = iCRBillEntry.DetailXML;

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
                        iCRBillEntry.ID = new Guid(outputID.Value.ToString());
                        iCRBillEntry.ICRBillEntryDetail = GetICRBillDetail(iCRBillEntry.ID, UA);

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return iCRBillEntry;
        }
        #endregion InsertICRBillEntry

        #region UpdateICRBillEntry
        public ICRBillEntry UpdateICRBillEntry(ICRBillEntry iCRBillEntry, UA UA)
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
                        cmd.CommandText = "[UpdateICRBillEntry]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = iCRBillEntry.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@ICRNo", SqlDbType.NVarChar, 50).Value = iCRBillEntry.ICRNo;
                        cmd.Parameters.Add("@ICRDate", SqlDbType.SmallDateTime).Value = iCRBillEntry.ICRDate;
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = iCRBillEntry.EmpID;
                        cmd.Parameters.Add("@JobNo", SqlDbType.NVarChar, 50).Value = iCRBillEntry.JobNo;
                        cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar, 250).Value = iCRBillEntry.CustomerName;
                        cmd.Parameters.Add("@CustomerContactNo", SqlDbType.NVarChar, 50).Value = iCRBillEntry.CustomerContactNo;
                        cmd.Parameters.Add("@CustomerLocation", SqlDbType.NVarChar, 50).Value = iCRBillEntry.CustomerLocation;
                        cmd.Parameters.Add("@AMCNO", SqlDbType.NVarChar, 50).Value = iCRBillEntry.AMCNO;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar, 20).Value = iCRBillEntry.PaymentMode;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = iCRBillEntry.Remarks;
                        cmd.Parameters.Add("@STAmount", SqlDbType.Decimal).Value = iCRBillEntry.STAmount;
                        cmd.Parameters.Add("@AMCValidFromDate", SqlDbType.SmallDateTime).Value = iCRBillEntry.AMCValidFromDate;
                        cmd.Parameters.Add("@AMCValidToDate", SqlDbType.SmallDateTime).Value = iCRBillEntry.AMCValidToDate;
                        cmd.Parameters.Add("@ModelNo", SqlDbType.NVarChar,20).Value = iCRBillEntry.ModelNo;
                        cmd.Parameters.Add("@SerialNo", SqlDbType.NVarChar,20).Value = iCRBillEntry.SerialNo;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = iCRBillEntry.Discount;
                        cmd.Parameters.Add("@PaymentRefNo", SqlDbType.NVarChar, 50).Value = iCRBillEntry.PaymentRefNo;
                        cmd.Parameters.Add("@TotalServiceTaxAmount", SqlDbType.Decimal).Value = iCRBillEntry.TotalServiceTaxAmt;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = iCRBillEntry.DetailXML;

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
                        iCRBillEntry.ICRBillEntryDetail = GetICRBillDetail(iCRBillEntry.ID, UA);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return iCRBillEntry;
        }
        #endregion UpdateICRBillEntry

        #region GetICRBillDetail
        public List<ICRBillEntryDetail> GetICRBillDetail(Guid? ID, UA UA)
        {
            List<ICRBillEntryDetail> ICRBillEntryDetailList = null;
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
                        cmd.CommandText = "[GetICRBillDetailByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ICRBillEntryDetailList = new List<ICRBillEntryDetail>();
                                while (sdr.Read())
                                {
                                    ICRBillEntryDetail _ICRBillEntryDetailObj = new ICRBillEntryDetail();
                                    {
                                        _ICRBillEntryDetailObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ICRBillEntryDetailObj.ID);
                                        _ICRBillEntryDetailObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _ICRBillEntryDetailObj.SCCode);                                       
                                        _ICRBillEntryDetailObj.Quantity = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);
                                        _ICRBillEntryDetailObj.ItemNo = (sdr["ItemNo"].ToString() != "" ? int.Parse(sdr["ItemNo"].ToString()) : 0);
                                        _ICRBillEntryDetailObj.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : 0);
                                        _ICRBillEntryDetailObj.Material = (sdr["Material"].ToString() != "" ? (sdr["Material"].ToString()) : _ICRBillEntryDetailObj.Material);
                                        _ICRBillEntryDetailObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _ICRBillEntryDetailObj.Description);
                                        _ICRBillEntryDetailObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _ICRBillEntryDetailObj.UOM);
                                    }

                                    ICRBillEntryDetailList.Add(_ICRBillEntryDetailObj);
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
            return ICRBillEntryDetailList;
        }
        #endregion GetICRBillDetail

        #region GetICRBillHeaderByID
        public ICRBillEntry GetICRBillHeaderByID(Guid ID, UA ua)
        {
            ICRBillEntry ICRBillEntryHeaderList = null;
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
                        cmd.CommandText = "[GetICRBillHeaderByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                // TCRBillEntryHeaderList = TCRBillEntry();
                                while (sdr.Read())
                                {
                                    ICRBillEntry _ICRBillEntryHeaderObj = new ICRBillEntry();
                                    {
                                        _ICRBillEntryHeaderObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ICRBillEntryHeaderObj.ID);
                                        _ICRBillEntryHeaderObj.EmpID = (sdr["EmpID"].ToString() != "" ? Guid.Parse(sdr["EmpID"].ToString()) : _ICRBillEntryHeaderObj.EmpID);
                                        _ICRBillEntryHeaderObj.ICRDate = (sdr["ICRDate"].ToString() != "" ? DateTime.Parse(sdr["ICRDate"].ToString()).ToString("dd-MMM-yyyy") : _ICRBillEntryHeaderObj.ICRDate);
                                        _ICRBillEntryHeaderObj.ICRNo = (sdr["ICRNo"].ToString() != "" ? (sdr["ICRNo"].ToString()) : _ICRBillEntryHeaderObj.ICRNo);
                                        _ICRBillEntryHeaderObj.JobNo = (sdr["JobNo"].ToString() != "" ? (sdr["JobNo"].ToString()) : _ICRBillEntryHeaderObj.JobNo);
                                        _ICRBillEntryHeaderObj.CustomerName = (sdr["CustomerName"].ToString() != "" ? (sdr["CustomerName"].ToString()) : _ICRBillEntryHeaderObj.CustomerName);
                                        _ICRBillEntryHeaderObj.CustomerContactNo = (sdr["CustomerContactNo"].ToString() != "" ? (sdr["CustomerContactNo"].ToString()) : _ICRBillEntryHeaderObj.CustomerContactNo);
                                        _ICRBillEntryHeaderObj.CustomerLocation = (sdr["CustomerLocation"].ToString() != "" ? (sdr["CustomerLocation"].ToString()) : _ICRBillEntryHeaderObj.CustomerLocation);
                                        _ICRBillEntryHeaderObj.PaymentRefNo = (sdr["PaymentRefNo"].ToString() != "" ? (sdr["PaymentRefNo"].ToString()) : _ICRBillEntryHeaderObj.PaymentRefNo);
                                        _ICRBillEntryHeaderObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? (sdr["PaymentMode"].ToString()) : _ICRBillEntryHeaderObj.PaymentMode);
                                        _ICRBillEntryHeaderObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _ICRBillEntryHeaderObj.Remarks);
                                        _ICRBillEntryHeaderObj.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : _ICRBillEntryHeaderObj.Discount);
                                        _ICRBillEntryHeaderObj.STAmount = (sdr["TotalValue"].ToString() != "" ? decimal.Parse(sdr["TotalValue"].ToString()) : _ICRBillEntryHeaderObj.STAmount);
                                        _ICRBillEntryHeaderObj.AMCValidFromDate = (sdr["AMCValidFromDate"].ToString() != "" ? DateTime.Parse(sdr["AMCValidFromDate"].ToString()).ToString("dd-MMM-yyyy") : _ICRBillEntryHeaderObj.AMCValidFromDate);
                                        _ICRBillEntryHeaderObj.AMCValidToDate = (sdr["AMCValidToDate"].ToString() != "" ? DateTime.Parse(sdr["AMCValidToDate"].ToString()).ToString("dd-MMM-yyyy") : _ICRBillEntryHeaderObj.AMCValidToDate);
                                        _ICRBillEntryHeaderObj.TotalServiceTaxAmt = (sdr["TotalServiceTaxAmount"].ToString() != "" ? decimal.Parse(sdr["TotalServiceTaxAmount"].ToString()) : _ICRBillEntryHeaderObj.TotalServiceTaxAmt);
                                        _ICRBillEntryHeaderObj.ModelNo = (sdr["ModelNo"].ToString() != "" ? (sdr["ModelNo"].ToString()) : _ICRBillEntryHeaderObj.ModelNo);
                                        _ICRBillEntryHeaderObj.SerialNo = (sdr["SerialNo"].ToString() != "" ? (sdr["SerialNo"].ToString()) : _ICRBillEntryHeaderObj.SerialNo);
                                        _ICRBillEntryHeaderObj.AMCNO = (sdr["AMCNO"].ToString() != "" ? (sdr["AMCNO"].ToString()) : _ICRBillEntryHeaderObj.AMCNO);
                                    }

                                    ICRBillEntryHeaderList = _ICRBillEntryHeaderObj;
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
            return ICRBillEntryHeaderList;
        }
        #endregion GetICRBillHeaderByID

        #region DeleteICRBillDetail
        public bool DeleteICRBillDetail(Guid ID, Guid HeaderID, UA UA)
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
                        cmd.CommandText = "[DeleteICRBillDetail]";
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
        #endregion DeleteICRBillDetail

        #region DeleteICRBillEntry
        public bool DeleteICRBillEntry(Guid ID, UA UA)
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
                        cmd.CommandText = "[DeleteICRBillEntry]";
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
        #endregion DeleteICRBillEntry
    }
}