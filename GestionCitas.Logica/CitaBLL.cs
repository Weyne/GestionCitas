using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCitas.Entidades;
using GestionCitas.Datos;
using System.Transactions;
using Base.Comunes;

namespace GestionCitas.Logica
{
    public class CitaBLL
    {
        #region singleton
        private static readonly CitaBLL _instancia = new CitaBLL();
        public static CitaBLL Instancia
        {
            get { return CitaBLL._instancia; }
        }
        #endregion singleton

        #region constantes
        private const String CITA_PENDIENTE = "PENDIENTE";
        #endregion

        #region metodos

        public String Mensaje { get; private set; }
        public Boolean Valida(CitaDTO item)
        {
            Mensaje = "";
            Boolean resultado = false;
            List<HorarioDTO> horarioMedico = null;
            List<CitaDTO> citasMedico = null;
            Boolean horarioDisponible = false;

            if (item.PacienteId <= 0)
                Mensaje += "Por favor, debe indicar el paciente\n\r";
            if (item.MedicoId <= 0)
                Mensaje += "Por favor, debe indicar el medico\n\r";
            if (item.FechaAtencion == DateTime.Parse("0001-01-01"))
                Mensaje += "Por favor, debe indicar la fecha de atención\n\r";
            if (item.HoraInicio == TimeSpan.Parse("00:00"))
                Mensaje += "Por favor, debe indicar la hora inicio de la cita\n\r";
            if (item.HoraFin == TimeSpan.Parse("00:00"))
                Mensaje += "Por favor, debe indicar la hora fin de la cita\n\r";
            if (item.HoraInicio != TimeSpan.Parse("00:00") && item.HoraFin != TimeSpan.Parse("00:00"))
            {
                if (item.HoraInicio >= item.HoraFin)
                    Mensaje += "La hora inicio no puede ser mayor o igual que la hora fin de la cita\n\r";

                if (item.MedicoId > 0)
                {
                    horarioMedico = HorarioBLL.Instancia.ListarHorariosPorMedico(item.MedicoId).Where(x => x.Activo == true && x.FechaAtencion == item.FechaAtencion && (item.HoraInicio >= x.HoraInicio && item.HoraInicio < x.HoraFin) && (item.HoraFin > x.HoraInicio && item.HoraFin <= x.HoraFin)).ToList();

                    if (horarioMedico == null || horarioMedico.Count == 0)
                        Mensaje += "El horario indicado para la cita no puede ser registrado,\n\rporque no se encuentro dentro del horario del médico";
                    else
                        horarioDisponible = true;

                    if (horarioDisponible)
                    {
                        citasMedico = CitaBLL.Instancia.ListarCitasMedico(item.MedicoId, item.FechaAtencion).Where(x => x.Id != item.Id && x.Activo == true && ((item.HoraInicio >= x.HoraInicio && item.HoraInicio < x.HoraFin) || (item.HoraFin > x.HoraInicio && item.HoraFin <= x.HoraFin))).ToList();

                        if (citasMedico != null && citasMedico.Count > 0)
                            Mensaje += "El horario indicado para la cita no puede ser registrado,\n\rporque ya existe una cita programada";
                    }

                }

            }

            if (Mensaje == "") resultado = true;

            return resultado;
        }
        public RespuestaSistema GrabarCita(CitaDTO item, String usuario)
        {
            RespuestaSistema objResultado = new RespuestaSistema();
            try
            {

                objResultado.Correcto = Valida(item);
                objResultado.Mensaje = Mensaje;
                if (objResultado.Correcto)
                {
                    int intPk;
                    if (item.Id == -1)
                    {
                        item.Estado = CITA_PENDIENTE;
                        intPk = CitaDAL.Instancia.GrabarCita(item, usuario);
                        if (intPk > 0)
                        {
                            objResultado.LlaveInsertada = intPk;
                            objResultado.Mensaje = MensajeSistema.OK_SAVE;
                            objResultado.Accion = MensajeSistema.ACTION_INSERT;
                            objResultado.Correcto = true;
                        }
                    }
                    else
                    {
                        objResultado.LlaveModificada = item.Id;
                        objResultado.Mensaje = MensajeSistema.OK_UPDATE;
                        objResultado.Accion = MensajeSistema.ACTION_UPDATE;
                        objResultado.Correcto = CitaDAL.Instancia.EditarCita(item, usuario);
                    }
                }

            }
            catch (Exception ex)
            {
                objResultado.Mensaje = string.Format("{0}\r{1}", MensajeSistema.ERROR_SAVE, ex.Message);
                objResultado.Correcto = false;
            }
            return objResultado;
        }
        public List<CitaDTO> ListarCitasMedico(Int32 medicoId, DateTime fechaAtencion)
        {
            try
            {
                return CitaDAL.Instancia.ListarCitasMedico(medicoId, fechaAtencion);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public CitaDTO ObtenerCita(Int32 citaId)
        {
            try
            {
                return CitaDAL.Instancia.ObtenerCita(citaId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}
