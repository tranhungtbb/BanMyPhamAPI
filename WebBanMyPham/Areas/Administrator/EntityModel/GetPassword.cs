using System.ComponentModel.DataAnnotations;

namespace WebBanMyPham.Areas.Administrator.EntityModel
{
    public class GetPassword
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}