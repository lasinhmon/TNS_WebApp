using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Threading.Tasks;
using System.Xml.Linq;
using TNS_Task.Areas.Tasks.Models;

namespace TNS_Task.Areas.Tasks.Pages
{
    public class APIModel : PageModel
    {
        private TNS.Auth.UserLogin_Info _UserLogin;

        public async Task<ActionResult> OnGet(string cmd, string taskKey, string comment = null, string content = null)
        {
            CheckAuth();
            JsonResult zResult = new JsonResult("");
            switch (cmd)
            {
                case "taskGet":
                    zResult = Task_Get(taskKey);
                    break;
                case "taskUpdateStatus":
                    zResult = Task_UpdateStatus(taskKey, content);
                    break;
                case "taskDel":
                    zResult = Task_Del(taskKey);
                    break;
                case "commentGet":
                    zResult = Comment_Get(taskKey);
                    break;
                case "commentAdd":
                    zResult = Comment_Add(taskKey, comment);
                    break;
                case "sharedGet":
                    zResult = Shared_Get(taskKey);
                    break;
                case "sharedSearch":
                    zResult = Shared_Search(content);
                    break;
                case "sharedAdd":
                    zResult = Shared_Add(taskKey, content);
                    break;
                case "sharedUpdate":
                    zResult = Shared_Update(taskKey, content);
                    break;
            }
            return zResult;

        }
        private JsonResult Task_Get(string taskKey)
        {
            JsonResult zResult = new JsonResult("");
            if (!string.IsNullOrEmpty(taskKey))
            {
                if (taskKey.Length == 36)
                {
                    Task_Record_Quick zRecord = new Models.Task_Record_Quick(taskKey);
                    zResult = new JsonResult(zRecord);
                }
            }
            return zResult;
        }
        private JsonResult Task_Del(string taskKey)
        {
            JsonResult zResult = new JsonResult("");
            if (!string.IsNullOrEmpty(taskKey))
            {
                if (taskKey.Length == 36)
                {
                    Task_Record zRecord = new Models.Task_Record();
                    zRecord.TaskKey = taskKey;
                    zRecord.OwnerBy = _UserLogin.Employee.Key;
                    zRecord.Delete_OwnerBy();
                    ItemReturn zItem = new ItemReturn();
                    zItem.Id = taskKey;
                    if (zRecord.Message.Length >= 3)
                        zItem.Code = zRecord.Message.Substring(0, 3);
                    zItem.Description = zRecord.Message;

                    zResult = new JsonResult(zItem);
                }
            }
            return zResult;
        }
        private JsonResult Task_UpdateStatus(string taskKey, string content)
        {
            JsonResult zResult = new JsonResult("");
            if (!string.IsNullOrEmpty(taskKey))
            {
                if (taskKey.Length == 36)
                {
                    string[] zItemVals = content.Split(':');

                    Task_Record zRecord = new Models.Task_Record();
                    zRecord.TaskKey = taskKey;
                    zRecord.StatusKey = int.Parse(zItemVals[0]);
                    zRecord.StatusName = zItemVals[1];
                    zRecord.Complete = int.Parse(zItemVals[2]);
                    zRecord.UpdateStatus();
                    ItemReturn zItem = new ItemReturn();
                    zItem.Id = taskKey;
                    if (zRecord.Message.Length >= 3)
                        zItem.Code = zRecord.Message.Substring(0, 3);
                    zItem.Description = zRecord.Message;

                    zResult = new JsonResult(zItem);
                }
            }
            return zResult;
        }
        
        private JsonResult Comment_Add(string taskKey, string comment)
        {
            JsonResult zResult = new JsonResult("");
            if (!string.IsNullOrEmpty(taskKey) && !string.IsNullOrEmpty(comment))
            {
                if (taskKey.Length == 36)
                {
                    Comment_Record zComment_Record = new TNS_Task.Areas.Tasks.Models.Comment_Record();
                    zComment_Record.Comment = comment;
                    zComment_Record.TaskKey = taskKey;
                    zComment_Record.EmployeeKey = _UserLogin.Employee.Key;
                    zComment_Record.EmployeeName = _UserLogin.Employee.Name;
                    zComment_Record.CreatedOn = DateTime.Now;
                    zComment_Record.Create();
                    zResult = new JsonResult(zComment_Record);
                }
            }
            return zResult;
        }
        private JsonResult Comment_Get(string taskKey)
        {
            JsonResult zResult = new JsonResult("");
            if (!string.IsNullOrEmpty(taskKey))
            {
                if (taskKey.Length == 36)
                {
                    List<Comment_Record> ListComment = AccessData.Task_Comment(taskKey);
                    zResult = new JsonResult(ListComment);
                }
            }
            return zResult;
        }

        private JsonResult Shared_Get(string taskKey)
        {
            JsonResult zResult = new JsonResult("");
            if (!string.IsNullOrEmpty(taskKey))
            {
                if (taskKey.Length == 36)
                {
                    List<TaskShared_Record> ListUser = AccessData.Shared_Users(taskKey);
                    zResult = new JsonResult(ListUser);
                }
            }
            return zResult;
        }
        private JsonResult Shared_Search(string content)
        {
            JsonResult zResult = new JsonResult("");
            if (!string.IsNullOrEmpty(content))
            {
                DataTable zTable = AccessData.Shared_SearchUsers(content);
                List<TaskShared_Record> ListUser = new List<TaskShared_Record>();
                foreach (DataRow zRow in zTable.Rows)
                {
                    TaskShared_Record zItem = new Models.TaskShared_Record();
                    zItem.EmployeeID = zRow["EmployeeID"].ToString();
                    zItem.EmployeeName = zRow["EmployeeName"].ToString();
                    zItem.PhotoPath = zRow["PhotoPath"].ToString();
                    zItem.EmployeeKey = zRow["EmployeeKey"].ToString();
                    ListUser.Add(zItem);
                }
                zResult = new JsonResult(ListUser);

            }
            return zResult;
        }
        private JsonResult Shared_Add(string taskKey, string content)
        {
            JsonResult zResult = new JsonResult("");
            string zErrorMessage = "";
            if (!string.IsNullOrEmpty(content))
            {
                string[] zListUser = content.Split(';');

                for (int i = 0; i < zListUser.Length; i++)
                {
                    string[] zUser = zListUser[i].Split(':');
                    if (zUser[0].Length == 36)
                    {
                        TaskShared_Record zUser_Record = new Models.TaskShared_Record(taskKey, zUser[0]);
                        if (zUser_Record.EmployeeName.Length == 0)
                        {
                            zUser_Record.EmployeeName = zUser[1];
                            zUser_Record.Create();
                            if (zUser_Record.Message.Substring(0, 3) == "ERR")
                            {
                                zErrorMessage = zUser_Record.Message;
                            }
                        }
                    }
                }

            }
            ItemReturn zItem = new ItemReturn();
            if (zErrorMessage.Length > 0)
            {
                zItem.Code = "ERR";
                zItem.Description = zErrorMessage;
            }
            else
                zItem.Code = "OK";
            zResult = new JsonResult(zItem);

            return zResult;
        }
        private JsonResult Shared_Update(string taskKey, string content)
        {
            JsonResult zResult = new JsonResult("");
            string zErrorMessage = "";

            if (!string.IsNullOrEmpty(content))
            {
                string[] zUser = content.Split(':');
                if (zUser[0].Length == 36)
                {
                    TaskShared_Record zUser_Record = new Models.TaskShared_Record(taskKey, zUser[0]);
                    if (zUser_Record.EmployeeName.Length > 0)
                    {
                        if (zUser[1].Substring(0, 1) == "1")
                            zUser_Record.RoleRead = true;
                        else
                            zUser_Record.RoleRead = false;

                        if (zUser[1].Substring(1, 1) == "1")
                            zUser_Record.RoleAdd = true;
                        else
                            zUser_Record.RoleAdd = false;

                        if (zUser[1].Substring(2, 1) == "1")
                            zUser_Record.RoleEdit = true;
                        else
                            zUser_Record.RoleEdit = false;

                        if (zUser[1].Substring(3, 1) == "1")
                            zUser_Record.RoleDel = true;
                        else
                            zUser_Record.RoleDel = false;

                        if (zUser[1].Substring(4, 1) == "1")
                            zUser_Record.RoleApproval = true;
                        else
                            zUser_Record.RoleApproval = false;

                        zUser_Record.Update();
                        if (zUser_Record.Message.Substring(0, 3) == "ERR")
                        {
                            zErrorMessage = zUser_Record.Message;
                        }
                    }
                }

            }
            ItemReturn zItem = new ItemReturn();
            if (zErrorMessage.Length > 0)
            {
                zItem.Code = "ERR";
                zItem.Description = zErrorMessage;
            }
            else
                zItem.Code = "OK";
            zResult = new JsonResult(zItem);

            return zResult;
        }
        private void CheckAuth()
        {
            _UserLogin = new TNS.Auth.UserLogin_Info(User);
        }

        private class ItemReturn
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Value { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }

        }

    }
}
