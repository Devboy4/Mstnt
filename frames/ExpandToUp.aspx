<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExpandToUp.aspx.cs" Inherits="default_ExpandToUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body style="background-image: url('./../images/ustbody_bg.jpg'); vertical-align: top;
    margin-top: 0px; margin-left: 0px;">
    <form id="form1" runat="server">
        <div>
            <table border="0" cellpadding="0" cellspacing="0" style="height: 6px; width: 100%;">
                <tr valign="top">
                    <td valign="top" align="center">
                        <asp:Image runat="server" ImageUrl="~/images/ustbar_close.gif" ToolTip="Aç / Kapa"
                            ID="expandup" Style="cursor: pointer; position: absolute; top: 0;" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
