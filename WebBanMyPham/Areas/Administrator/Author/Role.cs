using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeamplateHotel.Areas.Administrator.Author
{
    public class Role
    {
        public const string Admin = "1";
        public const string Editor = "2";

        public string _role;
        public string _roleName;
        public Role() { }
        public List<Role> ListRole()
        {
            var list = new List<Role>
                                       {
                                           new Role
                                               {
                                                   _role = Admin.ToString(),
                                                   _roleName = "Admin"
                                                   
                                               },
                                           new Role
                                               {
                                                   _role = Editor.ToString(),
                                                   _roleName = "Editor"
                                                   
                                               }
                                       };
            return list;
        }
    }
}