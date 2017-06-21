using SCManager.BusinessService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System.Text;
using System.Security.Cryptography;

namespace SCManager.BusinessService.Services
{
    public class AuthenticationBusiness : IAuthenticationBusiness
    {
        string key = System.Web.Configuration.WebConfigurationManager.AppSettings["cryptography"];
        private IAuthenticationRepository _authenticationRepository;
        private Const constObj = new Const();
        public AuthenticationBusiness(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }
        public User CheckUserCredentials(User user)
        {
            User _user = null;
            List<User> userList = null;

            try
            {
                userList = _authenticationRepository.GetAllUsers();
                userList = userList == null ? null : userList.Where(us => us.LoginName.ToLower() == user.LoginName.ToLower() && Decrypt(us.Password) == user.Password && us.Active).ToList();
                _user = (userList == null)||(userList.Count==0) ? null : userList[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _user;

        }

        public List<User> GetAllUsersInSystem()
        {
            List<User> userList = null;
            try
            {
                userList = _authenticationRepository.GetAllUsers();
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return userList;
        }

        private string Encrypt(string plainText)
        {
            //AES 128bit Cross Platform (Java and C#) Encryption Compatibility

            string encryptedText = "";
            try
            {

                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                var keyBytes = new byte[16];
                var secretKeyBytes = Encoding.UTF8.GetBytes(key);
                Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
                encryptedText = Convert.ToBase64String(new RijndaelManaged
                {
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7,
                    KeySize = 128,
                    BlockSize = 128,
                    Key = keyBytes,
                    IV = keyBytes
                }.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return encryptedText;
        }

        private string Decrypt(string encryptedText)
        {
            string plainText = "";
            try
            {
                var encryptedBytes = Convert.FromBase64String(encryptedText);
                var keyBytes = new byte[16];
                var secretKeyBytes = Encoding.UTF8.GetBytes(key);
                Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
                plainText = Encoding.UTF8.GetString(
                    new RijndaelManaged
                    {
                        Mode = CipherMode.CBC,
                        Padding = PaddingMode.PKCS7,
                        KeySize = 128,
                        BlockSize = 128,
                        Key = keyBytes,
                        IV = keyBytes
                    }.CreateDecryptor().TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return plainText;
        }

        public object UpdateUserProfile(UserProfile userProfile)
        {
            object result = null;
            List<User> userList = null;
            try
            {

                if(string.IsNullOrEmpty(userProfile.CurrentPassword))
                {
                    result = _authenticationRepository.UpdateUserProfile(userProfile);
                }
                else
                {
                    userList = _authenticationRepository.GetAllUsers();
                    userList = userList == null ? null : userList.Where(us => us.ID == userProfile.ID && us.serviceCenter.Code == userProfile.SCCode && Decrypt(us.Password) == userProfile.CurrentPassword).ToList();
                    if (userList != null && userList.Count > 0)
                    {
                        userProfile.NewPassword = Encrypt(userProfile.NewPassword);
                        result = _authenticationRepository.UpdateUserProfile(userProfile);
                    }
                    else
                    {
                       //not exist or wrong password
                       return "2";
                       
                    }

                }
                
              
              
               
            }
            catch(Exception ex)
            {

            }
            return result;
        }

        public List<ServiceCenter> GetAllServiceCenters()
        {
            List<ServiceCenter> serviceCenterList = null;
            try
            {
                serviceCenterList=_authenticationRepository.GetAllServiceCenters();
                serviceCenterList = serviceCenterList != null ? serviceCenterList.OrderBy(s => s.Code).ToList():null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return serviceCenterList;

        }

        public List<Role> GetAllRolesByServicecenter(string SCCode)
        {
            List<Role> RoleList = null;
            try
            {
                RoleList=_authenticationRepository.GetAllRolesByServicecenter(SCCode);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return RoleList;
        }

        public User GetUserDetailsByUser(Guid ID, string SCCode)
        {
            List<User> userList = null;
            try
            {
                userList = GetAllUsersInSystem();
                userList = userList != null && userList.Count > 0 ? userList.Where(u => u.ID == ID && u.serviceCenter.Code == SCCode).ToList() : null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return userList != null && userList.Count > 0 ? userList[0] : null;
        }

        public object InserUser(User user)
        {
            object result = null;
            try
            {
                if (!string.IsNullOrEmpty(user.Password))
                {
                    user.Password= Encrypt(user.Password);
                }
                result = _authenticationRepository.InserUser(user);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public object UpdateUser(User user)
        {
            object result = null;
            try
            {
                if(!string.IsNullOrEmpty(user.Password))
                {
                    user.Password = Encrypt(user.Password);
                }
              
                result = _authenticationRepository.UpdateUser(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public object DeleteUser(User user)
        {
            object result = null;
            try
            {
                

                result = _authenticationRepository.DeleteUser(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}