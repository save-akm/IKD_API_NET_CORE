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
    public class IKDMONITORINGController
    {
        private readonly IKDMONITORINGRepository ikdmonitoringrepository;
        public IKDMONITORINGController()
        {
            ikdmonitoringrepository = new IKDMONITORINGRepository();
        }
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<IKDMONITORING> Get()
        {
            return ikdmonitoringrepository.GetAll();
        }
        [HttpGet]
        [Route("GetAllN2N1G9JT_OLD")]
        public IEnumerable<IKDMONITORING> GetAllN2N1G9JT_OLD()
        {
            return ikdmonitoringrepository.GetAllN2N1G9JT_OLD();
        }

        [HttpGet]
        [Route("GetAllBoxUnpack")]
        public IEnumerable<IKDMONITORING> GetAllBoxUnpack()
        {
            return ikdmonitoringrepository.GetAllBoxUnpack();
        }

        [HttpGet]
        [Route("GetAllStaging")]
        public IEnumerable<IKDMONITORING> GetAllStaging()
        {
            return ikdmonitoringrepository.GetAllStaging();
        }

        [HttpGet]
        [Route("GetAllN2N1G9JT")]
        public IEnumerable<IKDMONITORING> GetAllN2N1G9JT()
        {
            return ikdmonitoringrepository.GetAllN2N1G9JT();
        }
    }
}