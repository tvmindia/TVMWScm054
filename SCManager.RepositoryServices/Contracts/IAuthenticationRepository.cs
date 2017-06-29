using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
    public interface IAuthenticationRepository
    {
        List<User> GetAllUsers();
        object UpdateUserProfile(UserProfile userProfile);
        List<ServiceCenter> GetAllServiceCenters();
        List<Role> GetAllRolesByServicecenter(string SCCode);
        object InserUser(User user);
        object UpdateUser(User user);
        object DeleteUser(User user);
        object EmailValidation(string emailID);
        object AddVerificationCode(User user);
        string SendEmail(string status,string message,string emailID,string verificationCode);
        object ResetPassword(User user);
    }
}
