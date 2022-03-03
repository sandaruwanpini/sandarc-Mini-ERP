using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Controllers.Manager;
using ERP.Controllers.POS.Gateway;
using ERP.Migrations;
using ERP.Models;
using ERP.Models.POS;
using ERP.Models.VModel;
using ERP.CSharpLib;

namespace ERP.Controllers.POS.Manager
{
    public class StockManager
    {
        public int InsertStock(PosStock stock, ErpManager manager)
        {
            using (ConnectionDatabase database = new ConnectionDatabase())
            {
                using (var trn = database.Database.BeginTransaction())
                {
                    foreach (var sd in stock.PosStockDetail)
                    {
                        stock.LessDiscunt += sd.Discount;
                        stock.TotalTax += sd.PurchaseTax;
                        var batch = database.PosProductBatchDbSet.Where(f => f.Id == sd.PosProductBatchId).Select(s => new {s.PosProductId, s.PurchaseRate}).First();
                        stock.Netvalue += sd.Qty*batch.PurchaseRate;
                        sd.PosProductId = batch.PosProductId;
                    }
                    stock.Netvalue = (stock.Netvalue + (stock.OtherCharges ?? 0) + (stock.TotalTax ?? 0)) - ((stock.LessDiscunt ?? 0) + (stock.OtherDiscount ?? 0));
                    stock.CmnCompanyId = manager.CmnId;
                    stock.CreatedBy = manager.UserId;
                    stock.CreatedDate = UTCDateTime.BDDateTime();
                    stock.PosStockTransactionType = 1;//for purchase received/stock transfer
                    database.PosStockDbSet.Add(stock);

                    #region for branch transfer stock

                    long n;
                    bool isNumeric = long.TryParse(stock.CompanyInvNo, out n);
                    if (isNumeric)
                    {
                        long companyInv = Convert.ToInt64(stock.CompanyInvNo);
                        var salInvoice = database.PosSalesInvoiceDbSet.FirstOrDefault(f => f.IsReceiveTransferStock == false && f.PosInvoiceType ==PosInvoiceType.StockTransfar && f.InvoiceNumber == companyInv);
                        if (salInvoice != null)
                        {
                            salInvoice.IsReceiveTransferStock = true;
                        }
                    }

                    #endregion

                    int rtr = database.SaveChanges();
                    trn.Commit();
                    return rtr;
                }
            }
        }

        public int InsertLocationTransfer(VmLocationTransfer locationTransfer, ErpManager manager)
        {
            using (ConnectionDatabase database = new ConnectionDatabase())
            {
                if (string.IsNullOrEmpty(locationTransfer.CompanyInvNo))
                    return -1010; //company inv no is required
                using (var trn = database.Database.BeginTransaction())
                {

                    List<PosStockDetail> locationTransferTo = new List<PosStockDetail>();
                    List<PosStockDetail> locationTransferFrom = new List<PosStockDetail>();
                    decimal totalAmt = 0;
                    foreach (var sd in locationTransfer.LocationTransferDetails)
                    {
                        decimal stockQty = (new StockGateway().GetStockByLocationWise(sd.PosProductBatchId,locationTransfer.FromPosBranchId ,locationTransfer.FromLocation, manager.CmnId));
                        if  (sd.Qty>stockQty)
                        {
                            return -1011;
                        }
                        var productInfo = database.PosProductBatchDbSet.Where(w => w.Id == sd.PosProductBatchId).Select(s => new {s.PosProductId, s.PurchaseRate}).First();

                        totalAmt += (sd.Qty*productInfo.PurchaseRate);
                        PosStockDetail stockDetailTo = new PosStockDetail()
                        {
                            Discount = 0,
                            PosProductBatchId = sd.PosProductBatchId,
                            PosProductId = productInfo.PosProductId,
                            PosStockTypeId = locationTransfer.ToTocation,
                            PurchaseTax = 0,
                            Qty = sd.Qty
                        };
                        //add to Location TransferTo
                        locationTransferTo.Add(stockDetailTo);

                        //add to Location TransferFrom
                        PosStockDetail stockDetailForm = new PosStockDetail()
                        {
                            Discount = 0,
                            PosProductBatchId = sd.PosProductBatchId,
                            PosProductId = productInfo.PosProductId,
                            PosStockTypeId = locationTransfer.FromLocation,
                            PurchaseTax = 0,
                            Qty =- sd.Qty
                        };
                        locationTransferFrom.Add(stockDetailForm);

                    }
                    PosStock stockTrnsFrom = new PosStock()
                    {
                        InvReferenceNo = "LocationTransfer From",
                    InvDate = locationTransfer.InvDate,
                        InvReceiveDate = UTCDateTime.BDDate(),
                        CompanyInvNo  = locationTransfer.CompanyInvNo,
                        PosBranchId = locationTransfer.FromPosBranchId,
                        PosStockTransactionType = 2, //2 for transfer from /reduce from
                        PosSupplierId = locationTransfer.PosSupplierId,
                        LessDiscunt = 0,
                        TotalTax = 0,
                        Netvalue = -totalAmt,
                        NetPayable = -totalAmt,
                        OtherDiscount = 0,
                        OtherCharges = 0,
                        CmnCompanyId = manager.CmnId,
                        CreatedBy = manager.UserId,
                        CreatedDate = UTCDateTime.BDDateTime(),
                        PosStockDetail = locationTransferFrom,
                        Remarks = locationTransfer.Remarks
                    };
                    database.PosStockDbSet.Add(stockTrnsFrom);
                    int rtr = database.SaveChanges();

                    PosStock stockTrnsTo = new PosStock()
                    {
                        InvReferenceNo = "LocationTransfer To",
                        InvDate = locationTransfer.InvDate,
                        InvReceiveDate = UTCDateTime.BDDate(),
                        CompanyInvNo = locationTransfer.CompanyInvNo,
                        PosBranchId = locationTransfer.ToPosBranchId,
                        PosStockTransactionType = 3, //3 for ==Transfer to location/Add To 
                        PosSupplierId = locationTransfer.PosSupplierId,
                        LessDiscunt = 0,
                        TotalTax = 0,
                        Netvalue = totalAmt,
                        NetPayable = totalAmt,
                        OtherDiscount = 0,
                        OtherCharges = 0,
                        CmnCompanyId = manager.CmnId,
                        CreatedBy = manager.UserId,
                        CreatedDate = UTCDateTime.BDDateTime(),
                        PosStockDetail = locationTransferTo,
                        Remarks = locationTransfer.Remarks,
                        
                    };
                    database.PosStockDbSet.Add(stockTrnsTo);
                    rtr += database.SaveChanges();

                    trn.Commit();
                    return rtr;
                }
            }
        }


        public int InsertStockAdjusrment(VmStockAdjustment stocjAdjust, ErpManager manager)
        {
            using (ConnectionDatabase database = new ConnectionDatabase())
            {
                if (string.IsNullOrEmpty(stocjAdjust.CompanyInvNo))
                    return -1010; //company inv no is required
                using (var trn = database.Database.BeginTransaction())
                {

                    List<PosStockDetail> stockDetails = new List<PosStockDetail>();
                    decimal totalAmt = 0;
                    foreach (var sd in stocjAdjust.StockAdjustmentDetails)
                    {
                        if (sd.Qty != 0)
                        {
                            var productInfo = database.PosProductBatchDbSet.Where(w => w.Id == sd.PosProductBatchId).Select(s => new {s.PosProductId, s.PurchaseRate}).First();
                            decimal tmpQty = stocjAdjust.TranactionType == 5 ? -sd.Qty : sd.Qty;
                            totalAmt += (tmpQty*productInfo.PurchaseRate);
                            PosStockDetail posStockDetail = new PosStockDetail()
                            {
                                Discount = 0,
                                PosProductBatchId = sd.PosProductBatchId,
                                PosProductId = productInfo.PosProductId,
                                PosStockTypeId = stocjAdjust.Location,
                                PurchaseTax = 0,
                                Qty = tmpQty
                            };
                            stockDetails.Add(posStockDetail);
                        }
                    }

                    PosStock stock = new PosStock()
                    {

                        InvReferenceNo = stocjAdjust.TranactionType == 5 ? "Reduce Stock" : "Add Stock",
                        InvDate = stocjAdjust.InvDate,
                        InvReceiveDate = UTCDateTime.BDDate(),
                        CompanyInvNo = stocjAdjust.CompanyInvNo,
                        PosBranchId = stocjAdjust.PosBranchId,
                        PosStockTransactionType = stocjAdjust.TranactionType,
                        PosSupplierId = stocjAdjust.PosSupplierId,
                        LessDiscunt = 0,
                        TotalTax = 0,
                        Netvalue = -totalAmt,
                        NetPayable = -totalAmt,
                        OtherDiscount = 0,
                        OtherCharges = 0,
                        CmnCompanyId = manager.CmnId,
                        CreatedBy = manager.UserId,
                        CreatedDate = UTCDateTime.BDDateTime(),
                        PosStockDetail = stockDetails,
                        Remarks = stocjAdjust.Remarks
                    };
                    database.PosStockDbSet.Add(stock);
                    int rtr = database.SaveChanges();
                    trn.Commit();
                    return rtr;
                }
            }
        }




    }
}