using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VModel
{
    public class VmBilling
    {
        public long PosProductId { set; get; }
        public long PosProductBatchId { set; get; }
        public long Qty { set; get; }
        public decimal Amount { set; get; }
    }

    public class VmSingleSchemeDiscount
    {
        public long PosProductId { set; get; }
        public long PosProductBatchId { set; get; }
        public decimal DiscountAmount { set; get; }
    }

    public class VmSchemeFreeProduct
    {
        public long PosProductId { set; get; }
        public string Code { set; get; }
        public string Name { set; get; }
        public long PosProductBatchId { set; get; }
        public string Batch { set; get; }
        public decimal Qty { set; get; }
    }


    public class VmGetBillingaItem
    {
        public long PosProductId { set; get; }
        public string ProductCode { set; get; }
        public long PosProductBatchId { set; get; }
        public string Name { set; get; }
        public decimal UnitPrice { set; get; }
        public decimal PurchaseRate { set; get; }
        public decimal Stock { set; get; }
        public decimal Vat { set; get; }
        public int SalesPriceIncOrExcVat { set; get; }
        public decimal Qty { set; get; }
        public decimal Discount { set; get; }
        public decimal OtherDiscount { set; get; }
        public int PosUomGroupId { set; get; }
        public  decimal Sd { set; get; }
        public bool IsPriceChangeable { set; get; }
        public string BatchName { set; get; }
        public int BatchCount { set; get; }

        public ICollection<VmUomDetails> UomDetails { set; get; }  

    }

    public class VmUomDetails
    {
      public  string UomCode { set; get; }
       public decimal  ConversionFactor { set; get; }
       public bool IsBaseUom { set; get; }
    }

    public class VmBatchDetails
    {
        public string BatchName { set; get; }
        public long PosProductBatchId { set; get; }
        public decimal SellingRate { set; get; }

    }

}
