using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_KPI.Areas.KPI_Accounting.Models
{
    public class Report
    {
        public static List<string[]> Result(int InMonth, int InYear)
        {
            DataTable zTable = new DataTable();
            string zMessage = "";
            string zSQL = "SELECT * " +
                          "FROM [dbo].[KPI_Accounting_Result] " +
                          "WHERE InMonth = @InMonth AND InYear = @InYear " +
                          "ORDER BY TRCD, EmployeeName";
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
                    zItem = new string[18];
                    zItem[0] = zRow["UserName"].ToString();
                    zItem[1] = zRow["EmployeeName"].ToString();
                    double zSumScore = 0;
                    double zAmount = 0;
                    double zScore = 0;
                    double.TryParse(zRow["ButToanThucHien"].ToString(),out zAmount);
                    double.TryParse(zRow["ButToanThucHien_Diem"].ToString(), out zScore);
                    zItem[2] = zAmount.ToString("#,###,##0") + "<span style='color:gray'> ("+ zScore.ToString("#,##0")+ ")</span>";
                    zSumScore += zScore;

                    double.TryParse(zRow["ButToanHuy"].ToString(), out zAmount);
                    double.TryParse(zRow["ButToanHuy_Diem"].ToString(), out zScore);
                    zItem[3] = zAmount.ToString("#,###,##0") + "<span style='color:gray'> (" + zScore.ToString("#,##0") + ")</span>";
                    zSumScore += zScore;

                    double.TryParse(zRow["ButToanAgritax"].ToString(), out zAmount);
                    double.TryParse(zRow["ButToanAgritax_Diem"].ToString(), out zScore);
                    zItem[4] = zAmount.ToString("#,###,##0") + "<span style='color:gray'> (" + zScore.ToString("#,##0") + ")</span>";
                    zSumScore += zScore;

                    double.TryParse(zRow["ButToanKhongHachToan"].ToString(), out zAmount);
                    double.TryParse(zRow["ButToanKhongHachToan_Diem"].ToString(), out zScore);
                    zItem[5] = zAmount.ToString("#,###,##0") + "<span style='color:gray'> (" + zScore.ToString("#,##0") + ")</span>";
                    zSumScore += zScore;

                    double.TryParse(zRow["ButToanPHUB"].ToString(), out zAmount);
                    double.TryParse(zRow["ButToanPHUB_Diem"].ToString(), out zScore);
                    zItem[6] = zAmount.ToString("#,###,##0") + "<span style='color:gray'> (" + zScore.ToString("#,##0") + ")</span>";
                    zSumScore += zScore;
                    //
                    double.TryParse(zRow["HauKiemButToan"].ToString(), out zAmount);
                    double.TryParse(zRow["HauKiemButToan_Diem"].ToString(), out zScore);
                    zItem[7] = zAmount.ToString("#,###,##0") + "<span style='color:gray'> (" + zScore.ToString("#,##0") + ")</span>";
                    zSumScore += zScore;

                    double.TryParse(zRow["TongDiemQuyDoi"].ToString(), out zAmount);
                    zItem[8] = zAmount.ToString("#,###,##0") + "<span style='color:gray'> (" + zScore.ToString("#,##0") + ")</span>";
                    zSumScore += zScore;

                    double.TryParse(zRow["DiemCongPhuTrach"].ToString(), out zAmount);
                    zItem[9] = zAmount.ToString("#,###,##0") + "<span style='color:gray'> (" + zScore.ToString("#,##0") + ")</span>";
                    zSumScore += zScore;

                    double.TryParse(zRow["HeSoCongViec"].ToString(), out zAmount);
                    zItem[10] = zAmount.ToString("#,###,##0") + "<span style='color:gray'> (" + zScore.ToString("#,##0") + ")</span>";
                    zSumScore += zScore;

                    double.TryParse(zRow["GDV_KS_ToTruong_PhuTrach"].ToString(), out zAmount);
                    zItem[11] = zAmount.ToString("#,###,##0") + "<span style='color:gray'> (" + zScore.ToString("#,##0") + ")</span>";
                    zSumScore += zScore;

                    //DiemThucHien
                    double zAmount_DiemThucHien;
                    double TongDiemQuyDoi = 0;
                    if (double.TryParse(zRow["TongDiemQuyDoi"].ToString(), out TongDiemQuyDoi)) { } 
                    double DiemCongPhuTrach = 0;
                    if (double.TryParse(zRow["DiemCongPhuTrach"].ToString(), out DiemCongPhuTrach)) { } 
                    double GDV_KS_ToTruong_PhuTrach = 0;
                    if (double.TryParse(zRow["GDV_KS_ToTruong_PhuTrach"].ToString(), out GDV_KS_ToTruong_PhuTrach)) { }
                    zAmount_DiemThucHien = (TongDiemQuyDoi + DiemCongPhuTrach + GDV_KS_ToTruong_PhuTrach) * 12;

                    zItem[12] = zAmount_DiemThucHien.ToString("#,###,##0") + "<span style='color:gray'> (" + zScore.ToString("#,##0") + ")</span>";
                    zSumScore += zScore;

                    //DiemBTBQ
                    double zAmount_DiemBTBQ;
                    double TRCD = 0;
                    if (double.TryParse(zRow["TRCD"].ToString(), out TRCD)) { }
                    zAmount_DiemBTBQ = (zAmount_DiemThucHien / TRCD);
                    //double.TryParse(zRow["DiemBTBQ"].ToString(), out zAmount);
                    zItem[13] = zAmount_DiemBTBQ.ToString("#,###,##0") + "<span style='color:gray'> (" + zScore.ToString("#,##0") + ")</span>";
                    zSumScore += zScore;

                    //DiemBTBQ_Rate
                    double zAmount_DiemBTBQ_Rate = (zAmount_DiemThucHien - zAmount_DiemBTBQ );
                    //double.TryParse(zRow["DiemBTBQ_Rate"].ToString(), out zAmount);
                    zItem[14] = zAmount.ToString("#,###,##0") + "<span style='color:gray'> (" + zScore.ToString("#,##0") + ")</span>";
                    zSumScore += zScore;


                    //Rate_Target
                    double zAmount_RateTarget = (zAmount_DiemThucHien / zAmount_DiemBTBQ);
                    //double.TryParse(zRow["Rate_Target"].ToString(), out zAmount);
                    zItem[15] = zAmount_RateTarget.ToString("#,###,##0") + "<span style='color:gray'> (" + zScore.ToString("#,##0") + ")</span>";
                    zSumScore += zScore;

                    double.TryParse(zRow["Note"].ToString(), out zAmount);
                    zItem[16] = zAmount.ToString("#,###,##0") + "<span style='color:gray'> (" + zScore.ToString("#,##0") + ")</span>";
                    zSumScore += zScore;

                    zItem[17] = zSumScore.ToString("#,###,##0");
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
    }
}
