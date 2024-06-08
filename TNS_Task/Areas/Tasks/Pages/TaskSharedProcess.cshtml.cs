using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.IO;
using System.Security.Claims;
using System.Xml.Linq;
using TNS_Task.Areas.Tasks.Models;

namespace TNS_Task.Areas.Tasks.Pages
{
    public class TaskSharedProcessModel : PageModel
    {
     
        private TNS.Auth.UserLogin_Info _UserLogin;
        public readonly List<TN_Item> TodoPriorities = AccessData.Priorities();
        public readonly List<TN_Item> TodoStatus = AccessData.Status();
        [BindProperty]
        public Task_Record TodoRequest { get;private set; }
        public Task_Record TodoReponse { get; set; }
        public List<Task_Record> ListReponse { get; set; }    
        public IActionResult OnGet(string key)
        {
            CheckAuth();
            TodoReponse = new Models.Task_Record();
            if (key.Length == 36)
            {
                TodoRequest = new Models.Task_Record(key);
                ListReponse = AccessData.TaskOfParent_ListFull(key);
            }
            else
            {
                TodoRequest = new Models.Task_Record();
                DateTime zDueDate = DateTime.Now.AddDays(6);
                zDueDate = new DateTime(zDueDate.Year, zDueDate.Month, zDueDate.Day, zDueDate.Hour, zDueDate.Minute, 0);

                TodoRequest.DueDate = zDueDate;
                //TaskInfo.FinishDate = zDueDate;

                TodoRequest.CreatedBy = _UserLogin.Employee.Key;
                TodoRequest.CreatedName = _UserLogin.Employee.Name;
                TodoRequest.CreatedOn = DateTime.Now;

                TodoRequest.ModifiedBy = _UserLogin.Employee.Key;
                TodoRequest.ModifiedName = _UserLogin.Employee.Name;


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
                    if(TodoReponse.TaskCode == null)
                    {
                        TodoReponse.TaskCode = "";
                    }
                    TodoReponse.StatusName = GetStatusName(TodoReponse.StatusKey.ToString());
                    TodoReponse.PriorityName = GetPriorityName(TodoReponse.PriorityKey.ToString());
                    TodoReponse.ModifiedBy = _UserLogin.Employee.Key;
                    TodoReponse.ModifiedName = _UserLogin.Employee.Name;
                    TodoReponse.Update();
                    //return LocalRedirect("~/Tasks/PersonalIndex");
                    
                }
                if (action == "SaveComment")
                {

                }
                if (action == "UploadFile")
                {
                    if (HttpContext.Request.Form.Files.Count > 0)
                    {
                        var zFile = HttpContext.Request.Form.Files[0];
                        if (zFile != null)
                        {
                            //await _FileUploadService.UploadFileAsync(zFile);
                            //Notice.Info.FileAttack = zFile.FileName;
                        }
                    }
                    
                }
            }

            return Page();
        }
        private void CheckAuth()
        {
            _UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        private string GetStatusName(string Value)
        {
            string zResult = "";
            foreach(TN_Item zItem in TodoStatus)
            {
                if(zItem.Value == Value)
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
