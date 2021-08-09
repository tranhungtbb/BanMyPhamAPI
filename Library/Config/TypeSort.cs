using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Config
{
    public class TypeSort
    {
        public const int Default = 1;
        public const int New = 2;
        public const int Discount = 3;
        public const int Increate = 4;
        public const int Decreate = 5;

        public int typeSort {get; set;}
        public string titleSort {get; set;}

        public static List<TypeSort> ListTypeSort()
        {
            var listTypeSort = new List<TypeSort>
                                       {
                                           new TypeSort
                                               {
                                                   typeSort = Default,
                                                   titleSort = "Mặc định",
                                               },
                                           new TypeSort
                                               {
                                                  typeSort = New,
                                                   titleSort = "Mới nhất",
                                               },

                                           new TypeSort
                                               {
                                                  typeSort = Discount,
                                                   titleSort = "Khuyến mại",
                                               },

                                           new TypeSort
                                               {
                                                   typeSort = Increate,
                                                   titleSort = "Tăng dần",
                                               },
                                           new TypeSort
                                               {
                                                  typeSort = Decreate,
                                                   titleSort = "Giảm dần",
                                               },

                                       };

            return listTypeSort;
        }
    }
}
