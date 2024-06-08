using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TNS_KPI.Areas.KPI_Debit.Models
{
    public class AccessData
    {
        public static DataTable DebitUpdate(int Amount, int InMonth, int InYear, string Content)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT TOP " + Amount.ToString() + " * FROM [dbo].[KPI_Debit_History] "
                        + "WHERE (CUSTSEQ LIKE @Content OR CUSTNM LIKE @Content OR TRCTCD LIKE @Content) AND "
                        + "InMonth = @InMonth AND InYear = @InYear ";


            zSQL += " ORDER BY CUSTNM, TRCTCD ";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zCommand.Parameters.Add("@Content", SqlDbType.NVarChar).Value = "%" + Content + "%";
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
        public static int DebitUpdate(int InMonth, int InYear, string Content)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT Count(*) FROM [dbo].[KPI_Debit_History] A "
                        + "WHERE (CUSTSEQ LIKE @Content OR CUSTNM LIKE @Content OR TRCTCD LIKE @Content) AND "
                        + "InMonth = @InMonth AND InYear = @InYear ";

            int zResult = 0;
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zCommand.Parameters.Add("@Content", SqlDbType.NVarChar).Value = "%" + Content + "%";
                string zReturn = zCommand.ExecuteScalar().ToString();
                int.TryParse(zReturn, out zResult);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string strMessage = ex.ToString();
            }
            return zResult;
        }
        public static DataTable DebitByTRCD(int Amount, int InMonth, int InYear, string TRCD, string Content)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT TOP " + Amount.ToString() + " * FROM [dbo].[KPI_Debit_History]  "
                        + "WHERE (CUSTSEQ LIKE @Content OR CUSTNM LIKE @Content OR TRCTCD LIKE @Content) AND "
                        + "InMonth = @InMonth AND InYear = @InYear ";
            if (TRCD.Length > 0)
                zSQL += " AND TRCTCD = @TRCD ";

            zSQL += " ORDER BY CUSTNM,TRCTCD ";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zCommand.Parameters.Add("@TRCD", SqlDbType.NVarChar).Value = TRCD;
                zCommand.Parameters.Add("@Content", SqlDbType.NVarChar).Value = "%" + Content + "%";
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
        public static int DebitByTRCD(int InMonth, int InYear, string TRCD, string Content)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT Count(*) FROM [dbo].[KPI_Debit_History] A "
                        + "WHERE (CUSTSEQ LIKE @Content OR CUSTNM LIKE @Content OR TRCTCD LIKE @Content) AND "
                        + "InMonth = @InMonth AND InYear = @InYear ";

            if (TRCD.Length > 0)
                zSQL += " AND TRCTCD = @TRCD ";
            int zResult = 0;
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zCommand.Parameters.Add("@TRCD", SqlDbType.NVarChar).Value = TRCD;
                zCommand.Parameters.Add("@Content", SqlDbType.NVarChar).Value = "%" + Content + "%";
                string zReturn = zCommand.ExecuteScalar().ToString();
                int.TryParse(zReturn, out zResult);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string strMessage = ex.ToString();
            }
            return zResult;
        }
        public static DataTable Department_Debit(int Amount, string InMonth, string InYear, string DepartmentID, string Content, string OrderBy)
        {
            DataTable zTable = new DataTable();
            string zSQL = @" SELECT TOP  " + Amount + @" A.AutoKey, A.CustomerID, A.CustomerName, A.Address,A.EmployeeID, A.EmployeeName, B.PhotoPath,  [dbo].[KPI_DebitConflictInDepartment](@DepartmentID, CustomerID) AS Conflict  
                        FROM[dbo].[KPI_Debit_Customer] A  
                        LEFT JOIN[dbo].[HRM_Employee] B ON A.EmployeeID = B.EmployeeID
                        WHERE A.InMonth = @InMonth AND A.InYear = @InYear AND A.DepartmentID = @DepartmentID AND A.RecordStatus != 99 
                        AND (A.CustomerName LIKE @Content OR A.EmployeeName LIKE @Content) ";
            if (OrderBy.Length > 0)
            {
                zSQL += "ORDER BY " + OrderBy;
            }
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);

                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zCommand.Parameters.Add("@DepartmentID", SqlDbType.NVarChar).Value = DepartmentID;
                zCommand.Parameters.Add("@Content", SqlDbType.NVarChar).Value = "%" + Content + "%";
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
        public static int Department_Debit_Count(string InMonth, string InYear ,string DepartmentID, string Content, string OrderBy)
        {
            int zResult = 0;
            DataTable zTable = new DataTable();
            string zSQL = @" SELECT Count(*) FROM [dbo].[KPI_Debit_Customer] A  
                        LEFT JOIN [dbo].[HRM_Employee] B ON A.EmployeeID = B.EmployeeID 
                        WHERE A.InMonth = @InMonth AND A.InYear = @InYear AND A.DepartmentID = @DepartmentID AND A.RecordStatus != 99 
                        AND (A.CustomerName LIKE @Content OR A.EmployeeName LIKE @Content) ";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);

                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zCommand.Parameters.Add("@DepartmentID", SqlDbType.NVarChar).Value = DepartmentID;
                zCommand.Parameters.Add("@Content", SqlDbType.NVarChar).Value = "%" + Content + "%";
                string zReturn = zCommand.ExecuteScalar().ToString();
                int.TryParse(zReturn, out zResult);
                zCommand.Dispose();
                zConnect.Close();

            }
            catch (Exception ex)
            {
                string strMessage = ex.ToString();
            }
            return zResult;
        }
        public static List<string[]> GetDepartment_DuNo(int InYear)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            /* string zSQL = "SELECT A.DepartmentID,A.DepartmentName,B.Rank FROM [dbo].[KPI_Service_Report] A  " +
                           "LEFT JOIN [dbo].[HRM_Department] B ON A.DepartmentID = B.DepartmentID  " +
                           "WHERE A.InYear = @InYear  " +
                           "GROUP BY A.DepartmentID,A.DepartmentName,B.Rank ORDER BY B.Rank ";*/
            //string zSQL = "SELECT DepartmentID,DepartmentName FROM [dbo].[HRM_Department] " +
            //              "WHERE ForReport = 'KPI' ORDER BY Rank";
            string zSQL = "SELECT distinct [DepartmentID] ,[DepartmentName] " +
                "FROM [TN_Banking].[dbo].[KPI_Debit_Department_Result] WHERE [InYear] =@InYear";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
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
                    zItem[0] = zRow["DepartmentID"].ToString();
                    zItem[1] = "";
                    zItem[2] = zRow["DepartmentName"].ToString();
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        public static double[] ServiceOfDepartment_DuNo(int InYear, string DepartmentID)
        {
            string zSQL = "SELECT InMonth, Sum(Customer_TotalMoney) AS Customer_TotalMoney ";
            zSQL += " FROM [KPI_Debit_Department_Result] ";
            zSQL += " WHERE InYear = @InYear AND DepartmentID = @DepartmentID ";
            zSQL += " GROUP BY InMonth ORDER BY InMonth ";

            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zCommand.Parameters.Add("@DepartmentID", SqlDbType.NVarChar).Value = DepartmentID;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            double[] zResult = new double[12];
            foreach (DataRow zRow in zTable.Rows)
            {
                int zMonth = int.Parse(zRow["InMonth"].ToString());
                zResult[zMonth - 1] = double.Parse(zRow["Customer_TotalMoney"].ToString());
            }
            return zResult;
        }
        public static List<string[]> DataDuNoByDepartment(int InYear)
        {
            List<string[]> zResult = new List<string[]>();
            List<string[]> zDepartments = GetDepartment_DuNo(InYear);

            foreach (string[] zInfo in zDepartments)
            {
                string[] zDepartmentInfo = new string[15];
                double zTotal = 0;
                zDepartmentInfo[0] = zInfo[0];
                zDepartmentInfo[1] = zInfo[2];
                double[] zQuantity = ServiceOfDepartment_DuNo(InYear, zInfo[0]);
                for (int i = 0; i < 12; i++)
                {
                    zDepartmentInfo[i + 2] = zQuantity[i].ToString("#,###,##0");
                    zTotal += zQuantity[i];
                }
                zDepartmentInfo[14] = zTotal.ToString("#,###,##0");
                zResult.Add(zDepartmentInfo);
            }
            return zResult;
        }

    }
}
