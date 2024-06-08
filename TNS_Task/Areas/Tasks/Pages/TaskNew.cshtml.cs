using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.IO;
using System.Security.Claims;
using System.Xml.Linq;
using TNS_Task.Areas.Tasks.Models;

namespace TNS_Task.Areas.Tasks.Pages
{
    public class TaskNewModel : PageModel
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

        public IActionResult OnGet()
        {
            CheckAuth();
            return Page();
        }

        public IActionResult OnPostSaveData([FromBody] Task_Record TaskNew)
        {

            return new JsonResult("");

        }

        private string GetStatusName(string Value)
        {
            string zResult = "";
            foreach (TN_Item zItem in TodoStatus)
            {
                if (zItem.Value == Value)
                {
                    zResult = zItem.Name;
                    break;
                }
            }
            return zResult;
        }
        private string GetPriorityName(string Value)
        {
            string zResult = "";
            foreach (TN_Item zItem in TodoPriorities)
            {
                if (zItem.Value == Value)
                {
                    zResult = zItem.Name;
                    break;
                }
            }
            return zResult;
        }
    }

}
