using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace TNS_KPI.Areas.KPI_Service.Models
{
    public class Categories_AccessData
    {
        public static List<string[]> GetFileId()
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            string zSQL = "SELECT distinct fileid FROM[TN_Banking].[dbo].[KPI_Service_IssuePos] ";
            //+
            //"WHERE InMonth = @InMonth AND InYear = @InYear ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                //   zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                //  zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
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
            // string[] zItem = new string[4];
            if (zMessage.Length > 0)
            {
                string[] zItem = new string[2];
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zResult.Add(zItem);
            }
            else
            {
                string btn_Edit = "";
                string btn_Del = "";

                int i = 0;
                foreach (DataRow zRow in zTable.Rows)
                {
                    string[] zItem = new string[2];
                    zItem = new string[2];
                    zItem[0] = zRow["fileid"].ToString();
                    //    zItem[1] = "";
                    //    zItem[2] = zRow["LastName"].ToString() + " " + zRow["FirstName"].ToString();
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        public static string Delete(string crtusr)
        {
            string zSQL = "UPDATE [dbo].[KPI_Service_IssueEMobile] SET stts='InActive' WHERE crtusr=@crtusr";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@crtusr", SqlDbType.NVarChar).Value = crtusr;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();


                //  _Message = "201 Created";
            }
            catch (Exception Err)
            {
                // _Message = "501 " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        public static string Delete1(string crtusr)
        {
            string zSQL = "UPDATE [dbo].[KPI_Service_IssueSMS] SET stts='InActive' WHERE crtusr=@crtusr";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@crtusr", SqlDbType.NVarChar).Value = crtusr;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();


                //  _Message = "201 Created";
            }
            catch (Exception Err)
            {
                // _Message = "501 " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        public static string Update(string crtusr, string pgd)
        {
            string zSQL = "";
            zSQL += "INSERT into [dbo].[KPI_Service_IssueEMobile] ";
            zSQL += "([AutoKey] ,[InMonth] ,[InYear] ,[rmk] ,[stts] ,[entydt] ,[crtusr] ,[pgd] ,[brcd] ,[custseq] ,[smscustseq] ,[mblno1] ,[mblno2] ,[mblno3] ,[mblno4] ,[mblno5] ,[mblno1useyn] ,[mblno2useyn] ,[mblno3useyn] ,[mblno4useyn] ,[mblno5useyn] ,[trlmtamt] ,[cncldt] ,[crtdtm] ,[uptusr] ,[uptdtm] ,[nm] ,[nmloc] ,[custtpcd] ,[stscd]) ";
            zSQL += "Values (newid(), MONTH(GETDATE()), YEAR(GETDATE()), '', 'Active', CONVERT(DATE, GETDATE(), 120), @crtusr, @pgd, 0, '', '', '', '', '', '', '', '', '', '', '', '', 0, '', '', '', '', '', '', '', '') ";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;

                zCommand.Parameters.Add("@crtusr", SqlDbType.NVarChar).Value = crtusr;
                zCommand.Parameters.Add("@pgd", SqlDbType.NVarChar).Value = pgd;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                //  _Message = "201 Created";
            }
            catch (Exception Err)
            {
                // _Message = "501 " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;

            // return null;
        }
        public static string Update1(string crtusr, string pgd)
        {
            //string zSQL = "INSERT into [dbo].[KPI_Service_IssueSMS] Values (newid(),MONTH(GETDATE()), YEAR(GETDATE()),'Active',CONVERT(DATE, GETDATE(), 120),@crtusr,@pgd)";
            string zSQL = "";
            zSQL += "INSERT into [dbo].[KPI_Service_IssueSMS] ";
            zSQL += "([AutoKey], [InMonth], [InYear], [stts], [entydt], [crtusr], [pgd], [brcd], [custseq], [smscustseq], [mblno1], [mblno2], [mblno3], [mblno4], [mblno5], [mblno1useyn], [mblno2useyn], [mblno3useyn], [mblno4useyn], [mblno5useyn], [trlmtamt], [rmk], [cncldt], [crtdtm], [uptusr], [uptdtm], [nm], [nmloc], [custtpcd], [stscd]) ";
            zSQL += "Values (newid(), MONTH(GETDATE()), YEAR(GETDATE()), 'Active', CONVERT(DATE, GETDATE(), 120), @crtusr, @pgd, 0, '', '', '', '', '', '', '', '', '', '', '', '', 0, '', '', '', '', '', '', '', '', '') ";

            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;

                zCommand.Parameters.Add("@crtusr", SqlDbType.NVarChar).Value = crtusr;
                zCommand.Parameters.Add("@pgd", SqlDbType.NVarChar).Value = pgd;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                //  _Message = "201 Created";
            }
            catch (Exception Err)
            {
                // _Message = "501 " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;

            // return null;
        }
        public static List<string[]> EMobile_GetPGD(int InMonth, int InYear)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            string zSQL = "SELECT distinct pgd FROM[TN_Banking].[dbo].[KPI_Service_IssueEMobile] WHERE pgd <> '#N/A'";
            //+
            //"WHERE InMonth = @InMonth AND InYear = @InYear ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                //   zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                //  zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
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
            // string[] zItem = new string[4];
            if (zMessage.Length > 0)
            {
                string[] zItem = new string[2];
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zResult.Add(zItem);
            }
            else
            {
                string btn_Edit = "";
                string btn_Del = "";

                int i = 0;
                foreach (DataRow zRow in zTable.Rows)
                {
                    string[] zItem = new string[2];
                    zItem = new string[2];
                    zItem[0] = zRow["pgd"].ToString();
                    //    zItem[1] = "";
                    //    zItem[2] = zRow["LastName"].ToString() + " " + zRow["FirstName"].ToString();
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        public static List<string[]> SMS_GetPGD(int InMonth, int InYear)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            string zSQL = "SELECT distinct [pgd] FROM [TN_Banking].[dbo].[KPI_Service_IssueSMS] WHERE pgd <> '#N/A'";
            //+
            //"WHERE InMonth = @InMonth AND InYear = @InYear ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                //   zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                //  zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
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
            // string[] zItem = new string[4];
            if (zMessage.Length > 0)
            {
                string[] zItem = new string[2];
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zResult.Add(zItem);
            }
            else
            {
                string btn_Edit = "";
                string btn_Del = "";

                int i = 0;
                foreach (DataRow zRow in zTable.Rows)
                {
                    string[] zItem = new string[2];
                    zItem = new string[2];
                    zItem[0] = zRow["pgd"].ToString();
                    //    zItem[1] = "";
                    //    zItem[2] = zRow["LastName"].ToString() + " " + zRow["FirstName"].ToString();
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        public static List<string[]> EMobile_GetSTTS(int InMonth, int InYear)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            string zSQL = "SELECT distinct [stts] FROM [TN_Banking].[dbo].[KPI_Service_IssueEMobile]";
            //+
            //"WHERE InMonth = @InMonth AND InYear = @InYear ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                //   zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                //  zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
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
            // string[] zItem = new string[4];
            if (zMessage.Length > 0)
            {
                string[] zItem = new string[2];
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zResult.Add(zItem);
            }
            else
            {
                string btn_Edit = "";
                string btn_Del = "";

                int i = 0;
                foreach (DataRow zRow in zTable.Rows)
                {
                    string[] zItem = new string[2];
                    zItem = new string[2];
                    zItem[0] = zRow["stts"].ToString();
                    //    zItem[1] = "";
                    //    zItem[2] = zRow["LastName"].ToString() + " " + zRow["FirstName"].ToString();
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        public static List<string[]> SMS_GetSTTS(int InMonth, int InYear)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            string zSQL = "SELECT distinct [stts] FROM [TN_Banking].[dbo].[KPI_Service_IssueSMS]";
            //+
            //"WHERE InMonth = @InMonth AND InYear = @InYear ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                //   zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                //  zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
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
            // string[] zItem = new string[4];
            if (zMessage.Length > 0)
            {
                string[] zItem = new string[2];
                zItem[0] = "ERROR";
                zItem[1] = zMessage;
                zResult.Add(zItem);
            }
            else
            {
                string btn_Edit = "";
                string btn_Del = "";

                int i = 0;
                foreach (DataRow zRow in zTable.Rows)
                {
                    string[] zItem = new string[2];
                    zItem = new string[2];
                    zItem[0] = zRow["stts"].ToString();
                    //    zItem[1] = "";
                    //    zItem[2] = zRow["LastName"].ToString() + " " + zRow["FirstName"].ToString();
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        //public static List<string[]> SMS_Getcusttpcd(int InMonth, int InYear)
        //{
        //    DataTable zTable = new DataTable();
        //    string zMessage = "";
        //    string zSQL = "SELECT distinct [custtpcd] FROM [TN_Banking].[dbo].[KPI_Service_IssueSMS]";
        //    //+
        //    //"WHERE InMonth = @InMonth AND InYear = @InYear ";
        //    string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
        //    try
        //    {
        //        SqlConnection zConnect = new SqlConnection(zConnectionString);
        //        zConnect.Open();
        //        SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
        //        //   zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
        //        //  zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
        //        SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
        //        zAdapter.Fill(zTable);
        //        zCommand.Dispose();
        //        zConnect.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        zMessage = ex.ToString();
        //    }
        //    List<string[]> zResult = new List<string[]>();
        //    // string[] zItem = new string[4];
        //    if (zMessage.Length > 0)
        //    {
        //        string[] zItem = new string[2];
        //        zItem[0] = "ERROR";
        //        zItem[1] = zMessage;
        //        zResult.Add(zItem);
        //    }
        //    else
        //    {
        //        string btn_Edit = "";
        //        string btn_Del = "";

        //        int i = 0;
        //        foreach (DataRow zRow in zTable.Rows)
        //        {
        //            string[] zItem = new string[2];
        //            zItem = new string[2];
        //            zItem[0] = zRow["custtpcd"].ToString();
        //            //    zItem[1] = "";
        //            //    zItem[2] = zRow["LastName"].ToString() + " " + zRow["FirstName"].ToString();
        //            zResult.Add(zItem);
        //        }
        //    }
        //    return zResult;
        //}
        public static List<string[]> DataOfTable(string SQL, int PageNumber, int PageSize)
        {
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SQL += " OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY OPTION(RECOMPILE)";
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                zCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                zCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = PageNumber;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            int n = zTable.Columns.Count;// cột = 5



            foreach (DataRow zRow in zTable.Rows)
            {

                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {

                    zItem[i] = zRow[i].ToString();


                }

                zResult.Add(zItem);

            }
            return zResult;
        }

        public static List<string[]> DataOfTable2(string SQL, List<int> FieldsIsDate, int PageNumber, int PageSize)
        {
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SQL += " OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY OPTION(RECOMPILE)";
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                zCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                zCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = PageNumber;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                for (int i = 0; i < FieldsIsDate.Count; i++)
                {
                    zItem[FieldsIsDate[i]] = zItem[FieldsIsDate[i]].Split(' ')[0];
                }
                zResult.Add(zItem);
            }
            return zResult;
        }
        public static List<string[]> DataOfTable(string SQL, List<int> FieldsIsDate)
        {
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                for (int i = 0; i < FieldsIsDate.Count; i++)
                {
                    zItem[FieldsIsDate[i]] = zItem[FieldsIsDate[i]].Split(' ')[0];
                }
                zResult.Add(zItem);
            }
            return zResult;
        }
        public static List<string[]> DataOfTable(string SQL)
        {
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                zResult.Add(zItem);
            }
            return zResult;
        }

        public static DataTable GetData(string SQL)
        {
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
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
        public static int DataCount(string SQL)
        {
            int zQuantity = 0;
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                string zResult = zCommand.ExecuteScalar().ToString();
                int.TryParse(zResult, out zQuantity);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            return zQuantity;
        }

    }
}
