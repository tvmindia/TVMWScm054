using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
   public interface IReceiveFromTechnicianRepository
    {
        List<ReceiveFromTechnician> GetReceiptsSheet(string empID, string transferDate, UA UA);
        List<ReceiveFromTechnician> GetAllReceiptsFromTechnician(string empID, string fromDate, string toDate, UA UA);
        List<ReceiveFromTechnician> InsertReceiveFromTechnician(ReceiveFromTechnician receiveFromTechnician, Guid? empID, DateTime? receiveDate, UA UA);
        List<ReceiveFromTechnician> UpdateReceiveFromTechnician(ReceiveFromTechnician receiveFromTechnician, Guid? empID, DateTime? issueDate, UA UA);
        string DeleteReceiveFromTechnician(string ID, UA ua);
    }
}
