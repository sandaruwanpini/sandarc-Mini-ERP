using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ERP.Models.Security;

namespace ERP.Models.Manager
{
    public class CommonPropertyWithIdentity
    {
        public int? CreatedBy { set; get; }
        public DateTime? CreatedDate { set; get; }
        public int? ModifiedBy { set; get; }
        public DateTime? ModifideDate { set; get; }
        public int CmnCompanyId { set; get; }
        
        public CmnCompany CmnCompany { set; get; }
    }

    public class CommonPropertyWithoutIdAndCompanyId
    {
        public int? CreatedBy { set; get; }
        public DateTime? CreatedDate { set; get; }
        public int? ModifiedBy { set; get; }
        public DateTime? ModifideDate { set; get; }

    }

    public class CommonProperty<T>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { set; get; }
        public int? CreatedBy { set; get; }
        public DateTime? CreatedDate { set; get; }
        public int? ModifiedBy { set; get; }
        public DateTime? ModifideDate { set; get; }
        public int CmnCompanyId { set; get; }
    }
}