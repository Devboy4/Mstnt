using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Data.Odbc;

/// <summary>
/// Summary description for ExcelUtils
/// </summary>
/// 
namespace Model.Crm
{
    public class ExcelUtils
    {
        public ExcelUtils()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static DataTable ExcelToDs(string _FileName)
        {
            DataTable _result = new DataTable(), ShemaDt = new DataTable();
            string strConnString = string.Empty, sSheetName = string.Empty;
            OleDbCommand selectCommand = new OleDbCommand();
            OleDbConnection connection = new OleDbConnection();
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            try
            {

                strConnString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=Yes;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text'", _FileName);

                connection.ConnectionString = strConnString;

                if (connection.State != ConnectionState.Open)
                    connection.Open();



                selectCommand.CommandText = "SELECT * FROM [Data$]";
                selectCommand.Connection = connection;
                adapter.SelectCommand = selectCommand;
                adapter.Fill(_result);





            }
            catch(Exception ex)
            {
                string ssss = ex.Message;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
                if (selectCommand != null)
                    selectCommand.Dispose();
                if (adapter != null)
                    adapter.Dispose();
            }
            return _result;
        }
    }
}