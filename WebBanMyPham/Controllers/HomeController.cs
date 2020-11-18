using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Routing;
using Library.DataBase;
using WebBanMyPham.Models;
//using System.

namespace WebBanMyPham.Controllers
{
    public class HomeController : ApiController
    {
        // function getListMenu by Location
        
        [Route("api/getMenuByLocation")]
        [HttpGet]
        public HttpResponseMessage menuByLocation(int location)
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var listMenu = db.Menus.Where(x => x.Status && x.Location == location).ToList();
                    if (listMenu == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.NotFound, code = 1, messages = "Not data", data = new object { } });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 0, messages = "Success", data = listMenu });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }

        // function getListSlide index
        [Route("api/getListSlide")]
        [HttpGet]
        public HttpResponseMessage listSlide()
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var listResult = db.Slides.OrderBy(x => x.Index).ToList();
                    if (listResult.Count == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.NotFound, code = 1, messages = "Not data", data = new object { } });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 0, messages = "Success", data = listResult });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }

        // function get detail Company

        [Route("api/getDetailCompany")]
        [HttpGet]
        public HttpResponseMessage detailCompany()
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var result = db.Companies.Select(x => new ShowCompany()
                    {
                        Name = x.Name,
                        Logo = x.Logo,
                        Tel = x.Tel,
                        Hotline = x.Hotline,
                        Email = x.Email,
                        Description = x.Description,
                        Address = x.Address,
                        FaceBook = x.Facebook,
                        CopyRight = x.CopyRight,
                        MetaTitle = x.MetaTitle,
                        MetaDescription = x.MetaDescription
                    }).FirstOrDefault();
                    if(result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.NotFound, code = 0, messages = "Not data", data = new object { } });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.NotFound, code = 1, messages = "Success", data = result });
                }
                catch(Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }
        // all product

        [Route("api/getAllProduct")]
        [HttpGet]
        public HttpResponseMessage listProducts()
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var listResult = db.Products.Where(x => x.Status).AsQueryable()
                        .Join(db.CategoryProducts, a => a.CategoryID, b => b.ID,
                        (a, b) => new ShowObject
                        {
                            ID = a.ID,
                            ProductName = a.ProductName,
                            Alias = a.Alias,
                            Image = a.Image,
                            Quantity = a.Quantity,
                            QuantitySold = a.QuantitySold,
                            Discount = (a.QuantitySold == null) ? 0 : Math.Round((double)a.QuantitySold / a.Quantity * 100, MidpointRounding.AwayFromZero),
                            Star = a.Start,
                            Price = a.Price,
                            Promote = a.Promote,
                            Madeby = a.MadeBy,
                            CategoryName = b.Category
                        }).ToList();
                    if (listResult.Count == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.NotFound, code = 1, messages = "Not data", data = new object { } });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 0, messages = "Success", data = listResult });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }



        // function getListProductSale sort by max %Discount

        [Route("api/getListProductSale")]
        [HttpGet]
        public HttpResponseMessage listProductSale()
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var listResult = db.Products.Where(x => x.Status).AsQueryable()
                        .Join(db.CategoryProducts, a => a.CategoryID, b => b.ID,
                        (a, b) => new ShowObject {
                            ID = a.ID,
                            ProductName = a.ProductName,
                            Alias = a.Alias,
                            Image = a.Image,
                            Quantity = a.Quantity,
                            QuantitySold = a.QuantitySold,
                            Discount = (a.QuantitySold == null) ? 0 : Math.Round((double)a.QuantitySold / a.Quantity * 100, MidpointRounding.AwayFromZero),
                            Star = a.Start,
                            Price = a.Price,
                            Promote = a.Promote,
                            Madeby = a.MadeBy,
                            CategoryName = b.Category
                        }).OrderBy(x=>x.Discount).Take(3).ToList();
                    if (listResult.Count == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.NotFound, code = 1, messages = "Not data", data = new object { } });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 0, messages = "Success", data = listResult });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }


        // function getListProductNew
        [Route("api/getListProductNew")]
        [HttpGet]
        public HttpResponseMessage listProductNew()
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var listResult = db.Products.Where(x => x.Status).AsQueryable()
                        .Join(db.CategoryProducts, a => a.CategoryID, b => b.ID,
                        (a, b) => new ShowObject
                        {
                            ID = a.ID,
                            ProductName = a.ProductName,
                            Alias = a.Alias,
                            Image = a.Image,
                            Quantity = a.Quantity,
                            QuantitySold = a.QuantitySold,
                            Discount = (a.QuantitySold == null) ? 0 : Math.Round((double)a.QuantitySold / a.Quantity * 100, MidpointRounding.AwayFromZero),
                            Star = a.Start,
                            Price = a.Price,
                            Promote = a.Promote,
                            Madeby = a.MadeBy,
                            CategoryName = b.Category
                        }).OrderBy(x=>x.ID).Take(15).ToList();
                    if (listResult.Count == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.NotFound, code = 1, messages = "Not data", data = new object { } });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 0, messages = "Success", data = listResult });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }

        // sản phẩm bán chạy cần phải lấy
        // dựa vào số lượng sản phẩm bán ra trong hóa đơn để lấy ra nhiều nhất
        // function getListProductBestSale
        [Route("api/getListProductBestSale")]
        [HttpGet]
        public HttpResponseMessage listProductBestSale()
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    // lấy danh sách product có số lượng trong hóa đơn nhiều nhất

                    //var listProduct = db.Products.
                    // https://stackoverflow.com/questions/16522645/linq-groupby-sum-and-count



                    // lấy
                    var list = db.Products.Where(x => x.Status).AsQueryable()
                        .Join(db.DetailInvoices, a => a.ID, b => b.IDProduct , (a,b) => new { a,b})
                        .GroupBy(x=>x.a.ID)
                        .Select(g => new
                        {
                            ID = g.Key,
                            Count = g.Sum(x=>x.b.Count)
                        }).OrderByDescending(x=>x.Count).ToList();

                    var listResult = new List<ShowObject>();

                    foreach (var item in list)
                    {
                        var k = db.Products.Where(x => x.ID == item.ID).AsQueryable()
                        .Join(db.CategoryProducts, a => a.CategoryID, b => b.ID,
                        (a, b) => new ShowObject
                        {
                            ID = a.ID,
                            ProductName = a.ProductName,
                            Alias = a.Alias,
                            Image = a.Image,
                            Quantity = a.Quantity,
                            QuantitySold = a.QuantitySold,
                            Discount = (a.QuantitySold == null) ? 0 : Math.Round((double)a.QuantitySold / a.Quantity * 100, MidpointRounding.AwayFromZero),
                            Star = a.Start,
                            Price = a.Price,
                            Promote = a.Promote,
                            Madeby = a.MadeBy,
                            CategoryName = b.Category
                        }).FirstOrDefault();
                        if(k != null)
                        {
                            listResult.Add(k);
                        }
                    }

                    

                    //listResult = db.Products 
                    if (listResult == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.NotFound, code = 1, messages = "Not data", data = new object { } });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 0, messages = "Success", data = listResult });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }

        // lấy danh sách các loại category
        
        [Route("api/getListCategory")]
        [HttpGet]
        public HttpResponseMessage listCategory()
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var listCate = db.CategoryProducts.ToList();
                    var listResult = new List<Categories>();
                    
                    listCate.ForEach(item =>
                        {
                            var products = db.Products.Where(x => x.Status && x.CategoryID == item.ID).ToList();
                            if (products.Count > 0)
                            {
                                listResult.Add(new Categories { ID = item.ID, CategoryName = item.Category });
                            }
                        }
                    );

                    if (listResult.Count == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.NotFound, code = 1, messages = "Not data", data = new object { } });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 0, messages = "Success", data = listResult });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }




        // function getListProductByCategory
        // nhưng mỗi danh mục phải lấy ít nhất 1 sản phẩm, tối đa 7 sản phẩm
        [Route("api/getListProductByCategory")]
        [HttpGet]
        public HttpResponseMessage listProductByCategory(int categoryID)
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var listProducts = db.Products.Where(x => x.Status).AsQueryable()
                        .Join(db.CategoryProducts.Where(x => x.ID == categoryID), a => a.CategoryID, b => b.ID,
                        (a,b)=>new { a,b }).Join(db.Menus,a=>a.a.MenuID,b=>b.ID, (a,b)
                    => new ShowObject
                       {
                           ID = a.a.ID,
                           ProductName = a.a.ProductName,
                           Alias = a.a.Alias,
                           Image = a.a.Image,
                           Quantity = a.a.Quantity,
                           QuantitySold = a.a.QuantitySold,
                           Discount = (a.a.QuantitySold == null) ? 0 : Math.Round((double)a.a.QuantitySold / a.a.Quantity * 100, MidpointRounding.AwayFromZero),
                           Star = a.a.Start,
                           Price = a.a.Price,
                           Promote = a.a.Promote,
                           Madeby = a.a.MadeBy,
                           CategoryName = a.b.Category,
                           MenuID = b.ID,
                           MenuIndex = b.Index
                       }).OrderBy(x=>x.MenuIndex).ToList();
                    var listResult = listProducts.GroupBy(x => x.MenuID).Select(x => x.First()).ToList();
                    listResult.ForEach(item => listProducts.Remove(item));
                    while (listResult.Count < 7)
                    {
                        listResult.AddRange(listProducts.GroupBy(x => x.MenuID).Select(x => x.First()).Take(7- listResult.Count()).ToList());
                    }
                    if (listResult.Count == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.NotFound, code = 1, messages = "Not data", data = new object { } });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 0, messages = "Success", data = listResult });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }


        // function getListProductByMenu
        [Route("api/getListProductByMenuID")]
        [HttpGet]
        public HttpResponseMessage listProductByMenu(int menuID)
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var listResult = db.Products.Where(x => x.Status).AsQueryable()
                        .Join(db.CategoryProducts, a => a.CategoryID, b => b.ID,
                        (a, b) => new { a, b }).Join(db.Menus.Where(x => x.ID == menuID), a => a.a.MenuID, b => b.ID, (a, b)
                              => new ShowObject
                              {
                                  ID = a.a.ID,
                                  ProductName = a.a.ProductName,
                                  Alias = a.a.Alias,
                                  Image = a.a.Image,
                                  Quantity = a.a.Quantity,
                                  QuantitySold = a.a.QuantitySold,
                                  Discount = (a.a.QuantitySold == null) ? 0 : Math.Round((double)a.a.QuantitySold / a.a.Quantity * 100, MidpointRounding.AwayFromZero),
                                  Star = a.a.Start,
                                  Price = a.a.Price,
                                  Promote = a.a.Promote,
                                  Madeby = a.a.MadeBy,
                                  CategoryName = a.b.Category,
                                  MenuID = b.ID,
                                  MenuIndex = b.Index
                              }).OrderBy(x => x.MenuIndex).ToList();
                    if (listResult.Count == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.NotFound, code = 1, messages = "Not data", data = new object { } });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 0, messages = "Success", data = listResult });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }



        // list sản phẩm liên quan
        // dựa vào productID lấy ra menuID (cùng danh mục)
        // function getListProductRecommend
        [Route("api/getListProductRecommend")]
        [HttpGet]
        public HttpResponseMessage listProductRecommend(int productID)
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var product = db.Products.FirstOrDefault(x => x.ID == productID);
                    var listResult = db.Products.Where(x => x.Status && x.MenuID == product.MenuID && x.ID != productID ).AsQueryable()
                        .Join(db.CategoryProducts, a => a.CategoryID, b => b.ID,
                        (a, b) => new ShowObject
                        {
                            ID = a.ID,
                            ProductName = a.ProductName,
                            Alias = a.Alias,
                            Image = a.Image,
                            Quantity = a.Quantity,
                            QuantitySold = a.QuantitySold,
                            Discount = (a.QuantitySold == null) ? 0 : Math.Round((double)a.QuantitySold / a.Quantity * 100, MidpointRounding.AwayFromZero),
                            Star = a.Start,
                            Price = a.Price,
                            Promote = a.Promote,
                            Madeby = a.MadeBy,
                            CategoryName = b.Category
                        }).OrderBy(x => x.ID).Take(15).ToList();
                    if (listResult.Count == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.NotFound, code = 1, messages = "Not data", data = new object { } });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 0, messages = "Success", data = listResult });
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messages = ex.Message });
                }
            }
        }


        // detailProduct
        // function detailProduct
        [Route("api/getProductByID")]
        [HttpGet]
        public HttpResponseMessage detailProduct(int id)
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var result = db.Products.Where(x => x.ID == id).AsQueryable()
                        .Join(db.CategoryProducts, a => a.CategoryID, b => b.ID, (a, b) => new { a, b })
                        .Join(db.Menus, a => a.a.MenuID, b => b.ID, (a, b) => new ShowObject {
                            ID = a.a.ID,
                            ProductName = a.a.ProductName,
                            Alias = a.a.Alias,
                            Image = a.a.Image,
                            Quantity = a.a.Quantity,
                            QuantitySold = a.a.QuantitySold,
                            Discount = (a.a.QuantitySold == null) ? 0 : Math.Round((double)a.a.QuantitySold / a.a.Quantity * 100, MidpointRounding.AwayFromZero),
                            Star = a.a.Start,
                            Price = a.a.Price,
                            Promote = a.a.Promote,
                            Madeby = a.a.MadeBy,
                            CategoryName = a.b.Category,
                            Content = a.a.Content,
                            MenuID = b.ID,
                            MenuName = b.Title
                        }).FirstOrDefault();
                    if (result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messager = "Not data", data = new object { } });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 0, messager = "Success", data =  result});
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messager = ex.Message });
                }
            }
        }

        // list tin tức

        [Route("api/getlistNews")]
        [HttpGet]
        public HttpResponseMessage listNews()
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var listResult = new List<ShowArticle>();
                    listResult = db.Articles.Where(x => x.Status == true)
                        .Join(db.Menus, a=>a.MenuID, b=>b.ID , (a,b)=> new { a,b})
                        .Select(x => new ShowArticle
                        {
                            ID = x.a.ID,
                            Title = x.a.Title,
                            Alias = x.a.Alias,
                            Image = x.a.Image,
                            Index = (int)x.a.Index,
                            Content = x.a.Content,
                            MenuID = x.b.ID,
                            MenuName = x.b.Title,
                            MenuAlias = x.b.Alias

                        }).OrderBy(x => x.Index).Take(10).ToList();
                    if (listResult.Count == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messager = "Not data", data = new object { } });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 0, messager = "Success", data = listResult });
                }
                catch(Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { status = HttpStatusCode.NotFound, code = 0, messager = ex.Message });
                }
            }
        }
    }
}
