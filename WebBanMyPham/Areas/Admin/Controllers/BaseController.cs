﻿using Library.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Library.DataBase;
namespace WebBanMyPham.Areas.Admin.Controllers
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
                    //Language language = db.Languages.FirstOrDefault();
                    string cookieClient = Request.Cookies["name_client"].Value;
                    string deCodecookieClient = CryptorEngine.Decrypt(cookieClient, true);
                    string userName = deCodecookieClient.Substring(0, deCodecookieClient.IndexOf("||"));
                    //string computerName = deCodecookieClient.Substring(deCodecookieClient.IndexOf("||") + 2, deCodecookieClient.Length - userName.Length - 2);

                    var user = db.Users.FirstOrDefault(a => a.UserName == userName);
                    if (user == null)
                    {
                        int cout = 0;
                        //HttpCookie langCookie = Request.Cookies["lang_client"];
                        //while (langCookie != null)
                        //{
                        //    langCookie.Expires = DateTime.Now.AddDays(-30);
                        //    HttpContext.Response.Cookies.Add(langCookie);
                        //    cout++;
                        //    if (cout == 10)
                        //        break;
                        //}
                        //cout = 0;
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
                              new RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Admin" }));
                    }

                    else
                    {
                        //CurrentSession.Role = (int)user.Role;
                        CurrentSession.Logined = true;
                        Session["UserName"] = user.UserName;

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
                    //if (Request.Cookies["lang_client"] != null)
                    //{
                    //    string languageKey = Request.Cookies["lang_client"].Value;
                    //    if (db.Languages.Any(a => a.ID == languageKey) == false)
                    //    {
                    //        HttpCookie langCookie = new HttpCookie("lang_client");
                    //        langCookie.Value = language.ID;
                    //        langCookie.Expires = DateTime.Now.AddDays(30);
                    //        HttpContext.Response.Cookies.Add(langCookie);
                    //    }
                    //}
                    //else
                    //{
                    //    HttpCookie langCookie = new HttpCookie("lang_client");
                    //    langCookie.Value = language.ID;
                    //    langCookie.Expires = DateTime.Now.AddDays(30);
                    //    HttpContext.Response.Cookies.Add(langCookie);
                    //}

                    base.OnActionExecuting(filterContext);
                }
            }
        }
    }
}