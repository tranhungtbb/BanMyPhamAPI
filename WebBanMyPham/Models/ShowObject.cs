using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMyPham.Models
{
    public class ShowObject
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string Alias { get; set; }
        public string Image { get; set; }
        public double Discount { get; set; }// %
        public double Quantity { get; set; }
        public double? QuantitySold { get; set; }
        public int Star { get; set; }
        public string Price { get; set; }
        public string Promote { get; set; }
        public string Madeby { get; set; }
        public string Trademark { get; set; }
        public string Content { get; set; }
        public int CategoryID { get; set; }
        public int CategoryIndex { get; set; }
        public string CategoryName { get; set; }
    }
}