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
        public ActionResult Cerrar()
        {
            Session["User"] = null;
            Session["name"] = null;
            Session["profile"] = null;
            return RedirectToAction("Index", "Home");
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
        [HttpGet]
        public ActionResult InsertarUnidad()
        {
            Materia mat = (Materia)Session["UserMateria"];
            ViewBag.Materia = mat.NombreMateria;
            return View();
        }

        [AuthorizeUser(idNivel: 4)]
        [HttpPost]
        public ActionResult InsertarUnidad(Unidad obj)
        {
            try
            {
                if (obj.nombre != null)
                {
                    using (mocOS_BDEntities bd = new mocOS_BDEntities())
                    {
                        Materia mat = (Materia)Session["UserMateria"];
                        ViewBag.Materia = mat.NombreMateria;
                        obj.idMateria = mat.idMateria;
                        bd.Unidad.Add(obj);
                        bd.SaveChanges();
                        return RedirectToAction("MostrarModulos");
                    }
                }
                else
                {
                    Materia mat = (Materia)Session["UserMateria"];
                    ViewBag.Materia = mat.NombreMateria;
                    return View();
                }
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al insertar la Unidad", mensaje);
                return View();
            }
        }


        [AuthorizeUser(idNivel: 4)]
        [HttpGet]
        public ActionResult EditarUnidad(int id)
        {
            try
            {
                Unidad Unimod = new Unidad();
                var Uni = (from d in bd.Unidad
                            where d.idUnidad == id
                            select d).FirstOrDefault();
                Unimod = Uni;
                return View(Unimod);
            }
            catch(Exception msg)
            {
                ModelState.AddModelError("Error al editar a la Alumno", msg);
                return View();
            }
            
        }

        [AuthorizeUser(idNivel: 4)]
        [HttpPost]
        public ActionResult EditarUnidad(Unidad obj)
        {
            try
            {
                if (obj.nombre != null)
                {
                    using (mocOS_BDEntities bd = new mocOS_BDEntities())
                    {
                        Materia mat = (Materia)Session["UserMateria"];
                        ViewBag.Materia = mat.NombreMateria;
                        obj.idMateria = mat.idMateria;
                        Unidad existe = bd.Unidad.Find(obj.idUnidad);
                        existe.nombre = obj.nombre;
                        bd.SaveChanges();
                        return RedirectToAction("MostrarModulos");
                    }
                }
                else
                {
                    Materia mat = (Materia)Session["UserMateria"];
                    ViewBag.Materia = mat.NombreMateria;
                    return View();
                }
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al insertar la Unidad", mensaje);
                return View();
            }
        }

        [AuthorizeUser(idNivel: 4)]
        public ActionResult EliminarUnidad(int id)
        {
            try
            {
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {
                    //Obteniendo los temas
                    List<Temas> DeleteTemas = bd.Temas.Where(x => x.idUnidad == id).ToList();

                    //Obteniendo las clases
                    List<Clase> LasClases = new List<Clase>();
                    foreach (var c in DeleteTemas)
                    {
                        Clase Laclase = bd.Clase.Where(x => x.idTemas == c.idTema).FirstOrDefault();
                        LasClases.Add(Laclase);
                    }

                    //Obteniendo los historiales
                    List<HistorialAsesoria> LosHistoriales = new List<HistorialAsesoria>();
                    foreach (var c in DeleteTemas)
                    {
                        HistorialAsesoria ElHistorial = bd.HistorialAsesoria.Where(x => x.idTema == c.idTema).FirstOrDefault();
                        LosHistoriales.Add(ElHistorial);
                    }

                    //Obteniendo los portafolios
                    List<Portafolio> LosPortafolios = new List<Portafolio>();
                    foreach (var c in LosHistoriales)
                    {
                        Portafolio ElPortafolio = bd.Portafolio.Where(x => x.idHistorial == c.idHistorial).FirstOrDefault();
                        LosPortafolios.Add(ElPortafolio);
                    }

                    //Eliminando todo

                    //Eliminando Documentos con id de clases
                    foreach (var i in LasClases)
                    {
                        if (bd.Documentos.Find(i.idClase) != null)
                        {
                            bd.Documentos.Remove(bd.Documentos.Find(i.idClase));
                            bd.SaveChanges();
                        }
                    }
                    //Eliminando Clases 
                    foreach (var i in LasClases)
                    {
                        if (bd.Clase.Find(i.idClase) != null)
                        {
                            bd.Clase.Remove(bd.Clase.Find(i.idClase));
                            bd.SaveChanges();
                        }
                    }
                    //Eliminado Portafolio
                    foreach (var i in LosPortafolios)
                    {
                        if (bd.Portafolio.Find(i.idPortafolio) != null)
                        {
                            bd.Portafolio.Remove(bd.Portafolio.Find(i.idPortafolio));
                            bd.SaveChanges();
                        }
                    }

                    //Eliminando historiales
                    foreach (var c in LosHistoriales)
                    {
                        if (bd.HistorialAsesoria.Find(c.idHistorial) != null)
                        {
                            bd.HistorialAsesoria.Remove(bd.HistorialAsesoria.Find(c.idHistorial));
                            bd.SaveChanges();
                        }
                    }

                    //Eliminando Temas
                    foreach (var c in DeleteTemas)
                    {
                        if (bd.Temas.Find(c.idTema) != null)
                        {
                            bd.Temas.Remove(bd.Temas.Find(c.idTema));
                            bd.SaveChanges();
                        }
                    }

                    Unidad Launidad = bd.Unidad.Find(id);
                    bd.Unidad.Remove(Launidad);
                    bd.SaveChanges();
                    return RedirectToAction("MostrarModulos");
                }
            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al editar a la Alumno", msg);
                return View();
            }

        }

    }
}
