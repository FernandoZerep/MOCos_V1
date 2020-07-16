using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MOCos_V1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your Contact page";
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Message = "Your Contact page";
            return View();
        }

        [HttpPost]
        public ActionResult Login(string correo, string password)
        {
            try
            {
                int? nivel = 0;
                string controlador = "";
                string action = "";
                using (mocOS_BDEntities db = new mocOS_BDEntities())
                {
                    var oUser = (from d in db.Usuario
                                 where d.Correo == correo && d.Contrasena == password.Trim()
                                 select d).FirstOrDefault();
                    if(oUser == null)
                    {
                        ViewBag.Error = "Usuario o Contraseña";
                        return View();
                    }
                    nivel = oUser.idTipoUsuario;
                    Session["User"] = oUser;
                    if(nivel == 1)
                    {
                        controlador = "Administrador";
                        action = "InicioAdmin";
                    }
                    if (nivel == 4)
                    {
                        controlador = "Maestro";
                        action = "Perfil";
                    }
                }
                return RedirectToAction(action, controlador);
                
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
            
        }

    }
}