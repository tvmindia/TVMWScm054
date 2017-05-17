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
                config.CreateMap<Form8BViewModel, Form8B>().ReverseMap();
                config.CreateMap<Form8BDetailViewModel, Form8BDetail>().ReverseMap();
                config.CreateMap<LocalPurchaseViewModel, LocalPurchase>().ReverseMap();
                config.CreateMap<LocalPurchaseDetailViewModel, LocalPurchaseDetail>().ReverseMap();
                config.CreateMap<LogDetailsViewModel, LogDetails>().ReverseMap();
                config.CreateMap<ReorderAlertViewModel , ReorderAlert>().ReverseMap();
                config.CreateMap<TechnicianSummaryViewModel, TechnicianSummary>().ReverseMap();
                config.CreateMap<ItemViewModel, Item>().ReverseMap();
                config.CreateMap<ItemDropdownViewModel, Item>().ReverseMap();
                config.CreateMap<ItemViewModel, Categories>().ReverseMap();
                config.CreateMap<ItemViewModel, SubCategories>().ReverseMap();
                config.CreateMap<EmployeesViewModel, Employees>().ReverseMap();
                config.CreateMap<CallandServiceTypesViewModel, CallTypes>().ReverseMap();
                config.CreateMap<CallandServiceTypesViewModel, ServiceTypes>().ReverseMap();
                config.CreateMap<DefectiveorDamagedViewModel, DefectiveDamage>().ReverseMap();
                config.CreateMap<SalesReturnViewModel, SalesReturn>().ReverseMap();
                config.CreateMap<CreditNotesViewModel, CreditNotes>().ReverseMap();
                config.CreateMap<OtherIncomeViewModel, OtherIncome>().ReverseMap();
                config.CreateMap<StockValueSummaryViewModel, StockValueSummary>().ReverseMap();
                config.CreateMap<IssueToTechnicianViewModel, IssueToTechnician>().ReverseMap();
                config.CreateMap<ReceiveFromTechnicianViewModel, ReceiveFromTechnician>().ReverseMap();
                config.CreateMap<ServiceCenterViewModel, ServiceCenter>().ReverseMap();
                config.CreateMap<UserViewModel, User>().ReverseMap();
                config.CreateMap<RoleViewModel, Role>().ReverseMap();
                config.CreateMap<LoginViewModel, User>().ReverseMap();


                config.CreateMap<OpeningDetailViewModel, OpeningDetail>().ReverseMap();
                config.CreateMap<OpeningSettingViewModel, OpeningSetting>().ReverseMap();
                config.CreateMap<StockSummaryViewModel, Item>().ReverseMap();
                config.CreateMap<SystemReportViewModel, SystemReport>().ReverseMap();
                config.CreateMap<StockLedgerViewModel, StockLedger>().ReverseMap();


            });
        }
    }
}