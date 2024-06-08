using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TNS_KPI.Areas.KPI_Accounting.Models
{
    public class Personal_Info
    {
        public string TenNhanVien { get; set; }
        public int[] TieuChi_TongButToan { get; set; }
        public int[] TieuChi_ButToanXuLy { get; set; }
        public int WorkingAtCounter_1 { get { return _WorkingAtCounter_1; } }
        public int WorkingAtCounter_2 { get { return _WorkingAtCounter_2; } }
        public int WorkingAtCounter { get { return WorkingAtCounter_1 + _WorkingAtCounter_2; } }
        /*
        4. SỐ BÚT TOÁN THỰC HIỆN 
        5. SỐ BÚT TOÁN HỦY 
        6. SỐ BÚT TOÁN AGRITAX 
        7. SỐ BÚT TOÁN KHÔNG HẠCH TOÁN 
        8. SỐ BÚT TOÁN PHUB 
        9. HẬU KIỂM BT 
        10. TỔNG ĐIỂM QUY ĐỔI   
        11. ĐIỂM CỘNG GDV PHỤ TRÁCH QUẢN LÝ ATM; SỔ PHỤ, THẺ    
        12. HỆ SỐ CÔNG VIỆC CỦA GDV 
        13. BQ BT GDV KS VÀ TỔ TRƯỞNG PHỤ TRÁCH 
        14. ĐIỂM THỰC HIỆN BT CỦA GDV TRONG THÁNG 
        15. ĐIỂM BÚT TOÁN BÌNH QUÂN/1 GDV TRONG THÁNG TẠI TỪNG ĐƠN VỊ 
        16. ĐIỂM BT BÌNH QUÂN +/- SO VỚI ĐIỂM BT BÌNH QUÂN CỦA GDV TRONG THÁNG  
        17. TỶ LỆ ĐẠT SO VỚI ĐIỂM BT BQ CỦA GDV 
        18. GHI CHÚ
        */
        private DataTable _PhuLuc;
        private string _col_crtusr;
        private int _WorkingAtCounter_1;
        private int _WorkingAtCounter_2;
        private int _Month;
        private int _Year;
        private string _Message = "";
        public Personal_Info(string Col_crtusr, int Month, int Year)
        {
            _col_crtusr = Col_crtusr;
            _Month = Month;
            _Year = Year;
            string[] zInfo = AccessData.EmployeeInfo(_col_crtusr);
            TenNhanVien = zInfo[2] + " " + zInfo[3];

            TieuChi_TongButToan = new int[19];
        }
        #region [ Phu Luc ]
        public void PhuLuc_Get()
        {
            _PhuLuc = GetDataList("SELECT * , 0 AS Quantity,0 AS Scores FROM [dbo].[KPI_Accounting_PhuLuc] ORDER BY Ranking");
        }
        public int[] PhuLuc_Calculator_I(string col_stscd)
        {
            int[] zVal = new int[2];
            int zQuantity_Sum = 0;
            int zScore_Sum = 0;
            int k = 0;
            PhuLuc_ButToan_Reset(col_stscd);
            foreach (DataRow zRow in _PhuLuc.Rows)
            {
                if (zRow["PhuLucID"].ToString() == "I")
                {
                    zVal = PhuLuc_ButToan_ByDate(col_stscd, zRow["Module"].ToString(), zRow["MaCodeNV"].ToString(), zRow["Analysis"].ToString(),
                                                int.Parse(zRow["GDV_QuayLoai1"].ToString()), int.Parse(zRow["GDV_QuayLoai2"].ToString()));
                    zRow["Quantity"] = zVal[0];
                    zRow["Scores"] = zVal[1];
                    zQuantity_Sum += zVal[0];
                    zScore_Sum += zVal[1];
                    k++;
                }
            }
            return new int[2] { zQuantity_Sum, zScore_Sum };
        }
        public void PhuLuc_Calculator_III()
        {
            int[] zVal = new int[2];
            //DataRow zRow = _PhuLuc.Rows[0];
            int k = 0;
            PhuLuc_ButToanNone_Reset();
            foreach (DataRow zRow in _PhuLuc.Rows)
            {
                if (zRow["PhuLucID"].ToString() == "III")
                {
                    zVal = PhuLuc_ButToanNone_ByDate(zRow["Module"].ToString(), zRow["MaCodeNV"].ToString(), zRow["Analysis"].ToString(),
                                                int.Parse(zRow["GDV_QuayLoai1"].ToString()), int.Parse(zRow["GDV_QuayLoai2"].ToString()));
                    zRow["Quantity"] = zVal[0];
                    zRow["Scores"] = zVal[1];

                    k++;
                }
            }
        }
        private string PhuLuc_ButToan_Reset(string col_stscd)
        {
            string zSQL = "UPDATE KPI_Accounting_HauKiemBT SET RepeatCalculator = 0 "
                    + "WHERE col_stscd = '" + col_stscd + "' AND InMonth = " + _Month + " AND InYear = " + _Year;

            return ExecuteNonQuery(zSQL);
        }
        private string PhuLuc_ButToanNone_Reset()
        {
            string zSQL = "UPDATE KPI_Accounting_HauKiemBT SET RepeatCalculator = 0 "
                    + "WHERE InMonth = " + _Month + " AND InYear = " + _Year;

            return ExecuteNonQuery(zSQL);
        }
        private JsonResult PhuLuc_ButToan_Strouble(string col_stscd)
        {
            string zSQL = "SELECT * FROM  KPI_Accounting_HauKiemBT "
                    + " WHERE RepeatCalculator != 1 AND col_stscd = '" + col_stscd + "' ANDInMonth = " + _Month + " AND InYear = " + _Year
                    + " ORDER BY RepeatCalculator DESC ";

            DataTable zData = GetDataList(zSQL);
            TableView zTable = new TableView();

            zTable.FieldsName.Add("Module");
            zTable.FieldsName.Add("Mã Code NV");
            zTable.FieldsName.Add("Tên bút toán");
            zTable.FieldsName.Add("Số Lượng");
            zTable.FieldsName.Add("Điểm số");

            int zQuantity;
            int zScores;

            int zQuantity_Total = 0;
            int zScores_Total = 0;
            string[] zItem;
            foreach (DataRow zRow in zData.Rows)
            {
                zQuantity = (int)zRow["Quantity"];
                zScores = (int)zRow["Scores"];
                if (zQuantity != 0 || zScores != 0)
                {
                    zItem = new string[6];
                    zItem[0] = zRow["AutoKey"].ToString();
                    zItem[1] = zRow["Module"].ToString();
                    zItem[2] = zRow["MaCodeNV"].ToString();
                    zItem[3] = zRow["TenButtoan"].ToString();
                    zItem[4] = zRow["Quantity"].ToString();
                    zItem[5] = zRow["Scores"].ToString();

                    zTable.DataOfTable.Add(zItem);

                    zQuantity_Total += zQuantity;
                    zScores_Total += zScores;
                }
            }
            zItem = new string[6];
            zItem[0] = "";
            zItem[1] = "";
            zItem[2] = "";
            zItem[3] = "TỔNG CỘNG";
            zItem[4] = zQuantity_Total.ToString("#,##0");
            zItem[5] = zScores_Total.ToString("#,##0");

            zTable.DataOfTable.Add(zItem);
            return new JsonResult(zTable);
        }
        private int[] PhuLuc_ButToan_ByDate(string col_stscd, string Module, string MaCodeNV, string Analysis, int Scores_Counter1, int Scores_Counter2)
        {
            string zFieldDate = " CAST(CASE WHEN col_crtdt IS NOT NULL THEN col_crtdt ELSE col_opndt END AS date) AS WorkingDate,";
            string zFieldQuantity = " COUNT(*) AS Quantity,";
            string zFieldAtCounter = " [dbo].[KPI_Accounting_WorkingAtLocation](CAST(CASE WHEN col_crtdt IS NOT NULL THEN col_crtdt ELSE col_opndt END AS date),'" + _col_crtusr + "') AS AtCounter ";
            string zGroupBy = " CAST(CASE WHEN col_crtdt IS NOT NULL THEN col_crtdt ELSE col_opndt END AS date) ";
            string zWhere = " WHERE col_stscd = '" + col_stscd + "' AND InMonth = " + _Month + " AND InYear = " + _Year + " AND col_crtusr = '" + _col_crtusr + "'" +
                " AND col_buscd = '" + Module + "' AND col_bchkcd = " + MaCodeNV + " " + Analysis;

            string zSQL = "SELECT " + zFieldDate + zFieldQuantity + zFieldAtCounter + " FROM [dbo].[KPI_Accounting_HauKiemBT] " + zWhere
               + " GROUP BY " + zGroupBy;

            DataTable zTable = GetDataList(zSQL);
            int zQuantity;
            int zQuantity_Sum = 0;
            int zScore_Sum = 0;

            foreach (DataRow zRow in zTable.Rows)
            {
                zQuantity = int.Parse(zRow["Quantity"].ToString());
                if (zRow["AtCounter"].ToString() == "1")
                {
                    zScore_Sum += zQuantity * Scores_Counter1;
                }
                if (zRow["AtCounter"].ToString() == "2")
                {
                    zScore_Sum += zQuantity * Scores_Counter2;
                }
                zQuantity_Sum += zQuantity;
            }
            zSQL = "UPDATE  KPI_Accounting_HauKiemBT SET RepeatCalculator = RepeatCalculator + 1 " + zWhere;
            ExecuteNonQuery(zSQL);

            return new int[2] { zQuantity_Sum, zScore_Sum };
        }
        private int[] PhuLuc_ButToanNone_ByDate(string Module, string MaCodeNV, string Analysis, int Scores_Counter1, int Scores_Counter2)
        {
            string zFieldDate = " CAST(CASE WHEN col_crtdt IS NOT NULL THEN col_crtdt ELSE col_opndt END AS date) AS WorkingDate,";
            string zFieldQuantity = " COUNT(*) AS Quantity,";
            string zFieldAtCounter = " [dbo].[KPI_Accounting_WorkingAtLocation](CAST(CASE WHEN col_crtdt IS NOT NULL THEN col_crtdt ELSE col_opndt END AS date),'" + _col_crtusr + "') AS AtCounter ";
            string zGroupBy = " CAST(CASE WHEN col_crtdt IS NOT NULL THEN col_crtdt ELSE col_opndt END AS date) ";
            string zWhere = " WHERE InMonth = " + _Month + " AND InYear = " + _Year + " AND col_crtusr = '" + _col_crtusr + "'" +
                " AND col_buscd = '" + Module + "' AND col_bchkcd = " + MaCodeNV + " " + Analysis;

            string zSQL = "SELECT " + zFieldDate + zFieldQuantity + zFieldAtCounter + " FROM [dbo].[KPI_Accounting_HauKiemBT_None] " + zWhere
               + " GROUP BY " + zGroupBy;

            DataTable zTable = GetDataList(zSQL);
            int zQuantity;
            int zQuantity_Sum = 0;
            int zScore_Sum = 0;

            foreach (DataRow zRow in zTable.Rows)
            {
                zQuantity = int.Parse(zRow["Quantity"].ToString());
                if (zRow["AtCounter"].ToString() == "1")
                {
                    zScore_Sum += zQuantity * Scores_Counter1;
                }
                if (zRow["AtCounter"].ToString() == "2")
                {
                    zScore_Sum += zQuantity * Scores_Counter2;
                }
                zQuantity_Sum += zQuantity;
            }
            zSQL = "UPDATE  KPI_Accounting_HauKiemBT_None SET RepeatCalculator = RepeatCalculator + 1 " + zWhere;
            ExecuteNonQuery(zSQL);

            return new int[2] { zQuantity_Sum, zScore_Sum };
        }
        public JsonResult View_PhuLuc()
        {
            TableView zTable = new TableView();

            zTable.FieldsName.Add("Module");
            zTable.FieldsName.Add("Mã Code NV");
            zTable.FieldsName.Add("Tên bút toán");
            zTable.FieldsName.Add("Số Lượng");
            zTable.FieldsName.Add("Điểm số");

            int zQuantity;
            int zScores;

            int zQuantity_Total = 0;
            int zScores_Total = 0;
            string[] zItem;
            foreach (DataRow zRow in _PhuLuc.Rows)
            {
                zQuantity = (int)zRow["Quantity"];
                zScores = (int)zRow["Scores"];
                if (zQuantity != 0 || zScores != 0)
                {
                    zItem = new string[6];
                    zItem[0] = zRow["AutoKey"].ToString();
                    zItem[1] = zRow["Module"].ToString();
                    zItem[2] = zRow["MaCodeNV"].ToString();
                    zItem[3] = zRow["TenButtoan"].ToString();
                    zItem[4] = zRow["Quantity"].ToString();
                    zItem[5] = zRow["Scores"].ToString();

                    zTable.DataOfTable.Add(zItem);

                    zQuantity_Total += zQuantity;
                    zScores_Total += zScores;
                }
            }
            zItem = new string[6];
            zItem[0] = "";
            zItem[1] = "";
            zItem[2] = "";
            zItem[3] = "TỔNG CỘNG";
            zItem[4] = zQuantity_Total.ToString("#,##0");
            zItem[5] = zScores_Total.ToString("#,##0");

            zTable.DataOfTable.Add(zItem);
            return new JsonResult(zTable);
        }
        public JsonResult View_PhuLuc_Item(string TableName, string col_stscd, string Module, string MaCodeNV, string Analysis, string NhatDuLieu)
        {
            string zFieldDate = " CAST(CASE WHEN col_crtdt IS NOT NULL THEN col_crtdt ELSE col_opndt END AS date) AS WorkingDate,";
            string zFieldQuantity = " COUNT(*) AS Quantity,";
            string zFieldAtCounter = " [dbo].[KPI_Accounting_WorkingAtLocation](CAST(CASE WHEN col_crtdt IS NOT NULL THEN col_crtdt ELSE col_opndt END AS date),'" + _col_crtusr + "') AS AtCounter ";
            string zGroupBy = " CAST(CASE WHEN col_crtdt IS NOT NULL THEN col_crtdt ELSE col_opndt END AS date) ";
            string zOrderBy = " CAST(CASE WHEN col_crtdt IS NOT NULL THEN col_crtdt ELSE col_opndt END AS date) ";

            string zWhere = " WHERE ";
            if (col_stscd.Trim().Length > 0)
                zWhere += " col_stscd = '" + col_stscd + "' AND ";

            zWhere += " InMonth = " + _Month + " AND InYear = " + _Year + " AND col_crtusr = '" + _col_crtusr + "'" +
                " AND col_buscd = '" + Module + "' AND col_bchkcd = " + MaCodeNV + " " + Analysis;

            string zSQL = "SELECT " + zFieldDate + zFieldQuantity + zFieldAtCounter + " FROM " + TableName + zWhere
                + " GROUP BY " + zGroupBy
                + " ORDER BY " + zOrderBy;
            DataTable zTableItem = GetDataList(zSQL);
            TableView zTable = new TableView();

            zTable.FieldsName.Add("Ngày");
            zTable.FieldsName.Add("Số lượng");
            zTable.FieldsName.Add("Quầy");
            DateTime zDate;
            string[] zItem = new string[3];
            zItem[0] = Module;
            zItem[1] = MaCodeNV;
            zItem[2] = NhatDuLieu;

            zTable.DataOfTable.Add(zItem);
            foreach (DataRow zRow in zTableItem.Rows)
            {
                zItem = new string[3];
                zDate = (DateTime)zRow["WorkingDate"];
                zItem[0] = zDate.ToString("dd/MM/yyyy");
                zItem[1] = zRow["Quantity"].ToString();
                zItem[2] = zRow["AtCounter"].ToString();

                zTable.DataOfTable.Add(zItem);
            }
            return new JsonResult(zTable);

        }

        #endregion

        #region [ Tieu Chi ]
        private int GetWorkingAtCounter(int CounterIndex)
        {

            string zSQL = "SELECT Count(*) FROM [dbo].[KPI_Accounting_AtCounter] "
               + " WHERE UserName = '" + _col_crtusr + "' AND InMonth = " + _Month.ToString() + " AND InYear = " + _Year + " AND AtCounter = " + CounterIndex;

            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;

                zResult = zCommand.ExecuteScalar().ToString();
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
            int zAmount = 0;
            int.TryParse(zResult, out zAmount);
            return zAmount;
        }
        public void TieuChi_Get()
        {
            _WorkingAtCounter_1 = GetWorkingAtCounter(1);
            _WorkingAtCounter_2 = GetWorkingAtCounter(2);
           
            TieuChi_TongButToan[4] = TieuChi_SoButToan_ThucHien();
            TieuChi_TongButToan[5] = TieuChi_SoButToan_Huy();
            TieuChi_TongButToan[6] = TieuChi_SoButToan_AgriTax();
            TieuChi_TongButToan[7] = TieuChi_SoButToan_KhongHachToan();
            TieuChi_TongButToan[8] = TieuChi_SoButToan_PHUB();
            TieuChi_TongButToan[9] = TieuChi_SoButToan_HauKiem();

            PhuLuc_Get();
            TieuChi_ButToanXuLy = new int[19];
            int[] zVal = PhuLuc_Calculator_I("Normal");
            TieuChi_ButToanXuLy[4] = zVal[0];
            zVal = PhuLuc_Calculator_I("Cancel");
            TieuChi_ButToanXuLy[5] = zVal[0];
        }
        private int TieuChi_SoButToan_ThucHien()
        {
            string zSQL = "SELECT COUNT(*) FROM [dbo].[KPI_Accounting_HauKiemBT] ";
            zSQL += " WHERE InMonth = " + _Month + " AND InYear = " + _Year + " AND col_crtusr = '" + _col_crtusr + "'";
            zSQL += " AND col_stscd = 'Normal'";
            return ExecuteScalar(zSQL);
        }
        private int TieuChi_SoButToan_Huy()
        {
            string zSQL = "SELECT COUNT(*) FROM [dbo].[KPI_Accounting_HauKiemBT] ";
            zSQL += " WHERE InMonth = " + _Month + " AND InYear = " + _Year + " AND col_crtusr = '" + _col_crtusr + "'";
            zSQL += " AND col_stscd = 'Cancel'";
            return ExecuteScalar(zSQL);
        }
        private int TieuChi_SoButToan_AgriTax()
        {
            string zSQL = "SELECT COUNT(*) FROM [dbo].[KPI_Accounting_Agritax] ";
            zSQL += " WHERE Month(NgayChungTu) = " + _Month + " AND Year(NgayChungTu) = " + _Year + " AND MaNhanVien = '" + _col_crtusr + "'";
            return ExecuteScalar(zSQL);
        }
        private int TieuChi_SoButToan_KhongHachToan()
        {
            string zSQL = "SELECT COUNT(*) FROM [dbo].[KPI_Accounting_HauKiemBT_None] ";
            zSQL += " WHERE InMonth = " + _Month + " AND InYear = " + _Year + " AND col_crtusr = '" + _col_crtusr + "'";
            return ExecuteScalar(zSQL);
        }
        private int TieuChi_SoButToan_PHUB()
        {
            string zSQL = "SELECT Sum(TongSoGD) FROM [dbo].[KPI_Accounting_PHUB] A  ";
            zSQL += "LEFT JOIN [dbo].[HRM_Employee] B ON A.GDV = B.CadresID ";
            zSQL += " WHERE InMonth = " + _Month + " AND InYear = " + _Year + " AND B.UserIPCAST = '" + _col_crtusr + "'";
            return ExecuteScalar(zSQL);
        }
        private int TieuChi_SoButToan_HauKiem()
        {
            string zSQL = "SELECT COUNT(*) FROM [dbo].[KPI_Accounting_HauKiemBT] ";
            zSQL += " WHERE InMonth = " + _Month + " AND InYear = " + _Year + " AND col_bchkusr = '" + _col_crtusr + "'";
            return ExecuteScalar(zSQL);
        }
        //
        #endregion

        private DataTable GetDataList(string SQL)
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
                _Message = "";
            }
            catch (Exception ex)
            {
                _Message = ex.ToString();
            }
            return zTable;

        }
        private int ExecuteScalar(string SQL)
        {
            int zResult = 0;
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                int.TryParse(zCommand.ExecuteScalar().ToString(), out zResult);

                zCommand.Dispose();
                zConnect.Close();
                _Message = "";
            }
            catch (Exception ex)
            {
                _Message = ex.ToString();
            }
            return zResult;

        }

        private string ExecuteNonQuery(string SQL)
        {
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                zResult = zCommand.ExecuteNonQuery().ToString();

                zCommand.Dispose();
                zConnect.Close();
                _Message = "";
            }
            catch (Exception ex)
            {
                _Message = ex.ToString();
            }
            return zResult;

        }

    }
    public class TableView
    {
        public List<string> FieldsName { get; set; }
        public List<string[]> DataOfTable { get; set; }
        public TableView()
        {
            FieldsName = new List<string>();
            DataOfTable = new List<string[]>();
        }

    }
}
