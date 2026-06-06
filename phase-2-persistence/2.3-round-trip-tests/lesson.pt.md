# Módulo 2.3 — Testes de Ida e Volta

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Ontem a gente salvou um *resumo* do reino. Hoje a gente salva o *reino inteiro* e carrega de volta. O teste que prova que isso funciona é um **teste de ida e volta** (round-trip test): qualquer reino que eu salvar deve bater consigo mesmo quando eu carregar de volta.

Adicionar suporte a carregamento também faz a gente mudar uma pequena parte do engine. Isso não é um problema — é parte da lição. Salvar estado real muitas vezes mostra coisas no modelo que precisam de ajuste, mesmo quando pareciam bem antes. Salvar coloca pressão honesta no seu modelo e mostra onde ele é fraco.

> **Words to watch**
>
> - **round-trip** — salvar, depois carregar; o resultado carregado deve ser igual ao original
> - **snapshot** — uma imagem de dados completa do reino em um momento no tempo
> - **factory method** — um método estático que retorna uma instância, usado no lugar de (ou junto com) um construtor; ex.: `Kingdom.LoadFrom(snap, ...)`
> - **property-based testing** — escrever *um* teste que afirma que algo é verdade para *qualquer* entrada; a gente vai fazer uma versão leve com um loop.

---

## Por que ida e volta

Bugs de salvamento são fáceis de perder. O arquivo de salvamento parece bem no Bloco de Notas. O carregamento retorna *algum* reino. Mas coisas pequenas podem errar: o `Level` de um prédio volta para 1, o trabalho de um cidadão se perde, a ordem dos recursos muda. Você não percebe até o jogador carregar um save três semanas depois e reclamar.

A correção: um teste que diz *"salve este reino, carregue de volta, e confira que tudo bate."* Rode em muitos reinos aleatórios. Se algum falhar, a forma JSON está errada. Um padrão de teste, e muita confiança de que salvar funciona.

## Salvar força design

Para fazer a ida e volta de um reino, precisamos *reconstruí-lo* a partir de dados. O construtor atual de `Kingdom` recebe `(name, IRandom, IClock)` e deixa você chamar `AddBuilding(...)` e `AddCitizen(...)`. Mas `Day` tem só `private set;`, então não tem como carregar `Day = 47`. O mesmo para `Building.Level`. O mesmo para `Resources` (você pode usar `Add`, mas o snapshot pode dizer `Gold = 250`).

A gente tem três opções:

**A.** Tornar esses setters públicos. Inseguro — agora qualquer um poderia mudar o estado de um jeito que não deveria.
**B.** Adicionar um método de fábrica estático `LoadFrom` que sabe como configurá-los. Mais limpo.
**C.** Usar reflection no carregamento para contornar os setters. Inteligente, mas frágil e difícil de seguir.

A opção B é a resposta padrão. A gente adiciona um pequeno *factory method* (um método estático que constrói e retorna um objeto) em `Kingdom`. Ele recebe um record de snapshot e retorna um `Kingdom` completamente carregado.

> **Lição dentro da lição:** adicionar suporte a carregamento muitas vezes faz você mudar o modelo. Isso é bom, não um problema. Um modelo que "parece certo" às vezes só parece certo para um shell. Salvar faz você encarar a forma real dos dados.

## Delta starter

- **NOVO:** `Kingdom.Engine/Snapshots/KingdomSnapshot.cs` (records: `KingdomSnapshot`, `BuildingSnapshot`, `CitizenSnapshot`)
- **MODIFICADO:** `Kingdom.Engine/Kingdom.cs` (adiciona `ToSnapshot()`, `static LoadFrom(snap, rng, clock)`)
- **MODIFICADO:** `Kingdom.Engine/Buildings/Building.cs` (adiciona construtor `protected` `(string name, int level)` para carregamento)
- **MODIFICADO:** `Kingdom.Engine/Buildings/Farm.cs`, `Lumberyard.cs`, `Mine.cs` (construtor de carregamento em cada)
- **MODIFICADO:** `Kingdom.Persistence/KingdomJsonStore.cs` (ganha `SaveFull` / `LoadFull` retornando um `Kingdom`)
- **NOVO:** `tests/Kingdom.Persistence.Tests/RoundTripTests.cs`

## Passo 1 — record `KingdomSnapshot`

`Kingdom.Engine/Snapshots/KingdomSnapshot.cs`:

```csharp
namespace Kingdom.Engine.Snapshots;

public record KingdomSnapshot(
    string Name,
    int Day,
    int Gold, int Wood, int Stone, int Food,
    BuildingSnapshot[] Buildings,
    CitizenSnapshot[] Citizens
);

public record BuildingSnapshot(string Kind, string Name, int Level);

public record CitizenSnapshot(string Name);
```

Um layout simples — arrays de records pequenos. Fácil para JSON, fácil de ler.

`Kind` é uma string: `"Farm"`, `"Lumberyard"`, `"Mine"`. A gente vai usá-la para escolher o tipo certo de prédio durante o carregamento.

## Passo 2 — `Building` ganha um construtor de carregamento

`Kingdom.Engine/Buildings/Building.cs`:

```csharp
namespace Kingdom.Engine.Buildings;

public class Building
{
    public string Name { get; }
    public int Level { get; private set; } = 1;

    public Building(string name) { Name = name; }

    // Para carregar: reconstruir um prédio em um nível específico
    protected Building(string name, int level) { Name = name; Level = level; }

    public void Upgrade() => Level++;

    public virtual void Tick(ResourceLedger ledger) { }
}
```

`protected` significa que só as subclasses podem usar este construtor. O jeito normal de fazer um prédio continua o mesmo (começa no nível 1).

`Farm.cs`, `Lumberyard.cs`, `Mine.cs` ganham um construtor correspondente cada:

```csharp
public class Farm : Building
{
    public Farm(string name) : base(name) { }
    public Farm(string name, int level) : base(name, level) { }   // para carregar
    public override void Tick(ResourceLedger ledger) => ledger.Add(Resource.Food, 5 * Level);
}
```

(O mesmo arranjo para Lumberyard e Mine.)

## Passo 3 — `Kingdom.ToSnapshot()` e `Kingdom.LoadFrom(...)`

Em `Kingdom.cs`, adicione:

```csharp
public KingdomSnapshot ToSnapshot()
{
    var buildings = Buildings
        .Select(b => new BuildingSnapshot(b.GetType().Name, b.Name, b.Level))
        .ToArray();
    var citizens = Citizens
        .Select(c => new CitizenSnapshot(c.Name))
        .ToArray();

    return new KingdomSnapshot(
        Name, Day,
        Resources.Get(Resource.Gold),
        Resources.Get(Resource.Wood),
        Resources.Get(Resource.Stone),
        Resources.Get(Resource.Food),
        buildings, citizens);
}

public static Kingdom LoadFrom(KingdomSnapshot snap, IRandom rng, IClock clock)
{
    var k = new Kingdom(snap.Name, rng, clock);

    // Os recursos são configurados no construtor — sobrescreva para os valores do snapshot
    k.Resources.SetTo(Resource.Gold, snap.Gold);
    k.Resources.SetTo(Resource.Wood, snap.Wood);
    k.Resources.SetTo(Resource.Stone, snap.Stone);
    k.Resources.SetTo(Resource.Food, snap.Food);

    foreach (var bs in snap.Buildings)
    {
        Building b = bs.Kind switch
        {
            "Farm"        => new Farm(bs.Name, bs.Level),
            "Lumberyard"  => new Lumberyard(bs.Name, bs.Level),
            "Mine"        => new Mine(bs.Name, bs.Level),
            _ => throw new InvalidOperationException($"Unknown building kind '{bs.Kind}'.")
        };
        k.AddBuilding(b);
    }
    foreach (var cs in snap.Citizens)
        k.AddCitizen(new Citizen(cs.Name));

    k._day = snap.Day;   // veja o passo 4
    return k;
}
```

`SetTo` é um novo método em `ResourceLedger` (Passo 5). `_day` é um novo campo privado de apoio (Passo 4).

## Passo 4 — `Kingdom.Day` vira um campo de apoio

Para configurar `Day` a partir de um método estático, precisamos de um jeito de escrever nele. Duas opções:

- Tornar o setter de `Day` `internal` (para ficar visível dentro do engine).
- Adicionar um campo privado `_day` que a propriedade lê.

A gente vai com o campo. Em `Kingdom.cs`:

```csharp
private int _day = 1;
public int Day => _day;

public void AdvanceDay()
{
    foreach (var b in Buildings) b.Tick(Resources);
    foreach (var _ in Citizens) Resources.Spend(Resource.Food, 1);

    var evt = _eventEngine.RollOnce(this);
    if (evt is not null) EventLog.Add(evt);

    _day++;
}
```

`Day` é somente leitura de fora da classe. `_day` ainda pode ser mudado de dentro. O lado de fora ainda vê a mesma coisa — mas `LoadFrom` agora pode escrever em `_day` diretamente.

## Passo 5 — `ResourceLedger.SetTo`

Em `Kingdom.Engine/Resources/ResourceLedger.cs`, adicione:

```csharp
public void SetTo(Resource r, int amount)
{
    if (amount < 0) throw new ArgumentException("Amount must be non-negative.");
    _amounts[r] = amount;
}
```

Mesma verificação que `Add`. É usado só para carregamento (e em testes). Não use dentro da lógica do jogo.

## Passo 6 — Persistence: `SaveFull` / `LoadFull`

Em `Kingdom.Persistence/KingdomJsonStore.cs`, adicione:

```csharp
using Kingdom.Engine.Snapshots;

// ... Save/Load (resumo) existente continua ...

public void SaveFull(Kingdom.Engine.Kingdom kingdom, string path)
{
    var snap = kingdom.ToSnapshot();
    File.WriteAllText(path, JsonSerializer.Serialize(snap, Options));
}

public Kingdom.Engine.Kingdom LoadFull(string path, IRandom rng, IClock clock)
{
    var snap = JsonSerializer.Deserialize<KingdomSnapshot>(File.ReadAllText(path))
        ?? throw new InvalidOperationException("Could not deserialize snapshot.");
    return Kingdom.Engine.Kingdom.LoadFrom(snap, rng, clock);
}
```

## Passo 7 — testes de ida e volta

`tests/Kingdom.Persistence.Tests/RoundTripTests.cs`:

```csharp
using Kingdom.Engine;
using Kingdom.Engine.Buildings;
using Kingdom.Engine.Citizens;
using Kingdom.Engine.Infrastructure;
using Kingdom.Persistence;
using Shouldly;

namespace Kingdom.Persistence.Tests;

public class RoundTripTests
{
    [Fact]
    public void Empty_Kingdom_Roundtrips()
    {
        var k = new global::Kingdom.Engine.Kingdom("Empty");
        Roundtrip(k).Name.ShouldBe("Empty");
    }

    [Fact]
    public void NameAndDay_Survive_Roundtrip()
    {
        var k = new global::Kingdom.Engine.Kingdom("Test", new SystemRandom(7), new SystemClock());
        for (int i = 0; i < 25; i++) k.AdvanceDay();
        var loaded = Roundtrip(k);
        loaded.Name.ShouldBe("Test");
        loaded.Day.ShouldBe(26);
    }

    [Fact]
    public void Buildings_AndLevels_Survive_Roundtrip()
    {
        var k = new global::Kingdom.Engine.Kingdom("X");
        var f = new Farm("F"); f.Upgrade(); f.Upgrade();   // nível 3
        k.AddBuilding(f);
        k.AddBuilding(new Mine("M"));
        k.AddBuilding(new Lumberyard("L"));

        var loaded = Roundtrip(k);

        loaded.Buildings.Count.ShouldBe(3);
        loaded.Buildings.OfType<Farm>().Single().Level.ShouldBe(3);
        loaded.Buildings.OfType<Mine>().Single().Name.ShouldBe("M");
    }

    [Fact]
    public void Resources_Survive_Roundtrip()
    {
        var k = new global::Kingdom.Engine.Kingdom("X");
        k.Resources.Add(Resource.Gold, 999);

        var loaded = Roundtrip(k);

        loaded.Resources.Get(Resource.Gold).ShouldBe(1099);  // 100 inicial + 999 adicionados
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(200)]
    public void AnyKingdom_Roundtrips(int days)
    {
        // Estilo de propriedade: mesma configuração, varia a contagem de dias
        var k = new global::Kingdom.Engine.Kingdom("Sweep", new SystemRandom(42), new SystemClock());
        k.AddBuilding(new Farm("F"));
        k.AddBuilding(new Lumberyard("L"));
        k.AddCitizen(new Citizen("A"));
        for (int i = 0; i < days; i++) k.AdvanceDay();

        var loaded = Roundtrip(k);

        loaded.Day.ShouldBe(k.Day);
        loaded.Buildings.Count.ShouldBe(k.Buildings.Count);
        foreach (var resource in Enum.GetValues<Resource>())
            loaded.Resources.Get(resource).ShouldBe(k.Resources.Get(resource));
    }

    private static Kingdom.Engine.Kingdom Roundtrip(Kingdom.Engine.Kingdom k)
    {
        var path = Path.Combine(Path.GetTempPath(), $"rt-{Guid.NewGuid():N}.json");
        try
        {
            var store = new KingdomJsonStore();
            store.SaveFull(k, path);
            return store.LoadFull(path, new SystemRandom(0), new SystemClock());
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}
```

O teste `[Theory]` roda quatro vezes — cada `[InlineData]` é uma rodada. Quando os quatro passam, a ida e volta funciona para todo esse grupo de reinos. Esse é o início de property-based testing.

Rode:

```powershell
dotnet test
```

Espere `Passed: 51` (43 + 8: 5 fatos de ida e volta + a 1 teoria expandindo para 4 casos — na verdade 4 fatos + 1 teoria × 4 entradas = 8).

## Mexa um pouco

Adicione um quinto `[InlineData]` — `999`. Ele roda em menos de um segundo. A ida e volta é rápida.

Edite um arquivo JSON salvo na mão: mude `"Gold": 100` para `"Gold": 999999`. Recarregue. O reino agora tem 999999 de ouro. JSON é honesto — o que estiver no arquivo vira o estado.

Apague o campo `Kind` de um prédio num snapshot. Recarregue. Ele joga um erro no `switch` — *"Unknown building kind ''."* Essa é uma falha boa: clara, e acontece imediatamente.

Adicione uma quarta subclasse de prédio (uma `Quarry`, talvez?). Adicione ao `switch`. O teste de ida e volta passa sem nenhuma mudança — porque o teste faz um loop sobre o que o reino realmente contém.

## O que você acabou de fazer

Você provou que salvar e carregar mantém o reino inteiro intacto — cinco fatos de ida e volta mais um `[Theory]` que roda quatro contagens de dias (51 testes no total, oito novos hoje). Para chegar lá, você mudou três pequenas partes do engine: um construtor `protected` em `Building`, um campo de apoio para `Day`, e um `SetTo` em `ResourceLedger`. Nenhum deles estava *errado* antes, mas não te davam um jeito de configurar o estado no carregamento — o tipo de lacuna pequena que salvar é bom em encontrar. Você também conheceu o padrão **factory method** (`Kingdom.LoadFrom`) e o início de property-based testing (`[Theory] + [InlineData]`).

**Conceitos que você já sabe nomear:**

- **round-trip** — salvar, carregar, afirmar que são iguais
- **snapshot** — forma completa de dados do modelo num momento
- **factory method** — `static` retornando uma instância
- **construtor `protected`** — só subclasses podem chamá-lo
- **property-based testing (leve)** — uma afirmação em muitas entradas

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — mostre para você mesmo, da sua própria cabeça, que a ideia grande pegou: um teste de ida e volta salva algo, carrega de volta, e confere que os dois batem. Ninguém corrige isso — bem, o executor de testes faz, e esse é o ponto. É o jeito mais rápido de descobrir o que ainda não pegou, enquanto ainda é pequeno para consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra o arquivo de teste de ida e volta. Sem olhar para os outros, escreva um novo `[Fact]` que:

1. Cria um reino e avança 30 dias.
2. Salva completo, e carrega de volta pelo helper `Roundtrip`.
3. Confere que o `Day` carregado é igual ao `Day` original.
4. Rode `dotnet test` — deve passar.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
[Fact]
public void Day_Survives_30DayRoundtrip()
{
    var k = new global::Kingdom.Engine.Kingdom("Mine", new SystemRandom(1), new SystemClock());
    for (int i = 0; i < 30; i++) k.AdvanceDay();

    var loaded = Roundtrip(k);

    loaded.Day.ShouldBe(k.Day);   // os dois são 31
}
```

A forma é sempre a mesma: construa, salve+carregue, afirme que o carregado é igual ao original. Se o teste passar, a forma de salvamento mantém `Day`. Se falhar, você encontrou um bug de salvamento de verdade — é um teste de ida e volta cumprindo sua função.

</details>

## Movimento git da semana — `git stash`

Você começou uma mudança. No meio do caminho, outra coisa apareceu — uma correção rápida, um experimento, ou uma lição que você queria começar com uma árvore limpa. *Stash* guarda suas mudanças atuais sem fazer commit, e te deixa com uma árvore de trabalho limpa.

No painel do Source Control do VS Code: abra o menu `...` (canto superior direito do painel) → *Stash → Stash*. Digite uma descrição, aperte Enter. Suas mudanças somem de *Changes*. Para recuperá-las: menu `...` → *Stash → Pop Latest Stash*.

> **Ou no terminal:**
>
> ```powershell
> git stash push -m "halfway through the round-trip test"
> git stash pop          # traz de volta
> ```

Um stash é seguro — suas mudanças estão salvas, não perdidas. Use sempre que quiser uma árvore limpa sem jogar fora o que você tinha.

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 2.3 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 2.3 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos pelo painel/CLI se você precisar relembrar. Leve as respostas do quiz de que você tiver menos certeza para a próxima conversa semanal.

## Próximo

O Módulo 2.4 apresenta o **SQL** — a linguagem que os bancos de dados falam. A gente vai configurar o SQLite (um banco de dados que vive em um único arquivo) e escrever nosso primeiro `INSERT` e `SELECT`. Arquivos JSON são ótimos para *um* save. Bancos de dados são ótimos para *cada save que o jogador já fez*, e você pode fazer perguntas sobre eles em qualquer direção.
