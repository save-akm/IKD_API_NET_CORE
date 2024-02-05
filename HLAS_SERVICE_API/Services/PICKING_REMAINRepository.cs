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
    public class PICKING_REMAINRepository
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
        public IEnumerable<PICKING_REMAIN> GetByIDDelivery(int DLVDTE1, int DLVDTE2, string DLVMNT)
        // public PICKING_REMAIN GetByIDsrc(int PICDTE1, int PICDTE2, string DLVMNT)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"SELECT COUNT(*) AS USUSCD FROM DC2LIB.DATUSER WHERE USUSCD=@USUSCD AND USPASS=@USPASS AND USSCCD = '01'";

                string sQuery =
                @"
				SELECT 
				*
				FROM (
	            SELECT PKLNO ,CPINO, DCPTNO, PRTCLR, SPPCDE, IFNULL(PCRQTY,0) AS PCRQTY, DLVQTY, DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT,SPLCSH,
	            CASE WHEN PCRQTY = DLVQTY THEN '1' ELSE PICSCN END AS PICSCN,
            	DLVSCN
	            FROM DC2LIB.D3DIWEKJ1
	            WHERE
                PCRQTY < DLVQTY
	            UNION ALL
	            SELECT PKLNO ,CPINO, DCPTNO, PRTCLR, SPPCDE, IFNULL(PCRQTY,0) AS PCRQTY, DLVQTY, DLVZNE, DVPLDT, DVPLTM, KDLTNO, PICDTE, PICTME,DLVMNT ,SPLCSH,
	            CASE WHEN PCRQTY = DLVQTY THEN '1' ELSE PICSCN END AS PICSCN,
            	DLVSCN
	            FROM DC2LIB.D3DISCIJ1
	            WHERE
	            PCRQTY < DLVQTY
	            UNION ALL
	            SELECT PKLNO ,CPINO, L1DCPN, L1PTCL, L1SPCD, IFNULL(PCRQTY,0) AS PCRQTY, L1DQTY, DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT ,SPLCSH, 
	            CASE WHEN PCRQTY = L1DQTY THEN '1' ELSE PICSCN END AS PICSCN,
            	DLVSCN
	            FROM DC2LIB.D3DILOTJ1
                WHERE
                PCRQTY < L1DQTY
                ) T1
                WHERE
				--DLVDTE BETWEEN '20201201' AND '20201203' AND
			    --DLVMNT = 'C ZONE 1'
				DLVDTE BETWEEN @DLVDTE1 AND @DLVDTE2 AND
				DLVMNT = @DLVMNT
                ORDER BY DLVDTE , DLVTME 
                ";
                dbConnection.Open();
                var result = dbConnection.Query<PICKING_REMAIN>(sQuery, new { DLVDTE1, DLVDTE2, DLVMNT });
                //var result = dbConnection.Query<PICKING_REMAIN>(sQuery).FirstOrDefault();
                //var result = dbConnection.Query<D3ZNEMB>(sQuery).ToList();
                dbConnection.Close();
                return result;
            }
        }
        public IEnumerable<PICKING_REMAIN> GetByIDPicking(int PICDTE1, int PICDTE2, string DLVMNT)
        // public PICKING_REMAIN GetByIDsrc(int PICDTE1, int PICDTE2, string DLVMNT)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"SELECT COUNT(*) AS USUSCD FROM DC2LIB.DATUSER WHERE USUSCD=@USUSCD AND USPASS=@USPASS AND USSCCD = '01'";

                string sQuery =
                @"
				SELECT 
				*
				FROM (
	            SELECT PKLNO ,CPINO, DCPTNO, PRTCLR, SPPCDE, IFNULL(PCRQTY,0) AS PCRQTY, DLVQTY, DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT,SPLCSH,
	            CASE WHEN PCRQTY = DLVQTY THEN '1' ELSE PICSCN END AS PICSCN,
            	DLVSCN
	            FROM DC2LIB.D3DIWEKJ1
	            WHERE
                PCRQTY < DLVQTY
	            UNION ALL
	            SELECT PKLNO ,CPINO, DCPTNO, PRTCLR, SPPCDE, IFNULL(PCRQTY,0) AS PCRQTY, DLVQTY, DLVZNE, DVPLDT, DVPLTM, KDLTNO, PICDTE, PICTME,DLVMNT ,SPLCSH,
	            CASE WHEN PCRQTY = DLVQTY THEN '1' ELSE PICSCN END AS PICSCN,
            	DLVSCN
	            FROM DC2LIB.D3DISCIJ1
	            WHERE
	            PCRQTY < DLVQTY
	            UNION ALL
	            SELECT PKLNO ,CPINO, L1DCPN, L1PTCL, L1SPCD, IFNULL(PCRQTY,0) AS PCRQTY, L1DQTY, DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT ,SPLCSH, 
	            CASE WHEN PCRQTY = L1DQTY THEN '1' ELSE PICSCN END AS PICSCN,
            	DLVSCN
	            FROM DC2LIB.D3DILOTJ1
                WHERE
                PCRQTY < L1DQTY
                ) T1
                WHERE
                --PICDTE BETWEEN '20201101' AND '20201203' AND
               --DLVMNT = 'C ZONE 1'
                PICDTE BETWEEN @PICDTE1 AND @PICDTE2 AND
                DLVMNT = @DLVMNT  
                ORDER BY PICDTE , PICTME 
                ";
                dbConnection.Open();
                var result = dbConnection.Query<PICKING_REMAIN>(sQuery, new { PICDTE1, PICDTE2, DLVMNT });
                //var result = dbConnection.Query<PICKING_REMAIN>(sQuery).FirstOrDefault();
                //var result = dbConnection.Query<D3ZNEMB>(sQuery).ToList();
                dbConnection.Close();
                return result;
                // '"+@PICDTE1+"'
            }
        }
        public IEnumerable<PICKING_REMAIN> GetByID(int PICDTE, string DLVMNT)
            //public PICKING_REMAIN GetByID(string DLVMNT, string PICDTE)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery =
                @"
				SELECT DCPTNO, PRTCLR, SPPCDE, PCRQTY, DLVQTY, DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT,SPLCSH FROM (
	            SELECT DCPTNO, PRTCLR, SPPCDE, IFNULL(PCRQTY,0) AS PCRQTY, DLVQTY, DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT,SPLCSH 
	            FROM DC2LIB.D3DIWEKJ1
	            WHERE
                PCRQTY < DLVQTY
	            UNION ALL
	            SELECT DCPTNO, PRTCLR, SPPCDE, IFNULL(PCRQTY,0) AS PCRQTY, DLVQTY, DLVZNE, DVPLDT, DVPLTM, KDLTNO, PICDTE, PICTME,DLVMNT ,SPLCSH 
	            FROM DC2LIB.D3DISCIJ1
	            WHERE
	            PCRQTY < DLVQTY
	            UNION ALL
	            SELECT L1DCPN, L1PTCL, L1SPCD, IFNULL(PCRQTY,0) AS PCRQTY, L1DQTY, DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT ,SPLCSH 
	            FROM DC2LIB.D3DILOTJ1
                WHERE
                PCRQTY < L1DQTY
                ) T1
                WHERE
                PICDTE = @PICDTE AND 
                DLVMNT = @DLVMNT
                ";
                dbConnection.Open();
                //var result = dbConnection.Query<PICKING_REMAIN>(sQuery, new {  PICDTE = PICDTE, DLVMNT = DLVMNT }).ToList();//, new { DLVMNT = DLVMNT, PICDTE = PICDTE}
                //dbConnection.Close();
                //return result;
                var result = dbConnection.Query<PICKING_REMAIN>(sQuery, new { PICDTE = PICDTE, DLVMNT = DLVMNT });
                //var result = dbConnection.Query<PICKING_REMAIN>(sQuery);
                //var result = dbConnection.Query<D3ZNEMB>(sQuery).ToList();
                dbConnection.Close();
                return result;
            }
        }
    }
}