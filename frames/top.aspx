<%@ Page Language="C#" AutoEventWireup="true" CodeFile="top.aspx.cs" Inherits="top" %>

<%@ Register Src="~/controls/olaylistesi.ascx" TagName="olaylistesi" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        html, body
        {
            height: 100%;
            margin: 0;
            padding: 0;
            color: #fff;
        }
        a
        {
            color: #fff;
        }
        #bg
        {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
        }
        #content
        {
            position: relative;
            z-index: 1;
        }
    </style>
    <!--[if IE 6]>
<style type="text/css">
/* some css fixes for IE browsers */
html {overflow-y:hidden;}
body {overflow-y:auto;}
#bg {position:absolute; z-index:-1;}
#content {position:static;}
</style>
<![endif]-->
    <script language="javascript">
        function ControlCalBack() {
            if (parent.frames['content'].CallbackPreview2 != null) {
                parent.frames['content'].CallbackPreview2.PerformCallback();
            }
        }
    </script>
</head>
<body style="background-image: url(../images/drdn_ustbg.jpg); background-repeat: repeat-x">
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width: 20%" valign="middle">
                <img src="../images/drdn_logo.jpg" style="padding-top: 10px" />
            </td>
            <td align="left" style="width: 60%" valign="middle">
                <asp:olaylistesi ID="olaylistesi1" runat="server" />
            </td>
            <td align="right" valign="middle" style="width: 20%">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td align="right" valign="middle">
                            <%-- &nbsp;<br />--%>
                            <%-- <img src="../images/drdn_mstnt.jpg" />--%>
                            <h2 style="color: White; font-family: Arial">
                                GÜNDEM</h2>
                            <%--  &nbsp;&nbsp;&nbsp;&nbsp;--%>
                        </td>
                        <td align="right">
                            <asp:Literal ID="ltr_Kullanici" runat="server" />
                        </td>
                        <td align="right">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="ButonCikis" OnClick="ButonCikis_Click" ImageUrl="../images/1398608023_exit.png"
                                            runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
