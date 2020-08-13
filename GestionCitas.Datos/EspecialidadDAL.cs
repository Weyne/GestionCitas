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
    public class EspecialidadDAL
    {
        #region patron singleton
        private static readonly EspecialidadDAL _instancia = new EspecialidadDAL();
        public static EspecialidadDAL Instancia
        {
            get { return EspecialidadDAL._instancia; }
        }
        #endregion

        #region metodos
        public List<EspecialidadDTO> ListarEspecialidades()
        {
            SqlCommand cmd = null;
            List<EspecialidadDTO> lista = new List<EspecialidadDTO>();
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_ESPECIALIDADES_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    EspecialidadDTO objEspecialidad = new EspecialidadDTO();
                    objEspecialidad.Id = Convert.ToInt16(dr["ID"]);
                    objEspecialidad.Nombre = dr["NOMBRE"].ToString();
                    objEspecialidad.Descripcion = dr["DESCRIPCION"].ToString();
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
        public EspecialidadDTO ObtenerEspecialidad(Int32 especialidadId)
        {
            SqlCommand cmd = null;
            EspecialidadDTO item = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_ESPECIALIDAD_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", especialidadId);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    item = new EspecialidadDTO();
                    item.Id = Convert.ToInt16(dr["ID"]);
                    item.Nombre = dr["NOMBRE"].ToString();
                    item.Descripcion = dr["DESCRIPCION"].ToString();
                    item.Activo = Convert.ToBoolean(dr["ACTIVO"]);
                }
                return item;
            }
            catch (Exception e) { throw e; }
            finally { if (cmd != null) { cmd.Connection.Close(); } }
        }

        public Int32 GrabarEspecialidad(EspecialidadDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Int16 PKCreado = 0;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_INSERTAR_ESPECIALIDAD_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NOMBRE", item.Nombre);
                cmd.Parameters.AddWithValue("@DESCRIPCION", item.Descripcion);

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

        public Boolean EditarEspecialidad(EspecialidadDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Boolean modifico = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_EDITAR_ESPECIALIDAD_SP", cn);
                cmd.Parameters.AddWithValue("@ID", item.Id);
                cmd.Parameters.AddWithValue("@NOMBRE", item.Nombre);
                cmd.Parameters.AddWithValue("@DESCRIPCION", item.Descripcion);
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
