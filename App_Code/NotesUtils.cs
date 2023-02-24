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

namespace Model.Crm
{
    public class NotesUtils
    {

        public static bool CheckFile(System.Web.UI.Page page, HttpContext ctx, FileUpload file)
        {
            try
            {
                if (file.FileName == null || file.FileName == "")
                    return true;

                string FileExtension = System.IO.Path.GetExtension(file.FileName);
                int FileSize = file.PostedFile.ContentLength;

                SqlCommand cmd = DB.SQL(ctx, "SELECT DosyaTuru,MaksimumBoyut,BoyutTuru FROM DosyaTuru WHERE DosyaTuru=@DosyaTuru");
                DB.AddParam(cmd, "@DosyaTuru", 10, FileExtension.ToLower());
                cmd.Prepare();
                IDataReader rdr = cmd.ExecuteReader();
                if (!rdr.Read())
                {
                    rdr.Close();
                    CrmUtils.MessageAlert(page, "Göndermek istediðiniz geçerli bir dosya deðildir!", "FileExtension");
                    return false;
                }

                int MaksimumBoyut = (int)rdr["MaksimumBoyut"];
                string BoyutTuru = rdr["BoyutTuru"].ToString();
                rdr.Close();

                int MaximumFileSize = MaksimumBoyut;

                if (BoyutTuru == "KB")
                    MaximumFileSize = MaximumFileSize * 1024;
                if (BoyutTuru == "MB")
                    MaximumFileSize = MaximumFileSize * 1024 * 1024;
                if (BoyutTuru == "GB")
                    MaximumFileSize = MaximumFileSize * 1024 * 1024 * 1024;

                if (FileSize > MaximumFileSize)
                {
                    CrmUtils.MessageAlert(page, "Göndermek istediðiniz dosyanýn boyutu geçerli sýnýrlar içinde deðildir!", "FileSize");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                CrmUtils.MessageAlert(page, "CheckFile - FileException", "FileException");
                return false;
            }
        }

        public static bool SaveFile(System.Web.UI.Page page, HttpContext ctx, FileUpload file, Guid NoteID, int RelatedID, int RelatedType)
        {
            if ((RelatedType < 1) || (RelatedType > 5))
                return false;

            string root = ctx.Server.MapPath("~").ToString();
            StringBuilder FilePath = new StringBuilder();
            FilePath.Append("/CRM/NotDosya/");

            if (RelatedType == 1)
            {
                SqlCommand cmd = DB.SQL(ctx, "SELECT IssueId,Convert(Nvarchar(20),IndexId) AS Unvan FROM Issue WHERE IndexId=@IssueID");
                DB.AddParam(cmd, "@IssueID", RelatedID);
                cmd.Prepare();
                IDataReader rdr = cmd.ExecuteReader();
                if (!rdr.Read())
                {
                    rdr.Close();
                    CrmUtils.MessageAlert(page, "Baðlý gündem bulunamadý!", "SaveFile1");
                    return false;
                }

                FilePath.Append("Bildirimler/");
                FilePath.Append(rdr["Unvan"].ToString());
                FilePath.Append(" (");
                FilePath.Append(rdr["IssueID"].ToString());
                FilePath.Append(")");
                rdr.Close();
            }
            if (RelatedType == 2)
            {
                SqlCommand cmd = DB.SQL(ctx, "SELECT FirmaID,LEFT(FirmaName,20) AS Unvan FROM Firma WHERE FirmaID=@FirmaID");
                DB.AddParam(cmd, "@FirmaID", RelatedID);
                cmd.Prepare();
                IDataReader rdr = cmd.ExecuteReader();
                if (!rdr.Read())
                {
                    rdr.Close();
                    CrmUtils.MessageAlert(page, "Baðlý Maðaza bulunamadý!", "SaveFile2");
                    return false;
                }

                FilePath.Append("Firma/");
                FilePath.Append(rdr["Unvan"].ToString());
                FilePath.Append(" (");
                FilePath.Append(rdr["FirmaID"].ToString());
                FilePath.Append(")");
                rdr.Close();
            }
            if (RelatedType == 3)
            {
                SqlCommand cmd = DB.SQL(ctx, "SELECT ProjeID,LEFT(Adi,20) AS Unvan FROM Proje WHERE ProjeID=@ProjeID");
                DB.AddParam(cmd, "@ProjeID", RelatedID);
                cmd.Prepare();
                IDataReader rdr = cmd.ExecuteReader();
                if (!rdr.Read())
                {
                    rdr.Close();
                    CrmUtils.MessageAlert(page, "Baðlý Departman bulunamadý!", "SaveFile2");
                    return false;
                }

                FilePath.Append("Proje/");
                FilePath.Append(rdr["Unvan"].ToString());
                FilePath.Append(" (");
                FilePath.Append(rdr["ProjeID"].ToString());
                FilePath.Append(")");
                rdr.Close();
            }
            if (RelatedType == 4)
            {
                SqlCommand cmd = DB.SQL(ctx, "SELECT BrTablosuID,Size AS Unvan FROM BrTablosu WHERE IndexId=@BrTablosuID");
                DB.AddParam(cmd, "@BrTablosuID", RelatedID);
                cmd.Prepare();
                IDataReader rdr = cmd.ExecuteReader();
                if (!rdr.Read())
                {
                    rdr.Close();
                    CrmUtils.MessageAlert(page, "Baðlý BR bulunamadý!", "SaveFile2");
                    return false;
                }

                FilePath.Append("BR/");
                FilePath.Append(rdr["Unvan"].ToString());
                FilePath.Append(" (");
                FilePath.Append(rdr["BrTablosuID"].ToString());
                FilePath.Append(")");
                rdr.Close();
            }
            FilePath.Append("/" + NoteID.ToString());

            try
            {
                if (!System.IO.Directory.Exists(root + FilePath.ToString()))
                    System.IO.Directory.CreateDirectory(root + FilePath.ToString());
                file.SaveAs(root + FilePath.ToString() + "/" + file.FileName);
            }
            catch (Exception ex)
            {
                CrmUtils.MessageAlert(page, "SaveFileException", "SaveFile5");
                return false;
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

            DB.BeginTrans(ctx);

            Guid id = Guid.NewGuid();

            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO NotDosya(NotDosyaID,NotID,DosyaAdi,DosyaBoyut,BoyutTuru,DosyaYolu,AllowedRoles,DeniedRoles,CreatedBy,CreationDate) ");
            sb.Append("VALUES (@NotDosyaID,@NotID,@DosyaAdi,@DosyaBoyut,@BoyutTuru,@DosyaYolu,@AllowedRoles,@DeniedRoles,@CreatedBy,@CreationDate) ");
            SqlCommand cmd2 = DB.SQL(ctx, sb.ToString());
            DB.AddParam(cmd2, "@NotDosyaID", id);
            DB.AddParam(cmd2, "@NotID", NoteID);
            DB.AddParam(cmd2, "@DosyaAdi", 255, file.FileName);
            DB.AddParam(cmd2, "@DosyaBoyut", FileSize);
            DB.AddParam(cmd2, "@BoyutTuru", 10, FileType);
            DB.AddParam(cmd2, "@DosyaYolu", 500, filepath);
            DB.AddParam(cmd2, "@AllowedRoles", 255, "");
            DB.AddParam(cmd2, "@DeniedRoles", 255, "");
            DB.AddParam(cmd2, "@CreatedBy", 60, Membership.GetUser().UserName);
            DB.AddParam(cmd2, "@CreationDate", DateTime.Now);
            cmd2.Prepare();
            cmd2.ExecuteNonQuery();

            DB.Commit(ctx);

            return true;
        }

        public static bool SaveFile(System.Web.UI.Page page, HttpContext ctx, FileUpload file, Guid NoteID, int RelatedID, int RelatedType, string Unvan, string IssueId)
        {
            if ((RelatedType < 1) || (RelatedType > 5))
                return false;

            string root = ctx.Server.MapPath("~").ToString();
            StringBuilder FilePath = new StringBuilder();
            FilePath.Append("/CRM/NotDosya/");

            if (RelatedType == 1)
            {
                FilePath.Append("Bildirimler/");
                FilePath.Append(Unvan);
                FilePath.Append(" (");
                FilePath.Append(IssueId);
                FilePath.Append(")");
            }
            if (RelatedType == 2)
            {
                SqlCommand cmd = DB.SQL(ctx, "SELECT FirmaID,LEFT(FirmaName,20) AS Unvan FROM Firma WHERE FirmaID=@FirmaID");
                DB.AddParam(cmd, "@FirmaID", RelatedID);
                cmd.Prepare();
                IDataReader rdr = cmd.ExecuteReader();
                if (!rdr.Read())
                {
                    rdr.Close();
                    CrmUtils.MessageAlert(page, "Baðlý Maðaza bulunamadý!", "SaveFile2");
                    return false;
                }

                FilePath.Append("Firma/");
                FilePath.Append(rdr["Unvan"].ToString());
                FilePath.Append(" (");
                FilePath.Append(rdr["FirmaID"].ToString());
                FilePath.Append(")");
                rdr.Close();
            }
            if (RelatedType == 3)
            {
                SqlCommand cmd = DB.SQL(ctx, "SELECT ProjeID,LEFT(Adi,20) AS Unvan FROM Proje WHERE ProjeID=@ProjeID");
                DB.AddParam(cmd, "@ProjeID", RelatedID);
                cmd.Prepare();
                IDataReader rdr = cmd.ExecuteReader();
                if (!rdr.Read())
                {
                    rdr.Close();
                    CrmUtils.MessageAlert(page, "Baðlý Departman bulunamadý!", "SaveFile2");
                    return false;
                }

                FilePath.Append("Proje/");
                FilePath.Append(rdr["Unvan"].ToString());
                FilePath.Append(" (");
                FilePath.Append(rdr["ProjeID"].ToString());
                FilePath.Append(")");
                rdr.Close();
            }
            if (RelatedType == 4)
            {
                SqlCommand cmd = DB.SQL(ctx, "SELECT BrTablosuID,Size AS Unvan FROM BrTablosu WHERE IndexId=@BrTablosuID");
                DB.AddParam(cmd, "@BrTablosuID", RelatedID);
                cmd.Prepare();
                IDataReader rdr = cmd.ExecuteReader();
                if (!rdr.Read())
                {
                    rdr.Close();
                    CrmUtils.MessageAlert(page, "Baðlý BR bulunamadý!", "SaveFile2");
                    return false;
                }

                FilePath.Append("BR/");
                FilePath.Append(rdr["Unvan"].ToString());
                FilePath.Append(" (");
                FilePath.Append(rdr["BrTablosuID"].ToString());
                FilePath.Append(")");
                rdr.Close();
            }
            FilePath.Append("/" + NoteID.ToString());

            try
            {
                if (!System.IO.Directory.Exists(root + FilePath.ToString()))
                    System.IO.Directory.CreateDirectory(root + FilePath.ToString());
                file.SaveAs(root + FilePath.ToString() + "/" + file.FileName);
            }
            catch (Exception ex)
            {
                CrmUtils.MessageAlert(page, "SaveFileException", "SaveFile5");
                return false;
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

            DB.BeginTrans(ctx);

            Guid id = Guid.NewGuid();

            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO NotDosya(NotDosyaID,NotID,DosyaAdi,DosyaBoyut,BoyutTuru,DosyaYolu,AllowedRoles,DeniedRoles,CreatedBy,CreationDate) ");
            sb.Append("VALUES (@NotDosyaID,@NotID,@DosyaAdi,@DosyaBoyut,@BoyutTuru,@DosyaYolu,@AllowedRoles,@DeniedRoles,@CreatedBy,@CreationDate) ");
            SqlCommand cmd2 = DB.SQL(ctx, sb.ToString());
            DB.AddParam(cmd2, "@NotDosyaID", id);
            DB.AddParam(cmd2, "@NotID", NoteID);
            DB.AddParam(cmd2, "@DosyaAdi", 255, file.FileName);
            DB.AddParam(cmd2, "@DosyaBoyut", FileSize);
            DB.AddParam(cmd2, "@BoyutTuru", 10, FileType);
            DB.AddParam(cmd2, "@DosyaYolu", 500, filepath);
            DB.AddParam(cmd2, "@AllowedRoles", 255, "");
            DB.AddParam(cmd2, "@DeniedRoles", 255, "");
            DB.AddParam(cmd2, "@CreatedBy", 60, Membership.GetUser().UserName);
            DB.AddParam(cmd2, "@CreationDate", DateTime.Now);
            cmd2.Prepare();
            cmd2.ExecuteNonQuery();

            DB.Commit(ctx);

            return true;
        }

        public static bool DeleteFile(System.Web.UI.Page page, HttpContext ctx, Guid NoteFileID)
        {
            SqlCommand cmd = DB.SQL(ctx, "SELECT DosyaYolu FROM NotDosya WHERE NotDosyaID=@NotDosyaID");
            DB.AddParam(cmd, "@NotDosyaID", NoteFileID);
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();
            if (!rdr.Read())
            {
                rdr.Close();
                CrmUtils.MessageAlert(page, "DeleteFile1 - Dosya bulunamadý!", "DeleteFile1");
                return false;
            }
            string FilePath = rdr["DosyaYolu"].ToString().Replace("/", "\\");
            rdr.Close();

            try
            {
                if (System.IO.File.Exists(FilePath))
                    System.IO.File.Delete(FilePath);
            }
            catch (Exception ex)
            {
                CrmUtils.MessageAlert(page, "DeleteFileException", "DeleteFile2");
                return false;
            }

            //DB.BeginTrans(ctx);

            cmd = DB.SQL(ctx, "DELETE FROM NotDosya WHERE NotDosyaID=@NotDosyaID");
            DB.AddParam(cmd, "@NotDosyaID", NoteFileID);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            //DB.Commit(ctx);

            return true;
        }

        public static bool DeleteFiles(System.Web.UI.Page page, HttpContext ctx, Guid NoteID)
        {
            string FilePath = null;
            string DirectoryPath = null;

            SqlCommand cmd = DB.SQL(ctx, "SELECT DosyaYolu FROM NotDosya WHERE NotID=@NotID");
            DB.AddParam(cmd, "@NotID", NoteID);
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                FilePath = rdr["DosyaYolu"].ToString().Replace("/", "\\");
                DirectoryPath = FilePath.Substring(0, FilePath.LastIndexOf("\\"));

                try
                {
                    if (System.IO.File.Exists(FilePath))
                        System.IO.File.Delete(FilePath);
                }
                catch (Exception ex)
                {
                    rdr.Close();
                    CrmUtils.MessageAlert(page, "DeleteFiles1 - DeleteFileException", "DeleteFiles1");
                    return false;
                }
            }
            rdr.Close();

            if (DirectoryPath != null)
            {
                try
                {
                    if (System.IO.Directory.Exists(DirectoryPath))
                        System.IO.Directory.Delete(DirectoryPath);
                }
                catch (Exception ex)
                {
                    rdr.Close();
                    CrmUtils.MessageAlert(page, "DeleteFiles2 - DeleteDirectoryException", "DeleteFiles2");
                    return false;
                }
            }

            //DB.BeginTrans(ctx);

            cmd = DB.SQL(ctx, "DELETE FROM NotDosya WHERE NotID=@NotID");
            DB.AddParam(cmd, "@NotID", NoteID);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd = DB.SQL(ctx, "DELETE FROM Notes WHERE NotID=@NotID");
            DB.AddParam(cmd, "@NotID", NoteID);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            //DB.Commit(ctx);

            return true;
        }

        public static bool DeleteNotes(System.Web.UI.Page page, HttpContext ctx, int RelatedID)
        {
            System.Collections.ArrayList FileList = new System.Collections.ArrayList();
            System.Collections.ArrayList DirectoryList = new System.Collections.ArrayList();
            string[] Attaches;
            SqlCommand cmd = DB.SQL(ctx, "SELECT t1.DosyaYolu FROM NotDosya t1,Notes t2 WHERE t1.NotID=t2.NotID and t2.BagliID=@BagliID");
            DB.AddParam(cmd, "@BagliID", RelatedID);
            cmd.Prepare();
            IDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string FilePath = rdr["DosyaYolu"].ToString().Replace("/", "\\");
                FileList.Add(FilePath);
                DirectoryList.Add(FilePath.Substring(0, FilePath.LastIndexOf("\\")));
                Attaches = System.IO.Directory.GetFiles(FilePath.Substring(0, FilePath.LastIndexOf("\\")));
                for (int i = 0; i < Attaches.Length; i++)
                {
                    if (System.IO.File.Exists(Attaches[i].ToString()))
                        System.IO.File.Delete(Attaches[i].ToString());
                }
            }
            rdr.Close();
            for (int i = 0; i < DirectoryList.Count; i++)
            {
                try
                {
                    if (System.IO.Directory.Exists(DirectoryList[i].ToString()))
                        System.IO.Directory.Delete(DirectoryList[i].ToString());
                }
                catch (Exception ex)
                {
                    CrmUtils.MessageAlert(page, "DeleteDirectoryException", "DeleteNotes2");
                    return false;
                }
            }

            //DB.BeginTrans(ctx);

            cmd = DB.SQL(ctx, "DELETE t1 FROM NotDosya t1,Notes t2 WHERE t1.NotID=t2.NotID and t2.BagliID=@BagliID");
            DB.AddParam(cmd, "@BagliID", RelatedID);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd = DB.SQL(ctx, "DELETE FROM Notes WHERE BagliID=@BagliID");
            DB.AddParam(cmd, "@BagliID", RelatedID);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            //DB.Commit(ctx);

            return true;
        }

        public static bool DeleteLogoes(System.Web.UI.Page page, HttpContext ctx, Guid RelatedID)
        {
            try
            {

                string Mappath = ctx.Server.MapPath("~").ToString();
                string Directory = Mappath + "\\CRM\\Logoes\\" + RelatedID.ToString();
                string[] Logoes = System.IO.Directory.GetFiles(Directory);
                if (!System.IO.Directory.Exists(Directory))
                    return false;
                if (Logoes.Length == 0)
                    return false;
                for (int i = 0; i < Logoes.Length; i++)
                {
                    if (System.IO.File.Exists(Logoes[i].ToString()))
                        System.IO.File.Delete(Logoes[i].ToString());
                }
                System.IO.Directory.Delete(Directory);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static string GetFileIcon(string FileName)
        {
            string FileExtension = FileName.Substring(FileName.LastIndexOf(".") + 1);

            switch (FileExtension.ToLower())
            {
                case "jpg":
                    return "jpg_ico.gif";

                case "jpeg":
                    return "jpg_ico.gif";

                case "gif":
                    return "jpg_ico.gif";

                case "png":
                    return "jpg_ico.gif";

                case "bmp":
                    return "jpg_ico.gif";

                case "txt":
                    return "txt_ico.gif";

                case "pdf":
                    return "pdf_icon.gif";

                case "doc":
                    return "doc_ico.gif";

                case "xls":
                    return "xls_ico.gif";

                case "rar":
                    return "rar_icon.gif";

                case "zip":
                    return "zipIco.gif";

                default:
                    return "ico_16_attach.gif";
            }
        }

        public static string SetNotHeader(System.Web.UI.Page page, HttpContext ctx, Guid RelatedID)
        {
            SqlCommand cmd = DB.SQL(ctx, "SELECT COUNT(*) NotAdedi FROM Notes WHERE BagliID=@BagliID");
            DB.AddParam(cmd, "@BagliID", RelatedID);
            cmd.Prepare();
            int NotAdedi = (int)cmd.ExecuteScalar();

            cmd = DB.SQL(ctx, "SELECT COUNT(t2.DosyaAdi) DosyaAdedi FROM Notes t1,NotDosya t2 WHERE t1.NotID=t2.NotID and t1.BagliID=@BagliID");
            DB.AddParam(cmd, "@BagliID", RelatedID);
            cmd.Prepare();
            int DosyaAdedi = (int)cmd.ExecuteScalar();

            return ("NOT (" + NotAdedi.ToString() + " not, " + DosyaAdedi.ToString() + " dosya)");
        }
    }
}
