// JScript File

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
    var PopupWin = window.open(href, name, "resizable=0,top=10,left=50,width=800,height=650,scrollbars=1");
    if(PopupWin)
        return PopupWin;
    else
        alert('Popup pencere açma özelliği browser unuz tarafından engellenmiştir!\nBu engeli kaldırmanız gerekmektedir!');
}

function OpenPopupWinBySize(href,width,height)
{
    var name = GetUniqueWinName();
    var option = "resizable=0,top=10,left=50,width=" + width + ",height=" + height + ",scrollbars=1";
    var PopupWin = window.open(href,name,option);
    if(PopupWin)
        return PopupWin;
    else
        alert('Popup pencere açma özelliği browser unuz tarafından engellenmiştir!\nBu engeli kaldırmanız gerekmektedir!');
}

function ToUpper(s,e)
{
    var val = s.GetValue()
    //val = val.toUpperCase().replace(/([^0-9A-Z])/g,"");
    val = val.toUpperCase().replace(/([ğ,Ğ,ü,Ü,ş,Ş,ı,İ,ö,Ö,ç,Ç])/g,"");
    s.SetValue(val);
}

function IsNumeric(s, e) {
    var number = parseInt(s.GetValue());
    if (isNaN(number)) {
         e.value = null;
         e.isValid = false;
         e.errorText = "Geçerli bir sayısal giriş değil!";
    }
}
