using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class ICRBillEntryBusiness : IICRBillEntryBusiness
    {
        private IICRBillEntryRepository _iICRBillEntryRepository;
        private ICommonBusiness _commonBusiness;

        public ICRBillEntryBusiness(IICRBillEntryRepository iICRBillEntryRepository, ICommonBusiness commonBusiness)
        {
            _iICRBillEntryRepository = iICRBillEntryRepository;
            _commonBusiness = commonBusiness;
        }

        public ICRBillEntry GetICRBillHeaderByID(Guid ID, UA ua)
        {
            try
            {
                ICRBillEntry Result = new ICRBillEntry();
                Result = _iICRBillEntryRepository.GetICRBillHeaderByID(ID, ua);
                Result.ICRBillEntryDetail = _iICRBillEntryRepository.GetICRBillDetail(ID, ua);
                ICRBillEntryBusinessL(Result);
                //Form8DetailBL(Result.Form8Detail);
                if (Result.ICRBillEntryDetail != null)
                {

                    ICRBillBL(Result.ICRBillEntryDetail);
                }
                return Result;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public ICRBillEntry InsertUpdate(ICRBillEntry iCRBillEntry, UA UA)
        {
            ICRBillEntry result = null;
            try
            {
                iCRBillEntry.DetailXML = _commonBusiness.GetXMLfromICRObject(iCRBillEntry.ICRBillEntryDetail, "Material", UA);
                if (iCRBillEntry.ID == null || iCRBillEntry.ID == Guid.Empty)
                {
                    result = _iICRBillEntryRepository.InsertICRBillEntry(iCRBillEntry, UA);
                }
                else
                {
                    result = _iICRBillEntryRepository.UpdateICRBillEntry(iCRBillEntry, UA);

                }

                //--------BL works ----------------------
                if (result != null)
                {
                    ICRBillEntryBusinessL(result);
                    ICRBillBL(iCRBillEntry.ICRBillEntryDetail);
                }



            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        public List<ICRBillEntry> GetAllICRBillEntry(UA UA)
        {
            List<ICRBillEntry> ICRBillEntryList = null;
            ICRBillEntryList = _iICRBillEntryRepository.GetAllICRBillEntry(UA);
            if (ICRBillEntryList != null)
            {
                foreach (ICRBillEntry dd in ICRBillEntryList)
                {

                    ICRBillEntryBusinessL(dd);
                    ICRBillBL(dd.ICRBillEntryDetail);
                }
            }
            return ICRBillEntryList;
        }
        private void ICRBillBL(List<ICRBillEntryDetail> List)
        {
            if (List != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
                int slno = 1;
                foreach (ICRBillEntryDetail F in List)
                {
                    F.SlNo = slno;
                    F.NetAmount = F.Quantity * F.Rate;

                    //F.NetAmount = F.BasicAmount - F.TradeDiscount;
                    slno = slno + 1;
                }
            }



        }
        public bool DeleteICRBillEntry(Guid ID, UA UA)
        {
            return _iICRBillEntryRepository.DeleteICRBillEntry(ID, UA);
        }
        public bool DeleteICRBillDetail(Guid ID, Guid HeaderID, UA UA)
        {
            return _iICRBillEntryRepository.DeleteICRBillDetail(ID, HeaderID, UA);
        }
        private void ICRBillEntryBusinessL(ICRBillEntry I)
        {
            if (I != null)
            {
                SCManagerSettings settings = new SCManagerSettings();

                I.Total = I.STAmount - I.Discount;

                if(I.TotalServiceTaxAmt==null)
                {
                    I.TotalServiceTaxAmt = 0;
                }
                if (I.Discount == null)
                {
                    I.Discount = 0;
                }
                I.GrandTotal = I.STAmount + I.TotalServiceTaxAmt - I.Discount;

                if (I.ICRDate != null)
                {
                    I.ICRDateFormatted = I.ICRDate;//.GetValueOrDefault().ToString(settings.dateformat);
                }
                if (I.AMCValidFromDate != null)
                {
                    I.AMCFromDateFormatted = I.AMCValidFromDate;//.GetValueOrDefault().ToString(settings.dateformat);
                }
                if (I.AMCValidToDate != null)
                {
                    I.AMCToDateFormatted = I.AMCValidToDate;//.GetValueOrDefault().ToString(settings.dateformat);
                }

            }

        }

        public List<ICRBillEntry> GetAllICRBillEntryForExport(UA UA)
        {
            List<ICRBillEntry> ICRBillEntryList = null;
            ICRBillEntryList = _iICRBillEntryRepository.GetAllICRBillEntryForExport(UA);            
            return ICRBillEntryList;
        }


    }
}