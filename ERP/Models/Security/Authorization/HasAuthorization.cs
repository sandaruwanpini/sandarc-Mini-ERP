using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using ERP.Controllers.Manager;

namespace ERP.Models.Security.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Event)]
    public class HasAuthorizationAttribute : AuthorizeAttribute

    {
        private ErpManager _erpManager;

        /// <summary>
        ///1=for check resource permission,
        ///2=for add,
        ///3=for edit,
        ///4=for delete
        ///5=print/Preview
        /// </summary>
        public int AccessLevel { get; set; }

        private int ReturnCode { set; get; }
        private bool ReturnStatus { set; get; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            _erpManager = new ErpManager();
            if (_erpManager.CmnId >= 1)
            {
                switch (AccessLevel)
                {
                    case 0:
                        return true;
                    case 1:
                        if (CheckResourcePermission())
                            return true;
                        //you have no resource permission
                        ReturnCode = -1001;
                        ReturnStatus = false;
                        return false;
                    case 2:
                        if (CheckRolePermission(AccessLevel))
                            return true;
                        //you have no add permission
                        ReturnCode = -1002;
                        ReturnStatus = false;
                        return false;
                    case 3:
                        if (CheckRolePermission(AccessLevel))
                            return true;
                        //you have no edit permission
                        ReturnCode = -1003;
                        ReturnStatus = false;
                        return false;
                    case 4:
                        if (CheckRolePermission(AccessLevel))
                            return true;
                        //you have no delete permission
                        ReturnCode = -1004;
                        ReturnStatus = false;
                        return false;
                    case 5:
                        if (CheckRolePermission(AccessLevel))
                            return true;
                        //you have no Print/Preview permission
                        ReturnCode = -1006;
                        ReturnStatus = false;
                        return false;
                    default:
                        //invalid request
                        ReturnCode = -1005;
                        ReturnStatus = false;
                        return false;
                }
            }
            //session timeout
            ReturnCode = -1000;
            ReturnStatus = false;
            return false;

        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (!ReturnStatus)
            {
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    //you have no resource permission
                    context.HttpContext.Response.StatusCode = 403;
                    context.Result = new JsonResult {Data = ReturnCode, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
                }
                else
                {
                    switch (ReturnCode)
                    {
                        case -1000:
                            //session timeout
                            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new {controller = "Security", action = "Login"}));
                            break;
                        case -1001:

                            //you have no resource permission on page redirect/load
                            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new {controller = "Home", action = "ErrorResourcePermission" }));
                            break;
                        case -1005:
                            //you have no resource permission on page redirect/load
                            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "ErrorInvalidRequest" }));
                            break;

                    }

                }
            }
        }
        

        /// <summary>
        /// 2 for add, 3 for edit, 4 for delete ,5 for print/preview
        /// </summary>
        /// <returns>0 for permission denine 1 for have permission</returns>
        public bool CheckRolePermission(int role)
        {
            if (_erpManager.GetResourcePermissions.Any() && _erpManager.GetRolePermissions.Any())
            {
                _erpManager = new ErpManager();
                Uri uriAddress = new Uri(HttpContext.Current.Request.UrlReferrer.AbsoluteUri);
                string method = uriAddress.Segments[0] + uriAddress.Segments[1] +
                                uriAddress.Segments[2] + (uriAddress.Segments.Count() > 3 ? uriAddress.Segments[3] : "");

                var resPrmi = _erpManager.GetResourcePermissions
                    .Where(w => w.Method == method && w.SecRoleId == _erpManager.RoleId)
                    .Select(s => new {s.SecResourceId}).FirstOrDefault();
                if (resPrmi != null)
                {
                    switch (role)
                    {
                        case 2:
                            if (_erpManager.GetRolePermissions.Any(w => w.SecResourceId == resPrmi.SecResourceId &&
                                                                        w.SecRoleId == _erpManager.RoleId && w.AddPermi))
                            {
                                return true;
                            }

                            break;
                        case 3:
                            if (_erpManager.GetRolePermissions.Any(w => w.SecResourceId == resPrmi.SecResourceId &&
                                                                        w.SecRoleId == _erpManager.RoleId && w.EditPermi))
                            {
                                return true;
                            }

                            break;
                        case 4:
                            if (_erpManager.GetRolePermissions.Any(w => w.SecResourceId == resPrmi.SecResourceId &&
                                                                        w.SecRoleId == _erpManager.RoleId && w.DeletePermi))
                            {
                                return true;
                            }
                            break;
                        case 5:
                            if (_erpManager.GetRolePermissions.Any(w => w.SecResourceId == resPrmi.SecResourceId &&
                                                                        w.SecRoleId == _erpManager.RoleId && w.PrintPermi))
                            {
                                return true;
                            }
                            break;
                    }

                }

                return false;
            }
            return false;
        }

        public bool CheckResourcePermission()
        {
            if (_erpManager.GetResourcePermissions.Any())
            {
                _erpManager = new ErpManager();
                Uri uriAddress = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
                string method = uriAddress.Segments[0] + uriAddress.Segments[1] + uriAddress.Segments[2] +
              (uriAddress.Segments.Count() > 3 ? uriAddress.Segments[3] : "");
                var resPermission = _erpManager.GetResourcePermissions.Where(w =>w.Method == method.Trim()).ToList();
                return resPermission.Any();
            }
            return false;
        }
    }
}