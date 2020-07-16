using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace MOCos_V1.Controllers
{
    public class AdministradorController : Controller
    {
        mocOS_BDEntities bd = new mocOS_BDEntities();
        // GET: Administrador
        public ActionResult InicioAdmin()
        {
            return View();
        }

        public ActionResult ConsultaAlumno()
        {
            try
            {
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {
                    List<Alumnos> lista = bd.Alumnos.Include(a => a.Cuatrimestre).Include(g => g.Grupo)
                        .Include(u => u.Usuario).ToList();
                    return View(lista);
                }
            }
            catch(Exception msg)
            {
                ModelState.AddModelError("Error al consultar a los alumnos", msg);
                return View();
            }
        }
        [HttpGet]
        public ActionResult InsertarAlumnoAdmin()
        {
                    ViewBag.idUsuario = new SelectList(bd.Usuario, "idUsuario", "Nombre");
                    ViewBag.idCuatrimestre = new SelectList(bd.Cuatrimestre, "idCuatrimestre", "Grado");
                    ViewBag.idGrupo = new SelectList(bd.Grupo, "idGrupo", "Grupo1");
                    return View();
        }
        [HttpPost]
        public ActionResult InsertarAlumnoAdmin(Alumnos obj)
        {
            try
            {
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {
                    bd.Alumnos.Add(obj);
                    bd.SaveChanges();
                    return RedirectToAction("ConsultaAlumno");
                }
            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al insertar a la Alumno", msg);
                return View();
            }
        }
    }
}
