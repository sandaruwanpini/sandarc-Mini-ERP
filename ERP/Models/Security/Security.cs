using ERP.Models.Manager;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Web;
using ERP.Models.POS;

namespace ERP.Models.Security
{
    [Table("SecModules")]
    public class SecModule
    {
        public int Id { set; get; }
        [StringLength(120)]
        public string Name { set; get; }
        public int CmnCompanyId { set; get; }
        public int Level { set; get; }
        public string IconUrl { set; get; }
        public bool Status { set; get; }
        public string Url { set; get; }
        public CmnCompany CmnCompany { set; get; }

    }

    [Table("CmnCompanies")]
    public class CmnCompany
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { set; get; }

        public int? CompanyIdOnExs { set; get; }

        [Required]
        [StringLength(180)]
        [Display(Name = "Company Name")]
        public string Name { set; get; }

        [StringLength(250)]
        [Display(Name = "Name Additional Language")]
        public string NameAddiLang { set; get; }

        [Required]
        [StringLength(230)]
        [Display(Name = "Company Address")]
        public string Address { set; get; }

        [StringLength(300)]
        [Display(Name = "Address Additional Language")]
        public string AddressAddiLang { set; get; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { set; get; }

        [StringLength(100)]
        public string RegistrationNo { set; get; }

        [Required]
        [StringLength(100)]
        public string VatRegNo { set; get; }

        [StringLength(100)]
        public string WebAddress { set; get; }

        [StringLength(300)]
        public string Others { set; get; }

        [Required]
        [StringLength(50)]
        public string Phone { set; get; }

        public string Logo { set; get; }

        [NotMapped]
        public HttpPostedFileBase LogoFile { get; set; }

        [NotMapped]
        public int BusinessType { set; get; }


        [StringLength(300)]
        public string AddressHeadOffice { get; set; }

        [StringLength(300)]
        public string AddressHeadOfficeAddiLang { get; set; }

        [NotMapped]
        public int NoOfBranch { set; get; }

        /// <summary>
        /// 1 for including vat, 
        /// 2 for excluding vat
        /// </summary>
        [Range(1,2)]
        public int SalesPriceIncOrExcVat { set; get; }

    }



    [Table("SecUsers")]
    public class SecUser
    {
        public int Id { set; get; }
        public int? HrmEmployeeId { set; get; }

        [Required]
        [StringLength(110)]
        [Display(Name = "Login Name")]
        public string LoginName { set; get; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Login Password")]
        public string Password { set; get; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Status")]
        public bool Status { set; get; }

        [Required]
        public string TerminalId { set; get; }

        [StringLength(100)]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }

        [Required]
        [Display(Name = "Company Name")]
        public int CmnCompanyId { set; get; }

        /// <summary>
        /// sadmin for create all role/resource from resource table
        /// </summary>
        [StringLength(40)]
        [Display(Name = "User Type")]
        public string Type { set; get; }

        public int? CreateBy { set; get; }
        public int? ModifiedBy { set; get; }
        public DateTime? ModifiedDate { set; get; }
        public DateTime? CreateDate { set; get; }
        public CmnCompany CmnCompany { set; get; }

    }


    [Table("SecRoles")]
    public class SecRole
    {
        public int Id { set; get; }

        [Required]
        [StringLength(120)]
        [Display(Name = "Role Name")]
        public string Name { set; get; }
        [Display(Name = "Level")]
        public int Level { set; get; }
        [Display(Name = "User Id")]
        public int? SecUserId { set; get; }
        [Required]
        [Display(Name = "Status")]
        public bool Status { set; get; }
        public int CmnCompanyId { set; get; }
        public int? CreateBy { set; get; }
        public int? ModifiedBy { set; get; }
        public DateTime? ModifiedDate { set; get; }
        public DateTime? CreateDate { set; get; }
        public SecUser SecUser { set; get; }
    }

    [Table("SecUserRoles")]
    public class SecUserRole
    {
        public int? Id { set; get; }
        [Required]
        [Display(Name = "User Id")]
        public int SecUserId { set; get; }
        [Required]
        [Display(Name = "Role Id")]
        public int SecRoleId { set; get; }
        [Required]
        [Display(Name = "Status")]
        public bool Status { set; get; }
        public int? CreateBy { set; get; }
        public int? ModifiedBy { set; get; }
        public DateTime? ModifiedDate { set; get; }
        public DateTime? CreateDate { set; get; }
        public SecUser SecUser { set; get; }
        public SecRole SecRole { set; get; }


    }




    [Table("SecResources")]
    public class SecResource
    {
        public int Id { set; get; }
        [Required]
        [StringLength(120)]
        [Display(Name = "Resource Name")]
        public string Name { set; get; }
        [Required]
        [StringLength(120)]
        [Display(Name = "Display Name")]
        public string DisplayName { set; get; }
        [Display(Name = "Resource Id")]
        public int? SecResourceId { set; get; }
        [Display(Name = "Module Id")]
        public int? SecModuleId { set; get; }
        [Required]
        [Display(Name = "Status")]
        public bool Status { get; set; }
        public int Serial { get; set; }
        public int Level { set; get; }
        public int RoleLevel { set; get; }
        public bool IsReport { set; get; }
        public string Method { set; get; }
        public int? CreateBy { set; get; }
        public int? ModifiedBy { set; get; }
        public DateTime? ModifiedDate { set; get; }
        public DateTime? CreateDate { set; get; }
        public string IconUrl { set; get; }

    }


    [Table("SecResourcePermission")]
    public class SecResourcePermission
    {
        public int Id { set; get; }

        [Required]
        [StringLength(120)]
        [Display(Name = "Resource Name")]
        public string Name { set; get; }

        [Required]
        [StringLength(120)]
        [Display(Name = "Display Name")]
        public string DisplayName { set; get; }


        [Display(Name = "Resource Id")]
        public int SecResourceId { set; get; }

        [Required]
        [Display(Name = "Role Id")]
        public int SecRoleId { set; get; }

        [Display(Name = "Module Id")]
        public int? SecModuleId { set; get; }

        [Required]
        [Display(Name = "Status")]
        public bool Status { get; set; }

        public int Serial { get; set; }

        public int Level { set; get; }
        public int RoleLevel { set; get; }
        public bool IsReport { set; get; }
        public string Method { set; get; }
        public int? CreateBy { set; get; }
        public int? ModifiedBy { set; get; }
        public DateTime? ModifiedDate { set; get; }
        public DateTime? CreateDate { set; get; }
        public SecModule SecModule { set; get; }
        public SecResource SecResource { set; get; }
        public SecRole SecRole { set; get; }

    }


    [Table("SecRolePermissions")]
    public class SecRolePermission
    {
        public int Id { set; get; }

        [Required]
        [Display(Name = "Resource Id")]
        public int SecResourceId { set; get; }

        [Required]
        [Display(Name = "Role Id")]
        public int SecRoleId { set; get; }

        [Required]
        [Display(Name = "Add Permission")]
        public bool AddPermi { set; get; }

        [Required]
        [Display(Name = "Read Only Permission")]
        public bool ReadonlyPermi { set; get; }

        [Required]
        [Display(Name = "Edit Permission")]
        public bool EditPermi { set; get; }

        [Required]
        [Display(Name = "Delete Permission")]
        public bool DeletePermi { set; get; }

        [Required]
        [Display(Name = "Print Permission")]
        public bool PrintPermi { set; get; }
        public int? CreateBy { set; get; }
        public int? ModifiedBy { set; get; }
        public DateTime? ModifiedDate { set; get; }
        public DateTime? CreateDate { set; get; }
        public SecRole SecRole { set; get; }
        public  SecResource SecResource { set; get; }

    }




    [Table("SecUserBranch")]
    public class SecUserBranch : CommonPropertyWithIdentity
    {
        public int Id { set; get; }
        public int SecUserId { set; get; }

        public int PosBranchId { set; get; }
        public PosBranch PosBranch { set; get; }
    }

    [Table("SecLoginHistories")]
    public class SecLoginHistory
    {
        public int Id { set; get; }

        [StringLength(1000)]
        public string LoginDateTime { set; get; }

        public int SecUserId { set; get; }

        [StringLength(20)]
        public string IpAddress { set; get; }

        [StringLength(1000)]
        public string LicenseKey { set; get; }

        public int CmnCompanyId { set; get; }
    }


}