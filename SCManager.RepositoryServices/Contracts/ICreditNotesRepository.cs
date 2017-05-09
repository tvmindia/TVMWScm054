using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
   public interface ICreditNotesRepository
    {
        List<CreditNotes> GetAllCreditNotes(UA UA, string showAllYN);
        object InsertCreditNotes(CreditNotes creditNotesObj);
        object UpdateCreditNotes(CreditNotes creditNotesObj);
        List<CreditNotes> GetCreditNotesByID(UA UA, string ID);
        string DeleteCreditNote(string ID, UA ua);
        List<CreditNotes> GetCreditNotesBetweenDates(UA UA, string fromDate, string toDate);
    }
}
