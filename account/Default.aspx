<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Async="true"
    Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MONEY TRANSFER</title>
    <script>
        function addToSh() {
            var dpt = document.getElementById("lstSh");
            if (document.getElementById("txtHesap1").value == '') {
                document.getElementById("txtHesap1").value = dpt.options[dpt.selectedIndex].value;
            }
            else {
                document.getElementById("txtHesap1").value += ',' + dpt.options[dpt.selectedIndex].value;
            }
        }
        function addToAh() {
            var dpt = document.getElementById("lstAh");
            if (document.getElementById("txtHesap2").value == '') {
                document.getElementById("txtHesap2").value = dpt.options[dpt.selectedIndex].value;
            }
            else {
                document.getElementById("txtHesap2").value += ',' + dpt.options[dpt.selectedIndex].value;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="2" style="width: 750px; height: 100%;
            font-size: 11px; font-family: Arial">
            <tr>
                <td align="left" colspan="2">
                    <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="red" Font-Size="13px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Literal runat="server" ID="ltrnot" Text=""></asp:Literal>
                </td>
            </tr>
        </table>
        <br />
        <table border="0" cellpadding="0" cellspacing="2" style="width: 750px; height: 100%;
            font-size: 11px; font-family: Arial">
            <tr>
                <td style="width: 100px; font-weight: bold" valign="top" align="left">
                    İşlem Kodu
                </td>
                <td style="width: 650px" valign="top" align="left">
                    <asp:TextBox ID="TxtIslemKodu" runat="server" Width="100px" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 100px; font-weight: bold" valign="top" align="left">
                    Para cinsi
                </td>
                <td style="width: 650px" valign="top" align="left">
                    <asp:DropDownList ID="MoneyType" runat="server" Width="70px">
                        <asp:ListItem Selected="True" Text="USD" Value="USD"></asp:ListItem>
                        <asp:ListItem Text="EUR" Value="EUR"></asp:ListItem>
                        <asp:ListItem Text="TRL" Value="TRL"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 100px; font-weight: bold" valign="top" align="left">
                    Seçilen Hesap
                </td>
                <td style="width: 650px" valign="top" align="left">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td valign="top" style="width: 70px">
                                <asp:DropDownList runat="server" ID="lstSh" Width="100%">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 20px">
                                <img alt="ekle" id="img1" src="images/add_16.gif" onclick="addToSh()" />
                            </td>
                            <td valign="top" style="width: 560px">
                                <asp:TextBox ID="txtHesap1" runat="server" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 100px; font-weight: bold" valign="top" align="left">
                    Aktarılacak Hesap
                </td>
                <td style="width: 650px" valign="top" align="left">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td valign="top" style="width: 70px">
                                <asp:DropDownList runat="server" ID="lstAh" Width="100%">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 20px">
                                <img alt="ekle" id="img2" src="images/add_16.gif" onclick="addToAh()" />
                            </td>
                            <td valign="top" style="width: 560px">
                                <asp:TextBox ID="txtHesap2" runat="server" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 100px; font-weight: bold" valign="top" align="left">
                    Aktarılacak Mikar
                </td>
                <td style="width: 650px" valign="top" align="left">
                    <asp:TextBox ID="txtMiktar" runat="server" Width="100%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 100px; font-weight: bold" valign="top" align="left">
                    Mail Adresi
                </td>
                <td style="width: 650px" valign="top" align="left">
                    <asp:TextBox ID="txtAdres" runat="server" Width="100%" Text="info@advisedtrading.com;e.duzgun@atig.com.tr"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 100px" valign="top" align="left">
                </td>
                <td style="width: 650px" valign="top" align="left">
                    <asp:Button ID="btnSendPrevious" runat="server" Text="Gönderilecek Mesajı Aşağıda Gör"
                        OnClick="btnSendPrevious_Click" />
                    <asp:Button ID="btnSend" runat="server" Text="Gönder" OnClick="btnSend_Click" />
                    <asp:Button ID="btnRefresh" runat="server" Text="Sayfayı Yenile" OnClick="btnRefresh_Click" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Literal ID="ltrSendMessage" runat="server" Text=""></asp:Literal>
    </div>
    </form>
</body>
</html>
