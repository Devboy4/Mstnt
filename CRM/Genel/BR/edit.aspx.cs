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

public partial class CRM_Genel_BR_edit : System.Web.UI.Page
{
    CrmUtils data = new CrmUtils();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Security.CheckPermission(this.Context, "BR", "Update"))
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
        InitGridTable(DTIssueActivity.Table);
        int id = 0;
        string sID = this.Request.Params["id"].Replace("'", "");
        if ((sID != null) && (sID.Trim() != "0"))
        {
            id = int.Parse(sID);
            this.HiddenID.Value = id.ToString();
            LoadDocument(id);
        }
        else
        {
            Tarih.Date = DateTime.Now;
            //isteyenProjeID.SelectedIndex = 0;
            BBrNo.Visible = false;
            IndexID.Visible = false;
            BCreatedBy.Visible = false;
            IssuedBy.Visible = false;
            BIssueDate.Visible = false;
            IssuedDate.Visible = false;
            BModifiedBy.Visible = false;
            ModifiedBy.Visible = false;
            BModificationDate.Visible = false;
            ModificationDate.Visible = false;
        }
        #region admin vb.. yetkilendirme
        if (Roles.IsUserInRole(Membership.GetUser().UserName, "Merkez Çalýþanlarý") ||
            Roles.IsUserInRole(Membership.GetUser().UserName, "Merkez Yöneticileri") ||
            Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator"))
        {

        }
        else
            istenilenProjeID.ReadOnly = true;
        #endregion

        

    }

    void fillcomboxes()
    {
        data.BindComboBoxesNoEmpty(this.Context, BrMarkaID, "Select BrMarkaID,Adi From BrMarka Order By Adi", "BrMarkaID", "Adi");
        data.BindComboBoxesNoEmpty(this.Context, isteyenProjeID, "Select ProjeID,Adi From Proje Order By Adi", "ProjeID", "Adi");
        data.BindComboBoxesNoEmpty(this.Context, istenilenProjeID, "Exec UserAllowedProjects '" + Membership.GetUser().UserName + "'", "ProjeID", "Adi");
        data.BindComboBoxesNoEmpty(this.Context, BrDurumID, "Select BrDurumID,Adi From BrDurum Order By Adi", "BrDurumID", "Adi");
    }

    private void LoadDocument(int id)
    {
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder();

        #region // Br Alanlar
        sb.Append("Select * From BrTablosu Where IndexId=@BrTablosuID");
        using (SqlCommand cmd = DB.SQL(this.Context, sb.ToString()))
        {
            DB.AddParam(cmd, "@BrTablosuID", id);
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();
            if (!rdr.Read())
            {
                this.Response.StatusCode = 500;
                this.Response.End();
                return;
            }
            this.Adet.Value = rdr["Adet"];
            this.OperasyonSuresi.Value = rdr["OperasyonSuresi"];
            this.StokKodu.Value = rdr["StokKodu"];
            this.Renk.Value = rdr["Renk"];
            this.Tarih.Value = rdr["Tarih"];
            this.Size.Value = rdr["Size"];
            this.irsaliyeNo.Value = rdr["irsaliyeNo"];
            this.MusteriAdi.Value = rdr["MusteriAdi"];
            this.MusteriTel.Value = rdr["MusteriTel"];
            this.PersonelAdi.Value = rdr["PersonelAdi"];

            if ((rdr["BrDurumID"].ToString() != null) && (rdr["BrDurumID"].ToString() != ""))
                this.BrDurumID.SelectedIndex = this.BrDurumID.Items.IndexOfValue(rdr["BrDurumID"]);
            if ((rdr["istenilenProjeID"].ToString() != null) && (rdr["istenilenProjeID"].ToString() != ""))
                this.istenilenProjeID.SelectedIndex = this.istenilenProjeID.Items.IndexOfValue(rdr["istenilenProjeID"]);
            if ((rdr["isteyenProjeID"].ToString() != null) && (rdr["isteyenProjeID"].ToString() != ""))
                this.isteyenProjeID.SelectedIndex = this.isteyenProjeID.Items.IndexOfValue(rdr["isteyenProjeID"]);
            if ((rdr["BrMarkaID"].ToString() != null) && (rdr["BrMarkaID"].ToString() != ""))
                this.BrMarkaID.SelectedIndex = this.BrMarkaID.Items.IndexOfValue(rdr["BrMarkaID"]);
            IndexID.Value = rdr["IndexID"].ToString();
            IssuedBy.Value = rdr["CreatedBy"].ToString();
            if (rdr["CreationDate"].ToString().Length > 0)
                this.IssuedDate.Value = Convert.ToDateTime(rdr["CreationDate"].ToString()).ToString("F", CultureInfo.CreateSpecificCulture("tr-TR"));
            if (rdr["ModificationDate"].ToString().Length > 0)
                this.ModificationDate.Value = Convert.ToDateTime(rdr["ModificationDate"].ToString()).ToString("F", CultureInfo.CreateSpecificCulture("tr-TR"));
            ModifiedBy.Value = rdr["ModifiedBy"].ToString();
            rdr.Close();
        }
        #endregion

        #region// BR Activite Doluyor

        sb = new StringBuilder();
        sb.Append("SELECT t1.*,t2.Adi DurumName FROM BRActivite t1 ");
        sb.Append("LEFT JOIN BrDurum t2 ON (t1.DurumID=t2.BrDurumID) LEFT JOIN BrTablosu t3 ON (t1.BrTablosuID=t3.BrTablosuID) ");
        sb.Append("WHERE t3.IndexId=@BrTablosuID ORDER BY t1.CommentDate DESC");
        using (SqlCommand cmd = DB.SQL(this.Context, sb.ToString()))
        {
            DB.AddParam(cmd, "@BrTablosuID", id);
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DataRow row = DTIssueActivity.Table.NewRow();
                row["ID"] = rdr["BRActiviteID"];
                row["Comment"] = rdr["Comment"].ToString();
                row["DurumName"] = rdr["DurumName"].ToString();
                row["CreatedBy"] = rdr["CreatedBy"].ToString();
                row["ModifiedBy"] = rdr["ModifiedBy"].ToString();
                row["CommentDate"] = rdr["CommentDate"].ToString();
                DTIssueActivity.Table.Rows.Add(row);
            }
            DTIssueActivity.Table.AcceptChanges();
            rdr.Close();
        }
        #endregion

        if (DTIssueActivity != null)
            DTIssueActivity.Dispose();

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
            if (!Security.CheckPermission(this.Context, "BR", "Insert"))
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
                if (!Security.CheckPermission(this.Context, "BR", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "BR", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                    return;
                }
            }

            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlýþ bilgi giriþi!", "stkey1");
                return;
            }
            if (BrDurumID.Text.ToUpper() == "ÝSTENDÝ" && String.IsNullOrEmpty(istenilenProjeID.Text))
            {
                CrmUtils.MessageAlert(this.Page, "Br Durumu istendi yapýyorsanýz istenilen maðaza girilmelidir.", "stkey1");
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
                Response.Write("<script language='Javascript'>{ parent.location.href='./edit.aspx?id=" + (String)Session["BelgeID"] + "&NoteOwner=4'; }</script>");
            }


        }

        #endregion

        #region savenew
        else if (e.Item.Name.Equals("savenew"))
        {
            if (bYeni)
            {
                if (!Security.CheckPermission(this.Context, "BR", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!'", "stkey1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "BR", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!'", "stkey1");
                    return;
                }
            }

            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlýþ bilgi giriþi!", "stkey1");
                return;
            }
            if (BrDurumID.Text.ToUpper() == "ÝSTENDÝ" && String.IsNullOrEmpty(istenilenProjeID.Text))
            {
                CrmUtils.MessageAlert(this.Page, "Br Durumu istendi yapýyorsanýz istenilen maðaza girilmelidir.", "stkey1");
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
                Response.Write("<script language='Javascript'>{ parent.location.href='./edit.aspx?id=0&NoteOwner=4'; }</script>");
            }
        }
        #endregion

        #region saveclose
        else if (e.Item.Name.Equals("saveclose"))
        {
            if (bYeni)
            {
                if (!Security.CheckPermission(this.Context, "BR", "Insert"))
                {
                    CrmUtils.MessageAlert(this.Page, "Kayýt iþlemi yapma yetkisine sahip deðilsiniz!'", "stkey1");
                    return;
                }
            }
            else
            {
                if (!Security.CheckPermission(this.Context, "BR", "Update"))
                {
                    CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!", "stkey1");
                    return;
                }
            }

            Validate();

            if (!this.IsValid)
            {
                this.Response.Write("<script language='javascript'>{ alert('Eksik veya yanlýþ bilgi giriþi!'); }</script>");
                return;
            }
            if (BrDurumID.Text.ToUpper() == "ÝSTENDÝ" && String.IsNullOrEmpty(istenilenProjeID.Text))
            {
                CrmUtils.MessageAlert(this.Page, "Br Durumu istendi yapýyorsanýz istenilen maðaza girilmelidir.", "stkey1");
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
            if (!Security.CheckPermission(this.Context, "BR", "Delete"))
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

    private int SaveDocument()
    {

        int id = -1;
        StringBuilder sb;
        if (this.HiddenID.Value != null && this.HiddenID.Value != "")
        {
            id =int.Parse(this.HiddenID.Value.ToString());

        }
        Guid guid1;
        SqlCommand cmd = null;
        DB.BeginTrans(this.Context);
        try
        {
            #region insert
            if (id == -1)
            {
                sb = new StringBuilder("Insert Into BrTablosu(");
                sb.Append("BrTablosuID");
                if (!CrmUtils.ControllToDate(this.Page, this.Tarih.Date.ToString()))
                    sb.Append(",Tarih");
                if (this.StokKodu.Value != null)
                    sb.Append(",StokKodu");
                if (this.Renk.Value != null)
                    sb.Append(",Renk");
                sb.Append(",Size");
                sb.Append(",BrMarkaID");
                sb.Append(",OperasyonSuresi");
                sb.Append(",isteyenProjeID");
                if (!String.IsNullOrEmpty(istenilenProjeID.Text))
                    sb.Append(",istenilenProjeID");
                sb.Append(",BrDurumID");
                if (this.irsaliyeNo.Value != null)
                    sb.Append(",irsaliyeNo");
                if (this.MusteriAdi.Value != null)
                    sb.Append(",MusteriAdi");
                if (this.MusteriTel.Value != null)
                    sb.Append(",MusteriTel");
                if (this.PersonelAdi.Value != null)
                    sb.Append(",PersonelAdi");
                if (this.Description.Text != "")
                    sb.Append(",Description");
                sb.Append(",Adet");
                sb.Append(",CreatedBy");
                sb.Append(",CreationDate) ");
                sb.Append("Values(");
                sb.Append("@BrTablosuID");
                if (!CrmUtils.ControllToDate(this.Page, this.Tarih.Date.ToString()))
                    sb.Append(",@Tarih");
                if (this.StokKodu.Value != null)
                    sb.Append(",@StokKodu");
                if (this.Renk.Value != null)
                    sb.Append(",@Renk");
                sb.Append(",@Size");
                sb.Append(",@BrMarkaID");
                sb.Append(",@OperasyonSuresi");
                sb.Append(",@isteyenProjeID");
                if (!String.IsNullOrEmpty(istenilenProjeID.Text))
                    sb.Append(",@istenilenProjeID");
                sb.Append(",@BrDurumID");
                if (this.irsaliyeNo.Value != null)
                    sb.Append(",@irsaliyeNo");
                if (this.MusteriAdi.Value != null)
                    sb.Append(",@MusteriAdi");
                if (this.MusteriTel.Value != null)
                    sb.Append(",@MusteriTel");
                if (this.PersonelAdi.Value != null)
                    sb.Append(",@PersonelAdi");
                if (this.Description.Text != "")
                    sb.Append(",@Description");
                if (this.Adet.Value != null)
                    sb.Append(",@Adet");
                sb.Append(",@CreatedBy");
                sb.Append(",@CreationDate) ");
                cmd = DB.SQL(this.Context, sb.ToString());
                guid1 = Guid.NewGuid();
                this.HiddenID.Value = id.ToString();
                DB.AddParam(cmd, "@BrTablosuID", guid1);
                DB.AddParam(cmd, "@CreatedBy", 100, Membership.GetUser().UserName);
                DB.AddParam(cmd, "@CreationDate", DateTime.Now);

            }
            #endregion

            #region update
            else
            {
                sb = new StringBuilder("Update BrTablosu Set ");
                sb.Append("Size=@Size");
                sb.Append(",BrMarkaID=@BrMarkaID");
                sb.Append(",isteyenProjeID=@isteyenProjeID");
                sb.Append(",OperasyonSuresi=@OperasyonSuresi");
                if (!String.IsNullOrEmpty(istenilenProjeID.Text))
                    sb.Append(",istenilenProjeID=@istenilenProjeID");
                else
                    sb.Append(",istenilenProjeID=Null");
                sb.Append(",BrDurumID=@BrDurumID");
                if (!CrmUtils.ControllToDate(this.Page, this.Tarih.Date.ToString()))
                    sb.Append(",Tarih=@Tarih");
                else
                    sb.Append(",Tarih=Null");
                if (this.StokKodu.Value != null)
                    sb.Append(",StokKodu=@StokKodu");
                else
                    sb.Append(",StokKodu=Null");
                if (this.Renk.Value != null)
                    sb.Append(",Renk=@Renk");
                else
                    sb.Append(",Renk=Null");
                if (this.irsaliyeNo.Value != null)
                    sb.Append(",irsaliyeNo=@irsaliyeNo");
                else
                    sb.Append(",irsaliyeNo=Null");
                if (this.MusteriAdi.Value != null)
                    sb.Append(",MusteriAdi=@MusteriAdi");
                else
                    sb.Append(",MusteriAdi=Null");
                if (this.MusteriTel.Value != null)
                    sb.Append(",MusteriTel=@MusteriTel");
                else
                    sb.Append(",MusteriTel=Null");
                if (this.PersonelAdi.Value != null)
                    sb.Append(",PersonelAdi=@PersonelAdi");
                else
                    sb.Append(",PersonelAdi=Null");
                if (this.Description.Text != "")
                    sb.Append(",Description=@Description");
                else
                    sb.Append(",Description=Null");
                if (this.Adet.Value != null)
                    sb.Append(",Adet=@Adet");
                else
                    sb.Append(",Adet=Null");
                sb.Append(",ModifiedBy=@ModifiedBy");
                sb.Append(",ModificationDate=@ModificationDate ");
                sb.Append("Where IndexId=@BrTablosuID");
                cmd = DB.SQL(this.Context, sb.ToString());
                DB.AddParam(cmd, "@BrTablosuID", id);
                DB.AddParam(cmd, "@ModifiedBy", 100, Membership.GetUser().UserName);
                DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
            }
            #endregion

            #region values
            if (!CrmUtils.ControllToDate(this.Page, this.Tarih.Date.ToString()))
                DB.AddParam(cmd, "@Tarih", this.Tarih.Date);
            if (this.StokKodu.Value != null)
                DB.AddParam(cmd, "@StokKodu", 100, this.StokKodu.Value.ToString().ToUpper());
            if (this.Renk.Value != null)
                DB.AddParam(cmd, "@Renk", 100, this.Renk.Value.ToString().ToUpper());
            DB.AddParam(cmd, "@Size", 100, this.Size.Value.ToString().ToUpper());
            if (this.irsaliyeNo.Value != null)
                DB.AddParam(cmd, "@irsaliyeNo", 50, this.irsaliyeNo.Value.ToString().ToUpper());
            if (this.MusteriAdi.Value != null)
                DB.AddParam(cmd, "@MusteriAdi", 100, this.MusteriAdi.Value.ToString().ToUpper());
            if (this.MusteriTel.Value != null)
                DB.AddParam(cmd, "@MusteriTel", 50, this.MusteriTel.Value.ToString().ToUpper());
            if (this.PersonelAdi.Value != null)
                DB.AddParam(cmd, "@PersonelAdi", 100, this.PersonelAdi.Value.ToString().ToUpper());
            if (this.Description.Text != "")
                DB.AddParam(cmd, "@Description", 500, this.Description.Text.ToString().ToUpper());
            if (this.Adet.Value != null)
                DB.AddParam(cmd, "@Adet", int.Parse(this.Adet.Value.ToString()));
            else
                DB.AddParam(cmd, "@Adet", 1);
            if (this.OperasyonSuresi.Value != null)
                DB.AddParam(cmd, "@OperasyonSuresi", int.Parse(this.OperasyonSuresi.Value.ToString()));
            else
                DB.AddParam(cmd, "@OperasyonSuresi", 1);
            DB.AddParam(cmd, "@BrMarkaID", new Guid(this.BrMarkaID.Value.ToString()));
            DB.AddParam(cmd, "@isteyenProjeID", new Guid(this.isteyenProjeID.Value.ToString()));
            if (!String.IsNullOrEmpty(istenilenProjeID.Text))
                DB.AddParam(cmd, "@istenilenProjeID", new Guid(this.istenilenProjeID.Value.ToString()));
            DB.AddParam(cmd, "@BrDurumID", new Guid(this.BrDurumID.Value.ToString()));
            #endregion

            cmd.Prepare();
            cmd.ExecuteNonQuery();
            DB.Commit(this.Context);
            Session["Hata"] = "Kayýt iþlemi baþarýlý!";
            return id;
        }
        catch (Exception ex)
        {
            DB.Rollback(this.Context);
            Session["Hata"] = ex.Message.Replace("'", null).Replace("\r", null).Replace("\n", null);
            return -1;
        }
    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("Comment", typeof(string));
        dt.Columns.Add("DurumName", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("CommentDate", typeof(DateTime));

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

            int id = 0;
            id = int.Parse(this.HiddenID.Value);

            SqlCommand cmd = DB.SQL(this.Context, "Delete From BrTablosu WHERE IndexId=@BrTablosuID");
            DB.AddParam(cmd, "@BrTablosuID", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd = DB.SQL(this.Context, "Delete t1 From BRActivite t1 LEFT JOIN BrTablosu t2 on (t1.BrTablosuID=t2.BrTablosuID)  WHERE t2.IndexId=@BrTablosuID");
            DB.AddParam(cmd, "@BrTablosuID", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            if (!NotesUtils.DeleteNotes(this.Page, this.Context, id))
            {
                DB.Rollback(this.Context);
                return;
            }
            DB.Commit(this.Context);
        }
    }


}
