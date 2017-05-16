using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class IssueToTechnicianBusiness : IIssueToTechnicianBusiness
    {
        private IIssueToTechnicianRepository _iIssueToTechnicianRepository;
        private ICommonBusiness _commonBusiness;

        public IssueToTechnicianBusiness(IIssueToTechnicianRepository iIssueToTechnicianRepository, ICommonBusiness commonBusiness)
        {
            _iIssueToTechnicianRepository = iIssueToTechnicianRepository;
            _commonBusiness = commonBusiness;
        }
        public List<IssueToTechnician> GetIssueSheets(string ID, string transferDate, UA UA)
        {
            List<IssueToTechnician> IssueToTechnicianlist = null;
            IssueToTechnicianlist = _iIssueToTechnicianRepository.GetIssueSheets(ID, transferDate, UA);          
            return IssueToTechnicianlist;

        }
        public List<IssueToTechnician> GetAllIssueToTechnician(string empID, string fromDate, string toDate, UA UA)
        {
            List<IssueToTechnician> IssueToTechnicianlist = null;
            IssueToTechnicianlist = _iIssueToTechnicianRepository.GetAllIssueToTechnician(empID,fromDate, toDate,UA);
            if (IssueToTechnicianlist != null)
            {
                foreach (IssueToTechnician itt in IssueToTechnicianlist)
                {

                    IssueToTech_DF(itt);
                }
            }
            return IssueToTechnicianlist;

        }
        private void IssueToTech_DF(IssueToTechnician itt)
        {
            if (itt != null)
            {
                SCManagerSettings settings = new SCManagerSettings();

                if (itt.IssueDate != null)
                    itt.DateFormatted = itt.IssueDate.GetValueOrDefault().ToString(settings.dateformat);
            }

        }
        public List<IssueToTechnician> InsertUpdateIssueToTechnician(List<IssueToTechnician> issueToTechnician, Guid? empID, DateTime? issueDate, UA UA)
        {
            List<IssueToTechnician> itemToTech = null;


            try
            {
                itemToTech = new List<IssueToTechnician>();
                if(issueToTechnician != null){

                    foreach (IssueToTechnician I in issueToTechnician) {
                        if (I.ID == null || I.ID == Guid.Empty)
                        {
                            if(I.MaterialID!=null && I.MaterialID!=Guid.Empty)
                            {
                                issueToTechnician = _iIssueToTechnicianRepository.InsertIssueToTechnician(I, empID, issueDate, UA);
                                if(issueToTechnician!=null)
                                {
                                    itemToTech.AddRange(issueToTechnician);
                                }                                
                            }
                            
                        }
                        else
                        {
                            issueToTechnician = _iIssueToTechnicianRepository.UpdateIssueToTechnician(I,empID,issueDate,UA);
                            if (issueToTechnician != null)
                            {
                                itemToTech.AddRange(issueToTechnician);
                            }

                        }


                    }
                }


            }
            catch (Exception)
            {

                throw;
            }

            return itemToTech;
        }

        public string DeleteIssueToTechnician(string ID, UA ua)
        {
            string status = null;
            try
            {
                status = _iIssueToTechnicianRepository.DeleteIssueToTechnician(ID, ua);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

    }
}