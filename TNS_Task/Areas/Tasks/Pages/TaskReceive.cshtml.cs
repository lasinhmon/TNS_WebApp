using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using TNS_Task.Areas.Tasks.Models;

namespace TNS_Task.Areas.Tasks.Pages
{
    [IgnoreAntiforgeryToken]
    public class TaskReceiveModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion

        public readonly List<TN_Item> TodoPriorities = AccessData.Priorities();
        public readonly List<TN_Item> TodoStatus = AccessData.Status();
        public void OnGet()
        {
            CheckAuth();
           
        }
        public IActionResult OnPostLoadData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            Models.Task_List zTaskList = new Models.Task_List("TaskKey,Subject,StartDate,DueDate,StatusName,Complete,PriorityKey,StatusKey");
            DataTable zTable = zTaskList.Owner_List_Smart(50, request.SearchContent, UserLogin.Employee.Key);
          
            int zRecordCount = 10;

            List<ItemView> zList = new List<ItemView>();
            ItemView zItem = new ItemView();
            if (zMessageLog.Length > 0)
            {
                zItem.ID = "ERROR";
                zItem.Subject = zMessageLog;
            }
            else
            {
                zItem.ID = "OK";
                zItem.Subject = "Số lượng Record là : " + zRecordCount.ToString("#,###,##0");
            }
            zList.Add(zItem);
            double zMoney = 0;
            DateTime zDate;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new ItemView();
                zItem.Key = zRow["TaskKey"].ToString();
                zItem.Subject = zRow["Subject"].ToString();
                zDate = (DateTime)zRow["StartDate"];
                zItem.StartDate = zDate.ToString("dd/MM/yyyy");
                zDate = (DateTime)zRow["DueDate"];
                zItem.DueDate = zDate.ToString("dd/MM/yyyy");

                zItem.StatusName = zRow["StatusName"].ToString();
                zItem.Complete = zRow["Complete"].ToString();
                zItem.PriorityKey = zRow["PriorityKey"].ToString();
                zItem.PriorityName = "<img src=\"/images/Priority_" + zRow["PriorityKey"].ToString() + ".png\"";

                zItem.StatusKey = zRow["StatusKey"].ToString();
                zList.Add(zItem);
            }
            return new JsonResult(zList);
        }
        public IActionResult OnPostSearchEmployee([FromBody] ItemRequest request)
        {
            CheckAuth();
            JsonResult zResult = new JsonResult("");
            string zMessageLog = "";
            ListItemView zListItem = new ListItemView();
            if (request.SearchContent.Trim().Length == 0)
            {
                zListItem.ID = "ERROR";
            }
            else
            {
                List<string[]> zData = Category_AccessData.SearchEmployee(request.SearchContent);
                if (zData[0][0] == "ERROR")
                {
                    zListItem.ID = "ERROR";
                    zListItem.Name = zData[0][1].ToString();
                }
                else
                {
                    zListItem.ID = "OK";
                    zListItem.DataOfTable = zData;
                }
            }
            return new JsonResult(zListItem);
        }
        public class ItemRequest
        {
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string SearchContent { get; set; }
        }
        public class ItemView
        {
            public string Key { get; set; }
            public string ID { get; set; }
            public string StartDate { get; set; }
            public string Subject { get; set; }
            public string DueDate { get; set; }
            public string Complete { get; set; }
            public string PriorityKey { get; set; }
            public string PriorityName{ get; set; }
            public string StatusKey { get; set; }
            public string StatusName { get; set; }
            public string Code { get; set; }
            public ItemView() { }

        }

        public class ListItemView
        {
            public string ID { get;set; }
            public string Name { get; set; }
            public List<string[]> DataOfTable { get; set; }
            public string Code { get; set; }
            public ListItemView() { }

        }
    }
}
