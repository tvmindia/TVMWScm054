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
    }
}
