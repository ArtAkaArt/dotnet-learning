explain select * from "Tanks"
left join "Units"
on "Tanks"."UnitId" = "Units"."Id"
where "Tanks"."Maxvolume" >1000