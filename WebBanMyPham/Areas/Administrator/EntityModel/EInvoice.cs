using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMyPham.Areas.Administrator.EntityModel
{
    public class EInvoice
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public int Total { get; set; }
        public int CustomerName { get; set; }
        public string CreateDate { get; set; }
    }
}