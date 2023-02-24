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
using DevExpress.Web.ASPxEditors;
using Model.Crm;
using DevExpress.Web.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxClasses;
using System.Globalization;
using DevExpress.Web.ASPxCallback;
using System.Collections.Generic;

public partial class CRM_Genel_Issue_Preview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsCallback || IsPostBack) return;
        Response.AppendHeader("Pragma", "no-cache");
        Response.AppendHeader("Cache-Control", "no-cache");

        Response.CacheControl = "no-cache";
        Response.Expires = -1;

        if (Request.QueryString["id"] == null) return;
        if (Security.CheckPermission(this.Context, "IsAllowedDeleteUsersToIssue", "Select"))
        {
            btnDeleteUsers.Visible = true;
            btnMakeDontdo.Visible = true;
        }
        int sID = int.Parse(this.Request.Params["id"]);
        if ((sID != null) && (sID != 0))
        {
            InitGridTable(this.DTIssueActivity.Table);
            InitGridDTIssueUsers(this.DTIssueUsers.Table);
            InitializeHistory(sID);
        }


    }

    void DeleteUsers()
    {
        try
        {
            grid2.UpdateEdit();
            string Ids = string.Empty;
            bool ilk = true;

            List<object> keyValues2 = this.grid2.GetSelectedFieldValues("ID");
            foreach (object key in keyValues2)
            {
                DataRow row2 = DTIssueUsers.Table.Rows.Find(key);
                if (ilk)
                {
                    Ids += row2["UserId"].ToString();
                    ilk = false;
                }
                else
                {
                    Ids += "," + row2["UserId"].ToString();
                }

            }

            StringBuilder sb = new StringBuilder();
            sb = new StringBuilder();
            sb.Append("EXEC DeleteUserAllowedIssueList @Ids,@IssueID");

            using (SqlCommand cmd = DB.SQL(this.Context, sb.ToString()))
            {
                DB.AddParam(cmd, "@Ids", 3500, Ids);
                DB.AddParam(cmd, "@IssueID", int.Parse(this.Request.Params["id"]));
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }
        catch
        {
        }

        finally
        {
            InitializeHistory(int.Parse(this.Request.Params["id"]));
            grid.DataBind();
            grid2.DataBind();
        }
    }

    void DontMakeUsers()
    {
        try
        {
            grid2.UpdateEdit();
            string Ids = string.Empty;
            bool ilk = true;

            List<object> keyValues2 = this.grid2.GetSelectedFieldValues("ID");
            foreach (object key in keyValues2)
            {
                DataRow row2 = DTIssueUsers.Table.Rows.Find(key);
                if (ilk)
                {
                    Ids += row2["UserId"].ToString();
                    ilk = false;
                }
                else
                {
                    Ids += "," + row2["UserId"].ToString();
                }

            }

            StringBuilder sb = new StringBuilder();
            sb = new StringBuilder();
            sb.Append("EXEC SP_DoDontMake @Ids,@IssueID");

            using (SqlCommand cmd = DB.SQL(this.Context, sb.ToString()))
            {
                DB.AddParam(cmd, "@Ids", 3500, Ids);
                DB.AddParam(cmd, "@IssueID", int.Parse(this.Request.Params["id"]));
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }
        catch
        {
        }

        finally
        {
            InitializeHistory(int.Parse(this.Request.Params["id"]));
            grid.DataBind();
            grid2.DataBind();
        }
    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Process", typeof(string));
        dt.Columns.Add("Comment", typeof(string));
        dt.Columns.Add("YaziRengi", typeof(string));
        dt.Columns.Add("DurumName", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("CommentDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitGridDTIssueUsers(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("UserId", typeof(int));
        dt.Columns.Add("IssueAntivirus", typeof(int));
        dt.Columns.Add("UserName", typeof(string));
        dt.Columns.Add("ProjeName", typeof(string));
        dt.Columns.Add("FirmaName", typeof(string));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitializeHistory(int id)
    {
        //IDataReader rdr;
        StringBuilder sb = new StringBuilder();
        using (DataSet ds = new DataSet())
        {

            using (SqlCommand cmd = DB.SQL(this.Context, "EXEC GetPreview @IssueID,@UserName"))
            {
                #region// IssueActivite Doluyor
                DTIssueActivity.Table.Rows.Clear();
                DB.AddParam(cmd, "@IssueID", id);
                DB.AddParam(cmd, "@UserName", 150, Membership.GetUser().UserName);
                cmd.Prepare();
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);
                }

                foreach (DataRow rdr in ds.Tables[0].Rows)
                {
                    DataRow row = DTIssueActivity.Table.NewRow();
                    row["ID"] = rdr["IndexId"];
                    row["Process"] = rdr["Process"];
                    row["Comment"] = rdr["Comment"];
                    row["YaziRengi"] = rdr["YaziRengi"];
                    row["DurumName"] = rdr["DurumName"];
                    row["CreatedBy"] = rdr["CreatedBy"];
                    row["ModifiedBy"] = rdr["ModifiedBy"];
                    row["CommentDate"] = rdr["CommentDate"];
                    DTIssueActivity.Table.Rows.Add(row);
                }

                DTIssueActivity.Table.AcceptChanges();
                #endregion

                #region// ilgililer listesi Doluyor
                DTIssueUsers.Table.Rows.Clear();

                foreach (DataRow rdr in ds.Tables[1].Rows)
                {
                    //DataRow[] rows = DTIssueUsers.Table.Select("UserName='" + rdr["UserName"].ToString() + "'");
                    //if (rows.Length > 0) continue;
                    DataRow row = DTIssueUsers.Table.NewRow();
                    row["ID"] = rdr["IndexId"];
                    row["UserId"] = rdr["UserId"];
                    row["IssueAntivirus"] = rdr["IssueAntivirus"];
                    row["UserName"] = rdr["UserName"].ToString();
                    row["ProjeName"] = rdr["ProjeName"].ToString();
                    row["FirmaName"] = rdr["FirmaName"].ToString();
                    DTIssueUsers.Table.Rows.Add(row);
                }
                DTIssueUsers.Table.AcceptChanges();
                #endregion
            }


        }

    }

    protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "Comment")
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

    protected void grid2_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (String.IsNullOrEmpty(e.Parameters)) return;
        string[] spt = e.Parameters.Split('|');
        switch (spt[0])
        {
            case "Select":
                if (Convert.ToBoolean(spt[1]))
                    this.grid2.Selection.SelectAll();
                else
                    this.grid2.Selection.UnselectAll();
                break;
            case "deleteuser":
                DeleteUsers();
                break;
            case "MakeDontDo":
                DontMakeUsers();
                break;

        }


    }
}
