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

public partial class MarjinalCRM_Settings_Ulke_list : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "Tanim - Ülke", "Select"))
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

        bool bInsert = Security.CheckPermission(this.Context, "Tanim - Ülke", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Tanim - Ülke", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Tanim - Ülke", "Delete");

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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["Adi"] == null)
        {
            e.RowError = "Lütfen Adý Alanýný Boþ Býrakmayýnýz...";
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public void menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("save"))
        {
            if (!Security.CheckPermission(this.Context, "Tanim - Ülke", "Insert"))
            {
                this.Response.Write("<script language='javascript'>alert('Yeni Kayýt iþlemi yapma yetkisine sahip deðilsiniz!');</script>");
                return;
            }
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dt"></param>
    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("UlkeID", typeof(Guid));
        dt.Columns.Add("Adi", typeof(string));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }
    /// <summary>
    /// 
    /// </summary>
    private void LoadDocument()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("SELECT * FROM Ulke ORDER BY Adi");

        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());

        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();

            row["ID"] = rdr["UlkeID"];
            row["UlkeID"] = rdr["UlkeID"];
            row["Adi"] = rdr["Adi"];

            this.DataTableList.Table.Rows.Add(row);
        }
        rdr.Close();

        this.DataTableList.Table.AcceptChanges();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool SaveDocument()
    {
        DB.BeginTrans(this.Context);

        SqlCommand cmd = null;
        StringBuilder sb = new StringBuilder();

        DataTable changes = this.DataTableList.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        sb = new StringBuilder();
                        sb.Append("INSERT INTO Ulke(UlkeID,Adi)");
                        sb.Append("VALUES(@UlkeID,@Adi)");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@UlkeID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@Adi", 255, row["Adi"].ToString());
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Modified:
                        sb = new StringBuilder();
                        sb.Append("UPDATE Ulke SET ");
                        sb.Append("Adi=@Adi ");
                        sb.Append("WHERE UlkeID=@UlkeID");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@UlkeID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@Adi", 255, row["Adi"].ToString());
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Deleted:
                        try
                        {
                            cmd = DB.SQL(this.Context, "EXEC UlkeDelete @UlkeID");
                            DB.AddParam(cmd, "@UlkeID", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            Session["ErrorText"] = ex.Message.ToString().Replace("'", null).Replace("\r\n", null);
                            DataTableList.Table.Clear();
                            LoadDocument();
                            grid.DataBind();
                            CrmUtils.MessageAlert(this.Page, (String)Session["ErrorText"], "stkeySilinemez");
                            return false;
                        }
                        break;
                }

            }
        }

        DB.Commit(this.Context);

        return true;
    }
}
