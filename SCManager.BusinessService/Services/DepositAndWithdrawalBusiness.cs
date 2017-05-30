using SCManager.BusinessService.Contracts;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class DepositAndWithdrawalBusiness: IDepositAndWithdrawalBusiness
    {
        private IDepositAndWithdrawalRepository _depositAndWithdrawalRepository;
        public DepositAndWithdrawalBusiness(IDepositAndWithdrawalRepository depositAndWithdrawalRepository)
        {
            _depositAndWithdrawalRepository = depositAndWithdrawalRepository;
        }
    }
}