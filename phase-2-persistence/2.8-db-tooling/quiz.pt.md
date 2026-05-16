# Quiz — Módulo 2.8

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. Por que instalar uma interface gráfica de banco de dados se você já tem o EF Core no seu código?

- **a.** O EF se recusa a conectar sem uma rodando ao lado dele
- **b.** O EF deixa você ler e gravar dados; uma interface gráfica deixa você *ver* o que está de fato no banco agora
- **c.** São funcionalmente idênticos e a interface gráfica só fica mais bonita
- **d.** Uma interface gráfica é exigida pelo runtime do .NET para suporte ao SQLite

## 2. O que `.schema kingdoms` mostra no `sqlite3`?

- **a.** Os dados dentro da tabela kingdoms, formatados como linhas
- **b.** A instrução CREATE TABLE — a estrutura da tabela
- **c.** Os índices definidos na tabela, mas não as colunas
- **d.** Nada de útil; o comando existe por compatibilidade com versões antigas

## 3. O que `dotnet ef migrations script` faz?

- **a.** Roda todas as migrações pendentes contra o banco de dados atual
- **b.** Mostra o SQL que as migrações *rodariam*, sem rodar ele
- **c.** Apaga os arquivos de migração do projeto
- **d.** Lista os arquivos de migração por nome, na ordem em que foram adicionados

## 4. Onde, num banco de dados SQLite, o EF acompanha quais migrações foram aplicadas?

- **a.** Num arquivo lateral, ao lado do .db
- **b.** Numa tabela especial chamada `__EFMigrationsHistory`
- **c.** Só na memória — a lista é reconstruída a cada conexão
- **d.** Embutido na string de conexão na hora do build

## 5. A lição diz *"tenha uma janela para o seu banco de dados antes de precisar de uma."* Por quê?

- **a.** Preparação para o futuro — quando algo quebrar, você quer a ferramenta de diagnóstico já instalada e conhecida
- **b.** O .NET exige uma ferramenta configurada antes de o EF conectar
- **c.** Faz parte dos requisitos do desafio do marco M3
- **d.** Pura preferência cosmética; nenhum motivo prático real

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
