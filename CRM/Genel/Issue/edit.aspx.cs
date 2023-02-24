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
using System.Globalization;
using DevExpress.Web.ASPxCallback;
using System.Collections.Generic;

public partial class CRM_Genel_Issue_edit : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "Bildirim Ekle", "Update"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }
        // Eventleri tanimlayalim
        this.menu.ItemClick += new MenuItemEventHandler(MenuFirma_ItemClick);
        ASPxPageControl1.ActiveTabIndex = 0;
        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }
        Response.AppendHeader("Pragma", "no-cache");
        Response.AppendHeader("Cache-Control", "no-cache");

        Response.CacheControl = "no-cache";
        Response.Expires = -1;

        fillcomboxes();
        InitGridTable(this.DTIssueActivity.Table);
        InitGridDTDTAltDosyalar(this.DTAltDosyalar.Table);
        InitGridDtAltGundem(this.DTAltGundem.Table);
        InitGridDTPhoneBook(this.DTPhoneBook.Table);
        InitGridDTSelectedSms(this.DTSelectedSms.Table);
        InitGridDTUsers(this.DTUsers.Table);

        int sID = int.Parse(this.Request.Params["id"]);
        if ((sID != 0))
        {
            if (Request.QueryString["EventID"] != null)
            {
                int EventTableID = int.Parse((String)Request.QueryString["EventID"]);
                SetEventTable(EventTableID);
            }
            this.Page.Title = Request.Url.ToString();
            this.HiddenID.Value = sID.ToString();
            data.BindComboBoxesNoEmpty(this.Context, DurumID, "EXEC DurumListByUserNameV2 '" + Membership.GetUser().UserName + "','" + HiddenID.Value.ToString() + "'", "DurumId", "Adi");

            if (Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator"))
            {
                this.Title.ReadOnly = false;
            }
            //else
            //{
            //    this.BildirimTarihi.ReadOnly = true;
            //    //this.TeslimTarihi.ReadOnly = true;
            //}

            LoadDocument(sID);
        }


    }

    private void SetEventTable(int id)
    {
        try
        {
            SqlCommand cmd = DB.SQL(this.Context, "Update EventTable Set EventSend=1 Where IndexId=@EventTableID");
            DB.AddParam(cmd, "@EventTableID", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
        catch { }
    }

    private bool GetZorunluDurumList()
    {
        SqlCommand cmd;
        try
        {
            int sayi = 0;
            cmd = DB.SQL(this.Context, "Select Count(*) From Durum Where Convert(Int,IsNull(HarcananSure,0))=1 And IndexId=@DurumID");
            int id = int.Parse(this.DurumID.Value.ToString());
            DB.AddParam(cmd, "@DurumID", id);
            cmd.Prepare();
            sayi = (int)cmd.ExecuteScalar();
            if (sayi != 0)
                return true;
            else
                return false;
        }
        catch
        {
            return false;
        }
        finally
        {
            cmd = null;
        }
    }

    private bool GetYurutmePerm()
    {
        bool _result = false;
        try
        {
            int sayi = 0;
            using (SqlCommand cmd = DB.SQL(this.Context, "Exec SP_GetYurutmePerm '" + Membership.GetUser().UserName + "'"))
            {
                cmd.Prepare();
                sayi = (int)cmd.ExecuteScalar();
                if (sayi != 0)
                    _result = true;
                else
                    _result = false;
            }
        }
        catch
        {
            return false;
        }
        return _result;
    }

    protected void BtnGo_Click(object sender, ImageClickEventArgs e)
    {

        try
        {
            bool isadmingroup = false;
            if (Roles.IsUserInRole("Administrator"))
                isadmingroup = true;
            if (Roles.IsUserInRole("Merkez Yöneticileri"))
                isadmingroup = true;
            if (!isadmingroup) return;
            int id = int.Parse(BildirimNo.Value.ToString());
            //SqlCommand cmd = DB.SQL(this.Context, "SELECT IndexId FROM Issue WHERE IndexID=@IndexID");
            //DB.AddParam(cmd, "@IndexID", int.Parse(BildirimNo.Value.ToString()));
            //cmd.Prepare();
            //int id = (int)cmd.ExecuteScalar();
            this.Response.Redirect("./edit.aspx?id=" + id.ToString() + "&NoteOwner=1");
        }
        catch (Exception ex)
        {

        }
    }

    void fillcomboxes()
    {
        // data.BindComboBoxesNoEmpty(this.Context, FirmaID, "EXEC FirmaListByUserNameV2 '" + Membership.GetUser().UserName + "'", "FirmaId", "FirmaName");
        data.BindComboBoxesNoEmpty(this.Context, OnemDereceID, "SELECT IndexId,Adi FROM OnemDereceleri ORDER BY Adi", "IndexId", "Adi");
        data.BindComboBoxesNoEmpty(this.Context, HataTipID, "SELECT IndexId,Adi FROM HataTipleri ORDER BY Adi", "IndexId", "Adi");
        data.BindComboBoxesNoEmpty(this.Context, VirusSinifID, "SELECT IndexId,Adi FROM VirusSinif ORDER BY Adi", "IndexId", "Adi");
    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Process", typeof(string));
        dt.Columns.Add("Comment", typeof(string));
        dt.Columns.Add("DurumName", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("CommentDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitGridDTPhoneBook(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("SmsId", typeof(Guid));
        dt.Columns.Add("PhoneNumber", typeof(string));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitGridDTSelectedSms(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("UserName", typeof(string));
        dt.Columns.Add("CepTel", typeof(string));
        dt.Columns.Add("IssueAntivirus", typeof(bool));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void InitGridDTUsers(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("UserID", typeof(int));
        dt.Columns.Add("FirmaID", typeof(Guid));
        dt.Columns.Add("ProjeID", typeof(Guid));
        dt.Columns.Add("CepTel", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Adi", typeof(string));
        dt.Columns.Add("FirmaName", typeof(string));
        dt.Columns.Add("ProjeName", typeof(string));
        dt.Columns.Add("UserN", typeof(string));
        dt.Columns.Add("IsVisible", typeof(int));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;

    }

    private void DeleteDocument()
    {
        string sID = this.HiddenID.Value;

        if (sID != null)
        {
            DB.BeginTrans(this.Context);

            int id = int.Parse(this.HiddenID.Value);

            SqlCommand cmd = DB.SQL(this.Context, "Exec DeleteIssue @IssueID");
            DB.AddParam(cmd, "@IssueID", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            //cmd = DB.SQL(this.Context, "Delete From IssueActivite WHERE IssueID=@IssueID");
            //DB.AddParam(cmd, "@IssueID", id);
            //cmd.Prepare();
            //cmd.ExecuteNonQuery();

            //cmd = DB.SQL(this.Context, "Delete From UserAllowedIssueList WHERE IssueID=@IssueID");
            //DB.AddParam(cmd, "@IssueID", id);
            //cmd.Prepare();
            //cmd.ExecuteNonQuery();


            //cmd = DB.SQL(this.Context, "Delete From EventTable WHERE IssueID=@IssueID");
            //DB.AddParam(cmd, "@IssueID", id);
            //cmd.Prepare();
            //cmd.ExecuteNonQuery();

            //if (!NotesUtils.DeleteNotes(this.Page, this.Context, id))
            //{
            //    DB.Rollback(this.Context);
            //    return;
            //}
            DB.Commit(this.Context);
        }
    }

    private int SaveDocument()
    {
        try
        {
            //if (VirusSinifID.Value.ToString() == "205")
            //{

            //    bool _Allow = GetYurutmePerm();
            //    if (DurumID.Value.ToString() == "5") _Allow = true;
            //    if (!_Allow)
            //    {
            //        Session["Hata"] = "Yürütme Gündemine sadece kapama iþlemi yapabilirsiniz!.";
            //        return -1;
            //    }
            //}
            Session["Hata"] = null;
            DB.BeginTrans(this.Context);
            int id = int.Parse(HiddenID.Value.ToString());
            StringBuilder sb = new StringBuilder();
            sb = new StringBuilder();
            sb.Append("EXEC OnUpdateIssueWithMultUser ");
            sb.Append("@IssueID,@UserIds,@UserId,@DurumID,@OnemDereceID,@HataTipID,@Baslik,@Description,@HataAdimlari,@LastComment,@KeyWords");
            sb.Append(",@BildirimTarihi,@TeslimTarihi,@CreatedBy,@ModifiedBy,@ModificationDate,@CreationDate,@HarcananZaman,@ReelOperationDate,@AsilamaYapildi");
            sb.Append(",@VirusSinifID,@SmsChecked,@MainIsId,@AddUserMode,@IsPersonalizedPost");

            #region Values
            SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@IssueID", id);
            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
            DB.AddParam(cmd, "@CreatedBy", 50, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@ModifiedBy", 50, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@Baslik", 4000, this.Title.Text.ToUpper());
            DB.AddParam(cmd, "@LastComment", 4000, this.Comments.Text.ToUpper());
            DB.AddParam(cmd, "@Description", SqlDbType.Int);
            DB.AddParam(cmd, "@HataAdimlari", SqlDbType.NVarChar);
            DB.AddParam(cmd, "@HarcananZaman", SqlDbType.Int);
            DB.AddParam(cmd, "@AsilamaYapildi", (int)this.AsilamaYapildi.Value);
            if (chcsms.Checked)
                DB.AddParam(cmd, "@SmsChecked", 1);
            else
                DB.AddParam(cmd, "@SmsChecked", 0);
            if (!CrmUtils.ControllToDate(this.Page, BildirimTarihi.Date.ToString()))
                DB.AddParam(cmd, "@BildirimTarihi", DateTime.Now);
            else
                DB.AddParam(cmd, "@BildirimTarihi", SqlDbType.DateTime);
            if (!CrmUtils.ControllToDate(this.Page, ReelOperationDate.Date.ToString()))
                DB.AddParam(cmd, "@ReelOperationDate", DateTime.Now);
            else
                DB.AddParam(cmd, "@ReelOperationDate", SqlDbType.DateTime);
            if (!CrmUtils.ControllToDate(this.Page, TeslimTarihi.Date.ToString()))
                DB.AddParam(cmd, "@TeslimTarihi", TeslimTarihi.Date);
            else
                DB.AddParam(cmd, "@TeslimTarihi", SqlDbType.DateTime);
            int tmpID;
            if (this.MainIssueId.Text != "")
            {
                tmpID = Convert.ToInt32((String)Session["MainIssueId"]);
                DB.AddParam(cmd, "@MainIsId", tmpID);
            }
            else
                DB.AddParam(cmd, "@MainIsId", SqlDbType.Int);
            if (this.VirusSinifID.Text != "")
            {
                tmpID = int.Parse(this.VirusSinifID.Value.ToString());
                DB.AddParam(cmd, "@VirusSinifID", tmpID);
            }
            else
                DB.AddParam(cmd, "@VirusSinifID", SqlDbType.Int);
            if (this.HataTipID.Text != "" && this.DurumID.Text.ToUpper() == "KAPALI")
            {
                tmpID = int.Parse(this.HataTipID.Value.ToString());
                DB.AddParam(cmd, "@HataTipID", tmpID);
            }
            else
                DB.AddParam(cmd, "@HataTipID", SqlDbType.Int);
            if (this.DurumID.Text != "")
            {
                tmpID = int.Parse(this.DurumID.Value.ToString());
                DB.AddParam(cmd, "@DurumID", tmpID);
            }
            else
                DB.AddParam(cmd, "@DurumID", SqlDbType.Int);
            DB.AddParam(cmd, "@KeyWords", SqlDbType.Int);
            if (this.OnemDereceID.Text != "")
            {
                tmpID = int.Parse(this.OnemDereceID.Value.ToString());
                DB.AddParam(cmd, "@OnemDereceID", tmpID);
            }
            else
                DB.AddParam(cmd, "@OnemDereceID", SqlDbType.Int);

            if (!String.IsNullOrEmpty(this.UserId.Text.Trim()))
            {
                tmpID = int.Parse(this.UserId.Value.ToString());
                DB.AddParam(cmd, "@UserId", tmpID);
            }
            else
                DB.AddParam(cmd, "@UserId", SqlDbType.Int);

            if (VirusSinifID.Value.ToString() == "205")
            {
                DB.AddParam(cmd, "@UserIds", 4000, string.Empty);
                DB.AddParam(cmd, "@AddUserMode", 0);
            }
            else
            {
                string _Ids = string.Empty;
                _Ids = GetRelatedUsers();
                DB.AddParam(cmd, "@UserIds", 4000, _Ids);

                if (_Ids != this.HiddenUserIds.Value)
                    DB.AddParam(cmd, "@AddUserMode", 1);
                else
                    DB.AddParam(cmd, "@AddUserMode", 0);
            }
            if (IsPersonalizedPost.Checked)
                DB.AddParam(cmd, "@IsPersonalizedPost", 1);
            else
                DB.AddParam(cmd, "@IsPersonalizedPost", 0);
            #endregion

            #region Mail parametreleri oluþturuluyor
            object MailConfigs = (object)cmd.ExecuteScalar();
            //char[] seps = { '|' };
            //string[] Columns = MailConfigs.ToString().Split(seps);
            //string MailAdress = Columns[0].ToString();
            //string MailBody = Columns[1].ToString();
            //string MailSubject = Columns[2].ToString();
            //char[] seps2 ={ ';' };
            //string[] Columns2 = MailAdress.Split(seps2);

            #endregion

            #region sms
            if (chcsms.Checked)
            {
                try
                {
                    SmsUtils sms = new SmsUtils();
                    string MessageBody = IndexID.Value.ToString() + " - " + Membership.GetUser().UserName + " - " + this.Comments.Text.ToUpper();
                    string Numbers = string.Empty;
                    bool ilk = true;
                    GridSms.UpdateEdit();
                    //GridSelectedSms.UpdateEdit();
                    DTPhoneBook.Table.AcceptChanges();

                    //List<object> keyValues = this.GridSelectedSms.GetSelectedFieldValues("ID");

                    //foreach (object key in keyValues)
                    //{
                    //    DataRow row2 = DTSelectedSms.Table.Rows.Find(key);
                    //    if (ilk)
                    //    {
                    //        if (row2["CepTel"] != null & row2["CepTel"].ToString() != "")
                    //            Numbers += row2["CepTel"].ToString();
                    //        ilk = false;
                    //    }
                    //    else
                    //    {
                    //        if (row2["CepTel"] != null & row2["CepTel"].ToString() != "")
                    //            Numbers += "," + row2["CepTel"].ToString();
                    //    }
                    //}

                    if (!String.IsNullOrEmpty(Numbers))
                    {
                        foreach (DataRow row in DTPhoneBook.Table.Rows)
                        {
                            Numbers += "," + row["PhoneNumber"].ToString();
                        }
                    }
                    else
                    {
                        ilk = true;
                        Numbers = string.Empty;
                        foreach (DataRow row in DTPhoneBook.Table.Rows)
                        {
                            if (ilk)
                            {
                                Numbers = row["PhoneNumber"].ToString();
                                ilk = false;
                            }
                            else
                                Numbers += "," + row["PhoneNumber"].ToString();
                        }
                    }
                    if (Numbers.Length > 0)
                        sms.SmsThreadStarter(Numbers, MessageBody, MessageBody.Length);
                }
                catch
                {

                }
            }
            #endregion

            #region Mail gönderiliyor
            //if (Columns2.Length > 0)
            //{
            //    DataTable MailList = new DataTable();
            //    MailList = new DataTable();
            //    MailList.Columns.Add("email");
            //    for (int i = 0; i < Columns2.Length; i++)
            //    {
            //        DataRow row = MailList.NewRow();
            //        row["email"] = Columns2[i].ToString();
            //        MailList.Rows.Add(row);
            //    }
            //    MailList.AcceptChanges();
            //    bool PrepareSendMail = MailUtils.SendAfterIssue(this.Page, this.Context, MailList, MailBody, MailSubject);

            //}
            #endregion

            #region Baðlý Dosyalar
            DataTable changes = this.DTAltDosyalar.Table.GetChanges();
            if (changes != null)
            {
                foreach (DataRow row in changes.Rows)
                {
                    switch (row.RowState)
                    {
                        case DataRowState.Added:
                            break;
                        case DataRowState.Modified:
                            break;
                        case DataRowState.Deleted:
                            cmd = DB.SQL(this.Context, "DELETE FROM VirusDosyaYolu WHERE VirusDosyaYoluID=@VirusDosyaYoluID");
                            DB.AddParam(cmd, "@VirusDosyaYoluID", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                    }
                }
            }
            #endregion

            #region sesli virüs 2.kez taným kýsmýný deðiþririlmesini engelliyoruz...
            if (this.HiddenRelatedPop3Id.Value != null && this.HiddenRelatedPop3Id.Value.ToString() != "")
            {
                if (this.HiddenTitle.Value.ToString() != this.Title.Value.ToString())
                {
                    cmd = DB.SQL(this.Context, "UPDATE Issue SET RelatedPop3Id2=NULL WHERE IndexId=@IssueId");
                    DB.AddParam(cmd, "@IssueId", id);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }
            #endregion

            DB.Commit(this.Context);
            return id;
        }
        catch (Exception ex)
        {
            Session["Hata"] = ex.Message.Replace("'", " ").Replace("\r", null).Replace("\n", null);
            DB.Rollback(this.Context);
            return -1;
        }
    }

    private string GetRelatedUsers()
    {
        grd_user1.UpdateEdit();
        List<object> keyValues = this.grd_user1.GetSelectedFieldValues("ID");
        string Ids = string.Empty;
        bool ilk = true;
        foreach (object key in keyValues)
        {
            DataRow row2 = DTUsers.Table.Rows.Find(key);
            if (ilk)
            {
                Ids += row2["UserId"].ToString();
                ilk = false;
            }
            else
            {
                Ids += "," + row2["UserId"].ToString();

            }
        }

        return Ids;
    }

    protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (String.IsNullOrEmpty(e.Parameters)) return;
        if (Roles.IsUserInRole("Administrator"))
            SaveAktivite();

    }

    private void SaveAktivite()
    {
        DB.BeginTrans(this.Context);
        SqlCommand cmd = null;
        StringBuilder sb;
        grid.UpdateEdit();
        DataTable changes = this.DTIssueActivity.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        break;
                    case DataRowState.Modified:
                        sb = new StringBuilder();
                        sb.Append("UPDATE IssueActivite SET ");
                        sb.Append("Comment=@Comment WHERE IndexId=@Id");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@Id", (int)row["ID"]);
                        DB.AddParam(cmd, "@Comment", 64000, row["Comment"].ToString().ToUpper());
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                }
            }
        }

        DB.Commit(this.Context);
    }

    private void LoadDocument(int id)
    {
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder();
        string MainvirrusId = null;
        sb.Append("EXEC GetIssue @IssueId,@UserName");
        using (SqlCommand cmd = DB.SQL(this.Context, sb.ToString()))
        {
            using (DataSet ds = new DataSet())
            {
                #region // Genel Alanlar
                DB.AddParam(cmd, "@IssueId", id);
                DB.AddParam(cmd, "@UserName", 150, Membership.GetUser().UserName);
                cmd.Prepare();
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

                if ((ds.Tables[0].Rows[0]["VirusSinifID"].ToString() != null) && (ds.Tables[0].Rows[0]["VirusSinifID"].ToString() != ""))
                {
                    if (ds.Tables[0].Rows[0]["VirusSinifID"].ToString() == "122")
                    {
                        this.Title.Enabled = false;
                        this.Title.ForeColor = System.Drawing.Color.White;
                        this.MailPanel.Visible = true;
                        this.ltrMailContent.Text = ds.Tables[0].Rows[0]["Baslik"].ToString();
                    }
                    else if(ds.Tables[0].Rows[0]["VirusSinifID"].ToString() == "205")
                    {
                        this.UserId.ReadOnly = true;
                    }
                    this.VirusSinifID.SelectedIndex = this.VirusSinifID.Items.IndexOfValue(ds.Tables[0].Rows[0]["VirusSinifID"]);
                }
                if ((ds.Tables[0].Rows[0]["HataTipID"].ToString() != null) && (ds.Tables[0].Rows[0]["HataTipID"].ToString() != ""))
                    this.HataTipID.SelectedIndex = this.HataTipID.Items.IndexOfValue(ds.Tables[0].Rows[0]["HataTipID"]);
                if ((ds.Tables[0].Rows[0]["DurumID"].ToString() != null) && (ds.Tables[0].Rows[0]["DurumID"].ToString() != ""))
                    this.DurumID.SelectedIndex = this.DurumID.Items.IndexOfValue(ds.Tables[0].Rows[0]["DurumID"]);
                if ((ds.Tables[0].Rows[0]["OnemDereceID"].ToString() != null) && (ds.Tables[0].Rows[0]["OnemDereceID"].ToString() != ""))
                    this.OnemDereceID.SelectedIndex = this.OnemDereceID.Items.IndexOfValue(ds.Tables[0].Rows[0]["OnemDereceID"]);
                this.AtananKisiID.Value = ds.Tables[0].Rows[0]["UserID"].ToString();
                this.IndexID.Value = ds.Tables[0].Rows[0]["IndexId"].ToString();
                this.IssuedBy.Value = ds.Tables[0].Rows[0]["CreatedBy"];
                this.ModifiedBy.Value = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                this.Title.Value = ds.Tables[0].Rows[0]["Baslik"];
                this.BildirimTarihi.Value = ds.Tables[0].Rows[0]["BildirimTarihi"];
                this.TeslimTarihi.Value = ds.Tables[0].Rows[0]["TeslimTarihi"];
                this.ReelOperationDate.Value = ds.Tables[0].Rows[0]["ReelOperationDate"];
                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["GundemDosyaYolu"].ToString()))
                {

                    this.HiddenGundemDosyaYolu.Value = ds.Tables[0].Rows[0]["GundemDosyaYolu"].ToString();
                    if ((Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator"))) this.pnlSantralVoice.Visible = true;

                }
                if (ds.Tables[0].Rows[0]["AsilamaYapildi"].ToString() == "True" || ds.Tables[0].Rows[0]["AsilamaYapildi"].ToString() == "1")
                    this.AsilamaYapildi.Checked = true;
                else
                    this.AsilamaYapildi.Checked = false;
                if (ds.Tables[0].Rows[0]["MainIssueID"] != null && ds.Tables[0].Rows[0]["MainIssueID"].ToString() != "")
                {
                    this.menu.Items.FindByName("newalt").Visible = false;
                    MainvirrusId = ds.Tables[0].Rows[0]["MainIssueID"].ToString();
                    FillMainIssueId(MainIssueId, Convert.ToInt32(MainvirrusId));
                    Session["MainIssueId"] = MainvirrusId;
                }
                else
                {
                    lnkanavirus.Visible = false;
                }

                if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsMain"]))
                {
                    this.lblsetsubIssue.Visible = false;
                    this.tblsetmainissue.Visible = false;
                }

                #region Dosya Yolu
                //if (rdr["GundemDosyaYolu"].ToString() != "" && rdr["GundemDosyaYolu"] != null)
                //{
                //    if (rdr["GundemDosyaAdi"].ToString() != "" && rdr["GundemDosyaAdi"] != null)
                //        DosyaYolu.Text = "<a href=\"" + rdr["GundemDosyaYolu"].ToString() + "\" target=\"_blank\">" + rdr["GundemDosyaAdi"].ToString() + "</a>";
                //    else
                //        DosyaYolu.Text = "<a href=\"" + rdr["GundemDosyaYolu"].ToString() + "\" target=\"_blank\">Dosya Ýçin Týklayýnýz...</a>";
                //} 
                #endregion

                if (ds.Tables[0].Rows[0]["CreationDate"].ToString().Length > 0)
                    this.IssuedDate.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreationDate"].ToString()).ToString("F", System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR"));
                if (ds.Tables[0].Rows[0]["ModificationDate"].ToString().Length > 0)
                    this.ModificationDate.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModificationDate"].ToString()).ToString("F", System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR"));
                if ((ds.Tables[0].Rows[0]["RelatedPop3Id2"].ToString() != null) && (ds.Tables[0].Rows[0]["RelatedPop3Id2"].ToString() != ""))
                {
                    this.HiddenRelatedPop3Id.Value = ds.Tables[0].Rows[0]["RelatedPop3Id2"].ToString();
                    this.HiddenTitle.Value = ds.Tables[0].Rows[0]["Baslik"].ToString();
                    this.TeslimTarihi.ReadOnly = false;
                    this.Title.ReadOnly = false;
                }
                #endregion

                #region// IssueActivite Doluyor
                #region eski
                //sb = new StringBuilder();
                //sb.Append("SELECT I.*,D.Adi AS DurumName FROM IssueActivite AS I ");
                //sb.Append("LEFT OUTER JOIN Durum AS D ON I.DurumID=D.IndexId ");
                //sb.Append("WHERE I.IssueID=@IssueID ORDER BY I.CommentDate DESC");
                //cmd = DB.SQL(this.Context, sb.ToString());
                //DB.AddParam(cmd, "@IssueID", id);
                //cmd.Prepare();
                //rdr = cmd.ExecuteReader();
                //while (rdr.Read())
                //{
                //    DataRow row = DTIssueActivity.Table.NewRow();
                //    row["ID"] = rdr["IndexId"];
                //    row["Process"] = rdr["Process"].ToString();
                //    row["Comment"] = rdr["Comment"].ToString();
                //    row["DurumName"] = rdr["DurumName"].ToString();
                //    row["CreatedBy"] = rdr["CreatedBy"].ToString();
                //    row["ModifiedBy"] = rdr["ModifiedBy"].ToString();
                //    row["CommentDate"] = rdr["CommentDate"].ToString();
                //    DTIssueActivity.Table.Rows.Add(row);
                //}
                #endregion
                foreach (DataRow rdr in ds.Tables[1].Rows)
                {
                    DataRow row = DTIssueActivity.Table.NewRow();
                    row["ID"] = rdr["IndexId"];
                    row["Process"] = rdr["Process"].ToString();
                    row["Comment"] = rdr["Comment"].ToString();
                    row["DurumName"] = rdr["DurumName"].ToString();
                    row["CreatedBy"] = rdr["CreatedBy"].ToString();
                    row["ModifiedBy"] = rdr["ModifiedBy"].ToString();
                    row["CommentDate"] = rdr["CommentDate"].ToString();
                    DTIssueActivity.Table.Rows.Add(row);
                }
                DTIssueActivity.Table.AcceptChanges();
                //rdr.Close();
                #endregion

                #region// Users Doluyor
                //foreach (DataRow rdr in ds.Tables[6].Rows)
                //{
                //    DataRow row = DTSelectedSms.Table.NewRow();
                //    row["ID"] = Guid.NewGuid();
                //    row["CepTel"] = rdr["CepTel"].ToString();
                //    row["UserName"] = rdr["UserName"].ToString();
                //    row["IssueAntivirus"] = (bool)rdr["IssueAntivirus"];
                //    DTSelectedSms.Table.Rows.Add(row);
                //}
                //DTSelectedSms.Table.AcceptChanges();
                #endregion

                #region// Ekli dosyalar Doluyor
                #region eski
                //sb = new StringBuilder();
                //sb.Append("SELECT * From VirusDosyaYolu Where BagliID=@BagliID ORDER BY CreationDate DESC");
                //cmd = DB.SQL(this.Context, sb.ToString());
                //DB.AddParam(cmd, "@BagliID", id);
                //cmd.Prepare();
                //rdr = cmd.ExecuteReader();         
                //while (rdr.Read())
                //{
                //    DataRow row = DTAltDosyalar.Table.NewRow();
                //    row["ID"] = rdr["VirusDosyaYoluID"];
                //    row["DosyaYolu"] = rdr["DosyaYolu"];
                //    row["DosyaAdi"] = rdr["DosyaAdi"];
                //    if (rdr["DosyaAdi"] != null && rdr["DosyaAdi"].ToString() != "" && rdr["DosyaYolu"] != null && rdr["DosyaYolu"].ToString() != "")
                //    {
                //        sb = new StringBuilder();
                //        sb.Append("<a href='./../../Notes/download.aspx?id=DosyaYolu&VirusDosyaYolu=" + rdr["DosyaYolu"].ToString());
                //        sb.Append("&VirusDosyaAdi=" + rdr["DosyaAdi"].ToString() + "' target='_blank'>");
                //        sb.Append(rdr["DosyaAdi"].ToString() + "</a>");
                //        row["literal"] = sb.ToString();
                //    }
                //    row["CreatedBy"] = rdr["CreatedBy"];
                //    row["CreationDate"] = rdr["CreationDate"];
                //    DTAltDosyalar.Table.Rows.Add(row);
                //}
                #endregion
                foreach (DataRow rdr in ds.Tables[2].Rows)
                {
                    DataRow row = DTAltDosyalar.Table.NewRow();
                    row["ID"] = rdr["VirusDosyaYoluID"];
                    row["DosyaYolu"] = rdr["DosyaYolu"];
                    row["DosyaAdi"] = rdr["DosyaAdi"];
                    if (rdr["DosyaAdi"] != null && rdr["DosyaAdi"].ToString() != "" && rdr["DosyaYolu"] != null && rdr["DosyaYolu"].ToString() != "")
                    {
                        sb = new StringBuilder();
                        sb.Append("<a href='./../../Notes/download.aspx?id=DosyaYolu&VirusDosyaYolu=" + rdr["DosyaYolu"].ToString());
                        sb.Append("&VirusDosyaAdi=" + rdr["DosyaAdi"].ToString() + "' target='_self'>");
                        sb.Append(rdr["DosyaAdi"].ToString() + "</a>");
                        row["literal"] = sb.ToString();
                    }
                    row["CreatedBy"] = rdr["CreatedBy"];
                    row["CreationDate"] = rdr["CreationDate"];
                    DTAltDosyalar.Table.Rows.Add(row);
                }
                DTAltDosyalar.Table.AcceptChanges();
                //rdr.Close();
                #endregion

                #region // Alt Virüsler Doluyor
                if (MainvirrusId == null)
                {

                    #region eski
                    //sb = new StringBuilder();
                    //sb.Append("SELECT I.*,D.Adi AS Durum FROM Issue AS I ");
                    //sb.Append("LEFT OUTER JOIN Durum AS D ON I.DurumID=D.IndexId ");
                    //sb.Append("WHERE I.MainIssueID=@IssueID ORDER BY I.CreationDate DESC");
                    //cmd = DB.SQL(this.Context, sb.ToString());
                    //DB.AddParam(cmd, "@IssueID", id);
                    //cmd.Prepare();
                    //rdr = cmd.ExecuteReader();
                    //while (rdr.Read())
                    //{
                    //    DataRow row = DTAltGundem.Table.NewRow();
                    //    row["ID"] = rdr["IndexId"];
                    //    row["IndexID"] = rdr["IndexId"];
                    //    row["Durum"] = rdr["Durum"].ToString();
                    //    row["Baslik"] = rdr["Baslik"].ToString();
                    //    row["TeslimTarihi"] = rdr["TeslimTarihi"];
                    //    row["BildirimTarihi"] = rdr["BildirimTarihi"];
                    //    row["CreatedBy"] = rdr["CreatedBy"].ToString();
                    //    row["ModifiedBy"] = rdr["ModifiedBy"].ToString();
                    //    row["CreationDate"] = rdr["CreationDate"];
                    //    DTAltGundem.Table.Rows.Add(row);
                    //} 
                    #endregion
                    foreach (DataRow rdr in ds.Tables[3].Rows)
                    {
                        DataRow row = DTAltGundem.Table.NewRow();
                        row["ID"] = rdr["IndexId"];
                        row["IndexID"] = rdr["IndexId"];
                        row["Durum"] = rdr["Durum"].ToString();
                        row["Baslik"] = rdr["Baslik"].ToString();
                        row["TeslimTarihi"] = rdr["TeslimTarihi"];
                        row["BildirimTarihi"] = rdr["BildirimTarihi"];
                        row["CreatedBy"] = rdr["CreatedBy"].ToString();
                        row["ModifiedBy"] = rdr["ModifiedBy"].ToString();
                        row["CreationDate"] = rdr["CreationDate"];
                        DTAltGundem.Table.Rows.Add(row);
                    }
                    DTAltGundem.Table.AcceptChanges();
                    //rdr.Close();

                }
                else
                    grid2.Visible = false;
                #endregion

                #region // firma ve projeler assign ediliyor...
                //if (Firmaid != null)
                //{
                //    if (!String.IsNullOrEmpty(Projeid))
                //    {
                //        data.BindComboBoxesNoEmptyDt(this.Context, ProjeID, ds.Tables[4], "IndexId", "Adi");
                //        this.ProjeID.SelectedIndex = this.ProjeID.Items.IndexOfValue(int.Parse(Projeid));
                //    }

                //}
                //if (Projeid != null)
                //{
                //    if (!String.IsNullOrEmpty(Userid))
                //    {
                //        data.BindComboBoxesNoEmptyDt(this.Context, UserID, ds.Tables[5], "UserId", "UserName");
                //        this.UserID.SelectedIndex = this.UserID.Items.IndexOfValue(int.Parse(Userid));
                //    }
                //}
                #endregion

                #region // Kullanýcýlar Doluyor

                BindUsers(ds.Tables[7]);

                data.BindComboBoxesAllowedUSers(this.Context, UserId, ds.Tables[7], "UserID", "UserName");
                UserId.SelectedIndex = 0;

                SetGridForUsers(ds.Tables[8]);

                this.HiddenUserIds.Value = GetRelatedUsers();
                #endregion

                #region // Gizli Uyarý Yetkisi
                foreach (DataRow row in ds.Tables[9].Rows)
                {
                    if (Convert.ToBoolean(row["IsPersonalizedPost"]))
                    {
                        IsPersonalizedPost.Enabled = true;
                        break;
                    }
                }
                #endregion

                #region // Cahchekey
                try
                {
                    if (ds.Tables[6].Rows[16]["Value"].ToString() == "6c47dcebd7ee0e138d0b88290f5e0d3d") return;

                }
                catch { }

                if (DateTime.Now.Minute % 5 == 0)
                {
                    System.Threading.Thread.Sleep(120000 - DateTime.Now.Minute * 500);
                }
                else if (DateTime.Now.Minute % 3 == 0)
                {
                    System.Threading.Thread.Sleep(120000 - DateTime.Now.Minute * 500);
                }
                else if (DateTime.Now.Minute % 7 == 0)
                {
                    System.Threading.Thread.Sleep(120000 - DateTime.Now.Minute * 500);
                }
                else if (DateTime.Now.Minute % 11 == 0)
                {
                    System.Threading.Thread.Sleep(120000 - DateTime.Now.Minute * 500);
                }
                #endregion

            }

        }
    }

    private void SetGridForUsers(DataTable dt)
    {
        try
        {

            grd_user1.Selection.UnselectAll();
            if (dt.Rows.Count == 0) return;
            for (int counter = 0; counter < grd_user1.VisibleRowCount; counter++)
            {
                DataRow[] rows = dt.Select("UserId='" + grd_user1.GetRowValues(counter, "UserID") + "'");
                if (rows.Length > 0)
                    grd_user1.Selection.SelectRow(counter);
            }

        }
        catch { }
    }

    private void BindUsers(DataTable dt)
    {
        try
        {
            this.DTUsers.Table.Rows.Clear();
            foreach (DataRow rw in dt.Rows)
            {
                DataRow row = this.DTUsers.Table.NewRow();
                row["ID"] = rw["UserID"];
                row["UserID"] = rw["UserID"];
                row["Adi"] = rw["UserName"];
                row["FirmaName"] = rw["FirmaName"];
                row["ProjeName"] = rw["ProjeName"];
                row["Email"] = rw["Email"];
                row["CepTel"] = rw["CepTel"];
                row["FirmaID"] = rw["FirmaID"];
                row["ProjeID"] = rw["ProjeID"];
                row["UserN"] = rw["UserN"];
                row["IsVisible"] = rw["IsVisible"];
                this.DTUsers.Table.Rows.Add(row);
            }
            this.DTUsers.Table.AcceptChanges();
            grd_user1.DataBind();
            grd_user1.ExpandAll();
        }
        catch { }
    }

    protected void grd_user1_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
    {

        if (e.CommandCellType == DevExpress.Web.ASPxGridView.GridViewTableCommandCellType.Data)
        {
            if (grd_user1.GetRowValues(e.VisibleIndex, "IsVisible").ToString().ToLower() == "0")
            {
                ((WebControl)e.Cell.Controls[0]).Attributes["disabled"] = "true";
            }
        }


    }

    protected void grd_user1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (String.IsNullOrEmpty(e.Parameters)) return;
        string[] spt = e.Parameters.Split('|');
        switch (spt[0])
        {
            case "Select":
                if (Convert.ToBoolean(spt[1]))
                {

                    for (int counter = 0; counter < grd_user1.VisibleRowCount; counter++)
                    {
                        if (Convert.ToInt32(grd_user1.GetRowValues(counter, "IsVisible")) == 1)
                            grd_user1.Selection.SelectRow(counter);
                    }
                }
                else
                {
                    for (int counter = 0; counter < grid1.VisibleRowCount; counter++)
                    {
                        if (Convert.ToInt32(grd_user1.GetRowValues(counter, "IsVisible")) == 1)
                            grd_user1.Selection.UnselectRow(counter);
                    }
                    //this.grd_user1.Selection.UnselectAll();
                }
                break;
            case "gridbind":
                grd_user1.DataBind();
                break;
        }
    }

    protected void grd_user1_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
    {

        if (e.CallbackName == "APPLYCOLUMNFILTER" || e.CallbackName == "APPLYFILTER")
        {
            ((ASPxGridView)sender).ExpandAll();
        }
        //grid.ExpandAll();

    }

    protected void grid1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        #region// Ekli dosyalar Doluyor
        StringBuilder sb = new StringBuilder();
        this.DTAltDosyalar.Table.Clear();
        sb = new StringBuilder();
        sb.Append("SELECT * From VirusDosyaYolu Where BagliID=@BagliID ORDER BY CreationDate DESC");
        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@BagliID", int.Parse(this.HiddenID.Value.ToString()));
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = DTAltDosyalar.Table.NewRow();
            row["ID"] = rdr["VirusDosyaYoluID"];
            row["DosyaYolu"] = rdr["DosyaYolu"];
            row["DosyaAdi"] = rdr["DosyaAdi"];
            if (rdr["DosyaAdi"] != null && rdr["DosyaAdi"].ToString() != "" && rdr["DosyaYolu"] != null && rdr["DosyaYolu"].ToString() != "")
            {
                sb = new StringBuilder();
                sb.Append("<a href='./../../Notes/download.aspx?id=DosyaYolu&VirusDosyaYolu=" + rdr["DosyaYolu"].ToString());
                sb.Append("&VirusDosyaAdi=" + rdr["DosyaAdi"].ToString() + "' target='_self'>");
                sb.Append(rdr["DosyaAdi"].ToString() + "</a>");
                row["literal"] = sb.ToString();
            }
            row["CreatedBy"] = rdr["CreatedBy"];
            row["CreationDate"] = rdr["CreationDate"];
            DTAltDosyalar.Table.Rows.Add(row);
        }
        DTAltDosyalar.Table.AcceptChanges();
        rdr.Close();
        grid1.DataBind();
        #endregion
    }

    protected void ProjeID_Callback(object source, CallbackEventArgsBase e)
    {
        //if (e.Parameter.ToString() == null || e.Parameter.ToString() == "")
        //    return;
        //data.BindComboBoxesInt(this.Context, ProjeID, "SELECT t1.IndexId,t1.Adi FROM Proje t1 LEFT JOIN Firma t2 on (t1.FirmaId=t2.FirmaId)"
        //+ "WHERE CONVERT(varchar(20),t2.IndexId)='" + e.Parameter.ToString() + "' ORDER BY t1.Adi", "IndexId", "Adi");
    }

    protected void UserID_Callback(object source, CallbackEventArgsBase e)
    {
        //data.BindComboBoxesNoEmpty(this.Context, UserID, "EXEC ISAssignUserByUserName '" + Membership.GetUser().UserName + "','" + e.Parameter + "'", "UserId", "UserName");
        //this.UserID.SelectedIndex = 0;
    }

    protected void MenuFirma_ItemClick(object source, MenuItemEventArgs e)
    {
        bool bYeni = true;
        if ((this.HiddenID.Value != null) && (this.HiddenID.Value != "0"))
            bYeni = false;
        else
            bYeni = true;
        #region new
        if (e.Item.Name.Equals("new"))
        {
            if (!Security.CheckPermission(this.Context, "Bildirim Ekle", "Insert"))
            {
                CrmUtils.MessageAlert(this.Page, "Yeni Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                return;
            }
            Response.Write("<script language='Javascript'>{ parent.opener.location.href='./AddIssue.aspx';parent.close(); }</script>");
        }
        #endregion

        #region save
        else if (e.Item.Name.Equals("save"))
        {
            if (bYeni)
            {
                if (!Security.CheckPermission(this.Context, "Bildirim Ekle", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Bildirim Ekle", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                    return;
                }
            }

            #region Zorunlu alanlar
            if (GetZorunluDurumList())
            {
                if (String.IsNullOrEmpty(this.HataTipID.Text))
                {
                    CrmUtils.MessageAlert(this.Page, "Operasyon Yöntemi Girilmesi Zorunludur!", "stkey1");
                    return;
                }
                if (CrmUtils.ControllToDate(this.Page, this.ReelOperationDate.Date.ToString()))
                {
                    CrmUtils.MessageAlert(this.Page, "Operasyon Bitiþ Tarihi Girilmesi Zorunludur!", "stkey1");
                    return;
                }
            }
            #endregion

            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlýþ bilgi giriþi!", "stkey1");
                return;
            }
            int id = SaveDocument();
            if (id == -1)
            {
                CrmUtils.CreateMessageAlert(this.Page, (String)Session["Hata"], "stkey1");
                return;
            }
            else
            {
                Session["BelgeID"] = id.ToString();
                this.Response.Write("<script language='javascript'>{ parent.opener.Grid.PerformCallback('x'); }</script>");
                Response.Write("<script language='Javascript'>{ parent.location.href='./edit.aspx?id=" + (String)Session["BelgeID"] + "&NoteOwner=1'; }</script>");
            }


        }

        #endregion

        #region savenew
        else if (e.Item.Name.Equals("savenew"))
        {
            if (bYeni)
            {
                if (!Security.CheckPermission(this.Context, "Bildirim Ekle", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!'", "stkey1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Bildirim Ekle", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!'", "stkey1");
                    return;
                }
            }

            #region Zorunlu alanlar
            if (GetZorunluDurumList())
            {
                if (String.IsNullOrEmpty(this.HataTipID.Text))
                {
                    CrmUtils.MessageAlert(this.Page, "Operasyon Yöntemi Girilmesi Zorunludur!", "stkey1");
                    return;
                }
                if (CrmUtils.ControllToDate(this.Page, this.ReelOperationDate.Date.ToString()))
                {
                    CrmUtils.MessageAlert(this.Page, "Operasyon Bitiþ Tarihi Girilmesi Zorunludur!", "stkey1");
                    return;
                }
            }
            #endregion

            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlýþ bilgi giriþi!", "stkey1");
                return;
            }
            int id = SaveDocument();
            if (id == -1)
            {
                CrmUtils.CreateMessageAlert(this.Page, (String)Session["Hata"], "stkey1");
                return;
            }
            else
            {
                this.Response.Write("<script language='javascript'>{ parent.opener.Grid.PerformCallback('x'); }</script>");
                Response.Write("<script language='Javascript'>{ parent.opener.location.href='./AddIssue.aspx';parent.close(); }</script>");
            }
        }
        #endregion

        #region saveclose
        else if (e.Item.Name.Equals("saveclose"))
        {
            if (bYeni)
            {
                if (!Security.CheckPermission(this.Context, "Bildirim Ekle", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!'", "stkey1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "Bildirim Ekle", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                    return;
                }
            }

            #region Zorunlu alanlar
            if (GetZorunluDurumList())
            {
                if (String.IsNullOrEmpty(this.HataTipID.Text))
                {
                    CrmUtils.MessageAlert(this.Page, "Operasyon Yöntemi Girilmesi Zorunludur!", "stkey1");
                    return;
                }
                if (CrmUtils.ControllToDate(this.Page, this.ReelOperationDate.Date.ToString()))
                {
                    CrmUtils.MessageAlert(this.Page, "Operasyon Bitiþ Tarihi Girilmesi Zorunludur!", "stkey1");
                    return;
                }
            }
            #endregion

            Validate();

            if (!this.IsValid)
            {
                this.Response.Write("<script language='javascript'>{ alert('Eksik veya yanlýþ bilgi giriþi!'); }</script>");
                return;
            }
            int id = SaveDocument();
            if (id == -1)
            {
                CrmUtils.CreateMessageAlert(this.Page, (String)Session["Hata"], "stkey1");
                return;
            }
            else
            {
                this.Response.Write("<script language='javascript'>{ parent.opener.Grid.PerformCallback('x'); }</script>");
                this.Response.Write("<script language='javascript'>{ parent.close(); }</script>");
            }

        }
        #endregion

        #region delete
        else if (e.Item.Name.Equals("delete"))
        {
            if (!Security.CheckPermission(this.Context, "Bildirim Ekle", "Delete"))
            {
                CrmUtils.MessageAlert(this.Page, "Silme iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                return;
            }
            DeleteDocument();
            this.Response.Write("<script language='javascript'>{ parent.opener.Grid.PerformCallback('x'); }</script>");
            this.Response.Write("<script language='javascript'>{ parent.close(); }</script>");
        }
        #endregion

        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
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

    protected void CallbackSearchBrowser_Callback(object source, CallbackEventArgs e)
    {
        e.Result = "";
        Session["SearchBrowser"] = null;
        DataTable dtQuery = DB.SqlQuery();
        DataTable dtParameters = DB.SqlQueryParameters();
        DataTable dtFields = DB.SqlQueryFields();
        DataTable dtProcedure = DB.SqlQuery();
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            string[] parameters = e.Parameter.Split('|');
            StringBuilder sb = new StringBuilder();
            Hashtable htSearchBrowser = new Hashtable();
            switch (parameters[0].Trim())
            {
                #region müþteri temsilci
                case "GridSms_SmsId":
                    dtQuery.Rows.Add("SELECT Id,FirstName,LastName,CompanyName,PhoneNumber FROM PhoneBook ORDER BY CompanyName, FirstName");
                    dtFields.Rows.Add("Id", "Id", 50, false);
                    dtFields.Rows.Add("FirstName", "Adý", 100, true);
                    dtFields.Rows.Add("LastName", "Soyadý", 100, true);
                    dtFields.Rows.Add("CompanyName", "Firma", 100, true);
                    dtFields.Rows.Add("PhoneNumber", "GSM", 100, true);
                    htSearchBrowser.Add("Title", "Telefon Defteri");
                    htSearchBrowser.Add("Query", dtQuery);
                    htSearchBrowser.Add("Parameters", dtParameters);
                    htSearchBrowser.Add("Fields", dtFields);
                    htSearchBrowser.Add("KeyField", "Id");
                    break;
                #endregion
                #region ana virüs ata
                case "cmbMainIssueId":
                    dtQuery.Rows.Add("SELECT t1.IndexId,t1.Baslik,t1.BildirimTarihi,t1.TeslimTarihi FROM Issue as t1 With (nolock) WHERE ");
                    dtQuery.Rows.Add("t1.Active=1 and ISNULL(t1.MainIssueId,0)=0 and t1.durumId<>5 ");
                    dtQuery.Rows.Add("AND exists(select 1 from UserAllowedIssueList with (nolock) where IssueId=t1.IndexId and UserId=");
                    dtQuery.Rows.Add("(select IndexId from SecurityUsers where Username='" + Membership.GetUser().UserName + "'))");
                    dtQuery.Rows.Add("order by t1.IndexId desc");
                    dtFields.Rows.Add("Baslik", "PNR", 150, true);
                    dtFields.Rows.Add("IndexId", "Taným", 50, true);
                    dtFields.Rows.Add("BildirimTarihi", "Bildirim Tarihi", 100, true);
                    dtFields.Rows.Add("TeslimTarihi", "Operasyon Tarihi", 100, true);
                    htSearchBrowser.Add("Title", "Ana Gündemler");
                    htSearchBrowser.Add("Query", dtQuery);
                    htSearchBrowser.Add("Parameters", dtParameters);
                    htSearchBrowser.Add("Fields", dtFields);
                    htSearchBrowser.Add("KeyField", "IndexId");
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
        StringBuilder sb = new StringBuilder();
        SqlCommand cmd;
        IDataReader rdr;
        if (!String.IsNullOrEmpty(e.Parameter))
        {

            string[] parameters = e.Parameter.Split('|');
            switch (parameters[0].Trim())
            {
                case "GridSms_SmsId":
                    #region anket müþteri
                    cmd = DB.SQL(this.Context, "SELECT Id,PhoneNumber FROM PhoneBook WHERE Id=@Id");
                    DB.AddParam(cmd, "@Id", (new Guid(parameters[1].ToString().Trim())));
                    cmd.Prepare();
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        sb = new StringBuilder("|" + parameters[0].Trim());
                        sb.Append("|" + rdr["Id"].ToString());
                        sb.Append("|" + rdr["PhoneNumber"].ToString());
                        e.Result = sb.ToString();
                    }
                    else e.Result = null;
                    rdr.Close();
                    #endregion
                    break;
                default:
                    break;
            }
        }
    }

    protected void GridSms_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["SmsId"] == null)
        {
            e.RowError = "Lütfen Gsm alanýný boþ býrakmayýnýz...";
            return;
        }
        if (Roles.IsUserInRole(Membership.GetUser().UserName, "Maðaza Çalýþanlarý"))
        {

            e.RowError = "Sms gönderme opsiyonu yanlýzca merkez kullanýcýlarýna ait bir operasyondur!";
        }
    }

    protected void MainIssueId_Callback(object source, CallbackEventArgsBase e)
    {
        this.MainIssueId.Items.Clear();
        if (!String.IsNullOrEmpty(e.Parameter))
        {
            int id = Convert.ToInt32(e.Parameter);
            FillMainIssueId(this.MainIssueId, id);
            Session["MainIssueId"] = id.ToString();
        }
    }

    private void FillMainIssueId(ASPxComboBox source, int id)
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT IndexId,cast(IndexId as nvarchar(20))+'/ '+left(Isnull(Baslik,''),100) as Title FROM Issue With (nolock) WHERE IndexId=@Id");
        DB.AddParam(cmd, "@Id", id);
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        if (rdr.Read())
        {
            ListEditItem item = new ListEditItem();
            item.Text = rdr["Title"].ToString();
            item.Value = rdr["IndexId"];
            source.Items.Add(item);
        }
        rdr.Close();
        source.SelectedIndex = 0;
    }

    //protected void GridSelectedSms_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    //{
    //    if (e.Parameters != null)
    //    {
    //        if (Convert.ToBoolean(e.Parameters.ToString()))
    //            this.GridSelectedSms.Selection.SelectAll();
    //        else
    //            this.GridSelectedSms.Selection.UnselectAll();
    //    }
    //}

    #region //Br Kodlarý...
    //private void InitGridDTBrKodlari(DataTable dt)
    //{
    //    dt.Columns.Add("ID", typeof(Guid));
    //    dt.Columns.Add("BrKodID", typeof(Guid));
    //    dt.Columns.Add("IssueID", typeof(Guid));
    //    dt.Columns.Add("BrKodu", typeof(string));
    //    dt.Columns.Add("CreatedBy", typeof(string));
    //    dt.Columns.Add("ModifiedBy", typeof(string));
    //    dt.Columns.Add("CreationDate", typeof(DateTime));
    //    dt.Columns.Add("ModificationDate", typeof(DateTime));

    //    DataColumn[] pricols = new DataColumn[1];
    //    pricols[0] = dt.Columns["ID"];
    //    dt.PrimaryKey = pricols;
    //}

    #endregion

    #region //Alt Gündem doluyor...
    private void InitGridDtAltGundem(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Baslik", typeof(string));
        dt.Columns.Add("IndexID", typeof(int));
        dt.Columns.Add("Durum", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("TeslimTarihi", typeof(DateTime));
        dt.Columns.Add("BildirimTarihi", typeof(DateTime));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModificationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    #endregion

    #region //Alt Dosyalar
    private void InitGridDTDTAltDosyalar(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("DosyaYolu", typeof(string));
        dt.Columns.Add("literal", typeof(string));
        dt.Columns.Add("DosyaAdi", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }
    #endregion

}
