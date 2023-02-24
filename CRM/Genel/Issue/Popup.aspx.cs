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
using System.Data.SqlClient;
using DevExpress.Web.ASPxMenu;
using DevExpress.Web.ASPxEditors;
using Model.Crm;
using DevExpress.Web.Data;
using DevExpress.Web.ASPxGridView;

public partial class MarjinalCRM_Genel_BultenGonderimi_Popup : System.Web.UI.Page
{
    DataTable dt = new DataTable();
    DataTable newdt = new DataTable();

    protected override object LoadPageStateFromPersistenceMedium()
    {
        return PageUtils.LoadPageStateFromPersistenceMedium(this.Context, this.Page);
    }

    protected override void SavePageStateToPersistenceMedium(object viewState)
    {
         PageUtils.SavePageStateToPersistenceMedium(this.Context, this.Page, viewState);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack || IsCallback)
            return;
    }



    protected void menu_ItemClick1(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("save"))
        {
            if (!NotesUtils.CheckFile(this.Page, this.Context,fileUpload))
            {
               return;
            }
            try
            {
                if (this.fileUpload.HasFile)
                {
                    dt = new DataTable();
                    dt = (DataTable)Session["DataTableNoteDosya"];
                    newdt = new DataTable();
                    newdt = MailUtils.AddAttachFile(this.Page, this.Context, fileUpload, dt);
                    Session["DataTableNoteDosya"] = newdt;
                    Response.Write("<script language='Javascript1.2'>{ parent.opener.GridNotDosya.PerformCallback('x'); }</script>");
                    Response.Write("<script language='Javascript1.2'>{ parent.close(); }</script>");
                }
            }
            catch (Exception ex)
            {
                CrmUtils.CreateMessageAlert(this.Page, "Bir Hata var:" + ex.Message, "stkey1");
            }
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }
}
