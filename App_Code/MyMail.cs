
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Data.SqlClient;

/// <summary>
/// Summary description for MyMail
/// </summary>
public class MyMail
{
    private System.Net.Mail.SmtpDeliveryMethod _SmtpDeliveryMethod;
    public System.Net.Mail.SmtpDeliveryMethod SmtpDeliveryMethod
    {
        get { return _SmtpDeliveryMethod; }
        set { _SmtpDeliveryMethod = value; }
    }

    private string _SmtpFrom;
    public string SmtpFrom
    {
        get { return _SmtpFrom; }
        set { _SmtpFrom = value; }
    }

    private bool _SmtpDefaultCredentials;
    public bool SmtpDefaultCredentials
    {
        get { return _SmtpDefaultCredentials; }
        set { _SmtpDefaultCredentials = value; }
    }

    private string _SmtpHost;
    public string SmtpHost
    {
        get { return _SmtpHost; }
        set { _SmtpHost = value; }
    }

    private int _SmtpPort;
    public int SmtpPort
    {
        get { return _SmtpPort; }
        set { _SmtpPort = value; }
    }

    private string _SmtpUserName;
    public string SmtpUserName
    {
        get { return _SmtpUserName; }
        set { _SmtpUserName = value; }
    }

    private string _SmtpPassword;
    public string SmtpPassword
    {
        get { return _SmtpPassword; }
        set { _SmtpPassword = value; }
    }

    private System.Net.Mail.MailMessage _NetMailMessage;
    public System.Net.Mail.MailMessage NetMailMessage
    {
        get { return _NetMailMessage; }
        set { _NetMailMessage = value; }
    }

    private string _SqlConnectionString;
    public string SqlConnectionString
    {
        get { return _SqlConnectionString; }
        set { _SqlConnectionString = value; }
    }

    private string _AppLogsUserName;
    public string AppLogsUserName
    {
        get { return _AppLogsUserName; }
        set { _AppLogsUserName = value; }
    }

    private string _AppLogsUrl;
    public string AppLogsUrl
    {
        get { return _AppLogsUrl; }
        set { _AppLogsUrl = value; }
    }

    private string _AppLogsMethod;
    public string AppLogsMethod
    {
        get { return _AppLogsMethod; }
        set { _AppLogsMethod = value; }
    }

    private bool _UseWebMail;
    public bool UseWebMail
    {
        get { return _UseWebMail; }
        set { _UseWebMail = value; }
    }

    private System.Web.Mail.MailMessage _WebMailMessage;
    public System.Web.Mail.MailMessage WebMailMessage
    {
        get { return _WebMailMessage; }
        set { _WebMailMessage = value; }
    }

    public MyMail(){}

    public void Send()
    {
        Thread mail = new Thread(SendThread);
        mail.IsBackground = true;
        mail.Start();
    }

    private void SendThread()
    {
        SqlConnection con = null;
        try
        {
            if (_UseWebMail)
            {
                System.Web.Mail.SmtpMail.SmtpServer = _SmtpHost;
                _WebMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", _SmtpHost);
                _WebMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2");
                _WebMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                _WebMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", _SmtpUserName);
                _WebMailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", _SmtpPassword);
                System.Web.Mail.SmtpMail.Send(_WebMailMessage);
            }
            else
            {
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.DeliveryMethod = _SmtpDeliveryMethod;
                smtp.UseDefaultCredentials = _SmtpDefaultCredentials;
                smtp.Host = _SmtpHost;
                smtp.Port = _SmtpPort;
                smtp.Credentials = new NetworkCredential(_SmtpUserName, _SmtpPassword);
                smtp.Send(_NetMailMessage);
            }
            //try
            //{
            //    con = new SqlConnection(_SqlConnectionString);
            //    con.Open();
            //    SqlCommand cmd = new SqlCommand("INSERT INTO AppLogs([username],[date],[url],[method],[message]) VALUES(@username,@date,@url,@method,@message)", con);
            //    cmd.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 60).Value = _AppLogsUserName;
            //    cmd.Parameters.Add("@date", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
            //    cmd.Parameters.Add("@url", System.Data.SqlDbType.NVarChar, 255).Value = _AppLogsUrl;
            //    cmd.Parameters.Add("@method", System.Data.SqlDbType.NVarChar, 60).Value = _AppLogsMethod;
            //    cmd.Parameters.Add("@message", System.Data.SqlDbType.NVarChar, 500).Value = "SMTPCLIENT : SUCCESS";
            //    cmd.CommandTimeout = 1000;
            //    cmd.ExecuteNonQuery();
            //}
            //catch (Exception ex2) { }
            //finally { if (con != null) con.Close(); }
        }
        catch (Exception ex) 
        {
            string message = "SMTPCLIENT : FAIL : " + ex.Message;
            //if (message.Length > 500) message = message.Substring(0, 500);
            //try
            //{
            //    con = new SqlConnection(_SqlConnectionString);
            //    con.Open();
            //    SqlCommand cmd = new SqlCommand("INSERT INTO AppLogs([username],[date],[url],[method],[message]) VALUES(@username,@date,@url,@method,@message)", con);
            //    cmd.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 60).Value = _AppLogsUserName;
            //    cmd.Parameters.Add("@date", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
            //    cmd.Parameters.Add("@url", System.Data.SqlDbType.NVarChar, 255).Value = _AppLogsUrl;
            //    cmd.Parameters.Add("@method", System.Data.SqlDbType.NVarChar, 60).Value = _AppLogsMethod;
            //    cmd.Parameters.Add("@message", System.Data.SqlDbType.NVarChar, 500).Value = message;
            //    cmd.CommandTimeout = 1000;
            //    cmd.ExecuteNonQuery();
            //}
            //catch (Exception ex2) { }
            //finally { if (con != null) con.Close(); }
        }
    }
}
