using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
    public interface IForm8TaxInvoiceRepository
    {
        List<Form8> GetAllForm8(UA UA);
        Form8 InsertForm8(Form8 frm8, UA UA);
        Form8 UpdateForm8(Form8 frm8, UA UA);
        bool DeleteForm8(Guid ID, UA UA);
        bool DeleteForm8Detail(Guid ID, Guid HeaderID, UA UA);
        Form8 GetForm8Header(Guid ID, UA UA);
        List<Form8Detail> GetForm8Detail(Guid ID, UA UA);
    }
}
