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

public partial class CRM_Settings_PeriyodikIsler_list : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "Bildirim Ekle", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        // Eventleri tanimlayalim
        this.menu.ItemClick += new MenuItemEventHandler(menu_ItemClick);

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        this.UserName.Value = Membership.GetUser().UserName;
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }

        bool bInsert = Security.CheckPermission(this.Context, "Bildirim Ekle", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Bildirim Ekle", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Bildirim Ekle", "Delete");

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
        dt.Columns.Add("IndexID", typeof(int));
        dt.Columns.Add("PeriyodikIslerID", typeof(Guid));
        dt.Columns.Add("Step", typeof(int));
        dt.Columns.Add("Saat", typeof(int));
        dt.Columns.Add("Active", typeof(int));
        dt.Columns.Add("BaslangicTarihi", typeof(DateTime));
        dt.Columns.Add("SonIslemTarihi", typeof(DateTime));
        dt.Columns.Add("SonrakiIslemTarihi", typeof(DateTime));
        dt.Columns.Add("Description", typeof(string));
        dt.Columns.Add("FirmaID", typeof(Guid));
        dt.Columns.Add("ProjeID", typeof(Guid));
        dt.Columns.Add("UserID", typeof(Guid));
        dt.Columns.Add("Baslik", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        dt.Columns.Add("CreationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
        dt.Columns["CreationDate"].DefaultValue = DateTime.Now;
    }

    private void LoadDocument()
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM PeriyodikIsler Where CreatedBy=@CreatedBy ORDER BY CreationDate Desc");
        DB.AddParam(cmd, "@CreatedBy", 100, Membership.GetUser().UserName);
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();

            row["ID"] = rdr["PeriyodikIslerID"];
            row["PeriyodikIslerID"] = rdr["PeriyodikIslerID"];
            row["Step"] = rdr["Step"];
            row["Saat"] = rdr["Saat"];
            row["BaslangicTarihi"] = rdr["BaslangicTarihi"];
            row["SonrakiIslemTarihi"] = rdr["SonrakiIslemTarihi"];
            row["SonIslemTarihi"] = rdr["SonIslemTarihi"];
            row["Description"] = rdr["Description"];
            row["IndexID"] = rdr["IndexID"];
            row["Active"] = rdr["Active"];
            row["FirmaID"] = rdr["FirmaID"];
            row["ProjeID"] = rdr["ProjeID"];
            row["UserID"] = rdr["UserID"];
            row["Baslik"] = rdr["Baslik"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
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
                        sb.Append("INSERT INTO PeriyodikIsler(PeriyodikIslerID,Step,Saat,BaslangicTarihi,Description,");
                        sb.Append("FirmaID,ProjeID,UserID,Baslik,CreatedBy,CreationDate) ");
                        sb.Append("VALUES(@PeriyodikIslerID,@Step,@Saat,@BaslangicTarihi,@Description,");
                        sb.Append("@FirmaID,@ProjeID,@UserID,@Baslik,@CreatedBy,@CreationDate)");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@PeriyodikIslerID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@Step", (int)row["Step"]);
                        DB.AddParam(cmd, "@Saat", (int)row["Saat"]);
                        DB.AddParam(cmd, "@BaslangicTarihi", (DateTime)row["BaslangicTarihi"]);
                        DB.AddParam(cmd, "@Description", 255, row["Description"].ToString().ToUpper());
                        DB.AddParam(cmd, "@FirmaID", (Guid)row["FirmaID"]);
                        DB.AddParam(cmd, "@ProjeID", (Guid)row["ProjeID"]);
                        DB.AddParam(cmd, "@UserID", (Guid)row["UserID"]);
                        DB.AddParam(cmd, "@Baslik", 4000, row["Baslik"].ToString().ToUpper());
                        DB.AddParam(cmd, "@CreatedBy", 255, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Modified:
                        sb = new StringBuilder();
                        sb.Append("EXEC UpdatePeriyodikIsler ");
                        sb.Append("@Step,@Saat,@PeriyodikIslerID,@BaslangicTarihi,@Description,@FirmaID,@Active,@ProjeID,");
                        sb.Append("@UserID,@Baslik,@ModifiedBy,@ModificationDate ");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@PeriyodikIslerID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@Step", (int)row["Step"]);
                        DB.AddParam(cmd, "@Saat", (int)row["Saat"]);
                        DB.AddParam(cmd, "@Active", (int)row["Active"]);
                        DB.AddParam(cmd, "@BaslangicTarihi", (DateTime)row["BaslangicTarihi"]);
                        DB.AddParam(cmd, "@Description", 255, row["Description"].ToString().ToUpper());
                        DB.AddParam(cmd, "@FirmaID", (Guid)row["FirmaID"]);
                        DB.AddParam(cmd, "@ProjeID", (Guid)row["ProjeID"]);
                        DB.AddParam(cmd, "@UserID", (Guid)row["UserID"]);
                        DB.AddParam(cmd, "@Baslik", 4000, row["Baslik"].ToString().ToUpper());
                        DB.AddParam(cmd, "@ModifiedBy", 255, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Deleted:
                        try
                        {
                            cmd = DB.SQL(this.Context, "DELETE FROM PeriyodikIsler WHERE PeriyodikIslerID=@PeriyodikIslerID");
                            DB.AddParam(cmd, "@PeriyodikIslerID", (Guid)row["ID", DataRowVersion.Original]);
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
        //DataRow[] Rows = DataTableList.Table.Select("Adi='" + e.NewValues["Adi"].ToString() + "'");
        //if (Rows.Length > 0)
        //    e.Cancel = true;
    }

    protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (!grid.IsEditing) return;
        switch (e.Column.FieldName)
        {
            case "Step":
                if (grid.IsNewRowEditing)
                {
                    ASPxSpinEdit sp = e.Editor as ASPxSpinEdit;
                    sp.Number = 1;
                }
                break;
            case "Saat":
                if (grid.IsNewRowEditing)
                {
                    ASPxSpinEdit sp = e.Editor as ASPxSpinEdit;
                    sp.Number = 8;
                }
                break;
            case "BaslangicTarihi":
                if (grid.IsNewRowEditing)
                {
                    ASPxDateEdit dt = e.Editor as ASPxDateEdit;
                    dt.Date = DateTime.Now;
                }
                break;
            case "ProjeID":
                ASPxComboBox cmbProje = e.Editor as ASPxComboBox;
                cmbProje.Callback += new DevExpress.Web.ASPxClasses.CallbackEventHandlerBase(cmbProje_Callback);
                break;
            case "UserID":
                ASPxComboBox cmbUser = e.Editor as ASPxComboBox;
                cmbUser.Callback += new DevExpress.Web.ASPxClasses.CallbackEventHandlerBase(cmbUser_Callback);
                break;
            case "Active":
                if (grid.IsNewRowEditing)
                {
                    ASPxCheckBox Active = e.Editor as ASPxCheckBox;
                    Active.Checked = true;
                }
                break;

        }
    }

    protected void cmbUser_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (e.Parameter.ToString() == null || e.Parameter.ToString() == "")
            return;
        ASPxComboBox combo = source as ASPxComboBox;
        data.BindComboBoxesNoEmpty(this.Context, combo, "EXEC ISAssignUserByUserName '" + Membership.GetUser().UserName + "','" + e.Parameter + "'", "UserID", "UserName");

    }

    protected void cmbProje_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (e.Parameter.ToString() == null || e.Parameter.ToString() == "")
            return;
        ASPxComboBox combo = source as ASPxComboBox;
        StringBuilder sb = new StringBuilder();
        sb.Append("EXEC AllowedProjeListV2 '" + Membership.GetUser().UserName.ToLower() + "','" + e.Parameter.ToString() + "'");
        data.BindComboBoxesNoEmpty(this.Context, combo, sb.ToString(), "ProjeID", "Adi");
    }

    protected void grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {

    }

    protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["Step"] == null)
        {
            e.RowError = "Lütfen Zaman Aralýðý alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["BaslangicTarihi"] == null)
        {
            e.RowError = "Lütfen Baþlangýç Tarihi alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["FirmaID"] == null)
        {
            e.RowError = "Lütfen Ýlgili Birim alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["ProjeID"] == null)
        {
            e.RowError = "Lütfen Departman alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["UserID"] == null)
        {
            e.RowError = "Lütfen Ýlgili Kiþi alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["Baslik"] == null)
        {
            e.RowError = "Lütfen Gündem Tanýsý alanýný boþ býrakmayýnýz...";
            return;
        }
    }

    protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "Baslik")
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }
    }
}
