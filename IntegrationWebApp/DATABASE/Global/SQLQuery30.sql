USE [master]
GO
/****** Object:  Database [SPS]    Script Date: 05/02/2023 11:38:26 PM ******/
CREATE DATABASE [SPS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SPS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\SPS.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SPS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\SPS_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [SPS] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SPS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SPS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SPS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SPS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SPS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SPS] SET ARITHABORT OFF 
GO
ALTER DATABASE [SPS] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SPS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SPS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SPS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SPS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SPS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SPS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SPS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SPS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SPS] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SPS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SPS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SPS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SPS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SPS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SPS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SPS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SPS] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SPS] SET  MULTI_USER 
GO
ALTER DATABASE [SPS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SPS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SPS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SPS] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [SPS] SET DELAYED_DURABILITY = DISABLED 
GO
USE [SPS]
GO
/****** Object:  UserDefinedTableType [dbo].[Type_SynckCategory]    Script Date: 05/02/2023 11:38:26 PM ******/
CREATE TYPE [dbo].[Type_SynckCategory] AS TABLE(
	[ID] [int] NOT NULL
)
GO
/****** Object:  Table [dbo].[tbl_Category]    Script Date: 05/02/2023 11:38:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_Category](
	[CtryId] [int] IDENTITY(1,1) NOT NULL,
	[CtryName] [varchar](50) NULL,
	[CtryDesc] [varchar](500) NULL,
	[isSyncked] [int] NULL,
	[isDeleted] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[Del_Category]    Script Date: 05/02/2023 11:38:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROC [dbo].[Del_Category]
@CtryId int=0 ,
@ReturnCode             INT OUT,
@ReturnDesc             VARCHAR(4000) OUT
AS
BEGIN TRY
	SET @ReturnCode = 0
	SET @ReturnDesc = 'Del_Category:'
 
 if @CtryId=0
 begin 
 DELETE FROM [dbo].[tbl_Category]
 end
 else
 begin
 DELETE FROM [dbo].[tbl_Category]
 where CtryId=@CtryId
 end
 

END TRY
BEGIN CATCH
	IF @ReturnCode = 0 
        SELECT @ReturnCode = Error_number () 
              ,@ReturnDesc = 'Del_Category:' + Error_message() 
		PRINT @ReturnDesc 

END CATCH
GO
/****** Object:  StoredProcedure [dbo].[Get_Category]    Script Date: 05/02/2023 11:38:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec Get_Category 0,0,''
CREATE PROC [dbo].[Get_Category]
@CtryId int ,
@ReturnCode             INT OUT,
@ReturnDesc             VARCHAR(4000) OUT
AS
BEGIN TRY
	SET @ReturnCode = 0
	SET @ReturnDesc = 'Get_Category:'
 
 SELECT 
 CtryId,
CtryName,
CtryDesc
 FROM [dbo].[tbl_Category] (NOLOCK) 
 WHERE CtryId=case  when @CtryId=0 then CtryId else  @CtryId end
  and ISNULL(isSyncked,0)=0
  and  ISNULL(isDeleted,0)=0

END TRY
BEGIN CATCH
	IF @ReturnCode = 0 
        SELECT @ReturnCode = Error_number () 
              ,@ReturnDesc = 'Get_Category:' + Error_message() 

		PRINT @ReturnDesc 

END CATCH


GO
/****** Object:  StoredProcedure [dbo].[Get_SynckCategory]    Script Date: 05/02/2023 11:38:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec Get_SynckCategory 'category',0,''
CREATE PROC [dbo].[Get_SynckCategory]
@SynckCategory varchar(200) ,
@ReturnCode             INT OUT,
@ReturnDesc             VARCHAR(4000) OUT
AS
BEGIN TRY
	SET @ReturnCode = 0
	SET @ReturnDesc = 'Get_SynckCategory:'
 
 if @SynckCategory='category'
 begin 

 exec Get_Category 0,0,'' 
 end



 
END TRY
BEGIN CATCH
	IF @ReturnCode = 0 
        SELECT @ReturnCode = Error_number () 
              ,@ReturnDesc = 'Get_Category:' + Error_message() 

		PRINT @ReturnDesc 

END CATCH
GO
/****** Object:  StoredProcedure [dbo].[Ins_Category]    Script Date: 05/02/2023 11:38:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROC [dbo].[Ins_Category]
@CtryName varchar(50) ,
@CtryDesc varchar(500) ,
@ReturnCode             INT OUT,
@ReturnDesc             VARCHAR(4000) OUT
AS
BEGIN TRY
	SET @ReturnCode = 0
	SET @ReturnDesc = 'Ins_Category:'
 
 INSERT INTO tbl_Category 
 (
 CtryName,
 CtryDesc
 )
VALUES 
(
@CtryName,
@CtryDesc
)

END TRY
BEGIN CATCH
	IF @ReturnCode = 0 
        SELECT @ReturnCode = Error_number () 
              ,@ReturnDesc = 'Ins_Category:' + Error_message() 

		PRINT @ReturnDesc 

END CATCH
GO
/****** Object:  StoredProcedure [dbo].[Upd_Category]    Script Date: 05/02/2023 11:38:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROC [dbo].[Upd_Category]
@CtryId int ,
@CtryName varchar(50) ,
@CtryDesc varchar(500) ,
@ReturnCode             INT OUT,
@ReturnDesc             VARCHAR(4000) OUT
AS
BEGIN TRY
	SET @ReturnCode = 0
	SET @ReturnDesc = 'Upd_Category:'
 

 update tbl_Category
 SET  
 CtryName=@CtryName,
 CtryDesc=@CtryDesc
 WHERE CtryId=@CtryId

END TRY
BEGIN CATCH
	IF @ReturnCode = 0 
        SELECT @ReturnCode = Error_number () 
              ,@ReturnDesc = 'Upd_Category:' + Error_message() 
		PRINT @ReturnDesc 

END CATCH
GO
/****** Object:  StoredProcedure [dbo].[UPD_SynckCategory]    Script Date: 05/02/2023 11:38:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[UPD_SynckCategory]
@tbl_SynckCategory dbo.Type_SynckCategory READONLY,
@SynckCategory varchar(200) ,
@ReturnCode             INT OUT,
@ReturnDesc             VARCHAR(4000) OUT
AS
BEGIN TRY
	SET @ReturnCode = 0
	SET @ReturnDesc = 'UPD_SynckCategory:' 

	if @SynckCategory='category'
		 begin 
		 update c set 
		 c.isSyncked=1 
		 from tbl_Category c,@tbl_SynckCategory s
		 where c.CtryId=s.ID 
	end
 

END TRY
BEGIN CATCH
	IF @ReturnCode = 0 
        SELECT @ReturnCode = Error_number () 
              ,@ReturnDesc = 'UPD_SynckCategory:' + Error_message() 

		PRINT @ReturnDesc 

END CATCH
GO
USE [master]
GO
ALTER DATABASE [SPS] SET  READ_WRITE 
GO
