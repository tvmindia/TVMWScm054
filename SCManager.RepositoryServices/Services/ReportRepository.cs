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
    public class ReportRepository: IReportRepository
    {
        private IDatabaseFactory _databaseFactory;
        private Const constObj = new Const();
        public ReportRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region GetAllItems
        public List<Item> GetItemsSummary(UA UA, string fromdate = null, string todate = null)
        {
            List<Item> Itemlist = null;
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
                        cmd.Parameters.Add("@Fromdate", SqlDbType.DateTime).Value = fromdate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = todate;
                        cmd.CommandText = "[RPT_StockSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Itemlist = new List<Item>();
                                while (sdr.Read())
                                {
                                    Item _ItemObj = new Item();
                                    {
                                        _ItemObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ItemObj.ID);
                                        _ItemObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _ItemObj.SCCode);
                                        _ItemObj.ItemCode = (sdr["ItemCode"].ToString() != "" ? (sdr["ItemCode"].ToString()) : _ItemObj.ItemCode);
                                        _ItemObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _ItemObj.Description);
                                        _ItemObj.CategoryID = (sdr["CategoryID"].ToString() != "" ? Guid.Parse(sdr["CategoryID"].ToString()) : _ItemObj.CategoryID);
                                        _ItemObj.Category = (sdr["Category"].ToString() != "" ? (sdr["Category"].ToString()) : _ItemObj.Category);
                                        _ItemObj.SubcategoryID = (sdr["SubcategoryID"].ToString() != "" ? Guid.Parse(sdr["SubcategoryID"].ToString()) : _ItemObj.SubcategoryID);
                                        _ItemObj.Subcategory = (sdr["Subcategory"].ToString() != "" ? (sdr["Subcategory"].ToString()) : _ItemObj.Subcategory);
                                        _ItemObj.Stock = (sdr["StockQty"].ToString() != "" ? (sdr["StockQty"].ToString()) : _ItemObj.Stock);
                                        _ItemObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _ItemObj.UOM);
                                        _ItemObj.ReorderQty = (sdr["ReorderQty"].ToString() != "" ? (sdr["ReorderQty"].ToString()) : _ItemObj.ReorderQty);
                                        _ItemObj.ProductCommission = (sdr["ProductCommission"].ToString() != "" ? float.Parse(sdr["ProductCommission"].ToString()) : _ItemObj.ProductCommission);
                                        _ItemObj.Remarks = (sdr["Remarks"].ToString() != "" ?(sdr["Remarks"].ToString()) : _ItemObj.Remarks);
                                        _ItemObj.SellingRate= (sdr["SellingRate"].ToString() != "" ? float.Parse(sdr["SellingRate"].ToString()) : _ItemObj.SellingRate);
                                    }

                                    Itemlist.Add(_ItemObj);
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
            return Itemlist;
        }
        #endregion  GetAllItems
        public List<StockLedger> GetStockLedger(UA UA, string fromdate = null, string todate = null)
        {
            List<StockLedger> stockLedgerList = null;
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
                        cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = fromdate;
                        cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = todate;
                        cmd.CommandText = "[RPT_StockLedger]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                stockLedgerList = new List<StockLedger>();
                                while (sdr.Read())
                                {
                                    StockLedger _stockLedgerObj = new StockLedger();
                                    {
                                        _stockLedgerObj.Order = (sdr["ORD"].ToString() != "" ? int.Parse(sdr["ORD"].ToString()) : _stockLedgerObj.Order);
                                        _stockLedgerObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _stockLedgerObj.SCCode);
                                        _stockLedgerObj.ItemCode = (sdr["ItemCode"].ToString() != "" ? (sdr["ItemCode"].ToString()) : _stockLedgerObj.ItemCode);
                                        _stockLedgerObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _stockLedgerObj.Description);
                                        _stockLedgerObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : _stockLedgerObj.Type);
                                        _stockLedgerObj.RefNo = (sdr["RefNo"].ToString() != "" ? (sdr["RefNo"].ToString()) : _stockLedgerObj.RefNo);
                                        _stockLedgerObj.Qty = (sdr["qty"].ToString() != "" ? decimal.Parse(sdr["qty"].ToString()) : _stockLedgerObj.Qty);
                                        _stockLedgerObj.Location = (sdr["Location"].ToString() != "" ? (sdr["Location"].ToString()) : _stockLedgerObj.Location);
                                        _stockLedgerObj.GroupCode= (sdr["GroupCode"].ToString() != "" ? (sdr["GroupCode"].ToString()) : _stockLedgerObj.GroupCode);
                                        _stockLedgerObj.logDetails = new LogDetails();
                                        _stockLedgerObj.logDetails.CreatedDate = (sdr["CreatedDate"].ToString() != "" ? (DateTime.Parse(sdr["CreatedDate"].ToString())) : _stockLedgerObj.logDetails.CreatedDate);
                                    }
                                    stockLedgerList.Add(_stockLedgerObj);
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
            return stockLedgerList;
        }
        public List<SystemReport> GetAllSysReports(UA ua)
        {
            List<SystemReport> Reportlist = null;
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
                        cmd.CommandText = "[GetAllSys_Reports]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Reportlist = new List<SystemReport>();
                                while (sdr.Read())
                                {
                                    SystemReport _ReportObj = new SystemReport();
                                    {
                                        _ReportObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ReportObj.ID);
                                        _ReportObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _ReportObj.SCCode);
                                        _ReportObj.ReportName = (sdr["ReportName"].ToString() != "" ? (sdr["ReportName"].ToString()) : _ReportObj.ReportName);
                                        _ReportObj.ReportDescription = (sdr["ReportDescription"].ToString() != "" ? (sdr["ReportDescription"].ToString()) : _ReportObj.ReportDescription);
                                        _ReportObj.Controller = (sdr["Controller"].ToString() != "" ? sdr["Controller"].ToString() : _ReportObj.Controller);
                                        _ReportObj.Action = (sdr["Action"].ToString() != "" ? sdr["Action"].ToString() : _ReportObj.Action);
                                        _ReportObj.SPName = (sdr["SPName"].ToString() != "" ? sdr["SPName"].ToString() : _ReportObj.SPName);
                                        _ReportObj.SQL = (sdr["SQL"].ToString() != "" ? sdr["SQL"].ToString() : _ReportObj.SQL);
                                        _ReportObj.Order= (sdr["Order"].ToString() != "" ? int.Parse(sdr["Order"].ToString()) : _ReportObj.Order);
                                    }
                                    Reportlist.Add(_ReportObj);
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
            return Reportlist;
        }
    }
}