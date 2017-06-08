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
    public class SalesRepository: ISalesRepository
    {
        private IDatabaseFactory _databaseFactory;
        public SalesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<SalesGraph> GetWeeklySalesDetails(UA UA)
        {

            List<SalesGraph> weeklySalesSummaryList = null;
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
                        cmd.CommandText = "[GetWeeklySales]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                weeklySalesSummaryList = new List<SalesGraph>();
                                while (sdr.Read())
                                {
                                    SalesGraph _weeklysalesDetailObj = new SalesGraph();
                                    {
                                        _weeklysalesDetailObj.Value = (sdr["Value"].ToString() != "" ? decimal.Parse(sdr["Value"].ToString()) : _weeklysalesDetailObj.Value);
                                        _weeklysalesDetailObj.Label = (sdr["Label"].ToString() != "" ? sdr["Label"].ToString() : _weeklysalesDetailObj.Label);

                                    }
                                    weeklySalesSummaryList.Add(_weeklysalesDetailObj);
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

            return weeklySalesSummaryList;


        }

    }
}