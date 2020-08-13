$(function () {
    var medico = {};
    var EliminarFilaSeleccionada = "";
    var indiceEspecialidad = 0,
        medicoIdEliminar = 0;
    var listaEspecialidades = new Array(),
        listaEspecialidadesEliminadas = new Array();
    var model = {

        //#region Listado


        RecogerDatosFormulario: function () {

            listaEspecialidades = [];

            $("#tbListadoEspecialidades tr").each(function (index, fila) {
                listaEspecialidades.push({
                    Id: $(fila).find("input[name=txtId]").val(),
                    EspecialidadId: $(fila).find("input[name=txtEspecialidadId]").val(),
                    Especialidad: $(fila).find("input[name=txtEspecialidad]").val(),
                    EspecialidadDescripcion: '',
                    Activo: true,
                });
            });

            listaEspecialidades = listaEspecialidades.concat(listaEspecialidadesEliminadas);

            var fechaNacimiento = util.GetDate($("#txtFechaNacimiento").val());

            medico = {
                Id: $("#txtIdentificador").val(),
                Nombres: $("#txtNombres").val(),
                Apellidos: $("#txtApellidos").val(),
                Dni: $("#txtDni").val(),
                Direccion: $("#txtDireccion").val(),
                Correo: $("#txtCorreo").val(),
                Telefono: $("#txtTelefono").val(),
                Sexo: $('input[name=rbSexo]:checked').val(),
                NumeroColegiatura: $("#txtNumColegiatura").val(),
                FechaNacimiento: (fechaNacimiento == null) ? '' : fechaNacimiento,
                Activo: true,
                ListaEspecialidades: listaEspecialidades
            };
        },

        CargarLista: function () {
            util.Ajax("ListarMedicos", JSON.stringify({}),
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
                            .attr('title', 'Editar Medico')
                            .on('click', function () { model.Obtener(id) })
                            .css('cursor', 'pointer')
                            .css('margin-right', '5px')
                            .appendTo(container);

                        $("<i />")
                            .attr('class', 'fa fa-fw fa-pencil')
                            .appendTo(link);

                        var link_eliminar = $('<a />')
                            .attr('title', 'Eliminar Medico')
                            .on('click', function () { model.EliminarMedico(id) })
                            .css('cursor', 'pointer')
                            .css('margin-right', '5px')
                            .appendTo(container);

                        $("<i />")
                            .attr('class', 'fa fa-fw fa-trash')
                            .appendTo(link_eliminar);
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

        EliminarMedico: function (id) {
            $("#formularioEliminarMedico").dialog("open");
            medicoIdEliminar = id;
        },

        Eliminar: function () {
            medico = {
                Id: medicoIdEliminar,
                Nombres: '',
                Apellidos: '',
                Dni: '',
                Direccion: '',
                Correo: '',
                Telefono: '',
                Sexo: '',
                NumeroColegiatura: '',
                FechaNacimiento: '',
                Activo: false,
                ListaEspecialidades: null
            };

            util.Ajax("EliminarMedico", JSON.stringify({ item: medico }),
            function (data) {
                var resultado = data.obj;

                if (resultado.Correcto == true) {
                    model.CargarLista();
                    util.MsgInfo(resultado.Mensaje);
                }
                else {
                    util.MsgAlert(resultado.Mensaje);
                }
                medicoIdEliminar = 0;
                $("#formularioEliminarMedico").dialog("close");
            });
        },

        CancelarEliminarMedico: function () {
            medicoIdEliminar = 0;
            $("#formularioEliminarMedico").dialog("close");
        },

        //#endregion Listado

        Guardar: function () {
            model.RecogerDatosFormulario();

            util.Ajax("GrabarMedico", JSON.stringify({ item: medico }),
            function (data) {
                var resultado = data.obj;

                if (resultado.Correcto == true) {
                    model.CargarLista();
                    model.LimpiarDatosFormulario();
                    if (medico.Id > 0) {
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

        LimpiarDatosFormulario: function () {
            $("#tbListadoEspecialidades").empty();

            listaEspecialidades = [];
            listaEspecialidadesEliminadas = [];
            $("#txtIdentificador").val("-1");
            $("#txtNombres").val("");
            $("#txtApellidos").val("");
            $("#txtDni").val("");
            $("#txtFechaNacimiento").val("");
            $("#txtDireccion").val("");
            $("#txtCorreo").val("");
            $("#txtTelefono").val("");
            $("#txtNumColegiatura").val("");
            $("#rbSexoMasculino").prop("checked", true);
            indiceEspecialidad = 0;
        },

        Nuevo: function () {
            model.LimpiarDatosFormulario();
            $("#formularioRegistrar").dialog("open");
        },

        Cancelar: function () {
            $("#formularioRegistrar").dialog("close");
        },

        Obtener: function (id) {

            util.Ajax("ObtenerMedico", JSON.stringify({ medicoId: id }),
            function (data) {
                var item = data.obj;
                if (item != null) {
                    $("#txtIdentificador").val(item.Id);
                    $("#txtDni").val(item.Dni);
                    $("#txtNumColegiatura").val(item.NumeroColegiatura);
                    $("#txtNombres").val(item.Nombres);
                    $("#txtApellidos").val(item.Apellidos);
                    var f = new Date(parseInt(item.FechaNacimiento.substr(6)));
                    var d = f.getDate();
                    var m = f.getMonth();
                    if (d < 10) d = '0' + d;
                    if (m < 10) m = '0' + m;
                    $("#txtFechaNacimiento").datepicker("option", "dateFormat", "dd/mm/yy");
                    $("#txtFechaNacimiento").datepicker("setDate", new Date(f.getFullYear(), m, d));
                    if (item.Sexo == 'M')
                        $("#rbSexoMasculino").prop("checked", true);
                    else
                        $("#rbSexoFemenino").prop("checked", true);
                    $("#txtCorreo").val(item.Correo);
                    $("#txtTelefono").val(item.Telefono);
                    $("#txtDireccion").val(item.Direccion);
                    $("#formularioRegistrar").dialog("open");
                    listaEspecialidadesEliminadas = [];
                    if (item.ListaEspecialidades != null)
                        model.ImprimirListaEspecialidades(item.ListaEspecialidades);
                }
            });
        },

        LimpiarListaEspecialidad: function () {
            indiceEspecialidad = 0;
            $("#tbListadoEspecialidades").html(
                            '<tr id="trEspecialidad0">' +
                                '<td style="width:10%" align="center">' +
                                    (indiceEspecialidad + 1) +
                                '</td>' +
                                '<td style="width:80%">' +
                                    '<input type="hidden" name="txtId" value="0" />' +
                                    '<input type="hidden" name="txtEspecialidadId" value="" />' +
                                    '<input type="text" name="txtEspecialidad" id="txtEspecialidad_0" value="" class="control-generate" />' +
                                '</td>' +
                                '<td style="width:10%" align="center">' +
                                    '<a title="Eliminar Especialidad" id="btnEliminarEspecialidad0" name="btnEliminarEspecialidad" style="cursor:pointer"><i class="fa fa-fw fa-trash"></i></a>' +
                                '</td>' +
                            '</tr>');

            $("#btnEliminarEspecialidad0").on("click", function () { return model.EliminarFilaEspecialidad(this); });
            model.AutocompletarEspecialidad("txtEspecialidad_0", 0);
            indiceEspecialidad++;
        },

        AgregarEspecialidad: function () {
            if (indiceEspecialidad == 0) {
                model.LimpiarListaEspecialidad();
            }
            else {
                $("#tbListadoEspecialidades tr:eq(0)").clone().appendTo("#tbListadoEspecialidades");
                $("#tbListadoEspecialidades tr:last td:eq(0)").html(indiceEspecialidad + 1);
                $("#tbListadoEspecialidades tr:last td:eq(1)").find("input[name=txtId]").val("0");
                $("#tbListadoEspecialidades tr:last td:eq(1)").find("input[name=txtEspecialidadId]").val("");
                $("#tbListadoEspecialidades tr:last td:eq(1)").find("input[name=txtEspecialidad]").val("").attr("id", "txtEspecialidad_" + indiceEspecialidad);
                model.AutocompletarEspecialidad("txtEspecialidad_" + indiceEspecialidad, indiceEspecialidad);
                $("#tbListadoEspecialidades tr:last td:eq(2)").find("a[name=btnEliminarEspecialidad]").on("click", function () { return model.EliminarFilaEspecialidad(this); }).css('display', 'inline').attr("id", "btnEliminarEspecialidad" + indiceEspecialidad);
                $("#tbListadoEspecialidades tr:last").attr("id", "trEspecialidad" + indiceEspecialidad);
                indiceEspecialidad += 1;

                if (indiceEspecialidad > 5) $("#tblCabecera").css('width', '97.8%');
                else $("#tblCabecera").css('width', '100%');
            }
        },

        ImprimirListaEspecialidades: function (lista) {
            if (lista == null) return;
            var strHtml = "";
            indiceEspecialidad = 0;
            for (var i = 0; i < lista.length; i++, indiceEspecialidad++) {
                strHtml += '<tr id="trEspecialidad' + i + '">' +
                                '<td style="width:10%" align="center">' +
                                    (i + 1) +
                                '</td>' +
                                '<td style="width:80%">' +
                                    '<input type="hidden" name="txtId" value="' + lista[i].Id + '" />' +
                                    '<input type="hidden" name="txtEspecialidadId" value="' + lista[i].EspecialidadId + '" />' +
                                    '<input type="text" name="txtEspecialidad" id="txtEspecialidad_' + i + '" value="' + lista[i].Especialidad + '" class="control-generate" />' +
                                '</td>' +
                                '<td style="width:10%" align="center">' +
                                    '<a title="Eliminar Especialidad" id="btnEliminarEspecialidad' + i + '" name="btnEliminarEspecialidad" style="cursor:pointer"><i class="fa fa-fw fa-trash"></i></a>' +
                                '</td>' +
                            '</tr>';
            }
            $("#tbListadoEspecialidades").html(strHtml);

            for (var j = 0; j < lista.length; j++) {
                model.AutocompletarEspecialidad("txtEspecialidad_" + j, j);
                $("#btnEliminarEspecialidad" + j + "").on("click", function () { return model.EliminarFilaEspecialidad(this); });
            }
        },

        AutocompletarEspecialidad: function (control, fila) {
            var asignaValores = function (d) {
                $("#tbListadoEspecialidades tr:eq(" + fila + ") td:eq(1)").find("input[name=txtEspecialidadId]").attr('value', d.Id);
                $("#" + control).val(d.Nombre);
            };

            var asignaValoresEnBlanco = function (value) {
                $("#tbListadoEspecialidades tr:eq(" + fila + ") td:eq(1)").find("input[name=txtEspecialidadId]").attr('value', "0");
                $("#" + control).val(value);
            };
            util.Autocompletar(control, 2, 150, 'BuscarEspecialidad', asignaValores, null, null, asignaValoresEnBlanco);
        },

        EliminarFilaEspecialidad: function (control) {
            var tr = $(control).parents().eq(1),
                id = tr.children()[1].children.txtId.value;


            EliminarFilaSeleccionada = control;

            if (id == 0 || id == "") {
                var indiceFila = tr.index();
                $(control).closest('tr').remove();
                indiceEspecialidad--;

                if (indiceEspecialidad > 5) $("#tblCabecera").css('width', '97.8%');
                else $("#tblCabecera").css('width', '100%');

                model.OrdenarCorrelativo("tbListadoEspecialidades", indiceFila);
                model.OrdenarControlesEspecialidades(indiceFila, { posColumna: 1, name: "txtEspecialidad" });
            } else {
                $("#formularioEliminarEspecialidad").dialog("open");
            }
        },

        OrdenarCorrelativo: function (tabla, indiceFila, control) {
            var totalFilas = $("#" + tabla + " tr").length;

            if (indiceFila < totalFilas) {
                var n = (totalFilas - indiceFila);
                for (var i = 0; i < n ; i++) {
                    if (control != undefined)
                        $("#" + tabla + " tr:eq(" + (indiceFila + i) + ") td:eq(0)").find("label[name=" + control + "]").html((indiceFila + 1 + i));
                    else
                        $("#" + tabla + " tr:eq(" + (indiceFila + i) + ") td:eq(0)").html((indiceFila + 1 + i));
                }
            }
        },

        OrdenarControlesEspecialidades: function (indiceFila, control1) {
            console.log("indice fila: " + indiceFila);
            var tabla = "tbListadoEspecialidades";
            var totalFilas = $("#" + tabla + " tr").length;
            if (indiceFila < totalFilas) {
                var n = (totalFilas - indiceFila);
                for (var i = 0; i < n ; i++) {
                    $("#" + tabla + " tr:eq(" + (indiceFila + i) + ")").attr("id", "trEspecialidad" + (indiceFila + i));
                    $("#" + tabla + " tr:eq(" + (indiceFila + i) + ")").find("a[name=btnEliminarEspecialidad]").attr("id", "btnEliminarEspecialidad" + (indiceFila + i));
                    if (control1 != undefined) {
                        $("#" + tabla + " tr:eq(" + (indiceFila + i) + ") td:eq(" + control1.posColumna + ")").find("input[name=" + control1.name + "]").attr("id", control1.name + "_" + (indiceFila + i));
                        model.AutocompletarEspecialidad(control1.name + "_" + (indiceFila + i), (indiceFila + i));
                    }
                }
            }
            console.log("indice fila: " + indiceFila);
            console.log("total filas: " + totalFilas);
        },

        EliminarEspecialidad: function () {
            if (EliminarFilaSeleccionada != undefined) {

                var tr = $(EliminarFilaSeleccionada).parents().eq(1);

                listaEspecialidadesEliminadas.push({
                    Id: tr.children()[1].children.txtId.value,
                    EspecialidadId: tr.children()[1].children.txtEspecialidadId.value,
                    Especialidad: tr.children()[1].children.txtEspecialidad.value,
                    EspecialidadDescripcion: '',
                    Activo: false,
                });

                var indiceFila = tr.index();
                $(EliminarFilaSeleccionada).closest('tr').remove();

                model.OrdenarCorrelativo("tbListadoEspecialidades", indiceFila);
                model.OrdenarControlesEspecialidades(indiceFila, { posColumna: 1, name: "txtEspecialidad" });
                indiceEspecialidad--;

                if (indiceEspecialidad > 5) $("#tblCabecera").css('width', '97.8%');
                else $("#tblCabecera").css('width', '100%');

                $("#formularioEliminarEspecialidad").dialog("close");
            }
        },

        CancelarEliminarEspecialidad: function () {
            $("#formularioEliminarEspecialidad").dialog("close");
        },

        MarcarMenu: function () {

            $("#optMantenimiento")
                .addClass("active")
                .addClass("menu-open");

            $("#optMedicos")
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

            $("#btnEliminarEspecialidad").button();
            $("#btnEliminarEspecialidad").on("click", this.EliminarEspecialidad);

            $("#btnCancelarEliminarEspecialidad").button();
            $("#btnCancelarEliminarEspecialidad").on("click", this.CancelarEliminarEspecialidad);

            $("#btnAgregarEspecialidad").button();
            $("#btnAgregarEspecialidad").on("click", this.AgregarEspecialidad);

            $("#btnEliminarMedico").button();
            $("#btnEliminarMedico").on("click", this.Eliminar);

            $("#btnCancelarEliminarMedico").button();
            $("#btnCancelarEliminarMedico").on("click", this.CancelarEliminarMedico);

            $('#txtFechaNacimiento').datepicker({ autoclose: true });


            util.Dialog("formularioRegistrar", 'Medico', 800, 540, 10, null, false);
            util.Dialog("formularioEliminarEspecialidad", "Eliminar Especialidad", 400, 161, 10, null, false);
            util.Dialog("formularioEliminarMedico", "Eliminar Medico", 400, 161, 10, null, false);

            $("#div-body").css("height", 140);

            this.ConfigurarGrilla();
            this.CargarLista();
            listaEspecialidadesEliminadas = [];
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