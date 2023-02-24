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
using DevExpress.Web.ASPxEditors;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
public partial class CRM_Raporlar_MagazaVirusleriChart : System.Web.UI.Page
{
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
        if (!Security.CheckPermission(this.Context, "BirimRapor", "Select"))
        {
            this.Response.Write("<html><body bgcolor='#e6e6fa'><p>Bu sayfayý görme yetkisine sahip deðilsiniz!</p></body></html>");
            this.Response.End();
        }

        // Eger event ile geliyorsa asagidakileri calistirmaya gerek yok
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }

        this.IlkTarih.Date = DateTime.Parse(DateTime.Now.Year.ToString() + "-01-01 00:00:00");
        //this.SonTarih.Date = DateTime.Parse(DateTime.Now.Year.ToString() + "-12-31 23:59:59");
        this.SonTarih.Date = DateTime.Now;

        FillComboxes();


    }

    protected void ProjeID_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        //if (e.Parameter == null || e.Parameter == "") return;
        //data.BindComboBoxes(this.Context, ProjeID, "Select ProjeID,Adi From Proje Where FirmaID='" + e.Parameter + "' Order By Adi", "ProjeID", "Adi");
    }

    private void FillComboxes()
    {
        SqlCommand cmd;
        IDataReader rdr;
        ListEditItem item;
        StringBuilder sb;

        this.FirmaID.Items.Clear();
        item = new ListEditItem();
        item.Text = "---Hepsi---";
        item.Value = Guid.Empty;
        this.FirmaID.Items.Add(item);
        if (Membership.GetUser().UserName.ToLower() == "admin" || Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator")
    || Roles.IsUserInRole(Membership.GetUser().UserName, "Merkez Yöneticileri"))
        {
            sb = new StringBuilder("SELECT FirmaID,FirmaName FROM Firma ORDER BY FirmaName");
            cmd = DB.SQL(this.Context, sb.ToString());
        }
        else
        {
            sb = new StringBuilder("EXEC FirmaListByUserName @UserName");
            cmd = DB.SQL(this.Context, sb.ToString());
            DB.AddParam(cmd, "@UserName", 100, Membership.GetUser().UserName);
        }
        cmd.Prepare();
        rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            item = new ListEditItem();
            item.Text = rdr["FirmaName"].ToString();
            item.Value = rdr["FirmaID"];
            this.FirmaID.Items.Add(item);
        }
        rdr.Close();
        this.FirmaID.SelectedIndex = 0;
    }

    protected void WebChartControl1_CustomCallback(object sender, CustomCallbackEventArgs e)
    {
        switch (e.Parameter)
        {
            case "LoadData":
                ChartLoadData();
                ChartShowLegends();
                ChartShowLabels();
                ChartSortOrder();
                break;
            case "ShowLegends":
                ChartShowLegends();
                break;
            case "ShowLabels":
                ChartShowLabels();
                break;
            case "SortOrder":
                ChartSortOrder();
                break;
        }
    }

    private void ChartLoadData()
    {
        BarChart();
    }

    private void InitTableDtDurumList(DataTable dt)
    {
        dt.Columns.Clear();
        dt.Columns.Add("ID", typeof(string));
        dt.Columns.Add("Durum", typeof(string));

    }

    private void BarChart()
    {
        StringBuilder sb = new StringBuilder();
        SqlCommand cmd;
        IDataReader rdr;
        sb = new StringBuilder("Exec rep_AylikVirusMagaza ");

        if (FirmaID.Value.ToString() != Guid.Empty.ToString()) sb.Append("@FirmaID");
        else sb.Append("Null");
        if (!CrmUtils.IsNullOrEmptyDateTime(this.IlkTarih.Date.ToString())) sb.Append(",@IlkTarih");
        else sb.Append(",Null");
        if (!CrmUtils.IsNullOrEmptyDateTime(this.SonTarih.Date.ToString())) sb.Append(",@SonTarih");
        else sb.Append(",Null");

        SqlConnection conn = DB.Connect(this.Context);
        cmd = DB.SQL(this.Context, sb.ToString());
        cmd.CommandTimeout = 1000;
        if (FirmaID.Value.ToString() != Guid.Empty.ToString()) DB.AddParam(cmd, "@FirmaID", new Guid(this.FirmaID.Value.ToString()));
        if (!CrmUtils.IsNullOrEmptyDateTime(this.IlkTarih.Date.ToString())) DB.AddParam(cmd, "@IlkTarih", this.IlkTarih.Date);
        if (!CrmUtils.IsNullOrEmptyDateTime(this.SonTarih.Date.ToString())) DB.AddParam(cmd, "@SonTarih", this.SonTarih.Date);

        DataTable dt = new DataTable();
        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = cmd;
        adapter.Fill(dt);


        this.WebChartControl1.Series.Clear();
        this.WebChartControl1.Legend.Visible = false;
        this.WebChartControl1.Titles[0].Text = "GÜNDEM RAPORU";

        Hashtable ColumnGrandTotal = new Hashtable();
        bool exist = false;
        foreach (DataRow row in dt.Rows)
        {
            exist = true;
            Series series = new Series(row["DurumAdi"].ToString(), ViewType.Bar);
            this.WebChartControl1.Series.Add(series);
            series.ArgumentScaleType = ScaleType.Qualitative;
            series.ValueScaleType = ScaleType.Numerical;
            series.LegendPointOptions.PointView = PointView.Argument;
            ((SideBySideBarSeriesLabel)series.Label).Visible = true;
            ((SideBySideBarSeriesView)series.View).ColorEach = false;
            switch (row["DurumAdi"].ToString().ToUpper())
            {
                case "ANTÝVÝRÜS":
                case "OKSÝJEN":
                case "GOL":
                case "KAPALI":
                    ((SideBySideBarSeriesView)series.View).Color = System.Drawing.Color.Green;
                    break;
                case "SAVSAKLAMA":
                case "SAVSAKLADIM":
                case "OMÝT":
                case "SAPMA":
                case "YAPILMADI":
                    ((SideBySideBarSeriesView)series.View).Color = System.Drawing.Color.Red;
                    break;
                case "VÝRÜS":
                case "PAS":
                case "KARBON":
                    ((SideBySideBarSeriesView)series.View).Color = System.Drawing.Color.Yellow;
                    break;
                case "ÝPTAL":
                    ((SideBySideBarSeriesView)series.View).Color = System.Drawing.Color.Black;
                    break;
                case "KRONÝK":
                    ((SideBySideBarSeriesView)series.View).Color = System.Drawing.Color.Aqua;
                    break;
                default:
                    ((SideBySideBarSeriesView)series.View).Color = System.Drawing.Color.Red;
                    break;
            }
            int ProjeSayac = int.Parse(row["ProjeSayac"].ToString());
            for (int i = 3; i <= ProjeSayac; i++)
            {
                double value = int.Parse(row[i].ToString());
                series.Points.Add(new SeriesPoint(dt.Columns[i].ToString(), new double[] { value }));
            }
        }

        if (exist)
        {
            ((XYDiagram)this.WebChartControl1.Diagram).AxisX.Title.Text = "BÝRÝMLER";
            ((XYDiagram)this.WebChartControl1.Diagram).AxisX.Title.Visible = true;
            ((XYDiagram)this.WebChartControl1.Diagram).AxisX.Interlaced = true;
            ((XYDiagram)this.WebChartControl1.Diagram).AxisX.GridLines.Visible = true;
            ((XYDiagram)this.WebChartControl1.Diagram).AxisX.Range.SideMarginsEnabled = true;
            ((XYDiagram)this.WebChartControl1.Diagram).AxisX.Label.Angle = -45;

            ((XYDiagram)this.WebChartControl1.Diagram).AxisY.Title.Text = "ADET";
            ((XYDiagram)this.WebChartControl1.Diagram).AxisY.Title.Visible = true;
            ((XYDiagram)this.WebChartControl1.Diagram).AxisY.Range.AlwaysShowZeroLevel = true;
            ((XYDiagram)this.WebChartControl1.Diagram).AxisY.Range.SideMarginsEnabled = true;
            ((XYDiagram)this.WebChartControl1.Diagram).AxisY.NumericOptions.Format = NumericFormat.Number;
        }
    }

    private void ChartShowLegends()
    {
        this.WebChartControl1.Legend.Visible = this.cbShowLegends.Checked;
    }

    private void ChartShowLabels()
    {
        //Series.Label.Visible = cbShowLabels.Checked;
        this.WebChartControl1.SeriesTemplate.Label.Visible = cbShowLegends.Checked;
    }

    private void ChartSortOrder()
    {
        //switch (cbSortOrder.SelectedIndex)
        //{
        //    case 1:
        //        this.WebChartControl1.SeriesSorting = SortingMode.Ascending;
        //        break;
        //    case 2:
        //        this.WebChartControl1.SeriesSorting = SortingMode.Descending;
        //        break;
        //    default:
        //        this.WebChartControl1.SeriesSorting = SortingMode.None;
        //        break;
        //}
    }
}
