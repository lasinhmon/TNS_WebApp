using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Xml.Linq;

namespace TNS_Task.Areas.Tasks.Models
{
    public class AccessData
    {
        public static List<string> EmployeeGetInfo(string EmployeeKey)
        {
            List<string> zResult = new List<string>();
            string zSQL = "SELECT EmployeeID, PhotoPath FROM [dbo].[HRM_Employee] WHERE EmployeeKey = @EmployeeKey AND RecordStatus != 99 ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = new Guid(EmployeeKey);
                SqlDataReader zReader = zCommand.ExecuteReader();
                if (zReader.HasRows)
                {
                    zReader.Read();
                    zResult.Add(zReader["EmployeeID"].ToString());
                    zResult.Add(zReader["PhotoPath"].ToString());

                }
                else
                {
                    zResult.Add("No Name");
                    zResult.Add("/images/users/avatar-0.jpg");
                }
                zReader.Close();
                zCommand.Dispose();
            }
            catch (Exception Err)
            {
                zResult.Add("No Name");
                zResult.Add("/images/users/avatar-0.jpg");
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }

        public static List<TaskShared_Record> Shared_Users(string TaskKey)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT * FROM [dbo].[CRM_Task_Shared] "
                       + "WHERE TaskKey = @TaskKey "
                       + "ORDER BY CreatedOn DESC";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(TaskKey);

                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.ToString();
            }
            List<TaskShared_Record> zListUser = new List<TaskShared_Record>();
            foreach (DataRow zRow in zTable.Rows)
            {
                zListUser.Add(new TaskShared_Record(zRow));
            }
            return zListUser;
        }
        public static DataTable Shared_SearchUsers(string Content)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT EmployeeKey, EmployeeID, LastName + ' ' + FirstName AS EmployeeName,PhotoPath FROM [dbo].[HRM_Employee] "
                        + "WHERE RecordStatus != 99 AND (EmployeeID LIKE @Content OR  LastName + ' ' + FirstName LIKE @Content )"
                        + "ORDER BY FirstName ";
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
                string zstrMessage = ex.ToString();
            }

            return zTable;
        }
        public static List<Comment_Record> Task_Comment(string TaskKey)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT * FROM [dbo].[CRM_Task_Comment] "
                        + "WHERE TaskKey = @TaskKey AND RecordStatus != 99 "
                        + "ORDER BY CreatedOn ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(TaskKey);

                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }

            List<Comment_Record> zResult = new List<Comment_Record>();
            foreach (DataRow zRow in zTable.Rows)
            {
                zResult.Add(new Comment_Record(zRow));
            }
            return zResult;
        }
        public static List<FileAttack_Record> Task_FileAttack(string TaskKey)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT * FROM [dbo].[CRM_Task_File] "
                        + "WHERE TaskKey = @TaskKey AND RecordStatus != 99 "
                        + "ORDER BY CreatedOn DESC";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(TaskKey);
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            List<FileAttack_Record> zResult = new List<FileAttack_Record>();
            foreach (DataRow zRow in zTable.Rows)
            {
                zResult.Add(new FileAttack_Record(zRow));
            }
            return zResult;
        }



        public static List<Task_Record> TaskOfParent_ListFull(string TaskKey)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT  *  "
                        + "FROM [dbo].[CRM_Task] "
                        + "WHERE ParentKey = @TaskKey "
                        + "ORDER BY CreatedOn DESC";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(TaskKey);
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            List<Task_Record> zResult = new List<Task_Record>();
            //foreach (DataRow zRow in zTable.Rows)
            //{
            //    Task_Record zItem = new Task_Record(zRow);

            //    zResult.Add(zItem);
            //}
            return zResult;
        }
        public static List<TN_Item> ReportToAndDepartment(string EmployeeKey)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT * FROM [dbo].[ListEmployeeInReportAndDepartment](@EmployeeKey)";
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
            List<TN_Item> zResult = new List<TN_Item>();
            foreach (DataRow zRow in zTable.Rows)
            {
                TN_Item zItem = new TN_Item();
                zItem.Name = zRow["LastName"].ToString() + " " + zRow["FirstName"].ToString();
                zItem.Value = zRow["EmployeeKey"].ToString();
                zItem.Obj = false;
                zResult.Add(zItem);
            }
            return zResult;
        }
        public static List<TN_Item> Priorities()
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT PriorityKey,PriorityName FROM [dbo].[CRM_TaskPriority] ORDER BY Rank ";
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

            List<TN_Item> zResult = new List<TN_Item>();
            foreach (DataRow zRow in zTable.Rows)
            {
                TN_Item zItem = new TN_Item();
                zItem.Name = zRow["PriorityName"].ToString();
                zItem.Value = zRow["PriorityKey"].ToString();
                zResult.Add(zItem);
            }
            return zResult;
        }
        public static List<TN_Item> Status()
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT StatusKey,StatusName FROM [dbo].[CRM_TaskStatus] "
                + " WHERE StatusKey <=3 ORDER BY Rank ";
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

            List<TN_Item> zResult = new List<TN_Item>();
            foreach (DataRow zRow in zTable.Rows)
            {
                TN_Item zItem = new TN_Item();
                zItem.Name = zRow["StatusName"].ToString();
                zItem.Value = zRow["StatusKey"].ToString();
                zResult.Add(zItem);
            }
            return zResult;
        }
    }
}