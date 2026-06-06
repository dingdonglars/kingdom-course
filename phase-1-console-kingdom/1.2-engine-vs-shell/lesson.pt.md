# Módulo 1.2 — Engine vs Shell

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

> **Aquecimento — 30 segundos, de cabeça.** Antes de hoje, traga de volta o que você construiu no Módulo 1.1:
>
> 1. Qual é a diferença entre uma *classe* e um *objeto*?
> 2. Por que deixar um campo `private` e acessá-lo por uma property — o que isso protege?
>
> Inseguro em alguma? Releia primeiro o **Módulo 1.1** — a aula de hoje fica bem em cima dele. Leve qualquer coisa que ficou frágil para o sync semanal.

Esta é a aula que dá nome ao resto do curso. Você pega o reino que escreveu no 1.1 — tudo em um projeto — e o divide em dois. As *regras* do reino (prédios, recursos, a matemática) se movem para uma class library chamada `Kingdom.Engine`. O programa que imprime coisas no terminal se torna um pequeno projeto `Kingdom.Console` que *usa* o engine. Mesmo código no fim, mas uma organização muito diferente. O ponto de hoje é ver por que essa organização importa antes que as fases posteriores peçam a você que a use.

Estamos apresentando uma palavra que você vai ver muito daqui para frente: **shell**. Uma shell é o que quer que fale com o mundo exterior — o console aqui, uma página web mais tarde no ano, Roblox depois disso. O engine nunca fala com o exterior. A shell sim. O engine só sabe sobre o reino. A shell sabe sobre pessoas.

Aqui está a forma de toda a ideia — um engine no meio, e ao longo do ano, várias shells ao redor:

```text
                  +------------------------------+
                  |        Kingdom.Engine        |
                  |  the rules: buildings,       |
                  |  citizens, resources, maths  |
                  |  (never prints, never asks)  |
                  +------------------------------+
                     ^            ^             ^
                     |            |             |
            +--------+---+   +----+-----+   +---+--------+
            |  Console   |   | Browser  |   |  Roblox    |
            | (Phase 1)  |   | (Phase 4)|   | (Phase 5)  |
            +------------+   +----------+   +------------+
              prints to        a web        a Luau
              the terminal     page         game
```

Mesmo engine, três shells. As setas significam "usa": cada shell *usa* o engine, nunca o contrário. O engine é a parte que você mantém o ano inteiro; cada shell é apenas uma forma de *jogar* o mesmo reino. É por isso que esta aula é de onde o curso tira o nome — acerte essa divisão hoje e as Fases 4 e 5 se tornam "escreva uma nova shell", não "reescreva tudo".

> **Words to watch**
>
> - **class library** — um projeto que compila para um `.dll`, não um `.exe`. Sem `Main`. Outros projetos o usam.
> - **engine** — a parte do código que é sobre o reino e suas regras
> - **shell** — a parte que fala com o mundo exterior (console, arquivos, rede, browser, Roblox)
> - **project reference** — um projeto dizendo *"eu dependo deste outro projeto"*
> - **solution** (`.slnx` ou `.sln`) — um arquivo que agrupa projetos relacionados para que sejam compilados juntos

---

## Por que dividir

No Módulo 1.1, suas classes `Building`, `Resource`, `Kingdom` e `Citizen` viviam no mesmo projeto que `Program.cs`. Funciona por enquanto. Mas pergunte a si mesmo: se você quisesse o mesmo reino em um site mais tarde no ano, o que faria? Você não pode reutilizar `Program.cs` — sites não têm console para imprimir. Você estaria copiando as classes do reino de um projeto para outro. A divisão de hoje impede que você precise copiar. O engine vira a lógica do reino. O console vira uma forma de *jogar* ele. A versão para browser da Fase 4 será uma forma diferente de jogar a mesma lógica. A versão Roblox da Fase 5 vai transformar a mesma lógica em Luau. O engine é a parte que você mantém em todas elas.

## Passo 1 — adicione o projeto engine

Sua solution (`Kingdom.slnx`) e seu projeto `Kingdom.Console` já existem desde o Módulo 1.0 — nada para desmontar. Hoje você adiciona **um** projeto novo, o engine, e aponta o console para ele. Aqui é para onde você vai:

```text
kingdom-game/
├─ Kingdom.Engine/                  ← class library (no Main, no Console)
│   ├─ Kingdom.cs
│   ├─ Building.cs
│   ├─ Citizen.cs
│   ├─ Resource.cs
│   ├─ ResourceLedger.cs            ← new — a class around the dictionary
│   └─ Kingdom.Engine.csproj
├─ Kingdom.Console/                 ← console app (no game logic)
│   ├─ Program.cs
│   └─ Kingdom.Console.csproj
└─ Kingdom.slnx                     ← ties them together
```

De dentro da pasta `kingdom-game`, rode três comandos:

```powershell
cd C:\code\kingdom\kingdom-game
dotnet new classlib -n Kingdom.Engine
dotnet sln add Kingdom.Engine
dotnet add Kingdom.Console reference Kingdom.Engine
```

- `dotnet new classlib -n Kingdom.Engine` cria o engine — uma **class library**, um projeto sem `Main`. Você não o roda sozinho; outros projetos o usam.
- `dotnet sln add Kingdom.Engine` o coloca na solution — um segundo cômodo na casa que você construiu no Módulo 1.0.
- A última linha é a importante. Ela escreve um `<ProjectReference>` dentro de `Kingdom.Console.csproj` que diz *"o projeto console depende do projeto engine."* O sistema de build lê essa linha, compila o engine primeiro, depois compila o console com o `.dll` do engine pronto para usar. A seta aponta em um só sentido: console → engine, nunca o contrário.

## Passo 2 — mova as classes para o Engine, e conheça o `namespace`

Mova os quatro arquivos de classe — `Building.cs`, `Citizen.cs`, `Resource.cs`, `Kingdom.cs` — para fora de `Kingdom.Console/` e para dentro de `Kingdom.Engine/`. (Apague também o `Class1.cs` vazio que o `dotnet new classlib` criou — você não precisa dele.)

No Módulo 1.1 esses arquivos não tinham linha de `namespace`, porque um projeto só não precisava de uma. Agora há dois projetos, então cada tipo precisa de um rótulo dizendo a que família ele pertence. Esse rótulo é um **namespace**. Adicione esta linha bem no topo de cada um dos quatro arquivos movidos:

```csharp
namespace Kingdom.Engine;
```

Então `Building.cs` agora começa assim:

```csharp
namespace Kingdom.Engine;

public class Building
{
    public string Name { get; }
    // ... o resto fica igual
}
```

Faça o mesmo para `Citizen.cs`, `Resource.cs` e `Kingdom.cs`. A regra é que o namespace combina com o projeto (e com a pasta) — a mesma ideia que você vai ver de novo no Módulo 1.9. Os tipos do engine agora vivem sob o rótulo `Kingdom.Engine`, e o console vai pedir por eles por esse nome no Passo 4.

## Passo 3 — apresente o `ResourceLedger`

O dicionário no `Kingdom` vai ganhar regras nos próximos módulos — se recusar a gastar mais do que você tem, se recusar quantias negativas, e assim por diante. Um dicionário simples não consegue fazer isso por conta própria; uma classe consegue. Então vamos colocá-lo dentro de uma classe agora, enquanto ainda é pequeno.

`Kingdom.Engine/ResourceLedger.cs`:

```csharp
namespace Kingdom.Engine;

public class ResourceLedger
{
    private readonly Dictionary<Resource, int> _amounts = new();

    public ResourceLedger()
    {
        foreach (Resource r in Enum.GetValues<Resource>())
            _amounts[r] = 0;
    }

    public int Get(Resource r) => _amounts[r];

    public void Add(Resource r, int amount)
    {
        if (amount < 0) throw new ArgumentException("Use Spend for negative amounts.");
        _amounts[r] += amount;
    }

    public bool Spend(Resource r, int amount)
    {
        if (amount < 0) throw new ArgumentException("Spend amount must be non-negative.");
        if (_amounts[r] < amount) return false;
        _amounts[r] -= amount;
        return true;
    }

    public IReadOnlyDictionary<Resource, int> Snapshot() => _amounts;
}
```

O dicionário é `private readonly` — o código fora da classe não pode alcançá-lo. A única forma de mudá-lo é por meio de `Add` e `Spend`, e os dois verificam suas entradas primeiro. `Snapshot()` o devolve como um `IReadOnlyDictionary` — o código de fora pode ler os quatro valores em um loop, mas não pode escrever neles.

Atualize `Kingdom.cs`:

```csharp
namespace Kingdom.Engine;

public class Kingdom
{
    public string Name { get; }
    public List<Building> Buildings { get; } = new();
    public List<Citizen> Citizens { get; } = new();
    public ResourceLedger Resources { get; } = new();

    public Kingdom(string name)
    {
        Name = name;
        Resources.Add(Resource.Gold, 100);
        Resources.Add(Resource.Wood, 50);
        Resources.Add(Resource.Stone, 20);
        Resources.Add(Resource.Food, 30);
    }

    public void AddBuilding(Building b) => Buildings.Add(b);
    public void AddCitizen(Citizen c) => Citizens.Add(c);
}
```

## Passo 4 — reescreva o `Program.cs`

`Kingdom.Console/Program.cs`:

```csharp
using Kingdom.Engine;

var kingdom = new Kingdom.Engine.Kingdom("Eldoria");
kingdom.AddBuilding(new Building("Main Farm"));
kingdom.AddBuilding(new Building("Old Mine"));
kingdom.AddCitizen(new Citizen("Lyra"));

PrintKingdom(kingdom);

void PrintKingdom(Kingdom.Engine.Kingdom k)
{
    Console.WriteLine($"== {k.Name} ==");
    Console.WriteLine($"Buildings ({k.Buildings.Count}):");
    foreach (var b in k.Buildings)
        Console.WriteLine($"  - {b.Name} (level {b.Level})");
    Console.WriteLine($"Citizens ({k.Citizens.Count}):");
    foreach (var c in k.Citizens)
        Console.WriteLine($"  - {c.Name}: {c.Job}");
    Console.WriteLine("Resources:");
    foreach (var (resource, count) in k.Resources.Snapshot())
        Console.WriteLine($"  {resource}: {count}");
}
```

Repare que o tipo é `Kingdom.Engine.Kingdom` — o nome completo. Escrevemos o nome completo porque `Kingdom` é também o nome de um namespace, então o C# precisa saber qual dos dois você quer dizer. (Você vai ver essa mesma situação de novo no Módulo 1.4, com o prefixo `global::` nos testes. É o mesmo tipo de confusão para o compilador.)

## Passo 5 — build e run

```powershell
dotnet build
dotnet run --project Kingdom.Console
```

Mesmo resultado do 1.1. Mas o layout é completamente diferente.

## Mexa um pouco

Abra `Kingdom.Engine/Kingdom.Engine.csproj`. Não tem linha `<OutputType>Exe</OutputType>` — é por isso que é uma library, não um programa que você pode rodar. Tente adicionar uma. A build falha, porque o engine não tem método `Main`. Tire a linha de volta. O engine não tem nada para *rodar* por conta própria; é uma coisa que outros projetos usam.

Tente adicionar `Console.WriteLine("hello");` a um método no `Kingdom`. Compila, mas você quebrou a regra. O engine não tem permissão para imprimir. Se algo dentro do engine precisa dizer algo, ele retorna um valor, e a shell decide o que fazer com ele.

Tente mudar `_amounts` no `ResourceLedger` de `private` para `public`. O compilador ainda fica feliz. Mas agora o código de fora poderia entrar e escrever o que quiser. Por que isso é ruim? Porque o ledger deveria recusar quantias negativas e recusar gastos excessivos, e chamar `_amounts.Add(...)` diretamente pula ambas essas verificações. Torne-o privado de novo.

## A linha-guia

O curso tem uma regra que continuamos voltando a ela — a **linha-guia**. Ela parece um pouco diferente em cada módulo, mas a ideia por baixo fica igual. Neste módulo a regra é: **o engine nunca referencia a shell. A shell referencia o engine.** Se você quiser chamar `Console.WriteLine` de dentro do engine, isso é o engine tentando se ligar ao console. A correção é sempre a mesma — o engine retorna um valor, e a shell decide o que imprimir.

Você vai ver essa regra crescer conforme o curso avança. Na Fase 2 o engine retorna dados de salvamento, e a shell de salvamento os escreve em um arquivo. Na Fase 3 o engine retorna um resultado, e a shell web o transforma em JSON. O engine nunca sabe como está sendo usado. Porque não sabe, o mesmo engine pode funcionar em cinco lugares diferentes.

## O que você acabou de fazer

Você pegou um único projeto e o dividiu em dois — um engine que guarda as regras do reino, e um console que fala com uma pessoa. Você adicionou o `ResourceLedger`, a primeira classe cujo trabalho todo é *proteger* um dicionário de ser usado de jeito errado. Você escreveu uma project reference em uma direção (console depende do engine) e nunca o contrário. Essa única direção é todo o ponto: o engine é a parte que você vai manter, e o console é a parte que você vai substituir quatro vezes este ano. Mesmo resultado do 1.1, mas o código está organizado diferente, e essa nova organização é a ideia principal do curso.

**Conceitos que você já sabe nomear:**

- **engine vs shell** — regras vs tudo que fala com o exterior
- **class library** — projeto `.dll`, sem `Main`, usado por outros
- **project reference** — seta de dependência de um jeito só entre projetos
- **solution** — arquivo que agrupa projetos para compilar juntos
- **read-only wrapping** — classe ao redor de um dicionário, acesso controlado

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a única grande ideia pegou: o engine e a shell, e qual tem permissão de usar a outra. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Sem olhar, responda isso em voz alta ou no papel:

1. Qual projeto depende do outro — o engine depende do console, ou o console do engine?
2. O engine tem uma regra que nunca pode quebrar. Qual é ela?
3. Se algo dentro do engine precisa dizer uma mensagem ao jogador, o que deve fazer, já que não tem permissão de imprimir?

Depois teste a regra 2 você mesmo: abra qualquer arquivo do engine e adicione `Console.WriteLine("hi");` dentro de um método. Ainda compila — o compilador não vai te parar. Mas você quebrou a regra. Tire a linha de volta.

<details><summary>Travou? Abra aqui para conferir.</summary>

- O **console depende do engine**, nunca o contrário. A project reference aponta em um jeito só: `Kingdom.Console` → `Kingdom.Engine`.
- A regra: **o engine nunca fala com o mundo exterior.** Sem `Console.WriteLine`, sem arquivos, sem rede. Esse é o trabalho da shell.
- Se o engine precisar dizer algo, ele **retorna um valor**, e a shell decide o que imprimir. É por isso que o mesmo engine pode depois rodar em um browser ou em Roblox — ele não sabe nem se importa qual shell está usando ele.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 1.2 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 1.2 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos do painel/CLI se você precisar relembrar. Leve as respostas do quiz das quais você tiver menos certeza para a próxima conversa semanal.

## Próximo

O Módulo 1.3 apresenta testes unitários. Os testes são sobre o engine, não a shell — e agora que o engine é seu próprio projeto, um projeto de testes pode referenciá-lo sem puxar o console também. A divisão que você fez hoje é o que torna a próxima aula possível.
