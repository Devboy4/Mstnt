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
using DevExpress.Web.Data;

public partial class CRM_IthalatTakip_Siparis_list : System.Web.UI.Page
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
        if (this.IsPostBack || this.IsCallback) return;

        if (!Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ", "Select"))
        {
            //this.Response.Write("<html><body bgcolor='#e6e6fa'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            //this.Response.End();
            CrmUtils.MessageAlert(this.Page, "Bu sayfayý görme yetkisine sahip deðilsiniz!", "Alert");
            return;
        }

        bool bInsert = Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Ýthalat Takip - Sipariþ", "Delete");

        if (bInsert || bUpdate || bDelete)
            this.menu.Items.FindByName("save").Visible = true;
        else
            this.menu.Items.FindByName("save").Visible = false;

        InitGrid(this.DTList.Table);

        for (int i = 0; i < this.Grid.Columns.Count; i++)
        {
            if (this.Grid.Columns[i] is GridViewCommandColumn)
            {
                //(this.Grid.Columns[i] as GridViewCommandColumn).NewButton.Visible = bInsert;
                //(this.Grid.Columns[i] as GridViewCommandColumn).EditButton.Visible = bUpdate;
                (this.Grid.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
                break;
            }
        }

        LoadDocument();
    }

    public void menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("excel"))
        {
            ExportUtils.GridExport(this.Page, this.gridExport, ExportType.xls, true);
            return;
        }
        else if (e.Item.Name.Equals("pdf"))
        {
            ExportUtils.GridExport(this.Page, this.gridExport, ExportType.pdf, true);
            return;
        }

        if (e.Item.Name.Equals("save"))
        {
            this.Grid.UpdateEdit();
            if (SaveDocument())
            {
                this.Response.Redirect("./list.aspx");
            }
            else
            {
                CrmUtils.MessageAlert(this.Page, ((String)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "Alert");
                return;
            }
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    private void InitGrid(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("SiparisNo", typeof(int));
        dt.Columns.Add("SiparisTarihi", typeof(DateTime));
        dt.Columns.Add("NebimNo", typeof(string));
        dt.Columns.Add("AsamaId", typeof(Guid));
        dt.Columns.Add("Asama", typeof(string));
        dt.Columns.Add("FirmaId", typeof(Guid));
        dt.Columns.Add("Firma", typeof(string));
        dt.Columns.Add("SezonId", typeof(Guid));
        dt.Columns.Add("Sezon", typeof(string));
        dt.Columns.Add("MarkaId", typeof(Guid));
        dt.Columns.Add("Marka", typeof(string));
        dt.Columns.Add("Adet", typeof(decimal));
        dt.Columns.Add("Tutar", typeof(decimal));
        dt.Columns.Add("ParaBirimiId", typeof(Guid));
        dt.Columns.Add("ParaBirimi", typeof(string));
        dt.Columns.Add("Iskonto", typeof(decimal));
        dt.Columns.Add("SevkSekliId", typeof(Guid));
        dt.Columns.Add("SevkSekli", typeof(string));
        dt.Columns.Add("OdemeSekliId", typeof(Guid));
        dt.Columns.Add("OdemeSekli", typeof(string));
        dt.Columns.Add("TasiyiciFirmaId", typeof(Guid));
        dt.Columns.Add("TasiyiciFirma", typeof(string));
        dt.Columns.Add("Aciklama", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("Kaydeden", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("Guncelleyen", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        dt.Columns.Add("BarcodeId", typeof(int));
        dt.Columns.Add("SalesPriceId", typeof(int));
        dt.Columns.Add("SystemImageUploadId", typeof(int));
        dt.Columns.Add("IsNebimInside", typeof(int));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument()
    {
        StringBuilder sb = new StringBuilder("SELECT t1.*,t2.Asama,t3.Adi Firma,t4.Sezon,t5.Marka,t6.ParaBirimi,t7.SevkSekli,t8.OdemeSekli,t9.Adi TasiyiciFirma");
        sb.Append(",(t10.FirstName+' '+t10.LastName)Kaydeden,(t11.FirstName+' '+t11.LastName)Guncelleyen ");
        sb.Append("FROM IthalatSiparis t1 ");
        sb.Append("LEFT JOIN IthalatAsama t2 ON(t1.AsamaId=t2.Id) ");
        sb.Append("LEFT JOIN IthalatFirma t3 ON(t1.FirmaId=t3.Id) ");
        sb.Append("LEFT JOIN IthalatSezon t4 ON(t1.SezonId=t4.Id) ");
        sb.Append("LEFT JOIN IthalatFirmaMarka t5 ON(t1.MarkaId=t5.Id) ");
        sb.Append("LEFT JOIN ParaBirimi t6 ON(t1.ParaBirimiId=t6.Id) ");
        sb.Append("LEFT JOIN IthalatSevkSekli t7 ON(t1.SevkSekliId=t7.Id) ");
        sb.Append("LEFT JOIN IthalatOdemeSekli t8 ON(t1.OdemeSekliId=t8.Id) ");
        sb.Append("LEFT JOIN IthalatTasiyiciFirma t9 ON(t1.TasiyiciFirmaId=t9.Id) ");
        sb.Append("LEFT JOIN SecurityUsers t10 ON(t1.CreatedBy=t10.UserName) ");
        sb.Append("LEFT JOIN SecurityUsers t11 ON(t1.ModifiedBy=t11.UserName) ");
        sb.Append("ORDER BY t1.Sayac desc ");
        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTList.Table.NewRow();
            row["ID"] = rdr["Id"];
            row["SiparisNo"] = rdr["Sayac"];
            row["SiparisTarihi"] = rdr["SiparisTarihi"];
            row["NebimNo"] = rdr["NebimNo"];
            row["AsamaId"] = rdr["AsamaId"];
            row["Asama"] = rdr["Asama"];
            row["FirmaId"] = rdr["FirmaId"];
            row["Firma"] = rdr["Firma"];
            row["SezonId"] = rdr["SezonId"];
            row["Sezon"] = rdr["Sezon"];
            row["MarkaId"] = rdr["MarkaId"];
            row["Marka"] = rdr["Marka"];
            row["Adet"] = rdr["Adet"];
            row["Tutar"] = rdr["Tutar"];
            row["ParaBirimiId"] = rdr["ParaBirimiId"];
            row["ParaBirimi"] = rdr["ParaBirimi"];
            row["Iskonto"] = rdr["Iskonto"];
            row["SevkSekliId"] = rdr["SevkSekliId"];
            row["SevkSekli"] = rdr["SevkSekli"];
            row["OdemeSekliId"] = rdr["OdemeSekliId"];
            row["OdemeSekli"] = rdr["OdemeSekli"];
            row["TasiyiciFirmaId"] = rdr["TasiyiciFirmaId"];
            row["TasiyiciFirma"] = rdr["TasiyiciFirma"];
            row["Aciklama"] = rdr["Aciklama"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["Kaydeden"] = rdr["Kaydeden"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["Guncelleyen"] = rdr["Guncelleyen"];
            row["ModificationDate"] = rdr["ModificationDate"];
            row["BarcodeId"] = rdr["BarcodeId"];
            row["SalesPriceId"] = rdr["SalesPriceId"];
            row["SystemImageUploadId"] = rdr["SystemImageUploadId"];
            row["IsNebimInside"] = rdr["IsNebimInside"];
            this.DTList.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTList.Table.AcceptChanges();
    }

    private bool SaveDocument()
    {
        DataTable changes = this.DTList.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Deleted:
                        DB.BeginTrans(this.Context);
                        #region delete
                        try
                        {
                            SqlCommand cmd = DB.SQL(this.Context, "DELETE FROM IthalatSiparis WHERE Id=@Id");
                            DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
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
                        #endregion
                        DB.Commit(this.Context);
                        break;
                }
            }
        }
        return true;
    }

    protected void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        //ASPxGridView grid = (ASPxGridView)sender;
        DataTable changes = this.DTList.Table.GetChanges();
        if (changes != null) e.Properties["cpIsDirty"] = true;
        else e.Properties["cpIsDirty"] = false;
    }

    protected void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        e.Cell.ToolTip = string.Format("{0}", e.CellValue);
    }
}
