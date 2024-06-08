using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using System.Xml.Linq;

namespace TNS_KPI.Areas.KPI_Accounting.Pages
{
    [IgnoreAntiforgeryToken]
    public class Calendar_AdminModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
            UserLogin.GetRole("KPI-Accountant_Scores_Admin");
        }
        #endregion


        public IActionResult OnGet()
        {
            CheckAuth();
            if (UserLogin.Role.IsRead)
            {
                return Page();
            }
            else
                return LocalRedirect("~/Warning?id=403");

        }
        public IActionResult OnPostView([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            int zMonth = int.Parse(request.MonthView.Substring(0, 2));
            int zYear = int.Parse(request.MonthView.Substring(3, 4));
            DateTime zMonthView = new DateTime(zYear, zMonth, 01);
            if (request.Action == "Next")
                zMonthView = zMonthView.AddMonths(1);
            if (request.Action == "Pre")
                zMonthView = zMonthView.AddMonths(-1);
            if (request.Action == "Now")
                zMonthView = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            List<string[]> zEmployeeInMonth = Models.AccessData.AtCounter_GetEmployee(zMonthView.Month, zMonthView.Year);
            List<ItemView> zList = new List<ItemView>();
            ItemView zItem = new ItemView();
            bool zContinuos = true;
            if (zEmployeeInMonth.Count == 1)
            {
                if (zEmployeeInMonth[0][0] == "ERROR")
                {
                    zItem = new ItemView();
                    zItem.ID = "ERROR";
                    zItem.Name = zEmployeeInMonth[0][1];
                    zList.Add(zItem);
                    zContinuos = false;
                }

            }
            if (zContinuos)
            {
                zItem = new ItemView();
                zItem.ID = zMonthView.ToString("MM-yyyy");
                int zDaysInMonth = DateTime.DaysInMonth(zMonthView.Year, zMonthView.Month);
                zItem.Name = zDaysInMonth.ToString();
                zList.Add(zItem);
                foreach (string[] zEmployee in zEmployeeInMonth)
                {
                    zItem = new ItemView();
                    zItem.UserName = zEmployee[0].ToString();
                    zItem.EmployeeID = zEmployee[1].ToString();
                    zItem.EmployeeName = zEmployee[2].ToString();
                    zItem.WorkAtCounter = new string[zDaysInMonth + 1];
                    for (int i = 0; i < zDaysInMonth + 1; i++)
                    {
                        zItem.WorkAtCounter[i] = "";
                    }
                    List<string[]> zDayWorkAtCounter = Models.AccessData.AtCounter_GetEmployee(zMonthView.Month, zMonthView.Year, zItem.UserName);
                    foreach (string[] zDay in zDayWorkAtCounter)
                    {
                        zItem.WorkAtCounter[int.Parse(zDay[0])] = zDay[1];
                    }
                    zList.Add(zItem);
                }

            }

            return new JsonResult(zList);
        }
        public IActionResult OnPostGeneralAnalytics([FromBody] ItemRequest request)
        {
            CheckAuth();
            int zMonth = int.Parse(request.MonthView.Substring(0, 2));
            int zYear = int.Parse(request.MonthView.Substring(3, 4));

            List<string[]> zData = Models.AnalyticsData.ReportQuick_AtCounter(zMonth, zYear);
            return new JsonResult(zData);
        }
        public class ItemRequest
        {
            public string MonthView { get; set; }
            public string Action { get; set; }
            public string AutoKey { get; set; }
            public string EmployeeSearch { get; set; }
        }
        public class ItemView
        {
            public string Key { get; set; }
            public string ID { get; set; }
            public string Name { get; set; }
            public string MonthUpdate { get; set; }
            public string UserName { get; set; }
            public string EmployeeName { get; set; }
            public string EmployeeID { get; set; }
            public string DepartmentKey { get; set; }
            public string DepartmentName { get; set; }
            public string[] WorkAtCounter { get; set; }
            public ItemView() { }

        }
    }
}
