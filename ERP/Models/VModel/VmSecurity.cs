using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VModel
{
    public class VmUserRolePermission
    {
        public bool ResourceStatus { set; get; }
        public int RolePermissionId { set; get; }
        public int? ResourcePermissionId { set; get; }
        public int Flag { set; get; }
        public bool? Add { set; get; }
        public bool? Edit { set; get; }
        public bool? Delete { set; get; }
    }

    public class ResourcePermissionViewModel
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string DisplayName { set; get; }

        public int SecResourceId { set; get; }

        public int SecRoleId { set; get; }

        public bool Status { get; set; }

        public int Serial { get; set; }

        public int Level { set; get; }
        public int RoleLevel { set; get; }
        public bool IsReport { set; get; }
        public string Method { set; get; }
        public string Icon { set; get; }

    }

    public class ResourceViewModel
    {
        public int RpermiId { get; set; }
        public int ResId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int PermiId { get; set; }
        public int? SecResourceId { get; set; }
        public bool Status { get; set; }
        public int Level { get; set; }
        public bool AddPermi { get; set; }
        public bool EditPermi { get; set; }
        public bool DeletePermi { get; set; }
        public int RolePrmId { get; set; }
        public int RoleLevel { get; set; }
        public bool IsReport { get; set; }

    }
}