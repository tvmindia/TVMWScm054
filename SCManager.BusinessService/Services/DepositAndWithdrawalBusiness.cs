using SCManager.BusinessService.Contracts;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.DataAccessObject.DTO;

namespace SCManager.BusinessService.Services
{
    public class DepositAndWithdrawalBusiness: IDepositAndWithdrawalBusiness
    {
        private IDepositAndWithdrawalRepository _depositAndWithdrawalRepository;
        public DepositAndWithdrawalBusiness(IDepositAndWithdrawalRepository depositAndWithdrawalRepository)
        {
            _depositAndWithdrawalRepository = depositAndWithdrawalRepository;
        }

        public object DeleteDepositAndWithdrawal(DepositAndWithdrawal depositAndWithdrawal)
        {
            try
            {
                return _depositAndWithdrawalRepository.DeleteDepositAndWithdrawal(depositAndWithdrawal);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public List<DepositAndWithdrawal> GetAllDepositAndWithdrawal(string SCCode)
        {
            List<DepositAndWithdrawal> DepositAndWithdrawalList = null;
            try
            {
                DepositAndWithdrawalList = _depositAndWithdrawalRepository.GetAllDepositAndWithdrawal(SCCode);
                DepositAndWithdrawalList = DepositAndWithdrawalList == null ? null : DepositAndWithdrawalList.Select(c => { c.RefDate = DateTime.Parse(c.RefDate).Date.ToString("yyyy-MM-dd"); return c; }).OrderByDescending(D=>D.RefDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DepositAndWithdrawalList;
        }
        public List<DepositAndWithdrawal> GetAllDepositAndWithdrawalBetweenDates(string SCCode,string FromDate,string ToDate)
        {
            List<DepositAndWithdrawal> DepositAndWithdrawalList = null;
            try
            {
                DepositAndWithdrawalList = GetAllDepositAndWithdrawal(SCCode);
                //Get only last month data
                DepositAndWithdrawalList = DepositAndWithdrawalList == null ? null : DepositAndWithdrawalList
                .Where(DW => DateTime.Parse(DW.RefDate) >= DateTime.Parse(FromDate) && DateTime.Parse(ToDate) >= DateTime.Parse(DW.RefDate)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DepositAndWithdrawalList;
        }

     

        public DepositAndWithdrawal GetDepositAndWithdrawalEntryByID(string SCCode, Guid ID)
        {
            List<DepositAndWithdrawal> DepWitList = null;
            try
            {
                DepWitList = GetAllDepositAndWithdrawal(SCCode);
                DepWitList = DepWitList == null ? null : DepWitList.Where(DW => DW.ID == ID).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ((DepWitList != null) && (DepWitList.Count > 0)) ? DepWitList[0] : null;
        }

        public object InsertDepositAndWithdrawal(DepositAndWithdrawal depositAndWithdrawal)
        {
            try
            {
                return _depositAndWithdrawalRepository.InsertDepositAndWithdrawal(depositAndWithdrawal);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public object UpdateDepositAndWithdrawal(DepositAndWithdrawal depositAndWithdrawal)
        {
            try
            {
                return _depositAndWithdrawalRepository.UpdateDepositAndWithdrawal(depositAndWithdrawal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}