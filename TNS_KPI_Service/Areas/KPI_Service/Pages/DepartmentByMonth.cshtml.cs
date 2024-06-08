using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TNS_KPI.Areas.KPI_Service.Pages
{
    [IgnoreAntiforgeryToken]
    public class DepartmentByMonthModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion
        public void OnGet()
        {
            CheckAuth();
        }
        public IActionResult OnPostLoadService([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            
            int zYearView = int.Parse(request.YearView);
            int zMonthView = int.Parse(request.MonthView);
            List<string[]> zDepartments = request.Departments;
            List<string[]> zData = Models.AccessData.DataReportByItem2(zMonthView,zYearView, zDepartments);
            return new JsonResult(zData);
        }
        public IActionResult OnPostLoadPGDInfo([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";

            int zYearView = int.Parse(request.YearView);
            int zMonthView = int.Parse(request.MonthView);
            string zDepartmentId = request.DepartmentID;
            List<string[]> zData = Models.AccessData.DataReportByItem1(zMonthView, zYearView, zDepartmentId);
            return new JsonResult(zData);
        }
        public IActionResult OnPostCollectData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            int zYear = int.Parse(request.YearView);
            List<string[]> zListDepartment = Models.AccessData.GetDepartment(zYear);

            return new JsonResult(zListDepartment);
        }
        public class ItemRequest
        {
            public string YearView { get; set; }
            public string MonthView { get; set; }
            public string DepartmentID { get; set; }
            public List<string[]> Departments { get; set; }
        }
    }
}
