using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCManager.DataAccessObject.DTO;

namespace SCManager.RepositoryServices.Contracts
{
    public interface IDynamicUIRepository
    {
        List<Menu> GetAllMenues();
        List<ReorderAlert> GetReorderAlertITems(UA UA);
        List<StockValueSummary> GetStockValueSummary(UA UA);
    }
}
