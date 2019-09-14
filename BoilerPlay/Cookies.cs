﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoilerPlay
{
    public class Cookies
    {
        public static readonly string CookieName = "BoilerPlayCookie";
        public static void WriteCookie(string cookieValue, HttpResponse Response)
        {
            //Create a new cookie, passing the name into the constructor
            HttpCookie cookie = new HttpCookie(Cookies.CookieName);

            //Set the cookies value
            cookie.Value = cookieValue;

            //Set the cookie to expire in 1 minute
            DateTime dtNow = DateTime.Now;
            TimeSpan tsMinute = new TimeSpan(0, 10, 0, 0);
            cookie.Expires = dtNow + tsMinute;

            //Add the cookie
            Response.Cookies.Add(cookie);
        }
        public static string ReadCookie(HttpRequest Request, HttpResponse Response)
        {
            //Grab the cookie
            HttpCookie cookie = Request.Cookies[Cookies.CookieName];

            //Check to make sure the cookie exists
            if (null == cookie)
            {
                Response.Write("Cookie not found. <br><hr>");
                return string.Empty;
            }
            else
            {
                //Write the cookie value
                String strCookieValue = cookie.Value.ToString();
                return strCookieValue;
                //Response.Write("The " + cookieName + " cookie contains: <b>"
                //                      + strCookieValue + "</b><br><hr>");
            }
        }
    }
}