// JScript File

function ComboButtonClick(source)
{
    var param=source;
    var bCallback=false;
    if(source=='GridSesEmail_EmailId')
    {
        bCallback=true;
    }
    if(bCallback==true)
    {
        CallbackSearchBrowser.SendCallback(param);
    }
}

function ComboEndCallback(source)
{
}

var SearchBrowserEventArgs=null;

function SearchBrowserCallbackComplete(e)
{
    SearchBrowserEventArgs=e;
}

function SearchBrowserEndCallback()
{
    if((SearchBrowserEventArgs!=null) && (SearchBrowserEventArgs!=''))
    {
        var e_parameter=SearchBrowserEventArgs.parameter;
        var e_result=SearchBrowserEventArgs.result;
        var value;
        value=window.showModalDialog('../../../CRM/SearchBrowser2.aspx?var='+GetUniqueWinName(), window, 'dialogHeight:400px;dialogWidth:600px;status:no');
        if((value!=undefined) && (value!=null))
        {
            if(value.length >= 0)
            {                
                if(e_result=='GridSesEmail_EmailId')
                {
                    bGridDirty = false;
                    CallbackGenel.SendCallback("GridSesEmail_EmailId|"+value);
                }
            }
        }
    }
    SearchBrowserEventArgs=null;
}

function CallbackGenelComplete(value)
{
    if ((value!=null) && (value!=''))
    {
        values = value.split('|');
        if(values[1]=='GridSesEmail_EmailId')
        {
            GridSesEmail.GetEditor("EmailId").SetValue(values[2]);
            GridSesEmail.GetEditor("Email").SetValue(values[3]);
            bGridDirty = false;
        }
    }
}