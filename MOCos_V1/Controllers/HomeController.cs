using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MOCos_V1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
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

    }
}