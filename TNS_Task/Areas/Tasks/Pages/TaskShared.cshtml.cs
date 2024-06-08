using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using TNS.Auth;
using TNS_Task.Areas.Tasks.Models;

namespace TNS_Task.Areas.Tasks.Pages
{
    public class TaskSharedModel : PageModel
    {
        private string _PageViewCookiedName = "Task_TaskShared";
        private string _PageRoleName = "Full";

        #region [ Condition view page current ]
        public string PageSearch { get; set; }
        public int PageCurrent { get; set; }
        #endregion

        public string ErrorMessage { get; private set; }
        public DataTable TaskListView { get; private set; }
        private Task_List _TaskList = new Models.Task_List("TaskKey,Subject,CreatedOn,FinishDate,StatusName,Complete,PriorityKey,StatusKey,OwnerName");
 
        public IActionResult OnGet()
        {
            #region [ Check Security ]
            CheckAuth();
            /*if (!RoleActonOnPage.IsRead)
            {
                return LocalRedirect("~/Warning?id=403");
            }
            */
            #endregion

            GetCookied(_PageViewCookiedName);
            
            this.TaskListView = _TaskList.Shared_List(200, UserLogin.Employee.Key);
            ErrorMessage = _TaskList.ErrorMessage;
            return Page();

        }
        public IActionResult OnPost(string action, string pageSearch)
        {
            #region [ Check Security ]
            CheckAuth();
            /*if (!RoleActonOnPage.IsRead)
            {
                return LocalRedirect("~/Warning?id=403");
            }
            */
            #endregion

            switch(action)
            {
                case "cmdSearch":
                    if (pageSearch != null)
                    {
                        this.TaskListView = _TaskList.Shared_List_Smart(200, pageSearch, UserLogin.Employee.Key);
                        this.PageSearch = pageSearch;
                    }
                    else
                    {
                        this.TaskListView = _TaskList.Shared_List(200, UserLogin.Employee.Key);
                    }
                    break;
            }
           
            return Page();
        }
     
        private void GetCookied(string cookieName)
        {
            string zCookieValue = Request.Cookies[cookieName];
            if (zCookieValue != null)
            {
                JsonResult zResult = new JsonResult(zCookieValue);
                if (zResult != null)
                {
                    //PageLastStatus zLastStatus = JsonConvert.DeserializeObject<PageLastStatus>(zResult.Value.ToString());
                   //this.PageSearch = zLastStatus.Search;
                    //this.PageCurrent = zLastStatus.PageView;
                }
                SetCookie(cookieName, string.Empty, -10);
                Response.Cookies.Delete(cookieName);
            }
        }
        private void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions zOption = new CookieOptions();
            if (expireTime.HasValue)
                zOption.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                zOption.Expires = DateTime.Now.AddMilliseconds(10);

            zOption.Secure = true;
            zOption.IsEssential = true;
            zOption.Path = Request.PathBase;
            zOption.SameSite = SameSiteMode.Lax;
            Response.Cookies.Append(key, value, zOption);
        }

        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new UserLogin_Info(User);
            UserLogin.GetRole(_PageRoleName);

        }
        #endregion
    }
}
