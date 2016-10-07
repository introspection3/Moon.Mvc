//获取当前年月日 2011-12-3
function m_getCurrentDateString() {
    var now = new Date();
    var year = now.getFullYear();
    var month = now.getMonth();
    var day = now.getDate();
    var strNow = year + "-" + (month + 1) + "-" + day;
    return strNow;
}
//获取当前时间 2011-12-3 12:56:07
function m_getDateTimeNow() {
    var now = new Date();
    var hours = now.getHours();
    var minutes = now.getMinutes();
    var seconds = now.getSeconds();
    return m_getCurrentDateString() + " " + hours + ":" + minutes + ":" + seconds;
}
//是否是时间格式
function m_isDateFormate(str) {
    var pattern = /^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$/;
    if (!pattern.exec(str)) {
        return false;
    }
    return true;
}

var m = window.m || {};
m.time = m.time || {};
m.time.getCurrentDateString = m_getCurrentDateString;
m.time.getDateTimeNow = m_getDateTimeNow;
m.time.isDateFormate = m_isDateFormate;
