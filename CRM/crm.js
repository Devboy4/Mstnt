// JScript File

//function changeAutoSize() {
//    var behavior = $find('Accordion1_AccordionExtender');
//    behavior.get_element().style.height = '400px';
//    size = AjaxControlToolkit.AutoSize.Fill;
//    behavior.set_AutoSize(size);

//    if (document.focus) {
//        document.focus();
//    }
//}
function openwin(link)
{
var excnt=window.open(""+link+"", "OurExternalWindow","menubar=no,scrollbar=yes,resizable=no,width=800,height=600");
excnt.focus();
}
function openmesajwin(link)
{
var excntmsj=window.open(""+link+"", "OurExternalMesajWindow","menubar=no,scrollbar=yes,resizable=no,width=550,height=400");
excntmsj.focus();
}
function opendifferentpage(link,pageID)
{
var excntmsj=window.open(""+link+"", pageID,"menubar=no,scrollbar=yes,resizable=no,width=800,height=600");
excntmsj.focus();
}
function MecraIDChanged(cmb)
{
   cmbBolumAdID.PerformCallback(cmbMecraID.GetValue());
}
function UlkeIDChanged(cmb)
{
   cmbSehirID.PerformCallback(cmbUlkeID.GetValue());
}
function UlkeIDChanged2(cmb)
{
   cmbUlkeID.PerformCallback(cmbUlkeID.GetValue());
}
function FirmaIDChanged(cmb)
{
   cmbBultenID.PerformCallback(cmbFirmaID.GetValue());
}
function FirmaIDChanged2(cmb)
{
   cmbOzelKod.PerformCallback(cmbFirmaID.GetValue());
}
function GridMailListFill(cmb)
{
   grd_KisiList.DataCallBack(cmbKisiListGruplari.GetValue());
}
function OnUlkeIDChanged(cmb) {
    grid.GetEditor("SehirID").SetValue(GridTalepSatir.GetEditor("UlkeID").GetValue());
}
function OnUlkeIDChanged(cmbUlkeID) {
    var UlkeID=cmbUlkeID.GetValue();
    if((UlkeID==null) || (UlkeID==''))
        UlkeID='00000000-0000-0000-0000-000000000000';
    grid.GetEditor("SehirID").PerformCallback(UlkeID);
}
function CallbackComplete(s, e) {
    // result = left;top;content text
    var indexOfFirstSeparator = e.result.indexOf(';');
    var indexOfSecondSeparator = e.result.indexOf(';', indexOfFirstSeparator + 1)
    
    var left = parseInt(e.result.substring(0, indexOfFirstSeparator));
    var top = parseInt(e.result.substring(indexOfFirstSeparator + 1, indexOfSecondSeparator));
    var contentText = e.result.substr(indexOfSecondSeparator + 1);
    
    popup.SetHeaderText("Popup notification via callback");
    popup.SetContentHTML(contentText);
    popup.ShowAtPos(left, top);
}

