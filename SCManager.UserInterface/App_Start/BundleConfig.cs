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

            bundles.Add(new StyleBundle("~/Content/UserCSS/Form8").Include("~/Content/UserCSS/Form8.css"));
            bundles.Add(new StyleBundle("~/Content/UserCSS/Form8B").Include("~/Content/UserCSS/Form8B.css"));
            bundles.Add(new StyleBundle("~/Content/UserCSS/Login").Include("~/Content/UserCSS/Login.css"));
            bundles.Add(new StyleBundle("~/Content/UserCSS/Item").Include("~/Content/UserCSS/Item.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-3.1.1.min.js")); 
          
            bundles.Add(new ScriptBundle("~/bundles/jqueryform").Include("~/Scripts/jquery.form.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryunobtrusiveajaxvalidate").Include("~/Scripts/jquery.validate.min.js", "~/Scripts/jquery.validate.unobtrusive.min.js", "~/Scripts/jquery.unobtrusive-ajax.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include("~/Scripts/DataTables/jquery.dataTables.min.js", "~/Scripts/DataTables/dataTables.bootstrap.min.js", "~/Scripts/DataTables/dataTables.responsive.min.js", "~/Scripts/DataTables/responsive.bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableSelect").Include("~/Scripts/DataTables/dataTables.select.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatablecheckbox").Include("~/Scripts/DataTables/dataTables.checkboxes.js"));
            bundles.Add(new ScriptBundle("~/bundles/userpluginjs").Include("~/Scripts/jquery.noty.packaged.min.js", "~/Scripts/custom.js", "~/Scripts/Chart.js", "~/Scripts/sweetalert.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrapdatepicker").Include("~/Scripts/bootstrap-datepicker.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/userpluginjs").Include("~/Scripts/jquery.noty.packaged.min.js", "~/Scripts/custom.js", "~/Scripts/Chart.js", "~/Scripts/sweetalert.min.js"));

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
        }
    }
}