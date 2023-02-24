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
using DevExpress.Web.ASPxClasses;
using System.Data;
using System;

public partial class CRM_Settings_VirusSinif_edit : System.Web.UI.Page
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
        if (IsPostBack || IsCallback)
            return;

        if (!Security.CheckPermission(this.Context, "Tanım - Virüs Sınıfları", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayı görme yetkisine sahip değilsiniz!</p></body></html>");
            this.Response.End();
        }

        InitGridTable(this.DataTableList.Table);
        fillcomboxes();
        string sID = this.Request.Params["id"].Replace("'", "");
        if ((sID != null) && (sID.Trim() != "0"))
        {
            this.Page.Title = Request.Url.ToString();
            Guid id = new Guid(sID);
            this.HiddenID.Value = id.ToString();
            LoadDocument(id);
        }




    }

    void fillcomboxes()
    {
        SqlCommand cmd = DB.SQL(this.Context, "Exec ISAssignUserByUserNamev4 @UserName");
        DB.AddParam(cmd, "@UserName", 100, Membership.GetUser().UserName);
        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = cmd;
        adapter.Fill(this.DTUsers.Table);

    }

    protected void ASPxMenu1_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("save"))
        {
            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlış bilgi girişi!", "stkey1");
                return;
            }
            grid.UpdateEdit();
            Guid id = SaveDocument();
            if (id == Guid.Empty)
            {
                CrmUtils.CreateMessageAlert(this.Page, (String)Session["Hata"], "stkey1");
                return;
            }
            else
            {
                Session["BelgeID"] = id.ToString();
                this.Response.Write("<script language='javascript'>{ parent.opener.location.reload(true);window.location.href='./edit.aspx?id=" + (String)Session["BelgeID"] + "';}</script>");
            }
        }
        else if (e.Item.Name.Equals("delete"))
        {
            if (!Security.CheckPermission(this.Context, "Tanım - Virüs Sınıfları", "Delete"))
            {
                CrmUtils.MessageAlert(this.Page, "Silme işlemi yapma yetkisine sahip değilsiniz!", "stkey1");
                return;
            }
            if (DeleteDocument())
                this.Response.Write("<script language='javascript'>{ parent.opener.location.reload(true);parent.close(); }</script>");
        }
    }

    private bool DeleteDocument()
    {
        string sID = this.HiddenID.Value;
        SqlCommand cmd;
        bool _return = false;
        if (sID != null)
        {
            DB.BeginTrans(this.Context);

            Guid id = Guid.Empty;
            id = new Guid(this.HiddenID.Value);
            try
            {
                cmd = DB.SQL(this.Context, "DELETE FROM VirusSinifUsers WHERE VirusSinifID=@Id");
                DB.AddParam(cmd, "@Id", id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd = DB.SQL(this.Context, "DELETE FROM VirusSinif WHERE VirusSinifID=@Id");
                DB.AddParam(cmd, "@Id", id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                DB.Commit(this.Context);
                _return= true;
            }
            catch (Exception ex)
            {
                DB.Rollback(this.Context);
                CrmUtils.MessageAlert(this.Page, ex.Message.ToString().Replace("'", null).Replace("\r", null).Replace("\n", null), "stkeySilinemez");
                _return= false;
            }
        }
        return _return;
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
                #region insert
                sb = new StringBuilder();
                sb.Append("INSERT INTO VirusSinif (");
                sb.Append("VirusSinifID");
                sb.Append(",Adi");
                if (this.Description.Text != null)
                    sb.Append(",Description");
                sb.Append(",CreatedBy");
                sb.Append(",CreationDate) ");
                sb.Append("VALUES(");
                sb.Append("@VirusSinifID");
                sb.Append(",@Adi");
                if (this.Description.Text != null)
                    sb.Append(",@Description");
                sb.Append(",@CreatedBy");
                sb.Append(",@CreationDate)");
                cmd = DB.SQL(this.Context, sb.ToString());
                id = Guid.NewGuid();
                DB.AddParam(cmd, "@VirusSinifID", id);
                DB.AddParam(cmd, "@CreatedBy", 100, Membership.GetUser().UserName);
                DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                #endregion
            }
            else
            {
                #region update
                sb = new StringBuilder();
                sb.Append("UPDATE VirusSinif SET ");
                sb.Append("Adi=@Adi");
                sb.Append(",Description=@Description ");
                sb.Append("WHERE VirusSinifID=@VirusSinifID");
                cmd = DB.SQL(this.Context, sb.ToString());
                DB.AddParam(cmd, "@VirusSinifID", id);
                DB.AddParam(cmd, "@ModifiedBy", 100, Membership.GetUser().UserName);
                DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                #endregion
            }

            #region values
            DB.AddParam(cmd, "@Description", 255, this.Description.Text.ToUpper());
            DB.AddParam(cmd, "@Adi", 150, this.Adi.Text.ToUpper());
            #endregion

            cmd.Prepare();
            cmd.ExecuteNonQuery();

            #region User List
            DataTable changes = this.DataTableList.Table.GetChanges();
            if (changes != null)
            {
                foreach (DataRow row in changes.Rows)
                {
                    switch (row.RowState)
                    {
                        case DataRowState.Added:
                            sb = new StringBuilder();
                            sb.Append("INSERT INTO VirusSinifUsers(Id,VirusSinifId,UserId,CreatedBy,CreationDate) ");
                            sb.Append("VALUES(@Id,@VirusSinifId,@UserId,@CreatedBy,@CreationDate)");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@VirusSinifId", id);
                            DB.AddParam(cmd, "@UserId", (Guid)row["UserId"]);
                            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Modified:
                            sb = new StringBuilder();
                            sb.Append("UPDATE VirusSinifUsers SET ");
                            sb.Append("UserId=@UserId,");
                            sb.Append("ModifiedBy=@ModifiedBy,");
                            sb.Append("ModificationDate=@ModificationDate ");
                            sb.Append("WHERE Id=@Id");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@UserId", (Guid)row["UserId"]);
                            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Deleted:
                            cmd = DB.SQL(this.Context, "DELETE FROM VirusSinifUsers WHERE ID=@ID");
                            DB.AddParam(cmd, "@ID", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                    }
                }
            }
            #endregion

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
        #region Fill Main Table
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder("SELECT * from Virussinif Where VirussinifId=@Id");

        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@Id", id);
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        if (!rdr.Read())
        {
            this.Response.StatusCode = 500;
            this.Response.End();
            return;
        }

        this.Adi.Text = rdr["Adi"].ToString();
        this.Description.Text = rdr["Description"].ToString();
        rdr.Close();
        #endregion

        #region FillUsers
        this.DataTableList.Table.Rows.Clear();
        sb = new StringBuilder("Select t1.*,t2.UserName,t2.FirstName,t2.LastName from VirusSinifUsers t1 ");
        sb.Append("LEFT JOIN SecurityUsers t2 on (t1.UserId=t2.UserId) Where t1.VirusSinifID=@Id And t2.Active=1");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@Id", id);
        cmd.Prepare();
        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();
            row["ID"] = rdr["Id"];
            row["UserId"] = rdr["UserID"];
            row["UserName"] = rdr["UserName"];
            row["FirstName"] = rdr["FirstName"];
            row["LastName"] = rdr["LastName"];
            this.DataTableList.Table.Rows.Add(row);

        }
        rdr.Close();
        this.DataTableList.Table.AcceptChanges();
        grid.DataBind();
        #endregion
    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("UserId", typeof(Guid));
        dt.Columns.Add("UserName", typeof(string));
        dt.Columns.Add("FirstName", typeof(string));
        dt.Columns.Add("LastName", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        dt.Columns.Add("CreationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["UserId"] == null)
        {
            e.RowError = "Lütfen kullanıcı alanını boş bırakmayınız...";
            return;
        }

        if (grid.IsNewRowEditing)
        {

            DataRow[] rows = DataTableList.Table.Select("UserId='" + e.NewValues["UserId"].ToString() + "'");

            if (rows.Length > 0)
            {
                e.RowError = "Bu kullanıcı daha önce eklenmiş, lütfen başka bir kullanıcı seçiniz...";
                return;
            }
        }
    }
}
