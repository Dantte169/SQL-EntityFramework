SELECT c.CountryName, c.IsoCode
FROM Countries AS c
WHERE c.CountryName LIKE '%A%A%A%'
ORDER BY c.IsoCode