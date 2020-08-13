using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCitas.Entidades;
using GestionCitas.Datos;
using Base.Comunes;

namespace GestionCitas.Logica
{
    public class HorarioBLL
    {

        #region singleton
        private static readonly HorarioBLL _instancia = new HorarioBLL();
        public static HorarioBLL Instancia
        {
            get { return HorarioBLL._instancia; }
        }
        #endregion singleton

        #region metodos
        public String Mensaje { get; private set; }
        public Boolean Valida(HorarioDTO item)
        {
            Mensaje = "";
            Boolean resultado = false;
            List<HorarioDTO> horarioMedico = null;

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
                    Mensaje += "La hora inicio no puede ser mayor o igual que la hora fin del horario\n\r";

                if (item.MedicoId > 0 && item.FechaAtencion != DateTime.Parse("0001-01-01"))
                {
                    horarioMedico = HorarioBLL.Instancia.ListarHorariosPorMedico(item.MedicoId)
                                    .Where(
                                            x =>
                                                x.Id != item.Id && x.Activo == true
                                                && x.FechaAtencion == item.FechaAtencion
                                                && ((item.HoraInicio >= x.HoraInicio && item.HoraInicio < x.HoraFin) || (item.HoraFin > x.HoraInicio && item.HoraFin <= x.HoraFin))
                                    ).ToList();

                    if (horarioMedico == null || horarioMedico.Count > 0)
                        Mensaje += "El horario indicado no puede ser registrado,\n\rporque ya existe un horario asignado dentro del rango de horas indicado";

                }

            }

            if (Mensaje == "") resultado = true;

            return resultado;
        }
        public List<HorarioDTO> ListarHorariosPorMedico(Int32 medicoId)
        {
            try
            {
                return HorarioDAL.Instancia.ListarHorariosPorMedico(medicoId);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public RespuestaSistema GrabarHorario(HorarioDTO item, String usuario)
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
                        intPk = HorarioDAL.Instancia.GrabarHorario(item, usuario);
                        if (intPk > 0)
                        {
                            objResultado.Mensaje = MensajeSistema.OK_SAVE;
                            objResultado.Correcto = true;
                        }
                    }
                    else
                    {
                        objResultado.Mensaje = MensajeSistema.OK_UPDATE;
                        objResultado.Correcto = HorarioDAL.Instancia.EditarHorario(item, usuario);
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
        public HorarioDTO ObtenerHorario(Int32 horarioId)
        {
            try
            {
                return HorarioDAL.Instancia.ObtenerHorario(horarioId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

    }
}
