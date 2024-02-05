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
    public class MNRPTDCCController
    {
        private readonly MNRPTDCCRepository mnrptdccrepository;
        public MNRPTDCCController()
        {
            mnrptdccrepository = new MNRPTDCCRepository();
        }
        [HttpGet]
        [Route("GetMonitoringRPT")]
        public IEnumerable<MNRPTDCC> GetMonitoringRPT(string GZONE, int ZONE)
        {
            //return hlasRepository.GetByID(DLVMNT, DLVZNE);
            return mnrptdccrepository.GetMonitoringRPT(GZONE, ZONE);
        }
        [HttpGet]
        [Route("GetMonitoringRPTDLV")]
        public IEnumerable<MNRPTDCC> GetMonitoringRPTDLV(string GZONE, string ZONE)
        {
            //return hlasRepository.GetByID(DLVMNT, DLVZNE);
            return mnrptdccrepository.GetMonitoringRPTDLV(GZONE, ZONE);
        }


    }
}
