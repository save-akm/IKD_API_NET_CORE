using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HLAS_SERVICE_API.Models;
using HLAS_SERVICE_API.Services;
namespace HLAS_SERVICE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TBL_MONITORINGController
    {
        private readonly TBL_MONITORINGRepository tbl_monitoringrepository;
        public TBL_MONITORINGController()
        {
            tbl_monitoringrepository = new TBL_MONITORINGRepository();
        }
        [HttpGet]
        [Route("GetMonitoringPLAN")]
        public IEnumerable<MONITORINGRCV> GetMonitoringPLAN()
        {
            return tbl_monitoringrepository.GetMonitoringPLAN();
        }
        [HttpGet]
        [Route("GetCoutPlan")]
        public IEnumerable<MONITORINGRCV> GetCoutPlan()
        {
            return tbl_monitoringrepository.GetCoutPlan();
        }
        [HttpGet]
        [Route("GetCoutActual")]
        public IEnumerable<MONITORINGRCV> GetCoutActual()
        {
            return tbl_monitoringrepository.GetCoutActual();
        }
        [HttpGet]
        [Route("GetPlanActualRemail")]
        public IEnumerable<MONITORINGRCV> GetPlanActualRemail()
        {
            return tbl_monitoringrepository.GetPlanActualRemail();
        }
        [HttpPost]
        [Route("GetReceiveMonitoringBetweenDate")]
        public IEnumerable<MONITORINGRCV> GetReceiveMonitoringBetweenDate(string PLDATE1, string PLDATE2)
        {
            return tbl_monitoringrepository.GetReceiveMonitoringBetweenDate(PLDATE1, PLDATE2);
        }


        [HttpDelete]
        [Route("GetReceiveMonitoringDelete")]
        public void Delete(string NOS)
        {
            tbl_monitoringrepository.GetReceiveMonitoringDelete(NOS);
        }
    }
}
