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
    public class D3DIWEKRepository
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

        public IEnumerable<D3DIWEK> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM DC2LIB.D3DIWEK";

                dbConnection.Open();
                var result = dbConnection.Query<D3DIWEK>(sQuery);
                //if (result == null) { return (); }
                dbConnection.Close();
                return result;

            }
        }
        public IEnumerable<D3DIWEK> GetByIDD3DIWEK(int PICDTE1, int PICDTE2)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery =
                @"
                SELECT * FROM DC2LIB.D3DIWEK                 
                WHERE
                PICDTE BETWEEN @PICDTE1 AND @PICDTE2
                ORDER BY PICDTE";
                dbConnection.Open();
                var result = dbConnection.Query<D3DIWEK>(sQuery, new { PICDTE1, PICDTE2});
                dbConnection.Close();
                return result;
            }
        }

    }

}