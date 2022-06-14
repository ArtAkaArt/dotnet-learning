--Оконные функции MS SQL или Postgres
--Из оконных функций чаще всего используется ROW_NUMBER()
--Нужно понимать логику выражения OVER(PARTITION BY ... ORDER BY ...)
--Напишите запрос, который будет удалять из таблицы Factory (см. скрипт создания таблицы 
--в предшествующих пунктах) дубликаты строк, сравнивая по двум столбцам
--Name и Description (удалению подлежат более поздние записи с бОльшим Id)
DELETE FROM "Factories"
WHERE "Id" IN
    (SELECT "Id"
     FROM 
        (SELECT "Id",
         ROW_NUMBER() OVER( PARTITION BY "Name", "Description"
         ORDER BY "Id") AS row_num
         FROM "Factories" ) t
     WHERE t.row_num > 1 );