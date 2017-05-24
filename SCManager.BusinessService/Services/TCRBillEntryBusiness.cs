using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class TCRBillEntryBusiness : ITCRBillEntryBusiness
    {
        private ITCRBillEntryRepository _iTCRBillEntryRepository;
        private ICommonBusiness _commonBusiness;

        public TCRBillEntryBusiness(ITCRBillEntryRepository iTCRBillEntryRepository, ICommonBusiness commonBusiness)
        {
            _iTCRBillEntryRepository = iTCRBillEntryRepository;
            _commonBusiness = commonBusiness;
        }

        public bool DeleteTCRBillEntry(Guid ID, UA UA)
        {
            return _iTCRBillEntryRepository.DeleteTCRBillEntry(ID, UA);
        }

        public bool DeleteTCRBillDetail(Guid ID, Guid HeaderID, UA UA)
        {
            return _iTCRBillEntryRepository.DeleteTCRBillDetail(ID, HeaderID, UA);
        }

        public TCRBillEntry InsertUpdate(TCRBillEntry tCRBillEntry, UA UA)
        {
            TCRBillEntry result = null;
            try
            {
                tCRBillEntry.DetailXML = _commonBusiness.GetXMLfromTCRObject(tCRBillEntry.TCRBillEntryDetail, "MaterialID", UA);
                if (tCRBillEntry.ID == null || tCRBillEntry.ID == Guid.Empty)
                {
                    result = _iTCRBillEntryRepository.InsertTCRBillEntry(tCRBillEntry, UA);
                }
                else
                {
                    result = _iTCRBillEntryRepository.UpdateTCRBillEntry(tCRBillEntry, UA);

                }

                //--------BL works ----------------------
                if (result != null)
                {
                    TCRBillBL(tCRBillEntry.TCRBillEntryDetail);
                    TCRBillEntryBusinessL(result);
                   
                }



            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        private void TCRBillBL(List<TCRBillEntryDetail> List)
        {
           
            if (List != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
                int slno = 1;
                foreach (TCRBillEntryDetail F in List)
                {
                    F.SlNo = slno;
                    F.NetAmount = F.Quantity * F.Rate;
                     
                    //F.NetAmount = F.BasicAmount - F.TradeDiscount;
                    slno = slno + 1;
                }

            }



        }
        public List<TCRBillEntry> GetAllTCRBillEntry(UA UA)
        {
            List<TCRBillEntry> TCRBillEntryList = null;
            TCRBillEntryList = _iTCRBillEntryRepository.GetAllTCRBillEntry(UA);
            if (TCRBillEntryList != null)
            {
                foreach (TCRBillEntry dd in TCRBillEntryList)
                {

                    TCRBillEntryBusinessL(dd);
                    TCRBillBL(dd.TCRBillEntryDetail);
                }
            }
            return TCRBillEntryList;
        }

        public TCRBillEntry GetTCRBillHeaderByID(Guid ID, UA ua)
        {
            try
            {
                TCRBillEntry Result = new TCRBillEntry();
                Result = _iTCRBillEntryRepository.GetTCRBillHeaderByID(ID, ua);
                Result.TCRBillEntryDetail = _iTCRBillEntryRepository.GetTCRBillDetail(ID, ua);
                TCRBillEntryBusinessL(Result);
                //Form8DetailBL(Result.Form8Detail);
                if(Result.TCRBillEntryDetail!=null)
                {
                   
                    TCRBillBL(Result.TCRBillEntryDetail);
                }
                return Result;
            }
            catch (Exception)
            {

                throw;
            }


        }


        public List<TCRBillEntry> GetAllJobNo(UA UA)
        {
            List<TCRBillEntry> tCRBillEntryList = null;
            tCRBillEntryList = _iTCRBillEntryRepository.GetAllJobNo(UA);
            return tCRBillEntryList;
        }
        private void TCRBillEntryBusinessL(TCRBillEntry T)
        {
            if (T != null)
            {
                T.Subtotal = 0;
                SCManagerSettings settings = new SCManagerSettings();

                if(T.TCRBillEntryDetail!=null)
                {
                    foreach (TCRBillEntryDetail F in T.TCRBillEntryDetail)
                    {

                        F.NetAmount = F.Quantity * F.Rate;

                        T.Subtotal = T.Subtotal + F.NetAmount;

                    }
                }
                
                T.GrandTotal = T.Subtotal + T.VATAmount - T.Discount;

                if (T.BillDate != null)
                    T.BillDateFormatted = T.BillDate.GetValueOrDefault().ToString(settings.dateformat);                
            }

        }
    }
}