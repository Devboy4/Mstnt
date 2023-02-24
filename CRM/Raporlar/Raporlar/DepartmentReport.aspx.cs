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

public partial class CRM_Raporlar_Raporlar_DepartmentReport : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "DepartmentReport", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayı görme yetkisine sahip değilsiniz!</p></body></html>");
            this.Response.End();
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
        sb = new StringBuilder("EXEC GetIssuesFromSelectedDepartment @FirmaId,@ProjeId,@StartDate,@EndDate,@Durums");
        int tmpID = -1;
        using (SqlCommand cmd = DB.SQL(this.Context, sb.ToString()))
        {
            using (DataSet ds = new DataSet())
            {

                if (this.FirmaID.Text.Trim() != "")
                {
                    tmpID = Convert.ToInt32(this.FirmaID.Value.ToString());
                    DB.AddParam(cmd, "@FirmaId", tmpID);
                }
                else
                    DB.AddParam(cmd, "@FirmaId", SqlDbType.Int);
                if (this.ProjeID.Text.Trim() != "")
                {
                    try
                    {
                        tmpID = Convert.ToInt32(this.ProjeID.Value.ToString());
                    }
                    catch (Exception)
                    {

                        tmpID = GetProjeId(this.ProjeID.Text);
                    }
                    
                    DB.AddParam(cmd, "@ProjeId", tmpID);
                }
                else
                    DB.AddParam(cmd, "@ProjeId", SqlDbType.Int);
                if (!CrmUtils.ControllToDate(this.Page, StartDate.Date.ToString()) &&
                     !CrmUtils.ControllToDate(this.Page, EndDate.Date.ToString()))
                {
                    DB.AddParam(cmd, "@StartDate", this.StartDate.Date);
                    DB.AddParam(cmd, "@EndDate", this.EndDate.Date);
                }

                #region Durum Filtreleniyor...
                bool ilkk = true;
                foreach (ListItem ite in this.DurumList1.Items)
                {
                    if (ite.Selected)
                    {
                        if (ilkk)
                        {
                            _Durums = ite.Value.ToString();
                            ilkk = false;
                        }
                        else
                        {
                            _Durums += "," + ite.Value.ToString();
                        }
                    }
                }
                #endregion

                if (_Durums != string.Empty)
                {
                    DB.AddParam(cmd, "@Durums", 500, _Durums);
                }
                else
                    DB.AddParam(cmd, "@Durums", SqlDbType.NVarChar);

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

    int GetProjeId(string ProjeName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select top 1 IndexID from Proje where FirmaID in(select FirmaID from Firma where Active = 1) and Adi = '"+ ProjeName + "'");
            using (SqlCommand cmd = DB.SQL(this.Context, "select "))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        catch (Exception)
        {

            return -1;
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

        data.BindComboBoxesInt(this.Context, FirmaID, "Select IndexId,FirmaName From Firma where Active=1 Order By FirmaName", "IndexId", "FirmaName");
        data.ChcList_Fill(DurumList1, "SELECT IndexId,Adi FROM Durum ORDER BY Adi", "IndexId", "Adi");

    }

    protected void ProjeID_Callback(object source, CallbackEventArgsBase e)
    {
        if (e.Parameter.ToString() == null || e.Parameter.ToString() == "")
            return;
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder();
        sb.Append("SELECT t1.IndexID ProjeID,Adi FROM Proje t1 left join Firma t2 on (t1.FirmaID=t2.FirmaID) WHERE t2.IndexID=" + e.Parameter.ToString() + " ORDER BY Adi ");
        data.BindComboBoxesInt(this.Context, ProjeID, sb.ToString(), "ProjeID", "Adi");
    }

    protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "Baslik")
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }
    }
}