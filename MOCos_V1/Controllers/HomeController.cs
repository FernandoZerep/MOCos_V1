using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MOCos_V1;
using MOCos_V1.Filters;


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
        public ActionResult pruebaregis()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InsertarUsuario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertarUsuario(Usuario obj)
        {
            try
            {
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {

                    bd.Usuario.Add(obj);
                    bd.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al ingresar al usuario", mensaje);
                return View();
            }
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
                    Session["name"] = oUser.Nombre + " " + oUser.ApellidoPaterno + " " + oUser.ApellidoMaterno;
                    Session["profile"] = oUser.FotoPerfil;
                    if (nivel == 1)
                    {
                        Session["Activo"] = db.Administrador.Include(a => a.Usuario).Include(b => b.Usuario.TiposUsuarios).Where(x => x.idUsuario == oUser.idUsuario).FirstOrDefault();
                        controlador = "Administrador";
                        action = "InicioAdmin";
                    }
                    if (nivel == 3)
                    {
                        Session["Activo"] = db.Alumnos.Include(a => a.Cuatrimestre).Include(g => g.Grupo)
                       .Include(u => u.Usuario).Include(t => t.Usuario.TiposUsuarios).Where(i => i.idUsuario == oUser.idUsuario).FirstOrDefault();
                        controlador = "Alumno";
                        action = "Inicio";
                    }
                    if (nivel == 4)
                    {
                        Session["Activo"] = db.Profesor.Include(u => u.Usuario).Include(t => t.Usuario.TiposUsuarios).Where(x => x.idUsuario == oUser.idUsuario).FirstOrDefault();

                        controlador = "Maestro";
                        action = "Perfil";
                    }
                }
                ViewBag.Sesion = Session["User"];
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