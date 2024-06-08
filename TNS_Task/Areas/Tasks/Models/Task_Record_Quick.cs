using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_Task.Areas.Tasks.Models
{
    public class Task_Record_Quick
    {
        #region [ Field Name ]
        public string TaskKey { get; set; }
        public string? TaskCode { get; set; }
        public string? TaskCodeName { get; set; }
        public string? Subject { get; set; }
        public string? TaskContent { get; set; }
        public string? StartDate { get; set; }
        public string? DueDate { get; set; }
        public string? FinishDate { get; set; }
        public int? StatusKey { get; set; }
        public string? StatusName { get; set; }
        public int? PriorityKey { get; set; }
        public string? PriorityName { get; set; }
        public int? Complete { get; set; }
        public string? Message { get; set; }
        #endregion

        #region [ Constructor Get Information ]
        public Task_Record_Quick()
        {

        }
        public Task_Record_Quick(string taskKey)
        {
            string zSQL = "SELECT TaskKey,Subject,TaskContent,StartDate,DueDate,FinishDate,StatusKey,StatusName,PriorityKey,PriorityName,Complete " +
                            "FROM [dbo].[CRM_Task] WHERE TaskKey = @TaskKey AND RecordStatus != 99 ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(taskKey);
                SqlDataReader zReader = zCommand.ExecuteReader();
                if (zReader.HasRows)
                {
                    zReader.Read();
                    DateTime zDateTime;
                    TaskKey = zReader["TaskKey"].ToString();
                    Subject = zReader["Subject"].ToString();
                    TaskContent = zReader["TaskContent"].ToString();
                    if (zReader["StartDate"] != DBNull.Value)
                    {
                        zDateTime = (DateTime)zReader["StartDate"];
                        StartDate = zDateTime.ToString("dd/MM/yyyy HH:mm");
                    }

                    if (zReader["DueDate"] != DBNull.Value)
                    {
                        zDateTime = (DateTime)zReader["DueDate"];
                        DueDate = zDateTime.ToString("dd/MM/yyyy HH:mm");
                    }
                    if (zReader["FinishDate"] != DBNull.Value)
                    {
                        zDateTime = (DateTime)zReader["FinishDate"];
                        FinishDate = zDateTime.ToString("dd/MM/yyyy HH:mm");
                    }

                    StatusKey = int.Parse(zReader["StatusKey"].ToString());
                    StatusName = zReader["StatusName"].ToString();
                    PriorityKey = int.Parse(zReader["PriorityKey"].ToString());
                    PriorityName = zReader["PriorityName"].ToString();
                    Complete = int.Parse(zReader["Complete"].ToString());

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
        }

        #endregion

    }


}
