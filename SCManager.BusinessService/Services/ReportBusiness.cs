using SCManager.BusinessService.Contracts;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.DataAccessObject.DTO;

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
    }
}