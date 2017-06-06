﻿using SCManager.BusinessService.Contracts;
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