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
using System.Data.SqlClient;
using Model.Crm;
using System.Text;
using DevExpress.Web.ASPxGridView;

public partial class CRM_Pop3MailAttachments_Pop3Attachment : System.Web.UI.Page
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
        if (this.IsPostBack || this.IsCallback) return;

        InitDTList(this.DTList.Table);

        this.Title = "";

        object oId = this.Request.Params["id"] as object;
        object oAttachmentName = this.Request.Params["AttachmentName"] as object;
        //string oId = "460B848C-DA5B-4317-BB66-E3DBC9096516";
        if ((oId != null))
        {
            int id = int.Parse(oId.ToString());
            string sAttachmentName = oAttachmentName != null ? oAttachmentName.ToString() : "";
            LoadDocument(id, sAttachmentName);
        }
    }

    private void InitDTList(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("MailId", typeof(Guid));
        dt.Columns.Add("AttachmentName", typeof(string));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument(int id, string AttachmentName)
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT MailAttachments,Id FROM Pop3Mails WHERE sayac=@Id");
        DB.AddParam(cmd, "@Id", id);
        cmd.CommandTimeout = 1000;
        IDataReader rdr = cmd.ExecuteReader();
        rdr.Read();
        string Attachments = rdr["MailAttachments"].ToString();
        Guid id2 = new Guid(rdr["Id"].ToString());
        rdr.Close();
        string[] _Attachments = Attachments.Split(';');

        this.DTList.Table.Clear();
        foreach (string item in _Attachments)
        {
            if (!String.IsNullOrEmpty(item))
            {
                DataRow row = this.DTList.Table.NewRow();
                Guid newId = Guid.NewGuid();
                row["ID"] = newId;
                row["MailId"] = id2;
                row["AttachmentName"] = item;
                this.DTList.Table.Rows.Add(row);
            }
        }
    }

    protected void GridNot_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "AttachmentName")
        {
            //bu kýsým bir procedure yazýlarak sql den de yapýlabilir 
            Literal literalFiles = this.Grid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "LiteralNotDosyalar") as Literal;
            Label labelNotID = this.Grid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "LabelNotID") as Label;
            Label AttachName = this.Grid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "AttachName") as Label;

            Guid id = new Guid(labelNotID.Text.ToString());
            StringBuilder sb = new StringBuilder("<img src='./../../images/toolbar.arrowright.gif'/>&nbsp;");
            sb.Append("<a href='download.aspx?id=" + id.ToString().Trim() + "&AttachName=" + AttachName.Text + "' target='_self'>");
            sb.Append("<img border=0 src='./../../images/" + NotesUtils.GetFileIcon(AttachName.Text) + "'" + "/>");
            sb.Append(AttachName.Text + "</a>");

            literalFiles.Text = sb.ToString();

        }
    }
}
