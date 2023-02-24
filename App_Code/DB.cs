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

namespace Model.Crm
{
    // Bu sinifta Database ile ilgili faydali seyler vardir.

    public class DB
    {

        public static SqlConnection Connect(HttpContext ctx)
        {
            if (ctx.Items.Contains("dbconn"))
            {
                return ctx.Items["dbconn"] as SqlConnection;
            }

            SqlConnection conn = DB.Connect();

            ctx.Items["dbconn"] = conn;

            return conn;
        }

        public static SqlConnection Connect()
        {
            ConnectionStringSettings css = ConfigurationManager.ConnectionStrings["LocalSqlServer"];
            SqlConnection conn = new SqlConnection(css.ConnectionString);
            conn.Open();
            return conn;
        }

        public static void BeginTrans(HttpContext ctx)
        {
            SqlConnection conn = DB.Connect(ctx);
            SqlTransaction trans = conn.BeginTransaction();
            ctx.Items["trans"] = trans;
        }

        public static SqlTransaction BeginTrans(SqlConnection conn)
        {
            SqlTransaction trans = conn.BeginTransaction();
            return trans;
        }

        public static void Commit(HttpContext ctx)
        {
            SqlTransaction trans = ctx.Items["trans"] as SqlTransaction;
            trans.Commit();
        }

        public static void Commit(SqlTransaction trans)
        {
            trans.Commit();
        }

        public static void Rollback(HttpContext ctx)
        {
            SqlTransaction trans = ctx.Items["trans"] as SqlTransaction;
            trans.Rollback();
        }

        public static void Rollback(SqlTransaction trans)
        {
            trans.Rollback();
        }

        public static SqlCommand SQL(SqlConnection conn, string sql)
        {
            return new SqlCommand(sql, conn);
        }

        public static SqlCommand SQL(SqlConnection conn, SqlTransaction trans, string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Transaction = trans;
            return cmd;
        }

        public static SqlCommand SQL(HttpContext ctx, string sql)
        {
            SqlConnection conn = DB.Connect(ctx);
            SqlCommand cmd = DB.SQL(conn, sql);

            if (ctx.Items.Contains("trans"))
            {
                cmd.Transaction = ctx.Items["trans"] as SqlTransaction;
            }

            return cmd;
        }

        public static void AddParam(SqlCommand cmd, string name, int len, string value)
        {
            SqlParameter param = cmd.Parameters.AddWithValue(name, (value == null ? "" : value));
            param.DbType = DbType.String;
            param.Size = len;
        }

        public static void AddParam(SqlCommand cmd, string name, int len, object value)
        {
            SqlParameter param = cmd.Parameters.AddWithValue(name, (value == null ? "" : (string)value));
            param.DbType = DbType.String;
            param.Size = len;
        }

        public static void AddParam(SqlCommand cmd, string name, int value)
        {
            cmd.Parameters.AddWithValue(name, value).DbType = DbType.Int32;
        }

        public static void AddParam(SqlCommand cmd, string name, Guid value)
        {
            cmd.Parameters.AddWithValue(name, value).DbType = DbType.Guid;
        }

        public static void AddParam(SqlCommand cmd, string name, DateTime value)
        {
            cmd.Parameters.AddWithValue(name, value).DbType = DbType.DateTime;
        }

        public static void AddParam(SqlCommand cmd, string name, decimal value)
        {
            SqlParameter param = cmd.Parameters.AddWithValue(name, value);
            param.SqlDbType = SqlDbType.Decimal;
            param.Precision = Convert.ToByte(18);
            param.Scale = Convert.ToByte(2);
        }

        public static void AddParam(SqlCommand cmd, string name, object value)
        {
            value = DBNull.Value;
            SqlParameter param = cmd.Parameters.AddWithValue(name, value);
        }

        public static void AddParam(SqlCommand cmd, string name, SqlDbType dbtype)
        {
            SqlParameter param = cmd.Parameters.AddWithValue(name, DBNull.Value);
            param.SqlDbType = dbtype;
        }

        public static DataTable SqlQueryConnection()
        {
            DataTable dtConnection = new DataTable("Connection");
            dtConnection.Columns.Add("connection", typeof(string));
            return dtConnection;
        }

        public static DataTable SqlQuery()
        {
            DataTable dtQuery = new DataTable("Query");
            dtQuery.Columns.Add("query", typeof(string));
            return dtQuery;
        }

        public static DataTable SqlQueryParameters()
        {
            DataTable dtParameters = new DataTable("Parameters");
            dtParameters.Columns.Add("name", typeof(string));
            dtParameters.Columns.Add("type", typeof(string));
            dtParameters.Columns.Add("value", typeof(object));
            dtParameters.Columns.Add("size", typeof(int));
            dtParameters.Columns.Add("precision", typeof(int));
            dtParameters.Columns.Add("scale", typeof(int));
            //dtParameters.Columns.Add("value_string", typeof(string));
            //dtParameters.Columns.Add("value_int", typeof(int));
            //dtParameters.Columns.Add("value_datetime", typeof(DateTime));
            //dtParameters.Columns.Add("value_decimal", typeof(Decimal));
            return dtParameters;
        }

        public static DataTable SqlQueryFields()
        {
            DataTable dtFields = new DataTable("Fields");
            dtFields.Columns.Add("name", typeof(string));
            dtFields.Columns.Add("caption", typeof(string));
            dtFields.Columns.Add("width", typeof(int));
            dtFields.Columns.Add("visible", typeof(bool));
            return dtFields;
        }

    }
}