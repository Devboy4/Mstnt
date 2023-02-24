<%@ Page Language="C#" AutoEventWireup="true" CodeFile="top.aspx.cs" Inherits="top"
     %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
html, body { height: 100%; margin: 0; padding: 0; color: #fff;}
a { color: #fff; }
#bg {position:fixed; top:0; left:0; width:100%; height:100%;}
#content {position:relative; z-index:1;}
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
</head>
<body>
    <form id="form1" runat="server">
        <img src="images/top_body_bg.jpg" alt="" id="bg" />
        <div id="content">
            <table border="0" cellpadding="0" cellspacing="1" width="100%">
                <tr>
                    <td style="width: 85%" valign="top">
                        <img src="images/deriden_logo.gif" style="padding-top: 3px" />
                    </td>
                    <td align="Right" valign="top" style="font-family: Arial; font-size: 12px; width: 20%;">
                        <table cellpadding="1" cellspacing="2" border="0" width="100%">
                            <tr>
                                <td align="center">
                                    <asp:Literal ID="ltr_Kullanici" runat="server" />
                                </td>
                                <td align="center">
                                    <table width="100%" cellpadding="0" cellspacing="2" border="0">
                                        <tr>
                                            <td>
                                                <asp:ImageButton ID="ButonCikis" OnClick="ButonCikis_Click" ImageUrl="images/nav_logout.gif"
                                                    runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td style="font-family: Arial; font-size: 12px; font-weight: bold">
                                                Çýkýþ</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
