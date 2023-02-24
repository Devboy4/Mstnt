
function GridPerformCallback(GridNo)
{
    var param='';
    var _FirmaId=FirmaId.GetValue();
    if((_FirmaId!=null) && (_FirmaId!='') && (_FirmaId!='00000000-0000-0000-0000-000000000000')) param=param+_FirmaId;
    param=param+'|';
    var _SezonId=SezonId.GetValue();
    if((_SezonId!=null) && (_SezonId!='') && (_SezonId!='00000000-0000-0000-0000-000000000000')) param=param+_SezonId;
    param=param+'|';
    var _MarkaId=MarkaId.GetValue();
    if((_MarkaId!=null) && (_MarkaId!='') && (_MarkaId!='00000000-0000-0000-0000-000000000000')) param=param+_MarkaId;
    param=param+'|';
    var _UrunId=UrunId.GetValue();
    if((_UrunId!=null) && (_UrunId!='') && (_UrunId!='00000000-0000-0000-0000-000000000000')) param=param+_UrunId;
    param=param;
    
    if(GridNo==1)Grid.PerformCallback(param);
    if(GridNo==2)Grid2.PerformCallback(param);
}

function FirmaIdChanged(source) {}
function SezonIdChanged(source) {}
function MarkaIdChanged(source) {}
function UrunIdChanged(source) {}

// SEARCH BROWSER işlemleri
function ComboButtonClick(source)
{
//    alert('ComboButtonClick');
    var param=source;
    var bCallback=false;
    if(source=='FirmaId')
    {
        bCallback=true;
    }
    if(source=='SezonId')
    {
        bCallback=true;
    }
    if(source=='MarkaId')
    {
        var _FirmaId=FirmaId.GetValue();
        if((_FirmaId!=null) && (_FirmaId!='') && (_FirmaId!='00000000-0000-0000-0000-000000000000'))
        {
            param=param+'|'+_FirmaId;
            bCallback=true;
        }
        else
        {
            alert('Öncelikle FİRMA seçmelisiniz!');
        }
    }
    if(source=='UrunId')
    {
        var _MarkaId=MarkaId.GetValue();
        if((_MarkaId!=null) && (_MarkaId!='') && (_MarkaId!='00000000-0000-0000-0000-000000000000'))
        {
            param=param+'|'+_MarkaId;
            bCallback=true;
        }
        else
        {
            alert('Öncelikle MARKA seçmelisiniz!');
        }
    }
    if(bCallback==true)
    {
        CallbackSearchBrowser.SendCallback(param);
    }
}

function ComboEndCallback(source)
{
    if(source=='FirmaId')
    {
        FirmaId.SelectedIndex=0;
        FirmaIdChanged(FirmaId);
    }
    if(source=='SezonId')
    {
        SezonId.SelectedIndex=0;
        SezonIdChanged(SezonId);
    }
    if(source=='MarkaId')
    {
        MarkaId.SelectedIndex=0;
        MarkaIdChanged(MarkaId);
    }
    if(source=='UrunId')
    {
        UrunId.SelectedIndex=0;
        UrunIdChanged(UrunId);
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
                if(e_result=='FirmaId')
                {
                    FirmaId.PerformCallback(value);
                }
                if(e_result=='SezonId')
                {
                    SezonId.PerformCallback(value);
                }
                if(e_result=='MarkaId')
                {
                    MarkaId.PerformCallback(value);
                }
                if(e_result=='UrunId')
                {
                    UrunId.PerformCallback(value);
                }
            }
        }
    }
    SearchBrowserEventArgs=null;
}
