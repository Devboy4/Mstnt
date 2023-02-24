<%@ Page Language="C#" AutoEventWireup="true" CodeFile="menu.aspx.cs" Inherits="menu" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxNavBar"
    TagPrefix="dxnb" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v8.3" Namespace="DevExpress.Web.ASPxTreeList"
    TagPrefix="dxwtl" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        html, body
        {
            height: 100%;
            margin: 0;
            padding: 0;
            color: #fff;
        }
        a
        {
            color: #fff;
        }
        #bg
        {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
        }
        #content
        {
            position: relative;
            z-index: 1;
            padding-top: 10;
            padding-left: 10;
        }
    </style>
    <!--[if IE 6]>
<style type="text/css">
/* some css fixes for IE browsers */
html {overflow-y:hidden;}
body {overflow-y:auto;}
#bg {position:absolute; z-index:-1;}
#content {position:static;}
</style>
<![endif]-->
    <link rel="stylesheet" type="text/css" href="../ModelCRM.css" />
    <script type="text/javascript">
        function NavBar_ExpandedChanged(s, e) {
            if (e.group.GetExpanded()) {
                if (e.group.name == 'Dosya Yönetimi') {
                    document.all['DivTreeList'].style.visibility = 'visible';
                }
                else {
                    document.all['DivTreeList'].style.visibility = 'hidden';
                }
            }
        }
    </script>
</head>
<body>
    <div id="content">
        <form id="form1" runat="server">
        <model:DataTable ID="DTList" runat="server" />
        <div style="width: 190px;">
            <dxnb:ASPxNavBar ID="NavBar" AllowSelectItem="True" runat="server" Width="100%" ExpandImage-Width="0px"
                ExpandImage-Height="0px" CollapseImage-Height="0px" CollapseImage-Width="0px"
                GroupSpacing="0px">
                <CollapseImage Height="16px" Width="16px"></CollapseImage>
                <ExpandImage Height="16px" Width="16px"></ExpandImage>
                <GroupContentStyle>
                    <Paddings PaddingBottom="2px" PaddingLeft="1px" PaddingRight="1px" PaddingTop="2px" />
                </GroupContentStyle>
            </dxnb:ASPxNavBar>
        </div>
        <div id="DivTreeList">
            <dxwtl:ASPxTreeList ID="TreeList" runat="server" AutoGenerateColumns="False" DataSourceID="DTList"
                Width="100%" KeyFieldName="ID" ParentFieldName="ParentId" ClientInstanceName="TreeList">
                <Columns>
                    <dxwtl:TreeListDataColumn FieldName="DirectoryName" Caption="Dizin Adý" Width="30%">
                        <DataCellTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 20px">
                                        <img src="../images/closed.png" alt="" />
                                    </td>
                                    <td>
                                        <dxe:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl='<%# "../CRM/FileManager/File/list.aspx?id="+Eval("ID") %>'
                                            Text='<%# Container.Text %>' Target="content">
                                        </dxe:ASPxHyperLink>
                                    </td>
                                </tr>
                            </table>
                        </DataCellTemplate>
                    </dxwtl:TreeListDataColumn>
                    <dxwtl:TreeListDataColumn FieldName="ID" ReadOnly="true" Visible="false" />
                    <dxwtl:TreeListDataColumn FieldName="ParentId" ReadOnly="true" Visible="false" />
                </Columns>
                <SettingsBehavior ExpandCollapseAction="NodeDblClick" AllowDragDrop="false" />
                <Settings GridLines="Both" ShowColumnHeaders="false" ShowFooter="false" ShowRoot="true"
                    ShowTreeLines="true" SuppressOuterGridLines="true" />
                <%--<Images ImageFolder="~/App_Themes/Glass/{0}/" />--%>
                <SettingsText LoadingPanelText="Yükleniyor..." />
            </dxwtl:ASPxTreeList>
        </div>
        <asp:Panel runat="server" ID="IPPanel" Visible="false">
            <hr />
            <iframe src="../Phone/Phone.aspx" height="350px" width="100%" scrolling="no"></iframe>
        </asp:Panel>
        </form>
    </div>
</body>
</html>
