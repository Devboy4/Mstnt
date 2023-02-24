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
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.Data;

public partial class CRM_Genel_Firma_list : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "Firmalar", "Select"))
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
        bool bInsert = Security.CheckPermission(this.Context, "Firmalar", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Firmalar", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Firmalar", "Delete");

        this.menu.Items.FindByName("save").Visible = (bInsert || bUpdate);
        for (int i = 0; i < this.grid.Columns.Count; i++)
        {
            if (this.grid.Columns[i] is GridViewCommandColumn)
            {
                (this.grid.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
                (this.grid.Columns[i] as GridViewCommandColumn).UpdateButton.Visible = bUpdate;
                (this.grid.Columns[i] as GridViewCommandColumn).NewButton.Visible = bInsert;
                break;
            }
        }
        InitGridTable(this.DataTableList.Table);
        LoadDocument();
    }

    protected void cmbSehirID_OnCallback(object source, CallbackEventArgsBase e)
    {
        if (String.IsNullOrEmpty(e.Parameter))
            return;
        ASPxComboBox cmb = source as ASPxComboBox;
        data.BindComboBoxes(this.Context, cmb, "SELECT SehirID,Adi FROM Sehir "
        + " WHERE UlkeID='" + e.Parameter.ToString() + "' ORDER BY Adi", "SehirID", "Adi");
    }

    protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (String.IsNullOrEmpty(e.Parameters))
            return;
       
            ASPxComboBox combo = (ASPxComboBox)grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["Sehir"], "GrSehirID");
            data.BindComboBoxes(this.Context, combo, "SELECT SehirID,Adi AS Sehir FROM Sehir "
                + " WHERE CONVERT(char(50),UlkeID)='" + e.Parameters.ToString() + "' ORDER BY Adi", "SehirID", "Sehir");

    }

    public void menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("new"))
        {
            //
        }
        else if (e.Item.Name.Equals("excel"))
        {

            CrmUtils.ExportToxls(gridExport, "grid", true);
        }
        else if (e.Item.Name.Equals("pdf"))
        {
            CrmUtils.ExportTopdf(gridExport, "grid", true);
        }
        else if (e.Item.Name.Equals("save"))
        {
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
        dt.Columns.Add("FirmaID", typeof(Guid));
        dt.Columns.Add("UlkeID", typeof(Guid));
        dt.Columns.Add("SehirID", typeof(Guid));
        dt.Columns.Add("FirmaName", typeof(string));
        dt.Columns.Add("MerkezMagaza", typeof(int));
        dt.Columns.Add("Sehir", typeof(string));
        dt.Columns.Add("Ulke", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("Active", typeof(int));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModificationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
        dt.Columns["CreationDate"].DefaultValue = DateTime.Now;
    }

    private void LoadDocument()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT  dbo.Firma.*, dbo.Ulke.Adi AS Ulke, dbo.Sehir.Adi AS Sehir");
        sb.Append(" FROM   dbo.Firma LEFT OUTER JOIN");
        sb.Append(" dbo.Sehir ON dbo.Firma.SehirID = dbo.Sehir.SehirID LEFT OUTER JOIN");
        sb.Append(" dbo.Ulke ON dbo.Firma.UlkeID = dbo.Ulke.UlkeID ORDER BY FirmaName");
        SqlConnection conn = DB.Connect(this.Context);
        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();

            row["ID"] = rdr["FirmaID"];
            row["FirmaID"] = rdr["FirmaID"];
            row["FirmaName"] = rdr["FirmaName"];
            row["MerkezMagaza"] = rdr["MerkezMagaza"];
            row["UlkeID"] = rdr["UlkeID"];
            row["SehirID"] = rdr["SehirID"];
            row["Ulke"] = rdr["Ulke"];
            row["Sehir"] = rdr["Sehir"];
            row["Active"] = rdr["Active"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModificationDate"] = rdr["ModificationDate"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["ModifiedBy"] = rdr["ModifiedBy"];

            this.DataTableList.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DataTableList.Table.AcceptChanges();
    }

    private bool SaveDocument()
    {
        StringBuilder sb = new StringBuilder();
        DB.BeginTrans(this.Context);

        SqlCommand cmd = null;

        DataTable changes = this.DataTableList.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        sb = new StringBuilder();
                        sb.Append("INSERT INTO Firma (");
                        sb.Append("FirmaName,MerkezMagaza,FirmaID,SehirID,UlkeID,Active,CreationDate,CreatedBy)");
                        sb.Append(" VALUES (");
                        sb.Append("@FirmaName,@MerkezMagaza,@FirmaID,@SehirID,@UlkeID,@Active,@CreationDate,@CreatedBy)");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@FirmaID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@FirmaName", 70, row["FirmaName"].ToString().ToUpper());
                        DB.AddParam(cmd, "@SehirID", (Guid)row["SehirID"]);
                        DB.AddParam(cmd, "@UlkeID", (Guid)row["UlkeID"]);
                        DB.AddParam(cmd, "@MerkezMagaza", (int)row["MerkezMagaza"]);
                        DB.AddParam(cmd, "@Active", int.Parse(row["Active"].ToString()));
                        DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName.ToString().ToUpper());
                        DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Deleted:
                        try
                        {
                            cmd = DB.SQL(this.Context, "UPDATE Firma SET Active='0' WHERE FirmaID=@FirmaID");
                            DB.AddParam(cmd, "@FirmaID", (Guid)row["ID", DataRowVersion.Original]);
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
                    case DataRowState.Modified:
                        sb = new StringBuilder();
                        sb.Append("UPDATE Firma SET ");
                        sb.Append("FirmaName=@FirmaName,");
                        sb.Append("SehirID=@SehirID,");
                        sb.Append("UlkeID=@UlkeID,");
                        sb.Append("MerkezMagaza=@MerkezMagaza,");
                        sb.Append("Active=@Active,");
                        sb.Append("ModifiedBy=@ModifiedBy,");
                        sb.Append("ModificationDate=@ModificationDate ");
                        sb.Append("WHERE FirmaID=@FirmaID");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@FirmaID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@FirmaName", 50, row["FirmaName"].ToString().ToUpper());
                        DB.AddParam(cmd, "@SehirID", (Guid)row["SehirID"]);
                        DB.AddParam(cmd, "@UlkeID", (Guid)row["UlkeID"]);
                        DB.AddParam(cmd, "@MerkezMagaza", (int)row["MerkezMagaza"]);
                        DB.AddParam(cmd, "@Active", int.Parse(row["Active"].ToString()));
                        DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName.ToString().ToUpper());
                        DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                }

            }
        }

        DB.Commit(this.Context);

        return true;
    }

    protected void grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["SehirID"] == null)
                e.NewValues["SehirID"] = DBNull.Value;
            if (e.NewValues["UlkeID"] == null)
                e.NewValues["UlkeID"] = DBNull.Value;
        }
    }

    protected void grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["SehirID"] == null)
                e.NewValues["SehirID"] = DBNull.Value;
            if (e.NewValues["UlkeID"] == null)
                e.NewValues["UlkeID"] = DBNull.Value;
        }
    }

    protected void Grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (this.grid.IsEditing && e.Column.FieldName == "SehirID")
        {
            ASPxComboBox combo = e.Editor as ASPxComboBox;
            combo.Callback += new CallbackEventHandlerBase(cmbSehirID_OnCallback);
        }
        if (this.grid.IsNewRowEditing && e.Column.FieldName == "MerkezMagaza")
        {
            ASPxCheckBox MerkezMagaza = e.Editor as ASPxCheckBox;
            MerkezMagaza.Value = 0;
            MerkezMagaza.Checked = false;
        }
        if (this.grid.IsNewRowEditing && e.Column.FieldName == "Active")
        {
            ASPxCheckBox Active = e.Editor as ASPxCheckBox;
            Active.Value = 0;
            Active.Checked = false;
        }
    }

    protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["FirmaName"] == null)
        {
            e.RowError = "Lütfen Müþteri alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["UlkeID"] == null)
        {
            e.RowError = "Lütfen Ülke alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["SehirID"] == null)
        {
            e.RowError = "Lütfen Þehir alanýný boþ býrakmayýnýz...";
            return;
        }
        if (grid.IsNewRowEditing)
        {
            DataRow[] Rows = DataTableList.Table.Select("FirmaName='" + e.NewValues["FirmaName"].ToString() + "'");
            if (Rows.Length > 0)
                e.RowError = "Ayný Firma daha önce tanýmlý görünüyor!";
        }
    }
}
