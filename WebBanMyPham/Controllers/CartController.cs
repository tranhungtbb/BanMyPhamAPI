﻿using Library.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;
using WebBanMyPham.Models;

namespace WebBanMyPham.Controllers
{
    public class CartController : ApiController
    {
        [Authorize(Roles = "User")]
        [Route("api/addToCart")]
        [HttpGet]
        public HttpResponseMessage addToCart(int id, int? count =1)
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var product = db.Products.FirstOrDefault(x => x.ID == id);
                    if (product == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = "Sản phẩm không tồn tại!" });
                    }
                    var identity = (ClaimsIdentity)User.Identity;
                    var ID = identity.Claims.FirstOrDefault(c => c.Type == "ID").Value;
                    var IDCustomer = Convert.ToInt16(ID);

                    var carts = db.Carts.Where(x => x.IDCustomer == IDCustomer).ToList();

                    var add = new Cart();
                    add.IDCustomer = int.Parse(ID);
                    add.IDProduct = product.ID;
                    add.Count = (int)count;
                    add.Price = product.Promote;
                    add.TotalPrice = product.Promote;
                    add.Image = product.Image;
                    add.ProductName = product.ProductName;
                    add.CategoryName = product.Trademark.TrademarkName;

                    bool status = false;
                    if (carts != null)
                    {
                        carts.ForEach(item => {
                            if (item.IDProduct == product.ID)
                            {
                                status = true;
                                item.Count = item.Count + (int)count;
                                item.TotalPrice = item.Count * item.Price;
                                db.SubmitChanges();
                            }
                        });
                        if(status == false)
                        {
                            db.Carts.InsertOnSubmit(add);
                            db.SubmitChanges();
                        }
                    }
                    else
                    {
                        db.Carts.InsertOnSubmit(add);
                        db.SubmitChanges();
                    }

                    


                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 1, messages = "Thêm giỏ hàng thành công!" });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }


        [Authorize(Roles = "User")]
        [Route("api/Carts")]
        [HttpGet]
        public HttpResponseMessage Carts()
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var identity = (ClaimsIdentity)User.Identity;
                    var ID = identity.Claims.FirstOrDefault(c => c.Type == "ID").Value;
                    var IDCustomer = Convert.ToInt16(ID);

                    var carts = db.Carts.Where(x => x.IDCustomer == IDCustomer).Select(x=> new ShowCard {
                        ID = x.ID,
                        ProductName = x.ProductName,
                        ProductID = x.IDProduct,
                        CustomerID = x.IDCustomer,
                        Count = x.Count,
                        Price = String.Format("{0:0,0}", x.Price),
                        TotalPriceString = String.Format("{0:0,0}", x.TotalPrice),
                        TotalPrice = x.TotalPrice,
                        Image = x.Image
                    }).ToList();
                    if(carts == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = "NotData", data = new object { } });
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 0, messages = "Success", data = carts });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }


        [Authorize(Roles = "User")]
        [Route("api/removeToCart")]
        [HttpGet]
        public HttpResponseMessage removeToCart(int productID) // chỉ cần productID là được
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var identity = (ClaimsIdentity)User.Identity;
                    var ID = identity.Claims.FirstOrDefault(c => c.Type == "ID").Value;
                    var IDCustomer = Convert.ToInt16(ID);

                    var item = db.Carts.FirstOrDefault(x => x.IDProduct == productID && x.IDCustomer == IDCustomer);
                    if (item == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, new { status = HttpStatusCode.NoContent, code = 0, messages = "Lỗi khi xóa giỏ hàng!"});
                    }
                    db.Carts.DeleteOnSubmit(item);
                    db.SubmitChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 1, messages = "Xóa sản phẩm thành công!" });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }

        [Authorize(Roles = "User")]
        [Route("api/updateToCart")]
        [HttpGet]
        public HttpResponseMessage updateToCart(int productID,int count) // count đây là 1 hoặc -1 khi ++,-- cart
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var identity = (ClaimsIdentity)User.Identity;
                    var ID = identity.Claims.FirstOrDefault(c => c.Type == "ID").Value;
                    var IDCustomer = Convert.ToInt16(ID);
                    var item = db.Carts.FirstOrDefault(x => x.IDProduct == productID && x.IDCustomer == IDCustomer);
                    if (item == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, new { status = HttpStatusCode.NoContent, code = 0, messages = "Lỗi khi cập nhập giỏ hàng!" });
                    }
                    item.Count = item.Count + count;
                    if(item.Count < 1)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.NotFound, code = 0, messages = "Số lượng sản phẩm phải > 1 !"});
                    }
                    item.TotalPrice = item.Count * item.Price;
                    db.SubmitChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 1, messages = "Cập nhập giỏ hàng thành công!" });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }


        [Authorize(Roles = "User")]
        [Route("api/Payment")]
        [HttpGet]
        public HttpResponseMessage payment() // count đây là 1 hoặc -1 khi ++,-- cart
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var identity = (ClaimsIdentity)User.Identity;
                    var ID = identity.Claims.FirstOrDefault(c => c.Type == "ID").Value;
                    var IDCustomer = Convert.ToInt16(ID);
                    var carts = db.Carts.Where(x => x.IDCustomer == IDCustomer).ToList();
                    if (carts == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, new { status = HttpStatusCode.NoContent, code = 0, messages = "Bạn chưa có sản phẩm nào!" });
                    }
                    var invoice = new Invoice
                    {
                        CreateDate = DateTime.Now,
                        Code = "2ncs3jrd4zt",
                        IDCustomer = IDCustomer,
                        Status = Library.Config.StatusInvoice.Receiving
                    };

                    db.Invoices.InsertOnSubmit(invoice);
                    db.SubmitChanges();

                    double total = 0;

                    carts.ForEach((item) =>
                    {
                        var detail = new DetailInvoice
                        {
                            IDInvoce = invoice.ID,
                            IDProduct = item.IDProduct,
                            Count = item.Count,
                            Price = item.Price,
                            TotalPrice = item.TotalPrice,
                        };
                        total += item.TotalPrice;
                        db.DetailInvoices.InsertOnSubmit(detail);
                        db.SubmitChanges();
                    });
                    invoice.Total = total;

                    db.Carts.DeleteAllOnSubmit(carts);
                    db.SubmitChanges();

                    var result = carts.Select(x => new ShowCard {
                        ProductName = x.ProductName,
                        Count = x.Count,
                        Price = String.Format("{0:0,0}", x.Price),
                        TotalPriceString = String.Format("{0:0,0}", x.TotalPrice),
                        TotalPrice = x.TotalPrice,
                        Image = x.Image
                    });
                    
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 1, messages = "Thanh toán thành công!", data = result });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }
    }
}