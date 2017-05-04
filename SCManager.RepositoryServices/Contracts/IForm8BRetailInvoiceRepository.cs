using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCManager.DataAccessObject.DTO;

namespace SCManager.RepositoryServices.Contracts
{
   public interface IForm8BRetailInvoiceRepository
    {
        List<Form8B> GetAllForm8B(UA UA);
        Form8B InsertForm8B(Form8B frm8, UA UA);
        Form8B UpdateForm8B(Form8B frm8, UA UA);
        bool DeleteForm8B(Guid ID, UA UA);
        bool DeleteForm8BDetail(Guid ID, Guid HeaderID, UA UA);
        Form8B GetForm8BHeader(Guid ID, UA UA);
        List<Form8BDetail> GetForm8BDetail(Guid ID, UA UA);
    }
}
