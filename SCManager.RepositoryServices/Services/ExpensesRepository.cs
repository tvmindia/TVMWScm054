using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.RepositoryServices.Services
{
    public class ExpensesRepository: IExpensesRepository
    {
        private IDatabaseFactory _databaseFactory;
        private Const constObj = new Const();
        public ExpensesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

    }
}