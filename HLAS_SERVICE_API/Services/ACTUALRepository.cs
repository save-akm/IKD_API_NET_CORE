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

    public class ACTUALRepository
    {
        private string AppConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["Secret"];
        public IDbConnection Connection
        {
            get
            {
                return new iDB2Connection(AppConnection);
                //return new SqlConnection(AppConnection);
            }
        }

		public IEnumerable<ACTUAL> GetActualReceiveBetween(string T1PLDT1, string T1PLDT2)
		{
			using (IDbConnection dbConnection = Connection)
			{
				StringBuilder sQuery = new StringBuilder();
				sQuery.AppendFormat(@"SELECT
								PART_NO AS T1PTNO,
								PART_NAME AS T1PTNM,
								MODL AS T1MODE,
								PART_TYPE AS T1TYPE,
								PLN_QTY AS T1PLQT,
								STK_ADD AS T1STAD,
								IFNULL(RCV_QTY,0) AS T1STQT,
								(PLN_QTY - IFNULL(ACM,0)) AS T1DFQT,NOs,NOstext
								FROM (
								SELECT 
								PART_NO 
								,PART_NAME
								,MODL
								,PART_TYPE
								,PLN_QTY
								,STK_ADD
								,RCV_QTY
								,SUM(SUM(RCV_QTY)) OVER (PARTITION BY PART_NO||PART_TYPE ORDER BY PART_NO,PART_NAME,MODL,PART_TYPE,STK_ADD) AS ACM,
								ROW_NUMBER() OVER (PARTITION BY PART_NO || PART_TYPE ORDER BY PART_NO,PART_NAME,MODL,PART_TYPE,STK_ADD DESC) AS NOs,
								ROW_NUMBER() OVER (PARTITION BY PART_NO || PART_TYPE ORDER BY PART_NO,PART_NAME,MODL,PART_TYPE,STK_ADD ASC) AS NOstext
								FROM (
								SELECT T1PTNO AS PART_NO,T1PTNM AS PART_NAME,T1MODL AS MODL,T1TYPE AS PART_TYPE,SUM(T1PLQT) AS PLN_QTY FROM DCPLIB.DCT001
								WHERE T1PLDT>='{0}' AND T1PLDT<='{1}'
								GROUP BY T1PTNO ,T1PTNM ,T1TYPE,T1MODL
								) AS T1
								FULL OUTER JOIN
								(
								SELECT T3PTNO,T3PTTP,T3STAD AS STK_ADD,SUM(T3STQT) AS RCV_QTY FROM DCPLIB.DCT003
								WHERE T3CTDT>='{0}' AND T3CTDT<='{1}'
								GROUP BY T3PTNO,T3PTTP,T3STAD
								) AS T3
								ON PART_NO=T3PTNO AND PART_TYPE=T3PTTP
								GROUP BY PART_NO,PART_NAME,MODL,PART_TYPE,PLN_QTY,STK_ADD,RCV_QTY
								) 
								ORDER BY  PART_NO,PART_NAME,MODL,PART_TYPE
								", T1PLDT1, T1PLDT2);
				dbConnection.Open();
				var result = dbConnection.Query<ACTUAL>(sQuery.ToString(), new { });
				dbConnection.Close();
				return result;
			}
		}
		public IEnumerable<ACTUAL> GetActualReceive(string T1PLDT1, string T1PLDT2)
        {
			//WHERE RECEIVE_DATE >= @T1PLDT1 AND RECEIVE_DATE <= @T1PLDT2
			using (IDbConnection dbConnection = Connection)
            {
                //string sQuery = @"SELECT * FROM DCPLIB.DCT004 WHERE T4TNCD= 'RCV' AND T4TNDT = @T4TNDT AND T4TYOP = @T4TYOP";
                string sQuery = @"SELECT
								PART_NO AS T1PTNO,
								PART_NAME AS T1PTNM,
								MODL AS T1MODE,
								PART_TYPE AS T1TYPE,
								PLN_QTY AS T1PLQT,
								STK_ADD AS T1STAD,
								IFNULL(RCV_QTY,0) AS T1STQT,
								(PLN_QTY - IFNULL(ACM,0)) AS T1DFQT,NOs,NOstext
								FROM (
								SELECT 
								PART_NO 
								,PART_NAME
								,MODL
								,PART_TYPE
								,PLN_QTY
								,STK_ADD
								,RCV_QTY
								,SUM(SUM(RCV_QTY)) OVER (PARTITION BY PART_NO||PART_TYPE ORDER BY PART_NO,PART_NAME,MODL,PART_TYPE,STK_ADD) AS ACM,
								ROW_NUMBER() OVER (PARTITION BY PART_NO || PART_TYPE ORDER BY PART_NO,PART_NAME,MODL,PART_TYPE,STK_ADD DESC) AS NOs,
								ROW_NUMBER() OVER (PARTITION BY PART_NO || PART_TYPE ORDER BY PART_NO,PART_NAME,MODL,PART_TYPE,STK_ADD ASC) AS NOstext
								FROM (
								SELECT T1PTNO AS PART_NO,T1PTNM AS PART_NAME,T1MODL AS MODL,T1TYPE AS PART_TYPE,SUM(T1PLQT) AS PLN_QTY FROM DCPLIB.DCT001
								WHERE T1PLDT>=@T1PLDT1
								GROUP BY T1PTNO ,T1PTNM ,T1TYPE,T1MODL
								) AS T1
								FULL OUTER JOIN
								(
								SELECT T3PTNO,T3PTTP,T3STAD AS STK_ADD,SUM(T3STQT) AS RCV_QTY FROM DCPLIB.DCT003
								WHERE T3CTDT=@T1PLDT2
								GROUP BY T3PTNO,T3PTTP,T3STAD
								) AS T3
								ON PART_NO=T3PTNO AND PART_TYPE=T3PTTP
								GROUP BY PART_NO,PART_NAME,MODL,PART_TYPE,PLN_QTY,STK_ADD,RCV_QTY
								) 
								ORDER BY  PART_NO,PART_NAME,MODL,PART_TYPE
							";
                dbConnection.Open();
                var result = dbConnection.Query<ACTUAL>(sQuery, new { T1PLDT1, T1PLDT2 });
                dbConnection.Close();
                return result;
            }
        }

		public IEnumerable<ACTUAL> GetMonitoringReceive()
		{
			using (IDbConnection dbConnection = Connection)
			{
				//string sQuery = @"SELECT * FROM DCPLIB.DCT004 WHERE T4TNCD= 'RCV' AND T4TNDT = @T4TNDT AND T4TYOP = @T4TYOP";
				string sQuery = @"SELECT
								PLN_DATE AS T1PLDT,
								PRT_NO AS T1PTNO,
								PRT_NM AS T1PTNM,
								MODL AS T1MODE,
								TYP AS T1TYPE,
								PLN_QTY AS T1PLQT,
								STK_AD AS T1STAD,
								RCV_QTY AS T1STQT,
								(PLN_QTY - ACM) AS T1DFQT,NOs,NOstext
							FROM
								(
								SELECT
									PLN_DATE,
									PRT_NO,
									PRT_NM,
									MODL,
									TYP,
									PLN_QTY,
									STK_AD,
									RCV_QTY,
									SUM(RCV_QTY) OVER (PARTITION BY PRT_NO || TYP ORDER BY PRT_NO,PRT_NM,MODL,TYP,STK_AD) AS ACM,
									ROW_NUMBER() OVER (PARTITION BY PRT_NO || TYP ORDER BY PRT_NO,PRT_NM,MODL,TYP,STK_AD DESC) AS NOs,
									ROW_NUMBER() OVER (PARTITION BY PRT_NO || TYP ORDER BY PRT_NO,PRT_NM,MODL,TYP,STK_AD ASC) AS NOstext
							--		CASE WHEN ROW_NUMBER() OVER (PARTITION BY PRT_NO || TYP ORDER BY PRT_NO,PRT_NM,MODL,TYP,STK_AD) = 1
							--		THEN 1 
							--		ELSE 0 
							--		END FLAG
								FROM
									(
									SELECT
										T1PLDT AS PLN_DATE,
										T1PTNO AS PRT_NO,
										T1PTNM AS PRT_NM,
										T1MODL AS MODL,
										T1TYPE AS TYP,
										T1PLQT AS PLN_QTY,
										STK_AD,
										IFNULL(SUM(RCV_QTY), 0) AS RCV_QTY
									FROM
										(
										SELECT
											T1PLDT,
											T1PTNO,
											T1PTNM,
											T1MODL,
											T1TYPE,
											T1PLQT,
											T4R.T4TNDT,
											T4R.T4PBCT,
											T4R.T4STQT AS RCV_QTY,
											T4R.T4STAD AS STK_AD,
											T4R.T4TYOP,
											T4C.T4TNDT,
											T4C.T4STQT,'0' AS TEST
										FROM
											(
											SELECT
												*
											FROM
												DCPLIB.DCT001
											WHERE
												T1PLDT = VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD')
											) AS T1
										LEFT JOIN
											(
											SELECT
												*
											FROM
												DCPLIB.DCT004
											WHERE
												T4TNCD = 'RCV'
							) AS T4R
							ON
											T1PLDT = T4R.T4TNDT
											AND T1PTNO = T4R.T4PTNO
											AND T1TYPE = T4R.T4PTTP
										LEFT JOIN
							(
											SELECT
												*
											FROM
												DCPLIB.DCT004
											WHERE
												T4TNCD = 'CRC'
							) AS T4C
							ON
											T4R.T4PBCT = T4C.T4PBCT
										WHERE
											T4C.T4STQT IS NULL
							)
									GROUP BY
										T1PLDT,
										T1PTNO,
										T1PTNM,
										T1MODL,
										T1TYPE,
										T1PLQT,
										STK_AD
							)
								GROUP BY
									PLN_DATE,
									PRT_NO,
									PRT_NM,
									MODL,
									TYP,
									PLN_QTY,
									STK_AD,
									RCV_QTY
							)
							ORDER BY
								PRT_NO,
								PRT_NM,
								MODL,
								TYP
							";
				dbConnection.Open();
				var result = dbConnection.Query<ACTUAL>(sQuery, new {});
				dbConnection.Close();
				return result;
			}
		}


	}
}
