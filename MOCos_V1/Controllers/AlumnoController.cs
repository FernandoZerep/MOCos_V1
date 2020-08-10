using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MOCos_V1;
using MOCos_V1.Filters;
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
        public ActionResult Contacto()
        {
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
        public ActionResult ConsultaProfesores()
        {
            try
            {
                List<Profesor> lista = bd.Profesor.Include(u => u.Usuario).ToList();
                return View(lista);
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al consultar a los profesores", mensaje);
                return View();
            }
        }

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

        [AuthorizeUser(idNivel: 3)]
        [HttpPost]
        public ActionResult EditarProfesor(Profesor obj)
        {
            try
            {               
                    Profesor existe = bd.Profesor.Where(i => i.idProfesor == obj.idProfesor).FirstOrDefault();

                    existe.idUsuario = obj.idUsuario;
                    existe.Cubo = obj.Cubo;
                    bd.SaveChanges();
                    return RedirectToAction("ConsultaProfesores");

            }
            catch (Exception msg)
            {
                ModelState.AddModelError("Error al editar al profesor", msg);
                return View();
            }
        }
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
                    idCuatrimestre = s.idCuatrimestre, Nombre = $"{s.Grado} {s.Especialidad} {s.Carrera}"
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
                    Alumnos existe = bd.Alumnos.Include(u => u.Usuario).Include(t => t.Usuario.TiposUsuarios).Include(c=>c.Cuatrimestre).Include(g=>g.Grupo).Where(i => i.idUsuario == obj.idUsuario).FirstOrDefault();
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
                        return RedirectToAction("Login", "Home");
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


    }
}