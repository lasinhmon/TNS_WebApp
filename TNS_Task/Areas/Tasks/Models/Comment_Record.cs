using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;

namespace TNS_Task.Areas.Tasks.Models
{
    public class Comment_Record
    {
        public long EntryKey { get; set; }
        public string TaskKey { get; set; }
        public string Comment { get; set; }
        public string EmployeeKey { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime CreatedOn { get; set; }

        public string? Message { get; set; }
        public Comment_Record()
        {

        }
        public Comment_Record(long entryKey)
        {
            string zSQL = "SELECT * FROM [dbo].[CRM_Task_Comment] WHERE EntryKey = @EntryKey AND RecordStatus != 99 ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@EntryKey", SqlDbType.BigInt).Value = entryKey;
                SqlDataReader zReader = zCommand.ExecuteReader();
                if (zReader.HasRows)
                {
                    zReader.Read();
                    EntryKey = long.Parse(zReader["EntryKey"].ToString());
                    TaskKey = zReader["TaskKey"].ToString();
                    Comment = zReader["Comment"].ToString();
                    EmployeeKey = zReader["EmployeeKey"].ToString();
                    EmployeeName = zReader["EmployeeName"].ToString();
                    CreatedOn = (DateTime)zReader["CreatedOn"];

                    Message = "200 OK";
                }
                else
                {
                    Message = "404 Not Found";
                }
                zReader.Close();
                zCommand.Dispose();
            }
            catch (Exception Err)
            {
                Message = "501 " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
        }
        public Comment_Record(DataRow Item)
        {
            EntryKey = int.Parse(Item["EntryKey"].ToString());
            TaskKey = Item["TaskKey"].ToString();
            Comment = Item["Comment"].ToString();
            EmployeeKey = Item["EmployeeKey"].ToString();
            EmployeeName = Item["EmployeeName"].ToString();
            CreatedOn = (DateTime)Item["CreatedOn"];
        }
        public string Create()
        {
            string zSQL = "INSERT INTO [dbo].[CRM_Task_Comment] ("
                       + "TaskKey, Comment ,EmployeeKey ,EmployeeName,CreatedOn) "
                       + "VALUES ( "
                       + "@TaskKey,@Comment ,@EmployeeKey ,@EmployeeName,GetDate()) ";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(TaskKey);
                zCommand.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = Comment;
                zCommand.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = new Guid(EmployeeKey);
                zCommand.Parameters.Add("@EmployeeName", SqlDbType.NVarChar).Value = EmployeeName;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                Message = "201 Created";
            }
            catch (Exception Err)
            {
                Message = "501 " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        public string Delete()
        {
            string zResult = "";
            //---------- String SQL Access Database ---------------
            string zSQL = "UPDATE [dbo].[CRM_Task_Comment] Set RecordStatus = 99 WHERE EntryKey = @EntryKey";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@EntryKey", SqlDbType.BigInt).Value = EntryKey;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                Message = "200 OK";
            }
            catch (Exception Err)
            {
                Message = "501 " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
    }
}
