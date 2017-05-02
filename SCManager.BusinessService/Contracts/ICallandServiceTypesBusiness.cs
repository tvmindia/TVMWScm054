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
        string UpdateCallandServiceTypes(CallTypes callTypesObj, ServiceTypes serviceTypesObj);
        List<ServiceTypes> GetServiceTypes(UA UA);
        List<CallTypes> GetCallTypes(UA UA);
    }
}
