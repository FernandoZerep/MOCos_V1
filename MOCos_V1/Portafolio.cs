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
    
    public partial class Portafolio
    {
        public int idPortafolio { get; set; }
        public Nullable<int> idHistorial { get; set; }
        public byte[] DocumentoContestado { get; set; }
        public Nullable<int> Calificacion { get; set; }
    
        public virtual HistorialAsesoria HistorialAsesoria { get; set; }
    }
}
