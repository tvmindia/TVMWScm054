using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.DataAccessObject.DTO;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;

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

        #region Public Variables

        //---* Keys assosiated with mail sending.its values are set in web.config ,app settings section -- *//

        string EmailFromAddress = System.Web.Configuration.WebConfigurationManager.AppSettings["EmailFromAddress"];
        string host = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTP-host"];
        string smtpUserName = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTP-UserName"];
        string smtpPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTP-Password"];
        string emailVerificationCode = System.Web.Configuration.WebConfigurationManager.AppSettings["VerificationCode"];
        string port = System.Web.Configuration.WebConfigurationManager.AppSettings["Port"];

        #endregion   Public Variables

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
                                            Code= sdr["SCCode"].ToString() != "" ? sdr["SCCode"].ToString() : _userObj.serviceCenter.Code,
                                            Description= sdr["ServiceCenter"].ToString() != "" ? sdr["ServiceCenter"].ToString() : _userObj.serviceCenter.Description
                                        };
                                        _userObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _userObj.ID);
                                        _userObj.RoleList = (sdr["RoleList"].ToString() != "" ? sdr["RoleList"].ToString() : _userObj.RoleList);
                                        _userObj.UserName = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : _userObj.UserName);
                                        _userObj.LoginName = (sdr["LoginName"].ToString() != "" ? sdr["LoginName"].ToString() : _userObj.LoginName);
                                        _userObj.Password = (sdr["Password"].ToString() != "" ? sdr["Password"].ToString() : _userObj.Password);
                                        _userObj.Email = (sdr["Email"].ToString() != "" ? sdr["Email"].ToString() : _userObj.Email);
                                        _userObj.Active = (sdr["ActiveYN"].ToString() != "" ? bool.Parse(sdr["ActiveYN"].ToString()) : _userObj.Active);
                                        _userObj.VerificationCode= (sdr["VerificationCode"].ToString() != "" ? sdr["VerificationCode"].ToString() : _userObj.VerificationCode);
                                        _userObj.VerificationCodeDate = (sdr["VerifyCodeDate"].ToString() != "" ?DateTime.Parse(sdr["VerifyCodeDate"].ToString()) : _userObj.VerificationCodeDate);
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

        public object UpdateUserProfile(UserProfile userProfile)
        {
           SqlParameter outParameter= null;
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
                        cmd.CommandText = "[UpdateUserProfile]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = userProfile.SCCode;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = userProfile.ID;
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 250).Value = userProfile.UserName;
                        cmd.Parameters.Add("@NewPassword", SqlDbType.NVarChar, 250).Value = userProfile.NewPassword;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = userProfile.logDetails.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = userProfile.logDetails.UpdatedDate;
                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                      
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        return outParameter.Value.ToString();
    }


        public object InserUser(User user)
        {
            SqlParameter outParameter = null,outParameter1=null;
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
                        cmd.CommandText = "[InsertUser]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = user.SCCode;
                        cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 250).Value = user.UserName;
                        cmd.Parameters.Add("@RoleList", SqlDbType.NVarChar, 250).Value = user.RoleList;
                        cmd.Parameters.Add("@LoginName", SqlDbType.NVarChar, 250).Value = user.LoginName;
                        cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 250).Value = user.Password;
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar,-1).Value = user.Email;
                        cmd.Parameters.Add("@Active", SqlDbType.Bit).Value = user.Active;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = user.logDetails.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = user.logDetails.CreatedDate;
                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        outParameter1 = cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier);
                        outParameter1.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            switch (outParameter.Value.ToString())
            {
                case "1":
                    return new
                    {
                        userID = Guid.Parse(outParameter1.Value.ToString()),
                        Status = constObj.InsertSuccess
                    };

                case "2":
                    return new
                    {

                        Status = constObj.LoginAndEmailExist
                    };

                default:
                    return new
                    {

                        Status = constObj.InsertFailure
                    };

            }
        
           
           
        }

        public object UpdateUser(User user)
        {
            SqlParameter outParameter = null;
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
                        cmd.CommandText = "[UpdateUser]"; 
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier).Value = user.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = user.SCCode;
                        cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 250).Value = user.UserName;
                        cmd.Parameters.Add("@RoleList", SqlDbType.NVarChar, 250).Value = user.RoleList;
                        cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 250).Value = user.Password;
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, -1).Value = user.Email;
                        cmd.Parameters.Add("@Active", SqlDbType.Bit).Value = user.Active;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = user.logDetails.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = user.logDetails.UpdatedDate;
                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            switch (outParameter.Value.ToString())
            {
                case "1":
                    return new
                    {
                       
                        Status = constObj.UpdateSuccess
                    };

                case "2":
                    return new
                    {

                        Status = constObj.LoginAndEmailExist
                    };

                default:
                    return new
                    {

                        Status = constObj.UpdateFailure
                    };

            }


        }

        public object DeleteUser(User user)
        {
            SqlParameter outParameter = null;
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
                        cmd.CommandText = "[DeleteUser]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier).Value = user.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = user.SCCode;
                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if(outParameter.Value.ToString()=="1")
            {
                return new
                {

                    Status = constObj.DeleteSuccess
                };
            }
            else
            {
                return new
                {

                    Status = constObj.DeleteFailure
                };
            }
           

        }

        public object EmailValidation(string emailID)
        {
            SqlParameter outParameter = null;
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
                        cmd.CommandText = "[EmailValidation]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar,100).Value = emailID;
                       // cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
                       
                return new
                {

                    Status = outParameter.Value.ToString()
                };                     


        }
        public List<ServiceCenter> GetAllServiceCenters()
        {
            List<ServiceCenter> serviceCenterList = null;
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
                        cmd.CommandText = "[GetAllServiceCenters]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                serviceCenterList = new List<ServiceCenter>();
                                while (sdr.Read())
                                {
                                    ServiceCenter _serviceCenterObj = new ServiceCenter();
                                    {

                                        _serviceCenterObj.Code = sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : _serviceCenterObj.Code;
                                        _serviceCenterObj.Description = sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : _serviceCenterObj.Description;
                               
                                    }
                                    serviceCenterList.Add(_serviceCenterObj);
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

            return serviceCenterList;


        }

        public List<Role> GetAllRolesByServicecenter(string SCCode)
        {
            List<Role> RoleList = null;
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
                        cmd.CommandText = "[GetAllRolesByServicecenter]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = SCCode;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                RoleList = new List<Role>();
                                while (sdr.Read())
                                {
                                    Role _role = new Role();
                                    {
                                     _role.ID = sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _role.ID;
                                    _role.RoleName = sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : _role.RoleName;
                                    }
                                    RoleList.Add(_role);
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

            return RoleList;
        }

        public object AddVerificationCode(User user)
        {
            SqlParameter outParameter = null;
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
                        cmd.CommandText = "[AddVerificationCode]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = user.SCCode;                       
                        cmd.Parameters.Add("@LoginName", SqlDbType.NVarChar, 255).Value = user.LoginName;
                        cmd.Parameters.Add("@VerificationCode", SqlDbType.NVarChar, 20).Value = user.VerificationCode;
                        cmd.Parameters.Add("@VerifyCodeDate", SqlDbType.DateTime).Value = user.VerificationCodeDate;                        
                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;                        
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
                    return new
                    {

                        Status = outParameter.Value.ToString()
                    };
            
        }

        public object ResetPassword(User user)
        {
            SqlParameter outParameter = null;
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
                        cmd.CommandText = "[ResetPassword]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 250).Value = user.Password;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = user.ID;                        
                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new
            {

                Status = outParameter.Value.ToString()
            };

        }

        #region SendEmail

        public string SendEmail(string status, string msg, string Email, string verificationCode)
        {            
            try
            {
                MailMessage Msg = new MailMessage();

                Msg.From = new MailAddress(EmailFromAddress);

                Msg.To.Add(Email);

                string message = "<body><h3>Hello ,</h3>" + msg + "<p>Enter Your Code in given field and change your Password<p><p><p><p>&nbsp;&nbsp;&nbsp;&nbsp; SCM&nbsp; Admin<p><p><p><p><p>Please do not reply to this email with your password. We will never ask for your password, and we strongly discourage you from sharing it with anyone.</body>";
                Msg.Subject = verificationCode;
                Msg.Body = message;
                Msg.IsBodyHtml = true;

                // your remote SMTP server IP.
                SmtpClient smtp = new SmtpClient();
                smtp.Host = host;
                smtp.Port = 587;
                //smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword);
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                smtp.Send(Msg);
                Msg = null;
                status = "1";
            }
            catch (Exception ex)
            {
                status = "500";//Exception of foreign key
              
            }
            return status;
        }


        #endregion SendEmail
    }
}