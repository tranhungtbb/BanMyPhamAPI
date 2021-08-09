using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Library.DataBase;
using Library.Security;

namespace WebBanMyPham.Areas.Administrator.Controllers
{
    public class BaseController : Controller
    {
        // GET: /Administrator/Base/
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (CurrentSession.Logined == false)
            {
                using (var db = new MyDBDataContext())
                {
                    string cookieClient = Request.Cookies["name_client"].Value;
                    string deCodecookieClient = CryptorEngine.Decrypt(cookieClient, true);
                    string userName = deCodecookieClient.Substring(0, deCodecookieClient.IndexOf("||"));
                    //string computerName = deCodecookieClient.Substring(deCodecookieClient.IndexOf("||") + 2, deCodecookieClient.Length - userName.Length - 2);

                    var user = db.Users.FirstOrDefault(a => a.UserName == userName);
                    if (user == null)
                    {
                        int cout = 0;
                        HttpCookie langCookie = Request.Cookies["lang_client"];
                        while (langCookie != null)
                        {
                            langCookie.Expires = DateTime.Now.AddDays(-30);
                            HttpContext.Response.Cookies.Add(langCookie);
                            cout++;
                            if (cout == 10)
                                break;
                        }
                        cout = 0;
                        HttpCookie nameCookie = Request.Cookies["name_client"];
                        while (nameCookie != null)
                        {
                            nameCookie.Expires = DateTime.Now.AddDays(-30);
                            HttpContext.Response.Cookies.Add(nameCookie);
                            cout++;
                            if (cout == 10)
                                break;
                        }

                        filterContext.Result =
                          new RedirectToRouteResult(
                              new RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Administrator" }));
                    }

                    else
                    {
                        //CurrentSession.Role = (int)user.Role;
                        CurrentSession.Logined = true;

                        //string clientMachineName = HttpContext.Request.UserHostName;
                        //Response.Write(clientMachineName);
                        //if (computerName.Equals(clientMachineName) == false)
                        //{
                        //    int cout = 0;
                        //    HttpCookie langCookie = Request.Cookies["lang_client"];
                        //    while (langCookie != null)
                        //    {
                        //        langCookie.Expires = DateTime.Now.AddDays(-30);
                        //        HttpContext.Response.Cookies.Add(langCookie);
                        //        cout++;
                        //        if (cout == 10)
                        //            break;
                        //    }
                        //    cout = 0;
                        //    HttpCookie nameCookie = Request.Cookies["name_client"];
                        //    while (nameCookie != null)
                        //    {
                        //        nameCookie.Expires = DateTime.Now.AddDays(-30);
                        //        HttpContext.Response.Cookies.Add(nameCookie);
                        //        cout++;
                        //        if (cout == 10)
                        //            break;
                        //    }
                        //    filterContext.Result =
                        //    new RedirectToRouteResult(
                        //    new RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Administrator" }));
                        //}
                        //else
                        //{
                    }
                    base.OnActionExecuting(filterContext);
                }
            }
        }
    }
}