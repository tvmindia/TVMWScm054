using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
   public interface ICategoriesBusiness
    {
        List<Categories> GetAllCategories(UA UA);
    }
}
