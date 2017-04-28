using SCManager.UserInterface.Models;
using SCManager.DataAccessObject.DTO;

namespace SCManager.UserInterface.App_Start
{
    public class MappingConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                //domain <===== viewmodel
                //viewmodel =====> domain
                //ReverseMap() makes it possible to map both ways.

                config.CreateMap<MenuViewModel, Menu>().ReverseMap();
                config.CreateMap<Form8ViewModel, Form8>().ReverseMap();
                config.CreateMap<Form8DetailViewModel, Form8Detail>().ReverseMap();
                config.CreateMap<LogDetailsViewModel, LogDetails>().ReverseMap();
                config.CreateMap<ReorderAlertViewModel , ReorderAlert>().ReverseMap();
                config.CreateMap<TechnicianSummaryViewModel, TechnicianSummary>().ReverseMap();
                config.CreateMap<ItemViewModel, Item>().ReverseMap();
                config.CreateMap<ItemDropdownViewModel, Item>().ReverseMap();
                config.CreateMap<ItemViewModel, Categories>().ReverseMap();
                config.CreateMap<ItemViewModel, SubCategories>().ReverseMap();
            });
        }
    }
}