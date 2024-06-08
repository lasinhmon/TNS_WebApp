using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace TNS_KPI.Areas.KPI_Accounting.Pages
{
    [IgnoreAntiforgeryToken]
    public class CalendarModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
            UserLogin.GetRole("KPI-Accountant_Calendar");
            UserLogin.Role.IsRead = true;
        }
        #endregion

        public void OnGet()
        {

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
           

            int zFirstDayOfMonth = DayOfWeek(zMonthView.DayOfWeek.ToString());
           
            int zDaysInMonth = DateTime.DaysInMonth(zMonthView.Year, zMonthView.Month);
            string[] zDataOfDays = new string[zDaysInMonth];
            int i = 0;
            for (i = 0; i < zDaysInMonth; i++)
            {
                zDataOfDays[i] = "0";
            }
            List<string[]> zDayWorkAtCounter = Models.AccessData.AtCounter_GetEmployee(zMonthView.Month, zMonthView.Year, UserLogin.UserName);
            foreach (string[] zDay in zDayWorkAtCounter)
            {
                zDataOfDays[int.Parse(zDay[0]) - 1] = zDay[1];
            }

            ItemView zItem = new ItemView();
            if (zMessageLog.Length > 0)
            {

                zItem.ID = "ERROR";
                zItem.Name = zMessageLog;
            }
            else
            {
                zItem.ID = "OK";
                zItem.Name = "";
                zItem.MonthView = zMonthView.ToString("MM-yyyy");
                zItem.FirstDayOfMonth = zFirstDayOfMonth;
                zItem.DaysInMonth = zDaysInMonth;
                zItem.Data = zDataOfDays;
            }
            return new JsonResult(zItem);
        }
        public IActionResult OnPostUpdate([FromBody] ItemView itemUpdate)
        {
            CheckAuth();
            string zMessageLog = "";
            string[] zDate = itemUpdate.Name.Split('-');
            DateTime zWorkingDate = new DateTime(int.Parse(zDate[2]), int.Parse(zDate[1]), int.Parse(zDate[0])); ;
            Models.AtCounter_Record zWorking = new Models.AtCounter_Record(UserLogin.UserName, zWorkingDate);
            zWorking.UserName = UserLogin.UserName;
            zWorking.EmployeeID = UserLogin.Employee.ID;
            zWorking.EmployeeName = UserLogin.Employee.Name;
            zWorking.WorkingDate = zWorkingDate;
            zWorking.AtCounter = int.Parse(itemUpdate.AtCounter);
            zWorking.CreatedBy = UserLogin.Employee.Key;
            zWorking.CreatedName = UserLogin.Employee.Name;
            zWorking.ModifiedBy = UserLogin.Employee.Key;
            zWorking.ModifiedName = UserLogin.Employee.Name;

            if (zWorking.AutoKey.Trim().Length == 36)
            {
                zWorking.Update();
            }
            else
            {
                zWorking.Create_ServerKey();
            }
            itemUpdate.Code = zWorking.Code;

            if (zWorking.Code == "501")
            {
                itemUpdate.ID = "ERROR";
                itemUpdate.Name = zWorking.Message;
            }
            else
                itemUpdate.ID = "OK";
            return new JsonResult(itemUpdate);
        }
        private int DayOfWeek(string DayName)
        {
            string[] zDayOfWeek = new string[7] { "Monday", "Tuesday", "Wednesday", "Thusday", "Friday", "Saturday", "Sunday" };
            int zIndex = 0;
            for (int i = 0; i < 7; i++)
            {
                if (zDayOfWeek[i] == DayName)
                {
                    zIndex = i;
                    break;
                }
            }
            return zIndex;
        }
        public class ItemRequest
        {
            public string MonthView { get; set; }
            public string Action { get; set; }
        }
        public class ItemView
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string AtCounter { get; set; }
            public string MonthView { get; set; }
            public int FirstDayOfMonth { get; set; }
            public int DaysInMonth { get; set; }
            public string[] Data { get; set; }

            public string btnAction { get; set; }
            public string Code { get; set; }
            public ItemView() { }

        }
    }
}
