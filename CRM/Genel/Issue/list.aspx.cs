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
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxCallback;
using DevExpress.Web.ASPxEditors;
using System.Collections.Generic;

public partial class CRM_Genel_Issue_list : System.Web.UI.Page
{
    CrmUtils data = new CrmUtils();

    DataTable Firmas = new DataTable();

    protected override object LoadPageStateFromPersistenceMedium()
    {
        return PageUtils.LoadPageStateFromPersistenceMedium(this.Context, this.Page);
    }

    protected override void SavePageStateToPersistenceMedium(object viewState)
    {
        PageUtils.SavePageStateToPersistenceMedium(this.Context, this.Page, viewState);
    }

    protected void grid_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
    {
        if (e.CallbackName == "APPLYCOLUMNFILTER" || e.CallbackName == "APPLYFILTER")
        {
            ((ASPxGridView)sender).ExpandAll();
        }
        //grid.ExpandAll();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Security.CheckPermission(this.Context, "Bildirim Ara", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }
        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        menu.ItemClick += new MenuItemEventHandler(MenuFirma_ItemClick);
        this.UserName.Value = Membership.GetUser().UserName;
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }
        Response.AppendHeader("Pragma", "no-cache");
        Response.AppendHeader("Cache-Control", "no-cache");

        Response.CacheControl = "no-cache";
        Response.Expires = -1;
        fillcomboxes();
        InitGridTable(this.DataTableList.Table);
        #region Admin Yetkilendirme
        if (Membership.GetUser().UserName.ToLower() == "admin" || Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator")
            || Roles.IsUserInRole(Membership.GetUser().UserName, "Merkez Yöneticileri"))
        {

        }
        else
        {
            lblKisiFiltrele.Visible = false;
            lblKisiFiltrele2.Visible = false;
            UzerineAtanan.Visible = false;
            BildirimiGiren.Visible = false;
            lblProjeSinif.Visible = false;
            ProjeSinifID.Visible = false;
        }
        #endregion
        //if ((String)Request.QueryString["id"] == null || (String)Request.QueryString["id"].ToString().Trim() == "") return;

        //this.IssueID.Value = (String)Request.QueryString["id"];
        //LoadDocument(this.ProjeID.Value.ToString());



    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("IssueID", typeof(Guid));
        dt.Columns.Add("MainIssueID", typeof(int));
        dt.Columns.Add("IsMain", typeof(int));
        dt.Columns.Add("RelatedPop3Id", typeof(int));
        dt.Columns.Add("IndexID", typeof(int));
        dt.Columns.Add("Baslik", typeof(string));
        dt.Columns.Add("YaziRengi", typeof(string));
        dt.Columns.Add("MainBaslik", typeof(string));
        dt.Columns.Add("FirmaID", typeof(int));
        dt.Columns.Add("HataTipID", typeof(int));
        dt.Columns.Add("VirusSinifID", typeof(int));
        dt.Columns.Add("FirmaName", typeof(string));
        dt.Columns.Add("HarcananSure", typeof(decimal));
        dt.Columns.Add("HarcananSure2", typeof(decimal));
        dt.Columns.Add("DurumName", typeof(string));
        dt.Columns.Add("Asilama", typeof(string));
        dt.Columns.Add("ProjeID", typeof(int));
        dt.Columns.Add("ProjeName", typeof(string));
        dt.Columns.Add("DurumID", typeof(int));
        dt.Columns.Add("Description", typeof(string));
        dt.Columns.Add("SonYorum", typeof(string));
        dt.Columns.Add("OnemDereceID", typeof(int));
        dt.Columns.Add("OnemDereceName", typeof(string));
        dt.Columns.Add("ReelOperationDate", typeof(DateTime));
        dt.Columns.Add("UserID", typeof(int));
        dt.Columns.Add("UserName", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("KeyWords", typeof(string));
        dt.Columns.Add("BildirimTarihi", typeof(DateTime));
        dt.Columns.Add("TeslimTarihi", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument()
    {
        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        SqlCommand cmd = DB.SQL(this.Context, "SELECT IndexId FROM SecurityUsers WHERE UserName=@UserName");
        DB.AddParam(cmd, "@UserName", 100, Membership.GetUser().UserName);
        int UserId = (int)cmd.ExecuteScalar();
        HiddenID.Value = UserId.ToString();
        sb = new StringBuilder();
        sb.Append("SELECT I.*,Datediff(d,I.BildirimTarihi,I.TeslimTarihi) As HarcananSure,");
        sb.Append("Datediff(d,I.BildirimTarihi,IsNull(I.ReelOperationdate,I.TeslimTarihi)) As HarcananSure2,");
        sb.Append("F.FirmaName,P.Adi AS ProjeName,D.Adi AS DurumName,O.Adi AS OnemDereceName,U.UserName");
        sb.Append(",'' as SonYorum ");
        sb.Append(",Case When I.MainIssueID Is Not Null Then t2.Baslik ");
        sb.Append("Else Null End As MainBaslik");
        sb.Append(",Case When I.MainIssueID Is Not Null Then t2.IndexID ");
        sb.Append("Else I.IndexID End As PNR");
        sb.Append(",YaziRengi=(SELECT Top 1 t10.Kodu FROM SecurityRoles t3 LEFT OUTER JOIN SecurityUserRoles t4 ON t4.RoleID=t3.RoleID ");
        sb.Append("LEFT OUTER JOIN SecurityUsers t6 ON t4.UserID=t6.UserID LEFT JOIN YaziRenkleri t10 ON t3.YaziRenkleriID=t10.YaziRenkleriID ");
        sb.Append("Where t6.UserName=I.CreatedBy) ");
        sb.Append("FROM Issue AS I WITH (NOLOCK) LEFT OUTER JOIN  Firma AS F ON  I.FirmaID=F.IndexId ");
        sb.Append("Left Outer Join Issue t2 On I.MainIssueID=t2.IndexId ");
        sb.Append("LEFT OUTER JOIN Proje AS P ON I.ProjeID=P.IndexId ");
        sb.Append("LEFT OUTER JOIN OnemDereceleri AS O ON I.OnemDereceID=O.IndexId ");
        sb.Append("LEFT OUTER JOIN VirusSinif Vrs ON I.VirusSinifId=Vrs.IndexId ");
        sb.Append("LEFT OUTER JOIN Durum AS D ON I.DurumID=D.IndexId ");
        sb.Append("LEFT OUTER JOIN SecurityUsers AS U ON I.UserID=U.IndexId WHERE I.Active='1' ");

        sb2 = new StringBuilder();
        sb2.Append("SELECT I.*,Datediff(d,I.BildirimTarihi,I.TeslimTarihi) As HarcananSure,");
        sb2.Append("Datediff(d,I.BildirimTarihi,IsNull(I.ReelOperationdate,I.TeslimTarihi)) As HarcananSure2,");
        sb2.Append("F.FirmaName,P.Adi AS ProjeName,D.Adi AS DurumName,O.Adi AS OnemDereceName,U.UserName");
        sb2.Append(",'' as SonYorum ");
        sb2.Append(",Case When I.MainIssueID Is Not Null Then t2.Baslik ");
        sb2.Append("Else Null End As MainBaslik");
        sb2.Append(",Case When I.MainIssueID Is Not Null Then t2.IndexID ");
        sb2.Append("Else I.IndexID End As PNR");
        sb2.Append(",YaziRengi=(SELECT Top 1 t10.Kodu FROM SecurityRoles t3 LEFT OUTER JOIN SecurityUserRoles t4 ON t4.RoleID=t3.RoleID ");
        sb2.Append("LEFT OUTER JOIN SecurityUsers t6 ON t4.UserID=t6.UserID LEFT JOIN YaziRenkleri t10 ON t3.YaziRenkleriID=t10.YaziRenkleriID ");
        sb2.Append("Where t6.UserName=I.CreatedBy) ");
        sb2.Append("FROM ClosedIssue AS I WITH (NOLOCK) LEFT OUTER JOIN  Firma AS F ON  I.FirmaID=F.IndexId ");
        sb2.Append("Left Outer Join ClosedIssue t2 On I.MainIssueID=t2.IndexId ");
        sb2.Append("LEFT OUTER JOIN Proje AS P ON I.ProjeID=P.IndexId ");
        sb2.Append("LEFT OUTER JOIN OnemDereceleri AS O ON I.OnemDereceID=O.IndexId ");
        sb2.Append("LEFT OUTER JOIN VirusSinif Vrs ON I.VirusSinifId=Vrs.IndexId ");
        sb2.Append("LEFT OUTER JOIN Durum AS D ON I.DurumID=D.IndexId ");
        sb2.Append("LEFT OUTER JOIN SecurityUsers AS U ON I.UserID=U.IndexId WHERE I.Active='1' ");
        if (Roles.IsUserInRole(Membership.GetUser().UserName, "Maðaza Koordinatör") && !Roles.IsUserInRole(Membership.GetUser().UserName, "Merkez Yöneticileri"))
        {
            if (!this.Atayan.Checked && !this.Atanan.Checked)
            {
                sb.Append(" AND (P.IsShop=1 or Exists (Select 1 From UserAllowedIssueList WITH (NOLOCK) Where IssueID=I.IndexId And UserID=@UserID))");

                sb2.Append(" AND (P.IsShop=1 or Exists (Select 1 From UserAllowedIssueList WITH (NOLOCK) Where IssueID=I.IndexId And UserID=@UserID))");
            }
        }
        else
        {
            if ((Membership.GetUser().UserName.ToLower() == "admin") || (Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator"))
                || (Roles.IsUserInRole(Membership.GetUser().UserName, "Merkez Yöneticileri")))
            {

            }
            else
            {
                if (!this.Atayan.Checked && !this.Atanan.Checked)
                {
                    sb.Append(" And Exists (Select 1 From UserAllowedIssueList WITH (NOLOCK) Where IssueID=I.IndexId And UserID=@UserID) ");

                    sb2.Append(" And Exists (Select 1 From UserAllowedClosedIssueList WITH (NOLOCK) Where IssueID=I.IndexId And UserID=@UserID) ");
                }
            }

        }


        //Filtreleme alanlarý
        #region IssueID Filtreleniyor...
        if (!String.IsNullOrEmpty(this.IssueID.Text))
        {
            sb.Append(" AND I.IndexID=@IndexID");
            sb2.Append(" AND I.IndexID=@IndexID");
        }
        #endregion
        #region Üzerine Atanan veya kiþinin atadýðý Filtreleniyor...
        if (Membership.GetUser().UserName.ToLower() == "admin" || Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator")
            || Roles.IsUserInRole(Membership.GetUser().UserName, "Merkez Yöneticileri"))
        {
            if (!String.IsNullOrEmpty(this.UzerineAtanan.Text) && !String.IsNullOrEmpty(this.BildirimiGiren.Text))
            {
                sb.Append(" AND (Exists(Select 1 From UserAllowedIssueList Where IssueID=I.IndexId And UserID='" + this.UzerineAtanan.Value.ToString() + "')");
                sb.Append(" OR Exists(Select 1 From UserAllowedIssueList t1 LEFT OUTER JOIN SecurityUsers t2 ON t1.UserID=t2.IndexId Where t1.IssueID=I.IndexId And t2.UserName='" + this.BildirimiGiren.Value.ToString() + "'))");

                sb2.Append(" AND (Exists(Select 1 From UserAllowedClosedIssueList Where IssueID=I.IndexId And UserID='" + this.UzerineAtanan.Value.ToString() + "')");
                sb2.Append(" OR Exists(Select 1 From UserAllowedClosedIssueList t1 LEFT OUTER JOIN SecurityUsers t2 ON t1.UserID=t2.IndexId Where t1.IssueID=I.IndexId And t2.UserName='" + this.BildirimiGiren.Value.ToString() + "'))");
            }
            else if (!String.IsNullOrEmpty(this.UzerineAtanan.Text) && String.IsNullOrEmpty(this.BildirimiGiren.Text))
            {
                if (!this.Atayan.Checked && !this.Atanan.Checked)
                {
                    sb.Append(" AND Exists(Select 1 From UserAllowedIssueList Where IssueID=I.IndexId And UserID='" + this.UzerineAtanan.Value.ToString() + "')");

                    sb2.Append(" AND Exists(Select 1 From UserAllowedClosedIssueList Where IssueID=I.IndexId And UserID='" + this.UzerineAtanan.Value.ToString() + "')");
                }
            }
            else if (!String.IsNullOrEmpty(this.BildirimiGiren.Text) && String.IsNullOrEmpty(this.UzerineAtanan.Text))
            {
                if (!this.Atayan.Checked && !this.Atanan.Checked)
                {
                    sb.Append(" AND Exists(Select 1 From UserAllowedIssueList t1 LEFT OUTER JOIN SecurityUsers t2 ON t1.UserID=t2.IndexId Where t1.IssueID=I.IndexId And t2.UserName='" + this.BildirimiGiren.Value.ToString() + "')");

                    sb2.Append(" AND Exists(Select 1 From UserAllowedClosedIssueList t1 LEFT OUTER JOIN SecurityUsers t2 ON t1.UserID=t2.IndexId Where t1.IssueID=I.IndexId And t2.UserName='" + this.BildirimiGiren.Value.ToString() + "')");
                }
            }

        }
        #endregion
        #region Firma Filtreniyor...
        //if (Membership.GetUser().UserName.ToLower() == "admin" || Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator")
        //    || Roles.IsUserInRole(Membership.GetUser().UserName, "Merkez Yöneticileri"))
        //{
        if (!String.IsNullOrEmpty(this.FirmaID.Text))
        {
            sb.Append(" AND I.FirmaID='" + FirmaID.Value.ToString() + "' ");

            sb2.Append(" AND I.FirmaID='" + FirmaID.Value.ToString() + "' ");
        }
        //}
        //else
        //{
        //    if (String.IsNullOrEmpty(this.FirmaID.Text))
        //    {
        //        bool ilk = true;
        //        if (FirmaID.Items.Count != 0)
        //        {
        //            foreach (DevExpress.Web.ASPxEditors.ListEditItem item in FirmaID.Items)
        //            {
        //                if (ilk)
        //                {
        //                    sb.Append(" AND CONVERT(VarChar(50),I.FirmaID) IN('" + item.Value.ToString() + "'");
        //                    ilk = false;
        //                }
        //                else
        //                    sb.Append(",'" + item.Value.ToString() + "'");
        //            }
        //            if (!ilk)
        //                sb.Append(")");
        //        }
        //        else
        //        {
        //            CrmUtils.CreateMessageAlert(this.Page, "Bildirim görüntülemek için gerekli firma izinleriniz yok görünüyor. lütfen Model bünyesindeki admin ile temasa geçiniz!", "stkey1");
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        sb.Append(" AND CONVERT(VarChar(50),I.FirmaID)='" + FirmaID.Value.ToString() + "' ");
        //    }
        //}
        #endregion
        #region Proje Filtreleniyor...
        //if (Membership.GetUser().UserName.ToLower() == "admin" || Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator")
        //    || Roles.IsUserInRole(Membership.GetUser().UserName, "Merkez Yöneticileri"))
        //{
        if (this.ProjeID.Text != " " && this.ProjeID.Text != "")
        {
            sb.Append(" AND P.Adi='" + this.ProjeID.Text + "' ");
            sb2.Append(" AND P.Adi='" + this.ProjeID.Text + "' ");
        }
        //}
        //else
        //{
        //    if (this.ProjeID.Text != " " && this.ProjeID.Text != "")
        //        sb.Append(" AND P.Adi='" + this.ProjeID.Text + "' ");
        //    else
        //    {
        //        bool ilk = true;
        //        DataTable dt = GetProjeList();
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                if (ilk)
        //                {
        //                    sb.Append(" AND CONVERT(VarChar(50),I.ProjeID) IN('" + row["ProjeID"].ToString() + "'");
        //                    ilk = false;
        //                }
        //                else
        //                    sb.Append(",'" + row["ProjeID"].ToString() + "'");
        //            }
        //            if (!ilk)
        //                sb.Append(")");
        //        }
        //        else
        //        {
        //            CrmUtils.CreateMessageAlert(this.Page, "Bildirim görüntülemek için gerekli Proje izinleriniz yok görünüyor. lütfen Model bünyesindeki admin ile temasa geçiniz!", "stkey1");
        //            return;
        //        }

        //    }
        //}
        #endregion
        #region Proje Sýnýf Filtreleniyor...
        bool ilkk2 = true;
        foreach (ListItem ite in ProjeSinifID.Items)
        {
            if (ite.Selected)
            {
                if (ilkk2)
                {
                    sb.Append(" AND CONVERT(VarChar(50),P.ProjeSinifID) IN('" + ite.Value.ToString() + "'");
                    sb2.Append(" AND CONVERT(VarChar(50),P.ProjeSinifID) IN('" + ite.Value.ToString() + "'");
                    ilkk2 = false;
                }
                else
                {
                    sb.Append(",'" + ite.Value.ToString() + "'");
                    sb2.Append(",'" + ite.Value.ToString() + "'");
                }
            }
        }
        if (!ilkk2)
        {
            sb.Append(")");
            sb2.Append(")");
        }
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
                    sb2.Append(" AND I.DurumID IN('" + ite.Value.ToString() + "'");
                    ilkk = false;
                }
                else
                {
                    sb.Append(",'" + ite.Value.ToString() + "'");
                    sb2.Append(",'" + ite.Value.ToString() + "'");
                }
            }
        }
        if (!ilkk)
        {
            sb.Append(")");
            sb2.Append(")");
        }
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
                    sb2.Append(" AND I.HataTipID IN('" + ite.Value.ToString() + "'");
                    ilkk = false;
                }
                else
                {
                    sb.Append(",'" + ite.Value.ToString() + "'");
                    sb2.Append(",'" + ite.Value.ToString() + "'");
                }
            }
        }
        if (!ilkk)
        {
            sb.Append(")");
            sb2.Append(")");
        }
        #endregion
        #region Virüs Sýnýflarý Filtreleniyor...
        ilkk = true;
        foreach (ListItem ite in chcVirusSiniflari.Items)
        {
            if (ite.Selected)
            {
                if (ilkk)
                {
                    sb.Append(" AND I.VirusSinifID IN('" + ite.Value.ToString() + "'");
                    sb2.Append(" AND I.VirusSinifID IN('" + ite.Value.ToString() + "'");
                    ilkk = false;
                }
                else
                {
                    sb.Append(",'" + ite.Value.ToString() + "'");
                    sb2.Append(",'" + ite.Value.ToString() + "'");
                }
            }
        }
        if (!ilkk)
        {
            sb.Append(")");
            sb2.Append(")");
        }
        #endregion
        #region Atadýklarým Filtreleniyor...
        if (Atayan.Checked)
        {
            sb.Append(" AND I.CreatedBy='" + Membership.GetUser().UserName.ToLower() + "'");
            sb2.Append(" AND I.CreatedBy='" + Membership.GetUser().UserName.ToLower() + "'");
        }
        #endregion
        #region Atananlar Filtreleniyor...
        if (Atanan.Checked)
        {
            sb.Append(" AND EXISTS (Select 1 from UserAllowedIssueList Where IssueId=I.IndexId and UserId='" + HiddenID.Value.ToString() + "' and IssueAntivirus=0)");

            sb2.Append(" AND EXISTS (Select 1 from UserAllowedClosedIssueList Where IssueId=I.IndexId and UserId='" + HiddenID.Value.ToString() + "' and IssueAntivirus=0)");
        }
        #endregion
        #region Anahtar Kelime Filtreleniyor...
        if (KeyWords.Value != null)
        {
            if ((Membership.GetUser().UserName.ToLower() == "mp1") || (Membership.GetUser().UserName.ToLower() == "th")
                || (Roles.IsUserInRole("Administrator")))
            {
                sb.Append(" AND DATEDIFF(day,getdate(),I.TeslimTarihi)<0");
            }
            else
            {
                sb.Append(" AND I.Keywords like '%" + this.KeyWords.Value.ToString() + "%'");
                sb2.Append(" AND I.Keywords like '%" + this.KeyWords.Value.ToString() + "%'");
            }
        }
        #endregion
        #region Virüs Tanýsý Filtreleniyor...
        if (TxtBaslik.Value != null)
        {
            sb.Append(" AND I.Baslik like '%" + this.TxtBaslik.Value.ToString() + "%'");
            sb2.Append(" AND I.Baslik like '%" + this.TxtBaslik.Value.ToString() + "%'");
        }
        #endregion
        #region Tarih Filtreleniyor...
        if (!CrmUtils.ControllToDate(this.Page, StartDate.Date.ToString()) &&
        !CrmUtils.ControllToDate(this.Page, EndDate.Date.ToString()))
        {
            sb.Append(" AND I.BildirimTarihi>=@StartDate");
            sb.Append(" AND I.BildirimTarihi<DateAdd(hour,24,@EndDate)");

            sb2.Append(" AND I.BildirimTarihi>=@StartDate");
            sb2.Append(" AND I.BildirimTarihi<DateAdd(hour,24,@EndDate)");
        }
        #endregion
        #region Alt gündem listeleme durumu kontrol ediliyor...
        if (ChkNotListSubVirus.Checked)
        {
            sb.Append(" AND I.IsAltGundem=0");
            sb.Append(" AND I.IsAltGundem=0");
        }
        #endregion
        //sb.Append(" ORDER BY I.IndexId DESC");
        sb2.Append(" ORDER BY I.IndexId DESC");
        sb.AppendLine(" UNION ALL ");
        //CacheLog(sb.ToString() + sb2.ToString());

        cmd = DB.SQL(this.Context, sb.ToString() + sb2.ToString());

        #region Parameters1
        if (!String.IsNullOrEmpty(this.IssueID.Text))
            DB.AddParam(cmd, "@IndexID", int.Parse(this.IssueID.Text));
        if (!CrmUtils.ControllToDate(this.Page, StartDate.Date.ToString()) &&
        !CrmUtils.ControllToDate(this.Page, EndDate.Date.ToString()))
        {
            DB.AddParam(cmd, "@StartDate", this.StartDate.Date);
            DB.AddParam(cmd, "@EndDate", this.EndDate.Date);
        }
        //if (KeyWords.Value != null)
        //    DB.AddParam(cmd, "@KeyWords", 255, this.KeyWords.Value);
        DB.AddParam(cmd, "@UserID", UserId);
        DB.AddParam(cmd, "@CreatedBy", 100, Membership.GetUser().UserName);
        #endregion

        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();

        #region FillGrid
        this.DataTableList.Table.Rows.Clear();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();
            row["ID"] = rdr["IssueID"];
            row["IssueID"] = rdr["IssueID"];
            row["MainIssueID"] = rdr["MainIssueID"];
            row["IndexID"] = rdr["IndexID"];
            row["IsMain"] = rdr["IsMain"];
            if (rdr["YaziRengi"] != null && rdr["YaziRengi"].ToString() != "")
                row["YaziRengi"] = rdr["YaziRengi"];
            else
                row["YaziRengi"] = "#000000";
            row["Baslik"] = rdr["Baslik"];
            row["MainBaslik"] = rdr["MainBaslik"];
            row["FirmaID"] = rdr["FirmaID"];
            row["FirmaName"] = rdr["FirmaName"];
            row["Description"] = rdr["LastComment"];
            row["HarcananSure"] = rdr["HarcananSure"];
            row["HarcananSure2"] = rdr["HarcananSure2"];
            if (rdr["AsilamaYapildi"] != null && (rdr["AsilamaYapildi"].ToString() == "1" || rdr["AsilamaYapildi"].ToString() == "True"))
                row["Asilama"] = "Yapýldý";
            else
                row["Asilama"] = "";
            row["SonYorum"] = rdr["SonYorum"];
            row["ProjeID"] = rdr["ProjeID"];
            row["ProjeName"] = rdr["ProjeName"];
            row["RelatedPop3Id"] = rdr["RelatedPop3Id"];
            row["DurumID"] = rdr["DurumID"];
            row["DurumName"] = rdr["DurumName"];
            row["OnemDereceID"] = rdr["OnemDereceID"];
            row["HataTipID"] = rdr["HataTipID"];
            row["VirusSinifID"] = rdr["VirusSinifID"];
            row["OnemDereceName"] = rdr["OnemDereceName"];
            row["TeslimTarihi"] = rdr["TeslimTarihi"];
            row["ReelOperationDate"] = rdr["ReelOperationDate"];
            row["UserID"] = rdr["UserID"];
            row["UserName"] = rdr["UserName"];
            row["CreatedBy"] = rdr["CreatedBy"].ToString().ToUpper();
            row["BildirimTarihi"] = rdr["BildirimTarihi"];
            row["KeyWords"] = rdr["KeyWords"];
            this.DataTableList.Table.Rows.Add(row);

        }
        this.DataTableList.Table.AcceptChanges();
        rdr.Close();
        grid.DataBind();
        grid.ExpandAll();

        if (this.DataTableList != null)
            this.DataTableList.Dispose();
        if (cmd != null)
            cmd.Dispose();
        if (rdr != null)
            rdr.Dispose();
        if (sb != null)
            sb = null;
        if (sb2 != null)
            sb2 = null;
        #endregion

    }

    private bool CacheLog(string value)
    {
        try
        {
            string FilePath = @Server.MapPath("~") + "\\" + "script.txt";
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(FilePath, true))
            {
                writer.WriteLine(value);
                writer.Close();
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
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

    private void MenuFirma_ItemClick(object source, MenuItemEventArgs e)
    {

        if (e.Item.Name.Equals("excel"))
        {
            grid.Columns["Description"].Visible = true;
            CrmUtils.ExportToxls(gridExport, "grid", true);
            grid.Columns["Description"].Visible = false;
        }
        else if (e.Item.Name.Equals("pdf"))
        {
            grid.Columns["Description"].Visible = true;
            CrmUtils.ExportTopdf(gridExport, "grid", true);
            grid.Columns["Description"].Visible = false;
        }
        else if (e.Item.Name.Equals("meeting"))
        {
            PrepareMeetingIssue();
        }
        else if (e.Item.Name.Equals("List"))
        {
            //LoadDocument();
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters != "x")
        {
            if (Convert.ToBoolean(e.Parameters.ToString()))
                this.grid.Selection.SelectAll();
            else
                this.grid.Selection.UnselectAll();
        }
        else
            LoadDocument();
        //GC.Collect();
        //
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

    protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType != GridViewRowType.Data) return;

        object Durum = (object)this.grid.GetRowValues(e.VisibleIndex, "DurumID");
        object YaziRengi = (object)this.grid.GetRowValues(e.VisibleIndex, "YaziRengi");

        if (Durum.ToString() == "9")
        {
            e.Row.ForeColor = System.Drawing.Color.FromName("#ff0000");
            //((ASPxHyperLink)grid.FindRowTemplateControl(e.VisibleIndex, "IssueLink")).ForeColor = System.Drawing.Color.FromName("#ff0000");
        }
        else
        {
            if (YaziRengi != null && YaziRengi.ToString() != "")
                e.Row.ForeColor = System.Drawing.Color.FromName(YaziRengi.ToString());
        }

        //if (Durum.ToString() == "Gol")
        //    e.Row.BackColor = System.Drawing.Color.FromName("#99ff66");
        //else if (Durum.ToString() == "VÝRÜS")
        //    e.Row.BackColor = System.Drawing.Color.FromName("#ffff99");
        //else if (Durum.ToString() == "SAVSAKLAMA")
        //    e.Row.BackColor = System.Drawing.Color.FromName("#ff6600");

    }

    protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "MainBaslik" || e.DataColumn.FieldName == "Baslik")
        {
            object Durum = (object)this.grid.GetRowValues(e.VisibleIndex, "DurumID");
            object YaziRengi = (object)this.grid.GetRowValues(e.VisibleIndex, "YaziRengi");
            ASPxHyperLink link1 = ((ASPxHyperLink)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "IssueLink"));

            if (Durum.ToString() == "9")
            {
                link1.ForeColor = System.Drawing.Color.FromName("#ff0000");
            }
            else
            {
                link1.ForeColor = System.Drawing.Color.FromName(YaziRengi.ToString());
            }

            e.Cell.ToolTip = string.Format("{0}", e.CellValue);
        }
        if (e.DataColumn.FieldName == "RelatedPop3Id")
        {

            object Pop3Id = (object)this.grid.GetRowValues(e.VisibleIndex, "RelatedPop3Id");
            ASPxHyperLink soundimage = this.grid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "img_Sound") as ASPxHyperLink;
            if (Pop3Id.ToString() == null || Pop3Id.ToString() == "")
            {
                soundimage.Visible = false;
            }
            else
            {
                //string root = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "CRM/Pop3MailAttachments/" + Pop3Id;
                //if (!System.IO.Directory.Exists(root))
                //{
                //    soundimage.Visible = false;
                //}
            }
        }
    }

    protected void CallbackPreview_Callback(object source, CallbackEventArgs e)
    {
        if (e.Parameter == null || e.Parameter == "")
        {
            e.Result = null;
        }
        else
            e.Result = e.Parameter;
    }

    private DataTable GetProjeList()
    {
        DataTable dt = new DataTable();
        try
        {

            dt.Columns.Add("ProjeID", typeof(int));
            dt.Columns.Add("Adi", typeof(string));

            StringBuilder sb = new StringBuilder();
            sb.Append("EXEC AllowedProjeList @UserName");

            SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@UserName", 100, Membership.GetUser().UserName);
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DataRow row = dt.NewRow();
                row["ProjeID"] = rdr["ProjeId"];
                row["Adi"] = rdr["Adi"];
                dt.Rows.Add(row);
            }
            rdr.Close();
            dt.AcceptChanges();
            return dt;


        }
        catch
        {
            dt = new DataTable();
            return dt;

        }
    }

    private void PrepareMeetingIssue()
    {
        try
        {
            grid.UpdateEdit();
            List<object> keyValues = this.grid.GetSelectedFieldValues("ID");
            string Ids = string.Empty;
            bool ilk = true;
            foreach (object key in keyValues)
            {
                DataRow row2 = DataTableList.Table.Rows.Find(key);
                if (ilk)
                {
                    Ids += row2["IndexID"].ToString();
                    ilk = false;
                }
                else
                    Ids += "," + row2["IndexID"].ToString();
            }

            if (Ids.Length > 0)
            {
                string scrpt = "var option = \"resizable=0,top=100,left=100,width=800,height=650,scrollbars=1\";";
                scrpt += "  var PopupWin = window.open('./AddIssuePopUp.aspx?Ids=" + Ids + "','toplantigundem_" + DateTime.Now.Millisecond.ToString() + "',option);PopupWin.focus();";
                Response.Write("<script language='Javascript'>{ " + scrpt + " }</script>");
            }
        }
        catch
        {

        }
    }
}
