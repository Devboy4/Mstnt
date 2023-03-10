USE [MSTNT3]
GO
/****** Object:  StoredProcedure [dbo].[GetUserSavsaklama]    Script Date: 14.11.2018 13:59:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetUserSavsaklama]
@UserId Int = null
,@StartDate Datetime = null
,@EndDate Datetime = null
AS
BEGIN
	DECLARE @Sql NVARCHAR(MAX)
	SET @Sql = 'Select t1.IssueId,t1.CreatedBy,t1.BildirimTarihi,t1.IndexId,t1.Baslik,t3.FirmaName,t4.Adi ProjeName
	,t5.Adi VirusSinifName,t1.TeslimTarihi,t6.Adi DurumName From Issue t1 with(nolock)
	LEFT JOIN Firma t3 ON t1.FirmaId=t3.IndexId
	LEFT JOIN Proje t4 ON t1.ProjeId=t4.IndexId
	LEFT JOIN VirusSinif t5 ON t1.VirusSinifId=t5.IndexId
	LEFT JOIN Durum t6 ON t1.DurumId=t6.IndexId Where
	exists(select 1 from UserAllowedIssueList with(nolock) where IssueId=t1.IndexId and UserId=@UserId and IssueAntivirus=0)
	and t1.Active=1 and t1.DurumId in(1,9) and datediff(day,t1.TeslimTarihi,GETDATE())>0 '
	if @StartDate is not null
	begin
		SET @Sql=@Sql+'And convert(datetime,convert(varchar(12),t1.BildirimTarihi))>=convert(datetime,convert(varchar(12),@StartDate)) '
	end
	if @EndDate is not null
	begin
		SET @Sql=@Sql+'And 	convert(datetime,convert(varchar(12),t1.BildirimTarihi))<=convert(datetime,convert(varchar(12),@EndDate)) '
	end
	--print @Sql
	EXEC sp_executesql @Sql,N'@UserId Int,@StartDate Datetime,@EndDate Datetime'
					,@UserId,@StartDate,@EndDate
END

-- GetUserSavsaklama 49,'2011-06-01',null

GO
ALTER PROCEDURE [dbo].[GetIssuesUsersFromSelectedDepartment]
@FirmaId int =null
,@ProjeId int =null
,@StartDate datetime = null
,@EndDate datetime = null
as
begin
	
	DECLARE @UserID INT
	DECLARE @sql nvarchar(max)

	SET @sql = 'Select NewId() ID, t1.IssueId, t2.Baslik
		, t2.CreatedBy, t3.UserName, t4.FirmaName, t5.Adi ProjeName, t2.BildirimTarihi, t2.TeslimTarihi
		,t6.Adi DurumName, t2.KeyWords 
		From UserallowedIssueList t1 with(nolock)
		left Join Issue t2 with(nolock) on (t1.IssueId=t2.IndexID)
		left join SecurityUsers t3 with(nolock) on (t1.UserId=t3.IndexId)
		left join Firma t4 on (t3.FirmaID=t4.FirmaID)
		left join Proje t5 on (t3.ProjeID=t5.ProjeID)
		left join Durum t6 on (t2.DurumId=t6.IndexID)
		where t2.Active=1 and t2.DurumId in(1,9) and t1.IssueAntivirus=0
		and datediff(day,t2.TeslimTarihi,GETDATE())>0 '
		
		if @StartDate is not null
		begin
			SET @Sql=@Sql+'And convert(datetime,convert(varchar(12),t2.BildirimTarihi))>=convert(datetime,convert(varchar(12),@StartDate)) '
		end
		if @EndDate is not null
		begin
			SET @Sql=@Sql+'And 	convert(datetime,convert(varchar(12),t2.BildirimTarihi))<=convert(datetime,convert(varchar(12),@EndDate)) '
		end
		if @FirmaId is not null
		begin
			SET @sql =@sql+'And t1.FirmaId=@FirmaId '
		end
		if @ProjeId is not null
		begin
			SET @sql =@sql+'And t1.ProjeId=@ProjeId '
		end


	SET @sql = @sql + 'ORDER BY t1.IssueId DESC'
	print @sql
	exec sp_executesql @sql, N'@StartDate DATETIME, @EndDate DATETIME, @FirmaId INT, @ProjeId INT'
	, @StartDate, @EndDate, @FirmaId, @ProjeId

end
GO
CREATE PROCEDURE [dbo].[Sp_GetIssueClassDelay]
@VirusSinifId int =null
,@StartDate datetime = null
,@EndDate datetime = null
as
begin
	
	DECLARE @UserID INT
	DECLARE @sql nvarchar(max)

	SET @sql = 'Select NewId() ID, t1.IssueId, t2.Baslik
		, t2.CreatedBy, t3.UserName, t4.FirmaName, t5.Adi ProjeName, t2.BildirimTarihi, t2.TeslimTarihi
		,t6.Adi DurumName, t2.KeyWords 
		From UserallowedIssueList t1 with(nolock)
		left Join Issue t2 with(nolock) on (t1.IssueId=t2.IndexID)
		left join SecurityUsers t3 with(nolock) on (t1.UserId=t3.IndexId)
		left join Firma t4 on (t3.FirmaID=t4.FirmaID)
		left join Proje t5 on (t3.ProjeID=t5.ProjeID)
		left join Durum t6 on (t2.DurumId=t6.IndexID)
		where t2.Active=1 and t2.DurumId in(1,9) and t1.IssueAntivirus=0 and t2.VirusSinifId=IsNull(@VirusSinifId,-1) '
		
		if @StartDate is not null
		begin
			SET @Sql=@Sql+'And convert(datetime,convert(varchar(12),t2.BildirimTarihi))>=convert(datetime,convert(varchar(12),@StartDate)) '
		end
		if @EndDate is not null
		begin
			SET @Sql=@Sql+'And 	convert(datetime,convert(varchar(12),t2.BildirimTarihi))<=convert(datetime,convert(varchar(12),@EndDate)) '
		end


	SET @sql = @sql + 'ORDER BY t1.IssueId DESC'
	print @sql
	exec sp_executesql @sql, N'@StartDate DATETIME, @EndDate DATETIME, @VirusSinifId INT'
	, @StartDate, @EndDate, @VirusSinifId

end
