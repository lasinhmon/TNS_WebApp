using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_KPI.Areas.KPI_Accounting.Models
{
    public class AccessData
    {
        public static List<string[]> GetDepartment_DuNo(int InYear)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            /* string zSQL = "SELECT A.DepartmentID,A.DepartmentName,B.Rank FROM [dbo].[KPI_Service_Report] A  " +
                           "LEFT JOIN [dbo].[HRM_Department] B ON A.DepartmentID = B.DepartmentID  " +
                           "WHERE A.InYear = @InYear  " +
                           "GROUP BY A.DepartmentID,A.DepartmentName,B.Rank ORDER BY B.Rank ";*/
            //string zSQL = "SELECT DepartmentID,DepartmentName FROM [dbo].[HRM_Department] " +
            //              "WHERE ForReport = 'KPI' ORDER BY Rank";
            string zSQL = "SELECT distinct [DepartmentID] ,[DepartmentName] " +
                "FROM [TN_Banking].[dbo].[KPI_Debit_Department_Result] WHERE [InYear] =@InYear";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem = new string[4];
            if (zMessage.Length > 0)
            {
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zResult.Add(zItem);
            }
            else
            {
                foreach (DataRow zRow in zTable.Rows)
                {
                    zItem = new string[4];
                    zItem[0] = zRow["DepartmentID"].ToString();
                    zItem[1] = "";
                    zItem[2] = zRow["DepartmentName"].ToString();
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        public static List<string[]> GetDepartment(int InYear)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            /* string zSQL = "SELECT A.DepartmentID,A.DepartmentName,B.Rank FROM [dbo].[KPI_Service_Report] A  " +
                           "LEFT JOIN [dbo].[HRM_Department] B ON A.DepartmentID = B.DepartmentID  " +
                           "WHERE A.InYear = @InYear  " +
                           "GROUP BY A.DepartmentID,A.DepartmentName,B.Rank ORDER BY B.Rank ";*/
            //string zSQL = "SELECT DepartmentID,DepartmentName FROM [dbo].[HRM_Department] " +
            //              "WHERE ForReport = 'KPI' ORDER BY Rank";
            string zSQL = "SELECT distinct [DepartmentID] ,[DepartmentName] " +
                "FROM [TN_Banking].[dbo].[KPI_BankAccount_Department_Result] WHERE [InYear] =@InYear";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem = new string[4];
            if (zMessage.Length > 0)
            {
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zResult.Add(zItem);
            }
            else
            {
                foreach (DataRow zRow in zTable.Rows)
                {
                    zItem = new string[4];
                    zItem[0] = zRow["DepartmentID"].ToString();
                    zItem[1] = "";
                    zItem[2] = zRow["DepartmentName"].ToString();
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        public static double[] ServiceOfDepartment_DuNo(int InYear, string DepartmentID)
        {
            string zSQL = "SELECT InMonth, Sum(Customer_TotalMoney) AS Customer_TotalMoney ";
            zSQL += " FROM [KPI_Debit_Department_Result] ";
            zSQL += " WHERE InYear = @InYear AND DepartmentID = @DepartmentID ";
            zSQL += " GROUP BY InMonth ORDER BY InMonth ";

            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zCommand.Parameters.Add("@DepartmentID", SqlDbType.NVarChar).Value = DepartmentID;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            double[] zResult = new double[12];
            foreach (DataRow zRow in zTable.Rows)
            {
                int zMonth = int.Parse(zRow["InMonth"].ToString());
                zResult[zMonth - 1] = double.Parse(zRow["Customer_TotalMoney"].ToString());
            }
            return zResult;
        }
        public static double[] ServiceOfDepartment(int InYear, string DepartmentID)
        {
            string zSQL = "SELECT InMonth, Sum(Account_TotalMoney) AS Account_TotalMoney ";
            zSQL += " FROM [KPI_BankAccount_Department_Result] ";
            zSQL += " WHERE InYear = @InYear AND DepartmentID = @DepartmentID ";
            zSQL += " GROUP BY InMonth ORDER BY InMonth ";

            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zCommand.Parameters.Add("@DepartmentID", SqlDbType.NVarChar).Value = DepartmentID;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            double[] zResult = new double[12];
            foreach (DataRow zRow in zTable.Rows)
            {
                int zMonth = int.Parse(zRow["InMonth"].ToString());
                zResult[zMonth - 1] = double.Parse(zRow["Account_TotalMoney"].ToString());
            }
            return zResult;
        }
        public static List<string[]> DataDuNoByDepartment(int InYear)
        {
            List<string[]> zResult = new List<string[]>();
            List<string[]> zDepartments = GetDepartment_DuNo(InYear);

            foreach (string[] zInfo in zDepartments)
            {
                string[] zDepartmentInfo = new string[15];
                double zTotal = 0;
                zDepartmentInfo[0] = zInfo[0];
                zDepartmentInfo[1] = zInfo[2];
                double[] zQuantity = ServiceOfDepartment_DuNo(InYear, zInfo[0]);
                for (int i = 0; i < 12; i++)
                {
                    zDepartmentInfo[i + 2] = zQuantity[i].ToString("#,###,##0");
                    zTotal += zQuantity[i];
                }
                zDepartmentInfo[14] = zTotal.ToString("#,###,##0");
                zResult.Add(zDepartmentInfo);
            }
            return zResult;
        }
        public static List<string[]> DataReportByDepartment(int InYear)
        {
            List<string[]> zResult = new List<string[]>();
            List<string[]> zDepartments = GetDepartment(InYear);

            foreach (string[] zInfo in zDepartments)
            {
                string[] zDepartmentInfo = new string[15];
                double zTotal = 0;
                zDepartmentInfo[0] = zInfo[0];
                zDepartmentInfo[1] = zInfo[2];
                double[] zQuantity = ServiceOfDepartment(InYear, zInfo[0]);
                for (int i = 0; i < 12; i++)
                {
                    zDepartmentInfo[i + 2] = zQuantity[i].ToString("#,###,##0");
                    zTotal += zQuantity[i];
                }
                zDepartmentInfo[14] = zTotal.ToString("#,###,##0");
                zResult.Add(zDepartmentInfo);
            }
            return zResult;
        }
        public static List<string[]> AtCounter_GetEmployee(int AtMonth, int AtYear)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            string zSQL = "SELECT UserName,EmployeeID,EmployeeName " +
                          "FROM [dbo].[KPI_Accounting_AtCounter] " +
                          "WHERE MONTH(WorkingDate) = @AtMonth AND YEAR(WorkingDate) = @AtYear  AND AtCounter > 0 " +
                          "GROUP BY UserName,EmployeeID, EmployeeName";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@AtMonth", SqlDbType.Int).Value = AtMonth;
                zCommand.Parameters.Add("@AtYear", SqlDbType.Int).Value = AtYear;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem = new string[4];
            if (zMessage.Length > 0)
            {
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zResult.Add(zItem);
            }
            else
            {
                foreach (DataRow zRow in zTable.Rows)
                {
                    zItem = new string[4];
                    zItem[0] = zRow["UserName"].ToString();
                    zItem[1] = zRow["EmployeeID"].ToString();
                    zItem[2] = zRow["EmployeeName"].ToString();
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        public static List<string[]> AtCounter_GetEmployee(int AtMonth, int AtYear, string UserName)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            string zSQL = "SELECT DAY(WorkingDate) AS Days, AtCounter FROM [dbo].[KPI_Accounting_AtCounter] " +
                          "WHERE MONTH(WorkingDate) = @AtMonth AND YEAR(WorkingDate) = @AtYear AND AtCounter > 0 AND UserName = @UserName " +
                          "ORDER BY DAY(WorkingDate)";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@AtMonth", SqlDbType.Int).Value = AtMonth;
                zCommand.Parameters.Add("@AtYear", SqlDbType.Int).Value = AtYear;
                zCommand.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = UserName;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem = new string[2];
            if (zMessage.Length > 0)
            {
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zResult.Add(zItem);
            }
            else
            {
                foreach (DataRow zRow in zTable.Rows)
                {
                    zItem = new string[2];
                    zItem[0] = zRow["Days"].ToString();
                    zItem[1] = zRow["AtCounter"].ToString();
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        public static DataTable ScoresOfMonth(int Month, int Year, out string Message)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT * FROM [dbo].[KPI_Accounting_ButToan] "
                        + "WHERE InMonth = @InMonth AND InYear = @InYear  "
                        + "ORDER BY Ranking ";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = Month;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = Year;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
                Message = "";
            }
            catch (Exception ex)
            {
                Message = ex.ToString();
            }
            return zTable;
        }
        public static DataTable ScoresOfYear(string UserName,int Year, out string Message)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT * FROM [dbo].[KPI_Accounting_ButToan] "
                        + "WHERE InYear = @InYear AND UserName = @UserName "
                        + "ORDER BY Ranking ";

            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = UserName;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = Year;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
                Message = "";
            }
            catch (Exception ex)
            {
                Message = ex.ToString();
            }
            return zTable;
        }
        //code
        public static List<String[]> Accouting_Result(int InMonth, int InYear)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            string zSQL = "SELECT Ranking, UserName, EmployeeName, ButToanThucHien, ButToanHuy, ButToanAgritax, ButToanKhongHachToan," +
                " ButToanPHUB, HauKiemButToan, TongDiemQuyDoi, DiemCongPhuTrach ,HeSoCongViec, GDV_KS_ToTruong_PhuTrach," +
                "DiemThucHien, DiemBTBQ ,DiemBTBQ_Rate, Rate_Target, Note" +
                " FROM [dbo].[KPI_Accounting_Result]" +
                " WHERE InMonth = @InMonth AND InYear = @InYear  " +
                " ORDER BY Ranking DESC";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem = new string[18];
            if (zMessage.Length > 0)
            {
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zResult.Add(zItem);
            }
            else
            {
                foreach (DataRow zRow in zTable.Rows)
                {
                    zItem = new string[18];
                    zItem[0] = zRow["Ranking"].ToString();
                    zItem[1] = zRow["UserName"].ToString();
                    zItem[2] = zRow["EmployeeName"].ToString();
                    zItem[3] = zRow["ButToanThucHien"].ToString();
                    zItem[4] = zRow["ButToanHuy"].ToString();
                    zItem[5] = zRow["ButToanAgritax"].ToString();
                    zItem[6] = zRow["ButToanKhongHachToan"].ToString();
                    zItem[7] = zRow["ButToanPHUB"].ToString();
                    zItem[8] = zRow["HauKiemButToan"].ToString();
                    zItem[9] = zRow["TongDiemQuyDoi"].ToString();
                    zItem[10] = zRow["DiemCongPhuTrach"].ToString();
                    zItem[11] = zRow["HeSoCongViec"].ToString();
                    zItem[12] = zRow["GDV_KS_ToTruong_PhuTrach"].ToString();
                    zItem[13] = zRow["DiemThucHien"].ToString();
                    zItem[14] = zRow["DiemBTBQ"].ToString();
                    zItem[15] = zRow["DiemBTBQ_Rate"].ToString();
                    zItem[16] = zRow["Rate_Target"].ToString();
                    zItem[17] = zRow["Note"].ToString();
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        //

        public static List<string[]> HauKiemBT_GetEmployee(int InMonth, int InYear)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            string zSQL = "SELECT col_crtusr,LastName, FirstName FROM [dbo].[KPI_Accounting_HauKiemBT] A " +
                          "LEFT JOIN [dbo].[HRM_Employee] B ON A.col_crtusr = B.UserIPCAST  " +
                          "WHERE MONTH(col_opndt) = @InMonth AND YEAR(col_opndt) = @InYear " +
                          "GROUP BY col_crtusr,LastName, FirstName ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem = new string[4];
            if (zMessage.Length > 0)
            {
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zResult.Add(zItem);
            }
            else
            {
                foreach (DataRow zRow in zTable.Rows)
                {
                    zItem = new string[4];
                    zItem[0] = zRow["col_crtusr"].ToString();
                    zItem[1] = "";
                    zItem[2] = zRow["LastName"].ToString() + " " + zRow["FirstName"].ToString();
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        public static List<string[]> HauKiemBT_None_GetEmployee(int InMonth, int InYear)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            string zSQL = "SELECT col_crtusr,LastName, FirstName FROM [dbo].[KPI_Accounting_HauKiemBT_None] A " +
                          "LEFT JOIN [dbo].[HRM_Employee] B ON A.col_crtusr = B.UserIPCAST  " +
                          "WHERE MONTH(col_opndt) = @InMonth AND YEAR(col_opndt) = @InYear " +
                          "GROUP BY col_crtusr,LastName, FirstName ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem = new string[4];
            if (zMessage.Length > 0)
            {
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zResult.Add(zItem);
            }
            else
            {
                foreach (DataRow zRow in zTable.Rows)
                {
                    zItem = new string[4];
                    zItem[0] = zRow["col_crtusr"].ToString();
                    zItem[1] = "";
                    zItem[2] = zRow["LastName"].ToString() + " " + zRow["FirstName"].ToString();
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        public static string Insert(DataRow CustomerInfo)
        {
            DataTable zTable = new DataTable();
            string zSQL = " INSERT INTO [dbo].[KPI_CustomerDeparment] " +
                          " (MaPhongBan ,TenPhongBan,MaKhachHang,TenKhachHang,SoTKTienGui,DienThoai,DiaChi)" +
                          " VALUES (@MaPhongBan ,@TenPhongBan,@MaKhachHang,@TenKhachHang,@SoTKTienGui, @DienThoai,@DiaChi)";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                try
                {
                    SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                    zCommand.Parameters.Add("@MaPhongBan", SqlDbType.NVarChar).Value = CustomerInfo[1].ToString().Trim();
                    zCommand.Parameters.Add("@TenPhongBan", SqlDbType.NVarChar).Value = CustomerInfo[2].ToString().Trim();
                    zCommand.Parameters.Add("@MaKhachHang", SqlDbType.NVarChar).Value = CustomerInfo[3].ToString().Trim();
                    zCommand.Parameters.Add("@TenKhachHang", SqlDbType.NVarChar).Value = CustomerInfo[4].ToString().Trim();
                    zCommand.Parameters.Add("@SoTKTienGui", SqlDbType.NVarChar).Value = CustomerInfo[5].ToString().Trim();
                    zCommand.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = CustomerInfo[6].ToString().Trim();
                    zCommand.Parameters.Add("@DienThoai", SqlDbType.NVarChar).Value = CustomerInfo[7].ToString().Trim();

                    zCommand.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = "";
                    zCommand.Parameters.Add("@CreatedName", SqlDbType.NVarChar).Value = "";

                    zResult = zCommand.ExecuteNonQuery().ToString();
                    zCommand.Dispose();
                    zResult = "200 OK";
                }
                catch (Exception Err)
                {
                    string zErr = Err.ToString();
                    if (zErr.Contains("duplicate key"))
                        zResult = "400 Duplicate key";
                    else
                        zResult = "501 " + Err.ToString();
                }
                finally
                {
                    zConnect.Close();
                }
            }
            catch (Exception ex)
            {
                zResult = ex.ToString();
            }
            return zResult;
        }

        public static string[] EmployeeInfo(string UserIPCAST)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            string zSQL = "SELECT EmployeeKey,EmployeeID,LastName,FirstName FROM [dbo].[HRM_Employee] WHERE UserIPCAST = @UserIPCAST AND RecordStatus != 99 ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@UserIPCAST", SqlDbType.NVarChar).Value = UserIPCAST;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            string[] zResult = new string[4];
            if (zTable.Rows.Count > 0)
            {
                zResult[0] = zTable.Rows[0]["EmployeeKey"].ToString();
                zResult[1] = zTable.Rows[0]["EmployeeID"].ToString();
                zResult[2] = zTable.Rows[0]["LastName"].ToString();
                zResult[3] = zTable.Rows[0]["FirstName"].ToString();
            }
            
            return zResult;
        }
    }
}
