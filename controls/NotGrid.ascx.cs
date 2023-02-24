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
using DevExpress.Web;
using DevExpress.Web.ASPxMenu;
using DevExpress.Web.ASPxEditors;
using Model.Crm;
using System.Text;
using DevExpress.Web.ASPxRoundPanel;
using DevExpress.Web.ASPxGridView;
using System.Collections.Generic;


public partial class controls_NotGrid : System.Web.UI.UserControl
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        string sBagliID = (String)Request.QueryString["id"].ToString().Trim();
        if (sBagliID == "" || sBagliID == null || sBagliID == "0")
            return;
        int BagliNesneTipi = int.Parse((String)Request.QueryString["NoteOwner"].ToString().Trim());
        if (BagliNesneTipi == 0)
            return;
        if ((sBagliID != null) && (sBagliID != "") && (sBagliID != "0"))
        {
            if ((BagliNesneTipi > 0) && (BagliNesneTipi < 7))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("JavaScript:PopWin=OpenPopupWinBySize('../../Notes/edit.aspx");
                sb.Append("?id=0&BagliID=" + sBagliID + "&Tip=" + BagliNesneTipi.ToString() + "','540','420');PopWin.focus();");
                this.ASPxRoundPanelNot.HeaderNavigateUrl = sb.ToString();

                if (this.DataTableNotes.Table.Columns.Count == 0)
                    InitGridTable(this.DataTableNotes.Table);
                int gBagliID = int.Parse(sBagliID);
                LoadDocument(gBagliID);
            }
        }
    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("NotID", typeof(Guid));
        dt.Columns.Add("BagliID", typeof(int));
        dt.Columns.Add("BagliNesneTipi", typeof(int));
        dt.Columns.Add("Tanim", typeof(string));
        dt.Columns.Add("Aciklama", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModificationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument(int BagliId)
    {
        this.DataTableNotes.Table.Rows.Clear();

        using (SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM Notes WHERE BagliID=@BagliID ORDER BY ModificationDate DESC,CreationDate DESC"))
        {
            DB.AddParam(cmd, "@BagliID", BagliId);
            cmd.Prepare();

            IDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DataRow row = this.DataTableNotes.Table.NewRow();

                row["ID"] = rdr["NotID"];
                row["NotID"] = rdr["NotID"];
                row["BagliID"] = rdr["BagliID"];
                row["BagliNesneTipi"] = rdr["BagliNesneTipi"];
                row["Tanim"] = rdr["Tanim"];
                row["Aciklama"] = rdr["Aciklama"];
                row["CreatedBy"] = rdr["CreatedBy"];
                row["CreationDate"] = rdr["CreationDate"];
                row["ModificationDate"] = rdr["ModificationDate"];

                this.DataTableNotes.Table.Rows.Add(row);
            }
            rdr.Close();
            
        }

        this.GridNot.DataSource = this.DataTableNotes.Table;
        this.GridNot.DataBind();
    }

    protected void GridNot_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.Name == "NotDosya")
        {
            //bu kýsým bir procedure yazýlarak sql den de yapýlabilir 
            Literal literalFiles = this.GridNot.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "LiteralNotDosyalar") as Literal;
            Label labelNotID = this.GridNot.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "LabelNotID") as Label;
            string _attlist = string.Empty;
            bool ilk = true;
            Guid id = new Guid(labelNotID.Text.ToString());
            StringBuilder sb = new StringBuilder();
            using (SqlCommand cmd = DB.SQL(this.Context, "SELECT NotDosyaID,DosyaAdi FROM NotDosya WHERE NotID=@NotID"))
            {
                DB.AddParam(cmd, "@NotID", id);
                cmd.Prepare();
                
                IDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (ilk)
                    {
                        _attlist = rdr["NotDosyaID"].ToString();
                        ilk = false;
                    }
                    else
                        _attlist += "," + rdr["NotDosyaID"].ToString();
                    string FileName = rdr["DosyaAdi"].ToString();
                    if (FileName.Length > 17)
                        FileName = FileName.Substring(0, 17) + "..." + "(" + FileName.Substring(FileName.LastIndexOf(".") + 1) + ")";


                    sb = new StringBuilder();
                    sb.Append("<img src='./../../../images/toolbar.arrowright.gif'/>&nbsp;");
                    sb.Append("<a href='../../Notes/download.aspx?id=" + rdr["NotDosyaID"].ToString() + "' target='_self'>");
                    sb.Append("<img border=0 src='./../../../images/" + NotesUtils.GetFileIcon(rdr["DosyaAdi"].ToString()) + "'" + "/>");
                    sb.Append(FileName + "</a><br>");

                    literalFiles.Text += sb.ToString();
                }
                rdr.Close();

                if (!string.IsNullOrEmpty(_attlist))
                {
                    if (_attlist.Contains(","))
                    {
                        sb = new StringBuilder();
                        sb.Append("<img src='./../../../images/toolbar.arrowright.gif'/>&nbsp;");
                        sb.Append("<a href='../../Notes/download.aspx?DownType=1&id=" + id.ToString() + "' target='_self'>");
                        sb.Append("<img border=0 src='./../../../images/expand.jpg'" + "/>");
                        sb.Append("Hepsini Ýndir</a><br>");

                        literalFiles.Text += sb.ToString();
                    }
                }

            }
        }
    }
}
