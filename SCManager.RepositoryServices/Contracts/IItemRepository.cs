using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCManager.DataAccessObject.DTO;

namespace SCManager.RepositoryServices.Contracts
{
    public interface IItemRepository
    {
        List<Item> GetAllItems(UA UA);
    }
}
