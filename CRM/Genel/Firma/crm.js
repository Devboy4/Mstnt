
function OnUlkeIDChanged(cmbUlkeID) {
    var UlkeID=cmbUlkeID.GetValue();
    if((UlkeID==null) || (UlkeID==''))
        UlkeID='00000000-0000-0000-0000-000000000000';
    grid.GetEditor("SehirID").PerformCallback(UlkeID);
}