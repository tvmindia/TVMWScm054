using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class SalesBusiness: ISalesBusiness
    {
        ISalesRepository _salesRepository;
        public SalesBusiness(ISalesRepository salesRepository)
        {
            _salesRepository = salesRepository;
        }
        public List<SalesGraph> GetWeeklySalesDetails(UA UA)
        {
            List<SalesGraph> WeeklySalesDeatails = null;
            try
            {
                WeeklySalesDeatails = _salesRepository.GetWeeklySalesDetails(UA).OrderBy(p => p.Label).ToList();
                if (WeeklySalesDeatails != null)
                {
                    foreach (var i in WeeklySalesDeatails)
                    {
                        DateTime dt = FirstDateOfWeek(int.Parse(DateTime.Now.Year.ToString()), int.Parse(i.Label));
                        int weekOfMonth = GenerateWeekNumber(dt) - GenerateWeekNumber(dt.AddDays(1 - dt.Day)) + 1;
                        string Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dt.Month);
                        i.Label = Month + " Wk " + weekOfMonth;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return WeeklySalesDeatails;
            //return _salesRepository.GetWeeklySalesDetails(UA);
        }
        public static int GenerateWeekNumber(DateTime dt)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
    }
}