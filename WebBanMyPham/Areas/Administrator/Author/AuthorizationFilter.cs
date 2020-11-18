////using DotNetOpenAuth.OpenId.Provider;
//using Library.DataBase;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace TeamplateHotel.Areas.Administrator.Author
//{
//    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
//    {
//        readonly string[] _claim;

//        public AuthorizationFilter(params string[] claim)
//        {
//            _claim = claim;
//        }
//        public string getUri(AuthorizationContext context, string controller, string action)
//        {
//            UrlHelper urlHelper = new UrlHelper(context.RequestContext);
//            return urlHelper.Action(action, controller);
//        }

//        public override void OnAuthorization(AuthorizationContext filterContext)
//        {
//            var db = new MyDBDataContext();
//            // N.V.T -- kiểm tra nếu không có yêu cầu nào thì cho phép truy cập 
//            if (_claim.Length == 0)
//            {
//                return;
//            }

//            string userName = HttpContext.Current.Session["UserName"].ToString();

//            User user = db.Users.FirstOrDefault(x => x.UserName == userName);

//            //if (user == null)
//            //{
//            //    // Có thể trả về một trang Unauthorised khác
//            //    filterContext.Result = new RedirectResult(getUri(filterContext, "Error", "Err403"));
//            //    return;
//            //}

//            // kiểm tra với danh sách yêu cầu
//            bool flag = false;
//            foreach (var item in _claim)
//            {
//                if (item.Equals(user.Role.ToString()))
//                    flag = true;
//            }
//            if (flag == false)
//            {
//                filterContext.Result = new RedirectResult(getUri(filterContext, "ControlPanel", "Index")); // bỏ
//            }

//        }
//    }
//}