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
using System.Collections.Generic;

public partial class CRM_Genel_IsPlan_edit2 : System.Web.UI.Page
{
    DataTable dt = new DataTable();
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
        if (IsPostBack || IsCallback)
            return;

        if (!Security.CheckPermission(this.Context, "IsPlani", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }
        InitGridTable(DataTableList.Table);
        InitGridDTDetail(DTDetail.Table);
        fillcomboxes();
        Guid id;
        if (!String.IsNullOrEmpty((String)Request.QueryString["Tarih1"]))
        {
            string tarih = (String)Request.QueryString["Tarih1"];
            string userId = (String)Request.QueryString["UserID"];
            string OldId = this.Request.Params["id"].Replace("'", "").Trim();

            SqlCommand cmd = DB.SQL(this.Context, "Select BildirimPlanID From BildirimPlanlari Where UserID=@UserID And Tarih1=@Tarih1");
            id = new Guid(userId);
            DB.AddParam(cmd, "@UserID", id);
            Tarih1.Text = tarih;
            DB.AddParam(cmd, "@Tarih1", Tarih1.Date);
            IDataReader rdr = cmd.ExecuteReader();

            if (!rdr.Read())
            {
                id = new Guid(OldId);
                this.HiddenID.Value = id.ToString();
                LoadDocument(id);
                rdr.Close();
                return;
            }

            id = new Guid(rdr["BildirimPlanID"].ToString());
            this.HiddenID.Value = id.ToString();
            LoadDocument(id);
            rdr.Close();
            return;
        }
        else
        {
            string sID = this.Request.Params["id"].Replace("'", "").Trim();
            if ((sID != null) && (sID != "0"))
            {
                id = new Guid(sID);
                this.HiddenID.Value = id.ToString();
                LoadDocument(id);

            }
        }
        this.UserID.Enabled = false;

    }

    private Guid GetUserID()
    {
        SqlCommand cmd = DB.SQL(this.Context, "Select UserID From SecurityUsers Where UserName=@UserName");
        DB.AddParam(cmd, "@UserName", 100, Membership.GetUser().UserName);
        cmd.Prepare();
        Guid id = (Guid)cmd.ExecuteScalar();
        return id;
    }

    void fillcomboxes()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT t1.UserID,(ISNULL(t1.UserName,'')+' ['+ISNULL(t1.FirstName,'')+' '+ISNULL(t1.LastName,'')+']') AS UserName FROM SecurityUsers t1 ");
        sb.Append("Left Outer Join aspnet_UsersInRoles t2 On t1.UserID=t2.UserID ");
        sb.Append("Left Outer Join aspnet_Roles t3 On t2.RoleID=t3.RoleID Where t3.RoleName='Model Standart Kullanýcý' ");
        sb.Append("Or t3.RoleName='Administrator' ORDER BY t1.UserName");
        data.BindComboBoxesNoEmpty(this.Context, UserID, sb.ToString(), "UserID", "UserName");
    }

    protected void ASPxMenu1_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("save"))
        {
            if (CrmUtils.ControllToDate(this.Page, Tarih1.Date.ToString()))
            {
                CrmUtils.CreateMessageAlert(this.Page, "Lütfen Tarih seçimi yapýnýz.", "stkey1");
                return;
            }
            if (UserID.SelectedIndex < 0 || UserID.Text == "")
            {
                CrmUtils.CreateMessageAlert(this.Page, "Lütfen kullanýcý seçimi yapýnýz.", "stkey1");
                return;
            }
            grid.UpdateEdit();
            Guid id = SaveDocument();
            if (id != Guid.Empty)
            {
                Session["BelgeID"] = id.ToString();
                this.Response.Write("<script language='javascript'>{ window.location.href='./edit2.aspx?id=" + (String)Session["BelgeID"] + "';}</script>");
            }
            else
            {
                CrmUtils.CreateMessageAlert(this.Page, (String)Session["Hata"], "stkey1");
                return;
            }
        }
    }

    private Guid SaveDocument()
    {
        try
        {
            DB.BeginTrans(this.Context);

            Guid id = Guid.Empty;
            if (this.HiddenID.Value.Length != 0)
            {
                id = new Guid(this.HiddenID.Value);
            }
            SqlCommand cmd = null;
            StringBuilder sb = new StringBuilder();
            if (id == Guid.Empty)
            {

            }
            else
            {
                #region update
                sb = new StringBuilder();
                sb.Append("UPDATE BildirimPlanlari SET ");
                sb.Append("UserID=@UserID");
                sb.Append(",Description=@Description");
                sb.Append(",Tarih1=@Tarih1");
                sb.Append(",ModifiedBy=@ModifiedBy");
                sb.Append(",ModificationDate=@ModificationDate");
                sb.Append(" WHERE BildirimPlanID=@BildirimPlanID");
                cmd = DB.SQL(this.Context, sb.ToString());
                DB.AddParam(cmd, "@BildirimPlanID", id);
                DB.AddParam(cmd, "@ModifiedBy", 100, Membership.GetUser().UserName);
                DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                #endregion
            }

            #region values
            DB.AddParam(cmd, "@Description", 500, this.Description.Text);
            Guid tempID;
            if (!String.IsNullOrEmpty(UserID.Text))
            {
                tempID = new Guid(UserID.Value.ToString());
                DB.AddParam(cmd, "@UserID", tempID);

            }
            else
                DB.AddParam(cmd, "@UserID", SqlDbType.UniqueIdentifier);
            DB.AddParam(cmd, "@Tarih1", Tarih1.Date);
            #endregion

            cmd.Prepare();
            cmd.ExecuteNonQuery();
            if (id != Guid.Empty)
            {
                #region Detay
                DataTable changes = this.DTDetail.Table.GetChanges();
                if (changes != null)
                {
                    foreach (DataRow row in changes.Rows)
                    {
                        switch (row.RowState)
                        {
                            case DataRowState.Added:
                                break;
                            case DataRowState.Modified:
                                sb = new StringBuilder();
                                sb.Append("Exec Update_BildirimPlanlariDetay @BildirimPlanlariDetayID,@Sirala,@ModifiedBy,@ModificationDate");
                                cmd = DB.SQL(this.Context, sb.ToString());
                                DB.AddParam(cmd, "@BildirimPlanlariDetayID", (Guid)row["ID"]);
                                DB.AddParam(cmd, "@Sirala", (int)row["Sirala"]);
                                DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                                DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                                cmd.Prepare();
                                cmd.ExecuteNonQuery();
                                break;
                            case DataRowState.Deleted:
                                cmd = DB.SQL(this.Context, "DELETE FROM BildirimPlanlariDetay WHERE BildirimPlanlariDetayID=@BildirimPlanlariDetayID");
                                DB.AddParam(cmd, "@BildirimPlanlariDetayID", (Guid)row["BildirimPlanlariDetayID", DataRowVersion.Original]);
                                cmd.Prepare();
                                cmd.ExecuteNonQuery();
                                break;
                        }
                    }
                }
                #endregion

                #region Seçimler
                List<object> keyValues = this.grid.GetSelectedFieldValues("ID");
                foreach (object key in keyValues)
                {
                    DataRow row = DataTableList.Table.Rows.Find(key);
                    sb = new StringBuilder();
                    sb.Append("Exec Insert_BildirimPlanlariDetay @BildirimPlanlariDetayID,@BildirimPlanID,@IssueID,@CreatedBy,@CreationDate");
                    cmd = DB.SQL(this.Context, sb.ToString());
                    DB.AddParam(cmd, "@BildirimPlanID", id);
                    DB.AddParam(cmd, "@IssueID", (Guid)row["IssueID"]);
                    DB.AddParam(cmd, "@BildirimPlanlariDetayID", Guid.NewGuid());
                    DB.AddParam(cmd, "@CreatedBy", 100, Membership.GetUser().UserName);
                    DB.AddParam(cmd, "@CreationDate", DateTime.Now.Date);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                #endregion
            }

            DB.Commit(this.Context);

            return id;
        }
        catch (Exception ex)
        {
            DB.Rollback(this.Context);
            Session["Hata"] = "Hata:" + ex.Message.Replace("'", null);
            return Guid.Empty;
        }


    }

    void LoadDocument(Guid id)
    {
        StringBuilder sb = new StringBuilder();
        SqlCommand cmd;
        IDataReader rdr;
        string UserId = null;

        sb = new StringBuilder();
        sb.Append("Select * From BildirimPlanlari Where BildirimPlanID=@BildirimPlanID");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@BildirimPlanID", id);

        cmd.Prepare();
        rdr = cmd.ExecuteReader();

        if (!rdr.Read())
        {
            this.Response.StatusCode = 500;
            this.Response.End();
            return;
        }
        if ((rdr["UserID"].ToString() != null) && (rdr["UserID"].ToString() != ""))
            this.UserID.SelectedIndex = this.UserID.Items.IndexOfValue(rdr["UserID"].ToString());
        UserId = rdr["UserID"].ToString();
        this.HiddenUserID.Value = UserId;        
        this.Tarih1.Value = rdr["Tarih1"];
        this.HiddenTarih1.Value = this.Tarih1.Date.ToShortDateString();
        this.Description.Text = rdr["Description"].ToString();
        rdr.Close();

        #region// Detay Ekleniyor...
        this.DTDetail.Table.Clear();
        sb = new StringBuilder();
        sb.Append("Select t1.*,t2.IndexID As IndexNo,t2.Baslik, t3.FirmaName, t4.Adi As ProjeAdi, t5.Adi As Durum, t2.KeyWords, ");
        sb.Append("t2.CreatedBy As bCreatedBy, t2.CreationDate As bCreationDate, t2.ModifiedBy As bModifiedBy, t2.ModificationDate As bModificationDate ");
        sb.Append("From BildirimPlanlariDetay t1 Left Outer Join Issue As t2 On t1.IssueID=t2.IssueID ");
        sb.Append("Left Outer Join Firma As t3 On t2.FirmaID=t3.FirmaID ");
        sb.Append("Left Outer Join Proje As t4 On t2.ProjeID=t4.ProjeID ");
        sb.Append("Left Outer Join Durum As t5 On t2.DurumID=t5.DurumID ");
        sb.Append("Where t1.BildirimPlanID=@BildirimPlanID ");
        sb.Append("Order By t1.Sirala");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@BildirimPlanID", id);
        cmd.Prepare();
        rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            DataRow row = this.DTDetail.Table.NewRow();

            row["ID"] = rdr["BildirimPlanlariDetayID"];
            row["IndexID"] = rdr["IndexID"];
            row["IndexNo"] = rdr["IndexNo"];
            row["BildirimPlanlariDetayID"] = rdr["BildirimPlanlariDetayID"];
            row["BildirimPlanID"] = rdr["BildirimPlanID"];
            row["KeyWords"] = rdr["KeyWords"];
            row["IssueID"] = rdr["IssueID"];
            row["Baslik"] = rdr["Baslik"];
            row["IssueID"] = rdr["IssueID"];
            row["Sirala"] = rdr["Sirala"];
            row["FirmaName"] = rdr["FirmaName"];
            row["ProjeAdi"] = rdr["ProjeAdi"];
            row["Durum"] = rdr["Durum"];
            row["CreatedBy"] = rdr["bCreatedBy"];
            row["ModifiedBy"] = rdr["bModifiedBy"];
            row["ModificationDate"] = rdr["bModificationDate"];
            row["CreationDate"] = rdr["bCreationDate"];

            this.DTDetail.Table.Rows.Add(row);
        }
        rdr.Close();

        this.DTDetail.Table.AcceptChanges();
        grid.DataBind();
        #endregion

        #region Seçim Gridi Doluyor
        sb = new StringBuilder();
        sb.Append("SELECT I.*,F.FirmaName,P.Adi AS ProjeName,D.Adi AS DurumName,O.Adi AS OnemDereceName,U.UserName ");
        sb.Append("FROM Issue AS I LEFT OUTER JOIN  Firma AS F ON  I.FirmaID=F.FirmaID ");
        sb.Append("LEFT OUTER JOIN Proje AS P ON I.ProjeID=P.ProjeID ");
        sb.Append("LEFT OUTER JOIN OnemDereceleri AS O ON I.OnemDereceID=O.OnemDereceID ");
        sb.Append("LEFT OUTER JOIN Durum AS D ON I.DurumID=D.DurumID ");
        sb.Append("LEFT OUTER JOIN SecurityUsers AS U ON I.UserID=U.UserID WHERE CONVERT(VarChar(50),I.IssueID)<>'' AND I.Active='1' ");

        sb.Append(" And D.Adi<>'Kapatýldý' And D.Adi<>'Ýptal' And D.Adi<>'Hata Deðil' And D.Adi<>'Tekrar Edilemedi'");

        sb.Append(" And I.UserID=@UserID");

        sb.Append(" ORDER BY I.BildirimTarihi DESC");
        cmd = DB.SQL(this.Context, sb.ToString());
        Guid Userid = new Guid(UserId);
        DB.AddParam(cmd, "@UserID", Userid);
        cmd.Prepare();
        rdr = cmd.ExecuteReader();

        #region FillGrid
        this.DataTableList.Table.Rows.Clear();
        while (rdr.Read())
        {
            DataRow[] rows = DTDetail.Table.Select("IssueID='" + rdr["IssueID"].ToString() + "'");
            if (rows.Length == 0)
            {
                DataRow row = this.DataTableList.Table.NewRow();
                row["ID"] = rdr["IssueID"];
                row["IssueID"] = rdr["IssueID"];
                row["IndexID"] = rdr["IndexID"];
                row["Baslik"] = rdr["Baslik"];
                row["FirmaID"] = rdr["FirmaID"];
                row["FirmaName"] = rdr["FirmaName"];
                row["Description"] = rdr["Description"];
                row["ProjeID"] = rdr["ProjeID"];
                row["ProjeName"] = rdr["ProjeName"];
                row["DurumID"] = rdr["DurumID"];
                row["DurumName"] = rdr["DurumName"];
                row["OnemDereceID"] = rdr["OnemDereceID"];
                row["OnemDereceName"] = rdr["OnemDereceName"];
                row["UserID"] = rdr["UserID"];
                row["UserName"] = rdr["UserName"];
                row["CreatedBy"] = rdr["CreatedBy"];
                row["BildirimTarihi"] = rdr["BildirimTarihi"];
                row["KeyWords"] = rdr["KeyWords"];
                this.DataTableList.Table.Rows.Add(row);

            }
        }

        rdr.Close();
        this.DataTableList.Table.AcceptChanges();
        grid.DataBind();
        #endregion

        #endregion

    }

    protected void Grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters.ToString() == "x")
        {
            Guid id = new Guid(this.UserID.Value.ToString());
            LoadDocument(id);
            return;
        }
        if (Convert.ToBoolean(e.Parameters.ToString()))
            this.grid.Selection.SelectAll();
        else
            this.grid.Selection.UnselectAll();
    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IssueID", typeof(Guid));
        dt.Columns.Add("IndexID", typeof(int));
        dt.Columns.Add("Baslik", typeof(string));
        dt.Columns.Add("FirmaID", typeof(Guid));
        dt.Columns.Add("FirmaName", typeof(string));
        dt.Columns.Add("ProjeID", typeof(Guid));
        dt.Columns.Add("ProjeName", typeof(string));
        dt.Columns.Add("DurumID", typeof(Guid));
        dt.Columns.Add("DurumName", typeof(string));
        dt.Columns.Add("Description", typeof(string));
        dt.Columns.Add("OnemDereceID", typeof(Guid));
        dt.Columns.Add("OnemDereceName", typeof(string));
        dt.Columns.Add("UserID", typeof(Guid));
        dt.Columns.Add("UserName", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("KeyWords", typeof(string));
        dt.Columns.Add("BildirimTarihi", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitGridDTDetail(DataTable dt)
    {

        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IndexID", typeof(int));
        dt.Columns.Add("IndexNo", typeof(int));
        dt.Columns.Add("BildirimPlanID", typeof(Guid));
        dt.Columns.Add("BildirimPlanlariDetayID", typeof(Guid));
        dt.Columns.Add("IssueID", typeof(Guid));
        dt.Columns.Add("Sirala", typeof(int));
        dt.Columns.Add("KeyWords", typeof(string));
        dt.Columns.Add("FirmaName", typeof(string));
        dt.Columns.Add("Baslik", typeof(string));
        dt.Columns.Add("ProjeAdi", typeof(string));
        dt.Columns.Add("Durum", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        dt.Columns.Add("CreationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
        dt.Columns["CreationDate"].DefaultValue = DateTime.Now;
    }
}
