using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data;
using System.Reflection;
using System.Security.Claims;

namespace TNS_KPI.Areas.KPI_Accounting.Pages
{
    [IgnoreAntiforgeryToken]
    public class PhuLuc_UserModel : PageModel
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
        public IActionResult OnPostLoadData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            DataTable zListNotice = new DataTable();
            ItemView zItem = new ItemView();
            string[] zDateConvert;

            string zListField = "AutoKey,Module,MaCodeNV,NhatDuLieu,TenButToan,Quantity,Score";

            zItem.FieldsName = new List<string>();
            string[] zTempName = zListField.Split(',');
            for (int i = 0; i < zTempName.Length; i++)
            {
                zItem.FieldsName.Add(zTempName[i]);
            }

            zItem.Info = new ItemInfo();
            zItem.Info.Name = "OK";

            //zItem.DataOfTable = Models.Categories_AccessData.DataOfTable(zSQL);

            return new JsonResult(zItem);
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
