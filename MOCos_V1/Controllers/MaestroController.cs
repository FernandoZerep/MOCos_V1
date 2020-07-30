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
        mocOS_BDEntities bd = new mocOS_BDEntities();
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
        public ActionResult MostrarModulos()
        {
            try
            {
                
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {
                    Usuario obj = new Usuario();
                    obj = (Usuario)Session["User"];
                    var prof = (from d in bd.Profesor
                                 where d.idUsuario == obj.idUsuario
                                 select d).FirstOrDefault();
                    var mat = (from d in bd.Materia
                                   where d.idCoordinador == prof.idProfesor
                                   select d).FirstOrDefault();
                    Session["UserMateria"] = mat;
                    ViewBag.Materia = mat.NombreMateria;
                    List<Unidad> Uni = bd.Unidad.Where(x => x.idMateria == mat.idMateria).ToList();
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
                    obj.idMateria = (int)Session["UserMateria"];
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
