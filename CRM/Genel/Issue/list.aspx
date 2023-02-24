<%@ page language="C#" masterpagefile="~/M1.master" autoeventwireup="true" codefile="list.aspx.cs"
    inherits="CRM_Genel_Issue_list" %>

<%@ register assembly="DevExpress.Web.ASPxGridView.v8.3.Export" namespace="DevExpress.Web.ASPxGridView.Export"
    tagprefix="dxwgv" %>
<%@ register assembly="DevExpress.Web.v8.3" namespace="DevExpress.Web.ASPxPanel"
    tagprefix="dxp" %>
<%@ register assembly="DevExpress.Web.v8.3" namespace="DevExpress.Web.ASPxCallback"
    tagprefix="dxcb" %>
<%@ register assembly="DevExpress.Web.v8.3" namespace="DevExpress.Web.ASPxClasses"
    tagprefix="dxw" %>
<%@ register assembly="DevExpress.Web.ASPxEditors.v8.3" namespace="DevExpress.Web.ASPxEditors"
    tagprefix="dxe" %>
<%@ register assembly="DevExpress.Web.ASPxGridView.v8.3" namespace="DevExpress.Web.ASPxGridView"
    tagprefix="dxwgv" %>
<%@ register assembly="DevExpress.Web.v8.3" namespace="DevExpress.Web.ASPxPopupControl"
    tagprefix="dxpc" %>
<%@ register assembly="DevExpress.Web.v8.3" namespace="DevExpress.Web.ASPxMenu" tagprefix="dxm" %>
<%@ register assembly="DevExpress.Web.v8.3" namespace="DevExpress.Web.ASPxRoundPanel"
    tagprefix="dxrp" %>
<%@ register assembly="DevExpress.Web.v8.3" namespace="DevExpress.Web.ASPxTabControl"
    tagprefix="dxtc" %>
<%@ register src="~/controls/DataTableControl.ascx" tagname="DataTable" tagprefix="model" %>
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
    <div>
        <script type="text/javascript" src="crm_20141215.js"></script>
        <model:datatable id="DataTableList" runat="server" />
        <asp:HiddenField ID="HiddenID" runat="server" />
        <asp:HiddenField ID="UserName" runat="server" />
        <asp:SqlDataSource ID="DSFirma" runat="server" SelectCommand="Select IndexId,FirmaName From Firma Order By FirmaName"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
        <asp:SqlDataSource ID="DSProje" runat="server" SelectCommand="Select IndexId,Adi From Proje Order By Adi"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
        <asp:SqlDataSource ID="DSOnemDerece" runat="server" SelectCommand="SELECT IndexId,Adi AS OnemDereceName FROM OnemDereceleri ORDER BY Adi"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
        <asp:SqlDataSource ID="DSUser" runat="server" SelectCommand="SELECT IndexId,(ISNULL(UserName,'')+' ['+ISNULL(FirstName,'')+' '+ISNULL(LastName,'')+']') AS UserName, UserName AS UsString FROM SecurityUsers Where Active=1 ORDER BY UserName"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
        <asp:SqlDataSource ID="DSHataTip" runat="server" SelectCommand="SELECT IndexId,Adi FROM HataTipleri ORDER BY Adi"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
        <asp:SqlDataSource ID="DSVirusSinif" runat="server" SelectCommand="SELECT IndexId,Adi FROM VirusSinif ORDER BY Adi"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
        <dxrp:aspxroundpanel id="ASPxRoundPanel2" runat="server" headertext="" width="100%"
            height="100%">
            <panelcollection>
                <dxp:panelcontent runat="server">
                    <dxm:aspxmenu id="menu" runat="server" autoseparators="RootOnly" cssfilepath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
                        csspostfix="Blue" imagefolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
                        itemspacing="0px" separatorheight="100%" separatorwidth="2px" showpopoutimages="True"
                        showsubmenushadow="False">
                        <submenustyle gutterwidth="0px" />
                        <rootitemsubmenuoffset firstitemx="-2" lastitemx="-2" x="-1" />
                        <submenuitemstyle imagespacing="19px" popoutimagespacing="30px">
                        </submenuitemstyle>
                        <itemsubmenuoffset firstitemx="2" firstitemy="-12" lastitemx="2" lastitemy="-12"
                            x="2" y="-12" />
                        <items>
                            <dxm:menuitem name="AddAttachment">
                                <template>
                                    <table width="50" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer; padding-right: 4px; border-right-width: 0px; width: 150px;"
                                                onclick="Grid.PerformCallback('x');">
                                                <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/List.gif" />
                                                Listele
                                            </td>
                                        </tr>
                                    </table>
                                </template>
                            </dxm:menuitem>
                            <dxm:menuitem name="AddExtrachtColumns">
                                <image url="~/images/new.gif" />
                                <template>
                                    <table width="150" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer; padding-right: 4px; border-right-width: 0px; width: 150px;"
                                                onclick="ShowHideCustomizationWindow();">
                                                <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/undo_16.gif" />
                                                Kolon
                                                Ekle / Çýkart
                                            </td>
                                        </tr>
                                    </table>
                                </template>
                            </dxm:menuitem>
                            <dxm:menuitem name="meeting" text="Toplantý Oluþtur" tooltip="Seçilen Gündemler hakkýnda toplantý oluþtur">
                                <image url="~/images/meeting.png" />
                            </dxm:menuitem>
                            <dxm:menuitem name="SetAntivirus" text="Seçilenleri Kapat" tooltip="Seçilenleri Kapat">
                                <image url="~/images/saveopen.png" />
                            </dxm:menuitem>
                            <dxm:menuitem name="excel" text="EXCEL" tooltip="EXCEL olarak kaydet">
                                <image url="~/images/xls_ico.gif" />
                            </dxm:menuitem>
                            <dxm:menuitem name="pdf" text="PDF" tooltip="PDF olarak kaydet">
                                <image url="~/images/pdf_icon.gif" />
                            </dxm:menuitem>
                        </items>
                    </dxm:aspxmenu>
                    <dxrp:aspxroundpanel id="ASPxRoundPanel1" runat="server" headertext="" width="750px"
                        backcolor="#EBF2F4" cssfilepath="~/App_Themes/Glass/{0}/styles.css" csspostfix="Glass"
                        showheader="False">
                        <panelcollection>
                            <dxrp:panelcontent id="PanelContent1" runat="server">
                                <table border="0" cellpadding="0" cellspacing="3" style="width: 750px">
                                    <tr>
                                        <td align="left" style="width: 150px" valign="top">Baþlangýç Tarihi
                                        </td>
                                        <td align="left" style="width: 250px" valign="top">
                                            <dxe:aspxdateedit id="StartDate" runat="server" cssfilepath="~/App_Themes/Glass/{0}/styles.css"
                                                csspostfix="Glass" imagefolder="~/App_Themes/Glass/{0}/">
                                                <buttonstyle cursor="pointer" width="13px">
                                                </buttonstyle>
                                            </dxe:aspxdateedit>
                                        </td>
                                        <td align="left" valign="top">Bitiþ Tarihi
                                        </td>
                                        <td align="left" style="width: 250px" valign="top">
                                            <dxe:aspxdateedit id="EndDate" runat="server" cssfilepath="~/App_Themes/Glass/{0}/styles.css"
                                                csspostfix="Glass" imagefolder="~/App_Themes/Glass/{0}/">
                                                <buttonstyle cursor="pointer" width="13px">
                                                </buttonstyle>
                                            </dxe:aspxdateedit>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 150px" valign="top">Gündem PNR No
                                        </td>
                                        <td align="left" style="width: 250px" valign="top">
                                            <dxe:aspxtextbox id="IssueID" runat="server" width="170px">
                                                <clientsideevents keypress="function(s,e) { if (window.event.keyCode == 13) {  event.returnValue=false; 
                                                    event.cancel = true; Grid.PerformCallback('x'); }}" />
                                            </dxe:aspxtextbox>
                                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="IssueID"
                                                ErrorMessage="Sadece Sayý Girilmelidir" MaximumValue="100000000" MinimumValue="0"
                                                Type="Integer">
                                            </asp:RangeValidator>
                                        </td>
                                        <td align="left" valign="top">Anahtar Kelime
                                        </td>
                                        <td align="left" style="width: 250px" valign="top">
                                            <dxe:aspxtextbox id="KeyWords" runat="server" width="170px">
                                            </dxe:aspxtextbox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 150px" valign="top">Gündem Tanýsý
                                        </td>
                                        <td align="left" colspan="3" valign="top">
                                            <dxe:aspxtextbox id="TxtBaslik" runat="server" width="100%">
                                                <clientsideevents keypress="function(s,e) { if (window.event.keyCode == 13) {    event.returnValue=false; 
                                                    event.cancel = true; Grid.PerformCallback('x'); }}" />
                                            </dxe:aspxtextbox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 150px" valign="top"></td>
                                        <td align="left" style="width: 250px" valign="top">
                                            <asp:CheckBox ID="Atanan" Text="Üzerimdeki Gündemleri Listele" runat="server" Checked="false" />
                                        </td>
                                        <td align="left" valign="top"></td>
                                        <td align="left" style="width: 250px" valign="top">
                                            <asp:CheckBox ID="Atayan" Text="Benim Tespit Ettiðim Gündemleri Listele" runat="server"
                                                Checked="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 150px" valign="top"></td>
                                        <td colspan="3" align="left" valign="top">
                                            <asp:CheckBox ID="ChkNotListSubVirus" Text="Alt Gündemleri Listeleme" runat="server"
                                                Checked="false" />
                                        </td>
                                    </tr>
                                </table>
                            </dxrp:panelcontent>
                        </panelcollection>
                    </dxrp:aspxroundpanel>
                    <table border="0" width="100%" style="background-image: url('../../../App_Themes/Blue (Horizontal orientation)/Web/mItemBack.gif');">
                        <tr>
                            <td align="center" style="width: 20px;">
                                <img id="img1" src="../../../App_Themes/Glass/PivotGrid/pgCollapsedButton.gif" alt="Daralt/Geniþlet"
                                    onclick="SetDivVisibility('div1','Img1');" onmouseover="document.getElementById('img1').style.cursor = 'pointer';"
                                    onmouseout="document.getElementById('img1').style.cursor = 'default';" />
                            </td>
                            <td align="left">
                                <dxe:aspxlabel id="lbl1" runat="server" text="Gündem Kriterleri" />
                            </td>
                        </tr>
                    </table>
                    <div id="div1" style="position: absolute; visibility: hidden; z-index=0">
                        <table border="0" cellpadding="0" cellspacing="3" style="width: 750px">
                            <tr>
                                <td align="left" style="width: 150px" valign="top">Ýlgili Birim Seç
                                </td>
                                <td align="left" style="width: 220px; width: 250px" valign="top">
                                    <dxe:aspxcombobox id="FirmaID" runat="server" cssfilepath="~/App_Themes/Glass/{0}/styles.css"
                                        csspostfix="Glass" enableincrementalfiltering="True" imagefolder="~/App_Themes/Glass/{0}/"
                                        width="200px" valuetype="System.Int32" clientinstancename="cmb_Firma" enablecallbackmode="true"
                                        callbackpagesize="15">
                                        <buttonstyle cursor="pointer" width="13px">
                                        </buttonstyle>
                                        <clientsideevents selectedindexchanged="function(s, e) {cmbProjeID.PerformCallback(s.GetValue());}" />
                                    </dxe:aspxcombobox>
                                </td>
                                <td align="left" valign="top">Departman Seç
                                </td>
                                <td align="left" style="width: 250px" valign="top">
                                    <dxe:aspxcombobox id="ProjeID" runat="server" cssfilepath="~/App_Themes/Glass/{0}/styles.css"
                                        csspostfix="Glass" imagefolder="~/App_Themes/Glass/{0}/" valuetype="System.Int32"
                                        enableincrementalfiltering="True" enablecallbackmode="true" clientinstancename="cmbProjeID"
                                        oncallback="ProjeID_Callback">
                                        <buttonstyle cursor="pointer" width="13px">
                                        </buttonstyle>
                                    </dxe:aspxcombobox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 150px" valign="top">
                                    <dxe:aspxlabel id="lblKisiFiltrele" runat="server" text="Üzerine Atanan" />
                                </td>
                                <td align="left" style="width: 250px" valign="top">
                                    <dxe:aspxcombobox id="UzerineAtanan" runat="server" cssfilepath="~/App_Themes/Glass/{0}/styles.css"
                                        width="200px" csspostfix="Glass" datasourceid="DSUser" enableincrementalfiltering="True"
                                        imagefolder="~/App_Themes/Glass/{0}/" valuetype="System.Int32" valuefield="IndexId"
                                        textfield="UserName" enablecallbackmode="true" callbackpagesize="15">
                                        <buttonstyle cursor="pointer" width="13px">
                                        </buttonstyle>
                                    </dxe:aspxcombobox>
                                </td>
                                <td align="left" style="width: 150px" valign="top">
                                    <dxe:aspxlabel id="lblKisiFiltrele2" runat="server" text="Atayan" />
                                </td>
                                <td align="left" style="width: 150px" valign="top">
                                    <dxe:aspxcombobox id="BildirimiGiren" runat="server" cssfilepath="~/App_Themes/Glass/{0}/styles.css"
                                        width="200px" csspostfix="Glass" datasourceid="DSUser" enableincrementalfiltering="True"
                                        imagefolder="~/App_Themes/Glass/{0}/" valuetype="System.String" valuefield="UsString"
                                        textfield="UserName" enablecallbackmode="true" callbackpagesize="15">
                                        <buttonstyle cursor="pointer" width="13px">
                                        </buttonstyle>
                                    </dxe:aspxcombobox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 150px" valign="top">Gündem Durum Seç
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
                                <td align="left" style="width: 150px" valign="top">Müdehale Yöntemi Seç
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
                                <td align="left" style="width: 150px" valign="top">Gündem Sýnýf Seç
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
                                    <dxe:aspxlabel id="lblProjeSinif" runat="server" text="Departman Grubu Seç" />
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
                    <dxwgv:aspxgridview id="grid" runat="server" autogeneratecolumns="False" cssfilepath="~/App_Themes/Glass/{0}/styles.css"
                        csspostfix="Glass" datasourceid="DataTableList" keyfieldname="ID" width="1540px"
                        clientinstancename="Grid" oncustomcallback="grid_CustomCallback" onhtmlrowprepared="grid_HtmlRowPrepared"
                        onhtmldatacellprepared="grid_HtmlDataCellPrepared" onafterperformcallback="grid_AfterPerformCallback">
                        <settingstext title="Gündem Listesi" grouppanel="Gruplamak istediðiniz kolon baþlýðýný buraya s&#252;r&#252;kleyiniz."
                            confirmdelete="Kayýt silinsin mi?" emptydatarow="#" customizationwindowcaption="Kolon Ekle/Çýkart" />
                        <settingsediting mode="inline" popupeditformwidth="750px" popupeditformhorizontaloffset="50"
                            popupeditformverticaloffset="50" />
                        <settingspager pagesize="50" showseparators="True">
                        </settingspager>
                        <images imagefolder="~/App_Themes/Glass/{0}/">
                        </images>
                        <settingscustomizationwindow enabled="True" />
                        <settingsloadingpanel text="Yükleniyor..." />
                        <settings showfilterrow="True" showstatusbar="Visible" showgroupedcolumns="false"
                            showpreview="True" showtitlepanel="True" showgrouppanel="true" />
                        <styles cssfilepath="~/App_Themes/Glass/{0}/styles.css" csspostfix="Glass">
                            <alternatingrow enabled="True">
                            </alternatingrow>
                            <header sortingimagespacing="5px" imagespacing="5px">
                            </header>
                        </styles>
                        <settingsbehavior columnresizemode="Control" confirmdelete="True" allowfocusedrow="true" />
                        <columns>
                            <dxwgv:gridviewcommandcolumn buttontype="Image" showselectcheckbox="true" visibleindex="0"
                                width="80px">
                                <headertemplate>
                                    <input id="Button1" type="button" onclick="Grid.PerformCallback(true);" value="+"
                                        title="Tümünü Seç" />
                                    <input id="Button2" type="button" onclick="Grid.PerformCallback(false);" value="-"
                                        title="Tümünü Seçme" />
                                </headertemplate>
                                <clearfilterbutton visible="true" text="Süzmeyi Temizle">
                                    <image url="~/images/reload2.jpg" alternatetext="Süzmeyi Temizle" />
                                </clearfilterbutton>
                            </dxwgv:gridviewcommandcolumn>
                            <dxwgv:gridviewdatacolumn fieldname="ID" showincustomizationform="false" visible="False">
                            </dxwgv:gridviewdatacolumn>
                            <dxwgv:gridviewdatacolumn fieldname="DurumID" showincustomizationform="false" visible="False">
                            </dxwgv:gridviewdatacolumn>
                            <dxwgv:gridviewdatacolumn fieldname="YaziRengi" showincustomizationform="false" visible="false">
                            </dxwgv:gridviewdatacolumn>
                            <dxwgv:gridviewdatacheckcolumn caption="Ana" width="40px" fieldname="IsMain">
                                <propertiescheckedit valuechecked="1" valueunchecked="0" valuetype="System.Int32">
                                </propertiescheckedit>
                            </dxwgv:gridviewdatacheckcolumn>
                            <dxwgv:gridviewdatacolumn fieldname="IndexID" width="60px" caption="PNR" sortorder="Descending"
                                settings-allowsort="False">
                                <settings allowsort="False"></settings>
                            </dxwgv:gridviewdatacolumn>
                            <dxwgv:gridviewdatacolumn caption="Tespit Eden" fieldname="CreatedBy" width="120px">
                                <settings autofiltercondition="Contains" />
                            </dxwgv:gridviewdatacolumn>
                            <dxwgv:gridviewdatacolumn fieldname="Baslik" caption="Gündem Tanýsý" width="300px">
                                <dataitemtemplate>
                                    <dxe:aspxhyperlink id="IssueLink" runat="server" navigateurl=<%#"JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id="+Eval("IndexId")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                                        text='<%#Eval("Baslik")%>'>
                                    </dxe:aspxhyperlink>
                                </dataitemtemplate>
                                <settings autofiltercondition="Contains" />
                            </dxwgv:gridviewdatacolumn>
                            <dxwgv:gridviewdatacolumn caption=" " width="20px">
                                <dataitemtemplate>
                                    <img src="../../../images/details.png" alt="Yazýþma Geçmiþini Aç" style="cursor: pointer"
                                        onclick="OpenIssueDetail('<%#Eval("IndexId") %>')" />
                                </dataitemtemplate>
                            </dxwgv:gridviewdatacolumn>
                            <dxwgv:gridviewdatacolumn caption=" " fieldname="RelatedPop3Id" width="20px">
                                <dataitemtemplate>
                                    <dxe:aspxhyperlink id="img_Sound" runat="server" navigateurl=<%#"JavaScript:OpenMedyaPage('"+Eval("RelatedPop3Id")+"');"%>
                                        imageurl="~/images/sound.png" imageheight="16px" imagewidth="16px">
                                    </dxe:aspxhyperlink>
                                </dataitemtemplate>
                            </dxwgv:gridviewdatacolumn>
                            <dxwgv:gridviewdatacomboboxcolumn caption="Birim" fieldname="FirmaID" width="75px">
                                <propertiescombobox textfield="FirmaName" datasourceid="DSFirma" valuefield="IndexId"
                                    enableincrementalfiltering="true" valuetype="System.Int32" enablecallbackmode="true"
                                    callbackpagesize="15">
                                </propertiescombobox>
                            </dxwgv:gridviewdatacomboboxcolumn>
                            <dxwgv:gridviewdatacomboboxcolumn caption="Departman" fieldname="ProjeID" width="75px">
                                <propertiescombobox textfield="Adi" datasourceid="DsProje" valuefield="IndexId" enableincrementalfiltering="true"
                                    valuetype="System.Int32" enablecallbackmode="true" callbackpagesize="15">
                                </propertiescombobox>
                            </dxwgv:gridviewdatacomboboxcolumn>
                            <dxwgv:gridviewdatadatecolumn caption="Tespit Tarihi" sortorder="Descending" width="60px"
                                fieldname="BildirimTarihi">
                            </dxwgv:gridviewdatadatecolumn>
                            <dxwgv:gridviewdatacolumn visible="false" fieldname="MainBaslik" caption="Ana Gündem">
                                <settings autofiltercondition="Contains" />
                            </dxwgv:gridviewdatacolumn>
                            <dxwgv:gridviewdatacomboboxcolumn caption="Gündem Ýlgilisi" fieldname="UserID" visible="false"
                                showincustomizationform="false">
                                <propertiescombobox textfield="UserName" datasourceid="DSUser" valuefield="IndexId"
                                    enableincrementalfiltering="true" valuetype="System.Int32" enablecallbackmode="true"
                                    callbackpagesize="15">
                                </propertiescombobox>
                            </dxwgv:gridviewdatacomboboxcolumn>
                            <dxwgv:gridviewdatacomboboxcolumn caption="Sýnýf" fieldname="VirusSinifID" width="75px">
                                <propertiescombobox textfield="Adi" datasourceid="DSVirusSinif" valuefield="IndexId"
                                    enableincrementalfiltering="true" valuetype="System.Int32" enablecallbackmode="true"
                                    callbackpagesize="15">
                                </propertiescombobox>
                            </dxwgv:gridviewdatacomboboxcolumn>
                            <dxwgv:gridviewdatatextcolumn caption="Op. Süresi" width="50px" fieldname="HarcananSure"
                                unboundtype="decimal">
                                <propertiestextedit displayformatstring="{0:#0}" />
                            </dxwgv:gridviewdatatextcolumn>
                            <dxwgv:gridviewdatatextcolumn width="60px" fieldname="HarcananSure2" unboundtype="decimal"
                                visible="false" showincustomizationform="false">
                                <headercaptiontemplate>
                                    Kapatýlma<br />
                                    Operasyon Süresi
                                </headercaptiontemplate>
                                <propertiestextedit displayformatstring="n" />
                            </dxwgv:gridviewdatatextcolumn>
                            <dxwgv:gridviewdatadatecolumn caption="Planlanan Op. Tarihi" fieldname="TeslimTarihi"
                                width="60px">
                                <headercaptiontemplate>
                                    Planlanan<br />
                                    Op. Tarihi
                                </headercaptiontemplate>
                            </dxwgv:gridviewdatadatecolumn>
                            <%--  <dxwgv:GridViewDataComboBoxColumn Caption="Önem Derecesi" FieldName="OnemDereceID"
                                Width="60px">
                                <HeaderCaptionTemplate>
                                    Önem<br />
                                    Derecesi
                                </HeaderCaptionTemplate>
                                <PropertiesComboBox TextField="OnemDereceName" DataSourceID="DSOnemDerece" ValueField="IndexId"
                                    EnableIncrementalFiltering="true" ValueType="System.Int32" EnableCallbackMode="true"
                                    CallbackPageSize="15">
                                </PropertiesComboBox>
                            </dxwgv:GridViewDataComboBoxColumn>--%>
                            <dxwgv:gridviewdatadatecolumn fieldname="ReelOperationDate" visible="false" width="100px"
                                showincustomizationform="false">
                            </dxwgv:gridviewdatadatecolumn>
                            <%--                            <dxwgv:GridViewDataComboBoxColumn Width="75px" FieldName="HataTipID">
                                <PropertiesComboBox TextField="Adi" DataSourceID="DSHataTip" ValueField="IndexId"
                                    EnableIncrementalFiltering="true" ValueType="System.Int32" EnableCallbackMode="true"
                                    CallbackPageSize="15">
                                </PropertiesComboBox>
                                <HeaderCaptionTemplate>
                                    Müdehale<br />
                                    Yöntemi
                                </HeaderCaptionTemplate>
                            </dxwgv:GridViewDataComboBoxColumn>
                            <dxwgv:GridViewDataColumn FieldName="Asilama" Caption="Aþý">
                            </dxwgv:GridViewDataColumn>--%>
                            <dxwgv:gridviewdatatextcolumn fieldname="DurumName" width="75px" sortorder="Descending"
                                settings-allowsort="False" caption="Durum" groupindex="0">
                                <settings allowsort="False"></settings>
                            </dxwgv:gridviewdatatextcolumn>
                            <dxwgv:gridviewdatamemocolumn width="250px" visible="false" showincustomizationform="false"
                                caption="Son Yorum" fieldname="Description">
                                <settings autofiltercondition="Contains" />
                            </dxwgv:gridviewdatamemocolumn>
                            <dxwgv:gridviewdatamemocolumn width="250px" caption="Son Yapýlan Yorum" visible="false"
                                showincustomizationform="false" fieldname="SonYorum">
                                <settings autofiltercondition="Contains" />
                            </dxwgv:gridviewdatamemocolumn>
                            <dxwgv:gridviewdatacolumn caption="Anahtar Kelime" fieldname="KeyWords" visible="false">
                                <settings autofiltercondition="Contains" />
                            </dxwgv:gridviewdatacolumn>
                        </columns>
                    </dxwgv:aspxgridview>
                    <dxwgv:aspxgridviewexporter exportedrowtype="Selected" id="gridExport" runat="server">
                        <styles>
                            <cell font-names="Verdana" font-size="8">
                            </cell>
                            <header font-names="Verdana" font-size="8">
                            </header>
                        </styles>
                    </dxwgv:aspxgridviewexporter>
                </dxp:panelcontent>
            </panelcollection>
        </dxrp:aspxroundpanel>
        <dxpc:aspxpopupcontrol id="popup" runat="server" allowdragging="True" allowresize="True"
            closeaction="OuterMouseClick" enableviewstate="False" popuphorizontalalign="WindowCenter"
            popupverticalalign="WindowCenter" showfooter="false" showheader="false" width="600px"
            height="300px" footertext="Paneli sað alt köþesinden tutup boyutlandýrabilirsiniz..."
            headertext="Gündem Tarihçesi" clientinstancename="FeedPopupControl" enablehierarchyrecreation="True"
            dragelement="Window" modal="false">
            <windows>
                <dxpc:popupwindow headertext="Gündem Tarihçesi" modal="false" name="Preview">
                </dxpc:popupwindow>
            </windows>
        </dxpc:aspxpopupcontrol>
        <dxcb:aspxcallback id="CallbackPreview" clientinstancename="CallbackPreview" oncallback="CallbackPreview_Callback"
            runat="server">
            <clientsideevents callbackcomplete="function(s, e) {   var win = FeedPopupControl.GetWindow(0);
                    FeedPopupControl.SetWindowContentUrl(win,'Preview.aspx?id='+e.result);
                    FeedPopupControl.ShowWindow(win);
                window.scrollTo(0,0); 
                }" />
        </dxcb:aspxcallback>
        <dxpc:aspxpopupcontrol id="popup2" runat="server" allowdragging="True" allowresize="True"
            closeaction="OuterMouseClick" enableviewstate="False" popuphorizontalalign="WindowCenter"
            popupverticalalign="WindowCenter" showfooter="false" showheader="true" width="600px"
            height="300px" footertext="Paneli sað alt köþesinden tutup boyutlandýrabilirsiniz..."
            headertext="Olay Penceresi" clientinstancename="FeedPopupControl2" enablehierarchyrecreation="True"
            dragelement="Window" modal="false">
            <windows>
                <dxpc:popupwindow headertext="Olay Penceresi" modal="false" name="Preview">
                </dxpc:popupwindow>
            </windows>
        </dxpc:aspxpopupcontrol>
        <dxcb:aspxcallback id="CallbackPreview2" clientinstancename="CallbackPreview2" runat="server">
            <clientsideevents callbackcomplete="function(s, e) {   var win = FeedPopupControl2.GetWindow(0);
                    FeedPopupControl2.SetWindowContentUrl(win,'../../../Frames/Events.aspx');
                    FeedPopupControl2.ShowWindow(win);}" />
        </dxcb:aspxcallback>
    </div>
</asp:Content>
