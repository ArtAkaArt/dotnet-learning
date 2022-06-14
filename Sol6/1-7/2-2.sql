--Суммарное значение Volume и MaxVolume, а также количество резервуаров по каждой установке, 
--с выводом имени установки, а также имени и описания завода, к которому относится установка
select "Units"."Id", "Factories"."Name", "Factories"."Description", 
sum("Tanks"."Volume") as "Volume", sum("Tanks"."Maxvolume") as "MaxVolume",
count ("Tanks") as "TankCount"
from "Units"
left join "Tanks"
on  "Units"."Id"= "Tanks"."UnitId"
left join "Factories"
on "Factories"."Id" = "Units"."FactoryId"
Group By "Units"."Id", "Factories"."Name", "Factories"."Description" 
order by "Id"