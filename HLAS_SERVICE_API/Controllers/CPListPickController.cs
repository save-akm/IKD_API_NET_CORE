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
    public class CPListPickController
    {
        private readonly CPListRepository cpRepository;

        public CPListPickController()
        {
            cpRepository = new CPListRepository();
        }
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<CLLIST> Get()
        {
            var result = cpRepository.GetAll();
            return result;
        }
    }
}
