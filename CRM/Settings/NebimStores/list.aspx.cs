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

public partial class MarjinalCRM_Settings_NebimStores_list : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "E-Ticaret - Depolar", "Select"))
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


        bool bUpdate = Security.CheckPermission(this.Context, "E-Ticaret - Depolar", "Update");

        if (bUpdate)
            this.menu.Items.FindByName("save").Visible = true;
        else
            this.menu.Items.FindByName("save").Visible = false;

        InitGridTable(this.DataTableList.Table);

        for (int i = 0; i < this.grid.Columns.Count; i++)
        {
            if (this.grid.Columns[i] is GridViewCommandColumn)
            {
                (this.grid.Columns[i] as GridViewCommandColumn).EditButton.Visible = bUpdate;
                break;
            }
        }

        using (SqlCommand cmd = DB.SQL(this.Context, "Exec SP_EqualToNebimStores"))
        {
            cmd.ExecuteNonQuery();
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
        dt.Columns.Add("NebimStoreID", typeof(Guid));
        dt.Columns.Add("Adi", typeof(string));
        dt.Columns.Add("ErpCode", typeof(string));
        dt.Columns.Add("StoreOpen", typeof(int));
        dt.Columns.Add("MinStok", typeof(int));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModificationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument()
    {

        using (SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM Et_NebimStores ORDER BY Adi"))
        {
            cmd.Prepare();

            IDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DataRow row = this.DataTableList.Table.NewRow();

                row["ID"] = rdr["NebimStoreID"];
                row["NebimStoreID"] = rdr["NebimStoreID"];
                row["Adi"] = rdr["Adi"];
                row["ErpCode"] = rdr["ErpCode"];
                row["StoreOpen"] = rdr["StoreOpen"];
                row["MinStok"] = rdr["MinStok"];
                row["CreationDate"] = rdr["CreationDate"];
                row["CreatedBy"] = rdr["CreatedBy"];
                row["ModifiedBy"] = rdr["ModifiedBy"];
                row["ModificationDate"] = rdr["ModificationDate"];

                this.DataTableList.Table.Rows.Add(row);
            }
            rdr.Close();

            this.DataTableList.Table.AcceptChanges();
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
                    case DataRowState.Modified:
                        sb = new StringBuilder();
                        sb.Append("UPDATE Et_NebimStores SET ");
                        sb.Append("ErpCode=@ErpCode,");
                        sb.Append("StoreOpen=@StoreOpen,");
                        sb.Append("MinStok=@MinStok,");
                        sb.Append("ModifiedBy=@ModifiedBy,");
                        sb.Append("ModificationDate=@ModificationDate ");
                        sb.Append("WHERE NebimStoreID=@NebimStoreID");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@NebimStoreID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@StoreOpen", (int)row["StoreOpen"]);
                        DB.AddParam(cmd, "@MinStok", (int)row["MinStok"]);
                        DB.AddParam(cmd, "@ErpCode", 50, row["ErpCode"].ToString());
                        DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
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


    protected void grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["StoreOpen"] == null)
                e.NewValues["StoreOpen"] = 0;
            if (e.NewValues["MinStok"] == null)
                e.NewValues["MinStok"] = 1;
        }
    }

    protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        
    }
}
