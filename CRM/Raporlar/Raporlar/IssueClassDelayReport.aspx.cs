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
using System.Collections.Generic;

public partial class CRM_Raporlar_Raporlar_IssueClassDelayReport : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "IssueClassDelayReport", "Select"))
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
        this.StartDate.Date = new DateTime(2008, 1, 1);
        this.EndDate.Date = DateTime.Now.Date;
        InitGridTable(this.DataTableList.Table);
        fillcomboxes();
    }

    public void menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("excel"))
        {
            CrmUtils.ExportToxls(gridExport, "grid", true);
        }
        else if (e.Item.Name.Equals("pdf"))
        {
            CrmUtils.ExportTopdf(gridExport, "grid", true);
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
        dt.Columns.Add("IssueId", typeof(int));
        dt.Columns.Add("Baslik", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("UserName", typeof(string));
        dt.Columns.Add("FirmaName", typeof(string));
        dt.Columns.Add("ProjeName", typeof(string));
        dt.Columns.Add("BildirimTarihi", typeof(DateTime));
        dt.Columns.Add("TeslimTarihi", typeof(DateTime));
        dt.Columns.Add("DurumName", typeof(string));
        dt.Columns.Add("KeyWords", typeof(string));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument()
    {
        string _Durums = string.Empty;
        this.DataTableList.Table.Rows.Clear();
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder("EXEC Sp_GetIssueClassDelay @VirusSinifId,@StartDate,@EndDate");
        int tmpID = -1;
        using (SqlCommand cmd = DB.SQL(this.Context, sb.ToString()))
        {
            using (DataSet ds = new DataSet())
            {

                if (this.VirusSinifID.Text.Trim() != "")
                {
                    tmpID = Convert.ToInt32(this.VirusSinifID.Value.ToString().Split('|')[0]);
                    DB.AddParam(cmd, "@VirusSinifId", tmpID);
                }
                else
                    DB.AddParam(cmd, "@VirusSinifId", SqlDbType.Int);
                if (!CrmUtils.ControllToDate(this.Page, StartDate.Date.ToString()) &&
                     !CrmUtils.ControllToDate(this.Page, EndDate.Date.ToString()))
                {
                    DB.AddParam(cmd, "@StartDate", this.StartDate.Date);
                    DB.AddParam(cmd, "@EndDate", this.EndDate.Date);
                }
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    cmd.CommandTimeout = 5000;
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);
                }
                foreach (DataRow rdr in ds.Tables[0].Rows)
                {
                    DataRow row = this.DataTableList.Table.NewRow();

                    row["ID"] = rdr["Id"];
                    row["IssueId"] = rdr["IssueId"];
                    row["Baslik"] = rdr["Baslik"];
                    row["CreatedBy"] = rdr["CreatedBy"];
                    row["UserName"] = rdr["UserName"];
                    row["FirmaName"] = rdr["FirmaName"];
                    row["ProjeName"] = rdr["ProjeName"];
                    row["BildirimTarihi"] = rdr["BildirimTarihi"];
                    row["TeslimTarihi"] = rdr["TeslimTarihi"];
                    row["DurumName"] = rdr["DurumName"];
                    row["KeyWords"] = rdr["KeyWords"];

                    this.DataTableList.Table.Rows.Add(row);
                }
                this.DataTableList.Table.AcceptChanges();
            }

            grid.DataBind();
            grid.ExpandAll();

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
        //GC.Collect();
        //
    }

    void fillcomboxes()
    {

        data.BindComboBoxesNoEmpty(this.Context, VirusSinifID, "EXEC SP_GetVirusSinifCreateIssue '" + Membership.GetUser().UserName + "'", "VirusSinifID", "Adi");


    }



    protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "Baslik")
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }
    }
}