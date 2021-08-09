using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebBanMyPham.Areas.Administrator.EntityModel
{
    public class ESlider
    {
        public int ID { get; set; }
        
        [MaxLength]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Image { get; set; }
        

        public bool IsMain { get; set; }

        [DisplayName("Mô tả")]
        public string Description { get; set; }
    }
}