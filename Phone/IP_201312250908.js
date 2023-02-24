// JScript File

window.onbeforeunload = closecwizagent;

function closecwizagent() {
    element = document.getElementById("CwizAgentWebApplet");
    element.closestate();
}
function CwizInit(initresult) {

    if (initresult == "true") {

        CwizLogin();
    }
}

function CwizLogin() {

    var vals = new Object();
    vals.username = document.getElementById("IPUserName").value;
    vals.password = document.getElementById("IPPassword").value;
    vals.server = "192.168.1.4";
    vals.extension = document.getElementById("IPAgentId").value;
    vals.agent = document.getElementById("IPAgentId").value;

    element = document.getElementById("CwizAgentWebApplet");

    element.callCwizLogin(vals);
}

function SipLogin(key) {
    var str = key;
    var res = str.split("|");
    //alert(str);
    if (res[0].length > 0) {
        document.getElementById("IPUserName").value = res[0];
        document.getElementById("IPPassword").value = res[1];
        document.getElementById("IPAgentId").value = res[2];
        //alert(document.getElementById("IPPassword").value);
        CwizLogin();
    }
}

function onCallStatusChange(params) {
    //    var value='Gelen parametreler ID: ' + params.id + ' state:' + params.state + ' laststate:' + params.laststate + ' CallerID:' + params.callerid + ' Yon:' + params.direction);
    //    alert(value);
    var code = document.getElementById('PhoneLine');
    var agent = document.getElementById('IPAgentId');
    IPControlCallBack.PerformCallback(agent.value + '|' + params.id + '|' + params.state + '|' + params.laststate + '|' + params.callerid + '|' + params.direction);
}
function addCode(key) {
    var code = document.getElementById('PhoneLine');
    code.value = code.value + key;
}
function clicktocall() {
    var code = document.getElementById('PhoneLine');
    var agent = document.getElementById('IPAgentId');
    IPCallCallback.PerformCallback(code.value + '|' + agent.value);
}
function backspace() {
    var code = document.getElementById('PhoneLine');
    if (code.value.length > 0) code.value = code.value.substring(0, code.value.length - 1);
    else code.value = code.value;
}