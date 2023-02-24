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
using System.IO;
using Ionic.Zip;

public partial class MarjinalCRM_Notes_download : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sID = this.Request.Params["id"].Replace("'", "");

        if ((sID != null) && (sID.Trim() != "0"))
        {
            string DosyaAdi = null;
            string DosyaYolu = null;
            Guid id = Guid.Empty;
            if (sID != "DosyaYolu")
            {
                
                id = new Guid(sID);
                if (Request.QueryString["DownType"] != null)
                {
                    DownloadZipFile(id);
                    return;

                }
            }
            if (Request.QueryString["VirusDosyaYolu"] == null && Request.QueryString["VirusDosyaAdi"] == null && sID != "DosyaYolu")
            {
                SqlCommand cmd = DB.SQL(this.Context, "SELECT DosyaAdi,DosyaYolu FROM NotDosya WHERE NotDosyaID=@NotDosyaID");
                DB.AddParam(cmd, "@NotDosyaID", id);
                cmd.Prepare();
                IDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    DosyaAdi = rdr["DosyaAdi"].ToString();
                    DosyaYolu = rdr["DosyaYolu"].ToString();
                }
                rdr.Close();
            }
            else
            {
                DosyaAdi = (String)Request.QueryString["VirusDosyaAdi"];
                DosyaYolu = (String)Request.QueryString["VirusDosyaYolu"];

            }

            string _FilePath = DosyaYolu.Replace(@"\/", "/"); //this.Context.Server.MapPath("~").ToString() + "\\" + DosyaYolu.Substring(DosyaYolu.LastIndexOf(@"CRM"), (DosyaYolu.Length - DosyaYolu.LastIndexOf(@"CRM")));
            FileInfo info = new FileInfo(_FilePath);
            string enCodeFileName = Server.UrlEncode(DosyaAdi);
            this.Response.AddHeader("Content-Disposition", "attachment;filename=" + enCodeFileName);
            Response.AddHeader("Content-Length", info.Length.ToString()); 
            Response.ContentType = "application/octet-stream";
            this.Response.TransmitFile(_FilePath.ToString());
            Response.End();
            
        }
    }

    void DownloadZipFile(Guid id)
    {
        string _filename = string.Empty;
        using (ZipFile zip = new ZipFile())
        {
            zip.AlternateEncodingUsage = ZipOption.AsNecessary;
            zip.AddDirectoryByName("Dosyalar");
            
            SqlCommand cmd = DB.SQL(this.Context, "select top 1 BagliId from Notes where NotID=@Id");
            DB.AddParam(cmd, "@Id", id);
            cmd.Prepare();
            _filename = cmd.ExecuteScalar().ToString();
           
            cmd = DB.SQL(this.Context, "SELECT DosyaYolu FROM NotDosya WHERE NotID=@Id");
            cmd.Parameters.Clear();
            DB.AddParam(cmd, "@Id", id);
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();
            int cnt = 0;
            string _newfilename = string.Empty, _wofilename = string.Empty;
            while (rdr.Read())
            {

                try
                {
                    zip.AddFile(rdr["DosyaYolu"].ToString(), "Dosyalar");
                }
                catch
                {
                    //try
                    //{
                    //    cnt++;
                    //    _wofilename = Path.GetFileNameWithoutExtension(rdr["DosyaYolu"].ToString());
                    //    _newfilename = rdr["DosyaYolu"].ToString().Replace(_wofilename, _wofilename + cnt.ToString());
                    //    zip.AddFile(_newfilename, "Dosyalar");
                    //}
                    //catch { }

                } 
            }
            rdr.Close();

            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + _filename + ".zip");
            Response.ContentType = "application/zip";
            zip.Save(Response.OutputStream);
            Response.End();
        }
    }
}
