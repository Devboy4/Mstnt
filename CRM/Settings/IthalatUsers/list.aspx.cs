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
using Model.Crm;
using DevExpress.Web.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;

public partial class CRM_Settings_IthalatUsers_list : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "Tanim - ithalat Yoneticileri", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        // Eventleri tanimlayalim
        this.menu.ItemClick += new MenuItemEventHandler(menu_ItemClick);

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }

        bool bInsert = Security.CheckPermission(this.Context, "Tanim - ithalat Yoneticileri", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Tanim - ithalat Yoneticileri", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Tanim - ithalat Yoneticileri", "Delete");

        if (bInsert || bUpdate)
            this.menu.Items.FindByName("save").Visible = true;
        else
            this.menu.Items.FindByName("save").Visible = false;

        InitGridTable(this.DataTableList.Table);
        InitGridTableDtUsers(this.DTUsers.Table);
        for (int i = 0; i < this.grid.Columns.Count; i++)
        {
            if (this.grid.Columns[i] is GridViewCommandColumn)
            {
                (this.grid.Columns[i] as GridViewCommandColumn).NewButton.Visible = bInsert;
                (this.grid.Columns[i] as GridViewCommandColumn).EditButton.Visible = bUpdate;
                (this.grid.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
                break;
            }
        }
        FillComboxes();
        LoadDocument();
    }

    private void FillComboxes()
    {

        StringBuilder sb = new StringBuilder();
        this.DTUsers.Table.Clear();
        sb = new StringBuilder();
        sb.Append("SELECT UserID,('['+IsNull(UserName,'')+'] '+IsNull(FirstName,'')+' '+IsNull(LastName,'')) As UserName ");
        sb.Append("FROM SecurityUsers ORDER BY UserName");
        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTUsers.Table.NewRow();
            row["UserID"] = rdr["UserID"];
            row["UserName"] = rdr["UserName"];
            this.DTUsers.Table.Rows.Add(row);
        }
        this.DTUsers.Table.AcceptChanges();
        rdr.Close();

    }

    public void menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("save"))
        {
            this.grid.UpdateEdit();
            if (SaveDocument())
            {
                this.Response.Redirect("./list.aspx");
            }
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IthalatUsersID", typeof(Guid));
        dt.Columns.Add("UserID", typeof(Guid));
        dt.Columns.Add("Description", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        dt.Columns.Add("CreationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
        dt.Columns["CreationDate"].DefaultValue = DateTime.Now;
    }

    private void InitGridTableDtUsers(DataTable dt)
    {
        dt.Columns.Add("UserID", typeof(Guid));
        dt.Columns.Add("UserName", typeof(string));

    }

    private void LoadDocument()
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM IthalatUsers ORDER BY IndexID Desc");
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();

            row["ID"] = rdr["IthalatUsersID"];
            row["UserID"] = rdr["UserID"];
            row["Description"] = rdr["Description"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];
            row["CreationDate"] = rdr["CreationDate"];

            this.DataTableList.Table.Rows.Add(row);
        }
        rdr.Close();

        this.DataTableList.Table.AcceptChanges();
    }

    protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (grid.IsEditing && e.Column.FieldName == "UserID")
        {
            ASPxComboBox cmb = e.Editor as ASPxComboBox;
            ListEditItem item;
            cmb.Items.Clear();
            cmb.DataSourceID = null;
            foreach (DataRow row in DTUsers.Table.Rows)
            {
                DataRow[] rows = DataTableList.Table.Select("UserID='" + row["UserID"].ToString() + "'");
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

    private bool SaveDocument()
    {
        DB.BeginTrans(this.Context);

        SqlCommand cmd = null;
        StringBuilder sb;

        DataTable changes = this.DataTableList.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        sb = new StringBuilder();
                        sb.Append("INSERT INTO IthalatUsers(IthalatUsersID,UserID,Description,CreatedBy,CreationDate)");
                        sb.Append("VALUES(@IthalatUsersID,@UserID,@Description,@CreatedBy,@CreationDate)");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@IthalatUsersID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@UserID", (Guid)row["UserID"]);
                        DB.AddParam(cmd, "@Description", 255, row["Description"].ToString().ToUpper());
                        DB.AddParam(cmd, "@CreatedBy", 100, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Modified:
                        sb = new StringBuilder();
                        sb.Append("UPDATE IthalatUsers SET ");
                        sb.Append("UserID=@UserID,");
                        sb.Append("Description=@Description,");
                        sb.Append("ModifiedBy=@ModifiedBy,");
                        sb.Append("ModificationDate=@ModificationDate ");
                        sb.Append("WHERE IthalatUsersID=@IthalatUsersID");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@IthalatUsersID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@UserID", (Guid)row["UserID"]);
                        DB.AddParam(cmd, "@Description", 255, row["Description"].ToString().ToUpper());
                        DB.AddParam(cmd, "@ModifiedBy", 255, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Deleted:
                        try
                        {
                            cmd = DB.SQL(this.Context, "DELETE FROM IthalatUsers WHERE IthalatUsersID=@IthalatUsersID");
                            DB.AddParam(cmd, "@IthalatUsersID", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            DataTableList.Table.Clear();
                            LoadDocument();
                            grid.DataBind();
                            CrmUtils.MessageAlert(this.Page, ex.Message.ToString().Replace("'", null).Replace("\r\n", null), "stkeySilinemez");
                            return false;
                        }
                        break;
                }
            }
        }

        DB.Commit(this.Context);

        return true;
    }

    protected void grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {

    }

    protected void grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {

    }

    protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["UserID"] == null)
        {
            e.RowError = "Lütfen Yönetici alanýný boþ býrakmayýnýz...";
            return;
        }
    }
}
