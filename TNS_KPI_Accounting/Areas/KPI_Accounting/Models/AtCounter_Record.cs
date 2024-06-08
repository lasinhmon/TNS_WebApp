using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace TNS_KPI.Areas.KPI_Accounting.Models
{
    public class AtCounter_Record
    {

        #region [ Field Name ]
        private string _AutoKey = "";
        private string _UserName = "";
        private string _EmployeeID = "";
        private string _EmployeeName = "";
        private DateTime? _WorkingDate = null;
        private int _AtCounter = 0;
        private DateTime? _CreatedOn = null;
        private string _CreatedBy = "";
        private string _CreatedName = "";
        private DateTime? _ModifiedOn = null;
        private string _ModifiedBy = "";
        private string _ModifiedName = "";
        private string _Message = "";
        #endregion

        #region [ Constructor Get Information ]
        public AtCounter_Record()
        {
            Guid zNewID = Guid.NewGuid();
            _AutoKey = zNewID.ToString();
        }
        public AtCounter_Record(string userName, DateTime workingDate)
        {
            string zSQL = "SELECT * FROM [dbo].[KPI_Accounting_AtCounter] WHERE UserName = @UserName AND WorkingDate = @WorkingDate";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = userName;
                zCommand.Parameters.Add("@WorkingDate", SqlDbType.Date).Value = workingDate;
                SqlDataReader zReader = zCommand.ExecuteReader();
                if (zReader.HasRows)
                {
                    zReader.Read();
                    _AutoKey = zReader["AutoKey"].ToString();
                    _UserName = zReader["UserName"].ToString();
                    _EmployeeID = zReader["EmployeeID"].ToString();
                    _EmployeeName = zReader["EmployeeName"].ToString();
                    _WorkingDate = (DateTime)zReader["WorkingDate"];
                    _AtCounter = int.Parse(zReader["AtCounter"].ToString());
                    if (zReader["CreatedOn"] != DBNull.Value)
                        _CreatedOn = (DateTime)zReader["CreatedOn"];
                    _CreatedBy = zReader["CreatedBy"].ToString();
                    _CreatedName = zReader["CreatedName"].ToString();
                    if (zReader["ModifiedOn"] != DBNull.Value)
                        _ModifiedOn = (DateTime)zReader["ModifiedOn"];
                    _ModifiedBy = zReader["ModifiedBy"].ToString();
                    _ModifiedName = zReader["ModifiedName"].ToString();
                    _Message = "200 OK";
                }
                else
                {
                    _Message = "404 Not Found";
                }
                zReader.Close();
                zCommand.Dispose();
            }
            catch (Exception Err)
            {
                _Message = "501 " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
        }

        #endregion

        #region [ Properties ]
        public string AutoKey
        {
            get { return _AutoKey; }
            set { _AutoKey = value; }
        }
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        public string EmployeeID
        {
            get { return _EmployeeID; }
            set { _EmployeeID = value; }
        }
        public string EmployeeName
        {
            get { return _EmployeeName; }
            set { _EmployeeName = value; }
        }
        public DateTime? WorkingDate
        {
            get { return _WorkingDate; }
            set { _WorkingDate = value; }
        }
        public int AtCounter
        {
            get { return _AtCounter; }
            set { _AtCounter = value; }
        }
        public DateTime? CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
        public string CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        public string CreatedName
        {
            get { return _CreatedName; }
            set { _CreatedName = value; }
        }
        public DateTime? ModifiedOn
        {
            get { return _ModifiedOn; }
            set { _ModifiedOn = value; }
        }
        public string ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }
        public string ModifiedName
        {
            get { return _ModifiedName; }
            set { _ModifiedName = value; }
        }
        public string Code
        {
            get
            {
                if (_Message.Length >= 3)
                    return _Message.Substring(0, 3);
                else return "";
            }
        }
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
        #endregion

        #region [ Constructor Update Information ]

        public string Create_ServerKey()
        {
            //---------- String SQL Access Database ---------------
            string zSQL = "INSERT INTO [dbo].[KPI_Accounting_AtCounter] ("
        + " UserName ,EmployeeID ,EmployeeName ,WorkingDate ,AtCounter ,CreatedBy ,CreatedName ,ModifiedBy ,ModifiedName ) "
         + " VALUES ( "
         + "@UserName ,@EmployeeID ,@EmployeeName ,@WorkingDate ,@AtCounter ,@CreatedBy ,@CreatedName ,@ModifiedBy ,@ModifiedName ) ";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = _UserName;
                zCommand.Parameters.Add("@EmployeeID", SqlDbType.NVarChar).Value = _EmployeeID;
                zCommand.Parameters.Add("@EmployeeName", SqlDbType.NVarChar).Value = _EmployeeName;
                if (_WorkingDate == null)
                    zCommand.Parameters.Add("@WorkingDate", SqlDbType.Date).Value = DBNull.Value;
                else
                    zCommand.Parameters.Add("@WorkingDate", SqlDbType.Date).Value = _WorkingDate;
                zCommand.Parameters.Add("@AtCounter", SqlDbType.Int).Value = _AtCounter;
                zCommand.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = _CreatedBy;
                zCommand.Parameters.Add("@CreatedName", SqlDbType.NVarChar).Value = _CreatedName;
                zCommand.Parameters.Add("@ModifiedBy", SqlDbType.NVarChar).Value = _ModifiedBy;
                zCommand.Parameters.Add("@ModifiedName", SqlDbType.NVarChar).Value = _ModifiedName;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                _Message = "201 Created";
            }
            catch (Exception Err)
            {
                _Message = "501 " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }


        public string Create_ClientKey()
        {
            //---------- String SQL Access Database ---------------
            string zSQL = "INSERT INTO [dbo].[KPI_Accounting_AtCounter] ("
        + " AutoKey ,UserName ,EmployeeID ,EmployeeName ,WorkingDate ,AtCounter ,CreatedBy ,CreatedName ,ModifiedBy ,ModifiedName ) "
         + " VALUES ( "
         + "@AutoKey ,@UserName ,@EmployeeID ,@EmployeeName ,@WorkingDate ,@AtCounter ,@CreatedBy ,@CreatedName ,@ModifiedBy ,@ModifiedName ) ";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                if (_AutoKey.Length == 36)
                    zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = new Guid(_AutoKey);
                else
                    zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = DBNull.Value;
                zCommand.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = _UserName; zCommand.Parameters.Add("@EmployeeID", SqlDbType.NVarChar).Value = _EmployeeID;
                zCommand.Parameters.Add("@EmployeeName", SqlDbType.NVarChar).Value = _EmployeeName;
                if (_WorkingDate == null)
                    zCommand.Parameters.Add("@WorkingDate", SqlDbType.Date).Value = DBNull.Value;
                else
                    zCommand.Parameters.Add("@WorkingDate", SqlDbType.Date).Value = _WorkingDate;
                zCommand.Parameters.Add("@AtCounter", SqlDbType.Int).Value = _AtCounter;
                zCommand.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = _CreatedBy;
                zCommand.Parameters.Add("@CreatedName", SqlDbType.NVarChar).Value = _CreatedName;
                zCommand.Parameters.Add("@ModifiedBy", SqlDbType.NVarChar).Value = _ModifiedBy;
                zCommand.Parameters.Add("@ModifiedName", SqlDbType.NVarChar).Value = _ModifiedName;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                _Message = "201 Created";
            }
            catch (Exception Err)
            {
                _Message = "501 " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }


        public string Update()
        {
            string zSQL = "UPDATE [dbo].[KPI_Accounting_AtCounter] SET "
                        + " UserName = @UserName,"
                        + " EmployeeID = @EmployeeID,"
                        + " EmployeeName = @EmployeeName,"
                        + " WorkingDate = @WorkingDate,"
                        + " AtCounter = @AtCounter,"
                        + " ModifiedOn = GetDate(),"
                        + " ModifiedBy = @ModifiedBy,"
                        + " ModifiedName = @ModifiedName"
                       + " WHERE AutoKey = @AutoKey";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                if (_AutoKey.Length == 36)
                    zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = new Guid(_AutoKey);
                else
                    zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = DBNull.Value;
                zCommand.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = _UserName;
                zCommand.Parameters.Add("@EmployeeID", SqlDbType.NVarChar).Value = _EmployeeID;
                zCommand.Parameters.Add("@EmployeeName", SqlDbType.NVarChar).Value = _EmployeeName;
                if (_WorkingDate == null)
                    zCommand.Parameters.Add("@WorkingDate", SqlDbType.Date).Value = DBNull.Value;
                else
                    zCommand.Parameters.Add("@WorkingDate", SqlDbType.Date).Value = _WorkingDate;
                zCommand.Parameters.Add("@AtCounter", SqlDbType.Int).Value = _AtCounter;
                zCommand.Parameters.Add("@ModifiedBy", SqlDbType.NVarChar).Value = _ModifiedBy;
                zCommand.Parameters.Add("@ModifiedName", SqlDbType.NVarChar).Value = _ModifiedName;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                _Message = "200 OK";
            }
            catch (Exception Err)
            {
                _Message = "501 " + Err.ToString();
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
            string zSQL = "UPDATE [dbo].[KPI_Accounting_AtCounter] Set RecordStatus = 99 WHERE AutoKey = @AutoKey";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = new Guid(_AutoKey);
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                _Message = "200 OK";
            }
            catch (Exception Err)
            {
                _Message = "501 " + Err.ToString();
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
            string zSQL = "DELETE FROM [dbo].[KPI_AccountantAtCounter] WHERE AutoKey = @AutoKey";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = new Guid(_AutoKey);
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                _Message = "200 OK";
            }
            catch (Exception Err)
            {
                _Message = "501 " + Err.ToString();
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
