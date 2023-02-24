using System;
using System.Data;
using System.Web;
using System.Collections;
using System.IO;
using System.Data.SqlClient;
using Model.Crm;
using System.Text;
using System.Collections.Generic;
using System.Configuration;

namespace Net.Mail
{
    /// <summary>
    /// Summary description for Pop3Utils
    /// </summary>
    public class Pop3Utils
    {
        public static bool CacheLog(string value)
        {
            try
            {
                string FilePath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "logs/ErrorLog.txt";
                using (StreamWriter writer = new StreamWriter(FilePath, true))
                {
                    writer.WriteLine(value + " Tarih :" + DateTime.Now.ToString());
                    writer.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
