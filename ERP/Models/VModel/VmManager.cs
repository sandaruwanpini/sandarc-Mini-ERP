using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VModel
{
    public class VmDatabaseBackup
    {
        public  string Path { set; get; }
        public  string DB { set; get; }
    }

    public class VmTodaySalesInfoDashboard
    {
        public int TotalInvoice { set; get; }
        public decimal Sales { set; get; }
        public decimal Due { set; get; }
        public decimal DueCollection { set; get; }
    }
}