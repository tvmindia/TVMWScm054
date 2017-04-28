using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class CategoriesBusiness : ICategoriesBusiness
    {
        private ICategoryRepository _categoryRepository;

        public CategoriesBusiness(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        public List<Categories> GetAllCategories(UA UA)
        {
            List<Categories> Categorieslist = null;
            Categorieslist = _categoryRepository.GetAllCategories(UA);
            return Categorieslist;
        }
    }
}