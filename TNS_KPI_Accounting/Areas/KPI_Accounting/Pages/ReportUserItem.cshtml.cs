using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using TNS.Auth;

namespace TNS_KPI.Areas.KPI_Accounting.Pages
{
    [IgnoreAntiforgeryToken]
    public class ReportUserItemModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion

        public void OnGet(string styleView)
        {
            CheckAuth();
        }
      
        public IActionResult OnPostLoadReport([FromBody] ItemRequest request)
        {
            CheckAuth();
            int zYearView = int.Parse(request.YearView);
            int zMonthView = int.Parse(request.MonthView);
            List<string[]> zData = Models.Report.Result(zMonthView,zYearView);
            return new JsonResult(zData);
        }
  
        public IActionResult OnPostCollectData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            int zYear = int.Parse(request.YearView);
            List<string[]> zListDepartment = new List<string[]>();// Models.AccessData.GetDepartment(zYear);

            return new JsonResult(zListDepartment);
        }

        public class ItemRequest
        {
            public string YearView { get; set; }
            public string MonthView { get; set; }
            public string DepartmentID { get; set; }
        }
    }
}
