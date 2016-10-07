
//提交表单 处理函数位置,表单ID,json数据,回调函数(注意要有name值)
function m_AjaxSubmit(action, formID, data, callback) {
    var frm = document.getElementById(formID);
    frm.method = "post";
    frm.encoding = "multipart/form-data";
    frm.action = action;
    var iframeID = formID + "_AjaxIFrame";
    frm.target = iframeID;
    var childnode = document.getElementById(iframeID);
    var ifrm;
    if (ifrm != undefined && ifrm != null) {
        frm.removeChild(childnode);
    }
    //-----------------------
    var isIE = navigator.appName.indexOf("Mic") != -1;
    ifrm = document.createElement("iframe");
    ifrm.id = iframeID;
    ifrm.name = iframeID;
    ifrm.style.display = "none";
    ifrm.target = "_parent";
    ifrm.name = iframeID;
    frm.appendChild(ifrm);
    if (isIE) {
        if (document.frames) {
            window.frames[iframeID.toString()].name = iframeID;
        }
    }
    var call = document.getElementById("callback");
    if (!call) {
        call = document.createElement("input");
    }
    call.name = "callback";
    call.id = "callback";
    call.value = callback;
    call.type = "hidden";
    frm.appendChild(call);
    //---------------------------------
    for (var name in data) {
        var input = document.getElementById(name);
        if (!input) {
            input = document.createElement("input");
        }
        input.name = name;
        input.id = name;
        input.value = data[name];
        input.type = "hidden";
        frm.appendChild(input);
    }
    frm.submit();
}
var m = window.m || {};
m.form = m.form || {};
m.form.ajaxSubmit = m_AjaxSubmit;