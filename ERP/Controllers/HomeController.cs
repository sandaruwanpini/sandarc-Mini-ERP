using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERP.Controllers.Manager;
using ERP.Controllers.Miscellaneous;
using ERP.Models;
using ERP.Models.Security.Authorization;

namespace ERP.Controllers
{

    public class HomeController : Controller
    {
        ErpManager _erpManager = new ErpManager();

        public ActionResult Test()
        {
            using (ConnectionDatabase database = new ConnectionDatabase())
            {
                var sss = (from l in database.PosVwPurchaseReceiptDbSet
                    select new
                    {
                        l.ProductName,
                        Qty = l.PosProduct.PosSalesInvoiceProduct.Select(s => s.Qty).Sum()
                    }).ToList();
                return null;
            }
        }

        public ActionResult ErrorResourcePermission()
        {
            return View();
        }

        public ActionResult ErrorInvalidRequest()
        {
            return View();
        }

        public ActionResult ErrorDatabaseConnection()
        {
            return View();
        }

        public ActionResult ErrorRequest()
        {
            string message = Request.QueryString["message"];
            ViewBag.Message = message;
            return View(ViewBag);
        }


        #region database backup

        [HasAuthorization]
        public ActionResult DatabaseBackup()
        {
            return View();
        }

        public ActionResult GetDrive()
        {
            try
            {
                return UtilityManager.JsonResultMax(Json(new DatabaseBackup().GetDriverList()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return _erpManager.ExceptionDefault(e);
            }

        }

        public ActionResult GetDatabase()
        {
            try
            {
                return UtilityManager.JsonResultMax(Json(new DatabaseBackup().GetDatabaseList()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return _erpManager.ExceptionDefault(e);
            }

        }

        public ActionResult BackupDB(string drive,string db,bool isMdfFile=false)
        {
            try
            {
                if (isMdfFile)
                    return UtilityManager.JsonResultMax(Json(new DatabaseBackup().BackupProsecssForMdf(drive, db)), JsonRequestBehavior.AllowGet);
                return UtilityManager.JsonResultMax(Json(new DatabaseBackup().BackupProsecss(drive, db)), JsonRequestBehavior.AllowGet);
            }
            catch (SqlException ex)
            {
                return Json(ex.Message, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.DenyGet);
            }

        }

        [HasAuthorization]
        public ActionResult SoftwareShortcutKey()
        {
            return View();
        }

        #endregion

    }
}