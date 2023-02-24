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
using System.IO;

public partial class CRM_Genel_WebSiparis_list : System.Web.UI.Page
{
    CrmUtils utls = new CrmUtils();
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
        if (!Security.CheckPermission(this.Context, "Genel - Web Sipariþ", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        // Eventleri tanimlayalim
        this.menu.ItemClick += new MenuItemEventHandler(menu_ItemClick);

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }

        StartDate.Date = DateTime.Now.AddMonths(-1);
        EndDate.Date = DateTime.Now;

        bool bInsert = Security.CheckPermission(this.Context, "Genel - Web Sipariþ", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Genel - Web Sipariþ", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Genel - Web Sipariþ", "Delete");

        if (bInsert || bUpdate)
            this.menu.Items.FindByName("save").Visible = true;
        else
            this.menu.Items.FindByName("save").Visible = false;

        InitGridTable(this.DataTableList.Table);


        for (int i = 0; i < this.grid.Columns.Count; i++)
        {
            if (this.grid.Columns[i] is GridViewCommandColumn)
            {
                (this.grid.Columns[i] as GridViewCommandColumn).NewButton.Visible = bInsert;
                (this.grid.Columns[i] as GridViewCommandColumn).EditButton.Visible = bUpdate;
                (this.grid.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
                break;
            }
        }
        LoadForm();
        LoadDocument();
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
        }
        else if (e.Item.Name.Equals("excel"))
        {
            CrmUtils.ExportToxls(gridExport, "grid", true);
        }
        else if (e.Item.Name.Equals("pdf"))
        {
            CrmUtils.ExportTopdf(gridExport, "grid", true);
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }
    void LoadForm()
    {
        int UserId = -1;
        using (SqlCommand cmd = DB.SQL(this.Context, "SELECT IndexId FROM SecurityUsers WHERE UserName=@UserName"))
        {
            DB.AddParam(cmd, "@UserName", 100, Membership.GetUser().UserName);
            UserId = (int)cmd.ExecuteScalar();
            this.HiddenUserId.Value = UserId.ToString();
        }

        ViewState["IsSipAdmin"] = "0";

        if (Security.CheckPermission(this.Context, "Genel - Web Sipariþ Admin", "Select"))
        {
            ViewState["IsSipAdmin"] = "1";

        }
        else
            menu.Items[2].Visible = false;
    }
    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IndexId", typeof(int));
        dt.Columns.Add("WebSiparisNo", typeof(string));
        dt.Columns.Add("UrunCardId", typeof(int));
        dt.Columns.Add("SiparisTarihi", typeof(DateTime));
        dt.Columns.Add("KargoFirma", typeof(string));
        dt.Columns.Add("KargoTakipNo", typeof(string));
        dt.Columns.Add("FaturaRefNo", typeof(string));
        dt.Columns.Add("SiparisKaynak", typeof(string));
        dt.Columns.Add("Durum", typeof(int));
        dt.Columns.Add("MagazaNot", typeof(string));
        dt.Columns.Add("MusteriNot", typeof(string));
        dt.Columns.Add("MerkezNot", typeof(string));
        dt.Columns.Add("TeslimatAliciAd", typeof(string));
        dt.Columns.Add("TeslimatTel", typeof(string));
        dt.Columns.Add("UyeMail", typeof(string));
        dt.Columns.Add("TeslimatAdres", typeof(string));
        dt.Columns.Add("TeslimatIlce", typeof(string));
        dt.Columns.Add("TeslimatSehir", typeof(string));
        dt.Columns.Add("Barkod", typeof(string));
        dt.Columns.Add("StokKodu", typeof(string));
        dt.Columns.Add("Beden", typeof(string));
        dt.Columns.Add("Adet", typeof(int));
        dt.Columns.Add("UrunAd", typeof(string));
        dt.Columns.Add("Marka", typeof(string));
        dt.Columns.Add("SiparisNot", typeof(string));
        dt.Columns.Add("AtananMagaza", typeof(int));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModificationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }
    private void LoadDocument()
    {      
        using (SqlCommand cmd = DB.SQL(this.Context, "EXEC SP_GetWebSiparis @UserId,@IsAdmin,@StartDate,@EndDate"))
        {
            DB.AddParam(cmd, "@UserID", Convert.ToInt32(this.HiddenUserId.Value));
            DB.AddParam(cmd, "@IsAdmin", ViewState["IsSipAdmin"].ToString() == "1" ? 1 : 0);
            DB.AddParam(cmd, "@StartDate", StartDate.Date);
            DB.AddParam(cmd, "@EndDate", EndDate.Date);
            cmd.CommandTimeout = 1000;
            cmd.Prepare();

            this.DataTableList.Table.Rows.Clear();

            IDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DataRow row = this.DataTableList.Table.NewRow();
                row["IndexId"] = rdr["Id"];
                row["ID"] = rdr["WebSiparisId"];
                row["WebSiparisNo"] = rdr["WebSipNo"];
                row["UrunCardId"] = rdr["UrunCardId"];
                row["SiparisTarihi"] = rdr["SiparisTarihi"];
                row["KargoFirma"] = rdr["KargoFirma"];
                row["KargoTakipNo"] = rdr["KargoTakipNo"];
                row["FaturaRefNo"] = rdr["FaturaRefNo"];
                row["SiparisKaynak"] = rdr["SiparisKaynak"];
                row["Durum"] = rdr["Durum"];
                row["MagazaNot"] = rdr["MagazaNot"];
                row["MusteriNot"] = rdr["MusteriNot"];
                row["MerkezNot"] = rdr["MerkezNot"];
                row["TeslimatAliciAd"] = rdr["TeslimatAliciAd"];
                row["TeslimatTel"] = rdr["TeslimatTel"];
                row["TeslimatAdres"] = rdr["TeslimatAdres"];
                row["TeslimatIlce"] = rdr["TeslimatIlce"];
                row["TeslimatSehir"] = rdr["TeslimatSehir"];
                row["UyeMail"] = rdr["UyeMail"];
                row["Barkod"] = rdr["Barkod"];
                row["StokKodu"] = rdr["StokKodu"];
                row["Beden"] = rdr["Beden"];
                row["Adet"] = rdr["Adet"];
                row["UrunAd"] = rdr["UrunAd"];
                row["Marka"] = rdr["Marka"];
                row["AtananMagaza"] = rdr["AtananMagaza"];
                row["CreatedBy"] = rdr["CreatedBy"];
                row["ModifiedBy"] = rdr["ModifiedBy"];
                row["ModificationDate"] = rdr["ModificationDate"];
                row["CreationDate"] = rdr["CreationDate"];

                this.DataTableList.Table.Rows.Add(row);
            }
            rdr.Close();
        }

        this.DataTableList.Table.AcceptChanges();

        grid.DataBind();

        if (this.DataTableList != null)
            this.DataTableList.Dispose();

    }
    private bool SaveDocument()
    {
        DB.BeginTrans(this.Context);

        SqlCommand cmd = null;
        StringBuilder sb;

        DataTable changes = this.DataTableList.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        sb = new StringBuilder();
                        sb.Append("EXEC Sp_InsertWebSiparis ");
                        sb.Append("@Id");
                        sb.Append(",@WebSiparisNo,@UrunCardId,@SiparisTarihi");
                        sb.Append(",@KargoFirma");
                        sb.Append(",@KargoTakipNo");
                        sb.Append(",@FaturaRefNo");
                        sb.Append(",@SipKaynak");
                        sb.Append(",@Durum");
                        sb.Append(",@MagazaNot");
                        sb.Append(",@MusteriNot");
                        sb.Append(",@MerkezNot");
                        sb.Append(",@TeslimatAliciAd");
                        sb.Append(",@TeslimatTel");
                        sb.Append(",@UyeMail");
                        sb.Append(",@TeslimatAdres");
                        sb.Append(",@TeslimatIlce");
                        sb.Append(",@TeslimatSehir");
                        sb.Append(",@Barkod");
                        sb.Append(",@StokKodu");
                        sb.Append(",@Beden");
                        sb.Append(",@Adet");
                        sb.Append(",@UrunAd");
                        sb.Append(",@Marka");
                        sb.Append(",@AtananMagaza");
                        sb.Append(",@CreatedBy");
                        sb.Append(",@CreationDate");


                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@WebSiparisNo", 50, row["WebSiparisNo"].ToString());
                        DB.AddParam(cmd, "@UrunCardId", (int)row["UrunCardId"]);
                        DB.AddParam(cmd, "@SiparisTarihi", (DateTime)row["SiparisTarihi"]);
                        DB.AddParam(cmd, "@KargoFirma", 150, row["KargoFirma"].ToString());
                        DB.AddParam(cmd, "@KargoTakipNo", 50, row["KargoTakipNo"].ToString());
                        DB.AddParam(cmd, "@FaturaRefNo", 150, row["FaturaRefNo"].ToString());
                        DB.AddParam(cmd, "@SipKaynak", 150, row["SiparisKaynak"].ToString());
                        DB.AddParam(cmd, "@Durum", (int)row["Durum"]);
                        DB.AddParam(cmd, "@MagazaNot", 500, row["MagazaNot"].ToString());
                        DB.AddParam(cmd, "@MusteriNot", 500, row["MusteriNot"].ToString());
                        DB.AddParam(cmd, "@MerkezNot", 500, row["MerkezNot"].ToString());
                        DB.AddParam(cmd, "@TeslimatAliciAd", 100, row["TeslimatAliciAd"].ToString());
                        DB.AddParam(cmd, "@TeslimatTel", 50, row["TeslimatTel"].ToString());
                        DB.AddParam(cmd, "@UyeMail", 100, row["UyeMail"].ToString());
                        DB.AddParam(cmd, "@TeslimatAdres", 500, row["TeslimatAdres"].ToString());
                        DB.AddParam(cmd, "@TeslimatIlce", 50, row["TeslimatIlce"].ToString());
                        DB.AddParam(cmd, "@TeslimatSehir", 50, row["TeslimatSehir"].ToString());
                        DB.AddParam(cmd, "@Barkod", 20, row["Barkod"].ToString());
                        DB.AddParam(cmd, "@StokKodu", 20, row["StokKodu"].ToString());
                        DB.AddParam(cmd, "@Beden", 20, row["Beden"].ToString());
                        DB.AddParam(cmd, "@Adet", (int)row["Adet"]);
                        DB.AddParam(cmd, "@UrunAd", 500, row["UrunAd"].ToString());
                        DB.AddParam(cmd, "@Marka", 150, row["Marka"].ToString());
                        DB.AddParam(cmd, "@AtananMagaza", (int)row["AtananMagaza"]);
                        DB.AddParam(cmd, "@CreatedBy", 255, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Modified:
                        sb = new StringBuilder();
                        sb.Append("EXEC Sp_UpdateWebSiparis ");
                        sb.Append("@Id");
                        sb.Append(",@WebSiparisNo,@UrunCardId,@SiparisTarihi");
                        sb.Append(",@KargoFirma");
                        sb.Append(",@KargoTakipNo");
                        sb.Append(",@FaturaRefNo");
                        sb.Append(",@SipKaynak");
                        sb.Append(",@Durum");
                        sb.Append(",@MagazaNot");
                        sb.Append(",@MusteriNot");
                        sb.Append(",@MerkezNot");
                        sb.Append(",@TeslimatAliciAd");
                        sb.Append(",@TeslimatTel");
                        sb.Append(",@UyeMail");
                        sb.Append(",@TeslimatAdres");
                        sb.Append(",@TeslimatIlce");
                        sb.Append(",@TeslimatSehir");
                        sb.Append(",@Barkod");
                        sb.Append(",@StokKodu");
                        sb.Append(",@Beden");
                        sb.Append(",@Adet");
                        sb.Append(",@UrunAd");
                        sb.Append(",@Marka");
                        sb.Append(",@AtananMagaza");
                        sb.Append(",@CreatedBy");
                        sb.Append(",@CreationDate");

                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@Id", (int)row["IndexId"]);
                        DB.AddParam(cmd, "@WebSiparisNo", 50, row["WebSiparisNo"].ToString());
                        DB.AddParam(cmd, "@UrunCardId", (int)row["UrunCardId"]);
                        DB.AddParam(cmd, "@SiparisTarihi", (DateTime)row["SiparisTarihi"]);
                        DB.AddParam(cmd, "@KargoFirma", 150, row["KargoFirma"].ToString());
                        DB.AddParam(cmd, "@KargoTakipNo", 50, row["KargoTakipNo"].ToString());
                        DB.AddParam(cmd, "@FaturaRefNo", 150, row["FaturaRefNo"].ToString());
                        DB.AddParam(cmd, "@SipKaynak", 150, row["SiparisKaynak"].ToString());
                        DB.AddParam(cmd, "@Durum", (int)row["Durum"]);
                        DB.AddParam(cmd, "@MagazaNot", 500, row["MagazaNot"].ToString());
                        DB.AddParam(cmd, "@MusteriNot", 500, row["MusteriNot"].ToString());
                        DB.AddParam(cmd, "@MerkezNot", 500, row["MerkezNot"].ToString());
                        DB.AddParam(cmd, "@TeslimatAliciAd", 100, row["TeslimatAliciAd"].ToString());
                        DB.AddParam(cmd, "@TeslimatTel", 50, row["TeslimatTel"].ToString());
                        DB.AddParam(cmd, "@UyeMail", 100, row["UyeMail"].ToString());
                        DB.AddParam(cmd, "@TeslimatAdres", 500, row["TeslimatAdres"].ToString());
                        DB.AddParam(cmd, "@TeslimatIlce", 50, row["TeslimatIlce"].ToString());
                        DB.AddParam(cmd, "@TeslimatSehir", 50, row["TeslimatSehir"].ToString());
                        DB.AddParam(cmd, "@Barkod", 20, row["Barkod"].ToString());
                        DB.AddParam(cmd, "@StokKodu", 20, row["StokKodu"].ToString());
                        DB.AddParam(cmd, "@Beden", 20, row["Beden"].ToString());
                        DB.AddParam(cmd, "@Adet", (int)row["Adet"]);
                        DB.AddParam(cmd, "@UrunAd", 500, row["UrunAd"].ToString());
                        DB.AddParam(cmd, "@Marka", 150, row["Marka"].ToString());
                        DB.AddParam(cmd, "@AtananMagaza", (int)row["AtananMagaza"]);
                        DB.AddParam(cmd, "@CreatedBy", 255, Membership.GetUser().UserName);
                        DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Deleted:
                        try
                        {
                            cmd = DB.SQL(this.Context, "EXEC Sp_DeleteWebSiparis @Id");
                            DB.AddParam(cmd, "@Id", (int)row["IndexId", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            DataTableList.Table.Clear();
                            LoadDocument();
                            grid.DataBind();
                            CrmUtils.MessageAlert(this.Page, ex.Message.ToString().Replace("'", null).Replace("\r\n", null), "stkeySilinemez");
                            return false;
                        }
                        break;
                }
            }
        }

        DB.Commit(this.Context);

        return true;
    }
    protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["Durum"] == null)
        {
            e.RowError = "Durum alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["WebSiparisNo"] == null)
        {
            e.RowError = "Sipariþ No alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["SiparisTarihi"] == null)
        {
            e.RowError = "Siparis Tarihi alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["AtananMagaza"] == null)
        {
            e.RowError = "Atanan Maðaza alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["Barkod"] == null)
        {
            e.RowError = "Barkod alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["StokKodu"] == null)
        {
            e.RowError = "Stok Kodu alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["Adet"] == null)
        {
            e.RowError = "Adet alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["UrunAd"] == null)
        {
            e.RowError = "Urun Adý alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["Marka"] == null)
        {
            e.RowError = "Marka alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["UrunCardId"] == null)
        {
            e.NewValues["UrunCardId"] = 0;
        }




        if (e.NewValues["KargoTakipNo"] != null)
        {
            if (e.NewValues["KargoTakipNo"].ToString().Length < 3)
            {
                e.RowError = "Lütfen Kargo Takip numarasýnýn uzunluðunu 3'ten büyük olacak þekilde giriniz.";
                return;
            }

            bool _ticket = false;

            if (e.NewValues["KargoTakipNo"].ToString().Substring(0, 3).ToLower() != "1ze") _ticket = true;
            if (e.NewValues["KargoTakipNo"].ToString().Substring(0, 3).ToLower() != "1z9") _ticket = true;

            if (!_ticket)
            {
                e.RowError = "Lütfen Kargo Takip numarasýný baþýnda 1ZE veya 1Z9 olarak yazýnýz...örn:(1ZE0532XXXXXXX,1ze0532XXXXXXX,1Z90532XXXXXXX)";
                return;
            }
        }

        if (grid.IsNewRowEditing)
        {
            if (e.NewValues["FaturaRefNo"] == null)
            {
                e.RowError = "Fatura Ref No alanýný boþ býrakmayýnýz...";
                return;
            }
            if (e.NewValues["SiparisKaynak"] == null)
            {
                e.RowError = "Sipariþ Kaynak alanýný boþ býrakmayýnýz...";
                return;
            }

            if (e.NewValues["WebSiparisNo"] != null)
            {
                DataRow[] Rows = DataTableList.Table.Select("WebSiparisNo='" + e.NewValues["WebSiparisNo"].ToString() + "' AND UrunCardId=" + e.NewValues["UrunCardId"].ToString() + "");
                if (Rows.Length > 0)
                {
                    e.RowError = "Web Sipariþ numarasý ve Ürün Satýr ekli görünüyor lütfen baþka numara serisi ile tekrar deneyiniz...";
                    return;
                }
            }
        }
    }
    protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        bool IsSipAdmin = ViewState["IsSipAdmin"].ToString() == "1" ? true : false;


        if (!IsSipAdmin)
        {
            if (e.Column.FieldName == "KargoTakipNo" | e.Column.FieldName == "MagazaNot")
                e.Editor.ReadOnly = false;
            else
                e.Editor.ReadOnly = true;
        }
    }
    protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        e.Cell.ToolTip = string.Format("{0}", e.CellValue);
    }
    protected void btnExcelUpload_Click(object sender, EventArgs e)
    {
        string saver = SaveExcel();

        if(string.IsNullOrEmpty(saver))
        {
            this.Response.Redirect("./list.aspx");

        }
        else
        {
            CrmUtils.CreateMessageAlert(this.Page, saver, "stkeyerr");
            return;
        }
    }
    private string SaveExcel()
    {
        string _saveresult = string.Empty;

        decimal _out;
        int _intout;
        DB.BeginTrans(this.Context);

        StringBuilder sb;
        try
        {
            if (!flpExcel.HasFile)
            {
                _saveresult = "Excel Dosyasý Seçiniz";
                return _saveresult;
            }
            string _fileex = Path.GetExtension(flpExcel.FileName);
            if (!IsFileExtensionAllowed(_fileex))
            {
                _saveresult = "Seçtiðiniz dosya excel dosyasý deðil, lütfen kontrol edip tekrar deneyiniz.";
                return _saveresult;
            }

            string _fullfilepath = Server.MapPath("~/account/images/") + flpExcel.FileName;

            if (File.Exists(_fullfilepath))
                File.Delete(_fullfilepath);

            flpExcel.SaveAs(_fullfilepath);

            DataTable dt = new DataTable();

            dt = ExcelUtils.ExcelToDs(_fullfilepath);

            DateTime newdt = DateTime.Now;
            foreach (DataRow row in dt.Rows)
            {
                if (string.IsNullOrEmpty(row[1].ToString())) continue;
                if (string.IsNullOrEmpty(row[2].ToString())) continue;
                if (string.IsNullOrEmpty(row[3].ToString())) continue;
                sb = new StringBuilder();
                sb.Append("EXEC Sp_InsertWebSiparisFromExcel ");
                sb.Append("@WebSiparisNo,@UrunCardId,@SiparisTarihi");
                sb.Append(",@KargoFirma");
                sb.Append(",@KargoTakipNo");
                sb.Append(",@FaturaRefNo");
                sb.Append(",@SipKaynak");
                sb.Append(",@Durum");
                sb.Append(",@MagazaNot");
                sb.Append(",@MusteriNot");
                sb.Append(",@TeslimatAliciAd");
                sb.Append(",@TeslimatTel");
                sb.Append(",@UyeMail");
                sb.Append(",@TeslimatAdres");
                sb.Append(",@TeslimatIlce");
                sb.Append(",@TeslimatSehir");
                sb.Append(",@Barkod");
                sb.Append(",@StokKodu");
                sb.Append(",@Beden");
                sb.Append(",@Adet");
                sb.Append(",@UrunAd");
                sb.Append(",@Marka");
                sb.Append(",@AtananMagaza");
                sb.Append(",@CreatedBy");
                sb.Append(",@CreationDate");

                using (SqlCommand cmd = DB.SQL(this.Context, sb.ToString()))
                {
                    DB.AddParam(cmd, "@WebSiparisNo", 50, row[2].ToString());
                    DB.AddParam(cmd, "@UrunCardId", Convert.ToInt32(row[3].ToString()));
                    DB.AddParam(cmd, "@SiparisTarihi", Convert.ToDateTime(row[1].ToString()));
                    DB.AddParam(cmd, "@KargoFirma", 150, row[6].ToString());
                    DB.AddParam(cmd, "@KargoTakipNo", 50, row[7].ToString());
                    DB.AddParam(cmd, "@FaturaRefNo", 150, row[4].ToString());
                    DB.AddParam(cmd, "@SipKaynak", 150, row[5].ToString());
                    DB.AddParam(cmd, "@Durum", 150, row[8].ToString());
                    DB.AddParam(cmd, "@MagazaNot", 500, row[9].ToString());
                    DB.AddParam(cmd, "@MusteriNot", 500, row[10].ToString());
                    DB.AddParam(cmd, "@TeslimatAliciAd", 100, row[11].ToString());
                    DB.AddParam(cmd, "@TeslimatTel", 50, row[12].ToString());
                    DB.AddParam(cmd, "@UyeMail", 100, row[13].ToString());
                    DB.AddParam(cmd, "@TeslimatAdres", 500, row[14].ToString());
                    DB.AddParam(cmd, "@TeslimatIlce", 50, row[15].ToString());
                    DB.AddParam(cmd, "@TeslimatSehir", 50, row[16].ToString());
                    DB.AddParam(cmd, "@Barkod", 20, row[17].ToString());
                    DB.AddParam(cmd, "@StokKodu", 20, row[18].ToString());
                    DB.AddParam(cmd, "@Beden", 20, row[19].ToString());
                    DB.AddParam(cmd, "@Adet", Convert.ToInt32(row[20].ToString()));
                    DB.AddParam(cmd, "@UrunAd", 500, row[21].ToString());
                    DB.AddParam(cmd, "@Marka", 150, row[22].ToString());
                    DB.AddParam(cmd, "@AtananMagaza",150, row[0].ToString());
                    DB.AddParam(cmd, "@CreatedBy", 255, Membership.GetUser().UserName);
                    DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }

            DB.Commit(this.Context);
        }
        catch (Exception ex)
        {
            DB.Rollback(this.Context);
            _saveresult = ex.Message;
        }



        return _saveresult;
    }
    private bool IsFileExtensionAllowed(string FileName)
    {
        bool _result = false;
        switch (FileName)
        {
            case ".xls":
            case ".xlsx":
                _result = true;
                break;
            default:
                _result = false;
                break;
        }

        return _result;
    }
    protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters == "x")
        {
            LoadDocument();
        }
    }
}
