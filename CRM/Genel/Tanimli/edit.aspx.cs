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
using System.Collections.Generic;
using DevExpress.Web.ASPxClasses;

public partial class CRM_Genel_Tanimli_edit : System.Web.UI.Page
{
    DataTable dt = new DataTable();
    CrmUtils data = new CrmUtils();

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

        if (!Security.CheckPermission(this.Context, "Bildirim Ekle", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayı görme yetkisine sahip değilsiniz!</p></body></html>");
            this.Response.End();
        }

        InitGridTable(this.DataTableList.Table);
        fillcomboxes();
        ControlDegerProject();
        string sID = this.Request.Params["id"].Replace("'", "");
        if ((sID != null) && (sID.Trim() != "0"))
        {
            this.Page.Title = Request.Url.ToString();
            Guid id = new Guid(sID);
            this.HiddenID.Value = id.ToString();
            LoadDocument(id);
        }

        


    }

    void fillcomboxes()
    {
        data.BindComboBoxesNoEmpty(this.Context, FirmaId, "EXEC FirmaListByUserName '" + Membership.GetUser().UserName + "'", "FirmaId", "FirmaName");
        data.BindComboBoxesNoEmpty(this.Context, ProjeId, "EXEC AllowedProjeList '" + Membership.GetUser().UserName + "'", "ProjeId", "Adi");
        data.BindComboBoxesNoEmpty(this.Context, VirusSinifId, "EXEC SP_GetVirusSinif '" + Membership.GetUser().UserName + "'", "IndexId", "Adi");
        using (SqlCommand cmd = DB.SQL(this.Context, "Exec ISAssignUserByUserNamev3 @UserName"))
        {
            DB.AddParam(cmd, "@UserName", 100, Membership.GetUser().UserName);
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                adapter.SelectCommand = cmd;
                adapter.Fill(this.DTUsers.Table);
            }
        }
        
    }

    private bool GetYurutmePerm()
    {
        bool _result = false;
        try
        {
            int sayi = 0;
            using (SqlCommand cmd = DB.SQL(this.Context, "Exec SP_GetYurutmePerm '" + Membership.GetUser().UserName + "'"))
            {
                cmd.Prepare();
                sayi = (int)cmd.ExecuteScalar();
                if (sayi != 0)
                    _result = true;
                else
                    _result = false;
            }
        }
        catch
        {
            return false;
        }
        return _result;
    }

    protected void ASPxMenu1_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("save"))
        {
            Validate();

            if (!this.IsValid)
            {
                CrmUtils.MessageAlert(this.Page, "Eksik veya yanlış bilgi girişi!", "stkey1");
                return;
            }
            if (!SaveDocument())
            {
                CrmUtils.CreateMessageAlert(this.Page, (String)Session["Hata"], "stkey1");
                return;
            }
            else
            {

                Response.Write("<script language='Javascript1.2'>{ parent.opener.location.href='./list.aspx'; }</script>");
                Response.Write("<script language='Javascript1.2'>{ parent.close(); }</script>");
            }
        }
    }

    private bool SaveDocument()
    {
        try
        {

            DB.BeginTrans(this.Context);
            Guid id = Guid.Empty;
            if (this.HiddenID.Value.Length != 0)
            {
                id = new Guid(this.HiddenID.Value);
            }
            bool bnewdocument = true;
            SqlCommand cmd = null;
            StringBuilder sb = new StringBuilder();
            if (id == Guid.Empty)
            {
                #region insert
                sb = new StringBuilder();
                sb.Append("INSERT INTO PeriyodikIsler (");
                sb.Append("PeriyodikIslerID");
                sb.Append(",Step");
                sb.Append(",Saat");
                sb.Append(",BaslangicTarihi");
                sb.Append(",FirmaID");
                sb.Append(",ProjeID");
                sb.Append(",Baslik");
                sb.Append(",Active");
                sb.Append(",OperationTime");
                sb.Append(",VirusSinifId");
                if (!CrmUtils.ControllToDate(this.Page, SonrakiIslemTarihi.Date.ToString()))
                    sb.Append(",SonrakiIslemTarihi");
                if (!CrmUtils.ControllToDate(this.Page, SonIslemTarihi.Date.ToString()))
                    sb.Append(",SonIslemTarihi");
                if (Description.Text != null)
                    sb.Append(",Description");
                sb.Append(",CreatedBy");
                sb.Append(",CreationDate)");
                sb.Append(" VALUES(");
                sb.Append("@PeriyodikIslerID");
                sb.Append(",@Step");
                sb.Append(",@Saat");
                sb.Append(",@BaslangicTarihi");
                sb.Append(",@FirmaId");
                sb.Append(",@ProjeId");
                sb.Append(",@Baslik");
                sb.Append(",@Active");
                sb.Append(",@OperationTime");
                sb.Append(",@VirusSinifId");
                if (!CrmUtils.ControllToDate(this.Page, SonrakiIslemTarihi.Date.ToString()))
                    sb.Append(",@SonrakiIslemTarihi");
                if (!CrmUtils.ControllToDate(this.Page, SonIslemTarihi.Date.ToString()))
                    sb.Append(",@SonIslemTarihi");
                if (Description.Text != null)
                    sb.Append(",@Description");
                sb.Append(",@CreatedBy");
                sb.Append(",@CreationDate) SELECT @@IDENTITY AS NewID");
                cmd = DB.SQL(this.Context, sb.ToString());
                id = Guid.NewGuid();
                DB.AddParam(cmd, "@PeriyodikIslerID", id);
                DB.AddParam(cmd, "@CreatedBy", 100, Membership.GetUser().UserName);
                DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                #endregion
            }
            else
            {
                bnewdocument = false;
                #region update
                sb = new StringBuilder();
                sb.Append("UPDATE PeriyodikIsler SET ");
                sb.Append("Step=@Step");
                sb.Append(",Saat=@Saat");
                sb.Append(",BaslangicTarihi=@BaslangicTarihi");
                sb.Append(",FirmaId=@FirmaId");
                sb.Append(",ProjeId=@ProjeId");
                sb.Append(",Active=@Active");
                sb.Append(",OperationTime=@OperationTime");
                sb.Append(",Baslik=@Baslik");
                sb.Append(",VirusSinifId=@VirusSinifId");
                sb.Append(",Description=@Description");
                sb.Append(",ModifiedBy=@ModifiedBy");
                sb.Append(",ModificationDate=@ModificationDate");
                sb.Append(" WHERE PeriyodikIslerID=@PeriyodikIslerID");
                cmd = DB.SQL(this.Context, sb.ToString());
                DB.AddParam(cmd, "@PeriyodikIslerID", id);
                DB.AddParam(cmd, "@ModifiedBy", 100, Membership.GetUser().UserName);
                DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                #endregion
            }

            #region values
            DB.AddParam(cmd, "@Step",  Convert.ToInt32(Step.Value));
            DB.AddParam(cmd, "@Saat", Convert.ToInt32(Saat.Value));
            DB.AddParam(cmd, "@OperationTime", Convert.ToInt32(OperationTime.Value));
            DB.AddParam(cmd, "@BaslangicTarihi", this.BaslangicTarihi.Date);
            DB.AddParam(cmd, "@Description", 255, this.Description.Text.ToUpper());
            DB.AddParam(cmd, "@Baslik", 4000, this.Baslik.Text.ToUpper());
            if (this.Active.Checked)
                DB.AddParam(cmd, "@Active", 1);
            else
                DB.AddParam(cmd, "@Active", 0);

            int tempID;
            if (!String.IsNullOrEmpty(FirmaId.Text))
            {
                tempID = int.Parse(FirmaId.Value.ToString());
                DB.AddParam(cmd, "@FirmaId", tempID);

            }
            else
                DB.AddParam(cmd, "@FirmaId", SqlDbType.Int);
            if (!String.IsNullOrEmpty(ProjeId.Text))
            {
                tempID = int.Parse(ProjeId.Value.ToString());
                DB.AddParam(cmd, "@ProjeId", tempID);

            }
            else
                DB.AddParam(cmd, "@ProjeId", SqlDbType.UniqueIdentifier);
            if (!String.IsNullOrEmpty(VirusSinifId.Text))
            {
                tempID = int.Parse(VirusSinifId.Value.ToString());
                DB.AddParam(cmd, "@VirusSinifId", tempID);

            }
            else
                DB.AddParam(cmd, "@VirusSinifId", SqlDbType.Int);

            #endregion

            cmd.Prepare();
            object stempID = null;
            if (bnewdocument)
                stempID = (object)cmd.ExecuteScalar();
            else
            {
                cmd.ExecuteNonQuery();
                stempID = this.HiddenIndexId.Value;
            }


            #region User List
            DataTable changes = this.DataTableList.Table.GetChanges();
            if (changes != null)
            {
                foreach (DataRow row in changes.Rows)
                {
                    switch (row.RowState)
                    {
                        case DataRowState.Added:
                            sb = new StringBuilder();                    
                            sb.Append("INSERT INTO UserListByTanimliVirus(Id,PeriyodikIsId,UserId,CreatedBy,CreationDate) ");
                            sb.Append("VALUES(@Id,@PeriyodikIsId,@UserId,@CreatedBy,@CreationDate)");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@PeriyodikIsId", int.Parse(stempID.ToString()));
                            DB.AddParam(cmd, "@UserId", (int)row["UserId"]);
                            DB.AddParam(cmd, "@CreatedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Modified:
                            sb = new StringBuilder();
                            sb.Append("UPDATE UserListByTanimliVirus SET ");
                            sb.Append("UserId=@UserId,");
                            sb.Append("ModifiedBy=@ModifiedBy,");
                            sb.Append("ModificationDate=@ModificationDate ");
                            sb.Append("WHERE Id=@Id");
                            cmd = DB.SQL(this.Context, sb.ToString());
                            DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                            DB.AddParam(cmd, "@UserId", (int)row["UserId"]);
                            DB.AddParam(cmd, "@ModifiedBy", 60, Membership.GetUser().UserName);
                            DB.AddParam(cmd, "@ModificationDate", DateTime.Now);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                        case DataRowState.Deleted:
                            cmd = DB.SQL(this.Context, "DELETE FROM UserListByTanimliVirus WHERE ID=@ID");
                            DB.AddParam(cmd, "@ID", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                            break;
                    }
                }
            }
            #endregion




            DB.Commit(this.Context);
            return true;
        }
        catch (Exception ex)
        {
            DB.Rollback(this.Context);
            Session["Hata"] = "Hata:" + ex.Message.Replace("'", null);
            return false;
        }
    }

    void LoadDocument(Guid id)
    {
        #region Fill Main Table
        StringBuilder sb = new StringBuilder();
        sb = new StringBuilder("SELECT * from PeriyodikIsler Where PeriyodikIslerID=@PeriyodikIslerID");

        SqlCommand cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@PeriyodikIslerID", id);
        cmd.Prepare();
        IDataReader rdr = cmd.ExecuteReader();
        if (!rdr.Read())
        {
            this.Response.StatusCode = 500;
            this.Response.End();
            return;
        }
        this.HiddenIndexId.Value = rdr["IndexId"].ToString();
        this.Baslik.Text = rdr["Baslik"].ToString();
        this.Description.Text = rdr["Description"].ToString();
        this.Step.Value = Convert.ToInt32(rdr["Step"]);
        this.Saat.Value = Convert.ToInt32(rdr["Saat"]);
        this.OperationTime.Value = Convert.ToInt32(rdr["OperationTime"]);
        this.BaslangicTarihi.Value = rdr["BaslangicTarihi"];
        this.SonIslemTarihi.Value = rdr["SonIslemTarihi"];
        this.SonrakiIslemTarihi.Value = rdr["SonrakiIslemTarihi"];
        this.Active.Checked = (bool)rdr["Active"];
        if ((rdr["FirmaId"].ToString() != null) && (rdr["FirmaId"].ToString() != ""))
            this.FirmaId.SelectedIndex = this.FirmaId.Items.IndexOfValue(rdr["FirmaId"]);
        if ((rdr["ProjeId"].ToString() != null) && (rdr["ProjeId"].ToString() != ""))
            this.ProjeId.SelectedIndex = this.ProjeId.Items.IndexOfValue(rdr["ProjeId"]);
        if ((rdr["VirusSinifId"].ToString() != null) && (rdr["VirusSinifId"].ToString() != ""))
            this.VirusSinifId.SelectedIndex = this.VirusSinifId.Items.IndexOfValue(rdr["VirusSinifId"]);
        rdr.Close(); 
        #endregion

        #region FillUsers
        this.DataTableList.Table.Rows.Clear();
        sb = new StringBuilder("Select t1.*,t2.UserName,t2.FirstName,t2.LastName from UserListByTanimliVirus t1 ");
        sb.Append("LEFT JOIN SecurityUsers t2 on (t1.UserId=t2.IndexId) left join PeriyodikIsler t3 on(t1.PeriyodikIsId=t3.IndexId)  Where t3.PeriyodikIslerId=@Id");
        cmd = DB.SQL(this.Context, sb.ToString());
        DB.AddParam(cmd, "@Id", id);
        cmd.Prepare();
        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();
            row["ID"] = rdr["Id"];
            row["UserId"] = rdr["UserId"];
            row["UserName"] = rdr["UserName"];
            row["FirstName"] = rdr["FirstName"];
            row["LastName"] = rdr["LastName"];
            this.DataTableList.Table.Rows.Add(row);

        }
        rdr.Close();
        this.DataTableList.Table.AcceptChanges();
        grid.DataBind();

        if(this.VirusSinifId.Value.ToString()=="205")
        {
            this.DTUsers.Table.Rows.Clear();
            using (cmd = DB.SQL(this.Context, "Exec ISAssignUserByUserNameV6 @UserName"))
            {
                DB.AddParam(cmd, "@UserName", 100, Membership.GetUser().UserName);
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(this.DTUsers.Table);
                }
            }
        }
        #endregion
    }

    protected void ProjeId_Callback(object source, CallbackEventArgsBase e)
    {
        if (e.Parameter.ToString() == null || e.Parameter.ToString() == "")
            return;
        StringBuilder sb = new StringBuilder();
        sb.Append("EXEC AllowedProjeListV3 '" + Membership.GetUser().UserName + "','" + e.Parameter.ToString() + "'");
        data.BindComboBoxesNoEmpty(this.Context, ProjeId, sb.ToString(), "ProjeID", "Adi");
    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("UserId", typeof(int));
        dt.Columns.Add("UserName", typeof(string));
        dt.Columns.Add("FirstName", typeof(string));
        dt.Columns.Add("LastName", typeof(string));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("ModifiedBy", typeof(string));
        dt.Columns.Add("ModificationDate", typeof(DateTime));
        dt.Columns.Add("CreationDate", typeof(DateTime));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["UserId"] == null)
        {
            e.RowError = "Lütfen kullanıcı alanını boş bırakmayınız...";
            return;
        }

        if (grid.IsNewRowEditing)
        {

            DataRow[] rows = DataTableList.Table.Select("UserId='" + e.NewValues["UserId"].ToString() + "'");

            if (rows.Length > 0)
            {
                e.RowError = "Bu kullanıcı daha önce eklenmiş, lütfen başka bir kullanıcı seçiniz...";
                return;
            }
        }
    }

    protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (String.IsNullOrEmpty(e.Parameters)) return;
        if(e.Parameters=="205")
        {
            this.DTUsers.Table.Rows.Clear();
            this.DataTableList.Table.Rows.Clear();
            using (SqlCommand cmd = DB.SQL(this.Context, "Exec ISAssignUserByUserNameV6 @UserName"))
            {
                DB.AddParam(cmd, "@UserName", 100, Membership.GetUser().UserName);
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(this.DTUsers.Table);
                    grid.DataBind();
                }
            }
        }
        else
        {
            this.DTUsers.Table.Rows.Clear();
            this.DataTableList.Table.Rows.Clear();
            using (SqlCommand cmd = DB.SQL(this.Context, "Exec ISAssignUserByUserNamev3 @UserName"))
            {
                DB.AddParam(cmd, "@UserName", 100, Membership.GetUser().UserName);
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(this.DTUsers.Table);
                    grid.DataBind();
                }
            }
        }

    }

    private void ControlDegerProject()
    {
        try
        {
            using (SqlCommand cmd = DB.SQL(this.Context, "EXEC ControlDegerProject '" + Membership.GetUser().UserName + "'"))
            {
                String ControlConfig = (String)cmd.ExecuteScalar();
                if (ControlConfig == null || ControlConfig == "Atama Yok") return;
                char[] seps = { '|' };
                string[] Columns = ControlConfig.ToString().Split(seps);
                string Projeid = Columns[0].ToString();
                string Firmaid = Columns[1].ToString();
                StringBuilder sb = new StringBuilder();
                sb.Append("EXEC AllowedProjeListV2 '" + Membership.GetUser().UserName.ToLower() + "','" + Firmaid + "'");
                data.BindComboBoxesNoEmpty(this.Context, ProjeId, sb.ToString(), "ProjeID", "Adi");
                ProjeId.SelectedIndex = ProjeId.Items.IndexOfValue(int.Parse(Projeid));




                FirmaId.SelectedIndex = FirmaId.Items.IndexOfValue(int.Parse(Firmaid));

                if (!GetYurutmePerm())
                {
                    FirmaId.Enabled = false;
                    ProjeId.Enabled = false;
                }
                else
                {
                    FirmaId.Enabled = true;
                    ProjeId.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
        }

    }
}
