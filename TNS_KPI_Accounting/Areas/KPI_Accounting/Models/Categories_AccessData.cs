using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace TNS_KPI.Areas.KPI_Accounting.Models
{
    public class Categories_AccessData
    {
        #region [ Department ]
        public static DataRow GetDepartmentInfo(string EmployeeKey)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT A.DepartmentKey,B.DepartmentID, A.DepartmentName ,B.TRCD FROM [dbo].[HRM_Employee] A "
                        + "LEFT JOIN [dbo].[HRM_Department] B ON A.DepartmentKey = B.DepartmentKey "
                        + "WHERE A.EmployeeKey = @EmployeeKey ";
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
                string strMessage = ex.ToString();
            }
            if (zTable.Rows.Count > 0)
            {
                return zTable.Rows[0];
            }
            else
                return null;
        }     
        public static DataTable DepartmentsForKPI()
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT '' AS AutoKey,DepartmentKey,DepartmentID, DepartmentName  "
                + " FROM [dbo].[HRM_Department] WHERE Class ='DP' AND ForReport = 'KPI' ORDER BY RANK ASC ";
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
                string zstrMessage = ex.ToString();
            }

            return zTable;
        }
        public static DataTable DepartmentsForReport()
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT DepartmentKey,DepartmentID, DepartmentName  "
                + " FROM [dbo].[HRM_Department] WHERE Class ='DP' "
                + " AND ForReport = 'KPI' ORDER BY RANK ASC ";
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
                string zstrMessage = ex.ToString();
            }

            return zTable;
        }
        public static List<string[]> DepartmentForControl()
        {
            string zMessage = "";
            string zSQL = "SELECT DepartmentKey,DepartmentID, DepartmentName "
                + " FROM [dbo].[HRM_Department] WHERE Class ='DP' AND ForReport='KPI' ORDER BY RANK ASC ";
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
            if(zMessage.Length > 0 )
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
        #endregion

        public static List<string[]> GetEmployeeSearch(string Content)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            string zSQL = "SELECT EmployeeKey, EmployeeID, LastName + ' ' + FirstName AS EmployeeName, DepartmentKey  FROM [dbo].[HRM_Employee] "
                        + "WHERE RecordStatus != 99 AND (EmployeeID LIKE @Content OR UserIPCAST LIKE @Content OR LastName + ' ' + FirstName LIKE @Content )";
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
            string[] zItem = new string[4];
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
                    zItem = new string[4];
                    zItem[0] = zRow["EmployeeKey"].ToString();
                    zItem[1] = zRow["EmployeeID"].ToString();
                    zItem[2] = zRow["EmployeeName"].ToString();
                    zItem[3] = zRow["DepartmentKey"].ToString();
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        public static List<string[]> KPI_Items()
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT ItemKey,ItemID,ItemName FROM [dbo].[KPI_Item]"
                + "WHERE Class = '01' AND Len(ItemID) = 1 AND InYear = 2023 ORDER BY ItemID ";
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
                string zstrMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;

            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[3];
                zItem[0] = zRow["ItemKey"].ToString();
                zItem[1] = zRow["ItemID"].ToString();
                zItem[2] = zRow["ItemName"].ToString();
                zResult.Add(zItem);
            }
            return zResult;
        }
        public static DataTable KPI_ItemByYear(int InYear)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT '' AS AutoKey,ItemID,ItemName,UnitKey,UnitName,0.0 AS QuantityTarget, 0.0 AS QuantityResult, 0 AS ScoresTarget, 0 AS ScoresResult FROM [dbo].[KPI_Item] " +
                          "WHERE Class = '01' AND InYear = @InYear " +
                          "ORDER BY ItemID";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
             

            }
            catch (Exception ex)
            {
                string strMessage = ex.ToString();
            }
            return zTable;
        }
        public static DataTable KPI_ItemParentByYear(int InYear)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT '' AS AutoKey,ItemID,ItemName,UnitKey,UnitName,0.0 AS QuantityTarget, 0.0 AS QuantityResult, 0 AS ScoresTarget, 0 AS ScoresResult FROM [dbo].[KPI_Item] " +
                          "WHERE Class = '01' AND InYear = @InYear AND LEN(ItemID) = 1 " +
                          "ORDER BY ItemID";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();


            }
            catch (Exception ex)
            {
                string strMessage = ex.ToString();
            }
            return zTable;
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
                for(int  i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                zResult.Add(zItem);
            }
            return zResult;
        }
        public static List<string[]> DataOfTable(string SQL,List<int> FieldsIsDate, int PageNumber, int PageSize)
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
                for(int i = 0; i < FieldsIsDate.Count;i++)
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
