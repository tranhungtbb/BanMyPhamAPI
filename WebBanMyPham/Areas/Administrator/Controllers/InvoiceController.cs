using Library.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanMyPham.Areas.Administrator.EntityModel;
using Library.Config;

namespace WebBanMyPham.Areas.Administrator.Controllers
{
    public class InvoiceController : BaseController
    {
        // GET: Administrator/Invoice
       
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Invoice";
            LoadData();
            return View();
        }


        [HttpPost]
        public JsonResult List(int status =0, string key = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDBDataContext())
                {
                    string keySearch = key.Replace(" ", string.Empty).ToLower();
                    var list = new List<Invoice>();
                    list = (status == 0) ? db.Invoices.ToList() : db.Invoices.Where(x => x.Status == status).ToList();
                    var records = list.AsEnumerable().Join(db.Customers, a => a.IDCustomer, b => b.ID, (a, b) => new { a, b })
                        .Where(a => a.a.Code.Replace(" ", string.Empty).ToLower().Contains(keySearch)
                            || a.b.FullName.Replace(" ", string.Empty).ToLower().Contains(keySearch)
                        )
                        .Join(StatusInvoice.Status, a=>a.a.Status, b=>b.Key, (a,b) => new
                        {
                            ID = a.a.ID,
                            Code = a.a.Code,
                            Status = b.Value,
                            FullName = a.b.FullName,
                            TotalPrice = a.a.Total,
                            CreateDate = a.a.CreateDate.ToString("MM/dd/yyyy H:mm")
                        }).OrderBy(a => a.ID).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = records.Count() });
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

        public void LoadData()
        {
            var db = new MyDBDataContext();
            var listStatus = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "Chọn trạng thái"}
            };

            listStatus.AddRange(StatusInvoice.Status.Select(a => new SelectListItem
            {
                Text = a.Value,
                Value = a.Key.ToString()
            }).ToList());
            ViewBag.listStatus = listStatus;
        }
    }
}