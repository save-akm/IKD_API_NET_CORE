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
    public class D3MANIFController
    {
        private readonly D3MANIFRepository d3mainifrepository;
    public D3MANIFController()
        {
            d3mainifrepository = new D3MANIFRepository();
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<D3MANIF> Get()
        {
            return d3mainifrepository.GetAll();
        }
        [HttpPost]
        [Route("GetD3MANFEbetween")]
        public IEnumerable<D3MANIF> GetD3MANFEbetween(string PICDTE1, string PICDTE2)
        {
            var result = d3mainifrepository.GetD3MANFEbetween(PICDTE1, PICDTE2);
            return result;

        }
    }
}