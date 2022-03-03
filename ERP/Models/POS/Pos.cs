using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Controllers.Manager;
using ERP.Models.Manager;
using ERP.Models.Security;

namespace ERP.Models.POS
{
    [Table("PosBillprintTemplate")]
    public class PosBillprintTemplate
    {
        public int Id { set; get; }
     
       [StringLength(250)]
        public string Name { set; get; }
        [StringLength(250)]
        public string Method { set; get; }
        public string Image { set; get; }
        [StringLength(500)]
        public string Remarks { set; get; }
    }

    [Table("PosBillprintTemplateOfBranch")]
    public class PosBillprintTemplateOfBranch : CommonPropertyWithoutIdAndCompanyId
    {
        public int Id { set; get; }

        //1 for stock transfer
        //2 for billing
        public int TemplateType { set; get; }

        [Index("UK_PosBillprintTemplateOfBranch_CmnCompanyId_PosBranchId_PosBillprintTemplateId", 1, IsUnique = true)]
        public int PosBillprintTemplateId { set; get; }

        [Index("UK_PosBillprintTemplateOfBranch_CmnCompanyId_PosBranchId_PosBillprintTemplateId", 2, IsUnique = true)]
        public int PosBranchId { set; get; }

        [StringLength(500)]
        public string Remarks { set; get; }

        [Index("UK_PosBillprintTemplateOfBranch_CmnCompanyId_PosBranchId_PosBillprintTemplateId", 3, IsUnique = true)]
        public int CmnCompanyId { set; get; }

        public CmnCompany CmnCompany { set; get; }

        public PosBillprintTemplate PosBillprintTemplate { set; get; }
    }


    [Table("PosTenders")]
    public class PosTender : CommonPropertyWithoutIdAndCompanyId
    {
        public int Id { set; get; }

        [StringLength(250)]
        [Index("UK_PosTenders_Name_Type_CmnCompanyId", 1, IsUnique = true)]
        [DisplayName("Payment method")]
        public string Name { set; get; }

        public int Order { set; get; }

        /// <summary>
        /// NA for default
        /// R round off,
        /// PA for Paidable amount,
        /// CR for Cash Receipt,
        /// CA for change Amount,
        /// DA for due amount
        /// </summary>
        [StringLength(2)]
        [Index("UK_PosTenders_Name_Type_CmnCompanyId", 2, IsUnique = true)]
        public string Type { set; get; }

        public bool IsEditable { set; get; }

        public int? PosTenderTypeId { set; get; }

        [Index("UK_PosTenders_Name_Type_CmnCompanyId", 3, IsUnique = true)]
        public int CmnCompanyId { set; get; }

        public CmnCompany CmnCompany { set; get; }

        public PosTenderType PosTenderType { set; get; }


    }

    [Table("PosTenderTypes")]
    public class PosTenderType:CommonPropertyWithoutIdAndCompanyId
    {
        public int Id { set; get; }

        [StringLength(250)]

        [Index("UK_PosTenderTypes_Name_CmnCompanyId", 1, IsUnique = true)]
        [DisplayName("Payment Type")]
        public string Name { set; get; }

        [Index("UK_PosTenderTypes_Name_CmnCompanyId", 2, IsUnique = true)]
        public int CmnCompanyId { set; get; }
        public bool IsEditable { set; get; }
        public CmnCompany CmnCompany { set; get; }

    }

    [Table("PosSalesInvoice")]
    public class PosSalesInvoice : CommonPropertyWithoutIdAndCompanyId
    {
        public long Id { set; get; }

        [Column(TypeName = "date")]
        public DateTime InvDate { set; get; }

        [Index("UK_PosSalesInvoice_InvoiceNumber_CmnCompanyId", 1, IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long InvoiceNumber { set; get; }

        public long PosCustomerId { set; get; }
        public decimal PaidableAmount { set; get; }
        public decimal ReceivedAmount { set; get; }
        public decimal DueAmount { set; get; }
        public decimal MrpTotal { set; get; }
        public decimal SdTotal { set; get; }
        public decimal VatTotal { set; get; }
        public decimal Discount { set; get; }
        public decimal OtherDiscount { set; get; }
        public decimal TotalAmt { set; get; }

        [Range(1,6,ErrorMessage = "Only allow 1 to 6 value only")]
        public PosInvoiceType PosInvoiceType { set; get; }

        /// <summary>
        /// true=if the branch transfer stock received
        /// other wise=false
        /// </summary>
        public bool IsReceiveTransferStock { set; get; }

        [DefaultValue("False")]
        public bool IsDuePaid { set; get; } = false;

        public int PosBranchId { set; get; }
        public PosBranch PosBranch { set; get; }
        public PosCustomer PosCustomer { set; get; }

        [Index("UK_PosSalesInvoice_InvoiceNumber_CmnCompanyId", 2, IsUnique = true)]
        public int CmnCompanyId { set; get; }

        [StringLength(500)]
        public string Remarks { set; get; }
        public long PosInvoiceGlobalFileNumberId { set; get; }
        public PosInvoiceGlobalFileNumber PosInvoiceGlobalFileNumber { set; get; }

        public CmnCompany CmnCompany { set; get; }
        public ICollection<PosSalesInvoiceFreeProduct> PosSalesInvoiceFreeProduct { set; get; }
        public ICollection<PosSalesInvoiceProduct> PosSalesInvoiceProduct { set; get; }
        public ICollection<PosSalesInvoiceTender> PosSalesInvoiceTender { set; get; }
        public ICollection<PosCustomerDueCollection> PosCustomerDueCollections { set; get; }

    }

    [Table("PosInvoiceGlobalFileNumber")]
    public class PosInvoiceGlobalFileNumber
    {
        public long Id { set; get; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Index("UK_PosInvoiceGlobalFileNumber_InvoiceGlobalFileNumber", IsUnique = true)]
        public long InvoiceGlobalFileNumber { set; get; }
        public int CmnCompanyId { set; get; }
        public CmnCompany CmnCompany { set; get; }

    }

    [Table("PosSalesInvoiceTenders")]
    public class PosSalesInvoiceTender
    {
        public long Id { set; get; }
        public long PosSalesInvoiceId { set; get; }
        public int PosTenderId { set; get; }
        public decimal TenderAmount { set; get; }

        [StringLength(500)]
        public string Remarks { set; get; }
        public PosSalesInvoice PosSalesInvoice { set; get; }
        public PosTender PosTender { set; get; }

    }

    [Table("PosSalesInvoiceProducts")]
    public class PosSalesInvoiceProduct : CommonPropertyWithoutIdAndCompanyId
    {
        public long Id { set; get; }

        public long PosProductId { set; get; }
        public long PosProductBatchId { set; get; }
        public decimal Qty { set; get; }
        public decimal Rate { set; get; }
        public decimal Vat { set; get; }
        public  decimal Sd { set; get; }
        public decimal SchDiscount { set; get; }
        public decimal OtherDiscount { set; get; }
        public long PosSalesInvoiceId { set; get; }

        public PosSalesInvoice PosSalesInvoice { set; get; }
        public PosProduct PosProduct { set; get; }
        public PosProductBatch PosProductBatch { set; get; }

    }

    [Table("PosSalesInvoiceFreeProducts")]
    public class PosSalesInvoiceFreeProduct 
    {
        public long Id { set; get; }

        public long PosProductId { set; get; }
        public long PosProductBatchId { set; get; }
        public decimal Qty { set; get; }
        public int ManualQty { set; get; }
        public long PosSalesInvoiceId { set; get; }

        public PosSalesInvoice PosSalesInvoice { set; get; }
        public PosProduct PosProduct { set; get; }
        public PosProductBatch PosProductBatch { set; get; }

    }

    [Table("PosCustomers")]
    public class PosCustomer : CommonPropertyWithoutIdAndCompanyId
    {
        public long Id { set; get; }
        public int PosCustomerClassId { set; get; }

        [Index("UK_PosCustomer_CustomerNo_CmnCompanyId", 1, IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long CustomerNo { set; get; }

        [StringLength(150)]
        public string FirstName { set; get; }

        [StringLength(150)]
        public string LastName { set; get; }

        [Required]
        [StringLength(20)]
        [Index("UK_PosCustomer_Phone_CmnCompanyId", 1, IsUnique = true)]
        public string Phone { set; get; }

        [StringLength(20)]
        public string AdditionalPhone { set; get; }

        [StringLength(250)]
        public string Address { set; get; }

        [StringLength(250)]
        public string Address2 { set; get; }

        public int PosRegionId { set; get; }
        public int PosCityOrNearestZoneId { set; get; }
        public bool IsPointAllowable { set; get; }
        public bool IsDueAllowable { set; get; }
        public bool IsDefaultPosCustomer { set; get; }
        public bool IsPosBranchCustomer { set; get; }
        public int PosBranchId { set; get; }
        public PosBranch PosBranch { set; get; }

        [Index("UK_PosCustomer_CustomerNo_CmnCompanyId", 2, IsUnique = true)]
        [Index("UK_PosCustomer_Phone_CmnCompanyId", 2, IsUnique = true)]
        public int CmnCompanyId { set; get; }

        public CmnCompany CmnCompany { set; get; }
        public PosRegion PosRegion { set; get; }
        public PosCityOrNearestZone PosCityOrNearestZone { set; get; }
        public PosCustomerClass PosCustomerClass { set; get; }
    }

    [Table("PosRegion")]
    public class PosRegion : CommonPropertyWithoutIdAndCompanyId
    {
        public int Id { set; get; }

        [Required]
        [StringLength(250)]
        [Index("UK_PosRegion_Name_CmnCompanyId", 1, IsUnique = true)]
        public string Name { set; get; }
        [Index("UK_PosRegion_Name_CmnCompanyId", 2, IsUnique = true)]
        public int CmnCompanyId { set; get; }
        public CmnCompany CmnCompany { set; get; }
    }

    [Table("PosCityOrNearestZone")]
    public class PosCityOrNearestZone : CommonPropertyWithoutIdAndCompanyId
    {
        public int Id { set; get; }

        [Required]
        [StringLength(250)]
        [Index("UK_PosCityOrNearestZone_Name_CmnCompanyId", 1, IsUnique = true)]
        public string Name { set; get; }

        [Index("UK_PosCityOrNearestZone_Name_CmnCompanyId", 2, IsUnique = true)]
        public int CmnCompanyId { set; get; }

        public CmnCompany CmnCompany { set; get; }

    }

    [Table("PosCustomerClass")]
    public class PosCustomerClass:CommonPropertyWithoutIdAndCompanyId
    {
        public int Id { set; get; }
        [Index("UK_PosCustomerClass_Name_CmnCompanyId", 1, IsUnique = true)]
        [StringLength(100)]
        public string Name { set; get; }
        public bool Status { set; get; }
        [Index("UK_PosCustomerClass_Name_CmnCompanyId", 2, IsUnique = true)]
        public int CmnCompanyId { set; get; }
        public CmnCompany CmnCompany { set; get; }
    }

    [Table("PosProductCategories")]
    public class PosProductCategory : CommonPropertyWithoutIdAndCompanyId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required]
        [StringLength(250)]
        [Index("UK_PosCategory_Name_CmnCompanyId", 1, IsUnique = true)]
        public string Name { set; get; }
        public string Description { get; set; }

        public ICollection<PosProduct> PosProducts { set; get; }

        [Index("UK_PosCategory_Name_CmnCompanyId", 2, IsUnique = true)]
        public int CmnCompanyId { set; get; }
        public CmnCompany CmnCompany { set; get; }
    }

    [Table("PosSalesProductWarrantyIssue")]
    public class PosSalesProductWarrantyIssue:CommonPropertyWithoutIdAndCompanyId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id   { get; set; }
        public long PosSalesInvoiceProductId { get; set; }


        [StringLength(500)]
        public string SerialOrImeiNo { set; get; }

        /// <summary>
        /// days month or year
        /// </summary>
        public int WarrantyPeriod { get; set; }

        /// <summary>
        /// 1 =Year warranty
        /// 2=month warranty
        /// 3=days warranty
        /// 4=year guarantee
        /// 5=month guarantee
        /// 6=days guarantee
        /// </summary>
        public int WarrantyType { set; get; }

        public int CmnCompanyId { set; get; }

        [StringLength(250)]
        public string Remarks { set; get; }

        public CmnCompany CmnCompany { set; get; }

        public PosSalesInvoiceProduct PosSalesInvoiceProduct { set; get; }
    }
}