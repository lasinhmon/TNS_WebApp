using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using TNS_KPI.Areas.KPI_Debit.Models;

namespace TNS_KPI.Areas.KPI_Debit.Pages
{
    [IgnoreAntiforgeryToken]
    public class ProcessForDeparments_DebitModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
            UserLogin.GetRole("Full");
        }
        #endregion

        public void OnGet(string monthView)
        {
            CheckAuth();
        }
        public readonly List<string[]> Departments = Models.Categories_AccessData.DepartmentsForControl();
         
        public IActionResult OnPostConflictDepartment([FromBody] ItemRequest request)
        {
            CheckAuth();
            JsonResult zResult = new JsonResult("");
            string zMessageLog = "";
            int zMonth, zYear;
            int.TryParse(request.InMonth, out zMonth);
            int.TryParse(request.InYear, out zYear);

            DataTable zDataConflic = Debit_Record.Department_GetAllConflic(zMonth, zYear, request.DepartmentID, request.SearchContent);

            List<ItemViewConflic> zList = new List<ItemViewConflic>();
            ItemViewConflic zItem = new ItemViewConflic();
            if (zMessageLog.Length > 0)
            {
                zItem.ID = "ERROR";
                zItem.Name = zMessageLog;
            }
            else
            {
                zItem.ID = "OK";
            }
            zList.Add(zItem);
            foreach (DataRow zRow in zDataConflic.Rows)
            {
                zItem = new ItemViewConflic();
                zItem.Key = zRow["Autokey"].ToString();
                zItem.CustomerID = zRow["CustomerID"].ToString();
                zItem.CustomerName = zRow["CustomerName"].ToString();
                zItem.ContractID = zRow["ContractID"].ToString();
                zItem.DepartmentID = zRow["DepartmentID"].ToString();
                zItem.DepartmentName = zRow["DepartmentName"].ToString();
                zItem.EmployeeID = zRow["EmployeeID"].ToString();
                zItem.EmployeeName = zRow["EmployeeName"].ToString();

                zList.Add(zItem);
            }

            return new JsonResult(zList);
        } 
        public class ItemRequest
        {
            public string AutoKey { get; set; }
            public string InMonth { get; set; }
            public string InYear { get; set; }
            public string CustomerID { get; set; }
            public string AccountNumber { get; set; }
            public string DepartmentID { get; set; }
            public string SearchContent { get; set; }
        }
        public class ItemView
        {
            public string Key { get; set; }
            public string ID { get; set; }
            public string Name { get; set; }
            public string AccountNumber { get; set; }
            public string trcd { get; set; }
            public string trcd_TotalTransaction { get; set; }
            public string trcd_TotalMoney { get; set; }
            public string Account_TotalTransaction { get; set; }
            public string Account_TotalMoney { get; set; }
            public string Code { get; set; }
            public ItemView() { }

        }

        public class ItemViewConflic
        {

            public string ID { get; set; }
            public string Name { get; set; }
            public string Key { get; set; }
            public string CustomerID { get; set; }
            public string CustomerName { get; set; }
            public string ContractID { get; set; }
            public string DepartmentID { get; set; }
            public string DepartmentName { get; set; }
            public string EmployeeID { get; set; }
            public string EmployeeName { get; set; } 
            public ItemViewConflic() { }

        }
    }
}
