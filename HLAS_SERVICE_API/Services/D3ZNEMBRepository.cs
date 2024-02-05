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
    public class HLASRepository
    {
        //private string connectionstring;
        private string AppConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ConnectionAS400"];
        public IDbConnection Connection
        {
            get
            {
                //return new iDB2Connection(connectionstring);
                return new iDB2Connection(AppConnection);
            }
        }

        public IEnumerable<D3ZNEMB > GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM DC2LIB.D3ZNEMB ORDER BY DLVZNE DESC";

               dbConnection.Open();
                var result = dbConnection.Query<D3ZNEMB>(sQuery);
                //if (result == null) { return (); }
                dbConnection.Close();
                return result;

            }
        }
        public IEnumerable<D3ZNEMB> GetAll1()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT DLVMNT FROM DC2LIB.D3ZNEMB GROUP BY DLVMNT";

                dbConnection.Open();
                var result = dbConnection.Query<D3ZNEMB>(sQuery);
                //if (result == null) { return (); }
                dbConnection.Close();
                return result;

            }
        }
        public void Add(D3ZNEMB d3znemb)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"INSERT INTO DC2LIB.D3ZNEMB (DLVMNT, DLVZNE, MONTIP, PIKFLG, DLVFLG, CRTDTE, CRTTME, CRTUSR, UPDDTE, UPDTME, UPDUSR) VALUES(@DLVMNT,@DLVZNE,@MONTIP,@PIKFLG,@DLVFLG,@CRTDTE,@CRTTME,@CRTUSR,@UPDDTE,@UPDTME,@UPDUSR)";
                //string sQuery = @"INSERT INTO DC2LIB.D3ZNEMB (DLVMNT, DLVZNE, MONTIP, PIKFLG, DLVFLG, CRTDTE, CRTTME, CRTUSR) VALUES(@DLVMNT,@DLVZNE,@MONTIP,@PIKFLG,@DLVFLG,@CRTDTE,@CRTTME,@CRTUSR)";
                string sQuery = @"INSERT INTO DC2LIB.D3ZNEMB (DLVMNT, DLVZNE, PIKFLG, DLVFLG) VALUES(@DLVMNT,@DLVZNE,@PIKFLG,@DLVFLG)";
                dbConnection.Open();
                //Dapper Mapping                
                //dbConnection.Execute(sQuery, new { DLVMNT = d3znemb.DLVMNT,DLVZNE = d3znemb.DLVZNE, MONTIP=d3znemb.MONTIP,PIKFLG=d3znemb.PIKFLG,DLVFLG=d3znemb.DLVFLG, CRTDTE=d3znemb.CRTDTE, CRTTME=d3znemb.CRTTME,CRTUSR=d3znemb.CRTUSR});
                //dbConnection.Execute(sQuery, new { DLVMNT = d3znemb.DLVMNT, DLVZNE = d3znemb.DLVZNE, MONTIP = d3znemb.MONTIP, PIKFLG = d3znemb.PIKFLG, DLVFLG = d3znemb.DLVFLG, CRTDTE = d3znemb.CRTDTE, CRTTME = d3znemb.CRTTME, CRTUSR = d3znemb.CRTUSR, UPDDTE = d3znemb.UPDDTE, UPDTME = d3znemb.UPDDTE, UPDUSR = d3znemb.UPDUSR });
                //dbConnection.Execute(sQuery, new { DLVMNT = d3znemb.DLVMNT, DLVZNE = d3znemb.DLVZNE, MONTIP = d3znemb.MONTIP, PIKFLG = d3znemb.PIKFLG, DLVFLG = d3znemb.DLVFLG, CRTDTE = d3znemb.CRTDTE, CRTTME = d3znemb.CRTTME, CRTUSR = d3znemb.CRTUSR });
                dbConnection.Execute(sQuery, new { DLVMNT = d3znemb.DLVMNT, DLVZNE = d3znemb.DLVZNE, PIKFLG = d3znemb.PIKFLG, DLVFLG = d3znemb.DLVFLG });
                dbConnection.Close();
            }
        }




        public IEnumerable<D3ZNEMB> GetByID(string DLVMNT, string DLVZNE)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT DLVMNT,DLVZNE,MONTIP,PIKFLG,DLVFLG,CRTDTE,CRTTME,CRTUSR,UPDDTE,UPDTME,UPDUSR FROM DC2LIB.D3ZNEMB WHERE DLVMNT=@DLVMNT AND DLVZNE=@DLVZNE";
                dbConnection.Open();
                var result = dbConnection.Query<D3ZNEMB>(sQuery, new { DLVMNT = DLVMNT, DLVZNE= DLVZNE }).ToList();
                //var result = dbConnection.Query<D3ZNEMB>(sQuery).ToList();
                dbConnection.Close();
                return result;
            }
        }
        public void Delete(string DLVZNEs, string DLVMNTs)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"DELETE FROM DC2LIB.D3ZNEMB WHERE DLVZNE=@DLVZNE AND DLVMNT = @DLVMNT";//
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { DLVMNT = DLVMNTs, DLVZNE = DLVZNEs  });
                dbConnection.Close(); 

            }
        }
        public void Update (string Newzne, string Oldzne, string DLVMNTold)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"UPDATE DC2LIB.D3ZNEMB SET DLVZNE = 'BALL1' WHERE DLVZNE='BALL2'";
                string sQuery = @"UPDATE DC2LIB.D3ZNEMB SET DLVZNE = @Newzne WHERE DLVZNE=@Oldzne AND DLVMNT=@DLVMNTold";
                dbConnection.Open();
                dbConnection.Execute(sQuery,new { Newzne, Oldzne, DLVMNTold });

                //dbConnection.Execute(sQuery, new { DLVMNT = d3znemb.DLVMNT, DLVZNE = d3znemb.DLVZNE, MONTIP = d3znemb.MONTIP, PIKFLG = d3znemb.PIKFLG, DLVFLG = d3znemb.DLVFLG, CRTDTE = d3znemb.CRTDTE, CRTTME = d3znemb.CRTTME, CRTUSR = d3znemb.CRTUSR, UPDDTE = d3znemb.UPDDTE, UPDTME = d3znemb.UPDDTE, UPDUSR = d3znemb.UPDUSR });
                dbConnection.Close();


            }
        }
        public void Update1(D3ZNEMB d3znemb)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"UPDATE DC2LIB.D3ZNEMB SET DLVZNE = 'BALL1' WHERE DLVZNE='BALL2'";
                string sQuery = @"UPDATE DC2LIB.D3ZNEMB SET MONTIP = @MONTIP WHERE DLVZNE=@DLVZNE";
                dbConnection.Open();
                dbConnection.Execute(sQuery, d3znemb);
                //dbConnection.Execute(sQuery, new { DLVMNT = d3znemb.DLVMNT, DLVZNE = d3znemb.DLVZNE });
                //dbConnection.Execute(sQuery, new { DLVMNT = d3znemb.DLVMNT, DLVZNE = d3znemb.DLVZNE, MONTIP = d3znemb.MONTIP, PIKFLG = d3znemb.PIKFLG, DLVFLG = d3znemb.DLVFLG, CRTDTE = d3znemb.CRTDTE, CRTTME = d3znemb.CRTTME, CRTUSR = d3znemb.CRTUSR, UPDDTE = d3znemb.UPDDTE, UPDTME = d3znemb.UPDDTE, UPDUSR = d3znemb.UPDUSR });
                dbConnection.Close();


            }
        }
        //public void Update2(string name,string aga, string id,string province)
        //{
        //    using (IDbConnection dbConnection = Connection)
        //    {


        //                    var sql = @"update employee set
        //        firstname = '"+name+@"',
        //        age, = '27'
        //    where
        //        id = '1'
        //    and
        //        province = 'Ayutthaya'

        //    ";
        //        dbConnection.Open();
        //        dbConnection.Execute(sql);
        //        dbConnection.Close();
        //    }

        //}

    }
}

