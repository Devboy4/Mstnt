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
using DevExpress.Web.ASPxCallback;

public partial class CRM_Settings_BrDurum_list : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "Tanim - BR Durumlari", "Select"))
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

        bool bInsert = Security.CheckPermission(this.Context, "Tanim - BR Durumlari", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Tanim - BR Durumlari", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Tanim - BR Durumlari", "Delete");

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
        dt.Columns.Add("BrDurumID", typeof(Guid));
        dt.Columns.Add("Adi", typeof(string));
        dt.Columns.Add("YaziRengi", typeof(string));
        dt.Columns.Add("Description", typeof(string));
        dt.Columns.Add("Sira", typeof(int));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        dt.Columns.Add("CreationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
        dt.Columns["CreationDate"].DefaultValue = DateTime.Now;
    }

    protected void CallbackSearchBrowser_Callback(object source, CallbackEventArgs e)
    {
        e.Result = "";
        Session["SearchBrowser"] = null;
        DataTable dtQuery = DB.SqlQuery();
        DataTable dtParameters = DB.SqlQueryParameters();
        DataTable dtFields = DB.SqlQueryFields();
        DataTable dtProcedure = DB.SqlQuery();
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            string[] parameters = e.Parameter.Split('|');
            StringBuilder sb = new StringBuilder();
            Hashtable htSearchBrowser = new Hashtable();
            switch (parameters[0].Trim())
            {
                #region M��teri
                case "YaziRengi":

                    break;
                #endregion
                default:
                    break;
            }
            Session["SearchBrowser"] = htSearchBrowser;
            e.Result = parameters[0].Trim();
        }
    }

    private void LoadDocument()
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM BrDurum ORDER BY Adi");
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();

            row["ID"] = rdr["BrDurumID"];
            row["Adi"] = rdr["Adi"];
            row["YaziRengi"] = rdr["YaziRengi"];
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
                        sb.Append("Insert Into  BrDurum(BrDurumID,Adi,YaziRengi,Description,CreatedBy,CreationDate)");
                        sb.Append("Values(@BrDurumID,@Adi,@YaziRengi,@Description,@CreatedBy,@CreationDate)");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@BrDurumID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@Adi", 100, row["Adi"].ToString().ToUpper());
                        DB.AddParam(cmd, "@YaziRengi", 50, row["YaziRengi"].ToString().ToUpper());
                        DB.AddParam(cmd, "@Description", 255, row["Description"].ToString().ToUpper());
                        DB.AddParam(cmd, "@CreatedBy", 255, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Modified:
                        sb = new StringBuilder();
                        sb.Append("Update BrDurum Set Adi=@Adi,Description=@Description,YaziRengi=@YaziRengi,");
                        sb.Append("ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate ");
                        sb.Append("Where BrDurumID=@BrDurumID");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@BrDurumID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@Adi", 100, row["Adi"].ToString().ToUpper());
                        DB.AddParam(cmd, "@YaziRengi", 50, row["YaziRengi"].ToString().ToUpper());
                        DB.AddParam(cmd, "@Description", 255, row["Description"].ToString().ToUpper());
                        DB.AddParam(cmd, "@ModifiedBy", 255, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Deleted:
                        try
                        {
                            cmd = DB.SQL(this.Context, "DELETE FROM BrDurum WHERE BrDurumID=@BrDurumID");
                            DB.AddParam(cmd, "@BrDurumID", (Guid)row["ID", DataRowVersion.Original]);
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
        if (e.NewValues["Adi"] == null)
        {
            e.RowError = "L�tfen Ad� alan�n� bo� b�rakmay�n�z...";
            return;
        }
        if (e.NewValues["YaziRengi"] == null)
        {
            e.RowError = "L�tfen Renk Kodu alan�n� bo� b�rakmay�n�z...";
            return;
        }
        if (grid.IsNewRowEditing)
        {
            DataRow[] Rows = DataTableList.Table.Select("Adi='" + e.NewValues["Adi"].ToString() + "'");
            if (Rows.Length > 0)
                e.RowError = "Bu Durum daha �nceden tan�ml� g�r�n�yor...";
        }
    }

    protected void CallbackGenel_Callback(object source, CallbackEventArgs e)
    {
        e.Result = "";
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            StringBuilder sb = new StringBuilder();
            string[] parameters = e.Parameter.Split('|');
            switch (parameters[0].Trim())
            {
                #region Yaz� Rengi
                case "YaziRengi":

                    sb = new StringBuilder("|" + parameters[0].Trim());
                    sb.Append("|" + parameters[1].Trim());
                    e.Result = sb.ToString();
                    break;
                #endregion
                default:
                    break;
            }
        }
    }
}
