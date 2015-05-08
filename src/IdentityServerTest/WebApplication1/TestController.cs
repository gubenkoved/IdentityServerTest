using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace WebApplication1
{
    [Route("test")]
    public class TestController : ApiController
    {
        public IHttpActionResult Get()
        {
            var caller = User as ClaimsPrincipal;

            var subjectClaim = caller.FindFirst("sub");
            if (subjectClaim != null)
            {
                return Json(new
                {
                    message = "OK user",
                    client = caller.FindFirst("client_id").Value,
                    subject = subjectClaim.Value
                });
            }
            else
            {
                return Json(new
                {
                    message = "OK computer",
                    client = caller.FindFirst("client_id").Value
                });
            }
        }
    }
}