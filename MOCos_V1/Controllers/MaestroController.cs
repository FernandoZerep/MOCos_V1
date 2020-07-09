using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult Temas()
        {
            ViewBag.Message = "Your Materias.";

            return View();
        }
    }
}
