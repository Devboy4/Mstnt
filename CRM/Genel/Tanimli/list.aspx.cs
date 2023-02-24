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
using DevExpress.Web.ASPxClasses;

public partial class CRM_Genel_Tanimli_list : System.Web.UI.Page
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
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayı görme yetkisine sahip değilsiniz!</p></body></html>");
            this.Response.End();
        }

        // Eventleri tanimlayalim
        this.menu.ItemClick += new MenuItemEventHandler(menu_ItemClick);

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }
        InitGridTable(this.DataTableList.Table);

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
        dt.Columns.Add("VirusSinif", typeof(string));
        dt.Columns.Add("Step", typeof(int));
        dt.Columns.Add("Saat", typeof(int));
        dt.Columns.Add("Active", typeof(int));
        dt.Columns.Add("BaslangicTarihi", typeof(DateTime));
        dt.Columns.Add("SonIslemTarihi", typeof(DateTime));
        dt.Columns.Add("SonrakiIslemTarihi", typeof(DateTime));
        dt.Columns.Add("Description", typeof(string));
        dt.Columns.Add("Firma", typeof(string));
        dt.Columns.Add("Proje", typeof(string));
        dt.Columns.Add("User", typeof(Guid));
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
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder("SELECT t1.*,t2.Adi VirusSinif,t3.FirmaName Firma,t4.Adi Proje FROM PeriyodikIsler t1 ");
        sb.Append("LEFT JOIN VirusSinif t2 on (t1.VirusSinifId=t2.IndexId) ");
        sb.Append("LEFT JOIN Firma t3 on (t1.FirmaId=t3.IndexId) ");
        if (Membership.GetUser().UserName.ToLower() != "admin" || !Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator")
            || !Roles.IsUserInRole(Membership.GetUser().UserName, "Merkez Yöneticileri"))
            sb.Append("LEFT JOIN Proje t4 on (t1.ProjeId=t4.IndexId) Where t1.CreatedBy=@CreatedBy ORDER BY t1.CreationDate Desc");
        else
            sb.Append("LEFT JOIN Proje t4 on (t1.ProjeId=t4.IndexId) ORDER BY t1.CreationDate Desc");

        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@CreatedBy", 100, Membership.GetUser().UserName);
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();

            row["ID"] = rdr["PeriyodikIslerID"];
            row["PeriyodikIslerID"] = rdr["PeriyodikIslerID"];
            row["VirusSinif"] = rdr["VirusSinif"];
            row["Step"] = rdr["Step"];
            row["Saat"] = rdr["Saat"];
            row["BaslangicTarihi"] = rdr["BaslangicTarihi"];
            row["SonrakiIslemTarihi"] = rdr["SonrakiIslemTarihi"];
            row["SonIslemTarihi"] = rdr["SonIslemTarihi"];
            row["Description"] = rdr["Description"];
            row["IndexID"] = rdr["IndexID"];
            row["Active"] = rdr["Active"];
            row["Firma"] = rdr["Firma"];
            row["Proje"] = rdr["Proje"];
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

    protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "Baslik")
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }
    }
}
