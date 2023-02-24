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

public partial class account_changepassword : System.Web.UI.Page
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
        if (this.YeniSifre.Text != this.YeniSifreTekrar.Text)
        {
            mesaj.Append("Yeni Þifre giriþleri birbirini tutmalý!<br>");
            this.Mesaj.Text = mesaj.ToString();
            return;
        }

        if (this.YeniSifre.Text.Length < Membership.Provider.MinRequiredPasswordLength)
        {
            mesaj.Append("Yeni Þifre uzunluðu en az " + Membership.Provider.MinRequiredPasswordLength.ToString() + " karakter olamalýdýr!<br>");
            this.Mesaj.Text = mesaj.ToString();
            return;
        }


        MembershipUser user = Membership.GetUser();
        if (user.IsLockedOut) user.UnlockUser();
        //conn = DB.Connect(this.Context);
        //cmd = DB.SQL(this.Context, "SELECT Password FROM SecurityUsers WHERE UserName=@UserName");
        //DB.AddParam(cmd, "@UserName", 60, user.UserName);
        //cmd.Prepare();
        string sOldPassword = this.EskiSifre.Text;// user.GetPassword(); //(string)cmd.ExecuteScalar();
        string sNewPassword = this.YeniSifre.Text;

        //if (this.EskiSifre.Text != sOldPassword)
        //{
        //    mesaj.Append("Eski Þifre yanlýþ girilmiþ!<br>");
        //    this.Mesaj.Text = mesaj.ToString();
        //    return;
        //}


        //sOldPassword = user.ResetPassword();
        bool changed = user.ChangePassword(sOldPassword, sNewPassword);
        if (!changed)
        {
            mesaj.Append("Þifre deðiþtirilemedi!<br>");
            this.Mesaj.Text = mesaj.ToString();
            return;
        }
        else
        {
            //cmd = DB.SQL(this.Context, "UPDATE SecurityUsers SET Password=@Password WHERE UserName=@UserName");
            //DB.AddParam(cmd, "@UserName", 60, user.UserName);
            //DB.AddParam(cmd, "@Password", 60, sNewPassword);
            //cmd.Prepare();
            try
            {
                //cmd.ExecuteNonQuery();
                mesaj.Append("Þifre deðiþtirildi!<br>");
                this.Mesaj.Text = mesaj.ToString();
                Membership.UpdateUser(user);
                FormsAuthentication.SignOut();
                this.Session.Abandon();
                Response.Write("<script language='Javascript'>{ reloadPage(); }</script>");
                //this.Response.Redirect("../../../CRM/Genel/Issue/GundemGiris.aspx");
            }
            catch (Exception ex)
            {
                this.Mesaj.Text = ex.ToString();

                return;
            }

        }



    }
}
