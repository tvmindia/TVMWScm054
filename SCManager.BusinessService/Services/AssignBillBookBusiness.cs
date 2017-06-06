using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
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