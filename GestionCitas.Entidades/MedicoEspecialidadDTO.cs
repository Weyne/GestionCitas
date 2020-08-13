using System;

namespace GestionCitas.Entidades
{
    public class MedicoEspecialidadDTO
    {
        public Int32 Id { get; set; }
        public Int32 EspecialidadId { get; set; }
        public String Especialidad { get; set; }
        public String EspecialidadDescripcion { get; set; }
        public Boolean Activo { get; set; }
    }
}
