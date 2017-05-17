using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class ReceiveFromTechnicianBusiness : IReceiveFromTechnicianBusiness
    {
        private IReceiveFromTechnicianRepository _iReceiveFromTechnicianRepository;
        private ICommonBusiness _commonBusiness;

        public ReceiveFromTechnicianBusiness(IReceiveFromTechnicianRepository iReceiveFromTechnicianRepository, ICommonBusiness commonBusiness)
        {
            _iReceiveFromTechnicianRepository = iReceiveFromTechnicianRepository;
            _commonBusiness = commonBusiness;
        }
        public List<ReceiveFromTechnician> GetReceiptsSheet(string ID, string transferDate, UA UA)
        {
            List<ReceiveFromTechnician> ReceiveFromTechnicianlist = null;
            ReceiveFromTechnicianlist = _iReceiveFromTechnicianRepository.GetReceiptsSheet(ID, transferDate, UA);
            return ReceiveFromTechnicianlist;

        }
        public List<ReceiveFromTechnician> GetAllReceiptsFromTechnician(string empID, string fromDate, string toDate, UA UA)
        {
            List<ReceiveFromTechnician> ReceiveFromTechnicianlist = null;
            ReceiveFromTechnicianlist = _iReceiveFromTechnicianRepository.GetAllReceiptsFromTechnician(empID, fromDate, toDate, UA);
            if (ReceiveFromTechnicianlist != null)
            {
                foreach (ReceiveFromTechnician rft in ReceiveFromTechnicianlist)
                {

                    ReceiveFromTech_DF(rft);
                }
            }
            return ReceiveFromTechnicianlist;

        }
        private void ReceiveFromTech_DF(ReceiveFromTechnician rft)
        {
            if (rft != null)
            {
                SCManagerSettings settings = new SCManagerSettings();

                if (rft.ReceiveDate != null)
                    rft.DateFormatted = rft.ReceiveDate.GetValueOrDefault().ToString(settings.dateformat);
            }

        }
        public List<ReceiveFromTechnician> InsertUpdateReceiveFromTechnician(List<ReceiveFromTechnician> receiveFromTechnician, Guid? empID, DateTime? receiveDate, UA UA)
        {
            List<ReceiveFromTechnician> receiveFromTech = null;


            try
            {
                receiveFromTech = new List<ReceiveFromTechnician>();
                if (receiveFromTechnician != null)
                {

                    foreach (ReceiveFromTechnician R in receiveFromTechnician)
                    {
                        if (R.ID == null || R.ID == Guid.Empty)
                        {
                            if (R.MaterialID != null && R.MaterialID != Guid.Empty)
                            {
                                receiveFromTechnician = _iReceiveFromTechnicianRepository.InsertReceiveFromTechnician(R, empID, receiveDate, UA);
                                if (receiveFromTechnician != null)
                                {
                                    receiveFromTech.AddRange(receiveFromTechnician);
                                }
                            }

                        }
                        else
                        {
                            receiveFromTechnician = _iReceiveFromTechnicianRepository.UpdateReceiveFromTechnician(R, empID, receiveDate, UA);
                            if (receiveFromTechnician != null)
                            {
                                receiveFromTech.AddRange(receiveFromTechnician);
                            }

                        }


                    }
                }


            }
            catch (Exception)
            {

                throw;
            }

            return receiveFromTech;
        }

        public string DeleteReceiveFromTechnician(string ID, UA ua)
        {
            string status = null;
            try
            {
                status = _iReceiveFromTechnicianRepository.DeleteReceiveFromTechnician(ID, ua);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

    }
}