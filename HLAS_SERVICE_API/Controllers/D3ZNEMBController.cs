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
    public class D3ZNEMBController
    {
        private readonly HLASRepository hlasRepository;
        public D3ZNEMBController()
        {
            hlasRepository = new HLASRepository();
        }
      
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<D3ZNEMB> Get()
        {
            var result = hlasRepository.GetAll();
            return result;
        }

        [HttpGet]
        [Route("GetGroupZone")]
        public IEnumerable<D3ZNEMB> Get1()
        {
            var result = hlasRepository.GetAll1();
            return result;
        }

        [HttpPost]
        [Route("GetBYID")]
        public IEnumerable<D3ZNEMB> Get(string DLVMNT,string DLVZNE)
        {
            //return hlasRepository.GetByID();
            return hlasRepository.GetByID(DLVMNT, DLVZNE);
        }
        [HttpDelete]
       //[Route("api/D3ZNEMB/Delete/{DLVMNTs}/{DLVZNEs}")]
        [Route("Delete")]
        //[Route("api/D3ZNEMB/Delete/")]
        public void Delete(string DLVZNEs, string DLVMNTs)
        {

            hlasRepository.Delete(DLVMNTs,DLVZNEs);

        }

        [HttpPost]
        [Route("Add")]
        public void Post([FromBody] D3ZNEMB d3znemb)
        {
           hlasRepository.Add(d3znemb);
        }

        //[HttpPost]
        //[Route("api/D3ZNEMB/Add1/")]
        //public void Post1([FromBody] D3ZNEMB d3znemb)
        //{
        //    if (ModelState.IsValid) hlasRepository.Add(d3znemb);
        //}

        [HttpPut]
        [Route("Update")]
        public void Put(string Newzne,string Oldzne,string DLVMNTold)
            //public void Put(string Newzne, string Oldzne, string DLVMNTold, [FromBody] D3ZNEMB d3znemb)
        {
           hlasRepository.Update(Newzne, Oldzne, DLVMNTold);
        }
        [HttpPut]
        [Route("Update1")]
        public void Put1(string DLVZNE, [FromBody] D3ZNEMB d3znemb)
        {
            d3znemb.DLVZNE = DLVZNE;
            // d3znemb.DLVZNE = Newzne;
            hlasRepository.Update1(d3znemb);
        }
    }
}
