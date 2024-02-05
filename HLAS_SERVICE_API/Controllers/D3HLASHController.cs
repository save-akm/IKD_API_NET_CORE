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
    public class D3HLASHController 
    {
        private readonly D3HLASHRepository d3hlashrepository;
    public D3HLASHController()
        {
            d3hlashrepository = new D3HLASHRepository();
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<D3HLASH> Get()
        {
            return d3hlashrepository.GetAll();
        }
        [HttpGet]
        [Route("GetAllUnpackok")]
        public IEnumerable<D3HLASH> GetAllUnpackok()
        {
            return d3hlashrepository.GetAllUnpackok();
        }
        [HttpPost]
        [Route("GetCaseStockByDate")]
        public IEnumerable<D3HLASH> GetCaseStockByDate(string PICDTE1, string PICDTE2)
        {
            var result = d3hlashrepository.GetCaseStockByDate(PICDTE1, PICDTE2);
            return result;

        }

    }
}