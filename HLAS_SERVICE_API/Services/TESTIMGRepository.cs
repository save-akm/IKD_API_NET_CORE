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
    public class TESTIMGRepository
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

        public IEnumerable<D3AJTME> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM DC2LIB.D3AJTME";

                dbConnection.Open();
                var result = dbConnection.Query<D3AJTME>(sQuery);
                //if (result == null) { return (); }
                dbConnection.Close();
                return result;

            }
        }



    }

}