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
    public class TaskAddModel : PageModel
    {

        private TNS.Auth.UserLogin_Info _UserLogin;
        public readonly List<TN_Item> TodoPriorities = AccessData.Priorities();
        public readonly List<TN_Item> TodoStatus = AccessData.Status();
        public string EmployeeLoginKey { get; private set; }
        public string Message { get; private set; }
       
        [BindProperty]
        public Task_Record Todo { get; set; }

        public IActionResult OnGet(string key)
        {
            CheckAuth();

            Todo = new TNS_Task.Areas.Tasks.Models.Task_Record();
            DateTime zDueDate = DateTime.Now.AddDays(6);
            zDueDate = new DateTime(zDueDate.Year, zDueDate.Month, zDueDate.Day, zDueDate.Hour, zDueDate.Minute, 0);

            Todo.DueDate = zDueDate;
            //TaskInfo.FinishDate = zDueDate;

            Todo.CreatedBy = _UserLogin.Employee.Key;
            Todo.CreatedName = _UserLogin.Employee.Name;
            Todo.CreatedOn = DateTime.Now;

            Todo.ModifiedBy = _UserLogin.Employee.Key;
            Todo.ModifiedName = _UserLogin.Employee.Name;

            return Page();

        }

        public async Task<ActionResult> OnPost(string action)
        {
            if (ModelState.IsValid)
            {
                CheckAuth();
                if (action == "SaveTask")
                {
                    if (Todo.TaskCode == null)
                    {
                        Todo.TaskCode = "";
                    }
                    Todo.StatusName = GetStatusName(Todo.StatusKey.ToString());
                    Todo.PriorityName = GetPriorityName(Todo.PriorityKey.ToString());
                    Todo.CreatedBy = _UserLogin.Employee.Key;
                    Todo.CreatedName = _UserLogin.Employee.Name;
                    Todo.ModifiedBy = _UserLogin.Employee.Key;
                    Todo.ModifiedName = _UserLogin.Employee.Name;
                    Todo.DueDate = DateTime.Now.AddDays(7);
                    Todo.Create();
                    Message = Todo.Message;
                    //SharedUsers = TNS_Task.Areas.Tasks.Models.AccessData.Shared_Users(Todo.TaskKey);

                    return Page();

                }

                if (action == "SaveReturn")
                {
                    if (Todo.TaskCode == null)
                    {
                        Todo.TaskCode = "";
                    }
                    Todo.StatusName = GetStatusName(Todo.StatusKey.ToString());
                    Todo.PriorityName = GetPriorityName(Todo.PriorityKey.ToString());
                    Todo.CreatedBy = _UserLogin.Employee.Key;
                    Todo.CreatedName = _UserLogin.Employee.Name;
                    Todo.ModifiedBy = _UserLogin.Employee.Key;
                    Todo.ModifiedName = _UserLogin.Employee.Name;
                    Todo.DueDate = DateTime.Now.AddDays(7);
                    Todo.Create();
                    return LocalRedirect("~/Tasks/TaskAll");
                }
            }

            return Page();
        }
        private void CheckAuth()
        {
            _UserLogin = new TNS.Auth.UserLogin_Info(User);
            EmployeeLoginKey = _UserLogin.Employee.Key;
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
