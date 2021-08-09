using System.Collections.Generic;

namespace Library.Config
{
    public class SystemMenuType
    {
        public const int AllProduct = 1;
        public const int CategoryProduct = 2;
        public const int AboutUs = 3;
        public const int Service = 4;
        public const int Article = 5;
        public const int Home = 6;



        public static Dictionary<int, string> CategoryType = new Dictionary<int, string>()
                                                                 {
                                                                     {AllProduct, "Tất cả sản phẩm"},
                                                                     {CategoryProduct, "Category Product"},
                                                                     {AboutUs, "About Us"},
                                                                     {Service, "Service"},
                                                                     {Article, "Article"},
                                                                     {Home, "Home"}
                                                                 };
    }
}