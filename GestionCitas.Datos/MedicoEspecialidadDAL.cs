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
    public class MedicoEspecialidadDAL
    {
        #region singleton
        private static readonly MedicoEspecialidadDAL _instancia = new MedicoEspecialidadDAL();
        public static MedicoEspecialidadDAL Instancia
        {
            get { return MedicoEspecialidadDAL._instancia; }
        }
        #endregion singleton

        #region metodos
        public List<MedicoEspecialidadDTO> ListarEspecialidadesPorMedico(Int32 medicoId)
        {
            SqlCommand cmd = null;
            List<MedicoEspecialidadDTO> lista = new List<MedicoEspecialidadDTO>();
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_MEDICOS_ESPECIALIDADES_POR_MEDICO_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MEDICOID", medicoId);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    MedicoEspecialidadDTO objEspecialidad = new MedicoEspecialidadDTO();
                    objEspecialidad.Id = Convert.ToInt16(dr["ID"]);
                    objEspecialidad.EspecialidadId = Convert.ToInt16(dr["ESPECIALIDADID"]);
                    objEspecialidad.Especialidad = dr["NOMBRE"].ToString();
                    objEspecialidad.EspecialidadDescripcion = dr["DESCRIPCION"].ToString();
                    objEspecialidad.Activo = Convert.ToBoolean(dr["ACTIVO"]);
                    lista.Add(objEspecialidad);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return lista;
        }

        public Boolean GrabarEspecialidadDeMedico(MedicoEspecialidadDTO item, Int32 medicoId, String usuario)
        {
            SqlCommand cmd = null;
            Int16 PKCreado = 0;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_INSERTAR_MEDICOS_ESPECIALIDADES_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MEDICOID", medicoId);
                cmd.Parameters.AddWithValue("@ESPECIALIDADID", item.EspecialidadId);
                cmd.Parameters.AddWithValue("@USUARIOREGISTRO", usuario);
                cn.Open();
                PKCreado = Convert.ToInt16(cmd.ExecuteScalar());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return (PKCreado > 0);
        }

        public Boolean EditarEspecialidad(MedicoEspecialidadDTO item, Int32 medicoId, String usuario)
        {
            SqlCommand cmd = null;
            Boolean modifico = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_EDITAR_MEDICOS_ESPECIALIDADES_SP", cn);
                cmd.Parameters.AddWithValue("@ID", item.Id);
                cmd.Parameters.AddWithValue("@MEDICOID", medicoId);
                cmd.Parameters.AddWithValue("@ESPECIALIDADID", item.EspecialidadId);
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

        #endregion
    }
}
