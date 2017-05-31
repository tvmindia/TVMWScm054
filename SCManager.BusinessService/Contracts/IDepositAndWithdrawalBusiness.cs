using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
     public interface IDepositAndWithdrawalBusiness
     {
        List<DepositAndWithdrawal> GetAllDepositAndWithdrawal(string SCCode);
       
        List<DepositAndWithdrawal> GetAllDepositAndWithdrawalBetweenDates(string SCCode, string FromDate, string ToDate);
        DepositAndWithdrawal GetDepositAndWithdrawalEntryByID(string SCCode,Guid ID);
        object InsertDepositAndWithdrawal(DepositAndWithdrawal depositAndWithdrawal);
        object UpdateDepositAndWithdrawal(DepositAndWithdrawal depositAndWithdrawal);
        object DeleteDepositAndWithdrawal(DepositAndWithdrawal depositAndWithdrawal);
    }
}
