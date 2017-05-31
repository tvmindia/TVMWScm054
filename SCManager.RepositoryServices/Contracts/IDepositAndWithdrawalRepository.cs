using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
    public interface IDepositAndWithdrawalRepository
    {
        object InsertDepositAndWithdrawal(DepositAndWithdrawal depositAndWithdrawal);
        object UpdateDepositAndWithdrawal(DepositAndWithdrawal depositAndWithdrawal);
        object DeleteDepositAndWithdrawal(DepositAndWithdrawal depositAndWithdrawal);
        
        List<DepositAndWithdrawal> GetAllDepositAndWithdrawal(string SCCode);
    }
}
