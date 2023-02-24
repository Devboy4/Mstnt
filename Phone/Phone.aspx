<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Phone.aspx.cs" Inherits="Phone_Phone" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxCallback"
    TagPrefix="dxcb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="./IP_201312250908.js?v=2" type="text/javascript"></script>
    <meta name="google" value="notranslate" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <!-- Include CSS to eliminate any default margins/padding and set the height of the html element and 
             the body element to 100%, because Firefox, or any Gecko based browser, interprets percentage as 
             the percentage of the height of its parent container, which has to be set explicitly.  Fix for
             Firefox 3.6 focus border issues.  Initially, don't display flashContent div so it won't show 
             if JavaScript disabled.
        -->
    <style type="text/css" media="screen">
        html, body
        {
            height: 100%;
        }
        body
        {
            margin: 0;
            padding: 0;
            overflow: auto;
            text-align: center;
            background-color: #ffffff;
        }
        object:focus
        {
            outline: none;
        }
        #flashContent
        {
            display: none;
        }
        #keypad
        {
            margin: auto;
            margin-top: 20px;
        }
        
        #keypad tr td
        {
            vertical-align: middle;
            text-align: center;
            border: 1px solid #000000;
            font-size: 13px;
            font-weight: bold;
            width: 20px;
            height: 10px;
            cursor: pointer;
            background-color: #B8BCBF;
            color: #333333;
        }
        #keypad tr td:hover
        {
            background-color: #999999;
            color: #FFFF00;
        }
        .display
        {
            width: 130px;
            margin: 10px auto auto auto;
            background-color: #000000;
            color: #00FF00;
            font-size: 13px;
            border: 1px solid #999999;
        }
        #message
        {
            text-align: center;
            color: #009900;
            font-size: 14px;
            font-weight: bold;
            display: none;
        }
    </style>
    <script type="text/javascript" src="swfobject.js"></script>
    <script type="text/javascript">
        // For version detection, set to min. required Flash Player version, or 0 (or 0.0.0), for no version detection. 
        var swfVersionStr = "10.2.0";
        // To use express install, set to playerProductInstall.swf, otherwise the empty string. 
        var xiSwfUrlStr = "playerProductInstall.swf";
        var flashvars = {};
        var params = {};
        params.quality = "high";
        params.bgcolor = "#ffffff";
        params.allowscriptaccess = "sameDomain";
        params.allowfullscreen = "true";
        var attributes = {};
        attributes.id = "CwizAgentWebApplet";
        attributes.name = "CwizAgentWebApplet";
        attributes.align = "middle";
        swfobject.embedSWF(
                "CwizAgentWebApplet.swf", "flashContent",
                "193", "70",
                swfVersionStr, xiSwfUrlStr,
                flashvars, params, attributes);
        // JavaScript enabled so display the flashContent div in case it is not replaced with a swf object.
        swfobject.createCSS("#flashContent", "display:block;text-align:left;");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="IPUserName" runat="server" Value="" />
    <asp:HiddenField ID="IPPassword" runat="server" Value="" />
    <asp:HiddenField ID="IPAgentId" runat="server" Value="" />
    <dxcb:ASPxCallback ID="IPCallCallback" ClientInstanceName="IPCallCallback" runat="server"
        OnCallback="IPCallCallback_Callback">
    </dxcb:ASPxCallback>
    <dxcb:ASPxCallback ID="IPControlCallBack" ClientInstanceName="IPControlCallBack"
        runat="server" OnCallback="IPControlCallBack_Callback">
    </dxcb:ASPxCallback>
    <asp:Panel ID="pnlmultipleIpAccounts" runat="server" Visible="false">
        <dxe:ASPxComboBox ID="cmbIpAccounts" runat="server" ImageFolder="~/App_Themes/Glass/{0}/"
            CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css">
            <Items>
                <dxe:ListEditItem Text="910" Value="Taha.ozer1@cwiz.deriden.com|1234|910" />
                <dxe:ListEditItem Text="911" Value="Taha.ozer@cwiz.deriden.com|1234|911" />
                <dxe:ListEditItem Text="912" Value="Taha.ozer@cwiz.deriden.com|1234|912" />
            </Items>
            <ClientSideEvents SelectedIndexChanged="function(s, e) {SipLogin(s.GetValue()); }" />
        </dxe:ASPxComboBox>
    </asp:Panel>
    <div id="flashContent">
        <p>
            To view this page ensure that Adobe Flash Player version 10.2.0 or greater is installed.
        </p>
        <script type="text/javascript">
            var pageHost = ((document.location.protocol == "https:") ? "https://" : "http://");
            document.write("<a href='http://www.adobe.com/go/getflashplayer'><img src='"
                                + pageHost + "www.adobe.com/images/shared/download_buttons/get_flash_player.gif' alt='Get Adobe Flash player' /></a>"); 
        </script>
    </div>
    <noscript>
        <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="193" height="70"
            id="CwizAgentWebApplet">
            <param name="movie" value="CwizAgentWebApplet.swf" />
            <param name="quality" value="high" />
            <param name="bgcolor" value="#ffffff" />
            <param name="allowScriptAccess" value="sameDomain" />
            <param name="allowFullScreen" value="true" />
            <!--[if !IE]>-->
            <object type="application/x-shockwave-flash" data="CwizAgentWebApplet.swf" width="193"
                height="70">
                <param name="quality" value="high" />
                <param name="bgcolor" value="#ffffff" />
                <param name="allowScriptAccess" value="sameDomain" />
                <param name="allowFullScreen" value="true" />
                <!--<![endif]-->
                <!--[if gte IE 6]>-->
                <p>
                    Either scripts and active content are not permitted to run or Adobe Flash Player
                    version 10.2.0 or greater is not installed.
                </p>
                <!--<![endif]-->
                <a href="http://www.adobe.com/go/getflashplayer">
                    <img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif"
                        alt="Get Adobe Flash Player" />
                </a>
                <!--[if !IE]>-->
            </object>
            <!--<![endif]-->
        </object>
    </noscript>
    <br />
    <table id="keypad" cellpadding="5" cellspacing="3">
        <tr>
            <td onclick="addCode('1');">
                1
            </td>
            <td onclick="addCode('2');">
                2
            </td>
            <td onclick="addCode('3');">
                3
            </td>
        </tr>
        <tr>
            <td onclick="addCode('4');">
                4
            </td>
            <td onclick="addCode('5');">
                5
            </td>
            <td onclick="addCode('6');">
                6
            </td>
        </tr>
        <tr>
            <td onclick="addCode('7');">
                7
            </td>
            <td onclick="addCode('8');">
                8
            </td>
            <td onclick="addCode('9');">
                9
            </td>
        </tr>
        <tr>
            <td onclick="addCode('*');">
                *
            </td>
            <td onclick="addCode('0');">
                0
            </td>
            <td onclick="addCode('#');">
                #
            </td>
        </tr>
        <tr>
            <td>
                <img id="Img2" src="../images/Close_to_call.png" width="32px" height="32px" alt="Ara" />
            </td>
            <td onclick="clicktocall();">
                <img id="clicktocallbutton" src="../images/click_to_call.png" alt="Ara" />
            </td>
            <td onclick="backspace();">
                <img id="Img1" src="../images/backspace.png" alt="Ara" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <input runat="server" type="text" id="PhoneLine" value="" class="display" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
