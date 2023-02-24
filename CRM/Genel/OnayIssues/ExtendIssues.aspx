<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="ExtendIssues.aspx.cs"
    Inherits="CRM_Genel_OnayIssues_ExtendIssues" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    tagprefix="dxw" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function SetDivVisibility(DivId, ImgId) {
            //if((DivId=="div1") || (DivId=="div2"))
            //{
            if (document.getElementById(DivId).style.visibility == "visible") {
                document.getElementById(DivId).style.visibility = "hidden";
                document.getElementById(DivId).style.position = "absolute";
                document.getElementById(ImgId).src = "../../../App_Themes/Glass/PivotGrid/pgCollapsedButton.gif";
            }
            else {
                document.getElementById(DivId).style.visibility = "visible";
                document.getElementById(DivId).style.position = "relative";
                document.getElementById(ImgId).src = "../../../App_Themes/Glass/PivotGrid/pgExpandedButton.gif";
            }
            //}
        }
    </script>
    <asp:SqlDataSource ID="DSUser" runat="server" SelectCommand="SELECT IndexId,(ISNULL(UserName,'')+' ['+ISNULL(FirstName,'')+' '+ISNULL(LastName,'')+']') AS UserName, UserName AS UsString FROM SecurityUsers Where Active=1 ORDER BY UserName"
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
    <div>
        <model:DataTable ID="DataTableList" runat="server" />
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
                <dxm:MenuItem Name="AddAttachment">
                    <Template>
                        <table width="50" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                    padding-right: 4px; border-right-width: 0px; width: 150px;" onclick="Grid.PerformCallback('List|0');">
                                    <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/List.gif" />Listele
                                </td>
                            </tr>
                        </table>
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Name="Extend" Text="Seçilenleri Uzat">
                    <Image Url="~/images/Extend.png" />
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="" Width="750px"
            BackColor="#EBF2F4" CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass"
            ShowHeader="False">
            <PanelCollection>
                <dxrp:PanelContent ID="PanelContent1" runat="server">
                    <table border="0" cellpadding="0" cellspacing="3" style="width: 750px">
                        <tr>
                            <td align="left" style="width: 150px" valign="top">
                                Başlangıç Tarihi
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                                <dxe:ASPxDateEdit ID="StartDate" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                    CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                    <ButtonStyle Cursor="pointer" Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td align="left" valign="top">
                                Bitiş Tarihi
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                                <dxe:ASPxDateEdit ID="EndDate" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                    CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                    <ButtonStyle Cursor="pointer" Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 150px" valign="top">
                                Gündem PNR No
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                                <dxe:ASPxTextBox ID="IssueID" runat="server" Width="170px">
                                    <ClientSideEvents KeyPress="function(s,e) { if (window.event.keyCode == 13) {  event.returnValue=false; 
                                                    event.cancel = true; Grid.PerformCallback('List|0'); }}" />
                                </dxe:ASPxTextBox>
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="IssueID"
                                    ErrorMessage="Sadece Sayı Girilmelidir" MaximumValue="100000000" MinimumValue="0"
                                    Type="Integer"></asp:RangeValidator>
                            </td>
                            <td align="left" valign="top">
                                Anahtar Kelime
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                                <dxe:ASPxTextBox ID="KeyWords" runat="server" Width="170px">
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 150px" valign="top">
                                Gündem Tanısı
                            </td>
                            <td align="left" colspan="3" valign="top">
                                <dxe:ASPxTextBox ID="TxtBaslik" runat="server" Width="100%">
                                    <ClientSideEvents KeyPress="function(s,e) { if (window.event.keyCode == 13) {    event.returnValue=false; 
                                                    event.cancel = true; Grid.PerformCallback('List|0'); }}" />
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 150px" valign="top">
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                                <asp:CheckBox ID="Atanan" Text="Üzerimdeki Gündemleri Listele" runat="server" Checked="false" />
                            </td>
                            <td align="left" valign="top">
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                                <asp:CheckBox ID="Atayan" Text="Benim Tespit Ettiğim Gündemleri Listele" runat="server"
                                    Checked="false" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 150px" valign="top">
                                Eklenecek Gün
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                                <dxe:ASPxTextBox ID="txtAddDay" runat="server" Width="170px" Text="1">                                  
                                </dxe:ASPxTextBox>
                                <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtAddDay"
                                    ErrorMessage="Sadece Sayı Girilmelidir" MaximumValue="100000000" MinimumValue="0"
                                    Type="Integer"></asp:RangeValidator>
                            </td>
                            <td align="left" valign="top">
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                            </td>
                        </tr>
                    </table>
                </dxrp:PanelContent>
            </PanelCollection>
        </dxrp:ASPxRoundPanel>
        <table border="0" width="100%" style="background-image: url('../../../App_Themes/Blue (Horizontal orientation)/Web/mItemBack.gif');">
            <tr>
                <td align="center" style="width: 20px;">
                    <img id="img1" src="../../../App_Themes/Glass/PivotGrid/pgCollapsedButton.gif" alt="Daralt/Genişlet"
                        onclick="SetDivVisibility('div1','Img1');" onmouseover="document.getElementById('img1').style.cursor = 'pointer';"
                        onmouseout="document.getElementById('img1').style.cursor = 'default';" />
                </td>
                <td align="left">
                    <dxe:ASPxLabel ID="lbl1" runat="server" Text="Gündem Kriterleri" />
                </td>
            </tr>
        </table>
        <div id="div1" style="position: absolute; visibility: hidden; z-index=0">
            <table border="0" cellpadding="0" cellspacing="3" style="width: 750px">
                <tr>
                    <td align="left" style="width: 150px" valign="top">
                        İlgili Birim Seç
                    </td>
                    <td align="left" style="width: 220px; width: 250px" valign="top">
                        <dxe:ASPxComboBox ID="FirmaID" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                            CssPostfix="Glass" EnableIncrementalFiltering="True" ImageFolder="~/App_Themes/Glass/{0}/"
                            Width="200px" ValueType="System.Int32" ClientInstanceName="cmb_Firma" EnableCallbackMode="true"
                            CallbackPageSize="15">
                            <ButtonStyle Cursor="pointer" Width="13px">
                            </ButtonStyle>
                            <ClientSideEvents SelectedIndexChanged="function(s, e) {cmbProjeID.PerformCallback(s.GetValue());}" />
                        </dxe:ASPxComboBox>
                    </td>
                    <td align="left" valign="top">
                        Departman Seç
                    </td>
                    <td align="left" style="width: 250px" valign="top">
                        <dxe:ASPxComboBox ID="ProjeID" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                            CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" ValueType="System.Int32"
                            EnableIncrementalFiltering="True" EnableCallbackMode="true" ClientInstanceName="cmbProjeID"
                            OnCallback="ProjeID_Callback">
                            <ButtonStyle Cursor="pointer" Width="13px">
                            </ButtonStyle>
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 150px" valign="top">
                        <dxe:ASPxLabel ID="lblKisiFiltrele" runat="server" Text="Üzerine Atanan" />
                    </td>
                    <td align="left" style="width: 250px" valign="top">
                        <dxe:ASPxComboBox ID="UzerineAtanan" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                            Width="200px" CssPostfix="Glass" DataSourceID="DSUser" EnableIncrementalFiltering="True"
                            ImageFolder="~/App_Themes/Glass/{0}/" ValueType="System.Int32" ValueField="IndexId"
                            TextField="UserName" EnableCallbackMode="true" CallbackPageSize="15">
                            <ButtonStyle Cursor="pointer" Width="13px">
                            </ButtonStyle>
                        </dxe:ASPxComboBox>
                    </td>
                    <td align="left" style="width: 150px" valign="top">
                        <dxe:ASPxLabel ID="lblKisiFiltrele2" runat="server" Text="Atayan" />
                    </td>
                    <td align="left" style="width: 150px" valign="top">
                        <dxe:ASPxComboBox ID="BildirimiGiren" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                            Width="200px" CssPostfix="Glass" DataSourceID="DSUser" EnableIncrementalFiltering="True"
                            ImageFolder="~/App_Themes/Glass/{0}/" ValueType="System.String" ValueField="UsString"
                            TextField="UserName" EnableCallbackMode="true" CallbackPageSize="15">
                            <ButtonStyle Cursor="pointer" Width="13px">
                            </ButtonStyle>
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 150px" valign="top">
                        Gündem Durum Seç
                    </td>
                    <td align="left" valign="top" colspan="3">
                        <asp:CheckBoxList ID="DurumList1" runat="server" CellPadding="0" CellSpacing="2"
                            Font-Bold="False" Font-Names="Arial" Font-Size="11px" RepeatColumns="3" Width="100%">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 150px" valign="top">
                        Müdehale Yöntemi Seç
                    </td>
                    <td align="left" valign="top" colspan="3">
                        <asp:CheckBoxList ID="chcMudehaleYontemi" runat="server" CellPadding="0" CellSpacing="2"
                            Font-Bold="False" Font-Names="Arial" Font-Size="11px" RepeatColumns="3" Width="100%">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 150px" valign="top">
                        Gündem Sınıf Seç
                    </td>
                    <td align="left" valign="top" colspan="3">
                        <asp:CheckBoxList ID="chcVirusSiniflari" runat="server" CellPadding="0" CellSpacing="2"
                            Font-Bold="False" Font-Names="Arial" Font-Size="11px" RepeatColumns="3" Width="100%">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 150px" valign="top">
                        <dxe:ASPxLabel ID="lblProjeSinif" runat="server" Text="Departman Grubu Seç" />
                    </td>
                    <td align="left" valign="top" colspan="3">
                        <asp:CheckBoxList ID="ProjeSinifID" runat="server" CellPadding="0" CellSpacing="2"
                            Font-Bold="False" Font-Names="Arial" Font-Size="11px" RepeatColumns="3" Width="100%">
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
        </div>
        <hr />
        <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DataTableList" KeyFieldName="ID" ClientInstanceName="Grid"
            OnCustomCallback="grid_CustomCallback" Width="1000px" OnAfterPerformCallback="grid_AfterPerformCallback">
            <SettingsText Title="Tarih Arttırımı Onay Listesi" GroupPanel="Gruplamak istediğiniz kolon başlığını buraya s&#252;r&#252;kleyiniz."
                ConfirmDelete="Kayıt silinsin mi?" EmptyDataRow="Kayıt Yok" />
            <SettingsPager PageSize="15" ShowSeparators="True">
            </SettingsPager>
            <Images ImageFolder="~/App_Themes/Glass/{0}/">
            </Images>
            <SettingsCustomizationWindow Enabled="True" />
            <SettingsLoadingPanel Text="Yükleniyor..." />
            <Settings ShowFilterRow="True" ShowStatusBar="Visible" ShowGroupedColumns="True"
                ShowGroupPanel="True" ShowPreview="True" ShowTitlePanel="True" />
            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <AlternatingRow Enabled="True">
                </AlternatingRow>
                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                </Header>
            </Styles>
            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" ShowSelectCheckbox="true" VisibleIndex="0"
                    Width="60px">
                    <HeaderTemplate>
                        <input id="Button1" type="button" onclick="Grid.PerformCallback('Select|true');"
                            value="+" title="Tümünü Seç" />
                        <input id="Button2" type="button" onclick="Grid.PerformCallback('Select|false');"
                            value="-" title="Tümünü Seçme" />
                    </HeaderTemplate>
                    <ClearFilterButton Visible="true" Text="Süzmeyi Temizle">
                        <Image Url="~/images/reload2.jpg" AlternateText="Süzmeyi Temizle" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="Id" Visible="False">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="PNR" Caption="PNR" Width="50px">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="VirusSinif" Caption="Gündem Sınıf" Width="150px">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Gündem Tanımı" FieldName="Baslik">
                    <DataItemTemplate>
                        <dxe:ASPxHyperLink ID="lbl_IssueID" CssClass="dxeBase" Font-Size="8pt" Font-Names="Arial"
                            runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('../Issue/edit.aspx?id="+Eval("PNR")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                            Text='<%#Eval("Baslik")%>'>
                        </dxe:ASPxHyperLink>
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Tespit Eden" FieldName="CreatedBy" Settings-AutoFilterCondition="Contains" />
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Operasyon Tarihi"
                    FieldName="TeslimTarihi" Width="175px">
                    <HeaderCaptionTemplate>
                        Planlanan Op.<br />
                        Tarihi
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataDateColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </div>
</asp:Content>
