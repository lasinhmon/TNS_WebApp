using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace TNS_Task.Areas.Tasks.Pages
{
    public class TasksEmployeeModel : PageModel
    {
        public Models.Task_Status[] TasksInMonth { get; set; }
        public void OnGet()
        {
            CheckAuth();
            TasksInMonth = new Models.Task_Status[12];
            for (int i = 0; i < 12; i++)
            {
                TasksInMonth[i] = new Models.Task_Status(i + 1, DateTime.Now.Year, _UserLogin.Employee.Key);
            }

        }

        #region [ Security ]
        private TNS.Auth.UserLogin_Info _UserLogin;
        private void CheckAuth()
        {
            _UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion
    }
}
