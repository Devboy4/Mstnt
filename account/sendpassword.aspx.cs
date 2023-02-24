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
using Model.Crm;
using System.Data.SqlClient;

public partial class anonymous_sendpassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsPostBack || this.IsCallback)
            return;
        this.Mesaj.Text = "";
    }

    protected void Degistir_Click(object sender, EventArgs e)
    {
        StringBuilder mesaj = new StringBuilder();
        try
        {
            string userName = Kullanici.Text;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            DB.BeginTrans(this.Context);
            MembershipUser user = Membership.GetUser(userName);
            try
            {
                string ss = user.UserName;
            }
            catch
            {
                mesaj = new StringBuilder();
                mesaj.Append("Sistemde b�yle bir kullan�c� bulunamad� !<br>");
                this.Mesaj.Text = mesaj.ToString();
                DB.Rollback(this.Context);
                return;
            }

            //if (user.IsLockedOut) user.UnlockUser();
            //if (!CrmUtils.IsEmail(user.Email))
            //{
            //    mesaj = new StringBuilder();
            //    mesaj.Append("Girilen kullan�c�n�n mail bilgisi sistemde yanl�� g�r�n�yor!<br>");
            //    mesaj.Append("L�tfen kullan�c�ya ait bilgileri kontrol edip tekrar deneyiniz.");
            //    this.Mesaj.Text = mesaj.ToString();
            //    DB.Rollback(this.Context);
            //    return;
            //}

            string sOldPassword = user.ResetPassword();
            string sNewPassword = Membership.GeneratePassword(11, 2);



            bool changed = user.ChangePassword(sOldPassword, sNewPassword);
            if (!changed)
            {
                mesaj = new StringBuilder();
                mesaj.Append("�ifre de�i�tirilemedi!<br>");
                this.Mesaj.Text = mesaj.ToString();
                DB.Rollback(this.Context);
            }
            else
            {
                //cmd = DB.SQL(this.Context, "UPDATE SecurityUsers SET Password=@Password WHERE UserName=@UserName");
                //DB.AddParam(cmd, "@UserName", 60, user.UserName);
                //DB.AddParam(cmd, "@Password", 60, sNewPassword);
                //cmd.Prepare();
                //string _useremail = "bilisimteknolojileri@deriden.com;hakan.dogan@msn.com";
                //if (CrmUtils.IsEmail(user.Email))
                //    _useremail = ";" + user.Email;
                try
                {
                    //cmd.ExecuteNonQuery();
                    string mailsubject = "MSTNT �ifre Hat�rlatma";
                    string mailbody = "Kullan�c� Ad� :" + user.UserName
                    + "<br>"
                    + "�ifre          :" + sNewPassword;
                    int mailgitti;
                    if (user.UserName.ToLower() == "developer" || user.UserName.ToLower() == "admin")
                        mailgitti = MailUtils.JustSendMail("hakan.dogan@msn.com", mailbody, mailsubject);
                    else
                        mailgitti = MailUtils.JustSendMail("net@deriden.com;bilisimteknolojileri@deriden.com;hakan.dogan@msn.com", mailbody, mailsubject);
                    mesaj = new StringBuilder();
                    mesaj.Append("�ifre mail adresinize g�nderildi!<br>");
                    this.Mesaj.Text = mesaj.ToString();
                    Membership.UpdateUser(user);
                }
                catch (Exception ex)
                {
                    this.Mesaj.Text = ex.ToString();
                    DB.Rollback(this.Context);
                    conn.Close();
                    return;
                }
            }
            DB.Commit(this.Context);
        }
        catch (Exception ex)
        {
            DB.Rollback(this.Context);
            this.Mesaj.Text = "�ifre G�nderilirken bir hata olu�tu <br>hata:" + ex.Message;
        }
        //}
        //conn.Close();
    }
}
