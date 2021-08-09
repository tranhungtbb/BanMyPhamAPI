using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMyPham.Models
{
    public class ShowCard
    {
        public int ID { get; set;}
        public int ProductID { get; set; }
        public int CustomerID { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }
        public string Price { get; set; }
        public string TotalPriceString { get; set; }
        public double TotalPrice { get; set; }

        public string Image { get; set; }
    }
}