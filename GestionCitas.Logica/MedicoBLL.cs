using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCitas.Entidades;
using GestionCitas.Datos;
using Base.Comunes;
using System.Transactions;

namespace GestionCitas.Logica
{
    public class MedicoBLL
    {
        #region singleton
        private static readonly MedicoBLL _instancia = new MedicoBLL();
        public static MedicoBLL Instancia
        {
            get { return MedicoBLL._instancia; }
        }
        #endregion singleton

        #region metodos

        public String Mensaje { get; private set; }

        public Boolean Valida(MedicoDTO item)
        {
            Mensaje = "";
            Boolean resultado = false;
            int numRegistrosAEliminar = 0;

            if (String.IsNullOrWhiteSpace(item.Dni))
                Mensaje += "Por favor, debe indicar el DNI\n\r";
            if (String.IsNullOrWhiteSpace(item.NumeroColegiatura))
                Mensaje += "Por favor, debe indicar el N° de colegiatura\n\r";
            if (String.IsNullOrWhiteSpace(item.Nombres))
                Mensaje += "Por favor, debe indicar un nombre\n\r";
            if (String.IsNullOrWhiteSpace(item.Apellidos))
                Mensaje += "Por favor, debe indicar los apellidos\n\r";
            if (item.FechaNacimiento == DateTime.Parse("0001-01-01"))
                Mensaje += "Por favor, debe indicar la fecha de nacimiento\n\r";
            if (String.IsNullOrWhiteSpace(item.Sexo))
                Mensaje += "Por favor, debe indicar el sexo\n\r";
            if (String.IsNullOrWhiteSpace(item.Telefono))
                Mensaje += "Por favor, debe indicar el teléfono\n\r";
            if (String.IsNullOrWhiteSpace(item.Direccion))
                Mensaje += "Por favor, debe indicar la dirección\n\r";
            if (String.IsNullOrWhiteSpace(item.Telefono))
                Mensaje += "Por favor, debe indicar el teléfono\n\r";

            if (!String.IsNullOrWhiteSpace(item.Dni))
            {
                List<MedicoDTO> ListadoMedicos = MedicoBLL.Instancia.ListarMedicos().Where(x => x.Dni == item.Dni && x.Id != item.Id).ToList();
                if (ListadoMedicos.Count > 0)
                    Mensaje += "El DNI Ingresado ya se encuentra asignado a otro médico\n\r";
            }

            if (item.ListaEspecialidades != null && item.ListaEspecialidades.Count > 0)
                numRegistrosAEliminar = item.ListaEspecialidades.Where(x => x.Activo == false).ToList().Count;

            if (item.ListaEspecialidades != null && item.ListaEspecialidades.Count > 0 && (numRegistrosAEliminar != item.ListaEspecialidades.Count))
            {
                int numEspecialidades = item.ListaEspecialidades.Count;
                Boolean esRegistroDuplicado = false;
                for (int i = 0; i < numEspecialidades; i++)
                {
                    if (!item.ListaEspecialidades[i].Activo) continue;

                    if (item.ListaEspecialidades[i].EspecialidadId == 0)
                    {
                        Mensaje += "Por favor, debe indicar el nombre de la especialidad\n\r";
                        break;
                    }
                    if (String.IsNullOrWhiteSpace(item.ListaEspecialidades[i].Especialidad))
                    {
                        Mensaje += "Por favor, debe indicar el nombre de la especialidad\n\r";
                        break;
                    }

                    for (int j = 0; j < numEspecialidades; j++)
                    {
                        if (!item.ListaEspecialidades[j].Activo) continue;
                        if (i != j)
                        {
                            if (item.ListaEspecialidades[i].EspecialidadId == item.ListaEspecialidades[j].EspecialidadId)
                            {
                                Mensaje += "La especialidad '" + item.ListaEspecialidades[i].Especialidad + "'\n\rse repite en la lista de especialidades\n\r";
                                esRegistroDuplicado = true;
                                break;
                            }
                        }
                    }
                    if (esRegistroDuplicado) break;
                }
            }
            else
                Mensaje += "Por favor, debe indicar al menos una especialidad\n\r";

            if (Mensaje == "") resultado = true;

            return resultado;
        }

        public List<MedicoDTO> ListarMedicos()
        {
            try
            {
                return MedicoDAL.Instancia.ListarMedicos();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public RespuestaSistema GrabarMedico(MedicoDTO item, String usuario)
        {
            RespuestaSistema objResultado = new RespuestaSistema();
            Int32 pkInsertado = -1;
            Boolean resultado = false;
            Mensaje = "";
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    resultado = Valida(item);
                    objResultado.Mensaje = Mensaje;
                    if (resultado)
                    {
                        if (item.Id == -1)
                        {
                            pkInsertado = MedicoDAL.Instancia.GrabarMedico(item, usuario);
                            if (pkInsertado > 0)
                            {
                                foreach (MedicoEspecialidadDTO objEspecialidad in item.ListaEspecialidades)
                                {
                                    resultado = MedicoEspecialidadDAL.Instancia.GrabarEspecialidadDeMedico(objEspecialidad, pkInsertado, usuario);
                                    if (resultado == false)
                                    {
                                        objResultado.Mensaje = string.Format("{0}\r", MensajeSistema.ERROR_UPDATE);
                                        objResultado.Correcto = resultado;
                                        return objResultado;
                                    }
                                }
                                if (resultado)
                                {
                                    objResultado.Mensaje = MensajeSistema.OK_SAVE;
                                    transactionScope.Complete();
                                }
                                else
                                {
                                    objResultado.Mensaje = MensajeSistema.ERROR_SAVE;
                                }
                            }
                            else
                            {
                                objResultado.Correcto = false;
                                objResultado.Mensaje = MensajeSistema.ERROR_UPDATE;
                            }
                        }
                        else
                        {
                            resultado = MedicoDAL.Instancia.EditarMedico(item, usuario);
                            if (resultado)
                            {
                                foreach (MedicoEspecialidadDTO objEspecialidad in item.ListaEspecialidades)
                                {
                                    if (objEspecialidad.Id == 0)
                                        resultado = MedicoEspecialidadDAL.Instancia.GrabarEspecialidadDeMedico(objEspecialidad, item.Id, usuario);
                                    else
                                        resultado = MedicoEspecialidadDAL.Instancia.EditarEspecialidad(objEspecialidad, item.Id, usuario);
                                    if (resultado == false)
                                    {
                                        objResultado.Mensaje = string.Format("{0}\r", MensajeSistema.ERROR_UPDATE);
                                        objResultado.Correcto = resultado;
                                        return objResultado;
                                    }
                                }
                                if (resultado)
                                {
                                    objResultado.Mensaje = MensajeSistema.OK_UPDATE;
                                    transactionScope.Complete();
                                }
                                else Mensaje = MensajeSistema.ERROR_UPDATE;
                            }
                            else
                            {
                                objResultado.Correcto = false;
                                objResultado.Mensaje = MensajeSistema.ERROR_UPDATE;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    objResultado.Mensaje = string.Format("{0}\r{1}", MensajeSistema.ERROR_SAVE, ex.Message);
                    resultado = false;
                }
                objResultado.Correcto = resultado;

                return objResultado;
            }
        }

        public RespuestaSistema EliminarMedico(MedicoDTO item, String usuario)
        {
            RespuestaSistema objResultado = new RespuestaSistema();
            Boolean resultado = false;
            Mensaje = "";
            item.Activo = false;
            try
            {
                resultado = MedicoDAL.Instancia.EditarMedico(item, usuario);
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
        public MedicoDTO ObtenerMedico(Int32 medicoId)
        {
            try
            {
                return MedicoDAL.Instancia.ObtenerMedico(medicoId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<MedicoDTO> ListarMedicosPorHorario(String opcionBusqueda, DateTime fechaAtencion, String filtro)
        {
            try
            {
                return MedicoDAL.Instancia.ListarMedicosPorHorario(opcionBusqueda, fechaAtencion, filtro);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion
    }
}
