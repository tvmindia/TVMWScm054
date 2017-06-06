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

        public List<TechnicianSalary> GetTechniciansCalculatedSalary(string SCCode, short? Month, short? Year)
        {
            List<TechnicianSalary> technicianSalaryList = null;
            try
            {
                technicianSalaryList = _technicianSalaryCalculationRepository.GetTechniciansCalculatedSalary(SCCode, Month, Year);
               // ItemList = ItemList == null ? null : ItemList.Select(item => { item.Value = int.Parse(item.Stock) * item.SellingRate; return item; }).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return technicianSalaryList;
        }
    }

}
