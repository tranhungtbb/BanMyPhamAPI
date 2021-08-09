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
                    var listMenu = db.CategoryProducts.Where(x => x.Status && x.Location == location)
                        .Select(x=> new ShowMenu {
                            ID = x.ID,
                            Title = x.Title,
                            Alias = x.Alias,
                            ParentID = (int)x.ParentID,
                            Type = x.Type,
                            Index = x.Index,
                            Location = x.Location,
                            Status = x.Status,
                            MetaDescription = x.MetaDescription,
                            MetaTitle = x.MetaTitle
                        })
                        .ToList();
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
                    var listResult = db.Slides.Where(x=>(bool)x.Ismain).OrderBy(x => x.Index).ToList();
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
                        .Join(db.Trademarks, a => a.TrademarkID, b => b.ID,
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
                            Price = String.Format("{0:0,0}", a.Price),
                            Promote = String.Format("{0:0,0}", a.Promote),
                            Madeby = a.MadeBy,
                            Trademark = b.TrademarkName
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
                        .Join(db.Trademarks, a => a.TrademarkID, b => b.ID,
                        (a, b) => new ShowObject {
                            ID = a.ID,
                            ProductName = a.ProductName,
                            Alias = a.Alias,
                            Image = a.Image,
                            Quantity = a.Quantity,
                            QuantitySold = a.QuantitySold,
                            Discount = (a.QuantitySold == null) ? 0 : Math.Round((double)a.QuantitySold / a.Quantity * 100, MidpointRounding.AwayFromZero),
                            Star = a.Start,
                            Price = String.Format("{0:0,0}", a.Price),
                            Promote = String.Format("{0:0,0}", a.Promote),
                            Madeby = a.MadeBy,
                            Trademark = b.TrademarkName
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
                        .Join(db.Trademarks, a => a.TrademarkID, b => b.ID,
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
                            Price = String.Format("{0:0,0}", a.Price),
                            Promote = String.Format("{0:0,0}", a.Promote),
                            Madeby = a.MadeBy,
                            Trademark = b.TrademarkName
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
                        .Join(db.Trademarks, a => a.TrademarkID, b => b.ID,
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
                            Price = String.Format("{0:0,0}", a.Price),
                            Promote = String.Format("{0:0,0}", a.Promote),
                            Madeby = a.MadeBy,
                            CategoryName = b.TrademarkName
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

        // lấy danh sách các thuong hieu
        
        [Route("api/getListTrademark")]
        [HttpGet]
        public HttpResponseMessage listTrademark()
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var listCate = db.Trademarks.ToList();
                    var listResult = new List<Categories>();
                    
                    listCate.ForEach(item =>
                        {
                            var products = db.Products.Where(x => x.Status && x.TrademarkID == item.ID).ToList();
                            if (products.Count > 0)
                            {
                                listResult.Add(new Categories { ID = item.ID, TrademarkName = item.TrademarkName });
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



        
        // nhưng mỗi danh mục phải lấy ít nhất 1 sản phẩm, tối đa 7 sản phẩm
        [Route("api/getListProductByTrademark")]
        [HttpGet]
        public HttpResponseMessage listProductByTradeMark(int trademarkID)
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var listProducts = db.Products.Where(x => x.Status).AsQueryable()
                        .Join(db.Trademarks.Where(x => x.ID == trademarkID), a => a.TrademarkID, b => b.ID,
                        (a,b)=>new { a,b }).Join(db.CategoryProducts,a=>a.a.CategoryID,b=>b.ID, (a,b)
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
                           Price = String.Format("{0:0,0}", a.a.Price),
                           Promote = String.Format("{0:0,0}", a.a.Promote),
                           Madeby = a.a.MadeBy,
                           CategoryName = b.Title,
                           CategoryID = b.ID,
                           CategoryIndex = b.Index
                       }).OrderBy(x=>x.CategoryIndex).ToList();
                    var listResult = listProducts.GroupBy(x => x.CategoryID).Select(x => x.First()).ToList();
                    listResult.ForEach(item => listProducts.Remove(item));
                    while (listResult.Count < 7)
                    {
                        listResult.AddRange(listProducts.GroupBy(x => x.CategoryIndex).Select(x => x.First()).Take(7- listResult.Count()).ToList());
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

        // list type sort
        [Route("api/getTypeSorts")]
        [HttpGet]
        public HttpResponseMessage listTypeSort() {
            var data = Library.Config.TypeSort.ListTypeSort();
            if (data.Count > 0) {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.OK, code = 1, messages = "Success", data = data });
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { status = HttpStatusCode.NotFound, code = 0, messages = "Not data", data = new object { } });
        }

        // function getListProductByMenu
        [Route("api/getListProductByCategory")]
        [HttpGet]
        public HttpResponseMessage listProductByCategory(string menuAlias = "", int? typeSort = 0,  int? pageSize = 9, int? pageNumber = 1)
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                   int pagesize = pageSize ?? 9;
                   int pagenumber = pageNumber ?? 1;
                    var listResult = db.Products.Where(x => x.Status).AsQueryable()
                        .Join(db.Trademarks, a => a.TrademarkID, b => b.ID,
                        (a, b) => new { a, b }).Join(db.CategoryProducts.Where(x => x.Alias == menuAlias), a => a.a.CategoryID, b => b.ID, (a, b)
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
                                  Price = String.Format("{0:0,0}", a.a.Price),
                                  Promote = String.Format("{0:0,0}",a.a.Promote),
                                  Madeby = a.a.MadeBy,
                                  CategoryName = a.b.TrademarkName,
                                  CategoryID = b.ID,
                                  CategoryIndex = b.Index
                              }).ToList();

                    switch (typeSort) {
                        case Library.Config.TypeSort.Default:
                            listResult = listResult.OrderBy(x=>x.ID).Skip((pagenumber-1) * pagesize).Take(pagesize).ToList();
                            break;
                        case Library.Config.TypeSort.Increate:
                            listResult = listResult.OrderByDescending(x => x.Price).Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
                            break;
                        case Library.Config.TypeSort.Decreate:
                            listResult = listResult.OrderBy(x => x.Price).Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
                            break;
                        case Library.Config.TypeSort.Discount:
                            listResult = listResult.OrderBy(x => x.Discount).Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
                            break;
                        case Library.Config.TypeSort.New:
                            listResult = listResult.OrderByDescending(x => x.ID).Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
                            break;
                        default:
                            listResult = listResult.OrderBy(x => x.ID).Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
                            break;
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
                    var listResult = db.Products.Where(x => x.Status && x.CategoryID == product.CategoryID && x.ID != productID ).AsQueryable()
                        .Join(db.Trademarks, a => a.TrademarkID, b => b.ID,
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
                            Price = String.Format("{0:0,0}", a.Price),
                            Promote = String.Format("{0:0,0}", a.Promote),
                            Madeby = a.MadeBy,
                            CategoryName = b.TrademarkName
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
                        .Join(db.Trademarks, a => a.TrademarkID, b => b.ID, (a, b) => new { a, b })
                        .Join(db.CategoryProducts, a => a.a.CategoryID, b => b.ID, (a, b) => new ShowObject {
                            ID = a.a.ID,
                            ProductName = a.a.ProductName,
                            Alias = a.a.Alias,
                            Image = a.a.Image,
                            Quantity = a.a.Quantity,
                            QuantitySold = a.a.QuantitySold,
                            Discount = (a.a.QuantitySold == null) ? 0 : Math.Round((double)a.a.QuantitySold / a.a.Quantity * 100, MidpointRounding.AwayFromZero),
                            Star = a.a.Start,
                            Price = String.Format("{0:0,0}", a.a.Price),
                            Promote = String.Format("{0:0,0}", a.a.Promote),
                            Madeby = a.a.MadeBy,
                            Trademark = a.b.TrademarkName,
                            Content = a.a.Content,
                            CategoryID = b.ID,
                            CategoryName = b.Title
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
                        .Join(db.CategoryProducts, a=>a.MenuID, b=>b.ID , (a,b)=> new { a,b})
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

        // search by key
        [Route("api/getListProductSearch")]
        [HttpGet]
        public HttpResponseMessage searchProduct(string key)
        {
            using (var db = new MyDBDataContext())
            {
                try
                {
                    var k = key.Replace(" ", string.Empty).ToLower();
                    var listResult = db.Products.Where(x => x.Status && x.ProductName.Replace(" ", string.Empty).ToLower().Contains(k)).AsQueryable()
                        .Join(db.Trademarks, a => a.TrademarkID, b => b.ID,
                        (a, b) => new { a, b }).Join(db.CategoryProducts, a => a.a.CategoryID, b => b.ID, (a, b)
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
                                  Price = String.Format("{0:0,0}", a.a.Price),
                                  Promote = String.Format("{0:0,0}", a.a.Promote),
                                  Madeby = a.a.MadeBy,
                                  Trademark = a.b.TrademarkName,
                                  CategoryID = b.ID,
                                  CategoryIndex = b.Index
                              }).OrderBy(x => x.CategoryIndex).ToList();
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
    }
}
