using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace MOCos_V1.Controllers
{
    public class AlumnoController : Controller
    {
        public ActionResult Inicio()
        {
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

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult pruebaregis()
        {
            return View();
        }

        public ActionResult ConsultaProfesores()
        {
            try
            {
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {
                    List<Profesor> lista = bd.Profesor.Include(u => u.Usuario).ToList();
                    return View(lista);
                }
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
    }
}