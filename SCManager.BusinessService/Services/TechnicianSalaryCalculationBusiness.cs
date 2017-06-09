using SCManager.BusinessService.Contracts;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.DataAccessObject.DTO;

namespace SCManager.BusinessService.Services
{
    public class TechnicianSalaryCalculationBusiness : ITechnicianSalaryCalculationBusiness
    {
        ITechnicianSalaryCalculationRepository _technicianSalaryCalculationRepository;
      
        public TechnicianSalaryCalculationBusiness(ITechnicianSalaryCalculationRepository technicianSalaryCalculationRepository)
        {
            _technicianSalaryCalculationRepository=technicianSalaryCalculationRepository;
           
        }

       

        public List<TechnicianSalary> GetTechniciansCalculatedSalary(string SCCode, string Month, string Year)
        {
            List<TechnicianSalary> technicianSalaryList = null;
            try
            {
                Int16? M=null;
                Int16? Y=null;
                if (!string.IsNullOrEmpty(Month))
                {
                    M = Int16.Parse(Month);
                }

                if(!string.IsNullOrEmpty(Year))
                {
                    Y= Int16.Parse(Year);
                }
                technicianSalaryList = _technicianSalaryCalculationRepository.GetTechniciansCalculatedSalary(SCCode, M, Y);
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return technicianSalaryList;
        }
        public List<TechnicianSalaryJobBreakUp> GetTechnicianJobCommissionBreakUp(string SCCode, Guid EmpID, short? Month, short? Year)
        {
            List<TechnicianSalaryJobBreakUp> TechnicianSalaryJobBreakUpList = null;
            try
            {
                //
                TechnicianSalaryJobBreakUpList = _technicianSalaryCalculationRepository.GetTechnicianJobCommissionBreakUp(SCCode, EmpID, Month, Year);
                TechnicianSalaryJobBreakUpList = TechnicianSalaryJobBreakUpList != null ? TechnicianSalaryJobBreakUpList.Select(c => { c.ServiceDate = DateTime.Parse(c.ServiceDate).Date.ToString("yyyy-MM-dd"); return c; }).ToList() : null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return TechnicianSalaryJobBreakUpList;
        }

        public List<TechnicianSalaryTCRBreakUp> GetTechnicianTCRCommissionBreakUp(string SCCode, Guid EmpID, short? Month, short? Year)
        {
            List<TechnicianSalaryTCRBreakUp> TechnicianSalaryTCRBreakUpList = null;
            try
            {
                //
                TechnicianSalaryTCRBreakUpList = _technicianSalaryCalculationRepository.GetTechnicianTCRCommissionBreakUp(SCCode, EmpID, Month, Year);
                TechnicianSalaryTCRBreakUpList = TechnicianSalaryTCRBreakUpList != null ? TechnicianSalaryTCRBreakUpList.Select(c => { c.BillDate = DateTime.Parse(c.BillDate).Date.ToString("yyyy-MM-dd"); return c; }).ToList() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TechnicianSalaryTCRBreakUpList;
        }

        public List<TechnicianSalaryAMCBreakUp> GetTechnicianAMCCommissionBreakUp(string SCCode, Guid EmpID, short? Month, short? Year)
        {
            List<TechnicianSalaryAMCBreakUp> TechnicianSalaryAMCBreakUpList = null;
            try
            {
                //
                TechnicianSalaryAMCBreakUpList = _technicianSalaryCalculationRepository.GetTechnicianAMCCommissionBreakUp(SCCode, EmpID, Month, Year);
                TechnicianSalaryAMCBreakUpList = TechnicianSalaryAMCBreakUpList != null ? TechnicianSalaryAMCBreakUpList.Select(c => { c.ICRDate = DateTime.Parse(c.ICRDate).Date.ToString("yyyy-MM-dd"); return c; }).ToList() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TechnicianSalaryAMCBreakUpList;
        }

        public List<TechnicianSalaryAdvanceBreakUp> GetTechnicianSalaryAdvanceBreakUp(string SCCode, Guid EmpID, short? Month, short? Year)
        {
            List<TechnicianSalaryAdvanceBreakUp> TechnicianSalaryAdvanceBreakUpList = null;
            try
            {
                //
                TechnicianSalaryAdvanceBreakUpList = _technicianSalaryCalculationRepository.GetTechnicianSalaryAdvanceBreakUp(SCCode, EmpID, Month, Year);
                TechnicianSalaryAdvanceBreakUpList = TechnicianSalaryAdvanceBreakUpList != null ? TechnicianSalaryAdvanceBreakUpList.Select(c => { c.RefDate = DateTime.Parse(c.RefDate).Date.ToString("yyyy-MM-dd"); return c; }).ToList() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TechnicianSalaryAdvanceBreakUpList;
        }
    }

}
