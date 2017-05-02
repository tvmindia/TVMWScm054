using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
  public  interface ICallandServiceTypesRepository
    {
        string UpdateServiceType(ServiceTypes serviceTypesObj);
        string UpdateCallType(CallTypes callTypesObj);
    }
}
