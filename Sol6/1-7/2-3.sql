--Суммарное значение VOLUME и MAXVOLUME резервуаров по каждому заводу
select "Factories"."Name", sum("Tanks"."Maxvolume") as "MaxVol", sum("Tanks"."Volume") as "Vol"
from "Factories"
left join "Units"
on "Units"."FactoryId"="Factories"."Id"
left join "Tanks"
on "Tanks"."UnitId"="Units"."Id"
group by "Factories"."Name"