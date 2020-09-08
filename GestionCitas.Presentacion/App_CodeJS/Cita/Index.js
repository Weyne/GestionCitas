$(function () {
    var opcionBusqueda = "",
        fechaAtencion = null,
        fechaConsulta = null;
        filtro = "";
    var model = {

        Buscar: function () {

            opcionBusqueda = $('input[name=rbFiltro]:checked').val();
            fechaAtencion = util.GetDate($("#txtFechaAtencion").val());
            filtro = $("#txtFiltro").val();

            console.log(opcionBusqueda);

            if (opcionBusqueda == null || opcionBusqueda == '') {
                util.MsgAlert("Selecciona una opción de busqueda");
                return;
            }
            if (fechaAtencion == null) {
                util.MsgAlert("ingresa una fecha de atención");
                return;
            }
            if (filtro == null || filtro == '') {
                filtro = '';
            }
            model.ConfigurarGrilla();
            util.Ajax("ListarMedicosPorHorario", JSON.stringify({ opcionBusqueda: opcionBusqueda, fechaAtencion: fechaAtencion, filtro: filtro }),
            function (data) {
                var lista = [];
                if (data != null) {
                    lista = data;
                    fechaConsulta = $("#txtFechaAtencion").val();
                }
                else {
                    util.MsgAlert("No se encontraron registros");
                }
                $("#grilla").dxDataGrid("instance").option({ 'dataSource': lista });
            });
        },

        ConfigurarGrilla: function () {
            util.Grid("grilla",
            [
                {
                    dataField: 'Apellidos'
                    , caption: 'Apellidos'
                    , width: '30%'
                },
                {
                    dataField: 'Nombres', caption: 'Nombres'
                    , columnAutoWidth: true
                    , width: '60%'
                },
                {
                    dataField: 'Id',
                    caption: 'Ver Horario',
                    width: '10%',
                    dataType: 'string',
                    allowEditing: false,
                    allowFiltering: false,
                    allowGrouping: false,
                    allowHiding: false,
                    allowReordering: false,
                    allowSorting: false,
                    alignment: 'center',
                    cellTemplate: function (container, options) {

                        var id = options.value;
                        var estado = options.row.data.Flujo;

                        var link = $('<a />')
                            .attr('title', 'Ver Horario')
                            .on('click', function () { model.MostrarHorario(id, fechaConsulta) })
                            .css('cursor', 'pointer')
                            .css('margin-right', '5px')
                            .appendTo(container);

                        $("<i />")
                            .attr('class', 'fa fa-fw fa-eye')
                            .appendTo(link);
                    }
                }
            ],
            $(window).height() - 400, true, true, false);

            $("#grilla").dxDataGrid("instance").option({ selection: { mode: 'none' } });
            $("#grilla").dxDataGrid("instance").option({
                pager: {
                    showInfo: true,
                    infoText: 'Página {0} de {1} ({2} registros)',
                    showNavigationButtons: true,
                    showPageSizeSelector: false,
                    visible: true
                },
                paging: {
                    pageSize: 30,
                    enabled: true
                }
            });

        },

        MostrarHorario: function (medicoId, fechaAtencion) {
            fechaAtencion = fechaAtencion.split("/")
            window.location.href = "Horario/" + medicoId + "/" + (fechaAtencion[2] + fechaAtencion[1] + fechaAtencion[0]);
        },

        MarcarMenu: function () {

            $("#otpGestionCita")
                .addClass("active")
                .addClass("menu-open");

            $("#optRegistrarCita")
                .addClass("active");
        },

        Inicio: function () {
            this.MarcarMenu();

            $("#btnBuscar").button();
            $("#btnBuscar").on("click", this.Buscar);

            $('#txtFechaAtencion').datepicker({ autoclose: true });

            $("#contenidoPagina").css('display', 'block');
        }
    }

    model.Inicio();
});