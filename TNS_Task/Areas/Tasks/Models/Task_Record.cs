using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_Task.Areas.Tasks.Models
{
    public class Task_Record
    {
        #region [ Field Name ]
        public string TaskKey { get; set; }
        public string? TaskCode { get; set; }
        public string? TaskCodeName { get; set; }
        public string? Subject { get; set; }
        public string? TaskContent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public float? Duration { get; set; }
        public int? StatusKey { get; set; }
        public string? StatusName { get; set; }
        public int? PriorityKey { get; set; }
        public string? PriorityName { get; set; }
        public int? Complete { get; set; }
        public int? CategoryKey { get; set; }
        public string? CategoryName { get; set; }
        public int? GroupKey { get; set; }
        public string? GroupName { get; set; }
        public string? ParentKey { get; set; }
        public string? CustomerKey { get; set; }
        public string? CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public string? ContractKey { get; set; }
        public string? ContractName { get; set; }
        public DateTime? Reminder { get; set; }
        public bool? Publish { get; set; }
        public int? RecordStatus { get; set; }
        public string? Class { get; set; }
        public DateTime? ApproveOn { get; set; }
        public string? ApproveBy { get; set; }
        public string? ApproveName { get; set; }
        public DateTime? OwnerOn { get; set; }
        public string? OwnerBy { get; set; }
        public string? OwnerName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedName { get; set; }
        public string? Message { get; set; }
        #endregion

        #region [ Constructor Get Information ]
        public Task_Record()
        {
            TaskKey = Guid.NewGuid().ToString();
            TaskCode = null;
            TaskCodeName = null;
            Subject = null;
            TaskContent = null;
            StartDate = null;
            DueDate = null;
            FinishDate = null;
            Duration = null;
            StatusKey = null;
            StatusName = null;
            PriorityKey = null;
            PriorityName = null;
            Complete = null;
            CategoryKey = null;
            CategoryName = null;
            GroupKey = null;
            GroupName = null;
            ParentKey = null;
            CustomerKey = null;
            CustomerID = null;
            CustomerName = null;
            ContractKey = null;
            ContractName = null;
            Reminder = null;

            Publish = null;
            RecordStatus = null;
            Class = null;

            ApproveOn = null;
            ApproveBy = null;
            ApproveName = null;
            OwnerOn = null;
            OwnerBy = null;
            OwnerName = null;
        }
        public Task_Record(string taskKey)
        {
            string zSQL = "SELECT * FROM [dbo].[CRM_Task] WHERE TaskKey = @TaskKey AND RecordStatus != 99 ";
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
                    TaskKey = zReader["TaskKey"].ToString();
                    TaskCode = zReader["TaskCode"].ToString();
                    TaskCodeName = zReader["TaskCodeName"].ToString();
                    Subject = zReader["Subject"].ToString();
                    TaskContent = zReader["TaskContent"].ToString();
                    if (zReader["StartDate"] != DBNull.Value)
                        StartDate = (DateTime)zReader["StartDate"];
                    if (zReader["DueDate"] != DBNull.Value)
                        DueDate = (DateTime)zReader["DueDate"];
                    if (zReader["FinishDate"] != DBNull.Value)
                        FinishDate = (DateTime)zReader["FinishDate"];
                    StatusKey = int.Parse(zReader["StatusKey"].ToString());
                    StatusName = zReader["StatusName"].ToString();
                    PriorityKey = int.Parse(zReader["PriorityKey"].ToString());
                    PriorityName = zReader["PriorityName"].ToString();
                    Complete = int.Parse(zReader["Complete"].ToString());
                    CategoryName = zReader["CategoryName"].ToString();
                    ParentKey = zReader["ParentKey"].ToString();
                    CustomerKey = zReader["CustomerKey"].ToString();
                    CustomerID = zReader["CustomerID"].ToString();
                    CustomerName = zReader["CustomerName"].ToString();
                    ContractKey = zReader["ContractKey"].ToString();
                    ContractName = zReader["ContractName"].ToString();
                    if (zReader["Reminder"] != DBNull.Value)
                        Reminder = (DateTime)zReader["Reminder"];

                    RecordStatus = int.Parse(zReader["RecordStatus"].ToString());
                    Class = zReader["Class"].ToString();

                    if (zReader["ApproveOn"] != DBNull.Value)
                        ApproveOn = (DateTime)zReader["ApproveOn"];
                    ApproveBy = zReader["ApproveBy"].ToString();
                    ApproveName = zReader["ApproveName"].ToString();

                    OwnerOn = (DateTime)zReader["OwnerOn"];
                    OwnerBy = zReader["OwnerBy"].ToString();
                    OwnerName = zReader["OwnerName"].ToString();
                    CreatedOn = (DateTime)zReader["CreatedOn"];
                    CreatedBy = zReader["CreatedBy"].ToString();
                    CreatedName = zReader["CreatedName"].ToString();
                    ModifiedOn = (DateTime)zReader["ModifiedOn"];
                    ModifiedBy = zReader["ModifiedBy"].ToString();
                    ModifiedName = zReader["ModifiedName"].ToString();
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

        #region [ Constructor Update Information ]
        public string Create()
        {
            //---------- String SQL Access Database ---------------
            string zFieldName = "TaskKey";
            string zFieldValue = "@TaskKey";
            if (TaskCode != null)
            {
                zFieldName += ",TaskCode";
                zFieldValue += ",@TaskCode";
            }

            if (TaskCodeName != null)
            {
                zFieldName += ",TaskCodeName";
                zFieldValue += ",@TaskCodeName";
            }
            if (Subject != null)
            {
                zFieldName += ",Subject";
                zFieldValue += ",@Subject";
            }
            if (TaskContent != null)
            {
                zFieldName += ",TaskContent";
                zFieldValue += ",@TaskContent";
            }

            if (StartDate != null)
            {
                zFieldName += ",StartDate";
                zFieldValue += ",@StartDate";
            }
            if (DueDate != null)
            {
                zFieldName += ",DueDate";
                zFieldValue += ",@DueDate";
            }
            if (FinishDate != null)
            {
                zFieldName += ",FinishDate";
                zFieldValue += ",@FinishDate";
            }

            if (StatusKey != null)
            {
                zFieldName += ",StatusKey";
                zFieldValue += ",@StatusKey";
            }
            if (StatusName != null)
            {
                zFieldName += ",StatusName";
                zFieldValue += ",@StatusName";
            }
            if (PriorityKey != null)
            {
                zFieldName += ",PriorityKey";
                zFieldValue += ",@PriorityKey";
            }
            if (PriorityName != null)
            {
                zFieldName += ",PriorityName";
                zFieldValue += ",@PriorityName";
            }
            if (Complete != null)
            {
                zFieldName += ",Complete";
                zFieldValue += ",@Complete";
            }
            if (CategoryKey != null)
            {
                zFieldName += ",CategoryKey";
                zFieldValue += ",@CategoryKey";
            }
            if (CategoryName != null)
            {
                zFieldName += ",CategoryName";
                zFieldValue += ",@CategoryName";
            }
            zFieldName += ",OwnerBy,OwnerName,CreatedBy,CreatedName,ModifiedBy,ModifiedName";
            zFieldValue += ",@OwnerBy,@OwnerName,@CreatedBy,@CreatedName,@ModifiedBy,@ModifiedName";

            string zSQL = "INSERT INTO [dbo].[CRM_Task] (" + zFieldName + ") VALUES(" + zFieldValue + ")";

            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(TaskKey);

                if (TaskCode != null)
                    zCommand.Parameters.Add("@TaskCode", SqlDbType.NVarChar).Value = TaskCode;
                if (TaskCodeName != null)
                    zCommand.Parameters.Add("@TaskCodeName", SqlDbType.NVarChar).Value = TaskCodeName;
                if (Subject != null)
                    zCommand.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = Subject;
                if (TaskContent != null)
                    zCommand.Parameters.Add("@TaskContent", SqlDbType.NText).Value = TaskContent;

                if (StartDate != null)
                    zCommand.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate;
                if (DueDate != null)
                    zCommand.Parameters.Add("@DueDate", SqlDbType.DateTime).Value = DueDate;
                if (FinishDate != null)
                    zCommand.Parameters.Add("@FinishDate", SqlDbType.DateTime).Value = FinishDate;
                if (StatusKey != null)
                    zCommand.Parameters.Add("@StatusKey", SqlDbType.Int).Value = StatusKey;
                if (StatusName != null)
                    zCommand.Parameters.Add("@StatusName", SqlDbType.NVarChar).Value = StatusName;
                if (PriorityKey != null)
                    zCommand.Parameters.Add("@PriorityKey", SqlDbType.Int).Value = PriorityKey;
                if (PriorityName != null)
                    zCommand.Parameters.Add("@PriorityName", SqlDbType.NVarChar).Value = PriorityName;
                if (Complete != null)
                    zCommand.Parameters.Add("@Complete", SqlDbType.Int).Value = Complete;
                if (CategoryKey != null)
                    zCommand.Parameters.Add("@CategoryKey", SqlDbType.Int).Value = CategoryKey;
                if (CategoryName != null)
                    zCommand.Parameters.Add("@CategoryName", SqlDbType.NVarChar).Value = CategoryName;

                zCommand.Parameters.Add("@OwnerBy", SqlDbType.UniqueIdentifier).Value = new Guid(CreatedBy);
                zCommand.Parameters.Add("@OwnerName", SqlDbType.NVarChar).Value = CreatedName;
                zCommand.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = new Guid(CreatedBy);
                zCommand.Parameters.Add("@CreatedName", SqlDbType.NVarChar).Value = CreatedName;
                zCommand.Parameters.Add("@ModifiedBy", SqlDbType.UniqueIdentifier).Value = new Guid(CreatedBy);
                zCommand.Parameters.Add("@ModifiedName", SqlDbType.NVarChar).Value = CreatedName;
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
            string zSQL = "UPDATE [dbo].[CRM_Task] SET ";
            if (TaskCode != null) zSQL += " TaskCode = @TaskCode,";
            if (TaskCodeName != null) zSQL += " TaskCodeName = @TaskCodeName,";
            if (Subject != null) zSQL += " Subject = @Subject,";
            if (TaskContent != null) zSQL += " TaskContent = @TaskContent,";
            if (StartDate != null) zSQL += " StartDate = @StartDate,";
            if (DueDate != null) zSQL += " DueDate = @DueDate,";
            if (FinishDate != null) zSQL += " FinishDate = @FinishDate,";
            if (Duration != null) zSQL += " Duration = @Duration,";
            if (StatusKey != null) zSQL += " StatusKey = @StatusKey,";
            if (StatusName != null) zSQL += " StatusName = @StatusName,";
            if (PriorityKey != null) zSQL += " PriorityKey = @PriorityKey,";
            if (PriorityName != null) zSQL += " PriorityName = @PriorityName,";
            if (Complete != null) zSQL += " Complete = @Complete,";
            if (CategoryKey != null) zSQL += " CategoryKey = @CategoryKey,";
            if (CategoryName != null) zSQL += " CategoryName = @CategoryName,";
            if (GroupKey != null) zSQL += " GroupKey = @GroupKey,";
            if (GroupName != null) zSQL += " GroupName = @GroupName,";
            if (ParentKey != null) zSQL += " ParentKey = @ParentKey,";
            if (CustomerKey != null) zSQL += " CustomerKey = @CustomerKey,";
            if (CustomerID != null) zSQL += " CustomerID = @CustomerID,";
            if (CustomerName != null) zSQL += " CustomerName = @CustomerName,";
            if (ContractKey != null) zSQL += " ContractKey = @ContractKey,";
            if (ContractName != null) zSQL += " ContractName = @ContractName,";
            if (Reminder != null) zSQL += " Reminder = @Reminder,";
            zSQL += " ModifiedOn = GetDate(),";
            zSQL += " ModifiedBy = @ModifiedBy,";
            zSQL += " ModifiedName = @ModifiedName";

            zSQL += " WHERE TaskKey = @TaskKey";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(TaskKey);
                if (TaskCode != null)
                    zCommand.Parameters.Add("@TaskCode", SqlDbType.NVarChar).Value = TaskCode;
                if (TaskCodeName != null)
                    zCommand.Parameters.Add("@TaskCodeName", SqlDbType.NVarChar).Value = TaskCodeName;
                if (Subject != null)
                    zCommand.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = Subject;
                if (TaskContent != null)
                    zCommand.Parameters.Add("@TaskContent", SqlDbType.NVarChar).Value = TaskContent;
                if (StartDate != null)
                    zCommand.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate;
                if (DueDate != null)
                    zCommand.Parameters.Add("@DueDate", SqlDbType.DateTime).Value = DueDate;
                if (FinishDate != null)
                    zCommand.Parameters.Add("@FinishDate", SqlDbType.DateTime).Value = FinishDate;
                if (Duration != null)
                    zCommand.Parameters.Add("@Duration", SqlDbType.Float).Value = Duration;
                if (StatusKey != null)
                    zCommand.Parameters.Add("@StatusKey", SqlDbType.Int).Value = StatusKey;
                if (StatusName != null)
                    zCommand.Parameters.Add("@StatusName", SqlDbType.NVarChar).Value = StatusName;
                if (PriorityKey != null)
                    zCommand.Parameters.Add("@PriorityKey", SqlDbType.Int).Value = PriorityKey;
                if (PriorityName != null)
                    zCommand.Parameters.Add("@PriorityName", SqlDbType.NVarChar).Value = PriorityName;
                if (Complete != null)
                    zCommand.Parameters.Add("@Complete", SqlDbType.Int).Value = Complete;
                if (CategoryKey != null)
                    zCommand.Parameters.Add("@CategoryKey", SqlDbType.Int).Value = CategoryKey;
                if (CategoryName != null)
                    zCommand.Parameters.Add("@CategoryName", SqlDbType.NVarChar).Value = CategoryName;
                if (GroupKey != null)
                    zCommand.Parameters.Add("@GroupKey", SqlDbType.Int).Value = GroupKey;
                if (GroupName != null)
                    zCommand.Parameters.Add("@GroupName", SqlDbType.NVarChar).Value = GroupName;
                if (ParentKey != null)
                    zCommand.Parameters.Add("@ParentKey", SqlDbType.UniqueIdentifier).Value = new Guid(ParentKey);

                if (CustomerKey != null)
                    zCommand.Parameters.Add("@CustomerKey", SqlDbType.UniqueIdentifier).Value = new Guid(CustomerKey);
                if (CustomerID != null)
                    zCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;
                if (CustomerName != null)
                    zCommand.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = CustomerName;
                if (ContractKey != null)
                    zCommand.Parameters.Add("@ContractKey", SqlDbType.UniqueIdentifier).Value = new Guid(ContractKey);
                if (ContractName != null)
                    zCommand.Parameters.Add("@ContractName", SqlDbType.NVarChar).Value = ContractName;
                if (Reminder != null)
                    zCommand.Parameters.Add("@Reminder", SqlDbType.DateTime).Value = DBNull.Value;


                zCommand.Parameters.Add("@ModifiedBy", SqlDbType.UniqueIdentifier).Value = new Guid(ModifiedBy);
                zCommand.Parameters.Add("@ModifiedName", SqlDbType.NVarChar).Value = ModifiedName;
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

        public string UpdateStatus()
        {
            string zResult = "";
            //---------- String SQL Access Database ---------------
            string zSQL = "UPDATE [dbo].[CRM_Task] Set StatusKey = @StatusKey, StatusName = @StatusName, Complete = @Complete "
                      + " WHERE TaskKey = @TaskKey ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(TaskKey);
                zCommand.Parameters.Add("@StatusKey", SqlDbType.Int).Value = StatusKey;
                zCommand.Parameters.Add("@StatusName", SqlDbType.NVarChar).Value = StatusName;
                zCommand.Parameters.Add("@Complete", SqlDbType.Int).Value = Complete;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                Message = "OK " + zResult;
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
        public string Delete_OwnerBy()
        {
            string zResult = "";
            //---------- String SQL Access Database ---------------
            string zSQL = "UPDATE [dbo].[CRM_Task] Set RecordStatus = 99 WHERE TaskKey = @TaskKey AND OwnerBy = @OwnerBy";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(TaskKey);
                zCommand.Parameters.Add("@OwnerBy", SqlDbType.UniqueIdentifier).Value = new Guid(OwnerBy);
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                Message = "OK " + zResult;
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
        public string Delete()
        {
            string zResult = "";
            //---------- String SQL Access Database ---------------
            string zSQL = "UPDATE [dbo].[CRM_Task] Set RecordStatus = 99 WHERE TaskKey = @TaskKey";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(TaskKey);
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
        public string Empty()
        {
            string zResult = "";
            //---------- String SQL Access Database ---------------
            string zSQL = "DELETE FROM [dbo].[CRM_Task] WHERE TaskKey = @TaskKey";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@TaskKey", SqlDbType.UniqueIdentifier).Value = new Guid(TaskKey);
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
        #endregion


    }


}
