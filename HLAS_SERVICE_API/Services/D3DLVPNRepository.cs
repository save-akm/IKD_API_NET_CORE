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
    public class D3DLVPNRepository
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
        

        public IEnumerable<D3DLVPN> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"
                SELECT *
                FROM DC2LIB.D3DLVPN 
                WHERE D1DLD2 = INTEGER(DATE(CURRENT TIMESTAMP)) 
                OR D1DLD2 = INTEGER(DATE(CURRENT TIMESTAMP)) 
                OR D1DLD9 = INTEGER(DATE(CURRENT TIMESTAMP))
                OR D1DLDJ = INTEGER(DATE(CURRENT TIMESTAMP))";
                dbConnection.Open();
                var result = dbConnection.Query<D3DLVPN>(sQuery);
                dbConnection.Close();
                return result;

            }
        }

        public IEnumerable<D3DLVPN> GetAllData()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"
				SELECT DISTINCT D1KDLT,D1MDNM,D1MDCD,D1TYPE,D1DLD2,D1DLT2,D1DLD1,D1DLT1,D1DLD9,D1DLT9,D1DLDJ,COUNT(*) AS D1PQTY
                FROM 
				DC2LIB.D3DLVPN WHERE D1STPL = 'IKD1'			
				GROUP BY D1KDLT,D1MDNM,D1MDCD,D1TYPE,D1DLD2,D1DLT2,D1DLD1,D1DLT1,D1DLD9,D1DLT9,D1DLDJ";
                dbConnection.Open();
                var result = dbConnection.Query<D3DLVPN>(sQuery);
                dbConnection.Close();
                return result;

            }
        }



    }
}