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
    public class TBL_TROLLEYRepository
    {
        private string AppConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ConnectionSQL"];

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppConnection);
            }
        }

        public IEnumerable<TBL_TROLLEY> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM (
                SELECT T1.TROLLEYID,T1.DCPART,T1.COLOR,T1.BOXID,T1.REMAINQTY,T1.BOXQTY,T1.PCNO,WMS_LOCATION,SUPPLIER,WAREHOUSECODE,CASETYPE,TYPE,Cycle,DATETIME,CASENO
		        FROM (
                SELECT TROLLEYID,SUBSTRING(QRCODE,70,15) AS DCPART,SUBSTRING(QRCODE,120,11) AS COLOR,
                CASE
                WHEN SUBSTRING(QRCODE,179,3) = '' THEN '000'+SUBSTRING(QRCODE,214,3)
                WHEN SUBSTRING(QRCODE,179,3) = '000' THEN '000'+SUBSTRING(QRCODE,214,3)
                ELSE SUBSTRING(QRCODE,179,3)+SUBSTRING(QRCODE,214,3)
                END AS BOXID,
                SUBSTRING(UPPER(QRCODE),1,12) AS PCNO,REMAINQTY AS REMAINQTY,BOXQTY,WMS_LOCATION,DATETIME,TYPE,Cycle,
															SUBSTRING(QRCODE,131,4)+SUBSTRING(QRCODE,229,1)+SUBSTRING(QRCODE,230,3)+SUBSTRING(QRCODE,135,4)+' '++SUBSTRING(QRCODE,153,5) AS CASENO
                FROM TBL_TROLLEY ) T1
                LEFT JOIN TBM_OVERFLOW T2
                ON T1.DCPART = T2.DCPART
                ) T3
                WHERE CAST(DATETIME AS DATE) = FORMAT (getdate(), 'yyyy-MM-dd')";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_TROLLEY>(sQuery);
                dbConnection.Close();
                return result;

            }
        }


        public IEnumerable<TBL_TROLLEY> GetReturnbyTypeandDate(string TROLLEY,string PICDTE1, string PICDTE2)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"SELECT * FROM TBL_CASE_RETURN WHERE CAST(DATETIME1 AS DATE) BETWEEN @PICDTE1 AND @PICDTE2 AND INOUT IN ('IN','OUT') ORDER BY NO DESC";
                string sQuery = @"SELECT * FROM (
                SELECT T1.QRCODE,
				SUBSTRING(T1.QRCODE,131,4)+SUBSTRING(T1.QRCODE,229,1)+SUBSTRING(T1.QRCODE,230,3)+SUBSTRING(T1.QRCODE,135,4)+' '++SUBSTRING(T1.QRCODE,153,5) AS cASENO,
				T1.TROLLEYID,T1.DCPART,T1.COLOR,T1.BOXID,T1.REMAINQTY,T1.BOXQTY,T1.PCNO,WMS_LOCATION,SUPPLIER,WAREHOUSECODE,CASETYPE,TYPE,Cycle,DATETIME 
				FROM (
                SELECT QRCODE,TROLLEYID,SUBSTRING(QRCODE,70,15) AS DCPART,SUBSTRING(QRCODE,120,11) AS COLOR,
                CASE
                WHEN SUBSTRING(QRCODE,179,3) = '' THEN '000'+SUBSTRING(QRCODE,214,3)
                WHEN SUBSTRING(QRCODE,179,3) = '000' THEN '000'+SUBSTRING(QRCODE,214,3)
                ELSE SUBSTRING(QRCODE,179,3)+SUBSTRING(QRCODE,214,3)
                END AS BOXID,
                SUBSTRING(UPPER(QRCODE),1,12) AS PCNO,REMAINQTY AS REMAINQTY,BOXQTY,WMS_LOCATION,DATETIME,TYPE,Cycle
                FROM TBL_TROLLEY ) T1
                LEFT JOIN TBM_OVERFLOW T2
                ON T1.DCPART = T2.DCPART
                ) T3
                WHERE TROLLEYID = @TROLLEY AND CAST(DATETIME AS DATE) BETWEEN @PICDTE1 AND @PICDTE2
                ORDER BY DATETIME DESC";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_TROLLEY>(sQuery, new { TROLLEY, PICDTE1, PICDTE2 });
                dbConnection.Close();
                return result;
            }
        }
        public IEnumerable<TBL_TROLLEY> GetReturnbyTypeandDateCycle(string TROLLEY, string PICDTE1, string PICDTE2, int Cycle)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"SELECT * FROM TBL_CASE_RETURN WHERE CAST(DATETIME1 AS DATE) BETWEEN @PICDTE1 AND @PICDTE2 AND INOUT IN ('IN','OUT') ORDER BY NO DESC";
                string sQuery = @"SELECT * FROM (
                SELECT T1.QRCODE,
				SUBSTRING(T1.QRCODE,131,4)+SUBSTRING(T1.QRCODE,229,1)+SUBSTRING(T1.QRCODE,230,3)+SUBSTRING(T1.QRCODE,135,4)+' '++SUBSTRING(T1.QRCODE,153,5) AS cASENO,
				T1.TROLLEYID,T1.DCPART,T1.COLOR,T1.BOXID,T1.REMAINQTY,T1.BOXQTY,T1.PCNO,WMS_LOCATION,SUPPLIER,WAREHOUSECODE,CASETYPE,TYPE,Cycle,DATETIME 
				FROM (
                SELECT QRCODE,TROLLEYID,SUBSTRING(QRCODE,70,15) AS DCPART,SUBSTRING(QRCODE,120,11) AS COLOR,
                CASE
                WHEN SUBSTRING(QRCODE,179,3) = '' THEN '000'+SUBSTRING(QRCODE,214,3)
                WHEN SUBSTRING(QRCODE,179,3) = '000' THEN '000'+SUBSTRING(QRCODE,214,3)
                ELSE SUBSTRING(QRCODE,179,3)+SUBSTRING(QRCODE,214,3)
                END AS BOXID,
                SUBSTRING(UPPER(QRCODE),1,12) AS PCNO,REMAINQTY AS REMAINQTY,BOXQTY,WMS_LOCATION,DATETIME,TYPE,Cycle
                FROM TBL_TROLLEY ) T1
                LEFT JOIN TBM_OVERFLOW T2
                ON T1.DCPART = T2.DCPART
                ) T3
                WHERE TROLLEYID = @TROLLEY AND CAST(DATETIME AS DATE) BETWEEN @PICDTE1 AND @PICDTE2 AND Cycle = @Cycle
                ORDER BY DATETIME DESC";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_TROLLEY>(sQuery, new { TROLLEY, PICDTE1, PICDTE2, Cycle });
                dbConnection.Close();
                return result;
            }
        }
        public IEnumerable<TBL_TROLLEY> GetTrolleyMaster()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT TROLLEYID FROM TBM_TROLLEY";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_TROLLEY>(sQuery);
                dbConnection.Close();
                return result;
            }
        }
        public IEnumerable<TBL_TROLLEY> GetTrolleyCycle(string TROLLEY, string PICDTE1, string PICDTE2)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"
                SELECT Cycle FROM TBL_TROLLEY
                WHERE TROLLEYID = @TROLLEY AND CAST(DATETIME AS DATE) BETWEEN @PICDTE1 AND @PICDTE2
                GROUP BY TROLLEYID,Cycle";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_TROLLEY>(sQuery, new { TROLLEY, PICDTE1, PICDTE2 });
                dbConnection.Close();
                return result;
            }
        }

    }
}