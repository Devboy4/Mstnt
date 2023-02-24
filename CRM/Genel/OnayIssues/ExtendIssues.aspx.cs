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
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxClasses;
using System.Collections.Generic;

public partial class CRM_Genel_OnayIssues_ExtendIssues : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "ExtendedGundem", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayı görme yetkisine sahip değilsiniz!</p></body></html>");
            this.Response.End();
        }

        // Eventleri tanimlayalim
        this.menu.ItemClick += new MenuItemEventHandler(menu_ItemClick);


        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }
        Response.AppendHeader("Pragma", "no-cache");
        Response.AppendHeader("Cache-Control", "no-cache");

        Response.CacheControl = "no-cache";
        Response.Expires = -1;
        InitGridTable(this.DataTableList.Table);
        fillcomboxes();
    }

    protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters != null)
        {
            string[] FirstParameters = e.Parameters.Split('|');
            switch (FirstParameters[0])
            {
                case "Select":
                    if (Convert.ToBoolean(FirstParameters[1]))
                        this.grid.Selection.SelectAll();
                    else
                        this.grid.Selection.UnselectAll();
                    break;
                case "List":
                    LoadDocument();
                    break;
            }

        }
    }

    public void menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("Extend"))
        {
            if (SaveDocument())
            {
                this.Response.Redirect("./ExtendIssues.aspx");
            }
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IndexID", typeof(int));
        dt.Columns.Add("PNR", typeof(int));
        dt.Columns.Add("VirusSinif", typeof(string));
        dt.Columns.Add("Baslik", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("TeslimTarihi", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument()
    {
        this.DataTableList.Table.Clear();
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder();
        sb.Append("SELECT I.IssueId,I.IndexID,I.Baslik,I.CreatedBy,I.TeslimTarihi,Vrs.Adi AS VirusSinif FROM Issue AS I WITH (NOLOCK) LEFT OUTER JOIN  Firma AS F ON  I.FirmaID=F.IndexId ");
        sb.Append("Left Outer Join Issue t2 On I.MainIssueID=t2.IndexId ");
        sb.Append("LEFT OUTER JOIN Proje AS P ON I.ProjeID=P.IndexId ");
        sb.Append("LEFT OUTER JOIN OnemDereceleri AS O ON I.OnemDereceID=O.IndexId ");
        sb.Append("LEFT OUTER JOIN VirusSinif Vrs ON I.VirusSinifId=Vrs.IndexId ");
        sb.Append("LEFT OUTER JOIN Durum AS D ON I.DurumID=D.IndexId ");
        sb.Append("LEFT OUTER JOIN SecurityUsers AS U ON I.UserID=U.IndexId WHERE I.Active='1' ");
        #region IssueID Filtreleniyor...
        if (!String.IsNullOrEmpty(this.IssueID.Text))
            sb.Append(" AND I.IndexID=@IndexID");
        #endregion
        #region Firma Filtreniyor...
        if (!String.IsNullOrEmpty(this.FirmaID.Text))
            sb.Append(" AND I.FirmaID='" + FirmaID.Value.ToString() + "' ");
        #endregion
        #region Proje Filtreleniyor...
        if (this.ProjeID.Text != " " && this.ProjeID.Text != "")
            sb.Append(" AND P.Adi='" + this.ProjeID.Text + "' ");
        #endregion
        #region Proje Sınıf Filtreleniyor...
        bool ilkk2 = true;
        foreach (ListItem ite in ProjeSinifID.Items)
        {
            if (ite.Selected)
            {
                if (ilkk2)
                {
                    sb.Append(" AND CONVERT(VarChar(50),P.ProjeSinifID) IN('" + ite.Value.ToString() + "'");
                    ilkk2 = false;
                }
                else
                    sb.Append(",'" + ite.Value.ToString() + "'");
            }
        }
        if (!ilkk2)
            sb.Append(")");
        #endregion
        #region Durum Filtreleniyor...
        bool ilkk = true;
        foreach (ListItem ite in DurumList1.Items)
        {
            if (ite.Selected)
            {
                if (ilkk)
                {
                    sb.Append(" AND I.DurumID IN('" + ite.Value.ToString() + "'");
                    ilkk = false;
                }
                else
                    sb.Append(",'" + ite.Value.ToString() + "'");
            }
        }
        if (!ilkk)
            sb.Append(")");
        #endregion
        #region Müdehale Yöntemi Filtreleniyor...
        ilkk = true;
        foreach (ListItem ite in chcMudehaleYontemi.Items)
        {
            if (ite.Selected)
            {
                if (ilkk)
                {
                    sb.Append(" AND I.HataTipID IN('" + ite.Value.ToString() + "'");
                    ilkk = false;
                }
                else
                    sb.Append(",'" + ite.Value.ToString() + "'");
            }
        }
        if (!ilkk)
            sb.Append(")");
        #endregion
        #region Virüs Sınıfları Filtreleniyor...
        ilkk = true;
        foreach (ListItem ite in chcVirusSiniflari.Items)
        {
            if (ite.Selected)
            {
                if (ilkk)
                {
                    sb.Append(" AND I.VirusSinifID IN('" + ite.Value.ToString() + "'");
                    ilkk = false;
                }
                else
                    sb.Append(",'" + ite.Value.ToString() + "'");
            }
        }
        if (!ilkk)
            sb.Append(")");
        #endregion
        #region Atadıklarım Filtreleniyor...
        if (Atayan.Checked)
            sb.Append(" AND I.CreatedBy='" + Membership.GetUser().UserName.ToLower() + "'");
        #endregion
        #region Atananlar Filtreleniyor...
        if (Atanan.Checked)
        {
            sb.Append(" AND EXISTS (Select 1 from UserAllowedIssueList t1 left join SecurityUsers t2 on t1.UserId=t2.IndexId Where t1.IssueId=I.IndexId and t2.UserName='" + Membership.GetUser().UserName + "')");
        }
        #endregion
        #region Anahtar Kelime Filtreleniyor...
        if (KeyWords.Value != null)
            sb.Append(" AND I.Keywords like '%" + this.KeyWords.Value.ToString() + "%'");
        #endregion
        #region Virüs Tanısı Filtreleniyor...
        if (TxtBaslik.Value != null)
            sb.Append(" AND I.Baslik like '%" + this.TxtBaslik.Value.ToString() + "%'");
        #endregion
        #region Tarih Filtreleniyor...
        if (!CrmUtils.ControllToDate(this.Page, StartDate.Date.ToString()) &&
        !CrmUtils.ControllToDate(this.Page, EndDate.Date.ToString()))
        {
            sb.Append(" AND I.BildirimTarihi>=@StartDate");
            sb.Append(" AND I.BildirimTarihi<DateAdd(hour,24,@EndDate)");
        }
        #endregion
        sb.Append(" ORDER BY I.IndexId DESC");
        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        #region Parameters1
        if (!String.IsNullOrEmpty(this.IssueID.Text))
            DB.AddParam(cmd, "@IndexID", int.Parse(this.IssueID.Text));
        if (!CrmUtils.ControllToDate(this.Page, StartDate.Date.ToString()) &&
        !CrmUtils.ControllToDate(this.Page, EndDate.Date.ToString()))
        {
            DB.AddParam(cmd, "@StartDate", this.StartDate.Date);
            DB.AddParam(cmd, "@EndDate", this.EndDate.Date);
        }
        #endregion
        cmd.Prepare();
        cmd.CommandTimeout = 1000;

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();

            row["ID"] = rdr["IssueId"];
            row["IndexID"] = rdr["IndexID"];
            row["PNR"] = rdr["IndexId"];
            row["Baslik"] = rdr["Baslik"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["TeslimTarihi"] = rdr["TeslimTarihi"];
            row["VirusSinif"] = rdr["VirusSinif"];

            this.DataTableList.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DataTableList.Table.AcceptChanges();
        grid.DataBind();
        grid.ExpandAll();


    }

    protected void grid_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
    {

        if (e.CallbackName == "APPLYCOLUMNFILTER" || e.CallbackName == "APPLYFILTER")
        {
            ((ASPxGridView)sender).ExpandAll();
        }
        //grid.ExpandAll();

    }

    private bool SaveDocument()
    {
        DB.BeginTrans(this.Context);

        SqlCommand cmd = null;
        StringBuilder sb;

        try
        {
            List<object> keyValues = this.grid.GetSelectedFieldValues("ID");
            string Ids = string.Empty;
            bool ilk = true;
            foreach (object key in keyValues)
            {
                DataRow row = DataTableList.Table.Rows.Find(key);
                if (ilk)
                {
                    Ids += row["IndexID"].ToString();
                    ilk = false;
                }
                else
                    Ids += "," + row["IndexID"].ToString();
            }

            sb = new StringBuilder();
            sb.Append("EXEC SaveOnayTable @Ids,@AddDay");
            cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@Ids", 64000, Ids);
            DB.AddParam(cmd, "@AddDay", int.Parse(txtAddDay.Value.ToString()));
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            CrmUtils.CreateMessageAlert(this.Page, "Seçilen Gündemler onay listesine gönderildi...", "saveonaytable");
        }
        catch (Exception ex)
        {
            CrmUtils.CreateMessageAlert(this.Page, "Hata...:" + ex.Message, "unsaveonaytable");
            DB.Rollback(this.Context);
            return false;
        }

        DB.Commit(this.Context);

        return true;
    }

    protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "Baslik")
        {
            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }
    }

    void fillcomboxes()
    {

        data.BindComboBoxesNoEmpty(this.Context, FirmaID, "Select IndexId,FirmaName From Firma Order By FirmaName", "IndexId", "FirmaName");
        data.ChcList_Fill(ProjeSinifID, "Select IndexId,Adi From ProjeSiniflari Order By Adi", "IndexId", "Adi");
        data.ChcList_Fill(chcMudehaleYontemi, "Select IndexId,Adi From HataTipleri Order By Adi", "IndexId", "Adi");
        data.ChcList_Fill(DurumList1, "SELECT IndexId,Adi FROM Durum ORDER BY Adi", "IndexId", "Adi");
        data.ChcList_Fill(chcVirusSiniflari, "SELECT IndexId,Adi FROM VirusSinif ORDER BY Adi", "IndexId", "Adi");

    }

    protected void ProjeID_Callback(object source, CallbackEventArgsBase e)
    {
        if (e.Parameter.ToString() == null || e.Parameter.ToString() == "")
            return;
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder();
        sb.Append("EXEC AllowedProjeListV2 '" + Membership.GetUser().UserName.ToLower() + "','" + e.Parameter.ToString() + "'");
        data.BindComboBoxesInt(this.Context, ProjeID, sb.ToString(), "ProjeID", "Adi");
    }
}