# Módulo 2.11 — Nomes que Valem o Espaço (fechamento M3)

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

O reino agora tem 35 testes de engine, JSON, SQLite, EF Core, migrations, save slots e uma UI interativa. O código funciona. Hoje é sobre uma habilidade mais profunda: uma passagem cuidadosa pelos nomes de tudo que você escreveu nesta fase. É o que transforma *"código que funciona"* em *"código que qualquer um pode ler seis meses depois — incluindo você-do-futuro."*

> **Words to watch**
>
> - **rename party** — uma sessão focada que faz *só* renomeações, nada mais
> - **scope of a name** — até onde um nome alcança: local (5 linhas) → método (50) → classe (500) → módulo → repositório → o mundo inteiro. Quanto maior o alcance, mais trabalho o nome tem que fazer.
> - **noise word** — palavras genéricas como `Manager`, `Helper`, `Util`, `Data`, `Info` que não te dizem o que a coisa realmente é
> - **earns its keep** — todo nome que você mantém deve estar fazendo trabalho real para o leitor

---

## Por que um módulo separado para nomeação

Nomes são a sua documentação. São a primeira coisa que um leitor vê. Um nome bom torna o código ao redor claro. Um nome fraco te força a ler o corpo inteiro só para entender onde é usado. Nomes ruins se acumulam — toda linha que chama `ProcessData(d)` não te ensina nada sobre o que ela faz.

Uma *rename party* — fazendo só renomeações em uma sessão focada — funciona bem porque:

- IDEs modernas tornam renomeações seguras (Refactor → Rename, F2 no VS / Rider).
- Um PR só-de-renomeação é fácil de revisar, porque não há mudanças de lógica misturadas.
- Uma sessão só pega nomes relacionados que deveriam mudar juntos (`KingdomData` → `KingdomEntity`, mais `ToData()` → `ToEntity()`).

Você vai fazer este exercício uma vez por grande bloco de trabalho. Depois de três ou quatro rodadas, os seus nomes de primeira tentativa começam a sair certos mais frequentemente.

## As cinco perguntas

Para cada nome público no seu engine, persistência e console, pergunte:

1. **Ele diz o que a coisa *é*?** `Buffer` (um buffer de quê?) vs `KingdomSnapshotJson` (um snapshot JSON de um reino).
2. **Um leitor novo conseguiria adivinhar o que ela faz só pelo nome?** `ToSummary()` vs `Convert()`.
3. **O nome tem o tamanho certo para o seu escopo?** Um nome de 3 letras está ótimo para um método de 5 linhas (`b` para prédio). Uma classe merece a forma completa (`KingdomEntity`).
4. **Tem uma noise word?** `KingdomManager` — gerenciador de *quê*? Remova ou substitua.
5. **Ele bate com os nomes ao lado?** Se você já tem `Save` e `Load`, o terceiro método deve ser `Delete`, não `Remove`. Escolha um conjunto de palavras e fique com ele.

## Exemplo — um caso do seu código

Olhe para `KingdomEfStore`:

```csharp
public int Save(Kingdom k);
public Kingdom Load(int id, IRandom rng, IClock clock);
public void Update(int id, Kingdom k);
public void Delete(int id);
public IReadOnlyList<KingdomEntity> ListAll();
public IReadOnlyList<KingdomSlotInfo> ListSlots();
```

Seis métodos. Leia de novo com as perguntas:

- `Save` / `Load` / `Update` / `Delete` — as palavras CRUD padrão. Elas batem.
- `ListAll` *vs* `ListSlots` — *os dois listam coisas*. O primeiro retorna entities completas; o segundo retorna informações pequenas e leves. Os dois nomes começam com `List`, o que é confuso, e `ListAll` não é honesto (ele não retorna tudo de verdade; não carrega os prédios relacionados). Melhor:
  - `ListSlots()` fica.
  - `ListAll()` → ou apague (é usado só em testes, e você pode substituir por `using var ctx; ctx.Kingdoms.ToList();`) ou renomeie para `ListAllEntities()` para ser honesto sobre o que retorna.

Vamos decidir: remova `ListAll`. Os testes não precisam realmente dele, e `ListSlots` cobre os métodos públicos.

Essa é uma renomeação real com uma razão por trás — não só "por estilo." Depois, o codebase tem *um nome a menos para aprender*.

## Exemplo — segundo caso

Olhe para `KingdomEntity`. Considere:

- `KingdomEntity.cs` e `BuildingEntity.cs` — claro: são entities EF (DTOs).
- `KingdomSnapshot.cs` (no engine) e `BuildingSnapshot.cs` — os dois são formas só de dados. Então por que dois nomes diferentes?
  - `*Snapshot` é a forma de dados do engine (usada pelo JSON no Módulo 2.3).
  - `*Entity` é a forma de dados do EF (usada pelo SQLite do Módulo 2.6 em diante).
  - **Eles não são a mesma coisa** — o snapshot tem `Kind` e `Citizens[]`; a entity tem propriedades de navegação. Formas diferentes para jeitos diferentes de salvar. Os dois nomes valem o espaço.

Às vezes a resposta certa é não renomear nada.

## O exercício — faça de verdade no seu repositório

Em uma sessão focada de 30 minutos:

1. Abra o projeto do engine. Percorra cada tipo e método público. Faça as cinco perguntas sobre cada um.
2. Faça o mesmo para a persistência.
3. Faça o mesmo para o console.
4. Use a **refatoração Rename do IDE** (F2 no VS / Rider; Ctrl+Shift+R em outros). Ela atualiza todo lugar onde o nome é usado — testes, comentários e referências de doc XML — tudo de uma vez. Nunca faça uma renomeação com buscar-e-substituir na mão; você vai perder algum uso em algum lugar.
5. Depois de cada renomeação: `dotnet build` (deve continuar com 0 erros) e `dotnet test` (deve continuar com 71 passando).
6. **Faça commit depois de *cada* renomeação**, com o prefixo `[M3-rename]` — por exemplo, *"[M3-rename] drop KingdomEfStore.ListAll (callers used ListSlots; redundant)"*. (Painel do Source Control → stage → commit → Sync. Ou no terminal: `git commit -am "[M3-rename] ..." && git push`.) Commits pequenos são fáceis de revisar, e fáceis de desfazer se uma renomeação acabar errada.

## Renomeações comuns que você pode fazer

- `_eventEngine` → `_events` (um campo privado; um nome curto está bem para um escopo curto)
- `KingdomDbContext.Kingdoms` → está bem; esse é o padrão de plural normal para DbSet do EF
- `KingdomEfStore.EnsureCreated()` → ainda é um nome bom, mesmo que agora chame `.Migrate()`; o que ele promete fazer não mudou
- `KingdomSummary.BuildingCount` → está bem; ele diz exatamente o que é
- Qualquer classe terminando em `Manager`, `Helper`, `Util` ou `Data` — olhe com cuidado
- Qualquer propriedade chamada `Info` — info sobre quê? Seja específico.

## Delta starter

Este módulo inclui só:

- `M3-rename-checklist.md` — uma lista de checkbox de alvos comuns de renomeação no *seu* repositório
- `wins.md.append.md` — um parágrafo de entrada M3 para o seu log de wins

Não há mudança de código para adicionar — as renomeações que você faz são **específicas ao seu código**.

## Ritual passo a passo (fechamento M3)

1. Rode a rename party contra o seu repositório.
2. `dotnet build` — 0 erros.
3. `dotnet test` — ainda 71 passando.
4. Adicione a entrada M3 ao `wins.md`.
5. Poste o seu antes/depois no `#wins` no Slack.
6. **Marque o marco.** Este é só pela CLI — o painel não tem um botão para tags:

   ```powershell
   git tag m3-phase-2-complete
   git push origin m3-phase-2-complete
   ```

7. **Abra o PR do M3.** No github.com → seu repositório `kingdom` → o banner *"phase-2 had recent pushes — Compare & pull request"* (ou *Pull requests → New pull request*, base `main`, compare `phase-2`). Título: `M3 — Phase 2 — Persistence`. Corpo: os bullets do `wins.md` deste marco, mais `**Reviewer:** @dingdonglars`. Lars revisa, depois aprova. Você faz o Merge, e apaga a branch `phase-2` quando perguntar. Depois, localmente: `git switch main && git pull`. (Passo a passo completo: Módulo 1.10.)

## Mexa um pouco

Leia a sua mensagem de commit mais recente. Ela é específica? *"refactor"* não diz nada; *"drop ListAll, keep ListSlots — same callers, fewer methods"* conta a história.

Escolha a coisa pior nomeada no seu repositório (você vai saber qual é). Renomeie. Note como o código fica mais fácil de ler em todo lugar onde é usado.

Tente o oposto — renomeie `Kingdom` para `K` em todo lugar. Salve numa branch. Leia o seu código com esse nome. Note como é muito mais difícil de seguir. Nomes longos valem o espaço quando o escopo é grande.

Leia [`Naming Things`](https://en.wikipedia.org/wiki/Naming_(parameter)) no seu tempo livre. Os ensaios conhecidos sobre nomeação continuam úteis por anos.

## O que você acabou de fazer

Você percorreu o codebase e tornou cada nome público mais preciso. Alguns você renomeou, alguns você removeu, e alguns você deixou como estavam depois de entender por que estavam certos. O código não ficou mais inteligente — mas um leitor futuro vai se deparar com muito menos surpresas. Você também fechou o M3: testes ainda 71 passando, salvar funciona em quatro shells (texto, JSON, SQLite, EF Core), e o reino se lembra entre sessões. Esse é o marco. Não pule os passos que seguem.

**Conceitos que você já sabe nomear:**

- **rename party** — uma sessão focada só de renomeações
- **scope of a name** — curto para escopo curto; longo para escopo longo
- **noise word** — `Manager`/`Helper`/`Util`/`Info` para revisar com cuidado
- **disciplina de vocabulário** — escolha `Save`/`Load`/`Delete`, não uma mistura
- **IDE Rename** — o único jeito seguro de mudar um nome em todo lugar

## Por sua conta

Hora de fechar o livro. Não role de volta para as cinco perguntas — mostre para você mesmo, da sua própria cabeça, que a ideia grande pegou: um nome bom diz o que a coisa é, e quanto mais amplo o seu alcance, mais trabalho ele tem que fazer. Ninguém corrige isso — é só para você. É o jeito mais rápido de descobrir o que ainda não pegou, enquanto ainda é pequeno para consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Aqui estão três nomes fracos. Para cada um, escolha um nome melhor e diga em uma frase *por que* é melhor:

1. uma classe chamada `DataManager` que carrega e salva jogadores
2. um método chamado `Process(p)` que transforma um jogador num record de save
3. um método chamado `DoIt()` que apaga um slot de save

<details><summary>Travou? Abra aqui para conferir.</summary>

Não há uma única resposta certa — nomes são um julgamento. Mas boas respostas parecem com estas:

- `DataManager` → `PlayerStore` (ou `PlayerRepository`). *Por quê:* `Manager` é uma noise word que não diz nada; `Store` diz o que ele faz — armazena jogadores.
- `Process(p)` → `ToSaveRecord(p)` (ou `ToSnapshot`). *Por quê:* `Process` poderia significar qualquer coisa; o novo nome diz o que ele retorna.
- `DoIt()` → `DeleteSlot()`. *Por quê:* um nome deve dizer o que acontece; e se a classe já tem `Save`/`Load`, `Delete` bate com as palavras ao redor.

O padrão em cada resposta: remova noise words, diga o que a coisa *é* ou *faz*, e bata com as palavras já ao redor.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md` (mais leve que o normal — com tema de nomeação). Anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 2.11 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 2.11 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve as respostas do quiz de que você tiver menos certeza para a próxima conversa semanal.

---

> **Você acabou de terminar o M3.** Hora do ritual:
>
> 1. **Atualização do README** — percorra de novo as quatro seções do Módulo 0.4 (*o que / como rodar / o que aprendi / o que vem a seguir*). Desde o fechamento do M2, você adicionou persistência em quatro backends e um seletor de save slots; *Como rodar* e *O que aprendi* precisam de um parágrafo que não existia antes. Polir o README é uma disciplina de marco — todo fechamento de marco a partir daqui volta para ele.
> 2. **Entrada no `journal/wins.md`** — um parágrafo com as suas próprias palavras sobre o que mudou entre M2 e M3. Inclua a contagem de testes, os quatro backends de save, o seletor de slots.
> 3. **Post no `#wins` do Slack** — link para o PR + uma captura de tela curta ou captura do terminal, e uma linha: *"Kingdom v2 — Persistido. Salva, fecha, reabre, ainda está lá."*
> 4. **Uma linha de antes/depois** — *"Há algumas semanas meu reino morria ao fechar. Hoje ele sobrevive entre sessões, com save slots."*
> 5. **Marque** — `git tag m3-phase-2-complete` depois `git push origin m3-phase-2-complete`. (Só pela CLI — o painel não tem um botão para tags.)
>
> Depois tire o resto do dia de folga.

## Entrada do log de wins da Fase 2 (modelo)

```markdown
## M3 — Phase 2 — Persistence

- 71 testes, determinísticos, em engine + persistência
- O mesmo engine agora salvável de quatro formas: arquivo de texto, JSON, SQLite bruto, EF Core
- UI real de save slots; você pode jogar entre sessões

Antes: `Console.WriteLine($"Day {kingdom.Day}");` e o reino morre ao fechar
Depois: Salva, fecha, reabre dias depois — seu reino está exatamente onde você deixou

Postado no #wins em YYYY-MM-DD.
```

## Próximo

**A Fase 3 começa.** Ela apresenta a **web API** — o seu engine, servido por HTTP. O mesmo engine de novo, um quarto shell. O navegador chega na Fase 4, e o AI Unlock acontece no final da Fase 3.
