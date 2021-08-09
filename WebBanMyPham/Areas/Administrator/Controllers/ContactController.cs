//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Library.DataBase;
//using Library.Security;

//namespace TeamplateHotel.Areas.Administrator.Controllers
//{
//    public class ContactController : BaseController
//    {
//        //
//        // GET: /Administrator/Contact/
//        public ActionResult Index()
//        {
//            var db = new MyDBDataContext();
            
//            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
//            ViewBag.Title = "Contact";
//            return View();
//        }

//        [HttpPost]
//        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
//        {
//            try
//            {
//                var db = new MyDBDataContext();
//                List<Contact> listContact = db.Contacts.ToList();

//                var records = listContact.Select(a => new
//                {
//                    a.ID,
//                    CreateDate = a.CreateDate.ToShortDateString(),
//                    a.FullName,
//                    a.Tel,
//                    a.Email,
          
//                }).Skip(jtStartIndex).Take(jtPageSize).ToList();
//                //Return result to jTable
//                return Json(new {Result = "OK", Records = records, TotalRecordCount = listContact.Count()});
//            }
//            catch (Exception ex)
//            {
//                return Json(new {Result = "ERROR", ex.Message});
//            }
//        }

//        [HttpPost]
//        public JsonResult Delete(int id)
//        {
//            var db = new MyDBDataContext();
//            Contact del = db.cont.FirstOrDefault(a => a.ID == id);
//            if (del == null)
//            {
//                return Json(new {Result = "ERROR", Message = "does not exist"});
//            }
//            db.Contacts.DeleteOnSubmit(del);
//            db.SubmitChanges();
//            return Json(new {Result = "OK", Message = "Successful" });
//        }

//        [HttpGet]
//        public ActionResult Detail(int Id)
//        {
//            var db = new MyDBDataContext();
//            Contact detail = db.Contacts.FirstOrDefault(a => a.ID == Id);
//            if (detail == null)
//            {
//                TempData["Messages"] = "does not exist";
//                return RedirectToAction("Index");
//            }
//            return View("Detail", detail);
//        }
//    }
//}