/* Check whether the database exists and drop it if it does */
IF  EXISTS(SELECT 1 FROM master.dbo.sysdatabases WHERE name = 'LibraryDB')
BEGIN
	DROP DATABASE [LibraryDB];
	PRINT '' PRINT '*** Dropping database LibraryDB'
END
GO

print '' print '*** Creating database LibraryDB'
GO
Create DATABASE [LibraryDB]
GO

PRINT '' PRINT '*** Using database LibraryDB'
GO
Use [LibraryDB]
GO

print '' print '*** Creating Employee Table'
GO 
CREATE TABLE [dbo].[Employee] (
	[EmployeeID]		[int]	IDENTITY(100000, 1) NOT NULL,
	[FirstName] 		[nvarchar] (50)				NOT NULL,
	[LastName]			[nvarchar] (100)			NOT NULL,
	[PhoneNumber]		[nvarchar] (11)				NOT NULL,
	[Email]				[nvarchar] (250)			NOT NULL,
	[PasswordHash]		[nvarchar] (100)			NOT NULL DEFAULT
		'9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E',
	[Active]			[bit]						NOT NULL DEFAULT 1,
	[RoleID]			[nvarchar] (50)				NOT NULL,
	
	Constraint [pk_EmployeeID] Primary Key ([EmployeeID] ASC),
	Constraint [ak_Email] UNIQUE([Email] ASC)	
)
GO


print '' print '*** Creating Role Table'
GO
CREATE TABLE [dbo].[Role] (
	[RoleID]		[nvarchar](50)		NOT NULL,
	[Description]	[nvarchar](250)		,
	
	CONSTRAINT [pk_RoleID] PRIMARY KEY([RoleID] ASC)
)
GO



print '' print '*** Creating Customer Table'
	GO
	Create Table [dbo].[Customer] (
	[CustomerID]		[int]	IDENTITY(100000, 1) NOT NULL,
	[FirstName] 		[nvarchar] (50)				NOT NULL,
	[LastName]			[nvarchar] (100)			NOT NULL,
	[PhoneNumber]		[nvarchar] (11)				NOT NULL,
	[Email]				[nvarchar] (250)			NOT NULL,
	[PasswordHash]		[nvarchar] (100)			NOT NULL DEFAULT
		'9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E',
		[Active]		[bit]						NOT NULL DEFAULT 1,
	
	Constraint [pk_CustomerID] Primary Key ([CustomerID] ASC),
	Constraint [ak_CustomerEmail] UNIQUE([Email] ASC)	
)
GO

print '' print '*** Creating Book Table'
GO
Create Table [dbo].[Book] (
	[BookID]			[nvarchar] (15)				Not Null,
	[Title]				[nvarchar] (75)				Not Null,
	[AuthorID]			[char] (5)					Not Null,
	[Publisher]			[nvarchar] (50)				Not Null,
	[Genre]				[nvarchar] (20)				Not Null,
	[Description]		[nvarchar] (200),
	[Status]			[nvarchar] (10)				Not Null DEFAULT 'Active',

	Constraint [pk_BookID] Primary Key ([BookID] ASC)
)
GO

print '' print '*** Creating Author Table'
GO
Create Table [dbo].[Author] (
	[AuthorID]			[char] (5)					Not Null,
	[FirstName] 		[nvarchar] (50)				NOT NULL,
	[LastName]			[nvarchar] (100)			NOT NULL,
	[PrimaryGenre]		[nvarchar] (20)				Not Null,
	[Description]		[nvarchar] (200)
	

	Constraint [pk_AuthorID] Primary Key ([AuthorID] ASC)
)
GO

print '' print '*** Creating Borrow Table'
GO
CREATE TABLE [dbo].[Borrow] (
	[BorrowID]			[int]	IDENTITY(100000, 1)	NOT NULL,
	[CustomerID]		[int]						NOT NULL,
	[DateBorrowed]		[date]						NOT NULL,
	[BookID]			[nvarchar] (15) 			NOT NULL,
    [DateReturned]		[date]
	
	CONSTRAINT [pk_BorrowID] PRIMARY KEY ([BorrowID] ASC)	
)
GO

print '' print '*** Creating Reserve Table'
GO
CREATE TABLE [dbo].[Reserve] (
	[ReserveID]			[int]	IDENTITY(100000, 1)	NOT NULL,
	[CustomerID]		[int]						NOT NULL,
	[EmployeeID]		[int]						NOT NULL,
	[DateBorrowed]		[date],
	[BookID]			[nvarchar] (15)				NOT NULL
	
	CONSTRAINT [pk_ReserveID] PRIMARY KEY ([ReserveID] ASC)	
)
GO

print '' print '*** Adding foreign key RoleID for Employee'
ALTER TABLE [dbo].[Employee] WITH NOCHECK 
	ADD CONSTRAINT [fk_RoleID] FOREIGN KEY([RoleID])
	REFERENCES [dbo].[Role]([RoleID])
	ON UPDATE CASCADE
GO

print '' print '*** Adding foreign key AuthorID for Book'
ALTER TABLE [dbo].[Book] WITH NOCHECK 
	ADD CONSTRAINT [fk_AuthorID] FOREIGN KEY([AuthorID])
	REFERENCES [dbo].[Author]([AuthorID])
	ON UPDATE CASCADE
GO


print '' print '*** Adding foreign key BookID for Borrow'
ALTER TABLE [dbo].[Borrow] WITH NOCHECK 
	ADD CONSTRAINT [fk_BorrowBookID] FOREIGN KEY([BookID])
	REFERENCES [dbo].[Book]([BookID])
	ON UPDATE CASCADE
GO
print '' print '*** Adding foreign key CustomerID for Borrow'
ALTER TABLE [dbo].[Borrow] WITH NOCHECK 
	ADD CONSTRAINT [fk_BorrowCustomerID] FOREIGN KEY([CustomerID])
	REFERENCES [dbo].[Customer]([CustomerID])
	ON UPDATE CASCADE
GO

print '' print '*** Adding foreign key BookID for Reserve'
ALTER TABLE [dbo].[Reserve] WITH NOCHECK 
	ADD CONSTRAINT [fk_ReserveBookID] FOREIGN KEY([BookID])
	REFERENCES [dbo].[Book]([BookID])
	ON UPDATE CASCADE
GO
print '' print '*** Adding foreign key CustomerID for Reserve'
ALTER TABLE [dbo].[Reserve] WITH NOCHECK 
	ADD CONSTRAINT [fk_ReserveCustomerID] FOREIGN KEY([CustomerID])
	REFERENCES [dbo].[Customer]([CustomerID])
	ON UPDATE CASCADE
GO
print '' print '*** Adding foreign key EmployeeID for Reserve'
ALTER TABLE [dbo].[Reserve] WITH NOCHECK 
	ADD CONSTRAINT [fk_ReserveEmployeeID] FOREIGN KEY([EmployeeID])
	REFERENCES [dbo].[Employee]([EmployeeID])
	ON UPDATE CASCADE
GO

print ''	print '***Inserting Role Records'
GO
INSERT INTO [dbo].[Role]
		([RoleID], [Description])
	VALUES
		('Basic', 'Manages book data'),
		('Admin', 'Updates roles, and Manages employees')
GO

print '' print '*** Inserting Employee Test Records'
GO
INSERT INTO [dbo].[Employee]
		([FirstName], [LastName], [PhoneNumber], [Email], [RoleID])
	VALUES
		('Anne', 'Frank', '13195551234', 'Anne@library.net', 'Basic'),
		('Ashley', 'Perez', '15605551234', 'Ashley@library.net', 'Admin')
GO

print '' print '*** Inserting Customer Test Records'
GO
Insert INTO [dbo].[Customer]
		([FirstName], [LastName], [PhoneNumber], [Email])
    Values
        ('Eryn', 'Davis', '15937501205', 'Eryn@gmail.com'),
        ('Ron', 'Weasley', '13191234567', 'Ron@gmail.com'),
        ('Nic', 'Snow', '13197073535', 'Nic@gmail.com'),
        ('Alex', 'Ander', '13191054123', 'Alex@gmail.com'),
        ('Nora', 'Aron', '13197506416', 'Nora@gmail.com')

GO

print '' print '*** Inserting Author Test Records'
GO
Insert INTO [dbo].[Author]
        ([AuthorID], [FirstName], [LastName], [PrimaryGenre], [Description])
    Values
        ('JR001', 'Joanne', 'Rowling', 'Fantasy', 
         'A British novelist, philanthropist, film producer, television producer and screenwriter, best known for writing the Harry Potter fantasy series'),
        ('SC001', 'Suzanne', 'Collins', 'Dystopian', 
         'An American television writer and author, best known as the author of The New York Times best selling series The Underland Chronicles and The Hunger Games trilogy '),
        ('CN001', 'Calvin', 'Newport', 'Nonfiction',
         'An associate professor of computer science at Georgetown University and the author of five self-improvement books. ')
 GO
 

print '' print '*** Inserting Book Test Records'
GO
Insert INTO [dbo].[Book]
        ([BookID], [Title], [AuthorID], [Publisher], [Genre], [Description])
    Values
        ('978-1455586691', 'Deep Work', 'CN001', 'Grand Central Publishing', 'NonFiction', 
         'Discusses the difference between focused work and distracted work and the value of focused work in the world today'),
         ('978-0439023481', 'The Hunger Games', 'SC001', 'Scholastic Press', 'Dystopian',
         'Follows Katniss Everdeen, a 16 year old girl from a poor district as she fights for her life in the deadly televised battle royale: The Hunger Games'),
         ('0-7475-3269-9', 'Harry Potter and The Sorcerers Stone', 'JR001', 'Bloomsbury Publishing', 'Fantasy',
         'Follows Harry Potter, a young wizard who discovers his magical heritage on his eleventh birthday, when he receives a letter of acceptance to Hogwarts School of Witchcraft and Wizardry')
GO


print '' print '*** Creating sp_authenticate_employee'
GO
CREATE PROCEDURE [dbo].[sp_authenticate_employee]
	(
		@Email					[nvarchar](250)	,
		@PasswordHash			[nvarchar](100)
	)
AS
	BEGIN
		SELECT COUNT([EmployeeID])
		FROM [Employee]
		WHERE [Email] = @Email
			AND [PasswordHash] = @PasswordHash
			AND [Active] = 1
	END
GO

	
print '' print '*** Creating sp_retrieve_employee_by_email'
GO
Create Procedure sp_retrieve_employee_by_email (
		@Email			[nvarchar](250)
)
AS
	Begin
		SELECT [EmployeeID], [FirstName], [LastName], [PhoneNumber], [RoleID]
		FROM [Employee]
		WHERE	[Email] = @Email
	END
GO


print '' print '*** Creating sp_authenticate_user'
GO
CREATE PROCEDURE [dbo].[sp_authenticate_user]
	(
		@Email					[nvarchar](250)	,
		@PasswordHash			[nvarchar](100)
	)
AS
	BEGIN
		SELECT COUNT([CustomerID])
		FROM [Customer]
		WHERE [Email] = @Email
			AND [PasswordHash] = @PasswordHash
			AND [Active] = 1
	END
GO

	
print '' print '*** Creating sp_retrieve_user_by_email'
GO
Create Procedure sp_retrieve_user_by_email (
		@Email			[nvarchar](250)
)
AS
	Begin
		SELECT [CustomerID], [FirstName], [LastName], [PhoneNumber]
		FROM [Customer]
		WHERE	[Email] = @Email
	END
GO

print '' print '*** Creating sp_update_employee_password'
GO
CREATE PROCEDURE [dbo].[sp_update_employee_password]
	(
		@Email					[nvarchar](250),
		@OldPasswordHash		[nvarchar](100),
		@NewPasswordHash		[nvarchar](100)
	)
AS
	BEGIN
		UPDATE [Employee]
			SET[PasswordHash] = @NewPasswordHash
			WHERE[Email] = @Email 
			AND[PasswordHash] = @OldPasswordHash
		return @@ROWCOUNT
	END
GO
print '' print '*** Creating sp_update_user_password'
GO
CREATE PROCEDURE [dbo].[sp_update_user_password]
	(
		@Email					[nvarchar](250),
		@OldPasswordHash		[nvarchar](100),
		@NewPasswordHash		[nvarchar](100)
	)
AS
	BEGIN
		UPDATE [Customer]
			SET[PasswordHash] = @NewPasswordHash
			WHERE[Email] = @Email 
			AND[PasswordHash] = @OldPasswordHash
		return @@ROWCOUNT
	END
GO


print '' print '*** Creating sp_show_all_books'
GO
Create Procedure [dbo].[sp_show_all_books]
AS
    Begin
        Select [Book].[BookID], [Book].[Title], [Book].[AuthorID], [Book].[Publisher], [Book].[Genre], 
        [Book].[Description], [Book].[Status], [Author].[FirstName], [Author].[LastName]
        From Book Join Author on Author.AuthorID = Book.AuthorID
    End
GO

print '' print '*** Creating sp_insert_book'
GO
CREATE PROCEDURE [dbo].[sp_insert_book]
    (
        @BookID	    		[nvarchar] (15),
        @Title				[nvarchar] (75),
        @AuthorID			[char] (5),
        @Publisher			[nvarchar] (50),
        @Genre				[nvarchar] (20),
        @Description		[nvarchar] (200)
    )
AS 
    Begin
        Insert Into [Book]
            ([BookID], [Title], [AuthorID], [Publisher], [Genre], [Description])
            
            Values(@BookID, @Title, @AuthorID, @Publisher, @Genre, @Description)
        Return @@Rowcount
    End
GO

print '' print '*** Creating sp_update_book'
GO
CREATE PROCEDURE [dbo].[sp_update_book]
    (
        @BookID	    		[nvarchar] (15),
        @Title				[nvarchar] (75),
        @AuthorID			[char] (5),
        @Publisher			[nvarchar] (50),
        @Genre				[nvarchar] (20),
        @Description		[nvarchar] (200),
        
        @OldBookID	    	[nvarchar] (15),
        @OldTitle			[nvarchar] (75),
        @OldAuthorID		[char] (5),
        @OldPublisher		[nvarchar] (50),
        @OldGenre			[nvarchar] (20),
        @OldDescription		[nvarchar] (200)
    )
AS
    Begin
        UPDATE [Book]
        SET [BookID] = @BookID,
        [Title] = @Title,
        [AuthorID] = @AuthorID,
        [Publisher] = @Publisher,
        [Genre] = @Genre,
        [Description] = @Description
        
        WHERE [BookID] = @OldBookID
        AND [Title] = @OldTitle
        AND [AuthorID] = @OldAuthorID
        AND [Publisher] = @OldPublisher
        AND [Genre] = @OldGenre
        AND [Description] = @OldDescription
        Return @@Rowcount
    END
GO


print '' print '*** Creating sp_deactivate_book'
GO
CREATE PROCEDURE [dbo].[sp_deactivate_book]
    (
        @BookID	    		[nvarchar] (15),
        @Title				[nvarchar] (75)
    )
AS
    Begin
        Update [Book]
        Set [Status] = "Inactive"
        Where [BookID] = @BookID
        AND [Title] = @Title
        Return @@Rowcount
    End
GO



print '' print'*** Creating sp_insert_borrow_record'
GO
Create Procedure [dbo].[sp_insert_borrow_record]
    (
        @CustomerID 		[int],
        @DateBorrowed		[date],
	    @BookID    			[nvarchar] (15)
    )
As 
    Begin
        Insert INTO Borrow ([CustomerID], [DateBorrowed], [BookID])
        VALUES(@CustomerID, @DateBorrowed, @BookID)
        Return @@ROWCOUNT
    End
GO

print '' print'*** Creating sp_return_borrowed_book'
GO
Create Procedure [dbo].[sp_return_borrowed_book]
    (
        @DateReturned		[date],
	    @BookID    			[nvarchar] (15),
        @CustomerID 		[int]
    )
AS
    BEGIN
        Update Borrow
        SET [DateReturned] = @DateReturned
        WHERE [BookID] = @BookID
        AND [DateReturned] is NULL
    END
GO

print '' print'*** Creating sp_update_book_status'
GO
Create Procedure [dbo].[sp_update_book_status]
    (
        @BookID	    		[nvarchar] (15),
        @Status		       	[nvarchar] (10),
        @OldStatus			[nvarchar] (10)	
    )
AS
    Begin  
        Update Book
        Set [Status] = @Status
        Where [Status] = @OldStatus
        AND [BookID] = @BookID
        Return @@ROWCOUNT
    END
GO

print '' print'*** Creating sp_show_borrowed_books_by_customer'
GO
Create Procedure [dbo].[sp_select_borrowed_books_by_customer]
    (
        @CustomerID 		[int]
    )
AS 
    BEGIN
        Select [Book].[BookID], [Book].[Title], [Book].[AuthorID], [Book].[Publisher], [Book].[Genre], 
        [Book].[Description], [Book].[Status], [Author].[FirstName], [Author].[LastName]
        From Book Join Author on Author.AuthorID = Book.AuthorID inner join Borrow on Book.BookID = Borrow.BookID
        Where Borrow.CustomerID = @CustomerID
        AND Borrow.DateReturned is null
    END
GO


print '' print'*** Creating sp_insert_reserve_record'
GO
Create Procedure [dbo].[sp_insert_reserve_record]
    (
        @EmployeeID         [int],
        @CustomerID         [int],
        @BookID	    		[nvarchar] (15)
    )
AS
    BEGIN
        Insert INTO RESERVE([EmployeeID], [CustomerID], [BookID])
        VALUES(@EmployeeID, @CustomerID, @BookID)
        RETURN @@ROWCOUNT
    END
GO

print '' print'*** Creating sp_select_reserved_books_by_customer'
GO
CREATE Procedure [dbo].[sp_select_reserved_books_by_customer]
    (
        @CustomerID         [int]
    )
AS  
    BEGIN
        Select [Book].[BookID], [Book].[Title], [Book].[AuthorID], [Book].[Publisher], [Book].[Genre], 
        [Book].[Description], [Book].[Status], [Author].[FirstName], [Author].[LastName]
        From Book Join Author on Author.AuthorID = Book.AuthorID inner join Reserve on Book.BookID = 
        Reserve.BookID
        Where Reserve.CustomerID = @CustomerID
        AND Reserve.DateBorrowed is null
    END
GO

print '' print'*** Creating sp_select_all_authors'
GO
Create Procedure [dbo].[sp_select_all_authors]
AS
    Begin
        Select [AuthorID], [FirstName], [LastName], [PrimaryGenre], [Description]
        From Author
    End
GO

print '' print'*** Creating sp_select_all_author_ids'
GO
Create Procedure [dbo].[sp_select_all_author_ids]
As
    Begin
        Select [AuthorID]
        From Author
    End
GO


print '' print'*** Creating sp_select_all_users'
GO
Create Procedure [dbo].[sp_select_all_users]
As
    Begin
        Select [CustomerID], [FirstName], [LastName], [PhoneNumber], [Email]
        From Customer
        Where [Active] = 1
    End
GO

print '' print'*** Creating sp_select_all_employees'
GO
Create Procedure [dbo].[sp_select_all_employees]
(
    @Email      [nvarchar] (250)
)
As
    Begin
        Select [EmployeeID], [FirstName], [LastName], [PhoneNumber], [Email], [RoleID]
        From Employee
        Where [Active] = 1
        and [Email] != @Email
    End
GO


print '' print'*** Creating sp_insert_employee'
GO
Create Procedure [dbo].[sp_insert_employee]
(
	@FirstName 		    [nvarchar] (50),
	@LastName			[nvarchar] (100),
	@PhoneNumber		[nvarchar] (11),
	@Email				[nvarchar] (250),
	@RoleID			    [nvarchar] (50)    
)
As
    Begin
        Insert Into Employee ([FirstName], [LastName], [PhoneNumber], [Email], [RoleID])
        Values(@FirstName, @LastName, @PhoneNumber, @Email, @RoleID)
        Return @@Rowcount
    End
GO

print '' print'*** Creating sp_insert_customer'
GO
Create Procedure [dbo].[sp_insert_customer]
(
	@FirstName 		    [nvarchar] (50),
	@LastName			[nvarchar] (100),
	@PhoneNumber		[nvarchar] (11),
	@Email				[nvarchar] (250)  
)
As
    Begin
        Insert Into Customer ([FirstName], [LastName], [PhoneNumber], [Email])
        Values(@FirstName, @LastName, @PhoneNumber, @Email)
        Return @@Rowcount
    End
GO


print '' print'*** Creating sp_deactivate_customer'
GO
Create Procedure [dbo].[sp_deactivate_customer]
(
    @CustomerID     [int]
)
AS
    Begin
        Update Customer
        Set[Active] = 0
        Where [CustomerID] = @CustomerID
        Return @@Rowcount
    End
GO

print '' print'*** Creating sp_deactivate_employee'
GO
Create Procedure [dbo].[sp_deactivate_employee]
(
    @EmployeeID     [int]
)
AS
    Begin
        Update Employee
        Set[Active] = 0
        Where [EmployeeID] = @EmployeeID
        Return @@Rowcount
    End
GO

print '' print'*** Creating sp_select_all_roles'
GO
Create Procedure [dbo].[sp_select_all_roles]
As
    Begin
        Select [RoleID]
        From Role
    End
GO

print '' print '*** Creating sp_select_book_by_id'
GO
Create Procedure [dbo].[sp_select_book_by_id]
(
    @BookID	    		[nvarchar] (15)
)
AS
    Begin
        Select [BookID], [Title], [Author].[AuthorID], [FirstName] + " " + [LastName] AS [AuthorName], [Publisher], [Book].[Genre], 
        [Book].[Description], [Status]
        From Book join Author on Book.AuthorID = Author.AuthorID
        Where Book.BookID = @BookID
        
    End
GO

print '' print '*** Creating sp_deactivate_employee_by_email'
GO
Create Procedure [dbo].[sp_deactivate_employee_by_email]
(
    @Email     [nvarchar] (250)
)
AS
    Begin
        Update Employee
        Set[Active] = 0
        Where [Email] = @Email
        Return @@Rowcount
    End
GO

