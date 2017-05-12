using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCManager.DataAccessObject.DTO;

namespace SCManager.RepositoryServices.Contracts
{
   public interface IOpeningSettingRepository
    {
        OpeningSetting InsertOpeningSetting(OpeningSetting opn, UA UA);
        OpeningSetting UpdateOpeningSetting(OpeningSetting opn, UA UA);
        OpeningSetting GetOpeningSettingHeader(UA UA);
        List<OpeningDetail> GetOpeningSettingDetail(UA UA);
        bool DeleteOpeningSettingDetail(Guid ID, UA UA);

    }
}
