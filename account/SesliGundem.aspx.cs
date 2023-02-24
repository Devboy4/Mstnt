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
using System.Globalization;
using Model.Crm;
using System.IO;
using System.Data.SqlClient;


public partial class account_SesliGundem : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }
        LoadForm();
    }

    void LoadForm()
    {
        try
        {
            CrmUtils.BindListBoxes(this.Context, lstUsers,
                "select IndexId,IsNull(Username,'') + ' [' +ISNULL(FirstName,'') + ' ' + ISNULL(LastName,'') + ']' as UserName From SecurityUsers Order By UserName"
                , "IndexId", "UserName");
        }
        catch
        {
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string Ids = string.Empty;
        bool ilk = true;
        foreach (ListItem liste in lstUsers.Items)
        {
            if (liste.Selected)
            {
                if (ilk)
                {
                    Ids = liste.Value;
                    ilk = false;
                }
                else
                    Ids += "," + liste.Value;

            }
        }
        if (String.IsNullOrEmpty(Ids))
        {
            CrmUtils.MessageAlert(this.Page, "Lütfen kullanýcý seçiniz!", "stkey1");
            return;
        }
        if (!Seslifile.HasFile)
        {
            CrmUtils.MessageAlert(this.Page, "Lütfen ses dosyasý seçiniz!", "stkey1");
            return;
        }
        if (this.txtpassword.Text != "Mstnt2015")
        {
            CrmUtils.MessageAlert(this.Page, "Ýþlem kodu yanlýþ!", "stkey1");
            return;
        }
        Guid id = Guid.NewGuid();
        Directory.CreateDirectory(Server.MapPath("../CRM/Pop3MailAttachments/" + id.ToString()));
        Seslifile.SaveAs(Server.MapPath("../CRM/Pop3MailAttachments/" + id.ToString() + "/" + Seslifile.FileName));

        using (SqlCommand cmd = DB.SQL(this.Context, "EXEC InsertVoiceIssue @Ids,@Id,@AttachmentName"))
        {
            try
            {
                DB.AddParam(cmd, "@Ids", 4000, Ids);
                DB.AddParam(cmd, "@Id", id);
                DB.AddParam(cmd, "@AttachmentName", 500, Seslifile.FileName);
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                CrmUtils.MessageAlert(this.Page, "Sesli gündem oluþturulamadý!", "stkey1");
                return;
            }

        }

        CrmUtils.MessageAlert(this.Page, "Sesli gündem oluþturuldu!", "stkey1");

        LoadForm();

    }
}
