



 <%@  Language="C#" Inherits="Moon.Web.MoonBasePage"
    MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content ID="TitleContent1" ContentPlaceHolderID="TitleContent" runat="server">
    设置中心--站点设置
</asp:Content>
<asp:Content ID="TitleH1Content1" ContentPlaceHolderID="TitleH1Content" runat="server">
    设置中心--站点设置
</asp:Content>
<asp:Content ID="PageIDContent1" ContentPlaceHolderID="PageIDContent" runat="server">
     id="pageAccountConfig"
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
	 
<script>
     function submitform(){
     	var data=$("#formSiteConfig").serializeArray();
     	data[5].value=htmlEncode(data[5].value);
     	var value=data["user_site.desription"];
        
     	$.post("/Admin/SiteConfigDO.ajax",data,function(ret){
     			 
     	});
     }
</script>

</asp:Content>
<asp:Content ID="SecondMenuContent1" ContentPlaceHolderID="SecondMenuContent" runat="server">
    <li><a data-ajax="false" href="<%=UrlUtil.Action2<AdminController>("AccountConfig")%>">账号设置</a></li>
    <li><a data-ajax="false" href="<%=UrlUtil.Action2<AdminController>("SiteConfig")%>" class="ui-btn-active">站点设置</a></li>
    <li><a data-ajax="false" href="<%=UrlUtil.Action2<AdminController>("FrontConfig")%>">前台设置</a></li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div data-role="content" data-theme="a">
       <style>
           textarea.ui-input-text{min-height:100px;}
       </style>
        	 <%var model=Model<user_site>();%>
           <form method="post" id="formSiteConfig">
                <div data-role="fieldcontain"  >
                     <%=MHtml.HiddenFor(model.user_id,"user_id" )%>
                     <%=MHtml.LableFor("站点名：","site_name")%>
                     <%=MHtml.TextBoxFor(model.site_name,"site_name","placeholder='给你的站点设置一个名字'",true )%>
                     <%=MHtml.LableFor("域名：","domain")%>
                     <%=MHtml.TextBoxFor(model.domain,"domain",model.user_id<=0?"":"readonly='readonly'  placeholder='(四个字母以上,如abcd,不要写成baidu.com这样的,这里针对于系统而言),仅此一次设置机会'",true)%>
                     <%=MHtml.LableFor("访问密码：","password")%>
                     <%=MHtml.TextBoxFor(model.domain,"password","placeholder='此站点的访问密码(非朋友所用)'",true)%>
                     <%=MHtml.LableFor("可访问性：","open_state")%>
                      <fieldset  data-role="controlgroup" data-type="horizontal" >
                      <%=MHtml.RadioGroupFor(model.open_state,"open_state",typeof(WebOpenState))%>
                       </fieldset >
                     <%=MHtml.LableFor("站点简介：","desription")%>
                     <%=MHtml.TextAreaFor(model.desription,"desription","placeholder='介绍一下你的站点' ",true)%>
                     
                    
                </div>
          </form>
          
        </div>
       <div data-role="controlgroup" data-type="horizontal" data-mini="true"  style='text-align:center;padding:20px 0;'>
            <input id="btnSiteConfig2" <%=MHtml.SubmitTo<AdminController>("SiteConfigDO","formSiteConfig","commonDialog('提示','设置成功',function(){return true;})","")%>   type="button" data-role="button" value="确定" >
             <input type='button' onclick='submitform();' value='test'/>
        </div>
</asp:Content>

