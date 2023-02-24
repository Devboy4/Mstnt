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

public partial class admin_User_edit : System.Web.UI.Page
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
        this.menu.ItemClick += new MenuItemEventHandler(Menu_ItemClick);
        if (this.IsPostBack || this.IsCallback) return;

        //if (!Security.CheckPermission(this.Context, "Kullanici Rol", "Select") 
        //    && !Security.CheckPermission(this.Context, "Kullanici Izin", "Select"))
        if (!Security.CheckPermission(this.Context, "Kullanici", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#e6e6fa'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        SetGridPermission();

        InitGridUserRoles(this.DTUserRoles.Table);
        InitGridUserPermissions(this.DTUserPermissions.Table);
        //InitGridDTUserGrup(this.DTUserGrup.Table);
        //InitGridDTUserGrupList(this.DTUserGrupList.Table);
        InitGridDTAllowedProject(this.DTAllowedProject.Table);
        InitGridDTProje(this.DTProje.Table);
        InitDTSesEmail(this.DTSesEmail.Table);

        FillComboxes();

        this.Title = "Kullanýcý - Yeni";
        this.ASPxPageControl1.ActiveTabPage = this.ASPxPageControl1.TabPages.FindByName("TabPageGenel");

        Guid id = Guid.Empty;

        string sID = this.Request.Params["id"].Replace("'", "").Trim();
        if ((sID != null) && (sID != "0"))
        {
            id = new Guid(sID);
            this.HiddenID.Value = id.ToString();
            LoadDocument(id);
            //this.Username.Enabled = false;
            this.Password.Enabled = false;

        }
        else
        {
            //ASPxPageControl1.TabPages.FindByName("TabPageUserGrup").Enabled = false;
            ASPxPageControl1.TabPages.FindByName("TabPageAllowedProject").Enabled = false;
        }

    }

    private void FillComboxes()
    {
        if ((String)Request.QueryString["id"] != "0")
        {
            StringBuilder sb = new StringBuilder();
            //this.DTUserGrup.Table.Clear();
            //sb = new StringBuilder();
            //sb.Append("SELECT UserGrupID, Adi AS UserGrupName FROM UserGrup ORDER BY Adi");
            //SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
            //cmd.Prepare();
            //IDataReader rdr = cmd.ExecuteReader();
            //while (rdr.Read())
            //{
            //    DataRow row = DTUserGrup.Table.NewRow();
            //    row["UserGrupID"] = rdr["UserGrupID"];
            //    row["UserGrupName"] = rdr["UserGrupName"];
            //    this.DTUserGrup.Table.Rows.Add(row);
            //}
            //this.DTUserGrup.Table.AcceptChanges();
            //rdr.Close();
            this.DTProje.Table.Clear();
            sb = new StringBuilder();
            sb.Append("SELECT ProjeID, Adi AS ProjeName FROM Proje ORDER BY Adi");
            SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DataRow row = this.DTProje.Table.NewRow();
                row["ProjeID"] = rdr["ProjeID"];
                row["ProjeName"] = rdr["ProjeName"];
                this.DTProje.Table.Rows.Add(row);
            }
            this.DTProje.Table.AcceptChanges();
            rdr.Close();
        }
    }

    private void SetGridPermission()
    {
        bool bInsert = Security.CheckPermission(this.Context, "Kullanici Rol", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Kullanici Rol", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Kullanici Rol", "Delete");
        for (int i = 0; i < this.GridUserRoles.Columns.Count; i++)
        {
            if (this.GridUserRoles.Columns[i] is GridViewCommandColumn)
            {
                (this.GridUserRoles.Columns[i] as GridViewCommandColumn).NewButton.Visible = bInsert;
                (this.GridUserRoles.Columns[i] as GridViewCommandColumn).EditButton.Visible = bUpdate;
                (this.GridUserRoles.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
                break;
            }
        }

        bInsert = Security.CheckPermission(this.Context, "Kullanici Izin", "Insert");
        bUpdate = Security.CheckPermission(this.Context, "Kullanici Izin", "Update");
        bDelete = Security.CheckPermission(this.Context, "Kullanici Izin", "Delete");
        for (int i = 0; i < this.GridUserRoles.Columns.Count; i++)
        {
            if (this.GridUserPermissions.Columns[i] is GridViewCommandColumn)
            {
                (this.GridUserPermissions.Columns[i] as GridViewCommandColumn).NewButton.Visible = bInsert;
                (this.GridUserPermissions.Columns[i] as GridViewCommandColumn).EditButton.Visible = bUpdate;
                (this.GridUserPermissions.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
                break;
            }
        }
    }

    private void Menu_ItemClick(object source, MenuItemEventArgs e)
    {
        bool bYeni = true;
        if ((this.HiddenID.Value != null) && (this.HiddenID.Value != "0"))
            bYeni = false;
        else
            bYeni = true;

        if (e.Item.Name.Equals("new"))
        {
            if (!Security.CheckPermission(this.Context, "Kullanici", "Insert"))
            {
                CrmUtils.MessageAlert(this.Page, "Yeni Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "MenuNew");
                return;
            }

            this.Response.Write("<script language='javascript'>{ parent.location.href='./edit.aspx?id=0'; }</script>");
        }

        else if (e.Item.Name.Equals("save"))
        {

            if (bYeni)
            {
                if (!Security.CheckPermission(this.Context, "Kullanici", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "MenuSave1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Kullanici", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!", "MenuSave2");
                    return;
                }
            }

            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlýþ bilgi giriþi!", "MenuSave3");
                return;
            }

            this.GridUserRoles.UpdateEdit();
            this.GridUserPermissions.UpdateEdit();
            //this.gridUserGrupList.UpdateEdit();
            this.GridSesEmail.UpdateEdit();


            Guid id = SaveDocument();
            if (id == Guid.Empty)
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "MenuSave4");
                return;
            }
            else
            {
                Session["BelgeID"] = id.ToString();
                this.Response.Write("<script language='javascript'>{ parent.opener.location.href=parent.opener.location.href'; }</script>");
                this.Response.Write("<script language='javascript'>{ parent.location.href='./edit.aspx?id=" + (String)Session["BelgeID"] + "';}</script>");
            }
        }
        else if (e.Item.Name.Equals("savenew"))
        {
            if (bYeni)
            {
                if (!Security.CheckPermission(this.Context, "Kullanici", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "MenuSaveNew1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Kullanici", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!", "MenuSaveNew2");
                    return;
                }
            }

            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlýþ bilgi giriþi!", "MenuSaveNew3");
                return;
            }


            this.GridUserRoles.UpdateEdit();
            this.GridUserPermissions.UpdateEdit();
            //this.gridUserGrupList.UpdateEdit();
            this.GridSesEmail.UpdateEdit();

            Guid id = SaveDocument();
            if (id == Guid.Empty)
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "MenuSaveNew4");
                return;
            }
            else
            {
                this.Response.Write("<script language='javascript'>{ parent.opener.location.href=parent.opener.location.href; }</script>");
                this.Response.Write("<script language='javascript'>{ parent.location.href='./edit.aspx?id=0';}</script>");
            }
        }
        else if (e.Item.Name.Equals("saveclose"))
        {
            if (bYeni)
            {
                if (!Security.CheckPermission(this.Context, "Kullanici", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "MenuSaveClose1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Kullanici", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!", "MenuSaveClose2");
                    return;
                }
            }

            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlýþ bilgi giriþi!", "MenuSaveClose3");
                return;
            }


            this.GridUserRoles.UpdateEdit();
            this.GridUserPermissions.UpdateEdit();
            //this.gridUserGrupList.UpdateEdit();
            this.GridSesEmail.UpdateEdit();

            Guid id = SaveDocument();
            if (id == Guid.Empty)
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "MenuSaveNew4");
                return;
            }
            else
            {
                this.Response.Write("<script language='javascript'>{ parent.opener.location.href=parent.opener.location.href; }</script>");
                this.Response.Write("<script language='javascript'>{ parent.close();}</script>");
            }
        }
        else if (e.Item.Name.Equals("delete"))
        {
            if (!Security.CheckPermission(this.Context, "Kullanici", "Delete"))
            {
                CrmUtils.MessageAlert(this.Page, "Silme iþlemi yapma yetkisine sahip deðilsiniz!", "MenuDelete1");
                return;
            }

            if (!DeleteDocument())
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "MenuDelete2");
                return;
            }
            else
            {
                this.Response.Write("<script language='javascript'>{ parent.opener.location.href=parent.opener.location.href; }</script>");
                this.Response.Write("<script language='javascript'>{ parent.close();}</script>");
            }
        }
        else if (e.Item.Name.Equals("Passive"))
        {
            if (!Security.CheckPermission(this.Context, "Kullanici", "Delete"))
            {
                CrmUtils.MessageAlert(this.Page, "Silme iþlemi yapma yetkisine sahip deðilsiniz!", "MenuDelete1");
                return;
            }

            if (!Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator"))
            {
                if (Roles.IsUserInRole(this.HiddenUserName.Value, "Administrator") || Roles.IsUserInRole(this.HiddenUserName.Value, "Merkez Yöneticileri"))
                {
                    CrmUtils.MessageAlert(this.Page, "Administrator yetkilisi dýþýnda bir kullanýcý Administrator veya Merkez Yöneticileri Rollerine Sahip bir kullanýcýyý Pasif Yapamaz.!", "MenuDelete3");
                    return;
                }
            }

            if (!SetUserStatu())
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "MenuDelete2");
                return;
            }
            else
            {
                this.Response.Write("<script language='javascript'>{ parent.opener.location.href=parent.opener.location.href; }</script>");
                this.Response.Write("<script language='javascript'>{ parent.close();}</script>");
            }
        }
        else if (e.Item.Name.Equals("changeusername"))
        {
            if (!ChangeUserName())
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "MenuDelete2");
                return;
            }
            else
            {
                this.Response.Write("<script language='javascript'>{ parent.opener.location.href=parent.opener.location.href; }</script>");
                this.Response.Write("<script language='javascript'>{ parent.close();}</script>");
            }
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    private void InitGridUserRoles(DataTable dt)
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

    private void InitGridUserPermissions(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("UserPermissionID", typeof(Guid));
        dt.Columns.Add("UserID", typeof(Guid));
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

    private void InitGridDTUserGrup(DataTable dt)
    {
        dt.Columns.Add("UserGrupID", typeof(Guid));
        dt.Columns.Add("UserGrupName", typeof(string));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["UserGrupID"];
        dt.PrimaryKey = pricols;
    }

    private void InitGridDTUserGrupList(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("UserGrupToUsersID", typeof(Guid));
        dt.Columns.Add("UserGrupID", typeof(Guid));
        dt.Columns.Add("UserID", typeof(Guid));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitGridDTAllowedProject(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("UserAllowedProjectID", typeof(Guid));
        dt.Columns.Add("ProjeID", typeof(Guid));
        dt.Columns.Add("FirmaName", typeof(string));
        dt.Columns.Add("UserID", typeof(Guid));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitGridDTProje(DataTable dt)
    {
        dt.Columns.Add("ProjeID", typeof(Guid));
        dt.Columns.Add("ProjeName", typeof(string));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ProjeID"];
        dt.PrimaryKey = pricols;
    }

    private void InitDTSesEmail(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("UserId", typeof(Guid));
        dt.Columns.Add("UserName", typeof(string));
        dt.Columns.Add("EmailId", typeof(Guid));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument(Guid id)
    {

        #region User
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM SecurityUsers WHERE UserID=@UserID");
        DB.AddParam(cmd, "@UserID", id);
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        if (!rdr.Read())
        {
            this.Response.StatusCode = 500;
            this.Response.End();
            return;
        }

        this.Title = "Kullanýcý - " + rdr["FirstName"].ToString() + " " + rdr["LastName"].ToString();

        this.FirstName.Value = rdr["FirstName"];
        this.LastName.Value = rdr["LastName"];
        this.Username.Value = rdr["Username"];
        this.IPUserName.Value = rdr["IPUserName"];
        this.IPPassword.Value = rdr["IPPassword"];
        this.IPDahili.Value = rdr["IPDahili"];
        this.HiddenUserName.Value = this.Username.Value.ToString();
        this.Password.Value = rdr["Password"];
        this.Email.Value = rdr["Email"];
        this.CepTel.Value = rdr["CepTel"];
        this.Department.Value = rdr["Department"];
        this.Savsaklamaal.Checked = (bool)rdr["Savsaklamaal"];
        string ProjeId = rdr["ProjeID"].ToString();
        string FirmaId = rdr["FirmaID"].ToString();
        rdr.Close();
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder();
        sb.Append("Select ProjeId,Adi from Proje Order By Adi");
        data.BindComboBoxes(this.Context, ProjeID, sb.ToString(), "ProjeID", "Adi");
        if ((ProjeId != null) && (ProjeId != "") && (ProjeId != Guid.Empty.ToString()))
            this.ProjeID.SelectedIndex = this.ProjeID.Items.IndexOfValue(new Guid(ProjeId));
        data.BindComboBoxes(this.Context, FirmaID, "Select FirmaId,FirmaName From Firma Order By FirmaName", "FirmaID", "FirmaName");
        if ((FirmaId != null) && (FirmaId != "") && (FirmaId != Guid.Empty.ToString()))
            this.FirmaID.SelectedIndex = this.FirmaID.Items.IndexOfValue(new Guid(FirmaId));
        #endregion

        #region Roller
        this.DTUserRoles.Table.Clear();
        sb = new StringBuilder();
        sb.Append("SELECT t1.* FROM SecurityUserRoles t1, SecurityRoles t2 WHERE t1.RoleID=t2.RoleID and t1.UserID=@UserID ORDER BY t2.Role");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@UserID", id);
        cmd.Prepare();

        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTUserRoles.Table.NewRow();

            row["ID"] = rdr["UserRoleID"];
            row["UserRoleID"] = rdr["UserRoleID"];
            row["UserID"] = rdr["UserID"];
            row["RoleID"] = rdr["RoleID"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];

            this.DTUserRoles.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTUserRoles.Table.AcceptChanges();
        #endregion

        #region Ýzinler
        this.DTUserPermissions.Table.Clear();
        sb = new StringBuilder();

        sb.Append("SELECT t1.* FROM SecurityUserPermissions t1, SecurityObjects t2 WHERE t1.ObjectID=t2.ObjectID and t1.UserID=@UserID ORDER BY t2.ObjectDescription");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@UserID", id);
        cmd.Prepare();

        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTUserPermissions.Table.NewRow();

            row["ID"] = rdr["UserPermissionID"];
            row["UserPermissionID"] = rdr["UserPermissionID"];
            row["UserID"] = rdr["UserID"];
            row["ObjectID"] = rdr["ObjectID"];
            row["Select"] = rdr["Select"];
            row["Insert"] = rdr["Insert"];
            row["Update"] = rdr["Update"];
            row["Delete"] = rdr["Delete"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];

            this.DTUserPermissions.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTUserPermissions.Table.AcceptChanges();
        #endregion

        #region Kullanýcý Grubu

        //sb = new StringBuilder();
        //sb.Append("SELECT * FROM UserGrupToUsers WHERE UserID=@UserID");
        //cmd = DB.SQL(this.Context, sb.ToString());
        //DB.AddParam(cmd, "@UserID", id);

        //rdr = cmd.ExecuteReader();

        //while (rdr.Read())
        //{
        //    DataRow row = this.DTUserGrupList.Table.NewRow();

        //    row["ID"] = rdr["UserGrupToUsersID"];
        //    row["UserGrupToUsersID"] = rdr["UserGrupToUsersID"];
        //    row["UserID"] = rdr["UserID"];
        //    row["UserGrupID"] = rdr["UserGrupID"];
        //    row["CreatedBy"] = rdr["CreatedBy"];
        //    row["CreationDate"] = rdr["CreationDate"];

        //    this.DTUserGrupList.Table.Rows.Add(row);
        //}
        //rdr.Close();
        //this.DTUserGrupList.Table.AcceptChanges();
        #endregion

        #region Proje izin

        sb = new StringBuilder();
        sb.Append("SELECT t1.*,t3.FirmaName FROM UserAllowedProject t1 Left Outer Join Proje t2 On t1.ProjeID=t2.ProjeID ");
        sb.Append("Left Outer Join Firma t3 On t2.FirmaID=t3.FirmaID ");
        sb.Append("WHERE UserID=@UserID");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@UserID", id);

        rdr = cmd.ExecuteReader();
        this.DTAllowedProject.Table.Clear();
        while (rdr.Read())
        {
            DataRow row = this.DTAllowedProject.Table.NewRow();

            row["ID"] = rdr["UserAllowedProjectID"];
            row["UserAllowedProjectID"] = rdr["UserAllowedProjectID"];
            row["FirmaName"] = rdr["FirmaName"];
            row["UserID"] = rdr["UserID"];
            row["ProjeID"] = rdr["ProjeID"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["CreationDate"] = rdr["CreationDate"];

            this.DTAllowedProject.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTAllowedProject.Table.AcceptChanges();
        #endregion

        #region ses email
        this.DTSesEmail.Table.Clear();
        cmd = DB.SQL(this.Context, "SELECT * FROM UserSesEmail WHERE UserId=@UserId");
        DB.AddParam(cmd, "@UserId", id);
        cmd.CommandTimeout = 1000;
        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow[] rows = this.DTSesEmail.Table.Select("ID='" + rdr["Id"].ToString() + "' OR Email='" + rdr["Email"].ToString() + "'");
            if (rows.Length == 0)
            {
                DataRow row = this.DTSesEmail.Table.NewRow();
                row["ID"] = rdr["Id"];
                row["UserId"] = rdr["UserId"];
                row["UserName"] = rdr["UserName"];
                row["EmailId"] = rdr["EmailId"];
                row["Email"] = rdr["Email"];
                row["CreatedBy"] = rdr["CreatedBy"];
                row["CreationDate"] = rdr["CreationDate"];
                row["ModifiedBy"] = rdr["ModifiedBy"];
                row["ModificationDate"] = rdr["ModificationDate"];
                this.DTSesEmail.Table.Rows.Add(row);
            }
        }
        rdr.Close();
        this.DTSesEmail.Table.AcceptChanges();
        #endregion
    }

    private bool ChangeUserName()
    {
        try
        {
            if (this.Request.Params["id"] == "0")
            {
                this.Session["HATA"] = "Yeni oluþturulan bir kullanýcýnýn adý deðiþtirilemez.!";
                return false;
            }
            if (this.HiddenUserName.Value != this.Username.Value.ToString())
            {
                if (!Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator"))
                {
                    if (Roles.IsUserInRole(this.HiddenUserName.Value, "Administrator") || Roles.IsUserInRole(this.HiddenUserName.Value, "Merkez Yöneticileri"))
                    {
                        this.Session["HATA"] = "Administrator yetkilisi dýþýnda bir kullanýcý Administrator veya Merkez Yöneticileri Rollerine Sahip bir kullanýcý adýný deðiþtiremez.!";
                        return false;
                    }
                }
                SqlCommand cmd = DB.SQL(this.Context, "EXEC aspnet_Membership_ChangeUserName @OldUserName,@NewUserName");
                DB.AddParam(cmd, "@OldUserName", 150, this.HiddenUserName.Value);
                DB.AddParam(cmd, "@NewUserName", 150, this.Username.Value);
                cmd.Prepare();
                cmd.CommandTimeout = 5000;
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            this.Session["HATA"] = "HATA:" + ex.Message.ToString();
            return false;
        }
        return true;
    }

    private Guid SaveDocument()
    {
        DB.BeginTrans(this.Context);
        Session["Hata"] = null;
        Guid id = Guid.Empty;
        bool IsNewDoc = true;
        if (this.HiddenID.Value.Length != 0)
        {
            id = new Guid(this.HiddenID.Value);
            IsNewDoc = false;
        }

        Guid gUserID = Guid.Empty;
        string email = null;

        SqlCommand cmd = null;
        StringBuilder sb = new StringBuilder();

        if (id == Guid.Empty)
        {
            #region Membership.CreateUser
            if (((this.Username.Value != null) && (this.Username.Value.ToString() != ""))
                && ((this.Password.Value != null) && (this.Password.Value.ToString() != "")))
            {
                if ((this.Email.Value != null) && (this.Email.Value.ToString() != ""))
                    email = this.Email.Value.ToString();
                else
                    email = this.Username.Value.ToString();

                MembershipCreateStatus status;
                Membership.CreateUser(this.Username.Value.ToString(), this.Password.Value.ToString(), email, null, null, true, out status);
                if (status != MembershipCreateStatus.Success)
                {
                    switch (status)
                    {
                        case MembershipCreateStatus.InvalidUserName:
                            this.Session["HATA"] = "'" + this.Username.Value.ToString() + "' kullanýcý adý geçersiz!'";
                            break;
                        case MembershipCreateStatus.DuplicateUserName:
                            this.Session["HATA"] = "'" + this.Username.Value.ToString() + "' kullanýcý adý daha önce kullanýlmýþ!'";
                            break;
                        case MembershipCreateStatus.InvalidPassword:
                            this.Session["HATA"] = "'" + this.Password.Value.ToString() + "' kullanýcý þifresi geçersiz!'";
                            break;
                        case MembershipCreateStatus.DuplicateEmail:
                            this.Session["HATA"] = "'" + email + "' e-posta adresi daha önce kullanýlmýþ!'";
                            break;
                        default:
                            this.Session["HATA"] = "'" + this.Username.Value.ToString() + "' kullanýcýsý oluþturulmadý!'";
                            break;
                    }

                    DB.Rollback(this.Context);
                    return Guid.Empty;
                    //this.Response.Redirect("./../CRM/hata.aspx");
                }

                cmd = DB.SQL(this.Context, "SELECT UserId FROM aspnet_Membership WHERE Email=@Email");
                DB.AddParam(cmd, "@Email", 255, email);
                cmd.Prepare();
                gUserID = (Guid)cmd.ExecuteScalar();
            }
            else
            {
                DB.Rollback(this.Context);
                this.Session["HATA"] = "Kullanýcý Adý veya Þifresi girilmemiþ!'";
                return Guid.Empty;
                //this.Response.Redirect("./../CRM/hata.aspx");
            }
            #endregion
        }
        else
        {
            #region Membership.UpdateUser
            //string sOldPassword = "";
            string sOldEmail = "";

            cmd = DB.SQL(this.Context, "SELECT Password,Email FROM SecurityUsers WHERE UserID=@UserID");
            DB.AddParam(cmd, "@UserID", id);
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                //sOldPassword = (string)rdr["Password"];
                sOldEmail = (string)rdr["Email"];
            }
            rdr.Close();

            string sNewPassword = (string)this.Password.Value.ToString();
            string sNewEmail = (this.Email.Value == null ? (string)this.HiddenUserName.Value.ToString() : (string)this.Email.Value.ToString());
            email = sNewEmail.ToString();

            if ((sOldEmail != sNewEmail))
            {

                MembershipUserCollection users = Membership.FindUsersByName(this.HiddenUserName.Value.ToString());
                if (users.Count != 0)
                {
                    //if (sOldPassword != sNewPassword)
                    //{
                    //    if (!(users[this.Username.Value.ToString()].ChangePassword(sOldPassword, sNewPassword)))
                    //    {
                    //        this.Session["HATA"] = "'" + this.Username.Value.ToString() + "' kullanýcý þifresi deðiþtirilemedi!'";
                    //        DB.Rollback(this.Context);
                    //        return Guid.Empty;
                    //        //this.Response.Redirect("./../CRM/hata.aspx");
                    //    }
                    //}
                    if (sOldEmail != sNewEmail)
                        users[this.HiddenUserName.Value.ToString()].Email = sNewEmail;
                    Membership.UpdateUser(users[this.HiddenUserName.Value.ToString()]);
                }
                else
                {
                    DB.Rollback(this.Context);
                    this.Session["HATA"] = "'" + this.HiddenUserName.Value.ToString() + "' kullanýcýsý bulunamadý!'";
                    return Guid.Empty;
                    //this.Response.Redirect("./../CRM/hata.aspx");
                }
            }
            #endregion
        }


        if (id == Guid.Empty)
        {
            #region INSERT INTO SecurityUsers
            sb = new StringBuilder();

            sb.Append("INSERT INTO SecurityUsers(UserID,UserName,Department,Password,FirstName,LastName,Email,ProjeID,FirmaID,CreatedBy,CreationDate,Savsaklamaal,CepTel,IPUserName,IPPassword,IPDahili) ");
            sb.Append("VALUES(@UserID,@UserName,@Department,@Password,@FirstName,@LastName,@Email,@ProjeID,@FirmaID,@CreatedBy,@CreationDate,@Savsaklamaal,@CepTel,@IPUserName,@IPPassword,@IPDahili)");

            id = gUserID;
            cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@UserID", id);
            if (this.Savsaklamaal.Checked)
                DB.AddParam(cmd, "@Savsaklamaal", 1);
            else
                DB.AddParam(cmd, "@Savsaklamaal", 0);
            DB.AddParam(cmd, "@UserName", 100, this.Username.Value.ToString());
            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
            #endregion
        }
        else
        {
            #region UPDATE SecurityUsers
            sb.Append("UPDATE SecurityUsers SET ");
            sb.Append("Password=@Password,FirstName=@FirstName,LastName=@LastName,Department=@Department,Email=@Email,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate");
            sb.Append(",ProjeID=@ProjeID,FirmaID=@FirmaID,Savsaklamaal=@Savsaklamaal,CepTel=@CepTel,IPUserName=@IPUserName,IPPassword=@IPPassword,IPDahili=@IPDahili ");
            sb.Append("WHERE UserID=@UserID");

            cmd = DB.SQL(this.Context, sb.ToString());

            DB.AddParam(cmd, "@UserID", id);
            if (this.Savsaklamaal.Checked)
                DB.AddParam(cmd, "@Savsaklamaal", 1);
            else
                DB.AddParam(cmd, "@Savsaklamaal", 0);
            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
            #endregion
        }

        #region Values
        DB.AddParam(cmd, "@Department", 255, this.Department.Value);
        DB.AddParam(cmd, "@CepTel", 20, this.CepTel.Value);
        DB.AddParam(cmd, "@Password", 100, this.Password.Value);
        DB.AddParam(cmd, "@FirstName", 60, this.FirstName.Value);
        DB.AddParam(cmd, "@LastName", 60, this.LastName.Value);
        DB.AddParam(cmd, "@Email", 255, email);
        DB.AddParam(cmd, "@IPUserName", 150, this.IPUserName.Value);
        DB.AddParam(cmd, "@IPPassword", 50, this.IPPassword.Value);
        DB.AddParam(cmd, "@IPDahili", 20, this.IPDahili.Value);
        if ((this.ProjeID.SelectedIndex >= 0) && (this.ProjeID.Value.ToString() != Guid.Empty.ToString()))
        {
            Guid tmpID = new Guid(this.ProjeID.Value.ToString());
            DB.AddParam(cmd, "@ProjeID", tmpID);
        }
        else
            DB.AddParam(cmd, "@ProjeID", SqlDbType.UniqueIdentifier);
        if ((this.FirmaID.SelectedIndex >= 0) && (this.FirmaID.Value.ToString() != Guid.Empty.ToString()))
        {
            Guid tmpID = new Guid(this.FirmaID.Value.ToString());
            DB.AddParam(cmd, "@FirmaID", tmpID);
        }
        else
            DB.AddParam(cmd, "@FirmaID", SqlDbType.UniqueIdentifier);
        #endregion

        cmd.Prepare();
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            if (IsNewDoc)
                Membership.DeleteUser(this.Username.Value.ToString());
            DB.Rollback(this.Context);
            this.Session["HATA"] = ex.Message.ToString();
            return Guid.Empty;
        }

        if (id != Guid.Empty)
        {
            cmd = DB.SQL(this.Context, "SELECT UserName FROM SecurityUsers WHERE UserID=@UserID");
            DB.AddParam(cmd, "@UserID", id);
            cmd.Prepare();
            string sUserName = (string)cmd.ExecuteScalar();

            #region Roller
            DataTable changes = this.DTUserRoles.Table.GetChanges();
            if (changes != null)
            {
                string sRole = null;
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
                            DB.AddParam(cmd, "@UserID", id);
                            DB.AddParam(cmd, "@RoleID", (Guid)row["RoleID"]);
                            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();

                            cmd = DB.SQL(this.Context, "SELECT Role FROM SecurityRoles WHERE RoleID=@RoleID");
                            DB.AddParam(cmd, "@RoleID", (Guid)row["RoleID"]);
                            cmd.Prepare();
                            sRole = (string)cmd.ExecuteScalar();

                            if (!Roles.RoleExists(sRole))
                            {
                                DB.Rollback(this.Context);
                                this.Session["HATA"] = sRole + " rolü sistemde tanýmlý deðil!";
                                return Guid.Empty;
                            }

                            if (!Roles.IsUserInRole(sUserName, sRole))
                            {
                                try
                                {
                                    Roles.AddUserToRole(sUserName, sRole);
                                }
                                catch (Exception ex)
                                {
                                    DB.Rollback(this.Context);
                                    this.Session["HATA"] = ex.Message.ToString();
                                    return Guid.Empty;
                                }
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
                            DB.AddParam(cmd, "@UserID", id);
                            DB.AddParam(cmd, "@RoleID", (Guid)row["RoleID"]);
                            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();

                            cmd = DB.SQL(this.Context, "SELECT Role FROM SecurityRoles WHERE RoleID=@RoleID");
                            DB.AddParam(cmd, "@RoleID", (Guid)row["RoleID", DataRowVersion.Original]);
                            cmd.Prepare();
                            sRole = (string)cmd.ExecuteScalar();

                            if (!Roles.RoleExists(sRole))
                            {
                                DB.Rollback(this.Context);
                                this.Session["HATA"] = sRole + " rolü sistemde tanýmlý deðil!";
                                return Guid.Empty;
                            }

                            if (Roles.IsUserInRole(sUserName, sRole))
                            {
                                try
                                {
                                    Roles.RemoveUserFromRole(sUserName, sRole);
                                }
                                catch (Exception ex)
                                {
                                    DB.Rollback(this.Context);
                                    this.Session["HATA"] = ex.Message.ToString();
                                    return Guid.Empty;
                                }
                            }

                            cmd = DB.SQL(this.Context, "SELECT Role FROM SecurityRoles WHERE RoleID=@RoleID");
                            DB.AddParam(cmd, "@RoleID", (Guid)row["RoleID"]);
                            cmd.Prepare();
                            sRole = (string)cmd.ExecuteScalar();

                            if (!Roles.RoleExists(sRole))
                            {
                                DB.Rollback(this.Context);
                                this.Session["HATA"] = sRole + " rolü sistemde tanýmlý deðil!";
                                return Guid.Empty;
                            }

                            if (!Roles.IsUserInRole(sUserName, sRole))
                            {
                                try
                                {
                                    Roles.AddUserToRole(sUserName, sRole);
                                }
                                catch (Exception ex)
                                {
                                    DB.Rollback(this.Context);
                                    this.Session["HATA"] = ex.Message.ToString();
                                    return Guid.Empty;
                                }
                            }

                            break;
                        case DataRowState.Deleted:
                            cmd = DB.SQL(this.Context, "DELETE FROM SecurityUserRoles WHERE UserRoleID=@UserRoleID");
                            DB.AddParam(cmd, "@UserRoleID", (Guid)row["UserRoleID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();

                            cmd = DB.SQL(this.Context, "SELECT Role FROM SecurityRoles WHERE RoleID=@RoleID");
                            DB.AddParam(cmd, "@RoleID", (Guid)row["RoleID", DataRowVersion.Original]);
                            cmd.Prepare();
                            sRole = (string)cmd.ExecuteScalar();

                            if (!Roles.RoleExists(sRole))
                            {
                                DB.Rollback(this.Context);
                                this.Session["HATA"] = sRole + " rolü sistemde tanýmlý deðil!";
                                return Guid.Empty;
                            }

                            if (Roles.IsUserInRole(sUserName, sRole))
                            {
                                try
                                {
                                    Roles.RemoveUserFromRole(sUserName, sRole);
                                }
                                catch (Exception ex)
                                {
                                    DB.Rollback(this.Context);
                                    this.Session["HATA"] = ex.Message.ToString();
                                    return Guid.Empty;
                                }
                            }

                            break;
                    }
                }
            }
            #endregion

            #region Ýzinler
            changes = this.DTUserPermissions.Table.GetChanges();
            if (changes != null)
            {
                foreach (DataRow row in changes.Rows)
                {
                    switch (row.RowState)
                    {
                        case DataRowState.Added:
                            sb = new StringBuilder();
                            sb.Append("INSERT INTO SecurityUserPermissions(UserPermissionID,UserID,ObjectID,[Select],[Insert],[Update],[Delete],CreatedBy,CreationDate) ");
                            sb.Append("VALUES(@UserPermissionID,@UserID,@ObjectID,@Select,@Insert,@Update,@Delete,@CreatedBy,@CreationDate)");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@UserPermissionID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@UserID", id);
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
                            sb.Append("UPDATE SecurityUserPermissions SET ");
                            sb.Append("UserID=@UserID,");
                            sb.Append("ObjectID=@ObjectID,");
                            sb.Append("[Select]=@Select,");
                            sb.Append("[Insert]=@Insert,");
                            sb.Append("[Update]=@Update,");
                            sb.Append("[Delete]=@Delete,");
                            sb.Append("ModifiedBy=@ModifiedBy,");
                            sb.Append("ModificationDate=@ModificationDate ");
                            sb.Append("WHERE UserPermissionID=@UserPermissionID");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@UserPermissionID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@UserID", id);
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
                            cmd = DB.SQL(this.Context, "DELETE FROM SecurityUserPermissions WHERE UserPermissionID=@UserPermissionID");
                            DB.AddParam(cmd, "@UserPermissionID", (Guid)row["UserPermissionID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                    }
                }
            }
            #endregion

            #region Kullanýcý Grubu
            //changes = this.DTUserGrupList.Table.GetChanges();
            //if (changes != null)
            //{
            //    foreach (DataRow row in changes.Rows)
            //    {
            //        switch (row.RowState)
            //        {
            //            case DataRowState.Added:
            //                sb = new StringBuilder();
            //                sb.Append("INSERT INTO UserGrupToUsers(UserGrupToUsersID,UserGrupID,UserID,CreatedBy,CreationDate) ");
            //                sb.Append("VALUES(@UserGrupToUsersID,@UserGrupID,@UserID,@CreatedBy,@CreationDate)");
            //                cmd = DB.SQL(this.Context, sb.ToString());
            //                DB.AddParam(cmd, "@UserGrupToUsersID", (Guid)row["ID"]);
            //                DB.AddParam(cmd, "@UserGrupID", (Guid)row["UserGrupID"]);
            //                DB.AddParam(cmd, "@UserID", id);
            //                DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
            //                DB.AddParam(cmd, "@CreationDate", DateTime.Now);
            //                cmd.Prepare();
            //                cmd.ExecuteNonQuery();
            //                break;
            //            case DataRowState.Modified:
            //                break;
            //            case DataRowState.Deleted:
            //                cmd = DB.SQL(this.Context, "DELETE FROM UserGrupToUsers WHERE UserGrupToUsersID=@UserGrupToUsersID");
            //                DB.AddParam(cmd, "@UserGrupToUsersID", (Guid)row["ID", DataRowVersion.Original]);
            //                cmd.Prepare();
            //                cmd.ExecuteNonQuery();
            //                break;
            //        }
            //    }
            //}
            #endregion

            #region Proje Ýzin
            changes = this.DTAllowedProject.Table.GetChanges();
            if (changes != null)
            {
                foreach (DataRow row in changes.Rows)
                {
                    switch (row.RowState)
                    {
                        case DataRowState.Added:
                            sb = new StringBuilder();
                            sb.Append("INSERT INTO UserAllowedProject(UserAllowedProjectID,ProjeID,UserID,CreatedBy,CreationDate) ");
                            sb.Append("VALUES(@UserAllowedProjectID,@ProjeID,@UserID,@CreatedBy,@CreationDate)");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@UserAllowedProjectID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@ProjeID", (Guid)row["ProjeID"]);
                            DB.AddParam(cmd, "@UserID", id);
                            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Modified:
                            break;
                        case DataRowState.Deleted:
                            cmd = DB.SQL(this.Context, "DELETE FROM UserAllowedProject WHERE UserAllowedProjectID=@UserAllowedProjectID");
                            DB.AddParam(cmd, "@UserAllowedProjectID", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                    }
                }
            }
            #endregion

            #region ses email
            changes = this.DTSesEmail.Table.GetChanges();
            if (changes != null)
            {
                foreach (DataRow row in changes.Rows)
                {
                    switch (row.RowState)
                    {
                        case DataRowState.Added:
                            sb = new StringBuilder();
                            sb.Append("INSERT INTO UserSesEmail(Id,UserId,UserName,EmailId,Email,CreatedBy,CreationDate) ");
                            sb.Append("VALUES(@Id,@UserId,@UserName,@EmailId,@Email,@CreatedBy,@CreationDate)");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", Guid.NewGuid());
                            DB.AddParam(cmd, "@UserId", id);
                            DB.AddParam(cmd, "@UserName", 60, sUserName);
                            DB.AddParam(cmd, "@EmailId", (Guid)row["EmailId"]);
                            DB.AddParam(cmd, "@Email", 255, row["Email"].ToString());
                            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Modified:
                            sb = new StringBuilder();
                            sb.Append("UPDATE UserSesEmail SET UserId=@UserId,UserName=@UserName,EmailId=@EmailId,Email=@Email,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate ");
                            sb.Append("WHERE Id=@Id");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@UserId", id);
                            DB.AddParam(cmd, "@UserName", 60, sUserName);
                            DB.AddParam(cmd, "@EmailId", (Guid)row["EmailId"]);
                            DB.AddParam(cmd, "@Email", 255, row["Email"].ToString());
                            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Deleted:
                            cmd = DB.SQL(this.Context, "DELETE FROM UserSesEmail WHERE Id=@Id");
                            DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                    }
                }
            }
            #endregion
        }

        DB.Commit(this.Context);

        return id;
    }

    protected void gridUserGrupList_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["UserGrupID"] == null)
        {
            e.RowError = "Lütfen Kullanýcý Grubu alanýný boþ býrakmayýnýz...";
            return;
        }
    }

    //protected void gridUserGrupList_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    //{
    //    if (gridUserGrupList.IsEditing && e.Column.FieldName == "UserGrupID")
    //    {
    //        ASPxComboBox cmb = e.Editor as ASPxComboBox;
    //        ListEditItem item;
    //        cmb.Items.Clear();
    //        cmb.DataSourceID = null;
    //        foreach (DataRow row in DTUserGrup.Table.Rows)
    //        {
    //            DataRow[] rows = DTUserGrupList.Table.Select("UserGrupID='" + row["UserGrupID"].ToString() + "'");
    //            if (rows.Length == 0)
    //            {
    //                item = new ListEditItem();
    //                item.Text = row["UserGrupName"].ToString();
    //                item.Value = row["UserGrupID"];
    //                cmb.Items.Add(item);
    //            }
    //        }
    //    }
    //}

    private bool DeleteDocument()
    {
        string sID = this.HiddenID.Value;
        if (sID != null)
        {
            DB.BeginTrans(this.Context);

            Guid id = Guid.Empty;
            id = new Guid(this.HiddenID.Value);

            SqlCommand cmd = DB.SQL(this.Context, "SELECT UserName FROM SecurityUsers WHERE UserID=@UserID");
            DB.AddParam(cmd, "@UserID", id);
            cmd.Prepare();
            string sUserName = (string)cmd.ExecuteScalar();

            if (!Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator"))
            {
                if (Roles.IsUserInRole(sUserName, "Administrator") || Roles.IsUserInRole(sUserName, "Merkez Yöneticileri"))
                {
                    DB.Rollback(this.Context);
                    this.Session["HATA"] = "Administrator yetkilisi dýþýnda bir kullanýcý Administrator veya Merkez Yöneticileri Rollerine Sahip bir kullanýcýyý silemez.";
                    //this.Response.Redirect("./../../hata.aspx");
                    return false;
                }
            }

            cmd = DB.SQL(this.Context, "EXEC DeleteUser @UserId");
            DB.AddParam(cmd, "@UserId", id);
            cmd.Prepare();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                DB.Rollback(this.Context);
                this.Session["HATA"] = ex.Message.ToString();
                //this.Response.Redirect("./../../hata.aspx");
                return false;
            }

            MembershipUserCollection users2 = Membership.FindUsersByName(sUserName);
            if (users2.Count != 0)
            {
                if (!Membership.DeleteUser(sUserName))
                {
                    DB.Rollback(this.Context);
                    this.Session["HATA"] = "'" + sUserName + "' kullanýcýsý silinemedi!'";
                    //this.Response.Redirect("./../../hata.aspx");
                    return false;
                }
            }

            DB.Commit(this.Context);
        }
        return true;
    }

    private bool SetUserStatu()
    {
        string sID = this.HiddenID.Value;

        if (sID != null)
        {
            DB.BeginTrans(this.Context);

            try
            {
                Guid id = Guid.Empty;
                id = new Guid(this.HiddenID.Value);

                SqlCommand cmd = DB.SQL(this.Context, "EXEC PassiveUserFromUserId @UserID");
                DB.AddParam(cmd, "@UserID", id);
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

    protected void GridUserRoles_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["RoleID"] == null)
            e.RowError = "Lütfen Rol alanýný boþ býrakmayýnýz...";
        if (!Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator"))
        {
            if (e.NewValues["RoleID"].ToString().ToUpper() == "36D0CFE9-15A5-44E6-84FB-0B5784CA1C5E"
                || e.NewValues["RoleID"].ToString().ToUpper() == "B70EA7AA-D0DF-4B12-8AA9-CD7D0A4E5E10"
                || e.NewValues["RoleID"].ToString().ToUpper() == "79F94C34-498C-4BAF-83FA-7CBD24FA2E65"
                || Membership.GetUser().UserName.ToLower() == "admin")
            {
                //e.RowError = "Admin haricindeki kullanýcýlar sadece Maðaza Çalýþanlarý veya Merkez Çalýþanlarý rollerini ekleyebilir!";,FDC392F9-F0B5-46B5-BF97-5E4E599A081A
                return;
            }
            else
            {
                e.RowError = "Admin haricindeki kullanýcýlar sadece Maðaza Çalýþanlarý,Merkez Çalýþanlarý,Dosya Yöneticisi rollerini ekleyebilir!";
            }
        }
    }

    protected void GridUserPermissions_RowInserting(object sender, ASPxDataInsertingEventArgs e)
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

    protected void GridUserPermissions_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
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

    protected void GridUserPermissions_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["ObjectID"] == null)
        {
            e.RowError = "Lütfen Nesne alanýný boþ býrakmayýnýz...";
            return;
        }
    }

    protected void gridAllowedProject_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["ProjeID"] == null)
        {
            e.RowError = "Lütfen Proje alanýný boþ býrakmayýnýz...";
            return;
        }
    }

    protected void GridSesEmail_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["EmailId"] == null)
        {
            e.RowError = "Lütfen E-Posta alanýný boþ býrakmayýnýz...";
            return;
        }
    }

    protected void gridAllowedProject_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (gridAllowedProject.IsEditing && e.Column.FieldName == "ProjeID")
        {
            ASPxComboBox cmb = e.Editor as ASPxComboBox;
            ListEditItem item;
            cmb.Items.Clear();
            cmb.DataSourceID = null;
            foreach (DataRow row in DTProje.Table.Rows)
            {
                DataRow[] rows = DTAllowedProject.Table.Select("ProjeID='" + row["ProjeID"].ToString() + "'");
                if (rows.Length == 0)
                {
                    item = new ListEditItem();
                    item.Text = row["ProjeName"].ToString();
                    item.Value = row["ProjeID"];
                    cmb.Items.Add(item);
                }
            }
        }
    }

    protected void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        ASPxGridView grid = (ASPxGridView)sender;

        DataTable changes = null;

        if (grid.ID == "GridUserRoles")
            changes = this.DTUserRoles.Table.GetChanges();
        if (grid.ID == "GridUserPermissions")
            changes = this.DTUserPermissions.Table.GetChanges();

        if (changes != null)
            e.Properties["cpIsDirty"] = true;
        else
            e.Properties["cpIsDirty"] = false;
    }

    protected void ProjeID_Callback(object source, CallbackEventArgsBase e)
    {
        if (!Page.IsCallback) return;
        if (e.Parameter.ToString() == null || e.Parameter.ToString() == "")
            return;
        data.BindComboBoxes(this.Context, ProjeID, "SELECT ProjeID,Adi FROM Proje "
        + "WHERE CONVERT(char(50),FirmaID)='" + e.Parameter.ToString() + "' ORDER BY Adi", "ProjeID", "Adi");
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
                #region müþteri temsilci
                case "GridSesEmail_EmailId":
                    dtQuery.Rows.Add("SELECT Id,Email,Aciklama FROM SesEmail ORDER BY Email");
                    dtFields.Rows.Add("Id", "Id", 50, false);
                    dtFields.Rows.Add("Email", "E-Posta", 100, true);
                    dtFields.Rows.Add("Aciklama", "Açýklama", 100, true);
                    htSearchBrowser.Add("Title", "Ses Gönderilen E-Posta Adresleri");
                    htSearchBrowser.Add("Query", dtQuery);
                    htSearchBrowser.Add("Parameters", dtParameters);
                    htSearchBrowser.Add("Fields", dtFields);
                    htSearchBrowser.Add("KeyField", "Id");
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
            string[] parameters = e.Parameter.Split('|');
            switch (parameters[0].Trim())
            {
                case "GridSesEmail_EmailId":
                    #region anket müþteri
                    cmd = DB.SQL(this.Context, "SELECT Id,Email FROM SesEmail WHERE Id=@Id");
                    DB.AddParam(cmd, "@Id", (new Guid(parameters[1].ToString().Trim())));
                    cmd.Prepare();
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        sb = new StringBuilder("|" + parameters[0].Trim());
                        sb.Append("|" + rdr["Id"].ToString());
                        sb.Append("|" + rdr["Email"].ToString());
                        e.Result = sb.ToString();
                    }
                    else e.Result = null;
                    rdr.Close();
                    #endregion
                    break;
                default:
                    break;
            }
        }
    }
}
