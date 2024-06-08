using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data;
using System.Reflection;
using System.Security.Claims;

namespace TNS_KPI.Areas.KPI_Accounting.Pages
{
    [IgnoreAntiforgeryToken]
    public class PersonalModel : PageModel
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
            zInfo.TieuChi_TongButToan = zPersonal.TieuChi_TongButToan;
            zInfo.WorkingAtCounter = zPersonal.WorkingAtCounter.ToString("#,##0");
            zInfo.WorkingAtCounter_1 = zPersonal.WorkingAtCounter_1.ToString("#,##0");
            zInfo.WorkingAtCounter_2 = zPersonal.WorkingAtCounter_2.ToString("#,##0");

          
            zInfo.TieuChi_ButToanXuLy = zPersonal.TieuChi_ButToanXuLy;
            return new JsonResult(zInfo);
        }
        public IActionResult OnPostLoadPhuLuc([FromBody] ItemRequest request)
        {
            CheckAuth();
            JsonResult zResult = new JsonResult("");
            string zMessageLog = "";
            string zUserID = request.UserID;
            int zMonthView = int.Parse(request.MonthView);
            int zYearView = int.Parse(request.YearView);
            Models.Personal_Info zPersonal = new Models.Personal_Info(zUserID, zMonthView, zYearView);
            zPersonal.PhuLuc_Get();
            switch (request.CategoryID)
            {
                case "I":
                    zPersonal.PhuLuc_Calculator_I(request.Status);
                    break;
                case "III":
                    zPersonal.PhuLuc_Calculator_III();
                    break;
            }
            
           
            zResult = zPersonal.View_PhuLuc();
            return zResult;
        }
        public IActionResult OnPostLoadPhuLucItem([FromBody] ItemRequest request)
        {
            CheckAuth();
            JsonResult zResult = new JsonResult("");
            string zMessageLog = "";
            string zUserID = request.UserID;
            int zMonthView = int.Parse(request.MonthView);
            int zYearView = int.Parse(request.YearView);
            DataTable zPhuLuc = Models.Categories_AccessData.GetData("SELECT * FROM KPI_Accounting_PhuLuc WHERE AutoKey ='" + request.Key + "'");
            Models.Personal_Info zPersonal = new Models.Personal_Info(zUserID, zMonthView, zYearView);

            if (zPhuLuc.Rows.Count > 0)
            {
                if (zPhuLuc.Rows[0]["PhuLucID"].ToString() == "I")
                {
                    zResult = zPersonal.View_PhuLuc_Item("[dbo].[KPI_Accounting_HauKiemBT]",
                        request.Status,
                        zPhuLuc.Rows[0]["Module"].ToString(),
                        zPhuLuc.Rows[0]["MaCodeNV"].ToString(),
                        zPhuLuc.Rows[0]["Analysis"].ToString(),
                        zPhuLuc.Rows[0]["NhatDuLieu"].ToString());
                }
                if (zPhuLuc.Rows[0]["PhuLucID"].ToString() == "III")
                {
                    zResult = zPersonal.View_PhuLuc_Item("[dbo].[KPI_Accounting_HauKiemBT_None]",
                        "",
                        zPhuLuc.Rows[0]["Module"].ToString(),
                        zPhuLuc.Rows[0]["MaCodeNV"].ToString(),
                        zPhuLuc.Rows[0]["Analysis"].ToString(),
                        zPhuLuc.Rows[0]["NhatDuLieu"].ToString());
                }
            }

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
            public string CategoryID { get; set; }
            public string Status { get; set; }
            public string PageSelected { get; set; }
            public string Key { get; set; }
        }
        public class ItemInfo
        {
            public string Key { get; set; }
            public string ID { get; set; }
            public string Name { get; set; }
            public string WorkingAtCounter { get; set; }
            public string WorkingAtCounter_1 { get; set; }
            public string WorkingAtCounter_2 { get; set; }
            public int[] TieuChi_TongButToan { get; set; }
            public int[] TieuChi_ButToanXuLy { get; set; }
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
