using System.Web.Optimization;

namespace SCManager.UserInterface.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css", "~/Content/bootstrap-theme.css", "~/Content/font-awesome.min.css", "~/Content/Custom.css", "~/Content/sweetalert.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrapdatepicker").Include("~/Content/bootstrap-datepicker3.min.css"));         
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatable").Include("~/Content/DataTables/css/dataTables.bootstrap.min.css", "~/Content/DataTables/css/responsive.bootstrap.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatablecheckbox").Include("~/Content/DataTables/css/dataTables.checkboxes.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableSelect").Include("~/Content/DataTables/css/select.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableButtons").Include("~/Content/DataTables/css/buttons.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableFixedColumns").Include("~/Content/DataTables/css/fixedColumns.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/UserCSS/Form8").Include("~/Content/UserCSS/Form8.css"));
            bundles.Add(new StyleBundle("~/Content/UserCSS/Form8B").Include("~/Content/UserCSS/Form8B.css"));
            bundles.Add(new StyleBundle("~/Content/UserCSS/Login").Include("~/Content/UserCSS/Login.css"));
            bundles.Add(new StyleBundle("~/Content/UserCSS/Item").Include("~/Content/UserCSS/Item.css"));
            bundles.Add(new StyleBundle("~/Content/UserCSS/Report").Include("~/Content/UserCSS/Report.css"));
            bundles.Add(new StyleBundle("~/Content/UserCSS/DailyServiceReport").Include("~/Content/UserCSS/DailyServiceReport.css"));
            
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-3.1.1.min.js")); 
          
            bundles.Add(new ScriptBundle("~/bundles/jqueryform").Include("~/Scripts/jquery.form.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryunobtrusiveajaxvalidate").Include("~/Scripts/jquery.validate.min.js", "~/Scripts/jquery.validate.unobtrusive.min.js", "~/Scripts/jquery.unobtrusive-ajax.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include("~/Scripts/DataTables/jquery.dataTables.min.js", "~/Scripts/DataTables/dataTables.bootstrap.min.js", "~/Scripts/DataTables/dataTables.responsive.min.js", "~/Scripts/DataTables/responsive.bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableSelect").Include("~/Scripts/DataTables/dataTables.select.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatablecheckbox").Include("~/Scripts/DataTables/dataTables.checkboxes.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableButtons").Include("~/Scripts/DataTables/dataTables.buttons.min.js", "~/Scripts/DataTables/buttons.flash.min.js","~/Scripts/DataTables/buttons.html5.min.js", "~/Scripts/DataTables/buttons.print.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableFixedColumns").Include("~/Scripts/DataTables/dataTables.fixedColumns.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jsZip").Include("~/Scripts/jszip.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/userpluginjs").Include("~/Scripts/jquery.noty.packaged.min.js", "~/Scripts/custom.js", "~/Scripts/Chart.js", "~/Scripts/sweetalert.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrapdatepicker").Include("~/Scripts/bootstrap-datepicker.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/userpluginjs").Include("~/Scripts/jquery.noty.packaged.min.js", "~/Scripts/custom.js", "~/Scripts/Chart.js", "~/Scripts/sweetalert.min.js"));
           
            bundles.Add(new ScriptBundle("~/bundles/OpeningSetting").Include("~/Scripts/UserJS/OpeningSetting.js"));
            bundles.Add(new ScriptBundle("~/bundles/Form8").Include("~/Scripts/UserJS/Form8.js"));
            bundles.Add(new ScriptBundle("~/bundles/Form8B").Include("~/Scripts/UserJS/Form8B.js"));
            bundles.Add(new ScriptBundle("~/bundles/LocalPurchase").Include("~/Scripts/UserJS/LocalPurchase.js"));
            bundles.Add(new ScriptBundle("~/bundles/Dashboard").Include("~/Scripts/UserJS/Dashboard.js"));
            bundles.Add(new ScriptBundle("~/bundles/demoGrid").Include("~/Scripts/UserJS/demoGrid.js"));
            bundles.Add(new ScriptBundle("~/bundles/editableGrid").Include("~/Scripts/editableGrid.js"));
            bundles.Add(new ScriptBundle("~/bundles/Item").Include("~/Scripts/UserJS/Item.js"));
            bundles.Add(new ScriptBundle("~/bundles/Employees").Include("~/Scripts/UserJS/Employees.js"));
            bundles.Add(new ScriptBundle("~/bundles/Login").Include("~/Scripts/UserJS/Login.js"));
            bundles.Add(new ScriptBundle("~/bundles/CallandServiceTypes").Include("~/Scripts/UserJS/CallandServiceTypes.js"));
            bundles.Add(new ScriptBundle("~/bundles/DefectDamage").Include("~/Scripts/UserJS/DefectiveDamage.js"));
            bundles.Add(new ScriptBundle("~/bundles/SalesReturn").Include("~/Scripts/UserJS/SalesReturn.js"));
            bundles.Add(new ScriptBundle("~/bundles/CreditNotes").Include("~/Scripts/UserJS/CreditNotes.js"));
            bundles.Add(new ScriptBundle("~/bundles/OtherIncome").Include("~/Scripts/UserJS/OtherIncome.js"));
            bundles.Add(new ScriptBundle("~/bundles/StockSummary").Include("~/Scripts/UserJS/StockSummary.js"));
            bundles.Add(new ScriptBundle("~/bundles/IssueToTechnician").Include("~/Scripts/UserJS/IssueToTechnician.js"));
            bundles.Add(new ScriptBundle("~/bundles/ReceiveFromTechnician").Include("~/Scripts/UserJS/ReceiveFromTechnician.js"));
            bundles.Add(new ScriptBundle("~/bundles/StockLedger").Include("~/Scripts/UserJS/StockLedger.js"));
            bundles.Add(new ScriptBundle("~/bundles/TechnicianStock").Include("~/Scripts/UserJS/TechnicianStock.js"));
            bundles.Add(new ScriptBundle("~/bundles/TCRBillEntry").Include("~/Scripts/UserJS/TCRBillEntry.js"));
            bundles.Add(new ScriptBundle("~/bundles/ICRBillEntry").Include("~/Scripts/UserJS/ICRBillEntry.js"));
            bundles.Add(new ScriptBundle("~/bundles/DailyServiceReport").Include("~/Scripts/UserJS/DailyServiceReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/JobAction").Include("~/Scripts/UserJS/JobActions.js"));
            bundles.Add(new ScriptBundle("~/bundles/Expenses").Include("~/Scripts/UserJS/Expenses.js"));
            bundles.Add(new ScriptBundle("~/bundles/ICRExpenses").Include("~/Scripts/UserJS/ICRExpenses.js"));
            bundles.Add(new ScriptBundle("~/bundles/IncomeExpense").Include("~/Scripts/UserJS/IncomeExpenseReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/Depositwithdrawal").Include("~/Scripts/UserJS/DepositAndWithdrawal.js"));
            bundles.Add(new ScriptBundle("~/bundles/OfficeBillEntry").Include("~/Scripts/UserJS/OfficeBillEntry.js"));
            bundles.Add(new ScriptBundle("~/bundles/ReceiveFromOtherSC").Include("~/Scripts/UserJS/ReceiveFromOtherSC.js"));
            bundles.Add(new ScriptBundle("~/bundles/IssueToOtherSC").Include("~/Scripts/UserJS/IssueToOtherSC.js"));
            bundles.Add(new ScriptBundle("~/bundles/TechnicianSalaryCalcu").Include("~/Scripts/UserJS/TechnicianSalaryCalculation.js"));
        }
    }
}