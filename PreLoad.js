// JScript File

document.write('<div id="loading" style="background-color:#d7e2e8;width:100px;height:20px;border-width:1px;border-style:solid;border-color:black;">');
document.write('<table border="0" cellpadding="1" cellspacing="1"><tr><td>');
//document.write('<img src="http://localhost:62263/MntCrm/images/bigroller_000000.gif" alt="" />');
document.write('</td></tr><tr><td>');
document.write('<span style="font-family:Arial;font-size:9pt;font-weight:bold;">Yükleniyor...</span>');
document.write('</td></tr></table>');
document.write('</div>');

// Created by: Simon Willison | http://simon.incutio.com/
function addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    } else {
        window.onload = function () {
            if (oldonload) {
                oldonload();
            }
            func();
        }
    }
}

addLoadEvent(function () {
    document.getElementById("loading").style.display = "none";
});


