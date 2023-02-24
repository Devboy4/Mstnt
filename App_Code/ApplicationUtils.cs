using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Caching;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Data.SqlClient;
using System.Web.Mail;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Security.Permissions;
using System.Threading;

namespace Model.Crm
{
    public class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            //return base.GetWebRequest(address);
            HttpWebRequest webRequest = (HttpWebRequest)base.GetWebRequest(address);
            webRequest.KeepAlive = false;
            webRequest.Timeout = System.Threading.Timeout.Infinite;
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            webRequest.ConnectionGroupName = Guid.NewGuid().ToString();
            //webRequest.ProtocolVersion = HttpVersion.Version10;
            //webRequest.SendChunked = true;
            return webRequest;
        }
    }


    public class ApplicationUtils
    {

        public void GetAppSettings()
        {

            using (SqlConnection conn = DB.Connect())

            using (SqlCommand cmd = DB.SQL(conn, "SELECT [key],[value] FROM AppSettings"))
            {
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                IDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ConfigurationManager.AppSettings.Set(rdr["key"].ToString(), rdr["value"].ToString());
                }

                rdr.Close();
            }

            #region yeni
            //string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //string path = System.IO.Path.Combine(baseDirectory, "AppSettings.xml");
            //XmlDocument xmldoc = new XmlDocument();
            //xmldoc.Load(path);
            //XmlElement appSettings = xmldoc.DocumentElement;

            //if (appSettings != null)
            //{
            //    int node_count = appSettings.ChildNodes.Count;
            //    for (int i = 0; i < node_count; i++)
            //    {
            //        XmlNode node = appSettings.ChildNodes.Item(i);
            //        if ((node.NodeType == XmlNodeType.Element) && (node.Name == "add"))
            //        {
            //            XmlNode key = node.Attributes.GetNamedItem("key");
            //            XmlNode value = node.Attributes.GetNamedItem("value");
            //            if (key != null)
            //                ConfigurationManager.AppSettings.Set(key.Value, value.Value);

            //            //cmd = DB.SQL(conn, "INSERT INTO AppSettings([key],[value]) VALUES(@key,@value)");
            //            //DB.AddParam(cmd, "@key", 255, key.Value);
            //            //DB.AddParam(cmd, "@value", 500, value.Value);
            //            //cmd.CommandTimeout = 1000;
            //            //cmd.Prepare();
            //            //cmd.ExecuteNonQuery();
            //        }
            //    }
            //    //conn.Close();
            //}
            ////xmldoc.Save(path); 
            #endregion
        }

        public void SetAppSettings(string sKey, string sValue)
        {
            using (SqlConnection conn = DB.Connect())
            using (SqlCommand cmd = DB.SQL(conn, "UPDATE AppSettings SET [value]=@value WHERE [key]=@key"))
            {
                DB.AddParam(cmd, "@key", 255, sKey);
                DB.AddParam(cmd, "@value", 500, sValue);
                cmd.CommandTimeout = 1000;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            #region eski
            //try
            //{
            //    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //    string path = System.IO.Path.Combine(baseDirectory, "AppSettings.xml");
            //    XmlDocument xmldoc = new XmlDocument();
            //    xmldoc.Load(path);
            //    XmlElement appSettings = xmldoc.DocumentElement;

            //    if (appSettings != null)
            //    {
            //        int node_count = appSettings.ChildNodes.Count;
            //        for (int i = 0; i < node_count; i++)
            //        {
            //            XmlNode node = appSettings.ChildNodes.Item(i);
            //            if ((node.NodeType == XmlNodeType.Element) && (node.Name == "add"))
            //            {
            //                XmlNode key = node.Attributes.GetNamedItem("key");
            //                XmlNode value = node.Attributes.GetNamedItem("value");
            //                if (key != null)
            //                {
            //                    if (key.Value == sKey)
            //                    {
            //                        value.Value = sValue;
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    xmldoc.Save(path);
            //}
            //catch (Exception ex)
            //{
            //    //CacheLog("RegisterServiceCache | EXCEPTION | " + DateTime.Now.ToString() + " | " + WindowsIdentity.GetCurrent().Name + " | " + ex.ToString());
            //    SqlConnection conn = null;
            //    string message = ex.Message;
            //    try
            //    {
            //        conn = DB.Connect();
            //        if (message.Length > 500) message = message.Substring(0, 500);
            //        SqlCommand cmd = DB.SQL(conn, "INSERT INTO AppLogs([username],[date],[url],[method],[message]) VALUES(@username,@date,@url,@method,@message)");
            //        DB.AddParam(cmd, "@username", 60, "system");
            //        DB.AddParam(cmd, "@date", DateTime.Now);
            //        DB.AddParam(cmd, "@url", 255, "");
            //        DB.AddParam(cmd, "@method", 60, "SetAppSettings");
            //        DB.AddParam(cmd, "@message", 500, message);
            //        cmd.CommandTimeout = 1000;
            //        cmd.Prepare();
            //        cmd.ExecuteNonQuery();
            //    }
            //    catch (Exception ex1) { }
            //    finally
            //    {
            //        if (conn != null)
            //        {
            //            conn.Close();
            //            conn = null;
            //        }
            //    }
            //}
            #endregion
        }

        public void RemoveCaches()
        {
            int keys = ConfigurationManager.AppSettings.Count;

            for (int i = 1; i <= keys; i++)
            {
                string CacheKey = "Cache" + i.ToString();
                if (null != ConfigurationManager.AppSettings[CacheKey])
                {
                    HttpContext.Current.Cache.Remove(CacheKey);
                }
            }
        }

        public void RegisterServiceCache()
        {
            try
            {
                int keys = ConfigurationManager.AppSettings.Count;

                for (int i = 1; i <= keys; i++)
                {
                    string CacheKey = "Cache" + i.ToString();
                    if (null != ConfigurationManager.AppSettings[CacheKey])
                    {
                        string CacheValue = (string)ConfigurationManager.AppSettings[CacheKey];
                        if ((HttpContext.Current == null) || (HttpContext.Current.Cache[CacheKey] == null))
                        {
                            int CacheExpireHour = Int16.Parse(ConfigurationManager.AppSettings[(CacheKey + "ExpireHour")].ToString());
                            int CacheExpireMinute = Int16.Parse(ConfigurationManager.AppSettings[(CacheKey + "ExpireMinute")].ToString());
                            int CacheExpireAddHour = Int16.Parse(ConfigurationManager.AppSettings[(CacheKey + "ExpireAddHour")].ToString());
                            int CacheExpireAddMinute = Int16.Parse(ConfigurationManager.AppSettings[(CacheKey + "ExpireAddMinute")].ToString());

                            DateTime BaseDate = new DateTime(
                                DateTime.Now.Year,
                                DateTime.Now.Month,
                                DateTime.Now.Day,
                                CacheExpireHour,
                                CacheExpireMinute,
                                0);

                            DateTime ExpireDate = BaseDate;
                            if (ExpireDate < DateTime.Now)
                            {
                                do
                                {
                                    ExpireDate = ExpireDate.AddHours(CacheExpireAddHour).AddMinutes(CacheExpireAddMinute);
                                } while (ExpireDate < DateTime.Now);
                            }

                            HttpContext.Current.Cache.Add(
                                CacheKey,
                                CacheValue,
                                null,
                                ExpireDate,
                                TimeSpan.Zero,
                                CacheItemPriority.NotRemovable,
                                new CacheItemRemovedCallback(Callback_CacheRemoved));

                            ConfigurationManager.AppSettings.Set((CacheKey + "ExpireHour"), ExpireDate.Hour.ToString());
                            ConfigurationManager.AppSettings.Set((CacheKey + "ExpireMinute"), ExpireDate.Minute.ToString());

                            SetAppSettings((CacheKey + "ExpireHour"), ExpireDate.Hour.ToString());
                            SetAppSettings((CacheKey + "ExpireMinute"), ExpireDate.Minute.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CacheLog("RegisterServiceCache | EXCEPTION | " + DateTime.Now.ToString() + " | " + WindowsIdentity.GetCurrent().Name + " | " + ex.ToString());

                string message = ex.Message;
                try
                {
                    if (message.Length > 500) message = message.Substring(0, 500);
                    using (SqlConnection conn = DB.Connect())
                    using (SqlCommand cmd = DB.SQL(conn, "INSERT INTO AppLogs([username],[date],[url],[method],[message]) VALUES(@username,@date,@url,@method,@message)"))
                    {
                        DB.AddParam(cmd, "@username", 60, "system");
                        DB.AddParam(cmd, "@date", DateTime.Now);
                        DB.AddParam(cmd, "@url", 255, "");
                        DB.AddParam(cmd, "@method", 60, "RegisterServiceCache");
                        DB.AddParam(cmd, "@message", 500, message);
                        cmd.CommandTimeout = 1000;
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex1) { }
            }
        }

        public void Callback_CacheRemoved(string key, object value, CacheItemRemovedReason reason)
        {
            GetAppSettings();
            if (reason == CacheItemRemovedReason.Expired)
            {
                //cache in expire date i kontrol ediliyor
                bool DoJob = false;
                if ((DateTime.Now.Hour == Int16.Parse(ConfigurationManager.AppSettings[key + "ExpireHour"]) &&
                    DateTime.Now.Minute == Int16.Parse(ConfigurationManager.AppSettings[key + "ExpireMinute"])))
                    DoJob = true;
                //Cache i tekrar olusturmak için
                HitPage();
                //RegisterServiceCache();
                //cache in expire date uygunsa iþlem yapýlýyor
                if (DoJob)
                {
                    //InvokeWebService(key);
                    CacheExpire(key);
                }
            }
        }

        private void HitPage()
        {
            try
            {

                using (WebClient client = new WebClient())
                {
                    //MyWebClient client = new MyWebClient();
                    client.DownloadData(new Uri(ConfigurationManager.AppSettings["HitPageUrl"].ToString()));
                }
            }
            catch (Exception ex)
            {
                CacheLog("HitPage | EXCEPTION1 | " + DateTime.Now.ToString() + " | " + WindowsIdentity.GetCurrent().Name + " | " + ex.ToString());
                string message = ex.Message;
                try
                {
                    if (message.Length > 500) message = message.Substring(0, 500);
                    using (SqlConnection conn = DB.Connect())
                    using (SqlCommand cmd = DB.SQL(conn, "INSERT INTO AppLogs([username],[date],[url],[method],[message]) VALUES(@username,@date,@url,@method,@message)"))
                    {
                        DB.AddParam(cmd, "@username", 60, "system");
                        DB.AddParam(cmd, "@date", DateTime.Now);
                        DB.AddParam(cmd, "@url", 255, "");
                        DB.AddParam(cmd, "@method", 60, "HitPage");
                        DB.AddParam(cmd, "@message", 500, message);
                        cmd.CommandTimeout = 1000;
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex1) { }
            }
        }

        private void CacheExpire(string key)
        {
            CacheLog("CacheExpire | " + key + " | START | " + DateTime.Now.ToString() + " | " + WindowsIdentity.GetCurrent().Name);

            if (key == "Cache1")
            {
                #region Kontrol Periyodik iþler
                //ArrayList Merkezler = new ArrayList();
                try
                {
                    ConnectionStringSettings css = ConfigurationManager.ConnectionStrings["LocalSqlServer"];
                    using (SqlConnection conn = DB.Connect())
                    using (SqlCommand cmd = DB.SQL(conn, "EXEC ControlPeriyodikIsler"))
                    {
                        cmd.CommandTimeout = 300;
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                    }

                    if (DateTime.Now.Hour == 1)
                    {
                        #region savsaklama listesi mail
                        DataTable dt = new DataTable();
                        dt.Columns.Add("sayac", typeof(int));
                        StringBuilder sb = new StringBuilder();
                        sb = new StringBuilder("Select t1.*,t2.CreatedBy TespitEden,t2.BildirimTarihi,");
                        sb.Append("Case When t2.MainIssueID Is Not Null Then t3.IndexID Else t2.IndexID End As PNR1 ");
                        sb.Append("from SavsaklamaMail t1 ");
                        sb.Append("LEFT JOIN Issue t2 ON (t1.PNR=t2.IndexId) LEFT JOIN Issue t3 ON (t2.MainIssueId=t3.IndexId) ");
                        sb.Append("Where t1.MailSend=0");
                        using (SqlConnection conn = DB.Connect())
                        using (SqlCommand cmd = DB.SQL(conn, sb.ToString()))
                        {
                            IDataReader rdr = cmd.ExecuteReader();

                            sb = new StringBuilder("<table border=\"1\" style=\"font-family: Arial; font-size: 12px; width: 900px\"> ");
                            sb.Append("<tr><td align=\"center\" colspan=\"5\"><b>ZAMANINDA YAPILMAYAN GÜNDEM LÝSTESÝ</b></td></tr>");
                            sb.Append("<tr>");
                            sb.Append("<td align=\"center\" style=\"width:80px\">");
                            sb.Append("<b>PNR</b>");
                            sb.Append("</td>");
                            sb.Append("<td align=\"center\" >");
                            sb.Append("<b>BAÞLIK</b>");
                            sb.Append("</td>");
                            sb.Append("<td align=\"center\" style=\"width:150px\">");
                            sb.Append("<b>TARÝH</b>");
                            sb.Append("</td>");
                            sb.Append("<td align=\"center\" style=\"width:100px\">");
                            sb.Append("<b>TESPÝT EDEN</b>");
                            sb.Append("</td>");
                            sb.Append("<td align=\"center\" style=\"width:150px\">");
                            sb.Append("<b>TESPÝT TARÝHÝ</b>");
                            sb.Append("</td>");
                            sb.Append("</tr>");

                            dt.Rows.Clear();
                            while (rdr.Read())
                            {

                                sb.Append("<tr>");
                                sb.Append("<td align=\"center\" style=\"width:80px\">");
                                sb.Append(rdr["PNR1"].ToString());
                                sb.Append("</td>");
                                sb.Append("<td align=\"center\" >");
                                sb.Append(rdr["Baslik"].ToString());
                                sb.Append("</td>");
                                sb.Append("<td align=\"center\" style=\"width:150px\">");
                                if (rdr["CreationDate"] != null && rdr["CreationDate"].ToString() != "")
                                    sb.Append(Convert.ToDateTime(rdr["CreationDate"].ToString()).ToString("F", System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR")));
                                sb.Append("</td>");
                                sb.Append("<td align=\"center\" style=\"width:100px\">");
                                sb.Append(rdr["TespitEden"].ToString());
                                sb.Append("</td>");
                                sb.Append("<td align=\"center\" style=\"width:150px\">");
                                if (rdr["BildirimTarihi"] != null && rdr["BildirimTarihi"].ToString() != "")
                                    sb.Append(Convert.ToDateTime(rdr["BildirimTarihi"].ToString()).ToString("F", System.Globalization.CultureInfo.CreateSpecificCulture("tr-TR")));
                                sb.Append("</td>");
                                sb.Append("</tr>");

                                DataRow row = dt.NewRow();
                                row["sayac"] = rdr["Sayac"];
                                dt.Rows.Add(row);
                            }
                            rdr.Close();
                        }
                        sb.Append("</table>");
                        dt.AcceptChanges();
                        if (dt.Rows.Count > 0)
                        {
                            //int issayi = MailUtils.JustSendMail("hakan.dogan@msn.com;biradam@biradam.com.tr", sb.ToString(), "Savsaklamaya Düþen Gündem Listesi");
                            int issayi = MailUtils.JustSendMail(ConfigurationManager.AppSettings["SavsaklamaSendList"], sb.ToString(), "Zamanýnda yapýlmayan Gündem Listesi");
                            foreach (DataRow row in dt.Rows)
                            {
                                using (SqlConnection conn = DB.Connect())
                                using (SqlCommand cmd = DB.SQL(conn, "Delete from SavsaklamaMail Where Sayac=@Sayac"))
                                {
                                    cmd.Parameters.Add("@Sayac", SqlDbType.Int);
                                    cmd.Parameters["@Sayac"].Value = Convert.ToInt32(row["Sayac"]);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        #endregion
                    }


                }
                catch (Exception ex) { }
                #endregion

                #region Excel
                //CacheExcel(key, Guid.Empty);
                //Thread.Sleep(500);
                //IEnumerator enumerator = Merkezler.GetEnumerator();
                //while (enumerator.MoveNext())
                //{
                //    Guid MerkezID = (Guid)enumerator.Current;
                //    CacheExcel(key, MerkezID);
                //    Thread.Sleep(500);
                //}
                #endregion

                #region Mail
                //CacheMail(key, Guid.Empty);
                //Thread.Sleep(500);
                //if (ConfigurationManager.AppSettings["SadeceSirket"].ToString() != "1")
                //{
                //    enumerator = Merkezler.GetEnumerator();
                //    while (enumerator.MoveNext())
                //    {
                //        Guid MerkezID = (Guid)enumerator.Current;
                //        CacheMail(key, MerkezID);
                //        Thread.Sleep(500);
                //    }
                //}
                #endregion


            }

            #region Pop3 mail
            if (key == "Cache2")//Download Pop3 Mail
            {
                //Hashtable parameters = new Hashtable();
                //parameters.Add("MailServer", ConfigurationManager.AppSettings["SesMailServerName"].ToString());
                //parameters.Add("MailPort", Convert.ToInt32(ConfigurationManager.AppSettings["SesMailServerPort"].ToString()));
                //parameters.Add("MailUseSsl", false);
                //parameters.Add("MailUserName", ConfigurationManager.AppSettings["SesMailUserName"].ToString());
                //parameters.Add("MailPassword", ConfigurationManager.AppSettings["SesMailPassword"].ToString());
                //parameters.Add("ReturnMail", ConfigurationManager.AppSettings["ReturnMail"].ToString());

                //try
                //{
                //    Net.Mail.Pop3Utils.SaveInboxToDb(HttpContext.Current, parameters);
                //}
                //catch (Exception ex) { }

                //try
                //{
                //    Thread thread = new Thread(new ThreadStart(Net.Mail.Pop3Utils.SaveInboxToDb2));
                //    thread.Start();
                //}
                //catch (Exception ex2) { }

                #region Ses Dosyasý için atanacak virüsler...
                ////ArrayList Merkezler = new ArrayList();
                //try
                //{
                //    ConnectionStringSettings css = ConfigurationManager.ConnectionStrings["LocalSqlServer"];
                //    conn = DB.Connect();

                //    SqlCommand cmd = DB.SQL(conn, "EXEC ControlSoundRelatedViruses");
                //    cmd.CommandTimeout = 1000;
                //    cmd.Prepare();
                //    cmd.ExecuteNonQuery();
                //}
                //catch (Exception ex) { }
                //finally
                //{
                //    if (conn != null)
                //    {
                //        conn.Close();
                //        conn = null;
                //    }
                //}
                #endregion

                //GC.Collect();
            }
            #endregion

            CacheLog("CacheExpire | " + key + " | FINISH | " + DateTime.Now.ToString() + " | " + WindowsIdentity.GetCurrent().Name);


        }

        private bool CacheDB(string CacheKey)
        {
            #region call web service
            Hashtable parameters = new Hashtable();
            parameters.Add("CacheKey", CacheKey);
            return InvokeWebService("CacheDB", parameters);
            #endregion

            #region manuel
            //bool ret = true;
            //SqlConnection conn = null;
            //#region try
            //try
            //{
            //    string DBSpName = (string)ConfigurationManager.AppSettings[CacheKey + "DBSpName"];
            //    conn = DB.Connect();
            //    SqlCommand cmd = DB.SQL(conn, ("EXEC " + DBSpName));
            //    cmd.CommandTimeout = 1000;
            //    cmd.Prepare();
            //    cmd.ExecuteNonQuery();
            //}
            //#endregion
            //#region catch
            //catch (Exception ex)
            //{
            //    CacheLog("CacheDB | EXCEPTION | " + DateTime.Now.ToString() + " | " + WindowsIdentity.GetCurrent().Name + " | " + ex.ToString());
            //    ret = false;
            //}
            //#endregion
            //#region finally
            //finally
            //{
            //    if (conn != null)
            //    {
            //        conn.Close();
            //        conn = null;
            //    }
            //}
            //#endregion
            //return ret; 
            #endregion

        }

        private bool CacheMail(string CacheKey, Guid MerkezID)
        {
            #region call web service
            Hashtable parameters = new Hashtable();
            parameters.Add("CacheKey", CacheKey);
            parameters.Add("MerkezID", MerkezID);
            return InvokeWebService("CacheMail", parameters);
            #endregion

            #region manuel
            //bool ret = true;
            //SqlConnection conn = null;
            //SqlCommand cmd = null;
            //if (CacheKey == "Cache1")
            //{
            //    #region try
            //    try
            //    {
            //        string MerkezAdi = null, MerkezEmail = null;
            //        conn = DB.Connect();
            //        cmd = DB.SQL(conn, "SELECT Adi,Email FROM Merkez WHERE MerkezID=@MerkezID");
            //        DB.AddParam(cmd, "@MerkezID", MerkezID);
            //        cmd.Prepare();
            //        IDataReader rdr = cmd.ExecuteReader();
            //        if (rdr.Read())
            //        {
            //            MerkezAdi = (string)rdr["Adi"];
            //            MerkezEmail = (string)rdr["Email"];
            //        }
            //        rdr.Close();

            //        string FilePath = ConfigurationManager.AppSettings["RWFilePath"].ToString() + ConfigurationManager.AppSettings["Report1FileName"].ToString();
            //        FilePath = FilePath.Replace("/", "\\");
            //        if (!String.IsNullOrEmpty(MerkezAdi))
            //            FilePath = FilePath.Replace(".xls", "") + " - " + MerkezAdi + ".xls";

            //        if (System.IO.File.Exists(FilePath))
            //        {
            //            MailMessage mail = new MailMessage();
            //            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", ConfigurationManager.AppSettings["SmtpServerName"].ToString());
            //            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2");
            //            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            //            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", ConfigurationManager.AppSettings["SmtpUserName"].ToString());
            //            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", ConfigurationManager.AppSettings["SmtpPassword"].ToString());
            //            mail.From = ConfigurationManager.AppSettings["Report1From"].ToString();
            //            if (String.IsNullOrEmpty(MerkezEmail))
            //            {
            //                mail.To = ConfigurationManager.AppSettings["Report1To"].ToString();
            //                mail.Cc = ConfigurationManager.AppSettings["Report1CC"].ToString();
            //            }
            //            else
            //            {
            //                mail.To = MerkezEmail;
            //                mail.Cc = "";
            //            }
            //            mail.Bcc = "";
            //            mail.BodyFormat = MailFormat.Text;
            //            mail.Priority = MailPriority.Normal;
            //            mail.Subject = ConfigurationManager.AppSettings["Report1Subject"].ToString();
            //            mail.Body = ConfigurationManager.AppSettings["Report1Body"].ToString();
            //            mail.Attachments.Add(new MailAttachment(FilePath));
            //            SmtpMail.SmtpServer = ConfigurationManager.AppSettings["SmtpServerName"].ToString();
            //            SmtpMail.Send(mail);
            //        }
            //    }
            //    #endregion
            //    #region catch
            //    catch (Exception ex)
            //    {
            //        CacheLog("CacheMail | EXCEPTION | " + DateTime.Now.ToString() + " | " + WindowsIdentity.GetCurrent().Name + " | " + ex.ToString());
            //        ret = false;
            //    }
            //    #endregion
            //    #region finally
            //    finally
            //    {
            //        if (conn != null)
            //        {
            //            conn.Close();
            //            conn = null;
            //        }
            //    }
            //    #endregion
            //}
            //return ret; 
            #endregion
        }

        private bool CacheExcel(string CacheKey, Guid MerkezID)
        {
            #region call web service
            Hashtable parameters = new Hashtable();
            parameters.Add("CacheKey", CacheKey);
            parameters.Add("MerkezID", MerkezID);
            return InvokeWebService("CacheExcel", parameters);
            #endregion

            #region manuel
            //bool ret = true;
            //StringBuilder sb;
            //SqlConnection conn = null;
            //SqlCommand cmd = null;
            //Microsoft.Office.Interop.Excel.ApplicationClass excel = null;
            //Microsoft.Office.Interop.Excel.Workbook workbook = null;
            //Microsoft.Office.Interop.Excel.Worksheet sheet1 = null;
            //Microsoft.Office.Interop.Excel.Worksheet sheet2 = null;
            //Microsoft.Office.Interop.Excel.Worksheet sheet3 = null;

            //string Tarih = DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString();

            //System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            //#region try
            //try
            //{
            //    excel = new Microsoft.Office.Interop.Excel.ApplicationClass();
            //    excel.Visible = false;
            //    workbook = excel.Workbooks.Add(Type.Missing);

            //    conn = DB.Connect();

            //    string FilePath = ConfigurationManager.AppSettings["RWFilePath"].ToString() + ConfigurationManager.AppSettings["Report1FileName"].ToString();
            //    FilePath = FilePath.Replace("/", "\\");

            //    if ((MerkezID != null) && (MerkezID != Guid.Empty))
            //    {
            //        cmd = DB.SQL(conn, "SELECT Adi FROM Merkez WHERE MerkezID=@MerkezID");
            //        DB.AddParam(cmd, "@MerkezID", MerkezID);
            //        cmd.Prepare();
            //        string MerkezAdi = (string)cmd.ExecuteScalar();
            //        FilePath = FilePath.Replace(".xls", "") + " - " + MerkezAdi + ".xls";
            //    }

            //    if (System.IO.File.Exists(FilePath))
            //        System.IO.File.Delete(FilePath);

            //    #region Sheet1
            //    Hashtable hashRenk = new Hashtable();
            //    Hashtable hashGrafik = new Hashtable();
            //    sheet1 = (Worksheet)workbook.Sheets[1];
            //    sheet1.Name = "Özet";

            //    sheet1.Cells[1, 1] = "Günlük Faaliyet Raporu - Özet   " + Tarih;
            //    sheet1.Cells[2, 4] = "Dünkü Faaliyet Hacmi";
            //    sheet1.Cells[2, 8] = "Ay Baþýndan Beri";
            //    sheet1.Cells[2, 12] = "Yýl Baþýndan Beri";
            //    sheet1.Cells[3, 1] = "Merkez";
            //    sheet1.Cells[3, 2] = "Hizmete Giriþ Tarihi";
            //    sheet1.Cells[3, 3] = "Günlük Ort.MNT Payý(YTL)";
            //    sheet1.Cells[3, 4] = "Hasta";
            //    sheet1.Cells[3, 5] = "Tetkik";
            //    sheet1.Cells[3, 6] = "Fatura";
            //    sheet1.Cells[3, 7] = "MNTPay";
            //    sheet1.Cells[3, 8] = "Hasta";
            //    sheet1.Cells[3, 9] = "Tetkik";
            //    sheet1.Cells[3, 10] = "Fatura";
            //    sheet1.Cells[3, 11] = "MNTPay";
            //    sheet1.Cells[3, 12] = "Hasta";
            //    sheet1.Cells[3, 13] = "Tetkik";
            //    sheet1.Cells[3, 14] = "Fatura";
            //    sheet1.Cells[3, 15] = "MNTPay";

            //    sb = new StringBuilder();
            //    if ((MerkezID != null) && (MerkezID != Guid.Empty))
            //        sb.Append("EXEC rep_GunlukFaaliyetOzet @MerkezID,@Tarih,0");
            //    else
            //        sb.Append("EXEC rep_GunlukFaaliyetOzet NULL,@Tarih,0");

            //    cmd = DB.SQL(conn, sb.ToString());
            //    if ((MerkezID != null) && (MerkezID != Guid.Empty))
            //        DB.AddParam(cmd, "@MerkezID", MerkezID);
            //    DB.AddParam(cmd, "@Tarih", DateTime.Now);
            //    cmd.CommandTimeout = 1000;
            //    cmd.Prepare();
            //    int i = 3;
            //    IDataReader rdr = cmd.ExecuteReader();
            //    while (rdr.Read())
            //    {
            //        i++;
            //        sheet1.Cells[i, 1] = rdr["Merkez"];
            //        sheet1.Cells[i, 2] = rdr["HizmeteGirisTarihi"];
            //        sheet1.Cells[i, 3] = rdr["Ortalama"];
            //        sheet1.Cells[i, 4] = rdr["GunHastaSayisi"];
            //        sheet1.Cells[i, 5] = rdr["GunTetkikTutar"];
            //        sheet1.Cells[i, 6] = rdr["GunFaturaTutar"];
            //        sheet1.Cells[i, 7] = rdr["GunMNTPay"];
            //        sheet1.Cells[i, 8] = rdr["AyHastaSayisi"];
            //        sheet1.Cells[i, 9] = rdr["AyTetkikTutar"];
            //        sheet1.Cells[i, 10] = rdr["AyFaturaTutar"];
            //        sheet1.Cells[i, 11] = rdr["AyMNTPay"];
            //        sheet1.Cells[i, 12] = rdr["YilHastaSayisi"];
            //        sheet1.Cells[i, 13] = rdr["YilTetkikTutar"];
            //        sheet1.Cells[i, 14] = rdr["AyFaturaTutar"];
            //        sheet1.Cells[i, 15] = rdr["YilMNTPay"];

            //        decimal val1 = (decimal)rdr["Ortalama"];
            //        decimal val2 = (decimal)rdr["GunFaturaTutar"];

            //        if (val1 != val2)
            //        {
            //            if (val1 > val2)
            //                hashRenk.Add(i, "red");
            //            else
            //                hashRenk.Add(i, "green");
            //        }

            //        string key = rdr["Merkez"].ToString();
            //        int value = (int)rdr["YilHastaSayisi"];
            //        hashGrafik.Add(key, value);
            //    }
            //    rdr.Close();

            //    ((Range)sheet1.get_Range("C4", ("C" + i.ToString()))).NumberFormat = "#,##0";
            //    ((Range)sheet1.get_Range("D4", ("D" + i.ToString()))).NumberFormat = "#,##0";
            //    ((Range)sheet1.get_Range("E4", ("G" + i.ToString()))).NumberFormat = "#,##0";
            //    ((Range)sheet1.get_Range("H4", ("H" + i.ToString()))).NumberFormat = "#,##0";
            //    ((Range)sheet1.get_Range("I4", ("K" + i.ToString()))).NumberFormat = "#,##0";
            //    ((Range)sheet1.get_Range("L4", ("L" + i.ToString()))).NumberFormat = "#,##0";
            //    ((Range)sheet1.get_Range("M4", ("O" + i.ToString()))).NumberFormat = "#,##0";

            //    Range range = (Range)sheet1.get_Range("A1", ("O" + i.ToString()));
            //    range.Rows.Font.Size = 8;
            //    range.Rows.Font.Name = "Arial";
            //    range.Cells.Borders.LineStyle = XlLineStyle.xlContinuous;
            //    range.Cells.Borders.Weight = XlBorderWeight.xlThin;
            //    range.Interior.ColorIndex = 36;   //36-45-40-34-38-6

            //    range = (Range)sheet1.get_Range("A1", "O1");
            //    range.Rows.Font.Size = 12;
            //    range.MergeCells = true;
            //    range.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            //    range = (Range)sheet1.get_Range("A1", "O3");
            //    range.Rows.Font.Bold = true;

            //    range = (Range)sheet1.get_Range("A2", "A3");
            //    range.MergeCells = true;
            //    //range.WrapText = true;
            //    range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //    range = (Range)sheet1.get_Range("B2", "B3");
            //    range.MergeCells = true;
            //    //range.WrapText = true;
            //    range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //    range = (Range)sheet1.get_Range("C2", "C3");
            //    range.MergeCells = true;
            //    //range.WrapText = true;
            //    range.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            //    range = (Range)sheet1.get_Range("D2", "G2");
            //    range.MergeCells = true;
            //    range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //    range = (Range)sheet1.get_Range("H2", "K2");
            //    range.MergeCells = true;
            //    range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //    range = (Range)sheet1.get_Range("L2", "O2");
            //    range.MergeCells = true;
            //    range.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            //    range = (Range)sheet1.get_Range("A1", ("O" + i.ToString()));
            //    range.Interior.ColorIndex = 36;   //36-45-40-34-38-6

            //    range = (Range)sheet1.get_Range("D2", ("G" + i.ToString()));
            //    range.Rows.Font.Size = 9;
            //    range.Interior.ColorIndex = 15;   //36-45-40-34-38-6
            //    range = (Range)sheet1.get_Range("H2", ("K" + i.ToString()));
            //    range.Rows.Font.Size = 9;
            //    range.Interior.ColorIndex = 34;   //36-45-40-34-38-6
            //    range = (Range)sheet1.get_Range("L2", ("O" + i.ToString()));
            //    range.Rows.Font.Size = 9;
            //    range.Interior.ColorIndex = 38;   //36-45-40-34-38-6

            //    i++;
            //    i++;
            //    sheet1.Cells[i, 1] = "TOPLAM";
            //    sheet1.Cells[i, 4] = "=SUM(D4:D" + (i - 2).ToString() + ")";
            //    sheet1.Cells[i, 5] = "=SUM(E4:E" + (i - 2).ToString() + ")";
            //    sheet1.Cells[i, 6] = "=SUM(F4:F" + (i - 2).ToString() + ")";
            //    sheet1.Cells[i, 7] = "=SUM(G4:G" + (i - 2).ToString() + ")";
            //    sheet1.Cells[i, 8] = "=SUM(H4:H" + (i - 2).ToString() + ")";
            //    sheet1.Cells[i, 9] = "=SUM(I4:I" + (i - 2).ToString() + ")";
            //    sheet1.Cells[i, 10] = "=SUM(J4:J" + (i - 2).ToString() + ")";
            //    sheet1.Cells[i, 11] = "=SUM(K4:K" + (i - 2).ToString() + ")";
            //    sheet1.Cells[i, 12] = "=SUM(L4:L" + (i - 2).ToString() + ")";
            //    sheet1.Cells[i, 13] = "=SUM(M4:M" + (i - 2).ToString() + ")";
            //    sheet1.Cells[i, 14] = "=SUM(N4:N" + (i - 2).ToString() + ")";
            //    sheet1.Cells[i, 15] = "=SUM(O4:O" + (i - 2).ToString() + ")";

            //    range = (Range)sheet1.get_Range(("A" + i.ToString()), ("O" + i.ToString()));
            //    range.Rows.Font.Name = "Arial";
            //    range.Cells.Borders.LineStyle = XlLineStyle.xlContinuous;
            //    range.Cells.Borders.Weight = XlBorderWeight.xlThin;
            //    range = (Range)sheet1.get_Range(("A" + i.ToString()), ("C" + i.ToString()));
            //    range.Rows.Font.Size = 10;
            //    range.Rows.Font.Bold = true;
            //    range.Interior.ColorIndex = 36;   //36-45-40-34-38-6
            //    range.MergeCells = true;
            //    range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //    range = (Range)sheet1.get_Range(("D" + i.ToString()), ("G" + i.ToString()));
            //    range.Rows.Font.Size = 9;
            //    range.Rows.Font.Bold = true;
            //    range.Interior.ColorIndex = 15;   //36-45-40-34-38-6
            //    range = (Range)sheet1.get_Range(("H" + i.ToString()), ("K" + i.ToString()));
            //    range.Rows.Font.Size = 9;
            //    range.Rows.Font.Bold = true;
            //    range.Interior.ColorIndex = 34;   //36-45-40-34-38-6
            //    range = (Range)sheet1.get_Range(("L" + i.ToString()), ("O" + i.ToString()));
            //    range.Rows.Font.Size = 9;
            //    range.Rows.Font.Bold = true;
            //    range.Interior.ColorIndex = 38;   //36-45-40-34-38-6

            //    IDictionaryEnumerator enumerator = hashRenk.GetEnumerator();
            //    while (enumerator.MoveNext())
            //    {
            //        string cell = "D" + enumerator.Key.ToString();
            //        range = (Range)sheet1.get_Range(cell, cell);

            //        if (enumerator.Value.ToString() == "red")
            //            range.Interior.ColorIndex = 3; //3-Red 4-Bright Green 10-Green 43-Lime
            //        if (enumerator.Value.ToString() == "green")
            //            range.Interior.ColorIndex = 4;
            //    }

            //    sheet1.Columns.AutoFit();
            //    #endregion

            //    #region Sheet3
            //    string ChartRange1 = "", ChartRange2 = "";
            //    if ((MerkezID == null) || (MerkezID == Guid.Empty))
            //    {
            //        sheet3 = (Worksheet)workbook.Sheets[3];
            //        //sheet3.Name = "Grafik";
            //        i = 0;
            //        enumerator = hashGrafik.GetEnumerator();
            //        while (enumerator.MoveNext())
            //        {
            //            int val3 = (int)enumerator.Value;
            //            if (val3 > 0)
            //            {
            //                i++;
            //                sheet3.Cells[i, 1] = enumerator.Key.ToString();
            //                sheet3.Cells[i, 2] = (int)enumerator.Value;
            //            }
            //        }
            //        ChartRange1 = "A1";
            //        ChartRange2 = "B" + i.ToString();
            //    }
            //    #endregion

            //    #region Sheet2
            //    sheet2 = (Worksheet)workbook.Sheets[2];
            //    sheet2.Name = "Detay";

            //    sheet2.Cells[1, 1] = "Günlük Faaliyet Raporu - Detay   " + Tarih;
            //    sheet2.Cells[2, 1] = "Merkez";
            //    sheet2.Cells[2, 2] = "Ad";
            //    sheet2.Cells[2, 3] = "Soyad";
            //    sheet2.Cells[2, 4] = "Ödeme Kurumu";
            //    sheet2.Cells[2, 5] = "Ýlk Gönderen Hekim";
            //    sheet2.Cells[2, 6] = "Ýlk Gönderen Hekim Kurumu";
            //    sheet2.Cells[2, 7] = "Tetkik";
            //    sheet2.Cells[2, 8] = "Tetkik Tutarý";
            //    sheet2.Cells[2, 9] = "Mnt Payý(%)";
            //    sheet2.Cells[2, 10] = "Fatura Tutarý";
            //    sheet2.Cells[2, 11] = "Hak Ediþ";
            //    sheet2.Cells[2, 12] = "Kayda Alan";
            //    sheet2.Cells[2, 13] = "Kayýt Tarihi";
            //    sheet2.Cells[2, 14] = "Ýþlem Tarihi";

            //    sb = new StringBuilder();
            //    if ((MerkezID != null) && (MerkezID != Guid.Empty))
            //        sb.Append("EXEC rep_GunlukFaaliyetDetay @MerkezID,@Tarih,0");
            //    else
            //        sb.Append("EXEC rep_GunlukFaaliyetDetay NULL,@Tarih,0");

            //    cmd = DB.SQL(conn, sb.ToString());
            //    if ((MerkezID != null) && (MerkezID != Guid.Empty))
            //        DB.AddParam(cmd, "@MerkezID", MerkezID);
            //    DB.AddParam(cmd, "@Tarih", DateTime.Now);
            //    cmd.CommandTimeout = 1000;
            //    cmd.Prepare();
            //    i = 2;
            //    rdr = cmd.ExecuteReader();
            //    while (rdr.Read())
            //    {
            //        i++;
            //        sheet2.Cells[i, 1] = rdr["MerkezAdi"];
            //        sheet2.Cells[i, 2] = rdr["HastaAdi"];
            //        sheet2.Cells[i, 3] = rdr["HastaSoyadi"];
            //        sheet2.Cells[i, 4] = rdr["OdemeKurumuAdi"];
            //        sheet2.Cells[i, 5] = rdr["GonderenHekim"];
            //        sheet2.Cells[i, 6] = rdr["GonderenKurum"];
            //        sheet2.Cells[i, 7] = rdr["HizmetAdi"];
            //        sheet2.Cells[i, 8] = rdr["TetkikBirimTutar"];
            //        sheet2.Cells[i, 9] = rdr["MntPayiYuzde"];
            //        sheet2.Cells[i, 10] = rdr["FaturaBirimTutar"];
            //        sheet2.Cells[i, 11] = rdr["HakEdis"];
            //        sheet2.Cells[i, 12] = rdr["TetkikCreatedBy"];
            //        sheet2.Cells[i, 13] = rdr["TetkikCreationDate"];
            //        sheet2.Cells[i, 14] = rdr["IslemTarihi"];
            //    }
            //    rdr.Close();

            //    ((Range)sheet2.get_Range("H3", ("K" + i.ToString()))).NumberFormat = "#,##0";
            //    ((Range)sheet2.get_Range("M3", ("N" + i.ToString()))).NumberFormat = "DD/MM/YYYY";

            //    range = (Range)sheet2.get_Range("A1", ("N" + i.ToString()));
            //    range.Rows.Font.Size = 8;
            //    range.Rows.Font.Name = "Arial";
            //    range.Cells.Borders.LineStyle = XlLineStyle.xlContinuous;
            //    range.Cells.Borders.Weight = XlBorderWeight.xlThin;
            //    range.Interior.ColorIndex = 34;   //36-45-40-34-38-6
            //    range = (Range)sheet2.get_Range("A1", "N1");
            //    range.Rows.Font.Size = 12;
            //    range.MergeCells = true;
            //    range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //    range = (Range)sheet2.get_Range("A1", "N2");
            //    range.Rows.Font.Bold = true;
            //    range = (Range)sheet2.get_Range("A2", "N2");
            //    range.Interior.ColorIndex = 45;   //36-45-40-34-38-6

            //    i++;
            //    i++;
            //    sheet2.Cells[i, 1] = "TOPLAM";
            //    sheet2.Cells[i, 7] = "Tetkik Adedi = " + (i - 4).ToString() + "";
            //    sheet2.Cells[i, 8] = "=SUM(H3:H" + (i - 2).ToString() + ")";
            //    sheet2.Cells[i, 10] = "=SUM(J3:J" + (i - 2).ToString() + ")";
            //    sheet2.Cells[i, 11] = "=SUM(K3:K" + (i - 2).ToString() + ")";

            //    range = (Range)sheet2.get_Range(("A" + i.ToString()), ("N" + i.ToString()));
            //    range.Cells.Borders.LineStyle = XlLineStyle.xlContinuous;
            //    range.Cells.Borders.Weight = XlBorderWeight.xlThin;
            //    range.Rows.Font.Name = "Arial";
            //    range.Rows.Font.Size = 9;
            //    range.Rows.Font.Bold = true;
            //    range.Interior.ColorIndex = 45;   //36-45-40-34-38-6
            //    range = (Range)sheet2.get_Range(("A" + i.ToString()), ("F" + i.ToString()));
            //    range.MergeCells = true;
            //    range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //    range = (Range)sheet2.get_Range(("L" + i.ToString()), ("N" + i.ToString()));
            //    range.MergeCells = true;

            //    sheet2.Columns.AutoFit();
            //    #endregion

            //    #region Chart
            //    if ((MerkezID == null) || (MerkezID == Guid.Empty))
            //    {
            //        Chart chart = (Chart)workbook.Charts.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //        chart.ChartType = XlChartType.xlBarClustered;
            //        chart.SetSourceData(sheet3.get_Range(ChartRange1, ChartRange2), 2);
            //        chart.HasTitle = true;
            //        chart.ChartTitle.Text = "YBB Merkez Hasta Sayýsý";
            //        chart.Legend.Delete();
            //        chart.Location(XlChartLocation.xlLocationAsNewSheet, "Grafik");
            //    }
            //    #endregion

            //    workbook.SaveAs(FilePath, XlFileFormat.xlExcel5, Type.Missing, Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //}
            //#endregion
            //#region catch
            //catch (Exception ex)
            //{
            //    CacheLog("CacheExcel | EXCEPTION | " + DateTime.Now.ToString() + " | " + WindowsIdentity.GetCurrent().Name + " | " + ex.ToString());
            //    ret = false;
            //}
            //#endregion
            //#region finally
            //finally
            //{
            //    sheet1 = null;
            //    if (workbook != null)
            //    {
            //        workbook.Close(false, Type.Missing, Type.Missing);
            //        workbook = null;
            //    }
            //    if (excel != null)
            //    {
            //        excel.Quit();
            //        excel = null;
            //    }
            //    if (conn != null)
            //    {
            //        conn.Close();
            //        conn = null;
            //    }
            //}
            //#endregion

            //System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;

            ////GC.Collect();
            ////GC.WaitForPendingFinalizers();
            ////GC.Collect();
            //return ret; 
            #endregion
        }

        private bool CacheLog(string value)
        {
            try
            {
                string FilePath = ConfigurationManager.AppSettings["RWFilePath"].ToString() + ConfigurationManager.AppSettings["LogFileName"].ToString();
                using (StreamWriter writer = new StreamWriter(FilePath, true))
                {
                    writer.WriteLine(value);
                    writer.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool InvokeWebService(string Method, Hashtable parameters)
        {
            //bool ret = true;
            //try
            //{
            //    MntWs.Service ws = new MntWs.Service();
            //    switch (Method)
            //    {
            //        case "CacheExcel":
            //            ret = ws.CacheExcel(((string)parameters["CacheKey"]), ((Guid)parameters["MerkezID"]));
            //            break;
            //        case "CacheMail":
            //            ret = ws.CacheMail(((string)parameters["CacheKey"]), ((Guid)parameters["MerkezID"]));
            //            break;
            //        case "CacheDB":
            //            ret = ws.CacheDB(((string)parameters["CacheKey"]));
            //            break;
            //    }

            //    #region eski
            //    //string SoapEnvelope = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            //    //SoapEnvelope += "<soap:Envelope ";
            //    //SoapEnvelope += "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ";
            //    //SoapEnvelope += "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" ";
            //    //SoapEnvelope += "xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">";
            //    //SoapEnvelope += "<soap:Body>";
            //    //SoapEnvelope += "<" + Method + " xmlns=\"http://tempuri.org/\">";
            //    //IDictionaryEnumerator enumerator = parameters.GetEnumerator();
            //    //while (enumerator.MoveNext())
            //    //{
            //    //    SoapEnvelope += "<" + enumerator.Key.ToString() + ">" + enumerator.Value + "</" + enumerator.Key.ToString() + ">";
            //    //}
            //    //SoapEnvelope += "</" + Method + ">";
            //    //SoapEnvelope += "</soap:Body>";
            //    //SoapEnvelope += "</soap:Envelope>";

            //    //string url = ConfigurationManager.AppSettings["WebServiceUrl"].ToString();
            //    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //    //ASCIIEncoding encoding = new ASCIIEncoding();
            //    //byte[] bytes = encoding.GetBytes(SoapEnvelope);

            //    //request.Method = "POST";
            //    //request.ContentLength = bytes.Length;
            //    //request.Headers.Add("SOAPAction: \"http://tempuri.org/" + Method + "\"");
            //    //request.ContentType = "text/xml; charset=utf-8";
            //    //request.KeepAlive = true;
            //    //request.Timeout = 100000;
            //    ////request.Proxy = System.Net.WebProxy.GetDefaultProxy();
            //    ////request.Proxy.Credentials = CredentialCache.DefaultCredentials;

            //    //Stream newStream = request.GetRequestStream();
            //    //newStream.Write(bytes, 0, bytes.Length);
            //    //newStream.Close();

            //    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //    //Stream dataStream = response.GetResponseStream();
            //    //StreamReader reader = new StreamReader(dataStream);
            //    //string responseFromServer = reader.ReadToEnd();
            //    //reader.Close();
            //    //response.Close();
            //    //request.Abort();
            //    //ret = responseFromServer.Contains("<" + Method + "Result>true</" + Method + "Result>"); 
            //    #endregion
            //}
            //catch (Exception ex)
            //{
            //    CacheLog("InvokeWebService | EXCEPTION | " + DateTime.Now.ToString() + " | " + WindowsIdentity.GetCurrent().Name + " | " + ex.ToString());
            //    SqlConnection conn = null;
            //    string message = ex.Message;
            //    try
            //    {
            //        conn = DB.Connect();
            //        if (message.Length > 500) message = message.Substring(0, 500);
            //        SqlCommand cmd = DB.SQL(conn, "INSERT INTO AppLogs([username],[date],[url],[method],[message]) VALUES(@username,@date,@url,@method,@message)");
            //        DB.AddParam(cmd, "@username", 60, "system");
            //        DB.AddParam(cmd, "@date", DateTime.Now);
            //        DB.AddParam(cmd, "@url", 255, "");
            //        DB.AddParam(cmd, "@method", 60, "InvokeWebService");
            //        DB.AddParam(cmd, "@message", 500, message);
            //        cmd.CommandTimeout = 1000;
            //        cmd.Prepare();
            //        cmd.ExecuteNonQuery();
            //    }
            //    catch (Exception ex1) { }
            //    finally
            //    {
            //        if (conn != null)
            //        {
            //            conn.Close();
            //            conn = null;
            //        }
            //    }
            //    ret = false;
            //}
            //return ret;
            return true;
        }
    }
}