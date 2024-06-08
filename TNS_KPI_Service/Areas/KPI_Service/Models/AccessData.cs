using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TNS_KPI.Areas.KPI_Service.Models
{
    public class AccessData
    {
       
        public static List<string[]> GetDepartment(int InYear)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
             string zSQL = "SELECT A.DepartmentID,A.DepartmentName,B.Rank FROM [dbo].[KPI_Service_Report] A  " +
                           "LEFT JOIN [dbo].[HRM_Department] B ON A.DepartmentID = B.DepartmentID  " +
                           "WHERE A.InYear = @InYear  " +
                           "GROUP BY A.DepartmentID,A.DepartmentName,B.Rank ORDER BY B.Rank ";
            //string zSQL = "SELECT DepartmentID,DepartmentName FROM [dbo].[HRM_Department] " +
            //              "WHERE ForReport = 'KPI' ORDER BY Rank";
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
        public static List<string[]> DataReportByItem1(int InMonth, int InYear, string DepartmentID)
        {
            string zSQL = "SELECT ItemID,ItemName ";

            zSQL += ", [dbo].[KPI_Service_ReportItem](" + InMonth.ToString() + "," + InYear + ",'" + DepartmentID + "',ItemID) AS Month" + InMonth.ToString();

            zSQL += " FROM [dbo].[KPI_Service_Item] ORDER BY ItemID ";

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
                string zstrMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            int n = zTable.Columns.Count;
            double zTotal = 0;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n + 1];
                zItem[0] = zRow[0].ToString();
                zItem[1] = zRow[1].ToString();
                zTotal = 0;
                for (int i = 2; i < n; i++)
                {
                    double zVal = double.Parse(zRow[i].ToString());
                    zItem[i] = zVal.ToString("#,###,##0");
                    zTotal += zVal;
                }
                zItem[n] = zTotal.ToString("#,###,##0");
                zResult.Add(zItem);
            }
            return zResult;
        }
        public static List<string[]> DataReportByItem(int InYear, string DepartmentID)
        {
            string zSQL = "SELECT ItemID,ItemName ";
            for (int i = 1; i <= 12; i++)
            {
                zSQL += ", [dbo].[KPI_Service_ReportItem](" + i.ToString() + "," + InYear + ",'" + DepartmentID + "',ItemID) AS Month" + i.ToString();
            }
            zSQL += " FROM [dbo].[KPI_Service_Item] ORDER BY ItemID ";

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
                string zstrMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            int n = zTable.Columns.Count;
            double zTotal = 0;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n + 1];
                zItem[0] = zRow[0].ToString();
                zItem[1] = zRow[1].ToString();
                zTotal = 0;
                for (int i = 2; i < n; i++)
                {
                    double zVal = double.Parse(zRow[i].ToString());
                    zItem[i] = zVal.ToString("#,###,##0");
                    zTotal += zVal;
                }
                zItem[n] = zTotal.ToString("#,###,##0");
                zResult.Add(zItem);
            }
            return zResult;
        }
        public static List<string[]> DataReportByItem2(int InMonth, int InYear, List<string[]> Departments)
        {
            Departments = GetDepartment(InYear);
            string zSQL = "SELECT ItemID,ItemName ";
            foreach (string[] DepartmentID in Departments)
            {
                zSQL += ", [dbo].[KPI_Service_ReportItem](" + InMonth.ToString() + "," + InYear + ",'" + DepartmentID[0] + "',ItemID) AS Month" + InMonth.ToString();
            }
            zSQL += " FROM [dbo].[KPI_Service_Item] ORDER BY ItemID ";

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
                string zstrMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            int n = zTable.Columns.Count;
            double zTotal = 0;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n + 1];
                zItem[0] = zRow[0].ToString();
                zItem[1] = zRow[1].ToString();
                zTotal = 0;
                for (int i = 2; i < n; i++)
                {
                    double zVal = double.Parse(zRow[i].ToString());
                    zItem[i] = zVal.ToString("#,###,##0");
                    zTotal += zVal;
                }
                zItem[n] = zTotal.ToString("#,###,##0");
                zResult.Add(zItem);
            }
            return zResult;
        }

        public static double[] ServiceOfDepartment(int InYear, string DepartmentID)
        {
            string zSQL = "SELECT InMonth, Sum(QuantityResult) AS QuantityResult ";
            zSQL += " FROM [dbo].[KPI_Service_Report] ";
            zSQL += " WHERE (ItemID = 'I' OR ItemID = 'II' OR ItemID = 'III') AND InYear = @InYear AND DepartmentID = @DepartmentID ";
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
                zResult[zMonth - 1] = double.Parse(zRow["QuantityResult"].ToString());
            }
            return zResult;
        }
        public static List<string[]> DataReportByDepartment(int InYear)
        {
            List<string[]> zResult = new List<string[]>();
            List<string[]> zDepartments = GetDepartment(InYear);

            double[] columnSums = new double[15];
            foreach (string[] zInfo in zDepartments)
            {
                string[] zDepartmentInfo = new string[15];// dòng
                double zTotal = 0;
                zDepartmentInfo[0] = zInfo[0];
                zDepartmentInfo[1] = zInfo[2];
                double[] zQuantity = ServiceOfDepartment(InYear, zInfo[0]);
                for (int i = 0; i < 12; i++)// cột
                {
                    zDepartmentInfo[i + 2] = zQuantity[i].ToString("#,###,##0");
                    columnSums[i + 2] += zQuantity[i];
                    zTotal += zQuantity[i];
                }
                zDepartmentInfo[14] = zTotal.ToString("#,###,##0");
                columnSums[14] += zTotal;
                zResult.Add(zDepartmentInfo);
            }
            string[] sumRow = new string[15];
            sumRow[0] = "Total"; // Label for the sum row
            sumRow[1] = "Total";
            for (int i = 2; i < 15; i++)
            {
                sumRow[i] = columnSums[i].ToString("#,###,##0");
            }

            // Add the sum row to the result list
            zResult.Add(sumRow);
            return zResult;
        }
    }
}
