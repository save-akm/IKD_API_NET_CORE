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
    public class HREMPTController
    {
        private readonly HREMPTRepository hremptrepository;
        public HREMPTController()
        {

            hremptrepository = new HREMPTRepository();

        }
        [HttpPost]
        [Route("GetByIDOverFlowIN")]
        public IEnumerable<HREMPT> GetByIDOverFlowIN(string PICDTE1, string PICDTE2)
        {
            var result = hremptrepository.GetHRByDate(PICDTE1, PICDTE2);
            return result;

        }
    }
}