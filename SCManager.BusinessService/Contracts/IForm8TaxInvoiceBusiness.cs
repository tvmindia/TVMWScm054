using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
    public interface IForm8TaxInvoiceBusiness
    {
        List<Form8> GetAllForm8(UA UA);
        Form8 InsertUpdate(Form8 frm8, UA UA);
    }
}
