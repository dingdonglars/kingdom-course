# Rodando o seu projeto — a única regra que te salva

> Tente primeiro o `running-your-project.md` em inglês. Use este aqui só quando uma palavra te travar.

> Leia uma vez. Volte aqui sempre que o *Run* ou o depurador não se comportar.
>
> **Setup assumido:** Windows + VS Code, seu repositório `kingdom` em `C:\code\kingdom`.

Há um hábito que faz rodar e depurar "simplesmente funcionar", e pulá-lo é o jeito número um de um iniciante ficar travado meia hora em algo que nem é um bug. O hábito tem duas formas, dependendo de onde você está no curso — mas são na verdade a mesma ideia: **abra a única coisa em que você está trabalhando, e nada mais.**

## Fase 0 (Spark Week): um *programa*, uma janela

Na Spark Week você constrói vários programas pequenos e separados. Cada um mora na sua própria pasta dentro de `kingdom`:

```
C:\code\kingdom\
├─ RoastOMatic\        <- um programa (tem seu próprio .csproj)
├─ TavernTab\          <- um programa (tem seu próprio .csproj)
├─ QuestBoard\         <- um programa (tem seu próprio .csproj)
└─ journal\            <- não é um programa, só suas notas
```

**Quando você trabalha em um deles, abra *a pasta daquele programa* como a janela — não a pasta `kingdom` inteira.** Para trabalhar no `QuestBoard`, a janela do VS Code precisa ser **`C:\code\kingdom\QuestBoard`**.

**Crie um novo** (você faz isso no começo de cada checkpoint da Spark Week):

```powershell
cd C:\code\kingdom
dotnet new console -o QuestBoard
```

Depois **File → Open Folder…** → `C:\code\kingdom\QuestBoard`. A barra de título deve dizer **QuestBoard**. Rode com um `dotnet run` simples (só um programa à vista, então não precisa de palavras extras), e depure com **F5**.

## Fase 1 em diante (o Reino): uma *solution*, uma janela

A partir do Módulo 1.0, você para de construir programinhas separados e constrói **uma** coisa — o Reino. Ele mora na sua própria pasta, `kingdom-game`, como uma **solution**: um container que guarda vários projetos (o engine, o console, depois a web e os testes).

```
C:\code\kingdom\
├─ (seus brinquedos da Spark Week, deixados em paz)
└─ kingdom-game\          <- abra ISTO, e mantenha aberto o ano todo
   ├─ Kingdom.slnx        <- a solution
   ├─ Kingdom.Console\    <- o programa que você roda
   ├─ Kingdom.Engine\     <- as regras (adicionado no 1.2)
   └─ tests\              <- seus testes (adicionado no 1.3)
```

**Abra `C:\code\kingdom\kingdom-game` como a janela — uma vez — e fique nela.** Você *não* fica pulando para dentro e para fora das pastas `Kingdom.Console` e `Kingdom.Engine`. O Reino é uma coisa só, então é uma janela só. A barra de título deve dizer **kingdom-game**.

**Rode** nomeando o projeto que você quer:

```powershell
dotnet run --project Kingdom.Console
```

A parte `--project Kingdom.Console` diz *qual* programa rodar. Você o nomeia porque a solution guarda mais de um projeto — e a partir da Fase 3 ela vai guardar mais de um programa que você *pode* rodar (o console e a web API). Nomeá-lo nunca é ambíguo, então é o hábito a manter.

**Rode seus testes** (a partir do Módulo 1.3) do terminal da mesma janela:

```powershell
dotnet test
```

Isso compila a solution inteira e roda todos os testes dela. (O painel Test Explorer à esquerda também os mostra, se você preferir clicar.)

**Depure** com **F5**: abra `Kingdom.Console/Program.cs`, ponha um ponto de parada vermelho à esquerda de uma linha, aperte F5. Enquanto o console for o único programa que pode rodar (Fases 1–2), o F5 o inicia sem complicação. Na primeira vez, o VS Code pode pedir para você escolher o alvo de execução uma vez — escolha `Kingdom.Console`.

## Quando o *Run* ou o F5 não se comportam — verifique isto primeiro

| O que você vê | O que significa | O conserto |
|---|---|---|
| `dotnet run` diz *"more than one project"* | **Fase 0:** você abriu a pasta `kingdom` inteira, não a pasta de um programa | Abra a pasta do único programa |
| `dotnet run` diz *"more than one project"* | **Fase 1+:** você não nomeou o projeto | Adicione `--project Kingdom.Console` (ou o projeto que você quer) |
| **F5** não faz nada, ou pede para "select a project" | Programas demais à vista, ou nenhum alvo de execução escolhido | Fase 0: abra a pasta do único programa. Fase 1+: escolha `Kingdom.Console` quando perguntar |
| A barra de título mostra **kingdom**, não seu programa nem **kingdom-game** | O repositório inteiro está aberto | **File → Open Folder…** → a pasta certa para a sua fase |

Nove em cada dez vezes, "o depurador está quebrado" é na verdade "a pasta errada está aberta." Olhe a barra de título antes de qualquer coisa.

## O único detalhe — o journal

Suas notas em `journal\` (`progress.md`, `quiz-notes.md`, `wins.md`) ficam no **topo** de `kingdom`, não dentro do seu programa nem de `kingdom-game`. Boa notícia: de qualquer uma dessas janelas o VS Code olha *para cima*, acha o repositório `kingdom`, e o **painel Source Control ainda mostra o repositório inteiro** — então você pode editar os arquivos de `journal/` e commitá-los direto do painel como sempre. (Os arquivos do journal só não vão aparecer na árvore de arquivos à esquerda, porque estão acima da pasta que você abriu.)

Se você preferir vê-los na árvore para um fechamento, ou:

- **Commite pelo terminal** — ele vê o repositório inteiro:
  ```powershell
  git add .
  git commit -m "Module 1.1 done"
  git push
  ```
- **Ou reabra a janela `kingdom`** rapidinho, edite seus arquivos de journal, depois commite e Sync pelo painel Source Control.

Qualquer um serve. Todo o resto — compilar, rodar, depurar, testar — fica na sua única janela apropriada para a fase.
