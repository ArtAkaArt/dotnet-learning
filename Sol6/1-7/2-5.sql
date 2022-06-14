--Выбрать все резервуары, относящиеся к газофракционным установкам =)
select "Tanks".*, "Units"."Description" as "UnitDescription"
from "Tanks"
left join  "Units"
on "Tanks"."UnitId"="Units"."Id"
where "Units"."Description" like 'Газо%'