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
    public class D3DVZNEController
    {
        private readonly D3DVZNERepository d3dvznerepository;
        public D3DVZNEController() 
        {

            d3dvznerepository = new D3DVZNERepository();

        }
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<D3DVZNE> Get()
        {
            //return hlasRepository.GetByID(DLVMNT, DLVZNE);
            return d3dvznerepository.GetAll();
        }

    }
}