using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionCitas.Logica; //importamos la capa lógica
using GestionCitas.Entidades; //importamos la capa entidades

namespace GestionCitas.Presentacion.Controllers
{
    public class EspecialidadController : Controller
    {
        //
        // GET: /Especialidad/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListarEspecialidades()
        {
            var items = (from temp in EspecialidadBLL.Instancia.ListarEspecialidades()
                         select new
                         {
                             Id = temp.Id,
                             Nombre = temp.Nombre,
                             Descripcion = temp.Descripcion,
                             Activo = temp.Activo
                         });
            return Json(items.ToList());
        }

        public ActionResult ObtenerEspecialidad(Int16 especialidadId)
        {
            var obj = EspecialidadBLL.Instancia.ObtenerEspecialidad(especialidadId);
            return Json(new { obj });
        }
        public ActionResult GrabarEspecialidad(EspecialidadDTO item)
        {
            var obj = EspecialidadBLL.Instancia.GrabarEspecialidad(item, "prueba");
            return Json(new { obj });
        }
	}
}