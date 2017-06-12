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
    public class Form8TaxInvoiceRepository : IForm8TaxInvoiceRepository
    {

        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public Form8TaxInvoiceRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region Methods

        public List<Form8> GetAllForm8(UA UA)
        {
            List<Form8> Form8list = null;
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
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar,5).Value = UA.SCCode;
                        cmd.CommandText = "[GetAllForm8]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Form8list = new List<Form8>();
                                while (sdr.Read())
                                {
                                    Form8 _Form8Obj = new Form8();
                                    {
                                        _Form8Obj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _Form8Obj.ID);
                                        _Form8Obj.SCCode = (sdr["SCCode"].ToString() != "" ?  (sdr["SCCode"].ToString()) : _Form8Obj.SCCode);
                                        _Form8Obj.ChallanDate = (sdr["ChallanDate"].ToString() != "" ? DateTime.Parse(sdr["ChallanDate"].ToString()).ToString("dd-MMM-yyyy") : _Form8Obj.ChallanDate);
                                        _Form8Obj.ChallanNo = (sdr["ChallanNo"].ToString() != "" ? (sdr["ChallanNo"].ToString()) : _Form8Obj.ChallanNo);
                                        _Form8Obj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy") : _Form8Obj.InvoiceDate);
                                        _Form8Obj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? (sdr["InvoiceNo"].ToString()) : _Form8Obj.InvoiceNo);
                                        _Form8Obj.PODate = (sdr["PODate"].ToString() != "" ? DateTime.Parse(sdr["PODate"].ToString()).ToString("dd-MMM-yyyy") : _Form8Obj.PODate);
                                        _Form8Obj.PONo = (sdr["PONo"].ToString() != "" ? (sdr["PONo"].ToString()) : _Form8Obj.PONo);
                                        _Form8Obj.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? (sdr["SaleOrderNo"].ToString()) : _Form8Obj.SaleOrderNo);
                                        _Form8Obj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _Form8Obj.Remarks);
                                        _Form8Obj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : _Form8Obj.VATAmount);
                                        _Form8Obj.Subtotal = (sdr["TotalValue"].ToString() != "" ? decimal.Parse(sdr["TotalValue"].ToString()) : _Form8Obj.Subtotal);
                                        _Form8Obj.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : _Form8Obj.Discount);
                                    }

                                    Form8list.Add(_Form8Obj);
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
            return Form8list;
        }

        public Form8 InsertForm8(Form8 frm8, UA UA) {
            //Form8 Result = null;
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
                        cmd.CommandText = "[InsertForm8]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar, 20).Value = frm8.InvoiceNo;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.SmallDateTime).Value = frm8.InvoiceDate;
                        cmd.Parameters.Add("@SaleOrderNo", SqlDbType.NVarChar, 20).Value = frm8.SaleOrderNo;
                        cmd.Parameters.Add("@ChallanNo", SqlDbType.NVarChar, 20).Value = frm8.ChallanNo;
                        cmd.Parameters.Add("@ChallanDate", SqlDbType.SmallDateTime).Value = frm8.ChallanDate;
                        cmd.Parameters.Add("@PONo", SqlDbType.NVarChar, 20).Value = frm8.PONo;
                        cmd.Parameters.Add("@PODate", SqlDbType.SmallDateTime).Value = frm8.PODate;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = frm8.Remarks;
                        cmd.Parameters.Add("@VATAmount", SqlDbType.Decimal).Value = frm8.VATAmount;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = frm8.Discount;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = frm8.DetailXML;

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
                        frm8.ID = new Guid(outputID.Value.ToString());
                        frm8.Form8Detail = GetForm8Detail(frm8.ID, UA);

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return frm8;
        }

        public Form8 UpdateForm8(Form8 frm8, UA UA)
        {
            Form8 Result = null;
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
                        cmd.CommandText = "[UpdateForm8]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = frm8.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar, 20).Value = frm8.InvoiceNo;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.SmallDateTime).Value = frm8.InvoiceDate;
                        cmd.Parameters.Add("@SaleOrderNo", SqlDbType.NVarChar, 20).Value = frm8.SaleOrderNo;
                        cmd.Parameters.Add("@ChallanNo", SqlDbType.NVarChar, 20).Value = frm8.ChallanNo;
                        cmd.Parameters.Add("@ChallanDate", SqlDbType.SmallDateTime).Value = frm8.ChallanDate;
                        cmd.Parameters.Add("@PONo", SqlDbType.NVarChar, 20).Value = frm8.PONo;
                        cmd.Parameters.Add("@PODate", SqlDbType.SmallDateTime).Value = frm8.PODate;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = frm8.Remarks;
                        cmd.Parameters.Add("@VATAmount", SqlDbType.Decimal).Value = frm8.VATAmount;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = frm8.Discount;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = frm8.DetailXML;

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
                        frm8.Form8Detail = GetForm8Detail(frm8.ID, UA);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return frm8;
        }

        public Form8 GetForm8Header(Guid ID, UA UA)
        {
            Form8 Form8 = null;
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
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value =ID;
                        cmd.CommandText = "[GetForm8HeaderByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                               
                                if (sdr.Read())
                                {
                                    Form8 _Form8Obj = new Form8();
                                    {
                                        _Form8Obj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _Form8Obj.ID);
                                        _Form8Obj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _Form8Obj.SCCode);
                                        _Form8Obj.ChallanDate = (sdr["ChallanDate"].ToString() != "" ? DateTime.Parse(sdr["ChallanDate"].ToString()).ToString("dd-MMM-yyyy") : _Form8Obj.ChallanDate);
                                        _Form8Obj.ChallanNo = (sdr["ChallanNo"].ToString() != "" ? (sdr["ChallanNo"].ToString()) : _Form8Obj.ChallanNo);
                                        _Form8Obj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy") : _Form8Obj.InvoiceDate);
                                        _Form8Obj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? (sdr["InvoiceNo"].ToString()) : _Form8Obj.InvoiceNo);
                                        _Form8Obj.PODate = (sdr["PODate"].ToString() != "" ? DateTime.Parse(sdr["PODate"].ToString()).ToString("dd-MMM-yyyy") : _Form8Obj.PODate);
                                        _Form8Obj.PONo = (sdr["PONo"].ToString() != "" ? (sdr["PONo"].ToString()) : _Form8Obj.PONo);
                                        _Form8Obj.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? (sdr["SaleOrderNo"].ToString()) : _Form8Obj.SaleOrderNo);
                                        _Form8Obj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _Form8Obj.Remarks);
                                        _Form8Obj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : _Form8Obj.VATAmount);
                                        _Form8Obj.Subtotal = (sdr["TotalValue"].ToString() != "" ? decimal.Parse(sdr["TotalValue"].ToString()) : _Form8Obj.Subtotal);
                                        _Form8Obj.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : _Form8Obj.Discount);
                                    }

                                    Form8=_Form8Obj;
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

        public List<Form8Detail> GetForm8Detail(Guid ID, UA UA)
        {
            List<Form8Detail> Form8DetailList = null;
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
                        cmd.CommandText = "[GetForm8DetailByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Form8DetailList = new List<Form8Detail>();
                                while (sdr.Read())
                                {
                                    Form8Detail _Form8DetailObj = new Form8Detail();
                                    {
                                        _Form8DetailObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _Form8DetailObj.ID);
                                        _Form8DetailObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _Form8DetailObj.SCCode);
                                        _Form8DetailObj.MaterialID = (sdr["ItemID"].ToString() != "" ? Guid.Parse(sdr["ItemID"].ToString()) : _Form8DetailObj.MaterialID);
                                        _Form8DetailObj.Quantity = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);
                                        _Form8DetailObj.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : 0);
                                        _Form8DetailObj.TradeDiscount = (sdr["TradeDiscount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscount"].ToString()) :0);
                                        _Form8DetailObj.Material = (sdr["Material"].ToString() != "" ? (sdr["Material"].ToString()) : _Form8DetailObj.Material);
                                        _Form8DetailObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _Form8DetailObj.UOM);
                                        _Form8DetailObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _Form8DetailObj.Description);
                                    }

                                    Form8DetailList.Add(_Form8DetailObj);
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
            return Form8DetailList;
        }

        public bool DeleteForm8(Guid ID, UA UA) {
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
                        cmd.CommandText = "[DeleteForm8]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value=ID;                       
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

        public bool DeleteForm8Detail(Guid ID,Guid HeaderID, UA UA)
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
                        cmd.CommandText = "[DeleteForm8Detail]";
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