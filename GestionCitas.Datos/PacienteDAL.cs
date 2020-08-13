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
    public class PacienteDAL
    {
        #region singleton
        private static readonly PacienteDAL _instancia = new PacienteDAL();
        public static PacienteDAL Instancia
        {
            get { return PacienteDAL._instancia; }
        }
        #endregion singleton

        #region metodos
        public List<PacienteDTO> ListarPacientes()
        {
            SqlCommand cmd = null;
            List<PacienteDTO> lista = new List<PacienteDTO>();
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_PACIENTES_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    PacienteDTO objPaciente = new PacienteDTO();
                    objPaciente.Id = Convert.ToInt16(dr["ID"]);
                    objPaciente.Nombres = dr["NOMBRES"].ToString();
                    objPaciente.Apellidos = dr["APELLIDOS"].ToString();
                    objPaciente.Dni = dr["DNI"].ToString();
                    objPaciente.Direccion = dr["DIRECCION"].ToString();
                    objPaciente.Telefono = dr["TELEFONO"].ToString();
                    objPaciente.Sexo = dr["SEXO"].ToString();
                    objPaciente.FechaNacimiento = Convert.ToDateTime(dr["FECHANACIMIENTO"].ToString());
                    objPaciente.Activo = Convert.ToBoolean(dr["ACTIVO"]);
                    lista.Add(objPaciente);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return lista;
        }

        public Int32 GrabarPaciente(PacienteDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Int16 PKCreado = 0;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_INSERTAR_PACIENTE_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NOMBRES", item.Nombres);
                cmd.Parameters.AddWithValue("@APELLIDOS", item.Apellidos);
                cmd.Parameters.AddWithValue("@DNI", item.Dni);
                cmd.Parameters.AddWithValue("@DIRECCION", item.Direccion);
                cmd.Parameters.AddWithValue("@TELEFONO", item.Telefono);
                cmd.Parameters.AddWithValue("@SEXO", item.Sexo);
                cmd.Parameters.AddWithValue("@FECHANACIMIENTO", item.FechaNacimiento);
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

        public Boolean EditarPaciente(PacienteDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Boolean modifico = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_EDITAR_PACIENTE_SP", cn);
                cmd.Parameters.AddWithValue("@ID", item.Id);
                cmd.Parameters.AddWithValue("@NOMBRES", item.Nombres);
                cmd.Parameters.AddWithValue("@APELLIDOS", item.Apellidos);
                cmd.Parameters.AddWithValue("@DNI", item.Dni);
                cmd.Parameters.AddWithValue("@DIRECCION", item.Direccion);
                cmd.Parameters.AddWithValue("@TELEFONO", item.Telefono);
                cmd.Parameters.AddWithValue("@SEXO", item.Sexo);
                cmd.Parameters.AddWithValue("@FECHANACIMIENTO", item.FechaNacimiento);
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

        public PacienteDTO ObtenerPaciente(Int32 medicoId)
        {
            SqlCommand cmd = null;
            PacienteDTO item = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_PACIENTE_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", medicoId);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    item = new PacienteDTO();
                    item.Id = Convert.ToInt16(dr["ID"]);
                    item.Nombres = dr["NOMBRES"].ToString();
                    item.Apellidos = dr["APELLIDOS"].ToString();
                    item.Dni = dr["DNI"].ToString();
                    item.Direccion = dr["DIRECCION"].ToString();
                    item.Telefono = dr["TELEFONO"].ToString();
                    item.Sexo = dr["SEXO"].ToString();
                    item.FechaNacimiento = Convert.ToDateTime(dr["FECHANACIMIENTO"].ToString());
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
