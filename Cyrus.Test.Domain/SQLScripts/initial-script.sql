USE [CyrusTest]
GO

/****** Object:  Table [dbo].[Categories]    Script Date: 2/21/2023 10:19:34 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Categories](
	[CategoryId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](150) NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  StoredProcedure [dbo].[sp_Category]    Script Date: 2/21/2023 11:11:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[sp_Category]
@Activity						NVARCHAR(50)		=		NULL,
-----------------------------------------------------------------
@SkipRow						INT					=		0,
@TakeRow						INT					=		10,
-----------------------------------------------------------------
@CategoryId						UNIQUEIDENTIFIER	=		NULL,
@Name							NVARCHAR(150)		=		NULL,
@Description					NVARCHAR(255)		=		NULL
-----------------------------------------------------------------
AS
IF @Activity = 'INSERT'
BEGIN
	INSERT INTO [dbo].[Categories] (CategoryId, Name, Description) VALUES (@CategoryId, @Name, @Description)
END
ELSE IF @Activity = 'UPDATE'
BEGIN
	UPDATE [dbo].[Categories]
	SET Name = ISNULL(@Name, Name),
		Description = ISNULL(@Description, Description)
	WHERE CategoryId = @CategoryId
END
ELSE IF @Activity = 'DELETE'
BEGIN
	DELETE FROM [dbo].[Categories] WHERE CategoryId = @CategoryId
END
ELSE IF @Activity = 'CHECK_DUPLICATE'
BEGIN
	SELECT TOP 1 CategoryId
	FROM [dbo].[Categories] (NOLOCK)
	WHERE Name = @Name
		AND (@CategoryId IS NULL OR CategoryId <> @CategoryId)
END
ELSE IF @Activity = 'GET_BY_ID'
BEGIN
	SELECT CategoryId, Name, Description
	FROM [dbo].[Categories] (NOLOCK)
	WHERE CategoryId = @CategoryId
END
ELSE IF @Activity = 'GET_ALL'
BEGIN
	SELECT CategoryId, Name, Description
	FROM [dbo].[Categories] (NOLOCK)
END
GO


