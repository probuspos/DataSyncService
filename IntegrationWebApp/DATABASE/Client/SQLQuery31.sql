USE [master]
GO
/****** Object:  Database [Study]    Script Date: 05/02/2023 11:41:05 PM ******/
CREATE DATABASE [Study]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Study', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Study.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Study_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Study_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Study] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Study].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Study] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Study] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Study] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Study] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Study] SET ARITHABORT OFF 
GO
ALTER DATABASE [Study] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Study] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Study] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Study] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Study] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Study] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Study] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Study] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Study] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Study] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Study] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Study] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Study] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Study] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Study] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Study] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Study] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Study] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Study] SET  MULTI_USER 
GO
ALTER DATABASE [Study] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Study] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Study] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Study] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Study] SET DELAYED_DURABILITY = DISABLED 
GO
USE [Study]
GO
/****** Object:  UserDefinedTableType [dbo].[Type_Category]    Script Date: 05/02/2023 11:41:05 PM ******/
CREATE TYPE [dbo].[Type_Category] AS TABLE(
	[CtryId] [int] NOT NULL,
	[CtryName] [varchar](50) NULL,
	[CtryDesc] [varchar](500) NULL
)
GO
/****** Object:  Table [dbo].[tbl_Category]    Script Date: 05/02/2023 11:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_Category](
	[CtryId] [int] NOT NULL,
	[CtryName] [varchar](50) NULL,
	[CtryDesc] [varchar](500) NULL,
	[isSyncked] [int] NULL,
	[isDeleted] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_Employee]    Script Date: 05/02/2023 11:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_Employee](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeName] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[Ins_Category]    Script Date: 05/02/2023 11:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[Ins_Category]
@tbl_Category dbo.Type_Category READONLY,
@ReturnCode             INT OUT,
@ReturnDesc             VARCHAR(4000) OUT
AS
BEGIN TRY
	SET @ReturnCode = 0
	SET @ReturnDesc = 'Ins_Category:' 

	MERGE tbl_Category AS Target
    USING @tbl_Category	AS Source
    ON Source.CtryId = Target.CtryId
    
    -- For Inserts
    WHEN NOT MATCHED BY Target THEN
        INSERT (CtryId,CtryName, CtryDesc,isSyncked,isDeleted) 
        VALUES (Source.CtryId,Source.CtryName,Source.CtryDesc,1,0)
    
    -- For Updates
    WHEN MATCHED THEN UPDATE SET
        Target.CtryName	= Source.CtryName,
        Target.CtryDesc		= Source.CtryDesc,
		 Target.isSyncked		= 1;
 

END TRY
BEGIN CATCH
	IF @ReturnCode = 0 
        SELECT @ReturnCode = Error_number () 
              ,@ReturnDesc = 'Ins_Category:' + Error_message() 

		PRINT @ReturnDesc 

END CATCH


GO
USE [master]
GO
ALTER DATABASE [Study] SET  READ_WRITE 
GO
