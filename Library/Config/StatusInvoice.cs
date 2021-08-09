using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Config
{
    public class StatusInvoice
    {
        public const int Receiving = 1;
        public const int Shipping = 2;
        public const int Successful = 3;



        public static Dictionary<int, string> Status = new Dictionary<int, string>()
                                                                 {
                                                                     {Receiving, "Đã tiếp nhận"},
                                                                     {Shipping, "Đang giao hàng"},
                                                                     {Successful, "Giao hàng thành công"}
                                                                 };
    }
}
