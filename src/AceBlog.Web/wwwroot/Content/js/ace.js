
String.format = function (str) {
    var args = arguments, re = new RegExp("%([1-" + args.length + "])", "g");
    return String(str).replace(
        re,
        function ($1, $2) {
            return args[$2];
        }
    );
}
String.formatDate = function (format) {
    return formatDate(this, format);
}
String.prototype.startsWith = function (str) {
    if (str === null || str === undefined || str == "" || this.length == 0 || str.length > this.length) {
        return false;
    }
    if (this.substr(0, str.length) == str) {
        return true;
    }

    return false;
}
String.prototype.endsWith = function (str) {
    if (str === null || str === undefined || str == "" || this.length == 0 || str.length > this.length) {
        return false;
    }

    if (this.substr(this.length - str.length) == str) {
        return true;
    }

    return false;
}

function formatDate(date, format) {
    if (!date || !format)
        return "";

    if (date.constructor == String) {
        date = date.replace(/-/g, "/").replace("T", " ");//IE11里面不能直接转换带"-",必须先替换成"/"，如果有T也要替换，不然 2018/1/1T10:10 这种格式会有问题
        date = new Date(date);
    }

    if (date.constructor == Date) {
        return date.format(format);
    }

    return date;
}
function isDateTimeFormat(str) {
    if (!str)
        return false;

    var dateString = str.replace(/-/g, "/").replace("T", " ");
    var date = new Date(dateString);
    if (date == "Invalid Date") {
        return false;
    }

    return true;
}
function compareDateTime(date1, date2) {
    date1 = convertToDate(date1);
    date2 = convertToDate(date2);

    if (date1 > date2)
        return 1;

    if (date1 < date2)
        return -1;

    return 0;
}
function isNumber(str) {
    if (str === null || str === undefined)
        return false;

    str = str.trim();
    if (str == "")
        return false;

    var reg = /(^[0-9]+$)|(^[0-9]+\.[0-9]+$)/;
    if (reg.test(str)) {
        return true;
    }

    return false;
}

Date.prototype.format = function (format) {
    var o =
    {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "H+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    };
    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
    return format;
};
Array.prototype.where = function (predicate) {
    var arr = this;
    var retArr = [];
    for (var i = 0; i < arr.length; i++) {
        var item = arr[i];
        if (predicate(item)) {
            retArr.push(item);
        }
    }
    return retArr;
};
Array.prototype.select = function (selector) {
    var arr = this;
    var retArr = [];
    for (var i = 0; i < arr.length; i++) {
        var item = arr[i];
        retArr.push(selector(item));
    }
    return retArr;
};
Array.prototype.first = function (predicate) {
    var arr = this;
    for (var i = 0; i < arr.length; i++) {
        var item = arr[i];
        if (predicate(item)) {
            return item;
        }
    }
    return null;
};
Array.prototype.any = function (predicate) {
    var arr = this;
    var retArr = [];
    for (var i = 0; i < arr.length; i++) {
        var item = arr[i];
        if (predicate(item)) {
            return true;
        }
    }
    return false;
};
Array.prototype.pushRange = function (items) {
    for (var i = 0; i < items.length; i++) {
        var item = items[i];
        this.push(item);
    }
};
Array.prototype.each = function (action) {
    for (var i = 0; i < this.length; i++) {
        var item = this[i];
        action(item);
    }
};
Array.prototype.sortBy = function (keySelector, rev) {
    /*
     *此方法返回一个新对象 
     */
    var arr = [];
    for (var i = 0; i < this.length; i++) {
        arr.push(this[i]);
    }

    //第二个参数没有传递 默认升序排列
    rev = rev !== undefined && rev !== false;

    var compare = function (a, b) {
        a = keySelector(a);
        b = keySelector(b);

        if (rev)
            return b - a;
        else
            return a - b;
    }

    arr.sort(compare);
    return arr;
}
Array.prototype.orderBy = function (keySelector) {
    return this.sortBy(keySelector, false);
}
Array.prototype.orderByDesc = function (keySelector) {
    return this.sortBy(keySelector, true);
}
Array.prototype.groupBy = function (keySelector) {
    var arr = this;
    var ret = {};
    for (var i = 0; i < arr.length; i++) {
        var item = arr[i];
        var key = keySelector(item);
        var group;
        if (key in ret) {
            group = ret[key];
        }
        else {
            group = [];
            ret[key] = group;
        }

        group.push(item);
    }

    return ret;
}



function newGuid() {
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20)) guid += "-";
    }
    return guid;
};
function isEmpty(str) {
    return str === null || str === "" || str === undefined;
}
function isNotEmpty(str) {
    return !isEmpty(str);
}

/* $ace */
(function ($) {
    var $ace = {};

    /* 返回 json 数据 */
    $ace.get = function (url, data, callback) {
        if ($.isFunction(data)) {
            callback = data;
            data = undefined;
        }

        url = parseUrl(url);
        var ret = execAjax("GET", url, data, callback);
        return ret;
    }
    $ace.post = function (url, data, callback) {
        if ($.isFunction(data)) {
            callback = data;
            data = undefined;
        }

        var ret = execAjax("POST", url, data, callback);
        return ret;
    }

    $ace.alert = function (msg, callBack) {
        layerAlert(msg, callBack);
    }
    $ace.confirm = function (msg, callBack) {
        layerConfirm(msg, callBack);
    }
    $ace.msg = function (msg, showTime, callBack) {
        layerMsg(msg, showTime, callBack);
    }
    /* 加载提示，不会自动消失，需手动关闭 */
    $ace.loading = function (msg) {
        var index = layer.msg(msg || "", {
            icon: 16
            , shade: 0.01
            , time: 1024 * 1000 * 1000 //设置超长等待，最后手动关闭
        });

        var loader = {
            close: function () {
                layer.close(index);
            }
        };

        return loader;
    }

    $ace.reload = function () {
        location.reload();
        return false;
    }

    /* 将当前 url 的参数值 */
    $ace.getQueryParam = function (name) {
        if (name === null || name === undefined || name === "")
            return "";
        name = ("" + name).toLowerCase();
        var search = location.search.slice(1);
        var arr = search.split("&");
        for (var i = 0; i < arr.length; i++) {
            var ar = arr[i].split("=");
            if (ar[0].toLowerCase() == name) {
                return decodeURIComponent(ar[1]);
            }
        }
        return "";
    }
    /* 将当前 url 参数转成一个 js 对象 */
    $ace.getQueryParams = function () {
        var params = {};
        var loc = window.location;

        if (!loc.search) {
            return params;
        }

        var paramsString;
        paramsString = loc.search.substr(1);//将?去掉
        var arr = paramsString.split("&");
        for (var i = 0; i < arr.length; i++) {
            var item = arr[i];
            var index = item.indexOf("=");
            if (index == -1)
                continue;
            var paramName = item.substr(0, index);
            if (!paramName)
                continue;
            var value = item.substr(index + 1);
            params[paramName] = decodeURIComponent(value);
        }
        return params;
    }
    $ace.convertToQueryString = function (model) {
        var c = "";
        var ret = "";
        for (var name in model) {
            var value = model[name];
            if (value === null || value === undefined || $.isFunction(value))
                continue;

            if ($.isPlainObject(value) || $.isArray(value))
                value = JSON.stringify(value);

            ret = ret + c;
            ret = ret + name + "=" + encodeURI(value);
            c = "&";
        }

        return ret;
    }

    $ace.cookie = {
        get: function (name) {
            var cookieValue = null;
            var search = name + "=";
            if (document.cookie.length > 0) {
                offset = document.cookie.indexOf(search);
                if (offset != -1) {
                    offset += search.length;
                    end = document.cookie.indexOf(";", offset);
                    if (end == -1)
                        end = document.cookie.length;
                    cookieValue = unescape(document.cookie.substring(offset, end));
                }
            }
            return cookieValue;
        },
        set: function (name, value, hours) {
            var expire = "";
            if (hours != null) {
                expire = new Date((new Date()).getTime() + hours * 3600000);
                expire = "; expires=" + expire.toGMTString();
            }
            document.cookie = name + "=" + escape(value) + expire + ";path=/";
        }
    };

    $ace.findName = function (optionList, value, valuePropName, textPropName) {
        valuePropName = valuePropName || "Id";
        textPropName = textPropName || "Name";
        return $ace.findText(optionList, value, valuePropName, textPropName);
    }
    $ace.findText = function (optionList, value, valuePropName, textPropName) {
        if (optionList.length == 0)
            return null;

        var valuePropertyNames;
        var textPropertyNames;
        if (valuePropName) {
            valuePropertyNames = [valuePropName];
            textPropertyNames = [textPropName];
        }
        else {
            valuePropertyNames = ["Id", "id", "Value", "value"];
            textPropertyNames = ["Name", "name", "Text", "text"];
        }

        valuePropName = valuePropertyNames.first(function (name) {
            return name in optionList[0];
        });
        textPropName = textPropertyNames.first(function (name) {
            return name in optionList[0];
        });

        var text = null;
        var len = optionList.length;
        for (var i = 0; i < len; i++) {
            if (optionList[i][valuePropName] == value) {
                text = optionList[i][textPropName];
                break;
            }
        }

        return text;
    }

    $ace.selectFields = function (items, fieldName) {
        return items.select(function (item) {
            return item[fieldName];
        });
    }
    $ace.selectIds = function (items) {
        if (items.length == 0)
            return [];

        var names = ["Id", "id", "ID"];

        var name;
        for (var i = 0; i < names.length; i++) {
            if (names[i] in items[0]) {
                name = names[i];
                break;
            }
        }

        return $ace.selectFields(items, name);
    }
    $ace.selectTexts = function (items) {
        if (items.length == 0)
            return [];

        var names = ["Name", "name", "Text", "text"];

        var name;
        for (var i = 0; i < names.length; i++) {
            if (names[i] in items[0]) {
                name = names[i];
                break;
            }
        }

        return $ace.selectFields(items, name);
    }

    $ace.browser = function () {
        var userAgent = navigator.userAgent;
        var isOpera = userAgent.indexOf("Opera") > -1;
        if (isOpera) {
            return "Opera"
        };
        if (userAgent.indexOf("Firefox") > -1) {
            return "FF";
        }
        if (userAgent.indexOf("Chrome") > -1) {
            if (window.navigator.webkitPersistentStorage.toString().indexOf('DeprecatedStorageQuota') > -1) {
                return "Chrome";
            } else {
                return "360";
            }
        }
        if (userAgent.indexOf("Safari") > -1) {
            return "Safari";
        }
        if (userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1 && !isOpera) {
            return "IE";
        };
    }
    $ace.download = function (url, data, method) {
        if (url && data) {
            data = typeof data == 'string' ? data : jQuery.param(data);
            var inputs = '';
            $.each(data.split('&'), function () {
                var pair = this.split('=');
                inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
            });
            $('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>').appendTo('body').submit().remove();
        };
    };

    /* 依赖 bootstrap ui */
    $ace.selectRow = function (selectedTr) {
        var c = "warning";
        $(selectedTr).addClass(c);
        $(selectedTr).siblings().removeClass(c);
        return true;
    }
    $ace.formatBool = function (val) {
        if (val == true) {
            return "是";
        }
        else if (val == false) {
            return "否";
        }

        return val;
    }


    function execAjax(type, url, data, callback) {
        var layerIndex = layer.load(2);
        var ret = $.ajax({
            url: url,
            type: type,
            dataType: "json",
            data: data,
            complete: function (xhr) {
                layer.close(layerIndex);
            },
            success: function (result) {
                var isStandardResult = ("Success" in result) && ("Msg" in result);
                if (isStandardResult) {
                    if (!result.Success) {
                        layerAlert(result.Msg || "操作失败");
                        return;
                    }

                    if (result.Success) {
                        /* 传 result，用 result.Data 还是 result.Msg，由调用者决定 */
                        callback(result);
                    }
                }
                else
                    callback(result);
            },
            error: errorCallback,
            statusCode: {
                401: function (xhr) {
                    var result = xhr.responseJSON;
                    var isStandardResult = ("Status" in result) && ("Msg" in result);
                    if (isStandardResult) {
                        layerAlert(result.Msg || "未登录或登录过期，请重新登录");
                        return;
                    }

                    layerAlert("未登录或登录过期，请重新登录");
                },
                403: function (xhr) {
                    var result = xhr.responseJSON;
                    var isStandardResult = ("Status" in result) && ("Msg" in result);
                    if (isStandardResult) {
                        layerAlert(result.Msg || "权限不足，禁止访问");
                        return;
                    }

                    layerAlert(result.Msg || "权限不足，禁止访问");
                }
            }
        });
        return ret;
    }
    function errorCallback(xhr, textStatus, errorThrown) {

        if (xhr.status == 401 || xhr.status == 403) {
            return;
        }

        var json = { statusCode: xhr.status, textStatus: textStatus, errorThrown: errorThrown };
        alert("请求失败: " + JSON.stringify(json));
    }
    function parseUrl(url) {
        if (url.indexOf("_dc=") < 0) {
            if (url.indexOf("?") >= 0) {
                url = url + "&_dc=" + (new Date().getTime());
            } else {
                url = url + "?_dc=" + (new Date().getTime());
            }
        }
        return url;
    };

    function layerAlert(msg, callBack) {
        msg = msg == null ? "" : msg;/* layer.alert 传 null 会报错 */
        var type = 'warning';
        var icon = "";
        if (type == 'success') {
            icon = "fa-check-circle";
        }
        if (type == 'error') {
            icon = "fa-times-circle";
        }
        if (type == 'warning') {
            icon = "fa-exclamation-circle";
        }

        var idx;
        idx = layer.alert(msg, {
            icon: icon,
            title: "系统提示",
            btn: ['确认'],
            btnclass: ['btn btn-primary'],
        }, function () {
            layer.close(idx);
            if (callBack)
                callBack();
        });
    }
    function layerConfirm(content, callBack) {
        var idx;
        idx = layer.confirm(content, {
            icon: "fa-exclamation-circle",
            title: "系统提示",
            btn: ['确认', '取消'],
            btnclass: ['btn btn-primary', 'btn btn-danger'],
        }, function () {
            layer.close(idx);
            callBack();
        }, function () {

        });
    }
    function layerMsg(msg, showTime, callBack) {
        var time = 2000;//单位毫秒

        if ($.isFunction(showTime))
            callBack = showTime;
        else if (showTime)
            time = showTime;

        msg = msg == null ? "" : msg;/* layer.msg 传 null 会报错 */

        if (!callBack) {
            callBack = function () { }
        }

        layer.msg(msg, { time: time, shift: 0 }, callBack);
    }


    window.$ace = $ace;
})($);


/* $ 扩展 */
(function ($) {

    /* 设置标签的值，同时触发 change 事件 */
    $.fn.setValue = function (val) {
        this.val(val);
        this.change();
    }

    /* 根据值设置 select 标签的选中项，同时触发 change 事件 */
    $.fn.setSelectedValue = function (val) {
        for (var j = 0; j < this.length; j++) {
            var selectElement = this[j];
            for (var i = 0; i < selectElement.options.length; i++) {
                var op = selectElement.options[i];
                if (op.value == val) {
                    op.selected = true;
                    $(selectElement).change();
                    break;
                }
            }
        }
    };
    $.fn.getSelectedValue = function () {
        var ret = "";
        var c = "";
        for (var j = 0; j < this.length; j++) {
            var selectElement = this[j];
            var idx = selectElement.selectedIndex;
            ret += c + selectElement[idx].value;
            c = ";";
        }

        return ret;
    };
    $.fn.getSelectedText = function () {
        var ret = "";
        var c = "";
        for (var j = 0; j < this.length; j++) {
            var selectElement = this[j];
            var idx = selectElement.selectedIndex;
            ret += c + selectElement[idx].text;
            c = ";";
        }

        return ret;
    };
    $.fn.getSelectedOption = function () {
        if (this.length == 0)
            return null;

        var selectElement = this[0];
        var idx = selectElement.selectedIndex;
        return { value: selectElement[idx].value, text: selectElement[idx].text };
    }
    $.fn.appendOptions = function (options, valueSource, textSource) {
        valueSource = valueSource || "value";
        textSource = textSource || "text";

        for (var i = 0; i < options.length; i++) {
            var option = options[i];

            var $option = $('<option></option>');
            $option.val(option[valueSource]);
            $option.text(option[textSource]);

            $(this).append($option);
        }
    }

    /* 设置 radio、checkbox 状态为选中 */
    $.fn.checked = function () {
        for (var j = 0; j < this.length; j++) {
            var ele = this[j];
            var old = ele.checked;
            ele.checked = true;
            if (old != ele.checked)
                $(ele).change();
        }
    }
    /* 设置 radio、checkbox 状态为未选中 */
    $.fn.unchecked = function () {
        for (var j = 0; j < this.length; j++) {
            var ele = this[j];
            var old = ele.checked;
            ele.checked = false;
            if (old != ele.checked)
                $(ele).change();
        }
    }
    /* 设置 radio、checkbox 状态。如果传入的 value 与控件的 value 相等，则设置为选中，否则不选中 */
    $.fn.setChecked = function (value) {
        var inputValues = [];

        if (value instanceof Array) {
            inputValues = value;
        }
        else {
            inputValues.push(value);
        }

        var values = [];

        for (var i = 0; i < inputValues.length; i++) {
            var value = inputValues[i];
            if (value === true)
                values.push("true");
            else if (value === false)
                values.push("false");
            else if (value !== undefined && value !== null) {
                values.push(value);
            }
        }

        for (var i = 0; i < this.length; i++) {
            var ele = this[i];
            var checked = false;
            for (var j = 0; j < values.length; j++) {
                var value = values[j];
                if (ele.value == value) {
                    checked = true;
                    break;
                }
            }

            var old = ele.checked;

            if (checked) {
                ele.checked = true;
            }
            else {
                ele.checked = false;
            }

            if (old != ele.checked)
                $(ele).change();
        }
    }
    /* 获取被选中的 radio、checkbox 值 */
    $.fn.getCheckedValue = function () {
        for (var j = 0; j < this.length; j++) {
            var ele = this[j];
            if (ele.checked == true) {
                return ele.value;
            }
        }

        return null;
    }

    $.fn.formValid = function () {
        return true;//未做前端校验
        return $(this).valid({
            errorPlacement: function (error, element) {
                element.parents('.formValue').addClass('has-error');
                element.parents('.has-error').find('i.error').remove();
                element.parents('.has-error').append('<i class="form-control-feedback fa fa-exclamation-circle error" data-placement="left" data-toggle="tooltip" title="' + error + '"></i>');
                $("[data-toggle='tooltip']").tooltip();
                if (element.parents('.input-group').hasClass('input-group')) {
                    element.parents('.has-error').find('i.error').css('right', '33px')
                }
            },
            success: function (element) {
                element.parents('.has-error').find('i.error').remove();
                element.parent().removeClass('has-error');
            }
        });
    }

    $.fn.getFormData = function () {
        var $form = $(this);
        var model = {};
        $form.find('input,select,textarea').each(function (r) {
            var $this = $(this);
            var name = $this.attr('name');

            if (!name)
                return;

            if ($this.attr("disabled") == "disabled")
                return;

            var nodeType = getNodeType(this);

            switch (nodeType) {
                case "checkbox":
                    var checkboxValues;
                    if (name in model) {
                        checkboxValues = model[name];
                    }
                    else {
                        checkboxValues = [];
                        model[name] = checkboxValues;
                    }

                    if (this.checked) {
                        checkboxValues.push($this.val());
                    }
                    break;
                case "radio":
                    if (model[name] === undefined)
                        model[name] = null;
                    if (this.checked == true)
                        model[name] = this.value;
                    break;
                case "button":
                    break;
                default:
                    var value = $this.val();
                    model[name] = value;
                    break;
            }
        });
        if ($('[name=__RequestVerificationToken]').length > 0) {
            model["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
        }
        return model;
    };
    $.fn.setFormData = function (data) {
        var $form = $(this);
        $form.find('input,select,textarea').each(function (r) {
            var $ele = $(this);
            var name = $ele.attr('name');
            if (!name)
                return;

            var value = null;
            if (name in data)
                value = data[name];
            else
                return;//如果模型中不存在相应的name属性，则不管

            var nodeType = getNodeType(this);

            switch (nodeType) {
                case "checkbox":
                    var values = value || []; //对于checkbox，value是一个数组

                    for (var i = 0; i < $ele.length; i++) {
                        var checkbox = $ele[i];
                        var checkboxValue = checkbox.value;
                        var checked = false;
                        for (var j = 0; j < values.length; j++) {
                            if (checkboxValue == values[j]) {
                                checked = true;
                                break;
                            }
                        }
                        checkbox.checked = checked;
                    }
                    break;
                case "radio":
                    $ele.setChecked(value);
                    break;
                case "select":
                    if ($.isPlainObject(value) && value.value) { //{value: 1, text: "123"}
                        value = value.value;
                    }
                    $ele.val(value).trigger("change");
                    break;
                case "button":
                    break;
                default:
                    $ele.val(value);
                    break;
            }
        });
    };
    function getNodeType(ele) {
        var nodeName = ele.nodeName.toLowerCase();
        var nodeType = nodeName;
        if (nodeName === "input") {
            nodeType = $ele.attr('type');
        }

        return nodeType;
    }

    $.fn.bindSelect = function (options) {
        var defaults = {
            value: "Id",
            text: "Name",
            change: null,
            items: [],
            placeholder: "-请选择-"
        };
        var options = $.extend(defaults, options);
        var $element = $(this);

        if (options.placeholder !== null && options.placeholder !== undefined)
            $element.append($("<option></option>").val("").html(options.placeholder));

        items = options.items;
        $.each(items, function (i) {
            $element.append($("<option></option>").val(items[i][options.value]).html(items[i][options.text]));
        });

        if (options.change)
            $element.change(options.change);

        return;
    }

    $.fn.disable = function () {
        var $element = $(this);
        $element.attr("disabled", "disabled");
    }
    $.fn.enable = function () {
        var $element = $(this);
        $element.removeAttr("disabled");
    }

    $.fn.paging = function (options) {
        var defaults = { totalCount: 0, pageSize: 20, currentPage: 1, showPageCount: 6, callback: function (page) { } };
        options = $.extend({}, defaults, options);

        function pagingHanler(page) {
            return function () { options.callback(page); };
        }
        function getPageBtn(text, pageTo) {
            var $btn = $('<a class="fx-page-btn" href="javascript:;"></a>');
            $btn.text(text);
            $btn.click(pagingHanler(pageTo));
            return $btn;
        }

        var $html = $('<div class="fx-page"></div>');

        var $btns = $('<span class="fx-page-btns"></span>');
        $html.append($btns);

        if (options.currentPage != 1) {
            var $btn = getPageBtn("上一页", options.currentPage - 1);
            $btns.append($btn);
        }

        var totalPage = 0;
        if (options.totalCount > 0) {
            if ((options.totalCount % options.pageSize) == 0) {
                totalPage = parseInt(options.totalCount / options.pageSize);
            }
            else {
                totalPage = parseInt(options.totalCount / options.pageSize) + 1;
            }
        }

        var min = options.currentPage - (options.showPageCount / 2);

        if (min < 1)
            min = 1;

        var max = min + options.showPageCount;

        if (max > totalPage) {
            max = totalPage;
            min = max - options.showPageCount;
            if (min < 1)
                min = 1;
        }



        if (min != 1) {
            var $btn = getPageBtn(1, 1);
            $btn.attr("title", "首页");
            $btns.append($btn);
            $btns.append('<span class="fx-page-btn">...</span>');
        }

        for (var i = min; i <= max; i++) {
            if (i == options.currentPage) {
                $btns.append('<span class="fx-page-btn fx-page-curr">' + i + '</span>');
                continue;
            }

            var $btn = getPageBtn(i, i);
            $btns.append($btn);
        }

        if (max != totalPage) {
            $btns.append('<span class="fx-page-btn">...</span>');
            var $btn = getPageBtn(totalPage, totalPage);
            $btns.append($btn);
        }

        if (options.currentPage < totalPage) {
            var $btn = getPageBtn("下一页", options.currentPage + 1);
            $btns.append($btn);
        }

        $html.append('<span>&nbsp;共<span class="fx-page-total">' + options.totalCount + '</span>条</span>');

        var $x = $('<span> 到第<input type="number" min="1" />页 <button type="button" class="fx-btn-go">GO</button></span>');

        var $toPageInput = $x.find('input');
        $toPageInput.val(options.currentPage);
        $toPageInput.keyup(function () {
            $toPageInput.val($toPageInput.val().replace(/\D/, ''));
        });

        $x.find('button').click(function () {
            var page = $toPageInput.val();
            if (page < 1 || page > totalPage || page == options.currentPage)
                return;

            pagingHanler(parseInt(page))();
        });

        $html.append($x);

        $(this).html($html);
    }

    $.loading = function (bool, text) {
        var $loadingpage = top.$("#loadingPage");
        var $loadingtext = $loadingpage.find('.loading-content');
        if (bool) {
            $loadingpage.show();
        } else {
            if ($loadingtext.attr('istableloading') == undefined) {
                $loadingpage.hide();
            }
        }
        if (!!text) {
            $loadingtext.html(text);
        } else {
            $loadingtext.html("数据加载中，请稍后…");
        }
        $loadingtext.css("left", (top.$('body').width() - $loadingtext.width()) / 2 - 50);
        $loadingtext.css("top", (top.$('body').height() - $loadingtext.height()) / 2);
    }
})($);



function ViewModel() {
    var me = this;

    me.searchModel = _ob({});
    me.url = null;//加载数据的url
    me.deleteUrl = null;
    me.modelKeyName = "Id"; /* 实体主键名称 */

    /* 如有必要，子类需重写 DataTable、Dialog */
    me.dataTable = new PagedDataTable(me);
    me.dialog = new Dialog();

    me.init = function () {
        me.loadData();
    }

    /* 添加按钮点击事件 */
    me.add = function () {
        ensureNotNull(me.dialog, "dialog");
        me.dialog.open(null, "添加");
    }

    /* 编辑按钮点击事件 */
    me.edit = function () {
        ensureNotNull(me.dataTable, "DataTable");
        ensureNotNull(me.dialog, "Dialog");
        me.dialog.open(me.dataTable.selectedModel(), "修改");
    }

    /* 删除按钮点击事件 */
    me.del = function () {
        $ace.confirm("确定要删除该条数据吗？", me.onDelete);
    }

    me.onDelete = function () {
        deleteRow();
    }
    /* 要求每行必须有 Id 属性，如果主键名不是 Id，则需要重写 me.ModelKeyName */
    function deleteRow() {
        if (me.deleteUrl == null)
            throw new Error("未指定 DeleteUrl");

        var url = me.deleteUrl;
        var params = { id: me.dataTable.selectedModel()[me.modelKeyName]() };
        $ace.post(url, params, function (result) {
            var msg = result.Msg || "删除成功";
            $ace.msg(msg);
            me.dataTable.removeSelectedModel();
        });
    }

    /* 搜索按钮点击事件 */
    me.search = function () {
        me.loadData();
    }
    me.getSearchModel = function () {
        var model = me.searchModel();
        model = JSON.parse(JSON.stringify(model));

        return model;
    }

    /* 加载数据 */
    me.loadData = function (page) {
        if (me.url == null)
            return;

        var data = me.getSearchModel();
        if (page)
            data.page = page;
        $ace.get(me.url, data, function (result) {
            me.dataTable.setData(result.Data);
        });
    }

    function ensureNotNull(obj, name) {
        if (!obj)
            throw new Error("属性 " + name + " 未初始化");
    }
}
function DataTable(vmOrDataLoader) {
    var me = this;

    me.deleteUrl = null;
    me.modelKeyName = "Id"; /* 实体主键名称 */

    me.models = _oba([]);
    me.selectedModel = _ob(null);

    me.getOrdinal = function ($index) {
        return $index + 1;
    }
    me.selectRow = function (model, event) {//已废弃
        me.selectedModel(model);
        $ace.selectRow(event.currentTarget);
        return true;
    }
    me.rowClick = function (model, event) {
        me.selectedModel(model);
        $ace.selectRow(event.currentTarget);
        return true;
    }
    me.removeSelectedModel = function () {
        var selectedModel = me.selectedModel();
        if (selectedModel) {
            me.models.remove(selectedModel);
        }
    }
    me.removeModel = function (model) {
        me.models.remove(model);
        var selectedModel = me.selectedModel();
        if (selectedModel == model) {
            me.selectedModel(null);
        }
    }

    me.setData = function (models) {
        var wrapedModels = $ko.toOb(models);
        me.models(wrapedModels);
        me.selectedModel(null);
    }
    me.setModels = function (models) {
        me.setData();
    }

    me.reload = function () {
        if (!vmOrDataLoader)
            throw new Error("未实现 loadData 方法");

        if ($.isFunction(vmOrDataLoader)) {
            vmOrDataLoader();
        }
        else {
            if ("loadData" in vmOrDataLoader)
                vmOrDataLoader.loadData();
            else
                throw new Error("vmOrDataLoader 未实现 loadData 方法");
        }
    }

    /* 编辑按钮点击事件 */
    me.edit = function (model) {
        if (vmOrDataLoader && vmOrDataLoader.dialog)
            vmOrDataLoader.dialog.open(model, "修改");
    }
    /* 删除按钮点击事件 */
    me.del = function (model) {
        $ace.confirm("确定要删除该条数据吗？", function () { me.onDelete(model); });
    }

    me.onDelete = function (model) {
        deleteRow(model);
    }
    /* 要求每行必须有 Id 属性，如果主键名不是 Id，则需要重写 me.ModelKeyName */
    function deleteRow(model) {
        if (me.deleteUrl == null)
            throw new Error("未指定 DeleteUrl");

        var url = me.deleteUrl;

        var params = { id: model[me.modelKeyName]() };
        $ace.post(url, params, function (result) {
            var msg = result.Msg || "删除成功";
            $ace.msg(msg);
            me.removeModel(model);
        });
    }

}
function PagedDataTable(vmOrDataLoader) {
    var me = this;
    DataTable.call(me, vmOrDataLoader);

    me.pagger = _ob({});

    me.getOrdinal = function ($index) {
        return (me.pagger().currentPage - 1) * me.pagger().pageSize + $index + 1;
    }

    me.setData = function (pagedData) {
        var wrapedData = $ko.toOb(pagedData);
        me.models(wrapedData.Models());

        var pageOptions = { totalCount: wrapedData.TotalCount(), pageSize: wrapedData.PageSize(), currentPage: wrapedData.CurrentPage(), showPageCount: 6 };

        pageOptions.callback = function (page) {
            me.loadData(page);
        }

        me.pagger(pageOptions);
        me.selectedModel(null);
    }

    me.loadData = function (page) {
        if (!vmOrDataLoader)
            throw new Error("未实现 loadData 方法");

        if ($.isFunction(vmOrDataLoader)) {
            vmOrDataLoader(page);
        }
        else {
            if ("loadData" in vmOrDataLoader)
                vmOrDataLoader.loadData(page);
            else
                throw new Error("vmOrDataLoader 未实现 loadData 方法");
        }
    }
    me.reload = function () {
        me.loadData(me.pagger().currentPage);
    }
}
function Dialog() {
    var me = this;

    /* must */
    me.title = _ob(null);
    me.isShow = _ob(false);

    me.editModel = _ob(null);
    me.model = _ob({});//绑定到界面的 model
    /* end must */

    /* must */
    me.close = function () {
        me.isShow(false);
    }
    me.open = function (model, title) {
        if (me.isShow()) {
            me.isShow(false);//确保触发值改变事件。有时候当模态框处于未打开状态时me.isShow()的值依然是true，比如按esc关闭模态框后
        }
        me.isShow(true);

        me.model({});
        if (model) {
            me.editModel(model);
        }
        else {
            me.editModel(null);
        }

        if (title)
            me.title(title);

        me.onOpen();
    }
    me.save = function () {
        me.onSave();
    }
    /* end must */


    me.onOpen = function () {
        var model = me.editModel();
        if (model) {
            var bindModel = $ko.toJS(model);
            me.model(bindModel);
        }
        else {
            me.model({});
        }
    }
    me.onSave = function () {
        throw new Error("未重写 OnSave 方法");
    }
}
