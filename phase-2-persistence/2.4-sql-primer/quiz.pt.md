# Quiz — Módulo 2.4

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O SQLite é melhor descrito como...

- **a.** Um serviço web ao qual você se conecta por HTTPS
- **b.** Um servidor de banco de dados como PostgreSQL ou MySQL, mas mais leve
- **c.** Uma biblioteca mais um banco de dados de arquivo único — sem servidor, sem instalação
- **d.** Uma ferramenta de backup para mover SQL entre sistemas

## 2. Por que usar parâmetros (`$name`) em vez de concatenação de string no SQL?

- **a.** Parâmetros têm desempenho mais rápido do lado do banco de dados
- **b.** Para prevenir injeção de SQL — entrada de usuário colada deixa um atacante rodar SQL arbitrário
- **c.** O compilador C# recusa strings de SQL concatenadas
- **d.** O SQLite lança um erro em comandos concatenados

## 3. Qual de `CREATE` / `INSERT` / `SELECT` / `UPDATE` / `DELETE` retorna linhas?

- **a.** `INSERT` — retorna as linhas que acabou de inserir
- **b.** `UPDATE` — retorna as linhas que modificou
- **c.** `SELECT` — retorna as linhas que correspondem à consulta
- **d.** `CREATE` — retorna a estrutura da nova tabela

## 4. Por que `using var conn = new SqliteConnection(...)` é importante?

- **a.** Abre a conexão automaticamente quando a execução chega na linha
- **b.** Garante que o `Dispose` rode (fechando a conexão) mesmo se uma exceção for lançada
- **c.** Habilita suporte a consultas assíncronas sem configuração extra
- **d.** É puramente cosmético e poderia ser omitido

## 5. A lição diz *"o banco de dados é um runtime."* O que isso quer dizer aqui?

- **a.** O SQLite vem com o próprio programa de shell de linha de comando
- **b.** Salvar é uma preocupação de runtime, não da engine — mesma engine, backend de armazenamento diferente
- **c.** Consultas SQL rodam dentro de um ambiente de script embutido
- **d.** Nada significativo; só terminologia

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
