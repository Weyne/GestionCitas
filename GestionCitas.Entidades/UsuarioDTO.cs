using System;

namespace GestionCitas.Entidades
{
    public class UsuarioDTO
    {
        public Int32 Id { get; set; }
        public String Nombre { get; set; }
        public String Clave { get; set; }
        public Int32 MedicoId { get; set; }
        public String MedicoNombres { get; set; }
        public String MedicoApellidos { get; set; }
        public Boolean Activo { get; set; }
    }
}
