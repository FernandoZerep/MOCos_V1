//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MOCos_V1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            this.Administrador = new HashSet<Administrador>();
            this.Alumnos = new HashSet<Alumnos>();
            this.Profesor = new HashSet<Profesor>();
        }
    
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MinLength(4, ErrorMessage = "{0} Debe terner una logitud mayor o igual a 4 caracteres")]
        [DisplayName("Nombre(s)")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "{0} Debe terner una logitud mayor o igual a 6 caracteres")]
        [DisplayName("Contraseña")]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MinLength(5, ErrorMessage = "{0} Debe terner una logitud mayor o igual a 5 caracteres")]
        [DisplayName("Apellidos Materno")]
        public string ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MinLength(5, ErrorMessage = "{0} Debe terner una logitud mayor o igual a 5 caracteres")]
        [DisplayName("Apellido Paterno")]
        public string ApellidoPaterno { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [MinLength(6, ErrorMessage = "{0} Debe terner una logitud mayor o igual a 6 caracteres")]
        [DisplayName("Dirección")]
        public string Dirección { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(10)]
        [DisplayName("Celular")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [DisplayName("Género")]

        public string Genero { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        [DataType(DataType.Date)]
        [DisplayName("Fecha de nacimiento")]
        public Nullable<System.DateTime> FechaDeNacimiento { get; set; }
        public byte[] FotoPerfil { get; set; }
        public Nullable<int> idTipoUsuario { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string Correo { get; set; }
        public string idToken { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Administrador> Administrador { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Alumnos> Alumnos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profesor> Profesor { get; set; }
        public virtual TiposUsuarios TiposUsuarios { get; set; }
    }
}
