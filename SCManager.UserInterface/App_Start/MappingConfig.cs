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
                config.CreateMap<TechnicianStockViewModel, TechnicianStock>().ReverseMap();
                config.CreateMap<TCRBillEntryViewModel, TCRBillEntry>().ReverseMap();
                config.CreateMap<TCRBillEntryDetailViewModel, TCRBillEntryDetail>().ReverseMap();
                config.CreateMap<ICRBillEntryViewModel, ICRBillEntry>().ReverseMap();
                config.CreateMap<ICRBillEntryDetailViewModel, ICRBillEntryDetail>().ReverseMap();
                config.CreateMap<ServiceTypeViewModel, ServiceType>().ReverseMap();
                config.CreateMap<JobViewModel, Job>().ReverseMap();
                config.CreateMap<CallTypeViewModel, CallTypes>().ReverseMap();
                config.CreateMap<JobViewModel, TechnicianJob>().ReverseMap();
                config.CreateMap<ExpensesViewModel, Expenses>().ReverseMap();
                config.CreateMap<ExpenseTypeViewModel, ExpenseType>().ReverseMap();
                config.CreateMap<ICRExpensesViewModel, ICRExpenses>().ReverseMap();
                config.CreateMap<IncomeExpenseViewModel, IncomeExpense>().ReverseMap();
                config.CreateMap<ServiceRegistrySummaryViewModel, ServiceRegistrySummary>().ReverseMap();
                config.CreateMap<DepositAndWithdrawalViewModel, DepositAndWithdrawal>().ReverseMap();
                config.CreateMap<OfficeBillEntryViewModel, OfficeBillEntry>().ReverseMap();
                config.CreateMap<OfficeBillEntryDetailViewModel, OfficeBillEntryDetail>().ReverseMap();
                config.CreateMap<JobCallTypesViewModel, JobCallTypes>().ReverseMap();
                config.CreateMap<ReceiveFromOtherSCViewModel, ReceiveFromOtherSC>().ReverseMap();
                config.CreateMap<ReceiveFromOtherSCDetailViewModel, ReceiveFromOtherScDetail>().ReverseMap();
                config.CreateMap<IssueToOtherSCViewModel, IssueToOtherSC>().ReverseMap();
                config.CreateMap<IssueToOtherSCDetailViewModel, IssueToOtherScDetail>().ReverseMap();
                config.CreateMap<TechnicianSalaryViewModel, TechnicianSalary>().ReverseMap();
            });
        }
    }
}