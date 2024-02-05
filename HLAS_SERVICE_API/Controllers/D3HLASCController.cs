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
    public class D3HLASCController
    {
        private readonly D3HLASCRepository d3hlascrepository;
    public D3HLASCController()
        {
        d3hlascrepository = new D3HLASCRepository();
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<D3HLASC> Get()
        {
            return d3hlascrepository.GetAll();
        }
        [HttpGet]
        [Route("GetAllUnpackok")]
        public IEnumerable<D3HLASC> GetAllUnpackok()
        {
            return d3hlascrepository.GetAllUnpackok();
        }
        [HttpPost]
        [Route("GetCaseStockByDate")]
        public IEnumerable<D3HLASC> GetCaseStockByDate(string PICDTE1, string PICDTE2)
        {
            var result = d3hlascrepository.GetCaseStockByDate(PICDTE1, PICDTE2);
            return result;
            //if (result.First == null) { return result; }

        }

    }
}