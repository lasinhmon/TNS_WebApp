using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TNS_KPI.Areas.KPI_Accounting.Models
{
    public class AnalyticsData
    {
        public static List<string[]> ReportQuick_HauKiemBT(int Month, int Year)
        {
            string zMessage = "";
            DataTable zTable = new DataTable();
            string zSQL = "";
            zSQL += "WITH ReportButtoan AS (";
            zSQL += "SELECT col_crtusr AS UserName, count(*) AS Total,0 AS QuantityChecked, 0 AS Normal, 0 AS Cancel FROM[dbo].[KPI_Accounting_HauKiemBT] ";
            zSQL += "WHERE InMonth = @InMonth AND InYear = @InYear ";
            zSQL += "GROUP BY col_crtusr ";
            zSQL += " UNION ALL ";
            zSQL += "SELECT col_bchkusr AS UserName, 0 AS Total,count(*) AS QuantityChecked, 0 AS Normal, 0 AS Cancel FROM[dbo].[KPI_Accounting_HauKiemBT] ";
            zSQL += "WHERE InMonth = @InMonth AND InYear = @InYear ";
            zSQL += "GROUP BY col_bchkusr ";
            zSQL += " UNION ALL ";
            zSQL += "SELECT col_crtusr AS UserName, 0 AS Total,0 AS QuantityChecked, count(*) AS Normal, 0 AS Cancel FROM[dbo].[KPI_Accounting_HauKiemBT] ";
            zSQL += "WHERE col_stscd = 'Normal' AND InMonth = @InMonth AND InYear = @InYear ";
            zSQL += "GROUP BY col_crtusr ";
            zSQL += " UNION ALL ";
            zSQL += "SELECT col_crtusr AS UserName, 0 AS Total,0 AS QuantityChecked, 0 AS Normal, count(*) AS Cancel FROM[dbo].[KPI_Accounting_HauKiemBT] ";
            zSQL += "WHERE col_stscd = 'Cancel' AND InMonth = @InMonth AND InYear = @InYear ";
            zSQL += "GROUP BY col_crtusr) ";

            zSQL += "SELECT UserName, SUM(Total) AS Total,SUM(QuantityChecked) AS QuantityChecked, SUM(Normal) AS Normal, SUM(Cancel) AS Cancel FROM ReportButtoan ";
            zSQL += "GROUP BY UserName ";
            zSQL += "ORDER BY SUM(Total) DESC, UserName DESC  ";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = Month;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = Year;
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
            string[] zItem = new string[5];
            if (zMessage.Length > 0)
            {
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zItem[2] = "";
                zItem[3] = "";
                zItem[4] = "";
                zResult.Add(zItem);
            }
            else
            {
                int zTotal,zQuantityChecked, zNormal, zCancel;
                int zTotalSum = 0, zQuantityCheckedSum = 0, zNormalSum = 0, zCancelSum = 0;
                foreach (DataRow zRow in zTable.Rows)
                {
                    zTotal = int.Parse(zRow["Total"].ToString());
                    zQuantityChecked = int.Parse(zRow["QuantityChecked"].ToString());
                    zNormal = int.Parse(zRow["Normal"].ToString());
                    zCancel = int.Parse(zRow["Cancel"].ToString());
                    zTotalSum += zTotal;
                    zQuantityCheckedSum += zQuantityChecked;
                    zNormalSum += zNormal;
                    zCancelSum += zCancel;

                    zItem = new string[5];
                    zItem[0] = zRow["UserName"].ToString();
                    zItem[1] = zTotal.ToString("#,###,##0");
                    zItem[2] = zNormal.ToString("#,###,##0");
                    zItem[3] = zCancel.ToString("#,###,##0");
                    zItem[4] = zQuantityChecked.ToString("#,###,##0");
                    zResult.Add(zItem);
                }

                string zStyle = "";
                if (zTotalSum != zNormalSum + zCancelSum)
                    zStyle += "color:maroon";
                zItem = new string[5];
                zItem[0] = "<span style='font-weight:bold;'>Tổng cộng</span>";
                zItem[1] = "<span style='font-weight:bold;'>" + zTotalSum.ToString("#,###,##0") + "</span>";
                zItem[2] = "<span style='font-weight:bold;" + zStyle + "'>" + zNormalSum.ToString("#,###,##0") + "</span>";
                zItem[3] = "<span style='font-weight:bold;" + zStyle + "'>" + zCancelSum.ToString("#,###,##0") + "</span>";
                zItem[4] = "<span style='font-weight:bold;" + zStyle + "'>" + zQuantityCheckedSum.ToString("#,###,##0") + "</span>";
                zResult.Insert(0, zItem);
            }
            return zResult;
        }
        public static List<string[]> ReportQuick_HauKiemBT_ByModule(int Month, int Year)
        {
            string zMessage = "";
            DataTable zTable = new DataTable();
            string zSQL = "";
            zSQL += "SELECT col_buscd, count(*) AS Total FROM [dbo].[KPI_Accounting_HauKiemBT] ";
            zSQL += "WHERE InMonth = @InMonth AND InYear = @InYear ";
            zSQL += "GROUP BY col_buscd ";
            zSQL += "ORDER BY count(*) DESC ";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = Month;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = Year;
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
                int zTotalSum = 0;
                foreach (DataRow zRow in zTable.Rows)
                {
                    zTotal = int.Parse(zRow["Total"].ToString());
                    //zTotalSum += zTotal;
                    zItem = new string[2];
                    zItem[0] = zRow["col_buscd"].ToString();
                    zItem[1] = zTotal.ToString("#,###,##0");
                    zResult.Add(zItem);
                }
               
            }

            return zResult;
        }

        public static List<string[]> ReportQuick_NoneHauKiemBT(int Month, int Year)
        {
            string zMessage = "";
            DataTable zTable = new DataTable();
            string zSQL = "";
            zSQL += "WITH ReportButtoanNone AS (";
            zSQL += "SELECT col_crtusr AS UserName, count(*) AS Total,0 AS QuantityChecked, 0 AS Normal, 0 AS Cancel FROM[dbo].[KPI_Accounting_HauKiemBT_None] ";
            zSQL += "WHERE InMonth = @InMonth AND InYear = @InYear ";
            zSQL += "GROUP BY col_crtusr ";
            zSQL += " UNION ALL ";
            zSQL += "SELECT col_bchkusr AS UserName, 0 AS Total,count(*) AS QuantityChecked, 0 AS Normal, 0 AS Cancel FROM[dbo].[KPI_Accounting_HauKiemBT_None] ";
            zSQL += "WHERE InMonth = @InMonth AND InYear = @InYear ";
            zSQL += "GROUP BY col_bchkusr ";
            zSQL += " UNION ALL ";
            zSQL += "SELECT col_crtusr AS UserName, 0 AS Total,0 AS QuantityChecked, count(*) AS Normal, 0 AS Cancel FROM[dbo].[KPI_Accounting_HauKiemBT_None] ";
            zSQL += "WHERE col_stscd = 'Normal' AND InMonth = @InMonth AND InYear = @InYear ";
            zSQL += "GROUP BY col_crtusr ";
            zSQL += " UNION ALL ";
            zSQL += "SELECT col_crtusr AS UserName, 0 AS Total,0 AS QuantityChecked, 0 AS Normal, count(*) AS Cancel FROM[dbo].[KPI_Accounting_HauKiemBT_None] ";
            zSQL += "WHERE col_stscd = 'Cancel' AND InMonth = @InMonth AND InYear = @InYear ";
            zSQL += "GROUP BY col_crtusr) ";

            zSQL += "SELECT UserName, SUM(Total) AS Total,SUM(QuantityChecked) AS QuantityChecked, SUM(Normal) AS Normal, SUM(Cancel) AS Cancel FROM ReportButtoanNone ";
            zSQL += "GROUP BY UserName ";
            zSQL += "ORDER BY SUM(Total) DESC, UserName DESC  ";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = Month;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = Year;
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
            string[] zItem = new string[5];
            if (zMessage.Length > 0)
            {
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zItem[2] = "";
                zItem[3] = "";
                zItem[4] = "";
                zResult.Add(zItem);
            }
            else
            {
                int zTotal, zQuantityChecked, zNormal, zCancel;
                int zTotalSum = 0, zQuantityCheckedSum = 0, zNormalSum = 0, zCancelSum = 0;
                foreach (DataRow zRow in zTable.Rows)
                {
                    zTotal = int.Parse(zRow["Total"].ToString());
                    zQuantityChecked = int.Parse(zRow["QuantityChecked"].ToString());
                    zNormal = int.Parse(zRow["Normal"].ToString());
                    zCancel = int.Parse(zRow["Cancel"].ToString());
                    zTotalSum += zTotal;
                    zQuantityCheckedSum += zQuantityChecked;
                    zNormalSum += zNormal;
                    zCancelSum += zCancel;

                    zItem = new string[5];
                    zItem[0] = zRow["UserName"].ToString();
                    zItem[1] = zTotal.ToString("#,###,##0");
                    zItem[2] = zNormal.ToString("#,###,##0");
                    zItem[3] = zCancel.ToString("#,###,##0");
                    zItem[4] = zQuantityChecked.ToString("#,###,##0");
                    zResult.Add(zItem);
                }

                string zStyle = "";
                if (zTotalSum != zNormalSum + zCancelSum)
                    zStyle += "color:maroon";
                zItem = new string[5];
                zItem[0] = "<span style='font-weight:bold;'>Tổng cộng</span>";
                zItem[1] = "<span style='font-weight:bold;'>" + zTotalSum.ToString("#,###,##0") + "</span>";
                zItem[2] = "<span style='font-weight:bold;" + zStyle + "'>" + zNormalSum.ToString("#,###,##0") + "</span>";
                zItem[3] = "<span style='font-weight:bold;" + zStyle + "'>" + zCancelSum.ToString("#,###,##0") + "</span>";
                zItem[4] = "<span style='font-weight:bold;" + zStyle + "'>" + zQuantityCheckedSum.ToString("#,###,##0") + "</span>";
                zResult.Insert(0, zItem);
            }
            return zResult;
        }
        public static List<string[]> ReportQuick_NoneHauKiemBT_ByModule(int Month, int Year)
        {
            string zMessage = "";
            DataTable zTable = new DataTable();
            string zSQL = "";
            zSQL += "SELECT col_buscd, count(*) AS Total FROM [dbo].[KPI_Accounting_HauKiemBT_None] ";
            zSQL += "WHERE InMonth = @InMonth AND InYear = @InYear ";
            zSQL += "GROUP BY col_buscd ";
            zSQL += "ORDER BY count(*) DESC ";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = Month;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = Year;
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
                int zTotalSum = 0;
                foreach (DataRow zRow in zTable.Rows)
                {
                    zTotal = int.Parse(zRow["Total"].ToString());
                    //zTotalSum += zTotal;
                    zItem = new string[2];
                    zItem[0] = zRow["col_buscd"].ToString();
                    zItem[1] = zTotal.ToString("#,###,##0");
                    zResult.Add(zItem);
                }

            }

            return zResult;
        }

        public static List<string[]> ReportQuick_PHUB(int Month, int Year)
        {
            string zMessage = "";
            DataTable zTable = new DataTable();
            string zSQL = "";
            zSQL += "SELECT GDV,Sum(TongSoGD) AS Total FROM [dbo].[KPI_Accounting_PHUB] ";
            zSQL += "WHERE InMonth = @InMonth AND InYear = @InYear ";
            zSQL += "GROUP BY GDV ";
            zSQL += "ORDER BY Sum(TongSoGD) DESC ";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = Month;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = Year;
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
                int zTotalSum = 0;
                foreach (DataRow zRow in zTable.Rows)
                {
                    zTotal = int.Parse(zRow["Total"].ToString());
                    //zTotalSum += zTotal;
                    zItem = new string[2];
                    zItem[0] = zRow["GDV"].ToString();
                    zItem[1] = zTotal.ToString("#,###,##0");
                    zResult.Add(zItem);
                }

            }

            return zResult;
        }
        public static List<string[]> ReportQuick_Argitax(int Month, int Year)
        {
            string zMessage = "";
            DataTable zTable = new DataTable();
            string zSQL = "";
            zSQL += "SELECT MaNhanVien, Count(*) AS Total FROM [dbo].[KPI_Accounting_Agritax] ";
            zSQL += "WHERE InMonth = @InMonth AND InYear = @InYear ";
            zSQL += "GROUP BY MaNhanVien ";
            zSQL += "ORDER BY Count(*) DESC ";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = Month;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = Year;
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
                int zTotalSum = 0;
                foreach (DataRow zRow in zTable.Rows)
                {
                    zTotal = int.Parse(zRow["Total"].ToString());
                    //zTotalSum += zTotal;
                    zItem = new string[2];
                    zItem[0] = zRow["MaNhanVien"].ToString();
                    zItem[1] = zTotal.ToString("#,###,##0");
                    zResult.Add(zItem);
                }

            }

            return zResult;
        }
        public static List<string[]> ReportQuick_AtCounter(int Month, int Year)
        {
            string zMessage = "";
            DataTable zTable = new DataTable();
            string zSQL = "";
            zSQL += "SELECT UserName,COUNT(*) AS Total FROM [dbo].[KPI_Accounting_AtCounter] ";
            zSQL += "WHERE AtCounter > 0 AND InMonth = @InMonth AND InYear = @InYear ";
            zSQL += "GROUP BY UserName ";
            zSQL += "ORDER BY Count(*) DESC ";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = Month;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = Year;
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
                int zTotalSum = 0;
                foreach (DataRow zRow in zTable.Rows)
                {
                    zTotal = int.Parse(zRow["Total"].ToString());
                    //zTotalSum += zTotal;
                    zItem = new string[2];
                    zItem[0] = zRow["UserName"].ToString();
                    zItem[1] = zTotal.ToString("#,###,##0");
                    zResult.Add(zItem);
                }

            }

            return zResult;
        }


        //=============================================
      
    }
}
