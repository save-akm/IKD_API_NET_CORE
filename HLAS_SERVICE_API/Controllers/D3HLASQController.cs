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
    public class D3HLASQController
    {
        private readonly D3HLASQRepository d3hlasqrepository;
    public D3HLASQController()
        {
            d3hlasqrepository = new D3HLASQRepository();
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<D3HLASQ> Get()
        {
            return d3hlasqrepository.GetAll();
        }

    }
}