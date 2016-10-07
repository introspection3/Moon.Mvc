//字典
function m_map() {
    var _count = 0;
    var _array = new Array();
    this.ContainsKey = function (k) {
        var ret = false;
        for (var i = 0; i < _array.length; i++) {
            if (_array[i].key == k) {
                ret = true;
                break;
            }
        }
        return ret;
    }
    this.GetCount = function () {
        return _count;
    }
    this.IsEmpty = function () {
        return _count == 0;
    }
    this.ContainsValue = function (v) {
        var ret = false;
        for (var i = 0; i < _array.length; i++) {
            if (_array[i].value == v) {
                ret = true;
                break;
            }
        }
        return ret;
    }
    this.Add = function (k, v) {
        if (this.ContainsKey(k) == false) {
            var data = { key: k, value: v };
            _array.push(data);
            _count++;
        }
    };
    this.Clear = function () {
        _array = new Array();
        _count = 0;
    }
    this.RemoveDataByKey = function (k) {
        var array = new Array();
        if (this.ContainsKey(k) == true) {
            for (var i = 0; i < _array.length; i++) {
                if (_array[i].key != k) {
                    array.push(_array[i]);
                }
            }
        }
        _array = array;
        _count--;
    }
    this.SetItem = function (key, value) {
        if (this.ContainsKey(key) == false) {
            alert("no[" + key + "]key,cant insert into");
            return;
        }
        for (var i = 0; i < _array.length; i++) {
            if (_array[i].key == key) {
                _array[i].value = value;
                break;
            }
        }
    }
    this.GetItem = function (k) {
        for (var i = 0; i < _array.length; i++) {
            if (_array[i].key == k) {
                return _array[i].value;
            }
        }
        return null;
    }
    this.ToString = function () {
        var ret = "{";
        for (var i = 0; i < _array.length; i++) {
            if (i != _array.length - 1) {
                ret += "\"" + _array[i].key + "\":\"" + _array[i].value + "\","
            }
            else {
                ret += "\"" + _array[i].key + "\":\"" + _array[i].value + "\""
            }
        }
        ret += "}";
        return ret;
    }
    this.ToJson = function () {
        var str = this.ToString();
        var json = eval('(' + str + ')');
        return json;
    }
    this.GetArray = function () {
        return _array;
    }
}

var m = window.m || {};
m.map = m.map || {};
m.map.ceate = new m_map();