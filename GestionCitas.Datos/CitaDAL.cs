using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using GestionCitas.Entidades;

namespace GestionCitas.Datos
{
    public class CitaDAL
    {
        #region singleton
        private static readonly CitaDAL _instancia = new CitaDAL();
        public static CitaDAL Instancia
        {
            get { return CitaDAL._instancia; }
        }
        #endregion singleton

        #region metodos
        public Int32 GrabarCita(CitaDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Int16 PKCreado = 0;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_INSERTAR_CITA_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MEDICOID", item.MedicoId);
                cmd.Parameters.AddWithValue("@PACIENTEID", item.PacienteId);
                cmd.Parameters.AddWithValue("@FECHAATENCION", item.FechaAtencion);
                cmd.Parameters.AddWithValue("@INICIOATENCION", item.HoraInicio);
                cmd.Parameters.AddWithValue("@FINATENCION", item.HoraFin);
                cmd.Parameters.AddWithValue("@ESTADO", item.Estado);
                cmd.Parameters.AddWithValue("@USUARIOREGISTRO", usuario);
                cn.Open();
                PKCreado = Convert.ToInt16(cmd.ExecuteScalar());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return PKCreado;
        }

        public Boolean EditarCita(CitaDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Boolean modifico = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_EDITAR_CITA_SP", cn);
                cmd.Parameters.AddWithValue("@ID", item.Id);
                cmd.Parameters.AddWithValue("@MEDICOID", item.MedicoId);
                cmd.Parameters.AddWithValue("@PACIENTEID", item.PacienteId);
                cmd.Parameters.AddWithValue("@FECHAATENCION", item.FechaAtencion);
                cmd.Parameters.AddWithValue("@INICIOATENCION", item.HoraInicio);
                cmd.Parameters.AddWithValue("@FINATENCION", item.HoraFin);
                cmd.Parameters.AddWithValue("@ESTADO", item.Estado);
                cmd.Parameters.AddWithValue("@OBSERVACIONES", item.Observaciones);
                cmd.Parameters.AddWithValue("@ACTIVO", item.Activo);
                cmd.Parameters.AddWithValue("@USUARIOMODIFICACION", usuario);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0) { modifico = true; }
                return modifico;
            }
            catch (Exception e) { throw e; }
            finally { if (cmd != null) { cmd.Connection.Close(); } }
        }
        public List<CitaDTO> ListarCitasMedico(Int32 medicoId, DateTime fechaAtencion)
        {
            SqlCommand cmd = null;
            List<CitaDTO> lista = new List<CitaDTO>();
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_CITAS_MEDICO", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FECHAATENCION", fechaAtencion);
                cmd.Parameters.AddWithValue("@MEDICOID", medicoId);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    CitaDTO objCita = new CitaDTO();
                    objCita.Id = Convert.ToInt32(dr["ID"]);
                    objCita.MedicoId = Convert.ToInt32(dr["MEDICOID"]);
                    objCita.FechaAtencion = Convert.ToDateTime(dr["FECHAATENCION"]);
                    objCita.FechaAtencionInicio = Convert.ToDateTime(dr["FECHAATENCIONINICIO"]);
                    objCita.FechaAtencionFin = Convert.ToDateTime(dr["FECHAATENCIONFIN"]);
                    objCita.HoraInicio = TimeSpan.Parse(dr["INICIOATENCION"].ToString());
                    objCita.HoraFin = TimeSpan.Parse(dr["FINATENCION"].ToString());
                    objCita.Estado = dr["ESTADO"].ToString();
                    objCita.PacienteNombres = dr["NOMBRES"].ToString();
                    objCita.PacienteApellidos = dr["APELLIDOS"].ToString();
                    objCita.Activo = Convert.ToBoolean(dr["ACTIVO"]);
                    lista.Add(objCita);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return lista;
        }

        public CitaDTO ObtenerCita(Int32 citaId)
        {
            SqlCommand cmd = null;
            CitaDTO item = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_CITA_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", citaId);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    item = new CitaDTO();
                    item.Id = Convert.ToInt32(dr["ID"]);
                    item.MedicoId = Convert.ToInt32(dr["MEDICOID"]);
                    item.FechaAtencion = Convert.ToDateTime(dr["FECHAATENCION"]);
                    item.FechaAtencionInicio = Convert.ToDateTime(dr["FECHAATENCIONINICIO"]);
                    item.FechaAtencionFin = Convert.ToDateTime(dr["FECHAATENCIONFIN"]);
                    item.HoraInicio = TimeSpan.Parse(dr["INICIOATENCION"].ToString());
                    item.HoraFin = TimeSpan.Parse(dr["FINATENCION"].ToString());
                    item.PacienteId = Convert.ToInt32(dr["PACIENTEID"]);
                    item.PacienteNombres = dr["NOMBRES"].ToString();
                    item.PacienteApellidos = dr["APELLIDOS"].ToString();
                    item.PacienteDni = dr["DNI"].ToString();
                    item.Estado = dr["ESTADO"].ToString();
                    item.Observaciones = dr["OBSERVACIONES"].ToString();
                    item.Activo = Convert.ToBoolean(dr["ACTIVO"]);
                }
                return item;
            }
            catch (Exception e) { throw e; }
            finally { if (cmd != null) { cmd.Connection.Close(); } }
        }
        #endregion
    }
}
