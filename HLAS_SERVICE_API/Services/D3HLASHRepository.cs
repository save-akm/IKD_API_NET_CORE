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
    public class D3HLASHRepository
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
        public object JsonXmlHelper { get; private set; }

        public IEnumerable<D3HLASH> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"
                SELECT * FROM DC2LIB.D3HLASH
                WHERE I8TRDT = INTEGER(DATE(CURRENT TIMESTAMP))  ORDER BY I8TRDT, I8TRTM DESC";
                dbConnection.Open();
                var result = dbConnection.Query<D3HLASH>(sQuery);
                dbConnection.Close();
                return result;

            }
        }
        public IEnumerable<D3HLASH> GetAllUnpackok()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"
                SELECT * FROM DC2LIB.D3HLASH
                WHERE I8UNPC = '1'  AND I8TRDT = INTEGER(DATE(CURRENT TIMESTAMP))  ORDER BY I8TRDT, I8TRTM DESC";
                dbConnection.Open();
                var result = dbConnection.Query<D3HLASH>(sQuery);
                dbConnection.Close();
                return result;

            }
        }
        public IEnumerable<D3HLASH> GetCaseStockByDate(string PICDTE1, string PICDTE2)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"
                SELECT I8PCNO, I8CCTL ,I8MDYE ,I8MODL ,I8MDFF ,I8CCNO ,I8SADD ,I8DATE ,I8TIME ,I8UNPC ,I8UNPD ,I8UNPT,CASE_NO,I8WMSL FROM (
                SELECT I8PCNO, I8CCTL ,I8MDYE ,I8MODL ,I8MDFF ,I8CCNO ,I8SADD ,I8DATE ,I8TIME ,I8UNPC ,I8UNPD ,I8UNPT,CASE_NO,
                SUBSTRING(CAST(I8DATE1 AS VARCHAR(19)), 1, 10) AS I8DATE1,I8WMSL 
                FROM (
                SELECT I8PCNO, I8CCTL ,I8MDYE ,I8MODL ,I8MDFF ,I8CCNO ,I8SADD ,I8DATE ,I8TIME ,I8UNPC ,I8UNPD ,I8UNPT,
                I8CCTL || I8MDYE || I8MODL || I8MDFF || '' || I8CCNO AS CASE_NO,I8WMSL
                ,TIMESTAMP_FORMAT(CONCAT(CHAR(I8DATE), CHAR(LPAD(I8DATE, 6, 0))), 'YYYYMMDD HH24MISS') AS I8DATE1
                FROM DC2LIB.D3HLASH WHERE I8UNPC = ''
                ) T1
                ) T2 WHERE I8DATE1 BETWEEN @PICDTE1 AND @PICDTE2 ORDER BY I8DATE1, I8TIME DESC";
                dbConnection.Open();
                var result = dbConnection.Query<D3HLASH>(sQuery, new { PICDTE1, PICDTE2 });
                dbConnection.Close();
                return result;
            }
        }

    }
}