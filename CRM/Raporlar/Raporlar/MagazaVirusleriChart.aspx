<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="MagazaVirusleriChart.aspx.cs"
    Inherits="CRM_Raporlar_MagazaVirusleriChart" %>

<%@ Register Assembly="DevExpress.XtraReports.v8.3.Web" Namespace="DevExpress.XtraReports.Web"
    TagPrefix="dxxr" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.XtraCharts.v8.3" Namespace="DevExpress.XtraCharts"
    TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v8.3.Web" Namespace="DevExpress.XtraCharts.Web"
    TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div>
            <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
                CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
                ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
                ShowSubMenuShadow="False" AutoPostBack="True">
                <SubMenuStyle GutterWidth="0px" />
                <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
                <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
                </SubMenuItemStyle>
                <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                    X="2" Y="-12" />
                <Items>
                    <dxm:MenuItem>
                        <Template>
                            <table width="50" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="dxmMenuItem_Blue" style="cursor: pointer;" onclick="javascript:chart.PerformCallback('LoadData');">
                                        <b>SORGULA</b>
                                    </td>
                                </tr>
                            </table>
                        </Template>
                    </dxm:MenuItem>
                </Items>
            </dxm:ASPxMenu>
            <hr />
            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Gündem Hareketleri Grafik"
                Width="1050px">
                <PanelCollection>
                    <dxrp:PanelContent ID="PanelContent2" runat="server">
                        <table cellpadding="1" cellspacing="1" border="0">
                            <tr>
                                <td colspan="1">
                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" ForeColor="Black" Text="Merkez" />
                                </td>
                                <td colspan="1" style="width: 400px">
                                    <dxe:ASPxComboBox ID="FirmaID" runat="server" Width="390px" ValueType="System.Guid"
                                        EnableIncrementalFiltering="True" ClientInstanceName="cmbFirmaID" CallbackPageSize="50"
                                        EnableCallbackMode="true">
                                    </dxe:ASPxComboBox>
                                </td>
                                <td colspan="1">
                                    <dxe:ASPxLabel ID="lblTarih1" runat="server" ForeColor="Black" Text="Ýlk Tarih" />
                                </td>
                                <td colspan="1" style="width: 150px">
                                    <dxe:ASPxDateEdit ID="IlkTarih" runat="server" Width="130px" />
                                </td>
                                <td colspan="1">
                                    <dxe:ASPxLabel ID="lblTarih2" runat="server" ForeColor="Black" Text="Son Tarih" />
                                </td>
                                <td colspan="1" style="width: 150px">
                                    <dxe:ASPxDateEdit ID="SonTarih" runat="server" Width="130px" />
                                </td>
                                <td colspan="1">
                                    <dxe:ASPxCheckBox ID="cbShowLegends" runat="server" Text="Grafik Yazýlarýný Göster"
                                        ClientInstanceName="cmbShowLegends" Checked="true">
                                        <ClientSideEvents CheckedChanged="function(s, e) { chart.PerformCallback('ShowLegends'); }" />
                                    </dxe:ASPxCheckBox>
                                </td>
                            </tr>
                        </table>
                    </dxrp:PanelContent>
                </PanelCollection>
            </dxrp:ASPxRoundPanel>
            <hr />
            <dxchartsui:WebChartControl ID="WebChartControl1" runat="server" Height="440px" Width="1050px"
                OnCustomCallback="WebChartControl1_CustomCallback" ClientInstanceName="chart"
                LoadingPanelText="Yükleniyor..." LoadingPanelStyle-BackColor="#d7e2e8" LoadingPanelImage-Url="../../images/bigflower_000000.gif">
                <SeriesTemplate LabelTypeName="SideBySideBarSeriesLabel" PointOptionsTypeName="PointOptions"
                    SeriesViewTypeName="SideBySideBarSeriesView">
                    <PointOptions HiddenSerializableString="to be serialized">
                    </PointOptions>
                    <Label HiddenSerializableString="to be serialized">
                        <FillStyle FillOptionsTypeName="SolidFillOptions">
                            <Options HiddenSerializableString="to be serialized" />
                        </FillStyle>
                    </Label>
                    <LegendPointOptions HiddenSerializableString="to be serialized">
                    </LegendPointOptions>
                    <View HiddenSerializableString="to be serialized">
                    </View>
                </SeriesTemplate>
                <FillStyle FillOptionsTypeName="SolidFillOptions">
                    <Options HiddenSerializableString="to be serialized" />
                </FillStyle>
                <Titles>
                    <cc1:ChartTitle Text="GÜNDEM DAÐILIMI (BÝRÝM)">
                    </cc1:ChartTitle>
                </Titles>
            </dxchartsui:WebChartControl>
        </div>
  </asp:Content>
