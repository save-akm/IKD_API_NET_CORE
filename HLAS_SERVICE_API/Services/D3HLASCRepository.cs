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
    public class D3HLASCRepository
    {
        private string AppConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ConnectionAS400AYT"];

        public IDbConnection Connection
        {
            get
            {
                //return new iDB2Connection(connectionstring);
                return new iDB2Connection(AppConnection);
            }
        }

        public IEnumerable<D3HLASC> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"
                SELECT I1PCNO, I1CCTL ,I1MDYE ,I1MODL ,I1MDFF ,I1CCNO ,I1SADD ,I1DATE ,I1TIME ,I1UNPC ,I1UNPD ,I1UNPT,
                I1CCTL || I1MDYE || I1MODL || I1MDFF || '' || I1CCNO AS CASE_NO,I1WMSL
                FROM DC2LIB.D3HLASC WHERE I1UNPC = ''  AND I1DATE = INTEGER(DATE(CURRENT TIMESTAMP))  ORDER BY I1DATE, I1TIME DESC";
                dbConnection.Open();
                var result = dbConnection.Query<D3HLASC>(sQuery);
                dbConnection.Close();
                return result;

            }
        }
        public IEnumerable<D3HLASC> GetAllUnpackok()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"
                SELECT I1PCNO, I1CCTL ,I1MDYE ,I1MODL ,I1MDFF ,I1CCNO ,I1SADD ,I1DATE ,I1TIME ,I1UNPC ,I1UNPD ,I1UNPT,
                I1CCTL || I1MDYE || I1MODL || I1MDFF || '' || I1CCNO AS CASE_NO,I1WMSL
                FROM DC2LIB.D3HLASC WHERE I1UNPC = '1'  AND I1DATE = INTEGER(DATE(CURRENT TIMESTAMP))  ORDER BY I1DATE, I1TIME DESC";
                dbConnection.Open();
                var result = dbConnection.Query<D3HLASC>(sQuery);
                dbConnection.Close();
                return result;

            }
        }
        public IEnumerable<D3HLASC> GetCaseStockByDate(string PICDTE1, string PICDTE2)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"
                SELECT I1PCNO, I1CCTL ,I1MDYE ,I1MODL ,I1MDFF ,I1CCNO ,I1SADD ,I1DATE ,I1TIME ,I1UNPC ,I1UNPD ,I1UNPT,CASE_NO,I1WMSL FROM (
                SELECT I1PCNO, I1CCTL ,I1MDYE ,I1MODL ,I1MDFF ,I1CCNO ,I1SADD ,I1DATE ,I1TIME ,I1UNPC ,I1UNPD ,I1UNPT,CASE_NO,
                SUBSTRING(CAST(I1DATE1 AS VARCHAR(19)), 1, 10) AS I1DATE1,I1WMSL 
                FROM (
                SELECT I1PCNO, I1CCTL ,I1MDYE ,I1MODL ,I1MDFF ,I1CCNO ,I1SADD ,I1DATE ,I1TIME ,I1UNPC ,I1UNPD ,I1UNPT,
                I1CCTL || I1MDYE || I1MODL || I1MDFF || '' || I1CCNO AS CASE_NO,I1WMSL
                ,TIMESTAMP_FORMAT(CONCAT(CHAR(I1DATE), CHAR(LPAD(I1DATE, 6, 0))), 'YYYYMMDD HH24MISS') AS I1DATE1
                FROM DC2LIB.D3HLASC WHERE I1UNPC = ''
                ) T1
                ) T2 WHERE I1DATE1 BETWEEN @PICDTE1 AND @PICDTE2 ORDER BY I1DATE1, I1TIME DESC";
                dbConnection.Open();
                var result = dbConnection.Query<D3HLASC>(sQuery, new { PICDTE1, PICDTE2 });
                dbConnection.Close();
                return result;
            }
        }

    }
}