using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_KPI.Areas.KPI_Service.Models
{
    public class AnalyticsData
    {
        public static List<string[]> ReportQuick_SMS(int Month, int Year, string stts)
        {
            string zMessage = "";
            DataTable zTable = new DataTable();
            string zSQL = "";
            zSQL += "Select pgd, COUNT (*) AS TOTAL FROM [dbo].[KPI_Service_IssueSMS] ";
            zSQL += "WHERE InMonth = @InMonth AND InYear = @InYear AND stts = @stts ";
            zSQL += "Group by pgd ";
            zSQL += "ORDER BY COUNT (*) DESC ";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = Month;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = Year;
                zCommand.Parameters.Add("@stts", SqlDbType.NVarChar).Value = stts;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
                zMessage = "";
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem = new string[2];
            if (zMessage.Length > 0)
            {
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zResult.Add(zItem);
            }
            else
            {
                int zTotal;
                foreach (DataRow zRow in zTable.Rows)
                {
                    zTotal = int.Parse(zRow["Total"].ToString());
                    zItem = new string[2];
                    zItem[0] = zRow["pgd"].ToString();
                    zItem[1] = zTotal.ToString("#,###,##0");
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        public static List<string[]> ReportQuick_EMobile(int Month, int Year, string stts)
        {
            string zMessage = "";
            DataTable zTable = new DataTable();
            string zSQL = "";
            zSQL += "Select pgd, COUNT (*) AS TOTAL FROM [dbo].[KPI_Service_IssueEMobile] ";
            zSQL += "WHERE InMonth = @InMonth AND InYear = @InYear AND stts = @stts ";
            zSQL += "Group by pgd ";
            zSQL += "ORDER BY COUNT (*) DESC ";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = Month;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = Year;
                zCommand.Parameters.Add("@stts", SqlDbType.NVarChar).Value = stts;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
                zMessage = "";
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem = new string[2];
            if (zMessage.Length > 0)
            {
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zResult.Add(zItem);
            }
            else
            {
                int zTotal;
                foreach (DataRow zRow in zTable.Rows)
                {
                    zTotal = int.Parse(zRow["Total"].ToString());
                    zItem = new string[2];
                    zItem[0] = zRow["pgd"].ToString();
                    zItem[1] = zTotal.ToString("#,###,##0");
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
    }
}
