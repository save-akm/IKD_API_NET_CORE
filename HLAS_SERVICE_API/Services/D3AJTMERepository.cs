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
    public class D3AJTMERepository
    {
        private string AppConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ConnectionAS400"];

        public IDbConnection Connection
        {
            get
            {
                //return new iDB2Connection(connectionstring);
                return new iDB2Connection(AppConnection);
                //return new SqlConnection()
                 // return new MySqlcon  
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
        public void Update(string D3NUMBE, string D3TYPET)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"UPDATE DC2LIB.D3ZNEMB SET DLVZNE = 'BALL1' WHERE DLVZNE='BALL2'";
                string sQuery = @"UPDATE DC2LIB.D3AJTME SET D3NUMBE = @D3NUMBE, D3TYPET = @D3TYPET WHERE D3NUMID='1'";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { D3NUMBE, D3TYPET});
                dbConnection.Close();
            }

        }
        public void Updatebyzone(string D3NUMBE, string D3TYPET, string D3ZONEM)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"UPDATE DC2LIB.D3ZNEMB SET DLVZNE = 'BALL1' WHERE DLVZNE='BALL2'";
                string sQuery = @"UPDATE DC2LIB.D3AJTME SET D3NUMBE = @D3NUMBE, D3TYPET = @D3TYPET WHERE D3ZONEM=@D3ZONEM";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { D3NUMBE, D3TYPET, D3ZONEM });
                dbConnection.Close();
            }

        }
    }

}