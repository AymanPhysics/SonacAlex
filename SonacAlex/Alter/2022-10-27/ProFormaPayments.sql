 
CREATE TABLE [dbo].ProFormaPayments(
	[CustomerId] [int] NOT NULL,
	[Flag] [int] NOT NULL,
	[InvoiceNo] [int] NOT NULL,
	[DayDate] [datetime] NULL,
	MyIndex bigint,
	Transfers [float] NULL,
	Deductions [float] NULL, 
	Description [nvarchar](1000) NULL,
	[UserName] [int] NULL,
	[MyGetDate] [datetime] NULL, 
	[Line] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NULL,
	[InsertUser] [int] NULL,  
) ON [PRIMARY]
GO 
ALTER TABLE [dbo].ProFormaPayments ADD  CONSTRAINT [DF__ProFormaPayments___Inser__42093416]  DEFAULT (getdate()) FOR [InsertDate]
GO
 
