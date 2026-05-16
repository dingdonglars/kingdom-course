# Quiz — Módulo 2.5

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que é uma chave estrangeira?

- **a.** Uma chave que abre bancos de dados escritos em outra linguagem
- **b.** Uma coluna cujo valor combina com um `id` de outra tabela — o elo entre duas tabelas
- **c.** Uma chave de uma linguagem de programação diferente da do próprio banco de dados
- **d.** Um recurso só do SQLite, ausente em outros bancos de dados

## 2. Qual é a diferença entre `INNER JOIN` e `LEFT JOIN`?

- **a.** Retornam resultados idênticos em bancos de dados modernos
- **b.** `INNER JOIN` retorna só as linhas com correspondência; `LEFT JOIN` retorna toda linha da tabela da esquerda, com `NULL` na direita onde não há correspondência
- **c.** `LEFT JOIN` roda mais rápido porque pula a correspondência
- **d.** `INNER JOIN` não aceita condições na cláusula `ON`

## 3. Por que a lição mantém os prédios na própria tabela deles em vez de enfiar eles numa coluna em `kingdoms`?

- **a.** O SQLite se recusa a guardar listas numa única célula
- **b.** Consultar dentro de uma coluna entupida é desajeitado e lento; uma tabela separada com uma chave estrangeira é consultável, indexável e combinável com JOIN
- **c.** Tradição que todos os bancos de dados seguem sem justificativa
- **d.** Só para fazer o esquema parecer maior

## 4. O que `GROUP BY k.id` faz na consulta com COUNT?

- **a.** Ordena as linhas de resultado pelo id do reino
- **b.** Junta as linhas numa só por reino, então `COUNT(b.id)` conta os prédios *de cada reino*
- **c.** Filtra as linhas que não têm um id
- **d.** Faz o JOIN da tabela com ela mesma uma segunda vez

## 5. Por que os apelidos de tabela `k` e `b` são usados (em vez dos nomes completos)?

- **a.** O SQL exige apelidos em toda referência de tabela
- **b.** Legibilidade — sem apelidos você repetiria `kingdoms.name`, `buildings.kingdom_id` em todo lugar; a consulta fica com cara de frase em inglês
- **c.** Para esconder do banco de dados os nomes reais das tabelas
- **d.** Apelidos rodam mais rápido que nomes completos

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
