using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using ERP.Models.Manager;
using ERP.Models.Security;

namespace ERP.Models.POS
{
    [Table("PosStockTypes")]
    public class PosStockType : CommonPropertyWithIdentity
    {
        public int Id { set; get; }

        [Required]
        [StringLength(250)]
        public string Name { set; get; }
        public bool IsBaseStock { set; get; }
    }

    [Table("PosStockDetails")]
    public class PosStockDetail
    {
        public long Id { set; get; }
        public long PosProductId { set; get; }

        public long PosProductBatchId { set; get; }

        [Required]
        public decimal Qty { set; get; }

        public int PosStockTypeId { set; get; }

        [Range(-999999999999999999.999, 999999999999999999.999)]
        public decimal? Discount { set; get; }

        [Range(-999999999999999999.999, 999999999999999999.999)]
        public decimal? PurchaseTax { set; get; }

        public long PosStockId { set; get; }

        public PosProduct PosProduct { set; get; }
        public PosProductBatch PosProductBatche { set; get; }
        public PosStockType PosStockType { set; get; }

        public PosStock PosStock { set; get; }
    }

    public class PosStock : CommonPropertyWithoutIdAndCompanyId
    {
        public long Id { set; get; }

        [StringLength(100)]
        public string InvReferenceNo { set; get; }

        [Required]
        [StringLength(100)]
        [Index("UK_PosStock_CompanyInvNo_PosStockTransactionType_CmnCmpanyId", 1, IsUnique = true)]
        public string CompanyInvNo { set; get; }

        [Required]
        public int PosSupplierId { set; get; }

        [Column(TypeName = "date")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InvDate { set; get; }

        [Required]
        [Column(TypeName = "date")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InvReceiveDate { set; get; }

        [Range(-999999999999999999.99, 999999999999999999.99)]
        public decimal? OtherDiscount { set; get; }

        [Range(-999999999999999999.99, 999999999999999999.99)]
        public decimal? LessDiscunt { set; get; }

        [Range(-999999999999999999.99, 999999999999999999.99)]
        public decimal? TotalTax { set; get; }

        [Range(-999999999999999999.99, 999999999999999999.99)]
        public decimal? OtherCharges { set; get; }

        [Required]
        [Range(-999999999999999999.99, 999999999999999999.99)]
        public decimal Netvalue { set; get; }

        [Required]
        [Range(-999999999999999999.99, 999999999999999999.99)]
        public decimal NetPayable { set; get; }


        [Index("UK_PosStock_CompanyInvNo_PosStockTransactionType_CmnCmpanyId", 2, IsUnique = true)]
        public int CmnCompanyId { set; get; }

        public int PosBranchId { set; get; }

        [StringLength(1000)]
        public string Remarks { set; get; }

        /// <summary>
        /// 1 for =purchase/stock transfer
        /// 2 for =transfer form location/ reduce from
        /// 3 for =Transfer to location/Add To 
        /// 4 for =stock adjustment-Add
        /// 5 for =stock adjustment-Reduce
        /// </summary>
        [Index("UK_PosStock_CompanyInvNo_PosStockTransactionType_CmnCmpanyId", 3, IsUnique = true)]
        public int PosStockTransactionType { set; get; }

        public PosSupplier PosSupplier { set; get; }
        public CmnCompany CmnCompany { set; get; }
        public PosBranch PosBranch { set; get; }
        public ICollection<PosStockDetail> PosStockDetail { set; get; }

    }


    [Table("PosVwPurchaseReceipt")]
    public class PosVwPurchaseReceipt
    {
        public long Id { set; get; }
        public long PosProductId { set; get; }
        public int PosProductCategoryId { set; get; }
        public string ProductCode { set; get; }
        public string ProductBarCode { set; get; }
        public string ProductName { set; get; }
        public long PosProductBatchId { set; get; }
        public string BatchName { set; get; }
        public decimal SellingRate { set; get; }
        public decimal PurchaseRate { set; get; }
        public string Mrp { set; get; }
        public int Weight { set; get; }
        public decimal Qty { set; get; }
        public int PosStockTypeId { set; get; }
        public string StockType { set; get; }
        public decimal? Discount { set; get; }
        public decimal? DiscountPer { set; get; }
        public decimal? PurchaseTax { set; get; }
        public long PosStockId { set; get; }
        public int CmnCompanyId { set; get; }
        public DateTime InvDate { set; get; }
        public DateTime InvReceiveDate { set; get; }
        public string CompanyInvNo { set; get; }
        public string InvReferenceNo { set; get; }
        public int PosBranchId { set; get; }
        public string BranchName { set; get; }
        public int PosUomGroupId { set; get; }

        [NotMapped]
        public string UomCode { set; get; }

        [NotMapped]
        public int PosUomMasterId { set; get; }

        public int PosSupplierId { set; get; }
        public decimal Vat { set; get; }

        public PosProduct PosProduct { set; get; }
        public PosProductBatch PosProductBatch { set; get; }
        public PosStockType PosStockType { set; get; }
        public PosStock PosStock { set; get; }
        public PosBranch PosBranch { set; get; }
        public PosUomGroup PosUomGroup { set; get; }
        //public PosUomMaster PosUomMaster { set; get; }
        public PosSupplier PosSupplier { set; get; }
    }

    public class PosCustomerDueCollection : CommonPropertyWithoutIdAndCompanyId
    {
        public int Id { get; set; }

        [ForeignKey("PosSalesInvoice")]
        public long PosSalesInvoiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("CmnCompany")]
        public int CmnCompanyId { get; set; }

        public PosSalesInvoice PosSalesInvoice { get; set; }
        public CmnCompany CmnCompany { get; set; }
    }


}