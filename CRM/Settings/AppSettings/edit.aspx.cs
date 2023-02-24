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
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxMenu;
using Model.Crm;
using System.Xml;
using System.IO;
using System.Data.SqlClient;

public partial class CRM_Tanimlar_AppSettings_edit : System.Web.UI.Page
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
        this.menu.ItemClick += new MenuItemEventHandler(Menu_ItemClick);
        if (this.IsPostBack || this.IsCallback) return;

        if (!Security.CheckPermission(this.Context, "Taným - Uygulama Tanýmlarý", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#e6e6fa'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        LoadDocument();
    }

    private void Menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("save"))
        {
            if (!Security.CheckPermission(this.Context, "Taným - Uygulama Tanýmlarý", "Update"))
            {
                CrmUtils.MessageAlert(this.Page, "Güncelleme iþlemi yapma yetkisine sahip deðilsiniz!", "MenuSave1");
                return;
            }

            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlýþ bilgi giriþi!", "MenuSave2");
                return;
            }

            SaveDocument();

            this.Response.Redirect("./edit.aspx");
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    private string Null2Empty(object value)
    {
        return (value == null ? "" : value.ToString());
    }

    private void LoadDocument()
    {
        try
        {
            using (SqlConnection conn = DB.Connect())
            using (SqlCommand cmd = DB.SQL(conn, "SELECT [key],[value] FROM AppSettings"))
            {
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                IDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string value = Null2Empty(rdr["value"]).Trim();
                    switch (rdr["key"].ToString().Trim())
                    {
                        case "Manset":
                            this.txtManset.Value = value;
                            break;

                    }
                }
                rdr.Close();
            }
        }
        catch { }

    }

    private bool SaveDocument()
    {
        Hashtable KeyValue = new Hashtable();
        KeyValue.Add("Manset", Null2Empty(this.txtManset.Value));

        IDictionaryEnumerator enumerator = KeyValue.GetEnumerator();
        while (enumerator.MoveNext())
        {
            SqlCommand cmd = DB.SQL(this.Context, "DELETE FROM AppSettings WHERE [key]=@key");
            DB.AddParam(cmd, "@key", 255, enumerator.Key.ToString());
            cmd.CommandTimeout = 1000;
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd = DB.SQL(this.Context, "INSERT INTO AppSettings([key],[value]) VALUES(@key,@value)");
            DB.AddParam(cmd, "@key", 255, enumerator.Key.ToString());
            DB.AddParam(cmd, "@value", 500, enumerator.Value.ToString());
            cmd.CommandTimeout = 1000;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        return true;
    }


    protected void BtnGetPop3Inbox_Click(object sender, EventArgs e)
    {
        try
        {
            //Hashtable parameters = new Hashtable();
            //parameters.Add("MailServer", this.SesMailServerName.Value.ToString());
            //parameters.Add("MailPort", Convert.ToInt32(this.SesMailServerPort.Value));
            //parameters.Add("MailUseSsl", true);
            //parameters.Add("MailUserName", this.SesMailUserName.Value.ToString());
            //parameters.Add("MailPassword", this.SesMailPassword.Value.ToString());

            //Net.Mail.Pop3Utils.SaveInboxToDb(this.Context, parameters);

            CrmUtils.MessageAlert(this.Page, "Mesajlarý alma iþlemi baþarýlý!", "alert");
        }
        catch (Exception ex)
        {
            CrmUtils.MessageAlert(this.Page, ex.Message.Replace("'", " ").Replace("\r", "").Replace("\n", ""), "alert");
        }
    }

}
