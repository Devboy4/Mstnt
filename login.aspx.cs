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
using System.Data.SqlClient;
using Model.Crm;

public partial class login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Context.Session["User"] = null;
        this.Context.Session["Permissions"] = null;
        if (!Page.IsPostBack)
        {
            CheckBox cek = (CheckBox)CrmLogin.FindControl("RememberMe");
            if (Request.Cookies["username"] == null || Request.Cookies["username"].Value.ToString().Trim() == ""
                || Request.Cookies["password"] == null || Request.Cookies["password"].Value.ToString().Trim() == "")
            {
                cek.Checked = false;
            }
            else
            {
                CrmLogin.UserName = Request.Cookies["username"].Value.ToString();
                TextBox pass = (TextBox)CrmLogin.FindControl("Password");
                pass.Attributes.Add("value", Request.Cookies["password"].Value.ToString());
                CrmLogin.RememberMeSet = true;
            }

            if (Request.QueryString["SecKey"] != null)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["SecKey"].ToString()))
                {
                    using (SqlConnection conn = DB.Connect(this.Context))
                    {
                        using (SqlCommand cmd = DB.SQL(this.Context, "Update AppSettings set [value]=@Value Where [key]='SecKey'"))
                        {
                            DB.AddParam(cmd, "@Value", 100, Request.QueryString["SecKey"].ToString());
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            if (Request.QueryString["ReturnUrl"] != null)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["ReturnUrl"].ToString()))
                {
                    if (Request.QueryString["ReturnUrl"].ToString().ToLower() == "%2f" 
                        || Request.QueryString["ReturnUrl"].ToString().ToLower() == "/"
                        || Request.QueryString["ReturnUrl"].ToString().ToLower() == @"\")
                        Response.Redirect("~/Login.aspx");
                }
            }

        }
    }

    protected void CrmLogin_Authenticate(object sender, AuthenticateEventArgs e)
    {
        DataTable dtUser = new DataTable();

        dtUser.Columns.Add("UserID", typeof(Guid));
        dtUser.Columns.Add("UserName", typeof(string));
        dtUser.Columns.Add("Password", typeof(string));
        dtUser.Columns.Add("FirstName", typeof(string));
        dtUser.Columns.Add("LastName", typeof(string));

        bool aktif = false;
        SqlConnection conn = DB.Connect(this.Context);
        SqlCommand cmd = DB.SQL(this.Context, "SELECT UserID,UserName,Password,FirstName,LastName FROM SecurityUsers WHERE UserName=@UserName and Password=@Password");
        DB.AddParam(cmd, "@UserName", 100, this.CrmLogin.UserName);
        DB.AddParam(cmd, "@Password", 100, this.CrmLogin.Password);
        cmd.Prepare();
        try
        {
            IDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                DataRow row = dtUser.NewRow();
                row["UserID"] = rdr["UserID"];
                row["UserName"] = rdr["UserName"];
                row["Password"] = rdr["Password"];
                row["FirstName"] = rdr["FirstName"];
                row["LastName"] = rdr["LastName"];
                aktif = true;
                dtUser.Rows.Add(row);
                dtUser.AcceptChanges();
            }
        }
        catch (Exception ex)
        {
            aktif = false;
        }
        conn.Close();

        if (aktif)
            this.Context.Session["User"] = dtUser;
        else
            this.Context.Session["User"] = null;

        e.Authenticated = aktif;
    }

    protected void Login1_LoggingIn(object sender, LoginCancelEventArgs e)
    {
        CheckBox cek = (CheckBox)CrmLogin.FindControl("RememberMe");
        if (cek.Checked == true)
        {
            HttpCookie cookie = new HttpCookie("username");
            HttpCookie cookie1 = new HttpCookie("password");
            cookie.Value = CrmLogin.UserName;
            cookie1.Value = CrmLogin.Password;

            cookie.Expires = DateTime.Now.AddMonths(1);//cookie Expires
            cookie1.Expires = DateTime.Now.AddMonths(1);//cookie Expires
            HttpContext.Current.Response.AppendCookie(cookie);
            HttpContext.Current.Response.AppendCookie(cookie1);
        }
        else
        {
            HttpContext.Current.Response.Cookies.Remove("username");
            HttpContext.Current.Response.Cookies.Remove("password");
            cek.Checked = false;
        }
    }
}
