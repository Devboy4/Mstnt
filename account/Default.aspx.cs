using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsPostBack || this.IsCallback)
        {
            return;
        }

        Filllist(lstSh);
        Filllist(lstAh);


        lblMessage.Text = "";
        ltrnot.Text = "<Br />Not: eğer birden fazla işlem yaptırmak istiyorsanız, aşağıdaki alanlara paralel olacak şekilde aralarına \",\" koymalısınız <Br />"
        + "örn Seçilen Hesap (90900,90901) Aktarılacak Hesap (90760,90761) Aktarılacak Mikar (10.000,5.000) <Br/><Br/>"
        + "Aşağıdaki mail adreslerinin yanına \";\" işareti koyarak yeni bir mail adresi ekleyebilirsiniz.<Br/>";

    }

    void Filllist(DropDownList lst)
    {
        lst.Items.Add("90900");
        lst.Items.Add("90901");
        lst.Items.Add("90902");
        lst.Items.Add("90903");
        lst.Items.Add("90904");
        lst.Items.Add("90905");
        lst.Items.Add("90906");
        lst.Items.Add("90907");
        lst.Items.Add("90908");
        lst.Items.Add("90909");
        lst.Items.Add("90910");
        lst.Items.Add("90911");
        lst.Items.Add("90912");
        lst.Items.Add("90913");
        lst.Items.Add("90914");
        lst.Items.Add("90915");
        lst.Items.Add("90751");
        lst.Items.Add("90761");
        lst.Items.Add("90764");
        lst.Items.Add("90500");
        lst.Items.Add("90501");
        lst.Items.Add("90502");
        lst.Items.Add("90503");
        lst.Items.Add("90504");
        lst.Items.Add("90505");
    }

    private bool Control()
    {
        if (String.IsNullOrEmpty(txtHesap1.Text))
        {
            lblMessage.Text = "Lütfen seçilen hesap alanını doldurunuz...";
            return false;
        }
        if (String.IsNullOrEmpty(txtHesap2.Text))
        {
            lblMessage.Text = "Lütfen aktarılacak hesap alanını doldurunuz...";
            return false;
        }
        if (String.IsNullOrEmpty(txtMiktar.Text))
        {
            lblMessage.Text = "Lütfen aktarılacak mikltar alanını doldurunuz...";
            return false;
        }

        if (TxtIslemKodu.Text != "Seray?.nakah")
        {
            lblMessage.Text = "İşlem kodu hatalı...";
            return false;
        }

        return true;
    }
    protected void btnSendPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            string[] _sh = txtHesap1.Text.Split(',');
            string[] _Ah = txtHesap2.Text.Split(',');
            string[] _Am = txtMiktar.Text.Split(',');

            string _Message = string.Empty;

            for (int i = 0; i < _sh.Length; i++)
            {
                //PLS WITHDRAW FROM MY ACC 90751 AMOUNT OF 15.000 USD AND PLS DEPOSIT THAT TO MY ACC 90900.
                try
                {
                    _Message += "PLS WITHDRAW FROM MY ACC " + _sh[i] + " AMOUNT OF " + _Am[i] + " " + MoneyType.SelectedValue + " AND PLS DEPOSIT THAT TO MY ACC " + _Ah[i] + ".<BR /><BR />";
                }
                catch { }


            }

            _Message += "TAHSİN EGEMEN<BR />";
            _Message += DateTime.Now.Date.ToString("dd.MM.yyyy");

            ltrSendMessage.Text = _Message;
        }
        catch { }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("./Default.aspx");
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        //info@advisedtrading.com
        ltrSendMessage.Text = "";
        lblMessage.Text = "";
        if (!Control()) return;

        string[] _sh = txtHesap1.Text.Split(',');
        string[] _Ah = txtHesap2.Text.Split(',');
        string[] _Am = txtMiktar.Text.Split(',');

        string _Message = string.Empty;

        for (int i = 0; i < _sh.Length; i++)
        {
            //PLS WITHDRAW FROM MY ACC 90751 AMOUNT OF 15.000 USD AND PLS DEPOSIT THAT TO MY ACC 90900.
            try
            {
                _Message += "PLS WITHDRAW FROM MY ACC " + _sh[i] + " AMOUNT OF " + _Am[i] + " " + MoneyType.SelectedValue + " AND PLS DEPOSIT THAT TO MY ACC " + _Ah[i] + ".<BR /><BR />";
            }
            catch { }


        }



        _Message += "TAHSİN EGEMEN<BR />";
        _Message += DateTime.Now.Date.ToString("dd.MM.yyyy");

        MailMessage mailMessage = new MailMessage();

        string[] adress = txtAdres.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string adr in adress)
        {
            mailMessage.To.Add(adr);
        }
        //mailMessage.To.Add(new MailAddress(txtAdres.Text));
        mailMessage.From = new MailAddress("t.egemen@hotmail.com");

        mailMessage.Subject = "MONEY TRANSFER‏";
        mailMessage.Body = _Message;
        mailMessage.IsBodyHtml = true;

        SmtpClient smtpClient = new SmtpClient("smtp.live.com", 587);
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new System.Net.NetworkCredential("t.egemen@hotmail.com", "727225pn");

        object userState = mailMessage;


        smtpClient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
        smtpClient.SendAsync(mailMessage, userState);

        ltrSendMessage.Text = _Message;

    }

    void ClearState()
    {
        txtHesap1.Text = "";
        txtHesap2.Text = "";
        txtMiktar.Text = "";

    }

    void smtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        MailMessage mailMessage = default(MailMessage);

        mailMessage = (MailMessage)e.UserState;

        if ((e.Cancelled))
        {
            lblMessage.Text = "şu adrese mail gönderimi iptal edildi. Adres=" + mailMessage.To[0].Address;
        }
        if ((e.Error != null))
        {
            lblMessage.Text = "hata  :" + e.Error.Message;
        }
        else
        {
            lblMessage.Text = "Mail aşağıdaki şekilde gönderildi...";
            ClearState();
        }
    }
}




