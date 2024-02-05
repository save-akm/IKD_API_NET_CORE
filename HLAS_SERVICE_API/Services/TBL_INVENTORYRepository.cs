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
    public class TBL_INVENTORYRepository
    {
        private string AppConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ConnectionSQL"];
        public IDbConnection Connection
        {
            get
            {
                //return new iDB2Connection(connectionstring);
                return new SqlConnection(AppConnection);
            }
        }

        public IEnumerable<TBL_TROLLEY> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery =
                @"SELECT SUBSTRING(QRCODE,70,15) AS DCPART,SUBSTRING(QRCODE,120,11) AS COLOR,
                CASE
                WHEN SUBSTRING(QRCODE,179,3) = '' THEN '000'+SUBSTRING(QRCODE,214,3)
                WHEN SUBSTRING(QRCODE,179,3) = '000' THEN '000'+SUBSTRING(QRCODE,214,3)
                ELSE SUBSTRING(QRCODE,179,3)+SUBSTRING(QRCODE,214,3)
                END AS BOXID,
                SUBSTRING(UPPER(QRCODE),1,12) AS PCNO,WMS_LOCATION,DATETIME,
                SUBSTRING(QRCODE,131,4)+SUBSTRING(QRCODE,229,1)+SUBSTRING(QRCODE,230,3)+SUBSTRING(QRCODE,135,4)+' '++SUBSTRING(QRCODE,153,5) AS CASENO,
                SUBSTRING(UPPER(QRCODE),172,7) AS KQTY,SUBSTRING(UPPER(QRCODE),158,7) AS PQTY,SUBSTRING(UPPER(QRCODE),165,7) AS BQTY
                FROM TBL_INVENTORY";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_TROLLEY>(sQuery);
                dbConnection.Close();
                return result;

            }
        }
  
    }
}