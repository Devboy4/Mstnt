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

public partial class admin_Role_edit : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "Rol Kullanici", "Select")
            && !Security.CheckPermission(this.Context, "Rol Ýzin", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#e6e6fa'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        // Eventleri tanimlayalim
        this.menu.ItemClick += new MenuItemEventHandler(Menu_ItemClick);

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }

        bool bInsert = Security.CheckPermission(this.Context, "Rol Kullanici", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Rol Kullanici", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Rol Kullanici", "Delete");
        for (int i = 0; i < this.GridRoleUsers.Columns.Count; i++)
        {
            if (this.GridRoleUsers.Columns[i] is GridViewCommandColumn)
            {
                (this.GridRoleUsers.Columns[i] as GridViewCommandColumn).NewButton.Visible = bInsert;
                (this.GridRoleUsers.Columns[i] as GridViewCommandColumn).EditButton.Visible = bUpdate;
                (this.GridRoleUsers.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
                break;
            }
        }

        bInsert = Security.CheckPermission(this.Context, "Rol Ýzin", "Insert");
        bUpdate = Security.CheckPermission(this.Context, "Rol Ýzin", "Update");
        bDelete = Security.CheckPermission(this.Context, "Rol Ýzin", "Delete");
        for (int i = 0; i < this.GridRolePermissions.Columns.Count; i++)
        {
            if (this.GridRolePermissions.Columns[i] is GridViewCommandColumn)
            {
                (this.GridRolePermissions.Columns[i] as GridViewCommandColumn).NewButton.Visible = bInsert;
                (this.GridRolePermissions.Columns[i] as GridViewCommandColumn).EditButton.Visible = bUpdate;
                (this.GridRolePermissions.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
                break;
            }
        }

        InitGridRoleUsers(this.DTRoleUsers.Table);
        InitGridRolePermissions(this.DTRolePermissions.Table);
        InitGridDtDurumList(this.DataTableDurumList.Table);
        InitGridDTDurumlar(this.DTDurumlar.Table);
        fillcomboxes();

        this.Title = "Rol - Yeni";

        Guid id = Guid.Empty;

        string sID = this.Request.Params["id"].Replace("'", "").Trim();
        if ((sID != null) && (sID != "0"))
        {
            id = new Guid(sID);
            this.HiddenID.Value = id.ToString();

            SqlCommand cmd = DB.SQL(this.Context, "SELECT Role FROM SecurityRoles WHERE RoleID=@RoleID");
            DB.AddParam(cmd, "@RoleID", id);
            cmd.Prepare();
            string sRoleName = (string)cmd.ExecuteScalar();
            this.GridRoleUsers.SettingsText.Title = "[" + sRoleName + "]  Kullanýcý Bilgileri";
            this.GridRolePermissions.SettingsText.Title = "[" + sRoleName + "]  Ýzin Bilgileri";
            this.Title = "Rol - " + sRoleName;
            LoadDocument(id);
        }
        else
        {
            this.Response.End();
        }
    }

    private void Menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("save"))
        {
            this.GridRoleUsers.UpdateEdit();
            this.GridRolePermissions.UpdateEdit();

            SaveDocument();

            this.Response.Write("<script language='javascript'>{ window.location.href='./edit.aspx?id=" + this.HiddenID.Value.ToString() + "';}</script>");

        }
        else if (e.Item.Name.Equals("saveclose"))
        {
            this.GridRoleUsers.UpdateEdit();
            this.GridRolePermissions.UpdateEdit();

            SaveDocument();

            this.Response.Write("<script language='javascript'>{ parent.close();}</script>");
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    private void InitGridRoleUsers(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("UserRoleID", typeof(Guid));
        dt.Columns.Add("UserID", typeof(Guid));
        dt.Columns.Add("RoleID", typeof(Guid));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitGridRolePermissions(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("RolePermissionID", typeof(Guid));
        dt.Columns.Add("RoleID", typeof(Guid));
        dt.Columns.Add("ObjectID", typeof(Guid));
        dt.Columns.Add("Select", typeof(int));
        dt.Columns.Add("Insert", typeof(int));
        dt.Columns.Add("Update", typeof(int));
        dt.Columns.Add("Delete", typeof(int));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitGridDTDurumlar(DataTable dt)
    {
        dt.Columns.Add("DurumID", typeof(Guid));
        dt.Columns.Add("DurumName", typeof(string));
    }

    private void InitGridDtDurumList(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("UserProfileDurumListID", typeof(Guid));
        dt.Columns.Add("UserProfileID", typeof(Guid));
        dt.Columns.Add("DurumID", typeof(Guid));
        dt.Columns.Add("RoleName", typeof(string));
        dt.Columns.Add("DurumName", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModificationDate", typeof(DateTime));


        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
        dt.Columns["CreationDate"].DefaultValue = DateTime.Now;
    }

    private void LoadDocument(Guid id)
    {
        #region Kullanýcýlar
        this.DTRoleUsers.Table.Clear();
        StringBuilder sb = new StringBuilder();

        sb.Append("SELECT t1.* FROM SecurityUserRoles t1, SecurityUsers t2 WHERE t1.UserID=t2.UserID and t1.RoleID=@RoleID ORDER BY t2.UserName");
        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@RoleID", id);
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTRoleUsers.Table.NewRow();

            row["ID"] = rdr["UserRoleID"];
            row["UserRoleID"] = rdr["UserRoleID"];
            row["UserID"] = rdr["UserID"];
            row["RoleID"] = rdr["RoleID"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];

            this.DTRoleUsers.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTRoleUsers.Table.AcceptChanges();
        #endregion

        #region Ýzinler
        this.DTRolePermissions.Table.Clear();
        sb = new StringBuilder();

        sb.Append("SELECT t1.* FROM SecurityRolePermissions t1, SecurityObjects t2 WHERE t1.ObjectID=t2.ObjectID and t1.RoleID=@RoleID ORDER BY t2.ObjectDescription");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@RoleID", id);
        cmd.Prepare();

        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTRolePermissions.Table.NewRow();

            row["ID"] = rdr["RolePermissionID"];
            row["RolePermissionID"] = rdr["RolePermissionID"];
            row["RoleID"] = rdr["RoleID"];
            row["ObjectID"] = rdr["ObjectID"];
            row["Select"] = rdr["Select"];
            row["Insert"] = rdr["Insert"];
            row["Update"] = rdr["Update"];
            row["Delete"] = rdr["Delete"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];

            this.DTRolePermissions.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTRolePermissions.Table.AcceptChanges();
        #endregion

        #region DurumListesi
        this.DataTableDurumList.Table.Clear();
        sb = new StringBuilder();
        sb.Append("SELECT d.Adi AS DurumName, R.Role AS RoleName, Upd.* FROM UserProfileDurumList AS  Upd ");
        sb.Append("LEFT OUTER JOIN Durum AS d ON Upd.DurumID=d.DurumID LEFT OUTER JOIN SecurityRoles AS R ON ");
        sb.Append("Upd.UserProfileID=R.RoleID ");
        sb.Append("WHERE Upd.UserProfileID=@RoleID");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@RoleID", id);
        cmd.Prepare();

        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableDurumList.Table.NewRow();
            row["ID"] = rdr["UserProfileDurumListID"];
            row["UserProfileDurumListID"] = rdr["UserProfileDurumListID"];
            row["UserProfileID"] = rdr["UserProfileID"];
            row["DurumID"] = rdr["DurumID"];
            row["RoleName"] = rdr["RoleName"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModificationDate"] = rdr["ModificationDate"];
            row["DurumName"] = rdr["DurumName"];

            this.DataTableDurumList.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DataTableDurumList.Table.AcceptChanges();
        #endregion

        #region Genel
        sb = new StringBuilder();
        sb.Append("SELECT * FROM SecurityRoles WHERE RoleID=@RoleID");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@RoleID", id);
        cmd.Prepare();
        rdr = cmd.ExecuteReader();
        if (!rdr.Read())
        {
            this.Response.StatusCode = 500;
            this.Response.End();
            return;
        }
        if (rdr["IsBildirimKisiAta"] != DBNull.Value && rdr["IsBildirimKisiAta"] != null)
        {
            if (Convert.ToBoolean(rdr["IsBildirimKisiAta"]))
                IsBildirimKisiAta.SelectedIndex = 1;
            else
                IsBildirimKisiAta.SelectedIndex = 0;
        }
        else
            IsBildirimKisiAta.SelectedIndex = 0;
        if (rdr["IsPersonalizedPost"] != DBNull.Value && rdr["IsPersonalizedPost"] != null)
        {
            if (Convert.ToBoolean(rdr["IsPersonalizedPost"]))
                IsPersonalizedPost.SelectedIndex = 1;
            else
                IsPersonalizedPost.SelectedIndex = 0;
        }
        else
            IsPersonalizedPost.SelectedIndex = 0;
        if ((rdr["YaziRenkleriID"].ToString() != null) && (rdr["YaziRenkleriID"].ToString() != ""))
            this.YaziRenkleriID.SelectedIndex = this.YaziRenkleriID.Items.IndexOfValue(rdr["YaziRenkleriID"]);
        rdr.Close();
        #endregion

    }

    private void fillcomboxes()
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT DurumID, Adi As DurumName FROM Durum ORDER BY Adi");
        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = DTDurumlar.Table.NewRow();
            row["DurumID"] = rdr["DurumID"];
            row["DurumName"] = rdr["DurumName"];
            DTDurumlar.Table.Rows.Add(row);

        }
        DTDurumlar.Table.AcceptChanges();
        rdr.Close();

        data.BindComboBoxes(this.Context, YaziRenkleriID, "Select YaziRenkleriID,(IsNull(Adi,'')+' ['+IsNull(Kodu,'')+']') As RenkAdi" +
            " From YaziRenkleri Order By Adi", "YaziRenkleriID", "RenkAdi");
    }

    private Guid SaveDocument()
    {
        DB.BeginTrans(this.Context);

        Guid id = Guid.Empty;
        if (this.HiddenID.Value.Length != 0)
        {
            id = new Guid(this.HiddenID.Value);
        }

        SqlCommand cmd = null;
        StringBuilder sb = new StringBuilder();

        if (Roles.Provider.ApplicationName != Membership.ApplicationName)
            Roles.Provider.ApplicationName = Membership.ApplicationName;

        if (id != Guid.Empty)
        {
            cmd = DB.SQL(this.Context, "SELECT Role FROM SecurityRoles WHERE RoleID=@RoleID");
            DB.AddParam(cmd, "@RoleID", id);
            cmd.Prepare();
            string sRole = (string)cmd.ExecuteScalar();

            if (!Roles.RoleExists(sRole))
            {
                CrmUtils.MessageAlert(this.Page, sRole + " rolü sistemde bulunamadý!", "RoleEditSave1");
                DB.Rollback(this.Context);
                return id;
            }

            #region Kullanýcýlar
            DataTable changes = this.DTRoleUsers.Table.GetChanges();
            if (changes != null)
            {
                string sUserName = null;
                foreach (DataRow row in changes.Rows)
                {
                    switch (row.RowState)
                    {
                        case DataRowState.Added:
                            sb = new StringBuilder();
                            sb.Append("INSERT INTO SecurityUserRoles(UserRoleID,UserID,RoleID,CreatedBy,CreationDate) ");
                            sb.Append("VALUES(@UserRoleID,@UserID,@RoleID,@CreatedBy,@CreationDate)");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@UserRoleID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@UserID", (Guid)row["UserID"]);
                            DB.AddParam(cmd, "@RoleID", id);
                            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();

                            cmd = DB.SQL(this.Context, "SELECT UserName FROM SecurityUsers WHERE UserID=@UserID");
                            DB.AddParam(cmd, "@UserID", (Guid)row["UserID"]);
                            cmd.Prepare();
                            sUserName = (string)cmd.ExecuteScalar();

                            if (Roles.IsUserInRole(sUserName, sRole))
                            {
                                DB.Rollback(this.Context);
                                return id;
                            }

                            try
                            {
                                Roles.AddUserToRole(sUserName, sRole);
                            }
                            catch (Exception ex)
                            {
                                DB.Rollback(this.Context);
                                return id;
                            }

                            break;
                        case DataRowState.Modified:
                            sb = new StringBuilder();
                            sb.Append("UPDATE SecurityUserRoles SET ");
                            sb.Append("UserID=@UserID,");
                            sb.Append("RoleID=@RoleID,");
                            sb.Append("ModifiedBy=@ModifiedBy,");
                            sb.Append("ModificationDate=@ModificationDate ");
                            sb.Append("WHERE UserRoleID=@UserRoleID");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@UserRoleID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@UserID", (Guid)row["UserID"]);
                            DB.AddParam(cmd, "@RoleID", id);
                            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();

                            cmd = DB.SQL(this.Context, "SELECT UserName FROM SecurityUsers WHERE UserID=@UserID");
                            DB.AddParam(cmd, "@UserID", (Guid)row["UserID", DataRowVersion.Original]);
                            cmd.Prepare();
                            sUserName = (string)cmd.ExecuteScalar();

                            if (Roles.IsUserInRole(sUserName, sRole))
                            {
                                try
                                {
                                    Roles.RemoveUserFromRole(sUserName, sRole);
                                }
                                catch (Exception ex)
                                {
                                    DB.Rollback(this.Context);
                                    return id;
                                }
                            }

                            cmd = DB.SQL(this.Context, "SELECT UserName FROM SecurityUsers WHERE UserID=@UserID");
                            DB.AddParam(cmd, "@UserID", (Guid)row["UserID"]);
                            cmd.Prepare();
                            sUserName = (string)cmd.ExecuteScalar();

                            if (Roles.IsUserInRole(sUserName, sRole))
                            {
                                DB.Rollback(this.Context);
                                return id;
                            }

                            try
                            {
                                Roles.AddUserToRole(sUserName, sRole);
                            }
                            catch (Exception ex)
                            {
                                DB.Rollback(this.Context);
                                return id;
                            }

                            break;
                        case DataRowState.Deleted:
                            cmd = DB.SQL(this.Context, "DELETE FROM SecurityUserRoles WHERE UserRoleID=@UserRoleID");
                            DB.AddParam(cmd, "@UserRoleID", (Guid)row["UserRoleID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();

                            cmd = DB.SQL(this.Context, "SELECT UserName FROM SecurityUsers WHERE UserID=@UserID");
                            DB.AddParam(cmd, "@UserID", (Guid)row["UserID", DataRowVersion.Original]);
                            cmd.Prepare();
                            sUserName = (string)cmd.ExecuteScalar();

                            if (Roles.IsUserInRole(sUserName, sRole))
                            {
                                try
                                {
                                    Roles.RemoveUserFromRole(sUserName, sRole);
                                }
                                catch (Exception ex)
                                {
                                    DB.Rollback(this.Context);
                                    return id;
                                }
                            }

                            break;
                    }
                }
            }

            #endregion

            #region Ýzinler
            changes = this.DTRolePermissions.Table.GetChanges();
            if (changes != null)
            {
                foreach (DataRow row in changes.Rows)
                {
                    switch (row.RowState)
                    {
                        case DataRowState.Added:
                            sb = new StringBuilder();
                            sb.Append("INSERT INTO SecurityRolePermissions(RolePermissionID,RoleID,ObjectID,[Select],[Insert],[Update],[Delete],CreatedBy,CreationDate) ");
                            sb.Append("VALUES(@RolePermissionID,@RoleID,@ObjectID,@Select,@Insert,@Update,@Delete,@CreatedBy,@CreationDate)");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@RolePermissionID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@RoleID", id);
                            DB.AddParam(cmd, "@ObjectID", (Guid)row["ObjectID"]);
                            DB.AddParam(cmd, "@Select", (int)row["Select"]);
                            DB.AddParam(cmd, "@Insert", (int)row["Insert"]);
                            DB.AddParam(cmd, "@Update", (int)row["Update"]);
                            DB.AddParam(cmd, "@Delete", (int)row["Delete"]);
                            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Modified:
                            sb = new StringBuilder();
                            sb.Append("UPDATE SecurityRolePermissions SET ");
                            sb.Append("RoleID=@RoleID,");
                            sb.Append("ObjectID=@ObjectID,");
                            sb.Append("[Select]=@Select,");
                            sb.Append("[Insert]=@Insert,");
                            sb.Append("[Update]=@Update,");
                            sb.Append("[Delete]=@Delete,");
                            sb.Append("ModifiedBy=@ModifiedBy,");
                            sb.Append("ModificationDate=@ModificationDate ");
                            sb.Append("WHERE RolePermissionID=@RolePermissionID");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@RolePermissionID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@RoleID", id);
                            DB.AddParam(cmd, "@ObjectID", (Guid)row["ObjectID"]);
                            DB.AddParam(cmd, "@Select", (int)row["Select"]);
                            DB.AddParam(cmd, "@Insert", (int)row["Insert"]);
                            DB.AddParam(cmd, "@Update", (int)row["Update"]);
                            DB.AddParam(cmd, "@Delete", (int)row["Delete"]);
                            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Deleted:
                            cmd = DB.SQL(this.Context, "DELETE FROM SecurityRolePermissions WHERE RolePermissionID=@RolePermissionID");
                            DB.AddParam(cmd, "@RolePermissionID", (Guid)row["RolePermissionID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                    }
                }
            }
            #endregion

            #region Durum Listesi
            changes = this.DataTableDurumList.Table.GetChanges();
            if (changes != null)
            {
                foreach (DataRow row in changes.Rows)
                {
                    switch (row.RowState)
                    {
                        case DataRowState.Added:
                            sb = new StringBuilder();
                            sb.Append("INSERT INTO UserProfileDurumList(UserProfileDurumListID,UserProfileID,DurumID,CreatedBy,CreationDate)");
                            sb.Append("VALUES(@UserProfileDurumListID,@UserProfileID,@DurumID,@CreatedBy,@CreationDate)");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@UserProfileDurumListID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@UserProfileID", id);
                            DB.AddParam(cmd, "@DurumID", (Guid)row["DurumID"]);
                            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Modified:
                            sb = new StringBuilder();
                            sb.Append("UPDATE UserProfileDurumList SET ");
                            sb.Append("UserProfileID=@UserProfileID,");
                            sb.Append("DurumID=@DurumID,");
                            sb.Append("Modifiedby=@Modifiedby,");
                            sb.Append("ModificationDate=@ModificationDate");
                            sb.Append(" WHERE UserProfileDurumListID=@UserProfileDurumListID");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@UserProfileDurumListID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@UserProfileID", id);
                            DB.AddParam(cmd, "@DurumID", (Guid)row["DurumID"]);
                            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Deleted:
                            cmd = DB.SQL(this.Context, "DELETE FROM UserProfileDurumList WHERE UserProfileDurumListID=@UserProfileDurumListID");
                            DB.AddParam(cmd, "@UserProfileDurumListID", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                    }
                }
            }
            #endregion

            #region Genel Bölümü Update
            sb = new StringBuilder();
            sb.Append("UPDATE SecurityRoles SET IsBildirimKisiAta=@IsBildirimKisiAta,IsPersonalizedPost=@IsPersonalizedPost,");
            sb.Append("YaziRenkleriID=@YaziRenkleriID WHERE RoleID=@RoleID");
            cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@RoleID", id);
            if (IsBildirimKisiAta.SelectedIndex > -1)
                DB.AddParam(cmd, "@IsBildirimKisiAta", IsBildirimKisiAta.SelectedIndex);
            else
                DB.AddParam(cmd, "@IsBildirimKisiAta", SqlDbType.Int);
            if (IsPersonalizedPost.SelectedIndex > -1)
                DB.AddParam(cmd, "@IsPersonalizedPost", IsPersonalizedPost.SelectedIndex);
            else
                DB.AddParam(cmd, "@IsPersonalizedPost", SqlDbType.Int);
            if (YaziRenkleriID.Text != "")
                DB.AddParam(cmd, "@YaziRenkleriID", new Guid(YaziRenkleriID.Value.ToString()));
            else
                DB.AddParam(cmd, "@YaziRenkleriID", SqlDbType.UniqueIdentifier);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
            #endregion

        }

        DB.Commit(this.Context);

        return id;
    }

    protected void GridRoleUsers_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["UserID"] == null)
            e.RowError = "Lütfen Kullanýcý alanýný boþ býrakmayýnýz...";
    }

    protected void GridRolePermissions_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["Select"] == null)
                e.NewValues["Select"] = 0;
            if (e.NewValues["Insert"] == null)
                e.NewValues["Insert"] = 0;
            if (e.NewValues["Update"] == null)
                e.NewValues["Update"] = 0;
            if (e.NewValues["Delete"] == null)
                e.NewValues["Delete"] = 0;
        }
    }

    protected void GridRolePermissions_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["Select"] == null)
                e.NewValues["Select"] = 0;
            if (e.NewValues["Insert"] == null)
                e.NewValues["Insert"] = 0;
            if (e.NewValues["Update"] == null)
                e.NewValues["Update"] = 0;
            if (e.NewValues["Delete"] == null)
                e.NewValues["Delete"] = 0;
        }
    }

    protected void GridRolePermissions_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["ObjectID"] == null)
        {
            e.RowError = "Lütfen Nesne alanýný boþ býrakmayýnýz...";
            return;
        }
    }

    protected void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        ASPxGridView grid = (ASPxGridView)sender;

        DataTable changes = null;

        if (grid.ID == "GridRoleUsers")
            changes = this.DTRoleUsers.Table.GetChanges();
        if (grid.ID == "GridRolePermissions")
            changes = this.DTRolePermissions.Table.GetChanges();

        if (changes != null)
            e.Properties["cpIsDirty"] = true;
        else
            e.Properties["cpIsDirty"] = false;
    }

    protected void GRD_DurumList_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues["DurumID"] == null)
            e.NewValues["DurumID"] = DBNull.Value;
    }

    protected void GRD_DurumList_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues["DurumID"] == null)
            e.NewValues["DurumID"] = DBNull.Value;
    }

    protected void GRD_DurumList_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["DurumID"] == null)
        {
            e.RowError = "Lütfen Bildirim Durumu Adý alanýný boþ býrakmayýnýz...";
            return;
        }
    }

    protected void GRD_DurumList_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {

        if (this.GRD_DurumList.IsEditing && e.Column.FieldName == "DurumID")
        {
            ASPxComboBox cmb = e.Editor as ASPxComboBox;
            ListEditItem item;
            cmb.Items.Clear();
            cmb.DataSourceID = null;
            foreach (DataRow row in DTDurumlar.Table.Rows)
            {
                DataRow[] rows = DataTableDurumList.Table.Select("DurumID='" + row["DurumID"].ToString() + "'");
                if (rows.Length == 0)
                {
                    item = new ListEditItem();
                    item.Text = row["DurumName"].ToString();
                    item.Value = row["DurumID"];
                    cmb.Items.Add(item);
                }
            }

        }
    }

}
