function LoadNav(sum, smallPageUrl, perCount, contentDomID) {
    pageNav.pre = '前一页';
    pageNav.next = '下一页';
    pageNav.fn = function (currentPageIndex, pageSum) {
        GetPage(currentPageIndex, smallPageUrl, perCount, contentDomID);
    };
    pageNav.go(1, sum);
}

function GetPage(currentPageIndex, smallPageUrl, perCount, contentDomID) {
    $.ajax({
        url: smallPageUrl,
        async: true,
        cache: false,
        data: { pageIndex: currentPageIndex, pageSize: perCount },
        success: function (html) {
            $('#' + contentDomID).html(html);
        }
    });
}

var m = window.m || {};
m.pager = m.pager || {};
m.pager.loadPager = function (sumDataCountUrl, smallPageUrl, perCount, contentDomID) {
    $.get(sumDataCountUrl, {}, function (ret) {
        var a = 0;
        if (ret % perCount == 0) {
            a = ret / perCount;
        } else {
            a = Math.floor(ret / perCount) + 1;
        }
        LoadNav(a, smallPageUrl, perCount, contentDomID);
    });
};
//http://www.cnblogs.com/humble/p/4887289.html




