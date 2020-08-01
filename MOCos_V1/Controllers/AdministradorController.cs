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
    public class AdministradorController : Controller
    {
        mocOS_BDEntities bd = new mocOS_BDEntities();
        // GET: Administrador
        [AuthorizeUser(idNivel: 1)]
        public ActionResult InicioAdmin()
        {

            return View();
        }
        [AuthorizeUser(idNivel: 1)]
        public ActionResult Cerrar()
        {
            Session["User"] = null;
            Session["name"] = null;
            Session["profile"] = null;
            return RedirectToAction("Index", "Home");
        }
        [AuthorizeUser(idNivel: 1)]
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
        [AuthorizeUser(idNivel: 1)]
        [HttpGet]
        public ActionResult InsertarAlumnoAdmin()
        {
                    ViewBag.idUsuario = new SelectList(bd.Usuario, "idUsuario", "Nombre");
                    ViewBag.idCuatrimestre = new SelectList(bd.Cuatrimestre, "idCuatrimestre", "Grado");
                    ViewBag.idGrupo = new SelectList(bd.Grupo, "idGrupo", "Grupo1");
                    return View();
        }
        [AuthorizeUser(idNivel: 1)]
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
        [AuthorizeUser(idNivel: 1)]
        [HttpGet]
        public ActionResult EditarAlumnoAdmin(int id)
        {
            try
            {

                Alumnos obj = bd.Alumnos.Where(i => i.idAlumno == id).FirstOrDefault();
                ViewBag.idUsuario = new SelectList(bd.Usuario, "idUsuario", "Nombre");
                ViewBag.selectusuario = obj.idUsuario;
                ViewBag.idCuatrimestre = new SelectList(bd.Cuatrimestre, "idCuatrimestre", "Grado");
                ViewBag.selectcuatrimestre = obj.idCuatrimestre;
                ViewBag.idGrupo = new SelectList(bd.Grupo, "idGrupo", "Grupo1");
                ViewBag.selectgrupo = obj.idGrupo;
                return View(obj);
                
            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al editar a la Alumno", msg);
                return View();
            }
        }

        [AuthorizeUser(idNivel: 1)]
        [HttpPost]
        public ActionResult EditarAlumnoAdmin(Alumnos obj)
        {
            try
            {
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {
                    Alumnos existe = bd.Alumnos.Where(i => i.idAlumno == obj.idAlumno).FirstOrDefault();
                    //mi error xddddddd
                    existe.idCuatrimestre = obj.idCuatrimestre;
                    existe.idGrupo = obj.idGrupo;

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
