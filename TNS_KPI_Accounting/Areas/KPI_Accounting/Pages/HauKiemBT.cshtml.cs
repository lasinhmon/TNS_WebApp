using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Security.Claims;

namespace TNS_KPI.Areas.KPI_Accounting.Pages
{
    [IgnoreAntiforgeryToken]
    public class HauKiemBTModel : PageModel
    {
        private int[] _ViewField;
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
            _PageSize = 25;
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
            string zListField = "col_buscd,col_bchkcd,col_stscd,col_troffcd,col_opndt,col_glseq,col_crtusr,col_crtdt,col_bchkusr,col_bchknm,Index_3,Index_8,Index_9,Index_15,name_2";
            string zListFieldNote = "C,E,J,K,L,M,S,W,AA,AB,AE,AJ,AK,AQ,AW";
            zItem.FieldsName = new List<string>();
            zItem.FieldsNote = new List<string>();
            string[] zTempName = zListField.Split(',');
            string[] zTempNote = zListFieldNote.Split(',');

            _ViewField = new int[zTempName.Length];

            string zWhere = " WHERE InMonth = " + request.MonthView + " AND InYear = " + request.YearView + " ";
            if (request.UserID.Trim().Length > 0)
            {
                zWhere += "AND col_crtusr ='" + request.UserID.Trim() + "' ";
            }
            if (request.Module.Trim().Length > 0)
            {
                zWhere += "AND col_buscd ='" + request.Module.Trim() + "' ";
            }
            if (request.CodeNV.Trim().Length > 0)
            {
                zWhere += "AND col_bchkcd ='" + request.CodeNV.Trim() + "' ";
            }
            if(request.Repeat != null)
            {
                if(request.Repeat.Contains('>'))
                {
                    string zTemp = request.Repeat.Replace('>', ' ');
                    zWhere += "AND RepeatCalculator > " + zTemp.Trim() + " ";
                }
                else
                {
                    zWhere += "AND RepeatCalculator = " + request.Repeat.Trim() + " ";
                }
            }
            
            if (request.SearchContent.Trim().Length > 0)
            {
                zWhere += "AND ";
                request.SearchContent = ReplaceFieldName(request.SearchContent, '=', " LIKE ", zTempName, zTempNote);
                request.SearchContent = ReplaceFieldName(request.SearchContent, '#', " NOT LIKE ", zTempName, zTempNote);

                request.SearchContent = request.SearchContent.Replace("&", " AND ");
                request.SearchContent = request.SearchContent.Replace("|", " NOT ");
                zWhere += request.SearchContent;
            }


            string zSQL_Count = "SELECT Count(*) FROM [dbo].[KPI_Accounting_HauKiemBT] " + zWhere;

            zTotal_Record = Models.Categories_AccessData.DataCount(zSQL_Count);
            zTotal_PageNumber = zTotal_Record / _PageSize;
            if (zTotal_Record % _PageSize != 0)
                zTotal_PageNumber++;
            if (zTotal_PageNumber > 12)
                zTotal_PageNumber = 12;

            zItem.Info = new ItemInfo();
            zItem.Info.Name = "OK";
            zItem.Info.PageSize = _PageSize.ToString();
            zItem.Info.PageNumber = zPageNumber.ToString();
            zItem.Info.PageTotal = zTotal_PageNumber.ToString();
            zItem.Info.RecordTotal = zTotal_Record.ToString("#,###,##0");
            _ViewField[0] = 1;
            _ViewField[1] = 1;
            _ViewField[2] = 1;

            zListField = "";
            for (int i = 0; i < zTempName.Length; i++)
            {
                if (_ViewField[i] == 1)
                {
                    zItem.FieldsName.Add(zTempName[i]);
                    zItem.FieldsNote.Add(zTempNote[i]);
                    zListField += zTempName[i] + ",";
                }
            }
            for (int i = 0; i < zTempName.Length; i++)
            {
                if (_ViewField[i] != 1)
                {
                    zItem.FieldsName.Add(zTempName[i]);
                    zItem.FieldsNote.Add(zTempNote[i]);
                    zListField += zTempName[i] + ",";
                }
            }
            zListField = zListField.Substring(0, zListField.Length - 1);
            string zSQL = "SELECT " + zListField + " FROM [dbo].[KPI_Accounting_HauKiemBT] " + zWhere + " ORDER BY col_buscd ";

            List<int> zFieldsIsDate = new List<int>();
            zFieldsIsDate.Add(4);
            zFieldsIsDate.Add(6);

            zItem.DataOfTable = Models.Categories_AccessData.DataOfTable(zSQL, zFieldsIsDate, zPageNumber, _PageSize);

            return new JsonResult(zItem);
        }

        public IActionResult OnPostLoadDataGroupBy([FromBody] ItemRequest request)
        {
            CheckAuth();
            _PageSize = 50;
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
            string zListField = "col_buscd,col_bchkcd,col_stscd,col_troffcd,col_opndt,col_glseq,col_crtusr,col_crtdt,col_bchkusr,col_bchknm,Index_3,Index_8,Index_9,Index_15,name_2";
            string zListFieldNote = "C,E,J,K,L,M,S,W,AA,AB,AE,AJ,AK,AQ,AW";
            zItem.FieldsName = new List<string>();
            zItem.FieldsNote = new List<string>();
            string[] zTempName = zListField.Split(',');
            string[] zTempNote = zListFieldNote.Split(',');

            _ViewField = new int[zTempName.Length];

            string zWhere = " WHERE InMonth = " + request.MonthView + " AND InYear = " + request.YearView + " ";
            if (request.UserID.Trim().Length > 0)
            {
                zWhere += "AND col_crtusr ='" + request.UserID.Trim() + "' ";
            }
            if (request.Module.Trim().Length > 0)
            {
                zWhere += "AND col_buscd ='" + request.Module.Trim() + "' ";
            }
            if (request.CodeNV.Trim().Length > 0)
            {
                zWhere += "AND col_bchkcd ='" + request.CodeNV.Trim() + "' ";
            }
            if (request.SearchContent.Trim().Length > 0)
            {
                zWhere += "AND ";
                request.SearchContent = ReplaceFieldName(request.SearchContent, '=', " LIKE ", zTempName, zTempNote);
                request.SearchContent = ReplaceFieldName(request.SearchContent, '#', " NOT LIKE ", zTempName, zTempNote);

                request.SearchContent = request.SearchContent.Replace("&", " AND ");
                request.SearchContent = request.SearchContent.Replace("|", " NOT ");
                zWhere += request.SearchContent;
            }

            zItem.Info = new ItemInfo();
            zItem.Info.Name = "OK";
            zItem.Info.PageSize = _PageSize.ToString();
            zItem.Info.PageNumber = "1";
            zItem.Info.PageTotal = "1";
            zItem.Info.RecordTotal = zTotal_Record.ToString("#,###,##0");

            zListField = "CAST(CASE WHEN col_crtdt IS NOT NULL THEN col_crtdt ELSE col_opndt END AS date) AS WorkingDate,"
                + "COUNT(*) AS Quantity, ";
            if (request.UserID.Trim().Length == 0)
            {
                zListField += " 0 AS AtCounter";

            }
            else
            {
                zListField += " [dbo].[KPI_Accounting_WorkingAtLocation](CAST(CASE WHEN col_crtdt IS NOT NULL THEN col_crtdt ELSE col_opndt END AS date),'" + request.UserID.Trim() + "') AS AtCounter ";

            }


            zItem.FieldsName.Add("WorkingDate");
            zItem.FieldsName.Add("Quantity");
            zItem.FieldsName.Add("AtCounter");

            zItem.FieldsNote.Add("1");
            zItem.FieldsNote.Add("2");
            zItem.FieldsNote.Add("3");

            string zGroupBy = " CAST(CASE WHEN col_crtdt IS NOT NULL THEN col_crtdt ELSE col_opndt END AS date) ";
            string zOrderBy = " CAST(CASE WHEN col_crtdt IS NOT NULL THEN col_crtdt ELSE col_opndt END AS date) ";
            string zSQL = "SELECT " + zListField + " FROM [dbo].[KPI_Accounting_HauKiemBT] " + zWhere
                + " GROUP BY " + zGroupBy
                + " ORDER BY " + zOrderBy;

            List<int> zFieldsIsDate = new List<int>();
            zFieldsIsDate.Add(0);

            zItem.DataOfTable = Models.Categories_AccessData.DataOfTable(zSQL, zFieldsIsDate);

            return new JsonResult(zItem);
        }
        public IActionResult OnPostSearchEmployee([FromBody] ItemRequest request)
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

        public IActionResult OnPostGeneralAnalytics([FromBody] ItemRequest request)
        {
            CheckAuth();
            List<string[]> zData = Models.AnalyticsData.ReportQuick_HauKiemBT(int.Parse(request.MonthView), int.Parse(request.YearView));
            return new JsonResult(zData);
        }
        public IActionResult OnPostAnalyticsByModule([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";

            ItemView zItem = new ItemView();
            List<string[]> zData = Models.AnalyticsData.ReportQuick_HauKiemBT_ByModule(int.Parse(request.MonthView), int.Parse(request.YearView));
            return new JsonResult(zData);
        }


        private string GetFieldName(string ID, string[] ListField, string[] ListID)
        {
            string zResult = "";
            int i = 0;
            foreach (string zID in ListID)
            {
                if (zID == ID)
                {
                    zResult = ListField[i];
                    _ViewField[i] = 1;
                    break;
                }
                i++;
            }
            return zResult;
        }
        private string ReplaceFieldName(string Orgizin, char Symbol, string ValReplace, string[] ListField, string[] ListID)
        {
            string zResult = "";
            int k = 0;
            int n = 0;
            int j = 0;
            string zFieldID = "";
            string zFieldName = "";
            while (true)
            {
                k = Orgizin.IndexOf(Symbol);
                if (k == -1) { break; }
                else
                {
                    j = k;
                    if (Orgizin[j - 1] == ' ')
                    {
                        j--;
                    }
                    j--;
                    if (j > 0)
                    {
                        if (Orgizin[j - 1] != ' ')
                        {
                            j--;
                        }
                    }
                    zFieldID = Orgizin.Substring(j, k - j).Trim();
                    zFieldName = GetFieldName(zFieldID, ListField, ListID);
                    n = Orgizin.Length - k;
                    if (zFieldName.Length > 0)
                    {
                        Orgizin = Orgizin.Substring(0, j) + zFieldName + ValReplace + Orgizin.Substring(k + 1, n - 1);
                    }
                }

            }
            zResult = Orgizin;
            return zResult;
        }
        public class ItemRequest
        {
            public string MonthView { get; set; }
            public string YearView { get; set; }
            public string UserID { get; set; }
            public string Module { get; set; }
            public string CodeNV { get; set; }
            public string SearchContent { get; set; }
            public string Repeat { get; set; }
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
            public string RecordTotal { get; set; }
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
