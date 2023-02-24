using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxNavBar;
using Model.Crm;

public partial class menu : System.Web.UI.Page
{
    //protected override object LoadPageStateFromPersistenceMedium()
    //{
    //    return PageUtils.LoadPageStateFromPersistenceMedium(this.Context, this.Page);
    //}

    //protected override void SavePageStateToPersistenceMedium(object viewState)
    //{
    //    PageUtils.SavePageStateToPersistenceMedium(this.Context, this.Page, viewState);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack || IsCallback) return;
        LoadUserValues();
        InitDTList(this.DTList.Table);
        FillDTList();
        FillMenu();
        this.NavBar.AutoCollapse = true;
        this.NavBar.Groups.FindByText("Kullanýcý").Expanded = true;
    }

    private void InitDTList(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("ParentId", typeof(Guid));
        dt.Columns.Add("DirectoryName", typeof(string));
        dt.Columns.Add("Description", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    void LoadUserValues()
    {
        using (SqlCommand cmd = DB.SQL(this.Context, "Select IPUserName,IPPassword,IPDahili From SecurityUsers Where UserName=@UserName"))
        {
            DB.AddParam(cmd, "@UserName", 100, Membership.GetUser().UserName);
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                if (!String.IsNullOrEmpty(rdr["IPUserName"].ToString()) && !String.IsNullOrEmpty(rdr["IPPassword"].ToString()) && !String.IsNullOrEmpty(rdr["IPDahili"].ToString()))
                {
                    Session["IPUserName"] = rdr["IPUserName"].ToString();
                    Session["IPPassword"] = rdr["IPPassword"].ToString();
                    Session["IPDahili"] = rdr["IPDahili"].ToString();
                    Session["UserName"] = Membership.GetUser().UserName;
                    this.IPPanel.Visible = true;
                }

            }
            rdr.Close();
        }
    }

    private void FillDTList()
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM FMDirectory");
        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTList.Table.NewRow();
            row["ID"] = rdr["Id"];
            row["ParentId"] = rdr["ParentId"];
            row["DirectoryName"] = rdr["DirectoryName"];
            row["Description"] = rdr["Description"].ToString();
            row["CreatedBy"] = rdr["CreatedBy"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];
            this.DTList.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTList.Table.AcceptChanges();
    }

    private void FillMenu()
    {
        NavBarGroup group = null;
        #region yönetim
        bool bKullanici = Security.CheckPermission(this.Context, "Kullanici", "Select");
        bool bRol = Security.CheckPermission(this.Context, "Rol", "Select");
        bool bKullaniciGrup = Security.CheckPermission(this.Context, "Kullanici Gruplari", "Select");

        if (bKullanici || bRol || bKullaniciGrup)
        {
            group = this.NavBar.Groups.Add("Yönetim");
            group.HeaderImage.Url = "~/images/Menu_yonetim.gif";
            group.Expanded = false;
            if (bKullanici)
                group.Items.Add("Kullanýcý Tanýmlarý", null, "~/images/Menu_Users.gif", "~/admin/admin/User/list.aspx", "content");
            //if (bKullaniciGrup)
            //    group.Items.Add("Kullanýcý Gruplarý", null, "~/images/Menu_UsersGrup.gif", "~/CRM/Genel/UserGrup/list.aspx", null);
            if (bRol)
                group.Items.Add("Roller", null, "~/images/Menu_UsersRoles.gif", "~/admin/admin/Role/list.aspx", "content");
        }
        #endregion
        #region kullanýcý
        bool bisozet = Security.CheckPermission(this.Context, "Raporlar - Job Summary", "Select");
        group = this.NavBar.Groups.Add("Kullanýcý");
        group.HeaderImage.Url = "~/images/Menu_UsersTop.gif";
        group.Expanded = false;
        group.Items.Add("Giriþ Sayfasý", null, "~/images/gohome.png", "~/CRM/Genel/Issue/GundemGiris.aspx", "content");
        if (bisozet)
            group.Items.Add("Ýþ Özet Raporu", null, "~/images/works.png", "~/CRM/Raporlar/Raporlar/JobSummary.aspx", "content");
        group.Items.Add("Çýkýþ", "cikis", "~/images/1398609481_exit.png", "~/logout.aspx", "_top");
        group.Items.Add("Þifre Deðiþtir", "sifredegistir", "~/images/Menu_ChangePsw.gif", "~/account/account/Passwordsettings/changepassword.aspx", "content");
        #endregion


        #region Onay Sayfalarý
        bool bOnayList = Security.CheckPermission(this.Context, "OnayIssues", "Select");
        bool bExtendedGundem = Security.CheckPermission(this.Context, "ExtendedGundem", "Select");
        if (bOnayList || bExtendedGundem)
        {
            group = this.NavBar.Groups.Add("Onay Sayfalarý");
            group.HeaderImage.Url = "~/images/approv_ico.png";
            group.Expanded = false;
            if (bOnayList)
                group.Items.Add("Uzatma Ýstekleri Onay Ekraný", "OnayIssues", "~/images/Ok-icon.png", "~/CRM/Genel/OnayIssues/list.aspx", "content");
            if (bExtendedGundem)
                group.Items.Add("Gündem Uzatma Ekraný", "ExtendedGundem", "~/images/Extend.png", "~/CRM/Genel/OnayIssues/ExtendIssues.aspx", "content");
        }
        #endregion
        #region genel
        bool bProje = Security.CheckPermission(this.Context, "Proje", "Select");
        bool bFirma = Security.CheckPermission(this.Context, "Firmalar", "Select");
        group.Expanded = false;
        group = this.NavBar.Groups.Add("Genel");
        group.HeaderImage.Url = "~/images/Menu_Genel.gif";
        if (bFirma)
            group.Items.Add("Ýlgili Birimler", null, "~/images/Menu_Musteriler.gif", "~/CRM/Genel/Firma/list.aspx", "content");
        if (bProje)
            group.Items.Add("Departmanlar", null, "~/images/Menu_Projeler.gif", "~/CRM/Genel/Proje/list.aspx", "content");
        #endregion
        #region gündemler
        bool bFindIssue = Security.CheckPermission(this.Context, "Bildirim Ara", "Select");
        bool bAddIssue = Security.CheckPermission(this.Context, "Bildirim Ekle", "Select");

        //bool bIsPlani = Security.CheckPermission(this.Context, "IsPlani", "Select");
        bool bBR = Security.CheckPermission(this.Context, "BR", "Select");
        if (bAddIssue || bFindIssue || bBR)
        {
            group = this.NavBar.Groups.Add("Gündem");
            group.HeaderImage.Url = "~/images/Menu_Bildirimler.gif";
            group.Expanded = false;
            if (bBR)
                group.Items.Add("BR", null, "~/images/Plain.png", "~/CRM/Genel/BR/list.aspx", "content");
            if (bAddIssue)
                group.Items.Add("Yeni Gündem Giriþi", null, "~/images/Menu_YeniBildirim.gif", "~/CRM/Genel/Issue/AddIssuePopUp.aspx?ProcId=1", "content");
            if (bAddIssue)
                group.Items.Add("Tanýmlý Gündemler", null, "~/images/Works.png", "~/CRM/Genel/Tanimli/list.aspx", "content");
            if (bFindIssue)
                group.Items.Add("Gündem Ara", null, "~/images/Menu_BildirimAra.gif", "~/CRM/Genel/Issue/list.aspx", "content");

        }
        #endregion
        #region E-Ticaret
        bool bEticaretDepo = Security.CheckPermission(this.Context, "E-Ticaret - Depolar", "Select");
        bool bWebSiparis = Security.CheckPermission(this.Context, "Genel - Web Sipariþ", "Select");
        if (bEticaretDepo | bWebSiparis)
        {
            group = this.NavBar.Groups.Add("E-Ticaret");
            group.HeaderImage.Url = "~/images/e-ticaret.png";
            group.Expanded = false;
            if (bWebSiparis)
                group.Items.Add("Web Sipariþleri", null, "~/images/e-ticaret16.png", "~/CRM/Genel/WebSiparis/list.aspx", "content");
            if (bEticaretDepo)
                group.Items.Add("Depolar", null, "~/images/stores.png", "~/CRM/Settings/NebimStores/list.aspx", "content");

        }
        #endregion
        #region ithalat takip
        bool bGroupOpen = false;
        bool bIthalatSiparis = Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ", "Select");
        bool bIthalatFirma = Security.CheckPermission(this.Context, "Ýthalat Takip - Firma", "Select");
        bool bIthalatAsama = Security.CheckPermission(this.Context, "Ýthalat Takip - Aþama", "Select");
        bool bIthalatSezon = Security.CheckPermission(this.Context, "Ýthalat Takip - Sezon", "Select");
        bool bIthalatTasiyiciFirma = Security.CheckPermission(this.Context, "Ýthalat Takip - Taþýyýcý Firma", "Select");
        bool bIthalatSevkSekli = Security.CheckPermission(this.Context, "Ýthalat Takip - Sevk Þekli", "Select");
        bool bIthalatOdemeSekli = Security.CheckPermission(this.Context, "Ýthalat Takip - Ödeme Þekli", "Select");
        bool bParaBirimi = Security.CheckPermission(this.Context, "Taným - Para Birimi", "Select");
        bool bBanka = Security.CheckPermission(this.Context, "Taným - Banka", "Select");
        bool bDepo = Security.CheckPermission(this.Context, "Taným - Depo", "Select");
        bool bGuvenlik = Security.CheckPermission(this.Context, "Ýthalat Takip - Güvenlik", "Select");
        bool bIthalatSiparisRapor = Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ Rapor", "Select");
        bool bIthalatUsers = Security.CheckPermission(this.Context, "Tanim - ithalat Yoneticileri", "Select");
        bGroupOpen = bIthalatSiparis || bIthalatFirma || bIthalatAsama || bIthalatSezon || bIthalatTasiyiciFirma || bIthalatSevkSekli
            || bIthalatOdemeSekli || bParaBirimi || bBanka || bDepo || bGuvenlik || bIthalatSiparisRapor || bIthalatUsers;
        if (bGroupOpen)
        {
            group = this.NavBar.Groups.Add("Ýthalat Takip");
            group.HeaderImage.Url = "~/images/Menu_order.png";
            group.Expanded = false;
            if (bIthalatSiparis) group.Items.Add("Sipariþler", null, null, "~/CRM/IthalatTakip/Siparis/list.aspx", "content");
            if (bIthalatFirma) group.Items.Add("Firmalar", null, null, "~/CRM/IthalatTakip/Firma/list.aspx", "content");
            if (bIthalatTasiyiciFirma) group.Items.Add("Taþýyýcý Firmalar", null, null, "~/CRM/IthalatTakip/TasiyiciFirma/list.aspx", "content");
            if (bIthalatAsama) group.Items.Add("Aþamalar", null, null, "~/CRM/IthalatTakip/Asama/list.aspx", "content");
            if (bIthalatSezon) group.Items.Add("Sezonlar", null, null, "~/CRM/IthalatTakip/Sezon/list.aspx", "content");
            if (bIthalatSevkSekli) group.Items.Add("Sevk Þekilleri", null, null, "~/CRM/IthalatTakip/SevkSekli/list.aspx", "content");
            if (bIthalatOdemeSekli) group.Items.Add("Ödeme Þekilleri", null, null, "~/CRM/IthalatTakip/OdemeSekli/list.aspx", "content");
            if (bParaBirimi) group.Items.Add("Para Birimleri", null, null, "~/CRM/IthalatTakip/ParaBirimi/list.aspx", "content");
            if (bBanka) group.Items.Add("Bankalar", null, null, "~/CRM/IthalatTakip/Banka/list.aspx", "content");
            if (bDepo) group.Items.Add("Depolar", null, null, "~/CRM/IthalatTakip/Depo/list.aspx", "content");
            if (bGuvenlik) group.Items.Add("Sipariþ Kayýt Güvenlik ", null, null, "~/CRM/IthalatTakip/Guvenlik/list.aspx", "content");
            if (bIthalatSiparisRapor) group.Items.Add("Rapor Sipariþler", null, null, "~/CRM/IthalatTakip/Rapor/ithalat_siparis.aspx", "content");
            if (bIthalatUsers) group.Items.Add("Ýthalat Yöneticileri", null, null, "~/CRM/Settings/IthalatUsers/list.aspx", "content");
        }
        #endregion
        #region rapor
        bool bbirimrapor = Security.CheckPermission(this.Context, "BirimRapor", "Select");
        bool bsavsaklamarapor = Security.CheckPermission(this.Context, "Raporlar - Savsaklama Raporu", "Select");
        bool bdepartmentdelayrapor = Security.CheckPermission(this.Context, "DepartmentDelayReport", "Select");
        bool bdepartmentrapor = Security.CheckPermission(this.Context, "DepartmentReport", "Select");
        bool bIssueClassdelayrapor = Security.CheckPermission(this.Context, "DepartmentDelayReport", "Select");

        if (bbirimrapor || bsavsaklamarapor || bdepartmentdelayrapor || bdepartmentrapor || bIssueClassdelayrapor)
        {
            group = this.NavBar.Groups.Add("Raporlar");
            group.HeaderImage.Url = "~/images/Menu_Raporlar.png";
            group.Expanded = false;
            if (bAddIssue)
                group.Items.Add("Gündem Raporu Chart", null, "~/images/Plain.png", "~/CRM/Raporlar/Raporlar/MagazaVirusleriChart.aspx", "content");
            if (bsavsaklamarapor)
                group.Items.Add("Kullanýcý Zamanýnda Yapýlmadý Raporu", null, "~/images/Plain.png", "~/CRM/Raporlar/Raporlar/UserSavsaklama.aspx", "content");
            if (bdepartmentdelayrapor)
                group.Items.Add("Departman Zamanýnda Yapýlmadý Raporu", null, "~/images/Plain.png", "~/CRM/Raporlar/Raporlar/DepartmentDelayReport.aspx", "content");
            if (bIssueClassdelayrapor)
                group.Items.Add("Gündem Sýnýfý Yapýlmadý Raporu", null, "~/images/Plain.png", "~/CRM/Raporlar/Raporlar/IssueClassDelayReport.aspx", "content");
            if (bdepartmentrapor)
                group.Items.Add("Departman Raporu", null, "~/images/Plain.png", "~/CRM/Raporlar/Raporlar/DepartmentReport.aspx", "content");

        }
        #endregion
        #region tanýmlar
        bool bDurum = Security.CheckPermission(this.Context, "Tanim - Bildirim Durumlari", "Select");
        bool bUlke = Security.CheckPermission(this.Context, "Tanim - Ülke", "Select");
        bool bSehir = Security.CheckPermission(this.Context, "Tanim - Sehir", "Select");
        bool bHataTipleri = Security.CheckPermission(this.Context, "Tanim - Hata Tipleri", "Select");
        bool bOnemDereceleri = Security.CheckPermission(this.Context, "Tanim - Önem Dereceleri", "Select");
        bool bDosyaTuru = Security.CheckPermission(this.Context, "Tanim - Dosya Türleri", "Select");
        bool bProjeSiniflari = Security.CheckPermission(this.Context, "Taným - Proje Sýnýflarý", "Select");
        bool bYaziRenkleri = Security.CheckPermission(this.Context, "YaziRenkleri", "Select");
        //bool bBrRenkKodlari = Security.CheckPermission(this.Context, "Tanim - Br Renk Kodlari", "Select");
        bool bVirusSinif = Security.CheckPermission(this.Context, "Taným - Virüs Sýnýflarý", "Select");
        bool bBrMarka = Security.CheckPermission(this.Context, "Tanim - BR Marka", "Select");
        bool bBrDurum = Security.CheckPermission(this.Context, "Tanim - BR Durumlari", "Select");
        bool bBrUsers = Security.CheckPermission(this.Context, "Tanim - Br Yoneticileri", "Select");
        bool bSesEmail = Security.CheckPermission(this.Context, "Taným - Ses Email", "Select");
        bool bAppSettings = Security.CheckPermission(this.Context, "Taným - Uygulama Tanýmlarý", "Select");
        bool bPhoneBook = Security.CheckPermission(this.Context, "Tanim - Phone Book", "Select");
        bool bPop3MailList = Security.CheckPermission(this.Context, "Taným - Pop3 Mail", "Select");

        if (bDurum || bHataTipleri || bSehir || bUlke || bOnemDereceleri || bDosyaTuru || bProjeSiniflari || bYaziRenkleri
            || bBrMarka || bBrDurum || bBrUsers || bVirusSinif || bSesEmail || bPhoneBook || bPop3MailList)
        {
            group = this.NavBar.Groups.Add("Tanýmlamalar");
            group.HeaderImage.Url = "~/images/Menu_Tanimlamalar.gif";
            group.Expanded = false;
            if (bDurum)
                group.Items.Add("Gündem Durumlarý", null, null, "~/CRM/Settings/Durum/list.aspx", "content");
            if (bDosyaTuru)
                group.Items.Add("Dosya Türleri", null, null, "~/CRM/Settings/DosyaTuru/list.aspx", "content");
            if (bHataTipleri)
                group.Items.Add("Operasyon Yöntemleri", null, null, "~/CRM/Settings/HataTipleri/list.aspx", "content");
            if (bProjeSiniflari)
                group.Items.Add("Departman Gruplarý", null, null, "~/CRM/Settings/ProjeSiniflari/list.aspx", "content");
            if (bOnemDereceleri)
                group.Items.Add("Önem Dereceleri", null, null, "~/CRM/Settings/OnemDereceleri/list.aspx", "content");
            if (bYaziRenkleri)
                group.Items.Add("Yazý Renkleri", null, null, "~/CRM/Settings/YaziRenkleri/list.aspx", "content");
            if (bBrUsers)
                group.Items.Add("Br Yöneticileri", null, null, "~/CRM/Settings/BrUsers/list.aspx", "content");
            if (bVirusSinif)
                group.Items.Add("Gündem Sýnýflarý", null, null, "~/CRM/Settings/VirusSinif/list.aspx", "content");
            if (bBrMarka)
                group.Items.Add("Br Marka", null, null, "~/CRM/Settings/BrMarka/list.aspx", "content");
            if (bBrDurum)
                group.Items.Add("Br Durumlarý", null, null, "~/CRM/Settings/BrDurum/list.aspx", "content");
            if (bUlke)
                group.Items.Add("Ülke Tanýmla", null, null, "~/CRM/Settings/Ulke/list.aspx", "content");
            if (bSehir)
                group.Items.Add("Sehir Tanýmla", null, null, "~/CRM/Settings/Sehir/list.aspx", "content");
            if (bSesEmail) group.Items.Add("Ses E-Posta Tanýmlarý", null, null, "~/CRM/Settings/SesEmail/list.aspx", "content");
            if (bAppSettings) group.Items.Add("Uygulama Tanýmlarý", null, null, "~/CRM/Settings/AppSettings/edit.aspx", "content");
            if (bPhoneBook) group.Items.Add("Telefon Defteri", null, null, "~/CRM/Settings/PhoneBook/list.aspx", "content");
            if (bPop3MailList) group.Items.Add("Pop Mail Listesi", null, null, "~/CRM/Settings/Pop3Mails/list.aspx", "content");
        }
        #endregion
        #region Dosya Yönetimi
        bool bFmDirectory = Security.CheckPermission(this.Context, "File Manager - Directory", "Select");
        bool bFmFile = Security.CheckPermission(this.Context, "File Manager - File", "Select");
        bGroupOpen = bFmDirectory || bFmFile;
        if (bGroupOpen)
        {
            group = this.NavBar.Groups.Add("Dosya Yönetimi", "Dosya Yönetimi");
            group.Expanded = false;
            if (bFmDirectory) group.Items.Add("Dizinler", null, null, "~/CRM/FileManager/Directory/list.aspx", "content");
        }
        this.TreeList.Visible = bFmFile;
        #endregion
    }

    void NavBar_ItemClick(object source, NavBarItemEventArgs e)
    {
        if (e.Item.Name.Equals("cikis"))
        {
            FormsAuthentication.SignOut();
            this.Session.Abandon();
            this.Response.Redirect("../login.aspx");
        }
    }

}
