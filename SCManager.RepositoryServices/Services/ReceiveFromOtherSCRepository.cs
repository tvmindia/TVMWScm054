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
    public class ReceiveFromOtherSCRepository : IReceiveFromOtherSCRepository
    {
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ReceiveFromOtherSCRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region GetAllOtherSCReceipt
        public List<ReceiveFromOtherSC> GetAllOtherSCReceipt(UA UA)
        {
            List<ReceiveFromOtherSC> ReceiveFromOtherSClist = null;
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
                        cmd.CommandText = "[GetAllOtherSCReceipt]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ReceiveFromOtherSClist = new List<ReceiveFromOtherSC>();
                                while (sdr.Read())
                                {
                                    ReceiveFromOtherSC _ReceiveFromOtherSClistObj = new ReceiveFromOtherSC();
                                    {
                                        _ReceiveFromOtherSClistObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ReceiveFromOtherSClistObj.ID);
                                        _ReceiveFromOtherSClistObj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy") : _ReceiveFromOtherSClistObj.InvoiceDate);
                                        _ReceiveFromOtherSClistObj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? (sdr["InvoiceNo"].ToString()) : _ReceiveFromOtherSClistObj.InvoiceNo);
                                       
                                        _ReceiveFromOtherSClistObj.FromSCName = (sdr["FromSCName"].ToString() != "" ? (sdr["FromSCName"].ToString()) : _ReceiveFromOtherSClistObj.FromSCName);
                                       
                                        _ReceiveFromOtherSClistObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _ReceiveFromOtherSClistObj.Remarks);
                                        _ReceiveFromOtherSClistObj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : _ReceiveFromOtherSClistObj.VATAmount);
                                       
                                        _ReceiveFromOtherSClistObj.Subtotal = (sdr["SubTotal"].ToString() != "" ? decimal.Parse(sdr["SubTotal"].ToString()) : _ReceiveFromOtherSClistObj.Subtotal);
                                       
                                    }

                                    ReceiveFromOtherSClist.Add(_ReceiveFromOtherSClistObj);
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
            return ReceiveFromOtherSClist;
        }
        #endregion  GetAllOtherSCReceipt

        #region InsertOtherSCReceipt
        public ReceiveFromOtherSC InsertOtherSCReceipt(ReceiveFromOtherSC receiveFromOtherSC, UA UA)
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
                        cmd.CommandText = "[InsertOtherSCReceipt]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar, 20).Value = receiveFromOtherSC.InvoiceNo;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.SmallDateTime).Value = receiveFromOtherSC.InvoiceDate;                       
                        cmd.Parameters.Add("@FromSCName", SqlDbType.NVarChar, 250).Value = receiveFromOtherSC.FromSCName;                                         
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = receiveFromOtherSC.Remarks;
                        cmd.Parameters.Add("@VATAmount", SqlDbType.Decimal).Value = receiveFromOtherSC.VATAmount;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = receiveFromOtherSC.DetailXML;

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
                        receiveFromOtherSC.ID = new Guid(outputID.Value.ToString());
                        receiveFromOtherSC.ReceiveFromOtherScDetail = GetOtherScReceiptDetail(receiveFromOtherSC.ID, UA);

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return receiveFromOtherSC;
        }
        #endregion InsertOtherSCReceipt

        #region UpdateOtherSCReceipt
        public ReceiveFromOtherSC UpdateOtherSCReceipt(ReceiveFromOtherSC receiveFromOtherSC, UA UA)
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
                        cmd.CommandText = "[UpdateOtherSCReceipt]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = receiveFromOtherSC.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar, 20).Value = receiveFromOtherSC.InvoiceNo;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.SmallDateTime).Value = receiveFromOtherSC.InvoiceDate;
                        cmd.Parameters.Add("@FromSCName", SqlDbType.NVarChar, 250).Value = receiveFromOtherSC.FromSCName;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = receiveFromOtherSC.Remarks;
                        cmd.Parameters.Add("@VATAmount", SqlDbType.Decimal).Value = receiveFromOtherSC.VATAmount;                   
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = receiveFromOtherSC.DetailXML;

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
                        receiveFromOtherSC.ReceiveFromOtherScDetail = GetOtherScReceiptDetail(receiveFromOtherSC.ID, UA);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return receiveFromOtherSC;
        }
        #endregion UpdateOtherSCReceipt

        #region GetOtherScReceiptDetail
        public List<ReceiveFromOtherScDetail> GetOtherScReceiptDetail(Guid ID, UA UA)
        {
            List<ReceiveFromOtherScDetail> receiveFromOtherScDetailList = null;
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
                        cmd.CommandText = "[GetOtherScReceiptDetailByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                receiveFromOtherScDetailList = new List<ReceiveFromOtherScDetail>();
                                while (sdr.Read())
                                {
                                    ReceiveFromOtherScDetail _ReceiveFromOtherScDetailObj = new ReceiveFromOtherScDetail();
                                    {
                                        _ReceiveFromOtherScDetailObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ReceiveFromOtherScDetailObj.ID);
                                        _ReceiveFromOtherScDetailObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _ReceiveFromOtherScDetailObj.SCCode);
                                        _ReceiveFromOtherScDetailObj.MaterialID = (sdr["ItemID"].ToString() != "" ? Guid.Parse(sdr["ItemID"].ToString()) : _ReceiveFromOtherScDetailObj.MaterialID);
                                        _ReceiveFromOtherScDetailObj.Quantity = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);
                                        _ReceiveFromOtherScDetailObj.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : 0);
                                        _ReceiveFromOtherScDetailObj.TradeDiscount = (sdr["TradeDiscount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscount"].ToString()) : 0);
                                        _ReceiveFromOtherScDetailObj.Material = (sdr["Material"].ToString() != "" ? (sdr["Material"].ToString()) : _ReceiveFromOtherScDetailObj.Material);
                                        _ReceiveFromOtherScDetailObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _ReceiveFromOtherScDetailObj.Description);
                                        _ReceiveFromOtherScDetailObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _ReceiveFromOtherScDetailObj.UOM);
                                    }

                                    receiveFromOtherScDetailList.Add(_ReceiveFromOtherScDetailObj);
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
            return receiveFromOtherScDetailList;
        }
        #endregion GetOtherScReceiptDetail

        #region GetOtherSCReceiptByID
        public ReceiveFromOtherSC GetOtherSCReceiptByID(Guid ID, UA ua)
        {
            ReceiveFromOtherSC ReceiveFromOtherSCList = null;
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
                        cmd.CommandText = "[GetOtherSCReceiptByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                               
                                while (sdr.Read())
                                {
                                    ReceiveFromOtherSC _ReceiveFromOtherSCObj = new ReceiveFromOtherSC();
                                    {
                                        _ReceiveFromOtherSCObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ReceiveFromOtherSCObj.ID);                                      
                                        _ReceiveFromOtherSCObj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy") : _ReceiveFromOtherSCObj.InvoiceDate);
                                        _ReceiveFromOtherSCObj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? (sdr["InvoiceNo"].ToString()) : _ReceiveFromOtherSCObj.InvoiceNo);                                       
                                        _ReceiveFromOtherSCObj.FromSCName = (sdr["FromSCName"].ToString() != "" ? (sdr["FromSCName"].ToString()) : _ReceiveFromOtherSCObj.FromSCName);
                                        _ReceiveFromOtherSCObj.Subtotal = (sdr["TotalValue"].ToString() != "" ? decimal.Parse(sdr["TotalValue"].ToString()) : _ReceiveFromOtherSCObj.Subtotal);
                                        _ReceiveFromOtherSCObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _ReceiveFromOtherSCObj.Remarks);                                       
                                        _ReceiveFromOtherSCObj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : _ReceiveFromOtherSCObj.VATAmount);

                                    }

                                    ReceiveFromOtherSCList = _ReceiveFromOtherSCObj;
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
            return ReceiveFromOtherSCList;
        }
        #endregion GetOtherSCReceiptByID

        #region DeleteOtherScReceiptDetail
        public bool DeleteOtherScReceiptDetail(Guid ID, Guid HeaderID, UA UA)
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
                        cmd.CommandText = "[DeleteOtherScReceiptDetail]";
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
        #endregion DeleteOtherScReceiptDetail

        #region DeleteOtherSCReceipt
        public bool DeleteOtherSCReceipt(Guid ID, UA UA)
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
                        cmd.CommandText = "[DeleteOtherSCReceipt]";
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
        #endregion DeleteOtherSCReceipt
    }
}