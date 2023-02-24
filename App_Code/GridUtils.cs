using System;
using System.Diagnostics;
using System.Data;
using System.Runtime.Serialization;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxMenu;
using log4net;

namespace Model.Crm
{

    [Serializable]
    public class MyDataControl : DataSourceControl, ISerializable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MyDataControl));
        
        private DataTable table;
        //private DataTableView view;

        public MyDataControl(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            this.table = info.GetValue("table", typeof(DataTable)) as DataTable;
            DumpTable("READ SERIAL", this.table);
            base.LoadControlState(info.GetValue("base", typeof(object)));

        }
        
        public static void HandleGrid(Page page, StateBag state, ASPxGridView grid)
        {
            MyDataControl.HandleGrid(page, state, grid, null);
        }

        //[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter= true)]
        public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            DumpTable("WRITE SERIAL", this.table);
            info.AddValue("table", this.table);
            info.AddValue("base", base.SaveControlState());
        }

        protected override void OnInit(EventArgs e)
        {
            DumpTable("ONINIT", this.table);
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);
        }

        protected override void LoadControlState(object savedState)
        {
            object[] rgState = (object[])savedState;

            base.LoadControlState(rgState[0]);
            this.table = rgState[1] as DataTable;
            DumpTable("LOADCONTROLSTATE", this.table);
        }

        protected override object SaveControlState()
        {
            object[] rgState = new object[2];
            rgState[0] = base.SaveControlState();
            rgState[1] = this.table;
            DumpTable("SAVECONTROLSTATE", this.table);

            return rgState;
        }


        public static void HandleGrid(Page page, StateBag state, ASPxGridView grid, DataTable table)
        {
            if (table == null)
            {
                log.Info("HANDLEGRID 2");
                MyDataControl mydata = state[grid.ID + "_data"] as MyDataControl;
                //MyDataControl mydata = page.FindControl(grid.ID + "_data") as MyDataControl;
                page.Controls.Add(mydata);

                //grid.DataSourceID = grid.ID + "_data";
                grid.DataSource = mydata;
                grid.KeyFieldName = mydata.table.Columns[0].ColumnName;
                grid.DataBind();
            }
            else
            {
                //
                // For first Page_Load
                //
                log.Info("HANDLEGRID 1");
                DataColumn[] pricols = new DataColumn[1];
                pricols[0] = table.Columns[0];
                table.PrimaryKey = pricols;

                MyDataControl mydata = new MyDataControl(table);
                mydata.ID = grid.ID + "_data";
                page.Controls.Add(mydata);
                state[mydata.ID] = mydata;
            }
        }


        public MyDataControl(DataTable table)
            : base()
        {
            this.table = table;    
        }
        
        protected override DataSourceView GetView(string viewName)
        {
            log.Info("GetView " + viewName);
            /*if (this.view == null)
            {
                this.view = new DataTableView(this, viewName, this.table);
            }
            return this.view;*/
            return new DataTableView(this, viewName, this.table);
        }


        private void DumpTable(string reason, DataTable table)
        {
            log.Info("TABLE DUMP FOR " + reason);
            foreach (DataRow row in table.Rows)
            {
                log.Info("ROW :" + row[0] + " - " + row[1]);
            }
        }
    }

    public class DataTableView : DataSourceView
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DataTableView));

        private DataTable table;

        public DataTableView(System.Web.UI.IDataSource owner, string viewName, DataTable table) 
            : base(owner, viewName)
        { 
            this.table = table;
            //DumpTable("Constructor", this.table);
        }

        protected override int ExecuteDelete(IDictionary keys, IDictionary oldValues)
        {
            int ret = 0;
            IEnumerator enumer = keys.GetEnumerator();
            while (enumer.MoveNext())
            {
                DictionaryEntry de = (DictionaryEntry)enumer.Current;
                DataRow row = this.table.Rows.Find(de.Value);
                row.Delete();
                //this.table.Rows.Remove(row);
                ret++;
            }

            return ret;
        }
        
        protected override int ExecuteInsert(IDictionary values)
        {
            //DumpTable("PRE ExecuteInsert", this.table);
            DataRow row = this.table.NewRow();
            row["ID"] = Guid.NewGuid();
            IDictionaryEnumerator enumerator = values.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                if (enumerator.Value == null)
                    row[enumerator.Key.ToString()] = DBNull.Value;
                else
                    row[enumerator.Key.ToString()] = enumerator.Value;
            }
            this.table.Rows.Add(row);
            //DumpTable("POST ExecuteInsert", this.table);
            return 1;
        }

        protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
        {
            DataView dv = new DataView(this.table);
            return dv;
        }

        protected override int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues)
        {
            //DumpTable("ExecuteUpdate", this.table);
            int ret = 0;
            IEnumerator enumer = keys.GetEnumerator();
            while (enumer.MoveNext())
            {
                DictionaryEntry de = (DictionaryEntry)enumer.Current;
                DataRow row = this.table.Rows.Find(de.Value);
                IDictionaryEnumerator enumerator = values.GetEnumerator();
                enumerator.Reset();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Value == null)
                        row[enumerator.Key.ToString()] = DBNull.Value;
                    else
                        row[enumerator.Key.ToString()] = enumerator.Value;
                }

                ret++;
            }

            return ret;
        }

        public override bool CanDelete { get { return true; } }
        public override bool CanInsert { get { return true; } }
        public override bool CanRetrieveTotalRowCount { get { return true; } }
        public override bool CanUpdate { get { return true; } }
    }
}