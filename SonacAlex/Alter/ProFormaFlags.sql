ct ProFormaFlags
go
delete ProFormaFlags
insert ProFormaFlags(id,name) select 1,'ProForma'
insert ProFormaFlags(id,name) select 2,'ISO'
insert ProFormaFlags(id,name) select 3,'CustomsInvoice'
insert ProFormaFlags(id,name) select 4,'RealInvoice'
insert ProFormaFlags(id,name) select 11,'Guarantees'
insert ProFormaFlags(id,name) select 12,'Companies'
insert ProFormaFlags(id,name) select 13,'Entry3'
insert ProFormaFlags(id,name) select 14,'Files'
insert ProFormaFlags(id,name) select 15,'Bills'
insert ProFormaFlags(id,name) select 16,'Outcomes'
insert ProFormaFlags(id,name) select 17,'LoadingSheet'
insert ProFormaFlags(id,name) select 18,'Booking'
go
ct Banks2
go
alter table banks2 add Notes nvarchar(4000)