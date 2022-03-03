using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VModel
{
    public class CustomerDueCollectionDto
    {
        public long PosSalesInvoiceId { get; set; }
        public decimal Amount { get; set; }
    }
}