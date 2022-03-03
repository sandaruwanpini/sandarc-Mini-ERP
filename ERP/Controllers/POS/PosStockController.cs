using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERP.Controllers.Manager;
using ERP.Controllers.POS.Gateway;
using ERP.Controllers.POS.Manager;
using ERP.Models;
using ERP.Models.POS;
using ERP.Models.Security.Authorization;
using ERP.Models.VModel;
using Newtonsoft.Json;

namespace ERP.Controllers.POS
{
    [HasAuthorization]
    public class PosStockController : Controller
    {
        ErpManager _erpManager = new ErpManager();

        [HasAuthorization(AccessLevel = 1)]
        public ActionResult PurchaseReceipt()
        {
            return View();
        }

        public ActionResult GetProductInfoForPurchasReceipt()
        {
            ActionResult rtr = _erpManager.ActionResultDefault;
            try
            {
                StockGateway gateway = new StockGateway();
                rtr = UtilityManager.JsonResultMax(Json(gateway.GetProductInfoForPurchasReceipt(_erpManager.CmnId)), JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                rtr = _erpManager.ExceptionDefault(e);
            }

            return rtr;
        }

        public ActionResult GetPurchaseReceipt(string companyInvNo)
        {
            ActionResult rtr = _erpManager.ActionResultDefault;
            try
            {
                StockGateway gateway = new StockGateway();
                rtr = UtilityManager.JsonResultMax(Json(gateway.GetPurchaseReceipt(companyInvNo, _erpManager.CmnId)), JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                rtr = _erpManager.ExceptionDefault(e);
            }

            return rtr;
        }

        public ActionResult GetProductDetailsForPurchaseReceipt(string productCode)
        {
            ActionResult rtr = _erpManager.ActionResultDefault;
            try
            {
                StockGateway gateway = new StockGateway();
                rtr = UtilityManager.JsonResultMax(Json(gateway.GetProductDetailsForPurchaseReceipt(_erpManager.CmnId, productCode)), JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                rtr = _erpManager.ExceptionDefault(e);
            }

            return rtr;
        }

        [HasAuthorization(AccessLevel = 2)]
        public ActionResult InsertPurchaseReceipt(string vmStock)
        {
            ActionResult rtr = _erpManager.ActionResultDefault;
            try
            {
                StockManager manager = new StockManager();
                PosStock stock = (PosStock) JsonConvert.DeserializeObject(vmStock, typeof (PosStock));
                rtr = UtilityManager.JsonResultMax(Json(manager.InsertStock(stock, _erpManager)), JsonRequestBehavior.AllowGet);

            }
            catch (DbUpdateException e)
            {
                rtr = _erpManager.DbUpdateExceptionEntity(e);
            }
            catch (Exception e)
            {
                rtr = _erpManager.ExceptionDefault(e);
            }

            return rtr;
        }


        public ActionResult GetBatchByProductCode(string productCode)
        {
            ActionResult rtr = _erpManager.ActionResultDefault;
            try
            {
                using (ConnectionDatabase database = new ConnectionDatabase())
                {
                    int cmnId = _erpManager.CmnId;
                    var lst = (from l in database.PosProductBatchDbSet
                        orderby l.SellingRate descending
                        where l.PosProduct.Code == productCode && l.CmnCompanyId == cmnId
                        select new
                        {
                            l.Id,
                            Name = l.BatchName,
                            l.PurchaseRate
                        }).ToList();

                    rtr = UtilityManager.JsonResultMax(Json(lst), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                rtr = _erpManager.ExceptionDefault(e);
            }

            return rtr;
        }

        public ActionResult GetStockTransfarInvoiceItem(long invoiceNo)
        {
            try
            {
                using (ConnectionDatabase database = new ConnectionDatabase())
                {
                    var invSts = (from inv in database.PosSalesInvoiceDbSet
                        join cus in database.PosCustomerDbSet on inv.PosCustomerId equals cus.Id
                        where inv.InvoiceNumber == invoiceNo && inv.PosInvoiceType == PosInvoiceType.StockTransfar && _erpManager.UserOffice.Contains(cus.PosBranchId)
                        select new {inv.IsReceiveTransferStock}).FirstOrDefault();
                    if (invSts != null)
                    {
                        if (!invSts.IsReceiveTransferStock)
                        {
                            return UtilityManager.JsonResultMax(
                                Json(new
                                {
                                    Status = 1,
                                    Result = new BillingGateway().GetPosBillingPrintData(_erpManager, invoiceNo,1)
                                }), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return UtilityManager.JsonResultMax(
                                Json(new
                                {
                                    Status = "InvoiceReceived",
                                    Result = ""
                                }), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return UtilityManager.JsonResultMax(
                            Json(new
                            {
                                Status = "InvalidInvoice",
                                Result = ""
                            }), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                return _erpManager.ExceptionDefault(e);
            }
        }

        [AllowAnonymous]
        public ActionResult GoToPurchaseReceiptForStockTransfer(long invNo)
        {
            TempData["invoiceNumber"] = invNo;
            return RedirectToAction("PurchaseReceipt", "PosStock");
        }

       
        #region location transfer

        [HasAuthorization(AccessLevel = 1)]
        public ActionResult LocationTransfer()
        {
            return View();
        }

        public ActionResult GetProductDetailsForLocationTransfer(string productCode,int fromLocation,int officeId)
        {
            ActionResult rtr = _erpManager.ActionResultDefault;
            try
            {
                StockGateway gateway = new StockGateway();
                rtr = UtilityManager.JsonResultMax(Json(gateway.GetProductDetailsForStockTransfer(_erpManager.CmnId, productCode,fromLocation, officeId)), JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                rtr = _erpManager.ExceptionDefault(e);
            }
            return rtr;
        }

        public ActionResult GetStockByLocationWise(int batchId,int officeId,int fromLocation)
        {
            ActionResult rtr = _erpManager.ActionResultDefault;
            try
            {
                rtr = UtilityManager.JsonResultMax(Json(new StockGateway().GetStockByLocationWise(batchId,officeId,fromLocation,_erpManager.CmnId)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                rtr = _erpManager.ExceptionDefault(e);
            }
            return rtr;
        }

        [HasAuthorization(AccessLevel = 2)]
        public ActionResult InsertLocationTransfer(string vmLocationTransfer)
        {
            ActionResult rtr = _erpManager.ActionResultDefault;
            try
            {
                StockManager manager = new StockManager();
                VmLocationTransfer locationTransfer = (VmLocationTransfer)JsonConvert.DeserializeObject(vmLocationTransfer, typeof(VmLocationTransfer));
                rtr = UtilityManager.JsonResultMax(Json(manager.InsertLocationTransfer(locationTransfer, _erpManager)), JsonRequestBehavior.AllowGet);

            }
            catch (DbUpdateException e)
            {
                rtr = _erpManager.DbUpdateExceptionEntity(e);
            }
            catch (Exception e)
            {
                rtr = _erpManager.ExceptionDefault(e);
            }

            return rtr;
        }



        #endregion


        #region Stock Adjustment

        [HasAuthorization(AccessLevel = 1)]
        public ActionResult StockAdjustment()
        {
            return View();
        }
        

        [HasAuthorization(AccessLevel = 2)]
        public ActionResult InsertStockAdjustment(string vmStockAdjustment)
        {
            ActionResult rtr = _erpManager.ActionResultDefault;
            try
            {
                StockManager manager = new StockManager();
                VmStockAdjustment stockAdjustment = (VmStockAdjustment)JsonConvert.DeserializeObject(vmStockAdjustment, typeof(VmStockAdjustment));
                rtr = UtilityManager.JsonResultMax(Json(manager.InsertStockAdjusrment(stockAdjustment, _erpManager)), JsonRequestBehavior.AllowGet);

            }
            catch (DbUpdateException e)
            {
                rtr = _erpManager.DbUpdateExceptionEntity(e);
            }
            catch (Exception e)
            {
                rtr = _erpManager.ExceptionDefault(e);
            }

            return rtr;
        }

        #endregion



    }
}