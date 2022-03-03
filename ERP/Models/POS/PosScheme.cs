using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Design;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using ERP.Models.Manager;
using ERP.Models.Security;

namespace ERP.Models.POS
{
    [Table("PosScheme")]
    public class PosScheme : CommonPropertyWithoutIdAndCompanyId
    {
        public long Id { set; get; }

        [Index("UK_PosScheme_SchemeCode_CmnCompanyId", 1, IsUnique = true)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long SchemeCode { set; get; }

        [StringLength(500)]
        public string Description { set; get; }

        public bool IsCombiScheme { set; get; }

        ///1 for quantity,2 for Amount/DISCOUNT
        public int SchemeType { set; get; }
        [Column(TypeName = "date")]
        public DateTime DateFrom { set; get; }
        [Column(TypeName = "date")]
        public DateTime DateTo { set; get; }
        public bool Status { set; get; }

        [Index("UK_PosScheme_SchemeCode_CmnCompanyId", 2, IsUnique = true)]
        public int CmnCompanyId { set; get; }

        public CmnCompany CmnCompany { set; get; }
        public ICollection<PosSchemeProduct> PosSchemeProduct { set; get; }  
    }

    [Table("PosSchemeBranch")]
    public class PosSchemeBranch
    {
        public long Id { set; get; }

        [Index("UK_PosSchemeBranch_PosSchemeId", 1, IsUnique = true)]
        public long PosSchemeId { set; get; }

        [Index("UK_PosSchemeBranch_PosSchemeId", 2, IsUnique = true)]
        //Key needed
        public int PosBranchId { set; get; }

        public PosBranch PosBranch { set; get; }
        public PosScheme PosScheme { set; get; }

    }

    [Table("PosSchemeCustomerClass")]
    public class PosSchemeCustomerClass
    {
        public long Id { set; get; }

        [Index("UK_PosSchemeCustomerClass_PosCustomerClassId_PosSchemeId", 1, IsUnique = true)]
        public long PosSchemeId { set; get; }

        [Index("UK_PosSchemeCustomerClass_PosCustomerClassId_PosSchemeId", 2, IsUnique = true)]
        public int PosCustomerClassId { set; get; }

        public PosCustomerClass PosCustomerClass { set; get; }
        public PosScheme PosScheme { set; get; }
    }

    [Table("PosSchemeProduct")]
    public class PosSchemeProduct
    {
        public long Id { set; get; }

        [Index("UK_PosSchemeProduct_PosSchemeId_PosProductId_PosProductBatchId", 1, IsUnique = true)]
        public long PosSchemeId { set; get; }

        [Index("UK_PosSchemeProduct_PosSchemeId_PosProductId_PosProductBatchId", 2, IsUnique = true)]
        public long PosProductId { set; get; }

        [Index("UK_PosSchemeProduct_PosSchemeId_PosProductId_PosProductBatchId", 3, IsUnique = true)]
        public long PosProductBatchId { set; get; }

        public PosScheme PosScheme { set; get; }
        public PosProduct PosProduct { set; get; }
        public PosProductBatch PosProductBatch { set; get; }
    }

    [Table("PosSchemeSlabs")]
    public class PosSchemeSlab
    {
        public long Id { set; get; }
        public long PosSchemeId { set; get; }
        //use only discount
        public decimal PurQtyOrAmt { set; get; }
        public int DiscountPer { set; get; }
        public int FlatAmt { set; get; }
        public PosScheme PosScheme { set; get; }

    }


    public class PosSchemeSlabFreeProduct
    {
        public long Id { set; get; }

        [Index("UK_PosSchemeSlabFreeProduct_PosSchemeId_PosSchemeSlabId_PosProductId_PosProductBatchId", 1, IsUnique = true)]
        public long PosSchemeId { set; get; }

        [Index("UK_PosSchemeSlabFreeProduct_PosSchemeId_PosSchemeSlabId_PosProductId_PosProductBatchId", 2, IsUnique = true)]
        public long PosSchemeSlabId { set; get; }

        [Index("UK_PosSchemeSlabFreeProduct_PosSchemeId_PosSchemeSlabId_PosProductId_PosProductBatchId", 3, IsUnique = true)]
        public long PosProductId { set; get; }

        [Index("UK_PosSchemeSlabFreeProduct_PosSchemeId_PosSchemeSlabId_PosProductId_PosProductBatchId", 4, IsUnique = true)]
        public long PosProductBatchId { set; get; }

        public int Qty { set; get; }
        public int PosUomMasterId { set; get; }

        /// <summary>
        /// 0 for AND ,1 for OR 
        /// </summary>
        public int Availability { set; get; }

        public PosScheme PosScheme { set; get; }
        public PosSchemeSlab PosSchemeSlab { set; get; }
        public PosProduct PosProduct { set; get; }
        public PosProductBatch PosProductBatch { set; get; }
        public PosUomMaster PosUomMaster { set; get; }

    }
}