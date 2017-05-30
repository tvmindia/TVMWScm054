using SCManager.BusinessService.Contracts;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class ICRExpensesBusiness: IICRExpensesBusiness
    {

        private IICRExpensesRepository _iicrexpensesRepository;
        public ICRExpensesBusiness(IICRExpensesRepository icrexpensesRepository)
        {
            _iicrexpensesRepository = icrexpensesRepository;
        }
    }
}