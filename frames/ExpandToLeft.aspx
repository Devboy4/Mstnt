<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExpandToLeft.aspx.cs" Inherits="default_ExpandToLeft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body style="background-image: url('./../images/body_bg.gif'); vertical-align: middle;
    margin-top: 0px; margin-left: 0px;">
    <form id="form1" runat="server">
        <div>
            <table border="0" cellpadding="0" cellspacing="0" style="height: 500px; width: 6px;">
                <tr>
                    <td height="100%" valign="middle">
                        <asp:Image runat="server" ImageUrl="~/images/bar_close.gif" ToolTip="Aç / Kapa" ID="expandleft"
                            Style="cursor: pointer;" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
