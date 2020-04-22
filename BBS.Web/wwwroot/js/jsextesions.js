(function (win, $) {
    String.prototype.isEmpty = function () {
        if (this.match(/^\s+$/)) {
            return true; // all space or \\n
        }
        if (this.match(/^[ ]+$/)) {
            return true; // all space
        }
        if (this.match(/^[ ]*$/)) {
            return true; // all space or empty
        }
        if (this.match(/^\s*$/)) {
            return true; // all space or \\n or empty
        }

        return false;
    }
    //  Form To Json数据
    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    }
    // 格式化
    $.format = function (source, params) {
        if (!params) return source;

        if (arguments.length === 1)
            return function () {
                var args = $.makeArray(arguments);
                args.unshift(source);
                return $.format.apply(this, args);
            };
        if (arguments.length > 2 && params.constructor !== Array) {
            params = $.makeArray(arguments).slice(1);
        }
        if (params.constructor !== Array) {
            params = [params];
        }
        $.each(params, function (i, n) {
            source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
        });
        return source;
    };
    // url参数
    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
    // 判断是否 json 字符串
    function isJSON(str) {
        if (typeof str == 'string') {
            try {
                var obj = JSON.parse(str);
                if (typeof obj == 'object' && obj) {
                    return true;
                } else {
                    return false;
                }

            } catch (e) {
                return false;
            }
        }
        return true;
    }
})(window, jQuery);

