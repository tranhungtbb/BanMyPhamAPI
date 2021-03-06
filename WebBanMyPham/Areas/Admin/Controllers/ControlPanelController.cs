﻿using System.Linq;
using System.Net;
using System.Web.Mvc;
using Library.DataBase;
using Library.Security;
using WebBanMyPham.Areas.Admin.Controllers;


namespace TeamplateHotel.Areas.Admin.Controllers
{
    public class ControlPanelController : BaseController
    {
        //
        // GET: /Administrator/ControlPanel/
        public ActionResult Index()
        {
            var db = new MyDBDataContext();
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            string cookieClient = Request.Cookies["name_client"].Value;
            string deCodecookieClient = CryptorEngine.Decrypt(cookieClient, true);
            string userName = deCodecookieClient.Substring(0, deCodecookieClient.IndexOf("||"));
            ViewBag.Users = db.Users.FirstOrDefault(a => a.UserName == userName);
            ViewBag.Title = "Administrator";
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            return View();
        }

    }
}
