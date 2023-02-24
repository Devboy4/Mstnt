<!DOCTYPE HTML PUBLIC
 "-//W3C//DTD HTML 4.01 Frameset//EN"
 "http://www.w3.org/TR/html4/frameset.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8">
    <title>MSTNT - CRM</title>
</head>
<%--<frameset cols="20%,80%">
 
  <frame name="lefty" src="a.html">
 
  <frameset rows="85%,15%">
  <frame name="topy" src="b.html">
  <frame name="bottomly" src="c.html">
  </frameset>
 
</frameset>--%>
<frameset rows="60,*,70" frameborder="NO" framespacing="0" border="0"> 
<frame name="top" src="./frames/top.aspx">
<frameset cols="200,*"> 
  <frame name="lefty" src="./frames/menu.aspx">
  <frame name="content" src="./CRM/Genel/Issue/GundemGiris.aspx">
</frameset>
  <frameset rows"*">
   <frame name="bottom" src="./frames/bottom.aspx">
</frameset>
</frameset>
