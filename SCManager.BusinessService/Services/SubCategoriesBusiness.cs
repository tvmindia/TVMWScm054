using SCManager.BusinessService.Contracts;
using SCManager.RepositoryServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;

namespace SCManager.BusinessService.Services
{
    public class SubCategoriesBusiness : ISubCategoriesBusiness
    {
        private ISubCategoryRepository _SubCategoryRepository;

        public SubCategoriesBusiness(ISubCategoryRepository SubCategoryRepository)
        {
            _SubCategoryRepository = SubCategoryRepository;
        }

        public List<SubCategories> GetAllSubCategories(UA UA,string categoryID)
        {
            List<SubCategories> SubCategorieslist = null;
            SubCategorieslist = _SubCategoryRepository.GetAllSubCategories(UA,categoryID);
            return SubCategorieslist;
        }

     
    }
}