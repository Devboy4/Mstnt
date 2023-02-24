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
using Model.Crm;
using DevExpress.Web.ASPxGridView;

public partial class CRM_SearchBrowser2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsPostBack || this.IsCallback) return;

        this.DTGrid.Table.Clear();
        if (this.Context.Session["SearchBrowser"] != null)
        {
            Hashtable htSearchBrowser = (Hashtable)this.Context.Session["SearchBrowser"];
            if (htSearchBrowser.ContainsKey("Query"))
            {
                string Title = (string)htSearchBrowser["Title"];
                DataTable dtQuery = (DataTable)htSearchBrowser["Query"];
                DataTable dtParameters = (DataTable)htSearchBrowser["Parameters"];
                DataTable dtFields = (DataTable)htSearchBrowser["Fields"];
                string KeyField = (string)htSearchBrowser["KeyField"];

                DataTable dtConnection = null;
                bool UseWebServis = false;
                string WebServisUrl = null;
                if (htSearchBrowser.ContainsKey("dtConnection")) dtConnection = (DataTable)htSearchBrowser["dtConnection"];
                if (htSearchBrowser.ContainsKey("UseWebServis")) UseWebServis = (bool)htSearchBrowser["UseWebServis"];
                if (htSearchBrowser.ContainsKey("WebServisUrl")) WebServisUrl = (string)htSearchBrowser["WebServisUrl"];

                this.Page.Title = Title;

                if (UseWebServis)
                {
                    #region erp dbs
                    //if (!String.IsNullOrEmpty(WebServisUrl))
                    //{
                    //    DataSet dsSql = new DataSet();
                    //    if (dtConnection != null) dsSql.Tables.Add(dtConnection);
                    //    dsSql.Tables.Add(dtQuery);
                    //    dsSql.Tables.Add(dtParameters);
                    //    CenterWS.DBService db = new CenterWS.DBService();
                    //    DataSet ds = db.Select(WebServisUrl, dsSql);
                    //    this.DTGrid.Table = ds.Tables[0];
                    //}
                    #endregion
                }
                else
                {
                    #region portal db
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < dtQuery.Rows.Count; i++) sb.Append(dtQuery.Rows[i][0]);
                    SqlConnection conn = DB.Connect(this.Context);
                    SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                    cmd.Parameters.Clear();
                    #region parameters
                    if (dtParameters != null)
                    {
                        foreach (DataRow row in dtParameters.Rows)
                        {
                            switch (row["type"].ToString().ToUpper())
                            {
                                case "VARCHAR":
                                    DB.AddParam(cmd, (string)row["name"], (int)row["size"], (string)row["value"]);
                                    break;
                                case "INT":
                                case "ÝNT":
                                    DB.AddParam(cmd, (string)row["name"], (int)row["value"]);
                                    break;
                                case "DATETIME":
                                case "DATETÝME":
                                    DB.AddParam(cmd, (string)row["name"], (DateTime)row["value"]);
                                    break;
                                case "DECIMAL":
                                case "DECÝMAL":
                                    DB.AddParam(cmd, (string)row["name"], (Decimal)row["value"]);
                                    break;
                                case "GUID":
                                case "GUÝD":
                                    DB.AddParam(cmd, (string)row["name"], (Guid)row["value"]);
                                    break;
                            }
                        }
                    }
                    #endregion
                    cmd.CommandTimeout = 1000;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(this.DTGrid.Table);
                    #endregion
                }

                #region fill grid
                GridViewDataColumn GridColumn = new GridViewDataColumn();
                int index = 0;
                foreach (DataColumn col in this.DTGrid.Table.Columns)
                {
                    GridColumn = new GridViewDataColumn();
                    GridColumn.FieldName = col.ColumnName;
                    if (col.ColumnName == KeyField)
                    {
                        GridColumn.Visible = false;
                        this.Grid.KeyFieldName = KeyField;
                    }
                    else
                        GridColumn.Visible = true;
                    this.Grid.Columns.Add(GridColumn);
                    GridColumn.Caption = (string)dtFields.Rows[index]["caption"];
                    GridColumn.Visible = (bool)dtFields.Rows[index]["visible"];
                    GridColumn.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                    //GridColumn.Width = (int)dtFields.Rows[index]["width"];
                    index++;
                }
                this.Grid.DataBind();
                #endregion
            }
        }
        this.Context.Session["SearchBrowser"] = null;
    }

    protected void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        e.Cell.ToolTip = string.Format("{0}", e.CellValue);
    }
}
