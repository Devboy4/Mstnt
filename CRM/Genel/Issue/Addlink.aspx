<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Addlink.aspx.cs" Inherits="CRM_Genel_Issue_Addlink" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>G�ndem Dosya Ekleme Mod�l�</title>
    <script src="../../../utils.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../../../ModelCRM.css" />
    <script src="../../crm.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="2" width="500">
            <tr>
                <td align="left" valign="top">
                    <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
                        CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
                        ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
                        ShowSubMenuShadow="False" Width="100px" AutoPostBack="True" OnItemClick="menu_ItemClick1">
                        <SubMenuStyle GutterWidth="0px" />
                        <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
                        <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
                        </SubMenuItemStyle>
                        <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                            X="2" Y="-12" />
                        <Items>
                            <dxm:MenuItem Name="save" Text="Link'i Ekle">
                                <Image Url="~/images/jpg2.gif" />
                            </dxm:MenuItem>
                        </Items>
                    </dxm:ASPxMenu>
                    <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="L�tfen bilgisayar�n�zdan veya a�daki bir makineden dosya se�iniz..."
                        Width="200px" BackColor="#EBF2F4" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                        CssPostfix="Glass">
                        <PanelCollection>
                            <dxrp:PanelContent ID="PanelContent1" runat="server">
                                <div style="text-align: left">
                                    <table border="0" cellpadding="0" cellspacing="1" style="width: 500px; height: 100%">
                                        <tr>
                                            <td colspan="2" align="left">                                              
                                                <asp:FileUpload ID="flp_logoyukle" runat="server" Width="97%" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </dxrp:PanelContent>
                        </PanelCollection>
                        <HeaderStyle BackColor="White" Height="23px">
                            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                            <BorderBottom BorderStyle="None" />
                        </HeaderStyle>
                        <TopRightCorner Height="5px" Url="~/App_Themes/Glass/Web/rpTopRightCorner.png" Width="5px" />
                        <HeaderContent>
                            <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpHeaderBack.gif" Repeat="RepeatX"
                                VerticalPosition="bottom" />
                        </HeaderContent>
                        <Content>
                            <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpContentBack.gif" Repeat="RepeatX"
                                VerticalPosition="bottom" />
                        </Content>
                        <BottomEdge BackColor="#D7E9F1">
                        </BottomEdge>
                        <HeaderLeftEdge>
                            <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpHeaderLeftEdge.gif" Repeat="RepeatX"
                                VerticalPosition="bottom" />
                        </HeaderLeftEdge>
                        <LeftEdge>
                            <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpLeftRightEdge.gif" Repeat="RepeatX"
                                VerticalPosition="bottom" />
                        </LeftEdge>
                        <TopLeftCorner Height="5px" Url="~/App_Themes/Glass/Web/rpTopLeftCorner.png" Width="5px" />
                        <BottomRightCorner Height="5px" Url="~/App_Themes/Glass/Web/rpBottomRightCorner.png"
                            Width="5px" />
                        <HeaderRightEdge>
                            <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpHeaderRightEdge.gif" VerticalPosition="bottom" />
                        </HeaderRightEdge>
                        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                        <NoHeaderTopRightCorner Height="5px" Url="~/App_Themes/Glass/Web/rpNoHeaderTopRightCorner.png"
                            Width="5px" />
                        <RightEdge>
                            <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpLeftRightEdge.gif" Repeat="RepeatX"
                                VerticalPosition="bottom" />
                        </RightEdge>
                        <NoHeaderTopEdge BackColor="#EBF2F4">
                        </NoHeaderTopEdge>
                        <BottomLeftCorner Height="5px" Url="~/App_Themes/Glass/Web/rpBottomLeftCorner.png"
                            Width="5px" />
                        <NoHeaderTopLeftCorner Height="5px" Url="~/App_Themes/Glass/Web/rpNoHeaderTopLeftCorner.png"
                            Width="5px" />
                        <Border BorderColor="#7EACB1" BorderStyle="Solid" BorderWidth="1px" />
                    </dxrp:ASPxRoundPanel>
                    <asp:HiddenField ID="hidden" runat="server" Visible="False" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
