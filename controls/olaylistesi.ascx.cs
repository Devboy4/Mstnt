using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxNavBar;
using Model.Crm;
using DevExpress.Web.ASPxCallback;

public partial class controls_olaylistesi : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        //if (!String.IsNullOrEmpty(Membership.GetUser().UserName))
        //{
        //    this.HiddenID.Value = Membership.GetUser().UserName;
        //    LoadDocument(this.HiddenID.Value);
        //}
        LoadDocument();
    }

    protected void MesajCallback_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (String.IsNullOrEmpty(e.Parameter)) return;
        LoadDocument();
    }

    private void LoadDocument()
    {
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataSet ds = new DataSet();
        using (SqlConnection conn = DB.Connect())
        using (SqlCommand cmd = DB.SQL(conn, "EXEC GetEventList @UserName"))
        {
            DB.AddParam(cmd, "@UserName", 150, Membership.GetUser().UserName);
            try
            {
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);

                if (Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) > 0)
                    imguyari.ImageUrl = "~/images/uyarivar.gif";
                else
                    imguyari.ImageUrl = "~/images/uyariyok.png";

                ltrManset.Text =  ds.Tables[1].Rows[0][0].ToString();
            }
            catch { }
            finally
            {
                if (adapter != null)
                {
                    adapter.Dispose();
                    adapter = null;
                }
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }
        }

    }




}
