using System;
using System.Collections;
using System.Web;
using System.Collections.Generic;

namespace GerenciamentoHotel.Models
{
    public class AppUser
    {
        public static UserAuthenticated Authenticated
        {
            get
            {
                return HttpContext.Current.Session["USER_SESSION"] as UserAuthenticated;
            }
        }

        public static void LogIn(tb_funcionario user,HashSet<string> allowedRoutes)
        {
            HttpContext.Current.Session["USER_SESSION"] = new UserAuthenticated(user,allowedRoutes);
        }

        public static void LogOut()
        {
            HttpContext.Current.Session["USER_SESSION"] = null;
        }
    }
}