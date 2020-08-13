using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionCitas.Entidades; //referencia a la capa entidades
using GestionCitas.Datos; //referencia a la capa de datos
using Base.Comunes; //referencia a la capa comunes
namespace GestionCitas.Logica
{
    public class EspecialidadBLL //recuerden siempre crear la clase de forma publica
    {
        //patrón singleton
        #region singleton
        private static readonly EspecialidadBLL _instancia = new EspecialidadBLL();
        public static EspecialidadBLL Instancia
        {
            get { return EspecialidadBLL._instancia; }
        }
        #endregion singleton

        #region metodos
        public String Mensaje { get; private set; }//variable que nos permite guardar los mensajes de errores que vamos encontrando

        public Boolean Valida(EspecialidadDTO item)
        {
            Mensaje = "";
            Boolean resultado = false;

            if (String.IsNullOrWhiteSpace(item.Nombre))
                Mensaje += "Por favor, debe indicar un nombre\n\r";
            if (String.IsNullOrWhiteSpace(item.Descripcion))
                Mensaje += "Por favor, debe indicar la descripción\n\r";

            if (Mensaje == "") resultado = true;

            return resultado;
        }

        public List<EspecialidadDTO> ListarEspecialidades()
        {
            try
            {
                return EspecialidadDAL.Instancia.ListarEspecialidades();
            }
            catch (Exception e) { throw e; }
        }
        public RespuestaSistema GrabarEspecialidad(EspecialidadDTO item, String usuario)
        {
            RespuestaSistema objResultado = new RespuestaSistema();
            try
            {

                objResultado.Correcto = Valida(item);
                objResultado.Mensaje = Mensaje;
                if (objResultado.Correcto)
                {
                    int intPk;
                    if (item.Id == -1)//si es un registro nuevo, entonces registro la especialidad
                    {
                        intPk = EspecialidadDAL.Instancia.GrabarEspecialidad(item, usuario);
                        if (intPk > 0)//si se retorno un pk, entonces todo salio bien
                        {
                            objResultado.Mensaje = MensajeSistema.OK_SAVE;
                            objResultado.Correcto = true;
                        }
                        else// algo ocurrio y no se pudo registrar, devolver mensaje de error
                        {
                            objResultado.Mensaje = MensajeSistema.ERROR_SAVE;
                            objResultado.Correcto = false;
                        }
                    }
                    else //en caso contrario, es una especialidad ya creada por lo tanto la acción a realizar es la de modificar
                    {
                        objResultado.Correcto = EspecialidadDAL.Instancia.EditarEspecialidad(item, usuario);
                        if (objResultado.Correcto)//si se retorno true entonces, mensaje de éxito
                            objResultado.Mensaje = MensajeSistema.OK_UPDATE;
                        else// algo ocurrio y no se pudo modificar, devolver mensaje de error
                            objResultado.Mensaje = MensajeSistema.ERROR_UPDATE;
                    }
                }

            }
            catch (Exception ex)
            {
                objResultado.Mensaje = string.Format("{0}\r{1}", MensajeSistema.ERROR_SAVE, ex.Message);
                objResultado.Correcto = false;
            }
            return objResultado;//retorno la respuesta de registro o modificación
        }

        public EspecialidadDTO ObtenerEspecialidad(Int32 especialidadId)
        {
            try
            {
                return EspecialidadDAL.Instancia.ObtenerEspecialidad(especialidadId);
            }
            catch (Exception e) { throw e; }
        }
        #endregion
    }
}
