<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="CRM_Genel_Issues_Default" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="System.Web.UI" TagPrefix="cc1" %>


<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxDataView"
    TagPrefix="dxdv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<%@ Register Src="~/controls/NotGrid.ascx" TagName="NotGrid" TagPrefix="model" %>
<%@ Register Src="~/controls/NotGrid.ascx" TagName="Not" TagPrefix="Model" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    
    
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../../ModelCRM.css" />
    <link rel="stylesheet" type="text/css" href="../../../PreLoad.css" />

    <script src="../../../PreLoad.js" type="text/javascript"></script>

    <script src="../../../utils.js" type="text/javascript"></script>

    <script src="./crm_20141215.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            
            <asp:HiddenField ID="HiddenID" runat="server" />
            <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
                CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
                ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
                ShowSubMenuShadow="False">
                <SubMenuStyle GutterWidth="0px" />
                <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
                <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
                </SubMenuItemStyle>
                
                <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                    X="2" Y="-12" />
                <Items>
                    <dxm:MenuItem Name="save" Text="Kaydet">
                        <Image Url="~/images/save.gif" />
                    </dxm:MenuItem>
                </Items>
            </dxm:ASPxMenu>
            <dxtc:ASPxPageControl ID="ASPxPageControl1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" ActiveTabIndex="0" TabSpacing="0px"
                Width="800px">
                <ContentStyle>
                    <Border BorderColor="#4986A2" />
                </ContentStyle>
                <TabPages>
                    <dxtc:TabPage Name="Genel" Text="Genel">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <dxrp:ASPxRoundPanel BackColor="#EBF2F4" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                    CssPostfix="Glass" ID="ASPxRoundPanel1" runat="server" Width="800px" HeaderText=""
                                    ShowHeader="False">
                                    <PanelCollection>
                                        <dxp:PanelContent runat="server">
                                            <table style="width: 100%" cellspacing="3" cellpadding="0" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 321px" valign="top">
                                                            Oluþturan Kiþi</td>
                                                        <td colspan="3">
                                                            <dxe:ASPxLabel ID="IssuedBy" runat="server">
                                                            </dxe:ASPxLabel>
                                                        </td>
                                                        <td colspan="1" valign="top">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 321px" valign="top">
                                                            Baþlýk</td>
                                                        <td colspan="3">
                                                            <dxe:ASPxTextBox runat="server" Width="522px" ID="Title">
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td colspan="1" valign="top">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Title"
                                                                ErrorMessage="Boþ Ge&#231;ilemez" Font-Bold="False" Font-Size="10px"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 321px" valign="top">
                                                            <span style="color: #000000">Açýklama</span></td>
                                                        <td colspan="3">
                                                            <dxe:ASPxMemo ID="Description" runat="server" Height="80px" Width="522px">
                                                            </dxe:ASPxMemo>
                                                        </td>
                                                        <td colspan="1" valign="top">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Description"
                                                                ErrorMessage="Boþ Ge&#231;ilemez" Font-Bold="False" Font-Size="10px"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 321px" valign="top">
                                                            Hata Adýmlarý</td>
                                                        <td colspan="3">
                                                            <dxe:ASPxMemo ID="HataAdimlari" runat="server" Height="80px" Width="522px">
                                                            </dxe:ASPxMemo>
                                                        </td>
                                                        <td colspan="1" valign="top">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 321px" valign="top">
                                                            Anahtar Kelimeler</td>
                                                        <td colspan="3">
                                                            <dxe:ASPxTextBox ID="KeyWords" runat="server" Width="522px">
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td colspan="1" valign="top">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 321px" valign="top">
                                                            Yorumlar</td>
                                                        <td colspan="3">
                                                            <dxe:ASPxMemo ID="Comments" runat="server" Height="80px" Width="522px">
                                                            </dxe:ASPxMemo>
                                                        </td>
                                                        <td colspan="1" valign="top">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 321px" valign="top">
                                                            <span style="color: #000000">Müþteri</span></td>
                                                        <td style="width: 283px">
                                                            <dxe:ASPxComboBox runat="server" ValueType="System.String" ImageFolder="~/App_Themes/Glass/{0}/"
                                                                CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="170px"
                                                                ID="FirmaID" EnableIncrementalFiltering="True" ClientInstanceName="cmbFirmaID">
                                                                <ButtonStyle Width="13px" Cursor="pointer">
                                                                </ButtonStyle>
                                                                <ClientSideEvents SelectedIndexChanged="function(s,e) {OnFirmaIDChanged(s);}" />
                                                            </dxe:ASPxComboBox>
                                                        </td>
                                                        <td style="width: 186px" valign="top">
                                                            <span style="color: #000000">Proje</span></td>
                                                        <td style="width: 282px">
                                                            <dxe:ASPxComboBox runat="server" ValueType="System.String" ImageFolder="~/App_Themes/Glass/{0}/"
                                                                CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="170px"
                                                                ID="ProjeID" EnableIncrementalFiltering="True" ClientInstanceName="cmbProjeID"
                                                                OnCallback="ProjeID_Callback">
                                                                <ButtonStyle Width="13px" Cursor="pointer">
                                                                </ButtonStyle>
                                                            </dxe:ASPxComboBox>
                                                        </td>
                                                        <td style="width: 150px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 321px" valign="top">
                                                            <span style="color: #000000">Bildirim Durumu</span></td>
                                                        <td style="width: 283px">
                                                            <dxe:ASPxComboBox runat="server" ValueType="System.String" ImageFolder="~/App_Themes/Glass/{0}/"
                                                                CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="170px"
                                                                ID="DurumID" EnableIncrementalFiltering="True">
                                                                <ButtonStyle Width="13px" Cursor="pointer">
                                                                </ButtonStyle>
                                                            </dxe:ASPxComboBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="DurumID"
                                                                ErrorMessage="Bildirim Durumu Boþ Ge&#231;ilemez" Font-Bold="False" Font-Size="10px"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td style="width: 186px" valign="top">
                                                            Önem Derecesi</td>
                                                        <td style="width: 282px" valign="top">
                                                            <dxe:ASPxComboBox runat="server" ValueType="System.String" ImageFolder="~/App_Themes/Glass/{0}/"
                                                                CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="170px"
                                                                ID="OnemDereceID" EnableIncrementalFiltering="True">
                                                                <ButtonStyle Width="13px" Cursor="pointer">
                                                                </ButtonStyle>
                                                            </dxe:ASPxComboBox>
                                                        </td>
                                                        <td style="width: 150px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 321px" valign="top">
                                                            Bildirim Tarihi</td>
                                                        <td style="width: 283px">
                                                            <dxe:ASPxDateEdit ID="BildirimTarihi" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                                CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                                                <CalendarProperties TodayButtonText="Bugün" ClearButtonText="Temizle" FastNavProperties-CancelButtonText="Ýptal"
                                                                    FastNavProperties-OkButtonText="Seç">
                                                                    <FooterStyle Spacing="4px"></FooterStyle>
                                                                    <HeaderStyle Spacing="1px"></HeaderStyle>
                                                                </CalendarProperties>
                                                                <ButtonStyle Cursor="pointer" Width="13px">
                                                                </ButtonStyle>
                                                            </dxe:ASPxDateEdit>
                                                        </td>
                                                        <td style="width: 186px" valign="top">
                                                            Bildirim Teslim Tarihi</td>
                                                        <td style="width: 282px" valign="top">
                                                            <dxe:ASPxDateEdit ID="TeslimTarihi" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                                CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                                                <CalendarProperties TodayButtonText="Bugün" ClearButtonText="Temizle" FastNavProperties-CancelButtonText="Ýptal"
                                                                    FastNavProperties-OkButtonText="Seç">
                                                                    <FooterStyle Spacing="4px"></FooterStyle>
                                                                    <HeaderStyle Spacing="1px"></HeaderStyle>
                                                                </CalendarProperties>
                                                                <ButtonStyle Cursor="pointer" Width="13px">
                                                                </ButtonStyle>
                                                            </dxe:ASPxDateEdit>
                                                        </td>
                                                        <td style="width: 150px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 321px" valign="top">
                                                            <span style="color: #000000">Bildirime Kiþi Ata</span></td>
                                                        <td style="width: 283px">
                                                            <dxe:ASPxComboBox runat="server" ValueType="System.String" ImageFolder="~/App_Themes/Glass/{0}/"
                                                                CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="170px"
                                                                ID="UserID" EnableIncrementalFiltering="True">
                                                                <ButtonStyle Width="13px" Cursor="pointer">
                                                                </ButtonStyle>
                                                            </dxe:ASPxComboBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="UserID"
                                                                ErrorMessage="Bildirime Kiþi Atamak Zorunludur" Font-Bold="False" Font-Size="10px"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td style="width: 186px" valign="top">
                                                            Bildirim Aktif / Pasif</td>
                                                        <td style="width: 282px" valign="top">
                                                            <dxe:ASPxCheckBox ID="Active" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                                CssPostfix="Glass" Text="Aktif">
                                                            </dxe:ASPxCheckBox>
                                                        </td>
                                                        <td style="width: 150px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 321px" valign="top">
                                                            Hata Tipi</td>
                                                        <td style="width: 283px">
                                                            <dxe:ASPxComboBox runat="server" ValueType="System.String" ImageFolder="~/App_Themes/Glass/{0}/"
                                                                CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="170px"
                                                                ID="HataTipID" EnableIncrementalFiltering="True">
                                                                <ButtonStyle Width="13px" Cursor="pointer">
                                                                </ButtonStyle>
                                                            </dxe:ASPxComboBox>
                                                        </td>
                                                        <td style="width: 186px" valign="top">
                                                        </td>
                                                        <td style="width: 282px" valign="top">
                                                        </td>
                                                        <td style="width: 150px">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </dxp:PanelContent>
                                    </PanelCollection>
                                    <BottomLeftCorner Height="5px" Url="~/App_Themes/Glass/Web/rpBottomLeftCorner.png"
                                        Width="5px" />
                                    <TopRightCorner Height="5px" Url="~/App_Themes/Glass/Web/rpTopRightCorner.png" Width="5px" />
                                    <BottomRightCorner Height="5px" Url="~/App_Themes/Glass/Web/rpBottomRightCorner.png"
                                        Width="5px" />
                                    <Content>
                                        <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpContentBack.gif" Repeat="RepeatX"
                                            VerticalPosition="bottom" />
                                    </Content>
                                    <NoHeaderTopEdge BackColor="#EBF2F4">
                                    </NoHeaderTopEdge>
                                    <HeaderLeftEdge>
                                        <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpHeaderLeftEdge.gif" Repeat="RepeatX"
                                            VerticalPosition="bottom" />
                                    </HeaderLeftEdge>
                                    <NoHeaderTopRightCorner Height="5px" Url="~/App_Themes/Glass/Web/rpNoHeaderTopRightCorner.png"
                                        Width="5px" />
                                    <RightEdge>
                                        <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpLeftRightEdge.gif" Repeat="RepeatX"
                                            VerticalPosition="bottom" />
                                    </RightEdge>
                                    <HeaderContent>
                                        <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpHeaderBack.gif" Repeat="RepeatX"
                                            VerticalPosition="bottom" />
                                    </HeaderContent>
                                    <BottomEdge BackColor="#D7E9F1">
                                    </BottomEdge>
                                    <TopLeftCorner Height="5px" Url="~/App_Themes/Glass/Web/rpTopLeftCorner.png" Width="5px" />
                                    <LeftEdge>
                                        <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpLeftRightEdge.gif" Repeat="RepeatX"
                                            VerticalPosition="bottom" />
                                    </LeftEdge>
                                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                    <NoHeaderTopLeftCorner Height="5px" Url="~/App_Themes/Glass/Web/rpNoHeaderTopLeftCorner.png"
                                        Width="5px" />
                                    <HeaderRightEdge>
                                        <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpHeaderRightEdge.gif" VerticalPosition="bottom" />
                                    </HeaderRightEdge>
                                    <Border BorderColor="#7EACB1" BorderStyle="Solid" BorderWidth="1px" />
                                    <HeaderStyle BackColor="White" Height="23px">
                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                        <BorderBottom BorderStyle="None" />
                                    </HeaderStyle>
                                </dxrp:ASPxRoundPanel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <dxtc:TabPage Name="Kayitlar" Text="Bildirim Kayýtlarý">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <dxrp:ASPxRoundPanel BackColor="#EBF2F4" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                    CssPostfix="Glass" ID="ASPxRoundPanel2" runat="server" Width="1000px" HeaderText="Bildirim Kayýtlarý">
                                    <PanelCollection>
                                        <dxp:PanelContent runat="server">
                                        </dxp:PanelContent>
                                    </PanelCollection>
                                    <BottomLeftCorner Height="5px" Url="~/App_Themes/Glass/Web/rpBottomLeftCorner.png"
                                        Width="5px" />
                                    <TopRightCorner Height="5px" Url="~/App_Themes/Glass/Web/rpTopRightCorner.png" Width="5px" />
                                    <BottomRightCorner Height="5px" Url="~/App_Themes/Glass/Web/rpBottomRightCorner.png"
                                        Width="5px" />
                                    <Content>
                                        <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpContentBack.gif" Repeat="RepeatX"
                                            VerticalPosition="bottom" />
                                    </Content>
                                    <NoHeaderTopEdge BackColor="#EBF2F4">
                                    </NoHeaderTopEdge>
                                    <HeaderLeftEdge>
                                        <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpHeaderLeftEdge.gif" Repeat="RepeatX"
                                            VerticalPosition="bottom" />
                                    </HeaderLeftEdge>
                                    <NoHeaderTopRightCorner Height="5px" Url="~/App_Themes/Glass/Web/rpNoHeaderTopRightCorner.png"
                                        Width="5px" />
                                    <RightEdge>
                                        <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpLeftRightEdge.gif" Repeat="RepeatX"
                                            VerticalPosition="bottom" />
                                    </RightEdge>
                                    <HeaderContent>
                                        <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpHeaderBack.gif" Repeat="RepeatX"
                                            VerticalPosition="bottom" />
                                    </HeaderContent>
                                    <BottomEdge BackColor="#D7E9F1">
                                    </BottomEdge>
                                    <TopLeftCorner Height="5px" Url="~/App_Themes/Glass/Web/rpTopLeftCorner.png" Width="5px" />
                                    <LeftEdge>
                                        <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpLeftRightEdge.gif" Repeat="RepeatX"
                                            VerticalPosition="bottom" />
                                    </LeftEdge>
                                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                    <NoHeaderTopLeftCorner Height="5px" Url="~/App_Themes/Glass/Web/rpNoHeaderTopLeftCorner.png"
                                        Width="5px" />
                                    <HeaderRightEdge>
                                        <BackgroundImage ImageUrl="~/App_Themes/Glass/Web/rpHeaderRightEdge.gif" VerticalPosition="bottom" />
                                    </HeaderRightEdge>
                                    <Border BorderColor="#7EACB1" BorderStyle="Solid" BorderWidth="1px" />
                                    <HeaderStyle BackColor="White" Height="23px">
                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                        <BorderBottom BorderStyle="None" />
                                    </HeaderStyle>
                                </dxrp:ASPxRoundPanel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <dxtc:TabPage Name="Not" Text="Not">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                </TabPages>
                <TabStyle HorizontalAlign="Center">
                </TabStyle>
                <Paddings PaddingLeft="0px" />
            </dxtc:ASPxPageControl>
        </div>
    </form>
</body>
</html>
