// JScript File

function OnFirmaIDChanged(cmb) {
    cmbProjeID.PerformCallback(cmbFirmaID.GetValue());
}
function ShowHideCustomizationWindow() {
    if (Grid.IsCustomizationWindowVisible())
        Grid.HideCustomizationWindow();
    else Grid.ShowCustomizationWindow();
}
function SetSonGecerlilikValue(day) {
    //alert(day);
    sonraki_tarih.setDate(now.getDate() + parseInt(1));
    return;
    if (day != null) {
        if (day == '') day = '1';
        if (day == '0') day = '1';
        var now = new Date();
        var sonraki_tarih = new Date();
        sonraki_tarih.setDate(now.getDate() + parseInt(day));
        d2.SetDate(sonraki_tarih);
        d2.UpdateDateEditInputs();

    }

}

function SetOperasyon(s) {
    //alert(Val);
    if (s != null) {

        var now = new Date()
        var hour = now.getHours();
        var minute = now.getMinutes();
        var second = now.getSeconds();
        var monthnumber = now.getMonth();
        var monthday = now.getDate();
        var year = now.getYear();


        var dmonth = s.GetDate().getMonth() + 1;
        var dyear = s.GetDate().getYear();
        var dday = s.GetDate().getDate();

        SpinOp.SetNumber(5);
    }

}


function OpenIssueDetail(value) {
    if (value != null && value != '') {
        CallbackPreview.PerformCallback(value);
    }
}


function OpenMedyaPage(value) {
    if (value != null && value != '') {
        var PopWin = OpenPopupWinBySizeResizable('../../Pop3MailAttachments/Pop3Attachment.aspx?id=' + value, 400, 300); PopWin.focus();
    }
}

function SetOperationDateByVirussinif(value) {

    var mySplitResult = value.split("|");
    SetSonGecerlilikValue(mySplitResult[1]);
    SpinOp.SetNumber(mySplitResult[1]);

    if (parseInt(mySplitResult[2]) == 0) {
        SpinOp.SetEnabled(false);
        d2.SetEnabled(false);
    }
    else {
        SpinOp.SetEnabled(true);
        d2.SetEnabled(true);
    }

    if (parseInt(mySplitResult[3]) == 0) {
        CheckSendSms.SetEnabled(false);
        CheckSendSms.SetChecked(false);
    }
    else {
        CheckSendSms.SetEnabled(true);
        CheckSendSms.SetChecked(true);
    }
}
      
      