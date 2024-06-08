using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using System.Xml.Linq;

namespace TNS_KPI.Areas.KPI_Accounting.Pages
{
    [IgnoreAntiforgeryToken]
    public class EntryScoresModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
           // UserLogin.GetRole("Employee/EmployeeList");
        }
        #endregion

        public string EmployeeName { get; set; }
        public List<string[]> ListDepartment = Models.Categories_AccessData.DepartmentForControl();
        public IActionResult OnGet()
        {
            CheckAuth();
            EmployeeName = UserLogin.Employee.Name.ToUpper();
            return Page();

        }
        public IActionResult OnPostView([FromBody] ItemRequest request)
        {
            CheckAuth();
            int zYearView = int.Parse(request.YearView);
            if (request.Action == "Next")
                zYearView = zYearView + 1;
            if (request.Action == "Pre")
                zYearView = zYearView - 1;
            if (request.Action == "Now")
                zYearView = DateTime.Now.Year;

            string zMessageLog = "";
            DataTable zDataScores = Models.AccessData.ScoresOfYear(UserLogin.UserName, zYearView, out zMessageLog);
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
                zItem = new ItemView();
                zItem.ID = zYearView.ToString();
                zList.Add(zItem);
                int i = 1;
                for (int j = 1; j <= 12; j++)
                {
                    zItem = new ItemView();
                    zItem.Name = j.ToString().PadLeft(2, '0') + "-" + zYearView.ToString();
                    zItem.Ranking = "";
                    zItem.ButToanThucHien = "";
                    zItem.ButToanHuy = "";
                    zItem.ButToanAgritax = "";
                    zItem.ButToanKhongHachToan = "";
                    zItem.ButToanPHUB = "";
                    zItem.HauKiemButToan = "";
                    zItem.TongDiemQuyDoi = "";
                    zItem.DiemCongPhuTrach = "";
                    zItem.HeSoCongViec = "";
                    zItem.GDV_KS_ToTruong_PhuTrach = "";
                    zItem.DiemThucHien = "";
                    zItem.DiemBTBQ = "";
                    zItem.DiemBTBQ_Rate = "";
                    zItem.Rate_Target = "";
                    zItem.Note = "";
                    foreach (DataRow zRow in zDataScores.Rows)
                    {
                        if (zRow["InMonth"].ToString() == j.ToString())
                        {
                            zItem.Ranking = zRow["Ranking"].ToString(); ;
                            zItem.ButToanThucHien = zRow["ButToanThucHien"].ToString(); ;
                            zItem.ButToanHuy = zRow["ButToanHuy"].ToString(); ;
                            zItem.ButToanAgritax = zRow["ButToanAgritax"].ToString(); ;
                            zItem.ButToanKhongHachToan = zRow["ButToanKhongHachToan"].ToString(); ;
                            zItem.ButToanPHUB = zRow["ButToanPHUB"].ToString(); ;
                            zItem.HauKiemButToan = zRow["HauKiemButToan"].ToString(); ;
                            zItem.TongDiemQuyDoi = zRow["TongDiemQuyDoi"].ToString(); ;
                            zItem.DiemCongPhuTrach = zRow["DiemCongPhuTrach"].ToString(); ;
                            zItem.HeSoCongViec = zRow["HeSoCongViec"].ToString(); ;
                            zItem.GDV_KS_ToTruong_PhuTrach = zRow["GDV_KS_ToTruong_PhuTrach"].ToString(); ;
                            zItem.DiemThucHien = zRow["DiemThucHien"].ToString(); ;
                            zItem.DiemBTBQ = zRow["DiemBTBQ"].ToString(); ;
                            zItem.DiemBTBQ_Rate = zRow["DiemBTBQ_Rate"].ToString(); ;
                            zItem.Rate_Target = zRow["Rate_Target"].ToString(); ;
                            zItem.Note = zRow["Note"].ToString(); ;

                            break;
                        }
                    }
                    
                    zList.Add(zItem);
                    i++;
                }

            }
            return new JsonResult(zList);
        }
        public IActionResult OnPostGetInfo([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            ItemView zItem = new ItemView();
            /*Models.Scores_Record zScores;
            if (request.AutoKey.Length == 36)
                zScores = new Models.Scores_Record(request.AutoKey);
            else
                zScores = new Models.Scores_Record();

            zItem.Key = zScores.AutoKey;
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
        public class ItemRequest
        {
            public string YearView { get; set; }
            public string Action { get; set; }
            public string AutoKey { get; set; }
            public string EmployeeID { get; set; }
        }
        public class ItemView
        {
            public string Key { get; set; }
            public string ID { get; set; }
            public string Name { get; set; }
            public string Ranking { get; set; }
            public string ButToanThucHien { get; set; }
            public string ButToanHuy { get; set; }
            public string ButToanAgritax { get; set; }
            public string ButToanKhongHachToan { get; set; }
            public string ButToanPHUB { get; set; }
            public string HauKiemButToan { get; set; }
            public string TongDiemQuyDoi { get; set; }
            public string DiemCongPhuTrach { get; set; }
            public string HeSoCongViec { get; set; }
            public string GDV_KS_ToTruong_PhuTrach { get; set; }
            public string DiemThucHien { get; set; }
            public string DiemBTBQ { get; set; }
            public string DiemBTBQ_Rate { get; set; }
            public string Rate_Target { get; set; }
            public string Note { get; set; }
            public string btnAction { get; set; }
            public string Code { get; set; }
            public ItemView() { }

        }
    }
}
