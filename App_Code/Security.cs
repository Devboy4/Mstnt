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
using System.Text;

namespace Model.Crm
{
    public class Security
    {
        //
        public static void SetSessionPermissions(HttpContext ctx)
        {
            DataTable dtPermissions = new DataTable();

            dtPermissions.Columns.Add("ObjectID", typeof(Guid));
            dtPermissions.Columns.Add("ObjectName", typeof(string));
            dtPermissions.Columns.Add("Select", typeof(int));
            dtPermissions.Columns.Add("Insert", typeof(int));
            dtPermissions.Columns.Add("Update", typeof(int));
            dtPermissions.Columns.Add("Delete", typeof(int));

            DataColumn[] pricols = new DataColumn[1];
            pricols[0] = dtPermissions.Columns["ObjectID"];
            dtPermissions.PrimaryKey = pricols;
            SqlCommand cmd = null;
            cmd = DB.SQL(ctx, "SELECT * FROM SecurityObjects");

            cmd.Prepare();

            IDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DataRow row = dtPermissions.NewRow();
                row["ObjectID"] = rdr["ObjectID"];
                row["ObjectName"] = rdr["ObjectName"];
                row["Select"] = 0;
                row["Insert"] = 0;
                row["Update"] = 0;
                row["Delete"] = 0;
                dtPermissions.Rows.Add(row);
            }
            rdr.Close();


            for (int i = 0; i < dtPermissions.Rows.Count; i++)
            {
                DataRow row = dtPermissions.Rows[i];
                if (Security.CheckPermission(ctx, row["ObjectName"].ToString(), "Select"))
                    row["Select"] = 1;
                else
                    row["Select"] = 0;
                if (Security.CheckPermission(ctx, row["ObjectName"].ToString(), "Insert"))
                    row["Insert"] = 1;
                else
                    row["Insert"] = 0;
                if (Security.CheckPermission(ctx, row["ObjectName"].ToString(), "Update"))
                    row["Update"] = 1;
                else
                    row["Update"] = 0;
                if (Security.CheckPermission(ctx, row["ObjectName"].ToString(), "Delete"))
                    row["Delete"] = 1;
                else
                    row["Delete"] = 0;
            }

            dtPermissions.AcceptChanges();
            ctx.Session["Permissions"] = dtPermissions;
        }

        //
        public static string CheckPermissionType(HttpContext ctx)
        {
            using (SqlCommand cmd = DB.SQL(ctx, "SELECT ISNULL(CONVERT(char(50),YetkiTipID),'') AS YetkiTipID FROM SecurityUsers WHERE UserName=@UserName"))
            {
                DB.AddParam(cmd, "@UserName", 255, Membership.GetUser().UserName);
                cmd.Prepare();
                string gUserID = (string)cmd.ExecuteScalar();
                if (gUserID.ToString().Replace("{", null).Replace("}", null).Trim() == "F7E3CB25-88AD-4E8A-89C9-AB4082995859")
                    return "Admin";
                else if (gUserID.ToString().Replace("{", null).Replace("}", null).Trim() == "C84359BE-B9B2-44D4-8B38-9026D9B9604B")
                    return "User";
                else if (gUserID.ToString().Replace("{", null).Replace("}", null).Trim() == "ADDEF710-12A7-401E-A945-93313D0C4B69")
                    return "Musteri";
                else
                    return "Musteri";
            }

        }

        public static bool CheckPermission(HttpContext ctx, string ObjectName, string AccessType)
        {

            if (Membership.GetUser().UserName.ToLower() == "admin")
                return true;
            if (Roles.IsUserInRole(Membership.GetUser().UserName, "Administrator"))
                return true;
            if (ctx.Session["Permissions"] != null)
            {
                DataTable dtPermissions = (DataTable)ctx.Session["Permissions"];
                DataRow[] rows = dtPermissions.Select("ObjectName='" + ObjectName + "' and " + AccessType + "=1");
                if (rows.GetUpperBound(0) == -1)
                    return false;
                else
                    return true;
            }
            else
            {
                //SqlConnection conn = DB.Connect();
                Guid gUserID, gObjectID;
                SqlCommand cmd = DB.SQL(ctx, "SELECT UserID FROM SecurityUsers WHERE UserName=@UserName");
                DB.AddParam(cmd, "@UserName", 255, Membership.GetUser().UserName);
                cmd.Prepare();
                gUserID = (Guid)cmd.ExecuteScalar();

                cmd = DB.SQL(ctx, "SELECT ObjectID FROM SecurityObjects WHERE ObjectName=@ObjectName");

                DB.AddParam(cmd, "@ObjectName", 255, ObjectName);
                cmd.Prepare();
                gObjectID = (Guid)cmd.ExecuteScalar();


                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT COUNT(*) FROM SecurityUserPermissions ");
                sb.Append("WHERE UserID=@UserID and ObjectID=@ObjectID and [" + AccessType + "]=1");
                int count;
                cmd = DB.SQL(ctx, sb.ToString());
                DB.AddParam(cmd, "@UserID", gUserID);
                DB.AddParam(cmd, "@ObjectID", gObjectID);
                cmd.Prepare();
                count = (int)cmd.ExecuteScalar();

                if (count != 0)
                {
                    //conn.Close();
                    if (cmd != null)
                    {
                        cmd.Dispose();
                        cmd = null;
                    }
                    return true;
                }
                else
                {
                    sb = new StringBuilder();
                    sb.Append("SELECT COUNT(*) FROM SecurityRolePermissions t1, SecurityUserRoles t2 ");
                    sb.Append("WHERE t1.RoleID=t2.RoleID ");
                    sb.Append("and t2.UserID=@UserID and t1.ObjectID=@ObjectID and t1.[" + AccessType + "]=1");
                    cmd = DB.SQL(ctx, sb.ToString());
                    DB.AddParam(cmd, "@UserID", gUserID);
                    DB.AddParam(cmd, "@ObjectID", gObjectID);
                    cmd.Prepare();
                    count = (int)cmd.ExecuteScalar();
                    //conn.Close();
                    if (cmd != null)
                    {
                        cmd.Dispose();
                        cmd = null;
                    }
                    if (count != 0)
                        return true;
                    else
                        return false;

                }

            }



        }
    }
}
