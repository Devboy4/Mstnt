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
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxCallback;
using DevExpress.Web.ASPxEditors;

public partial class CRM_Raporlar_UserSavsaklama : System.Web.UI.Page
{
    CrmUtils data = new CrmUtils();

    DataTable Firmas = new DataTable();

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
        if (!Security.CheckPermission(this.Context, "Raporlar - Savsaklama Raporu", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayı görme yetkisine sahip değilsiniz!</p></body></html>");
            this.Response.End();
        }
        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        menu.ItemClick += new MenuItemEventHandler(MenuFirma_ItemClick);

        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }

        InitGridTable(this.DataTableList.Table);


    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IndexID", typeof(int));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("Baslik", typeof(string));
        dt.Columns.Add("FirmaName", typeof(string));
        dt.Columns.Add("ProjeName", typeof(string));
        dt.Columns.Add("VirusSinifName", typeof(string));
        dt.Columns.Add("BildirimTarihi", typeof(DateTime));
        dt.Columns.Add("TeslimTarihi", typeof(DateTime));
        //dt.Columns.Add("SavsaklamaTarih", typeof(DateTime));
        dt.Columns.Add("DurumName", typeof(string));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument()
    {
        if (String.IsNullOrEmpty(this.UserId.Text))
        {
            CrmUtils.CreateMessageAlert(this.Page, "Lütfen Kullanıcı Seçiniz...", "alert");
            return;
        }
        StringBuilder sb = new StringBuilder();
        int UserId = int.Parse(this.UserId.Value.ToString());
        sb = new StringBuilder("Exec GetUserSavsaklama @UserId");
        if (!CrmUtils.ControllToDate(this.Page, StartDate.Date.ToString()))
            sb.Append(",@StartDate");
        if (!CrmUtils.ControllToDate(this.Page, EndDate.Date.ToString()))
            sb.Append(",@EndDate");
        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@UserId", UserId);
        if (!CrmUtils.ControllToDate(this.Page, StartDate.Date.ToString()))
            DB.AddParam(cmd, "@StartDate", StartDate.Date);
        if (!CrmUtils.ControllToDate(this.Page, EndDate.Date.ToString()))
            DB.AddParam(cmd, "@EndDate", EndDate.Date);
        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();

        #region FillGrid
        this.DataTableList.Table.Rows.Clear();
        while (rdr.Read())
        {
            DataRow[] rows = DataTableList.Table.Select("ID='" + rdr["IssueID"].ToString() + "'");
            if (rows.Length > 0) continue;
            DataRow row = this.DataTableList.Table.NewRow();
            row["ID"] = rdr["IssueID"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["IndexID"] = rdr["IndexId"];
            row["Baslik"] = rdr["Baslik"];
            row["FirmaName"] = rdr["FirmaName"];
            row["ProjeName"] = rdr["ProjeName"];
            row["VirusSinifName"] = rdr["VirusSinifName"];
            row["TeslimTarihi"] = rdr["TeslimTarihi"];
            //row["SavsaklamaTarih"] = rdr["SavsaklamaTarih"];
            row["BildirimTarihi"] = rdr["BildirimTarihi"];
            row["DurumName"] = rdr["DurumName"];

            this.DataTableList.Table.Rows.Add(row);

        }
        this.DataTableList.Table.AcceptChanges();
        rdr.Close();
        grid.DataBind();
        grid.ExpandAll();
        #endregion

    }

    private void MenuFirma_ItemClick(object source, MenuItemEventArgs e)
    {

        if (e.Item.Name.Equals("excel"))
        {
            CrmUtils.ExportToxls(gridExport, "grid", true);
        }
        else if (e.Item.Name.Equals("pdf"))
        {
            CrmUtils.ExportTopdf(gridExport, "grid", true);
        }
        else if (e.Item.Name.Equals("List"))
        {
            //LoadDocument();
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters != "x")
        {
            if (Convert.ToBoolean(e.Parameters.ToString()))
                this.grid.Selection.SelectAll();
            else
                this.grid.Selection.UnselectAll();
        }
        else
            LoadDocument();
    }

}
