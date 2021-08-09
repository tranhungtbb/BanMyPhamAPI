using Library.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanMyPham.Areas.Administrator.EntityModel;

namespace WebBanMyPham.Areas.Administrator.Controllers
{
    public class CustomerController : BaseController
    {
        // GET: Administrator/Cusnomer
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Customer";
            return View();
        }
        

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDBDataContext())
                {
                    List<Customer> cus = new List<Customer>();
                    cus = db.Customers.ToList();
                    
                    var records = cus.Select(a => new ECustomer
                    {
                        ID = a.ID,
                        FullName = a.FullName,
                        Phone = a.Phone,
                        Email = a.Email,
                        Address = a.Address,
                    }).OrderBy(a => a.ID).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = cus.Count() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var db = new MyDBDataContext();
            var cus = db.Customers.FirstOrDefault(x => x.ID == id);
            return View(cus);
        }

        

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDBDataContext())
                {
                    var del = db.Customers.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {

                        db.Customers.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Successful" });
                    }
                    return Json(new { Result = "ERROR", Message = "does not exist" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
    }
}