using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using TNS.Auth;

namespace TNS_KPI.Areas.KPI_Service.Pages
{
    [IgnoreAntiforgeryToken]
    public class DepartmentModel : PageModel
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
      
        public IActionResult OnPostLoadService([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            string zDepartmentID = request.DepartmentID;
            int zYearView = int.Parse(request.YearView);
            List<string[]> zData = Models.AccessData.DataReportByItem(zYearView, zDepartmentID);
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
            public string DepartmentID { get; set; }
        }
    }
}
