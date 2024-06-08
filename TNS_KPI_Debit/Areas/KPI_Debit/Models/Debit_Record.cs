using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_KPI.Areas.KPI_Debit.Models
{
    internal class Debit_Record
    {
        #region [ Field Name ]
        public string AutoKey { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        private int RecordStatus { get; set; }
        public string Style { get; set; }
        public string Class { get; set; }
        public string CodeLine { get; set; }
        public DateTime? CreatedOn { get; private set; }
        public string CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public DateTime? ModifiedOn { get; private set; }
        public string ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public string Message { get; set; }
        #endregion

        #region [ Constructor Get Information ]
        public Debit_Record()
        {

        }
        public Debit_Record(string autoKey)
        {
            string zSQL = "SELECT * FROM [dbo].[KPI_Debit_Customer] WHERE AutoKey = @AutoKey AND RecordStatus != 99 ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = new Guid(autoKey);
                SqlDataReader zReader = zCommand.ExecuteReader();
                if (zReader.HasRows)
                {
                    zReader.Read();
                    AutoKey = zReader["AutoKey"].ToString();
                    CustomerID = zReader["CustomerID"].ToString();
                    CustomerName = zReader["CustomerName"].ToString();
                    DepartmentID = zReader["DepartmentID"].ToString();
                    DepartmentName = zReader["DepartmentName"].ToString();
                    EmployeeID = zReader["EmployeeID"].ToString();
                    EmployeeName = zReader["EmployeeName"].ToString();
                    Telephone = zReader["Telephone"].ToString();
                    Address = zReader["Address"].ToString();
                    RecordStatus = int.Parse(zReader["RecordStatus"].ToString());
                    Style = zReader["Style"].ToString();
                    Class = zReader["Class"].ToString();
                    CodeLine = zReader["CodeLine"].ToString();
                    if (zReader["CreatedOn"] != DBNull.Value)
                        CreatedOn = (DateTime)zReader["CreatedOn"];
                    CreatedBy = zReader["CreatedBy"].ToString();
                    CreatedName = zReader["CreatedName"].ToString();
                    if (zReader["ModifiedOn"] != DBNull.Value)
                        ModifiedOn = (DateTime)zReader["ModifiedOn"];
                    ModifiedBy = zReader["ModifiedBy"].ToString();
                    ModifiedName = zReader["ModifiedName"].ToString();
                    Message = "OK";
                }
                else
                {
                    Message = "The record is not found";
                }
                zReader.Close();
                zCommand.Dispose();
            }
            catch (Exception Err)
            {
                Message = Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
        }

        public string IsHaveThisCustomer()
        {
            string zResult = "";
            string zSQL = "SELECT * FROM [dbo].[KPI_Debit_Customer] "
                        + "WHERE CustomerID = @CustomerID AND DepartmentID = @DepartmentID "
                        + "AND RecordStatus != 99 ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;
                zCommand.Parameters.Add("@DepartmentID", SqlDbType.NVarChar).Value = DepartmentID;
                SqlDataReader zReader = zCommand.ExecuteReader();
                if (zReader.HasRows)
                {
                    zResult = "OK";
                }
                zReader.Close();
                zCommand.Dispose();
            }
            catch (Exception Err)
            {
                zResult = "ERR";
                Message = Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        #endregion

        #region [ Constructor Update Information ]
        public string Create_Cast()
        {
            //---------- String SQL Access Database ---------------
            string zSQL = "INSERT INTO [dbo].[KPI_Debit_Customer] ("
                        + " CustomerID   ,CustomerName ,DepartmentID ,DepartmentName ,RecordStatus ,CreatedBy ,CreatedName ,ModifiedBy ,ModifiedName ) "
                        + " VALUES ( "
                        + "@CustomerID  ,@CustomerName ,@DepartmentID ,@DepartmentName ,@RecordStatus ,@CreatedBy ,@CreatedName ,@ModifiedBy ,@ModifiedName ) ";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;
                zCommand.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = CustomerName;
                zCommand.Parameters.Add("@DepartmentID", SqlDbType.NVarChar).Value = DepartmentID;
                zCommand.Parameters.Add("@DepartmentName", SqlDbType.NVarChar).Value = DepartmentName;
                zCommand.Parameters.Add("@RecordStatus", SqlDbType.Int).Value = RecordStatus;
                zCommand.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = CreatedBy;
                zCommand.Parameters.Add("@CreatedName", SqlDbType.NVarChar).Value = CreatedName;
                zCommand.Parameters.Add("@ModifiedBy", SqlDbType.NVarChar).Value = ModifiedBy;
                zCommand.Parameters.Add("@ModifiedName", SqlDbType.NVarChar).Value = ModifiedName;
                zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zResult = "OK";
                Message = "The record is created";
            }
            catch (Exception Err)
            {
                zResult = "ERR";
                Message = Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        public string Create_Cast_Employee()
        {
            //---------- String SQL Access Database ---------------
            string zSQL = "INSERT INTO [dbo].[KPI_Debit_Customer] ("
                        + " CustomerID  ,CustomerName ,DepartmentID ,DepartmentName ,EmployeeID,EmployeeName,RecordStatus ,CreatedBy ,CreatedName ,ModifiedBy ,ModifiedName ) "
                        + " VALUES ( "
                        + "@CustomerID  ,@CustomerName ,@DepartmentID ,@DepartmentName ,@EmployeeID,@EmployeeName,@RecordStatus ,@CreatedBy ,@CreatedName ,@ModifiedBy ,@ModifiedName ) ";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;
                zCommand.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = CustomerName;
                zCommand.Parameters.Add("@DepartmentID", SqlDbType.NVarChar).Value = DepartmentID;
                zCommand.Parameters.Add("@DepartmentName", SqlDbType.NVarChar).Value = DepartmentName;
                zCommand.Parameters.Add("@EmployeeID", SqlDbType.NVarChar).Value = EmployeeID;
                zCommand.Parameters.Add("@EmployeeName", SqlDbType.NVarChar).Value = EmployeeName;
                zCommand.Parameters.Add("@RecordStatus", SqlDbType.Int).Value = RecordStatus;
                zCommand.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = CreatedBy;
                zCommand.Parameters.Add("@CreatedName", SqlDbType.NVarChar).Value = CreatedName;
                zCommand.Parameters.Add("@ModifiedBy", SqlDbType.NVarChar).Value = ModifiedBy;
                zCommand.Parameters.Add("@ModifiedName", SqlDbType.NVarChar).Value = ModifiedName;
                zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zResult = "OK";
                Message = "The record is created";
            }
            catch (Exception Err)
            {
                zResult = "ERR";
                Message = Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        public string Create_Contract()
        {
            //---------- String SQL Access Database ---------------
            string zSQL = "INSERT INTO [dbo].[KPI_Debit_Customer] ("
                        + " CustomerID   ,CustomerName ,DepartmentID ,DepartmentName ,EmployeeID ,EmployeeName ,Telephone ,Address ,RecordStatus ,Style ,Class ,CodeLine ,CreatedBy ,CreatedName ,ModifiedBy ,ModifiedName ) "
                        + " VALUES ( "
                        + "@CustomerID  ,@CustomerName ,@DepartmentID ,@DepartmentName ,@EmployeeID ,@EmployeeName ,@Telephone ,@Address ,@RecordStatus ,@Style ,@Class ,@CodeLine ,@CreatedBy ,@CreatedName ,@ModifiedBy ,@ModifiedName ) ";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;
                zCommand.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = CustomerName;
                zCommand.Parameters.Add("@DepartmentID", SqlDbType.NVarChar).Value = DepartmentID;
                zCommand.Parameters.Add("@DepartmentName", SqlDbType.NVarChar).Value = DepartmentName;
                zCommand.Parameters.Add("@EmployeeID", SqlDbType.NVarChar).Value = EmployeeID;
                zCommand.Parameters.Add("@EmployeeName", SqlDbType.NVarChar).Value = EmployeeName;
                zCommand.Parameters.Add("@Telephone", SqlDbType.NVarChar).Value = Telephone;
                zCommand.Parameters.Add("@Address", SqlDbType.NVarChar).Value = Address;
                zCommand.Parameters.Add("@RecordStatus", SqlDbType.Int).Value = RecordStatus;
                zCommand.Parameters.Add("@Style", SqlDbType.NChar).Value = Style;
                zCommand.Parameters.Add("@Class", SqlDbType.NChar).Value = Class;
                zCommand.Parameters.Add("@CodeLine", SqlDbType.NChar).Value = CodeLine;
                zCommand.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = CreatedBy;
                zCommand.Parameters.Add("@CreatedName", SqlDbType.NVarChar).Value = CreatedName;
                zCommand.Parameters.Add("@ModifiedBy", SqlDbType.NVarChar).Value = ModifiedBy;
                zCommand.Parameters.Add("@ModifiedName", SqlDbType.NVarChar).Value = ModifiedName;
                zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zResult = "OK";
                Message = "The record is created";
            }
            catch (Exception Err)
            {
                zResult = "ERR";
                Message = Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }

        public string Update_Cast()
        {
            string zSQL = "UPDATE [dbo].[KPI_Debit_Customer] SET "
                        + " CustomerID = @CustomerID,"
                        + " CustomerName = @CustomerName,"
                        + " DepartmentID = @DepartmentID,"
                        + " DepartmentName = @DepartmentName,"
                        + " EmployeeID = @EmployeeID,"
                        + " EmployeeName = @EmployeeName,"
                        + " Telephone = @Telephone,"
                        + " Address = @Address,"
                        + " Style = @Stye,"
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
                zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = new Guid(AutoKey);
                zCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;
                zCommand.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = CustomerName;
                zCommand.Parameters.Add("@DepartmentID", SqlDbType.NVarChar).Value = DepartmentID;
                zCommand.Parameters.Add("@DepartmentName", SqlDbType.NVarChar).Value = DepartmentName;
                zCommand.Parameters.Add("@EmployeeID", SqlDbType.NVarChar).Value = EmployeeID;
                zCommand.Parameters.Add("@EmployeeName", SqlDbType.NVarChar).Value = EmployeeName;
                zCommand.Parameters.Add("@Telephone", SqlDbType.NVarChar).Value = Telephone;
                zCommand.Parameters.Add("@Address", SqlDbType.NVarChar).Value = Address;
                zCommand.Parameters.Add("@RecordStatus", SqlDbType.Int).Value = RecordStatus;
                zCommand.Parameters.Add("@Style", SqlDbType.NChar).Value = Style;
                zCommand.Parameters.Add("@ModifiedBy", SqlDbType.NVarChar).Value = ModifiedBy;
                zCommand.Parameters.Add("@ModifiedName", SqlDbType.NVarChar).Value = ModifiedName;
                zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zResult = "OK";
                Message = "The record is updated";
            }
            catch (Exception Err)
            {
                zResult = "ERR";
                Message = Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        public string Update_Contract()
        {
            string zSQL = "UPDATE [dbo].[KPI_Debit_Customer] SET "
                        + " CustomerID = @CustomerID,"
                        + " CustomerName = @CustomerName,"
                        + " DepartmentID = @DepartmentID,"
                        + " DepartmentName = @DepartmentName,"
                        + " EmployeeID = @EmployeeID,"
                        + " EmployeeName = @EmployeeName,"
                        + " Telephone = @Telephone,"
                        + " Address = @Address,"
                        + " Style = @Stye,"
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
                zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = new Guid(AutoKey);
                zCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;
                zCommand.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = CustomerName;
                zCommand.Parameters.Add("@DepartmentID", SqlDbType.NVarChar).Value = DepartmentID;
                zCommand.Parameters.Add("@DepartmentName", SqlDbType.NVarChar).Value = DepartmentName;
                zCommand.Parameters.Add("@EmployeeID", SqlDbType.NVarChar).Value = EmployeeID;
                zCommand.Parameters.Add("@EmployeeName", SqlDbType.NVarChar).Value = EmployeeName;
                zCommand.Parameters.Add("@Telephone", SqlDbType.NVarChar).Value = Telephone;
                zCommand.Parameters.Add("@Address", SqlDbType.NVarChar).Value = Address;
                zCommand.Parameters.Add("@RecordStatus", SqlDbType.Int).Value = RecordStatus;
                zCommand.Parameters.Add("@Style", SqlDbType.NChar).Value = Style;
                zCommand.Parameters.Add("@ModifiedBy", SqlDbType.NVarChar).Value = ModifiedBy;
                zCommand.Parameters.Add("@ModifiedName", SqlDbType.NVarChar).Value = ModifiedName;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zResult = "OK";
                Message = "The record is update";
            }
            catch (Exception Err)
            {
                zResult = "ERR";
                Message = Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        public string Update_Name(string Key, string Name)
        {
            string zSQL = "UPDATE [dbo].[KPI_Debit_Customer] SET "
                        + " CustomerName = @CustomerName"
                        + " WHERE AutoKey = @AutoKey";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = new Guid(Key);
                zCommand.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = Name;

                zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zResult = "OK";
            }
            catch (Exception Err)
            {
                zResult = "ERR";
                Message = Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        public string Update_Employeer()
        {
            string zSQL = "UPDATE [dbo].[KPI_Debit_Customer] SET "
                        + " EmployeeID = @EmployeeID,"
                        + " EmployeeName = @EmployeeName"
                        + " WHERE AutoKey = @AutoKey";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = new Guid(AutoKey);
                zCommand.Parameters.Add("@EmployeeID", SqlDbType.NVarChar).Value = EmployeeID;
                zCommand.Parameters.Add("@EmployeeName", SqlDbType.NVarChar).Value = EmployeeName;
                zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zResult = "OK";
            }
            catch (Exception Err)
            {
                zResult = "ERR ";
                Message = Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }

        public string Remove_Employeer()
        {
            string zSQL = "UPDATE [dbo].[KPI_Debit_Customer] SET "
                        + " EmployeeID = @EmployeeID,"
                        + " EmployeeName = @EmployeeName"
                        + " WHERE AutoKey = @AutoKey";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = new Guid(AutoKey);
                zCommand.Parameters.Add("@EmployeeID", SqlDbType.NVarChar).Value = DBNull.Value;
                zCommand.Parameters.Add("@EmployeeName", SqlDbType.NVarChar).Value = DBNull.Value;
                zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zResult = "OK";
            }
            catch (Exception Err)
            {
                zResult = "ERR ";
                Message = Err.ToString();
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
            string zSQL = "UPDATE [dbo].[KPI_Debit_Customer] Set RecordStatus = 99 WHERE AutoKey = @AutoKey";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = new Guid(AutoKey);
                zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zResult = "OK";
            }
            catch (Exception Err)
            {
                zResult = "ERR";
                Message = Err.ToString();
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
            string zSQL = "DELETE FROM [dbo].[KPI_Debit_Customer] WHERE AutoKey = @AutoKey";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = new Guid(AutoKey);
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zResult = "OK";
            }
            catch (Exception Err)
            {
                zResult = "OK";
                Message = Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        #endregion


        public static DataTable Department_GetAllConflic(int InMonth, int InYear, string DepartmentID, string Content)
        {
            DataTable zTable = new DataTable();

            string zSQL = @"  
                    WITH STK_Conflic AS (
	                    SELECT A.CustomerID AS STK  FROM KPI_Debit_Customer A
	                    WHERE A.InMonth = @InMonth AND A.InYear = @InYear AND DepartmentID = @DepartmentID AND (A.CustomerID LIKE @Content OR A.CustomerID LIKE @Content OR A.CustomerName LIKE @Content)  AND A.CustomerID IN (
				                      SELECT CustomerID
				                      FROM KPI_Debit_Customer
				                      WHERE InMonth = @InMonth  
					                    AND InYear = @InYear
				                      GROUP BY CustomerID  
				                      HAVING COUNT(*) >= 2
                      ) 
                    ) 

                    Select * From KPI_Debit_Customer A
                    where A.InMonth = @InMonth AND A.InYear = @InYear AND A.CustomerID in (SELECT * FROM STK_Conflic)
                    order by CustomerID ; ";
            string zMessage = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.NVarChar).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.NVarChar).Value = InYear;
                zCommand.Parameters.Add("@DepartmentID", SqlDbType.NVarChar).Value = DepartmentID;
                zCommand.Parameters.Add("@Content", SqlDbType.NVarChar).Value = "%" + Content + "%";
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();

            }
            catch (Exception ex)
            {
                string strMessage = ex.ToString();
            }
            return zTable;
        }

    }
}
