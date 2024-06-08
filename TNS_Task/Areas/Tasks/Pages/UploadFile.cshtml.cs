using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

using TNS_Task.Areas.Tasks.Models;

namespace TNS_Task.Areas.Tasks.Pages
{
    public class UploadFileModel : PageModel
    {
        private TNS.Auth.UserLogin_Info _UserLogin;
        public List<FileAttack_Record> ListFileAttack { get; set; }
        [BindProperty]
        public string TaskKey { get; set; }
      
        public IActionResult OnGet(string Key)
        {
            CheckAuth();
            if (Key != null && Key.Length == 36)
            {
                TaskKey = Key;
                ListFileAttack = AccessData.Task_FileAttack(TaskKey);
            }
            return Page();

        }
        public async Task<ActionResult> OnPost(string action, int key)
        {
            CheckAuth();

            if (action == "UploadFile")
            {
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    var zFile = HttpContext.Request.Form.Files[0];
                    if (zFile != null)
                    {
                        //await _FileUploadService.UploadFileAsync(zFile, "wwwroot\\documents\\tasks\\" + TaskKey + "\\");
                        FileAttack_Record zFile_Record = new Models.FileAttack_Record();
                        zFile_Record.FileName = zFile.FileName;
                        zFile_Record.FileSize = zFile.Length;

                        zFile_Record.FilePath = "/documents/tasks/" + TaskKey + "/" + zFile.FileName;

                        zFile_Record.TaskKey = TaskKey;
                        zFile_Record.EmployeeKey = _UserLogin.Employee.Key;
                        zFile_Record.EmployeeName = _UserLogin.Employee.Name;
                        zFile_Record.Create();
                        ListFileAttack = AccessData.Task_FileAttack(TaskKey);
                       
                    }
                }
            }
            if (action == "DeleteItem" && key >0)
            {
                FileAttack_Record zFile_Record = new FileAttack_Record();
                zFile_Record.EntryKey = key;
                zFile_Record.Delete();
                ListFileAttack = AccessData.Task_FileAttack(TaskKey);
            }

            return Page();
        }
        private void CheckAuth()
        {
            _UserLogin = new TNS.Auth.UserLogin_Info(User);
            //_UserLogin.GetRole("HRM_NTC");
        }
    }
}
