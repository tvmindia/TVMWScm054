using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class IssueToOtherSCBusiness : IIssueToOtherSCBusiness
    {
        private IIssueToOtherSCRepository _iIssueToOtherSCRepository;
        private ICommonBusiness _commonBusiness;

        public IssueToOtherSCBusiness(IIssueToOtherSCRepository iIssueToOtherSCRepository, ICommonBusiness commonBusiness)
        {
            _iIssueToOtherSCRepository = iIssueToOtherSCRepository;
            _commonBusiness = commonBusiness;
        }

        public bool DeleteIssueToOtherSC(Guid ID, UA UA)
        {
            return _iIssueToOtherSCRepository.DeleteIssueToOtherSC(ID, UA);
        }

        public bool DeleteIssueToOtherSCDetail(Guid ID, Guid HeaderID, UA UA)
        {
            return _iIssueToOtherSCRepository.DeleteIssueToOtherSCDetail(ID, HeaderID, UA);
        }

        public IssueToOtherSC InsertUpdate(IssueToOtherSC issueToOtherSC, UA UA)
        {
            IssueToOtherSC result = null;
            try
            {
                issueToOtherSC.DetailXML = _commonBusiness.GetXMLfromIssueToOtherSCObject(issueToOtherSC.IssueToOtherScDetail, "MaterialID", UA);
                if (issueToOtherSC.ID == null || issueToOtherSC.ID == Guid.Empty)
                {
                    result = _iIssueToOtherSCRepository.InsertIssueToOtherSC(issueToOtherSC, UA);
                }
                else
                {
                    result = _iIssueToOtherSCRepository.UpdateIssueToOtherSC(issueToOtherSC, UA);

                }

                //--------BL works ----------------------
                if (result != null)
                {
                    ReceiptOtherSCBL(issueToOtherSC.IssueToOtherScDetail);
                    ReceiptOtherSCBusinessL(result);

                }



            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public List<IssueToOtherSC> GetAllIssueToOtherSC(UA UA)
        {
            List<IssueToOtherSC> IssueToOtherSCList = null;
            IssueToOtherSCList = _iIssueToOtherSCRepository.GetAllIssueToOtherSC(UA);
            if (IssueToOtherSCList != null)
            {
                foreach (IssueToOtherSC dd in IssueToOtherSCList)
                {
                    dd.IssueToOtherScDetail = null;
                    ReceiptOtherSCBusinessL(dd);
                    ReceiptOtherSCBL(dd.IssueToOtherScDetail);
                }
            }
            return IssueToOtherSCList;
        }

        public IssueToOtherSC GetIssueToOtherSCByID(Guid ID, UA ua)
        {
            try
            {
                IssueToOtherSC Result = new IssueToOtherSC();
                Result = _iIssueToOtherSCRepository.GetIssueToOtherSCByID(ID, ua);
                Result.IssueToOtherScDetail = _iIssueToOtherSCRepository.GetIssueToOtherScDetail(ID, ua);
                ReceiptOtherSCBusinessL(Result);
                if (Result.IssueToOtherScDetail != null)
                {

                    ReceiptOtherSCBL(Result.IssueToOtherScDetail);
                }
                return Result;
            }
            catch (Exception)
            {

                throw;
            }


        }

        private void ReceiptOtherSCBL(List<IssueToOtherScDetail> List)
        {

            if (List != null)
            {
                SCManagerSettings settings = new SCManagerSettings();
                int slno = 1;
                foreach (IssueToOtherScDetail F in List)
                {
                    F.SlNo = slno;
                    F.BasicAmount = F.Quantity * F.Rate;
                    // F.NetAmount = F.BasicAmount - F.TradeDiscount;

                    //F.NetAmount = F.BasicAmount - F.TradeDiscount;
                    slno = slno + 1;
                }

            }



        }

        private void ReceiptOtherSCBusinessL(IssueToOtherSC T)
        {
            if (T != null)
            {

                SCManagerSettings settings = new SCManagerSettings();

                if (T.IssueToOtherScDetail != null)
                {
                    T.Subtotal = 0;
                    foreach (IssueToOtherScDetail F in T.IssueToOtherScDetail)
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