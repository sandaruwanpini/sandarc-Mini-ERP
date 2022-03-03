using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.Security.Authorization
{
    public class VmResourcePermission
    {
        public int Id { set; get; }
        public int SecRoleId { set; get; }
        public bool Status { get; set; }
        public string Method { set; get; }
        public int SecResourceId { set; get; }
    }

    public class VmRolePermission
    {
        public int Id { set; get; }
        public int SecResourceId { set; get; }
        public int SecRoleId { set; get; }
        public bool AddPermi { set; get; }
        public bool ReadonlyPermi { set; get; }
        public bool EditPermi { set; get; }
        public bool DeletePermi { set; get; }
        public bool PrintPermi { set; get; }
    }
}