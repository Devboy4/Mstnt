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

public partial class frames_Events : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsCallback || IsPostBack) return;
        InitGridTable(this.DTIssueActivity.Table);
        int id = GetUserId(Membership.GetUser().UserName);
        if (id != -1)
            InitializeHistory(id);
    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IssueID", typeof(int));
        dt.Columns.Add("IndexId", typeof(int));
        dt.Columns.Add("EventDesc", typeof(string));
        dt.Columns.Add("IssueDesc", typeof(string));
        dt.Columns.Add("YaziRengi", typeof(string));
        dt.Columns.Add("PNR", typeof(string));
        dt.Columns.Add("Baslik", typeof(string));
        dt.Columns.Add("UserName", typeof(string));
        dt.Columns.Add("DurumName", typeof(string));
        dt.Columns.Add("FirmaName", typeof(string));
        dt.Columns.Add("ProjeName", typeof(string));
        dt.Columns.Add("date", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private int GetUserId(string UserName)
    {
        try
        {
            using (SqlCommand cmd = DB.SQL(this.Context, "Select IndexId From SecurityUsers Where UserName=@UserName"))
            {
                DB.AddParam(cmd, "@UserName", 100, UserName);
                cmd.Prepare();
                int id = (int)cmd.ExecuteScalar();
                return id;
            }
        }
        catch
        {
            return -1;
        }

    }

    private void InitializeHistory(int id)
    {
        SqlCommand cmd;
        IDataReader rdr;
        StringBuilder sb = new StringBuilder();
        try
        {
            #region// Uyarý Listesi Doluyor
            DTIssueActivity.Table.Clear();
            sb = new StringBuilder();
            sb.Append("Select t1.*,t2.Adi AS DurumName,t3.FirmaName,t4.Adi ProjeName,t5.Baslik,t5.IndexId PNR,");
            //sb.Append(",Case When t5.MainIssueID Is Not Null Then t7.IndexID ");
            //sb.Append("Else t5.IndexID End As PNR, ");
            sb.Append("YaziRengi='#000000' ");
            sb.Append("From EventTable t1 ");
            sb.Append("LEFT OUTER JOIN Durum t2 ON t1.DurumID=t2.IndexId ");
            sb.Append("LEFT OUTER JOIN Firma t3 ON t1.FirmaID=t3.IndexId ");
            sb.Append("LEFT OUTER JOIN Proje t4 ON t1.ProjeID=t4.IndexId ");
            sb.Append("LEFT OUTER JOIN Issue t5 ON t1.IssueID=t5.IndexId ");
            //sb.Append("LEFT OUTER JOIN Issue t7 ON t5.MainIssueID=t7.IndexId ");
            sb.Append("Where t1.UserID=@UserID ");
            int sID = int.Parse(this.Request.Params["id"]);
            if ((sID != 0))
            {
                sb.Append("And t1.IssueId=" + sID + " ");
            }
            sb.Append("And t1.EventSend=0 ORDER BY t1.EventSend Asc,t1.date Desc ");
            cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@UserID", id);
            cmd.Prepare();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DataRow row = DTIssueActivity.Table.NewRow();
                row["ID"] = rdr["EventTableID"];
                row["IssueID"] = rdr["IssueID"];
                row["IndexId"] = rdr["IndexId"];
                row["EventDesc"] = rdr["EventDesc"].ToString().ToUpper();
                row["IssueDesc"] = rdr["IssueDesc"].ToString().ToUpper();
                row["YaziRengi"] = rdr["YaziRengi"];
                row["Baslik"] = rdr["Baslik"].ToString().ToUpper();
                row["DurumName"] = rdr["DurumName"];
                row["FirmaName"] = rdr["FirmaName"];
                row["ProjeName"] = rdr["ProjeName"];
                row["UserName"] = rdr["UserName"];
                row["PNR"] = rdr["PNR"];
                row["date"] = rdr["date"];
                DTIssueActivity.Table.Rows.Add(row);
            }
            DTIssueActivity.Table.AcceptChanges();
            rdr.Close();
            #endregion
            if (this.DTIssueActivity != null)
                this.DTIssueActivity.Dispose();
        }
        catch { }


    }

    protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "EventDesc" || e.DataColumn.FieldName == "IssueDesc" || e.DataColumn.FieldName == "Baslik")
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }
    }

    protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType != GridViewRowType.Data) return;

        object YaziRengi = (object)this.grid.GetRowValues(e.VisibleIndex, "YaziRengi");


        if (YaziRengi != null && YaziRengi.ToString() != "")
            e.Row.ForeColor = System.Drawing.Color.FromName(YaziRengi.ToString());

        //if (Durum.ToString() == "ANTÝVÝRÜS")
        //    e.Row.BackColor = System.Drawing.Color.FromName("#99ff66");
        //else if (Durum.ToString() == "VÝRÜS")
        //    e.Row.BackColor = System.Drawing.Color.FromName("#ffff99");
        //else if (Durum.ToString() == "SAVSAKLAMA")
        //    e.Row.BackColor = System.Drawing.Color.FromName("#ff6600");

    }
}
