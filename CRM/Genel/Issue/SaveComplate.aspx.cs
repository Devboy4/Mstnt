using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Model.Crm;

public partial class CRM_Genel_Issue_SaveComplate : System.Web.UI.Page
{
    protected override object LoadPageStateFromPersistenceMedium()
    {
        return PageUtils.LoadPageStateFromPersistenceMedium(this.Context, this.Page);
    }

    protected override void SavePageStateToPersistenceMedium(object viewState)
    {
        PageUtils.SavePageStateToPersistenceMedium(this.Context, this.Page, viewState);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack || IsCallback) return;
        if ((String)Request.QueryString["SaveText"] == null || (String)Request.QueryString["id"] == null) Response.End();

        SaveText.Value = (String)Request.QueryString["SaveText"];
        SonBildirim.Text = "<a href=\"#\" onclick=\"JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id=" + (String)Request.QueryString["id"] + "&NoteOwner=1',850,650);PopWin.focus();\""
        + " >http://www.model.biz.tr/CRM/Issue/edit.aspx?id=" + (String)Request.QueryString["id"] + "</a>";

        if (Request.QueryString["DosyaYolu"] != null && (String)Request.QueryString["DosyaYolu"] != ""
            + Request.QueryString["DosyaAdi"] != null && (String)Request.QueryString["DosyaAdi"] != "")
        {
            DosyaLink.Text += "<a href=\"" + (String)Request.QueryString["DosyaYolu"] + "\" target=\"_blank\">" + (String)Request.QueryString["DosyaAdi"] + "</a>";
        }

    }
}
