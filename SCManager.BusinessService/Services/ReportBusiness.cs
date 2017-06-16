using SCManager.BusinessService.Contracts;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.DataAccessObject.DTO;
using System.Data;

namespace SCManager.BusinessService.Services
{
    public class ReportBusiness: IReportBusiness
    {
        IReportRepository _reportRepository;
        public ReportBusiness(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public List<Item> GetItemsSummary(UA UA, string fromdate = null, string todate = null)
        {
            List<Item> ItemList = null;
            try
            {
                ItemList = _reportRepository.GetItemsSummary(UA, fromdate, todate);
                ItemList = ItemList == null ? null : ItemList.Select(item => { item.Value = int.Parse(item.Stock) * item.SellingRate; return item; }).ToList();
             
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }

        public List<SystemReport> GetAllSysReports(UA ua)
        {
            List<SystemReport> ReportList = null;
            try
            {
                ReportList = _reportRepository.GetAllSysReports(ua);
                ReportList = ReportList == null ? null : ReportList.OrderBy(rep => rep.Order).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ReportList;
        }

        public List<StockLedger> GetStockLedger(UA UA, string fromdate = null, string todate = null)
        {
            List<StockLedger> StockLedgerList = null;
            try
            {
               StockLedgerList = _reportRepository.GetStockLedger(UA, fromdate, todate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StockLedgerList;
        }

       public List<IncomeExpense> GetMonthlyIncomeAndExpenditure(UA UA, string fromdate = null, string todate = null)
        {
            List<IncomeExpense> IncomeexpenseList = null;
            try
            {
                IncomeexpenseList = _reportRepository.GetMonthlyIncomeAndExpenditure(UA, fromdate, todate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IncomeexpenseList;
        }
        public List<TechnicianStock> GetTechniciansStockSummary(UA UA, string fromdate = null, string todate = null)
        {
            List<TechnicianStock> TechnicianList = null;
            try
            {
                TechnicianList = _reportRepository.GetTechniciansStockSummary(UA, fromdate, todate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TechnicianList;
        }

        public List<AmcReport> GetAmcReportTable(UA UA, string fromdate = null, string todate = null)
        {
            List<AmcReport> ItemList = null;
            try
            {
                ItemList = _reportRepository.GetAmcReportTable(UA, fromdate, todate);
                if (ItemList != null)
                {
                    foreach (AmcReport EX in ItemList)
                    {
                        SCManagerSettings settings = new SCManagerSettings();

                        if (EX.AmcStartDate != null)
                            EX.AmcStartDate = Convert.ToDateTime(EX.AmcStartDate).ToString(settings.dateformat);
                        if (EX.AmcEndDate != null)
                            EX.AmcEndDate = Convert.ToDateTime(EX.AmcEndDate).ToString(settings.dateformat);
                    }
                }
                //ItemList = ItemList == null ? null : ItemList.Select(item => { item.Value = int.Parse(item.Stock) * item.SellingRate; return item; }).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }
        public DataTable GetTechnicianPerformance(UA UA, Guid EMPID, int? month = null, int? year = null)
        {
            DataTable PerformanceList = null;
            try
            {
                string d = "";
                int i = 0;
                PerformanceList = _reportRepository.GetTechnicianPerformance(UA, EMPID, month, year);
                if(PerformanceList!=null)
                {
                    PerformanceList.Columns.Remove("Date1");
                    PerformanceList.Columns.Remove("Date2");
                    foreach (DataRow dr in PerformanceList.Rows)
                    {

                        d = dr["Date"].ToString();
                        DateTime date = DateTime.Parse(d);
                        d = date.ToString("dd-MMM-yyyy");
                        PerformanceList.Rows[i]["Date"] = d;
                        i++;
                    }
                    PerformanceList.Columns["Date"].SetOrdinal(0);
                    DataRow row = PerformanceList.NewRow();
                    for (int j = 0; j < PerformanceList.Columns.Count; j++)
                    {
                        if(PerformanceList.Columns[j].Caption=="Date"|| PerformanceList.Columns[j].Caption == "Day")
                        {
                            row[j] = "Total";
                        }
                        else
                        {
                            //PerformanceList.Columns[j].DataType = typeof(int);
                            row[j] = PerformanceList.Compute("Sum([" + PerformanceList.Columns[j].Caption + "])", "");
                        }                        
                    }
                    PerformanceList.Rows.Add(row);
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PerformanceList;
        }
        
    }
}