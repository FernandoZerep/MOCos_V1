using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MOCos_V1;
using System.Data.Entity;

namespace MOCos_V1.Controllers
{
    public class MaestroController : Controller
    {
        public ActionResult Perfil()
        {
            ViewBag.Message = "Your perfil.";

            return View();
        }

        public ActionResult Modulos()
        {
            ViewBag.Message = "Your Topic.";

            return View();
        }

        [HttpGet]
        public ActionResult Insertar_Unidad()
        {
            return View();
        }

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
