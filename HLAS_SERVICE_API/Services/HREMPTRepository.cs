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
    public class HREMPTRepository
    {
        private string AppConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ConnectionSQLHRDC"];
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppConnection);
            }
        }

        public IEnumerable<HREMPT> GetHRByDate(string PICDTE1, string PICDTE2)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"
                SELECT
                RTRIM(LTRIM(TB_USER_CUSTOMINFO.sFieldValue7)) AS 'EmployeeID',
                RTRIM(LTRIM(TB_USER.sUserName)) AS 'NameEng',
                RTRIM(LTRIM(TB_USER.sUserID)) AS 'UserID',
                RTRIM(LTRIM(TB_USER_CUSTOMINFO.sFieldValue5)) AS 'Employee',
                CONVERT(VARCHAR(10), dateadd(s, ndatetime, '1970-01-01'), 103) + ' ' +
                SUBSTRING(CONVERT(VARCHAR(9), dateadd(s, ndatetime, '1970-01-01'), 108), 0, 9) + ' ' +
                SUBSTRING(CONVERT(VARCHAR(20), dateadd(s, ndatetime, '1970-01-01'), 100), 18, 30) AS 'DateTime', 
                nEventIdn AS 'EventID',
                CONVERT(varchar(10), nReaderIdn) AS 'DeviceID'
                FROM TB_EVENT_LOG
                INNER JOIN TB_USER ON TB_EVENT_LOG.nUserID = TB_USER.sUserID
                INNER JOIN TB_USER_CUSTOMINFO ON TB_USER.nUserIdn = TB_USER_CUSTOMINFO.nUserIdn
                WHERE dateadd(s, ndatetime,'1970-01-01')BETWEEN  @PICDTE1 + ' 00:00:00' AND  @PICDTE2 + ' 23:59:59'
                AND neventidn IN('55', '43')
                --AND nReaderIdn IN('538626019')
                ORDER BY dateadd(s, ndatetime, '1970-01-01') DESC--DESC--ASC               
                ";
                dbConnection.Open();
                var result = dbConnection.Query<HREMPT>(sQuery, new { PICDTE1, PICDTE2 });
                dbConnection.Close();
                return result;
            }
        }
    }
}