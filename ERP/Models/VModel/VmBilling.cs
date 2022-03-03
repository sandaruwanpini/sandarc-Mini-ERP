using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Controllers.Manager;
using ERP.Migrations;

namespace ERP.Models.VModel
{
    public class VmGetInvoice
    {
        public long InvoiceNumber { set; get; }
        public PosInvoiceType PosInvoiceType { set; get; }
        public decimal Discount { set; get; }
        public decimal OtherDiscount { set; get; }
        public DateTime InvDate { set; get; }
        public decimal DueAmount { set; get; }
        public decimal VatTotal { set; get; }
        public decimal TotalAmt { set; get; }
        public decimal MrpTotal { set; get; }
        public decimal SdTotal { set; get; }
        public long CustomerNo { set; get; }
        public string Remarks { set; get; }

        public List<VmSalesInviceProduct> SalesInviceProduct { set; get; }
        public List<VmSalesInvoiceFreeProduct> SalesInvoiceFreeProduct { set; get; }
        public List<VmSalesInvoiceTender> SalesInvoiceTender { set; get; }
    }

    public class VmSalesInvoiceFreeProduct
    {
        public string Code { set; get; }
        public long PosProductId { set; get; }
        public long PosProductBatchId { set; get; }
        public int ManualQty { set; get; }
        public string Name { set; get; }
        public string Batch { set; get; }
        public decimal Qty { set; get; }
    }

    public class VmSalesInviceProduct
    {
        public string ProductCode { set; get; }
        public string Name { set; get; }
        public long PosProductId { set; get; }
        public long PosProductBatchId { set; get; }
        public decimal Qty { set; get; }
        public decimal Sd { set; get; }
        public decimal Vat   { set; get; }
        public decimal Discount { set; get; }
        public decimal OtherDiscount { set; get; }
        public decimal UnitPrice { set; get; }
        public decimal PurchaseRate { set; get; }
        public decimal Stock { set; get; }
        public decimal VatPar { set; get; }
        public decimal SdPar { set; get; }
        public string BatchName { set; get; }
        public bool IsPriceChangeable { set; get; }
        public List<VmUomDetails> UomDetails { set; get; }
   }

    public class VmSalesInvoiceTender
    {
        public int PosTenderId { set; get; }
        public decimal TenderAmount { set; get; }
        public string Remarks { set; get; }
    }



}