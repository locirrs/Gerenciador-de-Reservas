using System;
using System.Collections;
using System.Web;
using System.Collections.Generic;

namespace GerenciamentoHotel.Models
{
    public class UserAuthenticated
    {
        public HashSet<string> _allowedRoutes;


        private tb_funcionario _usuarioLogado;



        public tb_funcionario UsuarioLogado { get { return _usuarioLogado; } }

        public HashSet<string> Permissions { get { return _allowedRoutes; } }


        public UserAuthenticated(tb_funcionario current, HashSet<string> allowedRoutes)
        {
            _usuarioLogado = current;
            _allowedRoutes = allowedRoutes;
            HttpContext.Current.Session["USER_SESSION"] = this;
        }

        public bool HasPermission(string controller)
        {
            if (this.UsuarioLogado.tipo_usuario == 1)
            {
                return true;
            }


            var key = string.Concat(controller,"Controller");

            return Permissions.Contains(key);
        }

    }
}