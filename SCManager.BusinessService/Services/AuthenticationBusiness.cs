﻿using SCManager.BusinessService.Contracts;
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
                userList = userList == null ? null : userList.Where(us => us.LoginName.ToLower() == user.LoginName.ToLower() && Decrypt(us.Password) == user.Password).ToList();
                _user = (userList == null)||(userList.Count==0) ? null : userList[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _user;

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
            }
            return plainText;
        }

    }
}