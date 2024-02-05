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
    public class DATUSERRepository
    {
        private string AppConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ConnectionAS400"];
        public IDbConnection Connection
        {
            get
            {
                //return new iDB2Connection(connectionstring);
                return new iDB2Connection(AppConnection);
            }
        }

        public IEnumerable<DATUSER> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM DC2LIB.DATUSER";

                dbConnection.Open();
                var result = dbConnection.Query<DATUSER>(sQuery);
                //if (result == null) { return (); }
                dbConnection.Close();
                return result;

            }
        }
        //public IEnumerable<DATUSER> GetByID(string USUSCD, string USPASS)
        public DATUSER GetByID(string USUSCD, string USPASS)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"SELECT COUNT(*) AS USUSCD FROM DC2LIB.DATUSER WHERE USUSCD=@USUSCD AND USPASS=@USPASS AND USSCCD = '01'";

                string sQuery = @"SELECT * FROM DC2LIB.DATUSER WHERE USUSCD=@USUSCD AND USPASS=@USPASS AND USSCCD = '01'";
                dbConnection.Open();
                var result = dbConnection.Query<DATUSER>(sQuery, new { USUSCD = USUSCD, USPASS = USPASS }).FirstOrDefault();
                //var result = dbConnection.Query<D3ZNEMB>(sQuery).ToList();
                dbConnection.Close();
                return result;
            }
        }
    }
}