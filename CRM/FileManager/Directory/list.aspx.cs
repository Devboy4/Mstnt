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
using System.IO;
using Model.Crm;
using DevExpress.Web.ASPxMenu;
using DevExpress.Web.Data;
using DevExpress.Web.ASPxTreeList;

public partial class CRM_FileManager_Directory_list : System.Web.UI.Page
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

        if (!Security.CheckPermission(this.Context, "File Manager - Directory", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#e6e6fa'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        InitDTList(this.DTList.Table);

        bool bInsert = Security.CheckPermission(this.Context, "File Manager - Directory", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "File Manager - Directory", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "File Manager - Directory", "Delete");

        if (bInsert || bUpdate || bDelete)
            this.menu.Items.FindByName("save").Visible = true;
        else
            this.menu.Items.FindByName("save").Visible = false;

        for (int i = 0; i < this.TreeList.Columns.Count; i++)
        {
            if (this.TreeList.Columns[i] is TreeListCommandColumn)
            {
                (this.TreeList.Columns[i] as TreeListCommandColumn).NewButton.Visible = bInsert;
                (this.TreeList.Columns[i] as TreeListCommandColumn).EditButton.Visible = bUpdate;
                (this.TreeList.Columns[i] as TreeListCommandColumn).DeleteButton.Visible = bDelete;
                break;
            }
        }

        LoadDocument();
    }

    public void menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("excel"))
        {
            ExportUtils.TreeListExport(this.Page, this.TreeListExporter, ExportType.xls, true);
            return;
        }
        else if (e.Item.Name.Equals("pdf"))
        {
            ExportUtils.TreeListExport(this.Page, this.TreeListExporter, ExportType.pdf, true);
            return;
        }

        if (e.Item.Name.Equals("save"))
        {
            this.TreeList.UpdateEdit();
            if (SaveDocument()) this.Response.Redirect("./list.aspx");
            else CrmUtils.MessageAlert(this.Page, ((string)this.Session["HATA"]).Replace("'", " ").Replace("\r", "").Replace("\n", ""), "Alert");
            return;
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
        dt.Columns.Add("ParentId", typeof(Guid));
        dt.Columns.Add("DirectoryName", typeof(string));
        dt.Columns.Add("Description", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("CreationDate", typeof(DateTime));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument()
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM FMDirectory");
        cmd.CommandTimeout = 1000;
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DTList.Table.NewRow();
            row["ID"] = rdr["Id"];
            row["ParentId"] = rdr["ParentId"];
            row["DirectoryName"] = rdr["DirectoryName"];
            row["Description"] = rdr["Description"].ToString();
            row["CreatedBy"] = rdr["CreatedBy"];
            row["CreationDate"] = rdr["CreationDate"];
            row["ModifiedBy"] = rdr["ModifiedBy"];
            row["ModificationDate"] = rdr["ModificationDate"];
            this.DTList.Table.Rows.Add(row);
        }
        rdr.Close();
        this.DTList.Table.AcceptChanges();
    }

    private bool SaveDocument()
    {
        DB.BeginTrans(this.Context);

        SqlCommand cmd = null;
        IDataReader rdr = null;
        StringBuilder sb, sb1, sb2;

        DataTable changes = this.DTList.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    #region insert
                    case DataRowState.Added:
                        sb1 = new StringBuilder("INSERT INTO FMDirectory(Id,DirectoryName,Description,CreatedBy,CreationDate");
                        sb2 = new StringBuilder("VALUES(@Id,@DirectoryName,@Description,@CreatedBy,@CreationDate");
                        if ((row["ParentId"] != null) && (!String.IsNullOrEmpty(row["ParentId"].ToString())))
                        {
                            sb1.Append(",ParentId");
                            sb2.Append(",@ParentId");
                        }
                        sb1.Append(") ");
                        sb2.Append(")");
                        try
                        {

                            //if (!Directory.Exists(row["DirectoryName"].ToString())) Directory.CreateDirectory(name);
                            cmd = DB.SQL(this.Context, (sb1.ToString() + sb2.ToString()));
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            if ((row["ParentId"] != null) && (!String.IsNullOrEmpty(row["ParentId"].ToString())))
                                DB.AddParam(cmd, "@ParentId", (Guid)row["ParentId"]);
                            DB.AddParam(cmd, "@DirectoryName", 255, row["DirectoryName"].ToString());
                            DB.AddParam(cmd, "@Description", 255, row["Description"].ToString());
                            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
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
                    #region update
                    case DataRowState.Modified:
                        sb = new StringBuilder("UPDATE FMDirectory SET ");
                        sb.Append("DirectoryName=@DirectoryName,Description=@Description");
                        sb.Append(",ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate");
                        if ((row["ParentId"] != null) && (!String.IsNullOrEmpty(row["ParentId"].ToString())))
                            sb.Append(",ParentId=@ParentId");
                        else
                            sb.Append(",ParentId=NULL");
                        sb.Append(" WHERE Id=@Id");
                        try
                        {
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            if ((row["ParentId"] != null) && (!String.IsNullOrEmpty(row["ParentId"].ToString())))
                                DB.AddParam(cmd, "@ParentId", (Guid)row["ParentId"]);
                            DB.AddParam(cmd, "@DirectoryName", 255, row["DirectoryName"].ToString());
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
                            #region dizinde veya alt dizinde dosya kontrolu
                            Hashtable _HtDirectories = new Hashtable();
                            Hashtable _HtDirectoriesChecked = new Hashtable();
                            Hashtable _HtSubDirectories = new Hashtable();
                            _HtDirectories.Add(row["ID", DataRowVersion.Original], 0);
                            bool _continue = (_HtDirectories.Count > 0);

                            while (_continue)
                            {
                                #region varsa alt dizinleri listeye ekle
                                foreach (Guid key in _HtSubDirectories.Keys)
                                {
                                    if (!_HtDirectories.ContainsKey(key)) _HtDirectories.Add(key, 0);
                                }
                                _HtSubDirectories = new Hashtable();
                                #endregion

                                #region kontrol edilenleri set et
                                foreach (Guid key in _HtDirectoriesChecked.Keys)
                                {
                                    _HtDirectories[key] = 1;
                                }
                                _HtDirectoriesChecked = new Hashtable();
                                #endregion

                                int _kontrol_edilen = 0;

                                foreach (Guid key in _HtDirectories.Keys)
                                {
                                    if (Convert.ToInt32(_HtDirectories[key]) == 0)
                                    {
                                        _kontrol_edilen++;

                                        #region dizin altýnda dosya kontrolu
                                        cmd = DB.SQL(this.Context, "SELECT COUNT(*) FROM FMFile WHERE DirectoryId=@Id");
                                        DB.AddParam(cmd, "@Id", key);
                                        cmd.CommandTimeout = 1000;
                                        cmd.Prepare();
                                        int _count = Convert.ToInt32(cmd.ExecuteScalar());
                                        if (_count > 0)
                                        {
                                            _continue = false;
                                            //break;
                                            DB.Rollback(this.Context);
                                            this.Session["HATA"] = "Dizinde veya alt dizinlerde dosya olduðundan " + row["DirectoryName", DataRowVersion.Original].ToString() + " dizini silemezsiniz!";
                                            return false;
                                        }
                                        //_HtDirectories[key] = 1;
                                        _HtDirectoriesChecked.Add(key, 1);
                                        #endregion

                                        #region alt dizinleri al listeye eklemek için
                                        cmd = DB.SQL(this.Context, "SELECT * FROM FMDirectory WHERE ParentId=@Id");
                                        DB.AddParam(cmd, "@Id", key);
                                        cmd.CommandTimeout = 1000;
                                        cmd.Prepare();
                                        rdr = cmd.ExecuteReader();
                                        while (rdr.Read())
                                        {
                                            _HtSubDirectories.Add(rdr["Id"], 0);
                                        }
                                        rdr.Close();
                                        #endregion
                                    }
                                }
                                if (_kontrol_edilen == 0) _continue = false;
                            } 
                            #endregion

                            //if (Directory.Exists(row["DirectoryName"].ToString())) Directory.Delete(name);
                            cmd = DB.SQL(this.Context, "DELETE FROM FMDirectory WHERE Id=@Id");
                            DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
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
                }
            }
        }

        DB.Commit(this.Context);

        return true;
    }

    protected void TreeList_InitNewNode(object sender, ASPxDataInitNewRowEventArgs e)
    {
        Guid _Id = Guid.NewGuid();
        if (e.NewValues["ID"] == null) e.NewValues["ID"] = _Id;
        if (e.NewValues["ParentId"] == null) e.NewValues["ParentId"] = DBNull.Value;
    }

    protected void TreeList_NodeInserting(object sender, ASPxDataInsertingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (!String.IsNullOrEmpty(TreeList.NewNodeParentKey))
                e.NewValues["ParentId"] = new Guid(TreeList.NewNodeParentKey);
            else
                if (e.NewValues["ParentId"] == null) e.NewValues["ParentId"] = DBNull.Value;
        }
    }

    protected void TreeList_NodeUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        if (e.NewValues.Values.Count > 0)
        {
            if (!String.IsNullOrEmpty(TreeList.NewNodeParentKey))
                e.NewValues["ParentId"] = new Guid(TreeList.NewNodeParentKey);
            else
                if (e.NewValues["ParentId"] == null) e.NewValues["ParentId"] = DBNull.Value;
        }
    }

    protected void TreeList_NodeValidating(object sender, TreeListNodeValidationEventArgs e)
    {
        if (e.NewValues["DirectoryName"] == null)
        {
            e.NodeError = "Lütfen Dizin Adý alanýný boþ býrakmayýnýz...";
            return;
        }
    }
}
