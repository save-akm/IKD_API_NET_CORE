using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HLAS_SERVICE_API.Models
{
    public class PICKING_REMAIN
    {
        public string PKLNO { get; set; }
        public string CPINO { get; set; }
        public string DCPTNO { get; set; }
        public string PRTDSC { get; set; }
        public string PRTCLR { get; set; }
        public string SPPCDE { get; set; }
        public int PCRQTY { get; set; }//int
        public int DLVQTY { get; set; }//int
        public string DLVZNE { get; set; }
        public int DLVDTE { get; set; }//int
        public int DLVTME { get; set; }//int
        public string KDLTFR { get; set; }
        public int PICDTE { get; set; }//int
        public int PICTME { get; set; } //int
        public string DLVMNT { get; set; }
        public string SPLCSH { get; set; }
        public string PICSCN { get; set; }//int
        public string DLVSCN { get; set; }//int
    }
}