using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SCManager.RepositoryServices.Services
{
    public class EmployeesRepository : IEmployeesRepository
    {
        Const c = new Const();
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public EmployeesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region Methods

        #region GetAllEmployees
        public List<Employees> GetAllEmployees(UA UA,string filter)
        {
            List<Employees> Emloyeeslist = null;
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
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@Filter", SqlDbType.NVarChar, 10).Value = filter;
                        cmd.CommandText = "[GetAllEmployees]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Emloyeeslist = new List<Employees>();
                                while (sdr.Read())
                                {
                                    Employees EmloyeesObj = new Employees();
                                    {
                                        EmloyeesObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : EmloyeesObj.ID);
                                        EmloyeesObj.Name = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : EmloyeesObj.Name);
                                        EmloyeesObj.Type = (sdr["Type"].ToString() != "" ? (sdr["Type"].ToString()) : EmloyeesObj.Type);
                                        EmloyeesObj.MobileNo = (sdr["MobileNo"].ToString() != "" ? (sdr["MobileNo"].ToString()) : EmloyeesObj.MobileNo);
                                        EmloyeesObj.Address = (sdr["Address"].ToString() != "" ? (sdr["Address"].ToString()) : EmloyeesObj.Address);
                                        EmloyeesObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : EmloyeesObj.Remarks); 
                                        EmloyeesObj.IsActive = (sdr["IsActive"].ToString() != "" ? bool.Parse(sdr["IsActive"].ToString()) : EmloyeesObj.IsActive);
                                    }

                                    Emloyeeslist.Add(EmloyeesObj);
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
            return Emloyeeslist;
        }
        #endregion  GetAllEmployees

        #region GetEmployeeByID
        public List<Employees> GetEmployeeByID(UA UA,string ID)
        {
            List<Employees> Emloyeeslist = null;
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
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                        cmd.CommandText = "[GetEmployeeByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Emloyeeslist = new List<Employees>();
                                while (sdr.Read())
                                {
                                    Employees EmployeesObj = new Employees();
                                    {
                                        EmployeesObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : EmployeesObj.ID);
                                        EmployeesObj.Name = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : EmployeesObj.Name);
                                        EmployeesObj.Type = (sdr["Type"].ToString() != "" ? (sdr["Type"].ToString()) : EmployeesObj.Type);
                                        EmployeesObj.MobileNo = (sdr["MobileNo"].ToString() != "" ? (sdr["MobileNo"].ToString()) : EmployeesObj.MobileNo);
                                        EmployeesObj.Address = (sdr["Address"].ToString() != "" ? (sdr["Address"].ToString()) : EmployeesObj.Address);
                                        EmployeesObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : EmployeesObj.Remarks);
                                        EmployeesObj.IsActive = (sdr["IsActive"].ToString() != "" ? bool.Parse(sdr["IsActive"].ToString()) : EmployeesObj.IsActive);
                                    }

                                    Emloyeeslist.Add(EmployeesObj);
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
            return Emloyeeslist;
        }
        #endregion GetEmployeeByID

        #region InsertEmployee
        public object InsertEmployee(Employees employeesObj)
        {
            SqlParameter outParameter, outParameter1 = null;
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
                        cmd.CommandText = "[InsertEmployee]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = employeesObj.SCCode;
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 250).Value = employeesObj.Name;
                        cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 50).Value = employeesObj.Type;
                        cmd.Parameters.Add("@MobileNo", SqlDbType.NVarChar, 50).Value = employeesObj.MobileNo;
                        cmd.Parameters.Add("@Address", SqlDbType.NVarChar, -1).Value = employeesObj.Address;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = employeesObj.Remarks;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = employeesObj.logDetails.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.SmallDateTime).Value = employeesObj.logDetails.CreatedDate;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = employeesObj.IsActive;
                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        outParameter1 = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outParameter1.Direction = ParameterDirection.Output;
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
                employeeID = Guid.Parse(outParameter1.Value.ToString()),
                Status = outParameter.Value.ToString(),
                Message=c.InsertSuccess
            };
        }
        #endregion InsertEmployee

        #region UpdateEmployee
        public object UpdateEmployee(Employees employeesObj)
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
                        cmd.CommandText = "[UpdateEmployee]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = employeesObj.SCCode;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value =Guid.Parse(employeesObj.ID.ToString());
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 250).Value = employeesObj.Name;
                        cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 50).Value = employeesObj.Type;
                        cmd.Parameters.Add("@MobileNo", SqlDbType.NVarChar, 50).Value = employeesObj.MobileNo;
                        cmd.Parameters.Add("@Address", SqlDbType.NVarChar, -1).Value = employeesObj.Address;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = employeesObj.Remarks;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = employeesObj.logDetails.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = employeesObj.logDetails.CreatedDate;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = employeesObj.IsActive;
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
                Status = outParameter.Value.ToString(),
                Message = c.UpdateSuccess
            };
        }
        #endregion UpdateEmployee

        #region DeleteEmployee
        public string DeleteEmployee(string ID,UA ua)
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
                        cmd.CommandText = "[DeleteEmployee]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar,5).Value = ua.SCCode;

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
        #endregion DeleteEmployee

        #region GetAllTechnicians
        public List<Employees> GetAllTechnicians(UA UA)
        {
            List<Employees> Technicianslist = null;
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
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.CommandText = "[GetAllTechnician]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Technicianslist = new List<Employees>();
                                while (sdr.Read())
                                {
                                    Employees employeesObj = new Employees();

                                    {
                                        employeesObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : employeesObj.ID);
                                        employeesObj.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : employeesObj.Name);

                                    };

                                    Technicianslist.Add(employeesObj);
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
            return Technicianslist;
        }
        #endregion GetAllTechnicians

        #endregion Methods
    }
}