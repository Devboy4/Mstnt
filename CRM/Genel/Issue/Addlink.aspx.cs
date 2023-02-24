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
using Model.Crm;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Data.SqlClient;
using System.Text;
using DevExpress.Web.ASPxMenu;

public partial class CRM_Genel_Issue_Addlink : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack || IsCallback)
        {
            return;
        }
        string sID = this.Request.Params["id"].Replace("'", "");

        if ((sID != null) && (sID.Trim() != "0"))
        {
            int id = int.Parse(sID);
            hidden.Value = id.ToString();
        }
    }

    protected void menu_ItemClick1(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("save"))
        {
            try
            {
                if (!flp_logoyukle.HasFile)
                {
                    CrmUtils.CreateMessageAlert(this.Page, "Lütfen Dosyanýn Varlýðýndan Emin Olunuz...", "stkey1");
                    return;
                }
                string dosya = null, uzanti = null;
                dosya = @"\\Filesrv\gulcin\" + flp_logoyukle.PostedFile.FileName.Replace(@"\\Filesrv\gulcin\", null).Replace(@"\\192.168.0.212\gulcin\", null);
                uzanti = flp_logoyukle.FileName;
                DB.BeginTrans(this.Context);
                if (hidden.Value.ToString() == "") return;
                int id = int.Parse(hidden.Value.ToString());
                StringBuilder sb = new StringBuilder();
                sb.Append("Insert Into VirusDosyaYolu (");
                sb.Append("VirusDosyaYoluID,BagliID,DosyaYolu,DosyaAdi,CreatedBy,CreationDate) ");
                sb.Append("Values(@VirusDosyaYoluID,@BagliID,@DosyaYolu,@DosyaAdi,@CreatedBy,@CreationDate)");
                SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
                DB.AddParam(cmd, "@VirusDosyaYoluID", Guid.NewGuid());
                DB.AddParam(cmd, "@BagliID", id);
                DB.AddParam(cmd, "@DosyaYolu", 500, dosya);
                DB.AddParam(cmd, "@DosyaAdi", 100, uzanti);
                DB.AddParam(cmd, "@CreatedBy", 100, Membership.GetUser().UserName);
                DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                DB.Commit(this.Context);

                this.Response.Write("<script language='javascript'>{ parent.opener.grid1.PerformCallback('gridBind'); }</script>");
                this.Response.Write("<script language='javascript'>{ parent.close();}</script>");

            }
            catch (Exception ex)
            {
                DB.Rollback(this.Context);
                CrmUtils.CreateMessageAlert(this.Page, "Bir Hata var:" + ex.Message.Replace("'", null), "stkey1");
            }
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }
}
