using Library.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using WebBanMyPham.Models;

namespace WebBanMyPham.Controllers
{
    public class CustomerController : ApiController
    {
        [Route("api/login")]
        [HttpGet]
        public HttpResponseMessage Login(string username, string password)
        {
            try
            {

                return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = "ccc" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
            }
        }
        [Authorize(Roles = "User")]
        [Route("api/login")]
        [HttpGet]
        public HttpResponseMessage Logout()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = "ccc" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
            }
        }
    }
}
