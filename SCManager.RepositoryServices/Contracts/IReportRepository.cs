using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
    public interface IReportRepository
    {
        List<SystemReport> GetAllSysReports(UA ua);
        List<Item> GetItemsSummary(UA UA, string fromdate = null, string todate = null);
        List<StockLedger> GetStockLedger(UA UA, string fromdate = null, string todate = null);
        List<TechnicianStock> GetTechniciansStockSummary(UA UA, string fromdate = null, string todate = null);
        List<IncomeExpense> GetMonthlyIncomeAndExpenditure(UA UA, string fromdate = null, string todate = null);
        List<AmcReport> GetAmcReportTable(UA UA, string fromdate, string todate);
        DataTable GetTechnicianPerformance(UA UA, Guid EMPID, int? month = null, int? year = null);
        List<AmcBaseValueSummary> GetAMCBaseValueSummary(UA UA, string fromdate, string todate);
        List<ProfitAndLossReport> GetProfitAndLossReport(UA UA, string fromdate, string todate);
    }
}
