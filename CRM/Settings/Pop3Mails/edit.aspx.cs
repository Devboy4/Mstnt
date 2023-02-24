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

public partial class CRM_Settings_Pop3Mails_edit : System.Web.UI.Page
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

        if (!Security.CheckPermission(this.Context, "Tanım - Pop3 Mail", "Select"))
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
            int id = Convert.ToInt32(sID);
            this.HiddenID.Value = id.ToString();
            LoadDocument(id);
        }




    }

    void fillcomboxes()
    {
        SqlCommand cmd = DB.SQL(this.Context, "Exec ISAssignUserByUserNameV3 @UserName");
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
            if (!Security.CheckPermission(this.Context, "Tanım - Pop3 Mail", "Update"))
            {
                CrmUtils.MessageAlert(this.Page, "Güncelleme işlemi yapma yetkisine sahip değilsiniz!", "stkey1");
                return;
            }
            grid.UpdateEdit();
            int id = SaveDocument();
            if (id == -1)
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
            if (!Security.CheckPermission(this.Context, "Tanım - Pop3 Mail", "Delete"))
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
                cmd = DB.SQL(this.Context, "DELETE FROM Pop3MailUsers WHERE PopMailListId=@Id");
                DB.AddParam(cmd, "@Id", id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd = DB.SQL(this.Context, "DELETE FROM PopMailList WHERE IndexId=@Id");
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

    private int SaveDocument()
    {
        try
        {

            DB.BeginTrans(this.Context);
            int id = -1;
            if (this.HiddenID.Value.Length != 0)
            {
                id = int.Parse(this.HiddenID.Value);
            }
            SqlCommand cmd = null;
            StringBuilder sb = new StringBuilder();

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
                            sb.Append("INSERT INTO Pop3MailUsers(Id,PopMailListId,UserId,CreatedBy,CreationDate) ");
                            sb.Append("VALUES(@Id,@PopMailListId,@UserId,@CreatedBy,@CreationDate)");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@PopMailListId", id);
                            DB.AddParam(cmd, "@UserId", (int)row["UserId"]);
                            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Modified:
                            sb = new StringBuilder();
                            sb.Append("UPDATE Pop3MailUsers SET ");
                            sb.Append("UserId=@UserId,");
                            sb.Append("ModifiedBy=@ModifiedBy,");
                            sb.Append("ModificationDate=@ModificationDate ");
                            sb.Append("WHERE Id=@Id");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@UserId", (int)row["UserId"]);
                            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Deleted:
                            cmd = DB.SQL(this.Context, "DELETE FROM Pop3MailUsers WHERE ID=@ID");
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
            return -1;
        }
    }

    void LoadDocument(int id)
    {
    
        StringBuilder sb = new StringBuilder();
        SqlCommand cmd;

        #region FillUsers
        this.DataTableList.Table.Rows.Clear();
        sb = new StringBuilder("Select t1.*,t2.UserName,t2.FirstName,t2.LastName from Pop3MailUsers t1 ");
        sb.Append("LEFT JOIN SecurityUsers t2 on (t1.UserId=t2.IndexId) Where t1.PopMailListId=@Id And t2.Active=1");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@Id", id);
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();
            row["IndexId"] = rdr["IndexId"];
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
        dt.Columns.Add("IndexId", typeof(int));
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("UserId", typeof(int));
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
