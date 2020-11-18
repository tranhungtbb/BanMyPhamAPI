using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMyPham.Areas.Admin.Controllers
{
    public class CommentController
    {
        //Show messages
        public static string Messages(object messages)
        {
            if (messages != null)
                return messages.ToString();
            return "";
        }
    }
}