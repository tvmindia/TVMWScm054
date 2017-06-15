using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
    public interface IReportBusiness
    {
        List<SystemReport> GetAllSysReports(UA ua);
        List<Item> GetItemsSummary(UA UA, string fromdate = null, string todate = null);
        List<StockLedger> GetStockLedger(UA UA, string fromdate = null, string todate = null);
        List<TechnicianStock> GetTechniciansStockSummary(UA UA, string fromdate = null, string todate = null);
        List<IncomeExpense> GetMonthlyIncomeAndExpenditure(UA UA, string fromdate = null, string todate = null);
        List<AmcReport> GetAmcReportTable(UA UA, string fromdate, string todate);
        
    }
}
