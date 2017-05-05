using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
   public interface IDefectiveDamageBusiness
    {
        List<DefectiveDamage> GetAllDefectiveDamaged(UA UA);
        object InsertUpdateDefectiveDamaged(DefectiveDamage defectiveDamageObj);
        List<DefectiveDamage> GetDefectiveDamagedByID(UA UA, string ID);
        string DeleteDefectiveDamaged(string ID, UA ua);
        string ReturnDefectiveDamaged(string ID, UA ua);
        string DefectiveDamagedValidation(string itemID, string empID, UA ua);
    }
}
