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
    public class D3AJTMEController
    {
        private readonly D3AJTMERepository d3ajtmerepository;
        public D3AJTMEController()
        {
            d3ajtmerepository = new D3AJTMERepository();
        }
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<D3AJTME> Get()
        {
            //return hlasRepository.GetByID(DLVMNT, DLVZNE);
            return d3ajtmerepository.GetAll();
        }

        [HttpPut]
        [Route("Update")]
        public void Put(string D3NUMBE, string D3TYPET)
        //public void Put(string Newzne, string Oldzne, string DLVMNTold, [FromBody] D3ZNEMB d3znemb)
        {
             d3ajtmerepository.Update(D3NUMBE, D3TYPET);
        }
        [HttpPut]
        [Route("Updatezone")]
        public void Updatezone(string D3NUMBE, string D3TYPET, string D3ZONEM)
        
        {
           d3ajtmerepository.Updatebyzone(D3NUMBE, D3TYPET, D3ZONEM);
        }
    }
}
