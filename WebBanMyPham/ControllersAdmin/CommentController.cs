using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Library.Config;
using Library.DataBase;
using Library.Security;
using WebBanMyPham.Areas.Administrator.ModelShow;

namespace WebBanMyPham.Controllers
{
    public class CommentController : BaseController
    {
        //////////////////////// Room ///////////////////////////////////
        
       
        //lấy họ tên người đăng nhập
        public static string GetFullName(string cookie)
        {
            using (var db = new MyDBDataContext())
            {
                string cookieClient = cookie;
                string deCodecookieClient = CryptorEngine.Decrypt(cookieClient, true);
                string userName = deCodecookieClient.Substring(0, deCodecookieClient.IndexOf("||"));
                return db.Users.FirstOrDefault(a => a.UserName == userName).FullName;
            }
        }
        //lấy danh sách menu theo theo kiểu menu
        [HttpPost]
        public JsonResult GetListMenu(int typeMenu)
        {
            //check logged
            var listMenu = new List<Menu>();

            listMenu = MenuController.GetListMenu(SystemMenuLocation.ListLocationMenu().ToList()[0].LocationId);
            listMenu = listMenu.Where(a => a.Type == typeMenu).ToList();

            foreach (Menu menu in listMenu)
            {
                string titleMenu = "";
                for (int i = 1; i <= menu.Level; i++)
                {
                    titleMenu += "|--";
                }
                menu.Title = titleMenu + menu.Title;
            }
            return Json(new {Result = "OK", ListMenu = listMenu.Select(a => new {MenuId = a.ID, a.Title}).ToList()});
        }
        //Show messages
        public static string Messages(object messages)
        {
            if (messages != null)
                return messages.ToString();
            return "";
        }
        //lấy danh sách ngôn ngữ bằng ajax
        [HttpPost]
        public JsonResult ListMenuByAjax(string languageId)
        {
            if (!CurrentSession.Logined)
            {
                return Json(new { Result = "ERROR", Message = "You do not have permission to access" });
            }
            using (var db = new MyDBDataContext())
            {
                var menus = new List<Options>();
                var listLanguageTemp = db.Menus.Select(a => new Options() { DisplayText = a.Title, Value = a.ID.ToString(CultureInfo.InvariantCulture) }).ToList();
                menus.Add(new Options()
                {
                    DisplayText = "Select a Menu",
                    Value = "0"
                });
                menus.AddRange(listLanguageTemp);
                return Json(new { Result = "OK", Options = menus });
            }
        }
    }
}