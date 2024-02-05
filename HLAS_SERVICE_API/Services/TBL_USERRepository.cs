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
    public class TBL_USERRepository
    {
        private string AppConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ConnectionSQL"];

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppConnection);
            }
        }

        public IEnumerable<TBL_USER> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM TBL_USER";

                dbConnection.Open();
                var result = dbConnection.Query<TBL_USER>(sQuery);
                //if (result == null) { return (); }
                dbConnection.Close();
                return result;

            }
        }
        //public IEnumerable<DATUSER> GetByID(string USUSCD, string USPASS)
        public TBL_USER GetByID(string USERNAME, string PASSWORD)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"SELECT COUNT(*) AS USUSCD FROM DC2LIB.DATUSER WHERE USUSCD=@USUSCD AND USPASS=@USPASS AND USSCCD = '01'";

                string sQuery = @"SELECT * FROM TBL_USER WHERE USERNAME=@USERNAME AND PASSWORD=@PASSWORD AND PERMISSION = 'ADMIN'";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_USER>(sQuery, new { USERNAME = USERNAME, PASSWORD = PASSWORD }).FirstOrDefault();
                //var result = dbConnection.Query<D3ZNEMB>(sQuery).ToList();
                dbConnection.Close();
                return result;
            }
        }
        public void AddUser(TBL_USER tbl_user)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"INSERT INTO DC2LIB.D3ZNEMB (DLVMNT, DLVZNE, MONTIP, PIKFLG, DLVFLG, CRTDTE, CRTTME, CRTUSR, UPDDTE, UPDTME, UPDUSR) VALUES(@DLVMNT,@DLVZNE,@MONTIP,@PIKFLG,@DLVFLG,@CRTDTE,@CRTTME,@CRTUSR,@UPDDTE,@UPDTME,@UPDUSR)";
                //string sQuery = @"INSERT INTO DC2LIB.D3ZNEMB (DLVMNT, DLVZNE, MONTIP, PIKFLG, DLVFLG, CRTDTE, CRTTME, CRTUSR) VALUES(@DLVMNT,@DLVZNE,@MONTIP,@PIKFLG,@DLVFLG,@CRTDTE,@CRTTME,@CRTUSR)";
                string sQuery = @"INSERT INTO TBL_USER (USERNAME, PASSWORD, PERMISSION) VALUES(@USERNAME,@PASSWORD,@PERMISSION)";
                dbConnection.Open();
                //Dapper Mapping                
                //dbConnection.Execute(sQuery, new { DLVMNT = d3znemb.DLVMNT,DLVZNE = d3znemb.DLVZNE, MONTIP=d3znemb.MONTIP,PIKFLG=d3znemb.PIKFLG,DLVFLG=d3znemb.DLVFLG, CRTDTE=d3znemb.CRTDTE, CRTTME=d3znemb.CRTTME,CRTUSR=d3znemb.CRTUSR});
                //dbConnection.Execute(sQuery, new { DLVMNT = d3znemb.DLVMNT, DLVZNE = d3znemb.DLVZNE, MONTIP = d3znemb.MONTIP, PIKFLG = d3znemb.PIKFLG, DLVFLG = d3znemb.DLVFLG, CRTDTE = d3znemb.CRTDTE, CRTTME = d3znemb.CRTTME, CRTUSR = d3znemb.CRTUSR, UPDDTE = d3znemb.UPDDTE, UPDTME = d3znemb.UPDDTE, UPDUSR = d3znemb.UPDUSR });
                //dbConnection.Execute(sQuery, new { DLVMNT = d3znemb.DLVMNT, DLVZNE = d3znemb.DLVZNE, MONTIP = d3znemb.MONTIP, PIKFLG = d3znemb.PIKFLG, DLVFLG = d3znemb.DLVFLG, CRTDTE = d3znemb.CRTDTE, CRTTME = d3znemb.CRTTME, CRTUSR = d3znemb.CRTUSR });
                dbConnection.Execute(sQuery, new { USERNAME = tbl_user.USERNAME, PASSWORD = tbl_user.PASSWORD, PERMISSION = tbl_user.PERMISSION });
                dbConnection.Close();
            }
        }
        public IEnumerable<TBL_USER> GetUserByIDandPass(string USERNAME, string PASSWORD)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"SELECT * FROM TBL_CASE_RETURN WHERE CAST(DATETIME1 AS DATE) BETWEEN @PICDTE1 AND @PICDTE2 AND INOUT IN ('IN','OUT') ORDER BY NO DESC";
                string sQuery = @"SELECT * FROM TBL_USER WHERE USERNAME=@USERNAME AND PASSWORD=@PASSWORD AND PERMISSION = 'ADMIN'";
                dbConnection.Open();
                var result = dbConnection.Query<TBL_USER>(sQuery, new { USERNAME, PASSWORD });
                dbConnection.Close();
                return result;
            }
        }
        public void Delete(string USERNAME, string PASSWORD)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"DELETE FROM TBL_USER WHERE USERNAME=@USERNAME AND PASSWORD = @PASSWORD";//
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { USERNAME = USERNAME, PASSWORD = PASSWORD });
                dbConnection.Close();

            }
        }

        public void Update(string USERNAME, string PASSWORD, string PERMISSION)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"UPDATE TBL_USER SET PASSWORD = @PASSWORD,PERMISSION = @PERMISSION  WHERE USERNAME=@USERNAME ";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { USERNAME, PASSWORD, PERMISSION });
                dbConnection.Close();
            }
        }
    }
}