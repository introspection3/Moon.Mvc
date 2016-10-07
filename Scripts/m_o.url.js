function MoonUrl(url) {
    this.Url = url;
    this.HomePagePath = "";
    this.GetParameters = function () {
        var ps = this.Url;
        var index = ps.indexOf("?");
        if (index > -1) {
            this.HomePagePath = ps.substr(0, index);

            var ret = ps.substr(index + 1);
            var jh = ret.indexOf("#");
            if (jh > -1) {
                ret = ret.substr(0, jh);
            }
            return ret;
        } else {
            this.HomePagePath = this.Url;
            return "";
        }
    }
    this.GetParameterValue = function (pname) {
        var ps = this.GetParameters();
        if (ps != "") {
            var arra = ps.split("&");
            var dic = new m_map();
            for (var i = 0; i < arra.length; i++) {
                var pv = arra[i];
                var pv_arra = pv.split("=");
                var p = pv_arra[0];

                var v = pv_arra[1];
                dic.Add(p, v);
            }
            if (dic.ContainsKey(pname)) {
                return dic.GetItem(pname);
            } else {
                return "";
            }
        } else {
            return "";
        }
    }
    this.ChangeUrlParameter = function (pname, value) {
        var ps = this.GetParameters();
        if (ps != "") {
            var arra = ps.split("&");
            var dic = new m_map();
            for (var i = 0; i < arra.length; i++) {
                var pv = arra[i];
                var pv_arra = pv.split("=");
                var p = pv_arra[0];
                var v = pv_arra[1];
                dic.Add(p, v);
            }
            if (dic.ContainsKey(pname)) {
                dic.SetItem(pname, value);
            } else {
                dic.Add(pname, value);
            }
            var str = "";
            var dicarra = dic.GetArray();
            for (var i = 0; i < dicarra.length; i++) {
                var pv = dicarra[i];
                str += pv.key + "=" + pv.value;
                if (i < dicarra.length - 1) {
                    str += "&";
                }
            }
            return this.HomePagePath + "?" + str;

        } else {
            return this.Url + "?" + panme + "=" + value;
        }
    }
    this.RemoveParameter = function (pname) {
        var ps = this.GetParameters();

        if (ps != "") {
            var arra = ps.split("&");
            var dic = new m_map();
            for (var i = 0; i < arra.length; i++) {
                var pv = arra[i];
                var pv_arra = pv.split("=");
                var p = pv_arra[0];
                var v = pv_arra[1];
                if (pname != p) {
                    dic.Add(p, v);
                }
            }

            var str = "";
            var dicarra = dic.GetArray();
            for (var i = 0; i < dicarra.length; i++) {
                var pv = dicarra[i];
                str += pv.key + "=" + pv.value;
                if (i < dicarra.length - 1) {
                    str += "&";
                }
            }
            return this.HomePagePath + "?" + str;

        } else {
            return this.Url;
        }
    }
}
var m = window.m || {};
m.url = m.url || {};
m.url.create = function (url) {
    return new MoonUrl(url);
}
//依赖 m_map.js
//参考
//http://www.cnblogs.com/humble/p/4887289.html
