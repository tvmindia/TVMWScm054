using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class OtherIncomeBusiness : IOtherIncomeBusiness
    {
        private IOtherIncomeRepository _iOtherIncomeRepository;

        public OtherIncomeBusiness(IOtherIncomeRepository iOtherIncomeRepository)
        {
            _iOtherIncomeRepository = iOtherIncomeRepository;
        }
        public List<OtherIncome> GetAllOtherIncome(UA UA, string showAllYN)
        {
            List<OtherIncome> OtherIncomelist = null;
            OtherIncomelist = _iOtherIncomeRepository.GetAllOtherIncome(UA, showAllYN);
            if (OtherIncomelist != null)
            {
                foreach (OtherIncome cn in OtherIncomelist)
                {

                    DefectiveDamage_DF(cn);
                }
            }
            return OtherIncomelist;

        }
        public List<OtherIncome> GetOtherIncomeByID(UA UA, string ID)
        {
            List<OtherIncome> OtherIncomelist = null;
            OtherIncomelist = _iOtherIncomeRepository.GetOtherIncomeByID(UA, ID);
            if (OtherIncomelist != null)
            {
                foreach (OtherIncome cn in OtherIncomelist)
                {

                    DefectiveDamage_DF(cn);
                }
            }
            return OtherIncomelist;

        }
        public string DeleteOtherIncome(string ID, UA ua)
        {
            string status = null;
            try
            {
                status = _iOtherIncomeRepository.DeleteOtherIncome(ID, ua);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        private void DefectiveDamage_DF(OtherIncome oi)
        {
            if (oi != null)
            {
                SCManagerSettings settings = new SCManagerSettings();

                if (oi.RefDate != null)
                    oi.RefDateFormatted = oi.RefDate;//.GetValueOrDefault().ToString(settings.dateformat);
            }

        }
        public List<OtherIncome> GetOtherIncomeBetweenDates(UA UA, string fromDate, string toDate)
        {
            List<OtherIncome> OtherIncomelist = null;
            OtherIncomelist = _iOtherIncomeRepository.GetOtherIncomeBetweenDates(UA, fromDate, toDate);
            if (OtherIncomelist != null)
            {
                foreach (OtherIncome cn in OtherIncomelist)
                {

                    DefectiveDamage_DF(cn);
                }
            }
            return OtherIncomelist;

        }
        public object InsertUpdateOtherIncome(OtherIncome otherIncomeObj)
        {
            object result = null;
            try
            {
                if (otherIncomeObj.ID == Guid.Empty)
                {
                    result = _iOtherIncomeRepository.InsertOtherIncome(otherIncomeObj);
                }
                else
                {
                    otherIncomeObj.RefNo = otherIncomeObj.HiddenRefNo;
                    result = _iOtherIncomeRepository.UpdateOtherIncome(otherIncomeObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public List<OtherIncome> GetAllIncomeType()
        {
            List<OtherIncome> otherIncomeList = null;
            otherIncomeList = _iOtherIncomeRepository.GetAllIncomeType();
            return otherIncomeList;
        }
    }
}