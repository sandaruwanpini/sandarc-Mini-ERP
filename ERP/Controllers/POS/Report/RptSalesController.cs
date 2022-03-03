using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERP.Controllers.Manager;
using ERP.Models.Security.Authorization;
using ERP.Report.Pos.Aspx;

namespace ERP.Controllers.POS.Report
{
    [HasAuthorization]
    public class RptSalesController : Controller
    {
        ErpManager _erpManager=new ErpManager();
        public ActionResult ItemWiseSales()
        {
            return View();
        }

        public ActionResult DetailedSales()
        {
            return View();
        }


        public ActionResult TotalSalesSummary()
        {
            return View();
        }

    }
}