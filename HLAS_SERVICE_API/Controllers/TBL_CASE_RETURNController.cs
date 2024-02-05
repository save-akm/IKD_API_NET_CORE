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
    public class TBL_CASE_RETURNController
    {
       
        private readonly TBL_CASE_RETURNRepository tbl_case_returnrepository;
        public TBL_CASE_RETURNController()
        {
            tbl_case_returnrepository = new TBL_CASE_RETURNRepository();
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<TBL_CASE_RETURN> Get()
        {
            return tbl_case_returnrepository.GetAll();
        }
        [HttpGet]
        [Route("GetAllCaseReceive")]
        public IEnumerable<TBL_CASE_RETURN> GetAllCaseRevice()
        {
            return tbl_case_returnrepository.GetAllCaseRevice();
        }
        [HttpPost]
        [Route("GetAllCaseALLBydate")]
        public IEnumerable<TBL_CASE_RETURN> GetAllCaseALLBydate(string PICDTE1, string PICDTE2)
        {
            var result = tbl_case_returnrepository.GetAllCaseALLBydate(PICDTE1, PICDTE2);
            return result;

        }
        [HttpPost]
        [Route("GetAllCaseINBydate")]
        public IEnumerable<TBL_CASE_RETURN> GetAllCaseINBydate(string PICDTE1, string PICDTE2)
        {
            var result = tbl_case_returnrepository.GetAllCaseINBydate(PICDTE1, PICDTE2);
            return result;

        }
        [HttpPost]
        [Route("GetAllCaseOUTBydate")]
        public IEnumerable<TBL_CASE_RETURN> GetAllCaseOUTBydate(string PICDTE1, string PICDTE2)
        {
            var result = tbl_case_returnrepository.GetAllCaseOUTBydate(PICDTE1, PICDTE2);
            return result;

        }

        [HttpGet]
        [Route("GetTest")]
        public IEnumerable<TBL_CASE_RETURN> GetTest()
        {
            var result = tbl_case_returnrepository.GetAllCaseRevice();
            var test = from s in result where s.PCNO == "38C3A3924213" && s.CASENO == "A149MPPP00A 00201" select s;
            return test;
        }
    }
}