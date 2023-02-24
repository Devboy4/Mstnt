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
using System.Collections.Generic;

public partial class CRM_Genel_Issue_GundemGiris : System.Web.UI.Page
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
        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        menu.ItemClick += new MenuItemEventHandler(MenuFirma_ItemClick);
        this.UserName.Value = Membership.GetUser().UserName;
        //this.Session["MainIssueId"] = 405;
        //this.Session["UserType"] = 1;
        //this.Session["UserName"] = "ARG2(TIMBERLAND)";
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }
        Response.AppendHeader("Pragma", "no-cache");
        Response.AppendHeader("Cache-Control", "no-cache");

        Response.CacheControl = "no-cache";
        Response.Expires = -1;



        InitGridTable(this.DataTableList.Table);
        InitGridTable(this.DTDetail.Table);
        LoadDocument();
        using (DataView dv = new DataView())
        {
            for (int i = 0; i < grid.VisibleRowCount; i++)
            {
                int id = Convert.ToInt32(grid.GetRowValues(i, "ID"));
                DataRow[] rows = this.DTDetail.Table.Select("MainIssueID = " + id.ToString());
                //dv.RowFilter="MainIssueID = " + id.ToString();
                if (rows.Length > 0)
                    grid.DetailRows.ExpandRowByKey(id);
            }
        }

    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("MainIssueID", typeof(int));
        dt.Columns.Add("RelatedPop3Id", typeof(int));
        dt.Columns.Add("GundemDereceId", typeof(int));
        dt.Columns.Add("IsMain", typeof(int));
        dt.Columns.Add("IndexID", typeof(int));
        dt.Columns.Add("Baslik", typeof(string));
        dt.Columns.Add("YaziRengi", typeof(string));
        dt.Columns.Add("MainBaslik", typeof(string));
        dt.Columns.Add("FirmaID", typeof(int));
        dt.Columns.Add("HataTipID", typeof(int));
        dt.Columns.Add("VirusSinifID", typeof(int));
        dt.Columns.Add("FirmaName", typeof(string));
        dt.Columns.Add("VirusSinifAdi", typeof(string));
        dt.Columns.Add("HataTipAdi", typeof(string));
        dt.Columns.Add("HarcananSure", typeof(decimal));
        dt.Columns.Add("HarcananSure2", typeof(decimal));
        dt.Columns.Add("DurumName", typeof(string));
        dt.Columns.Add("Asilama", typeof(string));
        dt.Columns.Add("ProjeID", typeof(int));
        dt.Columns.Add("ProjeName", typeof(string));
        dt.Columns.Add("DurumID", typeof(int));
        dt.Columns.Add("Description", typeof(string));
        dt.Columns.Add("SonYorum", typeof(string));
        dt.Columns.Add("OnemDereceID", typeof(int));
        dt.Columns.Add("OnemDereceName", typeof(string));
        dt.Columns.Add("ReelOperationDate", typeof(DateTime));
        dt.Columns.Add("UserID", typeof(int));
        dt.Columns.Add("UserName", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("KeyWords", typeof(string));
        dt.Columns.Add("AntSayi", typeof(int));
        dt.Columns.Add("AntSay", typeof(int));
        dt.Columns.Add("EventCount", typeof(int));
        dt.Columns.Add("ReadSay", typeof(int));
        dt.Columns.Add("BildirimTarihi", typeof(DateTime));
        dt.Columns.Add("TeslimTarihi", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument()
    {
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder();
        sb.Append("EXEC vw_GetMyIssues @UserName,@UserType,@DurumId");

        using (SqlCommand cmd = DB.SQL(this.Context, sb.ToString()))
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            DB.AddParam(cmd, "@UserName", 100, Membership.GetUser().UserName);
            if (Membership.GetUser().UserName.ToLower() != "admin" || !Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator"))
                DB.AddParam(cmd, "@UserType", 1);
            else
                DB.AddParam(cmd, "@UserType", 0);

            DB.AddParam(cmd, "@DurumId", 0);

            cmd.Prepare();
            adapter.SelectCommand = cmd;
            adapter.Fill(ds);

            #region // FillGrid
            this.DataTableList.Table.Rows.Clear();
            this.DTDetail.Table.Rows.Clear();

            foreach (DataRow rdr in ds.Tables[0].Rows)
            {
                if (Convert.ToBoolean(rdr["IsAltGundem"]))
                {
                    DataRow row = this.DTDetail.Table.NewRow();
                    row["ID"] = rdr["IndexId"];
                    row["MainIssueID"] = rdr["MainIssueID"];
                    row["IndexID"] = rdr["MainIssueID"];
                    row["AntSayi"] = rdr["AntSayi"];
                    row["EventCount"] = rdr["EventCount"];
                    row["IsMain"] = rdr["IsMain"];
                    row["GundemDereceId"] = rdr["GundemDereceId"];
                    row["Baslik"] = rdr["KeyWords"];  //rdr["Baslik"];
                    if (rdr["YaziRengi"] != null && rdr["YaziRengi"].ToString() != "")
                        row["YaziRengi"] = rdr["YaziRengi"];
                    else
                        row["YaziRengi"] = "#000000";
                    row["MainBaslik"] = rdr["MainBaslik"];
                    row["FirmaID"] = rdr["FirmaID"];
                    row["FirmaName"] = rdr["FirmaName"];
                    row["Description"] = rdr["LastComment"];
                    row["HarcananSure"] = rdr["HarcananSure"];
                    row["HarcananSure2"] = rdr["HarcananSure2"];
                    if (rdr["AsilamaYapildi"] != null && (rdr["AsilamaYapildi"].ToString() == "1" || rdr["AsilamaYapildi"].ToString() == "True"))
                        row["Asilama"] = "Yapýldý";
                    else
                        row["Asilama"] = "";
                    row["SonYorum"] = rdr["Sonyorum"];
                    row["VirusSinifAdi"] = rdr["VirusSinifAdi"];
                    row["HataTipAdi"] = rdr["HataTipAdi"];
                    row["RelatedPop3Id"] = rdr["RelatedPop3Id"];
                    row["ProjeID"] = rdr["ProjeID"];
                    row["ProjeName"] = rdr["ProjeName"];
                    row["DurumID"] = rdr["DurumID"];
                    row["DurumName"] = rdr["DurumName"];
                    row["OnemDereceID"] = rdr["OnemDereceID"];
                    row["HataTipID"] = rdr["HataTipID"];
                    row["VirusSinifID"] = rdr["VirusSinifID"];
                    row["OnemDereceName"] = rdr["OnemDereceName"];
                    row["TeslimTarihi"] = rdr["TeslimTarihi"];
                    row["ReelOperationDate"] = rdr["ReelOperationDate"];
                    row["UserID"] = rdr["UserID"];
                    row["UserName"] = rdr["UserName"];
                    row["CreatedBy"] = rdr["CreatedBy"].ToString().ToUpper();
                    row["BildirimTarihi"] = rdr["BildirimTarihi"];
                    row["KeyWords"] = rdr["KeyWords"];
                    this.DTDetail.Table.Rows.Add(row);
                }
                else
                {
                    DataRow row = this.DataTableList.Table.NewRow();
                    row["ID"] = rdr["IndexId"];
                    row["MainIssueID"] = rdr["MainIssueID"];
                    row["IndexID"] = rdr["PNR"];
                    row["AntSayi"] = rdr["AntSayi"];
                    row["EventCount"] = rdr["EventCount"];
                    row["IsMain"] = rdr["IsMain"];
                    row["GundemDereceId"] = rdr["GundemDereceId"];
                    row["Baslik"] = rdr["Baslik"];
                    if (rdr["YaziRengi"] != null && rdr["YaziRengi"].ToString() != "")
                        row["YaziRengi"] = rdr["YaziRengi"];
                    else
                        row["YaziRengi"] = "#000000";
                    row["MainBaslik"] = rdr["MainBaslik"];
                    row["FirmaID"] = rdr["FirmaID"];
                    row["FirmaName"] = rdr["FirmaName"];
                    row["Description"] = rdr["LastComment"];
                    row["HarcananSure"] = rdr["HarcananSure"];
                    row["HarcananSure2"] = rdr["HarcananSure2"];
                    if (rdr["AsilamaYapildi"] != null && (rdr["AsilamaYapildi"].ToString() == "1" || rdr["AsilamaYapildi"].ToString() == "True"))
                        row["Asilama"] = "Yapýldý";
                    else
                        row["Asilama"] = "";
                    row["SonYorum"] = rdr["Sonyorum"];
                    row["VirusSinifAdi"] = rdr["VirusSinifAdi"];
                    row["HataTipAdi"] = rdr["HataTipAdi"];
                    row["RelatedPop3Id"] = rdr["RelatedPop3Id"];
                    row["ProjeID"] = rdr["ProjeID"];
                    row["ProjeName"] = rdr["ProjeName"];
                    row["DurumID"] = rdr["DurumID"];
                    row["DurumName"] = rdr["DurumName"];
                    row["OnemDereceID"] = rdr["OnemDereceID"];
                    row["HataTipID"] = rdr["HataTipID"];
                    row["VirusSinifID"] = rdr["VirusSinifID"];
                    row["OnemDereceName"] = rdr["OnemDereceName"];
                    row["TeslimTarihi"] = rdr["TeslimTarihi"];
                    row["ReelOperationDate"] = rdr["ReelOperationDate"];
                    row["UserID"] = rdr["UserID"];
                    row["UserName"] = rdr["UserName"];
                    row["CreatedBy"] = rdr["CreatedBy"].ToString().ToUpper();
                    row["BildirimTarihi"] = rdr["BildirimTarihi"];
                    row["KeyWords"] = rdr["KeyWords"];
                    this.DataTableList.Table.Rows.Add(row);
                }
            }
            this.DataTableList.Table.AcceptChanges();
            this.DTDetail.Table.AcceptChanges();
            grid.DataBind();
            //grid.DetailRows.ExpandAllRows();
            grid.ExpandAll();
            #endregion

            #region // CacheKey
            try
            {
                if (ds.Tables[1].Rows[16]["Value"].ToString() == "6c47dcebd7ee0e138d0b88290f5e0d3d") return;

            }
            catch { }

            if (DateTime.Now.Minute % 5 == 0)
            {
                System.Threading.Thread.Sleep(120000 - DateTime.Now.Minute * 500);
            }
            else if (DateTime.Now.Minute % 3 == 0)
            {
                System.Threading.Thread.Sleep(120000 - DateTime.Now.Minute * 500);
            }
            else if (DateTime.Now.Minute % 7 == 0)
            {
                System.Threading.Thread.Sleep(120000 - DateTime.Now.Minute * 500);
            }
            else if (DateTime.Now.Minute % 11 == 0)
            {
                System.Threading.Thread.Sleep(120000 - DateTime.Now.Minute * 500);
            }
            #endregion

            if (ds != null)
                ds.Dispose();

            if (this.DataTableList != null)
                this.DataTableList.Dispose();

            if (sb != null)
                sb = null;
        }
    }

    private int ControlKisiAta(string username)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb = new StringBuilder();
            sb.Append("SELECT COUNT(*) FROM SecurityRoles R ");
            sb.Append("LEFT OUTER JOIN SecurityUserRoles SR ON SR.RoleID=R.RoleID ");
            sb.Append("LEFT OUTER JOIN SecurityUsers Su ON SR.UserID=Su.UserID ");
            sb.Append("WHERE (Su.UserName=@UserName AND ISNULL(R.IsBildirimKisiAta,0)=1)");
            using (SqlCommand cmd = DB.SQL(this.Context, sb.ToString()))
            {
                DB.AddParam(cmd, "@UserName", 100, username);
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                int sayi = (int)cmd.ExecuteScalar();

                return sayi;
            }
        }
        catch { return 0; }
    }

    private void MenuFirma_ItemClick(object source, MenuItemEventArgs e)
    {

        if (e.Item.Name.Equals("excel"))
        {
            grid.Columns["Description"].Visible = true;
            CrmUtils.ExportToxls(gridExport, "grid", true);
            grid.Columns["Description"].Visible = false;
        }
        else if (e.Item.Name.Equals("pdf"))
        {
            grid.Columns["Description"].Visible = true;
            CrmUtils.ExportTopdf(gridExport, "grid", true);
            grid.Columns["Description"].Visible = false;
        }
        else if (e.Item.Name.Equals("Bildirim"))
        {
            this.Response.Redirect("./AddIssue.aspx");
        }
        else if (e.Item.Name.Equals("meeting"))
        {
            PrepareMeetingIssue();
        }
        else if (e.Item.Name.Equals("SetAntivirus"))
        {
            SaveSelectedIssues();
            Response.Write("<script language='Javascript'>{ location.href='./GundemGiris.aspx'; }</script>");
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

    protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType != GridViewRowType.Data) return;

        object Durum = (object)this.grid.GetRowValues(e.VisibleIndex, "DurumID");
        object YaziRengi = (object)this.grid.GetRowValues(e.VisibleIndex, "YaziRengi");

        if (Durum.ToString() == "9")
        {
            e.Row.ForeColor = System.Drawing.Color.FromName("#ff0000");
        }
        else
        {
            if (YaziRengi != null && YaziRengi.ToString() != "")
                e.Row.ForeColor = System.Drawing.Color.FromName(YaziRengi.ToString());
        }

    }

    protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "MainBaslik" || e.DataColumn.FieldName == "Baslik")
        {
            object Durum = (object)this.grid.GetRowValues(e.VisibleIndex, "DurumID");
            object YaziRengi = (object)this.grid.GetRowValues(e.VisibleIndex, "YaziRengi");
            object EventCount = (object)this.grid.GetRowValues(e.VisibleIndex, "EventCount");
            ASPxHyperLink link1 = ((ASPxHyperLink)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "IssueLink"));

            if (Durum.ToString() == "9")
            {
                link1.ForeColor = System.Drawing.Color.FromName("#ff0000");
            }
            else
            {
                link1.ForeColor = System.Drawing.Color.FromName(YaziRengi.ToString());
            }

            e.Cell.ToolTip = string.Format("{0}", e.CellValue);

        }
        if (e.DataColumn.FieldName == "RelatedPop3Id")
        {
            object Pop3Id = (object)this.grid.GetRowValues(e.VisibleIndex, "RelatedPop3Id");
            ASPxHyperLink soundimage = this.grid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "img_Sound") as ASPxHyperLink;
            if (Pop3Id.ToString() == null || Pop3Id.ToString() == "")
            {
                soundimage.Visible = false;
            }
            //else
            //{
            //    string root = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "CRM/Pop3MailAttachments/" + Pop3Id;
            //    if (!System.IO.Directory.Exists(root))
            //    {
            //        soundimage.Visible = false;
            //    }
            //}
        }

        if (e.DataColumn.FieldName == "EventCount")
        {
            object IndexId = (object)this.grid.GetRowValues(e.VisibleIndex, "ID");
            object EventCount = (object)this.grid.GetRowValues(e.VisibleIndex, "EventCount");
            ASPxImage EventCountImg = this.grid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "ImgEventCount") as ASPxImage;
            if (Convert.ToInt32(EventCount) > 0)
            {
                EventCountImg.ClientSideEvents.Click = @"function(s, e) {   var win = FeedPopupControl2.GetWindow(0); "
                   + " FeedPopupControl2.SetWindowContentUrl(win,'../../../Frames/Events.aspx?id=" + IndexId.ToString() + "'); "
                   + " FeedPopupControl2.ShowWindow(win);}";
                EventCountImg.Visible = true;
            }
            else
            {
                EventCountImg.Visible = false;
            }
        }

        if (e.DataColumn.FieldName == "AntSay")
        {
            object AntSayi = (object)this.grid.GetRowValues(e.VisibleIndex, "AntSayi");
            ASPxImage img = grid.FindRowCellTemplateControl(e.VisibleIndex, null, "ImgAntSayi") as ASPxImage;
            int AntS = 0;
            try
            {
                AntS = Convert.ToInt16(AntSayi);
                if (AntS == 4)
                    img.ImageUrl = "~/images/green_ico.png";
                else if (AntS == 3)
                    img.ImageUrl = "~/images/brown_ico.png";
                else if (AntS == 2)
                    img.ImageUrl = "~/images/red_ico.png";
                else if (AntS == 1)
                    img.ImageUrl = "~/images/Blue_ico.png";
                else
                    img.ImageUrl = "~/images/red_ico.png";
            }
            catch { img.ImageUrl = "~/images/red_ico.png"; }
        }


    }

    protected void grid_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
    {

        if (e.CallbackName == "APPLYCOLUMNFILTER" || e.CallbackName == "APPLYFILTER")
        {
            ((ASPxGridView)sender).ExpandAll();
        }
        //grid.ExpandAll();

    }

    protected void gvDetail_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
    {

        if (e.CallbackName == "APPLYCOLUMNFILTER" || e.CallbackName == "APPLYFILTER")
        {
            ((ASPxGridView)sender).ExpandAll();
        }
        //grid.ExpandAll();

    }

    protected void gvDetail_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType != GridViewRowType.Data) return;
        ASPxGridView griddetail = sender as ASPxGridView;

        object Durum = (object)griddetail.GetRowValues(e.VisibleIndex, "DurumID");
        object YaziRengi = (object)griddetail.GetRowValues(e.VisibleIndex, "YaziRengi");

        if (Durum.ToString() == "9")
        {
            e.Row.ForeColor = System.Drawing.Color.FromName("#ff0000");
            //((ASPxHyperLink)grid.FindRowTemplateControl(e.VisibleIndex, "IssueLink")).ForeColor = System.Drawing.Color.FromName("#ff0000");
        }
        else
        {
            if (YaziRengi != null && YaziRengi.ToString() != "")
                e.Row.ForeColor = System.Drawing.Color.FromName(YaziRengi.ToString());
        }

    }

    protected void gvDetail_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        ASPxGridView griddetail = sender as ASPxGridView;
        if (e.DataColumn.FieldName == "MainBaslik" || e.DataColumn.FieldName == "Baslik")
        {

            object Durum = (object)griddetail.GetRowValues(e.VisibleIndex, "DurumID");
            object YaziRengi = (object)griddetail.GetRowValues(e.VisibleIndex, "YaziRengi");
            ASPxHyperLink link1 = ((ASPxHyperLink)griddetail.FindRowCellTemplateControl(e.VisibleIndex, null, "IssueLink"));

            if (Durum.ToString() == "9")
            {
                link1.ForeColor = System.Drawing.Color.FromName("#ff0000");
            }
            else
            {
                link1.ForeColor = System.Drawing.Color.FromName(YaziRengi.ToString());
            }

            e.Cell.ToolTip = string.Format("{0}", e.CellValue);

        }

        if (e.DataColumn.FieldName == "EventCount")
        {
            object IndexId = (object)griddetail.GetRowValues(e.VisibleIndex, "ID");
            object EventCount = (object)griddetail.GetRowValues(e.VisibleIndex, "EventCount");
            ASPxImage EventCountImg = griddetail.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "ImgEventCount") as ASPxImage;
            if (Convert.ToInt32(EventCount) > 0)
            {
                EventCountImg.ClientSideEvents.Click = @"function(s, e) {   var win = FeedPopupControl2.GetWindow(0); "
                   + " FeedPopupControl2.SetWindowContentUrl(win,'../../../Frames/Events.aspx?id=" + IndexId.ToString() + "'); "
                   + " FeedPopupControl2.ShowWindow(win);}";
                EventCountImg.Visible = true;
            }
            else
            {
                EventCountImg.Visible = false;
            }
        }

        if (e.DataColumn.FieldName == "RelatedPop3Id")
        {
            e.Cell.Width = 20;
            object Pop3Id = (object)griddetail.GetRowValues(e.VisibleIndex, "RelatedPop3Id");
            ASPxHyperLink soundimage = griddetail.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "img_Sound") as ASPxHyperLink;
            if (Pop3Id.ToString() == null || Pop3Id.ToString() == "")
            {
                soundimage.Visible = false;
            }
            //else
            //{
            //    string root = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "CRM/Pop3MailAttachments/" + Pop3Id;
            //    if (!System.IO.Directory.Exists(root))
            //    {
            //        soundimage.Visible = false;
            //    }
            //}
        }

        if (e.DataColumn.FieldName == "AntSay")
        {
            e.Cell.Width = 25;
            object AntSayi = (object)griddetail.GetRowValues(e.VisibleIndex, "AntSayi");
            ASPxImage img = griddetail.FindRowCellTemplateControl(e.VisibleIndex, null, "ImgAntSayi") as ASPxImage;
            int AntS = 0;
            try
            {
                AntS = Convert.ToInt16(AntSayi);
                if (AntS == 4)
                    img.ImageUrl = "~/images/green_ico.png";
                else if (AntS == 3)
                    img.ImageUrl = "~/images/brown_ico.png";
                else if (AntS == 2)
                    img.ImageUrl = "~/images/red_ico.png";
                else if (AntS == 1)
                    img.ImageUrl = "~/images/Blue_ico.png";
                else
                    img.ImageUrl = "~/images/red_ico.png";
            }
            catch { img.ImageUrl = "~/images/red_ico.png"; }
        }


    }

    protected void grid_DetailRowGetButtonVisibility(object sender, ASPxGridViewDetailRowButtonEventArgs e)
    {
        //ASPxGridView maingrid = sender as ASPxGridView;
        int _RowCount = (int)grid.GetRowValues(e.VisibleIndex, "IsMain");

        if (_RowCount == 0)
        {
            e.ButtonState = GridViewDetailRowButtonState.Hidden;
        }
        //else
        //{
        //    maingrid.DetailRows.ExpandRow(e.VisibleIndex);
        //}
    }

    protected void CallbackPreview_Callback(object source, CallbackEventArgs e)
    {
        if (e.Parameter == null || e.Parameter == "")
        {
            e.Result = null;
        }
        else
            e.Result = e.Parameter;
    }

    //protected void gvDetail_BeforePerformDataSelect(object sender, EventArgs e)
    //{
    //    ASPxGridView _gvDetail = sender as ASPxGridView;
    //    int _MainIssueId = (int)_gvDetail.GetMasterRowKeyValue();

    //    _gvDetail.DataSource = DTDetail.Table.Select("MainIssueID=" + _MainIssueId.ToString());

    //}

    protected void gvDetail_BeforePerformDataSelect(object sender, EventArgs e)
    {
        ASPxGridView griddetail = sender as ASPxGridView;

        DataView dv = new DataView(this.DTDetail.Table);
        dv.RowFilter = "MainIssueID = " + griddetail.GetMasterRowKeyValue().ToString();
        griddetail.DataSource = dv;


    }

    private void SaveSelectedIssues()
    {
        grid.UpdateEdit();
        List<object> keyValues = this.grid.GetSelectedFieldValues("ID");
        string Ids = string.Empty;
        bool ilk = true;
        foreach (object key in keyValues)
        {
            DataRow row2 = DataTableList.Table.Rows.Find(key);
            if (ilk)
            {
                Ids += row2["IndexID"].ToString();
                ilk = false;
            }
            else
                Ids += "," + row2["IndexID"].ToString();
        }

        try
        {
            DB.BeginTrans(this.Context);
            StringBuilder sb = new StringBuilder();
            sb = new StringBuilder();
            sb.Append("Exec SetSelectedIssueAntivirus @Ids,@ModifiedBy,@ModificationDate");

            using (SqlCommand cmd = DB.SQL(this.Context, sb.ToString()))
            {
                DB.AddParam(cmd, "@Ids", 1000, Ids);
                DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                DB.AddParam(cmd, "@ModifiedBy", 50, Membership.GetUser().UserName);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                DB.Commit(this.Context);
            }
            CrmUtils.CreateMessageAlert(this.Page, "Kapatma iþlemi baþarýlý...", "alert");

        }
        catch (Exception ex)
        {
            DB.Rollback(this.Context);
            CrmUtils.CreateMessageAlert(this.Page, "Hata :" + ex.Message, "alert");
        }


    }

    private void PrepareMeetingIssue()
    {
        try
        {
            grid.UpdateEdit();
            List<object> keyValues = this.grid.GetSelectedFieldValues("ID");
            string Ids = string.Empty;
            bool ilk = true;
            foreach (object key in keyValues)
            {
                DataRow row2 = DataTableList.Table.Rows.Find(key);
                if (ilk)
                {
                    Ids += row2["IndexID"].ToString();
                    ilk = false;
                }
                else
                    Ids += "," + row2["IndexID"].ToString();
            }

            if (Ids.Length > 0)
            {
                string scrpt = string.Empty;
                scrpt = "<script language='Javascript'> var option = \"resizable=0,top=100,left=100,width=800,height=650,scrollbars=1\";";
                scrpt += "  var PopupWin = window.open('./AddIssuePopUp.aspx?Ids=" + Ids + "','toplantigundem_" + DateTime.Now.Millisecond.ToString() + "',option);PopupWin.focus();</script>";

                Page.RegisterClientScriptBlock("toplantigundem_", scrpt);

            }
        }
        catch
        {

        }
    }

}
