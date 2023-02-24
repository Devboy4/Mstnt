using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Model.Crm
{

    public class RoleUtils
    {
        public static bool Check(HttpContext ctx, string AllowedRoles, string DeniedRoles)
        {
            if (AllowedRoles == null && DeniedRoles == null)
            {
                return true;
            }
            
            if (AllowedRoles.Length == 0 && DeniedRoles.Length == 0)
            {
                return true;
            }

            if (ctx.User.IsInRole("Admin"))
            {
                return true;
            }
            
            char[] seps = { ',', ';' };

            if (DeniedRoles != null && DeniedRoles.Length > 0)
            {
                string[] denieds = DeniedRoles.Split(seps);

                foreach (string denied in denieds)
                {
                    if (ctx.User.IsInRole(denied.Trim()))
                    {
                        return false;
                    }
                }
            }

            if (AllowedRoles != null && AllowedRoles.Length > 0)
            {
                string[] alloweds = AllowedRoles.Split(seps);

                foreach (string allowed in alloweds)
                {
                    if (ctx.User.IsInRole(allowed.Trim()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}