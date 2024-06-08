using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_Task.Areas.Tasks.Models
{
    public class Category_AccessData
    {
        public static List<string[]> SearchEmployee(string Content)
        {
            string zMessage = "";
            DataTable zTable = new DataTable();
            string zSQL = "SELECT EmployeeKey,EmployeeID,FirstName + ' ' + LastName AS EmployeeName, PhotoPath,DepartmentName "
                + "FROM [dbo].[HRM_Employee] A "

                + "WHERE A.RecordStatus != 99 ";

            zSQL += "AND (EmployeeID LIKE @Content OR "
                    + "(LastName + ' ' + FirstName) LIKE @Content)";

            zSQL += "ORDER BY DepartmentName, FirstName ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@Content", SqlDbType.NVarChar).Value = "%" + Content + "%";
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            int n = zTable.Columns.Count;
            string[] zItem = new string[n];
            if (zMessage.Length > 0)
            {
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zResult.Add(zItem);
            }
            else
            {
                foreach (DataRow zRow in zTable.Rows)
                {
                    zItem = new string[n];
                    for (int i = 0; i < n; i++)
                    {
                        zItem[i] = zRow[i].ToString();
                    }
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }

    }
}
