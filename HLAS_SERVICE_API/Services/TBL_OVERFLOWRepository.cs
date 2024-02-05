using System;
using Dapper;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using IBM.Data.DB2.iSeries;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using HLAS_SERVICE_API.Models;
namespace HLAS_SERVICE_API.Services
{
    public class TBL_OVERFLOWRepository
    {
        private string AppConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ConnectionSQL"];

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppConnection);
            }
        }
        public IEnumerable<TBL_OVERFLOW> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
               
                string sQuery = @"SELECT TOP(1000) * FROM TBL_OVERFLOW ORDER BY NO DESC";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_OVERFLOW>(sQuery);
                //if (result == null) { return (); }
                dbConnection.Close();
                return result;
            }
        }
        public IEnumerable<TBL_OVERFLOW> GetInnvetory()
        {
            using (IDbConnection dbConnection = Connection)
            {

                string sQuery = @"
                SELECT  PARTNO,COLOR,WMS_LOCATION,OVF_LOCATION,BOXIN AS QTYIN,BOXOUT AS QTYOUT,BOXIN-BOXOUT AS REMAIN FROM (
                SELECT PARTNO,COLOR, MAX(WMS_LOCATION) AS WMS_LOCATION,OVF_LOCATION, MAX(BOXIN) AS BOXIN, MAX(BOXOUT) AS BOXOUT FROM (
                SELECT PARTNO,COLOR, MAX(WMS_LOCATION) AS WMS_LOCATION,OVF_LOCATION, SUM(BOX) AS BOXIN,'0' AS BOXOUT, MAX(INOUT) AS INOUT  FROM TBL_OVERFLOW WHERE INOUT = 'IN' GROUP BY PARTNO, COLOR,OVF_LOCATION
                UNION ALL
                SELECT PARTNO,COLOR, MAX(WMS_LOCATION) AS WMS_LOCATION,OVF_LOCATION, '0' AS BOXIN,SUM(BOX) AS BOXOUT, MAX(INOUT) AS INOUT  FROM TBL_OVERFLOW WHERE INOUT = 'OUT' GROUP BY PARTNO, COLOR,OVF_LOCATION
                ) T1 GROUP BY PARTNO,COLOR,OVF_LOCATION 
                ) T2 ORDER BY PARTNO,COLOR DESC";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_OVERFLOW>(sQuery);
                //if (result == null) { return (); }
                dbConnection.Close();
                return result;
            }
        }
        public IEnumerable<TBL_OVERFLOW> GetAllIN()
        {
            using (IDbConnection dbConnection = Connection)
            {

                string sQuery = @"SELECT TOP(20)* FROM TBL_OVERFLOW WHERE INOUT = 'IN' ORDER BY NO DESC";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_OVERFLOW>(sQuery);
                dbConnection.Close();
                return result;
            }
        }
        public IEnumerable<TBL_OVERFLOW> GetAllOUT()
        {
            using (IDbConnection dbConnection = Connection)
            {

                string sQuery = @"SELECT TOP(20)* FROM TBL_OVERFLOW WHERE INOUT = 'OUT' ORDER BY NO DESC";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_OVERFLOW>(sQuery);
                dbConnection.Close();
                return result;
            }
        }
        public IEnumerable<TBL_OVERFLOW> GetByIDOverFlow(string PICDTE1, string PICDTE2)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //DLVDTE BETWEEN @DLVDTE1 AND @DLVDTE2
                //string sQuery =@"SELECT * FROM TBL_OVERFLOW WHERE CAST(DATEs AS DATE) = @PICDTE1";
                string sQuery = @"SELECT * FROM TBL_OVERFLOW WHERE CAST(DATEs AS DATE) BETWEEN @PICDTE1 AND @PICDTE2 ORDER BY NO DESC";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_OVERFLOW>(sQuery, new { PICDTE1, PICDTE2 });
                dbConnection.Close();
                return result;
            }
        }
        public IEnumerable<TBL_OVERFLOW> GetByIDOverFlowIN(string PICDTE1, string PICDTE2)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM TBL_OVERFLOW WHERE CAST(DATEs AS DATE) BETWEEN @PICDTE1 AND @PICDTE2 AND INOUT = 'IN' ORDER BY NO DESC";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_OVERFLOW>(sQuery, new { PICDTE1, PICDTE2 });
                dbConnection.Close();
                return result;
            }
        }
        public IEnumerable<TBL_OVERFLOW> GetByIDOverFlowOUT(string PICDTE1, string PICDTE2)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM TBL_OVERFLOW WHERE CAST(DATEs AS DATE) BETWEEN @PICDTE1 AND @PICDTE2 AND INOUT = 'OUT' ORDER BY NO DESC";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_OVERFLOW>(sQuery, new { PICDTE1, PICDTE2 });
                dbConnection.Close();
                return result;
            }
        }
    }
}