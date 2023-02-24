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

public partial class admin_User_list : System.Web.UI.Page
{
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
        this.menu.ItemClick += new MenuItemEventHandler(menu_ItemClick);

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }

        if (!Security.CheckPermission(this.Context, "Kullanici", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#e6e6fa'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        bool bInsert = Security.CheckPermission(this.Context, "Kullanici", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Kullanici", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Kullanici", "Delete");

        this.menu.Items.FindByName("new").Visible = bInsert;

        if (bInsert || bUpdate || bDelete)
            this.menu.Items.FindByName("save").Visible = true;
        else
            this.menu.Items.FindByName("save").Visible = false;

        InitGridTable(this.DTUsers.Table);

        for (int i = 0; i < this.grid.Columns.Count; i++)
        {
            if (this.grid.Columns[i] is GridViewCommandColumn)
            {
                (this.grid.Columns[i] as GridViewCommandColumn).NewButton.Visible = false;
                (this.grid.Columns[i] as GridViewCommandColumn).EditButton.Visible = false;
                (this.grid.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
                break;
            }
        }

        LoadDocument(1);
    }

    public void menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("save"))
        {
            this.grid.UpdateEdit();
            if (SaveDocument())
            {
                this.Response.Redirect("./list.aspx");
            }
            else
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "MenuSave1");
                return;
            }
        }
        else if (e.Item.Name.Equals("Passive"))
        {
            LoadDocument(0);
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
        dt.Columns.Add("UserID", typeof(Guid));
        dt.Columns.Add("UserName", typeof(string));
        dt.Columns.Add("Password", typeof(string));
        dt.Columns.Add("FirstName", typeof(string));
        dt.Columns.Add("LastName", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Title", typeof(string));
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument(int Active)
    {
        this.DTUsers.Table.Clear();
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM SecurityUsers Where Active=@Active ORDER BY UserName");
        DB.AddParam(cmd, "@Active", Active);
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTUsers.Table.NewRow();

            row["ID"] = rdr["UserID"];
            row["UserID"] = rdr["UserID"];
            row["UserName"] = rdr["UserName"];
            row["Password"] = rdr["Password"];
            row["FirstName"] = rdr["FirstName"];
            row["LastName"] = rdr["LastName"];
            row["Email"] = rdr["Email"];
            row["Title"] = rdr["Title"];
            row["Department"] = rdr["Department"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];

            this.DTUsers.Table.Rows.Add(row);
        }
        rdr.Close();

        this.DTUsers.Table.AcceptChanges();
        this.grid.DataBind();
    }

    private bool SaveDocument()
    {
        DB.BeginTrans(this.Context);

        SqlCommand cmd = null;
        StringBuilder sb;

        //Membership.ApplicationName = "/";
        DataTable changes = this.DTUsers.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        if (((row["UserName"] != null) && (row["UserName"].ToString() != ""))
                            && ((row["Password"] != null) && (row["Password"].ToString() != ""))
                            && ((row["Email"] != null) && (row["Email"].ToString() != "")))
                        {
                            MembershipCreateStatus status;
                            Membership.CreateUser(row["UserName"].ToString(), row["Password"].ToString(), row["Email"].ToString(), null, null, true, out status);
                            if (status != MembershipCreateStatus.Success)
                            {
                                switch (status)
                                {
                                    case MembershipCreateStatus.InvalidUserName:
                                        this.Session["HATA"] = "'" + row["UserName"].ToString() + "' kullanýcý adý geçersiz!'";
                                        break;
                                    case MembershipCreateStatus.DuplicateUserName:
                                        this.Session["HATA"] = "'" + row["UserName"].ToString() + "' kullanýcý adý daha önce kullanýlmýþ!'";
                                        break;
                                    case MembershipCreateStatus.InvalidPassword:
                                        this.Session["HATA"] = "'" + row["Password"].ToString() + "' kullanýcý þifresi geçersiz!'";
                                        break;
                                    case MembershipCreateStatus.DuplicateEmail:
                                        this.Session["HATA"] = "'" + row["Email"].ToString() + "' e-posta adresi daha önce kullanýlmýþ!'";
                                        break;
                                    default:
                                        this.Session["HATA"] = "'" + row["UserName"].ToString() + "' kullanýcýsý oluþturulmadý!'";
                                        break;
                                }

                                DB.Rollback(this.Context);
                                this.Response.Redirect("./../../CRM/hata.aspx");
                                return false;
                            }
                        }
                        else
                        {
                            DB.Rollback(this.Context);
                            return false;
                        }

                        cmd = DB.SQL(this.Context, "SELECT UserId FROM aspnet_Membership WHERE Email=@Email");
                        DB.AddParam(cmd, "@Email", 255, row["Email"].ToString());
                        cmd.Prepare();
                        Guid gUserID = (Guid)cmd.ExecuteScalar();

                        sb = new StringBuilder();
                        sb.Append("INSERT INTO SecurityUsers(UserID,UserName,Password,FirstName,LastName,Email,Title,Department,CreatedBy,CreationDate)");
                        sb.Append("VALUES(@UserID,@UserName,@Password,@FirstName,@LastName,@Email,@Title,@Department,@CreatedBy,@CreationDate)");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@UserID", gUserID);
                        DB.AddParam(cmd, "@UserName", 100, row["UserName"].ToString());
                        DB.AddParam(cmd, "@Password", 100, row["Password"].ToString());
                        DB.AddParam(cmd, "@FirstName", 60, row["FirstName"].ToString());
                        DB.AddParam(cmd, "@LastName", 60, row["LastName"].ToString());
                        DB.AddParam(cmd, "@Email", 255, row["Email"].ToString());
                        DB.AddParam(cmd, "@Title", 255, row["Title"].ToString());
                        DB.AddParam(cmd, "@Department", 255, row["Department"].ToString());
                        DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                        cmd.Prepare();
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Membership.DeleteUser(row["UserName"].ToString());
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            this.Response.Redirect("./../../CRM/hata.aspx");
                        }
                        break;
                    case DataRowState.Modified:
                        String sOldPassword = "";
                        String sOldEmail = "";

                        cmd = DB.SQL(this.Context, "SELECT Password,Email FROM SecurityUsers WHERE UserID=@UserID");
                        DB.AddParam(cmd, "@UserID", (Guid)row["ID"]);
                        cmd.Prepare();
                        IDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            sOldPassword = (String)rdr["Password"];
                            sOldEmail = (String)rdr["Email"];
                        }
                        rdr.Close();

                        String sNewPassword = (String)row["Password"].ToString();
                        String sNewEmail = (String)row["Email"].ToString();

                        if ((sOldPassword != sNewPassword) || (sOldEmail != sNewEmail))
                        {
                            MembershipUserCollection users = Membership.FindUsersByName(row["UserName"].ToString());
                            if (users.Count != 0)
                            {
                                if (sOldPassword != sNewPassword)
                                {
                                    sOldPassword = users[row["UserName"].ToString()].ResetPassword();
                                    if (!(users[row["UserName"].ToString()].ChangePassword(sOldPassword, row["Password"].ToString())))
                                    {
                                        this.Session["HATA"] = "'" + row["UserName"].ToString() + "' kullanýcý þifresi deðiþtirilemedi!'";
                                        DB.Rollback(this.Context);
                                        this.Response.Redirect("./../../CRM/hata.aspx");
                                        return false;
                                    }
                                }
                                if (sOldEmail != sNewEmail)
                                    users[row["UserName"].ToString()].Email = row["Email"].ToString();
                                Membership.UpdateUser(users[row["UserName"].ToString()]);
                            }
                            else
                            {
                                DB.Rollback(this.Context);
                                return false;
                            }
                        }

                        sb = new StringBuilder();
                        sb.Append("UPDATE SecurityUsers SET ");
                        //sb.Append("UserName=@UserName,");
                        sb.Append("Password=@Password,");
                        sb.Append("FirstName=@FirstName,");
                        sb.Append("LastName=@LastName,");
                        sb.Append("Email=@Email,");
                        sb.Append("Title=@Title,");
                        sb.Append("Department=@Department,");
                        sb.Append("ModifiedBy=@ModifiedBy,");
                        sb.Append("ModificationDate=@ModificationDate");
                        if ((row["MerkezID"].ToString() != "") && ((Guid)row["MerkezID"] != Guid.Empty))
                            sb.Append(",MerkezID=@MerkezID");
                        else
                            sb.Append(",MerkezID=NULL");
                        if ((row["UnvanID"].ToString() != "") && ((Guid)row["UnvanID"] != Guid.Empty))
                            sb.Append(",UnvanID=@UnvanID");
                        else
                            sb.Append(",UnvanID=NULL");
                        sb.Append(",TumMerkezBilgileri=@TumMerkezBilgileri");
                        sb.Append("WHERE UserID=@UserID");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@UserID", (Guid)row["ID"]);
                        //DB.AddParam(cmd, "@UserName", 100, row["UserName"].ToString());
                        DB.AddParam(cmd, "@Password", 100, row["Password"].ToString());
                        DB.AddParam(cmd, "@FirstName", 60, row["FirstName"].ToString());
                        DB.AddParam(cmd, "@LastName", 60, row["LastName"].ToString());
                        DB.AddParam(cmd, "@Email", 255, row["Email"].ToString());
                        DB.AddParam(cmd, "@Title", 255, row["Title"].ToString());
                        DB.AddParam(cmd, "@Department", 255, row["Department"].ToString());
                        DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                        if ((row["MerkezID"].ToString() != "") && ((Guid)row["MerkezID"] != Guid.Empty))
                            DB.AddParam(cmd, "@MerkezID", (Guid)row["MerkezID"]);
                        if ((row["UnvanID"].ToString() != "") && ((Guid)row["UnvanID"] != Guid.Empty))
                            DB.AddParam(cmd, "@UnvanID", (Guid)row["UnvanID"]);
                        DB.AddParam(cmd, "@TumMerkezBilgileri", ((bool)row["TumMerkezBilgileri"] == true ? 1 : 0));
                        cmd.Prepare();
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            this.Response.Redirect("./../../CRM/hata.aspx");
                        }
                        break;
                    case DataRowState.Deleted:
                        cmd = DB.SQL(this.Context, "EXEC DeleteUser @UserId");
                        DB.AddParam(cmd, "@UserId", (Guid)row["ID", DataRowVersion.Original]);
                        cmd.Prepare();
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            //this.Response.Redirect("./../../CRM/hata.aspx");
                            return false;
                        }

                        MembershipUserCollection users2 = Membership.FindUsersByName(row["UserName", DataRowVersion.Original].ToString());
                        if (users2.Count != 0)
                        {
                            if (!Membership.DeleteUser(row["UserName", DataRowVersion.Original].ToString()))
                            {
                                DB.Rollback(this.Context);
                                this.Session["HATA"] = "'" + row["UserName", DataRowVersion.Original].ToString() + "' kullanýcýsý silinemedi!'";
                                //this.Response.Redirect("./../../CRM/hata.aspx");
                                return false;
                            }
                        }
                        break;
                }
            }
        }

        DB.Commit(this.Context);

        return true;
    }

    protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["MerkezID"] == null)
                e.NewValues["MerkezID"] = DBNull.Value;
            if (e.NewValues["UnvanID"] == null)
                e.NewValues["UnvanID"] = DBNull.Value;
            if (e.NewValues["TumMerkezBilgileri"] == null)
                e.NewValues["TumMerkezBilgileri"] = false;
        }
    }

    protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["MerkezID"] == null)
                e.NewValues["MerkezID"] = DBNull.Value;
            if (e.NewValues["UnvanID"] == null)
                e.NewValues["UnvanID"] = DBNull.Value;
            if (e.NewValues["TumMerkezBilgileri"] == null)
                e.NewValues["TumMerkezBilgileri"] = false;
        }
    }

    protected void Grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["UserName"] == null)
        {
            e.RowError = "Lütfen Kullanýcý Adý alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["FirstName"] == null)
        {
            e.RowError = "Lütfen Adý alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["LastName"] == null)
        {
            e.RowError = "Lütfen Soyadý alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["Password"] == null)
        {
            e.RowError = "Lütfen Þifre alanýný boþ býrakmayýnýz...";
            return;
        }
        else
            if (e.NewValues["Password"].ToString().Trim().Length < 4)
            {
                e.RowError = "Þifre minimum 4 karakter olmalýdýr...";
                return;
            }
        if (e.NewValues["Email"] == null)
        {
            e.RowError = "Lütfen E-Posta alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["TumMerkezBilgileri"] == null)
        {
            e.NewValues["TumMerkezBilgileri"] = false;
        }

        if (e.NewValues["UserName"].ToString().Trim().ToLower() != "admin")
        {
            if (e.NewValues["MerkezID"] == null)
            {
                e.RowError = "Lütfen Merkez alanýný boþ býrakmayýnýz...";
                return;
            }
            if (e.NewValues["UnvanID"] == null)
            {
                e.RowError = "Lütfen Ünvan alanýný boþ býrakmayýnýz...";
                return;
            }
        }

        if (e.IsNewRow)
        {
            string username = e.NewValues["UserName"].ToString().Trim();
            DataRow[] rows = this.DTUsers.Table.Select("UserName='" + username + "'");

            if (rows.GetUpperBound(0) != -1)
            {
                e.RowError = "'" + username + "' kullanýcý adý daha önce tanýmlanmýþ...";
                return;
            }

            string email = e.NewValues["Email"].ToString().Trim();
            rows = this.DTUsers.Table.Select("Email='" + email + "'");

            if (rows.GetUpperBound(0) != -1)
            {
                e.RowError = "'" + email + "' e-posta adý daha önce tanýmlanmýþ...";
                return;
            }
        }
    }

    protected void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        DataTable changes = this.DTUsers.Table.GetChanges();

        if (changes != null)
            e.Properties["cpIsDirty"] = true;
        else
            e.Properties["cpIsDirty"] = false;
    }
}
