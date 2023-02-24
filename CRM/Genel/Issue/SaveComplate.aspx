<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaveComplate.aspx.cs" Inherits="CRM_Genel_Issue_SaveComplate" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>


<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../../ModelCRM.css" />
    <link rel="stylesheet" type="text/css" href="../../../PreLoad.css" />

    <script src="../../../PreLoad.js" type="text/javascript"></script>

    <script src="../../../utils.js" type="text/javascript"></script>

    <script src="./crm_20141215.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                <tr>
                    <td style="width: 50px" valign="top" align="center">
                        <img src="../../../images/info_16.gif" /></td>
                    <td style="width: 450px" valign="top">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 450px">
                            <tr>
                                <td style="width: 100%" align="left" colspan="2">
                                    <span style="font-weight: bold; color: lime">Gündem Kayýt Ýþlemi Baþarýlý...</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px" align="left">
                                    Bildirim Baþlýk</td>
                                <td style="width: 350px" align="left">
                                    <dxe:ASPxLabel ID="SaveText" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="8pt">
                                    </dxe:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px" align="left">
                                    Bildirim Link</td>
                                <td style="width: 350px" align="left" valign="top">
                                    <asp:Literal ID="SonBildirim" runat="server"></asp:Literal></td>
                            </tr>
                            <tr>
                                <td style="width: 100px" align="left">
                                    Dosya Link</td>
                                <td style="width: 350px" align="left" valign="top">
                                    <asp:Literal ID="DosyaLink" runat="server"></asp:Literal></td>
                            </tr>
                        </table>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
