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
    public class TBL_OVERFLOWController
    {
        private readonly TBL_OVERFLOWRepository tbl_oveflowrepository;
        public TBL_OVERFLOWController()
        {

            tbl_oveflowrepository = new TBL_OVERFLOWRepository();

        }
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<TBL_OVERFLOW> Get()
        {
            //return hlasRepository.GetByID(DLVMNT, DLVZNE);
            return tbl_oveflowrepository.GetAll();
        }
        [HttpGet]
        [Route("GetInnvetory")]
        public IEnumerable<TBL_OVERFLOW> GetInnvetory()
        {
            //return hlasRepository.GetByID(DLVMNT, DLVZNE);
            return tbl_oveflowrepository.GetInnvetory();
        }
        [HttpGet]
        [Route("GetIN")]
        public IEnumerable<TBL_OVERFLOW> GetAllIN()
        {
            //return hlasRepository.GetByID(DLVMNT, DLVZNE);
            return tbl_oveflowrepository.GetAllIN();
        }
        [HttpGet]
        [Route("GetOUT")]
        public IEnumerable<TBL_OVERFLOW> GetOUT()
        {
            //return hlasRepository.GetByID(DLVMNT, DLVZNE);
            return tbl_oveflowrepository.GetAllOUT();
        }


        [HttpPost]
        [Route("GetByIDOverFlow")]
        public IEnumerable<TBL_OVERFLOW> GetByIDOverFlow(string PICDTE1, string PICDTE2)
        {
            var result = tbl_oveflowrepository.GetByIDOverFlow(PICDTE1, PICDTE2);
            return result;

        }
        [HttpPost]
        [Route("GetByIDOverFlowIN")]
        public IEnumerable<TBL_OVERFLOW> GetByIDOverFlowIN(string PICDTE1, string PICDTE2)
        {
            var result = tbl_oveflowrepository.GetByIDOverFlowIN(PICDTE1, PICDTE2);
            return result;

        }
        [HttpPost]
        [Route("GetByIDOverFlowOUT")]
        public IEnumerable<TBL_OVERFLOW> GetByIDOverFlowOUT(string PICDTE1, string PICDTE2)
        {
            var result = tbl_oveflowrepository.GetByIDOverFlowOUT(PICDTE1, PICDTE2);
            return result;

        }
    }
}