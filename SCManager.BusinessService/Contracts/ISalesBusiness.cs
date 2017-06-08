using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Contracts
{
    public interface ISalesBusiness
    {
        List<SalesGraph> GetWeeklySalesDetails(UA UA);
    }
}