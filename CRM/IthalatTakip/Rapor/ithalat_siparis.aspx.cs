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
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxCallback;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using System.Drawing;

public partial class CRM_IthalatTakip_Rapor_ithalat_siparis : System.Web.UI.Page
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
        this.menu.ItemClick += new MenuItemEventHandler(menu_ItemClick);
        this.Menu1.ItemClick += new MenuItemEventHandler(menu1_ItemClick);
        this.Menu2.ItemClick += new MenuItemEventHandler(menu2_ItemClick);
        if (this.IsPostBack || this.IsCallback) return;

        if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ Rapor", "Select"))
        {
            //this.Response.Write("<html><body bgcolor='#e6e6fa'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            //this.Response.End();
            CrmUtils.MessageAlert(this.Page, "Bu sayfayý görme yetkisine sahip deðilsiniz!", "Alert");
            return;
        }
        UserVisibility();
        this.SiparisTarihi1.Date = DateTime.Parse(DateTime.Now.Year.ToString() + "-01-01 00:00:00");
        this.SiparisTarihi2.Date = DateTime.Now;
    }

    public void menu_ItemClick(object source, MenuItemEventArgs e)
    {
        //
    }

    public void menu1_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("excel"))
        {
            ExportUtils.GridExport(this.Page, this.GridExport1, ExportType.xls, true);
            return;
        }
        else if (e.Item.Name.Equals("pdf"))
        {
            ExportUtils.GridExport(this.Page, this.GridExport1, ExportType.pdf, true);
            return;
        }
    }

    public void menu2_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("excel"))
        {
            ExportUtils.GridExport(this.Page, this.GridExport2, ExportType.xls, true);
            return;
        }
        else if (e.Item.Name.Equals("pdf"))
        {
            ExportUtils.GridExport(this.Page, this.GridExport2, ExportType.pdf, true);
            return;
        }
    }

    private void LoadDocument(int GridNo, string parameters)
    {
        Guid _FirmaId = Guid.Empty;
        Guid _SezonId = Guid.Empty;
        Guid _MarkaId = Guid.Empty;
        Guid _UrunId = Guid.Empty;
        string[] parameter = parameters.Split('|');
        if (!String.IsNullOrEmpty(parameter[0])) _FirmaId = new Guid(parameter[0]);
        if (!String.IsNullOrEmpty(parameter[1])) _SezonId = new Guid(parameter[1]);
        if (!String.IsNullOrEmpty(parameter[2])) _MarkaId = new Guid(parameter[2]);
        if (!String.IsNullOrEmpty(parameter[3])) _UrunId = new Guid(parameter[3]);

        StringBuilder sb = new StringBuilder("EXEC rep_IthalatSiparis @RaporTuru");
        if (_FirmaId != Guid.Empty) sb.Append(",@FirmaId");
        else sb.Append(",NULL");
        if (_SezonId != Guid.Empty) sb.Append(",@SezonId");
        else sb.Append(",NULL");
        if (_MarkaId != Guid.Empty) sb.Append(",@MarkaId");
        else sb.Append(",NULL");
        if (_UrunId != Guid.Empty) sb.Append(",@UrunId");
        else sb.Append(",NULL");
        if (!CrmUtils.IsNullOrEmptyDateTime(this.SiparisTarihi1.Date.ToString())) sb.Append(",@SiparisTarihi1");
        else sb.Append(",NULL");
        if (!CrmUtils.IsNullOrEmptyDateTime(this.SiparisTarihi2.Date.ToString())) sb.Append(",@SiparisTarihi2");
        else sb.Append(",NULL");
        if (!CrmUtils.IsNullOrEmptyDateTime(this.YuklemeTarihi1.Date.ToString())) sb.Append(",@YuklemeTarihi1");
        else sb.Append(",NULL");
        if (!CrmUtils.IsNullOrEmptyDateTime(this.YuklemeTarihi2.Date.ToString())) sb.Append(",@YuklemeTarihi2");
        else sb.Append(",NULL");
        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@RaporTuru", GridNo);
        if (_FirmaId != Guid.Empty) DB.AddParam(cmd, "@FirmaId", _FirmaId);
        if (_SezonId != Guid.Empty) DB.AddParam(cmd, "@SezonId", _SezonId);
        if (_MarkaId != Guid.Empty) DB.AddParam(cmd, "@MarkaId", _MarkaId);
        if (_UrunId != Guid.Empty) DB.AddParam(cmd, "@UrunId", _UrunId);
        if (!CrmUtils.IsNullOrEmptyDateTime(this.SiparisTarihi1.Date.ToString())) DB.AddParam(cmd, "@SiparisTarihi1", this.SiparisTarihi1.Date);
        if (!CrmUtils.IsNullOrEmptyDateTime(this.SiparisTarihi2.Date.ToString())) DB.AddParam(cmd, "@SiparisTarihi2", this.SiparisTarihi2.Date);
        if (!CrmUtils.IsNullOrEmptyDateTime(this.YuklemeTarihi1.Date.ToString())) DB.AddParam(cmd, "@YuklemeTarihi1", this.YuklemeTarihi1.Date);
        if (!CrmUtils.IsNullOrEmptyDateTime(this.YuklemeTarihi2.Date.ToString())) DB.AddParam(cmd, "@YuklemeTarihi2", this.YuklemeTarihi2.Date);
        cmd.CommandTimeout = 1000;
        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = cmd;
        if (GridNo == 1)
        {
            this.DTRapor.Table.Clear();
            adapter.Fill(this.DTRapor.Table);
            this.Grid.DataBind();
        }
        if (GridNo == 2)
        {
            this.DTRapor2.Table.Clear();
            adapter.Fill(this.DTRapor2.Table);
            this.Grid2.DataBind();
        }
    }

    protected void Grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string Tarih1 = "";
        if (!CrmUtils.IsNullOrEmptyDateTime(this.SiparisTarihi1.Date.ToString()))
        {
            Tarih1 = this.SiparisTarihi1.Date.ToShortDateString();
        }
        else
        {
            Tarih1 = "01.01." + DateTime.Now.Year.ToString();
        }
        string Tarih2 = "";
        if (!CrmUtils.IsNullOrEmptyDateTime(this.SiparisTarihi2.Date.ToString()))
        {
            Tarih2 = this.SiparisTarihi2.Date.ToShortDateString();
        }
        else
        {
            Tarih2 = "31.12." + DateTime.Now.Year.ToString();
        }
        this.Grid.SettingsText.Title = "Sipariþler (" + Tarih1 + " - " + Tarih2 + ")";
        LoadDocument(1, e.Parameters);
    }

    protected void Grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType != GridViewRowType.Data) return;

        int _Tip = (int)this.Grid.GetRowValues(e.VisibleIndex, "Tip");

        if (_Tip == 0) e.Row.BackColor = Color.FromName("#ff9966");
        if (_Tip == 1) e.Row.BackColor = Color.FromName("#ff9966");
        if (_Tip == 2) e.Row.BackColor = Color.FromName("#99ffff");
    }

    protected void Grid2_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string Tarih1 = "";
        if (!CrmUtils.IsNullOrEmptyDateTime(this.SiparisTarihi1.Date.ToString()))
        {
            Tarih1 = this.SiparisTarihi1.Date.ToShortDateString();
        }
        else
        {
            Tarih1 = "01.01." + DateTime.Now.Year.ToString();
        }
        string Tarih2 = "";
        if (!CrmUtils.IsNullOrEmptyDateTime(this.SiparisTarihi2.Date.ToString()))
        {
            Tarih2 = this.SiparisTarihi2.Date.ToShortDateString();
        }
        else
        {
            Tarih2 = "31.12." + DateTime.Now.Year.ToString();
        }
        this.Grid2.SettingsText.Title = "Toplamlar (" + Tarih1 + " - " + Tarih2 + ")";
        LoadDocument(2, e.Parameters);
    }

    protected void Grid2_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        //if (e.RowType != GridViewRowType.Data) return;

        //int _Tip = (int)this.Grid.GetRowValues(e.VisibleIndex, "Tip");

        //if (_Tip == 0) e.Row.BackColor = Color.FromName("#ff9966");
        //if (_Tip == 1) e.Row.BackColor = Color.FromName("#ff9966");
        //if (_Tip == 2) e.Row.BackColor = Color.FromName("#99ffff");
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
                #region ürünler
                case "UrunId":
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
                default:
                    break;
            }
            Session["SearchBrowser"] = htSearchBrowser;
            e.Result = parameters[0].Trim();
        }
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
        }
        rdr.Close();
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
        }
        rdr.Close();
    }

    protected void UrunId_Callback(object source, CallbackEventArgsBase e)
    {
        this.UrunId.Items.Clear();
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            Guid id = new Guid(e.Parameter);
            FillUrunId(this.UrunId, id);
            this.UrunId.SelectedIndex = 0;
        }
    }

    private void FillUrunId(ASPxComboBox source, Guid id)
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT Id,Urun FROM IthalatFirmaMarkaUrun WHERE Id=@Id");
        DB.AddParam(cmd, "@Id", id);
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        if (rdr.Read())
        {
            ListEditItem item = new ListEditItem();
            item.Value = rdr["Id"];
            item.Text = rdr["Urun"].ToString();
            source.Items.Add(item);
        }
        rdr.Close();
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

            if (this.Grid.Columns[_column] != null)
                this.Grid.Columns[_column].Visible = _visible;

            if (this.Grid2.Columns[_column] != null)
                this.Grid2.Columns[_column].Visible = _visible;

            if (_column == "ParaBirimiId")
            {
                if (this.Grid.Columns["ParaBirimi"] != null)
                    this.Grid.Columns["ParaBirimi"].Visible = _visible;
                if (this.Grid2.Columns["ParaBirimi"] != null)
                    this.Grid2.Columns["ParaBirimi"].Visible = _visible;
            }
            if (_column == "UrunId")
            {
                if (this.Grid.Columns["Urun"] != null)
                    this.Grid.Columns["Urun"].Visible = _visible;
                if (this.Grid2.Columns["Urun"] != null)
                    this.Grid2.Columns["Urun"].Visible = _visible;
            }
            if (_column == "SezonId")
            {
                if (this.Grid.Columns["Sezon"] != null)
                    this.Grid.Columns["Sezon"].Visible = _visible;
                if (this.Grid2.Columns["Sezon"] != null)
                    this.Grid2.Columns["Sezon"].Visible = _visible;
            }
            if (_column == "AsamaId")
            {
                if (this.Grid.Columns["Asama"] != null)
                    this.Grid.Columns["Asama"].Visible = _visible;
                if (this.Grid2.Columns["Asama"] != null)
                    this.Grid2.Columns["Asama"].Visible = _visible;
            }
            if (_column == "SevkSekliId")
            {
                if (this.Grid.Columns["SevkSekli"] != null)
                    this.Grid.Columns["SevkSekli"].Visible = _visible;
                if (this.Grid2.Columns["SevkSekli"] != null)
                    this.Grid2.Columns["SevkSekli"].Visible = _visible;
            }
            if (_column == "OdemeSekliId")
            {
                if (this.Grid.Columns["OdemeSekli"] != null)
                    this.Grid.Columns["OdemeSekli"].Visible = _visible;
                if (this.Grid2.Columns["OdemeSekli"] != null)
                    this.Grid2.Columns["OdemeSekli"].Visible = _visible;
            }
            if (_column == "BankaId")
            {
                if (this.Grid.Columns["Banka"] != null)
                    this.Grid.Columns["Banka"].Visible = _visible;
                if (this.Grid2.Columns["Banka"] != null)
                    this.Grid2.Columns["Banka"].Visible = _visible;
            }
            if (_column == "TasiyiciFirmaId")
            {
                if (this.Grid.Columns["TasiyiciFirma"] != null)
                    this.Grid.Columns["TasiyiciFirma"].Visible = _visible;
                if (this.Grid2.Columns["TasiyiciFirma"] != null)
                    this.Grid2.Columns["TasiyiciFirma"].Visible = _visible;
            }
            if (_column == "DepoId")
            {
                if (this.Grid.Columns["Depo"] != null)
                    this.Grid.Columns["Depo"].Visible = _visible;
                if (this.Grid2.Columns["Depo"] != null)
                    this.Grid2.Columns["Depo"].Visible = _visible;
            }
        }
    }
}
