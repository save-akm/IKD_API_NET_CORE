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
    public class D3HLASRController
    {
        private readonly D3HLASRRepository d3hlasrrepository;
    public D3HLASRController()
        {
            d3hlasrrepository = new D3HLASRRepository();
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<D3HLASR> Get()
        {
            return d3hlasrrepository.GetAll();
        }


    }
}