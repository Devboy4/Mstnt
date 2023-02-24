using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;


namespace Model.Crm
{
    public class MailUtils
    {
        public static DataTable AddAttachFile(System.Web.UI.Page page, HttpContext ctx, FileUpload file, DataTable NewDataTable)
        {
            DataTable ExDataTable = new DataTable();
            Guid TempFolderID = Guid.NewGuid();
            try
            {
                if (!file.HasFile) return ExDataTable;
                string root = ctx.Server.MapPath("~").ToString();
                StringBuilder FilePath = new StringBuilder();
                FilePath.Append("/CRM/MailDosya/");
                ExDataTable = NewDataTable;
                FilePath.Append(TempFolderID.ToString());

                try
                {
                    if (!System.IO.Directory.Exists(root + FilePath.ToString()))
                        System.IO.Directory.CreateDirectory(root + FilePath.ToString());
                    file.SaveAs(root + FilePath.ToString() + "/" + file.FileName);
                }
                catch (Exception exi)
                {
                    CrmUtils.MessageAlert(page, "SaveFileException", "SaveFile5");
                    return ExDataTable;
                }

                int FileSize = file.PostedFile.ContentLength;
                string FileType = "Bayt";

                if ((FileSize >= 1024) && (FileSize < 1024 * 1024))
                {
                    FileType = "KB";
                    FileSize = FileSize / 1024;
                }
                else if ((FileSize >= (1024 * 1024)) && (FileSize < (1024 * 1024 * 1024)))
                {
                    FileType = "MB";
                    FileSize = FileSize / (1024 * 1024);
                }
                else if ((FileSize >= (1024 * 1024 * 1024)))
                {
                    FileType = "GB";
                    FileSize = FileSize / (1024 * 1024 * 1024);
                }

                string filepath = root + FilePath.ToString() + "/" + file.FileName;
                DataRow row = NewDataTable.NewRow();
                row["ID"] = Guid.NewGuid();
                row["NotDosyaID"] = TempFolderID.ToString();
                row["DosyaAdi"] = file.FileName;
                row["DosyaBoyut"] = FileSize;
                row["BoyutTuru"] = FileType;
                row["DosyaYolu"] = filepath;
                row["CreationDate"] = DateTime.Now;
                NewDataTable.Rows.Add(row);
                NewDataTable.AcceptChanges();
                return NewDataTable;

            }
            catch (Exception ex)
            {
                return ExDataTable;
            }

        }

        public static bool DeleteFile(System.Web.UI.Page page, HttpContext ctx, string FilePath)
        {
            try
            {
                FilePath = FilePath.ToString().Replace("/", "\\");
                string DirectoryPath = FilePath.Substring(0, FilePath.LastIndexOf("\\"));
                string[] DirectoryPathFolderList = System.IO.Directory.GetFiles(DirectoryPath);
                for (int i = 0; i < DirectoryPathFolderList.Length; i++)
                {
                    try
                    {
                        if (System.IO.File.Exists(DirectoryPathFolderList[i].ToString()))
                            System.IO.File.Delete(DirectoryPathFolderList[i].ToString());
                    }
                    catch (Exception ex)
                    {
                        CrmUtils.MessageAlert(page, "DeleteFiles1 - DeleteFileException", "DeleteFiles1");
                        return false;
                    }
                }
                try
                {
                    if (System.IO.Directory.Exists(DirectoryPath))
                        System.IO.Directory.Delete(DirectoryPath);
                }
                catch (Exception ex)
                {
                    CrmUtils.MessageAlert(page, "DeleteFiles2 - DeleteDirectoryException", "DeleteFiles2");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static int SendMail(System.Web.UI.Page page, HttpContext ctx, DataTable EmailList, DataTable AttachmentList, string MailBody, string MailSubject)
        {
            string HtmlPath = null;
            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();
            System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential("info@deriden.com", "321321");
            string root = page.Server.MapPath("~") + "/";

            try
            {
                if (EmailList.Rows.Count == 0) return -1;
                if (EmailList.Rows.Count > 100) return -1;
                MailAddress Adress = new MailAddress("info@deriden.com");
                smtpClient.Host = "csmtpauth.superonline.com";

                smtpClient.Port = 587;
                message.From = Adress;
                if (AttachmentList.Rows.Count > 0)
                {
                    foreach (DataRow row2 in AttachmentList.Rows)
                    {
                        if (row2["DosyaYolu"].ToString().Length > 0)
                        {
                            string Path = row2["DosyaYolu"].ToString();
                            Attachment data = new Attachment(Path, MediaTypeNames.Application.Octet);
                            message.Attachments.Add(data);
                        }
                    }
                }
                foreach (DataRow row1 in EmailList.Rows)
                {
                    if (row1["email"].ToString().Length > 0)
                    {
                        message.To.Clear();
                        message.To.Add(row1["email"].ToString());
                        message.BodyEncoding = System.Text.Encoding.GetEncoding("windows-1254");
                        message.Subject = MailSubject;
                        message.IsBodyHtml = true;
                        message.Priority = MailPriority.Normal;
                        message.Body = MailBody;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = basicAuthenticationInfo;
                        smtpClient.Send(message);
                    }
                }
                message.Attachments.Dispose();
                message.Dispose();
                if (AttachmentList.Rows.Count > 0)
                {
                    foreach (DataRow row2 in AttachmentList.Rows)
                    {
                        if (row2["DosyaYolu"].ToString().Length > 0)
                        {
                            string DeleteAtt = row2["DosyaYolu"].ToString();
                            bool bDirectoryDelete = MailUtils.DeleteFile(page, ctx, DeleteAtt);
                        }
                    }
                }
                return 1;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static DataTable AddAttachFileInIssue(System.Web.UI.Page page, HttpContext ctx, FileUpload file, DataTable NewDataTable, Guid OwnerId)
        {
            DataTable ExDataTable = new DataTable();
            Guid TempFolderID = Guid.NewGuid();
            try
            {
                if (!file.HasFile) return ExDataTable;
                string root = ctx.Server.MapPath("~").ToString();
                StringBuilder FilePath = new StringBuilder();
                FilePath.Append("/CRM/IssueAttachments/");
                ExDataTable = NewDataTable;
                FilePath.Append(TempFolderID.ToString());

                try
                {
                    if (!System.IO.Directory.Exists(root + FilePath.ToString()))
                        System.IO.Directory.CreateDirectory(root + FilePath.ToString());
                    file.SaveAs(root + FilePath.ToString() + "/" + file.FileName);
                }
                catch (Exception exi)
                {
                    CrmUtils.MessageAlert(page, "SaveFileException", "SaveFile5");
                    return ExDataTable;
                }

                int FileSize = file.PostedFile.ContentLength;
                string FileType = "Bayt";

                if ((FileSize >= 1024) && (FileSize < 1024 * 1024))
                {
                    FileType = "KB";
                    FileSize = FileSize / 1024;
                }
                else if ((FileSize >= (1024 * 1024)) && (FileSize < (1024 * 1024 * 1024)))
                {
                    FileType = "MB";
                    FileSize = FileSize / (1024 * 1024);
                }
                else if ((FileSize >= (1024 * 1024 * 1024)))
                {
                    FileType = "GB";
                    FileSize = FileSize / (1024 * 1024 * 1024);
                }

                string filepath = root + FilePath.ToString() + "/" + file.FileName;
                DataRow row = NewDataTable.NewRow();
                row["ID"] = Guid.NewGuid();
                row["OwnerID"] = OwnerId;
                row["NotDosyaID"] = TempFolderID.ToString();
                row["DosyaAdi"] = file.FileName;
                row["DosyaBoyut"] = FileSize;
                row["BoyutTuru"] = FileType;
                row["DosyaYolu"] = filepath;
                row["CreationDate"] = DateTime.Now;
                NewDataTable.Rows.Add(row);
                NewDataTable.AcceptChanges();
                return NewDataTable;

            }
            catch (Exception ex)
            {
                return ExDataTable;
            }

        }

        public static bool SendAfterIssue(System.Web.UI.Page page, HttpContext ctx, DataTable MailList, string MailBody, string MailSubject)
        {

            try
            {
                if (MailList.Rows.Count == 0) return false;
                //SmtpClient smtpClient = new SmtpClient();
                MailMessage message = new MailMessage();
                //System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential("model@mbi.com.tr", "model");
                //smtpClient.Host = "smtp.mbi.com.tr";
                //smtpClient.Timeout = 120;
                //smtpClient.Port = 25;
                bool ilk = true;
                message.CC.Clear();
                MailAddress Adres1 = null;

                #region // emailler
                foreach (DataRow row1 in MailList.Rows)
                {
                    if (row1["email"].ToString().Length > 0)
                    {
                        if (ilk)
                        {
                            message.To.Clear();
                            message.To.Add(row1["email"].ToString());
                            Adres1 = new MailAddress(row1["email"].ToString());
                            ilk = false;
                        }
                        else
                        {
                            if (CrmUtils.IsEmail(row1["email"].ToString()))
                            {
                                Adres1 = new MailAddress(row1["email"].ToString());
                                if (!message.To.Contains(Adres1) && !message.CC.Contains(Adres1))
                                {
                                    message.CC.Add(row1["email"].ToString());
                                }
                            }
                        }
                    }
                }
                #endregion

                MailAddress Adress = new MailAddress(ConfigurationManager.AppSettings["SmtpFrom"]);
                message.From = Adress;
                message.BodyEncoding = System.Text.Encoding.GetEncoding("windows-1254");
                message.Subject = MailSubject;
                message.IsBodyHtml = true;   
                message.Priority = MailPriority.Normal;
                message.Body = MailBody;
                MyMail mail = new MyMail();
                mail.SmtpDeliveryMethod = SmtpDeliveryMethod.Network;
                
                    
                mail.SmtpDefaultCredentials = false;
                mail.SmtpHost = "csmtpauth.superonline.com";

                mail.SmtpPort = 587;
                mail.SmtpUserName = "info@deriden.com";
                mail.SmtpPassword = "321321";
                mail.NetMailMessage = message;
                mail.UseWebMail = false;
                mail.Send();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static int JustSendMail(string Email, string MailBody, string MailSubject)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                MailMessage message = new MailMessage();
                System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpUserName"], ConfigurationManager.AppSettings["SmtpPassword"]);
                MailAddress Adress = new MailAddress(ConfigurationManager.AppSettings["SmtpFrom"]);

                string[] maillist = Email.Split(';');
                if (maillist.Length <= 0) return -1;
                message.To.Clear();
                if (maillist.Length > 1)
                {
                    for (int i = 0; i < maillist.Length; i++)
                    {
                        try
                        {
                            message.To.Add(maillist[i]);
                        }
                        catch { }
                       
                    }
                    
                }
                else
                    message.To.Add(Email);
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpEnabledSsl"]))
                    smtpClient.EnableSsl = true;
                smtpClient.Host = ConfigurationManager.AppSettings["SmtpServer"];
                smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                message.From = Adress;  
                message.Subject = MailSubject;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.Normal;
                message.Body = MailBody;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicAuthenticationInfo;
                smtpClient.Send(message);
                message.Dispose();
                return 1;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public static int JustSendMailWithAttach(string Email, string MailBody, string MailSubject, string[] Dosyalar)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                MailMessage message = new MailMessage();
                System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpUserName"], ConfigurationManager.AppSettings["SmtpPassword"]);
                MailAddress Adress = new MailAddress(ConfigurationManager.AppSettings["SmtpFrom"]);

                smtpClient.Host = ConfigurationManager.AppSettings["SmtpServer"];
                smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                message.From = Adress;
                message.To.Clear();
                message.To.Add(Email);
                message.Subject = MailSubject;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.Normal;
                message.Body = MailBody;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicAuthenticationInfo;
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpEnabledSsl"]))
                    smtpClient.EnableSsl = true;
                int sayi = 1;
                string extension = null;
                if (Dosyalar.Length > 0)
                {
                    message.Attachments.Clear();
                    for (int j = 0; j < Dosyalar.Length; j++)
                    {
                        if (Dosyalar[j].ToString().Length > 0)
                        {
                            string Path = Dosyalar[j].ToString().Trim();
                            if (File.Exists(Path))
                            {
                                Attachment data = new Attachment(Path, MediaTypeNames.Application.Octet);
                                ContentDisposition Disposion = data.ContentDisposition;
                                extension = System.IO.Path.GetExtension(Path);
                                Disposion.FileName = "Eklenti" + sayi.ToString() + extension.ToLower();
                                message.Attachments.Add(data);
                                sayi++;
                            }

                        }
                    }
                }

                smtpClient.Send(message);
                message.Dispose();
                return 1;
            }
            catch
            {
                return -1;
            }
        }
    }

}
