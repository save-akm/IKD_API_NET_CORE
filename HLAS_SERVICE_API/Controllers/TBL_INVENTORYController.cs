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
    public class TBL_INVENTORYController
    {
        private readonly TBL_INVENTORYRepository tbl_inventoryrepository;
        public TBL_INVENTORYController()
        {

            tbl_inventoryrepository = new TBL_INVENTORYRepository();

        }
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<TBL_TROLLEY> Get()
        {
            return tbl_inventoryrepository.GetAll();
        }
      
    }
}