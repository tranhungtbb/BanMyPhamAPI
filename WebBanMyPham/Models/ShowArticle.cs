using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMyPham.Models
{
    public class ShowArticle
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public int Index { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuAlias { get; set; }
    }
}