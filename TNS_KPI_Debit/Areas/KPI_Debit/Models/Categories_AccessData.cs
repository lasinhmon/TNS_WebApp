using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace TNS_KPI.Areas.KPI_Debit.Models
{
    public class Categories_AccessData
    {
        public static string[] GetEmployeeByID(string EmployeeID)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT EmployeeID, LastName + ' ' + FirstName AS EmployeeName, PhotoPath  FROM [dbo].[HRM_Employee] "
                        + "WHERE EmployeeID = @EmployeeID AND RecordStatus != 99 ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@EmployeeID", SqlDbType.NVarChar).Value = EmployeeID;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            string[] zResult = new string[3];
            if (zTable.Rows.Count > 0)
            {
                zResult[0] = zTable.Rows[0]["EmployeeID"].ToString();
                zResult[1] = zTable.Rows[0]["EmployeeName"].ToString();
                zResult[2] = zTable.Rows[0]["PhotoPath"].ToString();
            }
            return zResult;
        }
        public static List<string[]> DepartmentTRCDForControl()
        {
            string zMessage = "";
            string zSQL = "SELECT DepartmentKey,DepartmentID, DepartmentName, TRCD  "
                + " FROM [dbo].[HRM_Department] WHERE LEN(TRCD)> 0 ORDER BY TRCD ASC ";
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
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
            string[] zItem;
            if (zMessage.Length > 0)
            {
                zItem = new string[4];
                zItem[0] = "ERROR";
                zItem[1] = "zMessage";
                zResult.Add(zItem);
            }
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                zResult.Add(zItem);
            }

            return zResult;
        }
        public static string[] GetEmployeeInfo(string EmployeeKey)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT EmployeeID, LastName + ' ' + FirstName AS EmployeeName, PhotoPath  FROM [dbo].[HRM_Employee] "
                        + "WHERE EmployeeKey = @EmployeeKey AND RecordStatus != 99 ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = new Guid(EmployeeKey);
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            string[] zResult = new string[3];
            if (zTable.Rows.Count > 0)
            {
                zResult[0] = zTable.Rows[0]["EmployeeID"].ToString();
                zResult[1] = zTable.Rows[0]["EmployeeName"].ToString();
                zResult[2] = zTable.Rows[0]["PhotoPath"].ToString();
            }
            return zResult;
        }
        public static List<string[]> DepartmentsForControl()
        {

            string zMessage = "";
            string zSQL = "SELECT DepartmentKey,DepartmentID, DepartmentName, TRCD  "
                        + " FROM [dbo].[HRM_Department] ORDER BY Rank ASC ";
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
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
            string[] zItem;
            if (zMessage.Length > 0)
            {
                zItem = new string[3];
                zItem[0] = "ERROR";
                zItem[1] = "zMessage";
                zResult.Add(zItem);
            }
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                zResult.Add(zItem);
            }

            return zResult;
        }
        public static List<string[]> DataOfTable(string SQL, int PageNumber, int PageSize)
        {
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SQL += " OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY OPTION(RECOMPILE)";
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                zCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                zCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = PageNumber;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                zResult.Add(zItem);
            }
            return zResult;
        }
        public static List<string[]> DataOfTable(string SQL, List<int> FieldsIsDate, int PageNumber, int PageSize)
        {
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SQL += " OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY OPTION(RECOMPILE)";
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                zCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                zCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = PageNumber;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                for (int i = 0; i < FieldsIsDate.Count; i++)
                {
                    zItem[FieldsIsDate[i]] = zItem[FieldsIsDate[i]].Split(' ')[0];
                }
                zResult.Add(zItem);
            }
            return zResult;
        }
        public static List<string[]> DataOfTable(string SQL, List<int> FieldsIsDate)
        {
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                for (int i = 0; i < FieldsIsDate.Count; i++)
                {
                    zItem[FieldsIsDate[i]] = zItem[FieldsIsDate[i]].Split(' ')[0];
                }
                zResult.Add(zItem);
            }
            return zResult;
        }
        public static List<string[]> DataOfTable(string SQL)
        {
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                zResult.Add(zItem);
            }
            return zResult;
        }

        public static DataTable GetData(string SQL)
        {
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }

            return zTable;
        }
        public static int DataCount(string SQL)
        {
            int zQuantity = 0;
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                string zResult = zCommand.ExecuteScalar().ToString();
                int.TryParse(zResult, out zQuantity);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            return zQuantity;
        }

    }
}
