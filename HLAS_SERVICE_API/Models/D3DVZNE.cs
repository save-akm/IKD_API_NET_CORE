using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HLAS_SERVICE_API.Models
{
    public class D3DVZNE
    {
        public string DLVZNE { get; set; }
        public string DVZDSC { get; set; }
        public string DVZSDS { get; set; }
        public string DLVSTL { get; set; }

        public int DVACTM { get; set; }
        public int NOFBDY { get; set; }
        public string RLSADD { get; set; }
        public int CRTDTE { get; set; }
        public int CRTTME { get; set; }
        public string CRTUSR { get; set; }
        public int UPDDTE { get; set; }
        public int UPDTME { get; set; }
        public string UPDUSR { get; set; }
        public int LSTACS { get; set; }

    }
}