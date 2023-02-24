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

public partial class CRM_IthalatTakip_Tanim_Guvenlik_list : System.Web.UI.Page
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

        if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Güvenlik", "Select"))
        {
            //this.Response.Write("<html><body bgcolor='#e6e6fa'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            //this.Response.End();
            CrmUtils.MessageAlert(this.Page, "Bu sayfayý görme yetkisine sahip deðilsiniz!", "Alert");
            return;
        }

        bool bInsert = Security.CheckPermission(this.Context, "Ýthalat Takip - Güvenlik", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Ýthalat Takip - Güvenlik", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Ýthalat Takip - Güvenlik", "Delete");

        if (bInsert || bUpdate || bDelete)
            this.menu.Items.FindByName("save").Visible = true;
        else
            this.menu.Items.FindByName("save").Visible = false;

        InitGrid(this.DTList.Table);

        for (int i = 0; i < this.Grid.Columns.Count; i++)
        {
            if (this.Grid.Columns[i] is GridViewCommandColumn)
            {
                //(this.Grid.Columns[i] as GridViewCommandColumn).NewButton.Visible = bInsert;
                //(this.Grid.Columns[i] as GridViewCommandColumn).EditButton.Visible = bUpdate;
                (this.Grid.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
                break;
            }
        }

        LoadDocument();
    }

    public void menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("excel"))
        {
            ExportUtils.GridExport(this.Page, this.gridExport, ExportType.xls, true);
            return;
        }
        else if (e.Item.Name.Equals("pdf"))
        {
            ExportUtils.GridExport(this.Page, this.gridExport, ExportType.pdf, true);
            return;
        }

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
        dt.Columns.Add("Aciklama", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("Kaydeden", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("Guncelleyen", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument()
    {
        StringBuilder sb = new StringBuilder("SELECT t1.*,(t2.FirstName+' '+t2.LastName)Kaydeden,(t3.FirstName+' '+t3.LastName)Guncelleyen ");
        sb.Append("FROM SecurityEdit t1 ");
        sb.Append("LEFT JOIN SecurityUsers t2 ON(t1.CreatedBy=t2.UserName) ");
        sb.Append("LEFT JOIN SecurityUsers t3 ON(t1.ModifiedBy=t3.UserName) ");
        sb.Append("ORDER BY t1.Aciklama ");
        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTList.Table.NewRow();
            row["ID"] = rdr["Id"];
            row["Aciklama"] = rdr["Aciklama"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["Kaydeden"] = rdr["Kaydeden"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["Guncelleyen"] = rdr["Guncelleyen"];
            row["ModificationDate"] = rdr["ModificationDate"];
            this.DTList.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTList.Table.AcceptChanges();
    }

    private bool SaveDocument()
    {
        DataTable changes = this.DTList.Table.GetChanges();
        if (changes != null)
        {
            SqlCommand cmd = null;
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Deleted:
                        DB.BeginTrans(this.Context);
                        #region delete
                        try
                        {
                            cmd = DB.SQL(this.Context, "DELETE FROM SecurityEditUsers WHERE SecurityEditId=@Id");
                            DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.CommandTimeout = 1000;
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            cmd = DB.SQL(this.Context, "DELETE FROM SecurityEditRoles WHERE SecurityEditId=@Id");
                            DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.CommandTimeout = 1000;
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            cmd = DB.SQL(this.Context, "DELETE FROM SecurityEditColumns WHERE SecurityEditId=@Id");
                            DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.CommandTimeout = 1000;
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            cmd = DB.SQL(this.Context, "DELETE FROM SecurityEdit WHERE Id=@Id");
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
                        DB.Commit(this.Context);
                        break;
                }
            }
        }
        return true;
    }

    protected void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        //ASPxGridView grid = (ASPxGridView)sender;
        DataTable changes = this.DTList.Table.GetChanges();
        if (changes != null) e.Properties["cpIsDirty"] = true;
        else e.Properties["cpIsDirty"] = false;
    }

    protected void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        e.Cell.ToolTip = string.Format("{0}", e.CellValue);
    }
}
