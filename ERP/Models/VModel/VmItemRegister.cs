using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VModel
{
    public class ViewModelItemBatch
    {
        public Int64 Id { set; get; }
        public string BatchName { set; get; }
        public string Mrp { set; get; }
        public decimal PurchaseRate { set; get; }
        public decimal SellingRate { set; get; }
        public DateTime? DateFrom { set; get; }
        public DateTime? DateTo { set; get; }
        public int CmnCompanyId { set; get; }
        public int Weight { set; get; }
        public string BarCode { set; get; }
    }
}