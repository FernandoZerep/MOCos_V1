using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MOCos_V1;
using System.Data.Entity;
using MOCos_V1.Filters;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.Helpers;

namespace MOCos_V1.Controllers
{
    public class MaestroController : Controller
    {
        mocOS_BDEntities bd = new mocOS_BDEntities();

        [AuthorizeUser(idNivel: 4)]
        public ActionResult Inicio()
        {
            return View();
        }
        [AuthorizeUser(idNivel: 4)]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [AuthorizeUser(idNivel: 4)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your Contact page";
            return View();
        }
        [AuthorizeUser(idNivel: 4)]
        [HttpGet]
        public ActionResult Contacto()
        {
            return View();
        }
        [AuthorizeUser(idNivel: 4)]
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
                    //Obteniendo los temas
                    List<Temas> MostrarTemas = new List<Temas>();
                    foreach (var c in Uni)
                    {
                        List<Temas> Encontrados = bd.Temas.Where(x => x.idUnidad == c.idUnidad).ToList();
                        foreach (var i in Encontrados)
                        {
                            MostrarTemas.Add(i);
                        }
                    }
                    ViewBag.Temas = MostrarTemas;
                    return View(Uni);
                }
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al mostrar las Unidades", mensaje);
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
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al editar la Unidad", msg);
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
                ModelState.AddModelError("Error al editar la Unidad", mensaje);
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

                    //Obteniendo los Documentos
                    List<Documentos> LosDocumentos = new List<Documentos>();
                    foreach (var c in DeleteTemas)
                    {
                        Documentos ElDocumento = bd.Documentos.Where(x => x.idTema == c.idTema).FirstOrDefault();
                        LosDocumentos.Add(ElDocumento);
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
                    foreach (var i in LosDocumentos)
                    {
                        if (bd.Documentos.Find(i.idDocumento) != null)
                        {
                            bd.Documentos.Remove(bd.Documentos.Find(i.idDocumento));
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
                ModelState.AddModelError("Error al borrar la Unidad", msg);
                return View();
            }

        }


        [AuthorizeUser(idNivel: 4)]
        [HttpGet]
        public ActionResult InsertarTema(int id)
        {
            try
            {
                if (id > 0)
                {
                    using (mocOS_BDEntities bd = new mocOS_BDEntities())
                    {
                        Session["LaUnidad"] = bd.Unidad.Find(id);
                        Unidad Obtenido = (Unidad)Session["LaUnidad"];
                        ViewBag.LaUnidad = Obtenido.nombre;
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("MostrarModulos");
                }
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al insertar el Tema", mensaje);
                return RedirectToAction("MostrarModulos");
            }

        }

        [AuthorizeUser(idNivel: 4)]
        [HttpPost]
        public ActionResult InsertarTema(Temas obj)
        {
            try
            {
                if (obj.Nombre != null)
                {
                    using (mocOS_BDEntities bd = new mocOS_BDEntities())
                    {
                        Unidad Obtenido = (Unidad)Session["LaUnidad"];
                        ViewBag.LaUnidad = Obtenido.nombre;
                        obj.idUnidad = Obtenido.idUnidad;
                        bd.Temas.Add(obj);
                        bd.SaveChanges();
                        return RedirectToAction("MostrarModulos");
                    }
                }
                else
                {
                    Unidad Obtenido = (Unidad)Session["LaUnidad"];
                    ViewBag.LaUnidad = Obtenido.nombre;
                    return View();
                }
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al insertar el Tema", mensaje);
                return RedirectToAction("MostrarModulos");
            }
        }

        [AuthorizeUser(idNivel: 4)]
        [HttpGet]
        public ActionResult EditarTema(int id)
        {
            try
            {
                var LaUni = (from d in bd.Temas
                             where d.idTema == id
                             select d.idUnidad).FirstOrDefault();
                Session["LaUnidad"] = bd.Unidad.Find(LaUni);
                Unidad Obtenido = (Unidad)Session["LaUnidad"];
                ViewBag.LaUnidad = Obtenido.nombre;
                Temas Temamod = new Temas();
                var Eltema = (from d in bd.Temas
                              where d.idTema == id
                              select d).FirstOrDefault();
                Temamod = Eltema;
                return View(Temamod);
            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al editar el Tema", msg);
                return View();
            }

        }

        [AuthorizeUser(idNivel: 4)]
        [HttpPost]
        public ActionResult EditarTema(Temas obj)
        {
            try
            {
                if (obj.Nombre != null)
                {
                    using (mocOS_BDEntities bd = new mocOS_BDEntities())
                    {
                        Unidad Obtenido = (Unidad)Session["LaUnidad"];
                        ViewBag.LaUnidad = Obtenido.nombre;
                        obj.idUnidad = Obtenido.idUnidad;
                        Temas existe = bd.Temas.Find(obj.idTema);
                        existe.Nombre = obj.Nombre;
                        bd.SaveChanges();
                        return RedirectToAction("MostrarModulos");
                    }
                }
                else
                {
                    Unidad Obtenido = (Unidad)Session["LaUnidad"];
                    ViewBag.LaUnidad = Obtenido.nombre;
                    return View();
                }
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al editar el tema", mensaje);
                return View();
            }
        }

        [AuthorizeUser(idNivel: 4)]
        public ActionResult EliminarTema(int id)
        {
            try
            {
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {

                    //Obteniendo los documentos
                    List<Documentos> LosDocumentos = bd.Documentos.Where(x => x.idTema == id).ToList();

                    //Obteniendo los historiales
                    List<HistorialAsesoria> LosHistoriales = bd.HistorialAsesoria.Where(x => x.idTema == id).ToList();

                    //Obteniendo los portafolios
                    List<Portafolio> LosPortafolios = new List<Portafolio>();
                    foreach (var c in LosHistoriales)
                    {
                        Portafolio ElPortafolio = bd.Portafolio.Where(x => x.idHistorial == c.idHistorial).FirstOrDefault();
                        LosPortafolios.Add(ElPortafolio);
                    }

                    //Eliminando todo

                    //Eliminando Documentos 
                    foreach (var i in LosDocumentos)
                    {
                        if (bd.Documentos.Find(i.idDocumento) != null)
                        {
                            bd.Documentos.Remove(bd.Documentos.Find(i.idDocumento));
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

                    //Eliminando Tema

                    Temas ElTema = bd.Temas.Find(id);
                    bd.Temas.Remove(ElTema);
                    bd.SaveChanges();
                    return RedirectToAction("MostrarModulos");
                }
            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al borrar el tema", msg);
                return View();
            }

        }


        //EDITPROFILE
        [AuthorizeUser(idNivel: 4)]
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

        public class Person
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        [AuthorizeUser(idNivel: 4)]
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
                    existe.Cubo = obj.Cubo;

                    bd.SaveChanges();
                    if (FileBase.InputStream.Length != 0)
                    {

                        iagen = new WebImage(FileBase.InputStream);
                        insertar_imagen_perfil(iagen.GetBytes(), existe);
                    }

                    if (LaSesion.Correo != obj.Usuario.Correo)
                    {
                        Session["User"] = null;
                        Session["name"] = null;
                        Session["profile"] = null;
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        Profesor nuevo = bd.Profesor.Include(u => u.Usuario).Include(t => t.Usuario.TiposUsuarios).Include(m => m.Materia1).Where(x => x.idUsuario == obj.idUsuario).FirstOrDefault();
                        Session["User"] = nuevo.Usuario;
                        Session["name"] = nuevo.Usuario.Nombre + " " + nuevo.Usuario.ApellidoPaterno + " " + nuevo.Usuario.ApellidoMaterno;
                        Session["profile"] = nuevo.Usuario.FotoPerfil;
                        Session["Activo"] = nuevo;
                        return RedirectToAction("Inicio");
                    }

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

        [AuthorizeUser(idNivel: 3)]
        public void insertar_imagen_perfil(byte[] imagen, Profesor admi)
        {

            Usuario user = bd.Usuario.Where(x => x.idUsuario == admi.idUsuario).FirstOrDefault();
            user.FotoPerfil = imagen;
            bd.SaveChanges();
        }

        //

        [AuthorizeUser(idNivel: 4)]
        [HttpGet]
        public ActionResult InsertarDoc(int id)
        {
            try
            {
                if (id > 0)
                {
                    using (mocOS_BDEntities bd = new mocOS_BDEntities())
                    {
                        Session["ElTema"] = bd.Temas.Find(id);
                        Temas Obtenido = (Temas)Session["ElTema"];
                        ViewBag.Tema = Obtenido.Nombre;
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("MostrarDocTemas", id);
                }
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al insertar el Tema", mensaje);
                return RedirectToAction("MostrarModulos");
            }

        }

        [AuthorizeUser(idNivel: 4)]
        [HttpPost]
        public ActionResult InsertarDoc(Documentos ElDoc)
        {
            try
            {
                if (ElDoc.Nombre != null && ElDoc.Link != null)
                {
                    using (mocOS_BDEntities bd = new mocOS_BDEntities())
                    {
                        Temas Obtenido = (Temas)Session["ElTema"];
                        ViewBag.Tema = Obtenido.Nombre;
                        ElDoc.idTema = Obtenido.idTema;
                        bd.Documentos.Add(ElDoc);
                        bd.SaveChanges();
                        return RedirectToAction("MostrarModulos");
                    }
                }
                else
                {
                    Temas Obtenido = (Temas)Session["ElTema"];
                    ViewBag.Tema = Obtenido.Nombre;
                    return View();
                }
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al insertar el Tema", mensaje);
                return RedirectToAction("MostrarModulos");
            }
        }

        [AuthorizeUser(idNivel: 4)]
        [HttpGet]
        public ActionResult EditarDoc(int id)
        {
            try
            {
                Documentos Modificar = bd.Documentos.Find(id);
                Session["ElTema"] = bd.Temas.Find(Modificar.idTema);
                Temas Obtenido = (Temas)Session["ElTema"];
                ViewBag.Tema =Obtenido.Nombre;
                return View(Modificar);
            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al editar el Tema", msg);
                return View();
            }

        }

        [AuthorizeUser(idNivel: 4)]
        [HttpPost]
        public ActionResult EditarDoc(Documentos ElDoc)
        {
            try
            {
                if (ElDoc.Nombre != null && ElDoc.Link != null)
                {
                    using (mocOS_BDEntities bd = new mocOS_BDEntities())
                    {
                        Temas Obtenido = (Temas)Session["ElTema"];
                        ViewBag.Tema = Obtenido.Nombre;
                        ElDoc.idTema = Obtenido.idTema;
                        Documentos existe = bd.Documentos.Find(ElDoc.idDocumento);
                        existe.Link = ElDoc.Link;
                        existe.Nombre = ElDoc.Nombre;
                        bd.SaveChanges();
                        return RedirectToAction("MostrarModulos");
                    }
                }
                else
                {
                    Temas Obtenido = (Temas)Session["ElTema"];
                    ViewBag.Tema = Obtenido.Nombre;
                    return View();
                }
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al editar el tema", mensaje);
                return View();
            }
        }




        [AuthorizeUser(idNivel: 4)]
        public ActionResult MostrarDocTemas(int id)
        {
            try
            {

                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {
                    Temas ElTema = bd.Temas.Find(id);
                    Session["ElTema"] = ElTema;
                    ViewBag.Tema = ElTema.Nombre;
                    List<Documentos> Docs = bd.Documentos.Where(x => x.idTema == ElTema.idTema).ToList();
                    return View(Docs);
                }
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al mostrar los Documentos", mensaje);
                return View();
            }
        }
        [AuthorizeUser(idNivel: 4)]
        public ActionResult Consulta_Alumnos_Asesorados()
        {


            try
            {
                bool bandera = false;
                Usuario obj = new Usuario();
                obj = (Usuario)Session["User"];
                var pro = (from a in bd.Profesor where a.idUsuario == obj.idUsuario select a).FirstOrDefault();
                var his = (from h in bd.HistorialAsesoria where h.idProfesor == pro.idProfesor select h);

                List<int> idAlu = new List<int>();
                idAlu.Add(0);
                List<Alumnos> MostrarAlumno = new List<Alumnos>();
                foreach (var c in his)
                {

                    foreach (var d in idAlu)
                    {
                        if (d != (int)c.idAlumno)

                            bandera = true;

                        else
                            bandera = false;
                    }

                    if (bandera)
                    {

                        var alu = (from a in bd.Alumnos where a.idAlumno == c.idAlumno select a);
                        List<Alumnos> Encontrados = alu.ToList();
                        foreach (var i in Encontrados)
                        {
                            MostrarAlumno.Add(i);
                        }


                    }


                    idAlu.Add((int)c.idAlumno);
                }
                List<Usuario> MostrarUsuario = new List<Usuario>();
                foreach (var d in MostrarAlumno)
                {
                    List<Usuario> Encontrados_user = bd.Usuario.Where(p => p.idUsuario == d.idUsuario).ToList();
                    foreach (var i in Encontrados_user)
                    {
                        MostrarUsuario.Add(i);
                    }
                }
                ViewBag.Alu = MostrarAlumno;
                ViewBag.Usuario = MostrarUsuario;

                return View();
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al mostrar los alumnos", mensaje);
                return View();
            }
        }

    }
}
