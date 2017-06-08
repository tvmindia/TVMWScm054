using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.RepositoryServices.Contracts
{
    public interface ISalesRepository
    {
        List<SalesGraph> GetWeeklySalesDetails(UA UA);
    }
}