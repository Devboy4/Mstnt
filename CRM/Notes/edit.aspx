<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="MarjinalCRM_Notes_edit" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Not</title>


    <script src="../../../utils.js" type="text/javascript"></script>

    <script src="../../crm.js" type="text/javascript"></script>

</head>
<body  topmargin="0" leftmargin="0">
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="HiddenID" Visible="false" />
        <asp:HiddenField runat="server" ID="BagliID" Visible="false" />
        <asp:HiddenField runat="server" ID="BagliNesneTipi" Visible="false" />
        <model:DataTable ID="DataTableNotDosya" runat="server" />
        <div>
            <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
                CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
                ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
                ShowSubMenuShadow="False" AutoPostBack="True" Width="150px">
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
                    <dxm:MenuItem Name="saveclose" Text="Kaydet ve Kapat">
                        <Image Url="~/images/saveclose.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="delete" Text="Sil">
                        <Image Url="~/images/delete.gif" />
                    </dxm:MenuItem>
                </Items>
            </dxm:ASPxMenu>
            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="" Width="500px"
                BackColor="#EBF2F4" CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <PanelCollection>
                    <dxrp:PanelContent ID="PanelContent1" runat="server">
                        <table width="500" cellpadding="1" cellspacing="1" border="0">
                            <tr>
                                <td colspan="1" style="width: 100px">
                                    <dxe:ASPxLabel ID="ASPxLabel12" runat="server" ForeColor="#C00000" Text="Tanim" />
                                </td>
                                <td colspan="1" style="width: 400px">
                                    <dxe:ASPxTextBox ID="Tanim" runat="server" Width="350px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1" style="width: 100px">
                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" ForeColor="Black" Text="Aciklama" />
                                </td>
                                <td colspan="1" style="width: 400px">
                                    <dxe:ASPxMemo ID="Aciklama" runat="server" Width="350px" Rows="5" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 500px">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Tanim"
                                        ErrorMessage="Taným alaný boþ ge&#231;ilemez!" Font-Bold="True" Font-Size="10px"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </dxrp:PanelContent>
                </PanelCollection>
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
                <HeaderStyle BackColor="White" Height="23px">
                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                    <BorderBottom BorderStyle="None" />
                </HeaderStyle>
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
            <table width="500" cellpadding="1" cellspacing="1" border="0">
                <tr>
                    <td>
                        <table width="500" cellpadding="1" cellspacing="1" border="0">
                            <tr>
                                <td colspan="1" style="width: 100px">
                                    <dxe:ASPxLabel ID="ASPxLabel3" runat="server" ForeColor="Black" Text="Dosya" Width="100px" />
                                </td>
                                <td colspan="1" style="width: 350px">
                                    <asp:FileUpload ID="fileUpload" runat="server" Width="350px" />
                                </td>
                                <td colspan="1" style="width: 50px">
                                    <asp:Button ID="BtnDosyaEkle" runat="server" Text="Ekle" OnClick="BtnDosyaEkle_Click"
                                        Width="50px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="" Width="500px"
                BackColor="#EBF2F4" CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <PanelCollection>
                    <dxrp:PanelContent ID="PanelContent2" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" style="height: 150px">
                            <tr>
                                <td valign="top">
                                    <dxwgv:ASPxGridView ID="GridNotDosya" runat="server" AutoGenerateColumns="False"
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" DataSourceID="DataTableNotDosya"
                                        KeyFieldName="ID" Width="500px">
                                        <Columns>
                                            <dxwgv:GridViewCommandColumn Width="30px" VisibleIndex="0" ButtonType="Image">
                                                <DeleteButton Visible="True" Text="Sil">
                                                    <Image AlternateText="Sil" Url="~/images/delete.gif" />
                                                </DeleteButton>
                                            </dxwgv:GridViewCommandColumn>
                                            <dxwgv:GridViewDataColumn FieldName="ID" Visible="False" />
                                            <dxwgv:GridViewDataColumn FieldName="NotDosyaID" Visible="False" />
                                            <dxwgv:GridViewDataColumn FieldName="NotID" Visible="False" />
                                            <dxwgv:GridViewDataTextColumn Caption="Dosya" Name="NotDosya" Width="25%">
                                                <DataItemTemplate>
                                                    <asp:Literal runat="server" ID="LiteralNotDosya" Text='<%# Eval("Link") %>'></asp:Literal>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataColumn Caption="Dosya Boyutu" FieldName="DosyaBoyut" Width="70px" />
                                            <dxwgv:GridViewDataColumn Caption="Boyut Türü" FieldName="BoyutTuru" Width="60px" />
                                            <dxwgv:GridViewDataColumn Caption="Dosya Yolu" FieldName="DosyaYolu" Visible="False" />
                                            <dxwgv:GridViewDataColumn Caption="Oluþturma Tarihi" FieldName="CreationDate" Width="90px" />
                                            <dxwgv:GridViewDataColumn FieldName="AllowedRoles" Visible="false" />
                                            <dxwgv:GridViewDataColumn FieldName="DeniedRoles" Visible="false" />
                                            <dxwgv:GridViewDataColumn FieldName="Filter" UnboundType="String" Visible="false" />
                                        </Columns>
                                        <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                        </Images>
                                        <Settings ShowFilterRow="False" ShowStatusBar="Hidden" ShowGroupedColumns="False"
                                            ShowGroupPanel="False" ShowPreview="False" ShowTitlePanel="False" ShowVerticalScrollBar="False" />
                                        <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                            </Header>
                                            <AlternatingRow Enabled="True" />
                                        </Styles>
                                        <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                                        <SettingsPager PageSize="5" ShowSeparators="True">
                                        </SettingsPager>
                                        <SettingsCustomizationWindow Enabled="True" />
                                        <SettingsText Title="Dosyalar" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya s&#252;r&#252;kleyiniz."
                                            ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Kayýt yok" />
                                    </dxwgv:ASPxGridView>
                                </td>
                            </tr>
                        </table>
                    </dxrp:PanelContent>
                </PanelCollection>
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
                <HeaderStyle BackColor="White" Height="23px">
                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                    <BorderBottom BorderStyle="None" />
                </HeaderStyle>
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
        </div>
    </form>
</body>
</html>
