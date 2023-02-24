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

public partial class CRM_FileManager_File_preview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        object _Id = this.Request.Params["id"];

        if ((_Id != null) && (!String.IsNullOrEmpty(_Id.ToString())))
        {
            SqlCommand cmd = DB.SQL(this.Context, "SELECT FileName FROM FMFile WHERE Id=@Id");
            DB.AddParam(cmd, "@Id", (new Guid(_Id.ToString())));
            cmd.CommandTimeout = 1000;
            cmd.Prepare();
            string _FileName = cmd.ExecuteScalar().ToString();
            string _FilePath = this.Context.Server.MapPath("~").ToString() + "\\CRM\\FileManager\\Files\\" + _FileName;

            //this.Response.ClearContent();
            this.Response.AddHeader("Content-Disposition", "attachment;filename=" + _FileName);
            this.Response.TransmitFile(_FilePath.ToString());
        }
    }
}
