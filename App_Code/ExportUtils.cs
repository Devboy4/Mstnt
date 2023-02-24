using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using DevExpress.Utils;
using DevExpress.XtraPrinting;
using DevExpress.Web.ASPxTreeList.Export;
using DevExpress.Web.ASPxGridView.Export;

namespace Model.Crm
{
    /// <summary>
    /// 
    /// </summary>
    public enum ExportType {pdf, xls, csv, mht, rtf, txt, htm};

    /// <summary>
    /// Summary description for ExportUtils
    /// </summary>
    public class ExportUtils
    {
    

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="exporter"></param>
        /// <param name="exportType"></param>
        /// <param name="saveAs"></param>
        public static void TreeListExport(System.Web.UI.Page page, ASPxTreeListExporter exporter, ExportType exportType, bool saveAs)
        {
            string fileName = "TreeList";
            exporter.Settings.AutoWidth = true;
            exporter.Settings.ExpandAllNodes = true;

            switch (exportType)
            {
                case ExportType.pdf:
                    //PdfExportOptions pdfOption = new PdfExportOptions();
                    exporter.WritePdfToResponse(fileName);
                    break;
                case ExportType.xls:
                    XlsExportOptions xlsOption = new XlsExportOptions(true, true);
                    xlsOption.SheetName = fileName;
                    exporter.WriteXlsToResponse(fileName, xlsOption);
                    break;
                case ExportType.rtf:
                    //RtfExportOptions rtfOption = new RtfExportOptions();
                    exporter.WriteRtfToResponse(fileName);
                    break;
            }
       }

        public static void GridExport(System.Web.UI.Page page, ASPxGridViewExporter exporter, ExportType exportType, bool saveAs)
        {
            string title = exporter.GridView.SettingsText.Title;
            title = title.Replace('ç', 'c');
            title = title.Replace('Ç', 'C');
            title = title.Replace('ð', 'g');
            title = title.Replace('Ð', 'G');
            title = title.Replace('ý', 'i');
            title = title.Replace('Ý', 'I');
            title = title.Replace('ö', 'o');
            title = title.Replace('Ö', 'O');
            title = title.Replace('þ', 's');
            title = title.Replace('Þ', 'S');
            title = title.Replace('ü', 'u');
            title = title.Replace('Ü', 'U');
            string fileName = String.IsNullOrEmpty(exporter.GridView.SettingsText.Title) ? "Grid" : title;
            //string fileName = String.IsNullOrEmpty(exporter.GridView.SettingsText.Title) ? "Grid" : exporter.GridView.SettingsText.Title;

            switch (exportType)
            {
                case ExportType.pdf:
                    //PdfExportOptions pdfOption = new PdfExportOptions();
                    exporter.WritePdfToResponse(fileName);
                    break;
                case ExportType.xls:
                    XlsExportOptions xlsOption = new XlsExportOptions(true, true);
                    xlsOption.SheetName = fileName;
                    exporter.WriteXlsToResponse(fileName, xlsOption);
                    break;
                case ExportType.rtf:
                    //RtfExportOptions rtfOption = new RtfExportOptions();
                    exporter.WriteRtfToResponse(fileName);
                    break;
                case ExportType.csv:
                    //CsvExportOptions csvOption = new CsvExportOptions();
                    exporter.WriteCsvToResponse(fileName);
                    break;
            }

            //using (MemoryStream stream = new MemoryStream())
            //{
            //    string contentType = "", fileName = "Grid";

            //    switch (exportType)
            //    {
            //        case ExportType.pdf:
            //            contentType = "application/pdf";
            //            fileName += ".pdf";
            //            //PdfExportOptions pdfOption = new PdfExportOptions();
            //            exporter.WritePdf(stream);
            //            break;
            //        case ExportType.xls:
            //            contentType = "application/ms-excel";
            //            fileName += ".xls";
            //            XlsExportOptions xlsOption = new XlsExportOptions(false, true);
            //            xlsOption.SheetName = String.IsNullOrEmpty(exporter.GridView.SettingsText.Title) ? "Grid" : exporter.GridView.SettingsText.Title;
            //            exporter.WriteXls(stream, xlsOption);
            //            break;
            //        case ExportType.rtf:
            //            contentType = "text/enriched";
            //            fileName += ".rtf";
            //            //RtfExportOptions rtfOption = new RtfExportOptions();
            //            exporter.WriteRtf(stream);
            //            break;
            //        case ExportType.csv:
            //            contentType = "text/csv";
            //            fileName += ".txt";
            //            //CsvExportOptions csvOption = new CsvExportOptions();
            //            exporter.WriteCsv(stream);
            //            break;
            //        //case ExportType.mht:
            //        //    contentType = "multipart/related";
            //        //    fileName += ".mht";
            //        //    break;
            //        //case ExportType.txt:
            //        //    contentType = "text/plain";
            //        //    fileName += ".txt";
            //        //    break;
            //        //case ExportType.htm:
            //        //    contentType = "text/html";
            //        //    fileName += ".htm";
            //        //    break;
            //    }

            //    byte[] buffer = stream.GetBuffer();

            //    string disposition = saveAs ? "attachment" : "inline";

            //    page.Response.Clear();
            //    page.Response.Buffer = false;
            //    page.Response.AppendHeader("Content-Type", contentType);
            //    page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            //    page.Response.AppendHeader("Content-Disposition", disposition + "; filename=" + fileName);
            //    page.Response.BinaryWrite(buffer);
            //    //page.Response.End();
            //}
        }
    }
}