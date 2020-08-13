using System;

namespace GestionCitas.Entidades
{
    public class EspecialidadDTO
    {
        public Int32 Id { get; set; }
        public String Nombre { get; set; }
        public String Descripcion { get; set; }
        public Boolean Activo { get; set; }
    }
}
