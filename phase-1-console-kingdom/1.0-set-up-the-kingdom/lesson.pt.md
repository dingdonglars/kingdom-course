# Módulo 1.0 — Prepare o Reino

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

A Fase 1 começa hoje. Daqui para frente, algo muda: na Spark Week você construiu vários programas pequenos e separados — Roast-O-Matic, o Tavern Tab, e os outros. Cada um existia sozinho. Agora você começa a construir **uma** coisa — o Reino — e vai crescê-la o ano inteiro. No fim do curso, o mesmo Reino vai rodar no terminal, salvar em disco, servir uma página web e viver dentro de um jogo do Roblox.

Hoje é um dia calmo de preparação. Sem regras de jogo ainda — isso é no próximo módulo. Hoje você constrói a *casa* onde o Reino vai morar, aprende como vai abri-lo e rodá-lo daqui para frente, e faz ele dizer olá. Curto e tranquilo.

### Uma coisa, feita de várias partes

O Reino não vai ser um arquivo grande. Vão ser vários **projetos** — um para as regras, um para o programa que você roda, mais alguns depois para salvar e para a web. Para mantê-los juntos, eles moram dentro de uma **solution**: um container que guarda muitos projetos.

Pense numa casa. A solution é a casa; cada projeto é um cômodo. Hoje a casa tem um cômodo (o programa que você roda). Ao longo do ano você vai adicionar mais cômodos — mas ela continua sendo uma casa só, e você a mantém aberta em uma janela o tempo todo.

```text
   Solution Kingdom  (a casa — um arquivo: Kingdom.slnx)
     │
     └─ Kingdom.Console   (o primeiro cômodo: o programa que você roda)

        ... depois: Kingdom.Engine (as regras), testes,
            depois os cômodos Web e Roblox em suas próprias fases
```

> **Words to watch**
>
> - **solution** — um container que agrupa projetos relacionados para que eles compilem juntos. O arquivo dela termina em `.slnx`.
> - **project** — uma peça compilável (um programa ou uma biblioteca). Uma solution guarda um ou vários.
> - **`.slnx`** — o arquivo da solution. (Guias mais antigos na internet mostram `.sln` — mesma ideia, formato mais novo. Não se preocupe se vir os dois.)
> - **branch** — uma linha de trabalho separada no git. O trabalho da Fase 1 vai numa branch chamada `phase-1`.
> - **build** — transformar seu código em algo que o computador consegue rodar.

---

## Passo 1 — crie uma branch para o trabalho da Fase 1

Rode isto primeiro, antes de qualquer coisa:

```powershell
cd C:\code\kingdom
git switch -c phase-1
```

Você está agora numa *branch* chamada `phase-1`. Uma branch é uma linha de trabalho separada no git. De agora até o fim da Fase 1, seus commits vão para a `phase-1` em vez da `main`. No Módulo 1.10 você vai trazer todo esse trabalho de volta para a `main` por meio de um **pull request** — o lugar onde o Lars revisa uma fase inteira antes de ela entrar na `main`.

**Se "branch" e "pull request" parecerem confusos agora, isso é esperado** — é a primeira vez que você os encontra. O que você de fato faz é pouco: um comando no começo da fase (agora), e abrir um pull request no github.com no fim (o Módulo 1.10 te guia). A ideia para de parecer nebulosa depois de algumas semanas usando. O Módulo 1.8 volta às branches depois que você já tiver convivido com uma por um tempo.

Confirme que está nela:

```powershell
git status
```

A primeira linha deve dizer *"On branch phase-1"*.

## Passo 2 — dê ao Reino sua própria pasta

Seu repositório `kingdom` já guarda seus rascunhos da Spark Week. Para manter o Reino limpo e separado deles, ele ganha sua própria pasta:

```powershell
mkdir kingdom-game
cd kingdom-game
```

Tudo o que você construir pelo resto do ano mora aqui dentro. Seus brinquedos antigos ficam onde estão, fora do caminho.

## Passo 3 — crie a solution e seu primeiro projeto

Três comandos:

```powershell
dotnet new sln -n Kingdom
dotnet new console -n Kingdom.Console
dotnet sln add Kingdom.Console
```

Linha por linha:

- `dotnet new sln -n Kingdom` cria a **solution** — um arquivo chamado `Kingdom.slnx`. É a casa. Por enquanto está vazia.
- `dotnet new console -n Kingdom.Console` cria seu primeiro **projeto**: um programa de console numa pasta chamada `Kingdom.Console`. É o programa que você vai rodar.
- `dotnet sln add Kingdom.Console` coloca o projeto *dentro* da solution — move o cômodo para dentro da casa.

Sua pasta agora fica assim:

```text
C:\code\kingdom\kingdom-game\
├─ Kingdom.slnx              <- a solution (a casa)
└─ Kingdom.Console\          <- seu primeiro projeto (o programa)
   ├─ Kingdom.Console.csproj
   └─ Program.cs             <- uma linha de código, imprime "Hello, World!"
```

## Passo 4 — abra como sua janela (e mantenha aberta o ano todo)

**File → Open Folder…** → escolha `C:\code\kingdom\kingdom-game` → Open.

Esta é a janela que você mantém aberta pelo **resto do curso inteiro**. Você não fica pulando de pasta em pasta de projeto como fazia na Spark Week — o Reino é uma coisa só, então você abre uma vez e fica nela. A barra de título deve dizer **kingdom-game**.

> **Seu git continua funcionando.** Mesmo que a janela seja a pasta `kingdom-game`, o VS Code olha *para cima* e acha seu repositório `kingdom`, então o painel Source Control ainda diz **`kingdom`** e o Sync envia normalmente. Suas notas em `journal/` ficam lá em cima no repositório, e o painel Source Control as enxerga sem problema.

O guia completo de como rodar — e o que fazer se o Run ou o depurador algum dia se comportarem mal — fica no `running-your-project.md`. Leia uma vez; volte a ele sempre que algo estiver estranho.

## Passo 5 — rode

Abra um terminal (*View → Terminal*) e rode:

```powershell
dotnet run --project Kingdom.Console
```

Você deve ver:

```text
Hello, World!
```

É esse o ponto de hoje: a casa está construída, um cômodo está nela, e roda. A parte `--project Kingdom.Console` diz *qual* cômodo rodar. Você vai escrever esse mesmo comando (às vezes nomeando outro projeto) o ano inteiro, então vale a pena se acostumar agora.

Para **depurar** em vez disso, abra `Kingdom.Console/Program.cs`, clique à esquerda do número de uma linha para colocar um ponto de parada vermelho, e aperte **F5**. Por enquanto só há um programa que pode rodar, então o F5 sabe exatamente o que iniciar.

## Mexa um pouco

Abra `Kingdom.Console/Program.cs`. É uma linha: `Console.WriteLine("Hello, World!");`. Mude o texto para `Console.WriteLine("Eldoria desperta.");` e rode de novo. O programa é seu para mudar.

Abra o `Kingdom.slnx` no editor. É um pequeno trecho de XML que só lista os projetos da solution. Agora lista um. Você vai ver esse arquivo crescer conforme adiciona cômodos.

## O que você acabou de fazer

Você preparou a casa onde o seu Reino vai morar o ano inteiro. Criou uma branch `phase-1` para o trabalho da fase, deu ao Reino sua própria pasta `kingdom-game` para que seus brinquedos da Spark Week não o entulhem, e criou uma **solution** (`Kingdom.slnx`) com um **projeto** dentro dela (`Kingdom.Console`). Abriu essa pasta como sua única janela, rodou o programa com `dotnet run --project Kingdom.Console` e o viu imprimir. Sem lógica de jogo ainda — isso começa no próximo módulo — mas a oficina está pronta.

**Conceitos que você já sabe nomear:**

- **solution** — um container (`.slnx`) que agrupa projetos para compilarem juntos
- **project** — uma peça compilável; uma solution guarda um ou vários
- **branch** — uma linha de trabalho separada no git; a Fase 1 vive na `phase-1`
- **uma solution, uma janela** — abra `kingdom-game` uma vez e fique nela o ano todo
- **`dotnet run --project <nome>`** — roda um projeto nomeado da solution

## Por sua conta

Hora de fechar o livro. Não role de volta — prove para você mesmo, da sua própria cabeça, que a preparação pegou. Ninguém corrige isto — é só para você.

Sem olhar, responda em voz alta ou no papel:

1. Qual é a diferença entre uma **solution** e um **project**?
2. Qual pasta você abre no VS Code para trabalhar no Reino — e como isso é diferente de como você abria seus brinquedos da Spark Week?
3. Qual comando roda o programa, e o que a parte `--project` faz?

<details><summary>Travou? Abra aqui para conferir.</summary>

- Uma **solution** é o container (a casa, o arquivo `.slnx`); um **project** é uma peça compilável dentro dela (um cômodo). Uma solution pode guardar vários projetos.
- Você abre a **pasta `kingdom-game`** — o Reino inteiro — como uma janela, e a mantém aberta o ano todo. Na Spark Week cada brinquedo era separado, então você abria *a pasta de cada brinquedo*. O Reino é uma coisa só, então é uma janela só.
- `dotnet run --project Kingdom.Console` roda. `--project Kingdom.Console` diz *qual* projeto da solution rodar — útil quando a solution tiver mais de um programa que pode rodar.

Se alguma parte ficou frágil, dê uma olhada de volta no passo antes de seguir.

</details>

## Fechamento

1. **Progresso** — uma linha em `journal/progress.md`: `Module 1.0 — Set Up the Kingdom — DATE — fiz a solution e rodei o Hello. Learnt: uma frase.`
2. **Commit e push** — prepare seu trabalho, mensagem de commit `Module 1.0 done`, Sync. (Não há quiz num dia de preparação.)
3. **Poste no `#wins`** — uma linha: o Reino tem sua casa e roda. Adicione a URL do commit.

O Módulo 0.1 cobre o porquê e os passos do painel/CLI se você precisar relembrar como commitar.

## Próximo

O Módulo 1.1 enche o programa vazio com o primeiro código de verdade: quatro classes — `Resource`, `Building`, `Citizen`, `Kingdom` — que constroem um pequeno reino medieval e o imprimem no terminal. A oficina está pronta; agora você começa a construir.
