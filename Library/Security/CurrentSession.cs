﻿using System;
using System.Globalization;
using System.Threading;
using System.Web;
using Library.Config;
using System.Linq;
using Library.DataBase;

namespace Library.Security
{
    public class CurrentSession
    {
        public static void SetTimeOut(int hours)
        {
            HttpContext.Current.Session.Timeout = hours;
        }
        public static int GetTimeOut()
        {
            return HttpContext.Current.Session.Timeout;
        }

        public static string SystemErrors
        {
            get
            {
                return HttpContext.Current.Session["SystemErrors"] != null ? HttpContext.Current.Session["SystemErrors"].ToString() : null;
            }
            set
            {
                HttpContext.Current.Session["SystemErrors"] = value;
            }
        }

        public static string CapCha
        {
            get
            {
                return HttpContext.Current.Session["CapCha"] != null ? HttpContext.Current.Session["CapCha"].ToString() : null;
            }
            set
            {
                HttpContext.Current.Session["CapCha"] = value;
            }
        }

        public static DateTime LockUser
        {
            get
            {
                return HttpContext.Current.Session["LockUser"] != null
                           ? Convert.ToDateTime(HttpContext.Current.Session["LockUser"])
                           : DateTime.Now;
            }
            set
            {
                HttpContext.Current.Session["LockUser"] = value;
            }
        }

        public static bool Logined
        {
            get
            {
                return HttpContext.Current.Session["Logined"] != null && Convert.ToBoolean(HttpContext.Current.Session["Logined"]);
            }
            set
            {
                HttpContext.Current.Session["Logined"] = value;
            }
        }
        public static int Role
        {
            get
            {
                return HttpContext.Current.Session["Role"] != null ? Convert.ToInt32(HttpContext.Current.Session["Role"]) : 4;
            }
            set
            {
                HttpContext.Current.Session["Role"] = value;
            }
        }
        public static object UserId { get; set; }

        //clear all session
        public static void ClearAll()
        {
            HttpContext.Current.Session.RemoveAll();
        }
    }
}
