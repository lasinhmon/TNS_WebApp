using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data;
using System.Reflection;
using System.Security.Claims;

namespace TNS_KPI.Areas.KPI_Accounting.Pages
{
    [IgnoreAntiforgeryToken]
    public class Personal_ChartModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion

        private int _PageSize = 50;
        public void OnGet(string styleView)
        {
            CheckAuth();

        }
        public IActionResult OnPostLoadDataInfo([FromBody] ItemRequest request)
        {
            CheckAuth();
            JsonResult zResult;
            string zMessageLog = "";
            string zUserID = request.UserID;
            int zMonthView = int.Parse(request.MonthView);
            int zYearView = int.Parse(request.YearView);
            Models.Personal_Info zPersonal = new Models.Personal_Info(zUserID, zMonthView, zYearView);
            zPersonal.TieuChi_Get();
            
            ItemInfo zInfo = new ItemInfo();
            zInfo.ID = request.UserID;
            zInfo.Name = zPersonal.TenNhanVien;
            zInfo.TieuChi = zPersonal.TieuChi_TongButToan;
            return new JsonResult(zInfo);
        }
        public IActionResult OnPostLoadPhuLuc([FromBody] ItemRequest request)
        {
            CheckAuth();
            JsonResult zResult = new JsonResult("");
          
            return zResult;
        }
        public IActionResult OnPostLoadPhuLucItem([FromBody] ItemRequest request)
        {
            CheckAuth();
            JsonResult zResult = new JsonResult("");
           

            return zResult;
        }
        public IActionResult OnPostCollectData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            DataTable zListNotice = new DataTable();
            ItemView zItem = new ItemView();
            string[] zDateConvert;
            int zMonth = int.Parse(request.MonthView);
            int zYear = int.Parse(request.YearView);
            List<string[]> zListEmployee = Models.AccessData.HauKiemBT_GetEmployee(zMonth, zYear);

            return new JsonResult(zListEmployee);
        }

        public class ItemRequest
        {
            public string MonthView { get; set; }
            public string YearView { get; set; }
            public string UserID { get; set; }
            public string PageSelected { get; set; }
            public string Key { get; set; }
        }
        public class ItemInfo
        {
            public string Key { get; set; }
            public string ID { get; set; }
            public string Name { get; set; }
            public int[] TieuChi { get; set; }
            public ItemInfo() { }

        }
        public class ItemView
        {
            public List<string> FieldsName { get; set; }
            public List<string> FieldsNote { get; set; }
            public List<string[]> DataOfTable { get; set; }
            public string Code { get; set; }
            public ItemView() { }

        }


    }
}
