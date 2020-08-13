$(function () {
    var especialidad = {};
    var model = {

        Guardar: function () {
            model.RecogerDatosFormulario();

            util.Ajax("GrabarEspecialidad", JSON.stringify({ item: especialidad }),
            function (data) {
                var resultado = data.obj;

                if (resultado.Correcto == true) {
                    model.CargarLista();
                    model.LimpiarDatosFormulario();
                    $("#formularioRegistrar").dialog("close");
                    if (especialidad.Id > 0) {
                        util.MsgInfo(resultado.Mensaje);
                    }
                    else {
                        util.MsgInfo(resultado.Mensaje);
                    }
                    
                }
                else {
                    util.MsgAlert(resultado.Mensaje);
                }
            });
        },

        RecogerDatosFormulario: function () {
            especialidad = {
                Id: $("#txtIdentificador").val(),
                Nombre: $("#txtNombre").val(),
                Descripcion: $("#txtDescripcion").val(),
                Activo: ($("#chkActivo").prop('checked') ? true : false)
            };
        },

        CargarLista: function () {
            util.Ajax("ListarEspecialidades", JSON.stringify({}),
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
                    dataField: 'Nombre'
                    , caption: 'Nombre'
                    , width: '20%'
                    , cellTemplate: function (container, options) {
                        var nombre = options.value;
                        var label = $('<div />')
                            .css('word-wrap', 'break-word')
                            .text(nombre)
                            .attr('title', nombre)
                            .appendTo(container)
                            .parents("td")
                            .css('white-space', 'initial');
                    }
                },
                {
                    dataField: 'Descripcion', caption: 'Descripción'
                    , columnAutoWidth: true
                    , width: '60%'
                    , cellTemplate: function (container, options) {
                        var descripcion = options.value;
                        var label = $('<div />')
                            .css('word-wrap', 'break-word')
                            .text(descripcion)
                            .attr('title', descripcion)
                            .appendTo(container)
                            .parents("td")
                            .css('white-space', 'initial');
                    }
                },
                {
                    dataField: 'Activo',
                    caption: 'Estado',
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

                        var activo = options.value;
                        if (activo == true) {
                            $("<label />")
                                .text("Activo")
                                .css('font-weight', 'normal')
                                .appendTo(container);
                        }
                        else {
                            $("<div />")
                                .text("Inactivo")
                                .css('font-weight', 'normal')
                                .appendTo(container);
                        }
                    }
                },
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
                        var estado = options.row.data.Flujo;

                        var link = $('<a />')
                            .attr('title', 'Editar Especialidad')
                            .on('click', function () { model.Obtener(id) })
                            .css('cursor', 'pointer')
                            .css('margin-right', '5px')
                            .appendTo(container);

                        $("<i />")
                            .attr('class', 'fa fa-fw fa-pencil')
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

        },

        LimpiarDatosFormulario: function () {
            $("#txtNombre").val("");
            $("#txtDescripcion").val("");
            $("#txtIdentificador").val("-1");
            $("#chkActivo").prop('checked', true)
        },

        Nuevo: function () {
            model.LimpiarDatosFormulario();
            $("#formularioRegistrar").dialog("open");
        },

        Cancelar: function () {
            $("#formularioRegistrar").dialog("close");
        },

        Obtener: function (id) {

            util.Ajax("ObtenerEspecialidad", JSON.stringify({ especialidadId: id }),
            function (data) {
                var item = data.obj;
                if (item != null) {
                    $("#txtIdentificador").val(item.Id);
                    $("#txtNombre").val(item.Nombre);
                    $("#txtDescripcion").val(item.Descripcion);
                    $("#chkActivo").prop('checked', item.Activo);
                    $("#formularioRegistrar").dialog("open");
                }
            });
        },

        MarcarMenu: function () {

            $("#optMantenimiento")
                .addClass("active")
                .addClass("menu-open");

            $("#optEspecialidades")
                .addClass("active");
        },

        Inicio: function () {
            this.MarcarMenu();

            $("#btnNuevo").button();
            $("#btnNuevo").on("click", this.Nuevo);

            $("#btnCancelar").button();
            $("#btnCancelar").on("click", this.Cancelar);

            $("#btnGuardar").button();
            $("#btnGuardar").on("click", this.Guardar);

            util.Dialog("formularioRegistrar", 'Especialidad', 500, 450, 10, null, false);

            this.ConfigurarGrilla();
            this.CargarLista();
            $("#contenidoPagina").css('display', 'block');
        }
    }

    model.Inicio();
});