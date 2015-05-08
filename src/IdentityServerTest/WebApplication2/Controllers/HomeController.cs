using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Secure()
        {
            return View((User as ClaimsPrincipal).Claims);
        }


        public ActionResult SignInWithAzureAD()
        {
            if (!User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext()
                    .Authentication
                    .Challenge(new Microsoft.Owin.Security.AuthenticationProperties(new Dictionary<string, string>()
                    {
                        {"acr_values", "idp:AzureAD"},
                    }), "idsrv");
            }

            return new EmptyResult();
            //return RedirectToAction("Index");
        }

        public ActionResult PassAdditionalParameters()
        {
            if (!User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext()
                    .Authentication
                    .Challenge(new Microsoft.Owin.Security.AuthenticationProperties(new Dictionary<string, string>()
                    {
                        {"login_hint", "myemail@gmail.com"},
                        {"hd", "hd"},
                        {"anything", "here"},
                    }), "Google");
            }

            return new EmptyResult();
        }

        [Authorize]
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext()
                .Authentication
                .SignOut("Cookies");

            return RedirectToAction("Index");
        }
    }
}