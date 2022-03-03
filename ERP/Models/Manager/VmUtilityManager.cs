using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.Manager
{
    public class VmBarcodeAndQtySeparator
    {
        public string Barcode { set; get; }
        public int Qty { set; get; }
        public string BatchName { set; get; }
    }
}