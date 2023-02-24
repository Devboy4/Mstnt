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

public partial class CRM_Genel_BR_list : System.Web.UI.Page
{
    CrmUtils data = new CrmUtils();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Security.CheckPermission(this.Context, "BR", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }
        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        menu.ItemClick += new MenuItemEventHandler(MenuFirma_ItemClick);
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }
        fillcomboxes();
        InitGridTable(this.DataTableList.Table);
        FirstLoadDocument();

        
    }

    protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters != null && e.Parameters == "x")
        {
            FirstLoadDocument();
            grid.DataBind();
        }
        if (e.Parameters != null && e.Parameters == "y")
        {
            LoadDocument();
            grid.DataBind();
        }
    }

    private void LoadDocument()
    {
        DataTableList.Table.Clear();
        StringBuilder sb = new StringBuilder();
        if (Roles.IsUserInRole(Membership.GetUser().UserName, "Merkez Çalýþanlarý") ||
            Roles.IsUserInRole(Membership.GetUser().UserName, "Merkez Yöneticileri") ||
            Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator"))
        {
            sb = new StringBuilder();
            sb.Append("SELECT t1.*,t5.YaziRengi From BrTablosu t1 Left Outer Join BrDurum t5 On t1.BrDurumID=t5.BrDurumID ");
            sb.Append("Where t1.BrTablosuID Is Not Null ");
        }
        else
        {
            sb = new StringBuilder();
            sb.Append("SELECT t1.*,t5.YaziRengi From BrTablosu t1 Left Outer Join BrDurum t5 On t1.BrDurumID=t5.BrDurumID ");
            sb.Append("LEFT JOIN Issue t6 on (t1.RelatedIssue=t6.IssueId) ");
            sb.Append("Inner Join UserAllowedIssueList t3 on t6.IndexId=t3.IssueID ");
            sb.Append("Inner Join SecurityUsers t4 on t4.IndexId=t3.UserID ");
            sb.Append("Where t4.UserName=@UserName ");
        }

        #region Tarih Filtreleniyor...
        if (!CrmUtils.ControllToDate(this.Page, StartDate.Date.ToString()) &&
        !CrmUtils.ControllToDate(this.Page, EndDate.Date.ToString()))
        {
            sb.Append(" AND t1.Tarih>=@StartDate");
            sb.Append(" AND t1.Tarih<=@EndDate");
        }
        #endregion

        #region Durum Filtreleniyor...
        bool ilkk = true;
        foreach (ListItem ite in DurumList1.Items)
        {
            if (ite.Selected)
            {
                if (ilkk)
                {
                    sb.Append(" AND CONVERT(VarChar(50),t1.BrDurumID) IN('" + ite.Value.ToString() + "'");
                    ilkk = false;
                }
                else
                    sb.Append(",'" + ite.Value.ToString() + "'");
            }
        }
        if (!ilkk)
            sb.Append(")");
        #endregion

        #region indexId filtreleniyor...
        if (this.IndexID.Value != null)
            sb.Append(" AND t1.IndexID=@IndexID");
        sb.Append(" Order By t1.IndexID Desc");
        #endregion

        using (SqlCommand cmd = DB.SQL(this.Context, sb.ToString()))
        {

            #region values
            if (!CrmUtils.ControllToDate(this.Page, StartDate.Date.ToString()) &&
                  !CrmUtils.ControllToDate(this.Page, EndDate.Date.ToString()))
            {
                DB.AddParam(cmd, "@StartDate", this.StartDate.Date);
                DB.AddParam(cmd, "@EndDate", this.EndDate.Date);
            }
            if (this.IndexID.Value != null)
                DB.AddParam(cmd, "@IndexID", int.Parse(this.IndexID.Value.ToString()));
            DB.AddParam(cmd, "@UserName", 150, Membership.GetUser().UserName);
            #endregion

            #region fill data
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DataRow row = DataTableList.Table.NewRow();
                row["ID"] = rdr["BrTablosuID"];
                row["IndexID"] = rdr["IndexID"];
                row["RefNo"] = rdr["RefNo"];
                row["YaziRengi"] = rdr["YaziRengi"];
                row["StokKodu"] = rdr["StokKodu"];
                row["Renk"] = rdr["Renk"];
                row["Size"] = rdr["Size"];
                row["Tarih"] = rdr["Tarih"];
                row["Adet"] = rdr["Adet"].ToString();
                row["BrMarkaID"] = rdr["BrMarkaID"];
                row["isteyenProjeID"] = rdr["isteyenProjeID"];
                row["istenilenProjeID"] = rdr["istenilenProjeID"];
                row["BrDurumID"] = rdr["BrDurumID"];
                row["irsaliyeNo"] = rdr["irsaliyeNo"];
                row["MusteriAdi"] = rdr["MusteriAdi"];
                row["MusteriTel"] = rdr["MusteriTel"];
                row["PersonelAdi"] = rdr["PersonelAdi"];
                row["CreatedBy"] = rdr["CreatedBy"];
                row["ModifiedBy"] = rdr["ModifiedBy"];
                row["CreationDate"] = rdr["CreationDate"];
                row["ModificationDate"] = rdr["ModificationDate"];
                DataTableList.Table.Rows.Add(row);

            }
            DataTableList.Table.AcceptChanges();
            rdr.Close();
            #endregion

            if (DataTableList != null)
                DataTableList.Dispose();
            if (sb != null)
                sb = null;
        }

    }

    private void FirstLoadDocument()
    {
        DataTableList.Table.Clear();
        StringBuilder sb = new StringBuilder();
        if (Roles.IsUserInRole(Membership.GetUser().UserName, "Merkez Çalýþanlarý") ||
            Roles.IsUserInRole(Membership.GetUser().UserName, "Merkez Yöneticileri") ||
            Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator"))
        {
            sb = new StringBuilder();
            sb.Append("SELECT  TOP 1000 t1.*,t5.YaziRengi From BrTablosu t1 Left Outer Join BrDurum t5 On t1.BrDurumID=t5.BrDurumID "); 
            sb.Append("Order By t1.IndexID Desc ");
        }
        else
        {
            sb = new StringBuilder();
            sb.Append("SELECT TOP 1000 t1.*,t5.YaziRengi From BrTablosu t1 Left Outer Join BrDurum t5 On t1.BrDurumID=t5.BrDurumID ");
            sb.Append("LEFT JOIN Issue t6 on (t1.RelatedIssue=t6.IssueId) ");
            sb.Append("Inner Join UserAllowedIssueList t3 on t6.IndexId=t3.IssueId ");
            sb.Append("Inner Join SecurityUsers t4 on t4.IndexId=t3.UserID ");
            sb.Append("Where t4.UserName=@UserName Order By t1.IndexID Desc");
        }
        using (SqlCommand cmd = DB.SQL(this.Context, sb.ToString()))
        {
            DB.AddParam(cmd, "@UserName", 150, Membership.GetUser().UserName);
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DataRow row = DataTableList.Table.NewRow();
                row["ID"] = rdr["BrTablosuID"];
                row["IndexID"] = rdr["IndexID"];
                row["RefNo"] = rdr["RefNo"];
                row["StokKodu"] = rdr["StokKodu"];
                row["YaziRengi"] = rdr["YaziRengi"];
                row["Renk"] = rdr["Renk"];
                row["Tarih"] = rdr["Tarih"];
                row["Size"] = rdr["Size"];
                row["Adet"] = rdr["Adet"].ToString();
                row["BrMarkaID"] = rdr["BrMarkaID"];
                row["isteyenProjeID"] = rdr["isteyenProjeID"];
                row["istenilenProjeID"] = rdr["istenilenProjeID"];
                row["BrDurumID"] = rdr["BrDurumID"];
                row["irsaliyeNo"] = rdr["irsaliyeNo"];
                row["MusteriAdi"] = rdr["MusteriAdi"];
                row["MusteriTel"] = rdr["MusteriTel"];
                row["PersonelAdi"] = rdr["PersonelAdi"];
                row["CreatedBy"] = rdr["CreatedBy"];
                row["ModifiedBy"] = rdr["ModifiedBy"];
                row["CreationDate"] = rdr["CreationDate"];
                row["ModificationDate"] = rdr["ModificationDate"];
                DataTableList.Table.Rows.Add(row);

            }
            DataTableList.Table.AcceptChanges();
            rdr.Close();
        }

        if (DataTableList != null)
            DataTableList.Dispose();
        if (sb != null)
            sb = null;
    }

    void fillcomboxes()
    {

        data.ChcList_Fill(DurumList1, "SELECT BrDurumID,Adi FROM BrDurum ORDER BY Adi", "BrDurumID", "Adi");

    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IndexID", typeof(int));
        dt.Columns.Add("RefNo", typeof(string));
        dt.Columns.Add("StokKodu", typeof(string));
        dt.Columns.Add("YaziRengi", typeof(string));
        dt.Columns.Add("Tarih", typeof(DateTime));
        dt.Columns.Add("Renk", typeof(string));
        dt.Columns.Add("Size", typeof(string));
        dt.Columns.Add("Adet", typeof(string));
        dt.Columns.Add("BrMarkaID", typeof(Guid));
        dt.Columns.Add("isteyenProjeID", typeof(Guid));
        dt.Columns.Add("istenilenProjeID", typeof(Guid));
        dt.Columns.Add("BrDurumID", typeof(Guid));
        dt.Columns.Add("irsaliyeNo", typeof(string));
        dt.Columns.Add("MusteriAdi", typeof(string));
        dt.Columns.Add("MusteriTel", typeof(string));
        dt.Columns.Add("PersonelAdi", typeof(string));
        dt.Columns.Add("Description", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModificationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
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

    protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType != GridViewRowType.Data) return;

        object YaziRengi = (object)this.grid.GetRowValues(e.VisibleIndex, "YaziRengi");

        if (String.IsNullOrEmpty(YaziRengi.ToString()))
        {
            e.Row.ForeColor = System.Drawing.Color.FromName("#000000");
        }
        else
        {
            e.Row.ForeColor = System.Drawing.Color.FromName(YaziRengi.ToString());
        }
    }

    protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "IndexID")
        {
            object YaziRengi = (object)this.grid.GetRowValues(e.VisibleIndex, "YaziRengi");
            ASPxHyperLink link1 = ((ASPxHyperLink)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "IssueLink"));
            if (String.IsNullOrEmpty(YaziRengi.ToString()))
            {
                link1.ForeColor = System.Drawing.Color.FromName("#000000");
            }
            else
            {
                link1.ForeColor = System.Drawing.Color.FromName(YaziRengi.ToString());
            }
        }
    }
}
