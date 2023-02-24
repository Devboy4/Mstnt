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
using DevExpress.Web.ASPxMenu;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxCallback;

public partial class CRM_IthalatTakip_Tanim_Guvenlik_edit : System.Web.UI.Page
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
        this.menu.ItemClick += new MenuItemEventHandler(Menu_ItemClick);
        if (this.IsPostBack || this.IsCallback) return;

        if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Güvenlik", "Select"))
        {
            //this.Response.Write("<html><body bgcolor='#e6e6fa'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            //this.Response.End();
            CrmUtils.MessageAlert(this.Page, "Bu sayfayý görme yetkisine sahip deðilsiniz!", "Alert");
            return;
        }

        InitDTColumns(this.DTColumns.Table);
        InitDTRoles(this.DTRoles.Table);
        InitDTUsers(this.DTUsers.Table);

        this.PageAlt.ActiveTabPage = this.PageAlt.TabPages.FindByName("TabColumns");

        this.Title = "Sipariþ Kayýt Güvenlik - Yeni";
        Guid id = Guid.Empty;
        string sID = this.Request.Params["id"].Replace("'", "").Trim();
        if ((sID != null) && (sID != "0"))
        {
            id = new Guid(sID);
            LoadDocument(id);
            this.HiddenId.Value = id.ToString();
        }
        else
        {
            #region kolonlar
            this.DTColumns.Table.Clear();
            SqlCommand cmd = DB.SQL(this.Context, "SELECT t1.* FROM SecurityTableColumns t1 WHERE t1.TableName='IthalatSiparisYukleme' ORDER BY t1.Sayac");
            cmd.CommandTimeout = 1000;
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Guid _RowId = Guid.NewGuid();
                DataRow row = this.DTColumns.Table.NewRow();
                row["ID"] = _RowId;
                //row["SecurityEditId"] = rdr["SecurityEditId"];
                row["TableName"] = rdr["TableName"];
                row["TableCaption"] = rdr["TableCaption"];
                row["ColumnName"] = rdr["ColumnName"];
                row["ColumnCaption"] = rdr["ColumnCaption"];
                row["Select"] = 1;
                row["Insert"] = 1;
                row["Update"] = 1;
                row["Delete"] = 1;
                this.DTColumns.Table.Rows.Add(row);
            }
            rdr.Close();
            this.DTColumns.Table.AcceptChanges();
            #endregion
        }
    }

    private void Menu_ItemClick(object source, MenuItemEventArgs e)
    {
        bool bYeni = true;
        if ((this.HiddenId.Value != null) && (this.HiddenId.Value != "0")) bYeni = false;
        else bYeni = true;

        #region new
        if (e.Item.Name.Equals("new"))
        {
            if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Güvenlik", "Insert"))
            {
                CrmUtils.MessageAlert(this.Page, "Yeni Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "alert");
                return;
            }

            this.Response.Write("<script language='javascript'>{ parent.location.href='./edit.aspx?id=0'; }</script>");
        }
        #endregion
        #region save
        else if (e.Item.Name.Equals("save"))
        {
            if (bYeni)
            {
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Güvenlik", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "alert");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Güvenlik", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!", "alert");
                    return;
                }
            }

            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlýþ bilgi giriþi!", "alert");
                return;
            }

            this.GridColumns.UpdateEdit();
            this.GridRoles.UpdateEdit();
            this.GridUsers.UpdateEdit();

            Guid id = SaveDocument();
            if (id == Guid.Empty)
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "alert");
                return;
            }
            else
            {
                Session["BelgeID"] = id.ToString();
                this.Response.Write("<script language='javascript'>{ parent.opener.location.href=parent.opener.location.href; }</script>");
                this.Response.Write("<script language='javascript'>{ parent.location.href='./edit.aspx?id=" + (String)Session["BelgeID"] + "';}</script>");
            }
        }
        #endregion
        #region savenew
        else if (e.Item.Name.Equals("savenew"))
        {
            if (bYeni)
            {
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Güvenlik", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "alert");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Güvenlik", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!", "alert");
                    return;
                }
            }

            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlýþ bilgi giriþi!", "alert");
                return;
            }

            this.GridColumns.UpdateEdit();
            this.GridRoles.UpdateEdit();
            this.GridUsers.UpdateEdit();

            Guid id = SaveDocument();
            if (id == Guid.Empty)
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "alert");
                return;
            }
            else
            {
                this.Response.Write("<script language='javascript'>{ parent.opener.location.href=parent.opener.location.href; }</script>");

                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Güvenlik", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Yeni Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "alert");
                    return;
                }
                else
                    this.Response.Write("<script language='javascript'>{ parent.location.href='./edit.aspx?id=0';}</script>");
            }
        }
        #endregion
        #region saveclose
        else if (e.Item.Name.Equals("saveclose"))
        {
            if (bYeni)
            {
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Güvenlik", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "alert");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Güvenlik", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!", "alert");
                    return;
                }
            }

            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlýþ bilgi giriþi!", "alert");
                return;
            }

            this.GridColumns.UpdateEdit();
            this.GridRoles.UpdateEdit();
            this.GridUsers.UpdateEdit();

            Guid id = SaveDocument();
            if (id == Guid.Empty)
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "alert");
                return;
            }
            else
            {
                this.Response.Write("<script language='javascript'>{ parent.opener.location.href=parent.opener.location.href; }</script>");
                this.Response.Write("<script language='javascript'>{ parent.close();}</script>");
            }
        }
        #endregion
        #region delete
        else if (e.Item.Name.Equals("delete"))
        {
            if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Güvenlik", "Delete"))
            {
                CrmUtils.MessageAlert(this.Page, "Silme iþlemi yapma yetkisine sahip deðilsiniz!", "alert");
                return;
            }

            if (!DeleteDocument())
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "alert");
                return;
            }
            else
            {
                this.Response.Write("<script language='javascript'>{ parent.opener.location.href=parent.opener.location.href; }</script>");
                this.Response.Write("<script language='javascript'>{ parent.close();}</script>");
            }
        }
        #endregion
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    private void LoadDocument(Guid id)
    {
        #region ana
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM SecurityEdit WHERE Id=@Id");
        DB.AddParam(cmd, "@Id", id);
        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        if (!rdr.Read())
        {
            this.Response.StatusCode = 500;
            this.Response.End();
            return;
        }
        this.Title = "Sipariþ Kayýt Güvenlik - " + rdr["Aciklama"].ToString();
        this.Aciklama.Value = rdr["Aciklama"];
        rdr.Close();
        #endregion

        #region kolonlar
        this.DTColumns.Table.Clear();
        cmd = DB.SQL(this.Context, "SELECT t1.* FROM SecurityEditColumns t1 WHERE t1.SecurityEditId=@SecurityEditId ORDER BY t1.Sayac");
        DB.AddParam(cmd, "@SecurityEditId", id);
        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTColumns.Table.NewRow();
            row["ID"] = rdr["Id"];
            row["SecurityEditId"] = rdr["SecurityEditId"];
            row["TableName"] = rdr["TableName"];
            row["TableCaption"] = rdr["TableCaption"];
            row["ColumnName"] = rdr["ColumnName"];
            row["ColumnCaption"] = rdr["ColumnCaption"];
            row["Select"] = rdr["Select"];
            row["Insert"] = rdr["Insert"];
            row["Update"] = rdr["Update"];
            row["Delete"] = rdr["Delete"];
            this.DTColumns.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTColumns.Table.AcceptChanges();
        #endregion

        #region roller
        this.DTRoles.Table.Clear();
        cmd = DB.SQL(this.Context, "SELECT t1.* FROM SecurityEditRoles t1 WHERE t1.SecurityEditId=@SecurityEditId ORDER BY t1.Sayac");
        DB.AddParam(cmd, "@SecurityEditId", id);
        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTRoles.Table.NewRow();
            row["ID"] = rdr["Id"];
            row["SecurityEditId"] = rdr["SecurityEditId"];
            row["Role"] = rdr["Role"];
            this.DTRoles.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTRoles.Table.AcceptChanges();
        #endregion

        #region kullanýcýlar
        this.DTUsers.Table.Clear();
        cmd = DB.SQL(this.Context, "SELECT t1.*,t2.FirstName,t2.LastName FROM SecurityEditUsers t1 INNER JOIN SecurityUsers t2 ON(t1.UserName=t2.UserName) WHERE t1.SecurityEditId=@SecurityEditId ORDER BY t1.Sayac");
        DB.AddParam(cmd, "@SecurityEditId", id);
        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTUsers.Table.NewRow();
            row["ID"] = rdr["Id"];
            row["SecurityEditId"] = rdr["SecurityEditId"];
            row["UserName"] = rdr["UserName"];
            row["Adi"] = rdr["FirstName"];
            row["Soyadi"] = rdr["LastName"];
            this.DTUsers.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTUsers.Table.AcceptChanges();
        #endregion
    }

    private Guid SaveDocument()
    {
        DB.BeginTrans(this.Context);

        bool _NewDoc = true;
        Guid id = Guid.Empty;
        if (this.HiddenId.Value.Length != 0)
        {
            id = new Guid(this.HiddenId.Value);
            _NewDoc = false;
        }

        SqlCommand cmd = null;
        StringBuilder sb = new StringBuilder();

        #region ana
        if (id == Guid.Empty)
        {
            #region insert
            StringBuilder sb1 = new StringBuilder("INSERT INTO SecurityEdit(Id,Aciklama,CreatedBy,CreationDate");
            StringBuilder sb2 = new StringBuilder(" VALUES(@Id,@Aciklama,@CreatedBy,@CreationDate");
            sb1.Append(")");
            sb2.Append(")");
            id = Guid.NewGuid();
            cmd = DB.SQL(this.Context, (sb1.ToString() + sb2.ToString()));
            DB.AddParam(cmd, "@Id", id);
            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
            #endregion
        }
        else
        {
            #region update
            cmd = DB.SQL(this.Context, "UPDATE SecurityEdit SET Aciklama=@Aciklama,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate WHERE Id=@Id");
            DB.AddParam(cmd, "@Id", id);
            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
            #endregion
        }

        #region values
        DB.AddParam(cmd, "@Aciklama", 255, this.Aciklama.Value);
        #endregion

        try
        {
            cmd.CommandTimeout = 1000;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            DB.Rollback(this.Context);
            this.Session["HATA"] = ex.Message.ToString();
            return Guid.Empty;
        }
        #endregion

        #region kolonlar
        try
        {
            cmd = DB.SQL(this.Context, "DELETE FROM SecurityEditColumns WHERE SecurityEditId=@SecurityEditId");
            DB.AddParam(cmd, "@SecurityEditId", id);
            cmd.CommandTimeout = 1000;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            DB.Rollback(this.Context);
            this.Session["HATA"] = ex.Message.ToString();
            return Guid.Empty;
        }
        foreach (DataRow row in this.DTColumns.Table.Rows)
        {
            StringBuilder sb1 = new StringBuilder("INSERT INTO SecurityEditColumns(Id,SecurityEditId,TableName,TableCaption,ColumnName,ColumnCaption,[Select],[Insert],[Update],[Delete],CreatedBy,CreationDate) ");
            StringBuilder sb2 = new StringBuilder("VALUES(@Id,@SecurityEditId,@TableName,@TableCaption,@ColumnName,@ColumnCaption,@Select,@Insert,@Update,@Delete,@CreatedBy,@CreationDate)");
            try
            {
                cmd = DB.SQL(this.Context, (sb1.ToString() + sb2.ToString()));
                DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                DB.AddParam(cmd, "@SecurityEditId", id);
                DB.AddParam(cmd, "@TableName", 100, row["TableName"].ToString());
                DB.AddParam(cmd, "@TableCaption", 100, row["TableCaption"].ToString());
                DB.AddParam(cmd, "@ColumnName", 100, row["ColumnName"].ToString());
                DB.AddParam(cmd, "@ColumnCaption", 100, row["ColumnCaption"].ToString());
                DB.AddParam(cmd, "@Select", Convert.ToInt32(row["Select"]));
                DB.AddParam(cmd, "@Insert", Convert.ToInt32(row["Insert"]));
                DB.AddParam(cmd, "@Update", Convert.ToInt32(row["Update"]));
                DB.AddParam(cmd, "@Delete", Convert.ToInt32(row["Delete"]));
                DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                DB.Rollback(this.Context);
                this.Session["HATA"] = ex.Message.ToString();
                return Guid.Empty;
            }
        }
        #region eski
        //DataTable changes = this.DTColumns.Table.GetChanges();
        //if (changes != null)
        //{
        //    foreach (DataRow row in changes.Rows)
        //    {
        //        switch (row.RowState)
        //        {
        //            case DataRowState.Added:
        //                #region insert
        //                StringBuilder sb1 = new StringBuilder("INSERT INTO SecurityEditColumns(Id,SecurityEditId,TableName,TableCaption,ColumnName,ColumnCaption,[Select],[Insert],[Update],[Delete],CreatedBy,CreationDate) ");
        //                StringBuilder sb2 = new StringBuilder("VALUES(@Id,@SecurityEditId,@TableName,@TableCaption,@ColumnName,@ColumnCaption,@Select,@Insert,@Update,@Delete,@CreatedBy,@CreationDate)");
        //                try
        //                {
        //                    cmd = DB.SQL(this.Context, (sb1.ToString() + sb2.ToString()));
        //                    DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
        //                    DB.AddParam(cmd, "@SecurityEditId", id);
        //                    DB.AddParam(cmd, "@TableName", 100, row["TableName"].ToString());
        //                    DB.AddParam(cmd, "@TableCaption", 100, row["TableCaption"].ToString());
        //                    DB.AddParam(cmd, "@ColumnName", 100, row["ColumnName"].ToString());
        //                    DB.AddParam(cmd, "@ColumnCaption", 100, row["ColumnCaption"].ToString());
        //                    DB.AddParam(cmd, "@Select", Convert.ToInt32(row["Select"]));
        //                    DB.AddParam(cmd, "@Insert", Convert.ToInt32(row["Insert"]));
        //                    DB.AddParam(cmd, "@Update", Convert.ToInt32(row["Update"]));
        //                    DB.AddParam(cmd, "@Delete", Convert.ToInt32(row["Delete"]));
        //                    DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
        //                    DB.AddParam(cmd, "@CreationDate", DateTime.Now);
        //                    cmd.CommandTimeout = 1000;
        //                    cmd.Prepare();
        //                    cmd.ExecuteNonQuery();
        //                }
        //                catch (Exception ex)
        //                {
        //                    DB.Rollback(this.Context);
        //                    this.Session["HATA"] = ex.Message.ToString();
        //                    return Guid.Empty;
        //                }
        //                #endregion
        //                break;
        //            case DataRowState.Modified:
        //                #region update
        //                sb = new StringBuilder("UPDATE SecurityEditColumns ");
        //                sb.Append("SET SecurityEditId=@SecurityEditId,TableName=@TableName,TableCaption=@TableCaption,ColumnName=@ColumnName,ColumnCaption=@ColumnCaption");
        //                sb.Append(",[Select]=@Select,[Insert]=@Insert,[Update]=@Update,[Delete]=@Delete,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate ");
        //                sb.Append("WHERE Id=@Id");
        //                try
        //                {
        //                    cmd = DB.SQL(this.Context, sb.ToString());
        //                    DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
        //                    DB.AddParam(cmd, "@SecurityEditId", id);
        //                    DB.AddParam(cmd, "@TableName", 100, row["TableName"].ToString());
        //                    DB.AddParam(cmd, "@TableCaption", 100, row["TableCaption"].ToString());
        //                    DB.AddParam(cmd, "@ColumnName", 100, row["ColumnName"].ToString());
        //                    DB.AddParam(cmd, "@ColumnCaption", 100, row["ColumnCaption"].ToString());
        //                    DB.AddParam(cmd, "@Select", Convert.ToInt32(row["Select"]));
        //                    DB.AddParam(cmd, "@Insert", Convert.ToInt32(row["Insert"]));
        //                    DB.AddParam(cmd, "@Update", Convert.ToInt32(row["Update"]));
        //                    DB.AddParam(cmd, "@Delete", Convert.ToInt32(row["Delete"]));
        //                    DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
        //                    DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
        //                    cmd.CommandTimeout = 1000;
        //                    cmd.Prepare();
        //                    cmd.ExecuteNonQuery();
        //                }
        //                catch (Exception ex)
        //                {
        //                    DB.Rollback(this.Context);
        //                    this.Session["HATA"] = ex.Message.ToString();
        //                    return Guid.Empty;
        //                }
        //                #endregion
        //                break;
        //            case DataRowState.Deleted:
        //                #region delete
        //                try
        //                {
        //                    cmd = DB.SQL(this.Context, "DELETE FROM SecurityEditColumns WHERE Id=@Id");
        //                    DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
        //                    cmd.CommandTimeout = 1000;
        //                    cmd.Prepare();
        //                    cmd.ExecuteNonQuery();
        //                }
        //                catch (Exception ex)
        //                {
        //                    DB.Rollback(this.Context);
        //                    this.Session["HATA"] = ex.Message.ToString();
        //                    return Guid.Empty;
        //                }
        //                #endregion
        //                break;
        //        }
        //    }
        //} 
        #endregion
        #endregion

        #region roller
        DataTable changes = this.DTRoles.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        #region insert
                        StringBuilder sb1 = new StringBuilder("INSERT INTO SecurityEditRoles(Id,SecurityEditId,Role,CreatedBy,CreationDate) ");
                        StringBuilder sb2 = new StringBuilder("VALUES(@Id,@SecurityEditId,@Role,@CreatedBy,@CreationDate)");
                        try
                        {
                            cmd = DB.SQL(this.Context, (sb1.ToString() + sb2.ToString()));
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@SecurityEditId", id);
                            DB.AddParam(cmd, "@Role", 60, row["Role"].ToString());
                            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.CommandTimeout = 1000;
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            return Guid.Empty;
                        }
                        #endregion
                        break;
                    case DataRowState.Modified:
                        #region update
                        sb = new StringBuilder("UPDATE SecurityEditRoles ");
                        sb.Append("SET SecurityEditId=@SecurityEditId,Role=@Role,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate ");
                        sb.Append("WHERE Id=@Id");
                        try
                        {
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@SecurityEditId", id);
                            DB.AddParam(cmd, "@Role", 60, row["Role"].ToString());
                            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            cmd.CommandTimeout = 1000;
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            return Guid.Empty;
                        }
                        #endregion
                        break;
                    case DataRowState.Deleted:
                        #region delete
                        try
                        {
                            cmd = DB.SQL(this.Context, "DELETE FROM SecurityEditRoles WHERE Id=@Id");
                            DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.CommandTimeout = 1000;
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            return Guid.Empty;
                        }
                        #endregion
                        break;
                }
            }
        }
        #endregion

        #region kullanýcýlar
        changes = this.DTUsers.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        #region insert
                        StringBuilder sb1 = new StringBuilder("INSERT INTO SecurityEditUsers(Id,SecurityEditId,UserName,CreatedBy,CreationDate) ");
                        StringBuilder sb2 = new StringBuilder("VALUES(@Id,@SecurityEditId,@UserName,@CreatedBy,@CreationDate)");
                        try
                        {
                            cmd = DB.SQL(this.Context, (sb1.ToString() + sb2.ToString()));
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@SecurityEditId", id);
                            DB.AddParam(cmd, "@UserName", 60, row["UserName"].ToString());
                            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.CommandTimeout = 1000;
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            return Guid.Empty;
                        }
                        #endregion
                        break;
                    case DataRowState.Modified:
                        #region update
                        sb = new StringBuilder("UPDATE SecurityEditUsers ");
                        sb.Append("SET SecurityEditId=@SecurityEditId,UserName=@UserName,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate ");
                        sb.Append("WHERE Id=@Id");
                        try
                        {
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@SecurityEditId", id);
                            DB.AddParam(cmd, "@UserName", 60, row["UserName"].ToString());
                            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            cmd.CommandTimeout = 1000;
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            return Guid.Empty;
                        }
                        #endregion
                        break;
                    case DataRowState.Deleted:
                        #region delete
                        try
                        {
                            cmd = DB.SQL(this.Context, "DELETE FROM SecurityEditUsers WHERE Id=@Id");
                            DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.CommandTimeout = 1000;
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            return Guid.Empty;
                        }
                        #endregion
                        break;
                }
            }
        }
        #endregion

        DB.Commit(this.Context);
        this.HiddenId.Value = id.ToString();
        return id;
    }

    private bool DeleteDocument()
    {
        string sID = this.HiddenId.Value;

        if (sID != null)
        {
            DB.BeginTrans(this.Context);

            SqlCommand cmd = null;
            Guid id = Guid.Empty;
            id = new Guid(this.HiddenId.Value);

            try
            {
                cmd = DB.SQL(this.Context, "DELETE FROM SecurityEditUsers WHERE SecurityEditId=@Id");
                DB.AddParam(cmd, "@Id", id);
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd = DB.SQL(this.Context, "DELETE FROM SecurityEditRoles WHERE SecurityEditId=@Id");
                DB.AddParam(cmd, "@Id", id);
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd = DB.SQL(this.Context, "DELETE FROM SecurityEditColumns WHERE SecurityEditId=@Id");
                DB.AddParam(cmd, "@Id", id);
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd = DB.SQL(this.Context, "DELETE FROM SecurityEdit WHERE Id=@Id");
                DB.AddParam(cmd, "@Id", id);
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                DB.Rollback(this.Context);
                this.Session["HATA"] = ex.Message.ToString();
                return false;
            }

            DB.Commit(this.Context);
        }
        return true;
    }

    protected void CallbackSearchBrowser_Callback(object source, CallbackEventArgs e)
    {
        e.Result = "";
        Session["SearchBrowser"] = null;
        DataTable dtQuery = DB.SqlQuery();
        DataTable dtParameters = DB.SqlQueryParameters();
        DataTable dtFields = DB.SqlQueryFields();
        DataTable dtProcedure = DB.SqlQuery();
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            string[] parameters = e.Parameter.Split('|');
            StringBuilder sb = new StringBuilder();
            Hashtable htSearchBrowser = new Hashtable();
            switch (parameters[0].Trim())
            {
                #region anket rol
                case "GridRoles_Role":
                    dtQuery.Rows.Add("SELECT RoleId,Role FROM SecurityRoles ORDER BY Role");
                    dtFields.Rows.Add("RoleId", "RoleId", 50, false);
                    dtFields.Rows.Add("Role", "Rol", 100, true);
                    htSearchBrowser.Add("Title", "ROLLER");
                    htSearchBrowser.Add("Query", dtQuery);
                    htSearchBrowser.Add("dtProcedure", dtProcedure);
                    htSearchBrowser.Add("Parameters", dtParameters);
                    htSearchBrowser.Add("Fields", dtFields);
                    htSearchBrowser.Add("KeyField", "RoleId");
                    break;
                #endregion
                #region anket kullanýcý
                case "GridUsers_UserName":
                    dtQuery.Rows.Add("SELECT UserId,UserName,FirstName,LastName FROM SecurityUsers ORDER BY FirstName");
                    dtFields.Rows.Add("UserId", "UserId", 50, false);
                    dtFields.Rows.Add("UserName", "Kullanýcý Adý", 100, true);
                    dtFields.Rows.Add("FirstName", "Adý", 100, true);
                    dtFields.Rows.Add("LastName", "Soyadý", 100, true);
                    htSearchBrowser.Add("Title", "KULLANICILAR");
                    htSearchBrowser.Add("Query", dtQuery);
                    htSearchBrowser.Add("Parameters", dtParameters);
                    htSearchBrowser.Add("Fields", dtFields);
                    htSearchBrowser.Add("KeyField", "UserId");
                    break;
                #endregion
                default:
                    break;
            }
            Session["SearchBrowser"] = htSearchBrowser;
            e.Result = parameters[0].Trim();
        }
    }

    protected void CallbackGenel_Callback(object source, CallbackEventArgs e)
    {
        e.Result = "";
        StringBuilder sb = new StringBuilder();
        SqlCommand cmd;
        IDataReader rdr;
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            string cid = ConfigurationManager.AppSettings["cid"];
            string[] parameters = e.Parameter.Split('|');
            switch (parameters[0].Trim())
            {
                case "GridRoles_Role":
                    #region anket rol
                    cmd = DB.SQL(this.Context, "SELECT RoleId,Role FROM SecurityRoles WHERE RoleId=@RoleId");
                    DB.AddParam(cmd, "@RoleId", (new Guid(parameters[1].Trim())));
                    cmd.Prepare();
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        sb = new StringBuilder("|" + parameters[0].Trim());
                        sb.Append("|" + rdr["RoleId"].ToString());
                        sb.Append("|" + rdr["Role"].ToString());
                        e.Result = sb.ToString();
                    }
                    rdr.Close();
                    #endregion
                    break;
                case "GridUsers_UserName":
                    #region anket kullanýcý
                    cmd = DB.SQL(this.Context, "SELECT UserId,UserName,FirstName,LastName FROM SecurityUsers WHERE UserId=@UserId");
                    DB.AddParam(cmd, "@UserId", (new Guid(parameters[1].Trim())));
                    cmd.Prepare();
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        sb = new StringBuilder("|" + parameters[0].Trim());
                        sb.Append("|" + rdr["UserId"].ToString());
                        sb.Append("|" + rdr["UserName"].ToString());
                        sb.Append("|" + rdr["FirstName"].ToString());
                        sb.Append("|" + rdr["LastName"].ToString());
                        e.Result = sb.ToString();
                    }
                    rdr.Close();
                    #endregion
                    break;
                default:
                    break;
            }
        }
    }

    private void InitDTColumns(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("SecurityEditId", typeof(Guid));
        dt.Columns.Add("TableName", typeof(string));
        dt.Columns.Add("TableCaption", typeof(string));
        dt.Columns.Add("ColumnName", typeof(string));
        dt.Columns.Add("ColumnCaption", typeof(string));
        dt.Columns.Add("Select", typeof(int));
        dt.Columns.Add("Insert", typeof(int));
        dt.Columns.Add("Update", typeof(int));
        dt.Columns.Add("Delete", typeof(int));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitDTRoles(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("SecurityEditId", typeof(Guid));
        dt.Columns.Add("Role", typeof(string));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitDTUsers(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("SecurityEditId", typeof(Guid));
        dt.Columns.Add("UserName", typeof(string));
        dt.Columns.Add("Adi", typeof(string));
        dt.Columns.Add("Soyadi", typeof(string));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    protected void GridColumns_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (!this.GridColumns.IsEditing) return;

        switch (e.Column.FieldName)
        {
            case "col_name":
                break;
        }
    }

    protected void GridColumns_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        //if (e.NewValues.Values.Count > 0)
        //{
        //    if (e.NewValues["FirmaId"] == null) e.NewValues["FirmaId"] = DBNull.Value;
        //}
    }

    protected void GridColumns_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        //if (e.NewValues.Values.Count > 0)
        //{
        //    if (e.NewValues["FirmaId"] == null) e.NewValues["FirmaId"] = DBNull.Value;
        //}
    }

    protected void GridColumns_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        //if (e.NewValues["Marka"] == null)
        //{
        //    e.RowError = "Lütfen Marka alanýný boþ býrakmayýnýz...";
        //    return;
        //}
    }

    protected void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        ASPxGridView grid = (ASPxGridView)sender;
        DataTable changes = null;
        if (grid.ID == "GridColumns") changes = this.DTColumns.Table.GetChanges();
        if (grid.ID == "GridRoles") changes = this.DTRoles.Table.GetChanges();
        if (grid.ID == "GridUsers") changes = this.DTUsers.Table.GetChanges();
        if (changes != null) e.Properties["cpIsDirty"] = true;
        else e.Properties["cpIsDirty"] = false;
    }

    protected void GridRoles_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (!this.GridRoles.IsEditing) return;

        switch (e.Column.FieldName)
        {
            case "col_name":
                break;
        }
    }

    protected void GridRoles_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        //if (e.NewValues.Values.Count > 0)
        //{
        //    if (e.NewValues["Role"] == null) e.NewValues["Role"] = DBNull.Value;
        //}
    }

    protected void GridRoles_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        //if (e.NewValues.Values.Count > 0)
        //{
        //    if (e.NewValues["Role"] == null) e.NewValues["Role"] = DBNull.Value;
        //}
    }

    protected void GridRoles_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["Role"] == null)
        {
            e.RowError = "Lütfen Rol alanýný boþ býrakmayýnýz...";
            return;
        }
    }

    protected void GridUsers_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (!this.GridUsers.IsEditing) return;

        switch (e.Column.FieldName)
        {
            case "col_name":
                break;
        }
    }

    protected void GridUsers_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        //if (e.NewValues.Values.Count > 0)
        //{
        //    if (e.NewValues["UserName"] == null) e.NewValues["UserName"] = DBNull.Value;
        //}
    }

    protected void GridUsers_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        //if (e.NewValues.Values.Count > 0)
        //{
        //    if (e.NewValues["UserName"] == null) e.NewValues["UserName"] = DBNull.Value;
        //}
    }

    protected void GridUsers_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["UserName"] == null)
        {
            e.RowError = "Lütfen Kullanýcý Adý alanýný boþ býrakmayýnýz...";
            return;
        }
    }
}
