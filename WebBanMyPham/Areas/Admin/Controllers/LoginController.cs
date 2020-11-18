﻿using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Library.Config;
using Library.DataBase;
using Library.Security;
using WebBanMyPham.Areas.Admin.Controllers;
using TeamplateHotel.Areas.Admin;
using TeamplateHotel.Areas.Administrator.Models;
using WebBanMyPham.Areas.Admin.Models;
namespace TeamplateHotel.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Administrator/Login/

        [HttpGet]
        public ActionResult Index()
        {
            if (Request.Cookies["name_client"] != null)
            {
                return RedirectToAction("Index", "ControlPanel");
            }
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            

            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Index(LoginModel model)
        {
            Random random;
            if (TempData["Count"] != null && int.Parse(TempData["Count"].ToString()) >= 5)
            {
                TempData.Keep("Count");
                if (string.IsNullOrEmpty(model.Captcha))
                {
                    ViewBag.ShowCaptcha = true;
                    ModelState.AddModelError("Captcha", "Please enter captcha");
                }
                else
                {
                    if (model.Captcha != Session["Captcha"].ToString())
                    {
                        ViewBag.ShowCaptcha = true;
                        ModelState.AddModelError("Captcha", "Incorrect captcha ");
                    }
                }
            }
            if (string.IsNullOrEmpty(model.LanguageID))
            {
                ModelState.AddModelError("LanguageID", "Select Language");
            }
            if (ModelState.IsValid)
            {
                int checklogin = CheckLogin.CheckUserLogin(model.UserName, model.Password, model.LanguageID );
                switch (checklogin)
                {
                    case 1:

                        //Đăng nhập thành công
                        string cookieClient =  model.UserName + "||";
                        string decodeCookieClient = CryptorEngine.Encrypt(cookieClient, true);
                        HttpCookie userCookie = new HttpCookie("name_client");
                        userCookie.Value = decodeCookieClient;
                        userCookie.Expires = DateTime.Now.AddDays(30);
                        HttpContext.Response.Cookies.Add(userCookie);

                        HttpCookie langCookie = new HttpCookie("lang_client");
                        langCookie.Value = model.LanguageID;
                        langCookie.Expires = DateTime.Now.AddDays(30);
                        HttpContext.Response.Cookies.Add(langCookie);
                        
                        TempData["Count"] = 0;
                        TempData["Messages"] = "Logged in successfully";
                        return RedirectToAction("Index", "ControlPanel");
                    case 2:
                        TempData["Messages"] = "Account is locked";
                        return RedirectToAction("Index");
                    case 3:
                        if (TempData["Count"] == null)
                        {
                            TempData["Count"] = 1;
                            TempData.Keep("Count");
                        }
                        else
                        {
                            TempData["Count"] = int.Parse(TempData["Count"].ToString()) + 1;
                            TempData.Keep("Count");
                        }
                        if (int.Parse(TempData["Count"].ToString()) >= 5)
                        {
                            random = new Random();
                            int iNumber = random.Next(10000, 99999);
                            Session["Captcha"] = iNumber.ToString();

                            ViewBag.ShowCaptcha = true;
                            ViewBag.Message = CommentController.Messages(TempData["Message"]);
                            return View();
                        }
                        
                        ViewBag.Messages = "Username and password are incorrect";
                        return View(model);
                }
            }
            return View();
        }

        [HttpGet]
        [ValidateInput(true)]
        public ActionResult GetPassWord()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        public ActionResult GetPassWord(GetPassword model)
        {
            using (var db = new MyDBDataContext())
            {
                User detailUser =
                    db.Users.FirstOrDefault(a => a.Email == model.Email && a.UserName == model.UserName);
                if (Convert.ToDateTime(CurrentSession.LockUser) > DateTime.Now)
                {
                    DateTime dateBlog = Convert.ToDateTime(CurrentSession.LockUser);
                    int minuteLock = dateBlog.Minute + (dateBlog.Hour*60) - DateTime.Now.Minute - (DateTime.Now.Hour*60);
                    ModelState.AddModelError("Email",
                        "You have entered too many times, please come back later " + minuteLock + " Minute.");
                    return View();
                }
                if (detailUser == null)
                {
                    if (TempData["Count"] == null)
                    {
                        TempData["Count"] = 1;
                        TempData.Keep("Count");
                    }
                    else
                    {
                        TempData["Count"] = int.Parse(TempData["Count"].ToString()) + 1;
                        TempData.Keep("Count");
                    }
                    if (int.Parse(TempData["Count"].ToString()) == 5)
                    {
                        DateTime dateBlog = DateTime.Now.AddMinutes(1);
                        CurrentSession.LockUser = dateBlog;
                        int minuteLock = dateBlog.Minute + (dateBlog.Hour*60) - DateTime.Now.Minute -
                                         (DateTime.Now.Hour*60);
                        TempData.Remove("Count");
                        ModelState.AddModelError("Email",
                            "You have entered too many times, please come back later " + minuteLock + " Minute.");
                        return View();
                    }
                    ModelState.AddModelError("Email",
                        "Email or username is incorrect, you still " +
                        (5 - int.Parse(TempData["Count"].ToString())) + " enter");

                    return View();
                }
                string content =
                    System.IO.File.ReadAllText(Server.MapPath("/Areas/Administrator/Content/Teamplate/Forgot_Password.html"));
                content = content.Replace("{{Password}}", CryptorEngine.Decrypt(detailUser.PasswordOld, true));

                MailHelper.SendMail(detailUser.Email, "Password retrieval", content);


                ViewBag.Messeages = "Please login to the email password: " + model.Email + " to retrieval password.";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            int cout = 0;
            HttpCookie langCookie = Request.Cookies["lang_client"];
            while (langCookie !=null)
            {
                langCookie.Expires = DateTime.Now.AddDays(-30);
                HttpContext.Response.Cookies.Add(langCookie);
                cout++;
                if(cout == 10)
                    break;
            }
            cout = 0;
            HttpCookie nameCookie = Request.Cookies["name_client"];
            while (nameCookie !=null)
            {
                nameCookie.Expires = DateTime.Now.AddDays(-30);
                HttpContext.Response.Cookies.Add(nameCookie);
                cout++;
                if(cout == 10)
                    break;
            }
            CurrentSession.ClearAll();
            return RedirectToAction("Index");
        }
    }
}