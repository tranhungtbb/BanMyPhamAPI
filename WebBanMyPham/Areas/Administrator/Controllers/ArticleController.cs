﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Library.Config;
using Library.DataBase;
using Library.Utility;
using WebBanMyPham.Areas.Administrator.EntityModel;
using WebBanMyPham.Areas.Administrator.ModelShow;

namespace WebBanMyPham.Areas.Administrator.Controllers
{
    public class ArticleController : BaseController
    {
        // GET: /Administrator/Article/
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Article";
            LoadData();
            return View();
        }

        [HttpPost]
        public ActionResult UpdateIndex()
        {
            using (var db = new MyDBDataContext())
            {
                var records =
                    db.Articles.Join(db.CategoryProducts,
                        a => a.MenuID,
                        b => b.ID, (a, b) => new { a }).ToList();

                foreach (var record in records)
                {
                    string itemAdv = Request.Params[string.Format("Sort[{0}].Index", record.a.ID)];
                    if (itemAdv != null) {
                        int index;
                        int.TryParse(itemAdv, out index);
                        record.a.Index = index;
                        db.SubmitChanges();
                    }
                    
                }
                TempData["Messages"] = "Successful";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public JsonResult List(int menuId = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDBDataContext())
                {
                    List<Article> articles = new List<Article>();
                    if (menuId == 0)
                    {
                        articles = db.Articles.ToList();
                    }
                    else
                    {
                        articles = db.Articles.Where(a => a.MenuID == menuId).ToList();
                    }
                    var listArticle =
                        articles.Join(db.CategoryProducts,
                            a => a.MenuID, b => b.ID, (a, b) => new { a, b });
                    List<ShowArticle> records = listArticle.Select(a => new ShowArticle
                    {
                        ID = a.a.ID,
                        Title = a.a.Title,
                        TitleMenu = a.b.Title,
                        Index = a.a.Index,
                    }).OrderBy(a => a.Index).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = listArticle.Count() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            LoadData();
            ViewBag.Title = "Add article";
            var tab = new EArticle();
            return View(tab);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EArticle model)
        {
            using (var db = new MyDBDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(model.Alias))
                    {
                        model.Alias = StringHelper.ConvertToAlias(model.Alias);
                    }
                    try
                    {
                        var article = new Article
                        {
                            MenuID = model.MenuID,
                            Title = model.Title,
                            Alias = model.Alias,
                            Image = model.Image,
                            Description = model.Description,
                            Content = model.Content,
                            Index = 0,
                            MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Title : model.MetaTitle,
                            MetaDescription =
                                string.IsNullOrEmpty(model.MetaDescription) ? model.Title : model.Description,
                            Status = model.Status,
                            Home = model.Home,
                            CreateDate = DateTime.Now,
                    };

                        db.Articles.InsertOnSubmit(article);
                        db.SubmitChanges();
                        

                        TempData["Messages"] = "Successful";
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        LoadData();
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View();
                    }
                }
                LoadData();
                return View();
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            ViewBag.Title = "Update Article";
            using (var db = new MyDBDataContext())
            {
                Article detailArticle = db.Articles.FirstOrDefault(a => a.ID == id);
               
                if (detailArticle == null)
                {
                    TempData["Messages"] = "does not exist";
                    return RedirectToAction("Index");
                }
                List<SelectListItem> listmenu = new List<SelectListItem>();
                
                ViewBag.ListMenu3 = new SelectList(listmenu, "Text", "Value");
                var artile = new EArticle
                {
                    ID = detailArticle.ID,
                    MenuID = (int)detailArticle.MenuID,
                    Title = detailArticle.Title,
                    Alias = detailArticle.Alias,
                    Image = detailArticle.Image,
                    Description = detailArticle.Description,
                    Content = detailArticle.Content,
                    MetaTitle = detailArticle.MetaTitle,
                    MetaDescription = detailArticle.MetaDescription,
                    Status = (bool)detailArticle.Status,
                    Home = (bool)detailArticle.Home
                };
                
                

                LoadData();
                return View(artile);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(EArticle model)
        {
            using (var db = new MyDBDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(model.Alias))
                    {
                        model.Alias = StringHelper.ConvertToAlias(model.Alias);
                    }
                    try
                    {
                        Article article = db.Articles.FirstOrDefault(b => b.ID == model.ID);
                        if (article != null)
                        {
                            article.MenuID = model.MenuID;
                            article.Title = model.Title;
                            article.Alias = model.Alias;
                            article.Image = model.Image;
                            article.Description = model.Description;
                            article.Content = model.Content;
                            article.MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Title : model.MetaTitle;
                            article.MetaDescription = string.IsNullOrEmpty(model.MetaDescription)
                                ? model.Title
                                : model.MetaDescription;
                            article.Status = model.Status;
                            article.Home = model.Home;
                           
                            db.SubmitChanges();
                            
                            TempData["Messages"] = "Successful";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception exception)
                    {
                
                        LoadData();
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View();
                    }
                }
  
                LoadData();
                return View(model);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDBDataContext())
                {
                    Article del = db.Articles.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        
                        db.Articles.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new {Result = "OK", Message = "Successful" });
                    }
                    return Json(new {Result = "ERROR", Message = "does not exist"});
                }
            }
            catch (Exception ex)
            {
                return Json(new {Result = "ERROR", ex.Message});
            }
        }

        public void LoadData()
        {
            var listMenu = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "Select menu"}
            };
            var menus = new List<CategoryProduct>();

            menus =
                MenuController.GetListMenu(0).Where(
                    a =>
                        a.Type == SystemMenuType.Article ).ToList();

            foreach (CategoryProduct menu in menus)
            {
                string sub = "";
                for (int i = 0; i < menu.Index; i++)
                {
                    sub += "|--";
                }
                menu.Title = sub + menu.Title;
            }

            listMenu.AddRange(menus.OrderBy(a => a.Location).Select(a => new SelectListItem
            {
                Text = a.Title,
                Value = a.ID.ToString()
            }).ToList());
            ViewBag.ListMenu = listMenu;
        }
        
    }
}