using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class OfficeBillEntryBusiness : IOfficeBillEntryBusiness
    {
        private IOfficeBillEntryRepository _iOfficeBillEntryRepository;
        private ICommonBusiness _commonBusiness;
        public OfficeBillEntryBusiness(IOfficeBillEntryRepository iOfficeBillEntryRepository, ICommonBusiness commonBusiness)
        {
            _iOfficeBillEntryRepository = iOfficeBillEntryRepository;
            _commonBusiness = commonBusiness;
        }
        public List<OfficeBillEntry> GetAllOfficeBillEntry(UA UA)
        {
            List<OfficeBillEntry> OfficeBillEntryList = null;
            OfficeBillEntryList = _iOfficeBillEntryRepository.GetAllOfficeBillHeader(UA);
            if (OfficeBillEntryList != null)
            {
                foreach (OfficeBillEntry dd in OfficeBillEntryList)
                {
                    OfficeBillBL(dd.OfficeBillEntryDetail, dd);
                    OfficeBillEntryBusinessL(dd);
                    
                }
            }
            return OfficeBillEntryList;
        }
        private void OfficeBillEntryBusinessL(OfficeBillEntry I)
        {
            if (I != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
               
                if (I.Discount == null)
                {
                    I.Discount = 0;
                }
                I.GrandTotal = I.Subtotal + I.VATAmount - I.Discount;

                if (I.BillDate != null)
                    I.BillDateFormatted = I.BillDate;//.GetValueOrDefault().ToString(settings.dateformat);
            }

        }
        private void OfficeBillBL(List<OfficeBillEntryDetail> List, OfficeBillEntry I)
        {
            I.Subtotal = 0;
            if (List != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
                int slno = 1;
                foreach (OfficeBillEntryDetail F in List)
                {
                    F.SlNo = slno;
                    F.NetAmount = F.Quantity * F.Rate;
                    I.Subtotal = I.Subtotal + F.NetAmount;
                    //F.NetAmount = F.BasicAmount - F.TradeDiscount;
                    slno = slno + 1;
                }
            }



        }

        public OfficeBillEntry GetOfficeBillHeaderByID(Guid ID, UA ua)
        {
            try
            {
                OfficeBillEntry Result = new OfficeBillEntry();
                Result = _iOfficeBillEntryRepository.GetOfficeBillHeaderByID(ID, ua);
                Result.OfficeBillEntryDetail = _iOfficeBillEntryRepository.GetOfficeBillDetail(ID, ua);
                if (Result.OfficeBillEntryDetail != null)
                {

                    OfficeBillBL(Result.OfficeBillEntryDetail, Result);
                }
                OfficeBillEntryBusinessL(Result);
                //Form8DetailBL(Result.Form8Detail);
                
                return Result;
            }
            catch (Exception)
            {

                throw;
            }


        }
        public bool DeleteOfficeBillEntry(Guid ID, UA UA)
        {
            return _iOfficeBillEntryRepository.DeleteOfficeBillEntry(ID, UA);
        }
        public bool DeleteOfficeBillDetail(Guid ID, Guid HeaderID, UA UA)
        {
            return _iOfficeBillEntryRepository.DeleteOfficeBillDetail(ID, HeaderID, UA);
        }
        public OfficeBillEntry InsertUpdate(OfficeBillEntry officeBillEntry, UA UA)
        {
            OfficeBillEntry result = null;
            try
            {
                officeBillEntry.DetailXML = _commonBusiness.GetXMLfromOfficeObject(officeBillEntry.OfficeBillEntryDetail, "Material", UA);
                if (officeBillEntry.ID == null || officeBillEntry.ID == Guid.Empty)
                {
                    result = _iOfficeBillEntryRepository.InsertOfficeBillEntry(officeBillEntry, UA);
                }
                else
                {
                    result = _iOfficeBillEntryRepository.UpdateOfficeBillEntry(officeBillEntry, UA);

                }

                //--------BL works ----------------------
                if (result != null)
                {
                    OfficeBillEntryBusinessL(result);
                    OfficeBillBL(officeBillEntry.OfficeBillEntryDetail,officeBillEntry);
                }



            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

    }
}