using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
    public interface IAuthenticationBusiness
    {
        User CheckUserCredentials(User user);
        object UpdateUserProfile(UserProfile userProfile);
        List<User> GetAllUsersInSystem();
        List<ServiceCenter> GetAllServiceCenters();
        List<Role> GetAllRolesByServicecenter(string SCCode);
        User GetUserDetailsByUser(Guid ID,string SCCode);
        object InserUser(User user);
        object UpdateUser(User user);
        object DeleteUser(User user);
    }
}
