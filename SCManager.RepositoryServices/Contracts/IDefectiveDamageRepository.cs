using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
   public interface IDefectiveDamageRepository
    {
        List<DefectiveDamage> GetAllDefectiveDamaged(UA UA);
        object InsertDefectiveDamaged(DefectiveDamage defectiveDamageObj);
        object UpdateDefectiveDamaged(DefectiveDamage defectiveDamageObj);
        List<DefectiveDamage> GetDefectiveDamagedByID(UA UA, string ID);
        string DeleteDefectiveDamaged(string ID, UA ua);
        string ReturnDefectiveDamaged(string ID, UA ua);
        string DefectiveDamagedValidation(string itemID, string empID, string type, UA ua);
    }
}
