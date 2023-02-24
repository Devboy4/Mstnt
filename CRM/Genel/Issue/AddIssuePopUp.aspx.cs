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
using DevExpress.Web.ASPxClasses.Internal;

public partial class CRM_Genel_Issue_AddIssuePopUp : System.Web.UI.Page
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
        if (Request.QueryString["Ids"] != null)
        {
            Response.Write("<script language='Javascript'>{ parent.opener.location.href='./GundemGiris.aspx'; }</script>");
            if (!String.IsNullOrEmpty((String)Request.QueryString["Ids"]))
                InitialiseMeetingIssue((String)Request.QueryString["Ids"]);
        }
        InitGridDTRelatedUsers(this.DTRelatedUsers.Table);
        InitGridNotDosya(this.DataTableNotDosya.Table);

        fillcomboxes();
        this.HiddenID.Value = GetUserID(Membership.GetUser().UserName).ToString();
        ControlDegerProject();
        this.TeslimTarihi.MinDate = DateTime.Now.AddDays(-1);
        this.TeslimTarihi.Date = DateTime.Now;
        this.HiddenNewIssueId.Value = Guid.NewGuid().ToString();
        this.HiddenNotId.Value = Guid.Empty.ToString();

    }

    private void InitialiseMeetingIssue(string Ids)
    {
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataSet ds = new DataSet();
        sb.Append("Select (CAST(IndexId AS NVARCHAR(20)) +' / '+ LEFT(Baslik,50)) AS Tanim FROM Issue WITH (NOLOCK) WHERE IndexId in (" + Ids + ")");
        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        cmd.Prepare();
        adapter.SelectCommand = cmd;
        adapter.Fill(ds);
        StringBuilder sb1 = new StringBuilder();
        int sayac = 1;
        sb1.AppendLine("TOPLANTI OLUÞTURULDU...;");
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            sb1.AppendLine(sayac.ToString() + " - " + row["Tanim"].ToString());
            sayac++;
        }
        this.Description.Text = sb1.ToString();
    }

    private void SetGridForUsers(int id)
    {
        if (id == 205) return;
        DataTable dt = new DataTable();
        try
        {

            grid1.Selection.UnselectAll();
            StringBuilder sb = new StringBuilder();
            sb.Append("Select t3.IndexId UserId From VirusSinifUsers t1 left join VirusSinif t2 on (t1.VirusSinifID=t2.VirusSinifId) ");
            sb.Append("left join SecurityUsers t3 on (t1.UserId=t3.UserId) ");
            sb.Append("Where t2.IndexId=@Id and t3.Active=1");
            using (SqlCommand cmd = DB.SQL(this.Context, sb.ToString()))
            {
                DB.AddParam(cmd, "@Id", id);
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
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
            }

        }
        catch { }
        finally
        {
            if (dt != null)
            {
                dt.Dispose();
                dt = null;
            }
        }
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
                //this.grid1.FilterExpression = string.Empty;
                //this.grid1.ExpandAll();
                //this.grid1.DataBind();
                //if (Convert.ToBoolean(spt[1]))
                //    this.grid1.Selection.SelectAll();
                //else
                //    this.grid1.Selection.UnselectAll();
                break;
            case "gridbind":
                grid1.DataBind();
                break;
            case "SetUsers":
                int _Id = int.Parse(spt[1]);
                //if (_Id == 205)
                //{
                //    BindRelatedTableV1(Membership.GetUser().UserName);
                //}
                //else
                //{
                //    BindRelatedTable(Membership.GetUser().UserName);
                //    SetGridForUsers(_Id);
                //}
                SetGridForUsers(_Id);
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
        //data.BindComboBoxesNoEmpty(this.Context, GundemDerecesiId, "SELECT IndexId GundemDereceId,Adi FROM GundemDereceleri ORDER BY Adi", "GundemDereceId", "Adi");
        data.BindComboBoxesNoEmpty(this.Context, VirusSinifID, "EXEC SP_GetVirusSinifCreateIssue '" + Membership.GetUser().UserName + "'", "VirusSinifID", "Adi");
        DurumID.SelectedIndex = 1;
        try { OnemDereceID.SelectedIndex = OnemDereceID.Items.IndexOfText("Düþük"); }
        catch { }

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

    private void InitGridNotDosya(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("NotDosyaID", typeof(Guid));
        dt.Columns.Add("NotID", typeof(Guid));
        dt.Columns.Add("DosyaAdi", typeof(string));
        dt.Columns.Add("DosyaBoyut", typeof(int));
        dt.Columns.Add("BoyutTuru", typeof(string));
        dt.Columns.Add("DosyaYolu", typeof(string));
        dt.Columns.Add("Link", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("AllowedRoles", typeof(string));
        dt.Columns.Add("DeniedRoles", typeof(string));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    protected void ProjeID_Callback(object source, CallbackEventArgsBase e)
    {
        if (string.IsNullOrEmpty(e.Parameter.ToString())) return;

        StringBuilder sb = new StringBuilder();
        sb.Append("EXEC AllowedProjeListV3 '" + Membership.GetUser().UserName.ToLower() + "','" + e.Parameter.ToString() + "'");
        data.BindComboBoxesNoEmpty(this.Context, ProjeID, sb.ToString(), "ProjeID", "Adi");
        this.ProjeID.SelectedIndex = this.ProjeID.Items.IndexOfValue(int.Parse(this.HiddenID.Value.ToString()));

    }

    protected void MenuFirma_ItemClick(object source, MenuItemEventArgs e)
    {

        if (!Security.CheckPermission(this.Context, "Bildirim Ekle", "Insert"))
        {
            CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
            return;

        }

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
            if (id == -1)
            {
                CrmUtils.CreateMessageAlert(this.Page, (String)Session["Hata"], "stkey1");
                return;
            }
            else
            {
                if (Request.QueryString["ProcId"] != null)
                {
                    Response.Redirect("~/CRM/Genel/Issue/GundemGiris.aspx");
                }
                else
                {
                    this.Response.Write("<script language='javascript'>{ parent.opener.Grid.PerformCallback('x'); }</script>");
                    this.Response.Write("<script language='javascript'>{ parent.close(); }</script>");
                }
            }


        }

        #endregion

        #region saveclose
        else if (e.Item.Name.Equals("saveclose"))
        {

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
                if (Request.QueryString["ProcId"] != null)
                {
                    Response.Redirect("~/CRM/Genel/Issue/GundemGiris.aspx");
                }
                else
                {
                    this.Response.Write("<script language='javascript'>{ parent.opener.Grid.PerformCallback('x'); }</script>");
                    this.Response.Write("<script language='javascript'>{ parent.close(); }</script>");
                }
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
            Guid id = new Guid(this.HiddenNewIssueId.Value);
            StringBuilder sb = new StringBuilder();
            sb = new StringBuilder();
            sb.Append("EXEC OnAddIssue ");
            sb.Append("@IssueID,@FirmaID,@ProjeID,@UserID,@DurumID,@OnemDereceID,@Active,@Baslik,@Description,@HataAdimlari,@KeyWords");
            sb.Append(",@BildirimTarihi,@CreatedBy,@ModifiedBy,@ModificationDate,@CreationDate,@TeslimTarihi,@GundemDosyaYolu,@GundemDosyaAdi");
            sb.Append(",@VirusSinifID,@GundemDereceId,@NoteId");

            SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@IssueID", id);
            DB.AddParam(cmd, "@NoteId", new Guid(this.HiddenNotId.Value));
            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
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
            //tmpID = int.Parse(this.GundemDerecesiId.Value.ToString());
            DB.AddParam(cmd, "@GundemDereceId", 1);
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
            //SaveRelatedIssues(IssueIndex);
            DB.Commit(this.Context);
            if (CheckSendSms.Checked)
            {
                if (String.IsNullOrEmpty(numbers)) return IssueIndex;
                string MessageBody = IssueIndex.ToString() + " - " + Membership.GetUser().UserName + " - " + this.Description.Text.ToUpper();
                sms.SmsThreadStarter(numbers, MessageBody, MessageBody.Length);
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
                    if (!String.IsNullOrEmpty(emails))
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
        sb.Append("EXEC InsertUserAllowedIssueList @Ids,@IssueID,@CreationDate,@CreatedBy");

        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@Ids", 3500, Ids);
        DB.AddParam(cmd, "@IssueID", MainIssueID);
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
            using (SqlCommand cmd = DB.SQL(this.Context, "EXEC ControlDegerProject '" + Membership.GetUser().UserName + "'"))
            {
                String ControlConfig = (String)cmd.ExecuteScalar();
                if (ControlConfig == null || ControlConfig == "Atama Yok") return;
                char[] seps = { '|' };
                string[] Columns = ControlConfig.ToString().Split(seps);
                string Projeid = Columns[0].ToString();
                string Firmaid = Columns[1].ToString();
                this.HiddenFId.Value = Columns[1].ToString();
                this.HiddenPId.Value = Columns[0].ToString();
                StringBuilder sb = new StringBuilder();
                sb.Append("EXEC AllowedProjeListV2 '" + Membership.GetUser().UserName.ToLower() + "','" + Firmaid + "'");
                data.BindComboBoxesNoEmpty(this.Context, ProjeID, sb.ToString(), "ProjeID", "Adi");
                ProjeID.SelectedIndex = ProjeID.Items.IndexOfValue(int.Parse(Projeid));


                data.BindComboBoxesNoEmpty(this.Context, UserID, "EXEC ISAssignUserByUserName '" + Membership.GetUser().UserName + "','" + Projeid + "'", "UserID", "UserName");
                UserID.SelectedIndex = this.UserID.Items.IndexOfValue(int.Parse(this.HiddenID.Value.ToString()));


                FirmaID.SelectedIndex = FirmaID.Items.IndexOfValue(int.Parse(Firmaid));
                BindRelatedTable(Membership.GetUser().UserName);

                FirmaID.ClientEnabled = false;
                ProjeID.ClientEnabled = false;
                UserID.ClientEnabled = false;
            }
        }
        catch (Exception ex)
        {
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
                using (IDataReader rdr = cmd.ExecuteReader())
                {
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
                }
                this.DTRelatedUsers.Table.AcceptChanges();
                grid1.DataBind();
                grid1.ExpandAll();
            }
        }
        catch { }
    }

    private void BindRelatedTableV1(string username)
    {
        try
        {
            using (SqlCommand cmd = DB.SQL(this.Context, "EXEC ISAssignUserByUserNameV6 @UserName"))
            {
                DB.AddParam(cmd, "@UserName", 100, username);
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                using (IDataReader rdr = cmd.ExecuteReader())
                {
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
                }
                this.DTRelatedUsers.Table.AcceptChanges();
                grid1.DataBind();
                grid1.ExpandAll();
            }
        }
        catch { }
    }

    private void SaveNote()
    {
        SqlCommand cmd = null;
        StringBuilder sb = new StringBuilder();

        sb.Append("INSERT INTO Notes(");
        sb.Append("NotID");
        sb.Append(",BagliID");
        sb.Append(",BagliNesneTipi");
        sb.Append(",Tanim");
        sb.Append(",Aciklama");
        sb.Append(",AllowedRoles");
        sb.Append(",DeniedRoles");
        sb.Append(",CreatedBy");
        sb.Append(",CreationDate");
        sb.Append(")");

        sb.Append("VALUES (");
        sb.Append("@NotID");
        sb.Append(",@BagliID");
        sb.Append(",@BagliNesneTipi");
        sb.Append(",@Tanim");
        sb.Append(",@Aciklama");
        sb.Append(",@AllowedRoles");
        sb.Append(",@DeniedRoles");
        sb.Append(",@CreatedBy");
        sb.Append(",@CreationDate");
        sb.Append(")");

        cmd = DB.SQL(this.Context, sb.ToString());

        Guid id = Guid.NewGuid();
        DB.AddParam(cmd, "@NotID", id);
        DB.AddParam(cmd, "@CreationDate", DateTime.Now);
        DB.AddParam(cmd, "@CreatedBy", 50, Membership.GetUser().UserName);
        DB.AddParam(cmd, "@BagliID", -1);
        DB.AddParam(cmd, "@BagliNesneTipi", 1);
        DB.AddParam(cmd, "@Tanim", 255, this.Tanim.Text);
        DB.AddParam(cmd, "@Aciklama", 500, "");
        DB.AddParam(cmd, "@AllowedRoles", 255, "");
        DB.AddParam(cmd, "@DeniedRoles", 255, "");

        cmd.Prepare();
        cmd.ExecuteNonQuery();

        this.HiddenNotId.Value = id.ToString();
    }

    private void LoadNoteDosya()
    {
        SqlCommand cmd = null;
        StringBuilder sb;
        this.DataTableNotDosya.Table.Clear();

        sb = new StringBuilder();
        sb.Append("SELECT * FROM NotDosya WHERE NotID=@NotID");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@NotID", new Guid(this.HiddenNotId.Value));
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableNotDosya.Table.NewRow();

            row["ID"] = rdr["NotDosyaID"];
            row["NotDosyaID"] = rdr["NotDosyaID"];
            if ((rdr["NotID"] != null) && (rdr["NotID"].ToString() != Guid.Empty.ToString()))
                row["NotID"] = rdr["NotID"];
            row["DosyaAdi"] = rdr["DosyaAdi"];
            row["DosyaBoyut"] = rdr["DosyaBoyut"];
            row["BoyutTuru"] = rdr["BoyutTuru"];
            row["DosyaYolu"] = rdr["DosyaYolu"];
            row["CreationDate"] = rdr["CreationDate"];
            row["AllowedRoles"] = rdr["AllowedRoles"];
            row["DeniedRoles"] = rdr["DeniedRoles"];

            sb = new StringBuilder();
            sb.Append("<a href='./../../Notes/download.aspx?id=" + rdr["NotDosyaID"].ToString() + "' target='_self'>");
            sb.Append("<img border=0 src='./../../../images/" + NotesUtils.GetFileIcon(rdr["DosyaAdi"].ToString()) + "'" + "/>");
            sb.Append(rdr["DosyaAdi"].ToString() + "</a><br>");

            row["Link"] = sb.ToString();

            this.DataTableNotDosya.Table.Rows.Add(row);
        }
        rdr.Close();

        this.DataTableNotDosya.Table.AcceptChanges();
        this.GridNotDosya.DataBind();
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

    protected void BtnDosyaEkle_Click(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(this.Tanim.Text))
        {
            CrmUtils.MessageAlert(this.Page, "Not Baþlýk alaný boþ geçilemez!", "stkey1");
            return;
        }
        if (this.fileUpload.HasFile)
        {
            if (!NotesUtils.CheckFile(this.Page, this.Context, this.fileUpload))
                return;

            int BagliID = -1;
            int NesneTipi = 1;
            string _unvan = Guid.NewGuid().ToString();

            //if (this.Description.Text.Length > 20) _unvan = this.Description.Text.Substring(0, 20).ToUpper().TrimEnd().TrimStart();

            string sID = this.HiddenID.Value.ToString().Trim();

            if (this.HiddenNotId.Value == Guid.Empty.ToString())
            {
                SaveNote();

            }

            Guid NotID = new Guid(this.HiddenNotId.Value);

            if (!NotesUtils.SaveFile(this.Page, this.Context, this.fileUpload, NotID, BagliID, NesneTipi, _unvan, this.HiddenNewIssueId.Value))
                return;

            LoadNoteDosya();

        }
    }

    protected void BtnDeleteNote_Click(object sender, EventArgs e)
    {
        DataTable changes = this.DataTableNotDosya.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Deleted:
                        Guid NotDosyaID = (Guid)row["ID", DataRowVersion.Original];
                        if (!NotesUtils.DeleteFile(this.Page, this.Context, NotDosyaID))
                        {
                            return;
                        }
                        break;
                }
            }
        }

        this.DataTableNotDosya.Table.AcceptChanges();
        LoadNoteDosya();
    }



}
