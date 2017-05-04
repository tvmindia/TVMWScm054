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
    }
}
