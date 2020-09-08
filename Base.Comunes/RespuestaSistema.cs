using System;

namespace Base.Comunes
{
    public class RespuestaSistema
    {
        public String Mensaje { get; set; }
        public Boolean Correcto { get; set; }
        public Int32 LlaveInsertada { get; set; }
        public Int32 LlaveModificada { get; set; }
        public Int16 Accion { get; set; }
    }
}
