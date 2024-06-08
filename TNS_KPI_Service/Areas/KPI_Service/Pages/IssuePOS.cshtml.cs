using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Claims;

namespace TNS_KPI.Areas.KPI_Service.Pages
{
    [IgnoreAntiforgeryToken]
    public class IssuePOSModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion
        private int _PageSize = 25;
        public readonly List<string[]> FileIds = Models.Categories_AccessData.GetFileId();
        public IActionResult OnPostLoadTrandateByFileId([FromBody] ItemRequest request)
        {
            string zMessage = "";
            string zSQL = "SELECT distinct Trandate  "
                        + " FROM [dbo].[KPI_Service_IssuePOS] where fileid=@fileid ";
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;

                zCommand.Parameters.Add("@fileid", SqlDbType.NVarChar).Value = request.SearchContent;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            if (zMessage.Length > 0)
            {
                zItem = new string[3];
                zItem[0] = "ERROR";
                zItem[1] = "zMessage";
                zResult.Add(zItem);
            }
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                zResult.Add(zItem);
            }

            return new JsonResult(zResult);
        }
        public void OnGet(string styleView)
        {
            CheckAuth();

        }

       // LoadDataByField
        public IActionResult OnPostLoadDataByField([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            DataTable zListNotice = new DataTable();
           // Object tam = request.checkedValues;
            ItemView zItem = new ItemView();
            int zPageNumber = 1;
            int zTotal_Record = 0;
            int zTotal_PageNumber = 0;
            string[] zDateConvert;

            if (request.PageSelected.Trim().Length > 0)
            {
                zPageNumber = int.Parse(request.PageSelected.Substring(4, request.PageSelected.Length - 4));
            }
            
            //string zListField = " fileid,term,device,trandate, issfiid,mcc";
            string zListField = "SoThe,fileid,NgayGiaoDich,device,trandate,creditacct, issfiid,acqfiid,retailer,mcc,userdata,NgayHachToan,PhiCKChuaVat,Vat";
            string zWhere = " WHERE InMonth = " + request.MonthView + " AND InYear = " + request.YearView;
            if (request.SearchContent.Trim().Length > 0)
                zWhere += " AND " + request.SearchContent;
            string zSQL = "SELECT CONCAT(LEFT(trdt,4),'****_****_****',RIGHT(trdt,4)) as SoThe ,fileid,FORMAT(term,'dd/MM/yyyy') AS NgayGiaoDich," +
                "device,trandate,issfiid,acqfiid,retailer,mcc,userdata,FORMAT(CONVERT(DATE,CONVERT(VARCHAR,mcc,112),112),'dd/MM/yyyy') AS NgayHachToan," +
                "ROUND(cast([userdata] as float)/110*100,0) AS PhiCKChuaVat, " +
                "ROUND(cast([userdata] as float)/110*10,0) AS Vat " +
                "FROM [dbo].[KPI_Service_IssuePOS] " + zWhere + " ORDER BY Userdata DESC";


            string zSQL_Count = "SELECT Count(*) FROM [dbo].[KPI_Service_IssuePOS] " + zWhere;


            zItem.FieldsName = new List<string>();
            zItem.FieldsNote = new List<string>();
            string[] zTempName = zListField.Split(',');
            string[] zTempIndex = request.checkedValues;
            foreach(string zindex in zTempIndex)
            {
                if(int.TryParse(zindex,out int index) && index >=0 && index < zTempName.Length)
                zItem.FieldsName.Add(zTempName[index]);
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
            string[] a;
            if (zItem.DataOfTable.Count > 0)
            {
                List<string[]> filteredData = new List<string[]>();
                foreach (var row in zItem.DataOfTable)
                {
                    List<string> newRow = new List<string>();
                    foreach (var index in zTempIndex)
                    {
                        int columnIndex = int.Parse(index); // Chuyển đổi chỉ mục từ string sang int
                        newRow.Add(row[columnIndex]);
                    }
                    filteredData.Add(newRow.ToArray());
                }

                zItem.DataOfTable = filteredData;


                // Now you can work with the first string[] array
            }
            else
            {
                // Handle the case when DataOfTable is empty
            }

            
            return new JsonResult(zItem);
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
            //string zListField = " fileid,term,device,trandate, issfiid,mcc";
            string zListField = "SoThe,fileid,NgayGiaoDich,device,trandate, issfiid,acqfiid,retailer,mcc,userdata,NgayHachToan,PhiCKChuaVat,Vat";
            string zWhere = " WHERE InMonth = " + request.MonthView + " AND InYear = " + request.YearView;
            if (request.SearchContent.Trim().Length > 0)
                zWhere += " AND " + request.SearchContent;
            string zSQL = "SELECT CONCAT(LEFT(trdt,4),'****_****_****',RIGHT(trdt,4)) as SoThe ,fileid,FORMAT(term,'dd/MM/yyyy') AS NgayGiaoDich," +
                "device,trandate,issfiid,acqfiid,retailer,mcc,userdata,FORMAT(CONVERT(DATE,CONVERT(VARCHAR,mcc,112),112),'dd/MM/yyyy') AS NgayHachToan," +
                "ROUND(cast([userdata] as float)/110*100,0) AS PhiCKChuaVat, " +
                "ROUND(cast([userdata] as float)/110*10,0) AS Vat "+
                "FROM [dbo].[KPI_Service_IssuePOS] " + zWhere + " ORDER BY Userdata DESC";


            string zSQL_Count = "SELECT Count(*) FROM [dbo].[KPI_Service_IssuePOS] " + zWhere;


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
            List<string[]> zData = new List<string[]>();// Models.AnalyticsData.ReportQuick_PHUB(int.Parse(request.MonthView), int.Parse(request.YearView));
            return new JsonResult(zData);
        }
        public class ItemRequest
        {
            public string MonthView { get; set; }
            public string YearView { get; set; }
            public string SearchContent { get; set; }
            public string PageSelected { get; set; }
            public string[] checkedValues { get; set; }
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
