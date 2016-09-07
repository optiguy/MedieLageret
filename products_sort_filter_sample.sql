SELECT p.id, p.name, p.[image],p.price FROM Products AS p
INNER JOIN Product_has_categories AS pc ON p.id = pc.product_id 
INNER JOIN Product_has_genres AS pg ON p.id = pg.product_id
WHERE pc.category_id = 1

AND (name LIKE '%Test%' OR [description] LIKE '%Test%')
AND price BETWEEN 50 AND 100
AND [year] BETWEEN 1990 AND 2016
AND pg.genre_id IN(2,3,4)

GROUP BY p.id, p.name, p.price, p.[image]
ORDER BY price ASC