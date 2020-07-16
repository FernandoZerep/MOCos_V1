using MOCos_V1.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MOCos_V1.Controllers
{
    
    public class AdministradorController : Controller
    {
        [AuthorizeUser(idNivel: 1)]
        // GET: Administrador
        public ActionResult InicioAdmin()
        {
            return View();
        }
        [AuthorizeUser(idNivel: 1)]
        public ActionResult AgregarAlumno()
        {
            return View();
        }
    }
}
