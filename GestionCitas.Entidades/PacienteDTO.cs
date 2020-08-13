using System;

namespace GestionCitas.Entidades
{
    public class PacienteDTO
    {
        public Int32 Id { get; set; }
        public String Nombres { get; set; }
        public String Apellidos { get; set; }
        public String Dni { get; set; }
        public String Direccion { get; set; }
        public String Telefono { get; set; }
        public String Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public Boolean Activo { get; set; }
    }
}
