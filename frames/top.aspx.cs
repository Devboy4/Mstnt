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
using System.Text;

public partial class top : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string Kullanici = null;
            Kullanici = Membership.GetUser().UserName;
            StringBuilder sb = new StringBuilder();
            sb.Append("<table cellspacing=\"2\" cellpadding=\"0\" border=\"0\" style=\"width:150px;height:100%;\">");
            sb.Append("<td align=\"center\" style=\"width:100px\">");
            sb.Append("<span style=\"font-size: 8pt; font-family: Arial;color: #fff\">");
            sb.Append("Sayýn: " + Kullanici + "");
            sb.Append("<br/> Hoþgeldiniz !");
            sb.Append("</span");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            ltr_Kullanici.Text = sb.ToString();
            //sb = new StringBuilder();
            //sb.Append("<table cellspacing=\"2\" cellpadding=\"0\" border=\"0\" style=\"width:150px;height:100%;\">");
            //sb.Append("<tr><td align=\"center\" style=\"width:150px;\">");
            ////sb.Append("<a href=\"http://www.letsgosoft.com\" style=\"text-decoration: none;\" target=\"_blank\">");
            //sb.Append("<span style=\"font-size: 8pt; font-family: Arial;color: #FFFFFF\">");
            //sb.Append("© Copyright 2009-" + DateTime.Now.Year.ToString() + " Hes Yazýlým&nbsp;<br />");
            //sb.Append("www.hesyazilim.com</span></td>");
            //sb.Append("</tr>");
            //sb.Append("</table>");
            //ltrWritenBy.Text = sb.ToString();
        }
        catch (Exception ex)
        {

        }


    }

    protected void ButonCikis_Click(object sender, ImageClickEventArgs e)
    {        
        FormsAuthentication.SignOut();
        Session.Abandon();
        Response.Write("<script language='Javascript1.2'>this.parent.location.href='./../Login.aspx';</script>");
    }


}
