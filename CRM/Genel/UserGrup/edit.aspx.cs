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

public partial class CRM_Genel_UserGrup_edit : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "Kullanici Gruplari", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        // Eventleri tanimlayalim
        this.menu.ItemClick += new MenuItemEventHandler(Menu_ItemClick);
        ASPxPageControl1.ActiveTabIndex = 0;

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }
        InitGridAllowedProject(this.DTUserGrupToUsers.Table);
        InitGridDTUsers(this.DTUsers.Table);
        InitGridDTProje(this.DTProje.Table);
        InitGridDTProjeList(this.DTProjeList.Table);
        fillcomboxes();
        Guid id = Guid.Empty;

        string sID = this.Request.Params["id"].Replace("'", "").Trim();
        if ((sID != null) && (sID != "0"))
        {
            id = new Guid(sID);
            this.HiddenID.Value = id.ToString();
            LoadDocument(id);
        }
        else
        {
            this.ASPxPageControl2.Enabled = false;
        }

    }

    void fillcomboxes()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT UserID,(ISNULL(UserName,'')+' '+'['+ISNULL(FirstName,'')+' '+ISNULL(LastName,'')+']') AS ");
        sb.Append("UserName FROM SecurityUsers  ORDER BY UserName");
        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        IDataReader rdr = cmd.ExecuteReader();
        DTUsers.Table.Clear();
        while (rdr.Read())
        {
            DataRow row = DTUsers.Table.NewRow();
            row["UserID"] = rdr["UserID"];
            row["UserName"] = rdr["UserName"];
            DTUsers.Table.Rows.Add(row);
        }
        DTUsers.Table.AcceptChanges();
        rdr.Close();
        if((String)Request.QueryString["id"]!="0")
        {
            sb = new StringBuilder();
            sb.Append("SELECT ProjeID, Adi AS ProjeName FROM Proje ORDER BY Adi");
            cmd = DB.SQL(this.Context, sb.ToString());
            rdr = cmd.ExecuteReader();
            DTProje.Table.Clear();
            while (rdr.Read())
            {
                DataRow row = DTProje.Table.NewRow();
                row["ProjeID"] = rdr["ProjeID"];
                row["ProjeName"] = rdr["ProjeName"];
                DTProje.Table.Rows.Add(row);
            }
            DTProje.Table.AcceptChanges();
            rdr.Close();
        }

    }

    private void Menu_ItemClick(object source, MenuItemEventArgs e)
    {
        bool bYeni = true;
        if ((this.HiddenID.Value != null) && (this.HiddenID.Value != "0"))
            bYeni = false;
        else
            bYeni = true;
        if (e.Item.Name.Equals("new"))
        {
            if (!Security.CheckPermission(this.Context, "Kullanici Gruplari", "Insert"))
            {
                CrmUtils.MessageAlert(this.Page, "Yeni Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                return;
            }
            Response.Write("<script language='Javascript'>{ parent.location.href='./edit.aspx?id=0'; }</script>");
        }
        else if (e.Item.Name.Equals("save"))
        {
            if (bYeni)
            {
                if (!Security.CheckPermission(this.Context, "Kullanici Gruplari", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Kullanici Gruplari", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                    return;
                }
            }

            this.GRD_AlowedProjeList.UpdateEdit();
            Guid id = SaveDocument();
            Session["BelgeID"] = id.ToString();
            this.Response.Write("<script language='javascript'>{ parent.opener.location.reload(true);window.location.href='./edit.aspx?id=" + (String)Session["BelgeID"] + "';}</script>");

        }
        else if (e.Item.Name.Equals("saveclose"))
        {

            if (bYeni)
            {
                if (!Security.CheckPermission(this.Context, "Kullanici Gruplari", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Kullanici Gruplari", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                    return;
                }
            }
            this.GRD_AlowedProjeList.UpdateEdit();
            SaveDocument();
            this.Response.Write("<script language='javascript'>{ parent.opener.location.reload(true);parent.close();}</script>");
        }
        else if (e.Item.Name.Equals("delete"))
        {
            if (!Security.CheckPermission(this.Context, "Kullanici Gruplari", "Delete"))
            {
                CrmUtils.MessageAlert(this.Page, "Silme iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                return;
            }
            this.GRD_AlowedProjeList.UpdateEdit();
            DeleteDocument();
            this.Response.Write("<script language='javascript'>{ parent.opener.location.reload(true);parent.close();}</script>");
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    private void LoadDocument(Guid id)
    {

        StringBuilder sb = new StringBuilder();
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM UserGrup WHERE UserGrupID=@UserGrupID");
        DB.AddParam(cmd, "@UserGrupID", id);
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        if (!rdr.Read())
        {
            this.Response.StatusCode = 500;
            this.Response.End();
            return;
        }
        ProjeName.Value = rdr["Adi"].ToString();
        Description.Value = rdr["Description"].ToString();
        rdr.Close();

        #region User Ekleniyor...
        this.DTUserGrupToUsers.Table.Clear();
        sb.Append("SELECT Uap.*,Scu.FirstName,Scu.LastName FROM UserGrupToUsers Uap ");
        sb.Append("LEFT OUTER JOIN SecurityUsers Scu ON Uap.UserID=Scu.UserID ");
        sb.Append("WHERE Uap.UserGrupID=@UserGrupID ");
        sb.Append("ORDER BY CreationDate DESC");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@UserGrupID", id);
        cmd.Prepare();
        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTUserGrupToUsers.Table.NewRow();

            row["ID"] = rdr["UserGrupToUsersID"];
            row["UserGrupToUsersID"] = rdr["UserGrupToUsersID"];
            row["UserID"] = rdr["UserID"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["FirstName"] = rdr["FirstName"];
            row["LastName"] = rdr["LastName"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];

            this.DTUserGrupToUsers.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTUserGrupToUsers.Table.AcceptChanges();
        #endregion

        #region Projeler Ekleniyor...
        this.DTProjeList.Table.Clear();
        sb = new StringBuilder();
        sb.Append("SELECT * FROM ProjeUserGrupList WHERE UserGrupID=@UserGrupID ");
        sb.Append("ORDER BY CreationDate DESC");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@UserGrupID", id);
        cmd.Prepare();
        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTProjeList.Table.NewRow();

            row["ID"] = rdr["ProjeUserGrupListID"];
            row["ProjeUserGrupListID"] = rdr["ProjeUserGrupListID"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["UserGrupID"] = rdr["UserGrupID"];
            row["ProjeID"] = rdr["ProjeID"];
            row["CreationDate"] = rdr["CreationDate"];
    
            this.DTProjeList.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTProjeList.Table.AcceptChanges();
        #endregion

    }

    private Guid SaveDocument()
    {
        DB.BeginTrans(this.Context);

        Guid id = Guid.Empty;
        if (this.HiddenID.Value.Length != 0)
        {
            id = new Guid(this.HiddenID.Value);
        }
        SqlCommand cmd = null;
        StringBuilder sb = new StringBuilder();
        if (id == Guid.Empty)
        {
            sb = new StringBuilder();
            sb.Append("INSERT INTO UserGrup (");
            sb.Append("UserGrupID");
            sb.Append(",Adi");
            if (Description.Text != null)
                sb.Append(",Description");
            sb.Append(",CreatedBy");
            sb.Append(",CreationDate)");
            sb.Append(" VALUES(");
            sb.Append("@UserGrupID");
            sb.Append(",@Adi");
            if (Description.Text != null)
                sb.Append(",@Description");
            sb.Append(",@CreatedBy");
            sb.Append(",@CreationDate)");
            cmd = DB.SQL(this.Context, sb.ToString());
            id = Guid.NewGuid();
            DB.AddParam(cmd, "@UserGrupID", id);
            DB.AddParam(cmd, "@CreatedBy", 100, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
        }
        else
        {
            sb = new StringBuilder();
            sb.Append("UPDATE UserGrup SET ");
            sb.Append("Adi=@Adi");
            sb.Append(",Description=@Description");
            sb.Append(",ModifiedBy=@ModifiedBy");
            sb.Append(",ModificationDate=@ModificationDate");
            sb.Append(" WHERE UserGrupID=@UserGrupID");
            cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@UserGrupID", id);
            DB.AddParam(cmd, "@ModifiedBy", 100, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
        }
        DB.AddParam(cmd, "@Adi", 100, ProjeName.Value.ToString().ToUpper());
        DB.AddParam(cmd, "@Description", 255, this.Description.Text.ToUpper());
        string ControlCom = null;
        cmd.Prepare();
        cmd.ExecuteNonQuery();

        if (id != Guid.Empty)
        {
            #region Kullanýcý Grubu Kullanýcýlarý
            DataTable changes = this.DTUserGrupToUsers.Table.GetChanges();
            if (changes != null)
            {
                foreach (DataRow row in changes.Rows)
                {
                    switch (row.RowState)
                    {
                        case DataRowState.Added:
                            sb = new StringBuilder();
                            sb.Append("INSERT INTO UserGrupToUsers(UserGrupToUsersID,UserGrupID,UserID,CreatedBy,CreationDate)");
                            sb.Append("VALUES(@UserGrupToUsersID,@UserGrupID,@UserID,@CreatedBy,@CreationDate)");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@UserGrupToUsersID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@UserID", (Guid)row["UserID"]);
                            DB.AddParam(cmd, "@UserGrupID", id);
                            DB.AddParam(cmd, "@CreatedBy", 50, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Modified:
                            sb = new StringBuilder();
                            sb.Append("UPDATE UserGrupToUsers SET ");
                            sb.Append("UserID=@UserID,");
                            sb.Append("ModifiedBy=@ModifiedBy,");
                            sb.Append("ModificationDate=@ModificationDate");
                            sb.Append(" WHERE UserGrupToUsersID=@UserGrupToUsersID");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@UserGrupToUsersID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@UserID", (Guid)row["UserID"]);
                            DB.AddParam(cmd, "@ModifiedBy", 50, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Deleted:
                            cmd = DB.SQL(this.Context, "DELETE FROM UserGrupToUsers WHERE UserGrupToUsersID=@UserGrupToUsersID");
                            DB.AddParam(cmd, "@UserGrupToUsersID", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                    }
                }
            }
            #endregion

            #region Kullanýcý Grubu Projeleri
            changes = this.DTProjeList.Table.GetChanges();
            if (changes != null)
            {
                foreach (DataRow row in changes.Rows)
                {
                    switch (row.RowState)
                    {
                        case DataRowState.Added:
                            sb = new StringBuilder();
                            sb.Append("INSERT INTO ProjeUserGrupList(ProjeUserGrupListID,UserGrupID,ProjeID,CreatedBy,CreationDate)");
                            sb.Append("VALUES(@ProjeUserGrupListID,@UserGrupID,@ProjeID,@CreatedBy,@CreationDate)");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@ProjeUserGrupListID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@ProjeID", (Guid)row["ProjeID"]);
                            DB.AddParam(cmd, "@UserGrupID", id);
                            DB.AddParam(cmd, "@CreatedBy", 50, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Modified:
                            break;
                        case DataRowState.Deleted:
                            cmd = DB.SQL(this.Context, "DELETE FROM ProjeUserGrupList WHERE ProjeUserGrupListID=@ProjeUserGrupListID");
                            DB.AddParam(cmd, "@ProjeUserGrupListID", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                    }
                }
            }
            #endregion
        }

        DB.Commit(this.Context);

        return id;
    }

    protected void GRD_AlowedProjeList_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (GRD_AlowedProjeList.IsEditing && e.Column.FieldName == "UserID")
        {
            ASPxComboBox cmb = e.Editor as ASPxComboBox;
            ListEditItem item;
            cmb.Items.Clear();
            cmb.DataSourceID = null;
            foreach (DataRow row in DTUsers.Table.Rows)
            {
                DataRow[] rows = DTUserGrupToUsers.Table.Select("UserID='" + row["UserID"].ToString() + "'");
                if (rows.Length == 0)
                {
                    item = new ListEditItem();
                    item.Text = row["UserName"].ToString();
                    item.Value = row["UserID"];
                    cmb.Items.Add(item);
                }
            }
        }
    }

    protected void gridProjeList_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (GRD_AlowedProjeList.IsEditing && e.Column.FieldName == "ProjeID")
        {
            ASPxComboBox cmb = e.Editor as ASPxComboBox;
            ListEditItem item;
            cmb.Items.Clear();
            cmb.DataSourceID = null;
            foreach (DataRow row in DTProje.Table.Rows)
            {
                DataRow[] rows = DTProjeList.Table.Select("ProjeID='" + row["ProjeID"].ToString() + "'");
                if (rows.Length == 0)
                {
                    item = new ListEditItem();
                    item.Text = row["ProjeName"].ToString();
                    item.Value = row["ProjeID"];
                    cmb.Items.Add(item);
                }
            }
        }
    }

    private void InitGridAllowedProject(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("UserGrupToUsersID", typeof(Guid));
        dt.Columns.Add("UserID", typeof(Guid));
        dt.Columns.Add("UserGrupID", typeof(Guid));
        dt.Columns.Add("FirstName", typeof(string));
        dt.Columns.Add("LastName", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitGridDTProjeList(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("ProjeUserGrupListID", typeof(Guid));
        dt.Columns.Add("UserGrupID", typeof(Guid));
        dt.Columns.Add("ProjeID", typeof(Guid));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitGridDTUsers(DataTable dt)
    {
        dt.Columns.Add("UserID", typeof(Guid));
        dt.Columns.Add("UserName", typeof(string));

    }

    private void InitGridDTProje(DataTable dt)
    {
        dt.Columns.Add("ProjeID", typeof(Guid));
        dt.Columns.Add("ProjeName", typeof(string));

    }

    protected void GRD_AlowedProjeList_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["UserID"] == null)
                e.NewValues["UserID"] = DBNull.Value;
        }
    }

    protected void GRD_AlowedProjeList_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["UserID"] == null)
                e.NewValues["UserID"] = DBNull.Value;

        }
    }

    protected void GRD_AlowedProjeList_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["UserID"] == null)
        {
            e.RowError = "Lütfen Kiþi alanýný boþ býrakmayýnýz...";
            return;
        }

    }

    protected void gridProjeList_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["ProjeID"] == null)
        {
            e.RowError = "Lütfen Proje alanýný boþ býrakmayýnýz...";
            return;
        }

    }

    private void DeleteDocument()
    {
        string sID = this.HiddenID.Value;
        SqlCommand cmd;
        if (sID != null)
        {
            DB.BeginTrans(this.Context);

            Guid id = Guid.Empty;
            id = new Guid(this.HiddenID.Value);

            try
            {
                cmd = DB.SQL(this.Context, "DELETE FROM UserGrupToUsers WHERE UserGrupID=@UserGrupID");
                DB.AddParam(cmd, "@UserGrupID", id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd = DB.SQL(this.Context, "DELETE FROM UserGrup WHERE UserGrupID=@UserGrupID");
                DB.AddParam(cmd, "@UserGrupID", id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                DB.Commit(this.Context);
            }
            catch (Exception ex)
            {
                DB.Rollback(this.Context);
                CrmUtils.MessageAlert(this.Page, ex.Message.ToString().Replace("'", null).Replace("\r\n", null), "stkeySilinemez");
                return;
            }
            
        }
    }
}
