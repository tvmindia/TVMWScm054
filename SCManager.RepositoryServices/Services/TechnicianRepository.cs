using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.RepositoryServices.Contracts;
using SCManager.DataAccessObject.DTO;
using System.Data;
using System.Data.SqlClient;

namespace SCManager.RepositoryServices.Services
{
    public class TechnicianRepository : ITechnicianRepository
    {
           private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
           public TechnicianRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }


           public List<TechnicianSummary> GetTechnicianSummary(UA UA)
        {
            List<TechnicianSummary> TechnicianSummary = null;
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
                        cmd.CommandText = "[GetTechnicianSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                TechnicianSummary = new List<TechnicianSummary>();
                                while (sdr.Read())
                                {
                                    TechnicianSummary technicianSummaryObj = new TechnicianSummary();
                                    {
                                        technicianSummaryObj.Name = sdr["Name"].ToString();
                                        technicianSummaryObj.Calls = (sdr["Calls"].ToString() != "" ? Int16.Parse(sdr["Calls"].ToString()) : technicianSummaryObj.Calls);                                        
                                        technicianSummaryObj.StockValue = (sdr["StockValue"].ToString() != "" ? decimal.Parse(sdr["StockValue"].ToString()) : technicianSummaryObj.StockValue);

                                    }
                                    TechnicianSummary.Add(technicianSummaryObj);
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

            return TechnicianSummary;
        }
    }
}