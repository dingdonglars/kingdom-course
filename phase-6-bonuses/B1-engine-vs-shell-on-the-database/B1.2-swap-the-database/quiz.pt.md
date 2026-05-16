# Quiz — Bônus B1.2

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Quantas linhas de código mudam para trocar o SQLite pelo SQL Server?

- **a.** Cerca de três — a referência de pacote, a chamada do provedor, a string de conexão
- **b.** Cerca de cinquenta — provedor, métodos da store, configs de entidade, fixtures de teste
- **c.** Várias centenas, porque toda consulta tem que ser reescrita em T-SQL
- **d.** A engine inteira — store, entidades e consultas, tudo precisa ser mexido

## 2. Por que as migrações precisam ser regeradas quando você muda de provedor?

- **a.** Arquivos de migração contêm SQL específico do provedor — o mesmo `Add Column` em C# produz uma saída diferente por provedor
- **b.** Desempenho — migrações antigas rodam devagar demais contra o novo provedor
- **c.** O EF Core impõe isso como regra; não há um motivo técnico por trás
- **d.** Estilo — os arquivos novos ficam mais bonitos quando recém-gerados

## 3. A lição chama o resultado de "chato de propósito". Por que isso é o ponto?

- **a.** A regra engine-versus-shell previu que a troca seria pequena. O tédio é a prova de que a disciplina se manteve.
- **b.** O SQL Server é um banco de dados chato comparado a opções NoSQL mais novas
- **c.** A lição é curta para economizar tempo antes da instalação do SSMS de amanhã
- **d.** O EF Core é um framework chato que esconde tudo que é interessante

## 4. O mesmo padrão de três linhas funciona para...

- **a.** Só o SQL Server, porque a Microsoft fez o EF Core para o próprio banco dela
- **b.** SQL Server, PostgreSQL, MySQL e qualquer banco de dados para o qual o EF Core tenha um provedor
- **c.** Só o SQLite mais o SQL Server, porque os outros usam APIs diferentes
- **d.** Só bancos da Microsoft — provedores de código aberto precisam de uma abordagem diferente

## 5. Por que "os testes passam sem mudança" importa como o momento da prova?

- **a.** Testes descrevem o comportamento, não a implementação — passar significa que a engine ainda produz o mesmo reino contra uma store diferente
- **b.** Testes são exigidos pelo currículo; passar neles é só cumprir uma regra
- **c.** Desempenho — testes que passam rodam mais rápido que os que falham
- **d.** Estilo — vistos verdes ficam mais bonitos que vermelhos

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
