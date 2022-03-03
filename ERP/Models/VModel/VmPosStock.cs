using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERP.Models.VModel
{
    public class VmProductDetails
    {
        public string Code { set; get; }
        public string Name { set; get; }
        public decimal PurchaseRate { set; get; }
        public long PosProductBatchId { set; get; }
        public int PosUomGroupId { set; get; }
        public int PosUomMasterId { set; get; }
        
    }
    public class VmProductDetailsForStockTransfer
    {
        public string Code { set; get; }
        public string Name { set; get; }
        public decimal PurchaseRate { set; get; }
        public long PosProductBatchId { set; get; }
        public decimal StockOnHand { set; get; }
    }

    public class VmGetPosCurrentStockByLocationOnly
    {
        public string Location { set; get; }
        public decimal StockQty { set; get; }
    }


    public class VmLocationTransfer
    {
        public int PosSupplierId { set; get; }
        public int FromLocation { set; get; }
        public int ToTocation { set; get; }
        public int FromPosBranchId { set; get; }
        public int ToPosBranchId { set; get; }
        public DateTime InvDate { set; get; }
        public string CompanyInvNo { set; get; }
        public string Remarks { set; get; }
        public List<VmLocationTransferDetails> LocationTransferDetails { set; get; }
    }

    public class VmLocationTransferDetails
    {
        public string ProductCode { set; get; }
        public long PosProductBatchId { set; get; }
        public decimal Qty { set; get; }
    }


    public class VmStockAdjustment
    {
        public int PosSupplierId { set; get; }
        public int Location { set; get; }
        public int PosBranchId { set; get; }
        public int TranactionType { set; get; }
        public DateTime InvDate { set; get; }
        public string CompanyInvNo { set; get; }
        public string Remarks { set; get; }
        public List<VmStockAdjustmentDetails> StockAdjustmentDetails { set; get; }
    }

    public class VmStockAdjustmentDetails
    {
        public string ProductCode { set; get; }
        public long PosProductBatchId { set; get; }
        public decimal Qty { set; get; }
    }


}