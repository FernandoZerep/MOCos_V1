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
    
    public partial class Materia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Materia()
        {
            this.Unidad = new HashSet<Unidad>();
            this.Profesor1 = new HashSet<Profesor>();
        }
    
        public int idMateria { get; set; }
        public Nullable<int> idCoordinador { get; set; }
        public string NombreMateria { get; set; }
        public Nullable<int> idCuatrimestre { get; set; }
    
        public virtual Profesor Profesor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Unidad> Unidad { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profesor> Profesor1 { get; set; }
        public virtual Cuatrimestre Cuatrimestre { get; set; }
    }
}
