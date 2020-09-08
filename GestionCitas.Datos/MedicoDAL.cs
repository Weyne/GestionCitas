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
    public class MedicoDAL
    {
        #region singleton
        private static readonly MedicoDAL _instancia = new MedicoDAL();
        public static MedicoDAL Instancia
        {
            get { return MedicoDAL._instancia; }
        }
        #endregion singleton

        #region metodos

        public List<MedicoDTO> ListarMedicos()
        {
            SqlCommand cmd = null;
            List<MedicoDTO> lista = new List<MedicoDTO>();
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_MEDICOS_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    MedicoDTO objMedico = new MedicoDTO();
                    objMedico.Id = Convert.ToInt16(dr["ID"]);
                    objMedico.Nombres = dr["NOMBRES"].ToString();
                    objMedico.Apellidos = dr["APELLIDOS"].ToString();
                    objMedico.Dni = dr["DNI"].ToString();
                    objMedico.Direccion = dr["DIRECCION"].ToString();
                    objMedico.Correo = dr["CORREO"].ToString();
                    objMedico.Telefono = dr["TELEFONO"].ToString();
                    objMedico.Sexo = dr["SEXO"].ToString();
                    objMedico.NumeroColegiatura = dr["NUMCOLEGIATURA"].ToString();
                    objMedico.FechaNacimiento = Convert.ToDateTime(dr["FECHANACIMIENTO"].ToString());
                    objMedico.Activo = Convert.ToBoolean(dr["ACTIVO"]);
                    lista.Add(objMedico);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return lista;
        }

        public Int32 GrabarMedico(MedicoDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Int16 PKCreado = 0;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_INSERTAR_MEDICO_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NOMBRES", item.Nombres);
                cmd.Parameters.AddWithValue("@APELLIDOS", item.Apellidos);
                cmd.Parameters.AddWithValue("@DNI", item.Dni);
                cmd.Parameters.AddWithValue("@DIRECCION", item.Direccion);
                cmd.Parameters.AddWithValue("@CORREO", item.Correo);
                cmd.Parameters.AddWithValue("@TELEFONO", item.Telefono);
                cmd.Parameters.AddWithValue("@SEXO", item.Sexo);
                cmd.Parameters.AddWithValue("@NUMCOLEGIATURA", item.NumeroColegiatura);
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

        public Boolean EditarMedico(MedicoDTO item, String usuario)
        {
            SqlCommand cmd = null;
            Boolean modifico = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_EDITAR_MEDICO_SP", cn);
                cmd.Parameters.AddWithValue("@ID", item.Id);
                cmd.Parameters.AddWithValue("@NOMBRES", item.Nombres);
                cmd.Parameters.AddWithValue("@APELLIDOS", item.Apellidos);
                cmd.Parameters.AddWithValue("@DNI", item.Dni);
                cmd.Parameters.AddWithValue("@DIRECCION", item.Direccion);
                cmd.Parameters.AddWithValue("@CORREO", item.Correo);
                cmd.Parameters.AddWithValue("@TELEFONO", item.Telefono);
                cmd.Parameters.AddWithValue("@SEXO", item.Sexo);
                cmd.Parameters.AddWithValue("@NUMCOLEGIATURA", item.NumeroColegiatura);
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

        public MedicoDTO ObtenerMedico(Int32 medicoId)
        {
            SqlCommand cmd = null;
            MedicoDTO item = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_LEER_MEDICO_SP", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", medicoId);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    item = new MedicoDTO();
                    item.Id = Convert.ToInt16(dr["ID"]);
                    item.Nombres = dr["NOMBRES"].ToString();
                    item.Apellidos = dr["APELLIDOS"].ToString();
                    item.Dni = dr["DNI"].ToString();
                    item.Direccion = dr["DIRECCION"].ToString();
                    item.Correo = dr["CORREO"].ToString();
                    item.Telefono = dr["TELEFONO"].ToString();
                    item.Sexo = dr["SEXO"].ToString();
                    item.NumeroColegiatura = dr["NUMCOLEGIATURA"].ToString();
                    item.FechaNacimiento = Convert.ToDateTime(dr["FECHANACIMIENTO"].ToString());
                    item.Activo = Convert.ToBoolean(dr["ACTIVO"]);
                    item.ListaEspecialidades = MedicoEspecialidadDAL.Instancia.ListarEspecialidadesPorMedico(medicoId).Where(x => x.Activo == true).ToList();
                }
                return item;
            }
            catch (Exception e) { throw e; }
            finally { if (cmd != null) { cmd.Connection.Close(); } }
        }

        public List<MedicoDTO> ListarMedicosPorHorario(String opcionBusqueda, DateTime fechaAtencion, String filtro)
        {
            SqlCommand cmd = null;
            List<MedicoDTO> lista = new List<MedicoDTO>();
            try
            {
                SqlConnection cn = Conexion.Instancia.conectar();
                cmd = new SqlCommand("GC_BUSCAR_HORARIO", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TIPOBUSQUEDA", opcionBusqueda);
                cmd.Parameters.AddWithValue("@FECHAATENCION", fechaAtencion);
                cmd.Parameters.AddWithValue("@FILTRO", filtro);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    MedicoDTO objMedico = new MedicoDTO();
                    objMedico.Id = Convert.ToInt16(dr["MEDICOID"]);
                    objMedico.Nombres = dr["NOMBRES"].ToString();
                    objMedico.Apellidos = dr["APELLIDOS"].ToString();
                    lista.Add(objMedico);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return lista;
        }
        #endregion
    }
}
