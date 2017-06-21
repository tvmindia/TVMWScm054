using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class CreditNotesBusiness : ICreditNotesBusiness
    {
        private ICreditNotesRepository _iCreditNotesRepository;

        public CreditNotesBusiness(ICreditNotesRepository iCreditNotesRepository)
        {
            _iCreditNotesRepository = iCreditNotesRepository;
        }
        public List<CreditNotes> GetAllCreditNotes(UA UA, string showAllYN)
        {
            List<CreditNotes> CreditNoteslist = null;
            CreditNoteslist = _iCreditNotesRepository.GetAllCreditNotes(UA,showAllYN);
            if (CreditNoteslist != null)
            {
                foreach (CreditNotes cn in CreditNoteslist)
                {

                    DefectiveDamage_DF(cn);
                }
            }
            return CreditNoteslist;

        }
        public List<CreditNotes> GetCreditNotesBetweenDates(UA UA, string fromDate, string toDate)
        {
            List<CreditNotes> CreditNoteslist = null;
            CreditNoteslist = _iCreditNotesRepository.GetCreditNotesBetweenDates(UA, fromDate,toDate);
            if (CreditNoteslist != null)
            {
                foreach (CreditNotes cn in CreditNoteslist)
                {

                    DefectiveDamage_DF(cn);
                }
            }
            return CreditNoteslist;

        }
        private void DefectiveDamage_DF(CreditNotes cn)
        {
            if (cn != null)
            {
                SCManagerSettings settings = new SCManagerSettings();

                if (cn.Date != null)
                    cn.DateFormatted = cn.Date;//.GetValueOrDefault().ToString(settings.dateformat);
            }

        }
        public List<CreditNotes> GetCreditNotesByID(UA UA, string ID)
        {
            List<CreditNotes> creditNotesList = null;
            creditNotesList = _iCreditNotesRepository.GetCreditNotesByID(UA, ID);
            return creditNotesList;
        }
        public string DeleteCreditNote(string ID, UA ua)
        {
            string status = null;
            try
            {
                status = _iCreditNotesRepository.DeleteCreditNote(ID, ua);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public object InsertUpdateCreditNotes(CreditNotes creditNotesObj)
        {
            object result = null;
            try
            {
                if (creditNotesObj.ID == Guid.Empty)
                {
                    result = _iCreditNotesRepository.InsertCreditNotes(creditNotesObj);
                }
                else
                {
                    //creditNotesObj.CreditNoteNo = creditNotesObj.HiddenCreditNoteNo;
                    result = _iCreditNotesRepository.UpdateCreditNotes(creditNotesObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}