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
    public class TBL_TROLLEYController
    {
        private readonly TBL_TROLLEYRepository tbl_trolleyrepository;
        public TBL_TROLLEYController()
        {

            tbl_trolleyrepository = new TBL_TROLLEYRepository();

        }
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<TBL_TROLLEY> Get()
        {
            return tbl_trolleyrepository.GetAll();
        }
        [HttpGet]
        [Route("GetTrolleyMaster")]
        public IEnumerable<TBL_TROLLEY> GetTrolleyMaster()
        {
            return tbl_trolleyrepository.GetTrolleyMaster();
        }
        [HttpPost]
        [Route("GetReturnbyTypeandDate")]
        public IEnumerable<TBL_TROLLEY> GetReturnbyTypeandDate(string TROLLEY, string PICDTE1, string PICDTE2)
        {
            var result = tbl_trolleyrepository.GetReturnbyTypeandDate(TROLLEY, PICDTE1, PICDTE2);
            return result;

        }
        [HttpPost]
        [Route("GetReturnbyTypeandDateCycle")]
        public IEnumerable<TBL_TROLLEY> GetReturnbyTypeandDateCycle(string TROLLEY, string PICDTE1, string PICDTE2, int Cycle)
        {
            var result = tbl_trolleyrepository.GetReturnbyTypeandDateCycle(TROLLEY, PICDTE1, PICDTE2, Cycle);
            return result;

        }
        [HttpPost]
        [Route("GetTrolleyCycle")]
        public IEnumerable<TBL_TROLLEY> GetTrolleyCycle(string TROLLEY, string PICDTE1, string PICDTE2)
        {
            var result = tbl_trolleyrepository.GetTrolleyCycle(TROLLEY, PICDTE1, PICDTE2);
            return result;

        }
    }
}