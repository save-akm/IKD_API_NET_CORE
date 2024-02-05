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
    public class MNRPTDCCRepository
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

        public IEnumerable<MNRPTDCC> GetMonitoringRPT(string GZONE, int ZONE)
        {
            
            string GZ1 = GZONE;//"C ZONE 1";
            int Z1 = ZONE;//"AF";

            Console.Write(GZ1);
            Console.Write(Z1);
            using (IDbConnection dbConnection = Connection)
            {
                
                            string sQuery = @"
            with A1 as (
            select 
            DCPTNO,PRTDSC,PRTCLR,SPPCDE, PCRQTY,DLVQTY,CPIQTY,PICSCN,DLVSCN,DLVZNE
            ,PICDTE,PICTME,PICTME_TA,PICTME_D,DLVTME_D,PICTME_A,DLVTME_A
            ,case when (PICTME_A>BGB1 and PICTME_A<EDB1 and  TB1<>0 ) then  EDB1
             when (PICTME_A>BGB2 and PICTME_A<EDB2 and  TB2<>0 ) then  EDB2
             when (PICTME_A>BGB3 and PICTME_A<EDB3 and  TB3<>0 ) then  EDB3
             when (PICTME_A>BGB4 and PICTME_A<EDB4 and  TB4<>0 ) then  EDB4
             when (PICTME_A>BGB5 and PICTME_A<EDB5 and  TB5<>0 ) then  EDB5
             when (PICTME_A>BGB6 and PICTME_A<EDB6 and  TB6<>0 ) then  EDB6
             when (PICTME_A>BGB7 and PICTME_A<EDB7 and  TB7<>0 ) then  EDB7
             when (PICTME_A>BGB8 and PICTME_A<EDB8 and  TB8<>0 ) then  EDB8
             when (PICTME_A>BGB9 and PICTME_A<EDB9 and  TB9<>0 ) then  EDB9
            else PICTME_A end as TimeAFBRK
            ,DLVDTE,DLVTME,  KDLTFR, DLVMNT,SPLCSH
            ,BGB1,EDB1,BGB2,EDB2,BGB3,EDB3,BGB4,EDB4,BGB5,EDB5,BGB6,EDB6,BGB7,EDB7,BGB8,EDB8,BGB9,EDB9
            ,TB1,TB2,TB3,TB4,TB5,TB6,TB7,TB8,TB9
            from (
            select 
            DCPTNO,PRTDSC,PRTCLR,SPPCDE, PCRQTY,DLVQTY,CPIQTY,PICSCN,DLVSCN,DLVZNE
            ,PICDTE,PICTME,PICTME_TA,PICTME_D,DLVTME_D,PICTME_A,DLVTME_A
            ,DLVDTE,DLVTME
             ,  KDLTFR, DLVMNT,SPLCSH,BGB1,EDB1,BGB2,EDB2,BGB3,EDB3,BGB4,EDB4,BGB5,EDB5,BGB6,EDB6,BGB7,EDB7,BGB8,EDB8,BGB9,EDB9
            ,case when (INTEGER(TIME(BGB1))=0 and INTEGER(TIME(EDB1))=0) then 0 else 
            TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB1)-TIMESTAMP(BGB1)) )end as TB1
            ,case when (INTEGER(TIME(BGB2))=0 and INTEGER(TIME(EDB2))=0) then 0 else 
            TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB2)-TIMESTAMP(BGB2)) )end as TB2
            ,case when (INTEGER(TIME(BGB3))=0 and INTEGER(TIME(EDB3))=0) then 0 else 
            TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB3)-TIMESTAMP(BGB3)) )end as TB3
            ,case when (INTEGER(TIME(BGB4))=0 and INTEGER(TIME(EDB4))=0) then 0 else 
            TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB4)-TIMESTAMP(BGB4)) )end as TB4
            ,case when (INTEGER(TIME(BGB5))=0 and INTEGER(TIME(EDB5))=0) then 0 else 
            TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB5)-TIMESTAMP(BGB5)) )end as TB5
            ,case when (INTEGER(TIME(BGB6))=0 and INTEGER(TIME(EDB6))=0) then 0 else 
            TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB6)-TIMESTAMP(BGB6)) )end as TB6
            ,case when (INTEGER(TIME(BGB7))=0 and INTEGER(TIME(EDB7))=0) then 0 else 
            TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB7)-TIMESTAMP(BGB7)) )end as TB7
            ,case when (INTEGER(TIME(BGB8))=0 and INTEGER(TIME(EDB8))=0) then 0 else 
            TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB8)-TIMESTAMP(BGB8)) )end as TB8
            ,case when (INTEGER(TIME(BGB9))=0 and INTEGER(TIME(EDB9))=0) then 0 else 
            TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB9)-TIMESTAMP(BGB9)) )end as TB9
            from 
            (
            select DCPTNO,PRTDSC,PRTCLR,SPPCDE, PCRQTY,DLVQTY,CPIQTY,PICSCN,DLVSCN,DLVZNE
            ,PICDTE,PICTME,PICTME_TA,PICTME_D,DLVTME_D,PICTME_A,DLVTME_A, DLVDTE,DLVTME
             ,  KDLTFR, DLVMNT,SPLCSH
            ,case when brkbg1>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg1-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else   TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg1)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB1
            ,case when brked1>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked1-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else  TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked1)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB1
            ,case when brkbg2>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg2-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg2)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB2
            ,case when brked2>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked2-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else  TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked2)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB2
            ,case when brkbg3>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg3-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg3)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB3
            ,case when brked3>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked3-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked3)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB3
            ,case when brkbg4>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg4-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg4)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB4
            ,case when brked4>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked4-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked4)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB4
            ,case when brkbg5>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg5-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg5)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB5
            ,case when brked5>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked5-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else  TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked5)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB5
            ,case when brkbg6>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg6-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg6)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB6
            ,case when brked6>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked6-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else  TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked6)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB6
            ,case when brkbg7>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg7-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg7)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB7
            ,case when brked7>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked7-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else  TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked7)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB7
            ,case when brkbg8>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg8-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg8)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB8
            ,case when brked8>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked8-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked8)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB8
            ,case when brkbg9>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg9-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg9)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB9
            ,case when brked9>2400 then 
            TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked9-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
            else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked9)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB9
             from 
            (
            SELECT 
                        DCPTNO,PRTDSC,PRTCLR,SPPCDE
            ,CASE 
                        WHEN PCRQTY = '0' AND  CPIQTY = '0' THEN '0' 
                        WHEN PCRQTY = '0' AND  CPIQTY > '0' THEN DLVQTY-CPIQTY  
                        WHEN PCRQTY > '0' AND  CPIQTY = '0' THEN PCRQTY
                        WHEN PCRQTY > '0' AND  CPIQTY > '0' THEN DLVQTY-CPIQTY 
                        END AS PCRQTY,
                        PCRQTY||'/'||DLVQTY AS DLVQTY,CPIQTY,PICSCN
            ,DLVSCN,DLVZNE,PICDTE,PICTME
            ,INTEGER(TIME(TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE), CHAR(LPAD(PICTME, 6, 0))), 'YYYYMMDD HH24MISS') + (0 +0) MINUTES))/100 as PICTME_TA
            ,TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE), CHAR(LPAD(PICTME, 6, 0))), 'YYYYMMDD HH24MISS')AS PICTME_D
            ,TIMESTAMP_FORMAT(CONCAT(CHAR(DLVDTE), CHAR(LPAD(DLVTME, 6, 0))), 'YYYYMMDD HH24MISS') AS DLVTME_D
            ,TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE), CHAR(LPAD(PICTME, 6, 0))), 'YYYYMMDD HH24MISS') + (0 +0) MINUTES  AS PICTME_A
            ,TIMESTAMP_FORMAT(CONCAT(CHAR(DLVDTE), CHAR(LPAD(DLVTME, 6, 0))), 'YYYYMMDD HH24MISS') + (0 +0) MINUTES  AS DLVTME_A
             , DLVDTE,DLVTME   ,  KDLTFR, DLVMNT,SPLCSH ,LINENO
                        FROM 
            (
            select    
            CASE
                        WHEN LENGTH(TRIM(DCPTNO)) = 13 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,3) || '-' || SUBSTRING(TRIM(DCPTNO),10,4)
                        WHEN LENGTH(TRIM(DCPTNO)) = 15 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,3) || '-' || SUBSTRING(TRIM(DCPTNO),10,4) || '-' || SUBSTRING(TRIM(DCPTNO),14,2)
                        WHEN LENGTH(TRIM(DCPTNO)) = 12 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,5) || '-' || SUBSTRING(TRIM(DCPTNO),11,2) 
                        WHEN LENGTH(TRIM(DCPTNO)) = 10 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),5,5) 
                        ELSE 'Error'
                        END AS DCPTNO
            ,PRTDSC
            , CASE 
                        WHEN LENGTH(trim(PRTCLR)) = 11 THEN SUBSTRING(PRTCLR, 1, 2) || '-'|| SUBSTRING(PRTCLR, 3,9)
                        ELSE ''
                        END AS PRTCLR
              ,DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT,SPLCSH  
            ,LINENO , SPPCDE,PICSCN,DLVSCN,sum(PCRQTY) AS PCRQTY, sum(CPIQTY) AS CPIQTY,sum(DLVQTY) AS DLVQTY 
             from 
            (
            select  
             DCPTNO,PRTDSC, PRTCLR, SPPCDE, IFNULL(PCRQTY,0) AS PCRQTY,CPIQTY, DLVQTY
            , CASE WHEN PCRQTY = DLVQTY THEN '1' ELSE PICSCN END AS PICSCN
            ,DLVSCN,  DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT,SPLCSH  ,LINENO 
            FROM DC2LIB.D3DIWEKJ1 
            WHERE DLVMNT  = '" + GZ1 + @"'
            --AND PICDTE = INTEGER(DATE(CURRENT TIMESTAMP - (0 +0) MINUTES))
           -- AND INTEGER(PICTME/10000) = INTEGER(TIME(CURRENT TIMESTAMP - (0 +0) MINUTES))/10000 
            union all
            select 
            DCPTNO
            ,PRTDSC
            , PRTCLR
            , SPPCDE
            , IFNULL(PCRQTY,0) AS PCRQTY
            , CPIQTY
            , DLVQTY
            , CASE WHEN PCRQTY = DLVQTY THEN '1' ELSE PICSCN END AS PICSCN
            ,DLVSCN
            ,  DLVZNE
            , DVPLDT
            , DVPLTM
            , KDLTNO
            , PICDTE
            , PICTME
            ,DLVMNT 
            ,SPLCSH 
             ,'HATC034AF' as LINENO
            FROM DC2LIB.D3DISCIJ1 
            WHERE DLVMNT  = '" + GZ1 + @"'
            --AND PICDTE = INTEGER(DATE(CURRENT TIMESTAMP - (0 +0) MINUTES)) 
            --AND INTEGER(PICTME/10000) = INTEGER(TIME(CURRENT TIMESTAMP - (0 +0) MINUTES))/10000 
            union all 
            select 
            L1DCPN
            ,L1PTDS
            , L1PTCL
            , L1SPCD
            , IFNULL(PCRQTY,0) AS PCRQTY
            , L1DQTY
            , CPIQTY
            ,CASE WHEN PCRQTY = L1DQTY THEN '1' ELSE PICSCN END AS PICSCN
            ,DLVSCN
            ,  DLVZNE
            , DLVDTE
            , DLVTME
            , KDLTFR
            , PICDTE
            , PICTME
            ,DLVMNT
             ,SPLCSH   
            ,LINENO
             FROM DC2LIB.D3DILOTJ1 
             WHERE DLVMNT = '" + GZ1 + @"'
            --AND PICDTE = INTEGER(DATE(CURRENT TIMESTAMP - (0 +0) MINUTES)) 
            --AND INTEGER(PICTME/10000) = INTEGER(TIME(CURRENT TIMESTAMP - (0 +0) MINUTES))/10000 
            ) as T1
            WHERE
                        (PICDTE = INTEGER(DATE(CURRENT TIMESTAMP - (0 +" + Z1 + @") MINUTES)) AND  T1.PICTME>=80000 )OR 
                        (PICDTE = INTEGER(DATE(CURRENT TIMESTAMP + (1440 -" + Z1 + @") MINUTES)) AND   T1.PICTME<=75959 )
            group by CASE
                        WHEN LENGTH(TRIM(DCPTNO)) = 13 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,3) || '-' || SUBSTRING(TRIM(DCPTNO),10,4)
                        WHEN LENGTH(TRIM(DCPTNO)) = 15 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,3) || '-' || SUBSTRING(TRIM(DCPTNO),10,4) || '-' || SUBSTRING(TRIM(DCPTNO),14,2)
                        WHEN LENGTH(TRIM(DCPTNO)) = 12 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,5) || '-' || SUBSTRING(TRIM(DCPTNO),11,2) 
                        WHEN LENGTH(TRIM(DCPTNO)) = 10 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),5,5) 
                        ELSE 'Error'
                        END 
            --
            ,PRTDSC
            , CASE 
                        WHEN LENGTH(trim(PRTCLR)) = 11 THEN SUBSTRING(PRTCLR, 1, 2) || '-'|| SUBSTRING(PRTCLR, 3,9)
                        ELSE ''
                        END 
             ,DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT,SPLCSH  
            ,LINENO 
            , SPPCDE
            ,PICSCN
            ,DLVSCN
            --
            ) as T2
            WHERE PICSCN <> '1'
            ) as PIC
			            left join
			            (
			            select * from dc2lib.D3CLNDR
			            ) as CLN
			            on CLN.LINENO=PIC.LINENO and CLN.prddte=PICDTE
			            left join
			            (
			            select 
            --
            --
            * from dc2lib.D3WKGTM
			            ) as WKG
			            on CLN.LINENO=WKG.LINENO and CLN.prdwtp=WKG.prdwtp
            ) as T1	
            ) as T2		
            --
            order by PICDTE,PICTME
            ) 
            --
            ------------------------------------------
            select     
            DCPTNO,PRTCLR,SPLCSH,DLVQTY,DLVZNE
            --,PICTME_D
            ,NewTimePic AS PICTME,KDLTFR
            from 
            (
            select  
            DCPTNO,PRTCLR,SPLCSH,DLVQTY,DLVZNE,KDLTFR
            ,PICTME_D
            ,PICTME_A 
            ,PVNewPic
            ,TIMEAFBRK
            ,NextPic
            ,case when PICTME_A <> TIMEAFBRK then TIMEAFBRK
            when   (PICTME_A=TIMEAFBRK and PVNewPic<>'0001-01-01 00:00:00.000000' and TIMESTAMPDIFF(4, CHAR(TIMESTAMP(TIMEAFBRK) - TIMESTAMP(PVNewPic)))<NewDiff
            ) then TIMESTAMP(PVNewPic)+NewDiff MINUTES else TIMEAFBRK end NewTimePic
            ,CurDiff,NewDiff
            --
             from (
            select --C1.*,C2.*
            C1.DCPTNO,C1.PRTCLR,C1.SPLCSH,C1.DLVQTY,C1.DLVZNE,C1.KDLTFR
            ,C1.PICTME_D
            ,C1.PICTME_A 
            ,C1.TIMEAFBRK
            ,C2.CurDiff
            ,case when (CurDiff>100000 )  then 0 
            when (PVPic<BGB1 and C1.PICTME_D>EDB1 and TB1<>0) then  CurDiff-TB1  
            when (PVPic<BGB2 and C1.PICTME_D>EDB2 and TB2<>0) then  CurDiff-TB2
            when (PVPic<BGB3 and C1.PICTME_D>EDB3 and TB3<>0) then  CurDiff-TB3
            when (PVPic<BGB4 and C1.PICTME_D>EDB4 and TB4<>0) then  CurDiff-TB4
            when (PVPic<BGB5 and C1.PICTME_D>EDB5 and TB5<>0) then  CurDiff-TB5
            when (PVPic<BGB6 and C1.PICTME_D>EDB6 and TB6<>0) then  CurDiff-TB6
            when (PVPic<BGB7 and C1.PICTME_D>EDB7 and TB7<>0) then  CurDiff-TB7
            when (PVPic<BGB8 and C1.PICTME_D>EDB8 and TB8<>0) then  CurDiff-TB8
            when (PVPic<BGB9 and C1.PICTME_D>EDB9 and TB9<>0) then  CurDiff-TB9
            else CurDiff end as  NewDiff
            ,NextPic,PVNewPic
             from 
            (
            select * from A1
            ) as C1
            left join
            (
            select KDLTFR,PICTME_D
            ,PICTME_A,TIMEAFBRK
            ,PVPic ,NextPic ,PVNewPic
            ,TIMESTAMPDIFF(4, CHAR(TIMESTAMP(PICTME_D) - TIMESTAMP(PVPic)))  as  CurDiff
            from (
            select KDLTFR,PICTME_D,PICTME_A,TIMEAFBRK
            ,case when PVNewPic is null then '0001-01-01 00:00:00.000000' else PVNewPic end as PVNewPic
            ,case when PVPic is null then '0001-01-01 00:00:00.000000' else PVPic end as PVPic  
            ,case when NextPic is null then '0001-01-01 00:00:00.000000' else NextPic end as NextPic
            --
            from (
            select KDLTFR,PICTME_D,PICTME_A,TIMEAFBRK
            ,LAG(PICTME_D,1) OVER(ORDER BY PICTME_D)   AS PVPic
            ,LEAD(TIMEAFBRK,1) OVER(ORDER BY TIMEAFBRK)  AS NextPic
            ,LAG(TIMEAFBRK,1) OVER(ORDER BY TIMEAFBRK)   AS PVNewPic
            --
            from (
            select  KDLTFR,PICTME_D,PICTME_A,TIMEAFBRK  from A1
            group by KDLTFR,PICTME_D,PICTME_A,TIMEAFBRK
            ) as B1
            ) as B2
            ) as B3
            ) as C2
            on C1.KDLTFR=C2.KDLTFR
            ) as B4
            ) as B5
            --WHERE 
            --INTEGER(DATE(NewTimePic)) = INTEGER(DATE(CURRENT TIMESTAMP- (0) MINUTES))  AND
            --INTEGER(TIME(NewTimePic))/10000 = INTEGER(TIME(CURRENT TIMESTAMP - (0) MINUTES))/10000 
            order by PICTME_D
            ";

                    dbConnection.Open();          
                    var result = dbConnection.Query<MNRPTDCC>(sQuery);
                    Console.Write("NO 2" + result);
                    dbConnection.Close();
                    return result;
      
            }
        }
        public IEnumerable<MNRPTDCC> GetMonitoringRPTDLV(string GZONE, string ZONE)
        {
            string GZ1 = GZONE;//"C ZONE 1";
            string Z1 = ZONE;//"AF";

            Console.Write(GZ1);
            Console.Write(Z1);
            using (IDbConnection dbConnection = Connection)
            {

                string sQuery = @"
                with A1 as (
                select 
                DCPTNO,PRTDSC,PRTCLR,SPPCDE, PCRQTY,DLVQTY,CPIQTY,PICSCN,DLVSCN,DLVZNE
                ,PICDTE,PICTME,PICTME_TA,PICTME_D,DLVTME_D,PICTME_A,DLVTME_A
                ,case when (PICTME_A>BGB1 and PICTME_A<EDB1 and  TB1<>0 ) then  EDB1
                 when (PICTME_A>BGB2 and PICTME_A<EDB2 and  TB2<>0 ) then  EDB2
                 when (PICTME_A>BGB3 and PICTME_A<EDB3 and  TB3<>0 ) then  EDB3
                 when (PICTME_A>BGB4 and PICTME_A<EDB4 and  TB4<>0 ) then  EDB4
                 when (PICTME_A>BGB5 and PICTME_A<EDB5 and  TB5<>0 ) then  EDB5
                 when (PICTME_A>BGB6 and PICTME_A<EDB6 and  TB6<>0 ) then  EDB6
                 when (PICTME_A>BGB7 and PICTME_A<EDB7 and  TB7<>0 ) then  EDB7
                 when (PICTME_A>BGB8 and PICTME_A<EDB8 and  TB8<>0 ) then  EDB8
                 when (PICTME_A>BGB9 and PICTME_A<EDB9 and  TB9<>0 ) then  EDB9
                else PICTME_A end as TimeAFBRK
                ,DLVDTE,DLVTME,  KDLTFR, DLVMNT,SPLCSH
                ,BGB1,EDB1,BGB2,EDB2,BGB3,EDB3,BGB4,EDB4,BGB5,EDB5,BGB6,EDB6,BGB7,EDB7,BGB8,EDB8,BGB9,EDB9
                ,TB1,TB2,TB3,TB4,TB5,TB6,TB7,TB8,TB9
                from (
                select 
                DCPTNO,PRTDSC,PRTCLR,SPPCDE, PCRQTY,DLVQTY,CPIQTY,PICSCN,DLVSCN,DLVZNE
                ,PICDTE,PICTME,PICTME_TA,PICTME_D,DLVTME_D,PICTME_A,DLVTME_A
                ,DLVDTE,DLVTME
                ,KDLTFR, DLVMNT,SPLCSH,BGB1,EDB1,BGB2,EDB2,BGB3,EDB3,BGB4,EDB4,BGB5,EDB5,BGB6,EDB6,BGB7,EDB7,BGB8,EDB8,BGB9,EDB9
                ,case when (INTEGER(TIME(BGB1))=0 and INTEGER(TIME(EDB1))=0) then 0 else 
                TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB1)-TIMESTAMP(BGB1)) )end as TB1
                ,case when (INTEGER(TIME(BGB2))=0 and INTEGER(TIME(EDB2))=0) then 0 else 
                TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB2)-TIMESTAMP(BGB2)) )end as TB2
                ,case when (INTEGER(TIME(BGB3))=0 and INTEGER(TIME(EDB3))=0) then 0 else 
                TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB3)-TIMESTAMP(BGB3)) )end as TB3
                ,case when (INTEGER(TIME(BGB4))=0 and INTEGER(TIME(EDB4))=0) then 0 else 
                TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB4)-TIMESTAMP(BGB4)) )end as TB4
                ,case when (INTEGER(TIME(BGB5))=0 and INTEGER(TIME(EDB5))=0) then 0 else 
                TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB5)-TIMESTAMP(BGB5)) )end as TB5
                ,case when (INTEGER(TIME(BGB6))=0 and INTEGER(TIME(EDB6))=0) then 0 else 
                TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB6)-TIMESTAMP(BGB6)) )end as TB6
                ,case when (INTEGER(TIME(BGB7))=0 and INTEGER(TIME(EDB7))=0) then 0 else 
                TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB7)-TIMESTAMP(BGB7)) )end as TB7
                ,case when (INTEGER(TIME(BGB8))=0 and INTEGER(TIME(EDB8))=0) then 0 else 
                TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB8)-TIMESTAMP(BGB8)) )end as TB8
                ,case when (INTEGER(TIME(BGB9))=0 and INTEGER(TIME(EDB9))=0) then 0 else 
                TIMESTAMPDIFF(4, CHAR(TIMESTAMP(EDB9)-TIMESTAMP(BGB9)) )end as TB9
                from 
                (
                select DCPTNO,PRTDSC,PRTCLR,SPPCDE, PCRQTY,DLVQTY,CPIQTY,PICSCN,DLVSCN,DLVZNE
                ,PICDTE,PICTME,PICTME_TA,PICTME_D,DLVTME_D,PICTME_A,DLVTME_A, DLVDTE,DLVTME
                 ,  KDLTFR, DLVMNT,SPLCSH
                ,case when brkbg1>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg1-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else   TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg1)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB1
                ,case when brked1>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked1-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else  TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked1)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB1
                ,case when brkbg2>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg2-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg2)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB2
                ,case when brked2>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked2-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else  TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked2)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB2
                ,case when brkbg3>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg3-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg3)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB3
                ,case when brked3>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked3-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked3)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB3
                ,case when brkbg4>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg4-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg4)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB4
                ,case when brked4>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked4-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked4)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB4
                ,case when brkbg5>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg5-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg5)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB5
                ,case when brked5>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked5-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else  TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked5)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB5
                ,case when brkbg6>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg6-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg6)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB6
                ,case when brked6>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked6-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else  TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked6)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB6
                ,case when brkbg7>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg7-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg7)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB7
                ,case when brked7>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked7-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else  TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked7)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB7
                ,case when brkbg8>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg8-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg8)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB8
                ,case when brked8>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked8-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked8)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB8
                ,case when brkbg9>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg9-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brkbg9)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as BGB9
                ,case when brked9>2400 then 
                TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked9-2400)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') + 1 DAY 
                else TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE ), CHAR(LPAD((brked9)*(10000/100), 6, 0))), 'YYYYMMDD HH24MISS') end as EDB9
                 from 
                (
                SELECT 
                            DCPTNO,PRTDSC,PRTCLR,SPPCDE
                ,CASE 
                            WHEN PCRQTY = '0' AND  CPIQTY = '0' THEN '0' 
                            WHEN PCRQTY = '0' AND  CPIQTY > '0' THEN DLVQTY-CPIQTY  
                            WHEN PCRQTY > '0' AND  CPIQTY = '0' THEN PCRQTY
                            WHEN PCRQTY > '0' AND  CPIQTY > '0' THEN DLVQTY-CPIQTY 
                            END AS PCRQTY,
                            PCRQTY||'/'||DLVQTY AS DLVQTY,CPIQTY,PICSCN
                ,DLVSCN,DLVZNE,PICDTE,PICTME
                ,INTEGER(TIME(TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE), CHAR(LPAD(PICTME, 6, 0))), 'YYYYMMDD HH24MISS') + (0 +(SELECT D3NUMBE FROM DC2LIB.D3AJTME WHERE D3ZONEM ='" + Z1 + @"')) MINUTES))/100 as PICTME_TA
                ,TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE), CHAR(LPAD(PICTME, 6, 0))), 'YYYYMMDD HH24MISS')AS PICTME_D
                ,TIMESTAMP_FORMAT(CONCAT(CHAR(DLVDTE), CHAR(LPAD(DLVTME, 6, 0))), 'YYYYMMDD HH24MISS') AS DLVTME_D
                ,TIMESTAMP_FORMAT(CONCAT(CHAR(PICDTE), CHAR(LPAD(PICTME, 6, 0))), 'YYYYMMDD HH24MISS') + (0 +(SELECT D3NUMBE FROM DC2LIB.D3AJTME WHERE D3ZONEM ='" + Z1 + @"')) MINUTES  AS PICTME_A
                ,TIMESTAMP_FORMAT(CONCAT(CHAR(DLVDTE), CHAR(LPAD(DLVTME, 6, 0))), 'YYYYMMDD HH24MISS') + (0 +(SELECT D3NUMBE FROM DC2LIB.D3AJTME WHERE D3ZONEM ='" + Z1 + @"')) MINUTES  AS DLVTME_A
                 , DLVDTE,DLVTME   ,  KDLTFR, DLVMNT,SPLCSH ,LINENO
                            FROM 
                (
                select    
                CASE
                            WHEN LENGTH(TRIM(DCPTNO)) = 13 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,3) || '-' || SUBSTRING(TRIM(DCPTNO),10,4)
                            WHEN LENGTH(TRIM(DCPTNO)) = 15 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,3) || '-' || SUBSTRING(TRIM(DCPTNO),10,4) || '-' || SUBSTRING(TRIM(DCPTNO),14,2)
                            WHEN LENGTH(TRIM(DCPTNO)) = 12 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,5) || '-' || SUBSTRING(TRIM(DCPTNO),11,2) 
                            WHEN LENGTH(TRIM(DCPTNO)) = 10 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),5,5) 
                            ELSE 'Error'
                            END AS DCPTNO
                ,PRTDSC
                , CASE 
                            WHEN LENGTH(trim(PRTCLR)) = 11 THEN SUBSTRING(PRTCLR, 1, 2) || '-'|| SUBSTRING(PRTCLR, 3,9)
                            ELSE ''
                            END AS PRTCLR
                  ,DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT,SPLCSH  
                ,LINENO , SPPCDE,PICSCN,DLVSCN,sum(PCRQTY) AS PCRQTY, sum(CPIQTY) AS CPIQTY,sum(DLVQTY) AS DLVQTY 
                 from 
                (
                select  
                 DCPTNO,PRTDSC, PRTCLR, SPPCDE, IFNULL(PCRQTY,0) AS PCRQTY,CPIQTY, DLVQTY
                , CASE WHEN PCRQTY = DLVQTY THEN '1' ELSE PICSCN END AS PICSCN
                ,DLVSCN,  DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT,SPLCSH  ,LINENO 
                FROM DC2LIB.D3DIWEKJ1 
                WHERE DLVMNT = (SELECT DISTINCT(DLVMNT) FROM DC2LIB.D3ZNEMB WHERE DLVMNT = '" + GZ1 + @"')
                --AND PICDTE = INTEGER(DATE(CURRENT TIMESTAMP - (0 +(SELECT D3NUMBE FROM DC2LIB.D3AJTME WHERE D3ZONEM = '" + Z1 + @"')) MINUTES))
                --AND INTEGER(PICTME/10000) = INTEGER(TIME(CURRENT TIMESTAMP - (0 +180) MINUTES))/10000 
                union all
                select 
                DCPTNO
                ,PRTDSC
                , PRTCLR
                , SPPCDE
                , IFNULL(PCRQTY,0) AS PCRQTY
                , CPIQTY
                , DLVQTY
                , CASE WHEN PCRQTY = DLVQTY THEN '1' ELSE PICSCN END AS PICSCN
                ,DLVSCN
                ,  DLVZNE
                , DVPLDT
                , DVPLTM
                , KDLTNO
                , PICDTE
                , PICTME
                ,DLVMNT 
                ,SPLCSH 
                 ,'HATC034AF' as LINENO
                FROM DC2LIB.D3DISCIJ1 
                WHERE DLVMNT = (SELECT DISTINCT(DLVMNT) FROM DC2LIB.D3ZNEMB WHERE DLVMNT = '" + GZ1 + @"')
               --AND PICDTE = INTEGER(DATE(CURRENT TIMESTAMP - (0 +(SELECT D3NUMBE FROM DC2LIB.D3AJTME WHERE D3ZONEM = '" + Z1 + @"')) MINUTES)) 
                --AND INTEGER(PICTME/10000) = INTEGER(TIME(CURRENT TIMESTAMP - (0 +180) MINUTES))/10000 
                union all 
                select 
                L1DCPN
                ,L1PTDS
                , L1PTCL
                , L1SPCD
                , IFNULL(PCRQTY,0) AS PCRQTY
                , L1DQTY
                , CPIQTY
                ,CASE WHEN PCRQTY = L1DQTY THEN '1' ELSE PICSCN END AS PICSCN
                ,DLVSCN
                ,  DLVZNE
                , DLVDTE
                , DLVTME
                , KDLTFR
                , PICDTE
                , PICTME
                ,DLVMNT
                 ,SPLCSH   
                ,LINENO
                 FROM DC2LIB.D3DILOTJ1 
                 WHERE DLVMNT = (SELECT DISTINCT(DLVMNT) FROM DC2LIB.D3ZNEMB WHERE DLVMNT = '" + GZ1 + @"')
                --AND PICDTE = INTEGER(DATE(CURRENT TIMESTAMP - (0 +(SELECT D3NUMBE FROM DC2LIB.D3AJTME WHERE D3ZONEM = '" + Z1 + @"')) MINUTES)) 
                --AND INTEGER(PICTME/10000) = INTEGER(TIME(CURRENT TIMESTAMP - (0 +180) MINUTES))/10000 
                ) as T1
                WHERE
                            (PICDTE = INTEGER(DATE(CURRENT TIMESTAMP - (0 +(SELECT D3NUMBE FROM DC2LIB.D3AJTME WHERE D3ZONEM = '" + Z1 + @"')) MINUTES)) AND  T1.PICTME>=80000 )OR 
                            (PICDTE = INTEGER(DATE(CURRENT TIMESTAMP + (1440 -(SELECT D3NUMBE FROM DC2LIB.D3AJTME WHERE D3ZONEM ='" + Z1 + @"')) MINUTES)) AND   T1.PICTME<=75959 )
                group by CASE
                            WHEN LENGTH(TRIM(DCPTNO)) = 13 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,3) || '-' || SUBSTRING(TRIM(DCPTNO),10,4)
                            WHEN LENGTH(TRIM(DCPTNO)) = 15 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,3) || '-' || SUBSTRING(TRIM(DCPTNO),10,4) || '-' || SUBSTRING(TRIM(DCPTNO),14,2)
                            WHEN LENGTH(TRIM(DCPTNO)) = 12 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),6,5) || '-' || SUBSTRING(TRIM(DCPTNO),11,2) 
                            WHEN LENGTH(TRIM(DCPTNO)) = 10 THEN SUBSTRING(TRIM(DCPTNO),1,5) || '-' || SUBSTRING(TRIM(DCPTNO),5,5) 
                            ELSE 'Error'
                            END 
                --
                ,PRTDSC
                , CASE 
                            WHEN LENGTH(trim(PRTCLR)) = 11 THEN SUBSTRING(PRTCLR, 1, 2) || '-'|| SUBSTRING(PRTCLR, 3,9)
                            ELSE ''
                            END 
                 ,DLVZNE, DLVDTE, DLVTME, KDLTFR, PICDTE, PICTME,DLVMNT,SPLCSH  
                ,LINENO 
                , SPPCDE
                ,PICSCN
                ,DLVSCN
                --
                ) as T2
                WHERE PICSCN <> '1'
                ) as PIC
                   left join
                   (
                   select * from dc2lib.D3CLNDR
                   ) as CLN
                   on CLN.LINENO=PIC.LINENO and CLN.prddte=PICDTE
                   left join
                   (
                   select 
                --
                --
                * from dc2lib.D3WKGTM
                   ) as WKG
                   on CLN.LINENO=WKG.LINENO and CLN.prdwtp=WKG.prdwtp
                ) as T1	
                ) as T2		
                --
                order by PICDTE,PICTME
                ) 
                --
                ------------------------------------------
                select     
                DCPTNO,PRTCLR,SPLCSH,DLVQTY,DLVZNE,KDLTFR,
                --,PICTME_D
                --,NewTimePic AS PICTME,
                            SUBSTRING(CHAR(NewTimePic),1,10) AS PICDTE, 
                            SUBSTRING(CHAR(NewTimePic),12,8) AS PICTME
                from 
                (
                select  
                DCPTNO,PRTCLR,SPLCSH,DLVQTY,DLVZNE,KDLTFR
                ,PICTME_D
                ,PICTME_A 
                ,PVNewPic
                ,TIMEAFBRK
                ,NextPic
                ,case when PICTME_A <> TIMEAFBRK then TIMEAFBRK
                when   (PICTME_A=TIMEAFBRK and PVNewPic<>'0001-01-01 00:00:00.000000' and TIMESTAMPDIFF(4, CHAR(TIMESTAMP(TIMEAFBRK) - TIMESTAMP(PVNewPic)))<NewDiff
                ) then TIMESTAMP(PVNewPic)+NewDiff MINUTES else TIMEAFBRK end NewTimePic
                ,CurDiff,NewDiff
                --
                 from (
                select --C1.*,C2.*
                C1.DCPTNO,C1.PRTCLR,C1.SPLCSH,C1.DLVQTY,C1.DLVZNE,C1.KDLTFR
                ,C1.PICTME_D
                ,C1.PICTME_A 
                ,C1.TIMEAFBRK
                ,C2.CurDiff
                ,case when (CurDiff>100000 )  then 0 
                when (PVPic<BGB1 and C1.PICTME_D>EDB1 and TB1<>0) then  CurDiff-TB1  
                when (PVPic<BGB2 and C1.PICTME_D>EDB2 and TB2<>0) then  CurDiff-TB2
                when (PVPic<BGB3 and C1.PICTME_D>EDB3 and TB3<>0) then  CurDiff-TB3
                when (PVPic<BGB4 and C1.PICTME_D>EDB4 and TB4<>0) then  CurDiff-TB4
                when (PVPic<BGB5 and C1.PICTME_D>EDB5 and TB5<>0) then  CurDiff-TB5
                when (PVPic<BGB6 and C1.PICTME_D>EDB6 and TB6<>0) then  CurDiff-TB6
                when (PVPic<BGB7 and C1.PICTME_D>EDB7 and TB7<>0) then  CurDiff-TB7
                when (PVPic<BGB8 and C1.PICTME_D>EDB8 and TB8<>0) then  CurDiff-TB8
                when (PVPic<BGB9 and C1.PICTME_D>EDB9 and TB9<>0) then  CurDiff-TB9
                else CurDiff end as  NewDiff
                ,NextPic,PVNewPic
                 from 
                (
                select * from A1
                ) as C1
                left join
                (
                select KDLTFR,PICTME_D
                ,PICTME_A,TIMEAFBRK
                ,PVPic ,NextPic ,PVNewPic
                ,TIMESTAMPDIFF(4, CHAR(TIMESTAMP(PICTME_D) - TIMESTAMP(PVPic)))  as  CurDiff
                from (
                select KDLTFR,PICTME_D,PICTME_A,TIMEAFBRK
                ,case when PVNewPic is null then '0001-01-01 00:00:00.000000' else PVNewPic end as PVNewPic
                ,case when PVPic is null then '0001-01-01 00:00:00.000000' else PVPic end as PVPic  
                ,case when NextPic is null then '0001-01-01 00:00:00.000000' else NextPic end as NextPic
                --
                from (
                select KDLTFR,PICTME_D,PICTME_A,TIMEAFBRK
                ,LAG(PICTME_D,1) OVER(ORDER BY PICTME_D)   AS PVPic
                ,LEAD(TIMEAFBRK,1) OVER(ORDER BY TIMEAFBRK)  AS NextPic
                ,LAG(TIMEAFBRK,1) OVER(ORDER BY TIMEAFBRK)   AS PVNewPic
                --
                from (
                select  KDLTFR,PICTME_D,PICTME_A,TIMEAFBRK  from A1
                group by KDLTFR,PICTME_D,PICTME_A,TIMEAFBRK
                ) as B1
                ) as B2
                ) as B3
                ) as C2
                on C1.KDLTFR=C2.KDLTFR
                ) as B4
                ) as B5
                order by PICDTE, PICTME ASC";
                //string innerString = st.ToString();
                Console.Write("NO 1" + sQuery);
                Console.Write(GZ1);
                Console.Write(Z1);
                //Console.WriteLine(GZ1.ToString());
                dbConnection.Open();
                var result = dbConnection.Query<MNRPTDCC>(sQuery);
                Console.Write("NO 2" + result);
                dbConnection.Close();
                return result;
            }
        }


    }

}