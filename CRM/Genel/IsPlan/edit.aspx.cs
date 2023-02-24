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

public partial class CRM_Genel_IsPlan_edit : System.Web.UI.Page
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

        InitGridTable(this.DataTableList.Table);

        this.HiddenID.Value = Guid.NewGuid().ToString();
        Guid id;
        if (Session["PlanUserID"] != null && (String)Session["PlanUserID"] != "")
            id = new Guid((String)Session["PlanUserID"]);
        else
            id = GetUserID();

        fillcomboxes();

        this.UserID.SelectedIndex = this.UserID.Items.IndexOfValue(id.ToString().ToLower());

        LoadDocument(id);

        this.Tarih1.Date = DateTime.Now.Date;

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

    private Guid GetUserID()
    {
        SqlCommand cmd = DB.SQL(this.Context, "Select UserID From SecurityUsers Where UserName=@UserName");
        DB.AddParam(cmd, "@UserName", 100, Membership.GetUser().UserName);
        cmd.Prepare();
        Guid id = (Guid)cmd.ExecuteScalar();
        Session["PlanUserID"] = id.ToString();
        return id;
    }

    protected void ASPxMenu1_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("save"))
        {
            SqlCommand cmd = DB.SQL(this.Context, "Select Count(*) From BildirimPlanlari Where Year(Tarih1)=Year(GetDate()) And Month(Tarih1)=Month(GetDate()) And day(Tarih1)=day(GetDate()) And UserID=@UserID");
            Guid id = new Guid((String)Session["PlanUserID"]);
            DB.AddParam(cmd, "@UserID", id);
            cmd.Prepare();
            int sayi = (int)cmd.ExecuteScalar();
            if (sayi != 0)
            {
                CrmUtils.CreateMessageAlert(this.Page, "Ayný güne 2 bildirim planý giremezsiniz!", "stkey1");
                return;
            }
            if (SaveDocument())
            {
                Response.Write("<script language='Javascript1.2'>{ parent.opener.cmbUserID.SetValue('" + this.UserID.Value.ToString() + "');parent.opener.Grid.PerformCallback('" + this.UserID.Value.ToString() + "'); }</script>");
                Response.Write("<script language='Javascript1.2'>{ parent.close(); }</script>");
            }
            else
            {
                CrmUtils.CreateMessageAlert(this.Page, (String)Session["Hata"], "stkey1");
                return;
            }
        }
    }

    private bool SaveDocument()
    {
        try
        {
            if (CrmUtils.ControllToDate(this.Page, Tarih1.Date.ToString()))
            {
                Session["Hata"] = "Lütfen Tarih seçimi yapýnýz.";
                return false;
            }
            if (UserID.SelectedIndex < 0 || UserID.Text == "")
            {
                Session["Hata"] = "Lütfen kullanýcý seçimi yapýnýz.";
                return false;
            }
            DB.BeginTrans(this.Context);
            grid.UpdateEdit();
            SqlCommand cmd;
            Guid tempID;
            StringBuilder sb = new StringBuilder();
            sb = new StringBuilder();
            sb.Append("Insert Into BildirimPlanlari (BildirimPlanID,UserID,Tarih1,Description,CreatedBy,CreationDate) ");
            sb.Append("Values(@BildirimPlanID,@UserID,@Tarih1,@Description,@CreatedBy,@CreationDate)");
            cmd = DB.SQL(this.Context, sb.ToString());
            tempID = new Guid(this.HiddenID.Value.ToString());
            DB.AddParam(cmd, "@BildirimPlanID", tempID);
            tempID = new Guid(this.UserID.Value.ToString());
            DB.AddParam(cmd, "@UserID", tempID);
            DB.AddParam(cmd, "@Tarih1", Tarih1.Date);
            DB.AddParam(cmd, "@Description", 500, Description.Text);
            DB.AddParam(cmd, "@CreatedBy", 100, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@CreationDate", DateTime.Now.Date);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            List<object> keyValues = this.grid.GetSelectedFieldValues("ID");
            foreach (object key in keyValues)
            {
                DataRow row = DataTableList.Table.Rows.Find(key);
                sb = new StringBuilder();
                sb.Append("Exec Insert_BildirimPlanlariDetay @BildirimPlanlariDetayID,@BildirimPlanID,@IssueID,@CreatedBy,@CreationDate");
                cmd = DB.SQL(this.Context, sb.ToString());
                tempID = new Guid(this.HiddenID.Value.ToString());
                DB.AddParam(cmd, "@BildirimPlanID", tempID);
                DB.AddParam(cmd, "@IssueID", (Guid)row["IssueID"]);
                DB.AddParam(cmd, "@BildirimPlanlariDetayID", Guid.NewGuid());
                DB.AddParam(cmd, "@CreatedBy", 100, Membership.GetUser().UserName);
                DB.AddParam(cmd, "@CreationDate", DateTime.Now.Date);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            DB.Commit(this.Context);
            return true;
        }
        catch (Exception ex)
        {
            DB.Rollback(this.Context);
            Session["Hata"] = "Hata:" + ex.Message.Replace("'", null);
            return false;
        }
    }

    void LoadDocument(Guid id)
    {
        StringBuilder sb = new StringBuilder();
        Session["PlanUserID"] = id.ToString();
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
        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@UserID", id);
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();

        #region FillGrid
        this.DataTableList.Table.Rows.Clear();
        while (rdr.Read())
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
        rdr.Close();
        this.DataTableList.Table.AcceptChanges();
        grid.DataBind();
        #endregion
    }

    protected void Grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        try
        {
            if (Convert.ToBoolean(e.Parameters.ToString()))
                this.grid.Selection.SelectAll();
            else
                this.grid.Selection.UnselectAll();
            return;
        }
        catch
        {
            Guid id = new Guid(e.Parameters.ToString());
            LoadDocument(id);
            return;
        }
    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IssueID", typeof(Guid));
        dt.Columns.Add("IndexID", typeof(int));
        dt.Columns.Add("Index", typeof(int));
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
}
