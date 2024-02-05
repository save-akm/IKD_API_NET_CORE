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
    public class TBL_USERController 
    {
        private readonly TBL_USERRepository tbl_userrepository;
        public TBL_USERController()
        {

            tbl_userrepository = new TBL_USERRepository();

        }
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<TBL_USER> Get()
        {
            //return hlasRepository.GetByID(DLVMNT, DLVZNE);
            return tbl_userrepository.GetAll();
        }
        [HttpPost]
        [Route("GetBYID")]
        // public IEnumerable<DATUSER> Get(DATUSER item)
        public TBL_USER Get(TBL_USER item)
        {
            //return hlasRepository.GetByID();
            var result1 = tbl_userrepository.GetByID(item.USERNAME, item.PASSWORD);
            //return datuserrepository.GetByID(item.USUSCD, item.USPASS);
            return result1;
        }

        [HttpPost]
        [Route("AddUser")]
        public void Post([FromBody] TBL_USER tbl_user)
        {
            tbl_userrepository.AddUser(tbl_user);
        }


        [HttpPost]
        [Route("GetUserByIDandPass")]
        public IEnumerable<TBL_USER> GetUserByIDandPass(string USERNAME, string PASSWORD)
        {
            var result = tbl_userrepository.GetUserByIDandPass(USERNAME, PASSWORD);
            return result;

        }


        [HttpDelete]
        //[Route("api/D3ZNEMB/Delete/{DLVMNTs}/{DLVZNEs}")]
        [Route("DeleteByIDandPass")]
        //[Route("api/D3ZNEMB/Delete/")]
        public void Delete(string USERNAME, string PASSWORD)
        {

            tbl_userrepository.Delete(USERNAME, PASSWORD);

        }

        [HttpPut]
        [Route("Update")]
        public void Put(string USERNAME, string PASSWORD, string PERMISSION)
        //public void Put(string Newzne, string Oldzne, string DLVMNTold, [FromBody] D3ZNEMB d3znemb)
        {
          tbl_userrepository.Update(USERNAME, PASSWORD, PERMISSION);
        }
    }
}