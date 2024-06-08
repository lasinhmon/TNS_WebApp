using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data;
using System.Reflection;
using System.Security.Claims;

namespace TNS_KPI.Areas.KPI_Accounting.Pages
{
    [IgnoreAntiforgeryToken]
    public class HauKiemBT_PhubModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion
        private int _PageSize = 25;
        public void OnGet(string styleView)
        {
            CheckAuth();

        }
        public IActionResult OnPostLoadData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            DataTable zListNotice = new DataTable();
            ItemView zItem = new ItemView();
            int zPageNumber = 1;
            int zTotal_Record = 0;
            int zTotal_PageNumber = 0;
            string[] zDateConvert;
            if (request.PageSelected.Trim().Length > 0)
            {
                zPageNumber = int.Parse(request.PageSelected.Substring(4, request.PageSelected.Length - 4));
            }
            string zListField = "GDV,MAHK,TongSoGD,NgayCT";
           
            string zWhere = " WHERE InMonth = " + request.MonthView + " AND InYear = " + request.YearView;
            if (request.SearchContent.Trim().Length > 0)
                zWhere += " AND col_buscd LIKE '%" + request.SearchContent +"%'";
            string zSQL = "SELECT " + zListField + " FROM [dbo].[KPI_Accounting_PHUB] " + zWhere + " ORDER BY GDV ";
            string zSQL_Count = "SELECT Count(*) FROM [dbo].[KPI_Accounting_PHUB] " + zWhere;


            zItem.FieldsName = new List<string>();
            zItem.FieldsNote = new List<string>();
            string[] zTempName = zListField.Split(',');
            for (int i = 0; i < zTempName.Length; i++)
            {
                zItem.FieldsName.Add(zTempName[i]);
            }

            zTotal_Record = Models.Categories_AccessData.DataCount(zSQL_Count);
            zTotal_PageNumber = zTotal_Record / _PageSize;
            if (zTotal_Record % _PageSize != 0)
                zTotal_PageNumber++;

            zItem.Info = new ItemInfo();
            zItem.Info.Name = "OK";
            zItem.Info.PageSize = _PageSize.ToString();
            zItem.Info.PageNumber = zPageNumber.ToString();
            zItem.Info.PageTotal = zTotal_PageNumber.ToString();

            zItem.DataOfTable = Models.Categories_AccessData.DataOfTable(zSQL, zPageNumber, _PageSize);

            return new JsonResult(zItem);
        }
        public IActionResult OnPostUpdateData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            ItemView zItem = new ItemView();

            return new JsonResult(zItem);
        }
        public IActionResult OnPostGeneralAnalytics([FromBody] ItemRequest request)
        {
            CheckAuth();
            List<string[]> zData = Models.AnalyticsData.ReportQuick_PHUB(int.Parse(request.MonthView), int.Parse(request.YearView));
            return new JsonResult(zData);
        }
        public class ItemRequest
        {
            public string MonthView { get; set; }
            public string YearView { get; set; }
            public string SearchContent { get; set; }
            public string PageSelected { get; set; }
            public string Key { get; set; }
        }
        public class ItemInfo
        {
            public string Key { get; set; }
            public string Name { get; set; }
            public string PageSize { get; set; }
            public string PageNumber { get; set; }
            public string PageTotal { get; set; }
            public string Content { get; set; }
            public ItemInfo() { }

        }
        public class ItemView
        {
            public ItemInfo Info { get; set; }
            public List<string> FieldsName { get; set; }
            public List<string> FieldsNote { get; set; }
            public List<string[]> DataOfTable { get; set; }
            public string Code { get; set; }
            public ItemView() { }

        }
    }
}
