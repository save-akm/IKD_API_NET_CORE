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
    public class IKDMONITORINGRepository
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

        public IEnumerable<IKDMONITORING> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"
                SELECT MAX(D1KDLT) AS D1KDLT,MAX(D1MDNM) AS D1MDNM, MAX(D1MDCD) AS D1MDCD, MAX(D1TYPE) AS D1TYPE, MAX(D1PROC) AS D1PROC, MAX(D1PQTY) AS D1PQTY, MAX(D3QTYM) AS D3QTYM, MAX(D1STPL) AS D1STPL, MAX(DPTQTY) AS DPTQTY, MAX(RCVQTY) AS RCVQTY, MAX(PLANS) AS PLANS, MAX(ACTUAL) AS ACTUAL FROM (
                SELECT * FROM (
                SELECT D1KDLT,MAX(D1MDNM)AS D1MDNM, MAX(D1MDCD) AS D1MDCD, MAX(D1TYPE) AS D1TYPE, MAX(D1PROC) AS D1PROC,MAX(D1PQTY) AS D1PQTY,MAX(D3QTYM) AS D3QTYM,MAX(D1STPL) AS D1STPL,SUM(DPTQTY) AS DPTQTY,SUM(IFNULL(RCVQTY,0)) AS RCVQTY
                ,COUNT(*) AS PLANS, 0 AS ACTUAL
                FROM (
                SELECT D1KDLT,D1MDNM, D1MDCD, D1TYPE, D1DCFP, D1PCOR, D1DLTO, D1PROC,D1PQTY,D3QTYM,D1STPL,D1PQTY*D3QTYM AS DPTQTY FROM 
                (
                SELECT * FROM DC2LIB.D3DLVPN 
                WHERE D1STPL ='IKD1' AND (SUBSTRING(D1PROC,1,2) <> 'AE' OR SUBSTRING(D1PROC,1,2) <> 'AF')
                AND (D1DLD2 = VARCHAR_FORMAT(CURRENT TIMESTAMP-2 DAY, 'YYYYMMDD') 
                OR D1DLD1 = VARCHAR_FORMAT(CURRENT TIMESTAMP-1 DAY, 'YYYYMMDD') 
                OR D1DLD9 = VARCHAR_FORMAT(CURRENT TIMESTAMP-1 DAY, 'YYYYMMDD') 
                OR D1DLDJ =VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD')) 
                ) PL
                LEFT JOIN DC2LIB.D3PRTLT LT
                ON LT.D3MDTY = TRIM(PL.D1MDCD) || TRIM(PL.D1TYPE) AND LT.D3DCPT = PL.D1DCFP AND LT.D3LOCC = PL.D1PCOR
                ) T1
                LEFT JOIN (SELECT DCPTNO ,PRTCLR,KDLTFR ,SUM(RCVQTY) AS RCVQTY FROM DC2LIB.D3RCVRT WHERE SPLFLG = '2' GROUP BY DCPTNO,KDLTFR ,PRTCLR) AL
                ON REPLACE(AL.KDLTFR,'-','') = SUBSTRING(REPLACE(T1.D1KDLT,'-',''),1,2)||SUBSTRING(REPLACE(T1.D1KDLT,'-',''),5)
                AND AL.DCPTNO = T1.D1DCFP AND AL.PRTCLR = T1.D1PCOR
                GROUP BY D1KDLT,D1MDCD,D1TYPE ORDER BY D1KDLT
                )T2
                UNION ALL
                SELECT * FROM (
                --SELECT D1KDLT, D1MDCD, D1TYPE, D1DCFP, D1PCOR, D1DLTO, D1PROC,D1PQTY,D3QTYM,D1STPL,DPTQTY,RCVQTY FROM (
                SELECT D1KDLT,MAX(D1MDNM)AS D1MDNM, MAX(D1MDCD) AS D1MDCD, MAX(D1TYPE) AS D1TYPE, MAX(D1PROC) AS D1PROC,MAX(D1PQTY) AS D1PQTY,MAX(D3QTYM) AS D3QTYM,MAX(D1STPL) AS D1STPL,SUM(DPTQTY) AS DPTQTY,SUM(IFNULL(RCVQTY,0)) AS RCVQTY
                ,0 AS PLANS, COUNT(*) AS ACTUAL
                FROM (
                SELECT D1KDLT,D1MDNM, D1MDCD, D1TYPE, D1DCFP, D1PCOR, D1DLTO, D1PROC,D1PQTY,D3QTYM,D1STPL,D1PQTY*D3QTYM AS DPTQTY FROM 
                (
                SELECT * FROM DC2LIB.D3DLVPN 
                WHERE D1STPL ='IKD1' AND (SUBSTRING(D1PROC,1,2) <> 'AE' OR SUBSTRING(D1PROC,1,2) <> 'AF')
                AND (D1DLD2 = VARCHAR_FORMAT(CURRENT TIMESTAMP-2 DAY, 'YYYYMMDD') 
                OR D1DLD1 = VARCHAR_FORMAT(CURRENT TIMESTAMP-1 DAY, 'YYYYMMDD') 
                OR D1DLD9 = VARCHAR_FORMAT(CURRENT TIMESTAMP-1 DAY, 'YYYYMMDD') 
                OR D1DLDJ =VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD')) 
                ) PL
                LEFT JOIN DC2LIB.D3PRTLT LT
                ON LT.D3MDTY = TRIM(PL.D1MDCD) || TRIM(PL.D1TYPE) AND LT.D3DCPT = PL.D1DCFP AND LT.D3LOCC = PL.D1PCOR
                ) T1
                LEFT JOIN (SELECT DCPTNO ,PRTCLR,KDLTFR ,SUM(RCVQTY) AS RCVQTY FROM DC2LIB.D3RCVRT WHERE SPLFLG = '2' GROUP BY DCPTNO,KDLTFR ,PRTCLR) AL
                ON REPLACE(AL.KDLTFR,'-','') = SUBSTRING(REPLACE(T1.D1KDLT,'-',''),1,2)||SUBSTRING(REPLACE(T1.D1KDLT,'-',''),5)
                AND AL.DCPTNO = T1.D1DCFP AND AL.PRTCLR = T1.D1PCOR
                GROUP BY D1KDLT,D1MDCD,D1TYPE ORDER BY D1KDLT
                )T2
                WHERE DPTQTY = RCVQTY
                ) T3
                GROUP BY D1KDLT,D1MDCD,D1TYPE ORDER BY D1KDLT
                ";
                dbConnection.Open();
                var result = dbConnection.Query<IKDMONITORING>(sQuery);
                dbConnection.Close();
                return result;

            }
        }

        public IEnumerable<IKDMONITORING> GetAllBoxUnpack()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"
               SELECT MODEL, MTO, KDLOTNO,QTYLOT,WE_ON,
                SUBSTRING(CAST(N2 AS VARCHAR(19)), 1, 10) AS DATE_N2,
                SUBSTRING(CAST(N2 AS VARCHAR(19)), 12, 2)||':'||SUBSTRING(CAST(N2 AS VARCHAR(19)), 15, 2)||':'||SUBSTRING(CAST(N2 AS VARCHAR(19)), 18, 2) AS TIME_N2,
                PLANN2,ITEM_LOAD_N2,
                SUBSTRING(CAST(N1 AS VARCHAR(19)), 1, 10) AS DATE_N1,
                SUBSTRING(CAST(N1 AS VARCHAR(19)), 12, 2)||':'||SUBSTRING(CAST(N1 AS VARCHAR(19)), 15, 2)||':'||SUBSTRING(CAST(N1 AS VARCHAR(19)), 18, 2) AS TIME_N1,
                PLANN1,ITEM_LOAD_N1,
                SUBSTRING(CAST(JT AS VARCHAR(19)), 1, 10) AS DATE_JT,
                SUBSTRING(CAST(JT AS VARCHAR(19)), 12, 2)||':'||SUBSTRING(CAST(JT AS VARCHAR(19)), 15, 2)||':'||SUBSTRING(CAST(JT AS VARCHAR(19)), 18, 2)AS TIME_JT,
                PLANJT,ITEM_LOAD_JT
                FROM (
                SELECT MODEL, MTO, KDLOTNO,Q_LOT AS QTYLOT,
                VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') AS WE_ON,
                DATE_N2,TIME_N2,
                TIMESTAMP_FORMAT(CONCAT(CHAR(DATE_N2), CHAR(LPAD((TIME_N2), 6, 0))), 'YYYYMMDD HH24MISS') AS N2,
                ITEM_N2 AS PLANN2,ITEM_LOAD_N2,
                DATE_N1,TIME_N1,
                TIMESTAMP_FORMAT(CONCAT(CHAR(DATE_N1), CHAR(LPAD((TIME_N1), 6, 0))), 'YYYYMMDD HH24MISS') AS N1,
                ITEM_N1 AS PLANN1,ITEM_LOAD_N1,
                DATE_JT,TIME_JT*10000 AS TIME_JT,
                TIMESTAMP_FORMAT(CONCAT(CHAR(DATE_JT), CHAR(LPAD((TIME_JT*10000), 6, 0))), 'YYYYMMDD HH24MISS') AS JT,
                ITEM_JT AS PLANJT,ITEM_LOAD_JT                
                FROM (
                select 
                case when TD2.Model is not null then TD2.Model when TD1.Model is not null then TD1.Model when TD3.Model is not null then TD3.Model end as Model 
                ,case when TD2.MTO is not null then TD2.MTO when TD1.MTO is not null then TD1.MTO when TD3.MTO is not null then TD3.MTO end as MTO 
                ,case when TD2.KDLotNo is not null then TD2.KDLotNo when TD1.KDLotNo is not null then TD1.KDLotNo when TD3.KDLotNo is not null then TD3.KDLotNo end as KDLotNo 
                ,case when TD2.Q_Lot is not null then TD2.Q_Lot when TD1.Q_Lot is not null then TD1.Q_Lot when TD3.Q_Lot is not null then TD3.Q_Lot end as Q_Lot
                ,TD2.Date_N2, TD2.tn2 as Time_N2
                ,TD2.CItem as Item_N2
                ,TD2.QPLN  as Plan_N2
                ,TD2.C_Item_Rcv as Item_Unpack_N2
                ,TD2.Rcv  as Unpack_N2
                ,TD2.C_Item_Load as Item_Load_N2
                ,TD2.Loadd as Load_N2
                --
                ,TD1.Date_N1, TD1.tn1 as Time_N1
                ,TD1.CItem as Item_N1
                ,TD1.QPLN  as Plan_N1
                ,TD1.C_Item_Rcv as Item_Unpack_N1
                ,TD1.Rcv  as Unpack_N1
                ,TD1.C_Item_Load as Item_Load_N1
                ,TD1.Loadd as Load_N1
                --
                ,TD3.Date_JT, TD3.tj as Time_JT
                ,TD3.CItem as Item_JT
                ,TD3.QPLN  as Plan_JT
                ,TD3.C_Item_Rcv as Item_Unpack_JT
                ,TD3.Rcv  as Unpack_JT
                ,TD3.C_Item_Load as Item_Load_JT
                ,TD3.Loadd as Load_JT
                --
                from 
                (
                select 
                 P2.Model, P2.MTO,KDLotNo, ifnull(QL,0) Q_Lot
                ,dn2 as Date_N2, tn2
                ,count(Part) as CItem
                ,sum(ifnull(d3qtym,0)*QL) as QPLN
                ,sum(AC.C_Item) as C_Item_Rcv
                 ,sum(ifnull(AC.rcvqty,0)) as Rcv
                ,sum(AD.C_Item) as C_Item_Load
                ,sum(ifnull(AD.rcvqty,0)) As Loadd 
                --
                 from 
                (
                select d1mdnm as Model,trim(d1mdcd)||trim(d1type) as MTO,d1kdlt as KDLotNo,d1pqty as QL
                ,d1dld2 as dn2,d1dlt2 as tn2,d1dcfp as Part 
                from dc2lib.d3dlvpn
                where d1dld2>=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') as int) AND d1dld2<=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP +1 DAY, 'YYYYMMDD') as int)
                and (substring(d1proc,1,4)='WE-S') AND D1STPL ='IKD1'
                --
                ) as P2
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                group by dcptno,kdltfr 
                ) as AC
                on AC.dcptno=Part and replace(AC.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                --
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                where splflg<>''
                group by dcptno,kdltfr 
                ) as AD
                on AD.dcptno=Part and replace(AD.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                left join 
                (
                select * from dc2lib.d3prtlt
                ) as PL
                on d3mdty=MTO and d3dcpt=Part
                --
                group by P2.Model, P2.MTO,KDLotNo, QL
                ,dn2, tn2
                ) as TD2
                --
                -----------------------
                full outer join
                -----------------------
                (
                select 
                 P2.Model, P2.MTO,KDLotNo, ifnull(QL,0) Q_Lot
                ,dn1 as Date_N1, tn1
                ,count(Part) as CItem
                ,sum(ifnull(d3qtym,0)*QL) as QPLN
                 ,sum(AC.C_Item) as C_Item_Rcv
                 ,sum(ifnull(AC.rcvqty,0)) as Rcv
                ,sum(AD.C_Item) as C_Item_Load
                ,sum(ifnull(AD.rcvqty,0)) As Loadd
                --
                 from 
                (
                select d1mdnm as Model,trim(d1mdcd)||trim(d1type) as MTO,d1kdlt as KDLotNo,d1pqty as QL
                ,d1dld1 as dn1,d1dlt1 as tn1,d1dcfp as Part 
                from dc2lib.d3dlvpn
                where d1dld1>=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') as int) AND d1dld1<=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP + 1 DAY, 'YYYYMMDD') as int)                    
                and (substring(d1proc,1,4)='WE-D' or substring(d1proc,1,2)='IU' or substring(d1proc,1,2)='MC' OR substring(d1proc,1,2)='RE') AND D1STPL ='IKD1'
				union all 
 				select d1mdnm as Model,trim(d1mdcd)||trim(d1type) as MTO,d1kdlt as KDLotNo,d1pqty as QL
                ,d1dld9 as dn1,d1dlt9 as tn1,d1dcfp as Part 
                from dc2lib.d3dlvpn
                where 
                 d1dld9>=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') as int) AND d1dld9<=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP + 1 DAY, 'YYYYMMDD') as int)
                and ( substring(d1proc,1,2)='G9')
                --
                --
                ) as P2
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                group by dcptno,kdltfr 
                ) as AC
                on AC.dcptno=Part and replace(AC.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                --
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                where splflg<>''
                group by dcptno,kdltfr 
                ) as AD
                on AD.dcptno=Part and replace(AD.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                left join 
                (
                select * from dc2lib.d3prtlt
                ) as PL
                on d3mdty=MTO and d3dcpt=Part
                --
                group by P2.Model, P2.MTO,KDLotNo, QL
                ,dn1, tn1
                ) as TD1
                on TD1.KDLotNo=TD2.KDLotNo
                ---
                ---
                ---
                -----------------------
                full outer join
                -----------------------
                ---
                (
                select 
                 P2.Model, P2.MTO,KDLotNo, ifnull(QL,0) Q_Lot
                ,dj as Date_JT, tj
                ,count(Part) as CItem
                ,sum(ifnull(d3qtym,0)*QL) as QPLN
                 ,sum(AC.C_Item) as C_Item_Rcv
                 ,sum(ifnull(AC.rcvqty,0)) as Rcv
                ,sum(AD.C_Item) as C_Item_Load
                ,sum(ifnull(AD.rcvqty,0)) As Loadd
                ---
                 from 
                (
                select d1mdnm as Model,trim(d1mdcd)||trim(d1type) as MTO,d1kdlt as KDLotNo,d1pqty as QL
                ,d1dldj as dj,d1dltj as tj,d1dcfp as Part 
                from dc2lib.d3dlvpn
                where d1dldj>=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') as int) AND  d1dldj<=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP + 1 DAY, 'YYYYMMDD') as int) 
                --
                and (substring(d1proc,1,4)='WE-J' ) AND D1STPL ='IKD1'
                --
                --
                ) as P2
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                group by dcptno,kdltfr 
                ) as AC
                on AC.dcptno=Part and replace(AC.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                --
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                where splflg<>''
                group by dcptno,kdltfr 
                ) as AD
                on AD.dcptno=Part and replace(AD.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                left join 
                (
                select * from dc2lib.d3prtlt
                ) as PL
                on d3mdty=MTO and d3dcpt=Part
                --
                group by P2.Model, P2.MTO,KDLotNo, QL
                ,dj, tj
                ) as TD3
                on TD1.KDLotNo=TD3.KDLotNo
                ) T9
                ORDER BY DATE_N2
                )T10
                ";
                dbConnection.Open();
                var result = dbConnection.Query<IKDMONITORING>(sQuery);
                dbConnection.Close();
                return result;

            }
        }
        public IEnumerable<IKDMONITORING> GetAllStaging()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"
               SELECT MODEL, MTO, KDLOTNO,QTYLOT,WE_ON,
                SUBSTRING(CAST(N2 AS VARCHAR(19)), 1, 10) AS DATE_N2,
                SUBSTRING(CAST(N2 AS VARCHAR(19)), 12, 2)||':'||SUBSTRING(CAST(N2 AS VARCHAR(19)), 15, 2)||':'||SUBSTRING(CAST(N2 AS VARCHAR(19)), 18, 2) AS TIME_N2,
                PLANN2,ITEM_LOAD_N2,
                SUBSTRING(CAST(N1 AS VARCHAR(19)), 1, 10) AS DATE_N1,
                SUBSTRING(CAST(N1 AS VARCHAR(19)), 12, 2)||':'||SUBSTRING(CAST(N1 AS VARCHAR(19)), 15, 2)||':'||SUBSTRING(CAST(N1 AS VARCHAR(19)), 18, 2) AS TIME_N1,
                PLANN1,ITEM_LOAD_N1,
                SUBSTRING(CAST(JT AS VARCHAR(19)), 1, 10) AS DATE_JT,
                SUBSTRING(CAST(JT AS VARCHAR(19)), 12, 2)||':'||SUBSTRING(CAST(JT AS VARCHAR(19)), 15, 2)||':'||SUBSTRING(CAST(JT AS VARCHAR(19)), 18, 2)AS TIME_JT,
                PLANJT,ITEM_LOAD_JT
                FROM (
                SELECT MODEL, MTO, KDLOTNO,Q_LOT AS QTYLOT,
                VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') AS WE_ON,
                DATE_N2,TIME_N2,
                TIMESTAMP_FORMAT(CONCAT(CHAR(DATE_N2), CHAR(LPAD((TIME_N2), 6, 0))), 'YYYYMMDD HH24MISS') AS N2,
                ITEM_N2 AS PLANN2,ITEM_LOAD_N2,
                DATE_N1,TIME_N1,
                TIMESTAMP_FORMAT(CONCAT(CHAR(DATE_N1), CHAR(LPAD((TIME_N1), 6, 0))), 'YYYYMMDD HH24MISS') AS N1,
                ITEM_N1 AS PLANN1,ITEM_LOAD_N1,
                DATE_JT,TIME_JT*10000 AS TIME_JT,
                TIMESTAMP_FORMAT(CONCAT(CHAR(DATE_JT), CHAR(LPAD((TIME_JT*10000), 6, 0))), 'YYYYMMDD HH24MISS') AS JT,
                ITEM_JT AS PLANJT,ITEM_LOAD_JT                
                FROM (
                select 
                case when TD2.Model is not null then TD2.Model when TD1.Model is not null then TD1.Model when TD3.Model is not null then TD3.Model end as Model 
                ,case when TD2.MTO is not null then TD2.MTO when TD1.MTO is not null then TD1.MTO when TD3.MTO is not null then TD3.MTO end as MTO 
                ,case when TD2.KDLotNo is not null then TD2.KDLotNo when TD1.KDLotNo is not null then TD1.KDLotNo when TD3.KDLotNo is not null then TD3.KDLotNo end as KDLotNo 
                ,case when TD2.Q_Lot is not null then TD2.Q_Lot when TD1.Q_Lot is not null then TD1.Q_Lot when TD3.Q_Lot is not null then TD3.Q_Lot end as Q_Lot
                ,TD2.Date_N2, TD2.tn2 as Time_N2
                ,TD2.CItem as Item_N2
                ,TD2.QPLN  as Plan_N2
                ,TD2.C_Item_Rcv as Item_Unpack_N2
                ,TD2.Rcv  as Unpack_N2
                ,TD2.C_Item_Load as Item_Load_N2
                ,TD2.Loadd as Load_N2
                --
                ,TD1.Date_N1, TD1.tn1 as Time_N1
                ,TD1.CItem as Item_N1
                ,TD1.QPLN  as Plan_N1
                ,TD1.C_Item_Rcv as Item_Unpack_N1
                ,TD1.Rcv  as Unpack_N1
                ,TD1.C_Item_Load as Item_Load_N1
                ,TD1.Loadd as Load_N1
                --
                ,TD3.Date_JT, TD3.tj as Time_JT
                ,TD3.CItem as Item_JT
                ,TD3.QPLN  as Plan_JT
                ,TD3.C_Item_Rcv as Item_Unpack_JT
                ,TD3.Rcv  as Unpack_JT
                ,TD3.C_Item_Load as Item_Load_JT
                ,TD3.Loadd as Load_JT
                --
                from 
                (
                select 
                 P2.Model, P2.MTO,KDLotNo, ifnull(QL,0) Q_Lot
                ,dn2 as Date_N2, tn2
                ,count(Part) as CItem
                ,sum(ifnull(d3qtym,0)*QL) as QPLN
                ,sum(AC.C_Item) as C_Item_Rcv
                 ,sum(ifnull(AC.rcvqty,0)) as Rcv
                ,sum(AD.C_Item) as C_Item_Load
                ,sum(ifnull(AD.rcvqty,0)) As Loadd 
                --
                 from 
                (
                select d1mdnm as Model,trim(d1mdcd)||trim(d1type) as MTO,d1kdlt as KDLotNo,d1pqty as QL
                ,d1dld2 as dn2,d1dlt2 as tn2,d1dcfp as Part 
                from dc2lib.d3dlvpn
                where d1dld2>=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') as int) AND d1dld2<=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP +1 DAY, 'YYYYMMDD') as int)
                  and (substring(d1proc,1,4)='WE-S') AND D1STPL ='IKD1'
                --
                ) as P2
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                group by dcptno,kdltfr 
                ) as AC
                on AC.dcptno=Part and replace(AC.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                --
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                where splflg= '2' OR splflg= '3' AND splflg<> ' '
                group by dcptno,kdltfr 
                ) as AD
                on AD.dcptno=Part and replace(AD.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                left join 
                (
                select * from dc2lib.d3prtlt
                ) as PL
                on d3mdty=MTO and d3dcpt=Part
                --
                group by P2.Model, P2.MTO,KDLotNo, QL
                ,dn2, tn2
                ) as TD2
                --
                -----------------------
                full outer join
                -----------------------
                (
                select 
                 P2.Model, P2.MTO,KDLotNo, ifnull(QL,0) Q_Lot
                ,dn1 as Date_N1, tn1
                ,count(Part) as CItem
                ,sum(ifnull(d3qtym,0)*QL) as QPLN
                 ,sum(AC.C_Item) as C_Item_Rcv
                 ,sum(ifnull(AC.rcvqty,0)) as Rcv
                ,sum(AD.C_Item) as C_Item_Load
                ,sum(ifnull(AD.rcvqty,0)) As Loadd
                --
                 from 
                (
                select d1mdnm as Model,trim(d1mdcd)||trim(d1type) as MTO,d1kdlt as KDLotNo,d1pqty as QL
                ,d1dld1 as dn1,d1dlt1 as tn1,d1dcfp as Part 
                from dc2lib.d3dlvpn
                where d1dld1>=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') as int) AND d1dld1<=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP + 1 DAY, 'YYYYMMDD') as int)                    
                and (substring(d1proc,1,4)='WE-D' or substring(d1proc,1,2)='IU' or substring(d1proc,1,2)='MC' OR substring(d1proc,1,2)='RE') AND D1STPL ='IKD1'
				union all 
 				select d1mdnm as Model,trim(d1mdcd)||trim(d1type) as MTO,d1kdlt as KDLotNo,d1pqty as QL
                ,d1dld9 as dn1,d1dlt9 as tn1,d1dcfp as Part 
                from dc2lib.d3dlvpn
                where 
                 d1dld9>=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') as int) AND d1dld9<=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP + 1 DAY, 'YYYYMMDD') as int)
                and ( substring(d1proc,1,2)='G9')
                --
                --
                ) as P2
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                group by dcptno,kdltfr 
                ) as AC
                on AC.dcptno=Part and replace(AC.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                --
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                where splflg= '2' OR splflg= '3' AND splflg<> ' '
                group by dcptno,kdltfr 
                ) as AD
                on AD.dcptno=Part and replace(AD.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                left join 
                (
                select * from dc2lib.d3prtlt
                ) as PL
                on d3mdty=MTO and d3dcpt=Part
                --
                group by P2.Model, P2.MTO,KDLotNo, QL
                ,dn1, tn1
                ) as TD1
                on TD1.KDLotNo=TD2.KDLotNo
                ---
                ---
                ---
                -----------------------
                full outer join
                -----------------------
                ---
                (
                select 
                 P2.Model, P2.MTO,KDLotNo, ifnull(QL,0) Q_Lot
                ,dj as Date_JT, tj
                ,count(Part) as CItem
                ,sum(ifnull(d3qtym,0)*QL) as QPLN
                 ,sum(AC.C_Item) as C_Item_Rcv
                 ,sum(ifnull(AC.rcvqty,0)) as Rcv
                ,sum(AD.C_Item) as C_Item_Load
                ,sum(ifnull(AD.rcvqty,0)) As Loadd
                ---
                 from 
                (
                select d1mdnm as Model,trim(d1mdcd)||trim(d1type) as MTO,d1kdlt as KDLotNo,d1pqty as QL
                ,d1dldj as dj,d1dltj as tj,d1dcfp as Part 
                from dc2lib.d3dlvpn
                where d1dldj>=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') as int) AND  d1dldj<=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP + 1 DAY, 'YYYYMMDD') as int) 
                --
                and (substring(d1proc,1,4)='WE-J' ) AND D1STPL ='IKD1'
                --
                --
                ) as P2
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                group by dcptno,kdltfr 
                ) as AC
                on AC.dcptno=Part and replace(AC.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                --
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                where splflg= '2' OR splflg= '3' AND splflg<> ' '
                group by dcptno,kdltfr 
                ) as AD
                on AD.dcptno=Part and replace(AD.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                left join 
                (
                select * from dc2lib.d3prtlt
                ) as PL
                on d3mdty=MTO and d3dcpt=Part
                --
                group by P2.Model, P2.MTO,KDLotNo, QL
                ,dj, tj
                ) as TD3
                on TD1.KDLotNo=TD3.KDLotNo
                ) T9
                ORDER BY DATE_N2
                )T10
                ";
                dbConnection.Open();
                var result = dbConnection.Query<IKDMONITORING>(sQuery);
                dbConnection.Close();
                return result;

            }
        }
        public IEnumerable<IKDMONITORING> GetAllN2N1G9JT_OLD()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"
               SELECT MODEL, MTO, KDLOTNO,QTYLOT,WE_ON,
                SUBSTRING(CAST(N2 AS VARCHAR(19)), 1, 10) AS DATE_N2,
                SUBSTRING(CAST(N2 AS VARCHAR(19)), 12, 2)||':'||SUBSTRING(CAST(N2 AS VARCHAR(19)), 15, 2)||':'||SUBSTRING(CAST(N2 AS VARCHAR(19)), 18, 2) AS TIME_N2,
                PLANN2,ITEM_LOAD_N2,
                SUBSTRING(CAST(N1 AS VARCHAR(19)), 1, 10) AS DATE_N1,
                SUBSTRING(CAST(N1 AS VARCHAR(19)), 12, 2)||':'||SUBSTRING(CAST(N1 AS VARCHAR(19)), 15, 2)||':'||SUBSTRING(CAST(N1 AS VARCHAR(19)), 18, 2) AS TIME_N1,
                PLANN1,ITEM_LOAD_N1,
                SUBSTRING(CAST(JT AS VARCHAR(19)), 1, 10) AS DATE_JT,
                SUBSTRING(CAST(JT AS VARCHAR(19)), 12, 2)||':'||SUBSTRING(CAST(JT AS VARCHAR(19)), 15, 2)||':'||SUBSTRING(CAST(JT AS VARCHAR(19)), 18, 2)AS TIME_JT,
                PLANJT,ITEM_LOAD_JT
                FROM (
                SELECT MODEL, MTO, KDLOTNO,Q_LOT AS QTYLOT,
                VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') AS WE_ON,
                DATE_N2,TIME_N2,
                TIMESTAMP_FORMAT(CONCAT(CHAR(DATE_N2), CHAR(LPAD((TIME_N2), 6, 0))), 'YYYYMMDD HH24MISS') AS N2,
                ITEM_N2 AS PLANN2,ITEM_LOAD_N2,
                DATE_N1,TIME_N1,
                TIMESTAMP_FORMAT(CONCAT(CHAR(DATE_N1), CHAR(LPAD((TIME_N1), 6, 0))), 'YYYYMMDD HH24MISS') AS N1,
                ITEM_N1 AS PLANN1,ITEM_LOAD_N1,
                DATE_JT,TIME_JT*10000 AS TIME_JT,
                TIMESTAMP_FORMAT(CONCAT(CHAR(DATE_JT), CHAR(LPAD((TIME_JT*10000), 6, 0))), 'YYYYMMDD HH24MISS') AS JT,
                ITEM_JT AS PLANJT,ITEM_LOAD_JT                
                FROM (
                select 
                case when TD2.Model is not null then TD2.Model when TD1.Model is not null then TD1.Model when TD3.Model is not null then TD3.Model end as Model 
                ,case when TD2.MTO is not null then TD2.MTO when TD1.MTO is not null then TD1.MTO when TD3.MTO is not null then TD3.MTO end as MTO 
                ,case when TD2.KDLotNo is not null then TD2.KDLotNo when TD1.KDLotNo is not null then TD1.KDLotNo when TD3.KDLotNo is not null then TD3.KDLotNo end as KDLotNo 
                ,case when TD2.Q_Lot is not null then TD2.Q_Lot when TD1.Q_Lot is not null then TD1.Q_Lot when TD3.Q_Lot is not null then TD3.Q_Lot end as Q_Lot
                ,TD2.Date_N2, TD2.tn2 as Time_N2
                ,TD2.CItem as Item_N2
                ,TD2.QPLN  as Plan_N2
                ,TD2.C_Item_Rcv as Item_Unpack_N2
                ,TD2.Rcv  as Unpack_N2
                ,TD2.C_Item_Load as Item_Load_N2
                ,TD2.Loadd as Load_N2
                --
                ,TD1.Date_N1, TD1.tn1 as Time_N1
                ,TD1.CItem as Item_N1
                ,TD1.QPLN  as Plan_N1
                ,TD1.C_Item_Rcv as Item_Unpack_N1
                ,TD1.Rcv  as Unpack_N1
                ,TD1.C_Item_Load as Item_Load_N1
                ,TD1.Loadd as Load_N1
                --
                ,TD3.Date_JT, TD3.tj as Time_JT
                ,TD3.CItem as Item_JT
                ,TD3.QPLN  as Plan_JT
                ,TD3.C_Item_Rcv as Item_Unpack_JT
                ,TD3.Rcv  as Unpack_JT
                ,TD3.C_Item_Load as Item_Load_JT
                ,TD3.Loadd as Load_JT
                --
                from 
                (
                select 
                 P2.Model, P2.MTO,KDLotNo, ifnull(QL,0) Q_Lot
                ,dn2 as Date_N2, tn2
                ,count(Part) as CItem
                ,sum(ifnull(d3qtym,0)*QL) as QPLN
                ,sum(AC.C_Item) as C_Item_Rcv
                 ,sum(ifnull(AC.rcvqty,0)) as Rcv
                ,sum(AD.C_Item) as C_Item_Load
                ,sum(ifnull(AD.rcvqty,0)) As Loadd 
                --
                 from 
                (
                select d1mdnm as Model,trim(d1mdcd)||trim(d1type) as MTO,d1kdlt as KDLotNo,d1pqty as QL
                ,d1dld2 as dn2,d1dlt2 as tn2,d1dcfp as Part 
                from dc2lib.d3dlvpn
                where d1dld2>=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') as int) AND d1dld2<=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP +1 DAY, 'YYYYMMDD') as int)
                  and (substring(d1proc,1,4)='WE-S') AND D1STPL ='IKD1'
                --
                ) as P2
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                group by dcptno,kdltfr 
                ) as AC
                on AC.dcptno=Part and replace(AC.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                --
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                where splflg= '3' AND splflg<> ' '
                group by dcptno,kdltfr 
                ) as AD
                on AD.dcptno=Part and replace(AD.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                left join 
                (
                select * from dc2lib.d3prtlt
                ) as PL
                on d3mdty=MTO and d3dcpt=Part
                --
                group by P2.Model, P2.MTO,KDLotNo, QL
                ,dn2, tn2
                ) as TD2
                --
                -----------------------
                full outer join
                -----------------------
                (
                select 
                 P2.Model, P2.MTO,KDLotNo, ifnull(QL,0) Q_Lot
                ,dn1 as Date_N1, tn1
                ,count(Part) as CItem
                ,sum(ifnull(d3qtym,0)*QL) as QPLN
                 ,sum(AC.C_Item) as C_Item_Rcv
                 ,sum(ifnull(AC.rcvqty,0)) as Rcv
                ,sum(AD.C_Item) as C_Item_Load
                ,sum(ifnull(AD.rcvqty,0)) As Loadd
                --
                 from 
                (
                select d1mdnm as Model,trim(d1mdcd)||trim(d1type) as MTO,d1kdlt as KDLotNo,d1pqty as QL
                ,d1dld1 as dn1,d1dlt1 as tn1,d1dcfp as Part 
                from dc2lib.d3dlvpn
                where d1dld1>=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') as int) AND d1dld1<=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP + 1 DAY, 'YYYYMMDD') as int)                    
                and (substring(d1proc,1,4)='WE-D' or substring(d1proc,1,2)='IU' or substring(d1proc,1,2)='MC' OR substring(d1proc,1,2)='RE') AND D1STPL ='IKD1'
				union all 
 				select d1mdnm as Model,trim(d1mdcd)||trim(d1type) as MTO,d1kdlt as KDLotNo,d1pqty as QL
                ,d1dld9 as dn1,d1dlt9 as tn1,d1dcfp as Part 
                from dc2lib.d3dlvpn
                where 
                 d1dld9>=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') as int) AND d1dld9<=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP + 1 DAY, 'YYYYMMDD') as int)
                and ( substring(d1proc,1,2)='G9')
                --
                --
                ) as P2
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                group by dcptno,kdltfr 
                ) as AC
                on AC.dcptno=Part and replace(AC.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                --
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                where splflg= '3' AND splflg<> ' '
                group by dcptno,kdltfr 
                ) as AD
                on AD.dcptno=Part and replace(AD.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                left join 
                (
                select * from dc2lib.d3prtlt
                ) as PL
                on d3mdty=MTO and d3dcpt=Part
                --
                group by P2.Model, P2.MTO,KDLotNo, QL
                ,dn1, tn1
                ) as TD1
                on TD1.KDLotNo=TD2.KDLotNo
                ---
                ---
                ---
                -----------------------
                full outer join
                -----------------------
                ---
                (
                select 
                 P2.Model, P2.MTO,KDLotNo, ifnull(QL,0) Q_Lot
                ,dj as Date_JT, tj
                ,count(Part) as CItem
                ,sum(ifnull(d3qtym,0)*QL) as QPLN
                 ,sum(AC.C_Item) as C_Item_Rcv
                 ,sum(ifnull(AC.rcvqty,0)) as Rcv
                ,sum(AD.C_Item) as C_Item_Load
                ,sum(ifnull(AD.rcvqty,0)) As Loadd
                ---
                 from 
                (
                select d1mdnm as Model,trim(d1mdcd)||trim(d1type) as MTO,d1kdlt as KDLotNo,d1pqty as QL
                ,d1dldj as dj,d1dltj as tj,d1dcfp as Part 
                from dc2lib.d3dlvpn
                where d1dldj>=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') as int) AND  d1dldj<=cast(VARCHAR_FORMAT(CURRENT TIMESTAMP + 1 DAY, 'YYYYMMDD') as int) 
                --
                and (substring(d1proc,1,4)='WE-J' ) AND D1STPL ='IKD1'
                --
                --
                ) as P2
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                group by dcptno,kdltfr 
                ) as AC
                on AC.dcptno=Part and replace(AC.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                --
                left join 
                (
                select dcptno,kdltfr,1 as C_Item,sum(rcvqty) as rcvqty 
                from dc2lib.d3rcvrt 
                where splflg= '3' AND splflg<> ' '
                group by dcptno,kdltfr 
                ) as AD
                on AD.dcptno=Part and replace(AD.kdltfr,'-','')=substring(replace(KDLotNo,'-',''),1,2)||substring(replace(KDLotNo,'-',''),5)
                left join 
                (
                select * from dc2lib.d3prtlt
                ) as PL
                on d3mdty=MTO and d3dcpt=Part
                --
                group by P2.Model, P2.MTO,KDLotNo, QL
                ,dj, tj
                ) as TD3
                on TD1.KDLotNo=TD3.KDLotNo
                ) T9
                ORDER BY DATE_N2
                )T10
                ";
                dbConnection.Open();
                var result = dbConnection.Query<IKDMONITORING>(sQuery);
                dbConnection.Close();
                return result;

            }
        }
        public IEnumerable<IKDMONITORING> GetAllN2N1G9JT()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"CALL DC2LIB.DLVIKD";
                dbConnection.Open();
                var result = dbConnection.Query<IKDMONITORING>(sQuery);
                dbConnection.Close();
                return result;

            }
        }
    }
}