CREATE TABLE [Customer] (
    [Id] INT NOT NULL IDENTITY, 	
	[Name] varchar(400) NOT NULL,
    [State] varchar(100) NOT NULL,
    [Identification] bigint NOT NULL,	
    [DateCreation] datetime NOT NULL,
    [DateModified] datetime NULL)