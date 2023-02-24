create procedure SP_GetVirusSinif
@UserName nvarchar(150)
as
begin
	--213,146
	declare @UserIntId int 
	Select @UserIntId = IndexId from SecurityUsers where UserName=@UserName
	if @UserIntId in(213,146,115) begin
		SELECT IndexId,Adi FROM VirusSinif ORDER BY Adi
	end else begin
		SELECT IndexId,Adi FROM VirusSinif where IndexID!=205 ORDER BY Adi
	end


end
go
create procedure SP_GetVirusSinifCreateIssue
@UserName nvarchar(150)
as
begin
	--213,146
	declare @UserIntId int 
	Select @UserIntId = IndexId from SecurityUsers where UserName=@UserName
	if @UserIntId in(213,146,115) begin
		SELECT isnull(cast(IndexId as nvarchar(6)),'-1')+'|'+isnull(cast(OperasyonDay as nvarchar(6)),'0')+'|'+isnull(cast(cast(IsDateChange as int) as nvarchar(6)),'0')+'|'+isnull(cast(cast(IsSendSms as int) as nvarchar(6)),'0')  VirusSinifID,Adi FROM VirusSinif ORDER BY Adi
	end else begin
		SELECT isnull(cast(IndexId as nvarchar(6)),'-1')+'|'+isnull(cast(OperasyonDay as nvarchar(6)),'0')+'|'+isnull(cast(cast(IsDateChange as int) as nvarchar(6)),'0')+'|'+isnull(cast(cast(IsSendSms as int) as nvarchar(6)),'0')  VirusSinifID,Adi FROM VirusSinif where IndexID!=205 ORDER BY Adi
	end


end
go
CREATE PROCEDURE [dbo].[ISAssignUserByUserNamev6]
@UserName nvarchar(100)
AS
BEGIN
	DECLARE @ProjeId UniqueIdentifier
	DECLARE @UserID UniqueIdentifier
	SELECT @ProjeId=ProjeId,@UserID=UserID From SecurityUsers where UserName=@UserName
	Create Table #User (UserID INT, UserGId Uniqueidentifier, UserName NvarChar(150), FirmaName NvarChar(150), 
		ProjeName NvarChar(150),IsShop bit, FirmaID Uniqueidentifier, ProjeID Uniqueidentifier
		,UserN NvarChar(100),Email NVARCHAR(100),CepTel NVARCHAR(20),OrderNumber tinyint,IsVisible bit)
   DECLARE @ISAdminRole SmallInt
   DECLARE @RoleId UniqueIdentifier
   SELECT  @RoleId=RoleId FROM aspnet_Roles WHERE RoleName='Administrator'
   SELECT  @UserID=UserID FROM SecurityUsers WHERE UserName=@UserName
	SELECT	@ISAdminRole= COUNT(*) FROM aspnet_UsersInRoles WHERE UserId=@UserID AND RoleId=@RoleId
	Insert Into #User
	SELECT DISTINCT Su.IndexId,Su.UserID as UserGId, Upper(ISNULL(Su.UserName,'')+' ['+ISNULL(Su.FirstName,'')+' '+ISNULL(Su.LastName,'')+']') AS UserName
	,t3.FirmaName,t2.Adi ProjeName,t2.IsShop,t3.FirmaID,t2.ProjeID,Su.UserName as UserN,Su.Email,Su.CepTel,1,0
	FROM SecurityUsers Su
	Left Outer JOIN Proje t2 ON Su.ProjeID=t2.ProjeID
	Left Outer JOIN Firma t3 ON Su.FirmaID=t3.FirmaID
	Where Su.FirmaID Is Not null And Su.ProjeID Is Not Null And Su.Active=1
	and Su.IndexId in
	(
		Select t3.IndexId From VirusSinifUsers t1 left join VirusSinif t2 on (t1.VirusSinifID=t2.VirusSinifId)
		left join SecurityUsers t3 on (t1.UserId=t3.UserId)
		Where t2.IndexId=205 and t3.Active=1
	)

	IF @ProjeId IS NOT NULL
	BEGIN
		update t1 set t1.IsVisible=1 
		FROM #User t1
		where ProjeId in(
		select ProjeID from UserAllowedProject where UserID=@UserID
		)
	END

	update t1 set ProjeName='MAÐAZA', OrderNumber=254
	FROM #User t1 WHERE IsShop=1
	
	update t1 set OrderNumber=0
	FROM #User t1 WHERE UserID=1
	
	IF(LOWER(@UserName)='admin' OR @ISAdminRole>0)
    BEGIN
		update t1 set t1.IsVisible=1
		FROM #User t1 
    END

	Select * From #User Order By IsVisible desc, OrderNumber asc, FirmaName asc, UserName asc
	Drop Table #User	
END
go
create procedure SP_GetYurutmePerm
@UserName nvarchar(150)
as
begin
	declare @UserIntId int 
	Select @UserIntId = IndexId from SecurityUsers where UserName=@UserName
	if @UserIntId in(213,146) begin
	SELECT 1
	end else begin
	select 0
	end

end