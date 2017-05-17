using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
   public interface IReceiveFromTechnicianBusiness
    {
        List<ReceiveFromTechnician> GetReceiptsSheet(string ID, string transferDate, UA UA);
        List<ReceiveFromTechnician> GetAllReceiptsFromTechnician(string empID, string fromDate, string toDate, UA UA);
        List<ReceiveFromTechnician> InsertUpdateReceiveFromTechnician(List<ReceiveFromTechnician> receiveFromTechnician, Guid? empID, DateTime? receiveDate, UA UA);
        string DeleteReceiveFromTechnician(string ID, UA ua);
    }
}
