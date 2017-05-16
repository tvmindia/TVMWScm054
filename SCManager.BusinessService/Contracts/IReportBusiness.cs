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
        List<Item> GetItemsSummary(UA UA, string fromdate = null, string todate = null);
    }
}
