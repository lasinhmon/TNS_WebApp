using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TNS_Task.Areas.Tasks.Models
{
    public class TaskShared_Record
    {
        public string TaskKey { get; set; }
        public string EmployeeKey { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string PhotoPath { get; set; }
        public bool RoleRead { get; set; }
        public bool RoleAdd { get; set; }
        public bool RoleEdit { get; set; }
        public bool RoleDel { get; set; }
        public bool RoleApproval { get; set; }
        public DateTime CreatedOn { get; set; }

        public string Message { get; set; }
        public TaskShared_Record()
        {

        }
        public TaskShared_Record(string taskKey, string employeeKey)
        {
            TaskKey = taskKey;
            EmployeeKey = employeeKey;
            EmployeeName = "";

            string zSQL = "SELECT * FROM [dbo].[CRM_Task_Shared] WHERE TaskKey = @TaskKey AND EmployeeKey = @EmployeeKey ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(taskKey);
                zCommand.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = new Guid(employeeKey);
                SqlDataReader zReader = zCommand.ExecuteReader();
                if (zReader.HasRows)
                {
                    zReader.Read();
                    TaskKey = zReader["TaskKey"].ToString();
                    EmployeeKey = zReader["EmployeeKey"].ToString();
                    EmployeeName = zReader["EmployeeName"].ToString();
                    RoleRead = (bool)zReader["RoleRead"];
                    RoleAdd = (bool)zReader["RoleAdd"];
                    RoleEdit = (bool)zReader["RoleEdit"];
                    RoleDel = (bool)zReader["RoleDel"];
                    RoleApproval = (bool)zReader["RoleApproval"];

                    CreatedOn = (DateTime)zReader["CreatedOn"];

                    Message = "OK ";
                }
                else
                {
                    Message = "ERR Not Found";
                }
                zReader.Close();
                zCommand.Dispose();
            }
            catch (Exception Err)
            {
                Message = "ERR " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            if (Message == "OK ")
            {
                List<string> zMoreInfo = AccessData.EmployeeGetInfo(EmployeeKey);
                EmployeeID = zMoreInfo[0];
                PhotoPath = zMoreInfo[1];
            }
        }
        public TaskShared_Record(DataRow RowRecord)
        {
            try
            {
                TaskKey = RowRecord["TaskKey"].ToString();
                EmployeeKey = RowRecord["EmployeeKey"].ToString();
                EmployeeName = RowRecord["EmployeeName"].ToString();
                RoleRead = (bool)RowRecord["RoleRead"];
                RoleAdd = (bool)RowRecord["RoleAdd"];
                RoleEdit = (bool)RowRecord["RoleEdit"];
                RoleDel = (bool)RowRecord["RoleDel"];
                RoleApproval = (bool)RowRecord["RoleApproval"];
                CreatedOn = (DateTime)RowRecord["CreatedOn"];

                List<string> zMoreInfo = AccessData.EmployeeGetInfo(EmployeeKey);
                EmployeeID = zMoreInfo[0];
                PhotoPath = zMoreInfo[1];
                Message = "OK ";
            }

            catch (Exception Err)
            {
                Message = "ERR " + Err.ToString();
            }

        }

        public string Create()
        {
            //---------- String SQL Access Database ---------------
            string zFieldName = "TaskKey,EmployeeKey,EmployeeName,RoleRead";
            string zFieldValue = "@TaskKey,@EmployeeKey,@EmployeeName,1";

            string zSQL = "INSERT INTO [dbo].[CRM_Task_Shared] (" + zFieldName + ") VALUES(" + zFieldValue + ")";

            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(TaskKey);
                zCommand.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = new Guid(EmployeeKey);
                zCommand.Parameters.Add("@EmployeeName", SqlDbType.NVarChar).Value = EmployeeName;

                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                Message = "OK ";
            }
            catch (Exception Err)
            {
                Message = "ERR " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        public string Update()
        {
            string zSQL = "UPDATE [dbo].[CRM_Task_Shared] SET ";
            zSQL += " RoleRead = @RoleRead,";
            zSQL += " RoleAdd = @RoleAdd,";
            zSQL += " RoleEdit= @RoleEdit,";
            zSQL += " RoleDel = @RoleDel,";
            zSQL += " RoleApproval = @RoleApproval";
            zSQL += " WHERE TaskKey = @TaskKey AND EmployeeKey = @EmployeeKey ";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(TaskKey);
                zCommand.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = new Guid(EmployeeKey);
                zCommand.Parameters.Add("@RoleRead", SqlDbType.Bit).Value = RoleRead;
                zCommand.Parameters.Add("@RoleAdd", SqlDbType.Bit).Value = RoleAdd;
                zCommand.Parameters.Add("@RoleEdit", SqlDbType.Bit).Value = RoleEdit;
                zCommand.Parameters.Add("@RoleDel", SqlDbType.Bit).Value = RoleDel;
                zCommand.Parameters.Add("@RoleApproval", SqlDbType.Bit).Value = RoleApproval;

                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                Message = "OK ";
            }
            catch (Exception Err)
            {
                Message = "ERR " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
    }
}
