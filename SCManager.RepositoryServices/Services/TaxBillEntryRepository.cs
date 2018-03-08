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
    public class TaxBillEntryRepository:ITaxBillEntryRepository
    {
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public TaxBillEntryRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory


        #region GetAllTaxBill
        public List<TaxBillEntry> GetAllTaxBillEntry(UA UA)
        {
            List<TaxBillEntry> TaxBillEntrylist = null;
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
                        cmd.CommandText = "[GetAllTaxBill]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                TaxBillEntrylist = new List<TaxBillEntry>();
                                while (sdr.Read())
                                {
                                    TaxBillEntry TaxBillEntrylistObj = new TaxBillEntry();
                                    {
                                        TaxBillEntrylistObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : TaxBillEntrylistObj.ID);
                                        TaxBillEntrylistObj.BillDate = (sdr["BillDate"].ToString() != "" ? DateTime.Parse(sdr["BillDate"].ToString()).ToString("dd-MMM-yyyy") : TaxBillEntrylistObj.BillDate);
                                        TaxBillEntrylistObj.BillDateFormatted= (sdr["BillDate"].ToString() != "" ? DateTime.Parse(sdr["BillDate"].ToString()).ToString("dd-MMM-yyyy") : TaxBillEntrylistObj.BillDateFormatted);
                                        TaxBillEntrylistObj.BillNo = (sdr["BillNo"].ToString() != "" ? (sdr["BillNo"].ToString()) : TaxBillEntrylistObj.BillNo);
                                        TaxBillEntrylistObj.JobNo = (sdr["JobNo"].ToString() != "" ? (sdr["JobNo"].ToString()) : TaxBillEntrylistObj.JobNo);
                                        TaxBillEntrylistObj.CustomerName = (sdr["CustomerName"].ToString() != "" ? (sdr["CustomerName"].ToString()) : TaxBillEntrylistObj.CustomerName);
                                        TaxBillEntrylistObj.CustomerContactNo = (sdr["CustomerContactNo"].ToString() != "" ? (sdr["CustomerContactNo"].ToString()) : TaxBillEntrylistObj.CustomerContactNo);
                                        TaxBillEntrylistObj.CustomerLocation = (sdr["CustomerLocation"].ToString() != "" ? (sdr["CustomerLocation"].ToString()) : TaxBillEntrylistObj.CustomerLocation);
                                        TaxBillEntrylistObj.Technician = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : TaxBillEntrylistObj.Technician);
                                        TaxBillEntrylistObj.PaymentRefNo = (sdr["PaymentRefNo"].ToString() != "" ? (sdr["PaymentRefNo"].ToString()) : TaxBillEntrylistObj.PaymentRefNo);
                                        TaxBillEntrylistObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : TaxBillEntrylistObj.Remarks);
                                        TaxBillEntrylistObj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : TaxBillEntrylistObj.VATAmount);
                                        TaxBillEntrylistObj.ServiceCharge = (sdr["ServiceCharge"].ToString() != "" ? decimal.Parse(sdr["ServiceCharge"].ToString()) : TaxBillEntrylistObj.ServiceCharge);
                                        TaxBillEntrylistObj.Subtotal = (sdr["SubTotal"].ToString() != "" ? decimal.Parse(sdr["SubTotal"].ToString()) : TaxBillEntrylistObj.Subtotal);
                                        TaxBillEntrylistObj.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : TaxBillEntrylistObj.Discount);                                       
                                    }

                                    TaxBillEntrylist.Add(TaxBillEntrylistObj);
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
            return TaxBillEntrylist;
        }
        #endregion  GetAllTaxBill 


        #region GetTaxBillDetail
        public List<TaxBillEntryDetail> GetTaxBillDetail(Guid ID, UA UA)
        {
            List<TaxBillEntryDetail> TaxBillEntryDetailList = null;
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
                        cmd.CommandText = "[GetTaxBillDetailByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                TaxBillEntryDetailList = new List<TaxBillEntryDetail>();
                                while (sdr.Read())
                                {
                                    TaxBillEntryDetail TaxBillEntryDetailObj = new TaxBillEntryDetail();
                                    {
                                        TaxBillEntryDetailObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : TaxBillEntryDetailObj.ID);
                                        TaxBillEntryDetailObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : TaxBillEntryDetailObj.SCCode);
                                        TaxBillEntryDetailObj.MaterialID = (sdr["ItemID"].ToString() != "" ? Guid.Parse(sdr["ItemID"].ToString()) : TaxBillEntryDetailObj.MaterialID);
                                        TaxBillEntryDetailObj.Quantity = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);
                                        TaxBillEntryDetailObj.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : 0);
                                        TaxBillEntryDetailObj.Material = (sdr["Material"].ToString() != "" ? (sdr["Material"].ToString()) : TaxBillEntryDetailObj.Material);
                                        TaxBillEntryDetailObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : TaxBillEntryDetailObj.Description);
                                        TaxBillEntryDetailObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : TaxBillEntryDetailObj.UOM);
                                        TaxBillEntryDetailObj.ReferralRate=(sdr["ReferralRate"].ToString() != "" ? decimal.Parse(sdr["ReferralRate"].ToString()) : 0);
                                    }

                                    TaxBillEntryDetailList.Add(TaxBillEntryDetailObj);
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
            return TaxBillEntryDetailList;
        }
        #endregion GetTaxBillDetail


        #region GetTaxBillHeaderByID
        public TaxBillEntry GetTaxBillHeaderByID(Guid ID, UA ua)
        {
            TaxBillEntry TaxBillEntryHeaderList = null;
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
                        cmd.CommandText = "[GetTaxBillHeaderByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {                               
                                while (sdr.Read())
                                {
                                    TaxBillEntry TaxBillEntryHeaderObj = new TaxBillEntry();
                                    {
                                        TaxBillEntryHeaderObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : TaxBillEntryHeaderObj.ID);
                                        TaxBillEntryHeaderObj.EmpID = (sdr["EmpID"].ToString() != "" ? Guid.Parse(sdr["EmpID"].ToString()) : TaxBillEntryHeaderObj.EmpID);
                                        TaxBillEntryHeaderObj.BillDate = (sdr["BillDate"].ToString() != "" ? DateTime.Parse(sdr["BillDate"].ToString()).ToString("dd-MMM-yyyy") : TaxBillEntryHeaderObj.BillDate);
                                        TaxBillEntryHeaderObj.BillNo = (sdr["BillNo"].ToString() != "" ? (sdr["BillNo"].ToString()) : TaxBillEntryHeaderObj.BillNo);
                                        TaxBillEntryHeaderObj.JobNo = (sdr["JobNo"].ToString() != "" ? (sdr["JobNo"].ToString()) : TaxBillEntryHeaderObj.JobNo);
                                        TaxBillEntryHeaderObj.CustomerName = (sdr["CustomerName"].ToString() != "" ? (sdr["CustomerName"].ToString()) : TaxBillEntryHeaderObj.CustomerName);
                                        TaxBillEntryHeaderObj.CustomerContactNo = (sdr["CustomerContactNo"].ToString() != "" ? (sdr["CustomerContactNo"].ToString()) : TaxBillEntryHeaderObj.CustomerContactNo);
                                        TaxBillEntryHeaderObj.CustomerLocation = (sdr["CustomerLocation"].ToString() != "" ? (sdr["CustomerLocation"].ToString()) : TaxBillEntryHeaderObj.CustomerLocation);
                                        TaxBillEntryHeaderObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? (sdr["PaymentMode"].ToString()) : TaxBillEntryHeaderObj.PaymentMode);
                                        TaxBillEntryHeaderObj.PaymentRefNo = (sdr["PaymentRefNo"].ToString() != "" ? (sdr["PaymentRefNo"].ToString()) : TaxBillEntryHeaderObj.PaymentRefNo);
                                        TaxBillEntryHeaderObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : TaxBillEntryHeaderObj.Remarks);
                                        TaxBillEntryHeaderObj.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : TaxBillEntryHeaderObj.Discount);
                                        TaxBillEntryHeaderObj.ServiceCharge = (sdr["ServiceCharge"].ToString() != "" ? decimal.Parse(sdr["ServiceCharge"].ToString()) : TaxBillEntryHeaderObj.ServiceCharge);
                                        TaxBillEntryHeaderObj.SCCommAmount = (sdr["ServiceChargeComm"].ToString() != "" ? decimal.Parse(sdr["ServiceChargeComm"].ToString()) : TaxBillEntryHeaderObj.SCCommAmount);
                                        TaxBillEntryHeaderObj.SpecialComm = (sdr["SpecialComm"].ToString() != "" ? decimal.Parse(sdr["SpecialComm"].ToString()) : TaxBillEntryHeaderObj.SpecialComm);
                                        TaxBillEntryHeaderObj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : TaxBillEntryHeaderObj.VATAmount);

                                    }

                                    TaxBillEntryHeaderList = TaxBillEntryHeaderObj;
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
            return TaxBillEntryHeaderList;
        }
        #endregion GetTaxBillHeaderByID

        #region UpdateTaxBillEntry
        public TaxBillEntry UpdateTaxBillEntry(TaxBillEntry taxBillEntry,UA UA)
        {
            try
            {
                SqlParameter outputStatus; //outputStatusDisable;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                       
                        cmd.CommandText = "[UpdateTaxBillEntryDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Int).Value = taxBillEntry.IsActive;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = taxBillEntry.ID;                        
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@BillNo", SqlDbType.NVarChar, 50).Value = taxBillEntry.BillNo;
                        cmd.Parameters.Add("@BillDate", SqlDbType.SmallDateTime).Value = taxBillEntry.BillDate;
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = taxBillEntry.EmpID;
                        cmd.Parameters.Add("@JobNo", SqlDbType.NVarChar, 50).Value = taxBillEntry.JobNo;
                        cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar, 250).Value = taxBillEntry.CustomerName;
                        cmd.Parameters.Add("@CustomerContactNo", SqlDbType.NVarChar, 50).Value = taxBillEntry.CustomerContactNo;
                        cmd.Parameters.Add("@CustomerLocation", SqlDbType.NVarChar, 50).Value = taxBillEntry.CustomerLocation;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar, 20).Value = taxBillEntry.PaymentMode;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = taxBillEntry.Remarks;
                        cmd.Parameters.Add("@VATAmount", SqlDbType.Decimal).Value = taxBillEntry.VATAmount;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = taxBillEntry.Discount;
                        cmd.Parameters.Add("@ServiceCharge", SqlDbType.Decimal).Value = taxBillEntry.ServiceCharge;
                        cmd.Parameters.Add("@ServiceChargeComm", SqlDbType.Decimal).Value = taxBillEntry.SCCommAmount;                       
                        cmd.Parameters.Add("@PaymentRefNo", SqlDbType.NVarChar, 50).Value = taxBillEntry.PaymentRefNo;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = taxBillEntry.DetailXML;

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
                        taxBillEntry.TaxBillEntryDetail = GetTaxBillDetail(taxBillEntry.ID, UA);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return taxBillEntry;
        }
        #endregion UpdateTaxBillEntry

      
    }
}