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
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;

public partial class CRM_Settings_Pop3Mails_list : System.Web.UI.Page
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
        this.menu.ItemClick += new MenuItemEventHandler(menu_ItemClick);
        if (this.IsPostBack || this.IsCallback) return;

        if (!Security.CheckPermission(this.Context, "Taným - Pop3 Mail", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#e6e6fa'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        bool bInsert = Security.CheckPermission(this.Context, "Taným - Pop3 Mail", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Taným - Pop3 Mail", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Taným - Pop3 Mail", "Delete");

        if (bInsert || bUpdate || bDelete)
            this.menu.Items.FindByName("save").Visible = true;
        else
            this.menu.Items.FindByName("save").Visible = false;

        InitGrid(this.DTList.Table);
        InitGrid2(this.DTList2.Table);

        for (int i = 0; i < this.Grid.Columns.Count; i++)
        {
            if (this.Grid.Columns[i] is GridViewCommandColumn)
            {
                (this.Grid.Columns[i] as GridViewCommandColumn).NewButton.Visible = bInsert;
                (this.Grid.Columns[i] as GridViewCommandColumn).EditButton.Visible = bUpdate;
                (this.Grid.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
                (this.Grid2.Columns[i] as GridViewCommandColumn).NewButton.Visible = bInsert;
                (this.Grid2.Columns[i] as GridViewCommandColumn).EditButton.Visible = bUpdate;
                (this.Grid2.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
                break;
            }
        }

        LoadDocument();
    }

    public void menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("save"))
        {
            this.Grid.UpdateEdit();
            this.Grid2.UpdateEdit();
            if (SaveDocument())
            {
                this.Response.Redirect("./list.aspx");
            }
            else
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "Alert");
                return;
            }
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    private void InitGrid(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IndexId", typeof(int));
        dt.Columns.Add("Active", typeof(int));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Pop3Server", typeof(string));
        dt.Columns.Add("Pop3Port", typeof(string));
        dt.Columns.Add("Pop3UseSsl", typeof(int));
        dt.Columns.Add("MailPassword", typeof(string));
        dt.Columns.Add("MailUserName", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }
    private void InitGrid2(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IndexId", typeof(int));
        dt.Columns.Add("Active", typeof(int));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument()
    {
        this.DTList.Table.Rows.Clear();
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM PopMailList ORDER BY Email");
        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTList.Table.NewRow();
            row["ID"] = rdr["Id"];
            row["IndexId"] = rdr["IndexId"];
            row["Email"] = rdr["Email"];
            row["Active"] = rdr["Active"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];
            row["Pop3Server"] = rdr["Pop3Server"];
            row["Pop3Port"] = rdr["Pop3Port"];
            row["Pop3UseSsl"] = rdr["Pop3UseSsl"];
            row["MailPassword"] = rdr["MailPassword"];
            row["MailUserName"] = rdr["MailUserName"];
            this.DTList.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTList.Table.AcceptChanges();
        cmd = DB.SQL(this.Context, "SELECT * FROM Pop3MailAllowSenders ORDER BY Email");
        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTList2.Table.NewRow();
            row["ID"] = rdr["Id"];
            row["IndexId"] = rdr["IndexId"];
            row["Email"] = rdr["Email"];
            row["Active"] = rdr["Active"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];
            this.DTList2.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTList2.Table.AcceptChanges();
    }

    private bool SaveDocument()
    {
        DB.BeginTrans(this.Context);

        SqlCommand cmd = null;
        StringBuilder sb;

        DataTable changes = this.DTList.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    #region insert
                    case DataRowState.Added:
                        sb = new StringBuilder();
                        sb.Append("INSERT INTO PopMailList(Id,Email,Active,CreatedBy,CreationDate,Pop3Server,Pop3Port,Pop3UseSsl,MailPassword,MailUserName) ");
                        sb.Append("VALUES(@Id,@Email,@Active,@CreatedBy,@CreationDate,@Pop3Server,@Pop3Port,@Pop3UseSsl,@MailPassword,@MailUserName)");
                        try
                        {
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@Email", 255, row["Email"].ToString());
                            DB.AddParam(cmd, "@Active", (int)row["Active"]);
                            DB.AddParam(cmd, "@Pop3UseSsl", (int)row["Pop3UseSsl"]);
                            DB.AddParam(cmd, "@Pop3Server", 255, row["Pop3Server"].ToString());
                            DB.AddParam(cmd, "@Pop3Port", 50, row["Pop3Port"].ToString());
                            DB.AddParam(cmd, "@MailPassword", 255, row["MailPassword"].ToString());
                            DB.AddParam(cmd, "@MailUserName", 255, row["MailUserName"].ToString());
                            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            return false;
                        }
                        break;
                    #endregion
                    #region update
                    case DataRowState.Modified:
                        sb = new StringBuilder();
                        sb.Append("UPDATE PopMailList SET Email=@Email,Active=@Active,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate,");
                        sb.Append("Pop3UseSsl=@Pop3UseSsl,Pop3Server=@Pop3Server,Pop3Port=@Pop3Port,MailPassword=@MailPassword,MailUserName=@MailUserName ");
                        sb.Append("WHERE Id=@Id");
                        try
                        {
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@Email", 255, row["Email"].ToString());
                            DB.AddParam(cmd, "@Active", (int)row["Active"]);
                            DB.AddParam(cmd, "@Pop3UseSsl", (int)row["Pop3UseSsl"]);
                            DB.AddParam(cmd, "@Pop3Server", 255, row["Pop3Server"].ToString());
                            DB.AddParam(cmd, "@Pop3Port", 50, row["Pop3Port"].ToString());
                            DB.AddParam(cmd, "@MailPassword", 255, row["MailPassword"].ToString());
                            DB.AddParam(cmd, "@MailUserName", 255, row["MailUserName"].ToString());
                            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            return false;
                        }
                        break;
                    #endregion
                    #region delete
                    case DataRowState.Deleted:
                        try
                        {
                            cmd = DB.SQL(this.Context, "DELETE FROM PopMailList WHERE Id=@Id");
                            DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.CommandTimeout = 1000;
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            return false;
                        }
                        break;
                    #endregion
                }
            }
        }
        changes = this.DTList2.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    #region insert
                    case DataRowState.Added:
                        sb = new StringBuilder();
                        sb.Append("INSERT INTO Pop3MailAllowSenders(Id,Email,Active,CreatedBy,CreationDate) ");
                        sb.Append("VALUES(@Id,@Email,@Active,@CreatedBy,@CreationDate)");
                        try
                        {
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@Email", 100, row["Email"].ToString());
                            DB.AddParam(cmd, "@Active", (int)row["Active"]);
                            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            return false;
                        }
                        break;
                    #endregion
                    #region update
                    case DataRowState.Modified:
                        sb = new StringBuilder();
                        sb.Append("UPDATE Pop3MailAllowSenders SET Email=@Email,Active=@Active,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate ");
                        sb.Append("WHERE Id=@Id");
                        try
                        {
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@Email", 255, row["Email"].ToString());
                            DB.AddParam(cmd, "@Active", (int)row["Active"]);
                            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            return false;
                        }
                        break;
                    #endregion
                    #region delete
                    case DataRowState.Deleted:
                        try
                        {
                            cmd = DB.SQL(this.Context, "DELETE FROM Pop3MailAllowSenders WHERE Id=@Id");
                            DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            return false;
                        }
                        break;
                    #endregion
                }
            }
        }

        DB.Commit(this.Context);

        return true;
    }

    protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["Active"] == null)
                e.NewValues["Active"] = 0;
            if (e.NewValues["Pop3UseSsl"] == null)
                e.NewValues["Pop3UseSsl"] = 0;
        }
    }

    protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["Active"] == null)
                e.NewValues["Active"] = 0;
            if (e.NewValues["Pop3UseSsl"] == null)
                e.NewValues["Pop3UseSsl"] = 0;
        }
    }

    protected void Grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["Email"] == null)
        {
            e.RowError = "Lütfen E-Posta alanýný boþ býrakmayýnýz...";
            return;
        }

        if (e.NewValues["Pop3Server"] == null)
        {
            e.RowError = "Lütfen Pop3 Server alanýný boþ býrakmayýnýz...";
            return;
        }

        if (e.NewValues["MailPassword"] == null)
        {
            e.RowError = "Lütfen Mail Password alanýný boþ býrakmayýnýz...";
            return;
        }

        if (e.NewValues["MailUserName"] == null)
        {
            e.RowError = "Lütfen Mail UserName alanýný boþ býrakmayýnýz...";
            return;
        }

        if (!CrmUtils.IsEmail(Convert.ToString(e.NewValues["Email"])))
        {
            e.RowError = "Lütfen geçerli bir E-Posta giriniz...";
            return;
        }
    }

    protected void Grid2_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["Active"] == null)
                e.NewValues["Active"] = 0;
        }
    }

    protected void Grid2_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["Active"] == null)
                e.NewValues["Active"] = 0;
        }
    }

    protected void Grid2_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["Email"] == null)
        {
            e.RowError = "Lütfen Email alanýný boþ býrakmayýnýz...";
            return;
        }

        if (!CrmUtils.IsEmail(Convert.ToString(e.NewValues["Email"])))
        {
            e.RowError = "Lütfen geçerli bir email giriniz...";
            return;
        }

        if (Grid2.IsNewRowEditing)
        {
            DataRow[] Rows = DTList2.Table.Select("Email='" + e.NewValues["Email"].ToString() + "'");
            if (Rows.Length > 0)
                e.RowError = "bu  email tanýmlý görünüyor...";
        }
    }

    protected void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        //ASPxGridView grid = (ASPxGridView)sender;
        DataTable changes = this.DTList.Table.GetChanges();
        if (changes != null) e.Properties["cpIsDirty"] = true;
        else e.Properties["cpIsDirty"] = false;
    }
}
