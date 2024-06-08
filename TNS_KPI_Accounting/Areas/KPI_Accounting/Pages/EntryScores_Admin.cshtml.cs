using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using System.Xml.Linq;
using TNS.Auth;

namespace TNS_KPI.Areas.KPI_Accounting.Pages
{
    [IgnoreAntiforgeryToken]
    public class EntryScores_AdminModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
            UserLogin.GetRole("KPI-Accountant_Scores_Admin");
        }
        #endregion
        public List<string[]> ListDepartment = Models.Categories_AccessData.DepartmentForControl();
        public IActionResult OnGet()
        {
            CheckAuth();
            if (UserLogin.Role.IsRead)
            {
                return Page();
            }
            else
                return LocalRedirect("~/Warning?id=403");

        }

        public IActionResult OnPostView([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            int zMonth = int.Parse(request.MonthView.Substring(0, 2));
            int zYear = int.Parse(request.MonthView.Substring(3, 4));
            DateTime zMonthView = new DateTime(zYear, zMonth, 01);
            if (request.Action == "Next")
                zMonthView = zMonthView.AddMonths(1);
            if (request.Action == "Pre")
                zMonthView = zMonthView.AddMonths(-1);
            if (request.Action == "Now")
                zMonthView = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            List<ItemView> zList = new List<ItemView>();
            ItemView zItem = new ItemView();
            if (zMessageLog.Length > 0)
            {
                zItem = new ItemView();
                zItem.ID = "ERROR";
                zItem.Name = zMessageLog;
                zList.Add(zItem);
            }
            else
            {
                string btn_Edit = "";
                string btn_Del = "";
                zItem = new ItemView();
                zItem.ID = zMonthView.ToString("MM-yyyy");
                zList.Add(zItem);

                List<string[]> zListEmployee = Models.AccessData.HauKiemBT_GetEmployee(zMonthView.Month, zMonthView.Year);
                //List<string[]> zListEmployee = Models.AccessData.Accouting_Result(zMonthView.Month, zMonthView.Year);
                foreach (string[] zInfo in zListEmployee)
                {
                    zItem = new ItemView();
                    zItem.UserName = zInfo[0];
                    zItem.EmployeeID = zInfo[1];
                    zItem.EmployeeName = zInfo[2];
                    
                    zList.Add(zItem);
                }
            }
            return new JsonResult(zList);
        }
        
        public IActionResult OnPostCollectInfo([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            List<ItemView> zList = new List<ItemView>();
            ItemView zItem = new ItemView();
            if (request.EmployeeSearch.Length == 0)
            {
                zItem.ID = "ERROR";
                zItem.Name = "Vui lòng nhập thông tin cần tìm";
                zList.Add(zItem);
            }
            else
            {
                List<string[]> zListEmployee = Models.Categories_AccessData.GetEmployeeSearch(request.EmployeeSearch);
                if (zListEmployee.Count > 0)
                {
                    if (zListEmployee[0][0] == "ERROR")
                    {
                        zItem = new ItemView();
                        zItem.ID = "ERROR";
                        zItem.Name = zListEmployee[0][1];
                        zList.Add(zItem);
                    }
                    else
                    {
                        zItem = new ItemView();
                        zItem.ID = "OK";
                        zItem.Name = "";
                        zList.Add(zItem);
                        foreach (string[] zInfo in zListEmployee)
                        {
                            zItem = new ItemView();
                            zItem.UserName = zInfo[0];
                            zItem.EmployeeID = zInfo[1];
                            zItem.EmployeeName = zInfo[2];
                            zList.Add(zItem);
                        }
                    }
                }
                else
                {
                    zItem = new ItemView();
                    zItem.ID = "NONE";
                    zItem.Name = "";
                    zList.Add(zItem);
                }
            }
            return new JsonResult(zList);
        }
        public IActionResult OnPostGetInfo([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            ItemView zItem = new ItemView();
            /* Models.Scores_Record zScores;
             if (request.AutoKey.Length == 36)
                 zScores = new Models.Scores_Record(request.AutoKey);
             else
                 zScores = new Models.Scores_Record();

             zItem.Key = zScores.AutoKey;
             zItem.EmployeeKey = zScores.EmployeeKey;
             zItem.EmployeeID = zScores.EmployeeID;
             zItem.EmployeeName = zScores.EmployeeName;
             zItem.DepartmentKey = zScores.DepartmentKey;
             zItem.DepartmentName = zScores.DepartmentName;
             zItem.RankingInMonth = zScores.RankingInMonth.ToString();

             int zAccountingEntry_Done = int.Parse(zScores.AccountingEntry_Done.ToString());
             int zAccountingEntry_Cancel = int.Parse(zScores.AccountingEntry_Cancel.ToString());
             int zAccountingEntry_Agritax = int.Parse(zScores.AccountingEntry_Agritax.ToString());
             int zAccountingEntry_None = int.Parse(zScores.AccountingEntry_None.ToString());
             double zCoefficientTask = double.Parse(zScores.CoefficientTask.ToString());
             zItem.AccountingEntry_Done = zAccountingEntry_Done.ToString("#,###,###");
             zItem.AccountingEntry_Cancel = zAccountingEntry_Cancel.ToString("#,###,###");
             zItem.AccountingEntry_Agritax = zAccountingEntry_Agritax.ToString("#,###,###");
             zItem.AccountingEntry_None = zAccountingEntry_None.ToString("#,###,###");
             zItem.CoefficientTask = zCoefficientTask.ToString("#,###,###.00");

             zItem.Scores_AccountingEntry = zScores.Scores_AccountingEntry.ToString();
             zItem.Scores_Plus = zScores.Scores_Plus.ToString();
             zItem.Scores_Total = zScores.Scores_Total.ToString();
             zItem.Note = zScores.Note;
            */
            return new JsonResult(zItem);
        }
        public IActionResult OnPostUpdate([FromBody] ItemView itemUpdate)
        {
            CheckAuth();
            string zMessageLog = "";
            int zMonth = int.Parse(itemUpdate.MonthUpdate.Substring(0, 2));
            int zYear = int.Parse(itemUpdate.MonthUpdate.Substring(3, 4));

            /*Models.Scores_Record zScores = new Models.Scores_Record();
            zScores.AutoKey = itemUpdate.Key;
            zScores.InMonth = zMonth;
            zScores.InYear = zYear;
            zScores.EmployeeKey = itemUpdate.EmployeeKey;
            zScores.EmployeeID = itemUpdate.EmployeeID;
            zScores.EmployeeName = itemUpdate.EmployeeName;
            zScores.DepartmentKey = itemUpdate.DepartmentKey;
            zScores.DepartmentName = itemUpdate.DepartmentName;
            zScores.RankingInMonth = int.Parse(itemUpdate.RankingInMonth);
            zScores.AccountingEntry_Done = int.Parse(itemUpdate.AccountingEntry_Done);
            zScores.AccountingEntry_Cancel = int.Parse(itemUpdate.AccountingEntry_Cancel);
            zScores.AccountingEntry_Agritax = int.Parse(itemUpdate.AccountingEntry_Agritax);
            zScores.AccountingEntry_None = int.Parse(itemUpdate.AccountingEntry_None);
            zScores.CoefficientTask = float.Parse(itemUpdate.CoefficientTask);
            zScores.Scores_AccountingEntry = int.Parse(itemUpdate.Scores_AccountingEntry);
            zScores.Scores_Plus = int.Parse(itemUpdate.Scores_Plus);
            zScores.Scores_Total = int.Parse(itemUpdate.Scores_Total);
            zScores.Note = itemUpdate.Note;
            if (zScores.Note == null)
                zScores.Note = "";

            if (zScores.AutoKey.Trim().Length == 36)
            {
                if (UserLogin.Role.IsEdit)
                {
                    zScores.Update();
                }
                else
                {
                    itemUpdate.ID = "ERROR";
                    itemUpdate.Name = "Bạn không có quyền sửa thông tin !";
                }
            }
            else
            {
                if (UserLogin.Role.IsAdd)
                {
                    zScores.Create_ClientKey();
                    itemUpdate.Key = zScores.AutoKey;
                }
                else
                {
                    itemUpdate.ID = "ERROR";
                    itemUpdate.Name = "Bạn không có quyền thêm thông tin !";
                }
            }
            if (itemUpdate.ID != "ERROR")
            {
                itemUpdate.Code = zScores.Code;

                if (zScores.Code == "501")
                {
                    itemUpdate.ID = "ERROR";
                    itemUpdate.Name = zScores.Message;
                }
            }
            */
            return new JsonResult(itemUpdate);
        }
        public IActionResult OnPostDelete([FromBody] ItemRequest request)
        {
            CheckAuth();
            ItemView zItem = new ItemView();

            if (UserLogin.Role.IsDelete)
            {
                string zMessageLog = "";

                /*Models.Scores_Record zScore;
                if (request.AutoKey.Length == 36)
                {
                    zScore = new Models.Scores_Record();
                    zScore.AutoKey = request.AutoKey;
                    zScore.Delete();
                    zItem.Code = zScore.Code;
                    if (zScore.Code != "200")
                    {
                        zItem.ID = "ERROR";
                        zItem.Name = zScore.Message;
                    }
                    else
                    {
                        zItem.ID = "OK";
                    }
                }
                else
                {
                    zItem.ID = "ERROR";
                    zItem.Name = "Key not correct format";
                }
                */
            }
            else
            {
                zItem.ID = "ERROR";
                zItem.Name = "Bạn không có quyền xóa thông tin";
            }

            return new JsonResult(zItem);
        }
        public class ItemRequest
        {
            public string MonthView { get; set; }
            public string Action { get; set; }
            public string AutoKey { get; set; }
            public string EmployeeSearch { get; set; }
        }
        public class ItemView
        {
            public string Key { get; set; }
            public string ID { get; set; }
            public string Name { get; set; }
            public string MonthUpdate { get; set; }
            public string UserName { get; set; }
            public string EmployeeName { get; set; }
            public string EmployeeID { get; set; }
            public string TRCD { get; set; }
            public string DepartmentName { get; set; }
            public string RankingInMonth { get; set; }
            public string AccountingEntry_Done { get; set; }
            public string AccountingEntry_Cancel { get; set; }
            public string AccountingEntry_Agritax { get; set; }
            public string AccountingEntry_None { get; set; }
            public string CoefficientTask { get; set; }
            public string Scores_AccountingEntry { get; set; }
            public string Scores_Plus { get; set; }
            public string Scores_Total { get; set; }
            public string Note { get; set; }
            public string btnAction { get; set; }
            public string Code { get; set; }
            public ItemView() { }

        }
    }
}
