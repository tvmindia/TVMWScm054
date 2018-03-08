using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace SCManager.RepositoryServices.Contracts
{
    public interface IDatabaseFactory
    {
        SqlConnection GetDBConnection();

        Boolean DisconectDB();

        OleDbConnection GetOleDBConnection(int flag, string fname);
    }
}
