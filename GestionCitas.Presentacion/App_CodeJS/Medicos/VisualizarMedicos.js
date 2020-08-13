$(function () {
    var model = {

        //#region Listado

        CargarLista: function () {
            util.Ajax("ListarMedicosActivos", JSON.stringify({}),
            function (data) {
                var lista = [];
                if (data != null) {
                    lista = data;
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
                    , width: '25%'
                    , cellTemplate: function (container, options) {
                        var apellidos = options.value;
                        var label = $('<div />')
                            .css('word-wrap', 'break-word')
                            .text(apellidos)
                            .attr('title', apellidos)
                            .appendTo(container)
                            .parents("td")
                            .css('white-space', 'initial');
                    }
                },
                {
                    dataField: 'Nombres'
                    , caption: 'Nombres'
                    , width: '25%'
                    , cellTemplate: function (container, options) {
                        var nombres = options.value;
                        var label = $('<div />')
                            .css('word-wrap', 'break-word')
                            .text(nombres)
                            .attr('title', nombres)
                            .appendTo(container)
                            .parents("td")
                            .css('white-space', 'initial');
                    }
                },
                { dataField: 'Dni', caption: 'DNI', width: '15%', alignment: 'center' },
                { dataField: 'Telefono', caption: 'Telefono', width: '15%', alignment: 'center' },
                { dataField: 'NumeroColegiatura', caption: 'N° Colegiatura', width: '10%', alignment: 'center' },
                {
                    dataField: 'Id',
                    caption: 'Acciones',
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

                        var link = $('<a />')
                            .attr('title', 'Ver Horario de Medico')
                            .on('click', function () { model.VerHorario(id) })
                            .css('cursor', 'pointer')
                            .css('margin-right', '5px')
                            .appendTo(container);

                        $("<i />")
                            .attr('class', 'fa fa-fw fa-eye')
                            .appendTo(link);
                    }
                }
            ],
            $(window).height() - 280, true, true, false);

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

            var grilla = $("#grilla").dxDataGrid("instance");

            if (grilla != null && $(window).height() >= 490) {
                grilla.option({ height: $(window).height() - 280 });
                grilla.resize();
            }

        },

        VerHorario: function (medicoId) {
            window.location.href = "Horario/" + medicoId;
        },

        //#endregion Listado

        MarcarMenu: function () {

            $("#otpGestionPersonal")
                .addClass("active")
                .addClass("menu-open");

            $("#optListarMedicos")
                .addClass("active");
        },

        Inicio: function () {

            this.MarcarMenu();

            $("#div-body").css("height", 140);

            this.ConfigurarGrilla();
            this.CargarLista();

            $("#contenidoPagina").css('display', 'block');
        }
    }

    model.Inicio();

    window.onresize = function () {
        var grilla = $("#grilla").dxDataGrid("instance");

        if (grilla != null && $(window).height() >= 490) {
            grilla.option({ height: $(window).height() - 280 });
            grilla.resize();
        }
    }
});