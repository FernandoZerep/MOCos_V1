using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MOCos_V1.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple =false)]
    public class AuthorizeUser : AuthorizeAttribute
    {
        private Usuario oUsuario;
        private mocOS_BDEntities db = new mocOS_BDEntities();
        private int nivel;

        public AuthorizeUser(int idNivel)
        {
            this.nivel = idNivel;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                oUsuario = (Usuario)HttpContext.Current.Session["User"];
                var nivelOp = from m in db.Usuario
                              where m.Correo == oUsuario.Correo
                              && m.idTipoUsuario == nivel
                              select m.idTipoUsuario;

                if(nivelOp.ToList().Count() ==0)
                {
                    if (oUsuario.idTipoUsuario == 1)
                    {
                        filterContext.Result = new RedirectResult("~/Administrador/InicioAdmin");
                    }
                    if (oUsuario.idTipoUsuario == 4)
                    {
                        filterContext.Result = new RedirectResult("~/Maestro/Perfil");
                    }
                    if (oUsuario.idTipoUsuario == null)
                    {
                        filterContext.Result = new RedirectResult("~/Home/Index");
                    }
                }
            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
        }

    }
}