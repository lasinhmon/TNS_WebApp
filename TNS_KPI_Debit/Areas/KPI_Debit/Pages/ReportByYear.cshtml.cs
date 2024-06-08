using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using TNS.Auth;

namespace TNS_KPI.Areas.KPI_Debit.Pages
{
    [IgnoreAntiforgeryToken]
    public class ReportByYearModel : PageModel
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
            int zYearView = int.Parse(request.YearView);
            List<string[]> zData = Models.AccessData.DataDuNoByDepartment(zYearView);
            return new JsonResult(zData);
        }
        public class ItemRequest
        {
            public string YearView { get; set; }
            public string DepartmentID { get; set; }
        }
    }
}
