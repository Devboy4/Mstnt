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
using DevExpress.Web.ASPxClasses;
using System.Collections.Generic;

public partial class CRM_Genel_Issue_SaveAndNew : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "Bildirim Ekle", "Insert"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        this.menu.ItemClick += new MenuItemEventHandler(MenuFirma_ItemClick);

        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }
        InitGridDTRelatedUsers(this.DTRelatedUsers.Table);
        fillcomboxes();
        this.HiddenID.Value = GetUserID(Membership.GetUser().UserName).ToString();
        ControlDegerProject();

        this.TeslimTarihi.Date = DateTime.Now;

    }

    private Guid GetUserID(string UserID)
    {
        Guid id;
        try
        {
            SqlCommand cmd = DB.SQL(this.Context, "Select UserID From SecurityUsers Where UserName=@UserName");
            DB.AddParam(cmd, "@UserName", 100, UserID);
            cmd.Prepare();
            id = (Guid)cmd.ExecuteScalar();

        }
        catch
        {
            id = Guid.Empty;
        }
        return id;
    }

    void fillcomboxes()
    {
        data.BindComboBoxesNoEmpty(this.Context, FirmaID, "EXEC FirmaListByUserName '" + Membership.GetUser().UserName + "'", "FirmaID", "FirmaName");
        data.BindComboBoxesNoEmpty(this.Context, DurumID, "Select DurumID,Adi From Durum Where IlkGetir=1", "DurumID", "Adi");
        data.BindComboBoxesNoEmpty(this.Context, OnemDereceID, "SELECT OnemDereceID,Adi FROM OnemDereceleri ORDER BY Sira", "OnemDereceID", "Adi");
        DurumID.SelectedIndex = 1;
    }

    protected void ProjeID_Callback(object source, CallbackEventArgsBase e)
    {
        if (e.Parameter.ToString() == null || e.Parameter.ToString() == "")
            return;
        StringBuilder sb = new StringBuilder();
        sb.Append("EXEC AllowedProjeListV2 '" + Membership.GetUser().UserName.ToLower() + "','" + e.Parameter.ToString() + "'");
        data.BindComboBoxesNoEmpty(this.Context, ProjeID, sb.ToString(), "ProjeID", "Adi");
    }

    private void MenuFirma_ItemClick(object source, MenuItemEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        #region save
        if (e.Item.Name.Equals("save"))
        {
            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlýþ bilgi giriþi!", "stkey1");
                return;
            }
            Guid id = SaveDocument();
            if (id != Guid.Empty)
            {
                sb = new StringBuilder();
                //if (GundemDosyaYolu.Value != null)
                //{
                //    sb.Append("location.href='./SaveComplate.aspx?id=");
                //    sb.Append(id.ToString());
                //    sb.Append("&SaveText=");
                //    sb.Append(this.Description.Text);
                //    sb.Append("&DosyaYolu=");
                //    sb.Append(this.GundemDosyaYolu.Value.ToString());
                //    sb.Append("&DosyaAdi=");
                //    if (this.GundemDosyaAdi.Value != null)
                //        sb.Append(this.GundemDosyaAdi.Value.ToString());
                //    else
                //        sb.Append("Dosya için Týklayýnýz...");
                //    sb.Append("'");

                //}
                //else
                //{
                sb.Append("location.href='./SaveComplate.aspx?id=");
                sb.Append(id.ToString());
                sb.Append("&SaveText=");
                sb.Append(this.Description.Text);
                sb.Append("'");
                //}
                CrmUtils.CreateJavaScript(this.Page, "stkey1", sb.ToString());
            }

        }
        #endregion
        #region savenew
        else if (e.Item.Name.Equals("savenew"))
        {
            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlýþ bilgi giriþi!", "stkey1");
                return;
            }
            Guid id = SaveDocument();
            if (id != Guid.Empty)
            {
                sb = new StringBuilder();
                //if (GundemDosyaYolu.Value != null)
                //{
                //    sb.Append("location.href='./SaveAndNew.aspx?id=");
                //    sb.Append(id.ToString());
                //    sb.Append("&SaveText=");
                //    sb.Append(this.Description.Text);
                //    sb.Append("&DosyaYolu=");
                //    sb.Append(this.GundemDosyaYolu.Value.ToString());
                //    sb.Append("&DosyaAdi=");
                //    if (this.GundemDosyaAdi.Value != null)
                //        sb.Append(this.GundemDosyaAdi.Value.ToString());
                //    else
                //        sb.Append("Dosya için Týklayýnýz...");
                //    sb.Append("'");

                //}
                //else
                //{
                sb.Append("location.href='./SaveAndNew.aspx?id=");
                sb.Append(id.ToString());
                sb.Append("&SaveText=");
                sb.Append(this.Description.Text);
                sb.Append("'");
                //}
                CrmUtils.CreateJavaScript(this.Page, "stkey1", sb.ToString());
            }


        }
        #endregion
        #region saveopen
        else if (e.Item.Name.Equals("saveopen"))
        {
            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlýþ bilgi giriþi!", "stkey1");
                return;
            }
            Guid id = SaveDocument();
            if (id != Guid.Empty)
            {
                sb = new StringBuilder();
                //if (GundemDosyaYolu.Value != null)
                //{
                //    sb.Append("location.href='./SaveComplate.aspx?id=");
                //    sb.Append(id.ToString());
                //    sb.Append("&SaveText=");
                //    sb.Append(this.Description.Text);
                //    sb.Append("&DosyaYolu=");
                //    sb.Append(this.GundemDosyaYolu.Value.ToString());
                //    sb.Append("&DosyaAdi=");
                //    if (this.GundemDosyaAdi.Value != null)
                //        sb.Append(this.GundemDosyaAdi.Value.ToString());
                //    else
                //        sb.Append("Dosya için Týklayýnýz...");
                //    sb.Append("';");

                //}
                //else
                //{
                    sb.Append("location.href='./SaveComplate.aspx?id=");
                    sb.Append(id.ToString());
                    sb.Append("&SaveText=");
                    sb.Append(this.Description.Text);
                    sb.Append("';");
                //}
                CrmUtils.CreateJavaScript(this.Page, "stkey1", sb.ToString() +
                    "JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id=" + id.ToString() + "&NoteOwner=1',850,650);PopWin.focus();");

            }


        }
        #endregion
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }

    }

    private Guid SaveDocument()
    {

        try
        {
            DB.BeginTrans(this.Context);
            Guid id = Guid.NewGuid();
            StringBuilder sb = new StringBuilder();
            sb = new StringBuilder();
            sb.Append("EXEC OnAddIssue ");
            sb.Append("@IssueID,@FirmaID,@ProjeID,@UserID,@DurumID,@OnemDereceID,@Active,@Baslik,@Description,@HataAdimlari,@KeyWords");
            sb.Append(",@BildirimTarihi,@CreatedBy,@ModifiedBy,@ModificationDate,@CreationDate,@TeslimTarihi,@GundemDosyaYolu,@GundemDosyaAdi");

            SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@IssueID", id);
            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
            DB.AddParam(cmd, "@ModifiedBy", 50, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@CreatedBy", 50, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@Active", 1);
            DB.AddParam(cmd, "@Baslik", 255, this.Description.Text.ToUpper());
            DB.AddParam(cmd, "@Description", SqlDbType.Int);
            DB.AddParam(cmd, "@HataAdimlari", SqlDbType.Int);
            #region Gündem Dosyalarý
            //if (GundemDosyaYolu.Value != null)
            //    DB.AddParam(cmd, "@GundemDosyaYolu", 255, GundemDosyaYolu.Value);
            //else
            DB.AddParam(cmd, "@GundemDosyaYolu", SqlDbType.Int);
            //if (GundemDosyaAdi.Value != null)
            //    DB.AddParam(cmd, "@GundemDosyaAdi", 100, GundemDosyaAdi.Value);
            //else
            DB.AddParam(cmd, "@GundemDosyaAdi", SqlDbType.Int);
            #endregion
            if (!CrmUtils.ControllToDate(this.Page, this.TeslimTarihi.Date.ToString()))
            {
                DB.AddParam(cmd, "@TeslimTarihi", this.TeslimTarihi.Date);
            }
            else
            {
                if (this.OperasyonSuresi.Value.ToString() != "0")
                {
                    DateTime date = DateTime.Now.AddDays(int.Parse(this.OperasyonSuresi.Value.ToString()));
                    DB.AddParam(cmd, "@TeslimTarihi", date);
                }
                else
                    DB.AddParam(cmd, "@TeslimTarihi", DateTime.Now);
            }
            DB.AddParam(cmd, "@BildirimTarihi", DateTime.Now);
            Guid tmpID;
            tmpID = new Guid(this.FirmaID.Value.ToString());
            DB.AddParam(cmd, "@FirmaID", tmpID);
            tmpID = new Guid(this.ProjeID.Value.ToString());
            DB.AddParam(cmd, "@ProjeID", tmpID);
            tmpID = new Guid(this.DurumID.Value.ToString());
            DB.AddParam(cmd, "@DurumID", tmpID);
            tmpID = new Guid(this.UserID.Value.ToString());
            DB.AddParam(cmd, "@UserID", tmpID);
            DB.AddParam(cmd, "@KeyWords", SqlDbType.Int);
            tmpID = new Guid(this.OnemDereceID.Value.ToString());
            DB.AddParam(cmd, "@OnemDereceID", tmpID);
            object MailConfigs = (object)cmd.ExecuteScalar();
            char[] seps = { '|' };
            string[] Columns = MailConfigs.ToString().Split(seps);
            string MailAdress = Columns[0].ToString();
            string MailBody = Columns[1].ToString();
            string MailSubject = Columns[2].ToString();
            char[] seps2 ={ ';' };
            string[] Columns2 = MailAdress.Split(seps2);
            if (Columns2.Length > 0)
            {
                DataTable MailList = new DataTable();
                MailList = new DataTable();
                MailList.Columns.Add("email");
                for (int i = 0; i < Columns2.Length; i++)
                {
                    DataRow row = MailList.NewRow();
                    row["email"] = Columns2[i].ToString();
                    MailList.Rows.Add(row);
                }
                MailList.AcceptChanges();
                bool PrepareSendMail = MailUtils.SendAfterIssue(this.Page, this.Context, MailList, MailBody, MailSubject);
            }

            DB.Commit(this.Context);
            SaveRelatedIssues();
            return id;
        }
        catch (Exception ex)
        {
            Session["Hata"] = "Bildirim eklenemedi lütfen daha sonra tekrar deneyiniz!";
            DB.Rollback(this.Context);
            return Guid.Empty;
        }

    }

    private void InitGridDTRelatedUsers(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("UserID", typeof(Guid));
        dt.Columns.Add("Adi", typeof(string));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;

    }

    private bool SaveRelatedIssues()
    {
        grid1.UpdateEdit();
        List<object> keyValues = this.grid1.GetSelectedFieldValues("ID");
        foreach (object key in keyValues)
        {
            DataRow row2 = DTRelatedUsers.Table.Rows.Find(key);

            if (row2["ID"].ToString() != this.UserID.Value.ToString())
            {
                try
                {
                    Guid id = Guid.NewGuid();
                    StringBuilder sb = new StringBuilder();
                    sb = new StringBuilder();
                    sb.Append("EXEC OnAddIssue ");
                    sb.Append("@IssueID,@FirmaID,@ProjeID,@UserID,@DurumID,@OnemDereceID,@Active,@Baslik,@Description,@HataAdimlari,@KeyWords");
                    sb.Append(",@BildirimTarihi,@CreatedBy,@ModifiedBy,@ModificationDate,@CreationDate,@TeslimTarihi,@GundemDosyaYolu,@GundemDosyaAdi");

                    SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
                    DB.AddParam(cmd, "@IssueID", id);
                    DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                    DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                    DB.AddParam(cmd, "@ModifiedBy", 50, Membership.GetUser().UserName);
                    DB.AddParam(cmd, "@CreatedBy", 50, Membership.GetUser().UserName);
                    DB.AddParam(cmd, "@Active", 1);
                    DB.AddParam(cmd, "@Baslik", 255, this.Description.Text.ToUpper());
                    DB.AddParam(cmd, "@Description", SqlDbType.Int);
                    DB.AddParam(cmd, "@HataAdimlari", SqlDbType.Int);
                    #region Gündem Dosyalarý
                    //if (GundemDosyaYolu.Value != null)
                    //    DB.AddParam(cmd, "@GundemDosyaYolu", 255, GundemDosyaYolu.Value);
                    //else
                    DB.AddParam(cmd, "@GundemDosyaYolu", SqlDbType.Int);
                    //if (GundemDosyaAdi.Value != null)
                    //    DB.AddParam(cmd, "@GundemDosyaAdi", 100, GundemDosyaAdi.Value);
                    //else
                    DB.AddParam(cmd, "@GundemDosyaAdi", SqlDbType.Int);
                    #endregion
                    if (!CrmUtils.ControllToDate(this.Page, this.TeslimTarihi.Date.ToString()))
                    {
                        DB.AddParam(cmd, "@TeslimTarihi", this.TeslimTarihi.Date);
                    }
                    else
                    {
                        if (this.OperasyonSuresi.Value.ToString() != "0")
                        {
                            DateTime date = DateTime.Now.AddDays(int.Parse(this.OperasyonSuresi.Value.ToString()));
                            DB.AddParam(cmd, "@TeslimTarihi", date);
                        }
                        else
                            DB.AddParam(cmd, "@TeslimTarihi", DateTime.Now);
                    }
                    DB.AddParam(cmd, "@BildirimTarihi", DateTime.Now);
                    Guid tmpID;
                    tmpID = new Guid(this.FirmaID.Value.ToString());
                    DB.AddParam(cmd, "@FirmaID", tmpID);
                    tmpID = new Guid(this.ProjeID.Value.ToString());
                    DB.AddParam(cmd, "@ProjeID", tmpID);
                    tmpID = new Guid(this.DurumID.Value.ToString());
                    DB.AddParam(cmd, "@DurumID", tmpID);
                    tmpID = new Guid(row2["ID"].ToString());
                    DB.AddParam(cmd, "@UserID", tmpID);
                    DB.AddParam(cmd, "@KeyWords", SqlDbType.Int);
                    tmpID = new Guid(this.OnemDereceID.Value.ToString());
                    DB.AddParam(cmd, "@OnemDereceID", tmpID);
                    object MailConfigs = (object)cmd.ExecuteScalar();
                    char[] seps = { '|' };
                    string[] Columns = MailConfigs.ToString().Split(seps);
                    string MailAdress = Columns[0].ToString();
                    string MailBody = Columns[1].ToString();
                    string MailSubject = Columns[2].ToString();
                    char[] seps2 ={ ';' };
                    string[] Columns2 = MailAdress.Split(seps2);
                    if (Columns2.Length > 0)
                    {
                        DataTable MailList = new DataTable();
                        MailList = new DataTable();
                        MailList.Columns.Add("email");
                        for (int i = 0; i < Columns2.Length; i++)
                        {
                            DataRow row = MailList.NewRow();
                            row["email"] = Columns2[i].ToString();
                            MailList.Rows.Add(row);
                        }
                        MailList.AcceptChanges();
                        bool PrepareSendMail = MailUtils.SendAfterIssue(this.Page, this.Context, MailList, MailBody, MailSubject);
                    }

                }
                catch
                {
                }
            }

        }
        return true;
    }

    protected void UserID_Callback1(object source, CallbackEventArgsBase e)
    {
        data.BindComboBoxesNoEmpty(this.Context, UserID, "EXEC ISAssignUserByUserName '" + Membership.GetUser().UserName + "','" + e.Parameter + "'", "UserID", "UserName");
        this.DTRelatedUsers.Table.Clear();
        foreach (ListEditItem item in UserID.Items)
        {
            DataRow row = DTRelatedUsers.Table.NewRow();
            row["ID"] = item.Value;
            row["UserID"] = item.Value;
            row["Adi"] = item.Text;
            this.DTRelatedUsers.Table.Rows.Add(row);
        }
        this.DTRelatedUsers.Table.AcceptChanges();
    }

    private void ControlDegerProject()
    {
        try
        {
            SqlCommand cmd = DB.SQL(this.Context, "EXEC ControlDegerProject '" + Membership.GetUser().UserName + "'");
            String ControlConfig = (String)cmd.ExecuteScalar();
            if (ControlConfig == null || ControlConfig == "Atama Yok") return;
            char[] seps = { '|' };
            string[] Columns = ControlConfig.ToString().Split(seps);
            string Projeid = Columns[0].ToString();
            string Firmaid = Columns[1].ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append("EXEC AllowedProjeListV2 '" + Membership.GetUser().UserName.ToLower() + "','" + Firmaid + "'");
            data.BindComboBoxesNoEmpty(this.Context, ProjeID, sb.ToString(), "ProjeID", "Adi");
            ProjeID.SelectedIndex = ProjeID.Items.IndexOfValue(new Guid(Projeid));


            data.BindComboBoxesNoEmpty(this.Context, UserID, "EXEC ISAssignUserByUserName '" + Membership.GetUser().UserName + "','" + Projeid + "'", "UserID", "UserName");
            UserID.SelectedIndex = this.UserID.Items.IndexOfValue(new Guid(this.HiddenID.Value.ToString()));


            FirmaID.SelectedIndex = FirmaID.Items.IndexOfValue(new Guid(Firmaid));
        }
        catch (Exception ex)
        {
        }

    }

    protected void grid1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters != "gridbind")
        {
            if (Convert.ToBoolean(e.Parameters.ToString()))
                this.grid1.Selection.SelectAll();
            else
                this.grid1.Selection.UnselectAll();
        }
        else
            grid1.DataBind();


    }
}
