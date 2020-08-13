
var util = {

    timer: 0,

    Ajax: function (url, data, success, err) {
        return $.ajax({
            type: "POST",
            url: url,
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: success,
            error: function (x, s, e) {
                //if(x.responseJSON) alert(x.responseJSON.Message);
                console.log(e);
                if (err != null) err();
            }
        });
    },

    Autocompletar: function (id, minLen, timeOut, urlData, selectCallback, changeCallback, optionalParameters, nullCallback) {
        var utl = this;
        var control = $("#" + id).autocomplete(
                    {
                        delay: timeOut,
                        minLength: minLen,
                        autoFocus: true,
                        open: function () {
                            $(this).autocomplete('widget').css('z-index', 20000);
                            return false;
                        },
                        select: function (event, ui) {
                            $("#" + id).val(ui.item.Id);

                            if (selectCallback != null) selectCallback(ui.item);

                            return false;
                        },
                        focus: function (event, ui) {
                            return false;
                        },
                        change: function (event, ui) {
                            var valor = $("#" + id).val();

                            if (ui.item == null) {
                                if (nullCallback != null) nullCallback(valor);
                            }

                            if (changeCallback != null) changeCallback(ui.item);

                            return false;
                        },
                        source: function (request, response) {
                            var param = { cadena: request.term };

                            if (optionalParameters != null) {
                                optionalParameters.cadena = request.term;
                                param = optionalParameters;
                            }

                            utl.Ajax(urlData, JSON.stringify(param),
                            function (data) {
                                console.log("cargo lista!")
                                console.log(data);
                                response(data);
                            });
                        }
                    });


        $("#" + id).autocomplete("option", "appendTo", "body");

        $("#" + id).autocomplete.prototype._resizeMenu = function () {
            var ul = this.menu.element;
            ul.outerWidth(this.element.outerWidth());
        }

        if (control.data("uiAutocomplete") != undefined) {



            control.data("uiAutocomplete")._renderItem = function (ul, item) {
                return $('<li></li>')
                    .data("item.autocomplete", item)
                    .append('<a style="position: absolute;width:100%;">' + item.Nombre + '</a>')
                    .appendTo(ul);
            };
        }

    },

    Dialog: function (div, title, width, height, zindex, buttons, autoOpen) {

        if (autoOpen == null) autoOpen = true;

        $("#" + div).dialog({
            title: title,
            width: width,
            height: height,
            modal: true,
            resizable: false,
            zIndex: zindex,
            draggable: true,
            buttons: buttons,
            autoOpen: autoOpen,
            show: { effect: "fade" },
            hide: { effect: "fade" }
        }).width(width - 20).height(height);

    },

    DialogUrl: function (url, title, width, height, zindex) {

        var popup = $('<div></div>');
        var img = $('<img style="position:absolute;left:45%;z-index:1000;"  src="images/loading.gif" alt="cargando..."/>');
        var ifr = $('<iframe  src="' + url + '"  width=100% height="' + (height - 10) + 'px" frameborder=0 scrolling=no  AllowTransparency />');

        popup.append(img[0]);
        popup.prepend(ifr[0]);

        ifr.load(function () {
            img.remove();
            //ifr.show();
        });


        return popup.dialog({
            title: title,
            width: width,
            height: height,
            modal: true,
            resizable: false,
            zIndex: zindex,
            draggable: true
        }).width(width - 20).height(height);

    },

    Delay: function (callback, ms) {
        clearTimeout(this.timer);
        this.timer = setTimeout(callback, ms);
    },

    FormatJsonDate: function (JsonDate) {

        var f = new Date(parseInt(JsonDate.substr(6)));
        var d = f.getDate();
        var m = f.getMonth() + 1;
        if (d < 10) d = '0' + d;
        if (m < 10) m = '0' + m;

        return d + '/' + m + '/' + f.getFullYear();
        //return $.datepicker.formatDate('dd/mm/yy', d)
        //return d.toJSON().replace(/^(\d{4})\-(\d{2})\-(\d{2}).*$/, '$3/$2/$1');
    },

    FormatJsonDateTime: function (JsonDate) {

        var f = new Date(parseInt(JsonDate.substr(6)));
        var d = f.getDate();
        var m = f.getMonth() + 1;
        if (d < 10) d = '0' + d;
        if (m < 10) m = '0' + m;

        return d + '/' + m + '/' + f.getFullYear() + ' ' + f.getHours() + ':' + f.getMinutes();
    },

    FormatJsonTimeSpan: function (JsonTimeSpan) {
        var time = JsonTimeSpan;
        var Hours = (time.Hours < 10 ? '0' + time.Hours : time.Hours);
        var Minutes = (time.Minutes < 10 ? '0' + time.Minutes : time.Minutes);

        return Hours + ':' + Minutes;
    },

    Grid: function (id, columns, height, allowSearch, allowGroup, allowColumnResize, showRowLines) {

        $("#" + id).dxDataGrid({
            //dataSource: [],
            width: '100%',
            height: height,
            showColumnLines: true,
            showRowLines: (showRowLines != null) ? showRowLines : true,
            selection: { mode: 'single' },
            searchPanel: { visible: (allowSearch != null) ? allowSearch : true, width: '260px' },
            hoverStateEnabled: true,
            pager: { visible: 'auto' },
            paging: { enabled: false, pageSize: 100 },
            //scrolling: { mode: 'virtual',preloadEnabled:true },
            loadPanel: { enabled: true },
            allowColumnResizing: (allowColumnResize != null) ? allowColumnResize : false,
            groupPanel: {
                visible: (allowGroup != null) ? allowGroup : true
            },
            columns: columns,
            columnAutoWidth: true,
            showBorders: true
        });
    },

    MsgBox: function (message, type, title) {

        if (message == null) message = "";

        var msj = message.replace(/\r/g, '');

        var d = document.createElement('div');
        var icon = 'ico_Alert';

        if (type == 'INFO')
            icon = 'ico_Info';
        else if (type == 'ERROR')
            icon = 'ico_Error';

        if (title == null) title = 'Aviso';

        $(d).html('<div class="' + icon + '" style="display:table-cell; margin:4px;"></div><div style="display:table-cell; padding:5px;white-space: pre; min-width:220px;">' + msj + '<div>');

        if (typeof (_audio) != 'undefined') _audio.play();

        $(d).dialog({
            title: title,
            width: 'auto',
            minHeight: 20,
            modal: true,
            resizable: false,
            zIndex: 1000,
            draggable: true,
            buttons: { Aceptar: function () { $(this).dialog("close"); } }
        });

    },

    MsgAlert: function (message) {
        this.MsgBox(message, "", "Advertencia");
    },

    MsgError: function (message) {
        this.MsgBox(message, "ERROR", "Error");
    },

    MsgInfo: function (message) {
        this.MsgBox(message, "INFO", "Aviso");
    },

    GetDate: function (c) {

        if (this.IsValidDate(c)) {
            var from = c.split("/");
            return new Date(from[2], from[1] - 1, from[0]);
        }
        else
            return null;
    },

    IsValidDate: function (value, userFormat) {

        if (!userFormat) userFormat = 'dd/mm/yyyy';

        var delimiter = /[^mdy]/.exec(userFormat)[0];
        var theFormat = userFormat.split(delimiter);
        var theDate = value.split(delimiter);

        var isDate = function (date, format) {
            var m, d, y
            for (var i = 0, len = format.length; i < len; i++) {
                if (/m/.test(format[i])) m = date[i]
                if (/d/.test(format[i])) d = date[i]
                if (/y/.test(format[i])) y = date[i]
            }
            return (
              m > 0 && m < 13 &&
              y && y.length === 4 &&
              d > 0 && d <= (new Date(y, m, 0)).getDate()
            )
        }

        return isDate(theDate, theFormat);

    },
}
