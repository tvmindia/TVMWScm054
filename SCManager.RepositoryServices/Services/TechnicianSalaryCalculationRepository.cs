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
    public class TechnicianSalaryCalculationRepository: ITechnicianSalaryCalculationRepository
    {
        Const c = new Const();
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
       
        public TechnicianSalaryCalculationRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

      


        #endregion DataBaseFactory
        public List<TechnicianSalary> GetTechniciansCalculatedSalary(string SCCode, short? Month, short? Year)
        {
            List<TechnicianSalary> technicianSalaryList = null;
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
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = SCCode;
                        cmd.Parameters.Add("@month", SqlDbType.Int).Value = Month;
                        cmd.Parameters.Add("@year", SqlDbType.Int).Value = Year;
                        cmd.CommandText = "[CalculateSalaryForaMonth]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                technicianSalaryList = new List<TechnicianSalary>();
                                while (sdr.Read())
                                {
                                    TechnicianSalary technicianSalary = new TechnicianSalary();
                                    {
                                        technicianSalary.Name = sdr["Name"].ToString();
                                        technicianSalary.SCCode = sdr["SCCode"].ToString();
                                        technicianSalary.EmpID = (sdr["EmpID"].ToString() != "" ? Guid.Parse(sdr["EmpID"].ToString()) : technicianSalary.EmpID);
                                        technicianSalary.Month= (sdr["Month"].ToString() != "" ? Int16.Parse(sdr["Month"].ToString()) : technicianSalary.Month);
                                        technicianSalary.Year = (sdr["Year"].ToString() != "" ? Int16.Parse(sdr["Year"].ToString()) : technicianSalary.Year);
                                        technicianSalary.TotalCommission= (sdr["TotalComm"].ToString() != "" ? decimal.Parse(sdr["TotalComm"].ToString()) : technicianSalary.TotalCommission);
                                        technicianSalary.SalaryAdvance = (sdr["SalaryAdvance"].ToString() != "" ? decimal.Parse(sdr["SalaryAdvance"].ToString()) : technicianSalary.SalaryAdvance);
                                        technicianSalary.TotalPayable = (sdr["TotalPayable"].ToString() != "" ? decimal.Parse(sdr["TotalPayable"].ToString()) : technicianSalary.TotalPayable);
                                        technicianSalary.MajorCalls = (sdr["MajorCalls"].ToString() != "" ? int.Parse(sdr["MajorCalls"].ToString()) : technicianSalary.MajorCalls);
                                        technicianSalary.MajorCommission = (sdr["MajorComm"].ToString() != "" ? decimal.Parse(sdr["MajorComm"].ToString()) : technicianSalary.MajorCommission);
                                        technicianSalary.DemoCalls = (sdr["DemoCalls"].ToString() != "" ? int.Parse(sdr["DemoCalls"].ToString()) : technicianSalary.DemoCalls);
                                        technicianSalary.DemoCommission = (sdr["DemoComm"].ToString() != "" ? decimal.Parse(sdr["DemoComm"].ToString()) : technicianSalary.DemoCommission);
                                        technicianSalary.MandatoryCalls = (sdr["MandatoryCalls"].ToString() != "" ? int.Parse(sdr["MandatoryCalls"].ToString()) : technicianSalary.MandatoryCalls);
                                        technicianSalary.MandatoryCommission = (sdr["MandatoryComm"].ToString() != "" ? decimal.Parse(sdr["MandatoryComm"].ToString()) : technicianSalary.MandatoryCommission);
                                        technicianSalary.MinorCalls = (sdr["MinorCalls"].ToString() != "" ? int.Parse(sdr["MinorCalls"].ToString()) : technicianSalary.MinorCalls);
                                        technicianSalary.MinorCommission = (sdr["MinorComm"].ToString() != "" ? decimal.Parse(sdr["MinorComm"].ToString()) : technicianSalary.MinorCommission);
                                        technicianSalary.RepeatCalls = (sdr["RepeatCalls"].ToString() != "" ? int.Parse(sdr["RepeatCalls"].ToString()) : technicianSalary.RepeatCalls);
                                        technicianSalary.RepeatCommission = (sdr["RepeatComm"].ToString() != "" ? decimal.Parse(sdr["RepeatComm"].ToString()) : technicianSalary.RepeatCommission);
                                        technicianSalary.RepeatDeductCalls = (sdr["RepeatDeductCalls"].ToString() != "" ? int.Parse(sdr["RepeatDeductCalls"].ToString()) : technicianSalary.RepeatDeductCalls);
                                        technicianSalary.RepeatDeductCommission = (sdr["RepeatDeductComm"].ToString() != "" ? decimal.Parse(sdr["RepeatDeductComm"].ToString()) : technicianSalary.RepeatDeductCommission);
                                        technicianSalary.SpecialCommission = (sdr["SpecialCommission"].ToString() != "" ? decimal.Parse(sdr["SpecialCommission"].ToString()) : technicianSalary.SpecialCommission);
                                        technicianSalary.ServiceChargeCommission = (sdr["ServiceChargeComm"].ToString() != "" ? decimal.Parse(sdr["ServiceChargeComm"].ToString()) : technicianSalary.ServiceChargeCommission);
                                        technicianSalary.ProductCommission = (sdr["ProductCommission"].ToString() != "" ? decimal.Parse(sdr["ProductCommission"].ToString()) : technicianSalary.ProductCommission);
                                        technicianSalary.AMCCommission = (sdr["AMCCommission"].ToString() != "" ? decimal.Parse(sdr["AMCCommission"].ToString()) : technicianSalary.AMCCommission);
                                    
                                    }
                                    technicianSalaryList.Add(technicianSalary);
                                }
                            }//if
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return technicianSalaryList;
        }


        public List<TechnicianSalaryJobBreakUp> GetTechnicianJobCommissionBreakUp(string SCCode, Guid EmpID, short? Month, short? Year)
        {
            List<TechnicianSalaryJobBreakUp> technicianSalaryList = null;
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
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = SCCode;
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = EmpID;
                        cmd.Parameters.Add("@month", SqlDbType.Int).Value = Month;
                        cmd.Parameters.Add("@year", SqlDbType.Int).Value = Year;
                        cmd.CommandText = "[CalculateTechSalaryBreakup_Job]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                technicianSalaryList = new List<TechnicianSalaryJobBreakUp>();
                                while (sdr.Read())
                                {
                                    TechnicianSalaryJobBreakUp technicianSalary = new TechnicianSalaryJobBreakUp();
                                    {
                                        technicianSalary.ServiceDate = (sdr["ServiceDate"].ToString() != "" ? sdr["ServiceDate"].ToString() : technicianSalary.ServiceDate);
                                        technicianSalary.JobNo = (sdr["JobNo"].ToString() != "" ? sdr["JobNo"].ToString() : technicianSalary.JobNo);
                                        technicianSalary.CustomerName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : technicianSalary.CustomerName);
                                        technicianSalary.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : technicianSalary.Type);
                                        technicianSalary.Commission = (sdr["Commission"].ToString() != "" ? decimal.Parse(sdr["Commission"].ToString()) : technicianSalary.Commission);
                                        technicianSalary.SpecialCommission = (sdr["SpecialCommission"].ToString() != "" ? decimal.Parse(sdr["SpecialCommission"].ToString()) : technicianSalary.SpecialCommission);
                                        technicianSalary.Total = (sdr["Total"].ToString() != "" ? decimal.Parse(sdr["Total"].ToString()) : technicianSalary.SpecialCommission);
                                    }
                                    technicianSalaryList.Add(technicianSalary);
                                }
                            }//if
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return technicianSalaryList;
        }
    }
}
    
