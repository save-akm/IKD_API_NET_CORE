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
    public class D3DIWEKController
    {
        private readonly D3DIWEKRepository d3diwekrepository;
        public D3DIWEKController()
        {
            d3diwekrepository = new D3DIWEKRepository();
        }
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<D3DIWEK> Get()
        {
            return d3diwekrepository.GetAll();
        }

        [HttpPost]
        [Route("GetByD3DIWEK")]
        public IEnumerable<D3DIWEK> Get2(int PICDTE1, int PICDTE2)
        {
            var result = d3diwekrepository.GetByIDD3DIWEK(PICDTE1, PICDTE2);
            return result;
        }
    }
}
