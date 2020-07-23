using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MOCos_V1;
using System.Data.Entity;
using MOCos_V1.Filters;

namespace MOCos_V1.Controllers
{
    public class MaestroController : Controller
    {
        [AuthorizeUser(idNivel: 4)]
        public ActionResult Perfil()
        {
            ViewBag.Message = "Your perfil.";

            return View();
        }

        [AuthorizeUser(idNivel: 4)]
        public ActionResult Modulos()
        {
            ViewBag.Message = "Your Topic.";

            return View();
        }

        [AuthorizeUser(idNivel: 4)]
        [HttpPost]
        public ActionResult MostrarModulos()
        {
            try
            {
                Usuario obj = new Usuario();
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {
                    obj = (Usuario)Session["User"];
                    var idProf = (from d in bd.Profesor
                                 where d.idUsuario == obj.idUsuario
                                 select d).FirstOrDefault();
                    var Materia = (from d in bd.Materia
                                   where d.idCoordinador == idProf.idProfesor
                                   select d).FirstOrDefault();
                    Session["UserMateria"] = Materia;
                    List<Unidad> Uni = bd.Unidad.Include(n => n.nombre).ToList();
                    return View(Uni);
                }
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al mostrar la Unidad", mensaje);
                return View();
            }
        }

        [AuthorizeUser(idNivel: 4)]
        [HttpPost]
        public ActionResult Insertar_Unidad(Unidad obj)
        {
            try
            {
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {
                    obj.idMateria = 5;
                    bd.Unidad.Add(obj);
                    bd.SaveChanges();
                    return RedirectToAction("Modulos");
                }
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al insertar la Unidad", mensaje);
                return View();
            }
        }

        public ActionResult InsertarUnidad()
        {
            ViewBag.Message = "New Unit";

            return View();
        }
    }
}
