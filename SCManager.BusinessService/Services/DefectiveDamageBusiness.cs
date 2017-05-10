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
                    defectiveDamageObj.EmpID = defectiveDamageObj.HiddenEmpID;
                    defectiveDamageObj.Type = defectiveDamageObj.HiddenType;
                    result = _iDefectiveDamageRepository.UpdateDefectiveDamaged(defectiveDamageObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public string DeleteDefectiveDamaged(string ID, UA ua)
        {
            string status = null;
            try
            {
                status = _iDefectiveDamageRepository.DeleteDefectiveDamaged(ID, ua);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public string DefectiveDamagedValidation(string itemID, string empID, string type, UA ua)
        {
            string status = null;
            try
            {
                status = _iDefectiveDamageRepository.DefectiveDamagedValidation(itemID, empID,type, ua);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public string ReturnDefectiveDamaged(string ID, UA ua)
        {
            string status = null;
            try
            {
                status = _iDefectiveDamageRepository.ReturnDefectiveDamaged(ID, ua);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
    }
}