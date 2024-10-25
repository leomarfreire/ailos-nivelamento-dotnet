SELECT ASSUNTO,
       ANO,
       COUNT(*) AS QUANTIDADE
  FROM atendimentos
 GROUP BY ASSUNTO, ANO
HAVING COUNT(*) > 3
 ORDER BY ANO ASC, QUANTIDADE DESC;