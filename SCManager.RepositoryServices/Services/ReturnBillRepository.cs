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
    public class ReturnBillRepository:IReturnBillRepository
    {
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ReturnBillRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region Methods

        public List<ReturnBill> GetAllReturnBill(UA UA)
        {
            List<ReturnBill> ReturnBilllist = null;
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
                        cmd.CommandText = "[GetAllReturnBill]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ReturnBilllist = new List<ReturnBill>();
                                while (sdr.Read())
                                {
                                    ReturnBill _ReturnBillObj = new ReturnBill();
                                    {
                                        _ReturnBillObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ReturnBillObj.ID);
                                        _ReturnBillObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _ReturnBillObj.SCCode);
                                        _ReturnBillObj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? (sdr["InvoiceNo"].ToString()) : _ReturnBillObj.InvoiceNo);
                                        _ReturnBillObj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy") : _ReturnBillObj.InvoiceDate);                                       
                                        _ReturnBillObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _ReturnBillObj.Remarks);
                                        _ReturnBillObj.ShippingCustomerName = (sdr["ShippingCustomerName"].ToString() != "" ? (sdr["ShippingCustomerName"].ToString()) : _ReturnBillObj.ShippingCustomerName);
                                        _ReturnBillObj.TotalValue = (sdr["TotalValue"].ToString() != "" ? decimal.Parse(sdr["TotalValue"].ToString()) : _ReturnBillObj.TotalValue);
                                        _ReturnBillObj.TotalTaxAmount = (sdr["TotalTaxAmount"].ToString() != "" ? decimal.Parse(sdr["TotalTaxAmount"].ToString()) : _ReturnBillObj.TotalTaxAmount);
                                        _ReturnBillObj.GrandTotal = (sdr["GrandTotal"].ToString() != "" ? decimal.Parse(sdr["GrandTotal"].ToString()) : _ReturnBillObj.GrandTotal);
                                        _ReturnBillObj.TicketNo = (sdr["TicketNo"].ToString());
                                    }
                                    ReturnBilllist.Add(_ReturnBillObj);
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
            return ReturnBilllist;
        }


        public List<ReturnBill> GetAllFranchiseeDetail(UA UA)
        {
            List<ReturnBill> ReturnBilllist = null;
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
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.CommandText = "[GetFranchiseeDetails]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ReturnBilllist = new List<ReturnBill>();
                                while (sdr.Read())
                                {
                                    ReturnBill _ReturnBillObj = new ReturnBill();
                                    {
                                       
                                       // _ReturnBillObj.ServiceCenterCode = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : _ReturnBillObj.ServiceCenterCode);
                                        _ReturnBillObj.ServiceCenterDescription = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _ReturnBillObj.ServiceCenterDescription);                                     
                                        _ReturnBillObj.ServiceCenterAddress = (sdr["Address"].ToString() != "" ? (sdr["Address"].ToString()) : _ReturnBillObj.ServiceCenterAddress);
                                        _ReturnBillObj.ServiceCenterContactNo = (sdr["ContactNo"].ToString() != "" ? (sdr["ContactNo"].ToString()) : _ReturnBillObj.ServiceCenterContactNo);
                                        
                                        _ReturnBillObj.ServiceCenterGstIn = (sdr["GstIn"].ToString());
                                        _ReturnBillObj.ServiceCenterPanNo = (sdr["PanNo"].ToString());
                                        _ReturnBillObj.ServiceCenterPlace = (sdr["PlaceOfSupply"].ToString());
                                        _ReturnBillObj.ServiceCenterEmail = (sdr["Email"].ToString());
                                    }
                                    ReturnBilllist.Add(_ReturnBillObj);
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
            return ReturnBilllist;
        }



        public List<ReturnBill> GetSupplierDetail(UA UA)
        {
            List<ReturnBill> ReturnBilllist = null;
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
                        cmd.CommandText = "[GetSupplierDetails]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ReturnBilllist = new List<ReturnBill>();
                                while (sdr.Read())
                                {
                                    ReturnBill returnBillObj = new ReturnBill();
                                    {                                                                              
                                        returnBillObj.CompanyDescription = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : returnBillObj.CompanyDescription);
                                        returnBillObj.CompanyAddress = (sdr["Address"].ToString() != "" ? (sdr["Address"].ToString()) : returnBillObj.CompanyAddress);
                                        returnBillObj.CompanyContactNo= (sdr["ContactNo"].ToString() != "" ? (sdr["ContactNo"].ToString()) : returnBillObj.CompanyContactNo);

                                        returnBillObj.CompanyGstIn = (sdr["GstIn"].ToString());
                                        returnBillObj.CompanyPanNo = (sdr["PanNo"].ToString());
                                        returnBillObj.CompanyPlace = (sdr["PlaceOfSupply"].ToString());
                                        returnBillObj.CompanyEmail = (sdr["Email"].ToString());
                                    }
                                    ReturnBilllist.Add(returnBillObj);
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
            return ReturnBilllist;
        }

        public List<ReturnBillDetail> GetMaterialsFromDefectiveDamaged(string TicketNo, string SCCode)
        {
            List<ReturnBillDetail> ReturnBillTicketNoList = null;
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
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = SCCode;
                        cmd.Parameters.Add("@TicketNo", SqlDbType.NVarChar, 20).Value = TicketNo;
                        cmd.CommandText = "[GetMaterialsFromDefDamByTicket]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ReturnBillTicketNoList = new List<ReturnBillDetail>();
                                while (sdr.Read())
                                {
                                    ReturnBillDetail _ReturnBillObj = new ReturnBillDetail();
                                    {

                                        _ReturnBillObj.TicketNo = (sdr["TicketNo"].ToString() != "" ? (sdr["TicketNo"].ToString()) : _ReturnBillObj.TicketNo);
                                        _ReturnBillObj.CustomerName = (sdr["Customer"].ToString() != "" ? (sdr["Customer"].ToString()) : _ReturnBillObj.CustomerName);                                       
                                        //_ReturnBillObj.TotalValue = (sdr["TotalValue"].ToString() != "" ? decimal.Parse(sdr["TotalValue"].ToString()) : _ReturnBillObj.TotalValue);
                                        //_ReturnBillObj.TotalTaxAmount = (sdr["TotalTaxAmount"].ToString() != "" ? Decimal.Parse(sdr["TotalTaxAmount"].ToString()) : _ReturnBillObj.TotalTaxAmount);
                                        //_ReturnBillObj.GrandTotal = (sdr["GrandTotal"].ToString() != "" ? Decimal.Parse(sdr["GrandTotal"].ToString()) : _ReturnBillObj.GrandTotal);
                                        _ReturnBillObj.MaterialID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ReturnBillObj.MaterialID);
                                       _ReturnBillObj.Quantity = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);
                                        _ReturnBillObj.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : 0);
                                        _ReturnBillObj.TradeDiscount = (sdr["TradeDiscount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscount"].ToString()) : 0);
                                        _ReturnBillObj.CGSTPercentage = (sdr["CGSTPercentage"].ToString() != "" ? decimal.Parse(sdr["CGSTPercentage"].ToString()) : 0);
                                        _ReturnBillObj.SGSTPercentage = (sdr["SGSTPercentage"].ToString() != "" ? decimal.Parse(sdr["SGSTPercentage"].ToString()) : 0);
                                        _ReturnBillObj.Material = (sdr["ItemCode"].ToString() != "" ? (sdr["ItemCode"].ToString()) : _ReturnBillObj.Material);
                                        _ReturnBillObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _ReturnBillObj.UOM);
                                        _ReturnBillObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _ReturnBillObj.Description);
                                    }
                                    ReturnBillTicketNoList.Add(_ReturnBillObj);

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
            return ReturnBillTicketNoList;
        }

        public ReturnBill InsertReturnBill(ReturnBill rtb, UA UA)
        {
            ReturnBill Result = null;
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
                        cmd.CommandText = "[InsertReturnBill]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@TicketNo", SqlDbType.NVarChar, 20).Value = rtb.TicketNo;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar, 20).Value = rtb.InvoiceNo;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.SmallDateTime).Value = rtb.InvoiceDate;

                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = rtb.Remarks;
                        cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = rtb.CustomerName;
                        cmd.Parameters.Add("@CustomerAddress", SqlDbType.NVarChar).Value = rtb.CustomerAddress;                        
                        cmd.Parameters.Add("@CustomerPhoneNo", SqlDbType.NVarChar, 50).Value = rtb.CustomerPhoneNo;
                        cmd.Parameters.Add("@CustomerEmail", SqlDbType.NVarChar).Value = rtb.CustomerEmail;
                        cmd.Parameters.Add("@CustomerGstIn", SqlDbType.NVarChar).Value = rtb.CustomerGstIn;
                        cmd.Parameters.Add("@CustomerPanNo", SqlDbType.NVarChar).Value = rtb.CustomerPanNo;
                        cmd.Parameters.Add("@PlaceOfSupply", SqlDbType.NVarChar).Value = rtb.PlaceofSupply;
                        cmd.Parameters.Add("@ShippingCustomerName", SqlDbType.NVarChar).Value = rtb.ShippingCustomerName;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar).Value = rtb.ShippingAddress;
                        cmd.Parameters.Add("@ShippingPhoneNo", SqlDbType.NVarChar, 50).Value = rtb.ShippingCustomerPhoneNo;
                        cmd.Parameters.Add("@ShippingEmail", SqlDbType.NVarChar).Value = rtb.ShippingCustomerEmail;
                        cmd.Parameters.Add("@ShippingGstIn", SqlDbType.NVarChar).Value = rtb.ShippingGstIn;
                        cmd.Parameters.Add("@ShippingPanNo", SqlDbType.NVarChar).Value = rtb.ShippingPanNo;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = rtb.DetailXML;

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
                        rtb.ID = new Guid(outputID.Value.ToString());
                        rtb.ReturnBillDetail = GetReturnBillDetail(rtb.ID, UA);

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return rtb;
        }

        public ReturnBill UpdateReturnBill(ReturnBill rtb, UA UA)
        {
            ReturnBill Result = null;
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
                        cmd.CommandText = "[UpdateReturnBill]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = rtb.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        //cmd.Parameters.Add("@TicketNo", SqlDbType.NVarChar, 20).Value = rtb.TicketNo;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar, 20).Value = rtb.InvoiceNo;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.SmallDateTime).Value = rtb.InvoiceDate;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = rtb.Remarks;
                        cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = rtb.CustomerName;
                        cmd.Parameters.Add("@CustomerAddress", SqlDbType.NVarChar).Value = rtb.CustomerAddress;
                        cmd.Parameters.Add("@CustomerPhoneNo", SqlDbType.NVarChar, 50).Value = rtb.CustomerPhoneNo;
                        cmd.Parameters.Add("@CustomerEmail", SqlDbType.NVarChar).Value = rtb.CustomerEmail;
                        cmd.Parameters.Add("@CustomerGstIn", SqlDbType.NVarChar).Value = rtb.CustomerGstIn;
                        cmd.Parameters.Add("@CustomerPanNo", SqlDbType.NVarChar).Value = rtb.CustomerPanNo;
                        cmd.Parameters.Add("@PlaceOfSupply", SqlDbType.NVarChar).Value = rtb.PlaceofSupply;
                        cmd.Parameters.Add("@ShippingCustomerName", SqlDbType.NVarChar).Value = rtb.ShippingCustomerName;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar).Value = rtb.ShippingAddress;
                        cmd.Parameters.Add("@ShippingPhoneNo", SqlDbType.NVarChar, 50).Value = rtb.ShippingCustomerPhoneNo;
                        cmd.Parameters.Add("@ShippingEmail", SqlDbType.NVarChar).Value = rtb.ShippingCustomerEmail;
                        cmd.Parameters.Add("@ShippingGstIn", SqlDbType.NVarChar).Value = rtb.ShippingGstIn;
                        cmd.Parameters.Add("@ShippingPanNo", SqlDbType.NVarChar).Value = rtb.ShippingPanNo;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = rtb.DetailXML;

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
                        rtb.ReturnBillDetail = GetReturnBillDetail(rtb.ID, UA);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return rtb;
        }

        public ReturnBill GetReturnBillHeader(Guid ID, UA UA)
        {
            ReturnBill ReturnBill = null;
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
                        cmd.CommandText = "[GetReturnBillHeaderByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {

                                if (sdr.Read())
                                {
                                    ReturnBill _ReturnBillObj = new ReturnBill();
                                    {
                                        _ReturnBillObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ReturnBillObj.ID);
                                        _ReturnBillObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _ReturnBillObj.SCCode);
                                        _ReturnBillObj.TicketNo = (sdr["TicketNo"].ToString() != "" ? (sdr["TicketNo"].ToString()) : _ReturnBillObj.TicketNo);
                                        _ReturnBillObj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy") : _ReturnBillObj.InvoiceDate);
                                        _ReturnBillObj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? (sdr["InvoiceNo"].ToString()) : _ReturnBillObj.InvoiceNo);
                                        _ReturnBillObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _ReturnBillObj.Remarks);
                                        _ReturnBillObj.CustomerName = (sdr["CustomerName"].ToString() != "" ? (sdr["CustomerName"].ToString()) : _ReturnBillObj.CustomerName);
                                        _ReturnBillObj.CustomerPhoneNo = (sdr["CustomerPhoneNo"].ToString() != "" ? (sdr["CustomerPhoneNo"].ToString()) : _ReturnBillObj.CustomerPhoneNo);
                                        _ReturnBillObj.CustomerAddress = (sdr["CustomerAddress"].ToString() != "" ? (sdr["CustomerAddress"].ToString()) : _ReturnBillObj.CustomerAddress);
                                        _ReturnBillObj.CustomerEmail = (sdr["CustomerEmail"].ToString() != "" ? (sdr["CustomerEmail"].ToString()) : _ReturnBillObj.CustomerEmail);
                                        _ReturnBillObj.CustomerGstIn = (sdr["CustomerGstIn"].ToString() != "" ? (sdr["CustomerGstIn"].ToString()) : _ReturnBillObj.CustomerGstIn);
                                        _ReturnBillObj.CustomerPanNo = (sdr["CustomerPanNo"].ToString() != "" ? (sdr["CustomerPanNo"].ToString()) : _ReturnBillObj.CustomerPanNo);
                                        _ReturnBillObj.PlaceofSupply = (sdr["PlaceOfSupply"].ToString() != "" ? (sdr["PlaceOfSupply"].ToString()) : _ReturnBillObj.PlaceofSupply);
                                        _ReturnBillObj.TotalValue = (sdr["TotalValue"].ToString() != "" ? decimal.Parse(sdr["TotalValue"].ToString()) : _ReturnBillObj.TotalValue);
                                        _ReturnBillObj.ReturnStatusYN = (sdr["ReturnStatusYN"].ToString() != "" ? bool.Parse(sdr["ReturnStatusYN"].ToString()) : _ReturnBillObj.ReturnStatusYN);
                                        _ReturnBillObj.ShippingCustomerName = (sdr["ShippingCustomerName"].ToString() != "" ? (sdr["ShippingCustomerName"].ToString()) : _ReturnBillObj.ShippingCustomerName);
                                        _ReturnBillObj.ShippingCustomerPhoneNo = (sdr["ShippingPhoneNo"].ToString() != "" ? (sdr["ShippingPhoneNo"].ToString()) : _ReturnBillObj.ShippingCustomerPhoneNo);
                                        _ReturnBillObj.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? (sdr["ShippingAddress"].ToString()) : _ReturnBillObj.ShippingAddress);
                                        _ReturnBillObj.ShippingCustomerEmail = (sdr["ShippingEmail"].ToString() != "" ? (sdr["ShippingEmail"].ToString()) : _ReturnBillObj.ShippingCustomerEmail);
                                        _ReturnBillObj.ShippingGstIn = (sdr["ShippingGstIn"].ToString() != "" ? (sdr["ShippingGstIn"].ToString()) : _ReturnBillObj.ShippingGstIn);
                                        _ReturnBillObj.ShippingPanNo = (sdr["ShippingPanNo"].ToString() != "" ? (sdr["ShippingPanNo"].ToString()) : _ReturnBillObj.ShippingPanNo);
                                        // _ReturnBillObj.GrandTotal = (sdr["GrandTotal"].ToString() != "" ? decimal.Parse(sdr["GrandTotal"].ToString()) : _ReturnBillObj.GrandTotal);
                                        // _ReturnBillObj.TotalTaxAmount = (sdr["TotalTaxAmount"].ToString() != "" ? decimal.Parse(sdr["TotalTaxAmount"].ToString()) : _ReturnBillObj.TotalTaxAmount);


                                    }
                                    ReturnBill = _ReturnBillObj;
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
            return ReturnBill;
        }
        //-----------------------------//
        public List<ReturnBillDetail> GetReturnBillDetail(Guid ID, UA UA)
        {
            List<ReturnBillDetail> ReturnBillDetailList = null;
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
                        cmd.CommandText = "[GetReturnBillDetailByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ReturnBillDetailList = new List<ReturnBillDetail>();
                                while (sdr.Read())
                                {
                                    ReturnBillDetail _ReturnBillDetailObj = new ReturnBillDetail();
                                    {
                                        _ReturnBillDetailObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ReturnBillDetailObj.ID);
                                        _ReturnBillDetailObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _ReturnBillDetailObj.SCCode);
                                        _ReturnBillDetailObj.MaterialID = (sdr["ItemID"].ToString() != "" ? Guid.Parse(sdr["ItemID"].ToString()) : _ReturnBillDetailObj.MaterialID);
                                        _ReturnBillDetailObj.Quantity = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);
                                        _ReturnBillDetailObj.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : 0);
                                        _ReturnBillDetailObj.TradeDiscount = (sdr["TradeDiscount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscount"].ToString()) : 0);
                                        _ReturnBillDetailObj.CGSTPercentage = (sdr["CGSTPercentage"].ToString() != "" ? decimal.Parse(sdr["CGSTPercentage"].ToString()) : 0);
                                        _ReturnBillDetailObj.SGSTPercentage = (sdr["SGSTPercentage"].ToString() != "" ? decimal.Parse(sdr["SGSTPercentage"].ToString()) : 0);
                                        _ReturnBillDetailObj.Material = (sdr["Material"].ToString() != "" ? (sdr["Material"].ToString()) : _ReturnBillDetailObj.Material);
                                        _ReturnBillDetailObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _ReturnBillDetailObj.UOM);
                                        _ReturnBillDetailObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _ReturnBillDetailObj.Description);
                                    }

                                    ReturnBillDetailList.Add(_ReturnBillDetailObj);
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
            return ReturnBillDetailList;
        }

        public bool DeleteReturnBill(Guid ID, UA UA)
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
                        cmd.CommandText = "[DeleteReturnBill]";
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

        public bool DeleteReturnBillDetail(Guid ID, Guid HeaderID, UA UA)
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
                        cmd.CommandText = "[DeleteReturnBillDetail]";
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
        #region GetAllTicketNo
        public List<ReturnBill> GetAllTicketNo(UA UA)
        {
            List<ReturnBill> TicketNolist = null;
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
                        cmd.CommandText = "[GetAllTicketNo]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                TicketNolist = new List<ReturnBill>();
                                while (sdr.Read())
                                {

                                    ReturnBill returnBillObj = new ReturnBill();

                                    {
                                        //returnBillObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : returnBillObj.ID);
                                        returnBillObj.TicketNo = (sdr["TicketNo"].ToString() != "" ? sdr["TicketNo"].ToString() : returnBillObj.TicketNo);
                                    };

                                    TicketNolist.Add(returnBillObj);
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
            return TicketNolist;
        }
        #endregion GetAllTicketNo


        #region ReturnDefectiveDamaged
        public string ReturnDefectiveDamaged(string ID, UA ua,string TicketNo)
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
                        cmd.CommandText = "[ReturnDefectiveDamagedBillByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = ua.SCCode;                        
                        cmd.Parameters.Add("@ReturnStatusYN", SqlDbType.Bit).Value = true;
                        cmd.Parameters.Add("@TicketNo", SqlDbType.NVarChar, 20).Value =TicketNo;
                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                    return outParameter.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }
        #endregion ReturnDefectiveDamaged
        #endregion Methods

    }
}