# Quiz — Bônus B1.1

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que é o LocalDB?

- **a.** Um banco de dados NoSQL para guardar documentos JSON numa máquina só
- **b.** A edição de desenvolvedor do SQL Server — usuário único, uma instância por usuário, nenhum serviço para gerenciar
- **c.** Um substituto direto do SQLite escrito pela Microsoft para uso multiplataforma
- **d.** Uma ferramenta de log que captura consultas contra um SQL Server remoto

## 2. Por que a troca de SQLite para SQL Server é montada como uma lição?

- **a.** Porque o SQLite é inadequado para produção e temos que sair dele antes de publicar
- **b.** Porque a regra engine-versus-shell prevê que a troca vai ser pequena — e provar que ela é pequena é todo o objetivo
- **c.** Porque o LocalDB é mais rápido que o SQLite para a carga de trabalho do reino
- **d.** Porque a Microsoft paga melhor que a equipe do SQLite por tutoriais

## 3. A string de conexão do LocalDB começa com...

- **a.** `Server=(localdb)\MSSQLLocalDB;Database=...`
- **b.** `Server=localhost:5432;Database=...`
- **c.** `Data Source=:memory:;Mode=Shared`
- **d.** `mongodb://localhost:27017/kingdom`

## 4. Por que o LocalDB é só para Windows?

- **a.** A Microsoft não fez o port; no macOS ou Linux você roda a imagem Docker do SQL Server no lugar
- **b.** Por motivos de desempenho — a base de código depende de um recurso de kernel específico do Windows
- **c.** É uma restrição de licença; a Microsoft vende a edição de macOS separadamente
- **d.** Tradição; ninguém pediu um port para a Microsoft

## 5. O que `sqllocaldb info` faz?

- **a.** Imprime o número da versão do LocalDB do pacote instalado
- **b.** Lista as instâncias do LocalDB nesta máquina (a instância padrão é `MSSQLLocalDB`)
- **c.** Roda um pequeno teste de desempenho contra a instância padrão
- **d.** Necessário na hora da instalação; configura as tabelas de sistema

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
