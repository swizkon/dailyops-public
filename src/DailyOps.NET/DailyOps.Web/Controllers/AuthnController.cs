using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DailyOps.Web.Controllers
{
    public class AuthnController : Controller
    {
        //
        // GET: /Authn/

        [HttpGet, ActionName("login")]
        public ActionResult LoginView()
        {
            return View();
        }

        [HttpPost, ActionName("login")]
        public ActionResult HandleLogin(
            [System.Web.Http.FromBody] string username,
            [System.Web.Http.FromBody] string provider)
        {
            string cookieUser = username + "(" + provider + ")";

            FormsAuthentication.SetAuthCookie(cookieUser, true);

            string landingPage = HttpContext.Request.QueryString["ReturnUrl"] ?? "~";
            return Redirect(landingPage);
        }

        [HttpGet, ActionName("logout")]
        public ActionResult HandleLogout()
        {
            FormsAuthentication.SignOut();

            return Redirect("~/");
        }
    }
}
