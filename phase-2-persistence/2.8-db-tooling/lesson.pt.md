# Módulo 2.8 — Ferramentas de Banco de Dados

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Você pode escrever a estrutura perfeita, as migrations mais limpas e as melhores queries — e ainda assim perder horas quando algo parece errado, porque você não consegue ver o que realmente está no banco de dados. Hoje é curto e é sobre ferramentas: três jeitos de *olhar dentro* do seu arquivo SQLite, mais um comando EF que responde *"o que esta migration faria?"* em dez segundos.

> **Words to watch**
>
> - **DB Browser** — um app gratuito para SQLite — abra o arquivo `.db`, navegue nas tabelas, rode queries
> - **`sqlite3`** — a ferramenta oficial de linha de comando — o mesmo poder, sem interface gráfica
> - **VS Code SQLTools** — uma extensão que deixa você consultar o banco de dados dentro do editor
> - **migration script** — SQL gerado mostrando exatamente o que uma migration vai fazer

---

## Por que este módulo existe

Você não consegue ver dentro de um banco de dados até ter um jeito de olhar. A ferramenta certa transforma *"salvar está quebrado de algum jeito"* em *"a row 47 tem `gold = -1`, e aqui está o porquê."* A correção depois disso é fácil. Sem a ferramenta, você passa uma hora adicionando linhas de `Console.WriteLine` em vez disso.

Todo desenvolvedor tem uma ou duas ferramentas de banco de dados favoritas. Hoje a gente olha para três. Escolha a que parecer mais fácil para você.

---

## Ferramenta 1 — DB Browser for SQLite

A mais fácil de usar. Gratuita. Roda em qualquer sistema operacional.

- Download: <https://sqlitebrowser.org/>
- Abra, File → Open Database → escolha `bin/Debug/net10.0/saves/kingdoms-ef.db`
- Aba *Browse Data* — veja cada row em cada tabela
- Aba *Execute SQL* — escreva SQL, clique em rodar, veja a grade de resultados

Quando usar: na maioria das vezes. Clique para explorar. Os dados estão na sua frente.

---

## Ferramenta 2 — CLI `sqlite3`

Já vem no macOS. No Windows: baixe `sqlite-tools` em <https://sqlite.org/download.html> e coloque `sqlite3.exe` no seu PATH.

```powershell
sqlite3 saves/kingdoms-ef.db
```

Agora você está num prompt interativo. Comandos úteis:

```
.tables                       # lista tabelas
.schema kingdoms              # mostra o CREATE TABLE
SELECT * FROM kingdoms;       # query
.headers on                   # cabeçalhos de coluna na saída
.mode column                  # saída tabular
.quit                         # sair
```

Quando usar: em scripts, em CI, e sempre que abrir um app completo for mais do que você precisa. Depois de se acostumar, é muito mais rápido para uma query rápida.

---

## Ferramenta 3 — extensão SQLTools do VS Code

Se você vive no VS Code:

1. Instale as extensões: `SQLTools` e `SQLTools SQLite`.
2. Abra a Command Palette → *"SQLTools: Add new connection"* → SQLite → aponte para o arquivo `.db`.
3. A barra lateral de Connections agora mostra suas tabelas. Clique com o botão direito → *"Open Table"*.
4. Abra um arquivo `.sql`, escreva uma query, e aperte Ctrl+E duas vezes para rodá-la.

Quando usar: se você gosta de manter tudo no VS Code e nunca trocar de janela, isso encaixa melhor porque vive direto no editor.

---

## Ferramenta EF — `migrations script`

Um comando EF muito útil que poucas pessoas conhecem:

```powershell
dotnet ef migrations script --project Kingdom.Persistence --startup-project Kingdom.Console
```

Ele imprime o *SQL exato* que todas as migrations rodariam num banco de dados novo. Ele não roda nada — só te mostra o SQL.

Ou o SQL entre duas migrations específicas:

```powershell
dotnet ef migrations script NomeAnterior NomeAtual --project Kingdom.Persistence --startup-project Kingdom.Console
```

Quando usar:

- Você quer saber o que uma migration *vai* fazer antes de rodá-la — especialmente num app em produção.
- Você quer compartilhar o SQL com um colega, um administrador de banco de dados ou um revisor.
- Você está revisando uma mudança de estrutura como parte de uma mudança de código.

---

## Delta starter

Este módulo é só leitura e ferramentas. O starter contém:

- `tools/sqlite-tour.md` — uma lista de uma página dos comandos acima
- `tools/sample-queries.sql` — cinco queries prontas para copiar para o banco de dados kingdoms

Sem mudanças no engine ou na persistência, e sem novos testes.

`tools/sample-queries.sql`:

```sql
-- Cinco queries de exemplo contra o seu banco de dados kingdoms.
-- Copie qualquer uma delas para o DB Browser, sqlite3 ou SQLTools.

-- 1. Todo reino + quantos prédios tem
SELECT k.name, COUNT(b.id) AS building_count
FROM Kingdoms k
LEFT JOIN Buildings b ON b.KingdomId = k.Id
GROUP BY k.Id
ORDER BY building_count DESC;

-- 2. O reino mais rico (em gold)
SELECT name, gold FROM Kingdoms ORDER BY gold DESC LIMIT 1;

-- 3. Prédios de cada tipo (Farm/Lumberyard/Mine), com níveis totais
SELECT kind, COUNT(*) AS n, SUM(level) AS total_levels
FROM Buildings GROUP BY kind;

-- 4. Reinos que têm pelo menos uma Mine
SELECT DISTINCT k.name
FROM Kingdoms k
JOIN Buildings b ON b.KingdomId = k.Id
WHERE b.kind = 'Mine';

-- 5. O histórico de migrations (o que o EF já aplicou)
SELECT * FROM __EFMigrationsHistory;
```

## Mexa um pouco

Rode a query #1 contra um banco de dados com três saves. Veja como `LEFT JOIN` mais `GROUP BY` responde uma pergunta real em uma linha.

Edite os dados direto dentro do DB Browser (coloque o gold de um reino em 9999), salve, e rode o programa de novo. A mudança ainda está lá.

No `sqlite3`: use `.import data.csv kingdoms` para carregar muitas rows de uma vez. Útil quando você tem 1000 rows de teste e não quer digitá-las na mão.

Rode `dotnet ef migrations script -i`. O `-i` torna idempotente — o SQL verifica cada migration com um guarda estilo `IF NOT EXISTS` primeiro, então rodá-lo mais de uma vez não faz mal.

## O que você acabou de fazer

Você pegou três jeitos de ver dentro do seu banco de dados: DB Browser (o app fácil de usar), a linha de comando `sqlite3` (simples e rápida), e a extensão SQLTools do VS Code (direto no editor). Mais um comando EF — `migrations script` — que imprime o SQL que uma migration *rodaria* sem rodá-lo. Nenhum desses muda o seu código. O que eles mudam é a rapidez com que você consegue responder *"o que realmente está no banco de dados agora?"* Daqui a seis meses, essa é a diferença entre uma correção de cinco minutos e uma busca de cinco horas.

**Conceitos que você já sabe nomear:**

- **DB Browser** — app gratuito e fácil para SQLite
- **CLI `sqlite3`** — ferramenta oficial de linha de comando
- **SQLTools (VS Code)** — queries de banco de dados dentro do editor
- **`dotnet ef migrations script`** — veja o SQL sem aplicá-lo
- **`__EFMigrationsHistory`** — visível em qualquer uma das ferramentas acima

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — mostre para você mesmo, da sua própria cabeça, que a ideia grande pegou: você pode abrir o arquivo `.db` numa ferramenta e ver o que realmente está dentro. Ninguém corrige isso — é só para você. É o jeito mais rápido de descobrir o que ainda não pegou, enquanto ainda é pequeno para consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Escolha a ferramenta que você mais gostou — DB Browser, `sqlite3`, ou SQLTools. Depois:

1. Abra seu arquivo `kingdoms-ef.db`.
2. Encontre a lista de tabelas.
3. Olhe as rows de uma tabela.
4. Rode um `SELECT` seu.

Sem anotações — só abra e olhe.

<details><summary>Travou? Abra aqui para conferir.</summary>

O que você deveria ter conseguido:

- Abriu `bin/Debug/net10.0/saves/kingdoms-ef.db` na ferramenta escolhida.
- Viu as tabelas que o EF criou: `Kingdoms`, `Buildings` e `__EFMigrationsHistory`.
- Navegou pelas rows de uma tabela (ou rodou `SELECT * FROM Kingdoms;`).
- Rodou uma query que você mesmo escreveu e recebeu uma grade de resultados.

Se usou `sqlite3`, o caminho rápido é `.tables` para listá-las, depois `SELECT * FROM Kingdoms;`. Se a ferramenta mostra suas rows, você agora consegue ver dentro de qualquer banco de dados que você construir — essa é a habilidade inteira de hoje.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 2.8 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 2.8 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve as respostas do quiz de que você tiver menos certeza para a próxima conversa semanal.

## Próximo

Módulo 2.9 — **save slots** — usa tudo que a gente tem. Muitos reinos em um banco de dados: liste-os e carregue qualquer um deles. Uma experiência de save slot de verdade, como a tela de carregamento de um jogo.
