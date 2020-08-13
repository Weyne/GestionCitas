$(function () {
    var medicoId = "";
    var horario = {};
    var model = {

        ObtenerParametros: function () {
            medicoId = $("#medicoId").val();
        },

        ListarDatos: function () {

            util.Ajax("../ListarHorarioMedico", JSON.stringify({ medicoId: medicoId }),
            function (data) {
                var lista = [];
                var evento = {};
                if (data != null) {
                    lista = data;
                    console.log(lista);
                }
                else {
                    util.MsgAlert("No se encontraron registros");
                }
                $('#calendario').fullCalendar('addEventSource', lista)
            });
        },

        ObtenerHorario: function (id) {
            util.Ajax("../ObtenerHorario", JSON.stringify({ horarioId: id }),
            function (data) {
                var item = data.obj;
                model.LimpiarDatosFormulario();
                if (item != null) {
                    $("#txtIdentificador").val(item.Id);

                    var f = new Date(parseInt(item.FechaAtencion.substr(6)));
                    var d = f.getDate();
                    var m = f.getMonth();
                    if (d < 10) d = '0' + d;
                    if (m < 10) m = '0' + m;
                    $("#txtFechaAtencion").datepicker("option", "dateFormat", "dd/mm/yy");
                    $("#txtFechaAtencion").datepicker("setDate", new Date(f.getFullYear(), m, d));

                    $("#txtHoraInicio").val(util.FormatJsonTimeSpan(item.HoraInicio));
                    $("#txtHoraFin").val(util.FormatJsonTimeSpan(item.HoraFin));
                    $("#formularioRegistrar").dialog("open");
                }
            });
        },

        ConfigurarCalendario: function () {
            var date = new Date()
            var d = date.getDate(),
                m = date.getMonth(),
                y = date.getFullYear()
            $('#calendario').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                buttonText: {
                    today: 'Hoy',
                    month: 'Mes',
                    week: 'Semana',
                    day: 'Dia'
                },
                height: 500,
                editable: false,
                eventClick: function (calEvent, jsEvent, view) {
                    model.ObtenerHorario(calEvent.horarioId);
                }
            });
        },

        LimpiarDatosFormulario: function () {
            $("#txtIdentificador").val("-1");
            $("#txtFechaAtencion").val("");
            $("#txtHoraInicio").val("");
            $("#txtHoraFin").val("");
        },

        NuevoHorario: function () {
            model.LimpiarDatosFormulario();
            $("#formularioRegistrar").dialog("open");
        },

        Cancelar: function () {
            $("#formularioRegistrar").dialog("close");
        },

        RecogerDatosFormulario: function () {
            horario = {
                Id: $("#txtIdentificador").val(),
                MedicoId: $("#medicoId").val(),
                Medico: $("#txtMedico").val(),
                FechaAtencion: $("#txtFechaAtencion").val(),
                HoraInicio: $("#txtHoraInicio").val(),
                HoraFin: $("#txtHoraFin").val(),
                Activo: true
            };
        },

        Guardar: function () {
            model.RecogerDatosFormulario();

            util.Ajax("../GrabarHorario", JSON.stringify({ item: horario }),
            function (data) {
                var resultado = data.obj;

                if (resultado.Correcto == true) {
                    $('#calendario').fullCalendar('removeEvents');
                    model.ListarDatos();
                    model.LimpiarDatosFormulario();
                    if (horario.Id > 0) {
                        util.MsgInfo(resultado.Mensaje);
                    }
                    else {
                        util.MsgInfo(resultado.Mensaje);
                    }
                    $("#formularioRegistrar").dialog("close");
                }
                else {
                    util.MsgAlert(resultado.Mensaje);
                }
            });
        },

        MarcarMenu: function () {

            $("#otpGestionPersonal")
                .addClass("active")
                .addClass("menu-open");

            $("#optListarMedicos")
                .addClass("active");
        },

        Inicio: function () {
            this.MarcarMenu();

            this.ObtenerParametros();

            $("#btnNuevoHorario").button();
            $("#btnNuevoHorario").on("click", this.NuevoHorario);

            $("#btnGuardar").button();
            $("#btnGuardar").on("click", this.Guardar);

            $("#btnCancelar").button();
            $("#btnCancelar").on("click", this.Cancelar);

            $('#txtFechaAtencion').datepicker({ autoclose: true });

            util.Dialog("formularioRegistrar", 'Horario', 600, 'auto', 10, null, false);

            this.ConfigurarCalendario();
            this.ListarDatos();
            $("#contenidoPagina").css('display', 'block');
        }
    }

    model.Inicio();
});