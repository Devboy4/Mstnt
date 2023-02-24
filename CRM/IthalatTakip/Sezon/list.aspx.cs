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

public partial class CRM_IthalatTakip_Tanim_Sezon_list : System.Web.UI.Page
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

        if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Sezon", "Select"))
        {
            //this.Response.Write("<html><body bgcolor='#e6e6fa'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            //this.Response.End();
            CrmUtils.MessageAlert(this.Page, "Bu sayfayý görme yetkisine sahip deðilsiniz!", "Alert");
            return;
        }

        bool bInsert = Security.CheckPermission(this.Context, "Ýthalat Takip - Sezon", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Ýthalat Takip - Sezon", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Ýthalat Takip - Sezon", "Delete");

        if (bInsert || bUpdate || bDelete)
            this.menu.Items.FindByName("save").Visible = true;
        else
            this.menu.Items.FindByName("save").Visible = false;

        InitGrid(this.DTList.Table);

        for (int i = 0; i < this.Grid.Columns.Count; i++)
        {
            if (this.Grid.Columns[i] is GridViewCommandColumn)
            {
                (this.Grid.Columns[i] as GridViewCommandColumn).NewButton.Visible = bInsert;
                (this.Grid.Columns[i] as GridViewCommandColumn).EditButton.Visible = bUpdate;
                (this.Grid.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
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
        dt.Columns.Add("Sezon", typeof(string));
        dt.Columns.Add("Aciklama", typeof(string));
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
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM IthalatSezon ORDER BY Sezon");
        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTList.Table.NewRow();
            row["ID"] = rdr["Id"];
            row["Sezon"] = rdr["Sezon"];
            row["Aciklama"] = rdr["Aciklama"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];
            this.DTList.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTList.Table.AcceptChanges();
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
                    case DataRowState.Added:
                        #region insert
                        sb = new StringBuilder();
                        sb.Append("INSERT INTO IthalatSezon(Id,Sezon,Aciklama,CreatedBy,CreationDate)");
                        sb.Append("VALUES(@Id,@Sezon,@Aciklama,@CreatedBy,@CreationDate)");
                        try
                        {
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@Sezon", 100, row["Sezon"].ToString());
                            DB.AddParam(cmd, "@Aciklama", 255, row["Aciklama"].ToString());
                            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
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
                    case DataRowState.Modified:
                        #region update
                        sb = new StringBuilder("UPDATE IthalatSezon ");
                        sb.Append("SET Sezon=@Sezon,Aciklama=@Aciklama,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate ");
                        sb.Append("WHERE Id=@Id");
                        try
                        {
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@Sezon", 100, row["Sezon"].ToString());
                            DB.AddParam(cmd, "@Aciklama", 255, row["Aciklama"].ToString());
                            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
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
                        #endregion
                        break;
                    case DataRowState.Deleted:
                        #region delete
                        try
                        {
                            cmd = DB.SQL(this.Context, "DELETE FROM IthalatSezon WHERE Id=@Id");
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
                        #endregion
                        break;
                }
            }
        }

        DB.Commit(this.Context);
        return true;
    }

    protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        //if (e.NewValues.Values.Count > 0)
        //{
        //    if (e.NewValues["col_name"] == null) e.NewValues["col_name"] = DBNull.Value;
        //}
    }

    protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        //if (e.NewValues.Values.Count > 0)
        //{
        //    if (e.NewValues["col_name"] == null) e.NewValues["col_name"] = DBNull.Value;
        //}
    }

    protected void Grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["Sezon"] == null)
        {
            e.RowError = "Lütfen Sezon alanýný boþ býrakmayýnýz...";
            return;
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
