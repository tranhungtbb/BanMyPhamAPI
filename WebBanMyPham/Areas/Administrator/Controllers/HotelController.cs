using System;
using System.Linq;
using System.Web.Mvc;
using Library.DataBase;
using Library.Security;
using WebBanMyPham.Areas.Administrator.EntityModel;

namespace WebBanMyPham.Areas.Administrator.Controllers
{
    public class HotelController : BaseController
    {
        // GET: /Administrator/Article/
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Company";
            return View();
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDBDataContext())
                {
                    var list = db.Companies.Select(a => new
                    {
                        a.ID,
                        a.Name,
                        a.Logo,
                        a.Email,
                        a.Tel,
                        a.Address,
                        a.Hotline,
                    }).ToList();
                    //Return result to jTable
                    return Json(new {Result = "OK", Records = list, TotalRecordCount = list.Count()});
                }
            }
            catch (Exception ex)
            {
                return Json(new {Result = "ERROR", ex.Message});
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            using (var db = new MyDBDataContext())
            {
                if (db.Companies.Any())
                {
                    TempData["Messages"] = "Website is exist";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Title = "add company";
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EHotel model)
        {
            using (var db = new MyDBDataContext())
            {
                if (db.Companies.Any())
                {
                    TempData["Messages"] = "Website is exist";
                    return RedirectToAction("Index");
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        var hotel = new Company
                        {
                            Name = model.Name,
                            Logo = model.Logo,
                            Tel = model.Tel,
                            Email = model.Email,
                            Address = model.Address,
                            Facebook = model.FaceBook,
                            CopyRight = model.CopyRight,
                            Hotline = model.Hotline,
                            MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Name : model.MetaTitle,
                            MetaDescription =
                                string.IsNullOrEmpty(model.MetaDescription) ? model.Name : model.MetaDescription
                        };

                        db.Companies.InsertOnSubmit(hotel);
                        db.SubmitChanges();

                        TempData["Messages"] = "Successful";
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View();
                    }
                }
                return View();
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            ViewBag.Title = "Update Company";
            using (var db = new MyDBDataContext())
            {
                Company hotel = db.Companies.FirstOrDefault(a => a.ID == id);

                if (hotel == null)
                {
                    TempData["Messages"] = "Does not exist";
                    return RedirectToAction("Index");
                }

                var eHotel = new EHotel
                {
                    Name = hotel.Name,
                    Logo = hotel.Logo,
                    Tel = hotel.Tel,
                    Email = hotel.Email,
                    Address = hotel.Address,
                    FaceBook = hotel.Facebook,
                    CopyRight = hotel.CopyRight,
                    MetaTitle = hotel.MetaTitle,
                    MetaDescription = hotel.MetaDescription,
                    Hotline = hotel.Hotline,
                };
                return View(eHotel);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(EHotel model)
        {
            using (var db = new MyDBDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Company hotel = db.Companies.FirstOrDefault(b => b.ID == model.ID);
                        if (hotel != null)
                        {
                            hotel.Name = model.Name;
                            hotel.Logo = model.Logo;
                            hotel.Tel = model.Tel;
                            hotel.Email = model.Email;
                            hotel.Address = model.Address;
                            hotel.Facebook = model.FaceBook;
                            hotel.CopyRight = model.CopyRight;
                            hotel.Hotline = model.Hotline;
                            hotel.MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.Name : model.MetaTitle;
                            hotel.MetaDescription = string.IsNullOrEmpty(model.MetaDescription)
                                ? model.Name
                                : model.MetaDescription;

                            db.SubmitChanges();
                            TempData["Messages"] = "Successful";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception exception)
                    {
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View();
                    }
                }
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
                    Company del = db.Companies.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        db.Companies.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new {Result = "OK", Message = "Successful" });
                    }
                    return Json(new {Result = "ERROR", Message = "Does not exist"});
                }
            }
            catch (Exception ex)
            {
                return Json(new {Result = "ERROR", ex.Message});
            }
        }
    }
}