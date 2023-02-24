using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using DevExpress.Web.ASPxCallback;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Model.Crm;
using System.Data;

public partial class Phone_Phone : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack || IsCallback) return;
        if (Session["UserName"].ToString().ToUpper() == "MSTNT") this.pnlmultipleIpAccounts.Visible = true;
        this.IPUserName.Value = (String)Session["IPUserName"];
        this.IPPassword.Value = (String)Session["IPPassword"];
        this.IPAgentId.Value = (String)Session["IPDahili"];
        
    }

    protected void IPCallCallback_Callback(object source, CallbackEventArgs e)
    {
        try
        {
            string[] _callparams = e.Parameter.Split('|');
            string _Phonenumber = _callparams[0];
            string _agent = _callparams[1];
            WebRequest request = WebRequest.Create("http://192.168.1.4/yonetim/service/click2dial.php?src=SIP/" + _agent + "&dst=" + _Phonenumber);
            request.Credentials = new NetworkCredential("cwizwsapi", "cwizwsapipass");
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                //Console.WriteLine(response.StatusDescription);
                //Stream dataStream = response.GetResponseStream();
                //StreamReader reader = new StreamReader(dataStream);
                //string responseFromServer = reader.ReadToEnd();
                //reader.Close();
                //dataStream.Close();
                response.Close();
            }
        }
        catch { }

    }
    protected void IPControlCallBack_Callback(object source, CallbackEventArgs e)
    {
        string _result = "null";
        try
        {

            string[] _callparams = e.Parameter.Split('|');
            string _state = _callparams[2];
            if (_state.ToLower() != "complete") return;
            string _agent1 = _callparams[0];
            if (_agent1.ToLower() == "null" || String.IsNullOrEmpty(_agent1)) return;
            string _callId = _callparams[1];
            if (_callId.ToLower() == "null" || String.IsNullOrEmpty(_callId)) return;
            string _laststate = _callparams[3];
            string _agent2 = _callparams[4];
            if (_agent2.ToLower() == "undefined" || String.IsNullOrEmpty(_agent2)) return;
            string _direction = _callparams[5];
            //if (_direction.ToLower() == "gelen" || _direction.ToLower() == "giden")

            string _gundemdosyayolu = "http://192.168.1.4/yonetim/service/records.php?action=download&uid=" + _callId;
            using (SqlCommand cmd = DB.SQL(this.Context, "EXEC IPintegration @Agent1,@CallId,@State,@Agent2,@Direction,@GundemDosyaYolu"))
            {
                DB.AddParam(cmd, "@Agent1", 20, _agent1);
                DB.AddParam(cmd, "@CallId", 50, _callId);
                DB.AddParam(cmd, "@State", 20, _state.ToLower());
                DB.AddParam(cmd, "@Agent2", 20, _agent2);
                DB.AddParam(cmd, "@Direction", 20, _direction.ToLower());
                DB.AddParam(cmd, "@GundemDosyaYolu", 255, _gundemdosyayolu);

                _result = (string)cmd.ExecuteScalar();
            }
        }
        catch { _result = "null"; }
        
        e.Result = _result;
    }
}