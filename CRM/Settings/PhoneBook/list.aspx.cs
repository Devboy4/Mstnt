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
using Model.Crm;
using DevExpress.Web.Data;
using DevExpress.Web.ASPxGridView;

public partial class CRM_Settings_PhoneBook_list : System.Web.UI.Page
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
        if (!Security.CheckPermission(this.Context, "Tanim - Phone Book", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#acc0e9'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        // Eventleri tanimlayalim
        this.menu.ItemClick += new MenuItemEventHandler(menu_ItemClick);

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }

        bool bInsert = Security.CheckPermission(this.Context, "Tanim - Phone Book", "Insert");
        bool bUpdate = Security.CheckPermission(this.Context, "Tanim - Phone Book", "Update");
        bool bDelete = Security.CheckPermission(this.Context, "Tanim - Phone Book", "Delete");

        if (bInsert || bUpdate)
            this.menu.Items.FindByName("save").Visible = true;
        else
            this.menu.Items.FindByName("save").Visible = false;

        InitGridTable(this.DataTableList.Table);

        for (int i = 0; i < this.grid.Columns.Count; i++)
        {
            if (this.grid.Columns[i] is GridViewCommandColumn)
            {
                (this.grid.Columns[i] as GridViewCommandColumn).NewButton.Visible = bInsert;
                (this.grid.Columns[i] as GridViewCommandColumn).EditButton.Visible = bUpdate;
                (this.grid.Columns[i] as GridViewCommandColumn).DeleteButton.Visible = bDelete;
                break;
            }
        }

        LoadDocument();
    }

    public void menu_ItemClick(object source, MenuItemEventArgs e)
    {
        if (e.Item.Name.Equals("save"))
        {
            this.grid.UpdateEdit();
            if (SaveDocument())
            {
                this.Response.Redirect("./list.aspx");
            }
        }
        else
        {
            this.Response.StatusCode = 500;
            this.Response.End();
        }
    }

    private void InitGridTable(DataTable dt)
    {
        dt.Columns.Add("ID", typeof(Guid));
        dt.Columns.Add("FirstName", typeof(string));
        dt.Columns.Add("LastName", typeof(string));
        dt.Columns.Add("CompanyName", typeof(string));
        dt.Columns.Add("PhoneNumber", typeof(string));

        DataColumn[] pricols = new DataColumn[1];
        pricols[0] = dt.Columns["ID"];
        dt.PrimaryKey = pricols;
    }

    private void LoadDocument()
    {
        SqlCommand cmd = DB.SQL(this.Context, "SELECT * FROM PhoneBook ORDER BY CompanyName");
        cmd.Prepare();

        IDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            DataRow row = this.DataTableList.Table.NewRow();

            row["ID"] = rdr["Id"];
            row["FirstName"] = rdr["FirstName"];
            row["LastName"] = rdr["LastName"];
            row["CompanyName"] = rdr["CompanyName"];
            row["PhoneNumber"] = rdr["PhoneNumber"];

            this.DataTableList.Table.Rows.Add(row);
        }
        rdr.Close();

        this.DataTableList.Table.AcceptChanges();
    }

    private bool SaveDocument()
    {
        DB.BeginTrans(this.Context);

        SqlCommand cmd = null;
        StringBuilder sb;

        DataTable changes = this.DataTableList.Table.GetChanges();
        if (changes != null)
        {
            foreach (DataRow row in changes.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        sb = new StringBuilder();
                        sb.Append("INSERT INTO PhoneBook(Id,FirstName,LastName,CompanyName,PhoneNumber)");
                        sb.Append("VALUES(@Id,@FirstName,@LastName,@CompanyName,@PhoneNumber)");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@FirstName", 100, row["FirstName"].ToString().ToUpper());
                        DB.AddParam(cmd, "@LastName", 100, row["LastName"].ToString().ToUpper());
                        DB.AddParam(cmd, "@CompanyName", 100, row["CompanyName"].ToString().ToUpper());
                        DB.AddParam(cmd, "@PhoneNumber", 100, row["PhoneNumber"].ToString().ToUpper());
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Modified:
                        sb = new StringBuilder();
                        sb.Append("UPDATE PhoneBook SET ");
                        sb.Append("FirstName=@FirstName,");
                        sb.Append("LastName=@LastName,");
                        sb.Append("CompanyName=@CompanyName,");
                        sb.Append("PhoneNumber=@PhoneNumber ");
                        sb.Append("WHERE Id=@Id");
                        cmd = DB.SQL(this.Context, sb.ToString());
                        DB.AddParam(cmd, "@Id", (Guid)row["ID"]);
                        DB.AddParam(cmd, "@FirstName", 100, row["FirstName"].ToString().ToUpper());
                        DB.AddParam(cmd, "@LastName", 100, row["LastName"].ToString().ToUpper());
                        DB.AddParam(cmd, "@CompanyName", 100, row["CompanyName"].ToString().ToUpper());
                        DB.AddParam(cmd, "@PhoneNumber", 100, row["PhoneNumber"].ToString().ToUpper());
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        break;
                    case DataRowState.Deleted:
                        try
                        {
                            cmd = DB.SQL(this.Context, "DELETE FROM PhoneBook WHERE Id=@Id");
                            DB.AddParam(cmd, "@Id", (Guid)row["ID", DataRowVersion.Original]);
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DB.Rollback(this.Context);
                            DataTableList.Table.Clear();
                            LoadDocument();
                            grid.DataBind();
                            CrmUtils.MessageAlert(this.Page, ex.Message.ToString().Replace("'", null).Replace("\r\n", null), "stkeySilinemez");
                            return false;
                        }
                        break;
                }
            }
        }

        DB.Commit(this.Context);

        return true;
    }

    protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["FirstName"] == null)
        {
            e.RowError = "Lütfen Adý alanýný boþ býrakmayýnýz...";
            return;
        }
        if (e.NewValues["PhoneNumber"] == null)
        {
            e.RowError = "Lütfen GSM alanýný boþ býrakmayýnýz...";
            return;
        }

        if (e.NewValues["PhoneNumber"].ToString().Substring(0, 1) != "0")
        {
            e.RowError = "Lütfen GSM numarasýný baþýnda 0 olarak yazýnýz...örn:(0532XXXXXXX)";
            return;
        }
        if (e.NewValues["PhoneNumber"].ToString().Length != 11)
        {
            e.RowError = "Lütfen GSM numarasýný baþýnda 0 olarak 11 hane olarak giriniz... örn:(0532XXXXXXX)";
            return;
        }
        if (grid.IsNewRowEditing)
        {
            DataRow[] Rows = DataTableList.Table.Select("PhoneNumber='" + e.NewValues["PhoneNumber"].ToString() + "'");
            if (Rows.Length > 0)
            {
                e.RowError = "Gsm numarasý ekli görünüyor lütfen baþka bir numara ile tekrar deneyiniz...";
                return;
            }
        }
    }
}
