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

public partial class CRM_Genel_Issue_download : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sID = this.Request.Params["id"].Replace("'", "");

        if ((sID != null) && (sID.Trim() != "0"))
        {
            string DosyaAdi = null;
            string DosyaYolu = null;

            Guid id = new Guid(sID);

            SqlCommand cmd = DB.SQL(this.Context, "SELECT DosyaAdi,DosyaYolu FROM VirusDosyaYolu WHERE VirusDosyaYoluID=@VirusDosyaYoluID");
            DB.AddParam(cmd, "@VirusDosyaYoluID", id);
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                DosyaAdi = rdr["DosyaAdi"].ToString();
                DosyaYolu = rdr["DosyaYolu"].ToString();
            }
            rdr.Close();

            this.Response.AddHeader("Content-Disposition", "attachment;filename=" + DosyaAdi);
            this.Response.TransmitFile(DosyaYolu);
        }
    }
}
