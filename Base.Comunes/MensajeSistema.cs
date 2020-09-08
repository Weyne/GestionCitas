using System;

namespace Base.Comunes
{
    public sealed class MensajeSistema
    {
        public const String ERROR_DELETE = "No se pudo eliminar el registro.";
        public const String ERROR_SAVE = "No se pudo guardar el registro.";
        public const String ERROR_UPDATE = "No se pudo actualizar el registro.";
        public const String OK_DELETE = "El registro se elimino correctamente.";
        public const String OK_SAVE = "El registro se guardo correctamente.";
        public const String OK_UPDATE = "El registro se actualizo correctamente.";
        public const Int16 ACTION_INSERT = 1;
        public const Int16 ACTION_UPDATE = 2;
        public const Int16 ACTION_DELETE = 3;
    }
}
