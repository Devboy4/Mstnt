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

public partial class CRM_Genel_Issue_AddSubIssue : System.Web.UI.Page
{
    CrmUtils data = new CrmUtils();
    SmsUtils sms = new SmsUtils();

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
        InitGridDTRelatedUsers(DTRelatedUsers.Table);
        fillcomboxes();
        #region ana virüs bilgileri
        if ((String)Request.QueryString["SubId"] != null)
        {
            try
            {
                SqlCommand cmd = DB.SQL(this.Context, "Select Left(IsNull(Baslik,''),100)+' ['+IsNull(Convert(nvarChar(20),IndexID),'')+']' From Issue with (nolock) Where IndexId=@IssueID");
                DB.AddParam(cmd, "@IssueID", int.Parse((String)Request.QueryString["SubId"]));
                cmd.Prepare();
                string Bilgiler = (string)cmd.ExecuteScalar();
                AnaVirüsBilgisi.Value = Bilgiler;
            }
            catch { }
        }
        #endregion
        this.HiddenID.Value = GetUserID(Membership.GetUser().UserName).ToString();
        ControlDegerProject();

        this.TeslimTarihi.Date = DateTime.Now;
        this.TeslimTarihi.MinDate = DateTime.Now.AddDays(-1);
    }

    private void SetGridForUsers(int id)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            sb.Append("Select t3.IndexId UserId From VirusSinifUsers t1 left join VirusSinif t2 on (t1.VirusSinifID=t2.VirusSinifId) ");
            sb.Append("left join SecurityUsers t3 on (t1.UserId=t3.UserId) ");
            sb.Append("Where t2.IndexId=@Id");
            SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@Id", id);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                grid1.Selection.UnselectAll();
                return;
            }
            for (int counter = 0; counter < grid1.VisibleRowCount; counter++)
            {
                DataRow[] rows = dt.Select("UserId='" + grid1.GetRowValues(counter, "UserID") + "'");
                if (rows.Length > 0)
                    grid1.Selection.SelectRow(counter);
            }          

        }
        catch { }
    }

    protected void grid1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (String.IsNullOrEmpty(e.Parameters)) return;
        string[] spt = e.Parameters.Split('|');
        switch (spt[0])
        {
            case "Select":
                if (Convert.ToBoolean(spt[1]))
                {

                    for (int counter = 0; counter < grid1.VisibleRowCount; counter++)
                    {
                        if (Convert.ToInt32(grid1.GetRowValues(counter, "IsVisible")) == 1)
                            grid1.Selection.SelectRow(counter);
                    }
                }
                else
                {
                    for (int counter = 0; counter < grid1.VisibleRowCount; counter++)
                    {
                        if (Convert.ToInt32(grid1.GetRowValues(counter, "IsVisible")) == 1)
                            grid1.Selection.UnselectRow(counter);
                    }
                    //this.grid1.Selection.UnselectAll();
                }
                //if (Convert.ToBoolean(spt[1]))
                //    this.grid1.Selection.SelectAll();
                //else
                //    this.grid1.Selection.UnselectAll();
                break;
            case "gridbind":
                grid1.DataBind();
                break;
            case "SetUsers":
                SetGridForUsers(int.Parse(spt[1]));
                break;
        }
    }


    protected void grid_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
    {

        if (e.CallbackName == "APPLYCOLUMNFILTER" || e.CallbackName == "APPLYFILTER")
        {
            ((ASPxGridView)sender).ExpandAll();
        }
        //grid.ExpandAll();

    }

    private int GetUserID(string UserID)
    {
        int id = 0;
        try
        {
            SqlCommand cmd = DB.SQL(this.Context, "Select IndexId UserID From SecurityUsers Where UserName=@UserName");
            DB.AddParam(cmd, "@UserName", 100, UserID);
            cmd.Prepare();
            id = (int)cmd.ExecuteScalar();
        }
        catch
        {
            id = -1;
        }
        return id;
    }

    void fillcomboxes()
    {
        data.BindComboBoxesNoEmpty(this.Context, FirmaID, "EXEC FirmaListByUserName '" + Membership.GetUser().UserName + "'", "FirmaID", "FirmaName");
        data.BindComboBoxesNoEmpty(this.Context, DurumID, "Select IndexId DurumID,Adi From Durum Where IlkGetir=1", "DurumID", "Adi");
        data.BindComboBoxesNoEmpty(this.Context, OnemDereceID, "SELECT IndexId OnemDereceID,Adi FROM OnemDereceleri ORDER BY Sira", "OnemDereceID", "Adi");
        data.BindComboBoxesNoEmpty(this.Context, VirusSinifID, "SELECT isnull(cast(IndexId as nvarchar(6)),'-1')+'|'+isnull(cast(OperasyonDay as nvarchar(6)),'0')+'|'+isnull(cast(cast(IsDateChange as int) as nvarchar(6)),'0')  VirusSinifID,Adi FROM VirusSinif ORDER BY Adi", "VirusSinifID", "Adi");
        DurumID.SelectedIndex = 1;
    }

    private void InitGridDTRelatedUsers(DataTable dt)
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

    protected void ProjeID_Callback(object source, CallbackEventArgsBase e)
    {
        if (e.Parameter.ToString() == null || e.Parameter.ToString() == "")
            return;
        StringBuilder sb = new StringBuilder();
        sb.Append("EXEC AllowedProjeListV3 '" + Membership.GetUser().UserName.ToLower() + "','" + e.Parameter.ToString() + "'");
        data.BindComboBoxesNoEmpty(this.Context, ProjeID, sb.ToString(), "ProjeID", "Adi");
        this.ProjeID.SelectedIndex = this.ProjeID.Items.IndexOfValue(int.Parse(this.HiddenID.Value.ToString()));
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
            int id = SaveDocument();
            if (id != -1)
            {
                //sb = new StringBuilder();
                //sb.Append("location.href='./GundemGiris.aspx'");
                //CrmUtils.CreateJavaScript(this.Page, "stkey1", sb.ToString());
                Response.Redirect("GundemGiris.aspx");
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
            int id = SaveDocument();
            if (id != -1)
            {
                sb = new StringBuilder();
                sb.Append("SaveAndNew.aspx?id=");
                sb.Append(id.ToString());
                sb.Append("&SaveText=");
                sb.Append(this.Description.Text);
                sb.Append("'");
                //}
                //CrmUtils.CreateJavaScript(this.Page, "stkey1", sb.ToString());
                Response.Redirect(sb.ToString());
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
            int id = SaveDocument();
            if (id != -1)
            {
                sb = new StringBuilder();
                sb.Append("location.href='./GundemGiris.aspx';");
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

    private int SaveDocument()
    {
        int IssueIndex = -1;
        try
        {
            DB.BeginTrans(this.Context);
            Guid id = Guid.NewGuid();
            StringBuilder sb = new StringBuilder();
            sb = new StringBuilder();
            sb.Append("EXEC OnAddSubIssue ");
            sb.Append("@IssueID,@MainIssueID,@FirmaID,@ProjeID,@UserID,@DurumID,@OnemDereceID,@Active,@Baslik,@Description,@HataAdimlari,@KeyWords");
            sb.Append(",@BildirimTarihi,@CreatedBy,@ModifiedBy,@ModificationDate,@CreationDate,@TeslimTarihi,@GundemDosyaYolu,@GundemDosyaAdi");
            sb.Append(",@VirusSinifID");

            SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@IssueID", id);
            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
            DB.AddParam(cmd, "@MainIssueID", int.Parse((String)Request.QueryString["SubId"]));
            DB.AddParam(cmd, "@ModifiedBy", 50, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@CreatedBy", 50, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@Active", 1);
            DB.AddParam(cmd, "@Baslik", 4000, this.Description.Text.ToUpper());
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
            int tmpID;
            tmpID = int.Parse(this.FirmaID.Value.ToString());
            DB.AddParam(cmd, "@FirmaID", tmpID);
            string[] vsarray = null;
            if (this.VirusSinifID.Text != "")
            {
                vsarray = this.VirusSinifID.Value.ToString().Split('|');
                tmpID = int.Parse(vsarray[0]);
                DB.AddParam(cmd, "@VirusSinifID", tmpID);
            }
            else
                DB.AddParam(cmd, "@VirusSinifID", SqlDbType.Int);
            tmpID = int.Parse(this.ProjeID.Value.ToString());
            DB.AddParam(cmd, "@ProjeID", tmpID);
            tmpID = int.Parse(this.DurumID.Value.ToString());
            DB.AddParam(cmd, "@DurumID", tmpID);
            tmpID = int.Parse(this.UserID.Value.ToString());
            DB.AddParam(cmd, "@UserID", tmpID);
            DB.AddParam(cmd, "@KeyWords", SqlDbType.Int);
            tmpID = int.Parse(this.OnemDereceID.Value.ToString());
            DB.AddParam(cmd, "@OnemDereceID", tmpID);
            object MailConfigs = (object)cmd.ExecuteScalar();

            #region Mail istenirse açýlacak
            //char[] seps = { '|' };
            //string[] Columns = MailConfigs.ToString().Split(seps);
            //string MailAdress = Columns[0].ToString();
            //string MailBody = Columns[1].ToString();
            //string MailSubject = Columns[2].ToString();
            //char[] seps2 ={ ';' };
            //string[] Columns2 = MailAdress.Split(seps2);
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


            IssueIndex = int.Parse(MailConfigs.ToString());
            string numbers = SaveRelatedIssues(IssueIndex);
            DB.Commit(this.Context);

            if (CheckSendSms.Checked)
            {
                if (String.IsNullOrEmpty(numbers)) return IssueIndex;
                string MessageBody = (String)Request.QueryString["SubId"] + "/" + IssueIndex.ToString() + " - " + Membership.GetUser().UserName + " - " + this.Description.Text.ToUpper();
                int MessageBodySize = this.Description.Text.Length;
                string Numbers = numbers;
                sms.SmsThreadStarter(numbers, MessageBody, MessageBodySize);
            }
            return IssueIndex;
        }
        catch (Exception ex)
        {
            Session["Hata"] = "Gündem eklenemedi lütfen daha sonra tekrar deneyiniz!";
            DB.Rollback(this.Context);
            return -1;
        }

    }

    private string SaveRelatedIssues(int MainIssueID)
    {
        grid1.UpdateEdit();
        List<object> keyValues = this.grid1.GetSelectedFieldValues("ID");
        string Ids = string.Empty;
        string numbers = string.Empty;
        string emails = string.Empty;
        bool ilk = true;
        bool bothfree = false;
        foreach (object key in keyValues)
        {
            DataRow row2 = DTRelatedUsers.Table.Rows.Find(key);
            if (ilk)
            {
                if (row2["CepTel"] != null & row2["CepTel"].ToString() != "")
                    numbers += row2["CepTel"].ToString();
                if (row2["Email"] != null & row2["Email"].ToString() != "")
                    emails += row2["Email"].ToString();
                Ids += row2["UserId"].ToString();
                ilk = false;
            }
            else
            {
                Ids += "," + row2["UserId"].ToString();
                if (row2["Email"] != null & row2["Email"].ToString() != "")
                {
                    if (!String.IsNullOrEmpty(numbers))
                        emails += "," + row2["Email"].ToString();
                    else
                        emails += row2["Email"].ToString();
                }
                if (row2["CepTel"] != null & row2["CepTel"].ToString() != "")
                {
                    if (!String.IsNullOrEmpty(numbers))
                        numbers += "," + row2["CepTel"].ToString();
                    else
                        numbers += row2["CepTel"].ToString();
                }
            }

        }

        if (keyValues.Count == 0) bothfree = true;
        if (bothfree) Ids = UserID.Value.ToString();
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder();
        sb.Append("EXEC InsertUserAllowedIssueListForSub @Ids,@IssueID,@MainIssueID,@CreationDate,@CreatedBy");

        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@Ids", 3500, Ids);
        DB.AddParam(cmd, "@IssueID", MainIssueID);
        DB.AddParam(cmd, "@MainIssueID", int.Parse((String)Request.QueryString["SubId"]));
        DB.AddParam(cmd, "@CreationDate", DateTime.Now);
        DB.AddParam(cmd, "@CreatedBy", 50, Membership.GetUser().UserName);
        cmd.Prepare();
        cmd.ExecuteNonQuery();
        return numbers;
    }

    protected void UserID_Callback1(object source, CallbackEventArgsBase e)
    {
        data.BindComboBoxesNoEmpty(this.Context, UserID, "EXEC ISAssignUserByUserName '" + Membership.GetUser().UserName + "','" + e.Parameter + "'", "UserID", "UserName");
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
            ProjeID.SelectedIndex = ProjeID.Items.IndexOfValue(int.Parse(Projeid));


            data.BindComboBoxesNoEmpty(this.Context, UserID, "EXEC ISAssignUserByUserName '" + Membership.GetUser().UserName + "','" + Projeid + "'", "UserID", "UserName");
            UserID.SelectedIndex = this.UserID.Items.IndexOfValue(int.Parse(this.HiddenID.Value.ToString()));


            FirmaID.SelectedIndex = FirmaID.Items.IndexOfValue(int.Parse(Firmaid));
            BindRelatedTable(Membership.GetUser().UserName);
        }
        catch (Exception ex)
        {
        }

    }

    protected void grid1_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
    {

        if (e.CommandCellType == DevExpress.Web.ASPxGridView.GridViewTableCommandCellType.Data)
        {
            if (grid1.GetRowValues(e.VisibleIndex, "IsVisible").ToString().ToLower() == "0")
            {
                ((WebControl)e.Cell.Controls[0]).Attributes["disabled"] = "true";
            }
        }


    }

    private void BindRelatedTable(string username)
    {
        try
        {
            using (SqlCommand cmd = DB.SQL(this.Context, "EXEC ISAssignUserByUserNameV3 @UserName"))
            {
                DB.AddParam(cmd, "@UserName", 100, username);
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                IDataReader rdr = cmd.ExecuteReader();
                this.DTRelatedUsers.Table.Rows.Clear();
                while (rdr.Read())
                {

                    DataRow row = this.DTRelatedUsers.Table.NewRow();
                    row["ID"] = rdr["UserID"];
                    row["UserID"] = rdr["UserID"];
                    row["Adi"] = rdr["UserName"];
                    row["FirmaName"] = rdr["FirmaName"];
                    row["ProjeName"] = rdr["ProjeName"];
                    row["Email"] = rdr["Email"];
                    row["CepTel"] = rdr["CepTel"];
                    row["FirmaID"] = rdr["FirmaID"];
                    row["ProjeID"] = rdr["ProjeID"];
                    row["UserN"] = rdr["UserN"];
                    row["IsVisible"] = rdr["IsVisible"];
                    this.DTRelatedUsers.Table.Rows.Add(row);

                }
                rdr.Close();
                this.DTRelatedUsers.Table.AcceptChanges();
                grid1.DataBind();
                grid1.ExpandAll();
            }
        }
        catch { }
    }
}
