using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMyPham.Areas.Administrator.EntityModel
{
    public class EAboutUs
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public int? Index { get; set; }
    }
}