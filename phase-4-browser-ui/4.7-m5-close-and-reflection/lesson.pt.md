# Módulo 4.7 — Fechamento do M5 + Reflexão da Fase 0

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

O reino roda nos browsers. **Fase 4 concluída. Cinco marcos, quatro shells, um motor.** Hoje fecha a fase com o ritual do marco e um exercício tranquilo mas poderoso: releia o código que você escreveu na Fase 0 — a Spark Week — de novo, agora que você sabe muito mais. Oito meses de progresso, fácil de ver em uma comparação.

> **Words to watch**
>
> - **reflection** — reler o seu código antigo de novo, agora que você sabe mais.
> - **before/after** — a pequena nota de uma linha que você escreve em cada marco.
> - **wins log** — a sua lista de marcos em `journal/wins.md`.

---

## O exercício de reflexão

Abra o seu código da Fase 0. Os primeiros brinquedos que você escreveu — `RoastOMatic`, `NumberGuess`, `TinyAdventure`. Leia-os.

Escolha *um* desses brinquedos (o pior, de preferência) e passe trinta minutos limpando-o, agora que você sabe mais. Não adicione funcionalidades — só melhore o que está lá. Procure variáveis que deveriam ter sido constantes. Métodos que deveriam ser divididos em menores. Nomes que não dizem o que significam. Comentários que agora estão desatualizados. Números mágicos. A coisa que você faria *completamente diferente* agora.

Salve o diff. Faça commit com `[refactor] phase-0 <brinquedo> — applying lessons from phases 1-4`. Esse commit *é* a prova de que você consegue ver a diferença entre o seu código antigo e o seu código novo.

Não limpe *toda* a Fase 0 — isso é trabalho inútil. Um brinquedo é o experimento. O ponto é *ver a diferença*.

## Ritual do marco M5

Mesmo padrão do M2, M3 e M4. A atualização do README vem primeiro — essa é a regra; todo o resto segue.

Primeiro, **atualize o README** no root do repositório. Passe pelas quatro seções do Módulo 0.4 de novo. *Como rodar* agora precisa do passo `vite dev` ao lado da API; *O que aprendi* recebe um parágrafo da Fase 4 (o browser como runtime, TS vanilla, fetch + render). Não pule isso — todo fechamento de marco volta a este passo.

Segundo, abra `journal/wins.md` e escreva a entrada do M5:

```markdown
## M5 — Phase 4 — Browser-Playable Kingdom

- Live frontend URL: `https://kingdom-______.azurestaticapps.net`
- Vite + TypeScript + componentised vanilla TS
- Frontend tests in Vitest
- Four shells now: console, persistence, web API, browser
- Phase 0 reflection: refactored `<toy>`; commit `<hash>`

**Before:** the kingdom was a thing in my terminal.
**After:**  friends play in their browser. I read my old Spark Week code and saw how far I've come.

Posted to `#wins` on YYYY-MM-DD.
```

Terceiro, tire um screenshot da sua URL ao vivo com o reino renderizado nela, e poste em `#wins` no Slack.

Quarto, marque o marco. Este passo é só pela CLI — o painel não tem botão para tags:

```powershell
git tag m5-phase-4-complete
git push origin m5-phase-4-complete
```

Quinto, **abra o PR do M5.** No github.com → seu repositório `kingdom` → banner *"phase-4 had recent pushes — Compare & pull request"* (ou *Pull requests → New pull request*, base `main`, compare `phase-4`). Título: `M5 — Phase 4 — Browser-Playable Kingdom`. Corpo: os bullets do `wins.md` deste marco + `**Reviewer:** @dingdonglars` + a seção de assistência por IA do template pós-Desbloqueio. Lars lê antes da conversa semanal e Aprova; depois você Faz Merge e deleta o branch `phase-4`. Na sua máquina: `git switch main && git pull`. (Guia completo: Módulo 1.10.)

## Mexa um pouco

Compare o seu `KingdomCard.ts` do Módulo 4.4 com o seu `Program.cs` da Fase 0. Um arquivo tem vinte linhas de TypeScript limpo; o outro tem centenas de linhas fazendo uma coisa. O progresso é real, e você pode vê-lo.

Leia o `STANDARDS.md` de novo. Tem algo lá que parece óbvio agora e não parecia um ano atrás? Esse é um sinal de quanto você aprendeu.

Leia o `CLAUDE.md` de novo. Note que o modo é `post-unlock`. A forma como você usa a IA mudou desde que a Fase 4 começou? O que funciona bem? O que não funciona? Escreva duas frases em `journal/wins.md` se algo se destacar.

Tire um screenshot do seu reino rodando no browser e use-o como a primeira mensagem de commit da Fase 5 (Roblox). Ele marca a passagem da Fase 4 para a Fase 5.

## O que você acabou de fazer

A Fase 4 fechou. O reino agora toca em qualquer browser. Você limpou um brinquedo da Fase 0, o que prova que você consegue ver a diferença entre o seu código antigo e o seu código novo. O ritual do M5 está no disco — entrada em `wins.md`, post no Slack `#wins`, tag do marco enviada, PR aberto. Quatro shells, um motor, oito meses de progresso. Reler o código antigo de novo, agora que você sabe mais, é uma das formas mais baratas de verificar o quanto você aprendeu. Se você só sente uma vaga insatisfação com o código antigo, isso é ansiedade; se você consegue dizer exatamente o que mudaria (*"eu tiraria isso para um método, renomearia esta variável, deletaria este comentário"*), isso é habilidade real.

**Conceitos que você já sabe nomear:**

- **reflexão** — reler o seu código antigo de novo, agora que você sabe mais
- **before/after** — uma linha por marco; juntas elas contam a sua história ao longo do tempo
- **wins log** — `journal/wins.md`; a sua lista de marcos
- **insatisfação específica vs vaga** — saber exatamente o que mudar é habilidade; vaga insatisfação é só ansiedade

## Por sua conta

Hora de fechar o livro. Não role de volta. Ninguém corrige isto — é só para você. É o jeito mais fácil de ver se a diferença entre código antigo e novo é habilidade real ou só uma sensação vaga. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra o seu código antigo da Fase 0 mais uma vez. Da sua própria cabeça:

1. Escolha três linhas que você mudaria.
2. Para cada uma, diga a mudança *exata* em voz alta ou escreva — não "isso parece errado" mas uma edição concreta, como "eu renomearia esta variável para `goldCount`", "eu tiraria estas linhas para um método chamado `RollDice`" ou "eu deletaria este comentário, ele está desatualizado".

<details><summary>Travou? Abra aqui para conferir.</summary>

Não há uma única resposta certa aqui — é o seu código. Uma boa resposta nomeia a mudança *exata*, como estas:

- "Este `int x = 5;` deveria ser uma constante com nome — `const int StartingGold = 5;`."
- "Essas dez linhas que escolhem um número aleatório pertencem ao seu próprio método, `RollDice()`."
- "Esta variável `temp` deveria ser `roastLine` — o nome deveria dizer o que ela guarda."

Se suas três mudanças são assim tão específicas, essa é a habilidade sobre a qual a lição fala. Se tudo o que você consegue dizer é "parece bagunçado," essa é a vaga insatisfação — e nomear até uma mudança concreta é como você a transforma em habilidade.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas em `journal/quiz-notes.md`.
2. **Progresso** — uma linha em `journal/progress.md`: `Module 4.7 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 4.7 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 explica o porquê e os passos do painel/CLI caso precise rever. Leve à próxima conversa semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

**A Fase 5 começa.** A Fase 5 é o port para Roblox — o seu motor entra em um runtime diferente e uma linguagem diferente (Luau). Mesma ideia de motor versus shell de antes; é a prova de que a ideia nunca foi realmente sobre C#.
