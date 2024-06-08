using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TNS_KPI.Areas.KPI_Service.Models
{
    public class SMS_Model
    {
        #region [ Field Name ]
        private string _AutoKey = "";
        private string _Crtusr = "";
        private DateTime? _Entydt = null;
        private string _Pgd = "";
        private string _Stts = "";
        private string _Message = "";
        #endregion
        #region [ Properties ]
        public string AutoKey { get => _AutoKey; set => _AutoKey = value; }
        public string Stts { get => _Stts; set => _Stts = value; }
        public string Crtusr { get => _Crtusr; set => _Crtusr = value; }
        public DateTime? Entydt { get => _Entydt; set => _Entydt = value; }
        public string Pgd { get => _Pgd; set => _Pgd = value; }
        public string Message { get => _Message; set => _Message = value; }

        #endregion
        #region [ Constructor Get Information ]
        public SMS_Model()
        {
           
        }
        public SMS_Model(string tam)
        {

        }

        public string Save()
        {
            string zSQL = "INSERT into [dbo].[KPI_Service_IssueSMS] Values (newid(),MONTH(GETDATE()), YEAR(GETDATE()),'Active',CONVERT(DATE, GETDATE(), 120),@crtusr,@pgd)";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@crtusr", SqlDbType.NVarChar).Value = Crtusr;
                zCommand.Parameters.Add("@pgd", SqlDbType.NVarChar).Value = Pgd;
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
        #endregion

    }
}
