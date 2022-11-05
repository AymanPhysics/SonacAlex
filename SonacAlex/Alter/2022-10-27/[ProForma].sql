drop table [ProForma]
go
CREATE TABLE [dbo].[ProFormaMaster](
	[InvoiceNo] [int] NOT NULL,
	[Flag] [int] NOT NULL,
	[DayDate] [datetime] NULL,
	[CustomerId] [int] NOT NULL,
	[CostTypeId] [int] NULL,
	[LinkFile] [int] NULL,
	[MainAccNo] [nvarchar](100) NOT NULL,
	[SubAccNo] [int] NULL,
	[Value] [float] NULL,
	[Canceled] [int] NULL,
	[Notes] [nvarchar](1000) NULL,
	[UserName] [int] NULL,
	[MyGetDate] [datetime] NULL,
	[CostCenterId] [int] NULL,
	[CurrencyId] [int] NULL,
	[DocNo] [nvarchar](1000) NULL,
	[PurchaseAccNo] [varchar](100) NULL,
	[ImportMessageId] [bigint] NULL,
	[StoreId] [bigint] NULL,
	[StoreInvoiceNo] [bigint] NULL,
	[Line] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NULL,
	[InsertUser] [int] NULL,
	[AnalysisId] [bigint] NULL,
	[CostCenterSubId] [bigint] NULL,
	[IsPosted] [int] NOT NULL,
	[PaymentTypeId] [int] NULL,
	[FromPortId] [int] NULL,
	[ToPortId] [int] NULL,
	[ItemId] [int] NULL,
	[UnitQty] [float] NULL,
	[Qty] [float] NULL,
	[Price] [float] NULL,
	[SD_Notes] [nvarchar](1000) NULL,
	[UnitsWeightId] [int] NULL,
	[UnitsWeightQty] [float] NULL,
	[PreQty] [float] NULL,
	[MarkId] [int] NULL,
	[PriceTypeId] [int] NULL,
	[BankId] [int] NULL,
	[TypeOfPriceId] [int] NULL,
	[IsSent] [int] NULL,
	[ISONo] [nvarchar](100) NULL,
	[Advance] [float] NULL,
	[Remaining] [float] NULL,
	[Total] [float] NULL,
	[AdvanceDate] [datetime] NULL,
	[RemainingDate] [datetime] NULL,
	[ShippedPerId] [int] NULL,
	[Mark] [nvarchar](100) NULL,
	[ContainerTypeId] [int] NULL,
	[Sizes] [nvarchar](1000) NULL,
	[Freight] [real] NULL,
	[PaymentMothod] [nvarchar](4000) NULL,
	[SubCustomerId] [bigint] NOT NULL,
	primary key(
	[InvoiceNo] ,
	[Flag] ,
	[CustomerId] 
	)
)
go
CREATE TABLE [dbo].[ProFormaDetails](
	[InvoiceNo] [int] NOT NULL,
	[Flag] [int] NOT NULL,
	[DayDate] [datetime] NULL,
	[CustomerId] [int] NOT NULL,
	[CostTypeId] [int] NULL,
	[LinkFile] [int] NULL,
	[MainAccNo] [nvarchar](100) NOT NULL,
	[SubAccNo] [int] NULL,
	[Value] [float] NULL,
	[Canceled] [int] NULL,
	[Notes] [nvarchar](1000) NULL,
	[UserName] [int] NULL,
	[MyGetDate] [datetime] NULL,
	[CostCenterId] [int] NULL,
	[CurrencyId] [int] NULL,
	[DocNo] [nvarchar](1000) NULL,
	[PurchaseAccNo] [varchar](100) NULL,
	[ImportMessageId] [bigint] NULL,
	[StoreId] [bigint] NULL,
	[StoreInvoiceNo] [bigint] NULL,
	[Line] [bigint] IDENTITY(1,1) NOT NULL,
	[InsertDate] [datetime] NULL,
	[InsertUser] [int] NULL,
	[AnalysisId] [bigint] NULL,
	[CostCenterSubId] [bigint] NULL,
	[IsPosted] [int] NOT NULL,
	[PaymentTypeId] [int] NULL,
	[FromPortId] [int] NULL,
	[ToPortId] [int] NULL,
	[ItemId] [int] NULL,
	[UnitQty] [float] NULL,
	[Qty] [float] NULL,
	[Price] [float] NULL,
	[SD_Notes] [nvarchar](1000) NULL,
	[UnitsWeightId] [int] NULL,
	[UnitsWeightQty] [float] NULL,
	[PreQty] [float] NULL,
	[MarkId] [int] NULL,
	[PriceTypeId] [int] NULL,
	[BankId] [int] NULL,
	[TypeOfPriceId] [int] NULL,
	[IsSent] [int] NULL,
	[ISONo] [nvarchar](100) NULL,
	[Advance] [float] NULL,
	[Remaining] [float] NULL,
	[Total] [float] NULL,
	[AdvanceDate] [datetime] NULL,
	[RemainingDate] [datetime] NULL,
	[ShippedPerId] [int] NULL,
	[Mark] [nvarchar](100) NULL,
	[ContainerTypeId] [int] NULL,
	[Sizes] [nvarchar](1000) NULL,
	[Freight] [real] NULL,
	[PaymentMothod] [nvarchar](4000) NULL,
	[SubCustomerId] [bigint] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].ProFormaMaster ADD  CONSTRAINT [DF__ProFormaMaster___CostT__3E7D2C94]  DEFAULT ((0)) FOR [CostTypeId]
GO

ALTER TABLE [dbo].ProFormaMaster ADD  CONSTRAINT [DF__ProFormaMaster___LinkF__3F7150CD]  DEFAULT ((0)) FOR [LinkFile]
GO

ALTER TABLE [dbo].ProFormaMaster ADD  CONSTRAINT [DF__ProFormaMaster___MainA__0CD71D1F]  DEFAULT ('') FOR [MainAccNo]
GO

ALTER TABLE [dbo].ProFormaMaster ADD  CONSTRAINT [DF__ProFormaMaster___Inser__42093416]  DEFAULT (getdate()) FOR [InsertDate]
GO

ALTER TABLE [dbo].ProFormaMaster ADD  CONSTRAINT [DF__ProFormaMaster___Analy__77DC0039]  DEFAULT ((0)) FOR [AnalysisId]
GO

ALTER TABLE [dbo].ProFormaMaster ADD  CONSTRAINT [DF__ProFormaMaster___CostC__78D02472]  DEFAULT ((0)) FOR [CostCenterSubId]
GO

ALTER TABLE [dbo].ProFormaMaster ADD  DEFAULT ((0)) FOR [IsPosted]
GO

ALTER TABLE [dbo].ProFormaMaster ADD  DEFAULT ((0)) FOR [SubCustomerId]
GO



ALTER TABLE [dbo].ProFormaDetails ADD  CONSTRAINT [DF__ProFormaDetails___CostT__3E7D2C94]  DEFAULT ((0)) FOR [CostTypeId]
GO

ALTER TABLE [dbo].ProFormaDetails ADD  CONSTRAINT [DF__ProFormaDetails___LinkF__3F7150CD]  DEFAULT ((0)) FOR [LinkFile]
GO

ALTER TABLE [dbo].ProFormaDetails ADD  CONSTRAINT [DF__ProFormaDetails___MainA__0CD71D1F]  DEFAULT ('') FOR [MainAccNo]
GO

ALTER TABLE [dbo].ProFormaDetails ADD  CONSTRAINT [DF__ProFormaDetails___Inser__42093416]  DEFAULT (getdate()) FOR [InsertDate]
GO

ALTER TABLE [dbo].ProFormaDetails ADD  CONSTRAINT [DF__ProFormaDetails___Analy__77DC0039]  DEFAULT ((0)) FOR [AnalysisId]
GO

ALTER TABLE [dbo].ProFormaDetails ADD  CONSTRAINT [DF__ProFormaDetails___CostC__78D02472]  DEFAULT ((0)) FOR [CostCenterSubId]
GO

ALTER TABLE [dbo].ProFormaDetails ADD  DEFAULT ((0)) FOR [IsPosted]
GO

ALTER TABLE [dbo].ProFormaDetails ADD  DEFAULT ((0)) FOR [SubCustomerId]
GO

ct ItemTitles
go

ALTER TABLE [dbo].ProFormaDetails ADD  ItemsTitleId int