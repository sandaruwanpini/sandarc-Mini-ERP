using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VModel
{
    public class VmProductList
    {
        public string Code { set; get; }
        public string Name { set; get; }
        public string Batch { set; get; }
        public long PosProductBatchId { set; get; }

        public decimal Qty { set; get; }
        public decimal SchDiscount { set; get; }
        public decimal Sd { set; get; }
        public decimal UnitPrice { set; get; }
        public decimal PurchaseRate { set; get; }

        public decimal Vat { set; get; }
        public string SerialOrImeiNo { set; get; }
        public string WarrantyPeriod { set; get; }
    }

    public class VmInvoice
    {
        public string CustomerName { set; get; }
        public string Phone { set; get; }
        public string Address { set; get; }
        public string Address2 { set; get; }
        public decimal Discount { set; get; }
        public decimal DueAmount { set; get; }
        public string InvDate { set; get; }
        public decimal MrpTotal { set; get; }
        public decimal PaidableAmount { set; get; }
        public int PosInvoiceType { set; get; }
        public decimal ReceivedAmount { set; get; }
        public decimal SdTotal { set; get; }
        public decimal TotalAmt { set; get; }
        public decimal VatTotal { set; get; }
        public long InvoiceNumber { set; get; }
        public decimal PerProductWiseDiscount { set; get; }
    }

    public class VmTenderInfo
    {
        public string Name { set; get; }
        public decimal TenderAmount { set; get; }
        public string Type { set; get; }
    }

    public class VmBillprintMessage
    {
        public int BillPrintTemplateId { set; get; }
        public string Status { set; get; }
    }
    public class VmPosBillingPrint
    {
        public List<VmProductList> ProductList { set; get; }
        public VmInvoice Invoice { set; get; }
        public List<VmTenderInfo> TenderInfo { set; get; }
        public VmBillprintMessage BillprintMessage { set; get; }
        public string Company { set; get; }
        public string BranchAddress { set; get; }
        public string User { set; get; }
        public string Phone { set; get; }
        public string Branch { set; get; }
        public string VatRegNo { set; get; }
        public decimal RoundOffvalue { set; get; }
        public string PaidBy { set; get; }
        public decimal ChangeAmt { set; get; }
        public string FooterText { set; get; }
        public string PoweredBy { set; get; }
        public string TerminalNo { set; get; }

    }
}