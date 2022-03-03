using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VModel
{
    public class VmStockTransferInvoiceNo
    {
        public long InvoiceNumber { set; get; }
        public DateTime InvDate { set; get; }
        public DateTime CreatedDate { set; get; }
    }
}