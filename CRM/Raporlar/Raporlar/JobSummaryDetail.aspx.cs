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

public partial class CRM_Raporlar_JobSummaryDetail : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "Raporlar - Job Summary", "Select"))
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
        this.HiddenID.Value = Request.QueryString["UserId"].ToString();

        InitGridTable(this.DataTableList.Table);

        LoadDocument();
    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Sinif", typeof(string));
        dt.Columns.Add("Pnr", typeof(int));
        dt.Columns.Add("GorevTarihi", typeof(DateTime));
        dt.Columns.Add("SonIslemTarihi", typeof(DateTime));
        dt.Columns.Add("SonIslemFark", typeof(int));
        dt.Columns.Add("SonIslemBugunFark", typeof(int));        
        dt.Columns.Add("Baslik", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CvpYok", typeof(int));
        dt.Columns.Add("BendenBekleniyor", typeof(int));
        dt.Columns.Add("OnlardanBekleniyor", typeof(int));
        dt.Columns.Add("SadeceBenKapadim", typeof(int));
        dt.Columns.Add("SadeceOnlarKapadi", typeof(int));
        dt.Columns.Add("Acik", typeof(int));
        dt.Columns.Add("Kapali", typeof(int));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument()
    {
        if (String.IsNullOrEmpty(this.HiddenID.Value))
        {
            CrmUtils.CreateMessageAlert(this.Page, "Kullanıcı Yok...", "alert");
            return;
        }
        StringBuilder sb = new StringBuilder();
        int UserId = int.Parse(this.HiddenID.Value);
        sb = new StringBuilder("Exec SP_GetMyIssuesRpt2 @OpId,@UserId");
        using (SqlCommand cmd = DB.SQL(this.Context, sb.ToString()))
        {
            DB.AddParam(cmd, "@OpId", int.Parse(Request.QueryString["OpId"].ToString()));
            DB.AddParam(cmd, "@UserId", UserId);
            cmd.CommandTimeout = 1000;
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();

            #region FillGrid
            this.DataTableList.Table.Rows.Clear();
            int sayac = 1;
            while (rdr.Read())
            {
                DataRow row = this.DataTableList.Table.NewRow();
                row["ID"] = sayac;
                row["Sinif"] = rdr["Sinif"];
                row["CvpYok"] = rdr["CvpYok"];
                row["BendenBekleniyor"] = rdr["BendenBekleniyor"];
                row["OnlardanBekleniyor"] = rdr["OnlardanBekleniyor"];
                row["SadeceBenKapadim"] = rdr["SadeceBenKapadim"];
                row["SadeceOnlarKapadi"] = rdr["SadeceOnlarKapadi"];
                row["Acik"] = rdr["Acik"];
                row["Kapali"] = rdr["Kapali"];
                row["Pnr"] = rdr["Pnr"];
                row["GorevTarihi"] = rdr["GorevTarihi"];
                row["SonIslemTarihi"] = rdr["SonIslemTarihi"];
                row["SonIslemFark"] = rdr["SonIslemFark"];
                row["SonIslemBugunFark"] = rdr["SonIslemBugunFark"];
                row["Baslik"] = rdr["Baslik"];
                row["CreatedBy"] = rdr["CreatedBy"];

                this.DataTableList.Table.Rows.Add(row);
                sayac++;
            }
            this.DataTableList.Table.AcceptChanges();
            rdr.Close();


            if (Request.QueryString["Vs"] != null)
            {
                grid.FilterExpression = "Sinif='" + Request.QueryString["Vs"].ToString() + "'";
            }

            grid.DataBind();
            grid.ExpandAll();
        }
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
