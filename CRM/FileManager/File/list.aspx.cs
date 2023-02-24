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
using Model.Crm;
using DevExpress.Web.ASPxMenu;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using DevExpress.Web.ASPxUploadControl;
using System.IO;

public partial class CRM_FileManager_File_list : System.Web.UI.Page
{
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
        this.menu.ItemClick += new MenuItemEventHandler(menu_ItemClick);
        if (this.IsPostBack || this.IsCallback) return;

        if (!Security.CheckPermission(this.Context, "File Manager - File", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        InitDTList(this.DTList.Table);

        bool bInsert = Security.CheckPermission(this.Context, "File Manager - File", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "File Manager - File", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "File Manager - File", "Delete");

        if (bInsert || bUpdate || bDelete)
            this.menu.Items.FindByName("save").Visible = true;
        else
            this.menu.Items.FindByName("save").Visible = false;

        this.Uploader.Enabled = bInsert;
        this.BtnUpload.Enabled = bInsert;

        for (int i = 0; i < this.Grid.Columns.Count; i++)
        {
            if (this.Grid.Columns[i] is GridViewCommandColumn)
            {
                (this.Grid.Columns[i] as GridViewCommandColumn).NewButton.Visible = false;
                (this.Grid.Columns[i] as GridViewCommandColumn).EditButton.Visible = bUpdate;
                (this.Grid.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
                break;
            }
        }

        object oId = (object)this.Request.Params["id"];
        if ((oId != null) && (oId.ToString().Replace("'", "").Trim() != "0"))
        {
            Guid id = new Guid(oId.ToString().Replace("'", "").Trim());
            LoadDocument(id);
            this.DirectoryId.Value = id.ToString();
            Session["FMDirectoryId"] = this.DirectoryId.Value;
        }
    }

    public void menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("excel"))
        {
            ExportUtils.GridExport(this.Page, this.GridExporter, ExportType.xls, true);
            return;
        }
        else if (e.Item.Name.Equals("pdf"))
        {
            ExportUtils.GridExport(this.Page, this.GridExporter, ExportType.pdf, true);
            return;
        }

        if (e.Item.Name.Equals("save"))
        {
            this.Grid.UpdateEdit();
            if (SaveDocument())
                this.Response.Redirect("./list.aspx?id=" + this.DirectoryId.Value);
            else
                CrmUtils.MessageAlert(this.Page, ((string)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "Alert");
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    private void InitDTList(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("DirectoryId", typeof(Guid));
        dt.Columns.Add("DirectoryName", typeof(string));
        dt.Columns.Add("FileName", typeof(string));
        dt.Columns.Add("FileExtension", typeof(string));
        dt.Columns.Add("FileSize", typeof(int));
        dt.Columns.Add("FileSizeType", typeof(string));
        dt.Columns.Add("Description", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("Kaydeden", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("Guncelleyen", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument(Guid Id)
    {
        SqlCommand cmd = null;
        IDataReader rdr = null;
        StringBuilder sql = null;

        #region grid title
        Guid _Id = Id;
        ArrayList _DirectoryList = new ArrayList();
        while (_Id != Guid.Empty)
        {
            cmd = DB.SQL(this.Context, "SELECT DirectoryName,ParentId FROM FMDirectory WHERE Id=@Id");
            DB.AddParam(cmd, "@Id", _Id);
            cmd.CommandTimeout = 1000;
            cmd.Prepare();
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                //_Title.Append(rdr["DirectoryName"].ToString() + "/");
                _DirectoryList.Add(rdr["DirectoryName"]);
                if ((rdr["ParentId"] != null) && (!String.IsNullOrEmpty(rdr["ParentId"].ToString()))) _Id = new Guid(rdr["ParentId"].ToString());
                else _Id = Guid.Empty;
            }
            rdr.Close();
        }
        StringBuilder _Title = new StringBuilder("~/");
        for (int i = (_DirectoryList.Count - 1); i >= 0; i--)
        {
            _Title.Append(_DirectoryList[i].ToString() + "/");
        }
        this.Grid.SettingsText.Title = _Title.ToString();
        #endregion

        #region dtlist
        sql = new StringBuilder("SELECT t1.*,t2.DirectoryName,(t3.FirstName+' '+t3.LastName) Kaydeden,(t4.FirstName+' '+t4.LastName) Guncelleyen ");
        sql.Append("FROM FMFile t1 ");
        sql.Append("left join FMDirectory t2 on(t1.DirectoryId=t2.Id) ");
        sql.Append("left join SecurityUsers t3 on(t1.CreatedBy=t3.UserName) ");
        sql.Append("left join SecurityUsers t4 on(t1.ModifiedBy=t4.UserName) ");
        sql.Append("WHERE t1.DirectoryId=@DirectoryId ");
        sql.Append("ORDER BY t1.Sayac ASC");
        this.DTList.Table.Rows.Clear();
        cmd = DB.SQL(this.Context, sql.ToString());
        DB.AddParam(cmd, "@DirectoryId", Id);
        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTList.Table.NewRow();
            row["ID"] = rdr["Id"];
            row["DirectoryId"] = rdr["DirectoryId"];
            row["DirectoryName"] = rdr["DirectoryName"];
            row["FileName"] = rdr["FileName"];
            row["FileExtension"] = rdr["FileExtension"];
            row["FileSize"] = rdr["FileSize"];
            row["FileSizeType"] = rdr["FileSizeType"];
            row["Description"] = rdr["Description"];
            row["CreatedBy"] = rdr["CreatedBy"];
            row["Kaydeden"] = rdr["Kaydeden"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["Guncelleyen"] = rdr["Guncelleyen"];
            row["ModificationDate"] = rdr["ModificationDate"];
            this.DTList.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTList.Table.AcceptChanges();
        #endregion
    }

    private bool SaveDocument()
    {
        DB.BeginTrans(this.Context);

        SqlCommand cmd = null;
        StringBuilder sb;

        DataTable changes = this.DTList.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    #region insert
                    case DataRowState.Added:
                        break;
                    #endregion
                    #region update
                    case DataRowState.Modified:
                        sb = new StringBuilder("UPDATE FMFile SET DirectoryId=@DirectoryId,Description=@Description WHERE Id=@Id");
                        try
                        {
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@DirectoryId", (new Guid(this.DirectoryId.Value)));
                            DB.AddParam(cmd, "@Description", 255, row["Description"].ToString());
                            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            cmd.CommandTimeout = 1000;
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            return false;
                        }
                        break;
                    #endregion
                    #region delete
                    case DataRowState.Deleted:
                        try
                        {
                            string _FilePath = this.Context.Server.MapPath("~").ToString() + "\\CRM\\FileManager\\Files\\" + row["FileName", DataRowVersion.Original].ToString();

                            cmd = DB.SQL(this.Context, "DELETE FROM FMFile WHERE Id=@Id");
                            DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.CommandTimeout = 1000;
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();

                            if (File.Exists(_FilePath)) File.Delete(_FilePath);
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            this.Session["HATA"] = ex.Message.ToString();
                            return false;
                        }
                        break;
                    #endregion
                }
            }
        }

        DB.Commit(this.Context);

        return true;
    }

    protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
        }
    }

    protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
        }
    }

    protected void Grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["FileName"] == null)
        {
            e.RowError = "Lütfen Dosya Adý alanýný boþ býrakmayýnýz...";
            return;
        }
    }

    protected void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        //ASPxGridView grid = (ASPxGridView)sender;
        DataTable changes = this.DTList.Table.GetChanges();
        if (changes != null) e.Properties["cpIsDirty"] = true;
        else e.Properties["cpIsDirty"] = false;
    }

    protected void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        e.Cell.ToolTip = string.Format("{0}", e.CellValue);
    }

    protected void Grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string[] parameters = e.Parameters.Split('|');
        switch (parameters[0])
        {
            case "LoadData":
                Guid _DirectoryId = new Guid(Session["FMDirectoryId"].ToString());
                LoadDocument(_DirectoryId);
                this.Grid.DataBind();
                break;
        }
    }

    protected void Uploader_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
    {
        if ((Session["FMDirectoryId"] == null) || (String.IsNullOrEmpty(Session["FMDirectoryId"].ToString())))
        {
            e.IsValid = false;
            e.ErrorText = "Oturum bilgileri kaybedildi! Sayfayý tekrar yükleyiniz!";
            return;
        }

        string _FileName = this.Uploader.UploadedFiles[0].FileName;
        string _FileExtension = System.IO.Path.GetExtension(_FileName);
        int _FileSize = this.Uploader.UploadedFiles[0].PostedFile.ContentLength;
        byte[] _FileBytes = this.Uploader.UploadedFiles[0].FileBytes;

        string _FilePath = this.Context.Server.MapPath("~").ToString() + "\\CRM\\FileManager\\Files\\" + _FileName;

        SqlCommand cmd = null;
        IDataReader rdr = null;
        StringBuilder sql = null;

        #region dosya kontrolu
        try
        {
            cmd = DB.SQL(this.Context, "SELECT DosyaTuru,MaksimumBoyut,BoyutTuru FROM DosyaTuru WHERE DosyaTuru=@DosyaTuru");
            DB.AddParam(cmd, "@DosyaTuru", 10, _FileExtension.ToLower());
            cmd.Prepare();
            rdr = cmd.ExecuteReader();
            if (!rdr.Read())
            {
                rdr.Close();
                e.IsValid = false;
                e.ErrorText = "Yüklemek istediðiniz dosya sistemde tanýmlý dosya türleri içinde deðildir!";
                return;
            }
            int _MaxSize = (int)rdr["MaksimumBoyut"];
            string _SizeType = rdr["BoyutTuru"].ToString();
            rdr.Close();

            int _MaxFileSize = _MaxSize;
            if (_SizeType == "KB") _MaxFileSize = _MaxFileSize * 1024;
            else if (_SizeType == "MB") _MaxFileSize = _MaxFileSize * 1024 * 1024;
            else if (_SizeType == "GB") _MaxFileSize = _MaxFileSize * 1024 * 1024 * 1024;

            if (_FileSize > _MaxFileSize)
            {
                e.IsValid = false;
                e.ErrorText = "Yüklemek istediðiniz dosyanýn boyutu sistemde tanýmlý sýnýrlar içinde deðildir!";
                return;
            }

            cmd = DB.SQL(this.Context, "SELECT COUNT(*) FROM FMFile WHERE FileName=@FileName");
            DB.AddParam(cmd, "@FileName", 255, _FileName);
            cmd.CommandTimeout = 1000;
            cmd.Prepare();
            int _FileExist = Convert.ToInt32(cmd.ExecuteScalar());
            if (_FileExist > 0)
            {
                e.IsValid = false;
                e.ErrorText = _FileName + " dosyasý daha önce yüklenmiþ!";
                return;
            }
        }
        catch (Exception ex)
        {
            e.IsValid = false;
            e.ErrorText = ex.Message;
            return;
        } 
        #endregion

        #region boyut ayarlama
        string _FileSizeType = "B";
        if ((_FileSize >= 1024) && (_FileSize < 1024 * 1024))
        {
            _FileSizeType = "KB";
            _FileSize = _FileSize / 1024;
        }
        else if ((_FileSize >= (1024 * 1024)) && (_FileSize < (1024 * 1024 * 1024)))
        {
            _FileSizeType = "MB";
            _FileSize = _FileSize / (1024 * 1024);
        }
        else if ((_FileSize >= (1024 * 1024 * 1024)))
        {
            _FileSizeType = "GB";
            _FileSize = _FileSize / (1024 * 1024 * 1024);
        } 
        #endregion

        #region kayýt
        DB.BeginTrans(this.Context);
        try
        {
            Guid _Id = Guid.NewGuid();
            Guid _DirectoryId = new Guid(Session["FMDirectoryId"].ToString());
            sql = new StringBuilder("");
            sql.Append("INSERT INTO FMFile(Id,DirectoryId,FileName,FileExtension,FileSize,FileSizeType,Description,CreatedBy,CreationDate) ");
            sql.Append("VALUES(@Id,@DirectoryId,@FileName,@FileExtension,@FileSize,@FileSizeType,@Description,@CreatedBy,@CreationDate) ");
            cmd = DB.SQL(this.Context, sql.ToString());
            DB.AddParam(cmd, "@Id", _Id);
            DB.AddParam(cmd, "@DirectoryId", _DirectoryId);
            DB.AddParam(cmd, "@FileName", 255, _FileName);
            DB.AddParam(cmd, "@FileExtension", 10, _FileExtension);
            DB.AddParam(cmd, "@FileSize", _FileSize);
            DB.AddParam(cmd, "@FileSizeType", 10, _FileSizeType);
            DB.AddParam(cmd, "@Description", 255, "");
            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
            cmd.CommandTimeout = 1000;
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            using (Stream _File = File.Create(_FilePath))
            {
                _File.Write(_FileBytes, 0, _FileBytes.Length);
                _File.Close();
            }
            e.CallbackData = (_FileName + " dosyasý kaydedildi!");
        }
        catch (Exception ex)
        {
            DB.Rollback(this.Context);
            e.IsValid = false;
            e.ErrorText = ex.Message;
            return;
        }
        DB.Commit(this.Context); 
        #endregion
    }
}
