using System.ComponentModel;

namespace WebBanMyPham.Areas.Administrator.ModelShow
{
    public class ShowArticle
    {
        public int ID { get; set; }

        [DisplayName("Tiêu đề")]
        public string Title { get; set; }

        [DisplayName("Chuyên mục")]
        public string TitleMenu { get; set; }

        [DisplayName("Thứ tự hiển thị")]
        public int? Index { get; set; }

        [DisplayName("Trạng thái hiển thị")]
        public bool Status { get; set; }

        [DisplayName("Bài viết giới thiệu")]
        public bool Home { get; set; }

        [DisplayName("Bài viết hot")]
        public bool Hot { get; set; }

        [DisplayName("Nhân viên")]
        public bool About { get; set; }

        [DisplayName("Bài viết nổi bật")]
        public bool New { get; set; }
        
        [DisplayName("Bài viết giá trị")]
        public bool Value { get; set; }
    }
}