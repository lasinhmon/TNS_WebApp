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
    public class TaskProcessModel : PageModel
    {

        public string EmployeeLoginKey { get; private set; }

        public readonly List<TN_Item> TodoStatus = AccessData.Status();
        public List<FileAttack_Record> ListFileAttack { get; set; }
        public List<TaskShared_Record> SharedUsers { get; private set; }

        [BindProperty]
        public Task_Record Todo { get; set; }
        public string Message { get; private set; }
        public List<string> OwnerInfo { get; set; }
        public IActionResult OnGet(string key)
        {
            CheckAuth();
            if (key.Length == 36)
            {
                Todo = new Models.Task_Record(key);
                OwnerInfo = AccessData.EmployeeGetInfo(Todo.OwnerBy);
                ListFileAttack = AccessData.Task_FileAttack(key);
                SharedUsers = AccessData.Shared_Users(key);

                if (Todo.StatusKey != 3)
                {
                    
                    foreach (TaskShared_Record zItem in SharedUsers)
                    {
                        if (zItem.EmployeeKey == UserLogin.Employee.Key)
                        {
                            if (zItem.RoleEdit == true)
                            {
                                //RoleActionRecord.IsEdit = true;
                            }
                            if (zItem.RoleAdd == true)
                            {
                                //RoleActionRecord.IsAdd = true;
                            }
                            break;
                        }
                    }
                }
            }
            return Page();

        }

        public async Task<ActionResult> OnPost(string action)
        {
            //if (ModelState.IsValid)
            {
                CheckAuth();
                SharedUsers = AccessData.Shared_Users(Todo.TaskKey);

                foreach (TaskShared_Record zItem in SharedUsers)
                {
                    if (zItem.EmployeeKey == UserLogin.Employee.Key)
                    {
                        if (zItem.RoleEdit == true)
                        {
                            //RoleActionRecord.IsEdit = true;
                        }
                        if (zItem.RoleAdd == true)
                        {
                            //RoleActionRecord.IsAdd = true;
                        }
                        break;
                    }
                }
                //if (RoleActionRecord.IsEdit)
                {

                    if (Todo.StatusKey == 3)
                        Todo.Complete = 100;
                    else
                        if (Todo.Complete == 100)
                        Todo.StatusKey = 3;
                    Todo.StatusName = GetStatusName(Todo.StatusKey.ToString());

                    Todo.UpdateStatus();
                    Message = Todo.Message;
                }

                string zKey = Todo.TaskKey;
                Todo = new Models.Task_Record(zKey);
                OwnerInfo = AccessData.EmployeeGetInfo(Todo.OwnerBy);
                ListFileAttack = AccessData.Task_FileAttack(Todo.TaskKey);

            }

            return Page();
        }
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
      
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
          

        }
        #endregion
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
    }

}
