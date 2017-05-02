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
    public class Form8BRetailInvoiceRepository : IForm8BRetailInvoiceRepository
    {
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public Form8BRetailInvoiceRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region Methods
        public List<Form8B> GetAllForm8B(UA UA)
        {
            List<Form8B> Form8Blist = null;
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
                        cmd.CommandText = "[GetAllForm8B]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Form8Blist = new List<Form8B>();
                                while (sdr.Read())
                                {
                                    Form8B _Form8Obj = new Form8B();
                                    {
                                        _Form8Obj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _Form8Obj.ID);
                                        _Form8Obj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _Form8Obj.SCCode);
                                        _Form8Obj.ChallanDate = (sdr["ChallanDate"].ToString() != "" ? DateTime.Parse(sdr["ChallanDate"].ToString()).Date : _Form8Obj.ChallanDate);
                                        _Form8Obj.ChallanNo = (sdr["ChallanNo"].ToString() != "" ? (sdr["ChallanNo"].ToString()) : _Form8Obj.ChallanNo);
                                        _Form8Obj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).Date : _Form8Obj.InvoiceDate);
                                        _Form8Obj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? (sdr["InvoiceNo"].ToString()) : _Form8Obj.InvoiceNo);
                                        _Form8Obj.PODate = (sdr["PODate"].ToString() != "" ? DateTime.Parse(sdr["PODate"].ToString()).Date : _Form8Obj.PODate);
                                        _Form8Obj.PONo = (sdr["PONo"].ToString() != "" ? (sdr["PONo"].ToString()) : _Form8Obj.PONo);
                                        _Form8Obj.SPUNo = (sdr["SPUNo"].ToString() != "" ? (sdr["SPUNo"].ToString()) : _Form8Obj.SPUNo);
                                        _Form8Obj.CustomerBillAddrs = (sdr["CustomerBillAddrs"].ToString() != "" ? (sdr["CustomerBillAddrs"].ToString()) : _Form8Obj.CustomerBillAddrs);
                                        _Form8Obj.CustomerDelvAddrs = (sdr["CustomerDelvAddrs"].ToString() != "" ? (sdr["CustomerDelvAddrs"].ToString()) : _Form8Obj.CustomerDelvAddrs);
                                        _Form8Obj.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? (sdr["SaleOrderNo"].ToString()) : _Form8Obj.SaleOrderNo);
                                        _Form8Obj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _Form8Obj.Remarks);
                                        _Form8Obj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : _Form8Obj.VATAmount);
                                        _Form8Obj.Subtotal = (sdr["TotalValue"].ToString() != "" ? decimal.Parse(sdr["TotalValue"].ToString()) : _Form8Obj.Subtotal);
                                        _Form8Obj.VATExpense = (sdr["VATExpense"].ToString() != "" ? decimal.Parse(sdr["VATExpense"].ToString()) : _Form8Obj.VATExpense);
                                    }

                                    Form8Blist.Add(_Form8Obj);
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
            return Form8Blist;
        }

        public Form8B InsertForm8B(Form8B frm8B, UA UA)
        {
            Form8B Result = null;
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
                        cmd.CommandText = "[InsertForm8B]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar, 20).Value = frm8B.InvoiceNo;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.SmallDateTime).Value = frm8B.InvoiceDate;
                        cmd.Parameters.Add("@SaleOrderNo", SqlDbType.NVarChar, 20).Value = frm8B.SaleOrderNo;
                        cmd.Parameters.Add("@ChallanNo", SqlDbType.NVarChar, 20).Value = frm8B.ChallanNo;
                        cmd.Parameters.Add("@ChallanDate", SqlDbType.SmallDateTime).Value = frm8B.ChallanDate;
                        cmd.Parameters.Add("@PONo", SqlDbType.NVarChar, 20).Value = frm8B.PONo;
                        cmd.Parameters.Add("@PODate", SqlDbType.SmallDateTime).Value = frm8B.PODate;
                        cmd.Parameters.Add("@SPUNo", SqlDbType.NVarChar).Value = frm8B.SPUNo;
                        cmd.Parameters.Add("@CustomerBillAddrs", SqlDbType.NVarChar).Value = frm8B.CustomerBillAddrs;
                        cmd.Parameters.Add("@CustomerDelvAddrs", SqlDbType.NVarChar).Value = frm8B.CustomerDelvAddrs;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = frm8B.Remarks;
                        cmd.Parameters.Add("@VATAmount", SqlDbType.Decimal).Value = frm8B.VATAmount;
                        cmd.Parameters.Add("@VATExpense", SqlDbType.Decimal).Value = frm8B.VATExpense;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = frm8B.DetailXML;

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
                        frm8B.ID = new Guid(outputID.Value.ToString());
                        frm8B.Form8BDetail = GetForm8BDetail(frm8B.ID, UA);

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return frm8B;
        }

        public Form8B UpdateForm8B(Form8B frm8B, UA UA)
        {
            //Form8B Result = null;
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
                        cmd.CommandText = "[UpdateForm8B]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = frm8B.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar, 20).Value = frm8B.InvoiceNo;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.SmallDateTime).Value = frm8B.InvoiceDate;
                        cmd.Parameters.Add("@SaleOrderNo", SqlDbType.NVarChar, 20).Value = frm8B.SaleOrderNo;
                        cmd.Parameters.Add("@ChallanNo", SqlDbType.NVarChar, 20).Value = frm8B.ChallanNo;
                        cmd.Parameters.Add("@ChallanDate", SqlDbType.SmallDateTime).Value = frm8B.ChallanDate;
                        cmd.Parameters.Add("@PONo", SqlDbType.NVarChar, 20).Value = frm8B.PONo;
                        cmd.Parameters.Add("@PODate", SqlDbType.SmallDateTime).Value = frm8B.PODate;
                        cmd.Parameters.Add("@SPUNo", SqlDbType.NVarChar).Value = frm8B.SPUNo;
                        cmd.Parameters.Add("@CustomerBillAddrs", SqlDbType.NVarChar).Value = frm8B.CustomerBillAddrs;
                        cmd.Parameters.Add("@CustomerDelvAddrs", SqlDbType.NVarChar).Value = frm8B.CustomerDelvAddrs;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = frm8B.Remarks;
                        cmd.Parameters.Add("@VATAmount", SqlDbType.Decimal).Value = frm8B.VATAmount;
                        cmd.Parameters.Add("@VATExpense", SqlDbType.Decimal).Value = frm8B.VATExpense;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = frm8B.DetailXML;

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
                        frm8B.Form8BDetail = GetForm8BDetail(frm8B.ID, UA);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return frm8B;
        }

        public Form8B GetForm8BHeader(Guid ID, UA UA)
        {
            Form8B Form8 = null;
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
                        cmd.CommandText = "[GetForm8BHeaderByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {

                                if (sdr.Read())
                                {
                                    Form8B _Form8Obj = new Form8B();
                                    {
                                        _Form8Obj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _Form8Obj.ID);
                                        _Form8Obj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _Form8Obj.SCCode);
                                        _Form8Obj.ChallanDate = (sdr["ChallanDate"].ToString() != "" ? DateTime.Parse(sdr["ChallanDate"].ToString()).Date : _Form8Obj.ChallanDate);
                                        _Form8Obj.ChallanNo = (sdr["ChallanNo"].ToString() != "" ? (sdr["ChallanNo"].ToString()) : _Form8Obj.ChallanNo);
                                        _Form8Obj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).Date : _Form8Obj.InvoiceDate);
                                        _Form8Obj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? (sdr["InvoiceNo"].ToString()) : _Form8Obj.InvoiceNo);
                                        _Form8Obj.PODate = (sdr["PODate"].ToString() != "" ? DateTime.Parse(sdr["PODate"].ToString()).Date : _Form8Obj.PODate);
                                        _Form8Obj.PONo = (sdr["PONo"].ToString() != "" ? (sdr["PONo"].ToString()) : _Form8Obj.PONo);
                                        _Form8Obj.SPUNo = (sdr["SPUNo"].ToString() != "" ? (sdr["SPUNo"].ToString()) : _Form8Obj.SPUNo);
                                        _Form8Obj.CustomerBillAddrs = (sdr["CustomerBillAddrs"].ToString() != "" ? (sdr["CustomerBillAddrs"].ToString()) : _Form8Obj.CustomerBillAddrs);
                                        _Form8Obj.CustomerDelvAddrs = (sdr["CustomerDelvAddrs"].ToString() != "" ? (sdr["CustomerDelvAddrs"].ToString()) : _Form8Obj.CustomerDelvAddrs);
                                        _Form8Obj.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? (sdr["SaleOrderNo"].ToString()) : _Form8Obj.SaleOrderNo);
                                        _Form8Obj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _Form8Obj.Remarks);
                                        _Form8Obj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : _Form8Obj.VATAmount);
                                        _Form8Obj.Subtotal = (sdr["TotalValue"].ToString() != "" ? decimal.Parse(sdr["TotalValue"].ToString()) : _Form8Obj.Subtotal);
                                        _Form8Obj.VATExpense = (sdr["VATExpense"].ToString() != "" ? decimal.Parse(sdr["VATExpense"].ToString()) : _Form8Obj.VATExpense);
                                    }

                                    Form8 = _Form8Obj;
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
            return Form8;
        }

        public List<Form8BDetail> GetForm8BDetail(Guid ID, UA UA)
        {
            List<Form8BDetail> Form8BDetailList = null;
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
                        cmd.CommandText = "[GetForm8BDetailByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Form8BDetailList = new List<Form8BDetail>();
                                while (sdr.Read())
                                {
                                    Form8BDetail _Form8BDetailObj = new Form8BDetail();
                                    {
                                        _Form8BDetailObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _Form8BDetailObj.ID);
                                        _Form8BDetailObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _Form8BDetailObj.SCCode);
                                        _Form8BDetailObj.MaterialID = (sdr["ItemID"].ToString() != "" ? Guid.Parse(sdr["ItemID"].ToString()) : _Form8BDetailObj.MaterialID);
                                        _Form8BDetailObj.Quantity = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);
                                        _Form8BDetailObj.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : 0);
                                        _Form8BDetailObj.TradeDiscount = (sdr["TradeDiscount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscount"].ToString()) : 0);
                                        _Form8BDetailObj.Material = (sdr["Material"].ToString() != "" ? (sdr["Material"].ToString()) : _Form8BDetailObj.Material);
                                        _Form8BDetailObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _Form8BDetailObj.UOM);
                                    }

                                    Form8BDetailList.Add(_Form8BDetailObj);
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
            return Form8BDetailList;
        }

        public bool DeleteForm8B(Guid ID, UA UA)
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
                        cmd.CommandText = "[DeleteForm8B]";
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

        public bool DeleteForm8BDetail(Guid ID, Guid HeaderID, UA UA)
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
                        cmd.CommandText = "[DeleteForm8BDetail]";
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