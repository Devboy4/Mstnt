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

public partial class MarjinalCRM_Settings_DosyaTuru_list : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "Tanim - Dosya Türleri", "Select"))
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

        bool bInsert = Security.CheckPermission(this.Context, "Tanim - Dosya Türleri", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Tanim - Dosya Türleri", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Tanim - Dosya Türleri", "Delete");

        if (bInsert || bUpdate || bDelete)
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
        dt.Columns.Add("DosyaTuruID", typeof(Guid));
        dt.Columns.Add("DosyaTuru", typeof(string));
        dt.Columns.Add("MaksimumBoyut", typeof(int));
        dt.Columns.Add("BoyutTuru", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("AllowedRoles", typeof(string));
        dt.Columns.Add("DeniedRoles", typeof(string));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument()
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM DosyaTuru ORDER BY DosyaTuru");
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();

            row["ID"] = rdr["DosyaTuruID"];
            row["DosyaTuruID"] = rdr["DosyaTuruID"];
            row["DosyaTuru"] = rdr["DosyaTuru"];
            row["MaksimumBoyut"] = rdr["MaksimumBoyut"];
            row["BoyutTuru"] = rdr["BoyutTuru"];
            row["CreationDate"] = rdr["CreationDate"];
            row["AllowedRoles"] = rdr["AllowedRoles"];
            row["DeniedRoles"] = rdr["DeniedRoles"];

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
                        sb.Append("INSERT INTO DosyaTuru(DosyaTuruID,DosyaTuru,MaksimumBoyut,BoyutTuru,AllowedRoles,DeniedRoles,CreatedBy,CreationDate)");
                        sb.Append("VALUES(@DosyaTuruID,@DosyaTuru,@MaksimumBoyut,@BoyutTuru,@AllowedRoles,@DeniedRoles,@CreatedBy,@CreationDate)");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        string extension1 = "." + row["DosyaTuru"].ToString().Replace(".", "");
                        DB.AddParam(cmd, "@DosyaTuruID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@DosyaTuru", 10, extension1);
                        DB.AddParam(cmd, "@MaksimumBoyut", (int)row["MaksimumBoyut"]);
                        DB.AddParam(cmd, "@BoyutTuru", 10, row["BoyutTuru"].ToString());
                        DB.AddParam(cmd, "@AllowedRoles", 255, "");
                        DB.AddParam(cmd, "@DeniedRoles", 255, "");
                        DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Modified:
                        sb = new StringBuilder();
                        sb.Append("UPDATE DosyaTuru SET ");
                        sb.Append("DosyaTuru=@DosyaTuru,");
                        sb.Append("MaksimumBoyut=@MaksimumBoyut,");
                        sb.Append("BoyutTuru=@BoyutTuru,");
                        sb.Append("AllowedRoles=@AllowedRoles,");
                        sb.Append("DeniedRoles=@DeniedRoles,");
                        sb.Append("ModifiedBy=@ModifiedBy,");
                        sb.Append("ModificationDate=@ModificationDate ");
                        sb.Append("WHERE DosyaTuruID=@DosyaTuruID");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        string extension2 = "." + row["DosyaTuru"].ToString().Replace(".", "");
                        DB.AddParam(cmd, "@DosyaTuruID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@DosyaTuru", 10, extension2);
                        DB.AddParam(cmd, "@MaksimumBoyut", (int)row["MaksimumBoyut"]);
                        DB.AddParam(cmd, "@BoyutTuru", 10, row["BoyutTuru"].ToString());
                        DB.AddParam(cmd, "@AllowedRoles", 255, "");
                        DB.AddParam(cmd, "@DeniedRoles", 255, "");
                        DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Deleted:
                        cmd = DB.SQL(this.Context, "DELETE FROM DosyaTuru WHERE DosyaTuruID=@DosyaTuruID");
                        DB.AddParam(cmd, "@DosyaTuruID", (Guid)row["ID", DataRowVersion.Original]);
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
            if (e.NewValues["MaksimumBoyut"] == null)
                e.NewValues["MaksimumBoyut"] = 0;
            if (e.NewValues["BoyutTuru"] == null)
                e.NewValues["BoyutTuru"] = "Bayt";
        }
    }

    protected void grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["MaksimumBoyut"] == null)
                e.NewValues["MaksimumBoyut"] = 0;
            if (e.NewValues["BoyutTuru"] == null)
                e.NewValues["BoyutTuru"] = "Bayt";
        }
    }

    protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["DosyaTuru"] == null)
        {
            e.RowError = "Lütfen Dosya Uzantý alanýný boþ býrakmayýnýz...";
            return;
        }
    }
}
