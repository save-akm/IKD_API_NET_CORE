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
    public class TBL_MONITORINGRepository
    {
        private string AppConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ConnectionSQL"];

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppConnection);
            }
        }


        public IEnumerable<MONITORINGRCV> GetMonitoringPLAN()
        {
            using (IDbConnection dbConnection = Connection)
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.AppendFormat(@"EXEC sp_Get_Monitoring_Plan_Receive");
                //sQuery.AppendFormat(@"SELECT PCNO,CASENO,FROM_WH,TO_WH,CONVERT(varchar, PLANDATE, 3) AS PLANDATE,TYPE,A_PCNO,A_CASENO,A_DATETIME	 FROM (
                //SELECT P.PCNO,P.CASENO, P.FROM_WH,P.TO_WH,P.PLANDATE,P.TYPE, A.PCNO AS A_PCNO, A.CASENO AS A_CASENO, A.DATETIME1 AS A_DATETIME  FROM TBL_PLAN_RECEIVEING P
                //LEFT JOIN (SELECT * FROM TBL_CASE_RETURN WHERE INOUT = 'IN') A
                //ON P.PCNO = A.PCNO AND P.CASENO = A.CASENO ) AS T1
                //--WHERE CAST(P.PLANDATE AS DATE) =  CAST(GETDATE() AS DATE)) AS T1
                //WHERE T1.A_PCNO IS NULL AND T1.A_CASENO  IS NULL ORDER BY PLANDATE ASC");
                //sQuery.AppendFormat(@"", T1PLDT1, T1PLDT2);
                dbConnection.Open();
                var result = dbConnection.Query<MONITORINGRCV>(sQuery.ToString(), new { });
                dbConnection.Close();
                return result;

            }
        }
        public IEnumerable<MONITORINGRCV> GetCoutPlan()
        {
            using (IDbConnection dbConnection = Connection)
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.AppendFormat(@"SELECT COUNT(P.PCNO) AS PLANS  FROM TBL_PLAN_RECEIVEING P
                LEFT JOIN (SELECT * FROM TBL_CASE_RETURN WHERE INOUT = 'IN') A
                ON P.PCNO = A.PCNO AND P.CASENO = A.CASENO 
                WHERE CAST(P.PLANDATE AS DATE) =  CAST(GETDATE() AS DATE)");
                //sQuery.AppendFormat(@"", T1PLDT1, T1PLDT2);
                dbConnection.Open();
                var result = dbConnection.Query<MONITORINGRCV>(sQuery.ToString(), new { });
                dbConnection.Close();
                return result;

            }
        }
        public IEnumerable<MONITORINGRCV> GetCoutActual()
        {
            using (IDbConnection dbConnection = Connection)
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.AppendFormat(@"SELECT COUNT(*) AS ACTUALS FROM (
                SELECT P.PCNO,P.CASENO, P.FROM_WH,P.TO_WH,P.PLANDATE,P.TYPE, A.PCNO AS A_PCNO, A.CASENO AS A_CASENO, A.DATETIME1 AS A_DATETIME  FROM TBL_PLAN_RECEIVEING P
                LEFT JOIN (SELECT * FROM TBL_CASE_RETURN WHERE INOUT = 'IN') A
                ON P.PCNO = A.PCNO AND P.CASENO = A.CASENO 
                WHERE CAST(P.PLANDATE AS DATE) =  CAST(GETDATE() AS DATE) ) AS T1
                WHERE T1.A_PCNO IS NOT NULL AND T1.A_CASENO  IS NOT NULL");
                //sQuery.AppendFormat(@"", T1PLDT1, T1PLDT2);
                dbConnection.Open();
                var result = dbConnection.Query<MONITORINGRCV>(sQuery.ToString(), new { });
                dbConnection.Close();
                return result;
            }
        }
        public IEnumerable<MONITORINGRCV> GetPlanActualRemail()
        {
            using (IDbConnection dbConnection = Connection)
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.AppendFormat(@"EXEC sp_Get_PlanActualRemain_Receive");
                //sQuery.AppendFormat(@"SELECT SUM(PLANS) AS PLANS,SUM(ACTUALS) AS ACTUALS,SUM(PLANS)-SUM(ACTUALS) AS REMAIN  FROM (
                //SELECT COUNT(P.PCNO) AS PLANS, 0 AS ACTUALS FROM TBL_PLAN_RECEIVEING P
                //LEFT JOIN (SELECT * FROM TBL_CASE_RETURN WHERE INOUT = 'IN') A
                //ON P.PCNO = A.PCNO AND P.CASENO = A.CASENO 
                //WHERE CAST(P.PLANDATE AS DATE) =  CAST(GETDATE() AS DATE) --'20220126'
                //UNION ALL
                //SELECT 0 AS PLANS,COUNT(*) AS ACTUALS FROM (
                //SELECT P.PCNO,P.CASENO, P.FROM_WH,P.TO_WH,P.PLANDATE,P.TYPE, A.PCNO AS A_PCNO, A.CASENO AS A_CASENO, A.DATETIME1 AS A_DATETIME  FROM TBL_PLAN_RECEIVEING P
                //LEFT JOIN (SELECT * FROM TBL_CASE_RETURN WHERE INOUT = 'IN') A
                //ON P.PCNO = A.PCNO AND P.CASENO = A.CASENO 
                //WHERE CAST(P.PLANDATE AS DATE) =  CAST(GETDATE() AS DATE) ) AS T1
                //WHERE T1.A_PCNO IS NOT NULL AND T1.A_CASENO  IS NOT NULL) AS T2
                //");
                //sQuery.AppendFormat(@"", T1PLDT1, T1PLDT2);
                dbConnection.Open();
                var result = dbConnection.Query<MONITORINGRCV>(sQuery.ToString(), new { });
                dbConnection.Close();
                return result;
            }
        }
        public IEnumerable<MONITORINGRCV> GetReceiveMonitoringBetweenDate(string PLDATE1, string PLDATE2)
        {
            using (IDbConnection dbConnection = Connection)
            {
                StringBuilder sQuery = new StringBuilder();
                //sQuery.AppendFormat(@"EXEC sp_Get_inquiry_Receive @DATE1 ='{0}' , @DATE2 < '{1}'", PLDATE1, PLDATE2);
                sQuery.AppendFormat(@"EXEC sp_Get_inquiry_Receive '{0}','{1}'", PLDATE1, PLDATE2);
                dbConnection.Open();
                var result = dbConnection.Query<MONITORINGRCV>(sQuery.ToString(), new { });
                dbConnection.Close();
                return result;
            }
        }
        public void GetReceiveMonitoringDelete(string NOS)
        {
            using (IDbConnection dbConnection = Connection)
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.AppendFormat(@"EXEC sp_Get_inquiry_delete_Receive '{0}'", NOS);
                dbConnection.Open();
                dbConnection.Execute(sQuery.ToString(), new { });
                dbConnection.Close();

            }
        }
    }
}