--Список всех установок (Unit) с именем завода, к которому относится установка
select "Units".*, "Factories"."Name" as "FactoryName" from "Units"
left join "Factories"
on "Units"."FactoryId" = "Factories"."Id"