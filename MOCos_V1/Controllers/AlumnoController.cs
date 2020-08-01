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
    public class AlumnoController : Controller
    {
        mocOS_BDEntities bd = new mocOS_BDEntities();

        [AuthorizeUser(idNivel: 3)]
        public ActionResult Inicio()
        {
            return View();
        }
        [AuthorizeUser(idNivel: 3)]
        public ActionResult Cerrar()
        {
            Session["User"] = null;
            Session["name"] = null;
            Session["profile"] = null;
            return RedirectToAction("Index", "Home");
        }
        [AuthorizeUser(idNivel: 3)]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [AuthorizeUser(idNivel: 3)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your Contact page";
            return View();
        }
        [AuthorizeUser(idNivel: 3)]
        public ActionResult Login()
        {
            return View();
        }
        [AuthorizeUser(idNivel: 3)]
        public ActionResult pruebaregis()
        {
            return View();
        }
        [AuthorizeUser(idNivel: 3)]
        public ActionResult ConsultaProfesores()
        {
            try
            {
                List<Profesor> lista = bd.Profesor.Include(u => u.Usuario).ToList();
                return View(lista);
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al consultar a los profesores", mensaje);
                return View();
            }
        }

        //public ActionResult DetallesProfesor(int id)
        //{
        //    try
        //    {
        //        using (mocOS_BDEntities bd = new mocOS_BDEntities())
        //        {
        //            Profesor existe = bd.Profesor.Find(id);

        //            return View(existe);
        //        }
        //    }
        //    catch (Exception mensaje)
        //    {
        //        ModelState.AddModelError("Error al mostrar detalles de la persona", mensaje);
        //        return View();
        //    }
        //}
        [AuthorizeUser(idNivel: 3)]
        [HttpGet]
        public ActionResult EditarProfesor(int id)
        {
            try
            {

                Profesor obj = bd.Profesor.Include(u=> u.Usuario).Where(i => i.idProfesor == id).FirstOrDefault();
                ViewBag.idUsuario = new SelectList(bd.Usuario, "idUsuario", "Nombre");
                ViewBag.selectusuario = obj.idUsuario;
                return View(obj);
            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al editar al profesor", msg);
                return View();
            }
        }

        [AuthorizeUser(idNivel: 3)]
        [HttpPost]
        public ActionResult EditarProfesor(Profesor obj)
        {
            try
            {               
                    Profesor existe = bd.Profesor.Where(i => i.idProfesor == obj.idProfesor).FirstOrDefault();

                    existe.idUsuario = obj.idUsuario;
                    existe.Cubo = obj.Cubo;
                    bd.SaveChanges();
                    return RedirectToAction("ConsultaProfesores");

            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al editar al profesor", msg);
                return View();
            }
        }


    
    }
}