using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ERP.Models.Manager;
using ERP.Models.Security;

namespace ERP.Models.Accounting
{
    [Table("AccAccountTypes")]
    public class AccAccountType:CommonProperty<Int32>
    {
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Remarks { set; get; }
        public int AccFlag { set; get; }

        public CmnCompany CmnCompany { set; get; }
    }


}