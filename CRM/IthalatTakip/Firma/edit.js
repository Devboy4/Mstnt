// JScript File
function Menu_ItemClick(s, e)
{
//    alert(s);
//    alert(e.processOnServer);
//    alert(e.item.name);
    if((e.item.name=='delete'))
    {
        if(confirm('Belgeyi silmek istediğinize emin misiniz?'))
            e.processOnServer=true;
        else
            e.processOnServer=false;
    }
//    alert(e.processOnServer);
    bSubmitted = true;
}


function SevkSekliIdChanged(source) {}     
function OdemeSekliIdChanged(source) {}     


// SEARCH BROWSER işlemleri
function ComboButtonClick(source)
{
//    alert('ComboButtonClick');
    var param=source;
    var bCallback=false;
    if(source=='SevkSekliId')
    {
        bCallback=true;
    }
    if(source=='OdemeSekliId')
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
    if(source=='SevkSekliId')
    {
        SevkSekliId.SelectedIndex=0;
        SevkSekliIdChanged(SevkSekliId);
    }
    if(source=='OdemeSekliId')
    {
        OdemeSekliId.SelectedIndex=0;
        OdemeSekliIdChanged(OdemeSekliId);
    }
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
        var value=window.showModalDialog('../../SearchBrowser2.aspx?var='+GetUniqueWinName(), window, 'dialogHeight:400px;dialogWidth:600px;status:no');
        if((value!=undefined) && (value!=null))
        {
            if(value.length >= 0)
            {
                if(e_result=='SevkSekliId')
                {
                    SevkSekliId.PerformCallback(value);
                }
                if(e_result=='OdemeSekliId')
                {
                    OdemeSekliId.PerformCallback(value);
                }
            }
        }
    }
    SearchBrowserEventArgs=null;
}
