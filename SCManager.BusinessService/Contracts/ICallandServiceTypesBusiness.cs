using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
   public interface ICallandServiceTypesBusiness
    {
        Object UpdateServiceTypesAndCommission(ServiceTypes serviceTypesObj);
        ServiceTypes GetServiceTypes(UA UA);
       
    }
}
