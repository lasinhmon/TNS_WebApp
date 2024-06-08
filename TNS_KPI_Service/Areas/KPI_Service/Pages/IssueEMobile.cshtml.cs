using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace TNS_KPI.Areas.KPI_Service.Pages
{
    [IgnoreAntiforgeryToken]
    public class IssueEMobileModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion
        private int _PageSize = 25;
        public List<string[]>? zListPGD = Models.Categories_AccessData.EMobile_GetPGD(DateTime.Now.Month, DateTime.Now.Year);
        public List<string[]>? zListSTTS = Models.Categories_AccessData.EMobile_GetSTTS(DateTime.Now.Month, DateTime.Now.Year);
        public void OnGet(string styleView)
        {
            CheckAuth();

        }
        public IActionResult OnPostSaveUser([FromBody] ItemView itemUpdate)
        {
            string zMessageLog = "";
            Models.EMobile_Model eMobile_Model = new Models.EMobile_Model();
            eMobile_Model.Crtusr = itemUpdate.crtusr;
            eMobile_Model.Pgd = itemUpdate.pgd;
            eMobile_Model.Rmk = itemUpdate.rmk;
            zMessageLog = eMobile_Model.Save();
            // eMobile_Model.EmployeeID = itemUpdate.ID;
            //  eMobile_Model.FirstName = itemUpdate.FirstName;
            //  eMobile_Model.LastName = itemUpdate.LastName;


            //  itemUpdate.Name = zEmployee.LastName + " " + zEmployee.FirstName;
            //  if (zEmployee.EmployeeKey.Trim().Length == 36)// neu co id nghia la updatte , còn k la create
            //  {
            //      zEmployee.Update();

            //  }
            itemUpdate.Code = zMessageLog;
            return new JsonResult(itemUpdate);
        }
        public IActionResult OnPostCreateUser([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            Models.EMobile_Model eMobile_Model;
            eMobile_Model = new Models.EMobile_Model();
            ItemView zItem = new ItemView();
            return new JsonResult(zItem);
        }
        public IActionResult OnPostDelete([FromBody] ItemView itemUpdate)
        {
            string zMessageLog = "";
            zMessageLog = Models.Categories_AccessData.Delete(itemUpdate.crtusr);
            itemUpdate.Code = zMessageLog;
            return new JsonResult(itemUpdate);
        }
        public IActionResult OnPostUpdate([FromBody] ItemView itemUpdate)
        {
            //CheckAuth();
            string zMessageLog = "";
            zMessageLog = Models.Categories_AccessData.Update(itemUpdate.crtusr, itemUpdate.pgd);

            /*  Models.Employee_Record zEmployee = new Models.Employee_Record();
              zEmployee.EmployeeKey = itemUpdate.Key;
              zEmployee.EmployeeID = itemUpdate.ID;
              zEmployee.FirstName = itemUpdate.FirstName;
              zEmployee.LastName = itemUpdate.LastName;


              itemUpdate.Name = zEmployee.LastName + " " + zEmployee.FirstName;
              if (zEmployee.EmployeeKey.Trim().Length == 36)// neu co id nghia la updatte , còn k la create
              {
                  zEmployee.Update();

              }
              else
              {
                  zEmployee.Create_ServerKey();
                  itemUpdate.Key = zEmployee.EmployeeKey;
              }
              itemUpdate.Code = zEmployee.Code;

              if (zEmployee.Code == "501")
              {
                  itemUpdate.ID = "ERROR";
                  itemUpdate.Name = zEmployee.Message;
              }*/
            itemUpdate.Code = zMessageLog;
            //if(itemUpdate.Code=="501")
            return new JsonResult(itemUpdate);
            //  return null;
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
            string zListField = "crtusr,entydt,stts,rmk,custseq,mblno1,crtdtm, nmloc, custtpcd ";
            //string zFieldName = "Username,Ngay Nhap Lieu, Status, Remark ";

            string zWhere = " WHERE InMonth = " + request.MonthView + " AND InYear = " + request.YearView + "AND stts = '" + request.StatusView + "'";
            if (request.SearchContent.Trim().Length > 0)
                zWhere += " AND pgd='" + request.SearchContent + "'";
            string zSQL = "SELECT " + zListField + " FROM [dbo].[KPI_Service_IssueEMobile] " + zWhere + " ORDER BY entydt DESC";
            string zSQL_Count = "SELECT Count(*) FROM [dbo].[KPI_Service_IssueEMobile] " + zWhere;


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

            /* List<string[]> zButton = new List<string[]>();
             string[] zBtn;
             int n = 1;
             for (int i = 0; i < zItem.DataOfTable.Count; i++)
             {

                 zBtn = new string[n];
                 string btn_Edit = "";
                 for (int j = 0; j < n; j++)
                 {
                     btn_Edit = "<button type='button' class='btn btn-primary' onclick=\"Modal_Update_Show(" + j.ToString() + ",'Edit')\">";
                     btn_Edit += "   <i class='uil uil-pen font-size-18'></i>";
                     btn_Edit += "</button>";
                     zBtn[i] = btn_Edit;
                 }

                 zItem.btnAction.Add(zBtn);

             }*/

            //for (int i = 0; i < zItem.DataOfTable.Count; i++)
            //{
            //  string[] zBtn=new string[2];

            //}

            //  zItem.DataOfTable.Add()

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
            List<string[]> zData = Models.AnalyticsData.ReportQuick_EMobile(int.Parse(request.MonthView), int.Parse(request.YearView), request.StatusView);
            return new JsonResult(zData);
        }
        public IActionResult OnPostSearchPGD([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            DataTable zListNotice = new DataTable();
            ItemView zItem = new ItemView();
            string[] zDateConvert;
            int zMonth = int.Parse(request.MonthView);
            int zYear = int.Parse(request.YearView);
            zListPGD = Models.Categories_AccessData.EMobile_GetPGD(zMonth, zYear);

            return new JsonResult(zListPGD);
        }
        public class ItemRequest
        {
            public string MonthView { get; set; }
            public string YearView { get; set; }
            public string SearchContent { get; set; }
            public string PageSelected { get; set; }
            public string Key { get; set; }
            public string StatusView { get; set; }
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
            public string crtusr { get; set; }
            public string pgd { get; set; }
            public string rmk { get; set; }

            public List<string> FieldsName { get; set; }
            public List<string> FieldsNote { get; set; }
            public List<string[]> DataOfTable { get; set; }
            // public List<string[]> btnAction { get; set; }
            public string Code { get; set; }
            public ItemView() { }

        }
    }
}
