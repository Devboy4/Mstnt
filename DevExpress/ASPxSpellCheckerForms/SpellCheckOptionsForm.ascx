<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
        
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SpellCheckOptionsForm.ascx.cs" Inherits="SpellCheckOptionsForm" %>
<table id="mainSpellCheckOptionsFormTable" cellpadding="0" cellspacing="0" class="mainSpellCheckOptionsFormTable">
    <tr>
        <td class="contentSCOptionsFormContainer">
            <table id="optionsForm" cellpadding="0px" cellspacing="0px" style="width:100%">
                <tr>
                    <td>
                        <dxrp:ASPxRoundPanel ID="pnlOptions" runat="server" HeaderText="General options" Width="100%">
                            <PanelCollection>
                                <dxp:PanelContent runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <dxe:ASPxCheckBox id="chkbUpperCase" ClientInstanceName="chkbUpperCase" runat="server" Text="Ignore words in UPPERCASE">
                                                </dxe:ASPxCheckBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxCheckBox id="chkbMixedCase" ClientInstanceName="chkbMixedCase" runat="server" Text="Ignore words in MiXeDcAsE"></dxe:ASPxCheckBox>                    
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxCheckBox id="chkbNumbers" ClientInstanceName="chkbNumbers" runat="server" Text="Ignore words with numbers"></dxe:ASPxCheckBox>                    
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxCheckBox id="chkbEmails" ClientInstanceName="chkbEmails" runat="server" Text="Ignore e-mails"></dxe:ASPxCheckBox>                    
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxCheckBox id="chkbUrls" ClientInstanceName="chkbUrls" runat="server" Text="Ignore URLs"></dxe:ASPxCheckBox>                    
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                               <dxe:ASPxCheckBox id="chkbTags" ClientInstanceName="chkbTags" runat="server" Text="Ignore markup tags"></dxe:ASPxCheckBox>                    
                                            </td>
                                        </tr>
                                    </table>

                                </dxp:PanelContent>
                            </PanelCollection>
                        </dxrp:ASPxRoundPanel>
                    </td>
                </tr>
                <tr>
                    <td class="languagePanel">
                        <dxrp:ASPxRoundPanel ID="pnlLanguageSelection" runat="server" HeaderText="International dictionaries" Width="100%">
                            <PanelCollection>
                                <dxp:PanelContent runat="server">
                                    <table style="width:100%;">
                                        <tr>
                                            <td colspan="2">
                                                Choose which dictionary to use when checking your spelling.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Language:
                                            </td>
                                            <td align="left" style="width:70%;">
                                                <dxe:ASPxComboBox ID="comboLanguage" ClientInstanceName="comboLanguage" runat="server" Width="100%"></dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </dxp:PanelContent>
                            </PanelCollection>
                        </dxrp:ASPxRoundPanel>
                    </td>
                </tr>  
            </table>        
        </td>
    </tr>
    <tr class="footerBackground">
        <td>
            <table width="100%" cellpadding="0px" cellspacing="0px">
                <tr>
                    <td class="leftBottomButton" align="right">
                        <dxe:ASPxButton id="btnOK" runat="server" Text="OK" AutoPostBack="false" Width="100px" UseSubmitBehavior="false">
                        <ClientSideEvents Click="function(s, e) {aspxSCDialogComplete(true)}"/>
                        </dxe:ASPxButton>
                    </td>
                    <td class="rightBottomButton">
                        <dxe:ASPxButton id="btnCancel" runat="server" Text="Cancel" AutoPostBack="false" Width="100px" UseSubmitBehavior="false">
                        <ClientSideEvents Click="function(s, e) {aspxSCDialogComplete(false)}"/>                        
                        </dxe:ASPxButton>
                    </td>
                </tr>
            </table>                
        </td>
    </tr>
</table>

