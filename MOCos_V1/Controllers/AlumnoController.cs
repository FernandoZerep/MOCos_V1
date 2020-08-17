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



    }
}