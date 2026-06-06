# Módulo 4.0 — Desbloqueio: Engenharia de Contexto

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Você já programa há cerca de sete meses. Você tem um motor, um banco de dados e uma API na internet que qualquer pessoa com a URL pode chamar. Hoje a IA entra no jogo. Agora você pode usá-la como ferramenta para escrever código. A pergunta não é mais *a IA consegue escrever algo?* — claro que consegue — é *como faço a IA escrever algo que se encaixe no meu projeto?*

Este módulo dá um nome a essa habilidade. Ela se chama **engenharia de contexto** — escolher o que a IA vê antes de responder. Código gerado por IA costuma falhar do mesmo jeito: sai como código genérico de tutorial. Funciona sozinho, mas ignora suas regras, nomeia as coisas de forma diferente do seu código existente e usa bibliotecas que você não tem. O contexto corrige isso. Faça a mesma pergunta com o contexto certo e você recebe uma resposta útil em três segundos. Faça sem contexto e você gasta trinta minutos em conversas de vai e vem. Você investe um pouco de tempo em contexto e economiza muito depois.

> **Words to watch**
>
> - **context window** — o pedaço de texto que a IA consegue ler de uma vez. Tem um limite.
> - **context engineering** — a prática de escolher o que vai nessa janela.
> - **prompt** — o que você diz à IA; o enquadramento da sua pergunta.
> - **reference set** — a pequena pasta de documentos específicos do projeto que a IA lê primeiro.
> - **explanation rule** — a regra pós-Desbloqueio: você precisa ser capaz de explicar cada linha que você envia.

---

## Início da fase — branch `phase-4`

Antes de qualquer código (o porquê está no Módulo 1.1):

```powershell
cd C:\code\kingdom
git switch -c phase-4
```

Todo commit desta fase vai para `phase-4`. No Módulo 4.7 (fechamento do M5), você vai abrir um PR para integrá-lo de volta em `main`.

---

## O enquadramento em quatro passos

Quando você pede à IA para escrever algo que não é trivial, dê quatro coisas em ordem. Primeiro o objetivo, depois onde o código vai, depois suas regras e por último as armadilhas a evitar. Tem uma quinta coisa que você pode adicionar — um exemplo parecido do seu próprio código — mas aprenda a versão de quatro passos primeiro.

1. **O objetivo**, em uma frase. *"Preciso de um método que retorna o reino mais rico do usuário."*
2. **Onde vai** — caminho do arquivo, a classe que pertence, o que o chama.
3. **As regras** — as partes do `STANDARDS.md` que importam aqui, seu estilo de nomeação, como você trata erros.
4. **O que não deve fazer** — as armadilhas. *"Não use `new Random()` — sempre injetamos `IRandom`."*

A quinta coisa, quando você quiser: um método parecido de algum outro lugar do seu projeto. *"Veja como fizemos o `Save`; faça o `LoadRichest` do mesmo jeito."* Esse único exemplo muitas vezes vale mais do que dois parágrafos de explicação.

## Escopo — a disciplina de pedir pouco

Tutoriais mostram pedidos grandes. *"Escreva um fluxo de checkout."* No seu próprio trabalho, essas respostas não servem — código demais, suposições demais, difícil de verificar linha por linha. Faça o oposto. **Um método por vez. Um arquivo por vez.** Um pedido pequeno é fácil de planejar, fácil de ler e fácil de verificar.

Se o trabalho é maior, monte o esqueleto você mesmo primeiro. Esboce os métodos vazios. Escreva os nomes dos testes. Depois peça à IA para preencher um método cuja assinatura você já decidiu. Você fica no controle do design; a IA faz a digitação.

## Avaliação — verificando a resposta

Leia cada resposta que a IA der, linha por linha, antes de ela ir perto do seu código. A regra pós-Desbloqueio é uma frase: *você precisa ser capaz de explicar cada linha que você envia*. Ninguém verifica isso por você — é por sua honra. Ela também guia toda revisão de PR que você fará pelo resto do ano. A parte de assistência por IA da descrição do seu PR lista os arquivos que a IA tocou; Lars os lê na conversa semanal.

Há um erro que merece um nome: **APIs inventadas.** Quando a IA está segura demais de si mesma, ela chama métodos que não existem. `db.Kingdoms.GetRichest()` parece real, mas não está no seu código. Sempre rode o código. Sempre leia o diff.

## O reference set

Mantenha uma pasta pequena de arquivos que a IA lê antes de responder. O curso já te dá a maioria deles:

- `STANDARDS.md` — suas convenções
- `CLAUDE.md` — as regras do lado da máquina para a IA (carregado automaticamente via o import do `CLAUDE.md` raiz)
- `.claude/commands/` — seus slash commands (`/explain-this-concept`, `/code-review`, `/stuck-on-error`, `/walk-through-code`, mais `/lesson-review` e `/milestone-review`)
- `GLOSSARY.md` — termos usados no seu projeto
- Um `ARCHITECTURE.md` curto — o que há em cada projeto, como os dados fluem (você vai escrever o seu no Passo 3 abaixo)

Quando você usa o Claude Code, o `CLAUDE.md` raiz e os slash commands carregam sozinhos. Quando você usa uma janela de chat em vez disso, cole os dois ou três arquivos mais úteis no início da conversa.

## O que muda neste módulo

- **NOVO:** `ARCHITECTURE.md` (você escreve o seu no Passo 3)
- **NOVO:** `.claude/commands/implementation-help.md` — um slash command para pedidos de código (você instala o seu no Passo 4)

Nenhuma mudança de código hoje. Este módulo é sobre as ferramentas que você configura em torno da IA, não sobre o código que a IA escreve.

## Passo 1 — leia o `CLAUDE.md` pós-Desbloqueio

Abra o `CLAUDE.md`. Observe:

- O sinalizador de modo agora diz `post-unlock`.
- A seção de comportamento pós-Desbloqueio se aplica.
- A seção de assistência por IA do template de PR está em vigor.

Leia do início ao fim. Você vai voltar a ele muitas vezes.

## Passo 2 — experimente o enquadramento de quatro passos em uma tarefa real

Escolha uma tarefa pequena. *"Escreva um método que retorna o reino do usuário com mais ouro."*

Sem contexto, o prompt parece assim:

> Eu: escreva um método que retorna o reino com mais ouro

A IA inventa uma classe `Kingdom` que não tem nada a ver com a sua, usa um estilo diferente de LINQ e não sabe nada sobre o seu `KingdomEfStore.ListSlots`. A resposta é inútil.

Com contexto:

> Eu: Estou trabalhando em `Kingdom.Persistence/EfCore/KingdomEfStore.cs`. A classe já tem `Save`, `Load`, `Delete`, `ListSlots(string ownerSub)`. Preciso de um método novo `LoadRichest(string ownerSub)` que retorna o `KingdomSummary` do usuário com o maior ouro (ou null se ele não tiver reinos). Use o estilo dos outros métodos (usando `using var ctx = new KingdomDbContext(_dbPath); ... AsNoTracking()`). Não carregue entidades Kingdom completas — projete para resumo inline.

A IA escreve o método certo, no estilo certo, em três linhas. A mesma pergunta, resposta muito diferente.

## Passo 3 — escreva o seu `ARCHITECTURE.md`

Uma página no root do seu repositório. Tente trinta a cinquenta linhas.

```markdown
# Architecture

## Projects
- `Kingdom.Engine/` — domain logic. No IO. No frameworks. Subnamespaces: Buildings, Citizens, Resources, Events, Infrastructure (interfaces), Snapshots.
- `Kingdom.Persistence/` — JSON store + SQLite (via raw SqliteConnection) + EF Core. `KingdomEfStore` is the canonical persistence.
- `Kingdom.Console/` — interactive console shell (`SaveSlotUI` menu loop).
- `Kingdom.Api/` — minimal API. Auth via Google OAuth + cookies. Multi-user via `OwnerSub` claim. OpenAPI at `/openapi/v1.json`.
- `tests/Kingdom.*.Tests/` — xUnit + Shouldly + FakeItEasy. Integration tests use `WebApplicationFactory<Program>`.

## Data flow
HTTP request → cookie auth → `OwnerSub` extracted → `KingdomEfStore` (scoped query) → engine via `Kingdom.LoadFrom(snapshot, rng, clock)`.

## Conventions
- See `STANDARDS.md` for naming, commits, branches, tests.
- DTOs at every boundary (JSON store, EF entities, API request/response).
- Engine takes `IRandom` + `IClock` via constructor — never `new Random()`.
- All store methods take `ownerSub` first; never optional.

## Not here yet
- Citizens are minimal — just a Name. Plans to add a `Mood` field and a `CitizenHappy` event.
- Events: 3 kinds (TraderArrived, CitizenIll, BuildingBurned). The browser shell will add CitizenStarved.
```

Faça commit. A IA agora lê isso quando uma sessão começa, e você também, mais tarde.

## Passo 4 — instale o slash command de implementação

Coloque isso em `.claude/commands/implementation-help.md` no seu repositório:

```markdown
---
description: Post-Unlock implementation help. Asks for goal/where/conventions/traps before writing code.
---

You are being invoked via `/implementation-help`. The learner is past the AI Unlock and is asking you to write non-trivial code. Read `CLAUDE.md` and `STANDARDS.md` first.

If the learner already pasted the goal in `$ARGUMENTS`, use it. Otherwise ask for these in one combined message:

1. **Goal** — one sentence on what the code needs to do.
2. **Where** — file path + a snippet of the surrounding code.
3. **Existing patterns to match** — one or two small snippets from nearby methods.
4. **Conventions to follow** — relevant `STANDARDS.md` sections plus any project-specific quirks (link to them).
5. **Traps** — what *not* to do (e.g. *"don't use `new Random()` — use `IRandom`"*).

Once you have all five, write the implementation. Match the style. Stay inside the named conventions. Don't invent APIs that aren't visible in the code the learner showed you.

End your response with: *"Before you keep this, walk me through what each line does. If you can't explain a line, ask me about it instead of keeping it."*
```

Reinicie o Claude Code (ou rode *"Reload Window"* no VS Code). Agora digite `/` e seu novo comando deve estar na lista.

## Mexa um pouco

Use o enquadramento de quatro passos em uma tarefa real hoje. Veja como a resposta melhora muito. Depois tente dois prompts lado a lado: um só com o objetivo, outro com contexto completo. Guarde as duas respostas. Leia de novo daqui a um mês. Você vai ver a diferença mais claramente com o tempo.

O seu `ARCHITECTURE.md` vai ficar desatualizado conforme o projeto cresce. Mantê-lo atualizado é um trabalho pequeno por conta própria — verifique a cada marco, mesmo que a única mudança seja *"ainda correto."*

Verifique a saída da IA em busca de APIs inventadas. Quando ela está segura demais de si mesma, ela inventa coisas. Pegue cedo. Não integre um método que chama algo que não existe.

## O que você acabou de fazer

Você aprendeu a habilidade que vai usar pelo resto do curso. **Engenharia de contexto** é escolher o que a IA vê antes de responder — e o enquadramento de quatro passos (objetivo, onde, regras, armadilhas) é o movimento que você vai repetir. Você escreveu um `ARCHITECTURE.md` curto para que a IA comece a partir do seu projeto, não de um tutorial genérico. Você instalou um slash command `/implementation-help` para que o próximo pedido de código siga um conjunto claro de passos em vez de ser uma pergunta aberta. Você também conheceu a regra pós-Desbloqueio que tudo a partir daqui se baseia: você pode enviar código gerado por IA, *e* você pode explicar cada linha que enviou. Dois arquivos no disco; uma regra na cabeça.

**Conceitos que você já sabe nomear:**

- **engenharia de contexto** — escolher o que a IA vê para que a resposta se encaixe
- **o enquadramento de quatro passos** — objetivo, onde, regras, armadilhas
- **escopo** — um método, um arquivo por vez; pedidos pequenos ganham dos grandes
- **APIs inventadas** — o erro em que a IA chama métodos que não existem
- **a regra da explicação** — você precisa ser capaz de explicar cada linha que você envia

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Da sua própria cabeça:

1. Liste as quatro coisas que você dá à IA antes de ela escrever código para você, em ordem.
2. Diga cada uma em voz alta, ou escreva.

<details><summary>Travou? Abra aqui para conferir.</summary>

O enquadramento de quatro passos, em ordem:

1. **O objetivo** — uma frase sobre o que o código deve fazer.
2. **Onde vai** — o arquivo, a classe, o que o chama.
3. **As regras** — seu estilo de nomeação, seu `STANDARDS.md`, como você trata erros.
4. **O que não deve fazer** — as armadilhas. (Por exemplo: não use `new Random()` — use `IRandom`.)

Se você acertou os quatro na ordem certa, você tem o movimento. A quinta coisa — um exemplo parecido do seu próprio código — é o bônus que você adiciona quando os quatro já parecem fáceis.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas em `journal/quiz-notes.md`.
2. **Progresso** — uma linha em `journal/progress.md`: `Module 4.0 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 4.0 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos do painel/CLI caso precise rever. Leve à próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

O Módulo 4.1 começa o trabalho real no browser — HTML e CSS. A página mais simples útil que mostra o seu reino, aberta direto de um arquivo `.html` sem nenhuma etapa de build ainda.
