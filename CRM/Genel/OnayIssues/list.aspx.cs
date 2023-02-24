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

public partial class CRM_Genel_OnayIssues_list : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "OnayIssues", "Select"))
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

    protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters != null)
        {
            if (Convert.ToBoolean(e.Parameters.ToString()))
                this.grid.Selection.SelectAll();
            else
                this.grid.Selection.UnselectAll();
        }
    }

    public void menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("Ok"))
        {
            if (SaveDocument("Ok"))
            {
                this.Response.Redirect("./list.aspx");
            }
        }
        else if (e.Item.Name.Equals("Cancel"))
        {
            if (SaveDocument("Cancel"))
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
        dt.Columns.Add("IssueId", typeof(Guid));
        dt.Columns.Add("IndexID", typeof(int));
        dt.Columns.Add("PNR", typeof(int));
        dt.Columns.Add("Baslik", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("Aciklama", typeof(string));
        dt.Columns.Add("RequestDate", typeof(DateTime));
        dt.Columns.Add("CreationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument()
    {
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder("SELECT t1.*,t2.Baslik FROM OnayIssues t1 ");
        sb.Append("LEFT JOIN Issue t2 on (t1.IssueId=t2.IssueId) Order By t1.CreationDate Desc");

        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();

            row["ID"] = rdr["Id"];
            row["IssueId"] = rdr["IssueId"];
            row["IndexID"] = rdr["IndexID"];
            row["PNR"] = rdr["PNR"];
            row["Baslik"] = rdr["Baslik"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["Aciklama"] = rdr["Aciklama"];
            row["RequestDate"] = rdr["RequestDate"];
            row["CreationDate"] = rdr["CreationDate"];

            this.DataTableList.Table.Rows.Add(row);
        }
        rdr.Close();

        this.DataTableList.Table.AcceptChanges();
    }

    private bool SaveDocument(string ProcessName)
    {
        DB.BeginTrans(this.Context);

        SqlCommand cmd = null;
        StringBuilder sb;

        try
        {
            List<object> keyValues = this.grid.GetSelectedFieldValues("ID");
            string Ids = string.Empty;
            bool ilk = true;
            foreach (object key in keyValues)
            {
                DataRow row = DataTableList.Table.Rows.Find(key);
                if (ilk)
                {
                    Ids += row["IndexID"].ToString();
                    ilk = false;
                }
                else
                    Ids += "," + row["IndexID"].ToString();
            }

            sb = new StringBuilder();
            sb.Append("EXEC SaveOnayList @Ids,@ProcessName");
            cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@Ids", 3500, Ids);
            DB.AddParam(cmd, "@ProcessName", 50, ProcessName);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            DB.Rollback(this.Context);
            return false;
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