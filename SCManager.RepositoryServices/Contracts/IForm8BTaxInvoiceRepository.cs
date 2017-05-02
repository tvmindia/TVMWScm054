using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCManager.DataAccessObject.DTO;

namespace SCManager.RepositoryServices.Contracts
{
   public interface IForm8BTaxInvoiceRepository
    {
        List<Form8B> GetAllForm8B(UA UA);
    }
}
