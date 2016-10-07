/**
ajax提交数据到指定控制器方法
@baseUrl:如http://www.baidu.com,注意这种写法最后没有斜杆;
也可直接设置为'';
@controller:控制器名称,可以附带area,如:admin/home
@suffix:请求的后缀,如.htm
*/
function m_ajax(baseUrl, controller, suffix)
{
    this.baseUrl = baseUrl;
    this.controller = controller;
    this.suffix = suffix;

    this.getActionUrl = function (action) {
        var url = this.baseUrl + '/' + this.controller + '/' + action + this.suffix;
        return url;
    }

    this.post = function (action, data, backFunction) {
        var url = getActionUrl(action);
        $.post(url, data, function (ret) {
            backFunction(data);
        });
    }

    this.get = function (action, data, backFunction) {
        var url = getActionUrl(action);
        $.get(url, data, function (ret) {
            backFunction(data);
        });
    }

    this.postJson = function (action, data, backFunction) {
        var url = getActionUrl(action);
        $.post(url, data, function (ret) {
            var json = eval('(' + ret + ')');
            backFunction(json);
        });
    }

    this.getJson = function (action, data, backFunction) {
        var url = getActionUrl(action);
        $.get(url, data, function (ret) {
            var json = eval('(' + ret + ')');
            backFunction(json);
        });
    }

}
var m = window.m || {};
m.ajax = m.ajax || {};
/**
ajax提交数据到指定控制器方法
@baseUrl:如http://www.baidu.com,注意这种写法最后没有斜杆;
也可直接设置为'';
@controller:控制器名称,可以附带area,如:admin/home
@suffix:请求的后缀,如.htm,如果此参数不填就默认这个值
*/
m.ajax.create = function (baseUrl, controller, suffix)
{
    if (arguments.length == 2||suffix=='') {
        suffix = '.htm';
    }
    return new m_ajax(baseUrl, controller, suffix);
}
