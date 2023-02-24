<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login"
    UICulture="tr-TR" Culture="tr-TR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Deriden MSTNT CRM</title>
    <style type="text/css">
        #top
        {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
        }
        #bottom
        {
            position: absolute;
            bottom: 0;
            left: 0;
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="top">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td align="right" style="width: 100%; height: 60px; background-position: right center;
                    background-image: url(account/images/drdn_ustbg.jpg); background-repeat: repeat-x;
                    background-color: #ffffff;" valign="top">
                    <div style="text-align: right">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 60px;
                            background-color: transparent;">
                            <tr>
                                <td align="left" valign="middle">
                                    <img src="account/images/drdn_logo.jpg" />
                                </td>
                                <td align="right" valign="middle">
                                  <h2 style="color:White;font-family:Arial">GÜNDEM</h2>
                                   <%-- <img src="account/images/drdn_mstnt.jpg" />--%>
                                   <%-- &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="left: 35%; position: absolute; top: 30%">
        <asp:Login ID="CrmLogin" runat="server" BackColor="#EFF3FB" BorderColor="#B5C7DE"
            BorderPadding="4" BorderStyle="Solid" BorderWidth="0px" DestinationPageUrl="~/Default.aspx"
            FailureText="Giriþ Baþarýsýz" Font-Names="Verdana" Font-Size="10px" ForeColor="#333333"
            LoginButtonText="Giriþ" MembershipProvider="SqlProvider" OnLoggingIn="Login1_LoggingIn"
            PasswordLabelText="Parola" PasswordRecoveryText="Þifremi Unuttum" PasswordRecoveryUrl="~/account/sendpassword.aspx"
            PasswordRequiredErrorMessage="Þifre girmeniz gerekmektedir." RememberMeText="Beni Hatýrla"
            TitleText="MBS Bildirim Takibi Giriþ" UserNameLabelText="Kullanýcý" UserNameRequiredErrorMessage="Kullanýcý adý girmeniz gerekmektedir.">
            <TitleTextStyle BackColor="#507CD1" Font-Bold="True" Font-Size="10px" ForeColor="White" />
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            <TextBoxStyle Font-Size="10px" />
            <LoginButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px"
                Font-Names="Verdana" Font-Size="10px" ForeColor="#284E98" />
            <LayoutTemplate>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 453px">
                    <tr>
                        <td align="left" style="background-position: center center; font-weight: normal;
                            font-size: 12pt; background-image: url(account/images/drdn_ustbg.jpg); vertical-align: middle;
                            direction: ltr; background-repeat: repeat-x; font-family: Arial; height: 50px;
                            text-align: left; text-decoration: none" valign="middle">
                            <span style="color: #ffffff">&nbsp;&nbsp;&nbsp; CRM Giriþ</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="top">
                            <table border="0" cellpadding="0" style="font-size: 9pt; font-family: Arial; height: 50%;
                                background-color: #B8BCBF" width="100%">
                                <tr>
                                    <td align="left" style="width: 178px">
                                        Kullanýcý
                                    </td>
                                    <td align="left" style="font-size: 9pt; color: #0000ff; text-decoration: underline">
                                        <asp:TextBox ID="UserName" runat="server" Font-Size="10px" Width="167px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                            ErrorMessage="Kullanýcý adý girmeniz gerekmektedir." ToolTip="Kullanýcý adý girmeniz gerekmektedir."
                                            ValidationGroup="CrmLogin">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 178px">
                                        Þifre
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="Password" runat="server" Font-Size="10px" TextMode="Password" Width="167px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                            ErrorMessage="Þifre girmeniz gerekmektedir." ToolTip="Þifre girmeniz gerekmektedir."
                                            ValidationGroup="CrmLogin">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 178px">
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="RememberMe" runat="server" Text="Beni Hatýrla" />
                                        &nbsp; <a style="font-size: 10px;" href="account/sendpassword.aspx">Þifremi Unuttum</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 178px">
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="LoginButton" runat="server" BackColor="White" BorderColor="#507CD1"
                                            BorderStyle="Solid" BorderWidth="1px" CommandName="Login" Font-Names="Verdana"
                                            Font-Size="8pt" ForeColor="#284E98" Text="Giriþ" ValidationGroup="CrmLogin" />&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;<asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:Login>
    </div>
    <div id="bottom">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 70px;
            background-image: url(account/images/drdn_altbg.jpg); background-repeat: repeat-x">
            <tr>
                <td valign="middle" align="center" style="color: White; width: 100%; height: 100%;
                    font-size: 13px; font-weight: bold; font-family: Verdana">
                    deriden.com.tr
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
