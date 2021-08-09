using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMyPham.Models
{
    public class ShowMenu
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public int ParentID { get; set; }
        public int Type { get; set; }
        public int Index { get; set; }
        public int Location { get; set; }
        public bool Status { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
    }
}