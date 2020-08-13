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
    public class HorarioDAL
    {
        #region singleton
        private static readonly HorarioDAL _instancia = new HorarioDAL();
        public static HorarioDAL Instancia
        {
            get { return HorarioDAL._instancia; }
        }
        #endregion singleton

        #region metodos
        public Int32 GrabarHorario(HorarioDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Int16 PKCreado = 0;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_INSERTAR_HORARIO_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MEDICOID", item.MedicoId);
                cmd.Parameters.AddWithValue("@FECHAATENCION", item.FechaAtencion);
                cmd.Parameters.AddWithValue("@INICIOATENCION", item.HoraInicio);
                cmd.Parameters.AddWithValue("@FINATENCION", item.HoraFin);
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

        public Boolean EditarHorario(HorarioDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Boolean modifico = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_EDITAR_HORARIO_SP", cn);
                cmd.Parameters.AddWithValue("@ID", item.Id);
                cmd.Parameters.AddWithValue("@MEDICOID", item.MedicoId);
                cmd.Parameters.AddWithValue("@FECHAATENCION", item.FechaAtencion);
                cmd.Parameters.AddWithValue("@INICIOATENCION", item.HoraInicio);
                cmd.Parameters.AddWithValue("@FINATENCION", item.HoraFin);
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
        public List<HorarioDTO> ListarHorariosPorMedico(Int32 medicoId)
        {
            SqlCommand cmd = null;
            List<HorarioDTO> lista = new List<HorarioDTO>();
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_HORARIOS_POR_MEDICO_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MEDICOID", medicoId);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    HorarioDTO objHorario = new HorarioDTO();
                    objHorario.Id = Convert.ToInt16(dr["ID"]);
                    objHorario.MedicoId = Convert.ToInt32(dr["MEDICOID"]);
                    objHorario.FechaAtencion = Convert.ToDateTime(dr["FECHAATENCION"]);
                    objHorario.HoraInicio = TimeSpan.Parse(dr["INICIOATENCION"].ToString());
                    objHorario.HoraFin = TimeSpan.Parse(dr["FINATENCION"].ToString());
                    objHorario.Activo = Convert.ToBoolean(dr["ACTIVO"]);
                    lista.Add(objHorario);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return lista;
        }

        public HorarioDTO ObtenerHorario(Int32 horarioId)
        {
            SqlCommand cmd = null;
            HorarioDTO item = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_HORARIO_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", horarioId);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    item = new HorarioDTO();
                    item.Id = Convert.ToInt32(dr["ID"]);
                    item.MedicoId = Convert.ToInt32(dr["MEDICOID"]);
                    item.FechaAtencion = Convert.ToDateTime(dr["FECHAATENCION"]);
                    item.HoraInicio = TimeSpan.Parse(dr["INICIOATENCION"].ToString());
                    item.HoraFin = TimeSpan.Parse(dr["FINATENCION"].ToString());
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
