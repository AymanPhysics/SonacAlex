alter table ProFormaDetails add QtyTon real
alter table ProFormaDetails add GradeId int

go

ct Grades
go

delete Grades
insert Grades select 1,'Class 1',null,null
insert Grades select 2,'Class 2',null,null
