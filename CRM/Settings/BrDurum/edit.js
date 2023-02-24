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

function SezonIdChanged(source) {}
function AsamaIdChanged(source) {}
function MarkaIdChanged(source) {}
function FirmaIdChanged(source) 
{
    var _FirmaId=source.GetValue();
    if((_FirmaId!=null) && (_FirmaId!='') && (_FirmaId!='00000000-0000-0000-0000-000000000000'))
    {
        var param='FirmaId|'+_FirmaId;
        CallbackGenel.SendCallback(param);
    }
}
function TasiyiciFirmaIdChanged(source) {}
function ParaBirimiIdChanged(source) {}
function SevkSekliIdChanged(source) {}     
function OdemeSekliIdChanged(source) {}

// SEARCH BROWSER işlemleri
function ComboButtonClick(source)
{
//    alert('ComboButtonClick');
    var param=source;
    var bCallback=false;
    if(source=='SezonId')
    {
        bCallback=true;
    }
    if(source=='AsamaId')
    {
        bCallback=true;
    }
    if(source=='MarkaId')
    {
        var _FirmaId=FirmaId.GetValue();
        if((_FirmaId!=null) && (_FirmaId!='') && (_FirmaId!='00000000-0000-0000-0000-000000000000'))
        {
            bCallback=true;
        }
        else
        {
            alert('Öncelikle FİRMA seçmelisiniz!');
        }
    }
    if(source=='FirmaId')
    {
        bCallback=true;
    }
    if(source=='TasiyiciFirmaId')
    {
        bCallback=true;
    }
    if(source=='ParaBirimiId')
    {
        bCallback=true;
    }    
    if(source=='YaziRengi')
    {
        bCallback=true;
    }
    if(source=='OdemeSekliId')
    {
        bCallback=true;
    }
    if((source=='GridUrunId')||(source=='GridUrunId2'))
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
    if(source=='GridSezonId')
    {
        bCallback=true;
    }
    if(source=='GridAsamaId')
    {
        bCallback=true;
    }
    if(source=='GridSevkSekliId')
    {
        bCallback=true;
    }
    if(source=='GridOdemeSekliId')
    {
        bCallback=true;
    }
    if(source=='GridBankaId')
    {
        bCallback=true;
    }
    if(source=='GridTasiyiciFirmaId')
    {
        bCallback=true;
    }
    if(source=='GridDepoId')
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
    if(source=='SezonId')
    {
        SezonId.SelectedIndex=0;
        SezonIdChanged(SezonId);
    }
    if(source=='AsamaId')
    {
        AsamaId.SelectedIndex=0;
        AsamaIdChanged(AsamaId);
    }
    if(source=='MarkaId')
    {
        MarkaId.SelectedIndex=0;
        MarkaIdChanged(MarkaId);
    }
    if(source=='FirmaId')
    {
        FirmaId.SelectedIndex=0;
        FirmaIdChanged(FirmaId);
    }
    if(source=='TasiyiciFirmaId')
    {
        TasiyiciFirmaId.SelectedIndex=0;
        TasiyiciFirmaIdChanged(TasiyiciFirmaId);
    }
    if(source=='ParaBirimiId')
    {
        ParaBirimiId.SelectedIndex=0;
        ParaBirimiIdChanged(ParaBirimiId);
    }
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
        var value=window.showModalDialog('../../ColorPicker.aspx?var='+GetUniqueWinName(), window, 'dialogHeight:270px;dialogWidth:360px;status:no');
        if((value!=undefined) && (value!=null))
        {
            if(value.length >= 0)
            {
                if(e_result=='SezonId')
                {
                    SezonId.PerformCallback(value);
                }
                if(e_result=='AsamaId')
                {
                    AsamaId.PerformCallback(value);
                }
                if(e_result=='MarkaId')
                {
                    MarkaId.PerformCallback(value);
                }
                if(e_result=='FirmaId')
                {
                    FirmaId.PerformCallback(value);
                }
                if(e_result=='TasiyiciFirmaId')
                {
                    TasiyiciFirmaId.PerformCallback(value);
                }
                if(e_result=='ParaBirimiId')
                {
                    ParaBirimiId.PerformCallback(value);
                }
                if(e_result=='SevkSekliId')
                {
                    SevkSekliId.PerformCallback(value);
                }
                if(e_result=='OdemeSekliId')
                {
                    OdemeSekliId.PerformCallback(value);
                }
                if(e_result=='GridUrunId')
                {
                    bGridDirty = false;
                    CallbackGenel.SendCallback("GridUrunId|"+value);
                }
                if(e_result=='YaziRengi')
                {
                    bGridDirty = false;
                    CallbackGenel.SendCallback("YaziRengi|"+value);
                }
                if(e_result=='GridSezonId')
                {
                    bGridDirty = false;
                    CallbackGenel.SendCallback("GridSezonId|"+value);
                }
                if(e_result=='GridAsamaId')
                {
                    bGridDirty = false;
                    CallbackGenel.SendCallback("GridAsamaId|"+value);
                }
                if(e_result=='GridSevkSekliId')
                {
                    bGridDirty = false;
                    CallbackGenel.SendCallback("GridSevkSekliId|"+value);
                }
                if(e_result=='GridOdemeSekliId')
                {
                    bGridDirty = false;
                    CallbackGenel.SendCallback("GridOdemeSekliId|"+value);
                }
                if(e_result=='GridBankaId')
                {
                    bGridDirty = false;
                    CallbackGenel.SendCallback("GridBankaId|"+value);
                }
                if(e_result=='GridTasiyiciFirmaId')
                {
                    bGridDirty = false;
                    CallbackGenel.SendCallback("GridTasiyiciFirmaId|"+value);
                }
                if(e_result=='GridDepoId')
                {
                    bGridDirty = false;
                    CallbackGenel.SendCallback("GridDepoId|"+value);
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
        if(values[1]=='FirmaId')
        {
            if ((values[2]!=null) && (values[2]!=''))
                Iskonto.SetValue(values[2]);
            if ((values[3]!=null) && (values[3]!=''))
                SevkSekliId.PerformCallback(values[3]);
            if ((values[4]!=null) && (values[4]!=''))
                OdemeSekliId.PerformCallback(values[4]);
        }
        if(values[1]=='YaziRengi')
        {
            Grid.GetEditor("YaziRengi").SetValue(values[2]);
        }        
        if(values[1]=='GridUrunId2')
        {
            GridYukleme.GetEditor("UrunId").SetValue(values[2]);
            GridYukleme.GetEditor("Urun").SetValue(values[3]);
        }
        if(values[1]=='GridSezonId')
        {
            GridYukleme.GetEditor("SezonId").SetValue(values[2]);
            GridYukleme.GetEditor("Sezon").SetValue(values[3]);
        }
        if(values[1]=='GridAsamaId')
        {
            GridYukleme.GetEditor("AsamaId").SetValue(values[2]);
            GridYukleme.GetEditor("Asama").SetValue(values[3]);
        }
        if(values[1]=='GridSevkSekliId')
        {
            GridYukleme.GetEditor("SevkSekliId").SetValue(values[2]);
            GridYukleme.GetEditor("SevkSekli").SetValue(values[3]);
        }
        if(values[1]=='GridOdemeSekliId')
        {
            GridYukleme.GetEditor("OdemeSekliId").SetValue(values[2]);
            GridYukleme.GetEditor("OdemeSekli").SetValue(values[3]);
        }
        if(values[1]=='GridBankaId')
        {
            GridYukleme.GetEditor("BankaId").SetValue(values[2]);
            GridYukleme.GetEditor("Banka").SetValue(values[3]);
        }
        if(values[1]=='GridTasiyiciFirmaId')
        {
            GridYukleme.GetEditor("TasiyiciFirmaId").SetValue(values[2]);
            GridYukleme.GetEditor("TasiyiciFirma").SetValue(values[3]);
        }
        if(values[1]=='GridDepoId')
        {
            GridYukleme.GetEditor("DepoId").SetValue(values[2]);
            GridYukleme.GetEditor("Depo").SetValue(values[3]);
        }
    }
}
