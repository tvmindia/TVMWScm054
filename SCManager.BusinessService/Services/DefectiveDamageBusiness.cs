using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class DefectiveDamageBusiness : IDefectiveDamageBusiness
    {
        private IDefectiveDamageRepository _iDefectiveDamageRepository;
        public DefectiveDamageBusiness(IDefectiveDamageRepository iDefectiveDamageRepository)
        {
            _iDefectiveDamageRepository = iDefectiveDamageRepository;
        }

        public List<DefectiveDamage> GetAllDefectiveDamaged(UA UA)
        {
            List<DefectiveDamage> defectiveDamageList = null;
            defectiveDamageList = _iDefectiveDamageRepository.GetAllDefectiveDamaged(UA);
            if (defectiveDamageList != null)
            {
                foreach (DefectiveDamage dd in defectiveDamageList)
                {

                    DefectiveDamage_DF(dd);
                }
            }
            return defectiveDamageList;
        }
        private void DefectiveDamage_DF(DefectiveDamage dd)
        {
            if (dd != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
              
                if (dd.OpenDate != null)
                    dd.OpenDateFormatted = dd.OpenDate.GetValueOrDefault().ToString(settings.dateformat);                
            }

        }
        public List<DefectiveDamage> GetDefectiveDamagedByID(UA UA,string ID)
        {
            List<DefectiveDamage> defectiveDamageList = null;
            defectiveDamageList = _iDefectiveDamageRepository.GetDefectiveDamagedByID(UA,ID);
            return defectiveDamageList;
        }
        public object InsertUpdateDefectiveDamaged(DefectiveDamage defectiveDamageObj)
        {
            object result = null;
            try
            {
                defectiveDamageObj.ReturnStatusYN = false;
                if (defectiveDamageObj.ID == Guid.Empty)
                {                  
                    result = _iDefectiveDamageRepository.InsertDefectiveDamaged(defectiveDamageObj);
                }
                else
                {
                    result = _iDefectiveDamageRepository.UpdateDefectiveDamaged(defectiveDamageObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}