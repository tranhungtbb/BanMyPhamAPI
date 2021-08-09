using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebBanMyPham.Areas.Administrator.EntityModel
{
    public class EHotel
    {
        public int ID { get; set; }

        public string LanguageID { get; set; }

        [Required]
        [MaxLength]
        public string Name { get; set; }

        [Required]
        public string Logo { get; set; }
        

        [Required]
        [MaxLength]
        public string Tel { get; set; }

        [MaxLength]
        public string Fax { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(250)]
        public string Address { get; set; }
        

        public string FaceBook { get; set; }
        
        public string Twitter { get; set; }


        public string Youtube { get; set; }

        [Required]
        public string CopyRight { get; set; }

        [MaxLength(250)]
        public string MetaTitle { get; set; }

        [MaxLength(250)]
        public string MetaDescription { get; set; }
        

        [Required]
        public string Hotline { get; set; }

    }
}