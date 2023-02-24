// JScript File

function BtnUpload_Click(s, e) 
{
    Uploader.UploadFile();
}

function Uploader_Start(s, e) 
{
//    e.callbackData='deneme';
}

function Uploader_Complete(s, e) 
{
    if(e.isValid)
    {
        alert(e.callbackData);
        Grid.PerformCallback("LoadData");
    }
    else
    {
        alert(e.errorText);
    }
}



