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
    public class ACTUALController
    {
        private readonly ACTUALRepository actualRepository;
        public ACTUALController()
        {
            actualRepository = new ACTUALRepository();
        }
        [HttpPost]
        [Route("GetActualReceiveBetween")]
        public IEnumerable<ACTUAL> GetActualReceiveBetween(string T1PLDT1, string T1PLDT2)
        {
            var result = actualRepository.GetActualReceiveBetween(T1PLDT1, T1PLDT2);
            return result;
        }
        [HttpPost]
        [Route("GetActualReceive")]
        public IEnumerable<ACTUAL> GetActualReceive(string T1PLDT1, string T1PLDT2)
        {
            var result = actualRepository.GetActualReceive(T1PLDT1, T1PLDT2);
            return result;
        }

        [HttpGet]
        [Route("GetMonitoringReceive")]
        public IEnumerable<ACTUAL> GetMonitoringReceive()
        {
            var result = actualRepository.GetMonitoringReceive();
            return result;
        }


    }
}
