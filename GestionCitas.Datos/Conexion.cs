using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; //agregamos la referencia para trabajar con sql
using System.Data.SqlClient;//agregamos la referencia para trabajar con sql

namespace GestionCitas.Datos
{
    public class Conexion
    {
        #region singleton
        /*
         * Este bloque de código nos permite instanciar una sola vez a la clase y poder
         * reutilizarla en diferentes partes de nuestro código, ya que internamente
         * se inicializa una sola vez.
         */
        private static readonly Conexion _instancia = new Conexion();
        public static Conexion Instancia
        {
            get { return Conexion._instancia; }
        }
        #endregion singleton

        #region metodos

        /*
         *esta función nos devuelve la conexión a nuestra BD,
         *en este caso nos conectaremos con la seguridad integrada de sql
         *a la base de datos 'CLINICA_ODONTOLOGICA'.
         */
        public SqlConnection conectar()
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = "Data Source=.; Initial Catalog = CLINICA_ODONTOLOGICA;" +
                                   "Integrated Security=true";
            return cn;
        }
        #endregion metodos
    }
}
