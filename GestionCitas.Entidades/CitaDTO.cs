using System;

namespace GestionCitas.Entidades
{
    public class CitaDTO
    {
        public Int32 Id { get; set; }
        public Int32 MedicoId { get; set; }
        public String Medico { get; set; }
        public Int32 PacienteId { get; set; }
        public String PacienteNombres { get; set; }
        public String PacienteApellidos { get; set; }
        public String PacienteDni { get; set; }
        public String Estado { get; set; }
        public String Observaciones { get; set; }
        public DateTime FechaAtencion { get; set; }
        public DateTime FechaAtencionInicio { get; set; }
        public DateTime FechaAtencionFin { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public Boolean Activo { get; set; }
    }
}
