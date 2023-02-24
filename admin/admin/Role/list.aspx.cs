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

public partial class admin_Role_list : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Security.CheckPermission(this.Context, "Rol", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#e6e6fa'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        // Eventleri tanimlayalim
        this.menu.ItemClick += new MenuItemEventHandler(menu_ItemClick);

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }

        bool bInsert = Security.CheckPermission(this.Context, "Rol", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Rol", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Rol", "Delete");

        if (bInsert || bUpdate || bDelete)
            this.menu.Items.FindByName("save").Visible = true;
        else
            this.menu.Items.FindByName("save").Visible = false;

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

        InitGridTable(this.DataTableRoles.Table);
        LoadDocument();
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
        dt.Columns.Add("RoleID", typeof(Guid));
        dt.Columns.Add("Role", typeof(string));
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
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM SecurityRoles ORDER BY Role");
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableRoles.Table.NewRow();

            row["ID"] = rdr["RoleID"];
            row["RoleID"] = rdr["RoleID"];
            row["Role"] = rdr["Role"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];

            this.DataTableRoles.Table.Rows.Add(row);
        }
        rdr.Close();

        this.DataTableRoles.Table.AcceptChanges();
    }

    private bool SaveDocument()
    {
        DB.BeginTrans(this.Context);

        SqlCommand cmd = null;
        StringBuilder sb;

        if (Roles.Provider.ApplicationName != Membership.ApplicationName)
            Roles.Provider.ApplicationName = Membership.ApplicationName;

        DataTable changes = this.DataTableRoles.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        if ((row["Role"] != null) && (row["Role"].ToString() != ""))
                        {
                            try
                            {
                                Roles.CreateRole(row["Role"].ToString());
                            }
                            catch (Exception ex)
                            {
                                Response.Write("<script language='javascript'>alert('" + row["Role"].ToString() + " rolü oluþturulamadý! " + ex.ToString() + "');</script>");
                                DB.Rollback(this.Context);
                                return false;
                            }
                        }
                        else
                        {
                            DB.Rollback(this.Context);
                            return false;
                        }

                        cmd = DB.SQL(this.Context, "SELECT RoleId FROM aspnet_Roles WHERE RoleName=@Role");
                        DB.AddParam(cmd, "@Role", 255, row["Role"].ToString());
                        cmd.Prepare();
                        Guid gRoleID = (Guid)cmd.ExecuteScalar();

                        sb = new StringBuilder();
                        sb.Append("INSERT INTO SecurityRoles(RoleID,Role,CreatedBy,CreationDate)");
                        sb.Append("VALUES(@RoleID,@Role,@CreatedBy,@CreationDate)");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@RoleID", gRoleID);
                        DB.AddParam(cmd, "@Role", 255, row["Role"].ToString());
                        DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Modified:
                        sb = new StringBuilder();
                        if (row["Role"].ToString() == "Administrator")
                        {
                            CrmUtils.CreateMessageAlert(this.Page, "Administrator rolü deðiþtirilemez!", "stadminis");
                            DB.Rollback(this.Context);
                            return false;
                        }
                        sb.Append("UPDATE aspnet_Roles SET ");
                        sb.Append("RoleName=@Role,");
                        sb.Append("LoweredRoleName=@LowerRole ");
                        sb.Append("WHERE RoleID=@RoleID");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@RoleID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@Role", 255, row["Role"].ToString());
                        DB.AddParam(cmd, "@LowerRole", 255, row["Role"].ToString().ToLower());
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();

                        sb = new StringBuilder();
                        sb.Append("UPDATE SecurityRoles SET ");
                        sb.Append("Role=@Role,");
                        sb.Append("ModifiedBy=@ModifiedBy,");
                        sb.Append("ModificationDate=@ModificationDate ");
                        sb.Append("WHERE RoleID=@RoleID");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@RoleID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@Role", 255, row["Role"].ToString());
                        DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Deleted:
                        if (Roles.RoleExists(row["Role", DataRowVersion.Original].ToString()))
                        {
                            if (!Roles.DeleteRole(row["Role", DataRowVersion.Original].ToString()))
                            {
                                Response.Write("<script language='javascript'>alert('" + row["Role", DataRowVersion.Original].ToString() + " rolü silinemedi!');</script>");
                                DB.Rollback(this.Context);
                                return false;
                            }
                        }

                        cmd = DB.SQL(this.Context, "DELETE FROM SecurityRoles WHERE RoleID=@RoleID");
                        DB.AddParam(cmd, "@RoleID", (Guid)row["ID", DataRowVersion.Original]);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();

                        cmd = DB.SQL(this.Context, "DELETE FROM SecurityUserRoles WHERE RoleID=@RoleID");
                        DB.AddParam(cmd, "@RoleID", (Guid)row["ID", DataRowVersion.Original]);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();

                        cmd = DB.SQL(this.Context, "DELETE FROM SecurityRolePermissions WHERE RoleID=@RoleID");
                        DB.AddParam(cmd, "@RoleID", (Guid)row["ID", DataRowVersion.Original]);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                }
            }
        }

        DB.Commit(this.Context);

        return true;
    }

    protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["Role"] == null)
        {
            e.RowError = "Lütfen Rol alanýný boþ býrakmayýnýz...";
            return;
        }
        else
        {
            string role = e.NewValues["Role"].ToString().Trim();
            DataRow[] rows = this.DataTableRoles.Table.Select("Role='" + role + "'");

            if (rows.GetUpperBound(0) != -1)
            {
                e.RowError = "'" + role + "' rol adý daha önce tanýmlanmýþ...";
                return;
            }
        }
    }

    protected void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        ASPxGridView grid = (ASPxGridView)sender;

        DataTable changes = null;

        if (grid.ID.ToLower() == "grid")
            changes = this.DataTableRoles.Table.GetChanges();

        if (changes != null)
            e.Properties["cpIsDirty"] = true;
        else
            e.Properties["cpIsDirty"] = false;
    }
}
