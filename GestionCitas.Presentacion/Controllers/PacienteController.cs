using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionCitas.Logica;
using GestionCitas.Entidades;

namespace GestionCitas.Presentacion.Controllers
{
    public class PacienteController : Controller
    {
        //
        // GET: /Paciente/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListarPacientes()
        {
            var items = (from temp in PacienteBLL.Instancia.ListarPacientes().Where(x => x.Activo == true).ToList()
                         select new
                         {
                             Id = temp.Id,
                             Nombres = temp.Nombres,
                             Apellidos = temp.Apellidos,
                             Dni = temp.Dni,
                             Telefono = temp.Telefono,
                             Activo = (temp.Activo ? "Activo" : "Inactivo")
                         });
            return Json(items.ToList());
        }
        public ActionResult ObtenerPaciente(Int16 medicoId)
        {
            var obj = PacienteBLL.Instancia.ObtenerPaciente(medicoId);
            return Json(new { obj });
        }
        public ActionResult GrabarPaciente(PacienteDTO item)
        {
            var obj = PacienteBLL.Instancia.GrabarPaciente(item, "prueba");
            return Json(new { obj });
        }
        public ActionResult EliminarPaciente(PacienteDTO item)
        {
            item = PacienteBLL.Instancia.ObtenerPaciente(item.Id);
            var obj = PacienteBLL.Instancia.EliminarPaciente(item, "prueba");
            return Json(new { obj });
        }
	}
}