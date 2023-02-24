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
public partial class CRM_Genel_Proje_list : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "Proje", "Select"))
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

        bool bInsert = Security.CheckPermission(this.Context, "Proje", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Proje", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Proje", "Delete");

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
        dt.Columns.Add("ProjeID", typeof(Guid));
        dt.Columns.Add("Adi", typeof(string));
        dt.Columns.Add("ProjeAmiriID", typeof(Guid));
        dt.Columns.Add("ProjeSinifID", typeof(Guid));
        dt.Columns.Add("FirmaID", typeof(Guid));
        dt.Columns.Add("FirmaName", typeof(string));
        dt.Columns.Add("IsShop", typeof(int));
        dt.Columns.Add("ProjeAmir", typeof(string));
        dt.Columns.Add("ProjeMailAdresi", typeof(string));
        dt.Columns.Add("Description", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("OperasyonDay", typeof(int));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
        dt.Columns["CreationDate"].DefaultValue = DateTime.Now;
    }

    private void LoadDocument()
    {
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder();
        sb.Append("SELECT Pro.*, (ISNULL(U.UserName, '') + ' [' +(ISNULL(U.FirstName, '') + ' ' + ISNULL(U.LastName, '')+']')) AS ProjeAmir, F.FirmaName ");
        sb.Append("FROM  SecurityUsers AS U RIGHT OUTER JOIN Proje AS Pro ON U.UserID = Pro.ProjeAmiriID LEFT OUTER JOIN ");
        sb.Append("Firma AS F ON Pro.FirmaID = F.FirmaID ORDER BY Pro.Adi");
        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();

            row["ID"] = rdr["ProjeID"];
            row["ProjeID"] = rdr["ProjeID"];
            row["FirmaID"] = rdr["FirmaID"];
            row["FirmaName"] = rdr["FirmaName"];
            row["ProjeAmiriID"] = rdr["ProjeAmiriID"];
            row["ProjeSinifID"] = rdr["ProjeSinifID"];
            row["IsShop"] = rdr["IsShop"];
            row["ProjeAmir"] = rdr["ProjeAmir"];
            row["ProjeMailAdresi"] = rdr["ProjeMailAdresi"];
            row["Adi"] = rdr["Adi"];
            row["Description"] = rdr["Description"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];
            row["CreationDate"] = rdr["CreationDate"];
            row["OperasyonDay"] = rdr["OperasyonDay"];

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
                        sb.Append("INSERT INTO Proje(ProjeID,FirmaID,Adi,ProjeMailAdresi,ProjeSinifID,ProjeAmiriID,Description,IsShop,CreatedBy,CreationDate,OperasyonDay)");
                        sb.Append("VALUES(@ProjeID,@FirmaID,@Adi,@ProjeMailAdresi,@ProjeSinifID,@ProjeAmiriID,@Description,@IsShop,@CreatedBy,@CreationDate,@OperasyonDay)");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@ProjeID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@FirmaID", (Guid)row["FirmaID"]);
                        if (row["ProjeAmiriID"] != null && row["ProjeAmiriID"].ToString() != "")
                            DB.AddParam(cmd, "@ProjeAmiriID", (Guid)row["ProjeAmiriID"]);
                        else
                            DB.AddParam(cmd, "@ProjeAmiriID", SqlDbType.UniqueIdentifier);
                        if (row["ProjeSinifID"] != null && row["ProjeSinifID"].ToString() != "")
                            DB.AddParam(cmd, "@ProjeSinifID", (Guid)row["ProjeSinifID"]);
                        else
                            DB.AddParam(cmd, "@ProjeSinifID", SqlDbType.UniqueIdentifier);
                        DB.AddParam(cmd, "@Adi", 100, row["Adi"].ToString().ToUpper());
                        DB.AddParam(cmd, "@Description", 255, row["Description"].ToString().ToUpper());
                        DB.AddParam(cmd, "@ProjeMailAdresi", 255, row["ProjeMailAdresi"].ToString());
                        DB.AddParam(cmd, "@IsShop", (int)row["IsShop"]);
                        DB.AddParam(cmd, "@OperasyonDay", (int)row["OperasyonDay"]);
                        DB.AddParam(cmd, "@CreatedBy", 255, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Modified:
                        sb = new StringBuilder();
                        sb.Append("UPDATE Proje SET ");
                        sb.Append("Adi=@Adi,");
                        sb.Append("FirmaID=@FirmaID,");
                        sb.Append("ModifiedBy=@ModifiedBy,");
                        sb.Append("Description=@Description,");
                        sb.Append("ProjeSinifID=@ProjeSinifID,");
                        sb.Append("ProjeAmiriID=@ProjeAmiriID,");
                        sb.Append("IsShop=@IsShop,");
                        sb.Append("OperasyonDay=@OperasyonDay,");
                        sb.Append("ProjeMailAdresi=@ProjeMailAdresi,");
                        sb.Append("ModificationDate=@ModificationDate ");
                        sb.Append("WHERE ProjeID=@ProjeID");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@ProjeID", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@FirmaID", (Guid)row["FirmaID"]);
                        if (row["ProjeAmiriID"] != null && row["ProjeAmiriID"].ToString() != "")
                            DB.AddParam(cmd, "@ProjeAmiriID", (Guid)row["ProjeAmiriID"]);
                        else
                            DB.AddParam(cmd, "@ProjeAmiriID", SqlDbType.UniqueIdentifier);
                        if (row["ProjeSinifID"] != null && row["ProjeSinifID"].ToString() != "")
                            DB.AddParam(cmd, "@ProjeSinifID", (Guid)row["ProjeSinifID"]);
                        else
                            DB.AddParam(cmd, "@ProjeSinifID", SqlDbType.UniqueIdentifier);
                        DB.AddParam(cmd, "@Adi", 100, row["Adi"].ToString().ToUpper());
                        DB.AddParam(cmd, "@Description", 255, row["Description"].ToString().ToUpper());
                        DB.AddParam(cmd, "@ModifiedBy", 255, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@ProjeMailAdresi", 255, row["ProjeMailAdresi"].ToString());
                        DB.AddParam(cmd, "@IsShop", (int)row["IsShop"]);
                        DB.AddParam(cmd, "@OperasyonDay", (int)row["OperasyonDay"]);
                        DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Deleted:
                        try
                        {
                            cmd = DB.SQL(this.Context, "DELETE FROM Proje WHERE ProjeID=@ProjeID");
                            DB.AddParam(cmd, "@ProjeID", (Guid)row["ID", DataRowVersion.Original]);
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

    protected void grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {

        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["ProjeAmiriID"] == null)
                e.NewValues["ProjeAmiriID"] = DBNull.Value;
            if (e.NewValues["ProjeSinifID"] == null)
                e.NewValues["ProjeSinifID"] = DBNull.Value;
            if (e.NewValues["IsShop"] == null)
                e.NewValues["IsShop"] = 0;
            if (e.NewValues["OperasyonDay"] == null)
                e.NewValues["OperasyonDay"] = 3;
        }

    }

    protected void grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {

        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["ProjeSinifID"] == null)
                e.NewValues["ProjeSinifID"] = DBNull.Value;
            if (e.NewValues["IsShop"] == null)
                e.NewValues["IsShop"] = 0;
            if (e.NewValues["OperasyonDay"] == null)
                e.NewValues["OperasyonDay"] = 3;
        }

    }

    protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {

        if (e.NewValues["Adi"] == null)
        {
            e.RowError = "Lütfen Adý alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["FirmaID"] == null)
        {
            e.RowError = "Firma alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["ProjeMailAdresi"] != null)
        {
            if (!CrmUtils.IsEmail(e.NewValues["ProjeMailAdresi"].ToString()))
            {
                e.RowError = "E-Mail adresi geçersiz...";
                return;
            }
        }
        if (grid.IsNewRowEditing)
        {
            DataRow[] Rows = DataTableList.Table.Select("Adi='" + e.NewValues["Adi"].ToString() + "' And FirmaID='" + e.NewValues["FirmaID"].ToString() + "'");
            if (Rows.Length > 0)
            {
                e.RowError = "Bu Departman belirtilen Ýlgili Birim'ya daha önceden tanýmlý görünüyor...";
                return;
            }
        }
    }
}
