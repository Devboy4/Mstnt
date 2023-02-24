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

public partial class CRM_Genel_IsPlan_list : System.Web.UI.Page
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


        // Eventleri tanimlayalim
        this.menu.ItemClick += new MenuItemEventHandler(menu_ItemClick);

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }

        if (!Security.CheckPermission(this.Context, "IsPlani", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        InitGridDTDetail(DTDetail.Table);
        InitGridDTDetail2(DTDetail2.Table);
        InitGridTable(DTList.Table);

        #region Grid security
        //bool bInsert = Security.CheckPermission(this.Context, "IsPlani", "Insert");
        //bool bUpdate = Security.CheckPermission(this.Context, "IsPlani", "Update");
        //bool bDelete = Security.CheckPermission(this.Context, "IsPlani", "Delete");

        //if (bDelete)
        //    this.menu.Items.FindByName("save").Visible = true;
        //else
        //    this.menu.Items.FindByName("save").Visible = false;


        //InitGridTable(this.DataTableList.Table);

        //for (int i = 0; i < this.grid.Columns.Count; i++)
        //{
        //    if (this.grid.Columns[i] is GridViewCommandColumn)
        //    {
        //        (this.grid.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
        //        break;
        //    }
        //} 
        #endregion

        Guid id = GetUserID();

        HiddenUserID.Value = id.ToString();

        Session["PlanUserID"] = id.ToString();

        LoadDocument(id);

        fillcomboxes();

        this.UserID.SelectedIndex = this.UserID.Items.IndexOfValue(id.ToString().ToLower());

        Tarih1.Date = Convert.ToDateTime(DateTime.Now.Date.ToShortDateString());
    }

    private Guid GetUserID()
    {
        SqlCommand cmd = DB.SQL(this.Context, "Select UserID From SecurityUsers Where UserName=@UserName");
        DB.AddParam(cmd, "@UserName", 100, Membership.GetUser().UserName);
        cmd.Prepare();
        Guid id = (Guid)cmd.ExecuteScalar();
        return id;
    }

    void fillcomboxes()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT t1.UserID,(ISNULL(t1.UserName,'')+' ['+ISNULL(t1.FirstName,'')+' '+ISNULL(t1.LastName,'')+']') AS UserName FROM SecurityUsers t1 ");
        sb.Append("Left Outer Join aspnet_UsersInRoles t2 On t1.UserID=t2.UserID ");
        sb.Append("Left Outer Join aspnet_Roles t3 On t2.RoleID=t3.RoleID Where t3.RoleName='Model Standart Kullanýcý' ");
        sb.Append("Or t3.RoleName='Administrator' ORDER BY t1.UserName");
        data.BindComboBoxesNoEmpty(this.Context, UserID, sb.ToString(), "UserID", "UserName");
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

    protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters.ToString() == "x")
        {
            Guid id = new Guid(this.UserID.Value.ToString());
            LoadDocument(id);
            return;
        }
        if (e.Parameters.ToString() == "Tarih")
        {
            LoadDocument2();
            return;
        }
        if (e.Parameters.ToString() != "" || e.Parameters != null)
        {
            Guid id = new Guid(e.Parameters.ToString());
            LoadDocument(id);
            return;
        }
    }

    protected void detailGrid_DataSelect(object sender, EventArgs e)
    {
        ASPxGridView detailGrid = sender as ASPxGridView;
        DataRow[] rows = DTDetail.Table.Select("BildirimPlanID='" + detailGrid.GetMasterRowKeyValue().ToString() + "'");
        detailGrid.DataSource = null;
        detailGrid.DataSourceID = string.Empty;
        if (rows.Length == 0)
            return;
        DTDetail2.Table.Rows.Clear();
        string siparis_no = null;
        foreach (DataRow row in rows)
        {
            DataRow row2 = DTDetail2.Table.NewRow();
            row2["ID"] = row["BildirimPlanlariDetayID"];
            row2["IndexID"] = row["IndexID"];
            row2["IndexNo"] = row["IndexNo"];
            row2["BildirimPlanID"] = row["BildirimPlanID"];
            row2["BildirimPlanlariDetayID"] = row["BildirimPlanlariDetayID"];
            row2["IssueID"] = row["IssueID"];
            row2["KeyWords"] = row["KeyWords"];
            row2["Baslik"] = row["Baslik"];
            row2["Sirala"] = row["Sirala"];
            row2["FirmaName"] = row["FirmaName"];
            row2["ProjeAdi"] = row["ProjeAdi"];
            row2["Durum"] = row["Durum"];
            row2["CreatedBy"] = row["CreatedBy"];
            row2["ModifiedBy"] = row["ModifiedBy"];
            row2["ModificationDate"] = row["ModificationDate"];
            row2["CreationDate"] = row["CreationDate"];
            siparis_no = row["IndexID"].ToString();
            DTDetail2.Table.Rows.Add(row2);
        }
        DTDetail2.Table.AcceptChanges();
        detailGrid.SettingsText.Title = "Ýþ Planý detaylarý";
        detailGrid.DataSource = DTDetail2.Table;

    }

    private void InitGridTable(DataTable dt)
    {

        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IndexID", typeof(int));
        dt.Columns.Add("BildirimPlanID", typeof(Guid));
        dt.Columns.Add("UserID", typeof(Guid));
        dt.Columns.Add("Description", typeof(string));
        dt.Columns.Add("Tarih1", typeof(DateTime));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        dt.Columns.Add("CreationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
        dt.Columns["CreationDate"].DefaultValue = DateTime.Now;
    }

    private void InitGridDTDetail(DataTable dt)
    {

        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IndexID", typeof(int));
        dt.Columns.Add("IndexNo", typeof(int));
        dt.Columns.Add("BildirimPlanID", typeof(Guid));
        dt.Columns.Add("BildirimPlanlariDetayID", typeof(Guid));
        dt.Columns.Add("IssueID", typeof(Guid));
        dt.Columns.Add("KeyWords", typeof(string));
        dt.Columns.Add("FirmaName", typeof(string));
        dt.Columns.Add("Baslik", typeof(string));
        dt.Columns.Add("Sirala", typeof(int));
        dt.Columns.Add("ProjeAdi", typeof(string));
        dt.Columns.Add("Durum", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        dt.Columns.Add("CreationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
        dt.Columns["CreationDate"].DefaultValue = DateTime.Now;
    }

    private void InitGridDTDetail2(DataTable dt)
    {

        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IndexID", typeof(int));
        dt.Columns.Add("IndexNo", typeof(int));
        dt.Columns.Add("BildirimPlanID", typeof(Guid));
        dt.Columns.Add("BildirimPlanlariDetayID", typeof(Guid));
        dt.Columns.Add("IssueID", typeof(Guid));
        dt.Columns.Add("KeyWords", typeof(string));
        dt.Columns.Add("FirmaName", typeof(string));
        dt.Columns.Add("Baslik", typeof(string));
        dt.Columns.Add("Sirala", typeof(int));
        dt.Columns.Add("ProjeAdi", typeof(string));
        dt.Columns.Add("Durum", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        dt.Columns.Add("CreationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
        dt.Columns["CreationDate"].DefaultValue = DateTime.Now;
    }

    private void LoadDocument(Guid id)
    {
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder();
        Session["PlanUserID"] = id.ToString();
        sb.Append("SELECT * From BildirimPlanlari Where UserID=@UserID Order By Tarih1 Desc");

        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@UserID", id);
        cmd.Prepare();
        this.DTList.Table.Clear();
        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTList.Table.NewRow();

            row["ID"] = rdr["BildirimPlanID"];
            row["IndexID"] = rdr["IndexID"];
            row["Tarih1"] = rdr["Tarih1"];
            row["BildirimPlanID"] = rdr["BildirimPlanID"];
            row["UserID"] = rdr["UserID"];
            row["Description"] = rdr["Description"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];
            row["CreationDate"] = rdr["CreationDate"];

            this.DTList.Table.Rows.Add(row);
        }
        rdr.Close();

        this.DTList.Table.AcceptChanges();

        sb = new StringBuilder();
        DTDetail.Table.Clear();
        sb.Append("Select t1.*,t2.IndexID As IndexNo,t2.Baslik, t3.FirmaName, t4.Adi As ProjeAdi, t5.Adi As Durum, t2.KeyWords ");
        sb.Append("From BildirimPlanlariDetay t1 Left Outer Join Issue As t2 On t1.IssueID=t2.IssueID ");
        sb.Append("Left Outer Join Firma As t3 On t2.FirmaID=t3.FirmaID ");
        sb.Append("Left Outer Join Proje As t4 On t2.ProjeID=t4.ProjeID ");
        sb.Append("Left Outer Join Durum As t5 On t2.DurumID=t5.DurumID ");
        sb.Append("Order By t1.Sirala");

        cmd = DB.SQL(this.Context, sb.ToString());
        cmd.Prepare();
        rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            DataRow row = this.DTDetail.Table.NewRow();

            row["ID"] = rdr["BildirimPlanlariDetayID"];
            row["IndexID"] = rdr["IndexID"];
            row["IndexNo"] = rdr["IndexNo"];
            row["BildirimPlanlariDetayID"] = rdr["BildirimPlanlariDetayID"];
            row["BildirimPlanID"] = rdr["BildirimPlanID"];
            row["KeyWords"] = rdr["KeyWords"];
            row["IssueID"] = rdr["IssueID"];
            row["Baslik"] = rdr["Baslik"];
            row["Sirala"] = rdr["Sirala"];
            row["IssueID"] = rdr["IssueID"];
            row["FirmaName"] = rdr["FirmaName"];
            row["ProjeAdi"] = rdr["ProjeAdi"];
            row["Durum"] = rdr["Durum"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];
            row["CreationDate"] = rdr["CreationDate"];

            this.DTDetail.Table.Rows.Add(row);
        }
        rdr.Close();

        this.DTDetail.Table.AcceptChanges();
        grid.DataBind();

    }

    private void LoadDocument2()
    {
        if (CrmUtils.ControllToDate(this.Page, Tarih1.Date.ToString())) return;
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder();
        sb.Append("SELECT * From BildirimPlanlari Where Tarih1=@Tarih Order By CreationDate Desc");

        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@Tarih", Tarih1.Date);
        cmd.Prepare();
        this.DTList.Table.Clear();
        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTList.Table.NewRow();

            row["ID"] = rdr["BildirimPlanID"];
            row["IndexID"] = rdr["IndexID"];
            row["Tarih1"] = rdr["Tarih1"];
            row["BildirimPlanID"] = rdr["BildirimPlanID"];
            row["UserID"] = rdr["UserID"];
            row["Description"] = rdr["Description"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];
            row["CreationDate"] = rdr["CreationDate"];

            this.DTList.Table.Rows.Add(row);
        }
        rdr.Close();

        this.DTList.Table.AcceptChanges();

        #region MyRegion
        //sb = new StringBuilder();
        //DTDetail.Table.Clear();
        //sb.Append("Select t1.*,t2.IndexID As IndexNo,t2.Baslik, t3.FirmaName, t4.Adi As ProjeAdi, t5.Adi As Durum, t2.KeyWords ");
        //sb.Append("From BildirimPlanlariDetay t1 Left Outer Join Issue As t2 On t1.IssueID=t2.IssueID ");
        //sb.Append("Left Outer Join Firma As t3 On t2.FirmaID=t3.FirmaID ");
        //sb.Append("Left Outer Join Proje As t4 On t2.ProjeID=t4.ProjeID ");
        //sb.Append("Left Outer Join Durum As t5 On t2.DurumID=t5.DurumID ");
        //sb.Append("Order By t1.Sirala");

        //cmd = DB.SQL(this.Context, sb.ToString());
        //cmd.Prepare();
        //rdr = cmd.ExecuteReader();

        //while (rdr.Read())
        //{
        //    DataRow row = this.DTDetail.Table.NewRow();

        //    row["ID"] = rdr["BildirimPlanlariDetayID"];
        //    row["IndexID"] = rdr["IndexID"];
        //    row["IndexNo"] = rdr["IndexNo"];
        //    row["BildirimPlanlariDetayID"] = rdr["BildirimPlanlariDetayID"];
        //    row["BildirimPlanID"] = rdr["BildirimPlanID"];
        //    row["KeyWords"] = rdr["KeyWords"];
        //    row["IssueID"] = rdr["IssueID"];
        //    row["Baslik"] = rdr["Baslik"];
        //    row["Sirala"] = rdr["Sirala"];
        //    row["IssueID"] = rdr["IssueID"];
        //    row["FirmaName"] = rdr["FirmaName"];
        //    row["ProjeAdi"] = rdr["ProjeAdi"];
        //    row["Durum"] = rdr["Durum"];
        //    row["CreatedBy"] = rdr["CreatedBy"];
        //    row["ModifiedBy"] = rdr["ModifiedBy"];
        //    row["ModificationDate"] = rdr["ModificationDate"];
        //    row["CreationDate"] = rdr["CreationDate"];

        //    this.DTDetail.Table.Rows.Add(row);
        //}
        //rdr.Close();

        //this.DTDetail.Table.AcceptChanges(); 
        #endregion

        grid.DataBind();

    }

    private bool SaveDocument()
    {
        DB.BeginTrans(this.Context);

        SqlCommand cmd = null;
        StringBuilder sb;

        DataTable changes = this.DTList.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        break;
                    case DataRowState.Modified:
                        break;
                    case DataRowState.Deleted:
                        cmd = DB.SQL(this.Context, "DELETE FROM BildirimPlanlari WHERE BildirimPlanID=@BildirimPlanID");
                        DB.AddParam(cmd, "@BildirimPlanID", (Guid)row["ID", DataRowVersion.Original]);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();

                        cmd = DB.SQL(this.Context, "DELETE FROM BildirimPlanlariDetay WHERE BildirimPlanID=@BildirimPlanID");
                        DB.AddParam(cmd, "@BildirimPlanID", (Guid)row["ID", DataRowVersion.Original]);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                }
            }
        }

        DB.Commit(this.Context);

        return true;
    }

}
