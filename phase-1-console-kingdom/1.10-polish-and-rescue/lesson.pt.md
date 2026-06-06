# Módulo 1.10 — Polish and Repo Rescue (M2 close)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

> **Aquecimento — 30 segundos, de cabeça.** Antes de hoje, traga de volta o que você construiu no Módulo 1.9:
>
> 1. Você moveu classes para pastas como `Buildings/` e `Resources/`. O que tem que bater com cada pasta?
> 2. Qual classe é a *aggregate root*, e o que isso significa?
>
> Inseguro em alguma? Releia primeiro o **Módulo 1.9** — a aula de hoje fica bem em cima dele. Leve qualquer coisa que ficou frágil para o sync semanal.

A Fase 1 termina hoje. O reino é um engine de verdade — trinta e cinco testes, determinístico, organizado por área. Agora a gente o deixa mais bonito. Um README melhor, alguns comentários bem colocados, e os nomes que você desejou ter escolhido da primeira vez. Depois cobrimos o **fluxo de resgate de repositório** — os movimentos para saber no dia em que sua branch git estiver uma bagunça completa.

Não há código novo grande nesta aula. É um dia calmo de propósito. Fechar um marco parece assim: leia seu próprio repositório do começo ao fim, faça as correções pequenas, escreva um README curto que vai te ajudar mais tarde, depois marque o momento com os passos do marco no fim.

> **Words to watch**
>
> - **README** — o documento principal no topo de um repositório. Quatro seções que sempre importam: *o que / como rodar / o que você aprendeu / o que vem a seguir.*
> - **XML doc comment** — comentários `///` acima de um tipo ou método público. O editor os mostra como tooltips e no IntelliSense.
> - **stash** — `git stash` — guarda mudanças que você não commitou para depois, deixando a working tree limpa
> - **rescue** — quando sua branch está uma bagunça: salve uma cópia, resete, depois recupere só o que você queria

---

## Polish

### 1. README na raiz do repositório

O README é a primeira coisa que as pessoas veem quando abrem o repositório. É a primeira coisa que um estranho lê no GitHub — e a primeira coisa que *você* lê seis meses de agora, quando tiver esquecido como este projeto roda. Um bom README responde quatro perguntas antes que alguém precise perguntar: *o que é isso, como eu rodo, o que aprendi construindo, o que vem depois.* Quatro seções curtas são melhores do que um parágrafo longo, e você vai ficar feliz mais tarde por tê-los escrito.

Quatro seções, nessa ordem:

```markdown
# Kingdom

A console kingdom-management game. Phase 1 of the Kingdom Curriculum.

## Run it

\`\`\`powershell
dotnet run --project Kingdom.Console
\`\`\`

Output: Eldoria runs for 30 days, prints the final state and a deterministic event log.

## What I learned

- Engine vs shell — the engine never knows about Console
- Inheritance — Farm/Lumberyard/Mine override `Building.Tick`
- LINQ — `.OfType<>().Count()` instead of manual `for` loops
- Interfaces and dependency injection — `IRandom`/`IClock` make the engine testable
- FakeItEasy — exact control of dependencies in tests
- Sub-namespaces — `Kingdom.Engine.Buildings` and friends

## What's next

- Phase 2 (M3): persistence — save/load to a file, then SQLite
- Phase 3 (M4): web API — same engine, HTTP shell
```

### 2. Comentários de doc XML nos tipos públicos

Alguns comentários `///` acima dos tipos públicos. Pule campos privados e métodos óbvios — um comentário em cada property só adiciona bagunça. Comente o *porquê*, e as coisas que não são óbvias.

```csharp
/// <summary>
/// The aggregate root of the kingdom. Owns buildings, citizens, resources,
/// and the event log. Advance one tick at a time via <see cref="AdvanceDay"/>.
/// </summary>
public class Kingdom { ... }

/// <summary>Random number source. Production: <see cref="SystemRandom"/>. Tests: a FakeItEasy fake.</summary>
public interface IRandom { ... }
```

O `<see cref="..."/>` vira um link clicável no editor. Útil, mas não adicione muitos.

### 3. Passagem de nomes

Olhe para cada nome público. Poderia ser mais claro?

`RollOnce` está bem — rola uma vez. `Snapshot()` no `ResourceLedger` retorna um dicionário só de leitura — bem. `_eventEngine` (campo privado) — bem. Se você encontrar um nome antigo que não encaixa mais, renomeie. Os editores modernos tornam fácil renomear em todo o projeto e nos testes de uma vez.

### 4. Seção Tinker no README

Adicione três linhas em "What's next":

> Stretch ideas if you want to explore further before Phase 2:
> - Add a `Quarry` building (marble?)
> - Add a `Mood` enum to `Citizen` and an event `CitizenHappy`
> - Print a CSV of the event log at the end

Essas são ideias opcionais, não trabalho obrigatório.

---

## Resgate de repositório

Às vezes sua árvore git está uma bagunça: commits pela metade, uma branch que foi pelo seu próprio caminho, edições que você não quer. Saber como *resgatar* um estado funcionando economiza horas.

### Cenário A — mudanças sem commit que você não quer

```powershell
git status     # confirm what's modified
git diff       # review one last time
git stash      # set them aside (recover later with: git stash pop)
# OR
git restore .  # throw them away (irreversible!)
```

### Cenário B — trabalho commitado na branch errada

```powershell
# You committed to main, but it should be on a feature branch.
git log --oneline -5
git branch feature/my-thing       # save the commit on a new branch
git reset --hard origin/main      # rewind main
git checkout feature/my-thing     # continue on the right branch
```

### Cenário C — sua branch local está quebrada; o remoto está bom

```powershell
git fetch origin
git reset --hard origin/main      # warning: discards ALL local work, matches remote
```

> ⚠ `git reset --hard` é **destrutivo**. Sempre faça `git stash` ou commit-then-reset primeiro se você puder querer algo de volta.

### Cenário D — você quer só algumas das suas mudanças

```powershell
git diff HEAD                      # review everything that's changed
git add -p                         # interactive: stage hunks one by one
git commit -m "feat: just the bits I wanted"
git stash                          # set aside the rest for later
```

### Cenário E — resgate total: cherry-pick os commits bons para uma branch limpa

```powershell
git log --oneline                  # find the commit hashes you want to keep
git checkout main
git pull
git checkout -b feature/clean
git cherry-pick <hash1> <hash2>    # reapply just those commits
```

### A regra do resgate

> **Se você não sabe o que `git status` e `git log --oneline -10` dizem, pare. Leia. Depois decida.**

A maioria dos acidentes com git é *"eu não sabia de verdade em que estado estava, então tentei algo e piorou."* Ler o estado primeiro é a maior parte do resgate.

---

## Percorra seu repositório

Não há código novo hoje. Percorra *seu* repositório:

1. Verifique o README na raiz. Edite-o até combinar com o modelo de quatro seções acima.
2. Adicione comentários de doc `<summary>` para `Kingdom`, `IRandom`, `IClock`, `EventEngine` (os quatro tipos mais públicos). Pule o resto.
3. `dotnet build` — deve ainda ser 0 erros.
4. `dotnet test` — deve ainda ser 35 passando.
5. **Faça commit do polish.** *"[M2] polish: README + doc comments"*. (Painel Source Control → prepare → commit → Sync. Ou CLI: `git add . && git commit -m "[M2] polish: README + doc comments" && git push`.)

## Mexa um pouco

Rode `dotnet build /verbosity:diagnostic`. É muita saída, mas leia por cima uma vez só para ver o quanto o build faz por você.

Rode `dotnet --list-sdks` para verificar que você está no .NET 10.

Rode `git log --oneline --graph --decorate -20`. Você vai ver seus últimos vinte commits como uma árvore. O hábito de fazer commits pequenos agora aparece claramente na imagem.

## A linha-guia

A linha-guia neste módulo: **o repositório é o que você mantém**. Arrume o código, arrume o README, arrume o log de commits. O reino que você vai ficar feliz de olhar de volta em um ano é o que está organizado, não só o que funciona.

## O que você acabou de fazer

A Fase 1 fecha hoje. Você escreveu um README de quatro seções, adicionou comentários de doc para quatro tipos públicos, verificou seus próprios nomes uma vez, e aprendeu os cinco movimentos de resgate que você vai precisar em algum dia comum quando sua árvore git estiver uma bagunça. O código não mudou de verdade — os mesmos trinta e cinco testes ainda passam — e esse é o ponto. Terminar bem é uma habilidade por si só.

**Conceitos que você já sabe nomear:**

- **README quatro seções** — o que, como rodar, aprendido, próximo
- **Comentário de doc XML** — `///` nos tipos públicos, usado com moderação
- **`git stash`** — guarda trabalho com segurança sem perdê-lo
- **`git reset --hard`** — joga trabalho fora, útil quando você tem certeza
- **a regra do resgate** — leia o estado antes de agir sobre ele

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — e não abra nenhuma das aulas anteriores também. Esta olha de volta para toda a Fase 1 e pede que você diga a única grande ideia com suas palavras. Ninguém corrige isto — é só para você. Este é um desafio — cobre seis semanas, não uma aula — então travar é completamente normal. Travar é o ponto: ele mostra o que vale uma segunda olhada antes da Fase 2.

Sem buscar nada, responda com suas próprias palavras, em voz alta ou no papel:

1. **A Fase 1 tem o nome de uma regra. Qual é ela, e por que ela torna o engine fácil de testar?**
2. Nomeie as duas coisas que o engine recebe pelo seu constructor para que nunca precise criá-las por conta própria.

<details><summary>Travou? Abra aqui para conferir.</summary>

- A regra: **o engine guarda as regras do reino e nunca fala com o mundo exterior; a shell (console, web, Roblox) fala com o exterior e usa o engine.** A dependência aponta em um jeito só — shell → engine.
- Isso torna o engine **fácil de testar** porque o engine não cria nenhum dos seus ajudantes externos por conta própria. Eles chegam pelo constructor, então um teste pode entregar ao engine versões falsas que ele controla — como dados que sempre rolam o número que você escolhe.
- As duas que o engine recebe: **`IRandom`** (os dados) e **`IClock`** (o relógio). O console dá a ele o `SystemRandom` e `SystemClock` reais; os testes dão fakes.

Se qualquer parte disso pareceu instável, essa é a coisa para dar uma olhada — Módulo 1.2 para a regra, Módulo 1.8 para as dependências.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md` (mais leve que o normal — com tema do marco). Anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 1.10 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 1.10 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos do painel/CLI se você precisar relembrar. Leve as respostas do quiz das quais você tiver menos certeza para a próxima conversa semanal.

---

## O checkpoint M2 — mostre ao Lars

A Fase 1 é a maior coisa que você já construiu. Antes de ela entrar no `main`, você e o Lars sentam por cerca de quinze minutos e você o guia por ela — a mesma ideia amigável do checkpoint da metade depois do 1.5, e dos checkpoints lá na Fase 0. Dizer em voz alta é o jeito mais seguro de saber que a fase *inteira* pegou, não só que os testes ficaram verdes.

Este é o checkpoint para a Fase 2: o pull request abaixo entra no merge *depois* desta conversa, não antes. Não é uma prova que você possa reprovar — se algo estiver confuso, vocês dois acham juntos e escolhem o único módulo para revisar. Mas a Fase 2 se empilha direto em cima da Fase 1, então vale a pena estar firme primeiro.

Tenha o projeto aberto e rodando. O Lars vai pedir para você:

1. **Rodar o reino** por trinta dias e ler o estado final e o event log em voz alta.
2. **A grande ideia.** Abra um arquivo do engine e um arquivo da shell, e explique com suas próprias palavras por que o engine nunca fala com o exterior — e como essa única regra deixa o *mesmo* engine rodar num console agora, num browser na Fase 4, e no Roblox na Fase 5.
3. **O engine testável.** Mostre `IRandom` e `IClock` chegando pelo construtor, e explique como entregar um *fake* ao engine faz um teste dar a mesma resposta toda vez. Esta é a ideia em que os testes de salvar/carregar da Fase 2 se apoiam.
4. **Uma mudança ao vivo** à escolha dele — digamos, adicionar um prédio `Quarry`, ou uma nova consulta LINQ sobre os prédios.

Quando a explicação terminar e o Lars estiver satisfeito, ele aprova o pull request e você faz o merge. Esse é o checkpoint vencido — rumo à Fase 2.

## Abra o PR do marco

Você vem fazendo commits na branch `phase-1` desde o Módulo 1.0. Agora é hora de enviar toda a fase para Lars como um *pull request* — o lugar onde ele revisa seu trabalho — e fazer o merge para o `main`.

No github.com, abra seu repositório `kingdom`. Uma faixa amarela perto do topo diz algo como *"phase-1 had recent pushes — Compare & pull request"*. Clique em **Compare & pull request**. (Sem faixa? Vá para a aba *Pull requests* → *New pull request* → base: `main`, compare: `phase-1`.)

Preencha:

- **Title:** `M2 — Phase 1 — Console Kingdom`
- **Body:** os quatro pontos do `wins.md` mais uma linha `**Reviewer:** @dingdonglars`

Clique em **Create pull request**. O GitHub avisa Lars; ele revisa na aba *Files changed*, deixa comentários ou clica em **Approve**. Ele aprova *depois* do checkpoint M2 acima — a explicação faz parte da revisão, então faça isso primeiro. Se ele pedir mudanças, faça push de mais commits para `phase-1` — eles aparecem no pull request automaticamente. Quando a revisão estiver **Approved**, clique em **Merge pull request** → **Confirm merge**. O GitHub oferece deletar a branch `phase-1` — aceite; o histórico do merge agora vive no `main`.

Volte para o local:

```powershell
git switch main
git pull
```

Você está de volta no `main` com a Fase 1 integrada. A Fase 2 começa no Módulo 2.1 com uma nova branch `phase-2`.

---

## Ritual do marco — M2

Você acabou de terminar o **M2 — Kingdom v1, Console**. Hora dos passos do marco:

1. **Entrada no `journal/wins.md`.** Abra `wins.md` (na pasta `journal/` do seu repositório) e adicione um parágrafo, com suas palavras, sobre como o M2 pareceu. Mantenha curto.

2. **Post no `#wins` do Slack.** Poste um screenshot do reino rodando, um link para o pull request com merge, e uma legenda de uma linha.

3. **Uma frase de antes/depois.** Escolha algo que você não sabia fazer seis semanas atrás e algo que você consegue fazer hoje, e coloque em uma frase. Salve-a no `wins.md`. Você vai ficar feliz mais tarde por tê-la escrito.

   Exemplo: *"Seis semanas atrás eu nunca tinha aberto um terminal. Hoje construí um engine de reino determinístico com trinta e cinco testes."*

Depois descanse o resto do dia.

## Próximo

**A Fase 2 começa.** A Fase 2 apresenta *persistência* (salvamento) — escreva o reino em um arquivo (JSON), depois no SQLite. Mesmo engine, uma shell nova — a shell de salvamento. A primeira prova de verdade de que dividir o engine da shell valeu a pena.
