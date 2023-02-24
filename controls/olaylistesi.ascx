<%@ Control Language="C#" AutoEventWireup="true" CodeFile="olaylistesi.ascx.cs" Inherits="controls_olaylistesi" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTimer"
    TagPrefix="dxt" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxCallbackPanel"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<dxt:ASPxTimer ID="ASPxTimer1" Enabled="true" runat="server" Interval="30000" ClientInstanceName="Timer1">
    <ClientSideEvents Tick="function(s, e) { MesajCallback.PerformCallback('x'); }" />
</dxt:ASPxTimer>
<dxcp:ASPxCallbackPanel ID="MesajCallback" ClientInstanceName="MesajCallback" ShowLoadingPanel="false"
    HideContentOnCallback="false" runat="server" OnCallback="MesajCallback_Callback"
    LoadingPanelText="" Width="100%">
    <PanelCollection>
        <dxrp:PanelContent runat="server">


            <asp:HiddenField runat="server" ID="HiddenID" />
            <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%">
                <tr>
                    <td align="left" style="width: 80%; font-size: 12px; font-family: Arial; text-align: center">

                        <marquee id="mManset" behavior="scroll" direction="up" scrollamount="1" onmouseover="scrollAmount=0;"
                            onmouseout="scrollAmount=1;" height="50px" width="100%"><p><asp:Literal Text="" ID="ltrManset" runat="server"></asp:Literal></p></marquee>
                      
                    </td>
                    <td align="right" style="width: 15%">
                        <dxe:ASPxImage runat="server" ID="imguyari" ImageUrl="~/images/uyarivar.gif" Cursor="pointer">
                            <ClientSideEvents Click="function(s,e){ControlCalBack();}" />
                        </dxe:ASPxImage>
                    </td>
                </tr>
            </table>
        </dxrp:PanelContent>
    </PanelCollection>
    <Paddings PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px" PaddingTop="0px" />
</dxcp:ASPxCallbackPanel>
