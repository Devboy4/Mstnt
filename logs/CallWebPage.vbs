' VBScript File
On Error Resume Next
'Declare variables
Dim oRequest, URL
'The URL link.
URL = "http://localhost:2944/MSTNTV4/HitPage.aspx"
Set oRequest = CreateObject("Microsoft.XMLHTTP")
'Open the HTTP request and pass the URL to the objRequest object
oRequest.open "GET", URL , false
'Send the HTML Request
oRequest.Send
'Set the object to nothing
Set oRequest = Nothing


