using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.RepositoryServices.Contracts;
using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
namespace SCManager.BusinessService.Services
{
    public class OpeningSettingBusiness:IOpeningSettingBusiness
    {
        private IOpeningSettingRepository _OpeningSettingRepository;
        private ICommonBusiness _commonBusiness;

        public OpeningSettingBusiness(IOpeningSettingRepository OpeningSettingRepository, ICommonBusiness commonBusiness)
        {
            _OpeningSettingRepository = OpeningSettingRepository;
            _commonBusiness = commonBusiness;
        }
             

        public OpeningSetting InsertUpdate(OpeningSetting opn, UA UA)
        {
            OpeningSetting result = null;
            try
            {
                opn.DetailXML = _commonBusiness.GetXMLfromOpenDetail(opn.OpeningDetails, "MaterialID", UA);
                if (opn.ID == null || opn.ID == Guid.Empty)
                {
                    result = _OpeningSettingRepository.InsertOpeningSetting(opn, UA);
                }
                else
                {
                    result = _OpeningSettingRepository.UpdateOpeningSetting(opn, UA);

                }

                //--------BL works ----------------------
                if (result != null)
                {
                    OpeningSettingBL(result);
                    OpeningSettingDetailBL(result.OpeningDetails);
                }



            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        private void OpeningSettingBL(OpeningSetting F)
        {
            
        }

        private void OpeningSettingDetailBL(List<OpeningDetail> List)
        {
            if (List != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
                int slno = 1;
                foreach (OpeningDetail F in List)
                {
                    F.SlNo = slno;                   
                    slno = slno + 1;
                }
            }



        }

        public bool DeleteOpeningSettingDetail(Guid ID, UA UA)
        {
            return _OpeningSettingRepository.DeleteOpeningSettingDetail(ID,  UA);
        }     

        public OpeningSetting GetOpeningSetting(  UA ua)
        {
            try
            {
                OpeningSetting Result = new OpeningSetting();
                Result = _OpeningSettingRepository.GetOpeningSettingHeader(ua);
                Result.OpeningDetails = _OpeningSettingRepository.GetOpeningSettingDetail(  ua);
                OpeningSettingBL(Result);
                OpeningSettingDetailBL(Result.OpeningDetails);
                return Result;
            }
            catch (Exception)
            {

                throw;
            }


        }

    }
}