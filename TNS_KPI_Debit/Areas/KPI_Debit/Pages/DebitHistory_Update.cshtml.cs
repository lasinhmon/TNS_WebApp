using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace TNS_KPI.Areas.KPI_Debit.Pages
{
    [IgnoreAntiforgeryToken]
    public class DebitHistory_UpdateModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
            UserLogin.GetRole("Full");
        }
        #endregion
        public readonly List<string[]> Departments = Models.Categories_AccessData.DepartmentsForControl();
        public void OnGet()
        {
            CheckAuth();

        }
        public IActionResult OnPostLoadData([FromBody] ItemRequest request)
        {
            CheckAuth();
            JsonResult zResult = new JsonResult("");
            string zMessageLog = "";
            int zMonth = int.Parse(request.MonthView.Substring(0, 2));
            int zYear = int.Parse(request.MonthView.Substring(3, 4));


            int zRecordCount = Models.AccessData.DebitUpdate(zMonth, zYear, request.SearchContent);
            DataTable zTable = Models.AccessData.DebitUpdate(100, zMonth, zYear, request.SearchContent);


            List<ItemView> zList = new List<ItemView>();
            ItemView zItem = new ItemView();
            if (zMessageLog.Length > 0)
            {
                zItem.ID = "ERROR";
                zItem.Name = zMessageLog;
            }
            else
            {
                zItem.ID = "OK";
                zItem.Name = "Số lượng Record là : " + zRecordCount.ToString("#,###,##0");
            }
            zList.Add(zItem);
            double zMoney = 0;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new ItemView();
                zItem.Key = zRow["AutoKey"].ToString();
                zItem.ID = zRow["CUSTSEQ"].ToString();
                zItem.Name = zRow["CUSTNM"].ToString();
                zItem.trcd = zRow["TRCTCD"].ToString();
               // zItem.AccountNumber = zRow["SO_TAI_KHOAN"].ToString();
                zItem.DepartmentID = zRow["TRCTCD"].ToString();
                zItem.EmployeeID = zRow["OFFICER_ID"].ToString();
                zItem.EmployeeName = zRow["OFFICER_NAME"].ToString();

                if (double.TryParse(zRow["DU_NO"].ToString(), out zMoney))
                    zItem.Money = zMoney.ToString("#,###,###0");
                else
                    zItem.Money = "0";

                zList.Add(zItem);
            }
            if (request.SearchContent.Trim().Length == 0)
            {
                if (zTable.Rows.Count < zRecordCount)
                {
                    zItem = new ItemView();
                    zItem.ID = "...";
                    zItem.Name = "...";
                    zItem.trcd = "";
               //     zItem.AccountNumber = "...";
                    zItem.Money = "....";
                    zItem.DepartmentID = "...";
                    zItem.EmployeeID = "...";
                    zItem.EmployeeName = "...";
                    zList.Add(zItem);
                }

            }
            return new JsonResult(zList);
        }
        public IActionResult OnPostSearchEmployee([FromBody] ItemRequest request)
        {
            CheckAuth();
            JsonResult zResult = new JsonResult("");
            string zMessageLog = "";
            ItemView zItem = new ItemView();
            if (request.SearchContent.Trim().Length == 0)
            {
                zItem.ID = "ERROR";
            }
            else
            {
                string[] zInfo = Models.Categories_AccessData.GetEmployeeByID(request.SearchContent);
                if (zInfo[0] == null)
                {
                    zItem.ID = "ERROR";
                }
                else
                {
                    zItem.ID = "OK";
                    zItem.Name = zInfo[1];
                }
            }
            return new JsonResult(zItem);
        }
        public class ItemRequest
        {
            public string MonthView { get; set; }
            public string TRCD { get; set; }
            public string SearchContent { get; set; }
            public string Key { get; set; }
            public string EmployeeID { get; set; }
            public string DepartmentID { get; set; }
        }
        public class ItemView
        {
            public string Key { get; set; }
            public string ID { get; set; }
            public string Name { get; set; }
            public string Money { get; set; }
            public string trcd { get; set; }
            public string DepartmentID { get; set; }
            public string EmployeeID { get; set; }
            public string EmployeeName { get; set; }

            public string Code { get; set; }
            public ItemView() { }

        }
    }
}
