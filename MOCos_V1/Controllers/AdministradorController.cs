using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using MOCos_V1;
using MOCos_V1.Filters;
using Microsoft.Ajax.Utilities;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web.WebPages;

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
        public ActionResult PerfilAdmin()
        {
            return View();
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
        [AuthorizeUser(idNivel: 1)]
        [HttpGet]
        public ActionResult Insertar_Alumno_Administrador()
        {
            var genero = new SelectList(new[] { "Masculino", "Femenino" });
            ViewBag.genero = genero;
            ViewBag.idCuatrimestre = new SelectList(bd.Cuatrimestre, "idCuatrimestre", "Grado");
            ViewBag.idGrupo = new SelectList(bd.Grupo, "idGrupo", "Grupo1");
            return View();

        }

        [AuthorizeUser(idNivel: 1)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Insertar_Alumno_Administrador(Alumnos Alu, string Nombre, string ApellidoP, string ApellidoM, string Correo, string Contrasena, string Direccion, string celular, string genero, string Fecha, string Matricula)
        {
            WebImage iagen;
            HttpPostedFileBase FileBase = Request.Files[0];
            if (FileBase.InputStream.Length != 0)
            {

                iagen = new WebImage(FileBase.InputStream);
            }
            else
            {
                iagen = new WebImage("~/img/perfiles/noregister.png");
            }
            Usuario user = bd.Usuario.Create();

            if (genero == "Masculino")
                user.Genero = "M";
            else
                user.Genero = "F";

            user.Nombre = Nombre;
            user.ApellidoPaterno = ApellidoP;
            user.ApellidoMaterno = ApellidoM;
            user.Contrasena = Contrasena;
            user.Correo = Correo;
            user.Dirección = Direccion;
            user.Telefono = celular;
            user.FechaDeNacimiento = Convert.ToDateTime(Fecha); ;
            user.idTipoUsuario = 3;
            bd.Usuario.Add(user);
            bd.Alumnos.Add(Alu);
            bd.SaveChanges();
            insertar_imagen_alumno(iagen.GetBytes(), Alu, Matricula);

            return RedirectToAction("ConsultaAlumno");


        }

        [AuthorizeUser(idNivel: 1)]
        public void insertar_imagen_alumno(byte[] imagen, Alumnos alu,string Matricula)
        {

            Usuario user = bd.Usuario.OrderByDescending(x => x.idUsuario).First();
            user.FotoPerfil = imagen;
            alu.idUsuario = user.idUsuario;
            alu.Matricula = Matricula.ToUpper();
            bd.SaveChanges();
        }
        [AuthorizeUser(idNivel: 1)]
        [HttpGet]
        public ActionResult Insertar_Profesor_Administrador()
        {
            var genero = new SelectList(new[] { "Masculino", "Femenino" });
            ViewBag.genero = genero;
            ViewBag.idMateriaEnseña = new SelectList(bd.Materia, "idMateria", "NombreMateria");
            return View();

        }

        [AuthorizeUser(idNivel: 1)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Insertar_Profesor_Administrador(Profesor pro, string Nombre, string ApellidoP, string ApellidoM, string Correo, string Contrasena, string Direccion, string celular, string genero, string Fecha,string Cubo)
        {
            WebImage iagen;
            HttpPostedFileBase FileBase = Request.Files[0];
            if (FileBase.InputStream.Length != 0)
            {

                iagen = new WebImage(FileBase.InputStream);
            }
            else
            {
                iagen = new WebImage("~/img/perfiles/noregister.png");
            }
            Usuario user = bd.Usuario.Create();

            if (genero == "Masculino")
                user.Genero = "M";
            else
                user.Genero = "F";

            user.Nombre = Nombre;
            user.ApellidoPaterno = ApellidoP;
            user.ApellidoMaterno = ApellidoM;
            user.Contrasena = Contrasena;
            user.Correo = Correo;
            user.Dirección = Direccion;
            user.Telefono = celular;
            user.FechaDeNacimiento = Convert.ToDateTime(Fecha); ;
            user.idTipoUsuario = 2;
            bd.Usuario.Add(user);
            bd.Profesor.Add(pro);
            bd.SaveChanges();
            insertar_imagen_profesor(iagen.GetBytes(),pro,Cubo);

            return RedirectToAction("InicioAdmin");


        }
        [AuthorizeUser(idNivel: 1)]
        public void insertar_imagen_profesor(byte[] imagen, Profesor pro,string Cubo)
        {

            Usuario user = bd.Usuario.OrderByDescending(x => x.idUsuario).First();
            user.FotoPerfil = imagen;
            pro.idUsuario = user.idUsuario;
            pro.Cubo = Cubo;
            bd.SaveChanges();

        }
        [AuthorizeUser(idNivel: 1)]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bd.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
