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
    public class TBL_CASE_RETURNRepository
    {
        private string AppConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ConnectionSQL"];
        private string AppConnectionDB2 = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ConnectionAS400AYT"];


        public IDbConnection Connection
        {
            get
            {
                //return new iDB2Connection(connectionstring);
                return new SqlConnection(AppConnection);
                //return new MySqlConnection(AppConnection);
            }
        }
        public IDbConnection Connection1
        {
            get
            {
                return new iDB2Connection(AppConnectionDB2);
                //return new SqlConnection(AppConnection);
                //return new MySqlConnection(AppConnection);
            }
        }


        public IEnumerable<TBL_CASE_RETURN> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                DataTable dt = new DataTable();
                string sQuery = @"SELECT * FROM TBL_CASE_RETURN ORDER BY DATETIME1 DESC";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_CASE_RETURN>(sQuery);
                dbConnection.Close();             
                return result;

            }
        }

        public IEnumerable<TBL_CASE_RETURN> GetAllCaseRevice()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM TBL_CASE_RETURN WHERE CAST(DATETIME1 AS DATE) = FORMAT (getdate(), 'yyyy-MM-dd')  AND INOUT IN ('IN','OUT') AND TYPE IN ('NORMAL CASE','SPECAIL CASE') ORDER BY DATETIME1 DESC";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_CASE_RETURN>(sQuery);
                dbConnection.Close();
                return result;

            }
        }
        public IEnumerable<TBL_CASE_RETURN> GetAllCaseALLBydate(string PICDTE1, string PICDTE2)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM TBL_CASE_RETURN WHERE CAST(DATETIME1 AS DATE) BETWEEN @PICDTE1 AND @PICDTE2 AND INOUT IN ('IN','OUT') ORDER BY NO DESC";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_CASE_RETURN>(sQuery, new { PICDTE1, PICDTE2 });
                dbConnection.Close();
                return result;
            }
        }
        public IEnumerable<TBL_CASE_RETURN> GetAllCaseINBydate(string PICDTE1, string PICDTE2)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM TBL_CASE_RETURN WHERE CAST(DATETIME1 AS DATE) BETWEEN @PICDTE1 AND @PICDTE2 AND INOUT = 'IN' ORDER BY NO DESC";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_CASE_RETURN>(sQuery, new { PICDTE1, PICDTE2 });
                dbConnection.Close();
                return result;
            }
        }
        public IEnumerable<TBL_CASE_RETURN> GetAllCaseOUTBydate(string PICDTE1, string PICDTE2)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM TBL_CASE_RETURN WHERE CAST(DATETIME1 AS DATE) BETWEEN @PICDTE1 AND @PICDTE2 AND INOUT = 'OUT' ORDER BY NO DESC";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_CASE_RETURN>(sQuery, new { PICDTE1, PICDTE2 });
                dbConnection.Close();
                return result;
            }
        }

        //public IEnumerable<TBL_CASE_RETURN> Gettest()
        //{
        //    using (IDbConnection dbConnection = Connection)
        //    {
              
        //        string sQuery = @"SELECT * FROM TBL_CASE_RETURN ORDER BY DATETIME1 DESC";
        //        dbConnection.Open();
        //        var result = dbConnection.Query<TBL_CASE_RETURN>(sQuery);
        //        dbConnection.Close();
        //        return result;
        //    }
        //    using (IDbConnection dbConnection1 = Connection1)
        //    {

        //        string sQuery1 = @"SELECT * FROM TBL_CASE_RETURN ORDER BY DATETIME1 DESC";
        //        dbConnection1.Open();
        //        var result1 = dbConnection1.Query<TBL_CASE_RETURN>(sQuery1);
        //        dbConnection1.Close();
        //        return result1;
        //    }


        //    //return result;
        //}
    }
}