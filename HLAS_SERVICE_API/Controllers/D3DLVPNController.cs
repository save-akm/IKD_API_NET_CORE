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
    public class D3DLVPNController
    {
        private readonly D3DLVPNRepository d3dlvpnrepository;
    public D3DLVPNController()
        {
            d3dlvpnrepository = new D3DLVPNRepository();
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<D3DLVPN> Get()
        {
            return d3dlvpnrepository.GetAll();
        }
        [HttpGet]
        [Route("GetAllData")]
        public IEnumerable<D3DLVPN> GetAllData()
        {
            return d3dlvpnrepository.GetAllData();
        }


    }
}