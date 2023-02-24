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

public partial class CRM_Settings_VirusSinif_list : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "Tan�m - Vir�s S�n�flar�", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfay� g�rme yetkisine sahip de�ilsiniz!</p></body></html>");
            this.Response.End();
        }

        // Eventleri tanimlayalim
        this.menu.ItemClick += new MenuItemEventHandler(menu_ItemClick);

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }

        bool bInsert = Security.CheckPermission(this.Context, "Tan�m - Vir�s S�n�flar�", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Tan�m - Vir�s S�n�flar�", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Tan�m - Vir�s S�n�flar�", "Delete");

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
        dt.Columns.Add("VirusSinifID", typeof(Guid));
        dt.Columns.Add("Adi", typeof(string));
        dt.Columns.Add("OperasyonDay", typeof(int));
        dt.Columns.Add("IsDateChange", typeof(int));
        dt.Columns.Add("IsSendSms", typeof(int));
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

    private void LoadDocument()
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM VirusSinif ORDER BY Adi");
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();

            row["ID"] = rdr["VirusSinifID"];
            row["Adi"] = rdr["Adi"];
            row["IsDateChange"] = Convert.ToInt32(rdr["IsDateChange"]);
            row["IsSendSms"] = Convert.ToInt32(rdr["IsSendSms"]);
            row["OperasyonDay"] = rdr["OperasyonDay"];
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
                        sb.Append("INSERT INTO VirusSinif(VirusSinifID,Adi,Description,OperasyonDay,IsDateChange,IsSendSms,CreatedBy,CreationDate)");
                        sb.Append("VALUES(@VirusSinifID,@Adi,@Description,@OperasyonDay,@IsDateChange,@IsSendSms,@CreatedBy,@CreationDate)");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@VirusSinifID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@Adi", 100, row["Adi"].ToString().ToUpper());
                        DB.AddParam(cmd, "@Description", 255, row["Description"].ToString().ToUpper());
                        DB.AddParam(cmd, "@OperasyonDay", (int)row["OperasyonDay"]);
                        DB.AddParam(cmd, "@IsDateChange", (int)row["IsDateChange"]);
                        DB.AddParam(cmd, "@IsSendSms", (int)row["IsSendSms"]);
                        DB.AddParam(cmd, "@CreatedBy", 255, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Modified:
                        sb = new StringBuilder();
                        sb.Append("UPDATE VirusSinif SET ");
                        sb.Append("Adi=@Adi,");
                        sb.Append("Description=@Description,");
                        sb.Append("OperasyonDay=@OperasyonDay,");
                        sb.Append("IsDateChange=@IsDateChange,");
                        sb.Append("IsSendSms=@IsSendSms,");
                        sb.Append("ModifiedBy=@ModifiedBy,");
                        sb.Append("ModificationDate=@ModificationDate ");
                        sb.Append("WHERE VirusSinifID=@VirusSinifID");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@VirusSinifID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@Adi", 100, row["Adi"].ToString().ToUpper());
                        DB.AddParam(cmd, "@Description", 255, row["Description"].ToString().ToUpper());
                        DB.AddParam(cmd, "@OperasyonDay", (int)row["OperasyonDay"]);
                        DB.AddParam(cmd, "@IsDateChange", (int)row["IsDateChange"]);
                        DB.AddParam(cmd, "@IsSendSms", (int)row["IsSendSms"]);
                        DB.AddParam(cmd, "@ModifiedBy", 255, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Deleted:
                        try
                        {
                            cmd = DB.SQL(this.Context, "DELETE FROM VirusSinifUsers WHERE VirusSinifID=@Id");
                            DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            cmd = DB.SQL(this.Context, "DELETE FROM VirusSinif WHERE VirusSinifID=@Id");
                            DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
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
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["IsDateChange"] == null)
                e.NewValues["IsDateChange"] = 0;
            if (e.NewValues["IsSendSms"] == null)
                e.NewValues["IsSendSms"] = 0;
            if (e.NewValues["OperasyonDay"] == null)
                e.NewValues["OperasyonDay"] = 7;
        }

    }

    protected void grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["IsDateChange"] == null)
                e.NewValues["IsDateChange"] = 0;
            if (e.NewValues["IsSendSms"] == null)
                e.NewValues["IsSendSms"] = 0;
            if (e.NewValues["OperasyonDay"] == null)
                e.NewValues["OperasyonDay"] = 7;
        }
    }

    protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["Adi"] == null)
        {
            e.RowError = "L�tfen Ad� alan�n� bo� b�rakmay�n�z...";
            return;
        }
        if (grid.IsNewRowEditing)
        {
            DataRow[] Rows = DataTableList.Table.Select("Adi='" + e.NewValues["Adi"].ToString() + "'");
            if (Rows.Length > 0)
                e.RowError = "bu isimde G�ndem s�n�f� tan�ml� g�r�n�yor...";
        }
    }
}
