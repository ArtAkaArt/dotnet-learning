--Выбрать все установки, у которых есть по крайней мере один резервуар
--с текущим значением Volume выше 1000
select "Units".*, max("Tanks"."Volume") as "CurrentMaxTankVolume"
from "Units"
right join "Tanks"
on "Tanks"."UnitId" = "Units"."Id"
group by "Units"."Id"
having max("Tanks"."Volume")>1000