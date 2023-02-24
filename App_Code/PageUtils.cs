using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;

/// <summary>
/// Summary description for PageUtils
/// </summary>
public class PageUtils
{
    public PageUtils() { }

    public static void SavePageStateToPersistenceMedium(System.Web.HttpContext context, System.Web.UI.Page page, object viewState)
    {

        if (context.Session["dsViewState"] == null)
        {
            context.Session.Abandon();
            return;
        }
        LosFormatter lf = new LosFormatter();
        StringWriter sw = new StringWriter();
        lf.Serialize(sw, viewState);
        StringBuilder sb = sw.GetStringBuilder();
        string strViewState = sb.ToString();
        string strPageURL = page.Request.Url.Segments[page.Request.Url.Segments.Length - 1];
        DataSetViewState ds = (DataSetViewState)context.Session["dsViewState"];

        if (ds.ViewStateData.Rows.Contains(strPageURL))
        {
            DataRow row = ds.ViewStateData.Rows.Find(strPageURL);
            row["ViewState"] = strViewState;
        }
        else
        {
            DataRow row = ds.ViewStateData.NewRow();
            row["PageURL"] = strPageURL;
            row["ViewState"] = strViewState;
            ds.ViewStateData.Rows.Add(row);
        }
    }

    public static object LoadPageStateFromPersistenceMedium(System.Web.HttpContext context, System.Web.UI.Page page)
    {
        if (context.Session["dsViewState"] == null)
        {
            context.Session.Abandon();
            return null;
        }
        DataSetViewState ds = (DataSetViewState)context.Session["dsViewState"];
        string strPageURL = page.Request.Url.Segments[page.Request.Url.Segments.Length - 1];
        DataRow row = ds.ViewStateData.Rows.Find(strPageURL);
        string strViewState = (string)row["ViewState"];
        LosFormatter lf = new LosFormatter();
        return lf.Deserialize(strViewState);
    }
}
