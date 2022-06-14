--Как выбрать все значения таблицы, имеющие дубликаты по одному столбцу (HAVING)
SELECT "Maxvolume", count(*) as "CountNumber"
FROM "Tanks"
group by "Maxvolume"
having count(*) >1