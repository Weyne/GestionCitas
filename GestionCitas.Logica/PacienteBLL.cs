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
    public class PacienteBLL
    {
        #region singleton
        private static readonly PacienteBLL _instancia = new PacienteBLL();
        public static PacienteBLL Instancia
        {
            get { return PacienteBLL._instancia; }
        }
        #endregion singleton

        #region metodos
        public String Mensaje { get; private set; }

        public Boolean Valida(PacienteDTO item)
        {
            Mensaje = "";
            Boolean resultado = false;

            if (String.IsNullOrWhiteSpace(item.Dni))
                Mensaje += "Por favor, debe indicar el DNI\n\r";
            if (String.IsNullOrWhiteSpace(item.Nombres))
                Mensaje += "Por favor, debe indicar un nombre\n\r";
            if (String.IsNullOrWhiteSpace(item.Apellidos))
                Mensaje += "Por favor, debe indicar los apellidos\n\r";
            if (item.FechaNacimiento == DateTime.Parse("0001-01-01"))
                Mensaje += "Por favor, debe indicar la fecha de nacimiento\n\r";
            if (String.IsNullOrWhiteSpace(item.Sexo))
                Mensaje += "Por favor, debe indicar el sexo\n\r";
            if (String.IsNullOrWhiteSpace(item.Direccion))
                Mensaje += "Por favor, debe indicar la dirección\n\r";
            if (String.IsNullOrWhiteSpace(item.Telefono))
                Mensaje += "Por favor, debe indicar el teléfono\n\r";

            if (!String.IsNullOrWhiteSpace(item.Dni))
            {
                List<PacienteDTO> ListadoPacientes = ListarPacientes().Where(x => x.Dni == item.Dni && x.Id != item.Id).ToList();
                if (ListadoPacientes.Count > 0)
                    Mensaje += "El DNI Ingresado ya se encuentra asignado a otro paciente\n\r";
            }



            if (Mensaje == "") resultado = true;

            return resultado;
        }
        public List<PacienteDTO> ListarPacientes()
        {
            try
            {
                return PacienteDAL.Instancia.ListarPacientes();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public RespuestaSistema GrabarPaciente(PacienteDTO item, String usuario)
        {
            RespuestaSistema objResultado = new RespuestaSistema();
            objResultado.Correcto = false;
            try
            {
                Mensaje = "";
                int intPk;
                objResultado.Correcto = Valida(item);
                objResultado.Mensaje = Mensaje;
                if (objResultado.Correcto)
                {
                    if (item.Id == -1)
                    {
                        intPk = PacienteDAL.Instancia.GrabarPaciente(item, usuario);
                        if (intPk > 0)
                        {
                            objResultado.Mensaje = MensajeSistema.OK_SAVE;
                            objResultado.Correcto = true;
                        }
                    }
                    else
                    {
                        objResultado.Correcto = PacienteDAL.Instancia.EditarPaciente(item, usuario);
                        if (objResultado.Correcto)
                            objResultado.Mensaje = objResultado.Mensaje = MensajeSistema.OK_UPDATE;
                        else
                            objResultado.Mensaje = objResultado.Mensaje = MensajeSistema.ERROR_UPDATE;
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
        public PacienteDTO ObtenerPaciente(Int32 especialidadId)
        {
            try
            {
                return PacienteDAL.Instancia.ObtenerPaciente(especialidadId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public RespuestaSistema EliminarPaciente(PacienteDTO item, String usuario)
        {
            RespuestaSistema objResultado = new RespuestaSistema();
            Boolean resultado = false;
            Mensaje = "";
            item.Activo = false;
            try
            {
                resultado = PacienteDAL.Instancia.EditarPaciente(item, usuario);
                if (resultado)
                    Mensaje += MensajeSistema.OK_DELETE;
                else Mensaje += MensajeSistema.ERROR_DELETE;
            }
            catch (Exception e)
            {
                throw e;
            }
            objResultado.Mensaje = Mensaje;
            objResultado.Correcto = resultado;
            return objResultado;
        }
        #endregion
    }
}
