using Library.Config;
using Library.DataBase;
using Library.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamplateHotel.Areas.Administrator.EntityModel;
using WebBanMyPham.Areas.Administrator.Controllers;
using WebBanMyPham.Areas.Administrator.EntityModel;

namespace WebBanMyPham.Areas.Administrator.Controllers
{
    public class ProductController : BaseController
    {
        //
        // GET: /Administrator/ListHotel/

        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Danh sách sản phẩm";
            LoadData();
            return View();
        }
        

        [HttpPost]
        public JsonResult List(int menuId = 0, int locationId = 0, string key = "",int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            //End check
            try
            {
                var db = new MyDBDataContext();
                string keySearch = key.Replace(" ", string.Empty).ToLower();
                if (menuId != 0)
                {
                    var listHotel = menuId == 0
                        ? db.Products.Join(db.CategoryProducts,
                            a => a.CategoryID, b => b.ID, (a, b) => new { a, b }).ToList()
                        : db.Products.Join(
                            db.CategoryProducts.Where(m => m.ID == menuId),
                            a => a.CategoryID, b => b.ID, (a, b) => new { a, b }).ToList();
                    listHotel = listHotel.Where(x => x.a.ProductName.Replace(" ", string.Empty).ToLower().Contains(keySearch)).ToList();
                    var records = listHotel.Select(a => new
                    {
                        a.a.ID,
                        a.a.ProductName,
                        a.a.Image,
                        a.a.Price,
                        a.a.Status,
                    }).OrderByDescending(a => a.ID).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = listHotel.Count },
                        JsonRequestBehavior.AllowGet);
                }
                else
                {

                    var listHotel2 = db.Products.Join(db.CategoryProducts,
                            a => a.CategoryID, b => b.ID, (a, b) => new { a, b }).ToList();
                    listHotel2 = listHotel2.Where(x => x.a.ProductName.Replace(" ", string.Empty).ToLower().Contains(keySearch)).ToList();
                    var records2 = listHotel2.Select(a => new
                    {
                        a.a.ID,
                        a.a.ProductName,
                        a.a.Image,
                        a.a.Price,
                        a.a.Status,
                    }).OrderByDescending(a => a.ID).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records2, TotalRecordCount = listHotel2.Count },
                        JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Title = "Add new product";
            LoadData();

            var hotel = new EProduct();
            hotel.Status = true;
            return View(hotel);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EProduct model)
        {
            //End check
            using (var db = new MyDBDataContext())
            {

                //Kiểm tra xem alias thuộc hotel này đã tồn tại chưa
                var checkAlias =
                    db.Products.FirstOrDefault(
                        m => m.Alias == model.Alias && m.CategoryID == model.CategoryID);
                if (checkAlias != null)
                {
                    ModelState.AddModelError("Alias", "Hotel is exist.");
                }
                //Kiểm tra xem đã chọn đến chuyên mục con cuối cùng chưa
                if (db.CategoryProducts.Any(a => a.ParentID == model.CategoryID))
                {
                    ModelState.AddModelError("CategoryID", "You haven't selected the last tour category");
                }
                if (!string.IsNullOrEmpty(model.ProductName))
                {
                    model.Alias = StringHelper.ConvertToAlias(model.ProductName);
                }
                //if (ModelState.IsValid)
                //{
                if (string.IsNullOrEmpty(model.Alias))
                {
                    model.Alias = StringHelper.ConvertToAlias(model.Alias);
                }
                //model.TrademarkID = 2;
                try
                {
                    if (ModelState.IsValid)
                    {
                        var hotel = new Product
                        {
                            CreateDate = DateTime.Now,
                            TrademarkID = model.TrademarkID,
                            CategoryID = model.CategoryID,
                            ProductName = model.ProductName,
                            Alias = model.Alias,
                            Image = model.Image,
                            Price = model.Price,
                            Promote = model.Promote,
                            Quantity = model.Quantity,
                            MadeBy = model.MadeBy,
                            Description = model.Description,
                            Status = model.Status,
                            Start = model.Star,
                            MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.ProductName : model.MetaTitle,
                            MetaDescription = string.IsNullOrEmpty(model.MetaDescription) ? model.ProductName : model.MetaDescription,
                        };

                        db.Products.InsertOnSubmit(hotel);
                        db.SubmitChanges();

                        //Thêm hình ảnh cho tour
                        if (model.EGalleryITems != null)
                        {
                            foreach (var itemGallery in model.EGalleryITems)
                            {
                                var gallery = new Image
                                {
                                    Image1 = itemGallery.Image,
                                    IDProduct = hotel.ID
                                };
                                db.Images.InsertOnSubmit(gallery);
                            }
                            db.SubmitChanges();
                        }


                        TempData["Messages"] = "Insert successfull.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        LoadData();
                        return View(model);
                    }

                }
                catch (Exception exception)
                {
                    LoadData();
                    TempData["Messages"] = "Error: " + exception.Message;
                    return View(model);
                }
            }
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var db = new MyDBDataContext();
            var detail = db.Products.FirstOrDefault(a => a.ID == id);
            if (detail == null)
            {
                TempData["Messages"] = "Hotel is not exist";
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Update Hotel";
            LoadData();
            EProduct pro = new EProduct
            {
                ID = detail.ID,
                TrademarkID = detail.TrademarkID,
                CategoryID = detail.CategoryID,
                ProductName = detail.ProductName,
                Alias = detail.Alias,
                Image = detail.Image,
                Price = detail.Price,
                Promote = detail.Promote,
                Quantity = detail.Quantity,
                MadeBy = detail.MadeBy,
                Description = detail.Description,
                Status = detail.Status,
                Star = detail.Start,
                MetaTitle = detail.MetaTitle,
                MetaDescription = detail.MetaDescription,

            };
            //lấy danh sách hình ảnh
            //if (hotel.EGalleryITems != null)
            //{
            pro.EGalleryITems = db.Images.Where(a => a.IDProduct == pro.ID).Select(a => new EProduct.EGalleryITem
            {
                Image = a.Image1,
            }).ToList();


            return View(pro);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(EProduct model)
        {

            //Kiểm tra xem alias đã tồn tại chưa
            var db = new MyDBDataContext();
            var checkAlias =
                db.Products.FirstOrDefault(
                    m => m.Alias == model.Alias && m.CategoryID == model.CategoryID && m.ID != model.ID);
            if (checkAlias != null)
            {
                ModelState.AddModelError("Alias", "Product is exist.");
            }
            //Kiểm tra xem đã chọn đến chuyên mục con cuối cùng chưa
            if (db.CategoryProducts.Any(a => a.ParentID == model.CategoryID))
            {
                ModelState.AddModelError("CategoryID", "You haven't selected the last tour category");
            }
            if (string.IsNullOrEmpty(model.ProductName))
            {
                model.Alias = StringHelper.ConvertToAlias(model.ProductName);
            }
            if (string.IsNullOrEmpty(model.Alias))
            {
                model.Alias = StringHelper.ConvertToAlias(model.Alias);
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var pro = db.Products.FirstOrDefault(b => b.ID == model.ID);
                    if (pro != null)
                    {
                        pro.TrademarkID = model.TrademarkID;
                        pro.CategoryID = model.CategoryID;
                        pro.ProductName = model.ProductName;
                        pro.Alias = model.Alias;
                        pro.Image = model.Image;
                        pro.Price = model.Price;
                        pro.Promote = model.Promote;
                        pro.Quantity = model.Quantity;
                        pro.MadeBy = model.MadeBy;
                        pro.Description = model.Description;
                        pro.Status = model.Status;
                        pro.Start = model.Star;
                        pro.MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? model.ProductName : model.MetaTitle;
                        pro.MetaDescription = string.IsNullOrEmpty(model.MetaDescription) ? model.ProductName : model.MetaDescription;

                        db.SubmitChanges();

                        //xóa gallery 
                        db.Images.DeleteAllOnSubmit(db.Images.Where(a => a.IDProduct == pro.ID).ToList());
                        //Thêm gallery mới
                        if (model.EGalleryITems != null)
                        {
                            foreach (var itemGallery in model.EGalleryITems)
                            {
                                var gallery = new Image
                                {
                                    Image1 = itemGallery.Image,
                                    IDProduct = pro.ID,
                                };
                                db.Images.InsertOnSubmit(gallery);
                            }
                            db.SubmitChanges();
                        }

                        TempData["Messages"] = "Update successful.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    LoadData();
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                LoadData();
                TempData["Messages"] = "Error: " + exception.Message;
                return View(model);
            }
            //}
            LoadData();
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(int ID)
        {
            try
            {
                using (var db = new MyDBDataContext())
                {
                    var del = db.Products.FirstOrDefault(c => c.ID == ID);
                    if (del != null)
                    {
                        //xóa hết hình ảnh của phòng này
                        db.Images.DeleteAllOnSubmit(db.Images.Where(a => a.IDProduct == del.ID).ToList());
                        db.Products.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Delete successful" });
                    }
                    return Json(new { Result = "ERROR", Message = "Hotel is not exist" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public void LoadData()
        {
            var db = new MyDBDataContext();
            var listMenu = new List<SelectListItem>();
            listMenu.Add(new SelectListItem { Value = "0", Text = "---Select a category---" });
            var getListMenu = new List<CategoryProduct>();
            getListMenu = MenuController.GetListMenu(SystemMenuLocation.MainMenu).Where(a => a.Type == SystemMenuType.CategoryProduct).ToList();
            foreach (var menu in getListMenu)
            {
                string subTitle = "";
                for (int i = 1; i <= menu.Level; i++)
                {
                    subTitle += "|--";
                }
                menu.Title = subTitle + menu.Title;
            }
            //getListMenu = GetListMenu().Where( a =>
            //  a.Type == SystemMenuType.Hotel).ToList();
            listMenu.AddRange(getListMenu.Select(a => new SelectListItem
            {
                Value =
                    a.ID.ToString(
                        CultureInfo.InvariantCulture),
                Text = a.Title
            }).ToList());
            ViewBag.ListMenu = listMenu;

            var listCate = new List<SelectListItem>();
            listCate.Add(new SelectListItem { Value = "0", Text = "---Select a trademark---" });
            var listCategory = db.Trademarks.ToList();
            listCate.AddRange(listCategory.Select(a => new SelectListItem
            {
                Value =
                    a.ID.ToString(
                        CultureInfo.InvariantCulture),
                Text = a.TrademarkName
            }).ToList());
            ViewBag.ListCategory = listCate;






            List<SelectListItem> listStar = new List<SelectListItem>();
            listStar.Add(new SelectListItem
            {
                Text = "Select star",
                Value = ""
            });
            listStar.Add(new SelectListItem
            {
                Text = "2",
                Value = "2"
            });
            listStar.Add(new SelectListItem
            {
                Text = "3",
                Value = "3"
            });
            listStar.Add(new SelectListItem
            {
                Text = "4",
                Value = "4"
            });
            listStar.Add(new SelectListItem
            {
                Text = "5",
                Value = "5"
            });
            ViewBag.ListStar = listStar;
        }

    }
}
