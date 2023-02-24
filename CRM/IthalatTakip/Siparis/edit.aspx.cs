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

public partial class CRM_IthalatTakip_Siparis_edit : System.Web.UI.Page
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

        if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ", "Select"))
        {
            //this.Response.Write("<html><body bgcolor='#e6e6fa'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            //this.Response.End();
            CrmUtils.MessageAlert(this.Page, "Bu sayfayý görme yetkisine sahip deðilsiniz!", "Alert");
            return;
        }

        InitDTUrun(this.DTUrun.Table);
        InitDTYukleme(this.DTYukleme.Table);
        UserVisibility();

        Session["AsamaId"] = null;
        Session["Asama"] = null;
        Session["SezonId"] = null;
        Session["Sezon"] = null;
        Session["FirmaId"] = null;
        Session["Firma"] = null;
        Session["MarkaId"] = null;
        Session["Marka"] = null;
        Session["SevkSekliId"] = null;
        Session["SevkSekli"] = null;
        Session["OdemeSekliId"] = null;
        Session["OdemeSekli"] = null;
        Session["TasiyiciFirmaId"] = null;
        Session["TasiyiciFirma"] = null;
        Session["ParaBirimiId"] = null;
        Session["ParaBirimi"] = null;

        //this.PageAlt.ActiveTabPage = this.PageAlt.TabPages.FindByName("TabYukleme");
        this.Title = "Sipariþ - Yeni";
        this.SiparisTarihi.Date = DateTime.Now;
        Guid id = Guid.Empty;
        string sID = this.Request.Params["id"].Replace("'", "").Trim();
        if ((sID != null) && (sID != "0"))
        {
            id = new Guid(sID);
            LoadDocument(id);
            this.HiddenId.Value = id.ToString();
            this.SezonId.ReadOnly = true;
            this.SezonId.Buttons[0].Enabled = false;
            this.FirmaId.ReadOnly = true;
            this.FirmaId.Buttons[0].Enabled = false;
            this.MarkaId.ReadOnly = true;
            this.MarkaId.Buttons[0].Enabled = false;
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
            if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ", "Insert"))
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
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "MenuSave1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ", "Update"))
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

            this.GridUrun.UpdateEdit();
            this.GridYukleme.UpdateEdit();

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
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "MenuSaveNew1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ", "Update"))
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

            this.GridUrun.UpdateEdit();
            this.GridYukleme.UpdateEdit();

            Guid id = SaveDocument();
            if (id == Guid.Empty)
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "Menu");
                return;
            }
            else
            {
                this.Response.Write("<script language='javascript'>{ parent.opener.location.href=parent.opener.location.href; }</script>");

                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ", "Insert"))
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
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "MenuSaveClose1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ", "Update"))
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

            this.GridUrun.UpdateEdit();
            this.GridYukleme.UpdateEdit();

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
            if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ", "Delete"))
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
        using (SqlCommand cmd = DB.SQL(this.Context, "EXEC vw_GetIthalatSiparis @Id"))
        {
            cmd.CommandTimeout = 1000;
            using (DataSet ds = new DataSet())
            {
                DB.AddParam(cmd, "@Id", id);
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);
                }
                //IDataReader rdr = cmd.ExecuteReader();
                if (ds.Tables[0].Rows.Count == 0)
                {
                    this.Response.StatusCode = 500;
                    this.Response.End();
                    return;
                }

                #region genel
                this.Title = "Sipariþ - " + ds.Tables[0].Rows[0]["Sayac"].ToString();
                this.SiparisNo.Value = ds.Tables[0].Rows[0]["Sayac"];
                this.SiparisTarihi.Value = ds.Tables[0].Rows[0]["SiparisTarihi"];
                this.NebimNo.Value = ds.Tables[0].Rows[0]["NebimNo"];
                this.Aciklama.Value = ds.Tables[0].Rows[0]["Aciklama"];
                if ((ds.Tables[0].Rows[0]["Adet"] != null) && (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["Adet"].ToString())))
                    this.Adet.Value = Convert.ToInt32(ds.Tables[0].Rows[0]["Adet"]);
                if ((ds.Tables[0].Rows[0]["Tutar"] != null) && (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["Tutar"].ToString())))
                    this.Tutar.Value = Convert.ToDecimal(Convert.ToDouble(ds.Tables[0].Rows[0]["Tutar"]));
                if ((ds.Tables[0].Rows[0]["Iskonto"] != null) && (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["Iskonto"].ToString())))
                    this.Iskonto.Value = Convert.ToDecimal(Convert.ToDouble(ds.Tables[0].Rows[0]["Iskonto"]));
                this.SonYuklemeTarihi.Value = ds.Tables[0].Rows[0]["SonYuklemeTarihi"];
                this.YurtDisi.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["YurtDisi"]);
                if ((ds.Tables[0].Rows[0]["BarcodeId"] != null) && (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["BarcodeId"].ToString())))
                    this.BarcodeId.SelectedIndex = this.BarcodeId.Items.IndexOfValue(ds.Tables[0].Rows[0]["BarcodeId"].ToString());
                if ((ds.Tables[0].Rows[0]["SalesPriceId"] != null) && (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["SalesPriceId"].ToString())))
                    this.SalesPriceId.SelectedIndex = this.SalesPriceId.Items.IndexOfValue(ds.Tables[0].Rows[0]["SalesPriceId"].ToString());
                if ((ds.Tables[0].Rows[0]["SystemImageUploadId"] != null) && (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["SystemImageUploadId"].ToString())))
                    this.SystemImageUploadId.SelectedIndex = this.SystemImageUploadId.Items.IndexOfValue(ds.Tables[0].Rows[0]["SystemImageUploadId"].ToString());
                if ((ds.Tables[0].Rows[0]["IsNebimInside"] != null) && (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["IsNebimInside"].ToString())))
                    this.IsNebimInside.SelectedIndex = this.IsNebimInside.Items.IndexOfValue(ds.Tables[0].Rows[0]["IsNebimInside"].ToString());

                bool _ilk = true;
                foreach (DataRow rdr in ds.Tables[11].Rows)
                {
                    if (_ilk)
                    {
                        ltrPnrNumbers.Text = "<a href=\"#\" onclick=\"JavaScript:PopWin = OpenPopupWinBySize('../../Genel/Issue/edit.aspx?id=" + rdr["IndexId"].ToString() + "&NoteOwner=1',850,650);PopWin.focus();\">" + rdr["IndexId"].ToString() + "</a>";
                        _ilk = false;
                    }
                    else
                        ltrPnrNumbers.Text += "<br/> <a href=\"#\" onclick=\"JavaScript:PopWin = OpenPopupWinBySize('../../Genel/Issue/edit.aspx?id=" + rdr["IndexId"].ToString() + "&NoteOwner=1',850,650);PopWin.focus();\">" + rdr["IndexId"].ToString() + "</a>";

                }



                Guid _SezonId = Guid.Empty;
                if ((ds.Tables[0].Rows[0]["SezonId"] != null) && (ds.Tables[0].Rows[0]["SezonId"].ToString() != "") && (ds.Tables[0].Rows[0]["SezonId"].ToString() != Guid.Empty.ToString()))
                    _SezonId = new Guid(ds.Tables[0].Rows[0]["SezonId"].ToString());
                Guid _AsamaId = Guid.Empty;
                if ((ds.Tables[0].Rows[0]["AsamaId"] != null) && (ds.Tables[0].Rows[0]["AsamaId"].ToString() != "") && (ds.Tables[0].Rows[0]["AsamaId"].ToString() != Guid.Empty.ToString()))
                    _AsamaId = new Guid(ds.Tables[0].Rows[0]["AsamaId"].ToString());
                Guid _MarkaId = Guid.Empty;
                if ((ds.Tables[0].Rows[0]["MarkaId"] != null) && (ds.Tables[0].Rows[0]["MarkaId"].ToString() != "") && (ds.Tables[0].Rows[0]["MarkaId"].ToString() != Guid.Empty.ToString()))
                    _MarkaId = new Guid(ds.Tables[0].Rows[0]["MarkaId"].ToString());
                Guid _FirmaId = Guid.Empty;
                if ((ds.Tables[0].Rows[0]["FirmaId"] != null) && (ds.Tables[0].Rows[0]["FirmaId"].ToString() != "") && (ds.Tables[0].Rows[0]["FirmaId"].ToString() != Guid.Empty.ToString()))
                    _FirmaId = new Guid(ds.Tables[0].Rows[0]["FirmaId"].ToString());
                Guid _TasiyiciFirmaId = Guid.Empty;
                if ((ds.Tables[0].Rows[0]["TasiyiciFirmaId"] != null) && (ds.Tables[0].Rows[0]["TasiyiciFirmaId"].ToString() != "") && (ds.Tables[0].Rows[0]["TasiyiciFirmaId"].ToString() != Guid.Empty.ToString()))
                    _TasiyiciFirmaId = new Guid(ds.Tables[0].Rows[0]["TasiyiciFirmaId"].ToString());
                Guid _ParaBirimiId = Guid.Empty;
                if ((ds.Tables[0].Rows[0]["ParaBirimiId"] != null) && (ds.Tables[0].Rows[0]["ParaBirimiId"].ToString() != "") && (ds.Tables[0].Rows[0]["ParaBirimiId"].ToString() != Guid.Empty.ToString()))
                    _ParaBirimiId = new Guid(ds.Tables[0].Rows[0]["ParaBirimiId"].ToString());
                Guid _SevkSekliId = Guid.Empty;
                if ((ds.Tables[0].Rows[0]["SevkSekliId"] != null) && (ds.Tables[0].Rows[0]["SevkSekliId"].ToString() != "") && (ds.Tables[0].Rows[0]["SevkSekliId"].ToString() != Guid.Empty.ToString()))
                    _SevkSekliId = new Guid(ds.Tables[0].Rows[0]["SevkSekliId"].ToString());
                Guid _OdemeSekliId = Guid.Empty;
                if ((ds.Tables[0].Rows[0]["OdemeSekliId"] != null) && (ds.Tables[0].Rows[0]["OdemeSekliId"].ToString() != "") && (ds.Tables[0].Rows[0]["OdemeSekliId"].ToString() != Guid.Empty.ToString()))
                    _OdemeSekliId = new Guid(ds.Tables[0].Rows[0]["OdemeSekliId"].ToString());

                ListEditItem item = new ListEditItem();
                if (_SezonId != Guid.Empty)
                {
                    item = new ListEditItem();
                    item.Value = ds.Tables[3].Rows[0]["Id"];
                    item.Text = ds.Tables[3].Rows[0]["Sezon"].ToString();
                    this.SezonId.Items.Add(item);
                    Session["SezonId"] = ds.Tables[3].Rows[0]["Id"];
                    Session["Sezon"] = ds.Tables[3].Rows[0]["Sezon"].ToString();
                    this.SezonId.SelectedIndex = 0;
                }
                if (_AsamaId != Guid.Empty)
                {
                    item = new ListEditItem();
                    item.Value = ds.Tables[4].Rows[0]["Id"];
                    item.Text = ds.Tables[4].Rows[0]["Asama"].ToString();
                    this.AsamaId.Items.Add(item);
                    Session["AsamaId"] = ds.Tables[4].Rows[0]["Id"];
                    Session["Asama"] = ds.Tables[4].Rows[0]["Asama"].ToString();
                    this.AsamaId.SelectedIndex = 0;
                }
                if (_MarkaId != Guid.Empty)
                {
                    item = new ListEditItem();
                    item.Value = ds.Tables[5].Rows[0]["Id"];
                    item.Text = ds.Tables[5].Rows[0]["Marka"].ToString();
                    this.MarkaId.Items.Add(item);
                    Session["MarkaId"] = ds.Tables[5].Rows[0]["Id"];
                    Session["Marka"] = ds.Tables[5].Rows[0]["Marka"].ToString();
                    this.MarkaId.SelectedIndex = 0;
                }
                if (_FirmaId != Guid.Empty)
                {
                    item = new ListEditItem();
                    item.Value = ds.Tables[6].Rows[0]["Id"];
                    item.Text = ds.Tables[6].Rows[0]["Adi"].ToString();
                    this.FirmaId.Items.Add(item);
                    Session["FirmaId"] = ds.Tables[6].Rows[0]["Id"];
                    Session["Firma"] = ds.Tables[6].Rows[0]["Adi"].ToString();
                    this.FirmaId.SelectedIndex = 0;
                }
                if (_TasiyiciFirmaId != Guid.Empty)
                {
                    item = new ListEditItem();
                    item.Value = ds.Tables[7].Rows[0]["Id"];
                    item.Text = ds.Tables[7].Rows[0]["Adi"].ToString();
                    this.TasiyiciFirmaId.Items.Add(item);
                    Session["TasiyiciFirmaId"] = ds.Tables[7].Rows[0]["Id"];
                    Session["TasiyiciFirma"] = ds.Tables[7].Rows[0]["Adi"].ToString();
                    this.TasiyiciFirmaId.SelectedIndex = 0;
                }
                if (_ParaBirimiId != Guid.Empty)
                {
                    item = new ListEditItem();
                    item.Value = ds.Tables[8].Rows[0]["Id"];
                    item.Text = ds.Tables[8].Rows[0]["ParaBirimi"].ToString();
                    this.ParaBirimiId.Items.Add(item);
                    Session["ParaBirimiId"] = ds.Tables[8].Rows[0]["Id"];
                    Session["ParaBirimi"] = ds.Tables[8].Rows[0]["ParaBirimi"].ToString();
                    this.ParaBirimiId.SelectedIndex = 0;
                }
                if (_SevkSekliId != Guid.Empty)
                {
                    item = new ListEditItem();
                    item.Value = ds.Tables[9].Rows[0]["Id"];
                    item.Text = ds.Tables[9].Rows[0]["SevkSekli"].ToString();
                    this.SevkSekliId.Items.Add(item);
                    Session["SevkSekliId"] = ds.Tables[9].Rows[0]["Id"];
                    Session["SevkSekli"] = ds.Tables[9].Rows[0]["SevkSekli"].ToString();
                    this.SevkSekliId.SelectedIndex = 0;
                }
                if (_OdemeSekliId != Guid.Empty)
                {
                    item = new ListEditItem();
                    item.Value = ds.Tables[10].Rows[0]["Id"];
                    item.Text = ds.Tables[10].Rows[0]["OdemeSekli"].ToString();
                    this.OdemeSekliId.Items.Add(item);
                    Session["OdemeSekliId"] = ds.Tables[10].Rows[0]["Id"];
                    Session["OdemeSekli"] = ds.Tables[10].Rows[0]["OdemeSekli"].ToString();
                    this.OdemeSekliId.SelectedIndex = 0;
                }
                #endregion

                #region ürünler
                this.DTUrun.Table.Clear();
                foreach (DataRow rdr in ds.Tables[1].Rows)
                {
                    DataRow row = this.DTUrun.Table.NewRow();
                    row["ID"] = rdr["Id"];
                    row["SiparisId"] = rdr["SiparisId"];
                    row["UrunId"] = rdr["UrunId"];
                    row["Urun"] = rdr["Urun"];
                    row["Adet"] = rdr["Adet"];
                    row["Tutar"] = rdr["Tutar"];
                    this.DTUrun.Table.Rows.Add(row);
                }
                this.DTUrun.Table.AcceptChanges();
                #endregion

                #region yüklemeler
                this.DTYukleme.Table.Clear();
                foreach (DataRow rdr in ds.Tables[2].Rows)
                {
                    DataRow row = this.DTYukleme.Table.NewRow();
                    row["ID"] = rdr["Id"];
                    row["SiparisId"] = rdr["SiparisId"];
                    row["YuklemeNo"] = rdr["YuklemeNo"];
                    row["UrunId"] = rdr["UrunId"];
                    row["Urun"] = rdr["Urun"];
                    row["SezonId"] = rdr["SezonId"];
                    row["Sezon"] = rdr["Sezon"];
                    row["Adet"] = rdr["Adet"];
                    row["Tutar"] = rdr["Tutar"];
                    row["ProformaTarihi"] = rdr["ProformaTarihi"];
                    row["AsamaId"] = rdr["AsamaId"];
                    row["Asama"] = rdr["Asama"];
                    row["Iskonto"] = rdr["Iskonto"];
                    row["SevkSekliId"] = rdr["SevkSekliId"];
                    row["SevkSekli"] = rdr["SevkSekli"];
                    row["GelenTutar"] = rdr["GelenTutar"];
                    row["OdemeIskonto"] = rdr["OdemeIskonto"];
                    row["YapilanOdeme"] = rdr["YapilanOdeme"];
                    row["KalanOdeme"] = rdr["KalanOdeme"];
                    row["OdemeSekliId"] = rdr["OdemeSekliId"];
                    row["OdemeSekli"] = rdr["OdemeSekli"];
                    row["SonYuklemeTarihi"] = rdr["SonYuklemeTarihi"];
                    row["LCNo"] = rdr["LCNo"];
                    row["BankaId"] = rdr["BankaId"];
                    row["Banka"] = rdr["Banka"];
                    row["TasiyiciFirmaId"] = rdr["TasiyiciFirmaId"];
                    row["TasiyiciFirma"] = rdr["TasiyiciFirma"];
                    row["OdemeVadesi"] = rdr["OdemeVadesi"];
                    row["TahminiVarisTarihi"] = rdr["TahminiVarisTarihi"];
                    row["GumrukVarisTarihi"] = rdr["GumrukVarisTarihi"];
                    row["TahminiDepoGirisTarihi"] = rdr["TahminiDepoGirisTarihi"];
                    row["DepoGirisTarihi"] = rdr["DepoGirisTarihi"];
                    row["DepoId"] = rdr["DepoId"];
                    row["Depo"] = rdr["Depo"];
                    row["TahminiMagazaDagitimTarihi"] = rdr["TahminiMagazaDagitimTarihi"];
                    row["MagazaDagitimTarihi"] = rdr["MagazaDagitimTarihi"];
                    row["Agirlik"] = rdr["Agirlik"];
                    row["PaketAdet"] = rdr["PaketAdet"];
                    row["Aciklama"] = rdr["Aciklama"];
                    row["EksikUrun"] = rdr["EksikUrun"];
                    row["FazlaUrun"] = rdr["FazlaUrun"];
                    row["IsEkMaliyet"] = rdr["IsEkMaliyet"];
                    this.DTYukleme.Table.Rows.Add(row);

                }
                this.DTYukleme.Table.AcceptChanges();
                #endregion

            }
        }




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
        StringBuilder sb1 = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();

        #region siparis
        if (id == Guid.Empty)
        {
            #region insert
            sb1 = new StringBuilder("INSERT INTO IthalatSiparis(Id,NebimNo,Aciklama,YurtDisi,CreatedBy,CreationDate");
            sb2 = new StringBuilder(" VALUES(@Id,@NebimNo,@Aciklama,@YurtDisi,@CreatedBy,@CreationDate");

            if ((this.SiparisTarihi.Value != null) && (!CrmUtils.IsNullOrEmptyDateTime(this.SiparisTarihi.Value.ToString())))
            {
                sb1.Append(",SiparisTarihi");
                sb2.Append(",@SiparisTarihi");
            }
            if ((this.Adet.Value != null) && (!String.IsNullOrEmpty(this.Adet.Value.ToString())))
            {
                sb1.Append(",Adet");
                sb2.Append(",@Adet");
            }
            if ((this.Tutar.Value != null) && (!String.IsNullOrEmpty(this.Tutar.Value.ToString())))
            {
                sb1.Append(",Tutar");
                sb2.Append(",@Tutar");
            }
            if ((this.Iskonto.Value != null) && (!String.IsNullOrEmpty(this.Iskonto.Value.ToString())))
            {
                sb1.Append(",Iskonto");
                sb2.Append(",@Iskonto");
            }
            if ((this.SezonId.SelectedIndex >= 0) && (this.SezonId.Value.ToString() != Guid.Empty.ToString()))
            {
                sb1.Append(",SezonId");
                sb2.Append(",@SezonId");
            }
            if ((this.AsamaId.SelectedIndex >= 0) && (this.AsamaId.Value.ToString() != Guid.Empty.ToString()))
            {
                sb1.Append(",AsamaId");
                sb2.Append(",@AsamaId");
            }
            if ((this.MarkaId.SelectedIndex >= 0) && (this.MarkaId.Value.ToString() != Guid.Empty.ToString()))
            {
                sb1.Append(",MarkaId");
                sb2.Append(",@MarkaId");
            }
            if ((this.FirmaId.SelectedIndex >= 0) && (this.FirmaId.Value.ToString() != Guid.Empty.ToString()))
            {
                sb1.Append(",FirmaId");
                sb2.Append(",@FirmaId");
            }
            if ((this.TasiyiciFirmaId.SelectedIndex >= 0) && (this.TasiyiciFirmaId.Value.ToString() != Guid.Empty.ToString()))
            {
                sb1.Append(",TasiyiciFirmaId");
                sb2.Append(",@TasiyiciFirmaId");
            }
            if ((this.ParaBirimiId.SelectedIndex >= 0) && (this.ParaBirimiId.Value.ToString() != Guid.Empty.ToString()))
            {
                sb1.Append(",ParaBirimiId");
                sb2.Append(",@ParaBirimiId");
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
            if ((this.SonYuklemeTarihi.Value != null) && (!CrmUtils.IsNullOrEmptyDateTime(this.SonYuklemeTarihi.Value.ToString())))
            {
                sb1.Append(",SonYuklemeTarihi");
                sb2.Append(",@SonYuklemeTarihi");
            }
            if (!String.IsNullOrEmpty(this.BarcodeId.Text))
            {
                sb1.Append(",BarcodeId");
                sb2.Append(",@BarcodeId");
            }
            if (!String.IsNullOrEmpty(this.SalesPriceId.Text))
            {
                sb1.Append(",SalesPriceId");
                sb2.Append(",@SalesPriceId");
            }
            if (!String.IsNullOrEmpty(this.SystemImageUploadId.Text))
            {
                sb1.Append(",SystemImageUploadId");
                sb2.Append(",@SystemImageUploadId");
            }
            if (!String.IsNullOrEmpty(this.IsNebimInside.Text))
            {
                sb1.Append(",IsNebimInside");
                sb2.Append(",@IsNebimInside");
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
            sb.Append("UPDATE IthalatSiparis SET NebimNo=@NebimNo,Aciklama=@Aciklama,YurtDisi=@YurtDisi,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate");
            if ((this.SiparisTarihi.Value != null) && (!CrmUtils.IsNullOrEmptyDateTime(this.SiparisTarihi.Value.ToString()))) sb.Append(",SiparisTarihi=@SiparisTarihi");
            else sb.Append(",SiparisTarihi=NULL");
            if ((this.Adet.Value != null) && (!String.IsNullOrEmpty(this.Adet.Value.ToString()))) sb.Append(",Adet=@Adet");
            else sb.Append(",Adet=NULL");
            if ((this.Tutar.Value != null) && (!String.IsNullOrEmpty(this.Tutar.Value.ToString()))) sb.Append(",Tutar=@Tutar");
            else sb.Append(",Tutar=NULL");
            if ((this.Iskonto.Value != null) && (!String.IsNullOrEmpty(this.Iskonto.Value.ToString()))) sb.Append(",Iskonto=@Iskonto");
            else sb.Append(",Iskonto=NULL");
            if ((this.SezonId.SelectedIndex >= 0) && (this.SezonId.Value.ToString() != Guid.Empty.ToString())) sb.Append(",SezonId=@SezonId");
            else sb.Append(",SezonId=NULL");
            if ((this.AsamaId.SelectedIndex >= 0) && (this.AsamaId.Value.ToString() != Guid.Empty.ToString())) sb.Append(",AsamaId=@AsamaId");
            else sb.Append(",AsamaId=NULL");
            if ((this.MarkaId.SelectedIndex >= 0) && (this.MarkaId.Value.ToString() != Guid.Empty.ToString())) sb.Append(",MarkaId=@MarkaId");
            else sb.Append(",MarkaId=NULL");
            if ((this.FirmaId.SelectedIndex >= 0) && (this.FirmaId.Value.ToString() != Guid.Empty.ToString())) sb.Append(",FirmaId=@FirmaId");
            else sb.Append(",FirmaId=NULL");
            if ((this.TasiyiciFirmaId.SelectedIndex >= 0) && (this.TasiyiciFirmaId.Value.ToString() != Guid.Empty.ToString())) sb.Append(",TasiyiciFirmaId=@TasiyiciFirmaId");
            else sb.Append(",TasiyiciFirmaId=NULL");
            if ((this.ParaBirimiId.SelectedIndex >= 0) && (this.ParaBirimiId.Value.ToString() != Guid.Empty.ToString())) sb.Append(",ParaBirimiId=@ParaBirimiId");
            else sb.Append(",ParaBirimiId=NULL");
            if ((this.SevkSekliId.SelectedIndex >= 0) && (this.SevkSekliId.Value.ToString() != Guid.Empty.ToString())) sb.Append(",SevkSekliId=@SevkSekliId");
            else sb.Append(",SevkSekliId=NULL");
            if ((this.OdemeSekliId.SelectedIndex >= 0) && (this.OdemeSekliId.Value.ToString() != Guid.Empty.ToString())) sb.Append(",OdemeSekliId=@OdemeSekliId");
            else sb.Append(",OdemeSekliId=NULL");
            if ((this.SonYuklemeTarihi.Value != null) && (!CrmUtils.IsNullOrEmptyDateTime(this.SonYuklemeTarihi.Value.ToString()))) sb.Append(",SonYuklemeTarihi=@SonYuklemeTarihi");
            else sb.Append(",SonYuklemeTarihi=NULL");
            if (!String.IsNullOrEmpty(this.BarcodeId.Text)) sb.Append(",BarcodeId=@BarcodeId");
            else sb.Append(",BarcodeId=NULL");
            if (!String.IsNullOrEmpty(this.SalesPriceId.Text)) sb.Append(",SalesPriceId=@SalesPriceId");
            else sb.Append(",SalesPriceId=NULL");
            if (!String.IsNullOrEmpty(this.SystemImageUploadId.Text)) sb.Append(",SystemImageUploadId=@SystemImageUploadId");
            else sb.Append(",SystemImageUploadId=NULL");
            if (!String.IsNullOrEmpty(this.IsNebimInside.Text)) sb.Append(",IsNebimInside=@IsNebimInside");
            else sb.Append(",IsNebimInside=NULL");
            sb.Append(" WHERE Id=@Id");

            cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@Id", id);
            DB.AddParam(cmd, "@ModifiedBy", 50, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
            #endregion
        }

        #region values
        DB.AddParam(cmd, "@NebimNo", 100, this.NebimNo.Value);
        DB.AddParam(cmd, "@Aciklama", 255, this.Aciklama.Value);
        if ((this.SiparisTarihi.Value != null) && (!CrmUtils.IsNullOrEmptyDateTime(this.SiparisTarihi.Value.ToString())))
            DB.AddParam(cmd, "@SiparisTarihi", this.SiparisTarihi.Date);
        if ((this.Adet.Value != null) && (!String.IsNullOrEmpty(this.Adet.Value.ToString())))
            DB.AddParam(cmd, "@Adet", 60, this.Adet.Value.ToString().Replace(",", "."));
        if ((this.Tutar.Value != null) && (!String.IsNullOrEmpty(this.Tutar.Value.ToString())))
            DB.AddParam(cmd, "@Tutar", 60, this.Tutar.Value.ToString().Replace(",", "."));
        if ((this.Iskonto.Value != null) && (!String.IsNullOrEmpty(this.Iskonto.Value.ToString())))
            DB.AddParam(cmd, "@Iskonto", 60, this.Iskonto.Value.ToString().Replace(",", "."));
        if ((this.SezonId.SelectedIndex >= 0) && (this.SezonId.Value.ToString() != Guid.Empty.ToString()))
            DB.AddParam(cmd, "@SezonId", (new Guid(this.SezonId.Value.ToString())));
        if ((this.AsamaId.SelectedIndex >= 0) && (this.AsamaId.Value.ToString() != Guid.Empty.ToString()))
            DB.AddParam(cmd, "@AsamaId", (new Guid(this.AsamaId.Value.ToString())));
        if ((this.MarkaId.SelectedIndex >= 0) && (this.MarkaId.Value.ToString() != Guid.Empty.ToString()))
            DB.AddParam(cmd, "@MarkaId", (new Guid(this.MarkaId.Value.ToString())));
        if ((this.FirmaId.SelectedIndex >= 0) && (this.FirmaId.Value.ToString() != Guid.Empty.ToString()))
            DB.AddParam(cmd, "@FirmaId", (new Guid(this.FirmaId.Value.ToString())));
        if ((this.TasiyiciFirmaId.SelectedIndex >= 0) && (this.TasiyiciFirmaId.Value.ToString() != Guid.Empty.ToString()))
            DB.AddParam(cmd, "@TasiyiciFirmaId", (new Guid(this.TasiyiciFirmaId.Value.ToString())));
        if ((this.ParaBirimiId.SelectedIndex >= 0) && (this.ParaBirimiId.Value.ToString() != Guid.Empty.ToString()))
            DB.AddParam(cmd, "@ParaBirimiId", (new Guid(this.ParaBirimiId.Value.ToString())));
        if ((this.SevkSekliId.SelectedIndex >= 0) && (this.SevkSekliId.Value.ToString() != Guid.Empty.ToString()))
            DB.AddParam(cmd, "@SevkSekliId", (new Guid(this.SevkSekliId.Value.ToString())));
        if ((this.OdemeSekliId.SelectedIndex >= 0) && (this.OdemeSekliId.Value.ToString() != Guid.Empty.ToString()))
            DB.AddParam(cmd, "@OdemeSekliId", (new Guid(this.OdemeSekliId.Value.ToString())));
        if ((this.SonYuklemeTarihi.Value != null) && (!CrmUtils.IsNullOrEmptyDateTime(this.SonYuklemeTarihi.Value.ToString())))
            DB.AddParam(cmd, "@SonYuklemeTarihi", this.SonYuklemeTarihi.Date);
        DB.AddParam(cmd, "@YurtDisi", (this.YurtDisi.Checked ? 1 : 0));
        if (!String.IsNullOrEmpty(this.BarcodeId.Text))
            DB.AddParam(cmd, "@BarcodeId", int.Parse(this.BarcodeId.Value.ToString()));
        if (!String.IsNullOrEmpty(this.SalesPriceId.Text))
            DB.AddParam(cmd, "@SalesPriceId", int.Parse(this.SalesPriceId.Value.ToString()));
        if (!String.IsNullOrEmpty(this.SystemImageUploadId.Text))
            DB.AddParam(cmd, "@SystemImageUploadId", int.Parse(this.SystemImageUploadId.Value.ToString()));
        if (!String.IsNullOrEmpty(this.IsNebimInside.Text))
            DB.AddParam(cmd, "@IsNebimInside", int.Parse(this.IsNebimInside.Value.ToString()));
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

        #region siparis tarihce
        sb1 = new StringBuilder("INSERT INTO IthalatSiparisTarihce(Id,NebimNo,Aciklama,ModifiedBy,ModificationDate");
        sb1.Append(",SiparisTarihi,Adet,Tutar,Iskonto,SezonId,AsamaId,MarkaId,FirmaId,TasiyiciFirmaId");
        sb1.Append(",ParaBirimiId,SevkSekliId,OdemeSekliId,SonYuklemeTarihi,YurtDisi) ");
        sb2 = new StringBuilder("SELECT Id,NebimNo,Aciklama,@ModifiedBy,@ModificationDate");
        sb2.Append(",SiparisTarihi,Adet,Tutar,Iskonto,SezonId,AsamaId,MarkaId,FirmaId,TasiyiciFirmaId");
        sb2.Append(",ParaBirimiId,SevkSekliId,OdemeSekliId,SonYuklemeTarihi,YurtDisi ");
        sb2.Append("FROM IthalatSiparis WHERE Id=@Id");
        cmd = DB.SQL(this.Context, (sb1.ToString() + sb2.ToString()));
        DB.AddParam(cmd, "@Id", id);
        DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
        DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
        int _SiparisTarihceSayac = 0;
        try
        {
            cmd.CommandTimeout = 1000;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd = DB.SQL(this.Context, "SELECT MAX(Sayac) FROM IthalatSiparisTarihce WHERE Id=@Id");
            DB.AddParam(cmd, "@Id", id);
            cmd.CommandTimeout = 1000;
            _SiparisTarihceSayac = (int)cmd.ExecuteScalar();
        }
        catch (Exception ex)
        {
            DB.Rollback(this.Context);
            this.Session["HATA"] = ex.Message.ToString();
            return Guid.Empty;
        }
        #endregion

        #region ürünler
        DataTable changes = this.DTUrun.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        #region insert
                        sb1 = new StringBuilder("INSERT INTO IthalatSiparisUrun(Id,SiparisId,Aciklama,CreatedBy,CreationDate");
                        sb2 = new StringBuilder("VALUES(@Id,@SiparisId,@Aciklama,@CreatedBy,@CreationDate");
                        if ((row["UrunId"] != null) && (!String.IsNullOrEmpty(row["UrunId"].ToString())))
                        {
                            sb1.Append(",UrunId");
                            sb2.Append(",@UrunId");
                        }
                        if ((row["Adet"] != null) && (!String.IsNullOrEmpty(row["Adet"].ToString())))
                        {
                            sb1.Append(",Adet");
                            sb2.Append(",@Adet");
                        }
                        if ((row["Tutar"] != null) && (!String.IsNullOrEmpty(row["Tutar"].ToString())))
                        {
                            sb1.Append(",Tutar");
                            sb2.Append(",@Tutar");
                        }
                        sb1.Append(") ");
                        sb2.Append(")");
                        try
                        {
                            cmd = DB.SQL(this.Context, (sb1.ToString() + sb2.ToString()));
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@SiparisId", id);
                            if ((row["UrunId"] != null) && (!String.IsNullOrEmpty(row["UrunId"].ToString())))
                                DB.AddParam(cmd, "@UrunId", (new Guid(row["UrunId"].ToString())));
                            if ((row["Adet"] != null) && (!String.IsNullOrEmpty(row["Adet"].ToString())))
                                DB.AddParam(cmd, "@Adet", 60, row["Adet"].ToString().Replace(",", "."));
                            if ((row["Tutar"] != null) && (!String.IsNullOrEmpty(row["Tutar"].ToString())))
                                DB.AddParam(cmd, "@Tutar", 60, row["Tutar"].ToString().Replace(",", "."));
                            DB.AddParam(cmd, "@Aciklama", 255, row["Aciklama"].ToString());
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
                        sb = new StringBuilder("UPDATE IthalatSiparisUrun ");
                        sb.Append("SET SiparisId=@SiparisId,Aciklama=@Aciklama,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate");
                        if ((row["UrunId"] != null) && (!String.IsNullOrEmpty(row["UrunId"].ToString()))) sb.Append(",UrunId=@UrunId");
                        else sb.Append(",UrunId=NULL");
                        if ((row["Adet"] != null) && (!String.IsNullOrEmpty(row["Adet"].ToString()))) sb.Append(",Adet=@Adet");
                        else sb.Append(",Adet=NULL");
                        if ((row["Tutar"] != null) && (!String.IsNullOrEmpty(row["Tutar"].ToString()))) sb.Append(",Tutar=@Tutar");
                        else sb.Append(",Tutar=NULL");
                        sb.Append(" WHERE Id=@Id");
                        try
                        {
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@SiparisId", id);
                            if ((row["UrunId"] != null) && (!String.IsNullOrEmpty(row["UrunId"].ToString())))
                                DB.AddParam(cmd, "@UrunId", (new Guid(row["UrunId"].ToString())));
                            if ((row["Adet"] != null) && (!String.IsNullOrEmpty(row["Adet"].ToString())))
                                DB.AddParam(cmd, "@Adet", 60, row["Adet"].ToString().Replace(",", "."));
                            if ((row["Tutar"] != null) && (!String.IsNullOrEmpty(row["Tutar"].ToString())))
                                DB.AddParam(cmd, "@Tutar", 60, row["Tutar"].ToString().Replace(",", "."));
                            DB.AddParam(cmd, "@Aciklama", 255, row["Aciklama"].ToString());
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
                            cmd = DB.SQL(this.Context, "DELETE FROM IthalatSiparisUrun WHERE Id=@Id");
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

        #region ürünler tarihçe
        sb1 = new StringBuilder("INSERT INTO IthalatSiparisUrunTarihce(Id,SiparisSayac,SiparisId,UrunId,Adet,Tutar,Aciklama,ModifiedBy,ModificationDate) ");
        sb2 = new StringBuilder("SELECT Id,@SiparisSayac,SiparisId,UrunId,Adet,Tutar,Aciklama,@ModifiedBy,@ModificationDate ");
        sb2.Append("FROM IthalatSiparisUrun WHERE SiparisId=@SiparisId");
        try
        {
            cmd = DB.SQL(this.Context, (sb1.ToString() + sb2.ToString()));
            DB.AddParam(cmd, "@SiparisSayac", _SiparisTarihceSayac);
            DB.AddParam(cmd, "@SiparisId", id);
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

        #region yüklemeler
        changes = this.DTYukleme.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        #region insert
                        sb1 = new StringBuilder("INSERT INTO IthalatSiparisYukleme(Id,SiparisId,LCNo,Aciklama,CreatedBy,CreationDate");
                        sb2 = new StringBuilder("VALUES(@Id,@SiparisId,@LCNo,@Aciklama,@CreatedBy,@CreationDate");
                        if ((row["YuklemeNo"] != null) && (!String.IsNullOrEmpty(row["YuklemeNo"].ToString())))
                        {
                            sb1.Append(",YuklemeNo");
                            sb2.Append(",@YuklemeNo");
                        }
                        if ((row["UrunId"] != null) && (!String.IsNullOrEmpty(row["UrunId"].ToString())))
                        {
                            sb1.Append(",UrunId");
                            sb2.Append(",@UrunId");
                        }
                        if ((row["SezonId"] != null) && (!String.IsNullOrEmpty(row["SezonId"].ToString())))
                        {
                            sb1.Append(",SezonId");
                            sb2.Append(",@SezonId");
                        }
                        if ((row["Adet"] != null) && (!String.IsNullOrEmpty(row["Adet"].ToString())))
                        {
                            sb1.Append(",Adet");
                            sb2.Append(",@Adet");
                        }
                        if ((row["Tutar"] != null) && (!String.IsNullOrEmpty(row["Tutar"].ToString())))
                        {
                            sb1.Append(",Tutar");
                            sb2.Append(",@Tutar");
                        }
                        if ((row["ProformaTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["ProformaTarihi"].ToString())))
                        {
                            sb1.Append(",ProformaTarihi");
                            sb2.Append(",@ProformaTarihi");
                        }
                        if ((row["AsamaId"] != null) && (!String.IsNullOrEmpty(row["AsamaId"].ToString())))
                        {
                            sb1.Append(",AsamaId");
                            sb2.Append(",@AsamaId");
                        }
                        if ((row["Iskonto"] != null) && (!String.IsNullOrEmpty(row["Iskonto"].ToString())))
                        {
                            sb1.Append(",Iskonto");
                            sb2.Append(",@Iskonto");
                        }
                        if ((row["SevkSekliId"] != null) && (!String.IsNullOrEmpty(row["SevkSekliId"].ToString())))
                        {
                            sb1.Append(",SevkSekliId");
                            sb2.Append(",@SevkSekliId");
                        }
                        if ((row["GelenTutar"] != null) && (!String.IsNullOrEmpty(row["GelenTutar"].ToString())))
                        {
                            sb1.Append(",GelenTutar");
                            sb2.Append(",@GelenTutar");
                        }
                        if ((row["OdemeIskonto"] != null) && (!String.IsNullOrEmpty(row["OdemeIskonto"].ToString())))
                        {
                            sb1.Append(",OdemeIskonto");
                            sb2.Append(",@OdemeIskonto");
                        }
                        if ((row["YapilanOdeme"] != null) && (!String.IsNullOrEmpty(row["YapilanOdeme"].ToString())))
                        {
                            sb1.Append(",YapilanOdeme");
                            sb2.Append(",@YapilanOdeme");
                        }
                        if ((row["KalanOdeme"] != null) && (!String.IsNullOrEmpty(row["KalanOdeme"].ToString())))
                        {
                            sb1.Append(",KalanOdeme");
                            sb2.Append(",@KalanOdeme");
                        }
                        if ((row["OdemeSekliId"] != null) && (!String.IsNullOrEmpty(row["OdemeSekliId"].ToString())))
                        {
                            sb1.Append(",OdemeSekliId");
                            sb2.Append(",@OdemeSekliId");
                        }
                        if ((row["SonYuklemeTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["SonYuklemeTarihi"].ToString())))
                        {
                            sb1.Append(",SonYuklemeTarihi");
                            sb2.Append(",@SonYuklemeTarihi");
                        }
                        if ((row["BankaId"] != null) && (!String.IsNullOrEmpty(row["BankaId"].ToString())))
                        {
                            sb1.Append(",BankaId");
                            sb2.Append(",@BankaId");
                        }
                        if ((row["TasiyiciFirmaId"] != null) && (!String.IsNullOrEmpty(row["TasiyiciFirmaId"].ToString())))
                        {
                            sb1.Append(",TasiyiciFirmaId");
                            sb2.Append(",@TasiyiciFirmaId");
                        }
                        if ((row["OdemeVadesi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["OdemeVadesi"].ToString())))
                        {
                            sb1.Append(",OdemeVadesi");
                            sb2.Append(",@OdemeVadesi");
                        }
                        if ((row["TahminiVarisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["TahminiVarisTarihi"].ToString())))
                        {
                            sb1.Append(",TahminiVarisTarihi");
                            sb2.Append(",@TahminiVarisTarihi");
                        }
                        if ((row["GumrukVarisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["GumrukVarisTarihi"].ToString())))
                        {
                            sb1.Append(",GumrukVarisTarihi");
                            sb2.Append(",@GumrukVarisTarihi");
                        }
                        if ((row["TahminiDepoGirisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["TahminiDepoGirisTarihi"].ToString())))
                        {
                            sb1.Append(",TahminiDepoGirisTarihi");
                            sb2.Append(",@TahminiDepoGirisTarihi");
                        }
                        if ((row["DepoGirisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["DepoGirisTarihi"].ToString())))
                        {
                            sb1.Append(",DepoGirisTarihi");
                            sb2.Append(",@DepoGirisTarihi");
                        }
                        if ((row["DepoId"] != null) && (!String.IsNullOrEmpty(row["DepoId"].ToString())))
                        {
                            sb1.Append(",DepoId");
                            sb2.Append(",@DepoId");
                        }
                        if ((row["TahminiMagazaDagitimTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["TahminiMagazaDagitimTarihi"].ToString())))
                        {
                            sb1.Append(",TahminiMagazaDagitimTarihi");
                            sb2.Append(",@TahminiMagazaDagitimTarihi");
                        }
                        if ((row["MagazaDagitimTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["MagazaDagitimTarihi"].ToString())))
                        {
                            sb1.Append(",MagazaDagitimTarihi");
                            sb2.Append(",@MagazaDagitimTarihi");
                        }
                        if ((row["Agirlik"] != null) && (!String.IsNullOrEmpty(row["Agirlik"].ToString())))
                        {
                            sb1.Append(",Agirlik");
                            sb2.Append(",@Agirlik");
                        }
                        if ((row["PaketAdet"] != null) && (!String.IsNullOrEmpty(row["PaketAdet"].ToString())))
                        {
                            sb1.Append(",PaketAdet");
                            sb2.Append(",@PaketAdet");
                        }
                        if ((row["EksikUrun"] != null) && (!String.IsNullOrEmpty(row["EksikUrun"].ToString())))
                        {
                            sb1.Append(",EksikUrun");
                            sb2.Append(",@EksikUrun");
                        }
                        if ((row["FazlaUrun"] != null) && (!String.IsNullOrEmpty(row["FazlaUrun"].ToString())))
                        {
                            sb1.Append(",FazlaUrun");
                            sb2.Append(",@FazlaUrun");
                        }
                        if ((row["IsEkMaliyet"] != null) && (!String.IsNullOrEmpty(row["IsEkMaliyet"].ToString())))
                        {
                            sb1.Append(",IsEkMaliyet");
                            sb2.Append(",@IsEkMaliyet");
                        }
                        sb1.Append(") ");
                        sb2.Append(")");
                        try
                        {
                            cmd = DB.SQL(this.Context, (sb1.ToString() + sb2.ToString()));
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@SiparisId", id);
                            if ((row["YuklemeNo"] != null) && (!String.IsNullOrEmpty(row["YuklemeNo"].ToString())))
                                DB.AddParam(cmd, "@YuklemeNo", 60, row["YuklemeNo"].ToString().Replace(",", "."));
                            if ((row["UrunId"] != null) && (!String.IsNullOrEmpty(row["UrunId"].ToString())))
                                DB.AddParam(cmd, "@UrunId", (new Guid(row["UrunId"].ToString())));
                            if ((row["SezonId"] != null) && (!String.IsNullOrEmpty(row["SezonId"].ToString())))
                                DB.AddParam(cmd, "@SezonId", (new Guid(row["SezonId"].ToString())));
                            if ((row["Adet"] != null) && (!String.IsNullOrEmpty(row["Adet"].ToString())))
                                DB.AddParam(cmd, "@Adet", 60, row["Adet"].ToString().Replace(",", "."));
                            if ((row["Tutar"] != null) && (!String.IsNullOrEmpty(row["Tutar"].ToString())))
                                DB.AddParam(cmd, "@Tutar", 60, row["Tutar"].ToString().Replace(",", "."));
                            if ((row["ProformaTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["ProformaTarihi"].ToString())))
                                DB.AddParam(cmd, "@ProformaTarihi", (DateTime)row["ProformaTarihi"]);
                            if ((row["AsamaId"] != null) && (!String.IsNullOrEmpty(row["AsamaId"].ToString())))
                                DB.AddParam(cmd, "@AsamaId", (new Guid(row["AsamaId"].ToString())));
                            if ((row["Iskonto"] != null) && (!String.IsNullOrEmpty(row["Iskonto"].ToString())))
                                DB.AddParam(cmd, "@Iskonto", 60, row["Iskonto"].ToString().Replace(",", "."));
                            if ((row["SevkSekliId"] != null) && (!String.IsNullOrEmpty(row["SevkSekliId"].ToString())))
                                DB.AddParam(cmd, "@SevkSekliId", (new Guid(row["SevkSekliId"].ToString())));
                            if ((row["GelenTutar"] != null) && (!String.IsNullOrEmpty(row["GelenTutar"].ToString())))
                                DB.AddParam(cmd, "@GelenTutar", 60, row["GelenTutar"].ToString().Replace(",", "."));
                            if ((row["OdemeIskonto"] != null) && (!String.IsNullOrEmpty(row["OdemeIskonto"].ToString())))
                                DB.AddParam(cmd, "@OdemeIskonto", 60, row["OdemeIskonto"].ToString().Replace(",", "."));
                            if ((row["YapilanOdeme"] != null) && (!String.IsNullOrEmpty(row["YapilanOdeme"].ToString())))
                                DB.AddParam(cmd, "@YapilanOdeme", 60, row["YapilanOdeme"].ToString().Replace(",", "."));
                            if ((row["KalanOdeme"] != null) && (!String.IsNullOrEmpty(row["KalanOdeme"].ToString())))
                                DB.AddParam(cmd, "@KalanOdeme", 60, row["KalanOdeme"].ToString().Replace(",", "."));
                            if ((row["OdemeSekliId"] != null) && (!String.IsNullOrEmpty(row["OdemeSekliId"].ToString())))
                                DB.AddParam(cmd, "@OdemeSekliId", (new Guid(row["OdemeSekliId"].ToString())));
                            if ((row["SonYuklemeTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["SonYuklemeTarihi"].ToString())))
                                DB.AddParam(cmd, "@SonYuklemeTarihi", (DateTime)row["SonYuklemeTarihi"]);
                            if ((row["BankaId"] != null) && (!String.IsNullOrEmpty(row["BankaId"].ToString())))
                                DB.AddParam(cmd, "@BankaId", (new Guid(row["BankaId"].ToString())));
                            if ((row["TasiyiciFirmaId"] != null) && (!String.IsNullOrEmpty(row["TasiyiciFirmaId"].ToString())))
                                DB.AddParam(cmd, "@TasiyiciFirmaId", (new Guid(row["TasiyiciFirmaId"].ToString())));
                            if ((row["OdemeVadesi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["OdemeVadesi"].ToString())))
                                DB.AddParam(cmd, "@OdemeVadesi", (DateTime)row["OdemeVadesi"]);
                            if ((row["TahminiVarisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["TahminiVarisTarihi"].ToString())))
                                DB.AddParam(cmd, "@TahminiVarisTarihi", (DateTime)row["TahminiVarisTarihi"]);
                            if ((row["GumrukVarisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["GumrukVarisTarihi"].ToString())))
                                DB.AddParam(cmd, "@GumrukVarisTarihi", (DateTime)row["GumrukVarisTarihi"]);
                            if ((row["TahminiDepoGirisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["TahminiDepoGirisTarihi"].ToString())))
                                DB.AddParam(cmd, "@TahminiDepoGirisTarihi", (DateTime)row["TahminiDepoGirisTarihi"]);
                            if ((row["DepoGirisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["DepoGirisTarihi"].ToString())))
                                DB.AddParam(cmd, "@DepoGirisTarihi", (DateTime)row["DepoGirisTarihi"]);
                            if ((row["DepoId"] != null) && (!String.IsNullOrEmpty(row["DepoId"].ToString())))
                                DB.AddParam(cmd, "@DepoId", (new Guid(row["DepoId"].ToString())));
                            if ((row["TahminiMagazaDagitimTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["TahminiMagazaDagitimTarihi"].ToString())))
                                DB.AddParam(cmd, "@TahminiMagazaDagitimTarihi", (DateTime)row["TahminiMagazaDagitimTarihi"]);
                            if ((row["MagazaDagitimTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["MagazaDagitimTarihi"].ToString())))
                                DB.AddParam(cmd, "@MagazaDagitimTarihi", (DateTime)row["MagazaDagitimTarihi"]);
                            if ((row["Agirlik"] != null) && (!String.IsNullOrEmpty(row["Agirlik"].ToString())))
                                DB.AddParam(cmd, "@Agirlik", 60, row["Agirlik"].ToString().Replace(",", "."));
                            if ((row["PaketAdet"] != null) && (!String.IsNullOrEmpty(row["PaketAdet"].ToString())))
                                DB.AddParam(cmd, "@PaketAdet", 60, row["PaketAdet"].ToString().Replace(",", "."));
                            if ((row["EksikUrun"] != null) && (!String.IsNullOrEmpty(row["EksikUrun"].ToString())))
                                DB.AddParam(cmd, "@EksikUrun", 60, row["EksikUrun"].ToString().Replace(",", "."));
                            if ((row["FazlaUrun"] != null) && (!String.IsNullOrEmpty(row["FazlaUrun"].ToString())))
                                DB.AddParam(cmd, "@FazlaUrun", 60, row["FazlaUrun"].ToString().Replace(",", "."));
                            if ((row["IsEkMaliyet"] != null) && (!String.IsNullOrEmpty(row["IsEkMaliyet"].ToString())))
                                DB.AddParam(cmd, "@IsEkMaliyet", Convert.ToBoolean(row["IsEkMaliyet"]) ? 1 : 0);
                            DB.AddParam(cmd, "@LCNo", 255, row["LCNo"].ToString());
                            DB.AddParam(cmd, "@Aciklama", 255, row["Aciklama"].ToString());
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
                        sb = new StringBuilder("UPDATE IthalatSiparisYukleme ");
                        sb.Append("SET SiparisId=@SiparisId,LCNo=@LCNo,Aciklama=@Aciklama,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate");
                        if ((row["YuklemeNo"] != null) && (!String.IsNullOrEmpty(row["YuklemeNo"].ToString()))) sb.Append(",YuklemeNo=@YuklemeNo");
                        else sb.Append(",YuklemeNo=NULL");
                        if ((row["UrunId"] != null) && (!String.IsNullOrEmpty(row["UrunId"].ToString()))) sb.Append(",UrunId=@UrunId");
                        else sb.Append(",UrunId=NULL");
                        if ((row["SezonId"] != null) && (!String.IsNullOrEmpty(row["SezonId"].ToString()))) sb.Append(",SezonId=@SezonId");
                        else sb.Append(",SezonId=NULL");
                        if ((row["Adet"] != null) && (!String.IsNullOrEmpty(row["Adet"].ToString()))) sb.Append(",Adet=@Adet");
                        else sb.Append(",Adet=NULL");
                        if ((row["Tutar"] != null) && (!String.IsNullOrEmpty(row["Tutar"].ToString()))) sb.Append(",Tutar=@Tutar");
                        else sb.Append(",Tutar=NULL");
                        if ((row["ProformaTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["ProformaTarihi"].ToString()))) sb.Append(",ProformaTarihi=@ProformaTarihi");
                        else sb.Append(",ProformaTarihi=NULL");
                        if ((row["AsamaId"] != null) && (!String.IsNullOrEmpty(row["AsamaId"].ToString()))) sb.Append(",AsamaId=@AsamaId");
                        else sb.Append(",AsamaId=NULL");
                        if ((row["Iskonto"] != null) && (!String.IsNullOrEmpty(row["Iskonto"].ToString()))) sb.Append(",Iskonto=@Iskonto");
                        else sb.Append(",Iskonto=NULL");
                        if ((row["SevkSekliId"] != null) && (!String.IsNullOrEmpty(row["SevkSekliId"].ToString()))) sb.Append(",SevkSekliId=@SevkSekliId");
                        else sb.Append(",SevkSekliId=NULL");
                        if ((row["GelenTutar"] != null) && (!String.IsNullOrEmpty(row["GelenTutar"].ToString()))) sb.Append(",GelenTutar=@GelenTutar");
                        else sb.Append(",GelenTutar=NULL");
                        if ((row["OdemeIskonto"] != null) && (!String.IsNullOrEmpty(row["OdemeIskonto"].ToString()))) sb.Append(",OdemeIskonto=@OdemeIskonto");
                        else sb.Append(",OdemeIskonto=NULL");
                        if ((row["YapilanOdeme"] != null) && (!String.IsNullOrEmpty(row["YapilanOdeme"].ToString()))) sb.Append(",YapilanOdeme=@YapilanOdeme");
                        else sb.Append(",YapilanOdeme=NULL");
                        if ((row["KalanOdeme"] != null) && (!String.IsNullOrEmpty(row["KalanOdeme"].ToString()))) sb.Append(",KalanOdeme=@KalanOdeme");
                        else sb.Append(",KalanOdeme=NULL");
                        if ((row["OdemeSekliId"] != null) && (!String.IsNullOrEmpty(row["OdemeSekliId"].ToString()))) sb.Append(",OdemeSekliId=@OdemeSekliId");
                        else sb.Append(",OdemeSekliId=NULL");
                        if ((row["SonYuklemeTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["SonYuklemeTarihi"].ToString()))) sb.Append(",SonYuklemeTarihi=@SonYuklemeTarihi");
                        else sb.Append(",SonYuklemeTarihi=NULL");
                        if ((row["BankaId"] != null) && (!String.IsNullOrEmpty(row["BankaId"].ToString()))) sb.Append(",BankaId=@BankaId");
                        else sb.Append(",BankaId=NULL");
                        if ((row["TasiyiciFirmaId"] != null) && (!String.IsNullOrEmpty(row["TasiyiciFirmaId"].ToString()))) sb.Append(",TasiyiciFirmaId=@TasiyiciFirmaId");
                        else sb.Append(",TasiyiciFirmaId=NULL");
                        if ((row["OdemeVadesi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["OdemeVadesi"].ToString()))) sb.Append(",OdemeVadesi=@OdemeVadesi");
                        else sb.Append(",OdemeVadesi=NULL");
                        if ((row["TahminiVarisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["TahminiVarisTarihi"].ToString()))) sb.Append(",TahminiVarisTarihi=@TahminiVarisTarihi");
                        else sb.Append(",TahminiVarisTarihi=NULL");
                        if ((row["GumrukVarisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["GumrukVarisTarihi"].ToString()))) sb.Append(",GumrukVarisTarihi=@GumrukVarisTarihi");
                        else sb.Append(",GumrukVarisTarihi=NULL");
                        if ((row["TahminiDepoGirisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["TahminiDepoGirisTarihi"].ToString()))) sb.Append(",TahminiDepoGirisTarihi=@TahminiDepoGirisTarihi");
                        else sb.Append(",TahminiDepoGirisTarihi=NULL");
                        if ((row["DepoGirisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["DepoGirisTarihi"].ToString()))) sb.Append(",DepoGirisTarihi=@DepoGirisTarihi");
                        else sb.Append(",DepoGirisTarihi=NULL");
                        if ((row["DepoId"] != null) && (!String.IsNullOrEmpty(row["DepoId"].ToString()))) sb.Append(",DepoId=@DepoId");
                        else sb.Append(",DepoId=NULL");
                        if ((row["TahminiMagazaDagitimTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["TahminiMagazaDagitimTarihi"].ToString()))) sb.Append(",TahminiMagazaDagitimTarihi=@TahminiMagazaDagitimTarihi");
                        else sb.Append(",TahminiMagazaDagitimTarihi=NULL");
                        if ((row["MagazaDagitimTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["MagazaDagitimTarihi"].ToString()))) sb.Append(",MagazaDagitimTarihi=@MagazaDagitimTarihi");
                        else sb.Append(",MagazaDagitimTarihi=NULL");
                        if ((row["Agirlik"] != null) && (!String.IsNullOrEmpty(row["Agirlik"].ToString()))) sb.Append(",Agirlik=@Agirlik");
                        else sb.Append(",Agirlik=NULL");
                        if ((row["PaketAdet"] != null) && (!String.IsNullOrEmpty(row["PaketAdet"].ToString()))) sb.Append(",PaketAdet=@PaketAdet");
                        else sb.Append(",PaketAdet=NULL");
                        if ((row["EksikUrun"] != null) && (!String.IsNullOrEmpty(row["EksikUrun"].ToString()))) sb.Append(",EksikUrun=@EksikUrun");
                        else sb.Append(",EksikUrun=NULL");
                        if ((row["FazlaUrun"] != null) && (!String.IsNullOrEmpty(row["FazlaUrun"].ToString()))) sb.Append(",FazlaUrun=@FazlaUrun");
                        else sb.Append(",FazlaUrun=NULL");
                        if ((row["IsEkMaliyet"] != null) && (!String.IsNullOrEmpty(row["IsEkMaliyet"].ToString()))) sb.Append(",IsEkMaliyet=@IsEkMaliyet");
                        else sb.Append(",IsEkMaliyet=0");
                        sb.Append(" WHERE Id=@Id");
                        try
                        {
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@SiparisId", id);
                            if ((row["YuklemeNo"] != null) && (!String.IsNullOrEmpty(row["YuklemeNo"].ToString())))
                                DB.AddParam(cmd, "@YuklemeNo", 60, row["YuklemeNo"].ToString().Replace(",", "."));
                            if ((row["UrunId"] != null) && (!String.IsNullOrEmpty(row["UrunId"].ToString())))
                                DB.AddParam(cmd, "@UrunId", (new Guid(row["UrunId"].ToString())));
                            if ((row["SezonId"] != null) && (!String.IsNullOrEmpty(row["SezonId"].ToString())))
                                DB.AddParam(cmd, "@SezonId", (new Guid(row["SezonId"].ToString())));
                            if ((row["Adet"] != null) && (!String.IsNullOrEmpty(row["Adet"].ToString())))
                                DB.AddParam(cmd, "@Adet", 60, row["Adet"].ToString().Replace(",", "."));
                            if ((row["Tutar"] != null) && (!String.IsNullOrEmpty(row["Tutar"].ToString())))
                                DB.AddParam(cmd, "@Tutar", 60, row["Tutar"].ToString().Replace(",", "."));
                            if ((row["ProformaTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["ProformaTarihi"].ToString())))
                                DB.AddParam(cmd, "@ProformaTarihi", (DateTime)row["ProformaTarihi"]);
                            if ((row["AsamaId"] != null) && (!String.IsNullOrEmpty(row["AsamaId"].ToString())))
                                DB.AddParam(cmd, "@AsamaId", (new Guid(row["AsamaId"].ToString())));
                            if ((row["Iskonto"] != null) && (!String.IsNullOrEmpty(row["Iskonto"].ToString())))
                                DB.AddParam(cmd, "@Iskonto", 60, row["Iskonto"].ToString().Replace(",", "."));
                            if ((row["SevkSekliId"] != null) && (!String.IsNullOrEmpty(row["SevkSekliId"].ToString())))
                                DB.AddParam(cmd, "@SevkSekliId", (new Guid(row["SevkSekliId"].ToString())));
                            if ((row["GelenTutar"] != null) && (!String.IsNullOrEmpty(row["GelenTutar"].ToString())))
                                DB.AddParam(cmd, "@GelenTutar", 60, row["GelenTutar"].ToString().Replace(",", "."));
                            if ((row["OdemeIskonto"] != null) && (!String.IsNullOrEmpty(row["OdemeIskonto"].ToString())))
                                DB.AddParam(cmd, "@OdemeIskonto", 60, row["OdemeIskonto"].ToString().Replace(",", "."));
                            if ((row["YapilanOdeme"] != null) && (!String.IsNullOrEmpty(row["YapilanOdeme"].ToString())))
                                DB.AddParam(cmd, "@YapilanOdeme", 60, row["YapilanOdeme"].ToString().Replace(",", "."));
                            if ((row["KalanOdeme"] != null) && (!String.IsNullOrEmpty(row["KalanOdeme"].ToString())))
                                DB.AddParam(cmd, "@KalanOdeme", 60, row["KalanOdeme"].ToString().Replace(",", "."));
                            if ((row["OdemeSekliId"] != null) && (!String.IsNullOrEmpty(row["OdemeSekliId"].ToString())))
                                DB.AddParam(cmd, "@OdemeSekliId", (new Guid(row["OdemeSekliId"].ToString())));
                            if ((row["SonYuklemeTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["SonYuklemeTarihi"].ToString())))
                                DB.AddParam(cmd, "@SonYuklemeTarihi", (DateTime)row["SonYuklemeTarihi"]);
                            if ((row["BankaId"] != null) && (!String.IsNullOrEmpty(row["BankaId"].ToString())))
                                DB.AddParam(cmd, "@BankaId", (new Guid(row["BankaId"].ToString())));
                            if ((row["TasiyiciFirmaId"] != null) && (!String.IsNullOrEmpty(row["TasiyiciFirmaId"].ToString())))
                                DB.AddParam(cmd, "@TasiyiciFirmaId", (new Guid(row["TasiyiciFirmaId"].ToString())));
                            if ((row["OdemeVadesi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["OdemeVadesi"].ToString())))
                                DB.AddParam(cmd, "@OdemeVadesi", (DateTime)row["OdemeVadesi"]);
                            if ((row["TahminiVarisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["TahminiVarisTarihi"].ToString())))
                                DB.AddParam(cmd, "@TahminiVarisTarihi", (DateTime)row["TahminiVarisTarihi"]);
                            if ((row["GumrukVarisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["GumrukVarisTarihi"].ToString())))
                                DB.AddParam(cmd, "@GumrukVarisTarihi", (DateTime)row["GumrukVarisTarihi"]);
                            if ((row["TahminiDepoGirisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["TahminiDepoGirisTarihi"].ToString())))
                                DB.AddParam(cmd, "@TahminiDepoGirisTarihi", (DateTime)row["TahminiDepoGirisTarihi"]);
                            if ((row["DepoGirisTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["DepoGirisTarihi"].ToString())))
                                DB.AddParam(cmd, "@DepoGirisTarihi", (DateTime)row["DepoGirisTarihi"]);
                            if ((row["DepoId"] != null) && (!String.IsNullOrEmpty(row["DepoId"].ToString())))
                                DB.AddParam(cmd, "@DepoId", (new Guid(row["DepoId"].ToString())));
                            if ((row["TahminiMagazaDagitimTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["TahminiMagazaDagitimTarihi"].ToString())))
                                DB.AddParam(cmd, "@TahminiMagazaDagitimTarihi", (DateTime)row["TahminiMagazaDagitimTarihi"]);
                            if ((row["MagazaDagitimTarihi"] != null) && (!CrmUtils.IsNullOrEmptyDateTime(row["MagazaDagitimTarihi"].ToString())))
                                DB.AddParam(cmd, "@MagazaDagitimTarihi", (DateTime)row["MagazaDagitimTarihi"]);
                            if ((row["Agirlik"] != null) && (!String.IsNullOrEmpty(row["Agirlik"].ToString())))
                                DB.AddParam(cmd, "@Agirlik", 60, row["Agirlik"].ToString().Replace(",", "."));
                            if ((row["PaketAdet"] != null) && (!String.IsNullOrEmpty(row["PaketAdet"].ToString())))
                                DB.AddParam(cmd, "@PaketAdet", 60, row["PaketAdet"].ToString().Replace(",", "."));
                            if ((row["EksikUrun"] != null) && (!String.IsNullOrEmpty(row["EksikUrun"].ToString())))
                                DB.AddParam(cmd, "@EksikUrun", 60, row["EksikUrun"].ToString().Replace(",", "."));
                            if ((row["FazlaUrun"] != null) && (!String.IsNullOrEmpty(row["FazlaUrun"].ToString())))
                                DB.AddParam(cmd, "@FazlaUrun", 60, row["FazlaUrun"].ToString().Replace(",", "."));
                            if ((row["IsEkMaliyet"] != null) && (!String.IsNullOrEmpty(row["IsEkMaliyet"].ToString())))
                                DB.AddParam(cmd, "@IsEkMaliyet", Convert.ToBoolean(row["IsEkMaliyet"]) ? 1 : 0);
                            DB.AddParam(cmd, "@LCNo", 255, row["LCNo"].ToString());
                            DB.AddParam(cmd, "@Aciklama", 255, row["Aciklama"].ToString());
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
                            cmd = DB.SQL(this.Context, "DELETE FROM IthalatSiparisYukleme WHERE Id=@Id");
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

        #region yüklemeler tarihçe
        sb1 = new StringBuilder("INSERT INTO IthalatSiparisYuklemeTarihce(Id,SiparisSayac,SiparisId,LCNo,Aciklama,ModifiedBy,ModificationDate");
        sb1.Append(",YuklemeNo,UrunId,SezonId,Adet,Tutar,ProformaTarihi,AsamaId,Iskonto,SevkSekliId,GelenTutar");
        sb1.Append(",OdemeIskonto,YapilanOdeme,KalanOdeme,OdemeSekliId,SonYuklemeTarihi,BankaId,TasiyiciFirmaId");
        sb1.Append(",OdemeVadesi,TahminiVarisTarihi,GumrukVarisTarihi,TahminiDepoGirisTarihi");
        sb1.Append(",DepoGirisTarihi,DepoId,TahminiMagazaDagitimTarihi,MagazaDagitimTarihi,Agirlik,PaketAdet) ");
        sb2 = new StringBuilder("SELECT Id,@SiparisSayac,SiparisId,LCNo,Aciklama,@ModifiedBy,@ModificationDate");
        sb2.Append(",YuklemeNo,UrunId,SezonId,Adet,Tutar,ProformaTarihi,AsamaId,Iskonto,SevkSekliId,GelenTutar");
        sb2.Append(",OdemeIskonto,YapilanOdeme,KalanOdeme,OdemeSekliId,SonYuklemeTarihi,BankaId,TasiyiciFirmaId");
        sb2.Append(",OdemeVadesi,TahminiVarisTarihi,GumrukVarisTarihi,TahminiDepoGirisTarihi");
        sb2.Append(",DepoGirisTarihi,DepoId,TahminiMagazaDagitimTarihi,MagazaDagitimTarihi,Agirlik,PaketAdet ");
        sb2.Append(" FROM IthalatSiparisYukleme WHERE SiparisId=@SiparisId");
        try
        {
            cmd = DB.SQL(this.Context, (sb1.ToString() + sb2.ToString()));
            DB.AddParam(cmd, "@SiparisSayac", _SiparisTarihceSayac);
            DB.AddParam(cmd, "@SiparisId", id);
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
                cmd = DB.SQL(this.Context, "DELETE FROM IthalatSiparisYukleme WHERE SiparisId=@Id");
                DB.AddParam(cmd, "@Id", id);
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd = DB.SQL(this.Context, "DELETE FROM IthalatSiparisUrun WHERE SiparisId=@Id");
                DB.AddParam(cmd, "@Id", id);
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd = DB.SQL(this.Context, "DELETE FROM IthalatSiparis WHERE Id=@Id");
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

    protected void SezonId_Callback(object source, CallbackEventArgsBase e)
    {
        this.SezonId.Items.Clear();
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            Guid id = new Guid(e.Parameter);
            FillSezonId(this.SezonId, id);
            this.SezonId.SelectedIndex = 0;
        }
    }

    private void FillSezonId(ASPxComboBox source, Guid id)
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT Id,Sezon FROM IthalatSezon WHERE Id=@Id");
        DB.AddParam(cmd, "@Id", id);
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        if (rdr.Read())
        {
            ListEditItem item = new ListEditItem();
            item.Value = rdr["Id"];
            item.Text = rdr["Sezon"].ToString();
            source.Items.Add(item);
            Session["SezonId"] = rdr["Id"];
            Session["Sezon"] = rdr["Sezon"].ToString();
        }
        rdr.Close();
    }

    protected void AsamaId_Callback(object source, CallbackEventArgsBase e)
    {
        this.AsamaId.Items.Clear();
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            Guid id = new Guid(e.Parameter);
            FillAsamaId(this.AsamaId, id);
            this.AsamaId.SelectedIndex = 0;
        }
    }

    private void FillAsamaId(ASPxComboBox source, Guid id)
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT Id,Asama FROM IthalatAsama WHERE Id=@Id");
        DB.AddParam(cmd, "@Id", id);
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        if (rdr.Read())
        {
            ListEditItem item = new ListEditItem();
            item.Value = rdr["Id"];
            item.Text = rdr["Asama"].ToString();
            source.Items.Add(item);
            Session["AsamaId"] = rdr["Id"];
            Session["Asama"] = rdr["Asama"].ToString();
        }
        rdr.Close();
    }

    protected void MarkaId_Callback(object source, CallbackEventArgsBase e)
    {
        this.MarkaId.Items.Clear();
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            Guid id = new Guid(e.Parameter);
            FillMarkaId(this.MarkaId, id);
            this.MarkaId.SelectedIndex = 0;
        }
    }

    private void FillMarkaId(ASPxComboBox source, Guid id)
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT Id,Marka FROM IthalatFirmaMarka WHERE Id=@Id");
        DB.AddParam(cmd, "@Id", id);
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        if (rdr.Read())
        {
            ListEditItem item = new ListEditItem();
            item.Value = rdr["Id"];
            item.Text = rdr["Marka"].ToString();
            source.Items.Add(item);
            Session["MarkaId"] = rdr["Id"];
            Session["Marka"] = rdr["Marka"].ToString();
        }
        rdr.Close();
    }

    protected void FirmaId_Callback(object source, CallbackEventArgsBase e)
    {
        this.FirmaId.Items.Clear();
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            Guid id = new Guid(e.Parameter);
            FillFirmaId(this.FirmaId, id);
            this.FirmaId.SelectedIndex = 0;
        }
    }

    private void FillFirmaId(ASPxComboBox source, Guid id)
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT Id,Adi FROM IthalatFirma WHERE Id=@Id");
        DB.AddParam(cmd, "@Id", id);
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        if (rdr.Read())
        {
            ListEditItem item = new ListEditItem();
            item.Value = rdr["Id"];
            item.Text = rdr["Adi"].ToString();
            source.Items.Add(item);
            Session["FirmaId"] = rdr["Id"];
            Session["Firma"] = rdr["Adi"].ToString();
        }
        rdr.Close();
    }

    protected void TasiyiciFirmaId_Callback(object source, CallbackEventArgsBase e)
    {
        this.TasiyiciFirmaId.Items.Clear();
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            Guid id = new Guid(e.Parameter);
            FillTasiyiciFirmaId(this.TasiyiciFirmaId, id);
            this.TasiyiciFirmaId.SelectedIndex = 0;
        }
    }

    private void FillTasiyiciFirmaId(ASPxComboBox source, Guid id)
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT Id,Adi FROM IthalatTasiyiciFirma WHERE Id=@Id");
        DB.AddParam(cmd, "@Id", id);
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        if (rdr.Read())
        {
            ListEditItem item = new ListEditItem();
            item.Value = rdr["Id"];
            item.Text = rdr["Adi"].ToString();
            source.Items.Add(item);
            Session["TasiyiciFirmaId"] = rdr["Id"];
            Session["TasiyiciFirma"] = rdr["Adi"].ToString();
        }
        rdr.Close();
    }

    protected void ParaBirimiId_Callback(object source, CallbackEventArgsBase e)
    {
        this.ParaBirimiId.Items.Clear();
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            Guid id = new Guid(e.Parameter);
            FillParaBirimiId(this.ParaBirimiId, id);
            this.ParaBirimiId.SelectedIndex = 0;
        }
    }

    private void FillParaBirimiId(ASPxComboBox source, Guid id)
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT Id,ParaBirimi FROM ParaBirimi WHERE Id=@Id");
        DB.AddParam(cmd, "@Id", id);
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        if (rdr.Read())
        {
            ListEditItem item = new ListEditItem();
            item.Value = rdr["Id"];
            item.Text = rdr["ParaBirimi"].ToString();
            source.Items.Add(item);
            Session["ParaBirimiId"] = rdr["Id"];
            Session["ParaBirimi"] = rdr["ParaBirimi"].ToString();
        }
        rdr.Close();
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
            Session["SevkSekliId"] = rdr["Id"];
            Session["SevkSekli"] = rdr["SevkSekli"].ToString();
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
            Session["OdemeSekliId"] = rdr["Id"];
            Session["OdemeSekli"] = rdr["OdemeSekli"].ToString();
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
                #region sezon
                case "SezonId":
                case "GridSezonId":
                    dtQuery.Rows.Add("SELECT Id,Sezon FROM IthalatSezon ORDER BY Sezon");
                    dtFields.Rows.Add("Id", "Id", 50, false);
                    dtFields.Rows.Add("Sezon", "Sezon", 100, true);
                    htSearchBrowser.Add("Title", "SEZONLAR");
                    htSearchBrowser.Add("Query", dtQuery);
                    htSearchBrowser.Add("Parameters", dtParameters);
                    htSearchBrowser.Add("Fields", dtFields);
                    htSearchBrowser.Add("KeyField", "Id");
                    break;
                #endregion
                #region aþama
                case "AsamaId":
                case "GridAsamaId":
                    dtQuery.Rows.Add("SELECT Id,Asama FROM IthalatAsama ORDER BY Asama");
                    dtFields.Rows.Add("Id", "Id", 50, false);
                    dtFields.Rows.Add("Asama", "Aþama", 100, true);
                    htSearchBrowser.Add("Title", "AÞAMALAR");
                    htSearchBrowser.Add("Query", dtQuery);
                    htSearchBrowser.Add("Parameters", dtParameters);
                    htSearchBrowser.Add("Fields", dtFields);
                    htSearchBrowser.Add("KeyField", "Id");
                    break;
                #endregion
                #region marka
                case "MarkaId":
                    dtQuery.Rows.Add("SELECT Id,Marka FROM IthalatFirmaMarka WHERE FirmaId=@FirmaId ORDER BY Marka");
                    dtParameters.Rows.Add("@FirmaId", "Guid", (new Guid(parameters[1].Trim())), 0, 0, 0);
                    dtFields.Rows.Add("Id", "Id", 50, false);
                    dtFields.Rows.Add("Marka", "Marka", 100, true);
                    htSearchBrowser.Add("Title", "MARKALAR");
                    htSearchBrowser.Add("Query", dtQuery);
                    htSearchBrowser.Add("Parameters", dtParameters);
                    htSearchBrowser.Add("Fields", dtFields);
                    htSearchBrowser.Add("KeyField", "Id");
                    break;
                #endregion
                #region firma
                case "FirmaId":
                    dtQuery.Rows.Add("SELECT Id,Adi FROM IthalatFirma ORDER BY Adi");
                    dtFields.Rows.Add("Id", "Id", 50, false);
                    dtFields.Rows.Add("Adi", "Adý", 100, true);
                    htSearchBrowser.Add("Title", "FÝRMALAR");
                    htSearchBrowser.Add("Query", dtQuery);
                    htSearchBrowser.Add("Parameters", dtParameters);
                    htSearchBrowser.Add("Fields", dtFields);
                    htSearchBrowser.Add("KeyField", "Id");
                    break;
                #endregion
                #region taþýyýcý firma
                case "TasiyiciFirmaId":
                case "GridTasiyiciFirmaId":
                    dtQuery.Rows.Add("SELECT Id,Adi FROM IthalatTasiyiciFirma ORDER BY Adi");
                    dtFields.Rows.Add("Id", "Id", 50, false);
                    dtFields.Rows.Add("Adi", "Adý", 100, true);
                    htSearchBrowser.Add("Title", "TAÞIYICI FÝRMALAR");
                    htSearchBrowser.Add("Query", dtQuery);
                    htSearchBrowser.Add("Parameters", dtParameters);
                    htSearchBrowser.Add("Fields", dtFields);
                    htSearchBrowser.Add("KeyField", "Id");
                    break;
                #endregion
                #region para birimi
                case "ParaBirimiId":
                    dtQuery.Rows.Add("SELECT Id,ParaBirimi FROM ParaBirimi ORDER BY ParaBirimi");
                    dtFields.Rows.Add("Id", "Id", 50, false);
                    dtFields.Rows.Add("ParaBirimi", "Para Birimi", 100, true);
                    htSearchBrowser.Add("Title", "PARA BÝRÝMLERÝ");
                    htSearchBrowser.Add("Query", dtQuery);
                    htSearchBrowser.Add("Parameters", dtParameters);
                    htSearchBrowser.Add("Fields", dtFields);
                    htSearchBrowser.Add("KeyField", "Id");
                    break;
                #endregion
                #region sevk þekli
                case "SevkSekliId":
                case "GridSevkSekliId":
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
                case "GridOdemeSekliId":
                    dtQuery.Rows.Add("SELECT Id,OdemeSekli,Gun FROM IthalatOdemeSekli ORDER BY OdemeSekli");
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
                #region ürünler
                case "GridUrunId":
                case "GridUrunId2":
                    dtQuery.Rows.Add("SELECT Id,Urun FROM IthalatFirmaMarkaUrun WHERE MarkaId=@MarkaId ORDER BY Urun");
                    dtParameters.Rows.Add("@MarkaId", "Guid", (new Guid(parameters[1].Trim())), 0, 0, 0);
                    dtFields.Rows.Add("Id", "Id", 50, false);
                    dtFields.Rows.Add("Urun", "Ürün", 100, true);
                    htSearchBrowser.Add("Title", "ÜRÜNLER");
                    htSearchBrowser.Add("Query", dtQuery);
                    htSearchBrowser.Add("Parameters", dtParameters);
                    htSearchBrowser.Add("Fields", dtFields);
                    htSearchBrowser.Add("KeyField", "Id");
                    break;
                #endregion
                #region banka
                case "GridBankaId":
                    dtQuery.Rows.Add("SELECT Id,Banka FROM Banka ORDER BY Banka");
                    dtFields.Rows.Add("Id", "Id", 50, false);
                    dtFields.Rows.Add("Banka", "Banka", 100, true);
                    htSearchBrowser.Add("Title", "BANKALAR");
                    htSearchBrowser.Add("Query", dtQuery);
                    htSearchBrowser.Add("Parameters", dtParameters);
                    htSearchBrowser.Add("Fields", dtFields);
                    htSearchBrowser.Add("KeyField", "Id");
                    break;
                #endregion
                #region depo
                case "GridDepoId":
                    dtQuery.Rows.Add("SELECT Id,Depo FROM Depo ORDER BY Depo");
                    dtFields.Rows.Add("Id", "Id", 50, false);
                    dtFields.Rows.Add("Depo", "Depo", 100, true);
                    htSearchBrowser.Add("Title", "DEPOLAR");
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
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            SqlCommand cmd = null;
            IDataReader rdr = null;
            StringBuilder sb = new StringBuilder();
            string[] parameters = e.Parameter.Split('|');
            switch (parameters[0].Trim())
            {
                #region firma
                case "FirmaId":
                    cmd = DB.SQL(this.Context, "SELECT Iskonto,SevkSekliId,OdemeSekliId FROM IthalatFirma WHERE Id=@Id");
                    DB.AddParam(cmd, "@Id", (new Guid(parameters[1].Trim())));
                    cmd.CommandTimeout = 1000;
                    cmd.Prepare();
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        sb = new StringBuilder("|" + parameters[0].Trim());
                        sb.Append("|");
                        if ((rdr["Iskonto"] != null) && (!String.IsNullOrEmpty(rdr["Iskonto"].ToString()))) sb.Append(Convert.ToDouble(rdr["Iskonto"]).ToString().Replace(".", ","));
                        sb.Append("|");
                        if ((rdr["SevkSekliId"] != null) && (!String.IsNullOrEmpty(rdr["SevkSekliId"].ToString()))) sb.Append(rdr["SevkSekliId"]).ToString().Replace(".", ",");
                        sb.Append("|");
                        if ((rdr["OdemeSekliId"] != null) && (!String.IsNullOrEmpty(rdr["OdemeSekliId"].ToString()))) sb.Append(rdr["OdemeSekliId"]).ToString().Replace(".", ",");
                        e.Result = sb.ToString();
                    }
                    rdr.Close();
                    break;
                #endregion
                #region ürün
                case "GridUrunId":
                case "GridUrunId2":
                    cmd = DB.SQL(this.Context, "SELECT Id,Urun FROM IthalatFirmaMarkaUrun WHERE Id=@Id");
                    DB.AddParam(cmd, "@Id", (new Guid(parameters[1].Trim())));
                    cmd.CommandTimeout = 1000;
                    cmd.Prepare();
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        sb = new StringBuilder("|" + parameters[0].Trim());
                        sb.Append("|" + rdr["Id"].ToString());
                        sb.Append("|" + rdr["Urun"].ToString());
                        e.Result = sb.ToString();
                    }
                    rdr.Close();
                    break;
                #endregion
                #region sezon
                case "GridSezonId":
                    cmd = DB.SQL(this.Context, "SELECT Id,Sezon FROM IthalatSezon WHERE Id=@Id");
                    DB.AddParam(cmd, "@Id", (new Guid(parameters[1].Trim())));
                    cmd.CommandTimeout = 1000;
                    cmd.Prepare();
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        sb = new StringBuilder("|" + parameters[0].Trim());
                        sb.Append("|" + rdr["Id"].ToString());
                        sb.Append("|" + rdr["Sezon"].ToString());
                        e.Result = sb.ToString();
                    }
                    rdr.Close();
                    break;
                #endregion
                #region asama
                case "GridAsamaId":
                    cmd = DB.SQL(this.Context, "SELECT Id,Asama FROM IthalatAsama WHERE Id=@Id");
                    DB.AddParam(cmd, "@Id", (new Guid(parameters[1].Trim())));
                    cmd.CommandTimeout = 1000;
                    cmd.Prepare();
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        sb = new StringBuilder("|" + parameters[0].Trim());
                        sb.Append("|" + rdr["Id"].ToString());
                        sb.Append("|" + rdr["Asama"].ToString());
                        e.Result = sb.ToString();
                    }
                    rdr.Close();
                    break;
                #endregion
                #region sevk sekli
                case "GridSevkSekliId":
                    cmd = DB.SQL(this.Context, "SELECT Id,SevkSekli FROM IthalatSevkSekli WHERE Id=@Id");
                    DB.AddParam(cmd, "@Id", (new Guid(parameters[1].Trim())));
                    cmd.CommandTimeout = 1000;
                    cmd.Prepare();
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        sb = new StringBuilder("|" + parameters[0].Trim());
                        sb.Append("|" + rdr["Id"].ToString());
                        sb.Append("|" + rdr["SevkSekli"].ToString());
                        e.Result = sb.ToString();
                    }
                    rdr.Close();
                    break;
                #endregion
                #region ödeme sekli
                case "GridOdemeSekliId":
                    cmd = DB.SQL(this.Context, "SELECT Id,OdemeSekli FROM IthalatOdemeSekli WHERE Id=@Id");
                    DB.AddParam(cmd, "@Id", (new Guid(parameters[1].Trim())));
                    cmd.CommandTimeout = 1000;
                    cmd.Prepare();
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        sb = new StringBuilder("|" + parameters[0].Trim());
                        sb.Append("|" + rdr["Id"].ToString());
                        sb.Append("|" + rdr["OdemeSekli"].ToString());
                        e.Result = sb.ToString();
                    }
                    rdr.Close();
                    break;
                #endregion
                #region banka
                case "GridBankaId":
                    cmd = DB.SQL(this.Context, "SELECT Id,Banka FROM Banka WHERE Id=@Id");
                    DB.AddParam(cmd, "@Id", (new Guid(parameters[1].Trim())));
                    cmd.CommandTimeout = 1000;
                    cmd.Prepare();
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        sb = new StringBuilder("|" + parameters[0].Trim());
                        sb.Append("|" + rdr["Id"].ToString());
                        sb.Append("|" + rdr["Banka"].ToString());
                        e.Result = sb.ToString();
                    }
                    rdr.Close();
                    break;
                #endregion
                #region taþýyýcý firma
                case "GridTasiyiciFirmaId":
                    cmd = DB.SQL(this.Context, "SELECT Id,Adi FROM IthalatTasiyiciFirma WHERE Id=@Id");
                    DB.AddParam(cmd, "@Id", (new Guid(parameters[1].Trim())));
                    cmd.CommandTimeout = 1000;
                    cmd.Prepare();
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        sb = new StringBuilder("|" + parameters[0].Trim());
                        sb.Append("|" + rdr["Id"].ToString());
                        sb.Append("|" + rdr["Adi"].ToString());
                        e.Result = sb.ToString();
                    }
                    rdr.Close();
                    break;
                #endregion
                #region banka
                case "GridDepoId":
                    cmd = DB.SQL(this.Context, "SELECT Id,Depo FROM Depo WHERE Id=@Id");
                    DB.AddParam(cmd, "@Id", (new Guid(parameters[1].Trim())));
                    cmd.CommandTimeout = 1000;
                    cmd.Prepare();
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        sb = new StringBuilder("|" + parameters[0].Trim());
                        sb.Append("|" + rdr["Id"].ToString());
                        sb.Append("|" + rdr["Depo"].ToString());
                        e.Result = sb.ToString();
                    }
                    rdr.Close();
                    break;
                #endregion
                default:
                    break;
            }
        }
    }

    private void InitDTUrun(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("SiparisId", typeof(Guid));
        dt.Columns.Add("UrunId", typeof(Guid));
        dt.Columns.Add("Urun", typeof(string));
        dt.Columns.Add("Adet", typeof(int));
        dt.Columns.Add("Tutar", typeof(decimal));
        dt.Columns.Add("Aciklama", typeof(string));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitDTYukleme(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("SiparisId", typeof(Guid));
        dt.Columns.Add("YuklemeNo", typeof(int));
        dt.Columns.Add("UrunId", typeof(Guid));
        dt.Columns.Add("Urun", typeof(string));
        dt.Columns.Add("SezonId", typeof(Guid));
        dt.Columns.Add("Sezon", typeof(string));
        dt.Columns.Add("Adet", typeof(int));
        dt.Columns.Add("Tutar", typeof(decimal));
        dt.Columns.Add("ProformaTarihi", typeof(DateTime));
        dt.Columns.Add("AsamaId", typeof(Guid));
        dt.Columns.Add("Asama", typeof(string));
        dt.Columns.Add("Iskonto", typeof(decimal));
        dt.Columns.Add("SevkSekliId", typeof(Guid));
        dt.Columns.Add("SevkSekli", typeof(string));
        dt.Columns.Add("GelenTutar", typeof(decimal));
        dt.Columns.Add("OdemeIskonto", typeof(decimal));
        dt.Columns.Add("YapilanOdeme", typeof(decimal));
        dt.Columns.Add("KalanOdeme", typeof(decimal));
        dt.Columns.Add("OdemeSekliId", typeof(Guid));
        dt.Columns.Add("OdemeSekli", typeof(string));
        dt.Columns.Add("SonYuklemeTarihi", typeof(DateTime));
        dt.Columns.Add("LCNo", typeof(string));
        dt.Columns.Add("BankaId", typeof(Guid));
        dt.Columns.Add("Banka", typeof(string));
        dt.Columns.Add("TasiyiciFirmaId", typeof(Guid));
        dt.Columns.Add("TasiyiciFirma", typeof(string));
        dt.Columns.Add("OdemeVadesi", typeof(DateTime));
        dt.Columns.Add("TahminiVarisTarihi", typeof(DateTime));
        dt.Columns.Add("GumrukVarisTarihi", typeof(DateTime));
        dt.Columns.Add("TahminiDepoGirisTarihi", typeof(DateTime));
        dt.Columns.Add("DepoGirisTarihi", typeof(DateTime));
        dt.Columns.Add("DepoId", typeof(Guid));
        dt.Columns.Add("Depo", typeof(string));
        dt.Columns.Add("TahminiMagazaDagitimTarihi", typeof(DateTime));
        dt.Columns.Add("MagazaDagitimTarihi", typeof(DateTime));
        dt.Columns.Add("Agirlik", typeof(decimal));
        dt.Columns.Add("PaketAdet", typeof(int));
        dt.Columns.Add("Aciklama", typeof(string));
        dt.Columns.Add("EksikUrun", typeof(decimal));
        dt.Columns.Add("FazlaUrun", typeof(decimal));
        dt.Columns.Add("IsEkMaliyet", typeof(bool));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    protected void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        ASPxGridView grid = (ASPxGridView)sender;
        DataTable changes = null;
        if (grid.ID == "GridUrun") changes = this.DTUrun.Table.GetChanges();
        if (grid.ID == "GridYukleme") changes = this.DTYukleme.Table.GetChanges();
        if (changes != null) e.Properties["cpIsDirty"] = true;
        else e.Properties["cpIsDirty"] = false;
    }

    protected void GridUrun_InitNewRow(object sender, ASPxDataInitNewRowEventArgs e)
    {
        //Session["GridMessage"] = "";
        //if (this.siparis_asama.SelectedIndex > 0)
        //{
        //    this.GridUrun.CancelEdit();
        //    Session["GridMessage"] = "Aþamasý 'AÇIK' olmayan sipariþlerde deðiþiklik yapamazsýnýz!";
        //    return;
        //}
    }

    protected void GridUrun_StartRowEditing(object sender, ASPxStartRowEditingEventArgs e)
    {
        //Session["GridMessage"] = "";
        //if (!this.GridUrun.IsNewRowEditing)
        //{
        //    Guid id = (Guid)e.EditingKeyValue;
        //    DataRow row = this.DTUrun.Table.Rows.Find(id);
        //    if (row != null)
        //    {
        //        if (row["asama"].ToString().Trim() != "1")
        //        {
        //            e.Cancel = true;
        //            this.GridUrun.CancelEdit();
        //            Session["GridMessage"] = "Aþamasý 'AÇIK' olmayan satýrlarda deðiþiklik yapamazsýnýz!";
        //            return;
        //        }
        //    }
        //}
    }

    protected void GridUrun_CancelRowEditing(object sender, ASPxStartRowEditingEventArgs e)
    {
        //string s = "";
    }

    protected void GridUrun_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (!this.GridUrun.IsEditing) return;

        switch (e.Column.FieldName)
        {
            case "Adet":
                ASPxTextEdit _Adet = e.Editor as ASPxTextEdit;
                if ((_Adet.Value == null) || (_Adet.Value.ToString() == "")) _Adet.Value = "0";
                break;
            case "Tutar":
                ASPxTextEdit _Tutar = e.Editor as ASPxTextEdit;
                if ((_Tutar.Value == null) || (_Tutar.ToString() == "")) _Tutar.Value = "0";
                break;
        }
    }

    protected void GridUrun_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["UrunId"] == null) e.NewValues["UrunId"] = DBNull.Value;
            if (e.NewValues["Adet"] == null) e.NewValues["Adet"] = 0;
            if (e.NewValues["Tutar"] == null) e.NewValues["Tutar"] = 0;
        }
    }

    protected void GridUrun_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["UrunId"] == null) e.NewValues["UrunId"] = DBNull.Value;
            if (e.NewValues["Adet"] == null) e.NewValues["Adet"] = 0;
            if (e.NewValues["Tutar"] == null) e.NewValues["Tutar"] = 0;
        }
    }

    protected void GridUrun_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["UrunId"] == null)
        {
            e.RowError = "Lütfen Ürün alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["Adet"] == null)
        {
            e.RowError = "Lütfen Adet alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["Tutar"] == null)
        {
            e.RowError = "Lütfen Tutar alanýný boþ býrakmayýnýz...";
            return;
        }
    }

    protected void GridYukleme_InitNewRow(object sender, ASPxDataInitNewRowEventArgs e)
    {
        //Session["GridMessage"] = "";
        //if (this.siparis_asama.SelectedIndex > 0)
        //{
        //    this.GridUrun.CancelEdit();
        //    Session["GridMessage"] = "Aþamasý 'AÇIK' olmayan sipariþlerde deðiþiklik yapamazsýnýz!";
        //    return;
        //}
    }

    protected void GridYukleme_StartRowEditing(object sender, ASPxStartRowEditingEventArgs e)
    {
        //Session["GridMessage"] = "";
        //if (!this.GridUrun.IsNewRowEditing)
        //{
        //    Guid id = (Guid)e.EditingKeyValue;
        //    DataRow row = this.DTUrun.Table.Rows.Find(id);
        //    if (row != null)
        //    {
        //        if (row["asama"].ToString().Trim() != "1")
        //        {
        //            e.Cancel = true;
        //            this.GridUrun.CancelEdit();
        //            Session["GridMessage"] = "Aþamasý 'AÇIK' olmayan satýrlarda deðiþiklik yapamazsýnýz!";
        //            return;
        //        }
        //    }
        //}
    }

    protected void GridYukleme_CancelRowEditing(object sender, ASPxStartRowEditingEventArgs e)
    {
        //string s = "";
    }

    protected void GridYukleme_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (!this.GridYukleme.IsEditing) return;

        switch (e.Column.FieldName)
        {
            case "YuklemeNo":
                ASPxTextEdit _YuklemeNo = e.Editor as ASPxTextEdit;
                if ((_YuklemeNo.Value == null) || (_YuklemeNo.Value.ToString() == "")) _YuklemeNo.Value = "1";
                break;
            case "SezonId":
                ASPxTextEdit _SezonId = e.Editor as ASPxTextEdit;
                if ((_SezonId.Value == null) || (_SezonId.Value.ToString() == ""))
                {
                    if (this.SezonId.SelectedIndex >= 0) _SezonId.Value = this.SezonId.Value;
                    else if (Session["SezonId"] != null) _SezonId.Value = (Guid)Session["SezonId"];
                }
                break;
            case "Sezon":
                ASPxTextEdit _Sezon = e.Editor as ASPxTextEdit;
                if ((_Sezon.Value == null) || (_Sezon.Value.ToString() == ""))
                {
                    if (this.SezonId.SelectedIndex >= 0) _Sezon.Value = this.SezonId.Text;
                    else if (Session["Sezon"] != null) _Sezon.Value = (string)Session["Sezon"];
                }
                break;
            case "Adet":
                ASPxTextEdit _Adet = e.Editor as ASPxTextEdit;
                if ((_Adet.Value == null) || (_Adet.Value.ToString() == "")) _Adet.Value = "0";
                break;
            case "Tutar":
                ASPxTextEdit _Tutar = e.Editor as ASPxTextEdit;
                if ((_Tutar.Value == null) || (_Tutar.ToString() == "")) _Tutar.Value = "0";
                break;
            case "EksikUrun":
                ASPxTextEdit _EksikUrun = e.Editor as ASPxTextEdit;
                if ((_EksikUrun.Value == null) || (_EksikUrun.Value.ToString() == "")) _EksikUrun.Value = "0";
                break;
            case "FazlaUrun":
                ASPxTextEdit _FazlaUrun = e.Editor as ASPxTextEdit;
                if ((_FazlaUrun.Value == null) || (_FazlaUrun.ToString() == "")) _FazlaUrun.Value = "0";
                break;
            case "Iskonto":
                ASPxTextEdit _Iskonto = e.Editor as ASPxTextEdit;
                if ((_Iskonto.Value == null) || (_Iskonto.Value.ToString() == ""))
                    if ((this.Iskonto.Value != null) && (!String.IsNullOrEmpty(this.Iskonto.Value.ToString()))) _Iskonto.Value = this.Iskonto.Text;
                break;
            case "SevkSekliId":
                ASPxTextEdit _SevkSekliId = e.Editor as ASPxTextEdit;
                if ((_SevkSekliId.Value == null) || (_SevkSekliId.Value.ToString() == ""))
                {
                    if (this.SevkSekliId.SelectedIndex >= 0) _SevkSekliId.Value = this.SevkSekliId.Value;
                    else if (Session["SevkSekliId"] != null) _SevkSekliId.Value = (Guid)Session["SevkSekliId"];
                }
                break;
            case "SevkSekli":
                ASPxTextEdit _SevkSekli = e.Editor as ASPxTextEdit;
                if ((_SevkSekli.Value == null) || (_SevkSekli.Value.ToString() == ""))
                {
                    if (this.SevkSekliId.SelectedIndex >= 0) _SevkSekli.Value = this.SevkSekliId.Text;
                    else if (Session["SevkSekli"] != null) _SevkSekli.Value = (string)Session["SevkSekli"];
                }
                break;
            case "OdemeSekliId":
                ASPxTextEdit _OdemeSekliId = e.Editor as ASPxTextEdit;
                if ((_OdemeSekliId.Value == null) || (_OdemeSekliId.Value.ToString() == ""))
                {
                    if (this.OdemeSekliId.SelectedIndex >= 0) _OdemeSekliId.Value = this.OdemeSekliId.Value;
                    else if (Session["OdemeSekliId"] != null) _OdemeSekliId.Value = (Guid)Session["OdemeSekliId"];
                }
                break;
            case "OdemeSekli":
                ASPxTextEdit _OdemeSekli = e.Editor as ASPxTextEdit;
                if ((_OdemeSekli.Value == null) || (_OdemeSekli.Value.ToString() == ""))
                {
                    if (this.OdemeSekliId.SelectedIndex >= 0) _OdemeSekli.Value = this.OdemeSekliId.Text;
                    else if (Session["OdemeSekli"] != null) _OdemeSekli.Value = (string)Session["OdemeSekli"];
                }
                break;

            case "TasiyiciFirmaId":
                ASPxTextEdit _TasiyiciFirmaId = e.Editor as ASPxTextEdit;
                if ((_TasiyiciFirmaId.Value == null) || (_TasiyiciFirmaId.Value.ToString() == ""))
                {
                    if (this.TasiyiciFirmaId.SelectedIndex >= 0) _TasiyiciFirmaId.Value = this.TasiyiciFirmaId.Value;
                    else if (Session["TasiyiciFirmaId"] != null) _TasiyiciFirmaId.Value = (Guid)Session["TasiyiciFirmaId"];
                }
                break;
            case "TasiyiciFirma":
                ASPxTextEdit _TasiyiciFirma = e.Editor as ASPxTextEdit;
                if ((_TasiyiciFirma.Value == null) || (_TasiyiciFirma.Value.ToString() == ""))
                {
                    if (this.TasiyiciFirmaId.SelectedIndex >= 0) _TasiyiciFirma.Value = this.TasiyiciFirmaId.Text;
                    else if (Session["TasiyiciFirma"] != null) _TasiyiciFirma.Value = (string)Session["TasiyiciFirma"];
                }
                break;
        }
    }

    protected void GridYukleme_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["YuklemeNo"] == null) e.NewValues["YuklemeNo"] = 1;
            if (e.NewValues["UrunId"] == null) e.NewValues["UrunId"] = DBNull.Value;
            if (e.NewValues["SezonId"] == null)
            {
                if (this.SezonId.SelectedIndex >= 0)
                {
                    e.NewValues["SezonId"] = this.SezonId.Value;
                    e.NewValues["Sezon"] = this.SezonId.Text;
                }
            }
            if (e.NewValues["Adet"] == null) e.NewValues["Adet"] = 0;
            if (e.NewValues["Tutar"] == null) e.NewValues["Tutar"] = 0;
            if (e.NewValues["EksikUrun"] == null) e.NewValues["EksikUrun"] = 0;
            if (e.NewValues["FazlaUrun"] == null) e.NewValues["FazlaUrun"] = 0;
            if (e.NewValues["IsEkMaliyet"] == null) e.NewValues["IsEkMaliyet"] = false;
            if (e.NewValues["ProformaTarihi"] == null) e.NewValues["ProformaTarihi"] = DBNull.Value;
            if (e.NewValues["AsamaId"] == null) e.NewValues["AsamaId"] = DBNull.Value;
            if (e.NewValues["Iskonto"] == null)
            {
                if ((this.Iskonto.Value != null) && (!String.IsNullOrEmpty(this.Iskonto.Value.ToString())))
                {
                    e.NewValues["Iskonto"] = this.Iskonto.Value.ToString();
                }
            }
            if (e.NewValues["SevkSekliId"] == null)
            {
                if (this.SevkSekliId.SelectedIndex >= 0)
                {
                    e.NewValues["SevkSekliId"] = this.SevkSekliId.Value;
                    e.NewValues["SevkSekli"] = this.SevkSekliId.Text;
                }
            }
            if (e.NewValues["GelenTutar"] == null) e.NewValues["GelenTutar"] = DBNull.Value;
            if (e.NewValues["OdemeIskonto"] == null) e.NewValues["OdemeIskonto"] = DBNull.Value;
            if (e.NewValues["YapilanOdeme"] == null) e.NewValues["YapilanOdeme"] = DBNull.Value;
            if (e.NewValues["KalanOdeme"] == null) e.NewValues["KalanOdeme"] = DBNull.Value;
            if (e.NewValues["OdemeSekliId"] == null)
            {
                if (this.OdemeSekliId.SelectedIndex >= 0)
                {
                    e.NewValues["OdemeSekliId"] = this.OdemeSekliId.Value;
                    e.NewValues["OdemeSekli"] = this.OdemeSekliId.Text;
                }
            }
            if (e.NewValues["SonYuklemeTarihi"] == null) e.NewValues["SonYuklemeTarihi"] = DBNull.Value;
            if (e.NewValues["BankaId"] == null) e.NewValues["BankaId"] = DBNull.Value;
            if (e.NewValues["TasiyiciFirmaId"] == null)// e.NewValues["TasiyiciFirmaId"] = DBNull.Value;
            {
                if (this.TasiyiciFirmaId.SelectedIndex >= 0)
                {
                    e.NewValues["TasiyiciFirmaId"] = this.TasiyiciFirmaId.Value;
                    e.NewValues["TasiyiciFirma"] = this.TasiyiciFirmaId.Text;
                }
            }
            if (e.NewValues["OdemeVadesi"] == null) e.NewValues["OdemeVadesi"] = DBNull.Value;
            if (e.NewValues["TahminiVarisTarihi"] == null) e.NewValues["TahminiVarisTarihi"] = DBNull.Value;
            if (e.NewValues["GumrukVarisTarihi"] == null) e.NewValues["GumrukVarisTarihi"] = DBNull.Value;
            if (e.NewValues["TahminiDepoGirisTarihi"] == null) e.NewValues["TahminiDepoGirisTarihi"] = DBNull.Value;
            if (e.NewValues["DepoGirisTarihi"] == null) e.NewValues["DepoGirisTarihi"] = DBNull.Value;
            if (e.NewValues["DepoId"] == null) e.NewValues["DepoId"] = DBNull.Value;
            if (e.NewValues["TahminiMagazaDagitimTarihi"] == null) e.NewValues["TahminiMagazaDagitimTarihi"] = DBNull.Value;
            if (e.NewValues["MagazaDagitimTarihi"] == null) e.NewValues["MagazaDagitimTarihi"] = DBNull.Value;
            if (e.NewValues["Agirlik"] == null) e.NewValues["Agirlik"] = DBNull.Value;
            if (e.NewValues["PaketAdet"] == null) e.NewValues["PaketAdet"] = DBNull.Value;

            //e.NewValues["YapilanOdeme"] = e.NewValues["GelenTutar"];
            //if ((e.NewValues["GelenTutar"] != null) && (!String.IsNullOrEmpty(e.NewValues["GelenTutar"].ToString())))
            //{
            //    if ((e.NewValues["OdemeIskonto"] != null) && (!String.IsNullOrEmpty(e.NewValues["OdemeIskonto"].ToString())))
            //    {
            //        e.NewValues["YapilanOdeme"] = (Convert.ToDecimal(e.NewValues["GelenTutar"]) * (100 - Convert.ToDecimal(e.NewValues["OdemeIskonto"]))) / 100;
            //    }
            //}
            if ((e.NewValues["SonYuklemeTarihi"] != null) && (!String.IsNullOrEmpty(e.NewValues["SonYuklemeTarihi"].ToString())))
            {
                if ((e.NewValues["OdemeSekliId"] != null) && (!String.IsNullOrEmpty(e.NewValues["OdemeSekliId"].ToString())))
                {
                    SqlCommand cmd = DB.SQL(this.Context, "SELECT IsNull(Gun,0)Gun FROM IthalatOdemeSekli WHERE Id=@Id");
                    DB.AddParam(cmd, "@Id", (new Guid(e.NewValues["OdemeSekliId"].ToString())));
                    int _Gun = (int)cmd.ExecuteScalar();
                    DateTime _SonYuklemeTarihi = (DateTime)e.NewValues["SonYuklemeTarihi"];
                    e.NewValues["OdemeVadesi"] = _SonYuklemeTarihi.AddDays(_Gun);
                }
            }
        }
    }

    protected void GridYukleme_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (e.NewValues["YuklemeNo"] == null) e.NewValues["YuklemeNo"] = 1;
            if (e.NewValues["UrunId"] == null) e.NewValues["UrunId"] = DBNull.Value;
            if (e.NewValues["SezonId"] == null)
            {
                if (this.SezonId.SelectedIndex >= 0)
                {
                    e.NewValues["SezonId"] = this.SezonId.Value;
                    e.NewValues["Sezon"] = this.SezonId.Text;
                }
            }
            if (e.NewValues["Adet"] == null) e.NewValues["Adet"] = 0;
            if (e.NewValues["Tutar"] == null) e.NewValues["Tutar"] = 0;
            if (e.NewValues["EksikUrun"] == null) e.NewValues["EksikUrun"] = 0;
            if (e.NewValues["FazlaUrun"] == null) e.NewValues["FazlaUrun"] = 0;
            if (e.NewValues["IsEkMaliyet"] == null) e.NewValues["IsEkMaliyet"] = false;
            if (e.NewValues["ProformaTarihi"] == null) e.NewValues["ProformaTarihi"] = DBNull.Value;
            if (e.NewValues["AsamaId"] == null) e.NewValues["AsamaId"] = DBNull.Value;
            if (e.NewValues["Iskonto"] == null)
            {
                if ((this.Iskonto.Value != null) && (!String.IsNullOrEmpty(this.Iskonto.Value.ToString())))
                {
                    e.NewValues["Iskonto"] = this.Iskonto.Value.ToString();
                }
            }
            if (e.NewValues["SevkSekliId"] == null)
            {
                if (this.SevkSekliId.SelectedIndex >= 0)
                {
                    e.NewValues["SevkSekliId"] = this.SevkSekliId.Value;
                    e.NewValues["SevkSekli"] = this.SevkSekliId.Text;
                }
            }
            if (e.NewValues["GelenTutar"] == null) e.NewValues["GelenTutar"] = DBNull.Value;
            if (e.NewValues["OdemeIskonto"] == null) e.NewValues["OdemeIskonto"] = DBNull.Value;
            if (e.NewValues["YapilanOdeme"] == null) e.NewValues["YapilanOdeme"] = DBNull.Value;
            if (e.NewValues["KalanOdeme"] == null) e.NewValues["KalanOdeme"] = DBNull.Value;
            if (e.NewValues["OdemeSekliId"] == null)
            {
                if (this.OdemeSekliId.SelectedIndex >= 0)
                {
                    e.NewValues["OdemeSekliId"] = this.OdemeSekliId.Value;
                    e.NewValues["OdemeSekli"] = this.OdemeSekliId.Text;
                }
            }
            if (e.NewValues["SonYuklemeTarihi"] == null) e.NewValues["SonYuklemeTarihi"] = DBNull.Value;
            if (e.NewValues["BankaId"] == null) e.NewValues["BankaId"] = DBNull.Value;
            if (e.NewValues["TasiyiciFirmaId"] == null) //e.NewValues["TasiyiciFirmaId"] = DBNull.Value;
            {
                if (this.TasiyiciFirmaId.SelectedIndex >= 0)
                {
                    e.NewValues["TasiyiciFirmaId"] = this.TasiyiciFirmaId.Value;
                    e.NewValues["TasiyiciFirma"] = this.TasiyiciFirmaId.Text;
                }
            }
            if (e.NewValues["OdemeVadesi"] == null) e.NewValues["OdemeVadesi"] = DBNull.Value;
            if (e.NewValues["TahminiVarisTarihi"] == null) e.NewValues["TahminiVarisTarihi"] = DBNull.Value;
            if (e.NewValues["GumrukVarisTarihi"] == null) e.NewValues["GumrukVarisTarihi"] = DBNull.Value;
            if (e.NewValues["TahminiDepoGirisTarihi"] == null) e.NewValues["TahminiDepoGirisTarihi"] = DBNull.Value;
            if (e.NewValues["DepoGirisTarihi"] == null) e.NewValues["DepoGirisTarihi"] = DBNull.Value;
            if (e.NewValues["DepoId"] == null) e.NewValues["DepoId"] = DBNull.Value;
            if (e.NewValues["TahminiMagazaDagitimTarihi"] == null) e.NewValues["TahminiMagazaDagitimTarihi"] = DBNull.Value;
            if (e.NewValues["MagazaDagitimTarihi"] == null) e.NewValues["MagazaDagitimTarihi"] = DBNull.Value;
            if (e.NewValues["Agirlik"] == null) e.NewValues["Agirlik"] = DBNull.Value;
            if (e.NewValues["PaketAdet"] == null) e.NewValues["PaketAdet"] = DBNull.Value;

            //e.NewValues["YapilanOdeme"] = e.NewValues["GelenTutar"];
            //if ((e.NewValues["GelenTutar"] != null) && (!String.IsNullOrEmpty(e.NewValues["GelenTutar"].ToString())))
            //{
            //    if ((e.NewValues["OdemeIskonto"] != null) && (!String.IsNullOrEmpty(e.NewValues["OdemeIskonto"].ToString())))
            //    {
            //        e.NewValues["YapilanOdeme"] = (Convert.ToDecimal(e.NewValues["GelenTutar"]) * (100 - Convert.ToDecimal(e.NewValues["OdemeIskonto"]))) / 100;
            //    }
            //}
            if ((e.NewValues["SonYuklemeTarihi"] != null) && (!String.IsNullOrEmpty(e.NewValues["SonYuklemeTarihi"].ToString())))
            {
                if ((e.NewValues["OdemeSekliId"] != null) && (!String.IsNullOrEmpty(e.NewValues["OdemeSekliId"].ToString())))
                {
                    SqlCommand cmd = DB.SQL(this.Context, "SELECT IsNull(Gun,0)Gun FROM IthalatOdemeSekli WHERE Id=@Id");
                    DB.AddParam(cmd, "@Id", (new Guid(e.NewValues["OdemeSekliId"].ToString())));
                    int _Gun = (int)cmd.ExecuteScalar();
                    DateTime _SonYuklemeTarihi = (DateTime)e.NewValues["SonYuklemeTarihi"];
                    e.NewValues["OdemeVadesi"] = _SonYuklemeTarihi.AddDays(_Gun);
                }
            }
        }
    }

    protected void GridYukleme_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["YuklemeNo"] == null)
        {
            e.RowError = "Lütfen Yükleme No alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["Asama"] == null)
        {
            e.RowError = "Lütfen Aþama alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["UrunId"] == null)
        {
            e.RowError = "Lütfen Ürün alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["Adet"] == null)
        {
            e.RowError = "Lütfen Adet alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["Tutar"] == null)
        {
            e.RowError = "Lütfen Tutar alanýný boþ býrakmayýnýz...";
            return;
        }
    }

    private void UserVisibility()
    {
        if (Membership.GetUser().UserName.ToLower() == "admin") return;

        DataTable columns = new DataTable();
        SqlCommand cmd = DB.SQL(this.Context, "SELECT ColumnName FROM SecurityTableColumns WHERE TableName='IthalatSiparisYukleme' ORDER BY Sayac");
        cmd.CommandTimeout = 1000;
        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = cmd;
        adapter.Fill(columns);
        foreach (DataRow row in columns.Rows)
        {
            string _column = row["ColumnName"].ToString();
            bool _visible = false;
            StringBuilder sql = new StringBuilder("SELECT COUNT(*) FROM SecurityEdit t1 ");
            sql.Append("INNER JOIN SecurityEditColumns t2 ON(t1.Id=t2.SecurityEditId) ");
            sql.Append("INNER JOIN SecurityEditRoles t3 ON(t1.Id=t3.SecurityEditId) ");
            sql.Append("INNER JOIN (SELECT DISTINCT t12.Role,t13.UserName FROM SecurityUserRoles t11 ");
            sql.Append("            INNER JOIN SecurityRoles t12 ON(t11.RoleId=t12.RoleId) ");
            sql.Append("            INNER JOIN SecurityUsers t13 ON(t11.UserId=t13.UserId) ");
            sql.Append("            WHERE t13.UserName=@UserName) AS t4 ON(t3.Role=t4.Role) ");
            sql.Append("WHERE t2.ColumnName=@ColumnName and t2.[Select]=1");
            cmd = DB.SQL(this.Context, sql.ToString());
            DB.AddParam(cmd, "@UserName", 60, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@ColumnName", 100, _column);
            cmd.CommandTimeout = 1000;
            int _role = (int)cmd.ExecuteScalar();

            sql = new StringBuilder("SELECT COUNT(*) FROM SecurityEdit t1 ");
            sql.Append("INNER JOIN SecurityEditColumns t2 ON(t1.Id=t2.SecurityEditId) ");
            sql.Append("INNER JOIN SecurityEditUsers t3 ON(t1.Id=t3.SecurityEditId) ");
            sql.Append("WHERE t2.ColumnName=@ColumnName and t3.UserName=@UserName and t2.[Select]=1");
            cmd = DB.SQL(this.Context, sql.ToString());
            DB.AddParam(cmd, "@UserName", 60, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@ColumnName", 100, _column);
            cmd.CommandTimeout = 1000;
            int _user = (int)cmd.ExecuteScalar();

            if ((_role > 0) || (_user > 0)) _visible = true;
            this.GridYukleme.Columns[_column].Visible = _visible;
            if (_column == "UrunId")
            {
                this.GridYukleme.Columns["Urun"].Visible = _visible;
                this.GridYukleme.Columns["UrunAra"].Visible = _visible;
            }
            if (_column == "SezonId")
            {
                this.GridYukleme.Columns["Sezon"].Visible = _visible;
                this.GridYukleme.Columns["SezonAra"].Visible = _visible;
            }
            if (_column == "AsamaId")
            {
                this.GridYukleme.Columns["Asama"].Visible = _visible;
                this.GridYukleme.Columns["AsamaAra"].Visible = _visible;
            }
            if (_column == "SevkSekliId")
            {
                this.GridYukleme.Columns["SevkSekli"].Visible = _visible;
                this.GridYukleme.Columns["SevkSekliAra"].Visible = _visible;
            }
            if (_column == "OdemeSekliId")
            {
                this.GridYukleme.Columns["OdemeSekli"].Visible = _visible;
                this.GridYukleme.Columns["OdemeSekliAra"].Visible = _visible;
            }
            if (_column == "BankaId")
            {
                this.GridYukleme.Columns["Banka"].Visible = _visible;
                this.GridYukleme.Columns["BankaAra"].Visible = _visible;
            }
            if (_column == "TasiyiciFirmaId")
            {
                this.GridYukleme.Columns["TasiyiciFirma"].Visible = _visible;
                this.GridYukleme.Columns["TasiyiciFirmaAra"].Visible = _visible;
            }
            if (_column == "DepoId")
            {
                this.GridYukleme.Columns["Depo"].Visible = _visible;
                this.GridYukleme.Columns["DepoAra"].Visible = _visible;
            }
        }
    }
}
