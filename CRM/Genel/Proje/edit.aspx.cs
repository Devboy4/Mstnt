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

public partial class CRM_Genel_Proje_edit : System.Web.UI.Page
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
        this.menu.ItemClick += new MenuItemEventHandler(Menu_ItemClick);
        ASPxPageControl1.ActiveTabIndex = 0;

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }
        if (!Security.CheckPermission(this.Context, "Proje", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }
        InitGridAllowedProject(this.DataTableAllowedProjeList.Table);
        InitGridDTUsers(DTUsers.Table);
        //InitGridDTUserGrup(DTUserGrup.Table);
        //InitGridDTProjeUserGrupList(DTProjeUserGrupList.Table);
        fillcomboxes();
        Guid id = Guid.Empty;

        string sID = this.Request.Params["id"].Replace("'", "").Trim();
        if ((sID != null) && (sID != "0"))
        {
            id = new Guid(sID);
            this.HiddenID.Value = id.ToString();
            LoadDocument(id);
        }
        else
        {
            this.ASPxPageControl1.TabPages.FindByName("ProjeSettings").Enabled = false;
        }

    }

    void fillcomboxes()
    {
        data.BindComboBoxesNoEmpty(this.Context, FirmaID, "SELECT FirmaID,FirmaName FROM "
        + "Firma ORDER BY FirmaName", "FirmaID", "FirmaName");
        data.BindComboBoxes(this.Context, ProjeAmiriID, "SELECT UserID,UserName FROM "
        + "SecurityUsers Where Active=1 ORDER BY UserName", "UserID", "UserName");
        data.BindComboBoxes(this.Context, ProjeSinifID, "SELECT ProjeSinifID,Adi FROM "
        + "ProjeSiniflari ORDER BY Adi", "ProjeSinifID", "Adi");
        StringBuilder sb = new StringBuilder();
        #region Users
        sb.Append("SELECT UserID,(ISNULL(UserName,'')+' '+'['+ISNULL(FirstName,'')+' '+ISNULL(LastName,'')+']') AS ");
        sb.Append("UserName FROM SecurityUsers Where Active=1  ORDER BY UserName");
        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        IDataReader rdr = cmd.ExecuteReader();
        DTUsers.Table.Clear();
        while (rdr.Read())
        {
            DataRow row = DTUsers.Table.NewRow();
            row["UserID"] = rdr["UserID"];
            row["UserName"] = rdr["UserName"];
            DTUsers.Table.Rows.Add(row);
        }
        DTUsers.Table.AcceptChanges();
        rdr.Close();
        #endregion
        #region User Grup
        //sb = new StringBuilder();
        //sb.Append("SELECT UserGrupID, Adi AS UserGrupName FROM UserGrup ORDER BY Adi");
        //cmd = DB.SQL(this.Context, sb.ToString());
        //rdr = cmd.ExecuteReader();
        //DTUserGrup.Table.Clear();
        //while (rdr.Read())
        //{
        //    DataRow row = DTUserGrup.Table.NewRow();
        //    row["UserGrupID"] = rdr["UserGrupID"];
        //    row["UserGrupName"] = rdr["UserGrupName"];
        //    DTUserGrup.Table.Rows.Add(row);
        //}
        //DTUserGrup.Table.AcceptChanges();
        //rdr.Close();
        #endregion

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
            if (!Security.CheckPermission(this.Context, "Proje", "Insert"))
            {
                CrmUtils.MessageAlert(this.Page, "Yeni Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                return;
            }
            Response.Write("<script language='Javascript'>{ parent.location.href='./default.aspx?id=0'; }</script>");
        }
        else if (e.Item.Name.Equals("save"))
        {
            if (bYeni)
            {
                if (!Security.CheckPermission(this.Context, "Proje", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Proje", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                    return;
                }
            }

            this.GRD_AlowedProjeList.UpdateEdit();
            Guid id = SaveDocument();
            Session["BelgeID"] = id.ToString();
            this.Response.Write("<script language='javascript'>{ parent.opener.location.reload(true);window.location.href='./edit.aspx?id=" + (String)Session["BelgeID"] + "';}</script>");

        }
        else if (e.Item.Name.Equals("saveclose"))
        {

            if (bYeni)
            {
                if (!Security.CheckPermission(this.Context, "Proje", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Proje", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                    return;
                }
            }
            this.GRD_AlowedProjeList.UpdateEdit();
            SaveDocument();
            this.Response.Write("<script language='javascript'>{ parent.opener.location.reload(true);parent.close();}</script>");
        }
        else if (e.Item.Name.Equals("delete"))
        {
            if (!Security.CheckPermission(this.Context, "Proje", "Delete"))
            {
                CrmUtils.MessageAlert(this.Page, "Silme iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                return;
            }
            DeleteDocument();
            this.Response.Write("<script language='javascript'>{ parent.opener.location.reload(true);parent.close(); }</script>");
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    private void LoadDocument(Guid id)
    {

        StringBuilder sb = new StringBuilder();
        Guid sirketbilgileriID = Guid.Empty;
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM Proje WHERE ProjeID=@ProjeID");
        DB.AddParam(cmd, "@ProjeID", id);
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        if (!rdr.Read())
        {
            this.Response.StatusCode = 500;
            this.Response.End();
            return;
        }
        ProjeEmailAdresi.Value = rdr["ProjeMailAdresi"].ToString();
        ProjeName.Value = rdr["Adi"].ToString();
        Description.Value = rdr["Description"].ToString();
        if ((rdr["FirmaID"].ToString() != null) && (rdr["FirmaID"].ToString() != ""))
            this.FirmaID.SelectedIndex = this.FirmaID.Items.IndexOfValue(rdr["FirmaID"].ToString());
        if ((rdr["ProjeAmiriID"].ToString() != null) && (rdr["ProjeAmiriID"].ToString() != ""))
            this.ProjeAmiriID.SelectedIndex = this.ProjeAmiriID.Items.IndexOfValue(rdr["ProjeAmiriID"].ToString());
        if ((rdr["ProjeSinifID"].ToString() != null) && (rdr["ProjeSinifID"].ToString() != ""))
            this.ProjeSinifID.SelectedIndex = this.ProjeSinifID.Items.IndexOfValue(rdr["ProjeSinifID"].ToString());
        rdr.Close();
        #region// User Ekleniyor...
        this.DataTableAllowedProjeList.Table.Clear();
        sb.Append("SELECT Uap.*,Scu.FirstName,Scu.LastName FROM UserAllowedProject Uap ");
        sb.Append("LEFT OUTER JOIN SecurityUsers Scu ON Uap.UserID=Scu.UserID ");
        sb.Append("WHERE Uap.ProjeID=@ProjeID ");
        sb.Append("ORDER BY CreationDate DESC");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@ProjeID", id);
        cmd.Prepare();
        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableAllowedProjeList.Table.NewRow();

            row["ID"] = rdr["UserAllowedProjectID"];
            row["UserAllowedProjectID"] = rdr["UserAllowedProjectID"];
            row["UserID"] = rdr["UserID"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["FirstName"] = rdr["FirstName"];
            row["LastName"] = rdr["LastName"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];

            this.DataTableAllowedProjeList.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DataTableAllowedProjeList.Table.AcceptChanges();
        #endregion

        #region // User Grup Ekleniyor...
        //this.DTProjeUserGrupList.Table.Clear();
        //sb = new StringBuilder();
        //sb.Append("SELECT * FROM ProjeUserGrupList WHERE ProjeID=@ProjeID ");
        //sb.Append("ORDER BY CreationDate DESC");
        //cmd = DB.SQL(this.Context, sb.ToString());
        //DB.AddParam(cmd, "@ProjeID", id);
        //cmd.Prepare();
        //rdr = cmd.ExecuteReader();
        //while (rdr.Read())
        //{
        //    DataRow row = this.DTProjeUserGrupList.Table.NewRow();

        //    row["ID"] = rdr["ProjeUserGrupListID"];
        //    row["ProjeUserGrupListID"] = rdr["ProjeUserGrupListID"];
        //    row["UserGrupID"] = rdr["UserGrupID"];
        //    row["ProjeID"] = rdr["ProjeID"];
        //    row["CreatedBy"] = rdr["CreatedBy"];
        //    row["CreationDate"] = rdr["CreationDate"];
        //    row["ModifiedBy"] = rdr["ModifiedBy"];
        //    row["ModificationDate"] = rdr["ModificationDate"];

        //    this.DTProjeUserGrupList.Table.Rows.Add(row);
        //}
        //rdr.Close();
        //this.DTProjeUserGrupList.Table.AcceptChanges();
        #endregion


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
        if (id == Guid.Empty)
        {
            #region insert
            sb = new StringBuilder();
            sb.Append("INSERT INTO Proje (");
            sb.Append("ProjeID");
            sb.Append(",Adi");
            sb.Append(",FirmaID");
            if (ProjeAmiriID.SelectedIndex > 0)
                sb.Append(",ProjeAmiriID");
            if (ProjeSinifID.SelectedIndex > 0)
                sb.Append(",ProjeSinifID");
            if (!String.IsNullOrEmpty(ProjeEmailAdresi.Value.ToString()))
                sb.Append(",ProjeMailAdresi");
            if (Description.Text != null)
                sb.Append(",Description");
            sb.Append(",CreatedBy");
            sb.Append(",CreationDate)");
            sb.Append(" VALUES(");
            sb.Append("@ProjeID");
            sb.Append(",@FirmaID");
            if (ProjeAmiriID.SelectedIndex > 0)
                sb.Append(",@ProjeAmiriID");
            if (ProjeSinifID.SelectedIndex > 0)
                sb.Append(",@ProjeSinifID");
            if (!String.IsNullOrEmpty(ProjeEmailAdresi.Value.ToString()))
                sb.Append(",@ProjeMailAdresi");
            sb.Append(",@Adi");
            if (Description.Text != null)
                sb.Append(",@Description");
            sb.Append(",@CreatedBy");
            sb.Append(",@CreationDate)");
            cmd = DB.SQL(this.Context, sb.ToString());
            id = Guid.NewGuid();
            DB.AddParam(cmd, "@ProjeID", id);
            DB.AddParam(cmd, "@CreatedBy", 100, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
            #endregion
        }
        else
        {
            #region update
            sb = new StringBuilder();
            sb.Append("UPDATE Proje SET ");
            sb.Append("FirmaID=@FirmaID");
            sb.Append(",ProjeAmiriID=@ProjeAmiriID");
            sb.Append(",ProjeMailAdresi=@ProjeMailAdresi");
            sb.Append(",ProjeSinifID=@ProjeSinifID");
            sb.Append(",Adi=@Adi");
            sb.Append(",Description=@Description");
            sb.Append(",ModifiedBy=@ModifiedBy");
            sb.Append(",ModificationDate=@ModificationDate");
            sb.Append(" WHERE ProjeID=@ProjeID");
            cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@ProjeID", id);
            DB.AddParam(cmd, "@ModifiedBy", 100, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
            #endregion
        }
        #region values
        DB.AddParam(cmd, "@Adi", 100, ProjeName.Value.ToString().ToUpper());
        DB.AddParam(cmd, "@Description", 255, this.Description.Text.ToUpper());
        Guid tempID;
        if (!String.IsNullOrEmpty(FirmaID.Text))
        {
            tempID = new Guid(FirmaID.Value.ToString());
            DB.AddParam(cmd, "@FirmaID", tempID);

        }
        else
            DB.AddParam(cmd, "@FirmaID", SqlDbType.UniqueIdentifier);
        if (!String.IsNullOrEmpty(ProjeSinifID.Text))
        {
            tempID = new Guid(ProjeSinifID.Value.ToString());
            DB.AddParam(cmd, "@ProjeSinifID", tempID);

        }
        else
            DB.AddParam(cmd, "@ProjeSinifID", SqlDbType.UniqueIdentifier);
        if (!String.IsNullOrEmpty(ProjeAmiriID.Text))
        {
            tempID = new Guid(ProjeAmiriID.Value.ToString());
            DB.AddParam(cmd, "@ProjeAmiriID", tempID);

        }
        else
            DB.AddParam(cmd, "@ProjeAmiriID", SqlDbType.UniqueIdentifier);
        string ControlCom = null;
        ControlCom = (String)this.ProjeEmailAdresi.Value;
        if (!String.IsNullOrEmpty(ControlCom))
            DB.AddParam(cmd, "@ProjeMailAdresi", 100, this.ProjeEmailAdresi.Value.ToString());
        else
            DB.AddParam(cmd, "@ProjeMailAdresi", SqlDbType.Int);
        #endregion
        cmd.Prepare();
        cmd.ExecuteNonQuery();
        if (id != Guid.Empty)
        {
            #region Proje Ýzinli kullanýcý
            DataTable changes = this.DataTableAllowedProjeList.Table.GetChanges();
            if (changes != null)
            {
                foreach (DataRow row in changes.Rows)
                {
                    switch (row.RowState)
                    {
                        case DataRowState.Added:
                            sb = new StringBuilder();
                            sb.Append("INSERT INTO UserAllowedProject(UserAllowedProjectID,ProjeID,UserID,CreatedBy,CreationDate)");
                            sb.Append("VALUES(@UserAllowedProjectID,@ProjeID,@UserID,@CreatedBy,@CreationDate)");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@UserAllowedProjectID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@UserID", (Guid)row["UserID"]);
                            DB.AddParam(cmd, "@ProjeID", id);
                            DB.AddParam(cmd, "@CreatedBy", 50, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Modified:
                            sb = new StringBuilder();
                            sb.Append("UPDATE UserAllowedProject SET ");
                            sb.Append("UserID=@UserID,");
                            sb.Append("ProjeID=@ProjeID,");
                            sb.Append("ModifiedBy=@ModifiedBy,");
                            sb.Append("ModificationDate=@ModificationDate");
                            sb.Append(" WHERE UserAllowedProjectID=@UserAllowedProjectID");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@UserAllowedProjectID", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@UserID", (Guid)row["UserID"]);
                            DB.AddParam(cmd, "@ProjeID", id);
                            DB.AddParam(cmd, "@ModifiedBy", 50, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
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

            #region Ýzin verilen kullanýcý Grubu
            //changes = this.DTProjeUserGrupList.Table.GetChanges();
            //if (changes != null)
            //{
            //    foreach (DataRow row in changes.Rows)
            //    {
            //        switch (row.RowState)
            //        {
            //            case DataRowState.Added:
            //                sb = new StringBuilder();
            //                sb.Append("INSERT INTO ProjeUserGrupList(ProjeUserGrupListID,UserGrupID,ProjeID,CreatedBy,CreationDate)");
            //                sb.Append("VALUES(@ProjeUserGrupListID,@UserGrupID,@ProjeID,@CreatedBy,@CreationDate)");
            //                cmd = DB.SQL(this.Context, sb.ToString());
            //                DB.AddParam(cmd, "@ProjeUserGrupListID", (Guid)row["ID"]);
            //                DB.AddParam(cmd, "@ProjeID", id);
            //                DB.AddParam(cmd, "@UserGrupID", (Guid)row["UserGrupID"]);
            //                DB.AddParam(cmd, "@CreatedBy", 50, Membership.GetUser().UserName);
            //                DB.AddParam(cmd, "@CreationDate", DateTime.Now);
            //                cmd.Prepare();
            //                cmd.ExecuteNonQuery();
            //                break;
            //            case DataRowState.Modified:
            //                sb = new StringBuilder();
            //                sb.Append("UPDATE ProjeUserGrupList SET ");
            //                sb.Append("ProjeID=@ProjeID,");
            //                sb.Append("UserGrupID=@UserGrupID,");
            //                sb.Append("ModifiedBy=@ModifiedBy,");
            //                sb.Append("ModificationDate=@ModificationDate");
            //                sb.Append(" WHERE ProjeUserGrupListID=@ProjeUserGrupListID");
            //                cmd = DB.SQL(this.Context, sb.ToString());
            //                DB.AddParam(cmd, "@ProjeUserGrupListID", (Guid)row["ID"]);
            //                DB.AddParam(cmd, "@ProjeID", id);
            //                DB.AddParam(cmd, "@UserGrupID", (Guid)row["UserGrupID"]);
            //                DB.AddParam(cmd, "@ModifiedBy", 50, Membership.GetUser().UserName);
            //                DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
            //                cmd.Prepare();
            //                cmd.ExecuteNonQuery();
            //                break;
            //            case DataRowState.Deleted:
            //                cmd = DB.SQL(this.Context, "DELETE FROM ProjeUserGrupList WHERE ProjeUserGrupListID=@ProjeUserGrupListID");
            //                DB.AddParam(cmd, "@ProjeUserGrupListID", (Guid)row["ID", DataRowVersion.Original]);
            //                cmd.Prepare();
            //                cmd.ExecuteNonQuery();
            //                break;
            //        }
            //    }
            //} 
            #endregion
        }

        DB.Commit(this.Context);

        return id;
    }

    protected void GRD_AlowedProjeList_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (GRD_AlowedProjeList.IsEditing && e.Column.FieldName == "UserID")
        {
            ASPxComboBox cmb = e.Editor as ASPxComboBox;
            ListEditItem item;
            cmb.Items.Clear();
            cmb.DataSourceID = null;
            foreach (DataRow row in DTUsers.Table.Rows)
            {
                DataRow[] rows = DataTableAllowedProjeList.Table.Select("UserID='" + row["UserID"].ToString() + "'");
                if (rows.Length == 0)
                {
                    item = new ListEditItem();
                    item.Text = row["UserName"].ToString();
                    item.Value = row["UserID"];
                    cmb.Items.Add(item);
                }
            }
        }
    }

    private void InitGridAllowedProject(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("UserAllowedProjectID", typeof(Guid));
        dt.Columns.Add("UserID", typeof(Guid));
        dt.Columns.Add("ProjeID", typeof(Guid));
        dt.Columns.Add("FirstName", typeof(string));
        dt.Columns.Add("LastName", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitGridDTProjeUserGrupList(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("ProjeUserGrupListID", typeof(Guid));
        dt.Columns.Add("ProjeID", typeof(Guid));
        dt.Columns.Add("UserGrupID", typeof(Guid));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitGridDTUsers(DataTable dt)
    {
        dt.Columns.Add("UserID", typeof(Guid));
        dt.Columns.Add("UserName", typeof(string));

    }

    private void InitGridDTUserGrup(DataTable dt)
    {
        dt.Columns.Add("UserGrupID", typeof(Guid));
        dt.Columns.Add("UserGrupName", typeof(string));

    }

    protected void GRD_AlowedProjeList_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["UserID"] == null)
                e.NewValues["UserID"] = DBNull.Value;
        }
    }

    protected void GRD_AlowedProjeList_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["UserID"] == null)
                e.NewValues["UserID"] = DBNull.Value;

        }
    }

    protected void GRD_AlowedProjeList_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["UserID"] == null)
        {
            e.RowError = "Lütfen Kiþi alanýný boþ býrakmayýnýz...";
            return;
        }

    }

    protected void gridProjeUserGrupList_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["UserGrupID"] == null)
                e.NewValues["UserGrupID"] = DBNull.Value;
        }
    }

    protected void gridProjeUserGrupList_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["UserGrupID"] == null)
        {
            e.RowError = "Lütfen Kullanýcý Grup alanýný boþ býrakmayýnýz...";
            return;
        }

    }

    //protected void gridProjeUserGrupList_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    //{
    //    if (gridProjeUserGrupList.IsEditing && e.Column.FieldName == "UserGrupID")
    //    {
    //        ASPxComboBox cmb = e.Editor as ASPxComboBox;
    //        ListEditItem item;
    //        cmb.Items.Clear();
    //        cmb.DataSourceID = null;
    //        foreach (DataRow row in DTUserGrup.Table.Rows)
    //        {
    //            DataRow[] rows = DTProjeUserGrupList.Table.Select("UserGrupID='" + row["UserGrupID"].ToString() + "'");
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

    private void DeleteDocument()
    {
        string sID = this.HiddenID.Value;
        SqlCommand cmd;
        if (sID != null)
        {
            DB.BeginTrans(this.Context);

            Guid id = Guid.Empty;
            id = new Guid(this.HiddenID.Value);
            try
            {
                cmd = DB.SQL(this.Context, "DELETE FROM Proje WHERE ProjeID=@ProjeID");
                DB.AddParam(cmd, "@ProjeID", id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                DB.Commit(this.Context);
            }
            catch (Exception ex)
            {
                DB.Rollback(this.Context);
                CrmUtils.MessageAlert(this.Page, ex.Message.ToString().Replace("'", null).Replace("\r\n", null), "stkeySilinemez");
                return;
            }
        }
    }
}
