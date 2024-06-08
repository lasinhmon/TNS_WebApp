using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TNS_Task.Areas.Tasks.Models
{
    public class FileAttack_Record
    {
        public long EntryKey { get; set; }
        public string TaskKey { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string FileSizeUnit { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EmployeeKey { get; set; }
        public string EmployeeName { get; set; }
        public string? Message { get; set; }
        public FileAttack_Record()
        {
        }
        public FileAttack_Record(long entryKey)
        {
            string zSQL = "SELECT * FROM [dbo].[CRM_Task_File] WHERE EntryKey = @EntryKey AND RecordStatus != 99 ";
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
                    EntryKey = int.Parse(zReader["EntryKey"].ToString());
                    TaskKey = zReader["TaskKey"].ToString();
                    FileName = zReader["FileName"].ToString();
                    FilePath = zReader["FilePath"].ToString();
                    FileSize = long.Parse(zReader["FileSize"].ToString());
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
        public FileAttack_Record(DataRow Item)
        {
            EntryKey = long.Parse(Item["EntryKey"].ToString());
            TaskKey = Item["TaskKey"].ToString();
            FileName = Item["FileName"].ToString();
            FilePath = Item["FilePath"].ToString();
            FileSize = long.Parse(Item["FileSize"].ToString());
            EmployeeKey = Item["EmployeeKey"].ToString();
            EmployeeName = Item["EmployeeName"].ToString();
            CreatedOn = (DateTime)Item["CreatedOn"];
            if (FileSize < 1024)
            {
                FileSizeUnit = "B";
            }
            else if (FileSize < 1048576)
            {
                FileSize = (int)(FileSize / 1024);
                FileSizeUnit = "KB";
            }
            else
            {
                FileSize = (int)(FileSize / 1048576);
                FileSizeUnit = "MB";
            }


        }
        public void Create()
        {
            string zSQL = "INSERT INTO [dbo].[CRM_Task_File] ("
                       + "TaskKey, FileName, FilePath, FileSize, EmployeeKey, EmployeeName,CreatedOn) "
                       + "VALUES ( "
                       + "@TaskKey,@FileName,@FilePath, @FileSize ,@EmployeeKey ,@EmployeeName,GetDate()) ";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(TaskKey);
                zCommand.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = FileName;
                zCommand.Parameters.Add("@FilePath", SqlDbType.NVarChar).Value = FilePath;
                zCommand.Parameters.Add("@FileSize", SqlDbType.BigInt).Value = FileSize;
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
        }

        public string Delete()
        {
            string zResult = "";
            //---------- String SQL Access Database ---------------
            string zSQL = "UPDATE [dbo].[CRM_Task_File] Set RecordStatus = 99 WHERE EntryKey = @EntryKey";
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
