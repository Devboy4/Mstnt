USE [MSTNT3]
GO
/****** Object:  StoredProcedure [dbo].[Sp_InsertWebSiparis]    Script Date: 14.08.2018 10:02:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[Sp_InsertWebSiparis]
@Id uniqueidentifier
,@WebSiparisNo nvarchar(50)
,@UrunCardId int
,@SiparisTarihi datetime
,@KargoFirma nvarchar(150)
,@KargoTakipNo nvarchar(50)
,@FaturaRefNo nvarchar(150)
,@SipKaynak nvarchar(150)
,@Durum int
,@MagazaNot nvarchar(500)
,@MusteriNot nvarchar(500)
,@TeslimatAliciAd nvarchar(100)
,@TeslimatTel nvarchar(50)
,@UyeMail nvarchar(100)
,@TeslimatAdres nvarchar(500)
,@TeslimatIlce nvarchar(50)
,@TeslimatSehir nvarchar(50)
,@Barkod nvarchar(20)
,@StokKodu nvarchar(20)
,@Beden nvarchar(500)
,@Adet int
,@UrunAd nvarchar(500)
,@Marka nvarchar(150)
,@AtananMagaza int
,@CreatedBy nvarchar(50)
,@CreationDate datetime
AS
BEGIN

	insert into WebSiparis
	select @Id,@SiparisTarihi,@WebSiparisNo,@UrunCardId,@FaturaRefNo,@SipKaynak,@KargoFirma
	,@KargoTakipNo,@Durum,@MagazaNot,@MusteriNot
	,@TeslimatAliciAd,@TeslimatTel,@UyeMail,@TeslimatAdres,@TeslimatIlce,@TeslimatSehir
	,@Barkod,@StokKodu,@Beden,@Adet,@UrunAd,@Marka,@AtananMagaza
	,null,null,@CreatedBy,null,null,@CreationDate,1

	if(IsNull(@KargoTakipNo,'')!='')
	begin
		insert into WebSiparisKargoNoEntegrate
		select @KargoTakipNo,@WebSiparisNo,@UrunCardId,0,@CreatedBy,@CreationDate
	end
END

GO
ALTER procedure [dbo].[Sp_InsertWebSiparisFromExcel]
@WebSiparisNo nvarchar(50)
,@UrunCardId int
,@SiparisTarihi datetime
,@KargoFirma nvarchar(150)
,@KargoTakipNo nvarchar(50)
,@FaturaRefNo nvarchar(150)
,@SipKaynak nvarchar(150)
,@Durum nvarchar(150)
,@MagazaNot nvarchar(500)
,@MusteriNot nvarchar(500)
,@TeslimatAliciAd nvarchar(100)
,@TeslimatTel nvarchar(50)
,@UyeMail nvarchar(100)
,@TeslimatAdres nvarchar(500)
,@TeslimatIlce nvarchar(50)
,@TeslimatSehir nvarchar(50)
,@Barkod nvarchar(20)
,@StokKodu nvarchar(20)
,@Beden nvarchar(500)
,@Adet int
,@UrunAd nvarchar(500)
,@Marka nvarchar(150)
,@AtananMagaza nvarchar(150)
,@CreatedBy nvarchar(50)
,@CreationDate datetime
AS
BEGIN
	if not exists(select 1 from WebSiparis where WebSipNo=@WebSiparisNo and UrunCardId=@UrunCardId and IsActive=1)
	begin
		Declare @AtananMagazaId int = null, @DurumId int =0
		Select @AtananMagazaId=IndexId from SecurityUsers where UserName=@AtananMagaza
		select @DurumId=IntId from WebSiparisDurum where Adi=@Durum
		insert into WebSiparis
		select NEWID(),@SiparisTarihi,@WebSiparisNo,@UrunCardId,@FaturaRefNo,@SipKaynak
		,@KargoFirma,@KargoTakipNo,@DurumId,@MagazaNot,@MusteriNot
		,@TeslimatAliciAd,@TeslimatTel,@UyeMail,@TeslimatAdres,@TeslimatIlce,@TeslimatSehir
		,@Barkod,@StokKodu,@Beden,@Adet,@UrunAd,@Marka,@AtananMagazaId
		,null,null,@CreatedBy,null,null,@CreationDate,1

		if(IsNull(@KargoTakipNo,'')!='' and Isnull(@UrunCardId,'')!='')
		begin
			insert into WebSiparisKargoNoEntegrate
			select @KargoTakipNo,@WebSiparisNo,@UrunCardId,0,@CreatedBy,@CreationDate
		end
	end
END

GO

ALTER procedure [dbo].[Sp_UpdateWebSiparis]
@Id int
,@WebSiparisNo nvarchar(50)
,@UrunCardId int
,@SiparisTarihi datetime
,@KargoFirma nvarchar(150)
,@KargoTakipNo nvarchar(50)
,@FaturaRefNo nvarchar(150)
,@SipKaynak nvarchar(150)
,@Durum int
,@MagazaNot nvarchar(500)
,@MusteriNot nvarchar(500)
,@TeslimatAliciAd nvarchar(100)
,@TeslimatTel nvarchar(50)
,@UyeMail nvarchar(100)
,@TeslimatAdres nvarchar(500)
,@TeslimatIlce nvarchar(50)
,@TeslimatSehir nvarchar(50)
,@Barkod nvarchar(20)
,@StokKodu nvarchar(20)
,@Beden nvarchar(500)
,@Adet int
,@UrunAd nvarchar(500)
,@Marka nvarchar(150)
,@AtananMagaza int
,@ModifiedBy nvarchar(50)
,@ModificationDate datetime
AS
BEGIN
	if(IsNull(@KargoTakipNo,'')!='' and @UrunCardId>0)
	begin
		declare @kno  nvarchar(150) 
		select @kno=KargoTakipNo from  WebSiparis where Id=@Id
		if(@kno!=@KargoTakipNo)
		begin
			insert into WebSiparisKargoNoEntegrate
			select @KargoTakipNo,@WebSiparisNo,@UrunCardId,0,@ModifiedBy,@ModificationDate
		end
	end
	update WebSiparis
	set  KargoFirma= @KargoFirma,KargoTakipNo= @KargoTakipNo,UrunCardId= @UrunCardId
	,Durum= @Durum,MagazaNot= @MagazaNot,MusteriNot= @MusteriNot
	,TeslimatAliciAd= @TeslimatAliciAd,TeslimatTel= @TeslimatTel
	,UyeMail= @UyeMail,TeslimatAdres= @TeslimatAdres,TeslimatIlce= @TeslimatIlce
	,TeslimatSehir= @TeslimatSehir,Barkod= @Barkod,StokKodu= @StokKodu,Beden= @Beden
	,Adet=@Adet,UrunAd= @UrunAd,Marka= @Marka,AtananMagaza= @AtananMagaza
	,ModifiedBy=@ModifiedBy,ModificationDate=@ModificationDate
	,FaturaRefNo=@FaturaRefNo,SiparisKaynak=@SipKaynak
	 Where Id=@Id
END
GO
ALTER procedure [dbo].[Sp_DeleteWebSiparis]
@Id int
AS
BEGIN
	
	update WebSiparis
	set IsActive= 0 Where Id=@Id
END
GO

ALTER procedure [dbo].[SP_GetWebSiparis]
@UserId int,@IsAdmin int,@StartDate datetime, @EndDate datetime
AS
BEGIN
	if @IsAdmin=1 
	begin
		Select * from WebSiparis where IsActive=1 and SiparisTarihi>=@StartDate 
		and SiparisTarihi<DateAdd(hour,24,@EndDate)   Order by Id desc
	end
	else
	begin
		Select * from WebSiparis where IsActive=1 and AtananMagaza=@UserId and SiparisTarihi>=@StartDate 
		and SiparisTarihi<DateAdd(hour,24,@EndDate)   Order by Id desc
	end
	
END
GO
ALTER PROCEDURE SP_InsertWebSipFromTicimax
@WebSiparisNo nvarchar(50)
,@UrunCardId int
,@SiparisTarihi datetime
,@KargoFirma nvarchar(150)
,@KargoTakipNo nvarchar(50)
,@FaturaRefNo nvarchar(150)
,@SipKaynak nvarchar(150)
,@Durum int
,@MusteriNot nvarchar(500)
,@TeslimatAliciAd nvarchar(100)
,@TeslimatTel nvarchar(50)
,@UyeMail nvarchar(100)
,@TeslimatAdres nvarchar(500)
,@TeslimatIlce nvarchar(50)
,@TeslimatSehir nvarchar(50)
,@Barkod nvarchar(20)
,@StokKodu nvarchar(20)
,@Beden nvarchar(500)
,@Adet int
,@UrunAd nvarchar(500)
,@Marka nvarchar(150)
,@CreatedBy nvarchar(50)
,@CreationDate datetime
AS
BEGIN
	if not exists(select 1 from WebSiparis where WebSipNo=@WebSiparisNo and UrunCardId=@UrunCardId and IsActive=1)
	begin
	    insert into WebSiparis
		select NEWID(),@SiparisTarihi,@WebSiparisNo,@UrunCardId,null,@SipKaynak
		,@KargoFirma,@KargoTakipNo,@Durum,null,@MusteriNot
		,@TeslimatAliciAd,@TeslimatTel,@UyeMail,@TeslimatAdres,@TeslimatIlce,@TeslimatSehir
		,@Barkod,@StokKodu,@Beden,@Adet,@UrunAd,@Marka,null
		,null,null,@CreatedBy,null,null,@CreationDate,1
	end
END