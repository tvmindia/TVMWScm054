using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCManager.DataAccessObject.DTO;

namespace SCManager.BusinessService.Contracts
{
    public interface IOpeningSettingBusiness
    {
        OpeningSetting InsertUpdate(OpeningSetting opn, UA UA);
        bool DeleteOpeningSettingDetail(Guid ID, UA UA);
        OpeningSetting GetOpeningSetting(UA ua);
    }
}
