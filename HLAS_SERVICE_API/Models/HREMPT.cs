using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HLAS_SERVICE_API.Models
{
    public class HREMPT
    {         
        public string EmployeeID { get; set; }
        public string NameEng { get; set; }
        public string UserID { get; set; }
        public string Employee { get; set; }
        public string DateTime { get; set; }
        public string EventID { get; set; }
        public string DeviceID { get; set; }
    }
}