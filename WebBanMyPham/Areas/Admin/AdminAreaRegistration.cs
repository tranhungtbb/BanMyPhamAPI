using System.Web.Mvc;

namespace WebBanMyPham.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "admin/{controller}/{action}/{id}/{aliasMenu}",
                new { controller = "Login", action = "Index", id = UrlParameter.Optional, aliasMenu = UrlParameter.Optional }
            );
        }
    }
}