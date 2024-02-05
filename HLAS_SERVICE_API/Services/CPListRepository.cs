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
    public class CPListRepository
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

        public IEnumerable<CLLIST> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"SELECT * FROM DCPLIB.DCT004 WHERE T4TNCD= 'RCV' AND T4TNDT = @T4TNDT AND T4TYOP = @T4TYOP";
                string sQuery = @"SELECT DCPTNO,PRTCLR,SPLCSH,DLVQTY,DLVZNE,DLVTME,KDLTFR FROM (--,PICTME
                SELECT 
                CASE
                WHEN LENGTH(TRIM(DCPTNO)) = 13 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,3) || '-' || SUBSTRING(TRIM(DCPTNO),10,4)
                WHEN LENGTH(TRIM(DCPTNO)) = 15 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,3) || '-' || SUBSTRING(TRIM(DCPTNO),10,4) || '-' || SUBSTRING(TRIM(DCPTNO),14,2)
                WHEN LENGTH(TRIM(DCPTNO)) = 12 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,5) || '-' || SUBSTRING(TRIM(DCPTNO),11,2) 
                WHEN LENGTH(TRIM(DCPTNO)) = 10 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),5,5) 
                ELSE 'Error'
                END AS DCPTNO,PRTDSC,
                CASE 
                WHEN LENGTH(PRTCLR) = 11 THEN SUBSTRING(PRTCLR, 1, 2) || '-'|| SUBSTRING(PRTCLR, 3,9)
                ELSE 'Error'
                END AS PRTCLR,
                SPPCDE, PCRQTY||'/'||DLVQTY AS DLVQTY,
                DLVZNE,DLVMNT,  
                TIMESTAMP(TIMESTAMP_FORMAT(CONCAT(CHAR(DLVDTE), CHAR(LPAD(DLVTME, 6, 0))), 'YYYYMMDD HH24MISS')) AS DLVTME,
                KDLTFR,
                TIMESTAMP(TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE), CHAR(LPAD(PICTME, 6, 0))), 'YYYYMMDD HH24MISS')) AS PICTME
                ,SUBSTRING(CHAR(CURRENT TIMESTAMP ),12,2),SPLCSH
                FROM 
                (
                SELECT
                DCPTNO,PRTDSC, PRTCLR, SPPCDE, PCRQTY AS PCRQTY1, CPIQTY,
                CASE 
                WHEN PCRQTY = '0' AND  CPIQTY = '0' THEN '0' 
                WHEN PCRQTY = '0' AND  CPIQTY > '0' THEN DLVQTY-CPIQTY  
                WHEN PCRQTY > '0' AND  CPIQTY = '0' THEN PCRQTY
                WHEN PCRQTY > '0' AND  CPIQTY > '0' THEN DLVQTY-CPIQTY 
                END AS PCRQTY,
                DLVQTY ,DLVSCN,DLVZNE,PICDTE, PICTME, DLVDTE, DLVTME, KDLTFR, DLVMNT,SPLCSH
                FROM 
                (
                SELECT DCPTNO,MAX(PRTDSC) AS PRTDSC, PRTCLR, SPPCDE, MAX(PCRQTY) AS PCRQTY, MIN(CPIQTY) AS CPIQTY, MAX(DLVQTY) AS DLVQTY ,MIN(DLVSCN) AS DLVSCN,DLVZNE,PICDTE, PICTME, DLVDTE, DLVTME, KDLTFR, DLVMNT,SPLCSH 
                FROM --IFNULL(PCRQTY,0)
                (
                SELECT DCPTNO,PRTDSC, PRTCLR, SPPCDE, IFNULL(PCRQTY,0) AS PCRQTY, CPIQTY, DLVQTY,
                --DLVSCN,
                CASE WHEN PCRQTY = DLVQTY THEN '1' ELSE DLVSCN END AS DLVSCN,
                DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT,SPLCSH 
                FROM DC2LIB.D3DIWEKJ1
                WHERE
                TIMESTAMP(TIMESTAMP_FORMAT(CONCAT(CHAR(DLVDTE), CHAR(LPAD(DLVTME, 6, 0))), 'YYYYMMDD HH24MISS')) 
                BETWEEN TIMESTAMP ( '2020-10-19' , '08:00:00' ) AND TIMESTAMP ( '2020-10-20' , '07:59:59' )
                --BETWEEN TIMESTAMP ( CURRENT DATE , '08:00:00' ) AND TIMESTAMP ( CURRENT DATE + 1 DAY , '07:59:59' )
                AND
                LEFT(RIGHT('000000'||trim(CHAR(DLVTME)), 6),2) ='09'
                --LEFT(RIGHT('000000'||trim(CHAR(PICTME)), 6),2) = SUBSTRING(CHAR(CURRENT TIMESTAMP),12,2)
                --AND DLVDTE = VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD')--'20200331'
                AND DLVDTE = '20201019'
                AND DLVMNT = 'C ZONE'
                ) T1
                WHERE DLVSCN <> '1'
                GROUP BY DCPTNO, PRTCLR, SPPCDE, DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT,SPLCSH
                ) F1
                UNION ALL 
                SELECT 
                DCPTNO,PRTDSC, PRTCLR, SPPCDE, PCRQTY AS PCRQTY1, CPIQTY,
                CASE 
                WHEN PCRQTY = '0' AND  CPIQTY = '0' THEN '0' 
                WHEN PCRQTY = '0' AND  CPIQTY > '0' THEN DLVQTY-CPIQTY  
                WHEN PCRQTY > '0' AND  CPIQTY = '0' THEN PCRQTY
                WHEN PCRQTY > '0' AND  CPIQTY > '0' THEN DLVQTY-CPIQTY 
                END AS PCRQTY,
                DLVQTY, DLVSCN, DLVZNE, PICDTE, PICTME, DVPLDT, DVPLTM, KDLTNO, DLVMNT,SPLCSH 
                FROM
                (
                SELECT DCPTNO,MAX(PRTDSC) AS PRTDSC, PRTCLR, SPPCDE, MAX(PCRQTY) AS PCRQTY, MIN(CPIQTY) AS CPIQTY, MAX(DLVQTY) AS DLVQTY ,MIN(DLVSCN) AS DLVSCN,DLVZNE,PICDTE, PICTME, DVPLDT, DVPLTM, KDLTNO, DLVMNT,SPLCSH 
                FROM
                (
                SELECT DCPTNO,PRTDSC, PRTCLR, SPPCDE, IFNULL(PCRQTY,0) AS PCRQTY, CPIQTY, DLVQTY,
                CASE WHEN PCRQTY = DLVQTY THEN '1' ELSE DLVSCN END AS DLVSCN,
                DLVZNE, DVPLDT, DVPLTM, KDLTNO, PICDTE, PICTME,DLVMNT ,SPLCSH 
                FROM DC2LIB.D3DISCIJ1
                WHERE
                TIMESTAMP(TIMESTAMP_FORMAT(CONCAT(CHAR(DVPLDT), CHAR(LPAD(DVPLTM, 6, 0))), 'YYYYMMDD HH24MISS')) 
                BETWEEN TIMESTAMP ( '2020-10-19' , '08:00:00' ) AND TIMESTAMP ( '2020-10-20' , '07:59:59' )
                --BETWEEN TIMESTAMP ( CURRENT DATE , '08:00:00' ) AND TIMESTAMP ( CURRENT DATE + 1 DAY , '07:59:59' )
                AND
                LEFT(RIGHT('000000'||trim(CHAR(DVPLTM)), 6),2) ='09'
                --LEFT(RIGHT('000000'||trim(CHAR(DVPLTM)), 6),2) = SUBSTRING(CHAR(CURRENT TIMESTAMP),12,2)
                --AND DVPLDT = VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD')--'20200331'
                AND DVPLDT = '20201019'
                AND DLVMNT = 'C ZONE'
                ) T2
                WHERE DLVSCN <> '1'
                GROUP BY DCPTNO, PRTCLR, SPPCDE, DLVZNE, DVPLDT, DVPLTM, KDLTNO , PICDTE, PICTME,DLVMNT,SPLCSH 
                ) F2
                UNION ALL 
                SELECT 
                L1DCPN,L1PTDS, L1PTCL, L1SPCD, PCRQTY AS PCRQTY1, CPIQTY,
                CASE 
                WHEN PCRQTY = '0' AND  CPIQTY = '0' THEN '0' 
                WHEN PCRQTY = '0' AND  CPIQTY > '0' THEN L1DQTY-CPIQTY  
                WHEN PCRQTY > '0' AND  CPIQTY = '0' THEN PCRQTY 
                WHEN PCRQTY > '0' AND  CPIQTY > '0' THEN L1DQTY-CPIQTY 
                END AS PCRQTY,
                L1DQTY , DLVSCN, DLVZNE,PICDTE, PICTME, DLVDTE, DLVTME, KDLTFR, DLVMNT ,SPLCSH 
                FROM
                (
                SELECT L1DCPN,MAX(L1PTDS) AS L1PTDS, L1PTCL, L1SPCD, MAX(PCRQTY) AS PCRQTY, MIN(CPIQTY) AS CPIQTY, MAX(L1DQTY) AS L1DQTY ,MIN(DLVSCN) AS DLVSCN, DLVZNE,PICDTE, PICTME, DLVDTE, DLVTME, KDLTFR, DLVMNT ,SPLCSH 
                FROM --IFNULL(PCRQTY,0)
                (
                SELECT L1DCPN,L1PTDS, L1PTCL, L1SPCD, IFNULL(PCRQTY,0) AS PCRQTY, L1DQTY, CPIQTY,
                CASE WHEN PCRQTY = L1DQTY THEN '1' ELSE DLVSCN END AS DLVSCN,
                DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT ,SPLCSH 
                FROM DC2LIB.D3DILOTJ1
                WHERE
                TIMESTAMP(TIMESTAMP_FORMAT(CONCAT(CHAR(DLVDTE), CHAR(LPAD(DLVTME, 6, 0))), 'YYYYMMDD HH24MISS')) 
                BETWEEN TIMESTAMP ( '2020-10-19' , '08:00:00' ) AND TIMESTAMP ( '2020-10-20' , '07:59:59' )
                --BETWEEN TIMESTAMP ( CURRENT DATE , '08:00:00' ) AND TIMESTAMP ( CURRENT DATE + 1 DAY , '07:59:59' )
                AND
                LEFT(RIGHT('000000'||trim(CHAR(DLVTME)), 6),2) ='09'
                --LEFT(RIGHT('000000'||trim(CHAR(DLVTME)), 6),2) = SUBSTRING(CHAR(CURRENT TIMESTAMP - 60 MINUTES ),12,2)
                --AND DLVDTE = VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD')--'20200331'
                AND DLVDTE = '20201019'
                AND DLVMNT = 'C ZONE'
                ) T3
                WHERE DLVSCN <> '1'
                GROUP BY L1DCPN, L1PTCL, L1SPCD, DLVZNE, DLVDTE, DLVTME, KDLTFR , PICDTE, PICTME,DLVMNT,SPLCSH 
                ) F3
                ) T5
                ) T6
                ORDER BY PICTME, DCPTNO, KDLTFR ASC";
                dbConnection.Open();
                var result = dbConnection.Query<CLLIST>(sQuery, new { });
                dbConnection.Close();
                return result;
            }
        }
        public IEnumerable<CLLIST> GetAll1()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT DCPTNO,PRTCLR,SPLCSH,DLVQTY,DLVZNE,DLVTME,KDLTFR FROM (--,PICTME
                SELECT 
                CASE
                WHEN LENGTH(TRIM(DCPTNO)) = 13 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,3) || '-' || SUBSTRING(TRIM(DCPTNO),10,4)
                WHEN LENGTH(TRIM(DCPTNO)) = 15 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,3) || '-' || SUBSTRING(TRIM(DCPTNO),10,4) || '-' || SUBSTRING(TRIM(DCPTNO),14,2)
                WHEN LENGTH(TRIM(DCPTNO)) = 12 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,5) || '-' || SUBSTRING(TRIM(DCPTNO),11,2) 
                WHEN LENGTH(TRIM(DCPTNO)) = 10 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),5,5) 
                ELSE 'Error'
                END AS DCPTNO,PRTDSC,
                CASE 
                WHEN LENGTH(PRTCLR) = 11 THEN SUBSTRING(PRTCLR, 1, 2) || '-'|| SUBSTRING(PRTCLR, 3,9)
                ELSE 'Error'
                END AS PRTCLR,
                SPPCDE, PCRQTY||'/'||DLVQTY AS DLVQTY,
                DLVZNE,DLVMNT,  
                TIMESTAMP(TIMESTAMP_FORMAT(CONCAT(CHAR(DLVDTE), CHAR(LPAD(DLVTME, 6, 0))), 'YYYYMMDD HH24MISS')) AS DLVTME,
                KDLTFR,
                TIMESTAMP(TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE), CHAR(LPAD(PICTME, 6, 0))), 'YYYYMMDD HH24MISS')) AS PICTME
                ,SUBSTRING(CHAR(CURRENT TIMESTAMP ),12,2),SPLCSH
                FROM 
                (
                SELECT
                DCPTNO,PRTDSC, PRTCLR, SPPCDE, PCRQTY AS PCRQTY1, CPIQTY,
                CASE 
                WHEN PCRQTY = '0' AND  CPIQTY = '0' THEN '0' 
                WHEN PCRQTY = '0' AND  CPIQTY > '0' THEN DLVQTY-CPIQTY  
                WHEN PCRQTY > '0' AND  CPIQTY = '0' THEN PCRQTY
                WHEN PCRQTY > '0' AND  CPIQTY > '0' THEN DLVQTY-CPIQTY 
                END AS PCRQTY,
                DLVQTY ,DLVSCN,DLVZNE,PICDTE, PICTME, DLVDTE, DLVTME, KDLTFR, DLVMNT,SPLCSH
                FROM 
                (
                SELECT DCPTNO,MAX(PRTDSC) AS PRTDSC, PRTCLR, SPPCDE, MAX(PCRQTY) AS PCRQTY, MIN(CPIQTY) AS CPIQTY, MAX(DLVQTY) AS DLVQTY ,MIN(DLVSCN) AS DLVSCN,DLVZNE,PICDTE, PICTME, DLVDTE, DLVTME, KDLTFR, DLVMNT,SPLCSH 
                FROM --IFNULL(PCRQTY,0)
                (
                SELECT DCPTNO,PRTDSC, PRTCLR, SPPCDE, IFNULL(PCRQTY,0) AS PCRQTY, CPIQTY, DLVQTY,
                --DLVSCN,
                CASE WHEN PCRQTY = DLVQTY THEN '1' ELSE DLVSCN END AS DLVSCN,
                DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT,SPLCSH 
                FROM DC2LIB.D3DIWEKJ1
                WHERE
                TIMESTAMP(TIMESTAMP_FORMAT(CONCAT(CHAR(DLVDTE), CHAR(LPAD(DLVTME, 6, 0))), 'YYYYMMDD HH24MISS')) 
                BETWEEN TIMESTAMP ( '2020-10-19' , '08:00:00' ) AND TIMESTAMP ( '2020-10-20' , '07:59:59' )
                --BETWEEN TIMESTAMP ( CURRENT DATE , '08:00:00' ) AND TIMESTAMP ( CURRENT DATE + 1 DAY , '07:59:59' )
                AND
                LEFT(RIGHT('000000'||trim(CHAR(DLVTME)), 6),2) ='09'
                --LEFT(RIGHT('000000'||trim(CHAR(PICTME)), 6),2) = SUBSTRING(CHAR(CURRENT TIMESTAMP),12,2)
                --AND DLVDTE = VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD')--'20200331'
                AND DLVDTE = '20201019'
                AND DLVMNT = 'C ZONE'
                ) T1
                WHERE DLVSCN <> '1'
                GROUP BY DCPTNO, PRTCLR, SPPCDE, DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT,SPLCSH
                ) F1
                UNION ALL 
                SELECT 
                DCPTNO,PRTDSC, PRTCLR, SPPCDE, PCRQTY AS PCRQTY1, CPIQTY,
                CASE 
                WHEN PCRQTY = '0' AND  CPIQTY = '0' THEN '0' 
                WHEN PCRQTY = '0' AND  CPIQTY > '0' THEN DLVQTY-CPIQTY  
                WHEN PCRQTY > '0' AND  CPIQTY = '0' THEN PCRQTY
                WHEN PCRQTY > '0' AND  CPIQTY > '0' THEN DLVQTY-CPIQTY 
                END AS PCRQTY,
                DLVQTY, DLVSCN, DLVZNE, PICDTE, PICTME, DVPLDT, DVPLTM, KDLTNO, DLVMNT,SPLCSH 
                FROM
                (
                SELECT DCPTNO,MAX(PRTDSC) AS PRTDSC, PRTCLR, SPPCDE, MAX(PCRQTY) AS PCRQTY, MIN(CPIQTY) AS CPIQTY, MAX(DLVQTY) AS DLVQTY ,MIN(DLVSCN) AS DLVSCN,DLVZNE,PICDTE, PICTME, DVPLDT, DVPLTM, KDLTNO, DLVMNT,SPLCSH 
                FROM
                (
                SELECT DCPTNO,PRTDSC, PRTCLR, SPPCDE, IFNULL(PCRQTY,0) AS PCRQTY, CPIQTY, DLVQTY,
                CASE WHEN PCRQTY = DLVQTY THEN '1' ELSE DLVSCN END AS DLVSCN,
                DLVZNE, DVPLDT, DVPLTM, KDLTNO, PICDTE, PICTME,DLVMNT ,SPLCSH 
                FROM DC2LIB.D3DISCIJ1
                WHERE
                TIMESTAMP(TIMESTAMP_FORMAT(CONCAT(CHAR(DVPLDT), CHAR(LPAD(DVPLTM, 6, 0))), 'YYYYMMDD HH24MISS')) 
                BETWEEN TIMESTAMP ( '2020-10-19' , '08:00:00' ) AND TIMESTAMP ( '2020-10-20' , '07:59:59' )
                --BETWEEN TIMESTAMP ( CURRENT DATE , '08:00:00' ) AND TIMESTAMP ( CURRENT DATE + 1 DAY , '07:59:59' )
                AND
                LEFT(RIGHT('000000'||trim(CHAR(DVPLTM)), 6),2) ='09'
                --LEFT(RIGHT('000000'||trim(CHAR(DVPLTM)), 6),2) = SUBSTRING(CHAR(CURRENT TIMESTAMP),12,2)
                --AND DVPLDT = VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD')--'20200331'
                AND DVPLDT = '20201019'
                AND DLVMNT = 'C ZONE'
                ) T2
                WHERE DLVSCN <> '1'
                GROUP BY DCPTNO, PRTCLR, SPPCDE, DLVZNE, DVPLDT, DVPLTM, KDLTNO , PICDTE, PICTME,DLVMNT,SPLCSH 
                ) F2
                UNION ALL 
                SELECT 
                L1DCPN,L1PTDS, L1PTCL, L1SPCD, PCRQTY AS PCRQTY1, CPIQTY,
                CASE 
                WHEN PCRQTY = '0' AND  CPIQTY = '0' THEN '0' 
                WHEN PCRQTY = '0' AND  CPIQTY > '0' THEN L1DQTY-CPIQTY  
                WHEN PCRQTY > '0' AND  CPIQTY = '0' THEN PCRQTY 
                WHEN PCRQTY > '0' AND  CPIQTY > '0' THEN L1DQTY-CPIQTY 
                END AS PCRQTY,
                L1DQTY , DLVSCN, DLVZNE,PICDTE, PICTME, DLVDTE, DLVTME, KDLTFR, DLVMNT ,SPLCSH 
                FROM
                (
                SELECT L1DCPN,MAX(L1PTDS) AS L1PTDS, L1PTCL, L1SPCD, MAX(PCRQTY) AS PCRQTY, MIN(CPIQTY) AS CPIQTY, MAX(L1DQTY) AS L1DQTY ,MIN(DLVSCN) AS DLVSCN, DLVZNE,PICDTE, PICTME, DLVDTE, DLVTME, KDLTFR, DLVMNT ,SPLCSH 
                FROM --IFNULL(PCRQTY,0)
                (
                SELECT L1DCPN,L1PTDS, L1PTCL, L1SPCD, IFNULL(PCRQTY,0) AS PCRQTY, L1DQTY, CPIQTY,
                CASE WHEN PCRQTY = L1DQTY THEN '1' ELSE DLVSCN END AS DLVSCN,
                DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT ,SPLCSH 
                FROM DC2LIB.D3DILOTJ1
                WHERE
                TIMESTAMP(TIMESTAMP_FORMAT(CONCAT(CHAR(DLVDTE), CHAR(LPAD(DLVTME, 6, 0))), 'YYYYMMDD HH24MISS')) 
                BETWEEN TIMESTAMP ( '2020-10-19' , '08:00:00' ) AND TIMESTAMP ( '2020-10-20' , '07:59:59' )
                --BETWEEN TIMESTAMP ( CURRENT DATE , '08:00:00' ) AND TIMESTAMP ( CURRENT DATE + 1 DAY , '07:59:59' )
                AND
                LEFT(RIGHT('000000'||trim(CHAR(DLVTME)), 6),2) ='09'
                --LEFT(RIGHT('000000'||trim(CHAR(DLVTME)), 6),2) = SUBSTRING(CHAR(CURRENT TIMESTAMP - 60 MINUTES ),12,2)
                --AND DLVDTE = VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD')--'20200331'
                AND DLVDTE = '20201019'
                AND DLVMNT = 'C ZONE'
                ) T3
                WHERE DLVSCN <> '1'
                GROUP BY L1DCPN, L1PTCL, L1SPCD, DLVZNE, DLVDTE, DLVTME, KDLTFR , PICDTE, PICTME,DLVMNT,SPLCSH 
                ) F3
                ) T5
                ) T6
                ORDER BY PICTME, DCPTNO, KDLTFR ASC";

               dbConnection.Open();
               var result = dbConnection.Query<CLLIST>(sQuery);
               dbConnection.Close();
               return result;

            }
        }
    }
}

