using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCManager.DataAccessObject.DTO;

namespace SCManager.BusinessService.Contracts
{
    public interface IForm8BRetailInvoiceBusiness
    {
        List<Form8B> GetAllForm8B(UA UA);
        Form8B InsertUpdate(Form8B frm8, UA UA);
        bool DeleteForm8B(Guid ID, UA UA);
        bool DeleteForm8BDetail(Guid ID, Guid HeaderID, UA UA);
        Form8B GetForm8B(Guid ID, UA UA);
    }
}
