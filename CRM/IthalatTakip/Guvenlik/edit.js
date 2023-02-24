// JScript File
function Menu_ItemClick(s, e)
{
//    alert(s);
//    alert(e.processOnServer);
//    alert(e.item.name);
    if(e.item.name=='delete')
    {
        if(confirm('Belgeyi silmek istediğinize emin misiniz?'))
            e.processOnServer=true;
        else
            e.processOnServer=false;
    }
//    alert(e.processOnServer);
    bSubmitted = true;
}

function ComboButtonClick(source)
{
  //alert('ComboButtonClick');
    var param=source;
    var bCallback=false;
    if(source=='GridRoles_Role')
    {
        bCallback=true;
    }
    if(source=='GridUsers_UserName')
    {
        bCallback=true;
    }
    if(bCallback==true)
    {
        //alert('param='+param);
        CallbackSearchBrowser.SendCallback(param);
    }
}

function ComboEndCallback(source)
{
    //alert('ComboEndCallback');
   //alert(source);
//    if(source=='cmbMusteriId')
//    {
//        cmbMusteriId.SelectedIndex=0;
//    }
}

var SearchBrowserEventArgs=null;

function SearchBrowserCallbackComplete(e)
{
//alert('SearchBrowserCallbackComplete');
//alert('e.parameter='+e.parameter);
//alert('e.result='+e.result);
    SearchBrowserEventArgs=e;
}

function SearchBrowserEndCallback()
{
//alert('SearchBrowserEndCallback');
    if((SearchBrowserEventArgs!=null) && (SearchBrowserEventArgs!=''))
    {
        var e_parameter=SearchBrowserEventArgs.parameter;
        var e_result=SearchBrowserEventArgs.result;
//alert('e.parameter='+e_parameter);
//alert('e.result='+e_result);
        var value;
        value=window.showModalDialog('../../SearchBrowser2.aspx?var='+GetUniqueWinName(), window, 'dialogHeight:400px;dialogWidth:600px;status:no');
        if((value!=undefined) && (value!=null))
        {
            if(value.length >= 0)
            {                
                if(e_result=='GridRoles_Role')
                {
                    bGridDirty = false;
                    CallbackGenel.SendCallback("GridRoles_Role|"+value);
                }
                if(e_result=='GridUsers_UserName')
                {
                    bGridDirty = false;
                    CallbackGenel.SendCallback("GridUsers_UserName|"+value);
                }
                if(e_result=='cmbGridAnketMusteri_MusteriId')
                {
                    bGridDirty = false;
                    CallbackGenel.SendCallback("cmbGridAnketMusteri_MusteriId|"+value);
                }
            }
        }
    }
    SearchBrowserEventArgs=null;
}

function CallbackGenelComplete(value)
{
//    alert('CallbackGenelComplete');
//    alert('value='+value);
    if ((value!=null) && (value!=''))
    {
        values = value.split('|');
        if(values[1]=='GridRoles_Role')
        {
            GridRoles.GetEditor("Role").SetValue(values[3]);
            bGridDirty = false;
        }
        if(values[1]=='GridUsers_UserName')
        {
            GridUsers.GetEditor("UserName").SetValue(values[3]);
            GridUsers.GetEditor("Adi").SetValue(values[4]);
            GridUsers.GetEditor("Soyadi").SetValue(values[5]);
            bGridDirty = false;
        }
    }
}