<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="changepassword.aspx.cs"
    Inherits="account_changepassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <asp:Panel ID="Panel1" runat="server" BorderWidth="1" Width="260px">
            <table border="0" cellpadding="1" cellspacing="1" width="255px">
                <tr style="background-color: Gray">
                    <td colspan="2" align="center">
                        <asp:Label ID="Label5" runat="server" ForeColor="white" Font-Bold="true">Þifre Deðiþtirme</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        <asp:Label ID="Label1" runat="server" ForeColor="red" Font-Bold="true">Eski Þifre</asp:Label>
                    </td>
                    <td style="width: 150px">
                        <asp:TextBox ID="EskiSifre" runat="server" TextMode="Password" Width="140px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" ForeColor="red" Font-Bold="true">Yeni Þifre</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="YeniSifre" runat="server" TextMode="Password" Width="140px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" ForeColor="red" Font-Bold="true">Yeni Þifre Tekrar</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="YeniSifreTekrar" runat="server" TextMode="Password" Width="140px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Literal ID="Mesaj" runat="server" Text=""></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="Degistir" runat="server" Text="Deðiþtir" OnClick="Degistir_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
