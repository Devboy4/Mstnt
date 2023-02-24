using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;
using DevExpress.Web.ASPxEditors;
using DevExpress.WebUtils;
using DevExpress.Web.ASPxGridView;
using System.IO;
using DevExpress.Web.ASPxGridView.Export;

namespace Model.Crm
{
    public class CrmUtils
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        string sql;

        public void ExecuteNonQuery(string insert)
        {

            conn.Open();
            cmd.CommandText = insert;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
            conn.Close();


        }

        public static void BindListBoxes(HttpContext context, ListBox cmb, string sqltext, string ValueString, string TextValue)
        {

            IDataReader rdr2;
            ListItem item;
            SqlCommand cmd2;
            cmb.Items.Clear();
            cmd2 = DB.SQL(context, sqltext);
            cmd2.Prepare();
            rdr2 = cmd2.ExecuteReader();
            while (rdr2.Read())
            {
                item = new ListItem();
                item.Text = rdr2["" + TextValue + ""].ToString();
                item.Value = rdr2["" + ValueString + ""].ToString();
                cmb.Items.Add(item);
            }
            rdr2.Close();
        }

        public void BindComboBoxes(HttpContext context, ASPxComboBox cmb, string sqltext, string ValueString, string TextValue)
        {

            IDataReader rdr;
            ListEditItem item;
            cmb.Items.Clear();
            item = new ListEditItem();
            item.Text = "";
            item.ValueString = Guid.Empty.ToString();
            cmb.Items.Add(item);
            cmd = DB.SQL(context, sqltext);
            cmd.Prepare();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                item = new ListEditItem();
                item.Text = rdr["" + TextValue + ""].ToString();
                item.ValueString = rdr["" + ValueString + ""].ToString();
                cmb.Items.Add(item);
            }
            rdr.Close();
            cmb.SelectedIndex = 0;
        }

        public void BindComboBoxesInt(HttpContext context, ASPxComboBox cmb, string sqltext, string ValueString, string TextValue)
        {

            IDataReader rdr;
            ListEditItem item;
            cmb.Items.Clear();
            item = new ListEditItem();
            item.Text = string.Empty;
            item.Value = 0;
            cmb.Items.Add(item);
            cmd = DB.SQL(context, sqltext);
            cmd.Prepare();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                item = new ListEditItem();
                item.Text = rdr["" + TextValue + ""].ToString();
                item.Value = rdr["" + ValueString + ""];
                cmb.Items.Add(item);
            }
            rdr.Close();
            cmb.SelectedIndex = 0;          
        }

        public void BindComboBoxesNoEmpty(HttpContext context, ASPxComboBox cmb, string sqltext, string ValueString, string TextValue)
        {

            IDataReader rdr;
            ListEditItem item;
            cmb.Items.Clear();
            cmd = DB.SQL(context, sqltext);
            cmd.Prepare();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                item = new ListEditItem();
                item.Text = rdr["" + TextValue + ""].ToString();
                item.ValueString = rdr["" + ValueString + ""].ToString();
                cmb.Items.Add(item);
            }
            rdr.Close();
        }

        public void BindComboBoxesNoEmptyDt(HttpContext context, ASPxComboBox cmb, DataTable dt, string ValueString, string TextValue)
        {

            ListEditItem item;
            cmb.Items.Clear();
            foreach (DataRow rdr in dt.Rows)
            {
                item = new ListEditItem();
                item.Text = rdr["" + TextValue + ""].ToString();
                item.ValueString = rdr["" + ValueString + ""].ToString();
                cmb.Items.Add(item);
            }
        }

        public void BindComboBoxesDt(HttpContext context, ASPxComboBox cmb, DataTable dt, string ValueString, string TextValue)
        {

            ListEditItem item;
            cmb.Items.Clear();
            item = new ListEditItem();
            item.Text = "";
            item.ValueString = "0";
            cmb.Items.Add(item);
            foreach (DataRow rdr in dt.Rows)
            {
                item = new ListEditItem();
                item.Text = rdr["" + TextValue + ""].ToString();
                item.ValueString = rdr["" + ValueString + ""].ToString();
                cmb.Items.Add(item);
            }
        }

        public void BindComboBoxesAllowedUSers(HttpContext context, ASPxComboBox cmb, DataTable dt, string ValueString, string TextValue)
        {

            ListEditItem item;
            cmb.Items.Clear();
            item = new ListEditItem();
            item.Text = "";
            item.ValueString = "0";
            cmb.Items.Add(item);
            foreach (DataRow rdr in dt.Rows)
            {
                if (!Convert.ToBoolean(rdr["IsVisible"])) continue;
                item = new ListEditItem();
                item.Text = rdr["" + TextValue + ""].ToString();
                item.ValueString = rdr["" + ValueString + ""].ToString();
                cmb.Items.Add(item);
            }
        }

        public void BindComboBoxesinGrid(HttpContext context, ASPxComboBox cmb, string sqltext, string ValueString, string TextValue)
        {

            IDataReader rdr;
            ListEditItem item;
            cmb.Items.Clear();
            cmd = DB.SQL(context, sqltext);
            cmd.Prepare();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                item = new ListEditItem();
                item.Text = rdr["" + TextValue + ""].ToString();
                item.Value = new Guid(rdr["" + ValueString + ""].ToString());
                cmb.Items.Add(item);
            }
            rdr.Close();
        }

        public ASPxComboBox BindListBoxes(HttpContext context, ASPxComboBox cmb, string sqltext, string ValueString, string TextValue)
        {
            DataTable my_datatable = new DataTable();
            ListEditItem item = new ListEditItem();
            cmd.CommandText = sqltext;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            adapter.Fill(my_datatable);
            item = new ListEditItem();
            item.Text = "";
            item.ValueString = "00000000-0000-0000-0000-000000000000";
            cmb.Items.Add(item);
            for (int i = 0; i < my_datatable.Rows.Count; i++)
            {
                item = new ListEditItem();
                item.Text = my_datatable.Rows[i]["" + TextValue + ""].ToString();
                item.ValueString = my_datatable.Rows[i]["" + ValueString + ""].ToString();
                cmb.Items.Add(item);
            }
            return cmb;

        }

        public bool File_ControlRSM(FileUpload flp, int max_size)
        {
            bool var = false;
            try
            {
                string extension = System.IO.Path.GetExtension(flp.FileName);
                int file_size = flp.PostedFile.ContentLength;
                if (flp.FileName == null || flp.FileName == "")
                {
                    return true;
                }
                else
                {
                    if (Regex.IsMatch(extension.ToLower(), ".jpg|.jpeg|.gif|.png|.bmp") == false)
                    {
                        var = false;
                    }
                    else
                    {
                        var = true;

                    }
                    if (max_size < file_size)
                    {
                        var = false;
                    }
                }
                return var;
            }
            catch (Exception exception)
            {
                Exceptions(exception.Message, "CRMUTILS.cs_File_Control", "GetDate()");
                return false;
            }
        }

        public string isnull(string str)
        {
            try
            {
                string strr = null;
                if (str.Length < 1)
                {
                    strr = "0";

                }
                else
                {
                    strr = str;
                }
                return strr;
            }
            catch
            {
                return null;
            }
        }

        public static void CreateMessageAlert(System.Web.UI.Page aspxPage, string strMessage, string strKey)
        {
            string strScript = "<script language=JavaScript>alert('" + strMessage.Replace("'", null).Replace("\r", null).Replace("\n", null) + "')</script>";

            if (!aspxPage.IsStartupScriptRegistered(strKey))
            {
                aspxPage.RegisterStartupScript(strKey, strScript);
            }
        }

        public static void close_page(System.Web.UI.Page aspxPage, string strKey)
        {
            string strScript2 = "<script language=JavaScript>{ parent.close();parent.opener.location.reload();}</script>";
            if (!aspxPage.IsStartupScriptRegistered(strKey))
            {
                aspxPage.RegisterStartupScript(strKey, strScript2);
            }
        }

        public static void MessageAlert(System.Web.UI.Page page, string message, string key)
        {
            string script = "<script language='JavaScript'> alert('" + message + "'); </script>";

            if (!page.IsStartupScriptRegistered(key))
            {
                page.RegisterStartupScript(key, script);
            }
        }

        public void drp_Fill(DropDownList drp, string select, string dataValueField, string dataTextField)
        {
            DataTable my_datatable = new DataTable();
            cmd.CommandText = select;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            adapter.Fill(my_datatable);
            drp.DataSource = my_datatable.DefaultView;
            drp.DataTextField = dataTextField;
            drp.DataValueField = dataValueField;
            drp.DataBind();
        }

        public void drp_FillByHepsi(DropDownList drp, string select, string dataValuesField, string dataTextField, string Neyaz)
        {
            DataTable my_datatable2 = new DataTable();
            cmd.CommandText = select;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            adapter.Fill(my_datatable2);
            DataRow dr2 = my_datatable2.NewRow();
            dr2["" + dataValuesField + ""] = Guid.Empty.ToString();
            dr2["" + dataTextField + ""] = Neyaz;
            my_datatable2.Rows.InsertAt(dr2, 0);
            drp.DataSource = my_datatable2;
            drp.DataTextField = dataTextField;
            drp.DataValueField = dataValuesField;
            drp.DataBind();
            drp.SelectedIndex = 0;

        }

        public void Lst_Fill(ListBox Lst, string select, string dataValueField, string dataTextField)
        {
            DataTable list_dt = new DataTable();
            cmd.CommandText = select;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            adapter.Fill(list_dt);
            Lst.DataSource = list_dt.DefaultView;
            Lst.DataTextField = dataTextField;
            Lst.DataValueField = dataValueField;
            Lst.DataBind();
        }

        public void ChcList_Fill(CheckBoxList Lst, string select, string dataValueField, string dataTextField)
        {
            DataTable list_dt = new DataTable();
            cmd.CommandText = select;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            adapter.Fill(list_dt);
            Lst.DataSource = list_dt.DefaultView;
            Lst.DataTextField = dataTextField;
            Lst.DataValueField = dataValueField;
            Lst.DataBind();
        }

        public bool Search(string select, string target)
        {
            bool var = false; ;
            cmd.CommandText = select;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            adapter.Fill(ds, "search");
            for (int i = 0; i < ds.Tables["search"].Rows.Count; i++)
            {
                if (ds.Tables["search"].Rows[i][0].ToString() == target)
                {
                    var = true;
                }
            }
            return var;

        }

        public DataSet GetDataSet(DataSet ds, string select, string table_name)
        {
            cmd.CommandText = select;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            adapter.Fill(ds, table_name);
            ds.Tables[table_name].Clear();
            adapter.Fill(ds, table_name);

            return ds;
        }

        public DataTable GetDataTable(string select)
        {
            DataTable dt = new DataTable();
            cmd.CommandText = select;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            return dt;
        }

        public void search()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string ExecuteNonScalar(string select)
        {
            string id = null;
            conn.Open();
            cmd.CommandText = select;
            cmd.Connection = conn;
            id = cmd.ExecuteScalar().ToString();
            conn.Close();
            return id;

        }

        public void Exceptions(string Extext, string Expage, string Exdate)
        {
            sql = "insert into tbl_Exceptions("
            + "Extext,Expage,Exdate)"
            + " Values("
            + "'" + Extext.ToString().Replace("'", null) + "',"
            + "'" + Expage.ToString().Replace("'", null) + "',"
            + "" + Exdate + ")";
            GetDataTable(sql);
        }

        public void replaces(string txtt)
        {
            txtt = txtt.Replace("'", "");
            //string ts = txt;
        }

        public string clear(TextBox txt)
        {
            try
            {
                txt.Text = txt.Text.Replace("'", null);
                txt.Text = txt.Text.Replace("<", null);
                txt.Text = txt.Text.Replace(">", null);
                txt.Text = txt.Text.Replace("\r\n", null);
                return txt.Text;
            }
            catch
            {
                return null;
            }

        }

        public string clearAll(string text)
        {
            try
            {
                TextBox txt = new TextBox();
                txt.Text = text;
                return clear(txt);
            }
            catch
            {
                return null;
            }

        }

        public bool File_Control(FileUpload flp, int max_size)
        {
            bool var = false;
            try
            {
                string extension = System.IO.Path.GetExtension(flp.FileName);
                int file_size = flp.PostedFile.ContentLength;
                if (flp.FileName == null || flp.FileName == "")
                {
                    return true;
                }
                else
                {
                    if (Regex.IsMatch(extension.ToLower(), ".jpg|.jpeg|.gif|.png|.bmp|.txt|.jpg|.doc|.xls|.pdf|.jpeg|.rar|.zip") == false)
                    {
                        var = false;
                    }
                    else
                    {
                        var = true;

                    }
                    if (max_size < file_size)
                    {
                        var = false;
                    }
                }
                return var;
            }
            catch (Exception exception)
            {
                Exceptions(exception.Message, "hakan.cs", "GetDate()");
                return false;
            }
        }

        public static bool DeleteNotesAndAttachments(HttpContext ctx, Guid sRelatedId)
        {
            if ((sRelatedId.ToString() != null) && (sRelatedId.ToString() != "00000000-0000-0000-0000-000000000000"))
            {
                try
                {
                    string[] FileNames = null;  // dizin içinde dosyalar

                    SqlCommand cmd = DB.SQL(ctx, "SELECT NotesID FROM Notes WHERE OwnerID=@OwnerID ORDER BY NoteDate DESC");
                    DB.AddParam(cmd, "@OwnerID", sRelatedId);
                    cmd.Prepare();

                    IDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        // Ýlgili dizinin varlýðý kontrol ediliyor                       
                        if (System.IO.Directory.Exists(ctx.Server.MapPath("~").ToString() + "/CRM/AttachmentFiles/" + rdr["NotesID"].ToString()))
                        {
                            // Dizin içindeki dosya isimleri arraya atýlýyor
                            FileNames = System.IO.Directory.GetFiles(ctx.Server.MapPath("~").ToString() + "/CRM/AttachmentFiles/" + rdr["NotesID"].ToString());
                            if (FileNames.Length > 0)
                            {
                                // Dosyalar siliniyor
                                for (int j = 0; j < FileNames.Length; j++)
                                {
                                    System.IO.File.Delete(FileNames[j].ToString());
                                }
                            }
                            // Dizin siliniyor
                            System.IO.Directory.Delete(ctx.Server.MapPath("~").ToString() + "/CRM/AttachmentFiles/" + rdr["NotesID"].ToString());
                        }
                    }
                    rdr.Close();

                    cmd = DB.SQL(ctx, "DELETE a FROM Attachments a, Notes b WHERE a.NotesID=b.NotesID and b.OwnerID=@OwnerID");
                    DB.AddParam(cmd, "@OwnerID", sRelatedId);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    cmd = DB.SQL(ctx, "DELETE FROM Notes WHERE OwnerID=@OwnerID ");
                    DB.AddParam(cmd, "@OwnerID", sRelatedId);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public static void CreateScript(System.Web.UI.Page aspxPage, string script, string strKey)
        {
            string strScript = "<script language='JavaScript'>" + script + "</script>";

            if (!aspxPage.IsStartupScriptRegistered(strKey))
            {
                aspxPage.RegisterStartupScript(strKey, strScript);
            }
        }

        public static bool DeleteNoteAndAttachments(HttpContext ctx, Guid sId)
        {
            if ((sId.ToString() != null) && (sId.ToString() != "00000000-0000-0000-0000-000000000000"))
            {
                try
                {
                    string[] FileNames = null;  // dizin içinde dosyalar

                    SqlCommand cmd = DB.SQL(ctx, "SELECT NotesID FROM Notes WHERE NotesID=@NotesID");
                    DB.AddParam(cmd, "@NotesID", sId);
                    cmd.Prepare();

                    IDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        // Ýlgili dizinin varlýðý kontrol ediliyor                       
                        if (System.IO.Directory.Exists(ctx.Server.MapPath("~").ToString() + "/CRM/AttachmentFiles/" + rdr["NotesID"].ToString()))
                        {
                            // Dizin içindeki dosya isimleri arraya atýlýyor
                            FileNames = System.IO.Directory.GetFiles(ctx.Server.MapPath("~").ToString() + "/CRM/AttachmentFiles/" + rdr["NotesID"].ToString());
                            if (FileNames.Length > 0)
                            {
                                // Dosyalar siliniyor
                                for (int j = 0; j < FileNames.Length; j++)
                                {
                                    System.IO.File.Delete(FileNames[j].ToString());
                                }
                            }
                            // Dizin siliniyor
                            System.IO.Directory.Delete(ctx.Server.MapPath("~").ToString() + "/CRM/AttachmentFiles/" + rdr["NotesID"].ToString());
                        }
                    }
                    rdr.Close();

                    cmd = DB.SQL(ctx, "DELETE FROM Attachments WHERE NotesID=@NotesID");
                    DB.AddParam(cmd, "@NotesID", sId);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    cmd = DB.SQL(ctx, "DELETE FROM Notes WHERE NotesID=@NotesID ");
                    DB.AddParam(cmd, "@NotesID", sId);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public static void CreateJavaScript(System.Web.UI.Page page, string key, string script)
        {
            ClientScriptManager csm = page.ClientScript;
            if (!csm.IsStartupScriptRegistered(page.GetType(), key))
                csm.RegisterStartupScript(page.GetType(), key, script, true);
        }

        public static void CreateTxtFile(System.Web.UI.Page page, string args)
        {
            //TextWriter tw = new StreamWriter("MailAttachment.txt");
            //tw.WriteLine(args);
            //tw.Close();
            File.Create(page.Server.MapPath("~").ToString() + "/Deneme.txt");
        }

        public static bool ControllToDate(System.Web.UI.Page page, string date)
        {
            if (date == null || date.Trim() == "")
                return true;
            if (Convert.ToDateTime(date.Trim()).Date >= Convert.ToDateTime("01.01.1753 00:00:00").Date)
                return false;
            else
                return true;
        }

        public static bool IsEmail(string Email)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(Email))
                return (true);
            else
                return (false);
        }

        public static bool IsNullOrEmptyDateTime(string date)
        {
            if ((date == null) || (date.Trim() == ""))
                return true;
            else
            {
                if (Convert.ToDateTime(date) >= Convert.ToDateTime("01.01.1753 00:00:00"))
                    return false;
                else
                    return true;
            }
        }

        public static void ExportToxls(DevExpress.Web.ASPxGridView.Export.ASPxGridViewExporter gridExport, string gridID, bool Landscape)
        {
            gridExport.GridViewID = gridID;
            gridExport.TopMargin = 0;
            gridExport.LeftMargin = 0;
            gridExport.Landscape = Landscape;
            DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
            options.UseNativeFormat = false;
            if (gridExport.GridView.SettingsText.Title.Length > 0)
                options.SheetName = gridExport.GridView.SettingsText.Title;
            else
                options.SheetName = "grid";
            gridExport.WriteXlsToResponse(options);
        }

        public static void ExportTopdf(DevExpress.Web.ASPxGridView.Export.ASPxGridViewExporter gridExport, string gridID, bool Landscape)
        {
            gridExport.GridViewID = gridID;
            gridExport.TopMargin = 0;
            gridExport.LeftMargin = 0;
            gridExport.Landscape = Landscape;
            gridExport.WritePdfToResponse();
        }

    }
}