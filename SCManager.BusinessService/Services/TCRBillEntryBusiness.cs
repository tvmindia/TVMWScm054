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
                decimal t1;
                decimal t2;
                decimal t3;
                decimal net;
                foreach (TCRBillEntryDetail F in List)
                {
                    F.SlNo = slno;
                    F.NetAmount = F.Quantity * F.Rate-F.TradeDiscount;
                     
                    //F.NetAmount = F.BasicAmount - F.TradeDiscount;
                  

                    F.CgstAmount = (((F.Quantity * F.Rate) - (F.TradeDiscount)) * (F.CgstPercentage / 100));
                    F.SgstAmount = (((F.Quantity * F.Rate) - (F.TradeDiscount)) * (F.SgstPercentage / 100));
                    t1 = (F.CgstAmount ?? 0);
                    t2 = F.SgstAmount ?? 0;
                    t3 = F.TotalTaxAmount ?? 0;
                    net = F.NetAmount ?? 0;
                    //F.TotalTaxAmount = F.TotalTaxAmount + t1 + t2;//.Total Tax Amount calculation
                    F.TotalTaxAmount = t1 + t2 + t3;//.Total Tax Amount calculation

                    F.GrandTotal = net + t1 + t2;//. Grand Total calculation

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
                    dd.TCRBillEntryDetail = null;
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
              
                SCManagerSettings settings = new SCManagerSettings();

                if(T.TCRBillEntryDetail!=null)
                {
                    T.Subtotal = 0;
                    T.TotalAmount = 0;
                    foreach (TCRBillEntryDetail F in T.TCRBillEntryDetail)
                    {
                        F.SubTotalAmount = F.Quantity * F.Rate;
                        F.NetAmount = F.Quantity * F.Rate-F.TradeDiscount;

                        T.Subtotal = T.Subtotal + F.SubTotalAmount;
                        T.TotalAmount = T.TotalAmount+ F.NetAmount;
                        
                    }
                }


                //T.GrandTotal = T.Subtotal + T.VATAmount - T.Discount+T.ServiceCharge;
                //T.GrandTotal = T.Subtotal + (T.CGSTAmount + T.SGSTAmount) - T.Discount + T.ServiceCharge;

                T.GrandTotal = T.TotalAmount + (T.CGSTAmount+T.SGSTAmount) - T.Discount + T.ServiceCharge;
                T.TotalTaxAmount = (T.CGSTAmount + T.SGSTAmount);
                
                if (T.BillDate != null)
                    T.BillDateFormatted = T.BillDate;//.GetValueOrDefault().ToString(settings.dateformat);                
            }

        }
    }
}