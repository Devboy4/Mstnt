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
using log4net;
using Model.Crm;

public partial class controls_DataTableControl : UserControl, IDataSource
{
    private static readonly ILog log = LogManager.GetLogger(typeof(controls_DataTableControl));

    private Guid guid;

    public event EventHandler DataSourceChanged;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected override void OnInit(EventArgs e)
    {
        this.guid = Guid.NewGuid();
        this.Disposed += new EventHandler(controls_DataTableControl_Disposed);
        this.Page.RegisterRequiresControlState(this);
        base.OnInit(e);
    }

    void controls_DataTableControl_Disposed(object sender, EventArgs e)
    {
        this.Session[this.guid.ToString()] = null;        
    }

    protected override Object SaveControlState()
    {
        object[] o = new object[2];

        o[0] = base.SaveControlState();
        o[1] = this.guid;
        return o;
    }

    protected override void LoadControlState(object savedState)
    {
        object[] o = (object[])savedState;
        base.LoadControlState(o[0]);
        this.guid = (Guid)o[1];
    }

    public DataTable Table
    {
        get 
        {
            DataTable table = this.Session[this.guid.ToString()] as DataTable;
            if (table == null)
            {
                this.Table = new DataTable();
                this.Table.DefaultView.RowStateFilter |= DataViewRowState.Deleted;
                return this.Table;
            }
            else 
            {
                return table;
            }
        }

        set 
        {
            this.Session[this.guid.ToString()] = value;
        }
    }

    public DataSourceView GetView(string viewName)
    {
        return new DataTableView(this, viewName, this.Table);
    }

    public ICollection GetViewNames()
    {
        return null;
    }
}
