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
    public class BranchModel : PageModel
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
            int zYearView = int.Parse(request.YearView);
            List<string[]> zData = Models.AccessData.DataReportByDepartment(zYearView);
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
