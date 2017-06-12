using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class ReceiveFromOtherSCBusiness : IReceiveFromOtherSCBusiness
    {
        private IReceiveFromOtherSCRepository _iReceiveFromOtherSCRepository;
        private ICommonBusiness _commonBusiness;

        public ReceiveFromOtherSCBusiness(IReceiveFromOtherSCRepository iReceiveFromOtherSCRepository, ICommonBusiness commonBusiness)
        {
            _iReceiveFromOtherSCRepository = iReceiveFromOtherSCRepository;
            _commonBusiness = commonBusiness;
        }

        public bool DeleteOtherSCReceipt(Guid ID, UA UA)
        {
            return _iReceiveFromOtherSCRepository.DeleteOtherSCReceipt(ID, UA);
        }

        public bool DeleteOtherScReceiptDetail(Guid ID, Guid HeaderID, UA UA)
        {
            return _iReceiveFromOtherSCRepository.DeleteOtherScReceiptDetail(ID, HeaderID, UA);
        }

        public ReceiveFromOtherSC InsertUpdate(ReceiveFromOtherSC receiveFromOtherSC, UA UA)
        {
            ReceiveFromOtherSC result = null;
            try
            {
                receiveFromOtherSC.DetailXML = _commonBusiness.GetXMLfromOtherSCReceiptObject(receiveFromOtherSC.ReceiveFromOtherScDetail, "MaterialID", UA);
                if (receiveFromOtherSC.ID == null || receiveFromOtherSC.ID == Guid.Empty)
                {
                    result = _iReceiveFromOtherSCRepository.InsertOtherSCReceipt(receiveFromOtherSC, UA);
                }
                else
                {
                    result = _iReceiveFromOtherSCRepository.UpdateOtherSCReceipt(receiveFromOtherSC, UA);

                }

                //--------BL works ----------------------
                if (result != null)
                {
                    ReceiptOtherSCBL(receiveFromOtherSC.ReceiveFromOtherScDetail);
                    ReceiptOtherSCBusinessL(result);

                }



            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public List<ReceiveFromOtherSC> GetAllOtherSCReceipt(UA UA)
        {
            List<ReceiveFromOtherSC> ReceiveFromOtherSCList = null;
            ReceiveFromOtherSCList = _iReceiveFromOtherSCRepository.GetAllOtherSCReceipt(UA);
            if (ReceiveFromOtherSCList != null)
            {
                foreach (ReceiveFromOtherSC dd in ReceiveFromOtherSCList)
                {
                    dd.ReceiveFromOtherScDetail = null;
                    ReceiptOtherSCBusinessL(dd);
                    ReceiptOtherSCBL(dd.ReceiveFromOtherScDetail);
                }
            }
            return ReceiveFromOtherSCList;
        }

        public ReceiveFromOtherSC GetOtherSCReceiptByID(Guid ID, UA ua)
        {
            try
            {
                ReceiveFromOtherSC Result = new ReceiveFromOtherSC();
                Result = _iReceiveFromOtherSCRepository.GetOtherSCReceiptByID(ID, ua);
                Result.ReceiveFromOtherScDetail = _iReceiveFromOtherSCRepository.GetOtherScReceiptDetail(ID, ua);
                ReceiptOtherSCBusinessL(Result);              
                if (Result.ReceiveFromOtherScDetail != null)
                {

                    ReceiptOtherSCBL(Result.ReceiveFromOtherScDetail);
                }
                return Result;
            }
            catch (Exception)
            {

                throw;
            }


        }

        private void ReceiptOtherSCBL(List<ReceiveFromOtherScDetail> List)
        {

            if (List != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
                int slno = 1;
                foreach (ReceiveFromOtherScDetail F in List)
                {
                    F.SlNo = slno;
                    F.BasicAmount = F.Quantity * F.Rate;
                   // F.NetAmount = F.BasicAmount - F.TradeDiscount;

                    //F.NetAmount = F.BasicAmount - F.TradeDiscount;
                    slno = slno + 1;
                }

            }



        }

        private void ReceiptOtherSCBusinessL(ReceiveFromOtherSC T)
        {
            if (T != null)
            {

                SCManagerSettings settings = new SCManagerSettings();

                if (T.ReceiveFromOtherScDetail != null)
                {
                    T.Subtotal = 0;
                    foreach (ReceiveFromOtherScDetail F in T.ReceiveFromOtherScDetail)
                    {

                        F.BasicAmount = F.Quantity * F.Rate;
                        F.NetAmount = F.BasicAmount - F.TradeDiscount;

                        T.Subtotal = T.Subtotal + F.NetAmount;

                    }
                }

                T.GrandTotal = T.Subtotal + T.VATAmount;

                if (T.InvoiceDate != null)
                    T.InvoiceDateFormatted = T.InvoiceDate;//.GetValueOrDefault().ToString(settings.dateformat);
            }

        }

    }
}