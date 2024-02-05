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
    public class PICKING_REMAINController
    {
        private readonly PICKING_REMAINRepository picking_remainrepository;
        public PICKING_REMAINController()
        {
            picking_remainrepository = new PICKING_REMAINRepository();
        }

        [HttpPost]
        [Route("GetBYID")]
         public IEnumerable<PICKING_REMAIN> Get(PICKING_REMAIN item)
        {

            var result = picking_remainrepository.GetByID(item.PICDTE, item.DLVMNT);
            return result;
        }
        [HttpPost]
        [Route("GetByIDPicking")]
        public IEnumerable<PICKING_REMAIN> Get1(int PICDTE1, int PICDTE2, string DLVMNT)
        {
            var result = picking_remainrepository.GetByIDPicking(PICDTE1, PICDTE2, DLVMNT);
            return result;

        }
        [HttpPost]
        [Route("GetByIDDelivery")]
        public IEnumerable<PICKING_REMAIN> Get2(int DLVDTE1, int DLVDTE2, string DLVMNT)
        {
            var result = picking_remainrepository.GetByIDDelivery(DLVDTE1, DLVDTE2, DLVMNT);
            return result;

        }
    }
}
