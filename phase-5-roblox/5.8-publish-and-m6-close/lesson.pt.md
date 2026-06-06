# Módulo 5.8 — Publicar + Fechamento M6

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

O último passo técnico do ano é um botão: *File → Publish to Roblox*. Depois disso, uma URL. Depois disso, amigos jogam. **Um ano. Cinco shells. Um engine. M6.**

> **Words to watch**
>
> - **Publish** — enviar o seu `.rbxl` local para o Roblox. Gera um place ID.
> - **Place ID** — a URL única do seu jogo (`roblox.com/games/<id>/<nome>`).
> - **Final reflection** — a sessão que você faz com `journal/m6-looking-back.md`. Cerca de uma hora. Tranquilo.
> - **M6** — o último marco numerado do ano. Você é um programador que publicou cinco versões diferentes de um engine.

---

## Passo 1 — publicar

No Studio: *File → Publish to Roblox* → *Create New* → dê um nome ao place → salve.

Depois de salvar, você verá a URL do place: `roblox.com/games/<id>/<nome-do-seu-place>`. Qualquer pessoa com o link pode visitar. Por padrão, o place é público e gratuito para jogar.

Se quiser manter privado enquanto você polir: *Game Settings → Permissions → Friends only*, ou configure como não listado. Você pode voltar a público quando estiver pronto.

## Lista de verificação antes de publicar

Antes de clicar no botão, percorra esta lista uma vez:

- [ ] Scripts do servidor funcionam em um playtest do Studio sem erros no Output.
- [ ] DataStore ligado em *Game Settings → Security*.
- [ ] Testado com *Test → Local Server → 2 players* — multiplayer funciona.
- [ ] Vale a pena jogar por pelo menos sessenta segundos sem seu amigo desistir.
- [ ] Visual: tiles aparecem, clique de construção funciona, contador de dias faz tick.
- [ ] Recursos ficam não negativos; sem bugs de índice fora do intervalo.
- [ ] Salva ao sair do jogador; entrar de novo restaura o estado salvo.
- [ ] Sem informações pessoais em scripts (de acordo com a regra de padrões).

## Passo 2 — enviar a URL

Escolha três amigos. Mande a URL com uma frase: *"Eu construí isso. Experimenta."*

Depois fique com o que acontecer. Eles podem achar um bug. Podem sugerir funcionalidades. Podem parar de jogar depois de cinco minutos. Podem continuar por uma hora. O que quer que aconteça, o seu código se tornou parte do dia de outra pessoa. **É para isso que tudo isso serviu.**

## O ritual do M6

Mesmos passos dos marcos anteriores — maior, e o último:

1. **Atualize o README na raiz do repositório pela última vez.** Percorra as quatro seções do Módulo 0.4 de novo. O repositório agora tem cinco shells — *How to run* deve cobrir todos eles; *What I learned* é um parágrafo sobre o ano inteiro; *What's next* é honesto sobre se você vai continuar ou parar. Esta é a versão que as pessoas vão ler quando clicarem a partir da URL do jogo ao vivo.
2. Abra `journal/wins.md`. Escreva a entrada do M6:

   ```markdown
   ## M6 — Phase 5 — Roblox-Published Kingdom

   - **Public Roblox URL:** roblox.com/games/<id>/<name>
   - 5 shells, one engine: console / file with JSON and SQLite / web API / browser / Roblox
   - Engine ported from C# to Luau
   - DataStore persistence; multiplayer-ready; friends played

   **Before:** I asked Lars what programming was
   **After:**  I built and shipped a multiplayer game

   Posted to `#wins` on YYYY-MM-DD.
   ```

3. **Crie a tag do marco.** Esta é só pelo CLI — o painel não tem botão para tags: `git tag m6-phase-5-complete && git push origin m6-phase-5-complete`.
4. **Abra o PR do M6.** No github.com → seu repositório `kingdom` → banner *"phase-5 had recent pushes — Compare & pull request"* (ou *Pull requests → New pull request*, base `main`, compare `phase-5`). Título: `M6 — Phase 5 — Roblox-Published Kingdom`. Corpo: os bullets do `wins.md` deste marco + `**Reviewer:** @dingdonglars` + a seção de assistência de IA do template pós-Unlock. Este é o PR final. Depois que Lars aprovar e você fizer a viva (próximo passo), você faz o Merge e apaga o branch `phase-5`. (Guia completo: Módulo 1.10.)
5. Viva final com Lars — ele escolhe linhas aleatórias do engine e pede para você explicá-las, mais uma conversa "me conta a história de um ano".

## A reflexão final (uma sessão, cerca de uma hora)

Abra `journal/m6-looking-back.md`. Responda livremente:

1. **Qual é a única coisa que você ensinaria para outra pessoa?** Escolha a lição que mais te surpreendeu. Escreva como um post de blog de dois parágrafos que você enviaria para um amigo que está prestes a começar.
2. **O que é o engine, em um parágrafo?** Explique o fio condutor — engine vs shell — para uma pessoa inteligente que nunca programou. Não o que você construiu; o que você *aprendeu*.
3. **Qual é um projeto que você começaria em seguida?** Agora que você tem as ferramentas, o que você quer construir? Não precisa ser sobre Kingdom. Liste três ideias, escolha uma e esboce uma "menor versão interessante" estilo Fase 0.
4. **Releia o seu código da Spark Week.** Leia `journal/wins.md` do M0 ao M6 em ordem. Fique com isso. Perceba o que é diferente na forma como você pensa agora.

## Mexa um pouco (estes são *por diversão* agora)

- Adicione um placar com `leaderstats` embutido do Roblox para que cada jogador veja os kingdoms do topo.
- Adicione um comando de chat (`/build farm`) — uma pequena lembrança do terminal, agora dentro do Roblox.
- Adicione uma progressão simples: com 100 de ouro, desbloqueie o Lumberyard; com 500, desbloqueie a Mine.
- Mude um dos loops do Módulo 5.5 para usar `RunService.Heartbeat` para um tick mais rápido. O jogo parece diferente.
- Aprenda uma coisa nova por conta própria. Escolha qualquer coisa. O curso te deu as ferramentas para aprender o que vier a seguir.

## O que você acabou de fazer

Você pegou o engine que escreveu na Fase 1 e o publicou como um place público no Roblox. A lista de verificação pré-publicação mostrou que o place era realmente jogável. O clique no publish o transformou em uma URL que qualquer pessoa poderia abrir. As três mensagens transformaram essa URL em algo que outras pessoas jogaram. **Um ano. Cinco shells. Um engine.** Console, arquivo com JSON e SQLite, web API, browser, Roblox. Duas linguagens, dois lugares para rodar, um modelo que nunca mudou. Todo o ponto do curso se tornou real em suas mãos quando você enviou aquela URL.

**Conceitos que você já sabe nomear:**

- *publish* — `File → Publish to Roblox`; gera um place ID
- *place ID* — a URL que os amigos clicam para jogar o seu jogo
- *o ritual do M6* — entrada de wins, tag, PR de marco, viva
- *reflexão final* — a sessão de uma hora com `m6-looking-back.md`
- *a ideia principal, provada* — cinco shells, um engine, duas linguagens

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que o último grande movimento pegou. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que ainda não pegou, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem olhar:

1. Diga o caminho de menu do Studio que transforma o seu place local em uma URL que os amigos podem abrir.
2. Depois diga a frase que o curso inteiro estava provando — o engine versus onde ele roda.

<details><summary>Travou? Abra aqui para conferir.</summary>

O caminho de publicação: **File → Publish to Roblox** → *Create New* → dê um nome ao place → salve. Depois disso você tem uma URL, `roblox.com/games/<id>/<nome>`, que qualquer pessoa pode abrir.

A ideia: **o modelo dura; onde ele roda é só um detalhe.** O mesmo engine rodou em cinco shells — console, arquivo com JSON e SQLite, web API, browser, Roblox — e o `Building`, o `ResourceLedger` e o `Kingdom` nunca mudaram.

</details>

## A ideia principal, uma última vez

> **O modelo dura. Onde ele roda é só um detalhe.**
>
> Você provou isso cinco vezes — console, salvando em arquivo, web API, browser, Roblox. O mesmo `Building`, o mesmo `ResourceLedger`, o mesmo `Kingdom.AdvanceDay`. Lugares diferentes para rodar. Shells diferentes. O mesmo modelo todas as vezes.
>
> **Você é um programador.** Passado; futuro; os dois.

## Fechamento

1. **Quiz** — abra o `quiz.md` — o último. Anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 5.8 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 5.8 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre o dia de hoje, mais a URL do commit.

O Módulo 0.1 tem o porquê e os passos do painel/CLI se você precisar relembrar. Leve ao próximo sync semanal as respostas do quiz de que você tiver menos certeza.

## Próximo

Não há próximo módulo obrigatório. **Você terminou.**

Se quiser faixas bônus:

- **Fase 6 / B1** — como bancos de dados funcionam por dentro (construa um banco de dados pequeno do zero). Cerca de dez semanas.
- **Fase 6 / B2** — context engineering (um guia profundo e prático sobre programação assistida por IA). Cerca de dez semanas.

Se quiser iniciar o seu próprio projeto: a Fase 0 de qualquer coisa funciona. Construa a menor versão interessante, publique, depois melhore. Você já conhece o loop.

De qualquer forma: muito bem.
