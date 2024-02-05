using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HLAS_SERVICE_API.Models;
using HLAS_SERVICE_API.Services;
namespace HLAS_SERVICE_API.Controllers
{
//    [Route("api/[controller]")]
//    [ApiController]
    public class DATUSERController
    {
        private readonly DATUSERRepository datuserrepository;
        public DATUSERController()
        {
            datuserrepository = new DATUSERRepository();
        }
        [HttpGet]
        [Route("api/Login/GetAll/")]
        public IEnumerable<DATUSER> Get()
        {
            //return hlasRepository.GetByID(DLVMNT, DLVZNE);
            return datuserrepository.GetAll();
        }
        [HttpPost]
        [Route("api/Login/GetBYID/")]
       // public IEnumerable<DATUSER> Get(DATUSER item)
        public DATUSER Get(DATUSER item)
        {
            //return hlasRepository.GetByID();
            var result1 = datuserrepository.GetByID(item.USUSCD, item.USPASS);
            //return datuserrepository.GetByID(item.USUSCD, item.USPASS);
            return result1;
        }

    }
}