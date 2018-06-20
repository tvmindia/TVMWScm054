using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class TaxBillEntryBusiness:ITaxBillEntryBusiness
    {
        private ITaxBillEntryRepository _iTaxBillEntryRepository;
        private ICommonBusiness _commonBusiness;
        public TaxBillEntryBusiness(ITaxBillEntryRepository iTaxBillEntryRepository, ICommonBusiness commonBusiness)
        {
            _iTaxBillEntryRepository = iTaxBillEntryRepository;
            _commonBusiness = commonBusiness;
        }

        public List<TaxBillEntry> GetAllTaxBillEntry(UA UA)
        {
            List<TaxBillEntry> TaxBillEntryList = null;          
            TaxBillEntryList = _iTaxBillEntryRepository.GetAllTaxBillEntry(UA);
            if (TaxBillEntryList != null)
            {
                foreach (TaxBillEntry dd in TaxBillEntryList)
                {
                    dd.TaxBillEntryDetail = null;
                    TaxBillEntryBusinessL(dd);
                    TaxBillBL(dd.TaxBillEntryDetail);
                }
            }
                return TaxBillEntryList;
        }

        private void TaxBillEntryBusinessL(TaxBillEntry T)
        {
            if (T != null)
            {

                SCManagerSettings settings = new SCManagerSettings();

                if (T.TaxBillEntryDetail != null)
                {
                    T.Subtotal = 0;
                    foreach (TaxBillEntryDetail F in T.TaxBillEntryDetail)
                    {
                        F.NetAmount = F.Quantity * F.Rate;
                        T.Subtotal = T.Subtotal + F.NetAmount;
                    }
                }
                //T.GrandTotal = T.Subtotal + T.VATAmount - T.Discount + T.ServiceCharge;
                T.GrandTotal = T.Subtotal + (T.CGSTAmount+T.SGSTAmount) - T.Discount + T.ServiceCharge;
                T.TotalTaxAmount = T.CGSTAmount + T.SGSTAmount;

                if (T.BillDate != null)
                    T.BillDateFormatted = T.BillDate;//.GetValueOrDefault().ToString(settings.dateformat);                
            }
        }

        private void TaxBillBL(List<TaxBillEntryDetail> List)
        {

            if (List != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
                int slno = 1;
                foreach (TaxBillEntryDetail F in List)
                {
                    F.SlNo = slno;
                    F.NetAmount = F.Quantity * F.Rate;

                    //F.NetAmount = F.BasicAmount - F.TradeDiscount;
                    slno = slno + 1;
                }
            }
        }



        public TaxBillEntry GetTaxBillHeaderByID(Guid ID, UA ua)
        {
            try
            {
                TaxBillEntry Result = new TaxBillEntry();
                Result = _iTaxBillEntryRepository.GetTaxBillHeaderByID(ID, ua);
                Result.TaxBillEntryDetail = _iTaxBillEntryRepository.GetTaxBillDetail(ID, ua);
                TaxBillEntryBusinessL(Result);
              
                if (Result.TaxBillEntryDetail != null)
                {
                    TaxBillBL(Result.TaxBillEntryDetail);
                }
                return Result;
            }
            catch (Exception)
            {

                throw;
            }


        }


        public TaxBillEntry UpdateTaxBill(TaxBillEntry taxBillEntry, UA UA)
        {
            TaxBillEntry result = null;
            try
            {
                taxBillEntry.DetailXML = _commonBusiness.GetXMLfromTaxObject(taxBillEntry.TaxBillEntryDetail, "MaterialID", UA);
                if (taxBillEntry.ID != null || taxBillEntry.ID != Guid.Empty)
                {
                    result = _iTaxBillEntryRepository.UpdateTaxBillEntry(taxBillEntry, UA);
                }
             

                //--------BL works ----------------------
                if (result != null)
                {
                    TaxBillBL(taxBillEntry.TaxBillEntryDetail);
                    TaxBillEntryBusinessL(result);

                }

            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }


        public TaxBillEntry GetTaxBill(Guid ID, UA ua)
        {
            try
            {
                TaxBillEntry Result = new TaxBillEntry();
                Result = _iTaxBillEntryRepository.GetTaxBillHeaderByID(ID, ua);
                Result.TaxBillEntryDetail = _iTaxBillEntryRepository.GetTaxBillDetail(ID, ua);               
                TaxBillBL(Result.TaxBillEntryDetail);
                TaxBillEntryBusinessL(Result);
                return Result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TaxBillEntry> GetAllFranchiseeDetail(UA UA)
        {
            List<TaxBillEntry> TaxBillEntrylist = null;
            try
            {
                TaxBillEntrylist = _iTaxBillEntryRepository.GetAllFranchiseeDetail(UA);               

            }
            catch (Exception)
            {

                throw;
            }
            return TaxBillEntrylist;
        }

    }
}