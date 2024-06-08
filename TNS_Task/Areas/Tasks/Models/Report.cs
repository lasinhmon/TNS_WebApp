using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_Task.Areas.Tasks.Models
{
    public class Report
    {
        public static int Task_AmountStatus(int Month, int Year, int StatusKey, string EmployeeKey)
        {
            int zResult = 0;
            string zMessage = "";

            DataTable zTable = new DataTable();
            string zSQL = "SELECT COUNT(*) FROM [dbo].[CRM_Task] "
                        + "WHERE OwnerBy = @EmployeeKey AND RecordStatus != 99 AND "
                        + "MONTH(StartDate) = @Month AND YEAR(StartDate) = @Year AND StatusKey = @StatusKey ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = new Guid(EmployeeKey);
                zCommand.Parameters.Add("@StatusKey", SqlDbType.Int).Value = StatusKey;
                zCommand.Parameters.Add("@Month", SqlDbType.Int).Value = Month;
                zCommand.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                string zReturn = zCommand.ExecuteScalar().ToString();
                int.TryParse(zReturn, out zResult);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            return zResult;
        }
        public static int Task_AmountStatus(int Month, int Year, int StatusKey, int PriorityKey, string EmployeeKey)
        {
            int zResult = 0;
            string zMessage = "";

            DataTable zTable = new DataTable();
            string zSQL = "SELECT COUNT(*) FROM [dbo].[CRM_Task] "
                        + "WHERE OwnerBy = @EmployeeKey AND RecordStatus != 99 AND "
                        + "MONTH(StartDate) = @Month AND YEAR(StartDate) = @Year AND StatusKey = @StatusKey AND PriorityKey = @PriorityKey ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = new Guid(EmployeeKey);
                zCommand.Parameters.Add("@StatusKey", SqlDbType.Int).Value = StatusKey;
                zCommand.Parameters.Add("@PriorityKey", SqlDbType.Int).Value = PriorityKey;
                zCommand.Parameters.Add("@Month", SqlDbType.Int).Value = Month;
                zCommand.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                string zReturn = zCommand.ExecuteScalar().ToString();
                int.TryParse(zReturn, out zResult);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            return zResult;
        }
        public static int Task_AmountStatus(int Month, int Year, string EmployeeKey)
        {
            int zResult = 0;
            string zMessage = "";

            DataTable zTable = new DataTable();
            string zSQL = "SELECT COUNT(*) FROM [dbo].[CRM_Task] "
                        + "WHERE OwnerBy = @EmployeeKey AND RecordStatus != 99 AND "
                        + "MONTH(StartDate) = @Month AND YEAR(StartDate) = @Year ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = new Guid(EmployeeKey);
                zCommand.Parameters.Add("@Month", SqlDbType.Int).Value = Month;
                zCommand.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                string zReturn = zCommand.ExecuteScalar().ToString();
                int.TryParse(zReturn, out zResult);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            return zResult;
        }
    }
}
