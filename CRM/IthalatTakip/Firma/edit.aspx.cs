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

public partial class CRM_IthalatTakip_Tanim_Firma_edit : System.Web.UI.Page
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

        if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Firma", "Select"))
        {
            //this.Response.Write("<html><body bgcolor='#e6e6fa'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            //this.Response.End();
            CrmUtils.MessageAlert(this.Page, "Bu sayfayý görme yetkisine sahip deðilsiniz!", "Alert");
            return;
        }

        InitDTMarka(this.DTMarka.Table);
        InitDTUrun(this.DTUrun.Table);
        this.PageAlt.ActiveTabPage = this.PageAlt.TabPages.FindByName("TabMarka");
        this.Title = "Firma - Yeni";
        Guid id = Guid.Empty;
        string sID = this.Request.Params["id"].Replace("'", "").Trim();
        if ((sID != null) && (sID != "0"))
        {
            id = new Guid(sID);
            LoadDocument(id);
            this.HiddenId.Value = id.ToString();
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
            if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Firma", "Insert"))
            {
                CrmUtils.MessageAlert(this.Page, "Yeni Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "MenuNew");
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
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Firma", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "MenuSave1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Firma", "Update"))
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

            this.GridMarka.UpdateEdit();
            this.GridUrun.UpdateEdit();

            Guid id = SaveDocument();
            if (id == Guid.Empty)
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "Menu");
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
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Firma", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "MenuSaveNew1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Firma", "Update"))
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

            this.GridMarka.UpdateEdit();
            this.GridUrun.UpdateEdit();

            Guid id = SaveDocument();
            if (id == Guid.Empty)
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "Menu");
                return;
            }
            else
            {
                this.Response.Write("<script language='javascript'>{ parent.opener.location.href=parent.opener.location.href; }</script>");

                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Firma", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Yeni Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "Menu");
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
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Firma", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "MenuSaveClose1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Firma", "Update"))
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

            this.GridMarka.UpdateEdit();
            this.GridUrun.UpdateEdit();

            Guid id = SaveDocument();
            if (id == Guid.Empty)
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "Menu");
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
            if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Firma", "Delete"))
            {
                CrmUtils.MessageAlert(this.Page, "Silme iþlemi yapma yetkisine sahip deðilsiniz!", "MenuDelete1");
                return;
            }

            if (!DeleteDocument())
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "Menu");
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
        #region firma
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM IthalatFirma WHERE Id=@Id");
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
        this.Title = "Firma - " + rdr["Adi"].ToString();
        this.Adi.Value = rdr["Adi"];
        this.Aciklama.Value = rdr["Aciklama"];
        this.Iskonto.Value = rdr["Iskonto"];
        Guid _SevkSekliId = Guid.Empty;
        if ((rdr["SevkSekliId"] != null) && (rdr["SevkSekliId"].ToString() != "") && (rdr["SevkSekliId"].ToString() != Guid.Empty.ToString()))
            _SevkSekliId = new Guid(rdr["SevkSekliId"].ToString());
        Guid _OdemeSekliId = Guid.Empty;
        if ((rdr["OdemeSekliId"] != null) && (rdr["OdemeSekliId"].ToString() != "") && (rdr["OdemeSekliId"].ToString() != Guid.Empty.ToString()))
            _OdemeSekliId = new Guid(rdr["OdemeSekliId"].ToString());
        rdr.Close();

        if (_SevkSekliId != Guid.Empty)
        {
            FillSevkSekliId(this.SevkSekliId, _SevkSekliId);
            this.SevkSekliId.SelectedIndex = 0;
        }
        if (_OdemeSekliId != Guid.Empty)
        {
            FillOdemeSekliId(this.OdemeSekliId, _OdemeSekliId);
            this.OdemeSekliId.SelectedIndex = 0;
        }
        #endregion

        #region markalar
        this.DTMarka.Table.Clear();
        cmd = DB.SQL(this.Context, "SELECT t1.* FROM IthalatFirmaMarka t1 WHERE t1.FirmaId=@FirmaId ORDER BY Marka");
        DB.AddParam(cmd, "@FirmaId", id);
        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTMarka.Table.NewRow();
            row["ID"] = rdr["Id"];
            row["FirmaId"] = rdr["FirmaId"];
            row["Marka"] = rdr["Marka"];
            this.DTMarka.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTMarka.Table.AcceptChanges();
        #endregion

        #region ürünler
        this.DTUrun.Table.Clear();
        cmd = DB.SQL(this.Context, "SELECT t1.* FROM IthalatFirmaMarkaUrun t1 LEFT JOIN IthalatFirmaMarka t2 ON(t1.MarkaId=t2.Id) WHERE t1.FirmaId=@FirmaId ORDER BY t2.Marka,t1.Urun");
        DB.AddParam(cmd, "@FirmaId", id);
        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTUrun.Table.NewRow();
            row["ID"] = rdr["Id"];
            row["FirmaId"] = rdr["FirmaId"];
            row["MarkaId"] = rdr["MarkaId"];
            row["Urun"] = rdr["Urun"];
            this.DTUrun.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTUrun.Table.AcceptChanges();
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

        #region firma
        if (id == Guid.Empty)
        {
            #region insert
            StringBuilder sb1 = new StringBuilder("INSERT INTO IthalatFirma(Id,Adi,Aciklama,CreatedBy,CreationDate");
            StringBuilder sb2 = new StringBuilder(" VALUES(@Id,@Adi,@Aciklama,@CreatedBy,@CreationDate");

            if ((this.Iskonto.Value != null) && (!String.IsNullOrEmpty(this.Iskonto.Value.ToString())))
            {
                sb1.Append(",Iskonto");
                sb2.Append(",@Iskonto");
            }
            if ((this.SevkSekliId.SelectedIndex >= 0) && (this.SevkSekliId.Value.ToString() != Guid.Empty.ToString()))
            {
                sb1.Append(",SevkSekliId");
                sb2.Append(",@SevkSekliId");
            }
            if ((this.OdemeSekliId.SelectedIndex >= 0) && (this.OdemeSekliId.Value.ToString() != Guid.Empty.ToString()))
            {
                sb1.Append(",OdemeSekliId");
                sb2.Append(",@OdemeSekliId");
            }

            sb1.Append(")");
            sb2.Append(")");

            id = Guid.NewGuid();
            cmd = DB.SQL(this.Context, (sb1.ToString() + sb2.ToString()));
            DB.AddParam(cmd, "@Id", id);
            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
            DB.AddParam(cmd, "@CreatedBy", 50, Membership.GetUser().UserName);
            #endregion
        }
        else
        {
            #region update
            sb.Append("UPDATE IthalatFirma SET Adi=@Adi,Aciklama=@Aciklama,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate");
            if ((this.Iskonto.Value != null) && (!String.IsNullOrEmpty(this.Iskonto.Value.ToString()))) sb.Append(",Iskonto=@Iskonto");
            else sb.Append(",Iskonto=NULL");
            if ((this.SevkSekliId.SelectedIndex >= 0) && (this.SevkSekliId.Value.ToString() != Guid.Empty.ToString())) sb.Append(",SevkSekliId=@SevkSekliId");
            else sb.Append(",SevkSekliId=NULL");
            if ((this.OdemeSekliId.SelectedIndex >= 0) && (this.OdemeSekliId.Value.ToString() != Guid.Empty.ToString())) sb.Append(",OdemeSekliId=@OdemeSekliId");
            else sb.Append(",OdemeSekliId=NULL");
            sb.Append(" WHERE Id=@Id");

            cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@Id", id);
            DB.AddParam(cmd, "@ModifiedBy", 50, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
            #endregion
        }

        #region values
        DB.AddParam(cmd, "@Adi", 255, this.Adi.Value);
        DB.AddParam(cmd, "@Aciklama", 255, this.Aciklama.Value);
        if ((this.Iskonto.Value != null) && (!String.IsNullOrEmpty(this.Iskonto.Value.ToString())))
            DB.AddParam(cmd, "@Iskonto", 60, this.Iskonto.Value.ToString().Replace(",", "."));
        if ((this.SevkSekliId.SelectedIndex >= 0) && (this.SevkSekliId.Value.ToString() != Guid.Empty.ToString()))
            DB.AddParam(cmd, "@SevkSekliId", (new Guid(this.SevkSekliId.Value.ToString())));
        if ((this.OdemeSekliId.SelectedIndex >= 0) && (this.OdemeSekliId.Value.ToString() != Guid.Empty.ToString()))
            DB.AddParam(cmd, "@OdemeSekliId", (new Guid(this.OdemeSekliId.Value.ToString())));
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

        #region markalar
        DataTable changes = this.DTMarka.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        #region insert
                        StringBuilder sb1 = new StringBuilder("INSERT INTO IthalatFirmaMarka(Id,FirmaId,Marka,CreatedBy,CreationDate) ");
                        StringBuilder sb2 = new StringBuilder("VALUES(@Id,@FirmaId,@Marka,@CreatedBy,@CreationDate)");
                        try
                        {
                            cmd = DB.SQL(this.Context, (sb1.ToString() + sb2.ToString()));
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@FirmaId", id);
                            DB.AddParam(cmd, "@Marka", 255, row["Marka"].ToString());
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
                        sb = new StringBuilder("UPDATE IthalatFirmaMarka ");
                        sb.Append("SET FirmaId=@FirmaId,Marka=@Marka,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate ");
                        sb.Append("WHERE Id=@Id");
                        try
                        {
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@FirmaId", id);
                            DB.AddParam(cmd, "@Marka", 255, row["Marka"].ToString());
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
                            cmd = DB.SQL(this.Context, "DELETE FROM IthalatFirmaMarkaUrun WHERE MarkaId=@Id");
                            DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.CommandTimeout = 1000;
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            cmd = DB.SQL(this.Context, "DELETE FROM IthalatFirmaMarka WHERE Id=@Id");
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

        #region ürünler
        changes = this.DTUrun.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        #region insert
                        StringBuilder sb1 = new StringBuilder("INSERT INTO IthalatFirmaMarkaUrun(Id,FirmaId,MarkaId,Urun,CreatedBy,CreationDate) ");
                        StringBuilder sb2 = new StringBuilder("VALUES(@Id,@FirmaId,@MarkaId,@Urun,@CreatedBy,@CreationDate)");
                        try
                        {
                            cmd = DB.SQL(this.Context, (sb1.ToString() + sb2.ToString()));
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@FirmaId", id);
                            DB.AddParam(cmd, "@MarkaId", (new Guid(row["MarkaId"].ToString())));
                            DB.AddParam(cmd, "@Urun", 255, row["Urun"].ToString());
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
                        sb = new StringBuilder("UPDATE IthalatFirmaMarkaUrun ");
                        sb.Append("SET FirmaId=@FirmaId,MarkaId=@MarkaId,Urun=@Urun,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate ");
                        sb.Append("WHERE Id=@Id");
                        try
                        {
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@FirmaId", id);
                            DB.AddParam(cmd, "@MarkaId", (new Guid(row["MarkaId"].ToString())));
                            DB.AddParam(cmd, "@Urun", 255, row["Urun"].ToString());
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
                            cmd = DB.SQL(this.Context, "DELETE FROM IthalatFirmaMarkaUrun WHERE Id=@Id");
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
                cmd = DB.SQL(this.Context, "DELETE FROM IthalatFirmaMarkaUrun WHERE FirmaId=@Id");
                DB.AddParam(cmd, "@Id", id);
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd = DB.SQL(this.Context, "DELETE FROM IthalatFirmaMarka WHERE FirmaId=@Id");
                DB.AddParam(cmd, "@Id", id);
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd = DB.SQL(this.Context, "DELETE FROM IthalatFirma WHERE Id=@Id");
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

    protected void SevkSekliId_Callback(object source, CallbackEventArgsBase e)
    {
        this.SevkSekliId.Items.Clear();
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            Guid id = new Guid(e.Parameter);
            FillSevkSekliId(this.SevkSekliId, id);
            this.SevkSekliId.SelectedIndex = 0;
        }
    }

    private void FillSevkSekliId(ASPxComboBox source, Guid id)
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT Id,SevkSekli FROM IthalatSevkSekli WHERE Id=@Id");
        DB.AddParam(cmd, "@Id", id);
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        if (rdr.Read())
        {
            ListEditItem item = new ListEditItem();
            item.Value = rdr["Id"];
            item.Text = rdr["SevkSekli"].ToString();
            source.Items.Add(item);
        }
        rdr.Close();
    }

    protected void OdemeSekliId_Callback(object source, CallbackEventArgsBase e)
    {
        this.OdemeSekliId.Items.Clear();
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            Guid id = new Guid(e.Parameter);
            FillOdemeSekliId(this.OdemeSekliId, id);
            this.OdemeSekliId.SelectedIndex = 0;
        }
    }

    private void FillOdemeSekliId(ASPxComboBox source, Guid id)
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT Id,OdemeSekli FROM IthalatOdemeSekli WHERE Id=@Id");
        DB.AddParam(cmd, "@Id", id);
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        if (rdr.Read())
        {
            ListEditItem item = new ListEditItem();
            item.Value = rdr["Id"];
            item.Text = rdr["OdemeSekli"].ToString();
            source.Items.Add(item);
        }
        rdr.Close();
    }

    protected void CallbackSearchBrowser_Callback(object source, CallbackEventArgs e)
    {
        e.Result = "";
        Session["SearchBrowser"] = null;
        DataTable dtQuery = DB.SqlQuery();
        DataTable dtParameters = DB.SqlQueryParameters();
        DataTable dtFields = DB.SqlQueryFields();
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            string[] parameters = e.Parameter.Split('|');
            StringBuilder sb = new StringBuilder();
            Hashtable htSearchBrowser = new Hashtable();
            switch (parameters[0].Trim())
            {
                #region sevk þekli
                case "SevkSekliId":
                    dtQuery.Rows.Add("SELECT Id,SevkSekli FROM IthalatSevkSekli ORDER BY SevkSekli");
                    dtFields.Rows.Add("Id", "Id", 50, false);
                    dtFields.Rows.Add("SevkSekli", "Sevk Þekli", 100, true);
                    htSearchBrowser.Add("Title", "SEVK ÞEKÝLLERÝ");
                    htSearchBrowser.Add("Query", dtQuery);
                    htSearchBrowser.Add("Parameters", dtParameters);
                    htSearchBrowser.Add("Fields", dtFields);
                    htSearchBrowser.Add("KeyField", "Id");
                    break;
                #endregion
                #region ödeme þekli
                case "OdemeSekliId":
                    dtQuery.Rows.Add("SELECT Id,OdemeSekli,GUN FROM IthalatOdemeSekli ORDER BY OdemeSekli");
                    dtFields.Rows.Add("Id", "Id", 50, false);
                    dtFields.Rows.Add("OdemeSekli", "Ödeme Þekli", 100, true);
                    dtFields.Rows.Add("Gun", "Gün", 100, true);
                    htSearchBrowser.Add("Title", "ÖDEME ÞEKÝLLERÝ");
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

    private void InitDTMarka(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("FirmaId", typeof(Guid));
        dt.Columns.Add("Marka", typeof(string));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitDTUrun(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("FirmaId", typeof(Guid));
        dt.Columns.Add("MarkaId", typeof(Guid));
        dt.Columns.Add("Urun", typeof(string));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    protected void GridMarka_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (!this.GridMarka.IsEditing) return;

        switch (e.Column.FieldName)
        {
            case "col_name":
                break;
        }
    }

    protected void GridMarka_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["FirmaId"] == null) e.NewValues["FirmaId"] = DBNull.Value;
        }
    }

    protected void GridMarka_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["FirmaId"] == null) e.NewValues["FirmaId"] = DBNull.Value;
        }
    }

    protected void GridMarka_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["Marka"] == null)
        {
            e.RowError = "Lütfen Marka alanýný boþ býrakmayýnýz...";
            return;
        }
    }

    protected void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        ASPxGridView grid = (ASPxGridView)sender;
        DataTable changes = null;
        if (grid.ID == "GridMarka") changes = this.DTMarka.Table.GetChanges();
        if (grid.ID == "GridUrun") changes = this.DTUrun.Table.GetChanges();
        if (changes != null) e.Properties["cpIsDirty"] = true;
        else e.Properties["cpIsDirty"] = false;
    }

    protected void GridUrun_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (!this.GridUrun.IsEditing) return;

        switch (e.Column.FieldName)
        {
            case "col_name":
                break;
        }
    }

    protected void GridUrun_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["FirmaId"] == null) e.NewValues["FirmaId"] = DBNull.Value;
            if (e.NewValues["MarkaId"] == null) e.NewValues["MarkaId"] = DBNull.Value;
        }
    }

    protected void GridUrun_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["FirmaId"] == null) e.NewValues["FirmaId"] = DBNull.Value;
            if (e.NewValues["MarkaId"] == null) e.NewValues["MarkaId"] = DBNull.Value;
        }
    }

    protected void GridUrun_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["MarkaId"] == null)
        {
            e.RowError = "Lütfen Marka alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["Urun"] == null)
        {
            e.RowError = "Lütfen Ürün alanýný boþ býrakmayýnýz...";
            return;
        }
    }
}
