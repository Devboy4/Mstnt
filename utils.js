// JScript File

var bSubmitted = false;
window.onbeforeunload = HandleBeforeUnload;
    
function HandleBeforeUnload()
{
//    if (isDirty(theForm))
//    {
//      event.returnValue = "Değişiklikleri kaydetmek istermisiniz?";
//    }
}

function HandleSubmit(arg)
{
    if (event.srcElement.id == 'menu_DXI1_T') // Save in standart toolbar
    {
        bSubmitted = true;
    }
    else if (event.srcElement.id == 'menu_DXI2_T') // Save and New 
    {
        bSubmitted = true;
    }
    else if (event.srcElement.id == 'menu_DXI3_T') // Save and Close
    {
        bSubmitted = true;
    }
    else if (event.srcElement.id == 'menu_DXI4_T') // Sil
    {
        bSubmitted = true;
    }    

}


function dump(o)
{
    var s = "";
    for (var a in o)
    {
        //s += a + "=" + o[a] + " ";
        s += a + " ";
    }
    alert(s);
}


function isDirty(oForm)
{
    if (bSubmitted) 
    {
        return false;
    }

    var iNumElems = oForm.elements.length;
    var s = "";
    for (var i=0;i<iNumElems;i++)
    {
        var oElem = oForm.elements[i];
        
        //s += oElem.tagName + " (" + oElem.type ") ";
        if ("text" == oElem.type || "TEXTAREA" == oElem.tagName)
        {
            if (oElem.value != oElem.defaultValue)
            { 
                return true;
            }
        }
        else if ("checkbox" == oElem.type || "radio" == oElem.type)
        {
            if (oElem.checked != oElem.defaultChecked) 
            {
                return true;
            }
        }
        else if ("SELECT" == oElem.tagName)
        {
            var oOptions = oElem.options;
            var iNumOpts = oOptions.length;
            for (var j=0;j<iNumOpts;j++)
            {
                var oOpt = oOptions[j];
                if (oOpt.selected != oOpt.defaultSelected) 
                {
                    return true;
                }
            }
        }
    }
    //alert(s);
    return false;
}

function GetUniqueWinName()
{
    var now = new Date();
    var year = now.getFullYear();
    var month = now.getMonth();
    var day = now.getDay();
    var hour = now.getHours();
    var minute = now.getMinutes();
    var second = now.getSeconds();
    var millisecond = now.getMilliseconds();
    
    if (month < 10) {
        month = "0" + month;
    }
    
    if (day < 10) {
        day = "0" + day;
    }
    
    if (hour < 10) {
        hour = "0" + hour;
    }
    
    if (minute < 10) {
        minute = "0" + minute;
    }
    
    if (second < 10) {
        second = "0" + second;
    }
    
    return ("PW"+year+month+day+hour+minute+second+millisecond);
}

function OpenPopupWin(href)
{
    var name = GetUniqueWinName();
    var PopupWin = window.open(href, name, "resizable=0,top=100,left=100,width=800,height=650,scrollbars=1");
//    PopupWin.moveTo(100,100);
    return PopupWin;
}

function OpenPopupWinBySize(href,width,height)
{
    var name = GetUniqueWinName();
    var option = "resizable=0,top=100,left=100,width=" + width + ",height=" + height + ",scrollbars=1";
    var PopupWin = window.open(href,name,option);
//    PopupWin.moveTo(100,100);
    return PopupWin;
}
function OpenPopupWinBySizeResizable(href,width,height)
{
    var name = GetUniqueWinName();
    var option = "top=100,left=100,resizable=1,width=" + width + ",height=" + height + ",scrollbars=1";
    var PopupWin = window.open(href,name,option);
//    PopupWin.moveTo(100,100);
    return PopupWin;
}
function OpenPopupWinBySizeTopLeft(href,width,height,top,left)
{
    var name = GetUniqueWinName();
    var option = "resizable=0,top=" + top + ",left=" + left + ",width=" + width + ",height=" + height + ",scrollbars=1";
    var PopupWin = window.open(href,name,option);
//    PopupWin.moveTo(100,100);
    return PopupWin;
}
function OpenFullPage(href)
{
    var name = GetUniqueWinName();
    var PopupWin = window.open(href);
//    PopupWin.moveTo(100,100);
    return PopupWin;
}

function UpdataGrid(id)
{
  id.UpdateEdit();
  id.PerformCallback();
}

function ControlCalBack() {
    if (parent.frames['content'].CallbackPreview2 != null)
        parent.frames['content'].CallbackPreview2.PerformCallback();
}

function Menu_ItemClick(s, e) {
    if (e.item.name == 'delete') {
        if (confirm('Belgeyi silmek istediğinize emin misiniz?'))
            e.processOnServer = true;
        else
            e.processOnServer = false;
    }
    bSubmitted = true;
}