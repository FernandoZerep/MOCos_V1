﻿using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.WebPages;

namespace MOCos_V1.Controllers
{
    public class HomeController : Controller
    {
        mocOS_BDEntities bd = new mocOS_BDEntities();

        public ActionResult Index(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your Contact page";
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Message = "Your Contact page";
            return View();
        }
        public ActionResult pruebaregis()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InsertarUsuario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertarUsuario(Usuario obj)
        {
            try
            {
                using (mocOS_BDEntities bd = new mocOS_BDEntities())
                {

                    bd.Usuario.Add(obj);
                    bd.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception mensaje)
            {
                ModelState.AddModelError("Error al ingresar al usuario", mensaje);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(string correo, string password)
        {
            try
            {
                int? nivel = 0;
                string controlador = "";
                string action = "";
                using (mocOS_BDEntities db = new mocOS_BDEntities())
                {
                    var oUser = (from d in db.Usuario
                                 where d.Correo == correo && d.Contrasena == password.Trim()
                                 select d).FirstOrDefault();
                    if(oUser == null)
                    {
                        ViewBag.Error = "Usuario o Contraseña";
                        return View();
                    }
                    nivel = oUser.idTipoUsuario;
                    Session["User"] = oUser;
                    Session["name"] = oUser.Nombre + " " + oUser.ApellidoPaterno + " " + oUser.ApellidoMaterno;
                    Session["profile"] = oUser.FotoPerfil;
                    if (nivel == 1)
                    {
                        controlador = "Administrador";
                        action = "InicioAdmin";
                    }
                    if (nivel == 3)
                    {
                        controlador = "Alumno";
                        action = "Inicio";
                    }
                    if (nivel == 4)
                    {
                        controlador = "Maestro";
                        action = "Perfil";
                    }
                }
                ViewBag.Sesion = Session["User"];
                return RedirectToAction(action, controlador);
                
                
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

            
        }
        [HttpGet]
        public ActionResult Insertar_Alumno()
        {
            var genero = new SelectList(new[] { "Masculino", "Femenino" });
            ViewBag.genero = genero;
            ViewBag.idCuatrimestre = new SelectList(bd.Cuatrimestre, "idCuatrimestre", "Grado");
            ViewBag.idGrupo = new SelectList(bd.Grupo, "idGrupo", "Grupo1");
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Insertar_Alumno(Alumnos Alu, string Nombre,string ApellidoP ,string ApellidoM,string Correo,string Contrasena,string Direccion,string celular,string genero,string Fecha )
        {
            WebImage iagen;
            HttpPostedFileBase FileBase = Request.Files[0];
            if (FileBase.InputStream.Length != 0)
            {

                iagen = new WebImage(FileBase.InputStream);
            }
            else
            {
                iagen = new WebImage("~/Content/Registrar/assets/img/default-avatar.jpg");
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
                user.FechaDeNacimiento = Convert.ToDateTime(Fecha);
                user.idTipoUsuario = 3;
                bd.Usuario.Add(user);
                bd.Alumnos.Add(Alu);
                bd.SaveChanges();
                insertar(iagen.GetBytes(),Alu);

                return RedirectToAction("Login");
           
                
        }

        public void insertar(byte[] imagen, Alumnos alu)
        {

            Usuario user = bd.Usuario.OrderByDescending(x => x.idUsuario).First();
            user.FotoPerfil = imagen;
            alu.idUsuario = user.idUsuario;
            bd.SaveChanges();

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bd.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpGet]
        public ActionResult Insertar_Alumno1()
        {
            var genero = new SelectList(new[] { "Masculino", "Femenino" });
            ViewBag.genero = genero;
            ViewBag.idCuatrimestre = new SelectList(bd.Cuatrimestre, "idCuatrimestre", "Grado");
            ViewBag.idGrupo = new SelectList(bd.Grupo, "idGrupo", "Grupo1");
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Insertar_Alumno1(Alumnos Alu, string Nombre, string ApellidoP, string ApellidoM, string Correo, string Contrasena, string Direccion, string celular, string genero, string Fecha)
        {
            WebImage iagen;
            HttpPostedFileBase FileBase = Request.Files[0];
            if (FileBase.InputStream.Length != 0)
            {

                iagen = new WebImage(FileBase.InputStream);
            }
            else
            {
                iagen = new WebImage("~/Content/Registrar/assets/img/default-avatar.jpg");
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
            user.FechaDeNacimiento = Convert.ToDateTime(Fecha);
            user.idTipoUsuario = 3;
            bd.Usuario.Add(user);
            bd.Alumnos.Add(Alu);
            bd.SaveChanges();
            insertar(iagen.GetBytes(), Alu);

            return RedirectToAction("Login");


        }
    }
}