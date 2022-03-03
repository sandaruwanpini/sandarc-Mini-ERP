using ERP.Models.Manager;
using ERP.Models.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models.POS
{
    [Table("PosVwProductWithBatch")]
    public class PosVwProductWithBatch
    {
        public Int64 Id { set; get; }
        public long PosProductId { set; get; }
        public string Code { set; get; }
        public string BarCode { set; get; }
        public string Name { set; get; }
        public string NameInOtherLang { set; get; }
        public string BatchName { set; get; }
        public string Mrp { set; get; }
        public decimal PurchaseRate { set; get; }
        public decimal SellingRate { set; get; }
        public int CmnCompanyId { set; get; }

    }

    [Table("PosProducts")]
    public class PosProduct:CommonPropertyWithoutIdAndCompanyId
    {
        public Int64 Id { set; get; }

        [Required]
        [StringLength(250)]
        [Index("UK_PosProduct_Code_Name_CmnCompanyId", 1, IsUnique = true)]
        [Index("UK_PosProduct_Code_CompanyCode_CmnCompanyId", 1, IsUnique = true)]
        [Index("UK_PosProduct_Code_ShortName_CmnCompanyId", 1, IsUnique = true)]
        public string Code { set; get; }

        [Required]
        [StringLength(250)]
        [Index("UK_PosProduct_Code_CompanyCode_CmnCompanyId", 2, IsUnique = true)]
        public string CompanyCode { set; get; }

        [Required]
        [StringLength(1000)]
        [Index("UK_PosProduct_Code_Name_CmnCompanyId", 2, IsUnique = true)]
        public string Name { set; get; }

        
        [StringLength(1000)]
        public  string NameInOtherLang { set; get; }

        [Required]
        [StringLength(1000)]
        [Index("UK_PosProduct_Code_ShortName_CmnCompanyId", 2, IsUnique = true)]
        public string ShortName { set; get; }

        
        public int? PosSupplierId { set; get; }

        public int PosUomGroupId { set; get; }

        [ForeignKey("ProductCategory")]
        public int PosProductCategoryId { set; get; }

        public byte[] Image { set; get; }

        [Range(0.00, 99.99)]
        public decimal Vat { set; get; }

        [Range(0.00, 99.99)]
        public decimal Sd { set; get; }

        public bool IsHideToStock { set; get; }

        public bool IsPriceChangeable { set; get; }

        [Index("UK_PosProduct_Code_Name_CmnCompanyId", 3, IsUnique = true)]
        [Index("UK_PosProduct_Code_CompanyCode_CmnCompanyId", 3, IsUnique = true)]
        [Index("UK_PosProduct_Code_ShortName_CmnCompanyId", 3, IsUnique = true)]
        public int CmnCompanyId { set; get; }
        
        public CmnCompany Company { set; get; }
        public PosProductCategory ProductCategory { set; get; }
        public PosUomGroup PosUomGroup { set; get; }
        public PosSupplier PosSupplier { set; get; }
        public ICollection<PosProductBatch> PosProductBatch { set; get; } 
        public ICollection<PosStockDetail> PosStockDetail { set; get; }
        public ICollection<PosVwPurchaseReceipt> PosVwPurchaseReceipt { set; get; } 
        public ICollection<PosSalesInvoiceProduct> PosSalesInvoiceProduct { set; get; } 

        
    }

    [Table("PosSuppliers")]
    public class PosSupplier:CommonPropertyWithIdentity
    {
        public int Id { set; get; }

        [Required]
        [StringLength(1000)]
        public string Name { set; get; }

        [Required]
        [StringLength(5000)]
        public string Address { set; get; }

        [StringLength(100)]
        public string Phone { set; get; }

        [StringLength(100)]
        public string Fax { set; get; }
        
        [StringLength(200)]
        public string Email { set; get; }
    }

    [Table("PosProductBatch")]
    public class PosProductBatch: CommonPropertyWithoutIdAndCompanyId
    {
        public Int64 Id { set; get; }

       [Index("UK_PosProductId_BatchName_Mrp_CmnCompanyId",1,IsUnique = true)]
        public Int64 PosProductId { set; get; }

        [Required]
        [StringLength(100)]
        [Index("UK_PosProductId_BatchName_Mrp_CmnCompanyId",2, IsUnique = true)]
        public string BatchName { set; get; }

        [Required]
        [StringLength(100)]
        [Index("UK_PosProductId_BatchName_Mrp_CmnCompanyId",3, IsUnique = true)]
        public string Mrp { set; get; }

        [Required]
        [Range(0.001, 999999999999999999.999)]
        public decimal PurchaseRate { set; get; }

        [Required]
        [Range(0.001, 999999999999999999.999)]
        public decimal SellingRate { set; get; }

        
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateFrom { set; get; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateTo { set; get; }

        [Index("UK_PosProductId_BatchName_Mrp_CmnCompanyId", 4, IsUnique = true)]
        public int CmnCompanyId { set; get; }
        
        public int Weight { set; get; }

        [StringLength(100)]
        public string BarCode { set; get; }

        public CmnCompany CmnCompany { set; get; }
        public PosProduct PosProduct { set; get; }
    }


    [Table("PosUomMaster")]
    public class PosUomMaster : CommonPropertyWithoutIdAndCompanyId
    {
        public int Id { set; get; }

        [Required]
        [StringLength(50)]
        public string UomCode { set; get; }

        public bool IsBaseUom { set; get; }

        [Required]
        [StringLength(500)]
        public string UomDescription { set; get; }

        public int CmnCompanyId { set; get; }


        public CmnCompany CmnCompany { set; get; }
    }

    [Table("PosUomGroup")]
    public class PosUomGroup:CommonPropertyWithIdentity
    {
        public int Id { set; get; }

        [Required]
        [StringLength(250)]
        public string Name { set; get; }

        public ICollection<PosProduct> PosProducts { set; get; } 
        public ICollection<PosUomGroupDetail> PosUomGroupDetails { set; get; }
        public ICollection<PosVwUomDetail> PosVwUomDetail { set; get; }
    }


    [Table("PosUomGroupDetails")]
    public class PosUomGroupDetail
    {
        public Int64 Id { set; get; }

        [Index("UK_PosUomGroupId_PosUomMaster_CmnCmpanyId",1,IsUnique = true)]
        public int PosUomGroupId { set; get; }

        [Index("UK_PosUomGroupId_PosUomMaster_CmnCmpanyId", 2, IsUnique = true)]
        public int PosUomMasterId { set; get; }

        public decimal ConversionFactor { set; get; }

        [Index("UK_PosUomGroupId_PosUomMaster_CmnCmpanyId", 3, IsUnique = true)]
        public int CmnCompanyId { set; get; }

        public  CmnCompany CmnCompany { set; get; }
        public PosUomMaster PosUomMaster { set; get; }
        public PosUomGroup PosUomGroup { set; get; }
    }

    [Table("PosBranch")]
    public class PosBranch : CommonPropertyWithoutIdAndCompanyId
    {
        public int Id { set; get; }

        [Required]
        [StringLength(250)]
        [Index("UK_PosBranch_Name_CmnCmpanyId", 1, IsUnique = true)]
        public string Name { set; get; }

        [Required]
        [StringLength(500)]
        public string Address { set; get; }


        [StringLength(100)]
        public string Phone { set; get; }


        [StringLength(100)]
        public string Mobile { set; get; }

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { set; get; }

        [StringLength(100)]
        public string Fax { set; get; }


        [StringLength(500)]
        public string Remarks { set; get; }

        [Index("UK_PosBranch_Name_CmnCmpanyId", 2, IsUnique = true)]
        public int CmnCompanyId { set; get; }
        public  CmnCompany CmnCompany { set; get; }
    }

    [Table("PosVwUomDetails")]
    public class PosVwUomDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public int PosUomGroupId { get; set; }
        public string UomGroupName { get; set; }
        public int PosUomMasterId { get; set; }
        public bool IsBaseUom { get; set; }
        public string UomCode { get; set; }
        public decimal ConversionFactor { get; set; }
        public int CmnCompanyId { get; set; }

        public PosUomGroup PosUomGroup { set; get; }
        public PosUomMaster PosUomMaster { set; get; }
    }

    public class ItemRegister
    {
        public PosProduct Product { get; } = new PosProduct();
        public PosProductBatch ProductBatch { get; } = new PosProductBatch();

    }

    public class PosBillingReportText : CommonPropertyWithIdentity
    {
        public int Id { set; get; }
        public int PosBranchId { set; get; }

        [Required]
        [StringLength(200)]
        public string PoweredBy { set; get; }

        [Required]
        [StringLength(4000)]
        public string Text { set; get; }


        public PosBranch PosBranch { set; get; }
    }

}