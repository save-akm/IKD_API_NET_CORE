using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HLAS_SERVICE_API.Models
{
    public class TBL_USER
    {
              
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string PERMISSION { get; set; }
    }
}