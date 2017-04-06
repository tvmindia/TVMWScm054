using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCManager.DataAccessObject.DTO;

namespace SCManager.BusinessService.Contracts
{
    public interface ITechnicianBusiness
    {
        List<TechnicianSummary> GetTechnicianSummary(UA ua);
    }
}
