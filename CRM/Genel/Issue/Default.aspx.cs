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
using DevExpress.Web.ASPxMenu;
using DevExpress.Web.ASPxEditors;
using Model.Crm;
using DevExpress.Web.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxClasses;

public partial class CRM_Genel_Issues_Default : System.Web.UI.Page
{
    CrmUtils data = new CrmUtils();

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
        if (!Security.CheckPermission(this.Context, "Bildirim Ara", "Update"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }
        // Eventleri tanimlayalim
        this.menu.ItemClick += new MenuItemEventHandler(MenuFirma_ItemClick);
        ASPxPageControl1.ActiveTabIndex = 0;
        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }
        fillcomboxes();

        Guid id = Guid.Empty;

        string sID = this.Request.Params["id"].Replace("'", "");
        if ((sID != null) && (sID.Trim() != "0"))
        {
            this.Page.Title = Request.Url.ToString();
            id = new Guid(sID);
            LoadDocument(id);
            this.HiddenID.Value = id.ToString();
        }
    }

    void fillcomboxes()
    {
        data.BindComboBoxesInt(this.Context, FirmaID, "EXEC FirmaListByUserName '" + Membership.GetUser().UserName + "'", "FirmaID", "FirmaName");
        data.BindComboBoxesInt(this.Context, DurumID, "EXEC DurumListByUserName '" + Membership.GetUser().UserName + "'", "DurumID", "Adi");
        data.BindComboBoxesInt(this.Context, OnemDereceID, "SELECT OnemDereceID,Adi FROM OnemDereceleri ORDER BY Adi", "OnemDereceID", "Adi");
        data.BindComboBoxesInt(this.Context, HataTipID, "SELECT HataTipID,Adi FROM HataTipleri ORDER BY Adi", "HataTipID", "Adi");
        data.BindComboBoxesInt(this.Context, UserID, "EXEC ISAssignUserByUserName '" + Membership.GetUser().UserName + "'", "UserID", "UserName");
    }

    private void MenuFirma_ItemClick(object source, MenuItemEventArgs e)
    {

        if (e.Item.Name.Equals("save"))
        {
            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlýþ bilgi giriþi!", "stkey1");
                return;
            }
            Guid id = SaveDocument();
            Session["BelgeID"] = id.ToString();
            Response.Write("<script language='Javascript'>{ parent.location.href='?id=" + (String)Session["BelgeID"] + "'; }</script>");
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    private Guid SaveDocument()
    {
        try
        {
            DB.BeginTrans(this.Context);
            Guid id = Guid.NewGuid();
            StringBuilder sb = new StringBuilder();
            sb = new StringBuilder();
            sb.Append("EXEC OnUpdateIssue ");
            sb.Append("@IssueID,@FirmaID,@ProjeID,@UserID,@DurumID,@OnemDereceID,@Active,@Baslik,@Description,@HataAdimlari,@KeyWords");
            sb.Append(",@BildirimTarihi,@CreatedBy,@CreationDate");

            SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@IssueID", id);
            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
            DB.AddParam(cmd, "@CreatedBy", 50, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@Active", 1);
            DB.AddParam(cmd, "@Baslik", 255, this.Title.Value.ToString());
            DB.AddParam(cmd, "@Description", 4000, this.Description.Text);
            if (this.HataAdimlari.Text != null)
                DB.AddParam(cmd, "@HataAdimlari", 4000, this.HataAdimlari.Text);
            else
                DB.AddParam(cmd, "@HataAdimlari", SqlDbType.NVarChar);
            DB.AddParam(cmd, "@BildirimTarihi", DateTime.Now);
            Guid tmpID;
            tmpID = new Guid(this.FirmaID.Value.ToString());
            DB.AddParam(cmd, "@FirmaID", tmpID);
            tmpID = new Guid(this.ProjeID.Value.ToString());
            DB.AddParam(cmd, "@ProjeID", tmpID);
            tmpID = new Guid(this.DurumID.Value.ToString());
            DB.AddParam(cmd, "@DurumID", tmpID);
            tmpID = new Guid(this.UserID.Value.ToString());
            DB.AddParam(cmd, "@UserID", tmpID);
            if (this.KeyWords.Value != null)
                DB.AddParam(cmd, "@KeyWords", 255, this.KeyWords.Value.ToString());
            else
                DB.AddParam(cmd, "@KeyWords", SqlDbType.NVarChar);
            if (this.OnemDereceID.SelectedIndex > 0)
            {
                tmpID = new Guid(this.OnemDereceID.Value.ToString());
                DB.AddParam(cmd, "@OnemDereceID", tmpID);
            }
            else
                DB.AddParam(cmd, "@OnemDereceID", SqlDbType.UniqueIdentifier);
            cmd.ExecuteNonQuery();
            DB.Commit(this.Context);
            return id;
        }
        catch (Exception ex)
        {
            DB.Rollback(this.Context);
            return Guid.Empty;
        }
    }

    private void LoadDocument(Guid id)
    {
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder();
        sb.Append("SELECT * FROM Issue WHERE IssueID=@IssueID ");
        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@IssueID", id);
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        if (!rdr.Read())
        {
            this.Response.StatusCode = 500;
            this.Response.End();
            return;
        }
        if ((rdr["HataTipID"].ToString() != null) && (rdr["HataTipID"].ToString() != ""))
            this.HataTipID.SelectedIndex = this.HataTipID.Items.IndexOfValue(rdr["HataTipID"].ToString());
        if ((rdr["DurumID"].ToString() != null) && (rdr["DurumID"].ToString() != ""))
            this.DurumID.SelectedIndex = this.DurumID.Items.IndexOfValue(rdr["DurumID"].ToString());
        if ((rdr["OnemDereceID"].ToString() != null) && (rdr["OnemDereceID"].ToString() != ""))
            this.OnemDereceID.SelectedIndex = this.OnemDereceID.Items.IndexOfValue(rdr["OnemDereceID"].ToString());
        if ((rdr["UserID"].ToString() != null) && (rdr["UserID"].ToString() != ""))
            this.UserID.SelectedIndex = this.UserID.Items.IndexOfValue(rdr["UserID"].ToString());
        if ((rdr["FirmaID"].ToString() != null) && (rdr["FirmaID"].ToString() != ""))
            this.FirmaID.SelectedIndex = this.FirmaID.Items.IndexOfValue(rdr["FirmaID"].ToString());
        if ((rdr["ProjeID"].ToString() != null) && (rdr["ProjeID"].ToString() != ""))
            this.ProjeID.SelectedIndex = this.ProjeID.Items.IndexOfValue(rdr["ProjeID"].ToString());
        if (rdr["ModifiedBy"].ToString().Length == 0)
            this.IssuedBy.Value = rdr["CreatedBy"];
        else
            this.IssuedBy.Value = rdr["ModifiedBy"];
        this.Title.Value = rdr["Baslik"];
        this.Description.Text = rdr["Description"].ToString();
        this.HataAdimlari.Text = rdr["HataAdimlari"].ToString();
        this.KeyWords.Value = rdr["KeyWords"];
        this.Comments.Text = rdr["LastComment"].ToString();
        this.BildirimTarihi.Value = rdr["BildirimTarihi"];
        this.TeslimTarihi.Value = rdr["TeslimTarihi"];
        if (rdr["Active"].ToString() == "False")
            Active.Checked = false;
        else
            Active.Checked = true;
        rdr.Close();
    }

    protected void ProjeID_Callback(object source, CallbackEventArgsBase e)
    {
        if (e.Parameter.ToString() == null || e.Parameter.ToString() == "")
            return;
        data.BindComboBoxesInt(this.Context, ProjeID, "SELECT ProjeID,Adi FROM Proje "
        + " WHERE CONVERT(char(50),FirmaID)='" + e.Parameter.ToString() + "' ORDER BY Adi", "ProjeID", "Adi");
    }
}
