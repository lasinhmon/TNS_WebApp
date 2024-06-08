using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.IO;
using System.Security.Claims;
using System.Xml.Linq;
using TNS_Task.Areas.Tasks.Models;

namespace TNS_Task.Areas.Tasks.Pages
{
    public class TaskEditModel : PageModel
    {
        private IHostingEnvironment _environment;
        public TaskEditModel(IHostingEnvironment environment)
        {
            _environment = environment;
        }
        private TNS.Auth.UserLogin_Info _UserLogin;
        public readonly List<TN_Item> TodoPriorities = AccessData.Priorities();
        public readonly List<TN_Item> TodoStatus = AccessData.Status();
        public string EmployeeLoginKey { get; private set; }
        public string Message { get; private set; }
        [BindProperty]
        public Task_Record Todo { get; set; }

        [BindProperty]
        public List<IFormFile> FilesAttach { get; set; }
        public IActionResult OnGet(string key)
        {
            CheckAuth();

            if (key.Length == 36)
            {
                Todo = new Models.Task_Record(key);
            }
            else
            {
                return LocalRedirect("~/Tasks/TaskAdd");
            }

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
                    Todo.ModifiedBy = _UserLogin.Employee.Key;
                    Todo.ModifiedName = _UserLogin.Employee.Name;
                    Todo.Update();
                    Message = Todo.Message;
                    if (FilesAttach != null && FilesAttach.Count > 0)
                    {
                        foreach (IFormFile file in FilesAttach)
                        {
                            string path = Path.Combine(_environment.ContentRootPath, "uploaded", file.FileName);
                            using (var fs = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(fs);

                                FileAttack_Record fileAttach = new FileAttack_Record();
                                fileAttach.TaskKey = Todo.TaskKey;
                                fileAttach.FileName = file.FileName;
                                fileAttach.FilePath = path;
                                fileAttach.CreatedOn = DateTime.Now;
                                fileAttach.Create();
                            }
                        }
                    }
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
                    Todo.ModifiedBy = _UserLogin.Employee.Key;
                    Todo.ModifiedName = _UserLogin.Employee.Key;
                    Todo.Update();

                    if (FilesAttach != null && FilesAttach.Count > 0)
                    {
                        foreach (IFormFile file in FilesAttach)
                        {
                            string path = Path.Combine(_environment.ContentRootPath, "uploaded", file.FileName);
                            using (var fs = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(fs);

                                FileAttack_Record fileAttach = new FileAttack_Record();
                                fileAttach.TaskKey = Todo.TaskKey;
                                fileAttach.FileName = file.FileName;
                                fileAttach.FilePath = path;
                                fileAttach.CreatedOn = DateTime.Now;
                                fileAttach.Create();
                            }
                        }
                    }
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
