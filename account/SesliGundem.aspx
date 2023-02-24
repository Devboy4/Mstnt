<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SesliGundem.aspx.cs" Inherits="account_SesliGundem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sesli Gündem</title>
    <script>
        function addToSU() {
            var lsu = document.getElementById("lstUsers");
            var lssu = document.getElementById("lstSelectedUser");
            AddOpt = new Option(lsu.options[lsu.selectedIndex].text, lsu.options[lsu.selectedIndex].value);

            lssu.options.add(AddOpt);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" style="width: 1550px; font-family: Calibri; font-size: 40px"
            cellspacing="0">
            <tr>
                <td valign="top" style="width: 1000px">
                    <table border="0" style="width: 100%;" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 475px">
                                <b>Kullanýcýlar</b>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 475px">
                                <asp:ListBox ID="lstUsers" SelectionMode="Multiple" Width="100%" Height="250px"
                                    runat="server"></asp:ListBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" style="width: 1550px; font-family: Calibri; font-size: 40px"
            cellspacing="0">
            <tr>
                <td valign="top" style="width: 550px">
                    <table border="0" style="width: 100%; height: 100%" cellpadding="0" cellspacing="10">
                        <tr>
                            <td colspan="2" style="width: 100%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px">
                                <b>Ses Seç</b>
                            </td>
                            <td style="width: 400px">
                                <asp:FileUpload runat="server" Width="400px" Font-Size="40px" ID="Seslifile" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px">
                                <b>Ýþlem Kodu</b>
                            </td>
                            <td style="width: 400px">
                                <asp:TextBox runat="server" Width="100%" Font-Size="40px" ID="txtpassword" TextMode="Password" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="bottom">
                    <asp:Button runat="server" ID="BtnSave" Text="Sesli Gündem Yükle" Width="1550px"
                        Height="70px" Font-Size="40px" OnClick="BtnSave_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
