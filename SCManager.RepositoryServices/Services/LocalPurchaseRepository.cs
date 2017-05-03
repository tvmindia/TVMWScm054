using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.RepositoryServices.Contracts;
using SCManager.DataAccessObject.DTO;
using System.Data.SqlClient;
using System.Data;


namespace SCManager.RepositoryServices.Services
{
    public class LocalPurchaseRepository:ILocalPurchaseRepository
    {
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public LocalPurchaseRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory



        #region Methods
        public List<LocalPurchase> GetAllLocalPurchase(UA UA)
        {
            List<LocalPurchase> LocalPurchaselist = null;
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
                        cmd.CommandText = "[GetAllLocalPurchase]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                LocalPurchaselist = new List<LocalPurchase>();
                                while (sdr.Read())
                                {
                                    LocalPurchase _LPObj = new LocalPurchase();
                                    {
                                        _LPObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _LPObj.ID);
                                        _LPObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _LPObj.SCCode);
                                        _LPObj.VendorName = (sdr["VendorName"].ToString() != "" ? (sdr["VendorName"].ToString()) : _LPObj.VendorName);
                                        _LPObj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).Date : _LPObj.InvoiceDate);
                                        _LPObj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? (sdr["InvoiceNo"].ToString()) : _LPObj.InvoiceNo);
                                      
                                        _LPObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _LPObj.Remarks);
                                        _LPObj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : 0);
                                        _LPObj.Subtotal = (sdr["TotalValue"].ToString() != "" ? decimal.Parse(sdr["TotalValue"].ToString()) : 0);
                                      
                                    }

                                    LocalPurchaselist.Add(_LPObj);
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
            return LocalPurchaselist;
        }

        public LocalPurchase InsertLocalPurchase(LocalPurchase LP, UA UA)
        {
            LocalPurchase Result = null;
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
                        cmd.CommandText = "[InsertLocalPurchase]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar, 20).Value = LP.InvoiceNo;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.SmallDateTime).Value = LP.InvoiceDate;
                        cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar, 50).Value = LP.VendorName;                       
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = LP.Remarks;
                        cmd.Parameters.Add("@VATAmount", SqlDbType.Decimal).Value = LP.VATAmount;                       
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = LP.DetailXML;
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
                        LP.ID = new Guid(outputID.Value.ToString());
                        LP.LocalPurchaseDetail = GetLocalPurchaseDetail(LP.ID, UA);

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return LP;
        }

        public LocalPurchase UpdateLocalPurchase(LocalPurchase LP, UA UA)
        {
            //LocalPurchase Result = null;
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
                        cmd.CommandText = "[UpdateLocalPurchase]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = LP.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar, 20).Value = LP.InvoiceNo;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.SmallDateTime).Value = LP.InvoiceDate;
                        cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar, 20).Value = LP.VendorName;                       
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = LP.Remarks;
                        cmd.Parameters.Add("@VATAmount", SqlDbType.Decimal).Value = LP.VATAmount;                    
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = LP.DetailXML;
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
                        LP.LocalPurchaseDetail = GetLocalPurchaseDetail(LP.ID, UA);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return LP;
        }

        public LocalPurchase GetLocalPurchaseHeader(Guid ID, UA UA)
        {
            LocalPurchase LP = null;
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
                        cmd.CommandText = "[GetLocalPurchaseHeaderByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {

                                if (sdr.Read())
                                {
                                    LocalPurchase LPObj = new LocalPurchase();
                                    {
                                        LPObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : LPObj.ID);
                                        LPObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : LPObj.SCCode);
                                       
                                        LPObj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).Date : LPObj.InvoiceDate);
                                        LPObj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? (sdr["InvoiceNo"].ToString()) : LPObj.InvoiceNo);
                                        LPObj.VendorName = (sdr["VendorName"].ToString() != "" ? (sdr["VendorName"].ToString()) : LPObj.VendorName);

                                        LPObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : LPObj.Remarks);
                                        LPObj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : LPObj.VATAmount);
                                        LPObj.Subtotal = (sdr["TotalValue"].ToString() != "" ? decimal.Parse(sdr["TotalValue"].ToString()) : LPObj.Subtotal);
                                       
                                    }

                                    LP = LPObj;
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
            return LP;
        }

        public List<LocalPurchaseDetail> GetLocalPurchaseDetail(Guid ID, UA UA)
        {
            List<LocalPurchaseDetail> LocalPurchaseDetailList = null;
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
                        cmd.CommandText = "[GetLocalPurchaseDetailByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                LocalPurchaseDetailList = new List<LocalPurchaseDetail>();
                                while (sdr.Read())
                                {
                                    LocalPurchaseDetail _LocalPurchaseDetailObj = new LocalPurchaseDetail();
                                    {
                                        _LocalPurchaseDetailObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _LocalPurchaseDetailObj.ID);
                                        _LocalPurchaseDetailObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _LocalPurchaseDetailObj.SCCode);
                                        _LocalPurchaseDetailObj.MaterialID = (sdr["ItemID"].ToString() != "" ? Guid.Parse(sdr["ItemID"].ToString()) : _LocalPurchaseDetailObj.MaterialID);
                                        _LocalPurchaseDetailObj.Quantity = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);
                                        _LocalPurchaseDetailObj.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : 0);
                                        _LocalPurchaseDetailObj.TradeDiscount = (sdr["TradeDiscount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscount"].ToString()) : 0);
                                        _LocalPurchaseDetailObj.Material = (sdr["Material"].ToString() != "" ? (sdr["Material"].ToString()) : _LocalPurchaseDetailObj.Material);
                                        _LocalPurchaseDetailObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _LocalPurchaseDetailObj.UOM);
                                    }

                                    LocalPurchaseDetailList.Add(_LocalPurchaseDetailObj);
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
            return LocalPurchaseDetailList;
        }

        public bool DeleteLocalPurchase(Guid ID, UA UA)
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
                        cmd.CommandText = "[DeleteLocalPurchase]";
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

        public bool DeleteLocalPurchaseDetail(Guid ID, Guid HeaderID, UA UA)
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
                        cmd.CommandText = "[DeleteLocalPurchaseDetail]";
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

        #endregion Methods
    }
}