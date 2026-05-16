# Quiz — Módulo 2.7

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Por que não dá para continuar usando `EnsureCreated()` depois que o app é publicado?

- **a.** Ele roda mais devagar que as migrações em máquinas reais
- **b.** Ele só funciona num banco de dados vazio; uma vez que há dados, mudanças de esquema não podem ser aplicadas sem descartá-lo
- **c.** Foi descontinuado no EF Core 9 e removido no EF Core 10
- **d.** Ele usa memória demais em servidores de produção

## 2. O que `dotnet ef migrations add InitialCreate` produz?

- **a.** Um novo arquivo de banco de dados com o esquema aplicado
- **b.** Arquivos C# em `Migrations/` descrevendo a mudança como `Up`/`Down`, mais um retrato do modelo
- **c.** Arquivos de SQL puro combinando com o dialeto do provedor configurado
- **d.** Nada visível até você também rodar `database update`

## 3. Para que serve a tabela `__EFMigrationsHistory`?

- **a.** Registrar erros de migração que aconteceram durante a aplicação
- **b.** A contabilidade do EF — acompanha quais migrações já foram aplicadas a este banco de dados
- **c.** Guardar métricas de desempenho para ajuste de consultas
- **d.** Manter informações opcionais de depuração para o designer do EF

## 4. Por que `EnsureCreated` e `Migrate` não combinam?

- **a.** São funcionalmente idênticos e o EF escolhe um para você
- **b.** Uma vez que um banco é criado via `EnsureCreated`, ele não tem histórico de migração; aí o `Migrate` acha que toda migração é nova
- **c.** Eles usam dialetos de SQL incompatíveis internamente
- **d.** Desempenho — rodar os dois é duas vezes mais lento que rodar um

## 5. O que é *desvio de esquema* (schema drift)?

- **a.** Outro nome para injeção de SQL
- **b.** O estado em que o esquema do banco de dados diverge do modelo do código — o bug que as migrações existem para prevenir
- **c.** Um tipo específico de código de erro do EF Core
- **d.** Nada de real; só um nome sem conceito por trás

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
