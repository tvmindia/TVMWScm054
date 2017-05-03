using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.RepositoryServices.Contracts;
using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;

namespace SCManager.BusinessService.Services
{
    public class LocalPurchaseBusiness:ILocalPurchaseBusiness
    {
        private ILocalPurchaseRepository _localPurchaseRepository;
        private ICommonBusiness _commonBusiness;

        public LocalPurchaseBusiness(ILocalPurchaseRepository localPurchaseRepository, ICommonBusiness commonBusiness)
        {
            _localPurchaseRepository = localPurchaseRepository;
            _commonBusiness = commonBusiness;
        }


        public List<LocalPurchase> GetAllLocalPurchase(UA UA)
        {
            List<LocalPurchase> LocalPurchaselist = null;
            try
            {

                LocalPurchaselist = _localPurchaseRepository.GetAllLocalPurchase(UA);
                if (LocalPurchaselist != null) {  
                foreach (LocalPurchase F in LocalPurchaselist)
                {

                    LocalPurchase_BL(F);
                }
            }
            }
            catch (Exception)
            {

                throw;
            }
            return LocalPurchaselist;
        }

        private void LocalPurchase_BL(LocalPurchase F)
        {
            if (F != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
                F.GrandTotal = F.Subtotal + F.VATAmount;
                if (F.InvoiceDate != null)
                    F.InvoiceDateFormatted = F.InvoiceDate.ToString(settings.dateformat);

            }

        }
        private void LocalPurchase_DetailBL(List<LocalPurchaseDetail> List)
        {
            if (List != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
                int slno = 1;
                foreach (LocalPurchaseDetail F in List)
                {
                    F.SlNo = slno;
                    F.BasicAmount = F.Quantity * F.Rate;
                    F.NetAmount = F.BasicAmount - F.TradeDiscount;
                    slno = slno + 1;
                }
            }
        }



        public LocalPurchase InsertUpdate(LocalPurchase LP, UA UA)
        {
            LocalPurchase result = null;
            try
            {
                LP.DetailXML = _commonBusiness.GetXMLfromLocalPurchaseDetail(LP.LocalPurchaseDetail, "MaterialID", UA);
                if (LP.ID == null || LP.ID == Guid.Empty)
                {
                    result = _localPurchaseRepository.InsertLocalPurchase(LP, UA);
                }
                else
                {
                    result = _localPurchaseRepository.UpdateLocalPurchase(LP, UA);

                }

                //--------BL works ----------------------
                if (result != null)
                {
                    LocalPurchase_BL(result);
                    LocalPurchase_DetailBL(result.LocalPurchaseDetail);
                }



            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }


        public bool DeleteLocalPurchaseDetail(Guid ID, Guid HeaderID, UA UA)
        {
            return _localPurchaseRepository.DeleteLocalPurchaseDetail(ID, HeaderID, UA);
        }

        public bool DeleteLocalPurchase(Guid ID, UA UA)
        {
            return _localPurchaseRepository.DeleteLocalPurchase(ID, UA);
        }

        public LocalPurchase GetLocalPurchase(Guid ID, UA ua)
        {
            try
            {
                LocalPurchase Result = new LocalPurchase();
                Result = _localPurchaseRepository.GetLocalPurchaseHeader(ID, ua);
                Result.LocalPurchaseDetail = _localPurchaseRepository.GetLocalPurchaseDetail(ID, ua);
                LocalPurchase_BL(Result);
                LocalPurchase_DetailBL(Result.LocalPurchaseDetail);
                return Result;
            }
            catch (Exception)
            {

                throw;
            }


        }

    }
}