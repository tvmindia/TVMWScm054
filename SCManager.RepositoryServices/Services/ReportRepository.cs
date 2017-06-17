using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
                                        _stockLedgerObj.logDetails.CreatedDatestr= (sdr["CreatedDate"].ToString() != "" ? (DateTime.Parse(sdr["CreatedDate"].ToString())).ToString("dd-MMM-yyyy") : _stockLedgerObj.logDetails.CreatedDatestr);
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

        public List<IncomeExpense> GetMonthlyIncomeAndExpenditure(UA UA, string fromdate = null, string todate = null)
        {
            List<IncomeExpense> incomeexpenseList = null;
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
                        cmd.CommandText = "[RPT_MonthlyIncomeExpense]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                incomeexpenseList = new List<IncomeExpense>();
                                while (sdr.Read())
                                {
                                    IncomeExpense _incomeExpense = new IncomeExpense();
                                    {
                                        _incomeExpense.ReferenceNo = (sdr["Ref"].ToString() != "" ? sdr["Ref"].ToString() : _incomeExpense.ReferenceNo);
                                        _incomeExpense.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _incomeExpense.Description);
                                        _incomeExpense.AccountHead = (sdr["AcHead"].ToString() != "" ? (sdr["AcHead"].ToString()) : _incomeExpense.AccountHead);

                                        _incomeExpense.Income = (sdr["Income"].ToString() != "" ? decimal.Parse(sdr["Income"].ToString()) : _incomeExpense.Income);
                                        _incomeExpense.Expense = (sdr["Expense"].ToString() != "" ? decimal.Parse(sdr["Expense"].ToString()) : _incomeExpense.Expense);
                                        _incomeExpense.Balance = (sdr["Balance"].ToString() != "" ? decimal.Parse(sdr["Balance"].ToString()) : _incomeExpense.Balance);
                                        _incomeExpense.logDetails = new LogDetails();
                                        _incomeExpense.logDetails.CreatedDate = (sdr["CreatedDate"].ToString() != "" ? (DateTime.Parse(sdr["CreatedDate"].ToString())) : _incomeExpense.logDetails.CreatedDate);
                                        _incomeExpense.logDetails.CreatedDatestr= (sdr["CreatedDate"].ToString() != "" ? (DateTime.Parse(sdr["CreatedDate"].ToString())).ToString("dd-MMM-yyyy") : _incomeExpense.logDetails.CreatedDatestr);
                                    }
                                    incomeexpenseList.Add(_incomeExpense);
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
            return incomeexpenseList;
        }


        public List<TechnicianStock> GetTechniciansStockSummary(UA UA, string fromdate = null, string todate = null)
        {
            List<TechnicianStock> TechncianList = null;
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
                        cmd.CommandText = "[RPT_TechSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                TechncianList = new List<TechnicianStock>();
                                while (sdr.Read())
                                {
                                    TechnicianStock _technicianStockObj = new TechnicianStock();
                                    {
                                        _technicianStockObj.Name = (sdr["name"].ToString() != "" ? sdr["name"].ToString() : _technicianStockObj.Name);
                                        _technicianStockObj.ItemCode = (sdr["ItemCode"].ToString() != "" ? (sdr["ItemCode"].ToString()) : _technicianStockObj.ItemCode);
                                        _technicianStockObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _technicianStockObj.Description);
                                        _technicianStockObj.Stock = (sdr["StockQty"].ToString() != "" ? (decimal.Parse(sdr["StockQty"].ToString())) : _technicianStockObj.Stock);
                                        _technicianStockObj.Rate = (sdr["Rate"].ToString() != "" ? (decimal.Parse(sdr["Rate"].ToString())) : _technicianStockObj.Rate);
                                        _technicianStockObj.Value = (sdr["Value"].ToString() != "" ? (decimal.Parse(sdr["Value"].ToString())) : _technicianStockObj.Value);
                                    }
                                    TechncianList.Add(_technicianStockObj);
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
            return TechncianList;
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

        public List<AmcReport> GetAmcReportTable(UA UA, string fromdate, string todate)
        {
            List<AmcReport> Reportlist = null;
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
                        cmd.Parameters.Add("@CurrentDate", SqlDbType.DateTime).Value = UA.GetCurrentDateTime();
                        cmd.Parameters.Add("@Fromdate", SqlDbType.DateTime).Value = fromdate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = todate;
                        cmd.CommandText = "[RPT_AmcExpiryAlert]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Reportlist = new List<AmcReport>();
                                while (sdr.Read())
                                {
                                    AmcReport _ReportObj = new AmcReport();
                                    {
                                        _ReportObj.CustomerName = (sdr["CustomerName"].ToString() != "" ? (sdr["CustomerName"].ToString()) : _ReportObj.CustomerName);
                                        _ReportObj.Location = (sdr["CustomerLocation"].ToString() != "" ? (sdr["CustomerLocation"].ToString()) : _ReportObj.Location);
                                        _ReportObj.ContactNo = (sdr["CustomerContactNo"].ToString() != "" ? (sdr["CustomerContactNo"].ToString()) : _ReportObj.ContactNo);
                                        _ReportObj.Model = (sdr["ModelNo"].ToString() != "" ? sdr["ModelNo"].ToString() : _ReportObj.Model);
                                        _ReportObj.SerialNo = (sdr["SerialNo"].ToString() != "" ? sdr["SerialNo"].ToString() : _ReportObj.SerialNo);
                                        _ReportObj.AmcNo = (sdr["AMCNO"].ToString() != "" ? sdr["AMCNO"].ToString() : _ReportObj.AmcNo);
                                        _ReportObj.AmcStartDate = (sdr["AMCValidFromDate"].ToString() != "" ? sdr["AMCValidFromDate"].ToString() : _ReportObj.AmcStartDate);
                                        _ReportObj.AmcEndDate = (sdr["AMCValidToDate"].ToString() != "" ? sdr["AMCValidToDate"].ToString() : _ReportObj.AmcEndDate);
                                        _ReportObj.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : _ReportObj.Remarks);
                                        _ReportObj.DueDays = (sdr["DaysCount"].ToString() != "" ? sdr["DaysCount"].ToString() : _ReportObj.DueDays);
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
        public DataTable GetTechnicianPerformance(UA UA,Guid EMPID, int? month = null, int? year = null)
        {
            DataTable dt = null;
            try
            {
                
                SqlDataAdapter sda = null;
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
                        cmd.Parameters.Add("@EMPID", SqlDbType.UniqueIdentifier).Value = EMPID;
                        cmd.Parameters.Add("@month", SqlDbType.Int).Value = month;
                        cmd.Parameters.Add("@year", SqlDbType.Int).Value = year;
                        cmd.CommandText = "[RPT_GetTechPerformance]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        sda = new SqlDataAdapter();
                        sda.SelectCommand = cmd;
                        dt = new DataTable();
                        sda.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public List<AmcBaseValueSummary> GetAMCBaseValueSummary(UA UA, string fromdate, string todate)
        {
            List<AmcBaseValueSummary> AmcBaseValueSummaryList = null;
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
                        cmd.CommandText = "[RPT_AMCBaseValueSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                AmcBaseValueSummaryList = new List<AmcBaseValueSummary>();
                                while (sdr.Read())
                                {
                                    AmcBaseValueSummary _amcBaseValueSummary = new AmcBaseValueSummary();
                                    {
                                        _amcBaseValueSummary.ICRDate = (sdr["ICRDATE"].ToString() != "" ? sdr["ICRDATE"].ToString() : _amcBaseValueSummary.ICRDate);
                                        _amcBaseValueSummary.ICRNo = (sdr["ICRNo"].ToString() != "" ? (sdr["ICRNo"].ToString()) : _amcBaseValueSummary.ICRNo);
                                        _amcBaseValueSummary.AMCNo = (sdr["AMCNO"].ToString() != "" ? (sdr["AMCNO"].ToString()) : _amcBaseValueSummary.AMCNo);
                                        _amcBaseValueSummary.AMCValidFromDate = (sdr["AMCValidFromDate"].ToString() != "" ? sdr["AMCValidFromDate"].ToString() : _amcBaseValueSummary.AMCValidFromDate);
                                        _amcBaseValueSummary.AMCValidToDate = (sdr["AMCValidToDate"].ToString() != "" ? sdr["AMCValidToDate"].ToString() : _amcBaseValueSummary.AMCValidToDate);
                                        _amcBaseValueSummary.Technician = (sdr["Technician"].ToString() != "" ? sdr["Technician"].ToString() : _amcBaseValueSummary.Technician);
                                        _amcBaseValueSummary.CustomerName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : _amcBaseValueSummary.CustomerName);
                                        _amcBaseValueSummary.BaseAmount = (sdr["BaseAmount"].ToString() != "" ? (decimal.Parse(sdr["BaseAmount"].ToString())) : _amcBaseValueSummary.BaseAmount);
                                        _amcBaseValueSummary.ServiceCharge = (sdr["ServiceCharge"].ToString() != "" ? (decimal.Parse(sdr["ServiceCharge"].ToString())) : _amcBaseValueSummary.ServiceCharge);
                                        _amcBaseValueSummary.Discount = (sdr["Discount"].ToString() != "" ? (decimal.Parse(sdr["Discount"].ToString())) : _amcBaseValueSummary.Discount);
                                        _amcBaseValueSummary.Total = (sdr["Total"].ToString() != "" ? (decimal.Parse(sdr["Total"].ToString())) : _amcBaseValueSummary.Total);

                                    }
                                    AmcBaseValueSummaryList.Add(_amcBaseValueSummary);
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
            return AmcBaseValueSummaryList;
        }

        public List<ProfitAndLossReport> GetProfitAndLossReport(UA UA, string fromdate, string todate)
        {
            List<ProfitAndLossReport> ProfitAndLossReportList = null;
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
                        cmd.CommandText = "[RPT_ProfitAndLoss]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ProfitAndLossReportList = new List<ProfitAndLossReport>();
                                while (sdr.Read())
                                {
                                    ProfitAndLossReport _profitAndLossReport = new ProfitAndLossReport();
                                    {
                                        _profitAndLossReport.SCCode = (sdr["SCCode"].ToString() != "" ? sdr["SCCode"].ToString() : _profitAndLossReport.SCCode);
                                        _profitAndLossReport.BaseType = (sdr["BaseType"].ToString() != "" ? (sdr["BaseType"].ToString()) : _profitAndLossReport.BaseType);
                                        _profitAndLossReport.Type = (sdr["Type"].ToString() != "" ? (sdr["Type"].ToString()) : _profitAndLossReport.Type);
                                        _profitAndLossReport.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : _profitAndLossReport.Description);
                                        _profitAndLossReport.Amount = (sdr["Amount"].ToString() != "" ? (decimal.Parse(sdr["Amount"].ToString())) : _profitAndLossReport.Amount);
                                       

                                    }
                                    ProfitAndLossReportList.Add(_profitAndLossReport);
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
            return ProfitAndLossReportList;
        }
    }
}