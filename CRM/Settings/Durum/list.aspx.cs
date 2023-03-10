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

public partial class CRM_Settings_Durum_list : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "Tanim - Bildirim Durumlari", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfay? g?rme yetkisine sahip de?ilsiniz!</p></body></html>");
            this.Response.End();
        }

        // Eventleri tanimlayalim
        this.menu.ItemClick += new MenuItemEventHandler(menu_ItemClick);

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }

        bool bInsert = Security.CheckPermission(this.Context, "Tanim - Bildirim Durumlari", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Tanim - Bildirim Durumlari", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Tanim - Bildirim Durumlari", "Delete");

        if (bInsert || bUpdate)
            this.menu.Items.FindByName("save").Visible = true;
        else
            this.menu.Items.FindByName("save").Visible = false;

        InitGridTable(this.DataTableList.Table);

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
        dt.Columns.Add("DurumID", typeof(Guid));
        dt.Columns.Add("Adi", typeof(string));
        dt.Columns.Add("Description", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("HarcananSure", typeof(int));
        dt.Columns.Add("IlkGetir", typeof(int));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        dt.Columns.Add("CreationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
        dt.Columns["CreationDate"].DefaultValue = DateTime.Now;
    }

    private void LoadDocument()
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM Durum ORDER BY Adi");
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();

            row["ID"] = rdr["DurumID"];
            row["Adi"] = rdr["Adi"];
            row["Description"] = rdr["Description"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["HarcananSure"] = rdr["HarcananSure"];
            row["IlkGetir"] = rdr["IlkGetir"];
            row["ModificationDate"] = rdr["ModificationDate"];
            row["CreationDate"] = rdr["CreationDate"];

            this.DataTableList.Table.Rows.Add(row);
        }
        rdr.Close();

        this.DataTableList.Table.AcceptChanges();
    }

    private bool SaveDocument()
    {
        DB.BeginTrans(this.Context);
        try
        {
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
                            if ((int)row["IlkGetir"] == 1)
                            {
                                cmd = DB.SQL(this.Context, "Update Durum Set IlkGetir=0");
                                cmd.Prepare();
                                cmd.ExecuteNonQuery();
                            }
                            sb = new StringBuilder();
                            sb.Append("INSERT INTO Durum(DurumID,Adi,Description,HarcananSure,IlkGetir,CreatedBy,CreationDate)");
                            sb.Append("VALUES(@DurumID,@Adi,@Description,@HarcananSure,@IlkGetir,@CreatedBy,@CreationDate)");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@DurumID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@Adi", 100, row["Adi"].ToString().ToUpper());
                            DB.AddParam(cmd, "@Description", 255, row["Description"].ToString().ToUpper());
                            DB.AddParam(cmd, "@CreatedBy", 255, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            DB.AddParam(cmd, "@HarcananSure", (int)row["HarcananSure"]);
                            DB.AddParam(cmd, "@IlkGetir", (int)row["IlkGetir"]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Modified:
                            if ((int)row["IlkGetir"] == 1)
                            {
                                cmd = DB.SQL(this.Context, "Update Durum Set IlkGetir=0");
                                cmd.Prepare();
                                cmd.ExecuteNonQuery();
                            }
                            sb = new StringBuilder();
                            sb.Append("UPDATE Durum SET ");
                            sb.Append("Adi=@Adi,");
                            sb.Append("HarcananSure=@HarcananSure,");
                            sb.Append("IlkGetir=@IlkGetir,");
                            sb.Append("Description=@Description,");
                            sb.Append("ModifiedBy=@ModifiedBy,");
                            sb.Append("ModificationDate=@ModificationDate ");
                            sb.Append("WHERE DurumID=@DurumID");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@DurumID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@Adi", 100, row["Adi"].ToString().ToUpper());
                            DB.AddParam(cmd, "@Description", 255, row["Description"].ToString().ToUpper());
                            DB.AddParam(cmd, "@ModifiedBy", 255, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            DB.AddParam(cmd, "@HarcananSure", (int)row["HarcananSure"]);
                            DB.AddParam(cmd, "@IlkGetir", (int)row["IlkGetir"]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Deleted:
                            try
                            {
                                cmd = DB.SQL(this.Context, "DELETE FROM Durum WHERE DurumID=@DurumID");
                                DB.AddParam(cmd, "@DurumID", (Guid)row["ID", DataRowVersion.Original]);
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
        catch (Exception ex)
        {
            DB.Rollback(this.Context);
            return false;
        }
    }

    protected void grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["HarcananSure"] == null)
                e.NewValues["HarcananSure"] = 0;
            if (e.NewValues["IlkGetir"] == null)
                e.NewValues["IlkGetir"] = 0;
        }
        DataRow[] Rows = DataTableList.Table.Select("Adi='" + e.NewValues["Adi"].ToString() + "'");
        if (Rows.Length > 0)
            e.Cancel = true;
    }

    protected void grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["HarcananSure"] == null)
                e.NewValues["HarcananSure"] = 0;
            if (e.NewValues["IlkGetir"] == null)
                e.NewValues["IlkGetir"] = 0;
        }
    }

    protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["Adi"] == null)
        {
            e.RowError = "L?tfen Ad? alan?n? bo? b?rakmay?n?z...";
            return;
        }
    }
}
