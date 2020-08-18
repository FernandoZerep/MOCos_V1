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
using System.Web.WebPages;
using System.Net;
using System.Net.Mail;

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
        public ActionResult ContactoAdmin()
        {

            return View();
        }
        [AuthorizeUser(idNivel: 1)]
        public ActionResult MapaAdmin()
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
        [HttpGet]
        public ActionResult Contacto()
        {
            return View();
        }
        [AuthorizeUser(idNivel: 1)]
        [HttpPost]
        public ActionResult Contacto(string Nombre, string Asunto, string Correo, string Mensaje)
        {
            try
            {
                string co = "pruebaplicacion5@gmail.com", rec = "utp0000288@alumno.utpuebla.edu.mx", ms = "";

                MailMessage correo = new MailMessage();
                ms += "Nombre: " + Nombre;
                ms += "\n Correo: " + Correo; ;
                ms += "\n Mensaje: " + Mensaje;
                correo.From = new MailAddress(co);
                correo.To.Add(rec);
                correo.Subject = Asunto;
                correo.Body = ms;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;
                //configuracion del servidor stmp

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                string sCuentaCorreo = co;
                string SPasswordCorreo = "Prueba1412";
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(sCuentaCorreo, SPasswordCorreo);
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(correo);
                ViewBag.Mensaje = "Mensaje enviado correctamente";




            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }


        //EDITPROFILE
        [AuthorizeUser(idNivel: 1)]
        [HttpGet]
        public ActionResult EditarAdminAdmin(int id)
        {
            try
            {

                Administrador obj = bd.Administrador.Include(u => u.Usuario).Include(t => t.Usuario.TiposUsuarios).Where(i => i.idUsuario == id).FirstOrDefault();
                //el primer userlist no es necesario pero que se quede mientras
                obj.UserList = new SelectList(bd.Usuario, "idUsuario", "Nombre");
                obj.Usuario.UserTipoList = new SelectList(bd.TiposUsuarios, "idTipo", "Nombre");
                var gene = new[] {
                                  new Person { Id = "F", Name = "Femenino" },
                                  new Person { Id = "M", Name = "Masculino" }
                                  };
                obj.Usuario.UserGenList = new SelectList(gene, "Id", "Name");
                //ViewBag.idUsuario = new SelectList(bd.Usuario, "idUsuario", "Nombre");
                //ViewBag.selectusuario = obj.idUsuario;
                //ViewBag.idCuatrimestre = new SelectList(bd.Cuatrimestre, "idCuatrimestre", "Grado");
                //ViewBag.selectcuatrimestre = obj.idCuatrimestre;
                //ViewBag.idGrupo = new SelectList(bd.Grupo, "idGrupo", "Grupo1");
                //ViewBag.selectgrupo = obj.idGrupo;
                return View(obj);

            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al editar a la Alumno", msg);
                return View();
            }
        }

        public class Person
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        //EDITPROFILE
        [AuthorizeUser(idNivel: 1)]
        [HttpGet]
        public ActionResult EditarOnlyUser(int id)
        {
            try
            {

                Usuario obj = bd.Usuario.Include(u => u.Administrador).Include(t => t.TiposUsuarios).Where(i => i.idUsuario == id).FirstOrDefault();
                obj.UserTipoList = new SelectList(bd.TiposUsuarios, "idTipo", "Nombre");
                var gene = new[] {
                                  new Person { Id = "F", Name = "Femenino" },
                                  new Person { Id = "M", Name = "Masculino" }
                                  };
                obj.UserGenList = new SelectList(gene, "Id", "Name");
                //ViewBag.idUsuario = new SelectList(bd.Usuario, "idUsuario", "Nombre");
                //ViewBag.selectusuario = obj.idUsuario;
                //ViewBag.idCuatrimestre = new SelectList(bd.Cuatrimestre, "idCuatrimestre", "Grado");
                //ViewBag.selectcuatrimestre = obj.idCuatrimestre;
                //ViewBag.idGrupo = new SelectList(bd.Grupo, "idGrupo", "Grupo1");
                //ViewBag.selectgrupo = obj.idGrupo;
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
        public ActionResult EditarAdminAdmin(Administrador obj)
        {
            try
            {
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {
                    Administrador existe = bd.Administrador.Include(u => u.Usuario).Include(t => t.Usuario.TiposUsuarios).Where(x => x.idUsuario == obj.idUsuario).SingleOrDefault();
                    WebImage iagen;
                    HttpPostedFileBase FileBase = Request.Files[0];

                    Usuario LaSesion = (Usuario)Session["User"];
                    existe.Usuario.Nombre = obj.Usuario.Nombre;
                    //if (obj.Usuario.Contrasena == "")
                    //    return View(obj);
                    //else
                    existe.Usuario.Contrasena = obj.Usuario.Contrasena;
                    existe.Usuario.ApellidoMaterno = obj.Usuario.ApellidoMaterno;
                    existe.Usuario.ApellidoPaterno = obj.Usuario.ApellidoPaterno;
                    existe.Usuario.Dirección = obj.Usuario.Dirección;
                    existe.Usuario.Telefono = obj.Usuario.Telefono;
                    existe.Usuario.Genero = obj.Usuario.Genero;
                    existe.Usuario.FechaDeNacimiento = obj.Usuario.FechaDeNacimiento;
                    existe.Usuario.Correo = obj.Usuario.Correo;
                    existe.Tipo = obj.Tipo;

                    bd.SaveChanges();
                    if (FileBase.InputStream.Length != 0)
                    {

                        iagen = new WebImage(FileBase.InputStream);
                        insertar_imagen_admin(iagen.GetBytes(), existe);
                    }

                    if (LaSesion.Correo != obj.Usuario.Correo)
                    {
                        Session["User"] = null;
                        Session["name"] = null;
                        Session["profile"] = null;
                        return RedirectToAction("Login", "Home");
                    }
                    else
                    {
                        Administrador nuevo = bd.Administrador.Include(a => a.Usuario).Include(b => b.Usuario.TiposUsuarios).Where(x => x.idUsuario == obj.idUsuario).FirstOrDefault();
                        Session["User"] = nuevo.Usuario;
                        Session["name"] = nuevo.Usuario.Nombre + " " + nuevo.Usuario.ApellidoPaterno + " " + nuevo.Usuario.ApellidoMaterno;
                        Session["profile"] = nuevo.Usuario.FotoPerfil;
                        Session["Activo"] = nuevo;
                        return RedirectToAction("InicioAdmin");
                    }

                }
            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al insertar a la Alumno", msg);
                obj.UserList = new SelectList(bd.Usuario, "idUsuario", "Nombre");
                obj.Usuario.UserTipoList = new SelectList(bd.TiposUsuarios, "idTipo", "Nombre");
                var gene = new[] {
                                  new Person { Id = "F", Name = "Femenino" },
                                  new Person { Id = "M", Name = "Masculino" }
                                  };
                obj.Usuario.UserGenList = new SelectList(gene, "Id", "Name");
                return View(obj);
            }
        }
        [AuthorizeUser(idNivel: 1)]
        public void insertar_imagen_admin(byte[] imagen, Administrador admi)
        {

            Usuario user = bd.Usuario.Where(x => x.idUsuario == admi.idUsuario).FirstOrDefault();
            user.FotoPerfil = imagen;
            bd.SaveChanges();
        }
        //fin edit profile


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

            return RedirectToAction("ConsultaDocente");


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
        //CRUD PARA PROFESORES
        [AuthorizeUser(idNivel: 1)]
        public ActionResult ConsultaDocente()
        {
            try
            {
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {
                    List<Profesor> lista = bd.Profesor.Include(u => u.Usuario).Include(m => m.Materia1).ToList();
                    return View(lista);
                }
            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al consultar a los alumnos", msg);
                return View();
            }
        }
        //CRUD PARA MATERIAS
        [AuthorizeUser(idNivel: 1)]
        public ActionResult ConsultaMaterias ()
        {
            try
            {
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {
                    List<Materia> lista = bd.Materia.Include(c => c.Cuatrimestre).Include(o => o.Profesor).Include(u=>u.Profesor.Usuario).ToList();
                    return View(lista);
                }
            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al consultar a los alumnos", msg);
                return View();
            }
        }


        //EDITPROFILE
        [AuthorizeUser(idNivel: 1)]
        [HttpGet]
        public ActionResult EditarAlumnoPerfil(int id)
        {
            try
            {

                Alumnos obj = bd.Alumnos.Include(u => u.Usuario).Include(t => t.Usuario.TiposUsuarios).Include(c => c.Cuatrimestre).Include(g => g.Grupo).Where(i => i.idUsuario == id).FirstOrDefault();
                //el primer userlist no es necesario pero que se quede mientras
                obj.UserList = new SelectList(bd.Usuario, "idUsuario", "Nombre");

                var Cuatrimestre = bd.Cuatrimestre.AsEnumerable().Select(s => new
                {
                    idCuatrimestre = s.idCuatrimestre,
                    Nombre = $"{s.Grado} {s.Especialidad} {s.Carrera}"
                });
                obj.UserCuatri = new SelectList(Cuatrimestre, "idCuatrimestre", "Nombre");
                obj.UserGrupo = new SelectList(bd.Grupo, "idGrupo", "Grupo1");
                obj.Usuario.UserTipoList = new SelectList(bd.TiposUsuarios, "idTipo", "Nombre");
                var gene = new[] {
                                  new Person { Id = "F", Name = "Femenino" },
                                  new Person { Id = "M", Name = "Masculino" }
                                  };
                obj.Usuario.UserGenList = new SelectList(gene, "Id", "Name");
                //ViewBag.idUsuario = new SelectList(bd.Usuario, "idUsuario", "Nombre");
                //ViewBag.selectusuario = obj.idUsuario;
                //ViewBag.idCuatrimestre = new SelectList(bd.Cuatrimestre, "idCuatrimestre", "Grado");
                //ViewBag.selectcuatrimestre = obj.idCuatrimestre;
                //ViewBag.idGrupo = new SelectList(bd.Grupo, "idGrupo", "Grupo1");
                //ViewBag.selectgrupo = obj.idGrupo;
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
        public ActionResult EditarAlumnoPerfil(Alumnos obj)
        {
            try
            {
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {
                    Alumnos existe = bd.Alumnos.Include(u => u.Usuario).Include(t => t.Usuario.TiposUsuarios).Include(c => c.Cuatrimestre).Include(g => g.Grupo).Where(i => i.idUsuario == obj.idUsuario).FirstOrDefault();
                    WebImage iagen;
                    HttpPostedFileBase FileBase = Request.Files[0];

                    Usuario LaSesion = (Usuario)Session["User"];
                    existe.Usuario.Nombre = obj.Usuario.Nombre;
                    //if (obj.Usuario.Contrasena == "")
                    //    return View(obj);
                    //else
                    existe.Usuario.Contrasena = obj.Usuario.Contrasena;
                    existe.Usuario.ApellidoMaterno = obj.Usuario.ApellidoMaterno;
                    existe.Usuario.ApellidoPaterno = obj.Usuario.ApellidoPaterno;
                    existe.Usuario.Dirección = obj.Usuario.Dirección;
                    existe.Usuario.Telefono = obj.Usuario.Telefono;
                    existe.Usuario.Genero = obj.Usuario.Genero;
                    existe.Usuario.FechaDeNacimiento = obj.Usuario.FechaDeNacimiento;
                    existe.Usuario.Correo = obj.Usuario.Correo;
                    existe.Matricula = obj.Matricula;
                    existe.idCuatrimestre = obj.idCuatrimestre;
                    existe.idGrupo = obj.idGrupo;

                    bd.SaveChanges();
                    if (FileBase.InputStream.Length != 0)
                    {

                        iagen = new WebImage(FileBase.InputStream);
                        insertar_imagen_perfil(iagen.GetBytes(), existe);
                    }

                    return RedirectToAction("ConsultaAlumno");

                }
            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al insertar a la Alumno", msg);
                //el primer userlist no es necesario pero que se quede mientras
                obj.UserList = new SelectList(bd.Usuario, "idUsuario", "Nombre");

                var Cuatrimestre = bd.Cuatrimestre.AsEnumerable().Select(s => new
                {
                    idCuatrimestre = s.idCuatrimestre,
                    Nombre = $"{s.Grado} {s.Especialidad} {s.Carrera}"
                });
                obj.UserCuatri = new SelectList(Cuatrimestre, "idCuatrimestre", "Nombre");
                obj.UserGrupo = new SelectList(bd.Grupo, "idGrupo", "Grupo1");
                obj.Usuario.UserTipoList = new SelectList(bd.TiposUsuarios, "idTipo", "Nombre");
                var gene = new[] {
                                  new Person { Id = "F", Name = "Femenino" },
                                  new Person { Id = "M", Name = "Masculino" }
                                  };
                obj.Usuario.UserGenList = new SelectList(gene, "Id", "Name");
                return View(obj);
            }
        }
        [AuthorizeUser(idNivel: 3)]
        public void insertar_imagen_perfil(byte[] imagen, Alumnos admi)
        {

            Usuario user = bd.Usuario.Where(x => x.idUsuario == admi.idUsuario).FirstOrDefault();
            user.FotoPerfil = imagen;
            bd.SaveChanges();
        }
        //fin edit profile
        //EDITPROFILE
        [AuthorizeUser(idNivel: 1)]
        [HttpGet]
        public ActionResult EditarDocentePerfil(int id)
        {
            try
            {

                Profesor obj = bd.Profesor.Include(u => u.Usuario).Include(t => t.Usuario.TiposUsuarios).Include(m => m.Materia1).Where(x => x.idUsuario == id).FirstOrDefault();
                //el primer userlist no es necesario pero que se quede mientras
                obj.UserEnseña = new SelectList(bd.Materia, "idMateria", "NombreMateria");
                obj.Usuario.UserTipoList = new SelectList(bd.TiposUsuarios, "idTipo", "Nombre");
                var gene = new[] {
                                  new Person { Id = "F", Name = "Femenino" },
                                  new Person { Id = "M", Name = "Masculino" }
                                  };
                obj.Usuario.UserGenList = new SelectList(gene, "Id", "Name");
                //ViewBag.idUsuario = new SelectList(bd.Usuario, "idUsuario", "Nombre");
                //ViewBag.selectusuario = obj.idUsuario;
                //ViewBag.idCuatrimestre = new SelectList(bd.Cuatrimestre, "idCuatrimestre", "Grado");
                //ViewBag.selectcuatrimestre = obj.idCuatrimestre;
                //ViewBag.idGrupo = new SelectList(bd.Grupo, "idGrupo", "Grupo1");
                //ViewBag.selectgrupo = obj.idGrupo;
                return View(obj);

            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al editar al docente", msg);
                return View();
            }
        }

        [AuthorizeUser(idNivel: 1)]
        [HttpPost]
        public ActionResult EditarDocentePerfil(Profesor obj)
        {
            try
            {
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {
                    Profesor existe = bd.Profesor.Include(u => u.Usuario).Include(t => t.Usuario.TiposUsuarios).Include(m => m.Materia1).Where(x => x.idUsuario == obj.idUsuario).FirstOrDefault();
                    WebImage iagen;
                    HttpPostedFileBase FileBase = Request.Files[0];

                    Usuario LaSesion = (Usuario)Session["User"];
                    existe.Usuario.Nombre = obj.Usuario.Nombre;
                    //if (obj.Usuario.Contrasena == "")
                    //    return View(obj);
                    //else
                    existe.Usuario.Contrasena = obj.Usuario.Contrasena;
                    existe.Usuario.ApellidoMaterno = obj.Usuario.ApellidoMaterno;
                    existe.Usuario.ApellidoPaterno = obj.Usuario.ApellidoPaterno;
                    existe.Usuario.Dirección = obj.Usuario.Dirección;
                    existe.Usuario.Telefono = obj.Usuario.Telefono;
                    existe.Usuario.Genero = obj.Usuario.Genero;
                    existe.Usuario.FechaDeNacimiento = obj.Usuario.FechaDeNacimiento;
                    existe.Usuario.Correo = obj.Usuario.Correo;
                    existe.idMateriaEnseña = obj.idMateriaEnseña;
                    existe.Cubo = obj.Cubo;

                    bd.SaveChanges();
                    if (FileBase.InputStream.Length != 0)
                    {

                        iagen = new WebImage(FileBase.InputStream);
                        insertar_imagen_perfil(iagen.GetBytes(), existe);
                    }


                    return RedirectToAction("ConsultaDocente");

                }
            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al insertar a la Alumno", msg);
                //el primer userlist no es necesario pero que se quede mientras
                obj.UserEnseña = new SelectList(bd.Materia, "idMateria", "NombreMateria");
                obj.Usuario.UserTipoList = new SelectList(bd.TiposUsuarios, "idTipo", "Nombre");
                var gene = new[] {
                                  new Person { Id = "F", Name = "Femenino" },
                                  new Person { Id = "M", Name = "Masculino" }
                                  };
                obj.Usuario.UserGenList = new SelectList(gene, "Id", "Name");
                return View(obj);
            }
        }
        [AuthorizeUser(idNivel: 1)]
        public void insertar_imagen_perfil(byte[] imagen, Profesor admi)
        {

            Usuario user = bd.Usuario.Where(x => x.idUsuario == admi.idUsuario).FirstOrDefault();
            user.FotoPerfil = imagen;
            bd.SaveChanges();
        }
        //fin edit profile



        //Eliminar perfil alumno
        [AuthorizeUser(idNivel: 1)]
        public ActionResult EliminarAlumno(int id)
        {
            int idalu = bd.Alumnos.Where(x => x.idUsuario == id).Select(X => X.idAlumno).SingleOrDefault();
            var historial = bd.HistorialAsesoria.Where(x => x.idAlumno == idalu);
            var alumno = bd.Alumnos.Where(x => x.idUsuario == id).FirstOrDefault();
            var usuario = bd.Usuario.Where(X => X.idUsuario == id).FirstOrDefault();

            bd.HistorialAsesoria.RemoveRange(historial);
            bd.Alumnos.Remove(alumno);
            bd.Usuario.Remove(usuario);
            bd.SaveChanges();
            return RedirectToAction("ConsultaAlumno");
        }
        //Fin de eliminar perfil alumno
        //Eliminar perfil docente
        [AuthorizeUser(idNivel: 1)]
        public ActionResult EliminarDocente(int id)
        {
            int idprof = bd.Profesor.Where(x => x.idUsuario == id).Select(X => X.idProfesor).SingleOrDefault();
            Materia existe = bd.Materia.Where(x => x.idCoordinador == idprof).FirstOrDefault();
            if (existe!=null)
            {
                existe.idCoordinador = null;
            }
            var historial = bd.HistorialAsesoria.Where(x => x.idProfesor == idprof);
            var profe = bd.Profesor.Where(x => x.idUsuario == id).FirstOrDefault();
            var usuario = bd.Usuario.Where(X => X.idUsuario == id).FirstOrDefault();

            bd.HistorialAsesoria.RemoveRange(historial);
            bd.Profesor.Remove(profe);
            bd.Usuario.Remove(usuario);
            bd.SaveChanges();
            return RedirectToAction("ConsultaDocente");
        }
        //Fin de eliminar perfil alumno
    }
}
