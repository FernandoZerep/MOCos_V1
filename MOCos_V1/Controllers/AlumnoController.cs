using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MOCos_V1;
using MOCos_V1.Filters;
using System.Net;
using System.Net.Mail;
using System.Web.Helpers;

namespace MOCos_V1.Controllers
{
    public class AlumnoController : Controller
    {
        mocOS_BDEntities bd = new mocOS_BDEntities();

        [AuthorizeUser(idNivel: 3)]
        public ActionResult Inicio()
        {
            return View();
        }
        [AuthorizeUser(idNivel: 3)]
        public ActionResult Cerrar()
        {
            Session["User"] = null;
            Session["name"] = null;
            Session["profile"] = null;
            return RedirectToAction("Index", "Home");
        }
        [AuthorizeUser(idNivel: 3)]
        public ActionResult Mapa()
        {
            
            return View();
        }
        [AuthorizeUser(idNivel: 3)]
        [HttpGet]
        public ActionResult Contacto()
        {
            return View();
        }
        [AuthorizeUser(idNivel: 3)]
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
        [AuthorizeUser(idNivel: 3)]
        public ActionResult Login()
        {
            return View();
        }
        //[AuthorizeUser(idNivel: 3)]
        //public ActionResult Mostrar_Materias()
        //{
        //    try
        //    { 
        //            Usuario obj = new Usuario();
        //            obj = (Usuario)Session["User"];
        //        var mat = (from d in bd.Materia
        //                   select d);
        //            Session["UserMateria"] = mat;
        //            ViewBag.Materia = mat;
        //            List<Unidad> Uni = bd.Unidad.Where(x => x.idMateria == mat.idMateria).ToList();
        //            //Obteniendo los temas
        //            List<Temas> MostrarTemas = new List<Temas>();
        //            foreach (var c in Uni)
        //            {
        //                List<Temas> Encontrados = bd.Temas.Where(x => x.idUnidad == c.idUnidad).ToList();
        //                foreach (var i in Encontrados)
        //                {
        //                    MostrarTemas.Add(i);
        //                }
        //            }
        //            ViewBag.Temas = MostrarTemas;
        //            return View(Uni);
        //    }
        //    catch (Exception mensaje)
        //    {
        //        ModelState.AddModelError("Error al mostrar las Unidades", mensaje);
        //        return View();
        //    }
        //}
        //public List<string> regresamaterias()
        //{

        //     List<Materia> ListaMate = bd.Materia.Include(x=> x.NombreMateria).ToList();
        //    return ListaMate;
        //}
        ////[AuthorizeUser(idNivel: 3)]
        //public ActionResult pruebaregis()
        //{
        //    return View();
        //}
        //[AuthorizeUser(idNivel: 3)]
        

        //public ActionResult DetallesProfesor(int id)
        //{
        //    try
        //    {
        //        using (mocOS_BDEntities bd = new mocOS_BDEntities())
        //        {
        //            Profesor existe = bd.Profesor.Find(id);

        //            return View(existe);
        //        }
        //    }
        //    catch (Exception mensaje)
        //    {
        //        ModelState.AddModelError("Error al mostrar detalles de la persona", mensaje);
        //        return View();
        //    }
        //}
        //[AuthorizeUser(idNivel: 3)]
        //[HttpGet]
        //public ActionResult EditarProfesor(int id)
        //{
        //    try
        //    {

        //        Profesor obj = bd.Profesor.Include(u=> u.Usuario).Where(i => i.idProfesor == id).FirstOrDefault();
        //        ViewBag.idUsuario = new SelectList(bd.Usuario, "idUsuario", "Nombre");
        //        ViewBag.selectusuario = obj.idUsuario;
        //        return View(obj);
        //    }
        //    catch (Exception msg)
        //    {
        //        ModelState.AddModelError("Error al editar al profesor", msg);
        //        return View();
        //    }
        //}

        //[AuthorizeUser(idNivel: 3)]
        //[HttpPost]
        //public ActionResult EditarProfesor(Profesor obj)
        //{
        //    try
        //    {               
        //            Profesor existe = bd.Profesor.Where(i => i.idProfesor == obj.idProfesor).FirstOrDefault();

        //            existe.idUsuario = obj.idUsuario;
        //            existe.Cubo = obj.Cubo;
        //            bd.SaveChanges();
        //            return RedirectToAction("ConsultaProfesores");

        //    }
        //    catch (Exception msg)
        //    {
        //        ModelState.AddModelError("Error al editar al profesor", msg);
        //        return View();
        //    }
        //}


        //EDITPROFILE
        [AuthorizeUser(idNivel: 3)]
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

        public class Person
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        [AuthorizeUser(idNivel: 3)]
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

                    if (LaSesion.Correo != obj.Usuario.Correo)
                    {
                        Session["User"] = null;
                        Session["name"] = null;
                        Session["profile"] = null;
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        Alumnos nuevo = bd.Alumnos.Include(u => u.Usuario).Include(t => t.Usuario.TiposUsuarios).Include(c => c.Cuatrimestre).Include(g => g.Grupo).Where(i => i.idUsuario == obj.idUsuario).FirstOrDefault();
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

        [AuthorizeUser(idNivel: 3)]
        public ActionResult ConsultaMaterias()
        {
            try
            {
               
                    List<Materia> lista = bd.Materia.Include(c => c.Cuatrimestre).Include(o => o.Profesor).Include(u => u.Profesor.Usuario).ToList();
                    return View(lista);
               
            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al consultar a los alumnos", msg);
                return View();
            }
        }

        [AuthorizeUser(idNivel: 3)]
        public ActionResult MostrarModulos(int id)
        {
            try
            {
                var mat = (from d in bd.Materia
                           where d.idMateria == id
                           select d).FirstOrDefault();
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
                List<Profesor> pro = bd.Profesor.Include(u => u.Usuario).Where(x => x.idMateriaEnseña == id).ToList();
                ViewBag.Profesores = pro;
                ViewBag.Temas = MostrarTemas;
                return View(Uni);
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al mostrar las Unidades", mensaje);
                return View();
            }
        }
        [AuthorizeUser(idNivel: 3)]
        public ActionResult Registrar_asesoria(int id, int idPro)
        {
            HistorialAsesoria ha = new HistorialAsesoria();
            Usuario obj = new Usuario();
            obj = (Usuario)Session["User"];

            var alu = (from d in bd.Alumnos
                       where d.idUsuario == obj.idUsuario
                       select d).FirstOrDefault();
            ha.FechaRegistro = DateTime.Today;
            ha.idAlumno = alu.idAlumno;
            ha.idTema = id;
            ha.idProfesor = idPro;
            bd.HistorialAsesoria.Add(ha);
            bd.SaveChanges();
            Correo_Aviso(obj, id, idPro);

            return RedirectToAction("Consulta_Profesores_Asesores");
        }
        [AuthorizeUser(idNivel: 3)]
        [HttpPost]
        public ActionResult Correo_Aviso(Usuario user, int id, int idPro)
        {
            try
            {
                var tema = (from d in bd.Temas where d.idTema == id select d).FirstOrDefault();
                var uni = (from u in bd.Unidad where u.idUnidad == tema.idUnidad select u).FirstOrDefault();
                var mat = (from m in bd.Materia where m.idMateria == uni.idMateria select m).FirstOrDefault();
                var pro = (from p in bd.Profesor where p.idProfesor == idPro select p).FirstOrDefault();
                var usu = (from us in bd.Usuario where us.idUsuario == pro.idUsuario select us).FirstOrDefault();
                string co = "pruebaplicacion5@gmail.com", rec = "oscar.c.t.03@gmail.com";
                MailMessage correo = new MailMessage();
                correo.From = new MailAddress(co);
                correo.To.Add(rec);
                correo.Subject = "Aviso de Asesoria";
                correo.Body = "El usuario " + user.Nombre + " " + user.ApellidoPaterno + " " + user.ApellidoMaterno + " con correo: " + user.Correo + " " +
                    "se a registrado a una asesoría en el tema de: " + tema.Nombre + " en la materia de: " + mat.NombreMateria + ". " +
                    "Con el profesor(a) " + usu.Nombre + " " + usu.ApellidoPaterno + " " + usu.ApellidoMaterno;
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
        [AuthorizeUser(idNivel: 3)]
        public ActionResult Consulta_Profesores_Asesores()
        {
   

            try
            {
                bool bandera = false;
                Usuario obj = new Usuario();
                obj = (Usuario)Session["User"];
                var alu= (from a in bd.Alumnos where a.idUsuario ==obj.idUsuario select a).FirstOrDefault();
                var his = (from h in bd.HistorialAsesoria where h.idAlumno == alu.idAlumno select h );

                List<int> idProfe = new List<int>();
                idProfe.Add(0);
                List<Profesor> MostrarProfes = new List<Profesor>();
                foreach (var c in his)
                {
                   
                    foreach(var d in idProfe)
                    {
                        if (d != (int)c.idProfesor)
                        
                            bandera = true;
                        
                        else
                            bandera = false;
                     }

                    if (bandera)
                        {

                            var pro = (from p in bd.Profesor join m in bd.Materia on p.idMateriaEnseña equals m.idMateria where p.idProfesor == c.idProfesor select p);
                            List<Profesor> Encontrados = pro.ToList();
                            foreach (var i in Encontrados)
                            {
                                MostrarProfes.Add(i);
                            }
                           

                        }
                       
                    
                    idProfe.Add((int)c.idProfesor);
                }
                List<Usuario> MostrarUsuario = new List<Usuario>();
                foreach (var d in MostrarProfes)
                {
                    List<Usuario> Encontrados_user = bd.Usuario.Where(p => p.idUsuario ==d.idUsuario).ToList();
                    foreach (var i in Encontrados_user)
                    {
                        MostrarUsuario.Add(i);
                    }
                }
                ViewBag.Profe= MostrarProfes;
                ViewBag.Usuario = MostrarUsuario;
               
                return View();
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al mostrar los asesores", mensaje);
                return View();
            }
        }

    }
}