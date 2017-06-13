using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class AssignBillBookBusiness : IAssignBillBookBusiness
    {
        private IAssignBillBookRepository _iAssignBillBookRepository;

        public AssignBillBookBusiness(IAssignBillBookRepository iAssignBillBookRepository)
        {
            _iAssignBillBookRepository = iAssignBillBookRepository;
        }
        public List<AssignBillBook> GetAllBillBook(UA UA)
        {
            List<AssignBillBook> AssignBillBooklist = null;
            AssignBillBooklist = _iAssignBillBookRepository.GetAllBillBook(UA);          
            return AssignBillBooklist;

        }
        public List<AssignBillBook> GeBillBookByID(UA UA, string ID)
        {
            List<AssignBillBook> assignBillBookList = null;
            assignBillBookList = _iAssignBillBookRepository.GeBillBookByID(UA, ID);
            return assignBillBookList;
        }
        public object BillBookNumberValidation(UA UA, string BookNo, string billBookType, string empID)
        {
            object result = null;
            result = _iAssignBillBookRepository.BillBookNumberValidation(UA, BookNo,billBookType,empID);
            return result;
        }
        public DataSet GetMissingSerials(string seriesStart, string seriesEnd, string BillBookType, UA UA)
        {
            DataSet ds = new DataSet();
            ds = _iAssignBillBookRepository.GetMissingSerials(seriesStart,seriesEnd,BillBookType,UA);
            return ds;
        }
        public string DeleteBillBook(string ID, UA ua)
        {
            string status = null;
            try
            {
                status = _iAssignBillBookRepository.DeleteBillBook(ID, ua);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }


        public string BillBookRangeValidation(string seriesStart, string seriesEnd,string BillNo, string BillBookType, UA UA)
        {
            string status = null;
            try
            {
                status = _iAssignBillBookRepository.BillBookRangeValidation(seriesStart,seriesEnd,BillNo,BillBookType, UA);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public object InsertUpdateBillBook(AssignBillBook assignBillBookObj)
        {
            object result = null;
            try
            {
                if (assignBillBookObj.ID == Guid.Empty)
                {
                    result = _iAssignBillBookRepository.InsertBillBook(assignBillBookObj);
                }
                else
                {                   
                    result = _iAssignBillBookRepository.UpdateBillBook(assignBillBookObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

    }
}