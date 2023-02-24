// JScript File

function ComboButtonClick(source) {
    var param = source;
    var bCallback = false;
    if (source == 'GridSms_SmsId') {
        bCallback = true;
    }
    if (source == 'cmbMainIssueId') {
        bCallback = true;
    }
    if (bCallback == true) {
        CallbackSearchBrowser.SendCallback(param);
    }
}

function ComboEndCallback(source) {
    if (source == 'cmbMainIssueId') {
        cmbMainIssueId.SelectedIndex = 0;
        MainIssueIdChanged(cmbMainIssueId);
    }
}
function MainIssueIdChanged(source) { }


var SearchBrowserEventArgs = null;

function SearchBrowserCallbackComplete(e) {
    SearchBrowserEventArgs = e;
}

function SearchBrowserEndCallback() {
    if ((SearchBrowserEventArgs != null) && (SearchBrowserEventArgs != '')) {
        var e_parameter = SearchBrowserEventArgs.parameter;
        var e_result = SearchBrowserEventArgs.result;
        var value;
        value = window.showModalDialog('../../../CRM/SearchBrowser2.aspx?var=' + GetUniqueWinName(), window, 'dialogHeight:400px;dialogWidth:600px;status:no');
        if ((value != undefined) && (value != null)) {
            if (value.length >= 0) {
                if (e_result == 'GridSms_SmsId') {
                    bGridDirty = false;
                    CallbackGenel.SendCallback("GridSms_SmsId|" + value);
                }
                if (e_result == 'cmbMainIssueId') {
                    cmbMainIssueId.PerformCallback(value);
                }
            }
        }
    }
    SearchBrowserEventArgs = null;
}

function CallbackGenelComplete(value) {
    if ((value != null) && (value != '')) {
        values = value.split('|');
        if (values[1] == 'GridSms_SmsId') {
            GridSesEmail.GetEditor("SmsId").SetValue(values[2]);
            GridSesEmail.GetEditor("PhoneNumber").SetValue(values[3]);
            bGridDirty = false;
        }
    }
}

function LoadMainVirusScreen() {

    var value = cmbMainIssueId.GetValue();
    //alert(value);
    var PopWin = OpenPopupWinBySize2('./edit.aspx?id=' + value + '&NoteOwner=1', 850, 650); PopWin.focus();

}

function SetSantralLink() {

    var _gundemdosyayolu = document.getElementById("HiddenGundemDosyaYolu").value;
    //alert(value);
    if (_gundemdosyayolu != null) var PopWin = OpenPopupWinBySize2(_gundemdosyayolu, 850, 650); PopWin.focus();

}

function OpenPopupWinBySize2(href, width, height) {
    var name = GetUniqueWinName();
    var option = "resizable=0,top=100,left=100,width=" + width + ",height=" + height + ",scrollbars=1";
    var PopupWin = window.open(href, name, option);
    //    PopupWin.moveTo(100,100);
    return PopupWin;
}

function GetUniqueWinName2() {
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

    return ("PW" + year + month + day + hour + minute + second + millisecond);
}

