using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.DataAccessObject.DTO;
using System.Data.SqlClient;
using System.Data;

namespace SCManager.RepositoryServices.Services
{
    
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private IDatabaseFactory _databaseFactory;
        private Const constObj = new Const();
        public AuthenticationRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public List<User> GetAllUsers()
        {
            List<User> userList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[GetAllUsers]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                userList = new List<User>();
                                while (sdr.Read())
                                {
                                    User _userObj = new User();
                                    {
                                        _userObj.serviceCenter = new ServiceCenter()
                                        {
                                            Code= sdr["SCCode"].ToString() != "" ? sdr["SCCode"].ToString() : _userObj.serviceCenter.Code
                                        };
                                        _userObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _userObj.ID);
                                        _userObj.RoleList = (sdr["RoleList"].ToString() != "" ? sdr["RoleList"].ToString() : _userObj.RoleList);
                                        _userObj.UserName = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : _userObj.UserName);
                                        _userObj.LoginName = (sdr["LoginName"].ToString() != "" ? sdr["LoginName"].ToString() : _userObj.LoginName);
                                        _userObj.Password = (sdr["Password"].ToString() != "" ? sdr["Password"].ToString() : _userObj.Password);
                                        if (!string.IsNullOrEmpty(_userObj.RoleList))
                                        {
                                            _userObj.Roles = _userObj.RoleList.Split(',').Select(t => t.Trim()).ToArray();
                                        }
                                       
                                    }
                                    userList.Add(_userObj);
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return userList;


        }
    }
}