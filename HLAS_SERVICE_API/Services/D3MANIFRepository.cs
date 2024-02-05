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
    public class D3MANIFRepository
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

        public IEnumerable<D3MANIF> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT *  FROM DC2LIB.D3MANFE LIMIT 5000";
                dbConnection.Open();
                var result = dbConnection.Query<D3MANIF>(sQuery);
                dbConnection.Close();
                return result;

            }
        }

        public IEnumerable<D3MANIF> GetD3MANFEbetween(string PICDTE1, string PICDTE2)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT *  FROM DC2LIB.D3MANFE WHERE CAST(PORTET AS DATE) BETWEEN @PICDTE1 AND @PICDTE2";
                dbConnection.Open();
                var result = dbConnection.Query<D3MANIF>(sQuery, new { PICDTE1, PICDTE2 });
                dbConnection.Close();
                return result;
            }
        }


    }
}