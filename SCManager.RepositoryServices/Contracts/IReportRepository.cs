using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
    public interface IReportRepository
    {
       List<Item> GetItemsSummary(UA UA, string fromdate = null, string todate = null);
       List<SystemReport> GetAllSysReports(UA ua);
    }
}
