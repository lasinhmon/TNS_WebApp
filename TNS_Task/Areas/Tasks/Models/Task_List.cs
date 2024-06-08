using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_Task.Areas.Tasks.Models
{
    public class Task_List
    {
        private string DB_Fields = "*";
        public string ErrorMessage { get; set; }
        public Task_List()
        {

        }
        public Task_List(string Fields)
        {
            DB_Fields = Fields;
        }
        public DataTable Owner_List_Smart(int Amount, string Content, string EmployeeKey)
        {
            Content = Content.Trim();
            DataTable zTable = new DataTable();
            if (Content.Length == 0)
            {
                zTable = Owner_List(Amount, EmployeeKey);
            }
            else
            {
                bool zFromDateFound = false;
                DateTime zFromDate = new DateTime();
                bool zToDateFound = false;
                DateTime zToDate = new DateTime();
                string[] zItems = Content.Split(' ');
                if (zItems.Length > 0)
                {
                    zFromDateFound = Function.ConvertDateVN(zItems[0], out zFromDate);
                    if (zItems.Length >= 3)
                    {
                        zToDateFound = Function.ConvertDateVN(zItems[2], out zToDate);
                        if (zToDateFound && zItems[1].ToUpper() == "TO")
                            zToDateFound = true;
                        else
                            zToDateFound = false;
                    }
                }
                if (zFromDateFound && !zToDateFound)
                {
                    if (zItems.Length == 1)
                    {
                        zTable = Owner_List(Amount, zFromDate, EmployeeKey);
                    }
                    else
                    {
                        string zSearch = Content.Substring(zItems[0].Length, Content.Length - zItems[0].Length);
                        zTable = Owner_List(Amount, zFromDate, zSearch, EmployeeKey);
                    }
                }
                else if (zFromDateFound && zToDateFound)
                {
                    if (zItems.Length == 3)
                    {
                        zTable = Owner_List(Amount, zFromDate, zToDate, EmployeeKey);
                    }
                    else
                    {
                        string zSearch = "";
                        for (int i = 3; i < zItems.Length; i++)
                        {
                            zSearch += zItems[i] + " ";
                        }
                        zTable = Owner_List(Amount, zFromDate, zToDate, zSearch, EmployeeKey);
                    }
                }
                else
                {
                    zTable = Owner_List(Amount, Content, EmployeeKey);
                }
            }
            return zTable;
        }
        public DataTable Owner_List(int Amount, string EmployeeKey)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT TOP " + Amount + " " + DB_Fields + "  "
                        + "FROM [dbo].[CRM_Task] "
                        + "WHERE OwnerBy = @EmployeeKey AND RecordStatus != 99 "
                        + "ORDER BY CreatedOn DESC";
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
                ErrorMessage = ex.ToString();
            }
            return zTable;
        }
        public DataTable Owner_List(int Amount, string Content, string EmployeeKey)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT TOP " + Amount + " " + DB_Fields + "  "
                        + "FROM [dbo].[CRM_Task] "
                        + "WHERE OwnerBy = @EmployeeKey AND RecordStatus != 99 AND "
                        + "(Subject LIKE @Content OR TaskContent LIKE @Content OR PriorityName LIKE @Content OR StatusName LIKE @Content) "
                        + "ORDER BY CreatedOn DESC";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = new Guid(EmployeeKey);
                zCommand.Parameters.Add("@Content", SqlDbType.NVarChar).Value = "%" + Content.Trim() + "%";
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
            }
            return zTable;
        }
        public DataTable Owner_List(int Amount, DateTime AtDate, string EmployeeKey)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT TOP " + Amount + " " + DB_Fields + "  "
                        + "FROM [dbo].[CRM_Task] "
                        + "WHERE OwnerBy = @EmployeeKey AND RecordStatus != 99 AND ("
                        + "(DueDate BETWEEN @FromDate AND @ToDate) OR "
                        + "(StartDate BETWEEN @FromDate AND @ToDate) OR "
                        + "(FinishDate BETWEEN @FromDate AND @ToDate) OR "
                        + "(CreatedOn BETWEEN @FromDate AND @ToDate)) "
                        + "ORDER BY CreatedOn DESC";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = new Guid(EmployeeKey);
                zCommand.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = new DateTime(AtDate.Year, AtDate.Month, AtDate.Day, 00, 01, 01);
                zCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = new DateTime(AtDate.Year, AtDate.Month, AtDate.Day, 23, 50, 50);
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
            }

            return zTable;
        }
        public DataTable Owner_List(int Amount, DateTime AtDate, string Content, string EmployeeKey)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT TOP " + Amount + " " + DB_Fields + "  "
                        + "FROM [dbo].[CRM_Task] "
                        + "WHERE OwnerBy = @EmployeeKey AND RecordStatus != 99 AND "
                        + "(Subject LIKE @Content OR TaskContent LIKE @Content OR PriorityName LIKE @Content OR StatusName LIKE @Content) "
                        + "AND ((DueDate BETWEEN @FromDate AND @ToDate) OR "
                        + "(StartDate BETWEEN @FromDate AND @ToDate) OR "
                        + "(FinishDate BETWEEN @FromDate AND @ToDate) OR "
                        + "(CreatedOn BETWEEN @FromDate AND @ToDate)) "
                        + "ORDER BY CreatedOn DESC";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = new Guid(EmployeeKey);
                zCommand.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = new DateTime(AtDate.Year, AtDate.Month, AtDate.Day, 00, 01, 01);
                zCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = new DateTime(AtDate.Year, AtDate.Month, AtDate.Day, 23, 50, 50);
                zCommand.Parameters.Add("@Content", SqlDbType.NVarChar).Value = "%" + Content.Trim() + "%";
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
            }
            return zTable;
        }
        public DataTable Owner_List(int Amount, DateTime FromDate, DateTime ToDate, string EmployeeKey)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT TOP " + Amount + DB_Fields + "  "
                        + "FROM [dbo].[CRM_Task] "
                        + "WHERE OwnerBy = @EmployeeKey AND RecordStatus != 99 AND ("
                        + "(DueDate BETWEEN @FromDate AND @ToDate) OR "
                        + "(StartDate BETWEEN @FromDate AND @ToDate) OR "
                        + "(FinishDate BETWEEN @FromDate AND @ToDate) OR "
                        + "(CreatedOn BETWEEN @FromDate AND @ToDate)) "
                        + "ORDER BY CreatedOn DESC";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = new Guid(EmployeeKey);
                zCommand.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 00, 01, 01);
                zCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 50, 50);
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
            }

            return zTable;
        }
        public DataTable Owner_List(int Amount, DateTime FromDate, DateTime ToDate, string Content, string EmployeeKey)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT TOP " + Amount + " " + DB_Fields + "  "
                        + "FROM [dbo].[CRM_Task] "
                       + "WHERE OwnerBy = @EmployeeKey AND RecordStatus != 99 AND "
                        + "(Subject LIKE @Content OR TaskContent LIKE @Content OR PriorityName LIKE @Content OR StatusName LIKE @Content) "
                        + "AND ((DueDate BETWEEN @FromDate AND @ToDate) OR "
                        + "(StartDate BETWEEN @FromDate AND @ToDate) OR "
                        + "(FinishDate BETWEEN @FromDate AND @ToDate) OR "
                        + "(CreatedOn BETWEEN @FromDate AND @ToDate)) "
                        + "ORDER BY CreatedOn DESC";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = new Guid(EmployeeKey);
                zCommand.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 00, 01, 01);
                zCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 50, 50);
                zCommand.Parameters.Add("@Content", SqlDbType.NVarChar).Value = "%" + Content.Trim() + "%";
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
            }
            return zTable;
        }


        public DataTable Shared_List_Smart(int Amount, string Content, string EmployeeKey)
        {
            Content = Content.Trim();
            DataTable zTable = new DataTable();
            if (Content.Length == 0)
            {
                zTable = Shared_List(Amount, EmployeeKey);
            }
            else
            {
                bool zFromDateFound = false;
                DateTime zFromDate = new DateTime();
                bool zToDateFound = false;
                DateTime zToDate = new DateTime();
                string[] zItems = Content.Split(' ');
                if (zItems.Length > 0)
                {
                    zFromDateFound = Function.ConvertDateVN(zItems[0], out zFromDate);
                    if (zItems.Length >= 3)
                    {
                        zToDateFound = Function.ConvertDateVN(zItems[2], out zToDate);
                        if (zToDateFound && zItems[1].ToUpper() == "TO")
                            zToDateFound = true;
                        else
                            zToDateFound = false;
                    }
                }
                if (zFromDateFound && !zToDateFound)
                {
                    if (zItems.Length == 1)
                    {
                        //zTable = Shared_List(Amount, zFromDate, EmployeeKey);
                    }
                    else
                    {
                        string zSearch = Content.Substring(zItems[0].Length, Content.Length - zItems[0].Length);
                        //zTable = Shared_List(Amount, zFromDate, zSearch, EmployeeKey);
                    }
                }
                else if (zFromDateFound && zToDateFound)
                {
                    if (zItems.Length == 3)
                    {
                        // zTable = Shared_List(Amount, zFromDate, zToDate, EmployeeKey);
                    }
                    else
                    {
                        string zSearch = "";
                        for (int i = 3; i < zItems.Length; i++)
                        {
                            zSearch += zItems[i] + " ";
                        }
                        //zTable = Shared_List(Amount, zFromDate, zToDate, zSearch, EmployeeKey);
                    }
                }
                else
                {
                    zTable = Shared_List(Amount, Content, EmployeeKey);
                }
            }
            return zTable;
        }
        public DataTable Shared_List(int Amount, string EmployeeKey)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT TOP " + Amount + " " + DB_Fields + "  "
                        + "FROM [dbo].[CRM_Task] "
                        + "WHERE RecordStatus != 99 AND "
                        + "TaskKey IN (SELECT TaskKey FROM [dbo].[CRM_Task_Shared] WHERE EmployeeKey = @EmployeeKey AND RoleRead = 1) "
                        + "ORDER BY CreatedOn DESC";
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
                ErrorMessage = ex.ToString();
            }
            return zTable;
        }

        public DataTable Shared_List(int Amount, string Content, string EmployeeKey)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT TOP " + Amount + " " + DB_Fields + "  "
                        + "FROM [dbo].[CRM_Task] "
                        + "WHERE RecordStatus != 99 AND "
                        + "TaskKey IN (SELECT TaskKey FROM [dbo].[CRM_Task_Shared] WHERE EmployeeKey = @EmployeeKey AND RoleRead = 1) AND "
                        + "(Subject LIKE @Content OR TaskContent LIKE @Content OR PriorityName LIKE @Content OR StatusName LIKE @Content OR OwnerName LIKE @Content ) "
                        + "ORDER BY CreatedOn DESC";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = new Guid(EmployeeKey);
                zCommand.Parameters.Add("@Content", SqlDbType.NVarChar).Value = "%" + Content.Trim() + "%";
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
            }
            return zTable;
        }

    }
}
