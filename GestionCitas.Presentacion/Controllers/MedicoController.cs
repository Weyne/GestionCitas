using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionCitas.Logica;
using GestionCitas.Entidades;

namespace GestionCitas.Presentacion.Controllers
{
    public class MedicoController : Controller
    {
        //
        // GET: /Medico/
        public ActionResult Index()
        {
            return View();
        }

        #region CORE: Registrar Horarios
        public ActionResult VisualizarMedicos()
        {
            return View();
        }

        public ActionResult Horario(String medicoId)
        {
            if (medicoId == null || medicoId == "0")
            {
                return RedirectToAction("VisualizarMedicos");
            }
            else
            {
                ViewBag.medicoId = medicoId;
                MedicoDTO objMedico = MedicoBLL.Instancia.ObtenerMedico(Convert.ToInt32(medicoId));
                if (objMedico != null)
                {
                    ViewBag.Head = "Horario del médico: " + objMedico.Apellidos + " " + objMedico.Nombres;
                    ViewBag.Medico = objMedico.Apellidos + " " + objMedico.Nombres;
                }
                else ViewBag.Head = "Solicitud no valida";
            }
            return View();
        }

        public ActionResult GrabarHorario(HorarioDTO item)
        {
            var obj = HorarioBLL.Instancia.GrabarHorario(item, "prueba");
            return Json(new { obj });
        }
        public ActionResult ObtenerHorario(Int32 horarioId)
        {
            var obj = HorarioBLL.Instancia.ObtenerHorario(horarioId);
            return Json(new { obj });
        }
        public ActionResult ListarHorarioMedico(Int32 medicoId)
        {
            var items = (from temp in HorarioBLL.Instancia.ListarHorariosPorMedico(medicoId)
                         select new
                         {
                             horarioId = temp.Id,
                             title = "Horario Registrado",
                             start = temp.FechaAtencion.ToString("yyy-MM-dd") + "T" + (temp.HoraInicio.Hours < 10 ? "0" + temp.HoraInicio.Hours.ToString() : temp.HoraInicio.Hours.ToString()) + ":" + (temp.HoraInicio.Minutes < 10 ? "0" + temp.HoraInicio.Minutes.ToString() : temp.HoraInicio.Minutes.ToString()),
                             end = temp.FechaAtencion.ToString("yyy-MM-dd") + "T" + (temp.HoraFin.Hours < 10 ? "0" + temp.HoraFin.Hours.ToString() : temp.HoraFin.Hours.ToString()) + ":" + (temp.HoraFin.Minutes < 10 ? "0" + temp.HoraFin.Minutes.ToString() : temp.HoraFin.Minutes.ToString()),
                             allDay = false,
                             backgroundColor = "#00c0ef",
                             borderColor = "#00c0ef",
                         });
            return Json(items.ToList());
        }

        #endregion CORE: Registrar Horarios

        public ActionResult ListarMedicosActivos()
        {
            var items = (from temp in MedicoBLL.Instancia.ListarMedicos().Where(x => x.Activo == true)
                         select new
                         {
                             Id = temp.Id,
                             Nombres = temp.Nombres,
                             Apellidos = temp.Apellidos,
                             Dni = temp.Dni,
                             Telefono = temp.Telefono,
                             NumeroColegiatura = temp.NumeroColegiatura,
                             Activo = temp.Activo
                         });
            return Json(items.ToList());
        }
        public ActionResult ListarMedicos()
        {
            var items = (from temp in MedicoBLL.Instancia.ListarMedicos()
                         select new
                         {
                             Id = temp.Id,
                             Nombres = temp.Nombres,
                             Apellidos = temp.Apellidos,
                             Dni = temp.Dni,
                             Telefono = temp.Telefono,
                             NumeroColegiatura = temp.NumeroColegiatura,
                             Activo = temp.Activo
                         });
            return Json(items.ToList());
        }
        public ActionResult ObtenerMedico(Int32 medicoId)
        {
            var obj = MedicoBLL.Instancia.ObtenerMedico(medicoId);
            return Json(new { obj });
        }
        public ActionResult GrabarMedico(MedicoDTO item)
        {
            var obj = MedicoBLL.Instancia.GrabarMedico(item, "prueba");
            return Json(new { obj });
        }
        public ActionResult EliminarMedico(MedicoDTO item)
        {
            item = MedicoBLL.Instancia.ObtenerMedico(item.Id);
            var obj = MedicoBLL.Instancia.EliminarMedico(item, "prueba");
            return Json(new { obj });
        }
        public ActionResult BuscarEspecialidad(String cadena)
        {
            cadena = cadena.ToLower();
            List<EspecialidadDTO> lista = EspecialidadBLL.Instancia.ListarEspecialidades().Where(x => x.Activo == true && x.Nombre.ToLower().Contains(cadena)).ToList();
            var items = (from temp in lista
                         select new
                         {
                             Id = temp.Id,
                             Nombre = temp.Nombre,
                             Activo = temp.Activo
                         });
            return Json(items.ToList());
        }
	}
}