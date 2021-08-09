//using ProjectLibrary.Database;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace TeamplateHotel.Areas.Administrator.Controllers
//{
//    public class BannerController : Controller
//    {
//        //
//        // GET: /Administrator/Banner/

//        public ActionResult Index()
//        {
//            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
//            ViewBag.Title = "Banner Home";
//            return View();
//        }
//        [HttpPost]
//        public ActionResult UpdateIndex()
//        {
//            using (var db = new MyDbDataContext())
//            {
//                List<Banner> records =
//                    db.Banners.ToList();

//                TempData["Messages"] = "Successful";
//                return RedirectToAction("Index");
//            }
//        }
//        [HttpPost]
//        public JsonResult List(int menuId = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
//        {
//            try
//            {
//                using (var db = new MyDbDataContext())
//                {
//                    var records = db.Banners.Select(a => new {
//                        a.ID,
//                        a.Title,
//                        a.Image,
//                        a.Description,
//                        a.Status,
//                    }).ToList();
//                    return Json(new { Result = "OK", Records = records, TotalRecordCount = records.Count });
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { Result = "ERROR", ex.Message });
//            }
//        }
//        [HttpGet]
//        public ActionResult Create()
//        {
//            ViewBag.Title = "add Banner";

//            var model = new Banner();
//            return View(model);
//        }

//        [HttpPost]
//        [ValidateInput(false)]
//        public ActionResult Create(Banner model)
//        {
//            using (var db = new MyDbDataContext())
//            {
//                if (ModelState.IsValid)
//                {
//                    try
//                    {

//                        var list = new Banner
//                        {
//                            Title = model.Title,
//                            Image = model.Image,
//                            Description = model.Description,

//                            Status = model.Status,
                       
//                        };

//                        db.Banners.InsertOnSubmit(list);
//                        db.SubmitChanges();

//                        TempData["Messages"] = "Successful";
//                        return RedirectToAction("Index");
//                    }
//                    catch (Exception exception)
//                    {
       
//                        ViewBag.Messages = "Error: " + exception.Message;
//                        return View(model);
//                    }
//                }

//                return View(model);
//            }
//        }
//        [HttpGet]
//        public ActionResult Update(int id)
//        {
//            using (var db = new MyDbDataContext())
//            {
//                Banner detailBanner = db.Banners.FirstOrDefault(a => a.ID == id);
//                if (detailBanner == null)
//                {
//                    TempData["Messages"] = "Banner not exist";
//                    return RedirectToAction("Index");
//                }

//                var banner = new Banner
//                {
//                    ID = detailBanner.ID,
//                    Title = detailBanner.Title,
//                    Description = detailBanner.Description,
//                    Image = detailBanner.Image,
//                    Status = detailBanner.Status,
//                };

//                ViewBag.Title = "udpate Banner";
//                return View(banner);
//            }
//        }

//        [HttpPost]
//        [ValidateInput(false)]
//        public ActionResult Update(Banner model)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    using (var db = new MyDbDataContext())
//                    {
//                        Banner listBanner = db.Banners.FirstOrDefault(b => b.ID == model.ID);

//                        if (listBanner != null)
//                        {
//                            listBanner.Title = model.Title;
//                            listBanner.Image = model.Image;
//                            listBanner.Description = model.Description;
//                            listBanner.Status = model.Status;
                
//                            db.SubmitChanges();
//                            TempData["Messages"] = "Successful";
//                            return RedirectToAction("Index");
//                        }
//                    }
//                }
//                catch (Exception exception)
//                {

//                    ViewBag.Messages = "Error: " + exception.Message;
//                    return View(model);
//                }
//            }

//            return View(model);
//        }
//        [HttpPost]
//        public JsonResult Delete(int id)
//        {
//            try
//            {
//                using (var db = new MyDbDataContext())
//                {
//                    Banner del = db.Banners.FirstOrDefault(c => c.ID == id);
//                    if (del != null)
//                    {
//                        db.Banners.DeleteOnSubmit(del);
//                        db.SubmitChanges();
//                        return Json(new { Result = "OK", Message = "Successful" });
//                    }
//                    return Json(new { Result = "ERROR", Message = "Banner not exist" });
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { Result = "ERROR", ex.Message });
//            }
//        }

//    }
//}
