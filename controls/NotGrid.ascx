<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NotGrid.ascx.cs" Inherits="controls_NotGrid" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<model:DataTable ID="DataTableNotes" runat="server" />
<dxrp:ASPxRoundPanel ID="ASPxRoundPanelNot" runat="server" HeaderText="Not Yaz" Width="100%"
    BackColor="#EBF2F4" CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
    <PanelCollection>
        <dxrp:PanelContent ID="PanelContent5" runat="server">
            <dxwgv:ASPxGridView ID="GridNot" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" KeyFieldName="NotID" Width="100%" SettingsEditing-Mode="Inline"
                OnHtmlDataCellPrepared="GridNot_HtmlDataCellPrepared">
                <Columns>
                    <dxwgv:GridViewDataColumn FieldName="ID" Visible="False" />
                    <dxwgv:GridViewDataColumn FieldName="NotID" Visible="False" />
                    <dxwgv:GridViewDataColumn FieldName="BagliID" Visible="False" />
                    <dxwgv:GridViewDataColumn FieldName="BagliNesneTipi" Visible="False" />
                    <dxwgv:GridViewDataColumn Caption="Tanım" FieldName="Tanim" Width="20%">
                        <DataItemTemplate>
                            <dxe:ASPxHyperLink ID="ASPxHyperLinkNot" CssClass="dxeBase" runat="server" NavigateUrl=<%#"JavaScript:PopWin=OpenPopupWinBySize('../../Notes/edit.aspx?id=" + Eval("NotID") + "&BagliID=" + Eval("BagliID") + "&Tip=" + Eval("BagliID") + "','540','420');PopWin.focus();"%>
                                Text='<%#Eval("Tanim") %>'>
                            </dxe:ASPxHyperLink>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataMemoColumn Caption="Açıklama" FieldName="Aciklama" Width="25%" />
                    <dxwgv:GridViewDataMemoColumn Caption="Ekleyen" FieldName="CreatedBy" Width="15%" />
                    <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Oluşturma Tarihi"
                        FieldName="CreationDate" Width="15%" />
                    <dxwgv:GridViewDataTextColumn Caption="Dosyalar" Name="NotDosya" Width="25%">
                        <DataItemTemplate>
                            <asp:Label ID="LabelNotID" Visible="false" runat="server" Text='<%# Eval("NotID") %>'></asp:Label>
                            <asp:Literal runat="server" ID="LiteralNotDosyalar" Text=""></asp:Literal>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataColumn FieldName="Filter" UnboundType="String" Visible="False" />
                </Columns>
                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                </Images>
                <Settings ShowFilterRow="False" ShowStatusBar="Hidden" ShowGroupedColumns="False"
                    ShowGroupPanel="False" ShowPreview="True" ShowTitlePanel="False" ShowVerticalScrollBar="False" />
                <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                    </Header>
                    <AlternatingRow Enabled="True" />
                </Styles>
                <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                <SettingsPager PageSize="15" ShowSeparators="True">
                </SettingsPager>
                <SettingsCustomizationWindow Enabled="True" />
                <SettingsText Title="" GroupPanel="Gruplamak istediğiniz kolon başlığını buraya sürükleyiniz."
                    ConfirmDelete="Kayıt silinsin mi?" EmptyDataRow="Kayıt yok" />
            </dxwgv:ASPxGridView>
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
