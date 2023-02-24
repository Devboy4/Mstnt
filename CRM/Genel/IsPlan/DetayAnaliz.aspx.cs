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

public partial class CRM_Genel_IsPlan_DetayAnaliz : System.Web.UI.Page
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

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }

        if (!Security.CheckPermission(this.Context, "IsPlaniAnaliz", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        InitGridTable(DTList.Table);

        Guid id = GetUserID();

        HiddenUserID.Value = id.ToString();

        Session["PlanUserID"] = id.ToString();

        //LoadDocument(id);

        fillcomboxes();

        this.UserID.SelectedIndex = this.UserID.Items.IndexOfValue(id.ToString().ToLower());
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

    protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters.ToString() == "x")
        {
            Guid id = new Guid(this.UserID.Value.ToString());
            LoadDocument(id);
            return;
        }
    }

    private void InitGridTable(DataTable dt)
    {

        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IndexID", typeof(int));
        dt.Columns.Add("IssueID", typeof(Guid));
        dt.Columns.Add("Baslik", typeof(string));
        dt.Columns.Add("Proje", typeof(string));
        dt.Columns.Add("Sayi", typeof(int));
        dt.Columns.Add("Template", typeof(string));
        dt.Columns.Add("BildirimDurumu", typeof(string));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument(Guid id)
    {
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder();
        Session["PlanUserID"] = id.ToString();
        sb.Append("Exec GetUserPlanAnaliz @UserID,@Tarih1,@Tarih2");

        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());

        #region Values
        DB.AddParam(cmd, "@UserID", id);
        if (!CrmUtils.ControllToDate(this.Page, this.Tarih1.Date.ToString()))
            DB.AddParam(cmd, "@Tarih1", this.Tarih1.Date);
        else
            DB.AddParam(cmd, "@Tarih1", SqlDbType.Int);
        if (!CrmUtils.ControllToDate(this.Page, this.Tarih2.Date.ToString()))
            DB.AddParam(cmd, "@Tarih2", this.Tarih2.Date);
        else
            DB.AddParam(cmd, "@Tarih2", SqlDbType.Int); 
        #endregion

        cmd.Prepare();
        this.DTList.Table.Clear();
        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTList.Table.NewRow();

            row["ID"] = rdr["IssueID"];
            row["IndexID"] = rdr["IndexID"];
            row["IssueID"] = rdr["IssueID"];
            row["Baslik"] = rdr["Baslik"];
            row["Proje"] = rdr["Proje"];
            row["Sayi"] = rdr["Sayi"];
            row["Template"] = rdr["Template"];
            row["BildirimDurumu"] = rdr["BildirimDurumu"];

            this.DTList.Table.Rows.Add(row);
        }
        rdr.Close();

        this.DTList.Table.AcceptChanges();

        grid.DataBind();

    }

}
