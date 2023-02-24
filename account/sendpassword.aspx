<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sendpassword.aspx.cs" Inherits="anonymous_sendpassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Þifremi Unuttum</title>
</head>
<body style="background-image: url(images/anabg.gif)">
    <form id="form1" runat="server">
    <div>
        &nbsp;<table style="width: 100%; height: 100%">
            <tr>
                <td align="center" valign="bottom">
                </td>
                <td align="center" valign="bottom">
                </td>
                <td align="center" valign="bottom">
                </td>
            </tr>
            <tr>
                <td align="center" valign="bottom">
                </td>
                <td align="center" valign="middle">
                    <table border="0" cellpadding="1" cellspacing="1" width="255px">
                        <tr style="background-color: Gray">
                            <td colspan="2" align="center">
                                <asp:Label ID="Label5" runat="server" ForeColor="white" Font-Bold="true">Þifre Hatýrlatma</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Font-Bold="true">Kullanýcý Adý</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Kullanici" runat="server" Width="140px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Literal ID="Mesaj" runat="server" Text=""></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="Degistir" runat="server" Text="Gönder" OnClick="Degistir_Click" />
                            </td>
                        </tr>
                    </table>
                    <%--<asp:PasswordRecovery ID="Passwordrecovery1" runat="server" MembershipProvider="SqlProvider"
                            Font-Names="Arial" Font-Size="12px" SubmitButtonText="Parolamý Gönder" SuccessText="Parolanýz size gönderildi !"
                            UserNameFailureText="Giriþ bilgilerinize ulaþýlamadý lütfen tekrar deneyiniz ! "
                            UserNameInstructionText="Lütfen kullanýcý adýnýzý giriniz" UserNameLabelText="Kullanýcý Adý:"
                            UserNameRequiredErrorMessage="Kullanýcý Adý girilmedi !" UserNameTitleText="">
                            <TextBoxStyle Font-Names="Arial" />
                            <SubmitButtonStyle Font-Names="Arial" Font-Size="12px" />
                            <MailDefinition IsBodyHtml="false" From="destek@marjinal.com.tr" Subject="Koza Mailing Þifre Hatýrlatma">
                            </MailDefinition>
                        </asp:PasswordRecovery>--%>
                </td>
                <td align="center" valign="bottom">
                </td>
            </tr>
            <tr>
                <td align="center" valign="bottom">
                </td>
                <td align="center" valign="bottom">
                </td>
                <td align="center" valign="bottom">
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
