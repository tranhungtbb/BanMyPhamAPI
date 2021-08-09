using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Library.Config;
using Library.DataBase;
using TeamplateHotel.Areas.Administrator.EntityModel;
using System.Globalization;
using WebBanMyPham.Areas.Administrator.EntityModel;
using WebBanMyPham.Areas.Administrator.Controllers;
using WebBanMyPham.Areas.Administrator.Models;

namespace TeamplateHotel.Controllers
{
    public class SliderController : BaseController
    {
        // GET: /Administrator/Slider/
        public ActionResult Index()
        {
            LoadData();
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Sliders";
            return View();
        }

        [HttpPost]
        public ActionResult UpdateIndex()
        {
            using (var db = new MyDBDataContext())
            {
                List<Slide> records =
                    db.Slides.ToList();
                foreach (Slide record in records)
                {
                    string itemAdv = Request.Params[string.Format("Sort[{0}].Index", record.ID)];
                    int index;
                    int.TryParse(itemAdv, out index);
                    record.Index = index;
                    db.SubmitChanges();
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
                    var records = db.Slides.Select(a=> new {
                        a.ID,
                        a.Title,
                        a.Image,
                        a.Index
                    }).ToList();
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = records.Count });
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
            ViewBag.Title = "add slide";
            LoadData();
            ViewBag.Menus = LoadData("", Request.Cookies["lang_client"].Value);
            var model = new ESlider();
            model.ViewAll = true;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ESlider model)
        {
            using (var db = new MyDBDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (model.ViewAll)
                        {
                            model.MenuIDs = "";
                        }
                        var slider = new Slide
                        {
                            Title = model.Title,
                            //MenuID = model.MenuID,
                            Image = model.Image,
                            Index = 0,
                            Description = model.Description,
                        };

                        db.Slides.InsertOnSubmit(slider);
                        db.SubmitChanges();

                        TempData["Messages"] = "Successful";
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        LoadData();
                        ViewBag.Menus = LoadData(model.MenuIDs, Request.Cookies["lang_client"].Value);
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View(model);
                    }
                }
                LoadData();
                ViewBag.Menus = LoadData(model.MenuIDs, Request.Cookies["lang_client"].Value);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            using (var db = new MyDBDataContext())
            {
                var detailSlider = db.Slides.FirstOrDefault(a => a.ID == id);
                if (detailSlider == null)
                {
                    TempData["Messages"] = "Slide not exist";
                    return RedirectToAction("Index");
                }
                LoadData();
                var slider = new ESlider
                {
                    ID = detailSlider.ID,
                    Title = detailSlider.Title,
                    Image = detailSlider.Image,
                    Description = detailSlider.Description,
                };
                ViewBag.Title = "udpate slide";
                ViewBag.Menus = LoadData(slider.MenuIDs, Request.Cookies["lang_client"].Value);
                return View(slider);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(ESlider model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new MyDBDataContext())
                    {
                        var slider = db.Slides.FirstOrDefault(b => b.ID == model.ID);
                        if (model.ViewAll)
                        {
                            model.MenuIDs = "";
                        }

                        if (slider != null)
                        {
                            slider.Title = model.Title;
                            slider.Image = model.Image;
                            slider.Description = model.Description;
                            db.SubmitChanges();
                            TempData["Messages"] = "Successful";
                            return RedirectToAction("Index");
                        }
                    }
                }
                catch (Exception exception)
                {
                    LoadData();
                    ViewBag.Menus = LoadData(model.MenuIDs, Request.Cookies["lang_client"].Value);
                    ViewBag.Messages = "Error: " + exception.Message;
                    return View(model);
                }
            }
            ViewBag.Menus = LoadData(model.MenuIDs, Request.Cookies["lang_client"].Value);
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDBDataContext())
                {
                    Slide del = db.Slides.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        db.Slides.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Successful" });
                    }
                    return Json(new { Result = "ERROR", Message = "Slide not exist" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        //Lấy danh sách menu khi thay đổi hotel
        public List<MenuCheck> LoadData(string menuIDs, string languageID)
        {
            //check logged
            var menuIsSelect = new List<MenuCheck>();
            List<Menu> listMenu =
                MenuController.GetListMenu(SystemMenuLocation.ListLocationMenu().ToList()[0].LocationId);
            var listMenuIds = new int[1];
            if (string.IsNullOrEmpty(menuIDs) == false)
            {
                listMenuIds =
                    menuIDs.Substring(0, menuIDs.Length - 1)
                        .Split(',')
                        .Select(n => Convert.ToInt32(n))
                        .ToArray();
            }
            menuIsSelect =
                listMenu.Select(a => new MenuCheck
                {
                    Checked = listMenuIds.Contains(a.ID) ? "checked" : "",
                    Level = a.Level,
                    ID = a.ID,
                    Title = a.Title
                }).ToList();
            return menuIsSelect;
        }


        public void LoadData()
        {
            var listMenu = new List<SelectListItem>();
            listMenu.Add(new SelectListItem { Value = "0", Text = "---Select a menu---" });
            List<Menu> getListMenu =
                MenuController.GetListMenu(SystemMenuLocation.ListLocationMenu().ToList()[0].LocationId)
                    .ToList();

            foreach (Menu menu in getListMenu)
            {
                string subTitle = "";
                for (int i = 1; i <= menu.Level; i++)
                {
                    subTitle += "|--";
                }
                menu.Title = subTitle + menu.Title;
            }
            listMenu.AddRange(getListMenu.Select(a => new SelectListItem
            {
                Value =
                    a.ID.ToString(
                        CultureInfo.InvariantCulture),
                Text = a.Title
            }).ToList());
            ViewBag.ListMenu = listMenu;
        }
    }
}