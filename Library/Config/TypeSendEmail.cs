using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Library.Config
{
    public class TypeSendEmail
    {
        public static int Contact = 1;
        public static int Booking = 2;
        public static int Register = 3;
        public static List<ListItem> ListTypeSendEmail()
        {
            var listTypeSendEmail = new List<ListItem>
            {
                new ListItem
                {
                    Value = Contact.ToString(),
                    Text = "Liên hệ",
                },
                new ListItem
                {
                    Value = Booking.ToString(),
                    Text = "Đặt hàng",
                },
                new ListItem
                {
                    Value = Register.ToString(),
                    Text = "Đăng kí",
                }
            };

            return listTypeSendEmail;
        }
    }
}