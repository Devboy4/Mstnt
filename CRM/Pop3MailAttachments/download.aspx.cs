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

public partial class CRM_Pop3MailAttachments_download : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sID = this.Request.Params["id"].Replace("'", "");

        if ((sID != null) && (sID.Trim() != "0"))
        {
            string DosyaAdi = null;
            string DosyaYolu = null;

            if (Request.QueryString["AttachName"] != null)
            {
                DosyaAdi = (String)Request.QueryString["AttachName"];
                DosyaYolu = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "CRM\\Pop3MailAttachments\\" + sID + "\\" + DosyaAdi;
            }

            this.Response.AddHeader("Content-Disposition", "attachment;filename=" + DosyaAdi);
            this.Response.TransmitFile(DosyaYolu);
        }
    }
}
