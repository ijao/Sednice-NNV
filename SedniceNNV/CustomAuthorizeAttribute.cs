using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SedniceNNV
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class CustomApiAuthorize: AuthorizeAttribute
    {



        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isauthorize = base.AuthorizeCore(httpContext);

            

            //if (Users.Split(',').Contains(httpContext.Session["KorisnickoIme"].ToString())) ovako radi
            if (httpContext.Session["Uloga"] == null)
            {


                return false;
            }


            else
            {
                if (Roles.Split(',').Contains(httpContext.Session["Uloga"].ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //
        // Summary:
        //     Processes HTTP requests that fail authorization.
        //
        // Parameters:
        //   filterContext:
        //     Encapsulates the information for using System.Web.Mvc.AuthorizeAttribute. The
        //     filterContext object contains the controller, HTTP context, request context,
        //     action result, and route data.
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {

                filterContext.Result = new RedirectResult("~/Account/Login");
            }

            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}