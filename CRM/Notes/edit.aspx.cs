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
using System.Text;
using System.Data.SqlClient;
using Model.Crm;
using DevExpress.Web.ASPxMenu;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.Data;
using DevExpress.Web.ASPxGridView;

public partial class MarjinalCRM_Notes_edit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Eventleri tanimlayalim
        this.menu.ItemClick += new MenuItemEventHandler(Menu_ItemClick);

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }

        string sID = this.Request.Params["id"].Replace("'", "");
        string sBagliID = null;
        int nNesneTipi = -1;

        if ((sID != null) && (sID.Trim() != "0"))
        {
            Guid id = new Guid(sID);
            SqlCommand cmd = DB.SQL(this.Context, "SELECT BagliID,BagliNesneTipi FROM Notes WHERE NotID=@NotID");
            DB.AddParam(cmd, "@NotID", id);
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                sBagliID = rdr["BagliID"].ToString();
                nNesneTipi = (int)rdr["BagliNesneTipi"];
            }
            rdr.Close();
        }
        else
        {
            sBagliID = this.Request.Params["BagliId"].Replace("'", "");
            nNesneTipi = Int16.Parse(this.Request.Params["Tip"].Replace("'", ""));
        }

        if ((nNesneTipi < 1) || (nNesneTipi > 7))
            this.Response.End();

        string sNot = null;
        if (nNesneTipi == 1)
            sNot = "Kiþi Not";
        if (nNesneTipi == 2)
            sNot = "Firma Not";
        if (nNesneTipi == 3)
            sNot = "Mecra Not";
        if (nNesneTipi == 4)
            sNot = "Özel Günler Not";
        if (nNesneTipi == 5)
            sNot = "Etkinlik Not";
        if (nNesneTipi == 6)
            sNot = "Haber Not";

        if (!Security.CheckPermission(this.Context, sNot, "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        bool bInsert = Security.CheckPermission(this.Context, sNot, "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, sNot, "Update");
        bool bDelete = Security.CheckPermission(this.Context, sNot, "Delete");

        this.menu.Items.FindByName("save").Visible = (bInsert || bUpdate || bDelete);
        this.menu.Items.FindByName("saveclose").Visible = (bInsert || bUpdate || bDelete);
        this.menu.Items.FindByName("delete").Visible = bDelete;

        for (int i = 0; i < this.GridNotDosya.Columns.Count; i++)
        {
            if (this.GridNotDosya.Columns[i] is GridViewCommandColumn)
            {
                (this.GridNotDosya.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
                break;
            }
        }

        InitGridNotDosya(this.DataTableNotDosya.Table);

        StringBuilder sb = new StringBuilder();

        if ((sBagliID != null) && (sBagliID.Trim() != "0"))
        {
            sb = new StringBuilder();

            int id = int.Parse(sBagliID);

            if (nNesneTipi == 1)
            {
                SqlCommand cmd = DB.SQL(this.Context, "SELECT Baslik FROM Issue WHERE IndexId=@IssueID");
                DB.AddParam(cmd, "@IssueID", id);
                cmd.Prepare();
                string sss = (string)cmd.ExecuteScalar();
                sb.Append("Not [Issue - " + sss + "]");
            }
            if (nNesneTipi == 2)
            {
                SqlCommand cmd = DB.SQL(this.Context, "SELECT FirmaName AS AdSoyad FROM Firma WHERE FirmaID=@FirmaID");
                DB.AddParam(cmd, "@FirmaID", id);
                cmd.Prepare();
                string sss = (string)cmd.ExecuteScalar();
                sb.Append("Not [Firma - " + sss + "]");
            }
            if (nNesneTipi == 3)
            {
                SqlCommand cmd = DB.SQL(this.Context, "SELECT Adi FROM Proje WHERE ProjeID=@ProjeID");
                DB.AddParam(cmd, "@ProjeID", id);
                cmd.Prepare();
                string sss = (string)cmd.ExecuteScalar();
                sb.Append("ProjeID - " + sss + "]");
            }
            this.GridNotDosya.SettingsText.Title = "Not";
            this.Title = sb.ToString();
            this.HiddenID.Value = sID;
        }
        else
        {
            this.HiddenID.Value = Guid.Empty.ToString();
        }

        this.BagliID.Value = sBagliID.ToString();
        this.BagliNesneTipi.Value = nNesneTipi.ToString(); //0-Manuel 1-Issue 2-Firma 3-Proje...
        if ((sID != null) && (sID.Trim() != "0"))
        {
            Guid id = new Guid(sID);
            LoadDocument(id);
        }
    }

    private void Menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("save"))
        {
            Validate();

            if (!this.IsValid)
            {
                this.Response.Write("<script language='javascript'>{ alert('Eksik veya yanlýþ bilgi giriþi!'); }</script>");
                return;
            }

            this.GridNotDosya.UpdateEdit();

            if (SaveDocument())
            {
                CrmUtils.close_page(this.Page, "stkey2");
            }
            
        }
        else if (e.Item.Name.Equals("saveclose"))
        {
            Validate();

            if (!this.IsValid)
            {
                this.Response.Write("<script language='javascript'>{ alert('Eksik veya yanlýþ bilgi giriþi!'); }</script>");
                return;
            }

            this.GridNotDosya.UpdateEdit();

            if (SaveDocument())
            {
                CrmUtils.close_page(this.Page, "stkey2");
            }
        }
        else if (e.Item.Name.Equals("delete"))
        {
            DeleteDocument();

            CrmUtils.close_page(this.Page, "stkey2");
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    private void InitGridNotDosya(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("NotDosyaID", typeof(Guid));
        dt.Columns.Add("NotID", typeof(Guid));
        dt.Columns.Add("DosyaAdi", typeof(string));
        dt.Columns.Add("DosyaBoyut", typeof(int));
        dt.Columns.Add("BoyutTuru", typeof(string));
        dt.Columns.Add("DosyaYolu", typeof(string));
        dt.Columns.Add("Link", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("AllowedRoles", typeof(string));
        dt.Columns.Add("DeniedRoles", typeof(string));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument(Guid id)
    {
        SqlConnection conn = DB.Connect(this.Context);
        SqlCommand cmd = null;
        StringBuilder sb;

        sb = new StringBuilder();
        sb.Append("SELECT * FROM Notes WHERE NotID=@NotID");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@NotID", id);
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        if (rdr.Read())
        {
            this.Tanim.Value = rdr["Tanim"];
            this.Aciklama.Value = rdr["Aciklama"];
        }
        rdr.Close();

        this.DataTableNotDosya.Table.Clear();

        sb = new StringBuilder();
        sb.Append("SELECT * FROM NotDosya WHERE NotID=@NotID");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@NotID", id);
        cmd.Prepare();

        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableNotDosya.Table.NewRow();

            row["ID"] = rdr["NotDosyaID"];
            row["NotDosyaID"] = rdr["NotDosyaID"];
            if ((rdr["NotID"] != null) && (rdr["NotID"].ToString() != Guid.Empty.ToString()))
                row["NotID"] = rdr["NotID"];
            row["DosyaAdi"] = rdr["DosyaAdi"];
            row["DosyaBoyut"] = rdr["DosyaBoyut"];
            row["BoyutTuru"] = rdr["BoyutTuru"];
            row["DosyaYolu"] = rdr["DosyaYolu"];
            row["CreationDate"] = rdr["CreationDate"];
            row["AllowedRoles"] = rdr["AllowedRoles"];
            row["DeniedRoles"] = rdr["DeniedRoles"];

            sb = new StringBuilder();
            sb.Append("<a href='./download.aspx?id=" + rdr["NotDosyaID"].ToString() + "' target='_self'>");
            sb.Append("<img border=0 src='./../../images/" + NotesUtils.GetFileIcon(rdr["DosyaAdi"].ToString()) + "'" + "/>");
            sb.Append(rdr["DosyaAdi"].ToString() + "</a><br>");

            row["Link"] = sb.ToString();

            this.DataTableNotDosya.Table.Rows.Add(row);
        }
        rdr.Close();
        conn.Close();

        this.DataTableNotDosya.Table.AcceptChanges();
    }

    private bool SaveDocument()
    {
        DB.BeginTrans(this.Context);

        Guid id = Guid.Empty;

        string sID = this.HiddenID.Value.ToString().Trim();

        if ((sID != null) && (sID != "0"))
        {
            id = new Guid(this.HiddenID.Value);
        }

        SqlCommand cmd = null;
        StringBuilder sb = new StringBuilder();



        if (id == Guid.Empty)
        {
            sb.Append("INSERT INTO Notes(");
            sb.Append("NotID");
            sb.Append(",BagliID");
            sb.Append(",BagliNesneTipi");
            sb.Append(",Tanim");
            sb.Append(",Aciklama");
            sb.Append(",AllowedRoles");
            sb.Append(",DeniedRoles");
            sb.Append(",CreatedBy");
            sb.Append(",CreationDate");
            sb.Append(")");

            sb.Append("VALUES (");
            sb.Append("@NotID");
            sb.Append(",@BagliID");
            sb.Append(",@BagliNesneTipi");
            sb.Append(",@Tanim");
            sb.Append(",@Aciklama");
            sb.Append(",@AllowedRoles");
            sb.Append(",@DeniedRoles");
            sb.Append(",@CreatedBy");
            sb.Append(",@CreationDate");
            sb.Append(")");

            cmd = DB.SQL(this.Context, sb.ToString());

            id = Guid.NewGuid();
            DB.AddParam(cmd, "@NotID", id);
            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
            DB.AddParam(cmd, "@CreatedBy", 50, Membership.GetUser().UserName);

            this.HiddenID.Value = id.ToString();
        }
        else
        {
            sb.Append("UPDATE Notes SET ");
            sb.Append("BagliID=@BagliID");
            sb.Append(",BagliNesneTipi=@BagliNesneTipi");
            sb.Append(",Tanim=@Tanim");
            sb.Append(",Aciklama=@Aciklama");
            sb.Append(",AllowedRoles=@AllowedRoles");
            sb.Append(",DeniedRoles=@DeniedRoles");
            sb.Append(",ModifiedBy=@ModifiedBy");
            sb.Append(",ModificationDate=@ModificationDate");
            sb.Append(" WHERE NotID=@NotID");

            cmd = DB.SQL(this.Context, sb.ToString());

            DB.AddParam(cmd, "@NotID", id);
            DB.AddParam(cmd, "@ModifiedBy", 50, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
        }

        int gBagliID = int.Parse(this.BagliID.Value.ToString().Trim());
        int nNesneTipi = Int16.Parse(this.BagliNesneTipi.Value.ToString().Trim());

        string tanim = this.Tanim.Value.ToString().Trim();
        if ((tanim == null) || (tanim == ""))
            tanim = "???";

        DB.AddParam(cmd, "@BagliID", gBagliID);
        DB.AddParam(cmd, "@BagliNesneTipi", nNesneTipi);
        DB.AddParam(cmd, "@Tanim", 255, this.Tanim.Value);
        DB.AddParam(cmd, "@Aciklama", 500, this.Aciklama.Value);
        DB.AddParam(cmd, "@AllowedRoles", 255, "");
        DB.AddParam(cmd, "@DeniedRoles", 255, "");

        cmd.Prepare();
        cmd.ExecuteNonQuery();

        DataTable changes = this.DataTableNotDosya.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Deleted:
                        Guid NotDosyaID = (Guid)row["ID", DataRowVersion.Original];
                        if (!NotesUtils.DeleteFile(this.Page, this.Context, NotDosyaID))
                        {
                            DB.Rollback(this.Context);
                            return false;
                        }
                        break;
                }
            }
        }

        DB.Commit(this.Context);

        return true;
    }

    private void DeleteDocument()
    {
        string sID = this.HiddenID.Value.ToString().Trim();

        if ((sID != null) && (sID != "0"))
        {
            DB.BeginTrans(this.Context);

            Guid NotID = new Guid(sID);

            if (!NotesUtils.DeleteFiles(this.Page, this.Context, NotID))
            {
                DB.Rollback(this.Context);
                return;
            }

            DB.Commit(this.Context);
        }
    }

    protected void BtnDosyaEkle_Click(object sender, EventArgs e)
    {
        if (this.fileUpload.HasFile)
        {
            if (!NotesUtils.CheckFile(this.Page, this.Context, this.fileUpload))
                return;

            int BagliID = int.Parse(this.BagliID.Value.ToString().Trim());
            int NesneTipi = Int16.Parse(this.BagliNesneTipi.Value.ToString().Trim());

            string sID = this.HiddenID.Value.ToString().Trim();

            if ((sID == null) || (sID == "0"))
            {
                if (!SaveDocument())
                    return;
            }

            sID = this.HiddenID.Value.ToString().Trim();

            if ((sID == null) || (sID == "0"))
                return;

            Guid NotID = new Guid(sID);

            if (!NotesUtils.SaveFile(this.Page, this.Context, this.fileUpload, NotID, BagliID, NesneTipi))
                return;

            this.Response.Redirect("./edit.aspx?id=" + NotID.ToString() + "&BagliId=" + BagliID.ToString() + "&Tip=" + NesneTipi.ToString());
        }
    }
}
