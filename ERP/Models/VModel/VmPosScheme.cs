using ERP.Models.POS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VModel
{
    public class VmPosScheme
    {
        public long Id { get; set;}
        public long SchemeCode { get; set; }
        public string Description { get; set; }
        public bool IsCombiScheme { get; set; }
        public int SchemeType { set; get; }
        public DateTime DateFrom { set; get; }
        public DateTime DateTo { set; get; }
        public bool Status { set; get; }
        public List<int> PosBranch { set; get; }
        public List<int> CusClass { set; get; }
        public List<VmPosSchemePrd> PosProducts { set; get; }
        public List<VmPosSchemePrdSchemeSlab> PosSchemeSlab { set; get; }
    }

    public class VmPosSchemePrd
    {        
        public long ProductId { set; get; }
        public long ProductBatchId { set; get; }
    }

    public class VmPosSchemePrdSchemeSlab
    {
        public decimal PurQtyOrAmt { set; get; }
        public int DiscountPer { set; get; }
        public int FlatAmt { set; get; }
        public List<VmPosSchemePrdSchemeSlabFree> Products { set; get; }
    }

    public class VmPosSchemePrdSchemeSlabFree
    {        
        public long PosProductId { set; get; }
        public string PosProductName { set; get; }
        public long PosProductBatchId { set; get; }
        public string PosProductBatchName { set; get; }
        public int Qty { set; get; }
        public int PosUomMasterId { set; get; }
        public int Availability { set; get; }

    }
}