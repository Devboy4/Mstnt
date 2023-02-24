<%--
{************************************************************************************}
{                                                                                    }
{   DO NOT MODIFY THIS FILE!                                                         }
{                                                                                    }
{   It will be overwritten without prompting when a new version becomes              }
{   available. All your changes will be lost.                                        }
{                                                                                    }
{   This file contains the default template and is required for the form             }
{   rendering. Improper modifications may result in incorrect behavior of            }
{   dialog forms.                                                                    }
{                                                                                    }
{************************************************************************************}
--%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ImagePropertiesForm.ascx.cs" Inherits="ImagePropertiesForm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
  
<table cellpadding="0" cellspacing="0" id="imagePropertiesForm">
    <tr>
        <td>
            <dxe:ASPxLabel ID="lblSize" runat="server" Text="Size:" AssociatedControlID="cmbSize"></dxe:ASPxLabel>
            <div class="captionIndent"></div>            
            <dxe:ASPxComboBox ID="cmbSize" runat="server" SelectedIndex="0" ValueType="System.String" ClientInstanceName="_dxeCmbSize">
                <Items>
                    <dxe:ListEditItem Text="Original image size" Value="original" />
                    <dxe:ListEditItem Text="Custom size" Value="custom" />
                </Items>
                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCmbSizeSelectedIndexChanged(s); }" />                
            </dxe:ASPxComboBox>
        </td>
    </tr>
    <tr id="_dxeSizePropertiesRow" style="display:none;">
        <td class="imageSizeEditorsCell">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="captionCell">                                
                                    <dxe:ASPxLabel ID="lblWidth" runat="server" Text="Width:" AssociatedControlID="spnWidth"></dxe:ASPxLabel></td>
                                <td>
                                    <dxe:ASPxSpinEdit ID="spnWidth" ClientInstanceName="_dxeSpnWidth" runat="server" Height="21px" Number="1" Width="52px" AllowNull="False" NumberType="Integer" MaxValue="10000" MinValue="1">
                                        <SpinButtons ShowIncrementButtons="False">
                                        </SpinButtons>
                                        <ClientSideEvents NumberChanged="function(s, e) { aspxSizeSpinNumberChanged(&quot;width&quot;); }" KeyUp="function(s, e) { aspxSizeSpinKeyUp(&quot;width&quot;, e.htmlEvent); }"/>
                                    </dxe:ASPxSpinEdit>                        
                                </td>
                                <td class="pixelSizeCell"><dxe:ASPxLabel ID="lblPixelWidth" runat="server" Text="pixels"></dxe:ASPxLabel></td>
                            </tr>
                            <tr>
                                <td colspan="3" class="separatorCell"></td>
                            </tr>
                            <tr>
                                <td class="captionCell"><dxe:ASPxLabel ID="lblHeight" runat="server" Text="Height:" AssociatedControlID="spnHeight"></dxe:ASPxLabel></td>
                                <td>
                                    <dxe:ASPxSpinEdit ID="spnHeight" ClientInstanceName="_dxeSpnHeight" runat="server" Height="21px" Number="1" Width="52px" AllowNull="False" MinValue="1" NumberType="Integer" MaxValue="10000">
                                        <SpinButtons ShowIncrementButtons="False">
                                        </SpinButtons>
                                        <ClientSideEvents NumberChanged="function(s, e) { aspxSizeSpinNumberChanged(&quot;height&quot;); }" KeyUp="function(s, e) { aspxSizeSpinKeyUp(&quot;height&quot;, e.htmlEvent);  }"/>
                                    </dxe:ASPxSpinEdit>
                                </td>
                                <td class="pixelSizeCell"><dxe:ASPxLabel ID="lblPixelHeight" runat="server" Text="pixels"></dxe:ASPxLabel></td> 
                            </tr>
                        </table>                    
                    </td>
                    <td class="constrainProportionsCell">
                        <table cellpadding="0" cellspacing="0">
                            <tr><td id="cellContarainTop" runat="server"></td></tr>
                            <tr><td id="cellContarainSwitcher" runat="server" style="cursor: pointer;"></td></tr>
                            <tr><td id="cellContarainBottom" runat="server"></td></tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div class="fieldSeparator"></div>
            <dxe:ASPxCheckBox ID="ckbCreateThumbnail" runat="server" Text="Create thumbnail" ClientInstanceName="_dxeCkbCreateThumbnail">
                <ClientSideEvents CheckedChanged="function(s, e) { OnCreateThumbnailCheckedChanged(s.GetChecked()) }" />
            </dxe:ASPxCheckBox>
            <div id="_dxeThumbnailFileNameArea" class="thumbnailFileNameArea" style="display:none;">
                <dxe:ASPxLabel ID="lblThumbnailFileName" runat="server" Text="New image name:" AssociatedControlID="txbThumbnailFileName"></dxe:ASPxLabel>            
                <div class="ñaptionIndent"></div>            
                <dxe:ASPxTextBox ID="txbThumbnailFileName" ClientInstanceName="_dxeThumbnailFileName" runat="server" Width="170px" AutoCompleteType="Disabled">
                    <ValidationSettings ErrorDisplayMode="Text" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidateOnLeave="False" ValidationGroup="_dxeThumbnailFileNameGroup">
                        <ErrorFrameStyle Font-Size="10px">
                            <Paddings PaddingRight="0px" />                        
                        </ErrorFrameStyle>
                        <RegularExpression ValidationExpression="^([^\.\&quot;\|*?><:\\\/])*(([^\.\&quot;\|*?><:\\\/])[\.][j|J][p|P][g|G])$" ErrorText="Invalid file name (example: thumbnail.jpg)"/>
                        <RequiredField IsRequired="True" ErrorText="This field is required" />
                    </ValidationSettings>
                </dxe:ASPxTextBox>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <div class="fieldSeparator"></div>        
        </td>
    </tr>
    <tr>
        <td>
            <dxe:ASPxLabel ID="lblImagePosition" runat="server" Text="Position:" AssociatedControlID="cmbImagePosition"></dxe:ASPxLabel>        
            <div class="captionIndent"></div>                        
            <dxe:ASPxComboBox ID="cmbImagePosition" ClientInstanceName="_dxeCmbImagePosition" runat="server" SelectedIndex="0" ValueType="System.String">
                <Items>
                    <dxe:ListEditItem Text="Left-aligned" Value="left" />
                    <dxe:ListEditItem Text="Center" Value="center" />
                    <dxe:ListEditItem Text="Right-aligned" Value="right" />
                </Items>
            </dxe:ASPxComboBox>            
        </td>
    </tr>
    <tr>
        <td>
            <div class="fieldSeparator"></div>        
        </td>
    </tr>    
    <tr>
        <td>
            <dxe:ASPxLabel ID="lblImageDescription" runat="server" Text="Description:" AssociatedControlID="txbDescription"></dxe:ASPxLabel>
            <div class="captionIndent"></div>
            <dxe:ASPxTextBox ID="txbDescription" ClientInstanceName="_dxeTxbDescription" runat="server" Width="170px" AutoCompleteType="Disabled">
            </dxe:ASPxTextBox>
        </td>
    </tr>    
</table>

<script type="text/javascript" id="dxss_ImagePropertiesForm">
    function OnCmbSizeSelectedIndexChanged(cmb) {
        var isShow = cmb.GetValue() != "original";
        _aspxSetElementDisplay(_aspxGetElementById("_dxeSizePropertiesRow"), isShow);
        if (isShow)
            aspxAdjustControlsSizeInDialogWindow();
    }
    function OnCreateThumbnailCheckedChanged(isChecked) {
        _aspxSetElementDisplay(_aspxGetElementById("_dxeThumbnailFileNameArea"), isChecked);
        if (isChecked)
            aspxAdjustControlsSizeInDialogWindow();
    }
</script>