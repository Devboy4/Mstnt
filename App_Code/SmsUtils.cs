using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.Text;
using System.Threading;

/// <summary>
/// Summary description for SmsUtils
/// </summary>

public class SmsUtils
{

    #region Definitions
    public string _prmPostAddress;
    public string _PassWord;
    public string _Command;
    public string _Numbers;
    public string _Mesgbody;
    public string _UserName;

    public string prmPostAddress
    {
        get { return _prmPostAddress; }
        set { _prmPostAddress = value; }
    }


    public string UserName
    {
        get { return _UserName; }
        set { _UserName = value; }
    }
    public string PassWord
    {
        get { return _PassWord; }
        set { _PassWord = value; }
    }
    public string Numbers
    {
        get { return _Numbers; }
        set { _Numbers = value; }
    }
    public string Mesgbody
    {
        get { return _Mesgbody; }
        set { _Mesgbody = value; }
    }



    #endregion
    #region Constructor
    public SmsUtils()
    {
        this.prmPostAddress = "http://processor.smsorigin.com/xml/process.aspx";
        this.UserName = "solmarkaparkgun";
        this.PassWord = "gms8KS4Xnd";
        this.Numbers = string.Empty;
        this.Mesgbody = string.Empty;
    }
    #endregion

    public string HTTPPoster(string prmSendData)
    {
        try
        {
            using (WebClient wUpload = new WebClient())
            {
                Byte[] bPostArray = Encoding.UTF8.GetBytes(prmSendData);
                Byte[] bResponse = wUpload.UploadData(prmPostAddress, "POST", bPostArray);
                Char[] sReturnChars = Encoding.ASCII.GetChars(bResponse);
                string sWebPage = new string(sReturnChars);
                return sWebPage;
            }
        }
        catch
        {
            return "-1";
        }
    }

    public string ConvertTurkish(string gelenveri)
    {
        try
        {
            gelenveri = gelenveri.Replace("ğ", "G");
            gelenveri = gelenveri.Replace("Ğ", "G");
            gelenveri = gelenveri.Replace("Ş", "S");
            gelenveri = gelenveri.Replace("ş", "S");
            gelenveri = gelenveri.Replace("Ç", "C");
            gelenveri = gelenveri.Replace("ç", "C");
            gelenveri = gelenveri.Replace("İ", "I");
            gelenveri = gelenveri.Replace("ı", "I");
            gelenveri = gelenveri.Replace("Ö", "O");
            gelenveri = gelenveri.Replace("ö", "O");
            gelenveri = gelenveri.Replace("Ü", "U");
            gelenveri = gelenveri.Replace("ü", "U");
            gelenveri = gelenveri.Replace("~", " ");
            gelenveri = gelenveri.Replace("€", " EUR");
            return gelenveri.ToUpper();
        }
        catch
        {
            return string.Empty;
        }
    }

    public void SendStarted(object args)
    {
        try
        {
            Array argArray = new object[3];
            argArray = (Array)args;
            string p1 = (string)argArray.GetValue(0);
            string p2 = (string)argArray.GetValue(1);
            int p3 = (int)argArray.GetValue(2);
            StringBuilder sb = new StringBuilder();
            sb.Append("<MainmsgBody><Command>0</Command><PlatformID>1</PlatformID><ChannelCode>325</ChannelCode><UserName>" + this.UserName + "</UserName><PassWord>" + this.PassWord + "</PassWord>");
            sb.Append("<Type>2</Type><Concat>1</Concat><Originator>MARKAPARK</Originator>");
            sb.Append("<Mesgbody>" + p2 + "</Mesgbody>");
            sb.Append("<Numbers>" + p1 + "</Numbers>");
            sb.Append("<SDate>"+ string.Format("{0:ddMMyyyyHHmm}", DateTime.Now) + "</SDate><EDate>"+ string.Format("{0:ddMMyyyyHHmm}", DateTime.Now.AddHours(2)) + "</EDate></MainmsgBody>");
            string httpposter = HTTPPoster(sb.ToString());
        }
        catch (Exception ex)
        {
        }
    }


    public void SmsThreadStarter(string NNumbers, string NMessageBody, int NMessagesize)
    {
        object args = new object[3] { NNumbers, NMessageBody, NMessagesize };
        Thread thread = new Thread(new ParameterizedThreadStart(SendStarted));
        thread.Name = Guid.NewGuid().ToString();
        thread.Start(args);
    }



}
