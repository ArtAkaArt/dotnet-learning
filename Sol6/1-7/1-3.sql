--Как выбрать данные сразу из нескольких таблиц, если записи объединены одним ключом? 
--(LEFT JOIN, RIGHT JOIN, INNER JOIN)
select * from "Units"
inner join "Factories"
on "Units"."FactoryId"="Factories"."Id"