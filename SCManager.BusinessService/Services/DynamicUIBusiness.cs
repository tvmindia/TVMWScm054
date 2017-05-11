using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.DataAccessObject.DTO;
using SCManager.BusinessService.Contracts;
using SCManager.RepositoryServices.Contracts;

namespace SCManager.BusinessService.Services
{
    public class DynamicUIBusiness: IDynamicUIBusiness
    {
        private IDynamicUIRepository _dynamicUIRepository;
        /// <summary>
        /// Constructor Injection:-Getting IDynamicUIBusiness implementing object
        /// </summary>
        /// <param name="dynamicUIBusiness"></param>
        public DynamicUIBusiness(IDynamicUIRepository dynamicUIRespository)
        {
            _dynamicUIRepository = dynamicUIRespository;
        }

        public List<Menu> GetAllMenues()
        {
            try
            {
                return _dynamicUIRepository.GetAllMenues();
            }
            catch (Exception)
            {
                throw;
            }


        }


        public List<ReorderAlert> GetReorderAlertITems(UA UA)
        {
            try
            {
                return _dynamicUIRepository.GetReorderAlertITems(UA);
            }
            catch (Exception)
            {
                throw;
            }


        }

        public List<StockValueSummary> GetStockValueSummary(UA UA) {
            List<StockValueSummary> result = new List<StockValueSummary>();
            result= _dynamicUIRepository.GetStockValueSummary(UA);
            if (result != null) {
                int r = 100;
                int g = 130;
                int b = 160;
                string color = "rgba($r$,$g$,$b$,0.6)";
                foreach (StockValueSummary s in result)
                {
                    s.color = color.Replace("$r$", r.ToString()).Replace("$g$", g.ToString()).Replace("$b$", b.ToString());
                    b = b + 50;
                    g = g + 30;
                    r = r + 10;
                    if (b > 250) {
                        b = 0;                        
                    }
                    if (g > 250) {
                        g = 0;                      
                        
                    }
                    if (r > 250) {
                        r = 0;
                    }
                }

            }


            return result;
        }
    }
}