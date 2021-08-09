using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanMyPham.Areas.Administrator.EntityModel
{
    public class EProduct
    {
        public int ID { get; set; }

        [DisplayName("Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Category")]
        public int CategoryID { get; set; }

        [DisplayName("Trademark")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Trademark")]
        public int TrademarkID { get; set; }

        [DisplayName("Product Name")]
        [Required(ErrorMessage = "Please enter hotel name")]
        public string ProductName { get; set; }

        [DisplayName("Alias")]
        public string Alias { get; set; }

        [DisplayName("Image")]
        [Required(ErrorMessage = "Please select image")]
        public string Image { get; set; }

        [DisplayName("Price")]
        public double Price { get; set; } 

        [DisplayName("Promote")]
        public double Promote { get; set; }



        [DisplayName("Số lượng còn lại")]
        [Required(ErrorMessage = "Please enter Quantity")]
        public double Quantity { get; set; }
        


        [DisplayName("Status")]
        public bool Status { get; set; }

        [DisplayName("Xuất xứ")]
        public string MadeBy { get; set; }

        [DisplayName("Star")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select Star.")]
        public int Star { get; set; }

        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }


        public List<EGalleryITem> EGalleryITems { get; set; }
        public class EGalleryITem
        {
            public string Image { get; set; }
            public int IndexImage { get; set; }
        }

    }
}