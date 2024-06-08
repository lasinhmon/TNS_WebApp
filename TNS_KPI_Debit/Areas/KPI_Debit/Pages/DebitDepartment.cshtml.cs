using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace TNS_KPI.Areas.KPI_Debit.Pages
{
    [IgnoreAntiforgeryToken]
    public class DebitDepartmentModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
            UserLogin.GetRole("Full");
        }
        #endregion
        public readonly List<string[]> Departments = Models.Categories_AccessData.DepartmentsForControl();
        public void OnGet()
        {
            CheckAuth();
        }
        public IActionResult OnPostLoadData([FromBody] ItemRequest request)
        {
            CheckAuth();
            JsonResult zResult = new JsonResult("");
            string zMessageLog = "";
           
            int zRecordCount = Models.AccessData.Department_Debit_Count(request.InMonth, request.InYear, request.DepartmentID, request.SearchContent, "");
            DataTable zTable = Models.AccessData.Department_Debit(200, request.InMonth, request.InYear, request.DepartmentID, request.SearchContent, "");

            List<ItemView> zList = new List<ItemView>();
            ItemView zItem = new ItemView();
            if (zMessageLog.Length > 0)
            {
                zItem.ID = "ERROR";
                zItem.Name = zMessageLog;
            }
            else
            {
                zItem.ID = "OK";
                zItem.Name = "Số lượng Record là : " + zRecordCount.ToString("#,###,##0");
            }
            zList.Add(zItem);
            double zMoney = 0;
            int i = 1;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new ItemView();
                zItem.ID = zRow["CustomerID"].ToString();
                zItem.Name = zRow["CustomerName"].ToString();
                zItem.EmployeeName = zRow["EmployeeName"].ToString();
                zItem.AccountNumber = zRow["Address"].ToString();
                zItem.PhotoPath = zRow["PhotoPath"].ToString();
                zItem.btn_SelectEmployee_Action = " onclick=\"Approval_Action(" + i.ToString() + ",'" + zRow["AutoKey"].ToString() + "')\"";
                zItem.ConflictContent = "";// zRow["SoTKTienGui"].ToString();
                if (zItem.EmployeeName.Length > 0)
                    zItem.Status = "1";
                else
                    zItem.Status = "0";

                zList.Add(zItem);
                i++;
            }
            if (request.SearchContent.Trim().Length == 0)
            {
                if (zTable.Rows.Count < zRecordCount)
                {
                    zItem = new ItemView();
                    zItem.ID = "...";
                    zItem.Name = "...";
                    zItem.AccountNumber = "...";
                    zItem.PhotoPath = "...";
                    zItem.ConflictContent = "...";
                    zList.Add(zItem);
                }

            }
            return new JsonResult(zList);
        }
        public IActionResult OnPostApprovalEmployee([FromBody] ItemRequest request)
        {
            CheckAuth();
            JsonResult zResult = new JsonResult("");
            string zMessageLog = "";
            ItemView zItemReturn = new ItemView();
            if (request.AutoKey.Length == 36)
            {
                string zResult_Action = "";
                string[] zEmployeeInfo = Models.Categories_AccessData.GetEmployeeInfo(UserLogin.Employee.Key);
                if (zEmployeeInfo[0] != null)
                {
                    Models.Debit_Record zRecord = new Models.Debit_Record();
                    zRecord.AutoKey = request.AutoKey;
                    if (request.Status.Trim() == "0")
                    {
                        zRecord.EmployeeID = zEmployeeInfo[0];
                        zRecord.EmployeeName = zEmployeeInfo[1];
                    }
                    else
                    {
                        zRecord.EmployeeID = "";
                        zRecord.EmployeeName = "";
                    }
                    zResult_Action = zRecord.Update_Employeer();

                    if (zResult_Action == "OK")
                    {
                        zItemReturn.ID = "OK";
                        zItemReturn.EmployeeName = zRecord.EmployeeName;
                        if (zRecord.EmployeeName.Length > 0)
                            zItemReturn.PhotoPath = zEmployeeInfo[2];
                        else
                            zItemReturn.PhotoPath = "/images/users/avatar-0.jpg";

                    }
                    else
                    {
                        zItemReturn.ID = "ERROR";
                        zItemReturn.Name = zRecord.Message;
                    }
                }
                else
                {
                    zItemReturn.ID = "ERROR";
                    zItemReturn.Name = "Can't not found this employee";
                }
            }
            else
            {
                zItemReturn.ID = "ERROR";
                zItemReturn.Name = "Error Key";
            }
            return new JsonResult(zItemReturn);
        }
        public class ItemRequest
        {
            public string AutoKey { get; set; }
            public string DepartmentID { get; set; }
            public string SearchContent { get; set; }
            public string Status { get; set; }
            public string InMonth { get; set; }
            public string InYear { get; set; }
        }
        public class ItemView
        {
            public string Key { get; set; }
            public string ID { get; set; }
            public string Name { get; set; }
            public string AccountNumber { get; set; }
            public string EmployeeID { get; set; }
            public string EmployeeName { get; set; }
            public string PhotoPath { get; set; }
            public string ConflictContent { get; set; }
            public string Status { get; set; }
            public string btn_SelectEmployee_Action { get; set; }
            public string Code { get; set; }
            public ItemView() { }

        }
    }
}
