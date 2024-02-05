using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HLAS_SERVICE_API.Models
{
    public class TBL_OVERFLOW
    {
              
        public string PARTNO { get; set; }
        public string COLOR { get; set; }
        public string PARTNO_COLOR { get; set; }
        public string SUPPLIER { get; set; }
        public string ZONE { get; set; }
        public string WMS_LOCATION { get; set; }
        public string INOUT { get; set; }
        public string SHIFT { get; set; }
        public string DATEs { get; set; }
        public string BOX { get; set; }
        public string CALCULATE { get; set; }
        public string OVF_LOCATION { get; set; }
        public string REASON { get; set; }
        public string EMP_ID { get; set; }

        public string QTYIN { get; set; }
        public string QTYOUT { get; set; }
        public string REMAIN { get; set; }


    }
}