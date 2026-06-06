# Módulo 1.1 — OOP Basics

> Tente primeiro o `lesson.md` em inglês. Use este aqui só quando uma palavra te travar.

Hoje o Kingdom começa. Você vai criar quatro novos tipos — `Resource`, `Building`, `Citizen`, `Kingdom` — e a partir deles construir um pequeno reino medieval que imprime no seu terminal: dois prédios, um cidadão e um tesouro. Nada disso *faz* algo ainda. Tudo bem. Primeiro o reino precisa de um corpo; ensinamos ele a se mover no Módulo 1.4.

Antes de qualquer código, vale um minuto na grande mudança que começa aqui — porque o resto do curso inteiro está construído sobre ela. Esta é a maior ideia nova do curso, então vá devagar hoje. Não se espera que você se sinta fluente no fim de uma aula; espera-se que você tenha *encontrado* ela. A coisa vai encaixando nos próximos módulos, conforme você for usando.

### O que muda hoje

Na Fase 0, um programa era uma lista de passos, com alguns dados por perto. No Caravan Ledger você tinha dois dicionários — `crates` e `price` — alinhados na mão, unidos pelo nome da caravana. Funcionava, mas *você* era quem mantinha os dois fatos juntos. Nada no código em si dizia "esses pertencem à mesma caravana." Conforme um programa cresce, esse tipo de organização solta é exatamente onde os bugs se escondem.

**Programação orientada a objetos** (OOP) é uma forma diferente de organizar código. Em vez de dados em um lugar e os passos que os mudam em outro lugar, você os agrupa em uma coisa só — um **objeto**. Um objeto `Building` guarda seu próprio nome e nível *e* sabe como se atualizar. Os fatos e as coisas que você pode fazer com eles vivem na mesma caixa. Um objeto `Caravan` guardaria suas caixas e seu preço juntos — então eles nunca podem se separar, porque agora são *uma coisa só*, não dois dicionários que você tem que lembrar de manter alinhados.

É essa a mudança que você está fazendo: de um monte de variáveis soltas para um mundo feito de **coisas que cuidam de si mesmas**. O Kingdom é um encaixe perfeito, porque ele realmente *é* feito de coisas — prédios, cidadãos, recursos.

### Classe e objeto — o modelo e a coisa construída a partir dele

Você descreve um tipo de coisa uma vez, em uma **classe**. A classe é um modelo: ela diz o que todo prédio *tem* (um nome, um nível) e o que ele *pode fazer* (se atualizar). Então `new Building("Main Farm")` cria um prédio real a partir desse modelo. A coisa que você recebe de volta é um **objeto** (também chamado de *instância*). Uma classe, muitos objetos — um modelo de `Building`, e quantas fazendas, minas e serrarias você quiser.

```text
   CLASS  (the blueprint)              OBJECTS  (made with `new`)

   +-----------------------+        new Building("Main Farm")
   |  Building             |          +----------------------+
   |  has:  Name, Level    |  ----->  | Name:  "Main Farm"   |
   |  can:  Upgrade()      |          | Level: 1             |
   +-----------------------+          +----------------------+
       one blueprint...
                                    new Building("Old Mine")
                                      +----------------------+
                                      | Name:  "Old Mine"    |
                                      | Level: 1             |
                                      +----------------------+
                                        ...many objects
```

É a mesma ideia que a planta de uma casa: um papel, muitas casas reais construídas a partir dele. Cada casa é uma coisa separada, mas todas vieram do mesmo plano.

Não tente memorizar as palavras abaixo ainda. Você vai encontrar cada uma no código, onde faz muito mais sentido do que qualquer definição. A lista é só para que você saiba o que observar enquanto avança.

> **Words to watch**
>
> - **class** — um modelo para criar objetos
> - **object** (também *instance*) — uma coisa criada a partir de uma classe com `new`
> - **property** — um valor com nome em um objeto, com `get` e um `set` opcional
> - **constructor** — o método especial que roda quando um objeto é criado (`new Building(...)`)
> - **encapsulation** — uma classe escondendo seus valores internos atrás de métodos e propriedades, para que o código fora da classe não possa mudá-los diretamente
> - **enum** — um conjunto nomeado de valores permitidos (ex.: `Resource.Gold`, `Resource.Wood`)
> - **`new`** — a palavra-chave do C# que chama um construtor e te dá de volta um objeto novo

---

## Abertura de fase — crie uma branch para o trabalho da Fase 1

Rode isso primeiro, antes de qualquer código:

```powershell
cd C:\code\kingdom
git switch -c phase-1
```

Você está agora em uma *branch* chamada `phase-1`. Uma branch é uma linha separada de trabalho no git. De agora até o fim da Fase 1, seus commits vão para `phase-1` em vez de `main`. No Módulo 1.10 (o fim da Fase 1), você vai trazer todo esse trabalho de volta para o `main` por meio de um **pull request**. Um pull request é o jeito que o GitHub deixa alguém revisar seu trabalho antes de entrar no `main`. Lars revisa, aprova, e você faz o merge. O motivo: o `main` fica como a linha de *trabalho bom e revisado*, e o pull request no fim da fase mostra toda a sua fase de uma vez para revisar. É assim que times reais fazem mudanças.

**Se "branch" e "pull request" parecerem confusos agora, isso é esperado.** É a primeira vez que você os viu. O que você realmente precisa fazer é pequeno. Rode um comando no começo de cada fase (`git switch -c phase-N`) e abra um pull request no github.com no fim. É isso por hoje. Você vai entender o que branches e pull requests realmente são ao longo dos próximos dez módulos. O Módulo 1.8 volta às branches depois que você tiver usado uma por algumas semanas. O Módulo 1.10 percorre o passo do pull request por completo. O Bônus B3 (muito mais tarde) vai fundo em como o git funciona por dentro, se você quiser.

Por agora: rode o comando, confirme que está na branch nova, e siga em frente. Depois de algumas semanas usando branches, a ideia para de parecer estranha.

Confirme:

```powershell
git status
```

A primeira linha deve dizer *"On branch phase-1"*. Agora a aula de verdade.

---

## Passo 1 — comece um projeto novo

Crie um novo projeto para o Kingdom:

```powershell
cd <your-repo-root>
dotnet new console -n KingdomConsole
cd KingdomConsole
```

Você vai criar cinco arquivos aqui. Cada um guarda uma classe (ou um enum). A regra normal no C# é um tipo por arquivo, e o nome do arquivo combina com o nome do tipo. Vamos seguir essa regra.

## Passo 2 — `Resource.cs`, o enum de recursos

```csharp
namespace KingdomConsole;

public enum Resource
{
    Gold,
    Wood,
    Stone,
    Food
}
```

Um *enum* é um conjunto fixo de valores com nome. Em qualquer lugar que você use `Resource`, as únicas opções permitidas são `Resource.Gold`, `Resource.Wood`, `Resource.Stone`, `Resource.Food`. O compilador recusa qualquer outra coisa. Essa é a parte boa: você não pode passar `42` ou `"goldd"` por acidente para algo que espera um recurso. É como um menu suspenso onde você só pode escolher entre as quatro opções dadas.

## Passo 3 — `Building.cs`

```csharp
namespace KingdomConsole;

public class Building
{
    public string Name { get; }
    public int Level { get; private set; } = 1;

    public Building(string name)
    {
        Name = name;
    }

    public void Upgrade()
    {
        Level++;
    }
}
```

Três coisas para ler com cuidado aqui. `Name` é uma property — um valor com nome no prédio. Ela só tem `get`, sem `set`. Isso significa que você pode ler `b.Name` de fora, mas nunca pode escrever `b.Name = "outra coisa"`. Uma vez que um prédio é criado, seu nome não muda. `Level` tem `get` e `private set`. Qualquer um pode ler, mas só o código dentro da classe `Building` pode mudar. Começa em `1`. Para aumentar o nível, você chama `Upgrade()`. Isso é encapsulation: o código fora da classe não define os números ele mesmo. Ele pede para a classe fazer algo, e a classe decide o que muda.

A linha `public Building(string name)` é o **constructor**. Ele tem o mesmo nome que a classe. Quando você escreve `new Building("Main Farm")`, esse constructor roda uma vez. Seu trabalho é configurar as coisas. Aqui ele copia o parâmetro `name` na property `Name`.

## Passo 4 — `Citizen.cs`

```csharp
namespace KingdomConsole;

public class Citizen
{
    public string Name { get; }
    public string Job { get; set; } = "Idle";

    public Citizen(string name)
    {
        Name = name;
    }
}
```

Mesmo padrão — `Name` é só leitura, `Job` pode ser lida e escrita e começa em `"Idle"`. O reino muda o trabalho com o tempo. O nome fica igual.

## Passo 5 — `Kingdom.cs`

```csharp
namespace KingdomConsole;

public class Kingdom
{
    public string Name { get; }
    public List<Building> Buildings { get; } = new();
    public List<Citizen> Citizens { get; } = new();
    public Dictionary<Resource, int> Resources { get; } = new();

    public Kingdom(string name)
    {
        Name = name;
        Resources[Resource.Gold] = 100;
        Resources[Resource.Wood] = 50;
        Resources[Resource.Stone] = 20;
        Resources[Resource.Food] = 30;
    }

    public void AddBuilding(Building b) => Buildings.Add(b);
    public void AddCitizen(Citizen c) => Citizens.Add(c);
}
```

Um `Kingdom` tem três coleções — seus prédios, seus cidadãos e seus recursos — mais um nome. O constructor enche o tesouro para começar: 100 de ouro, 50 de madeira, 20 de pedra, 30 de comida. Os dois métodos curtos no fundo deixam o código de fora adicionar prédios e cidadãos. Eles usam a forma **expression-bodied** do C# — `=>` em vez de `{ ... }` para métodos de uma linha. Mesmo significado, menos digitação.

## Passo 6 — `Program.cs`

```csharp
using KingdomConsole;

var kingdom = new Kingdom("Eldoria");
kingdom.AddBuilding(new Building("Main Farm"));
kingdom.AddBuilding(new Building("Old Mine"));
kingdom.AddCitizen(new Citizen("Lyra"));

Console.WriteLine($"== {kingdom.Name} ==");
Console.WriteLine($"Buildings ({kingdom.Buildings.Count}):");
foreach (var b in kingdom.Buildings)
    Console.WriteLine($"  - {b.Name} (level {b.Level})");

Console.WriteLine($"Citizens ({kingdom.Citizens.Count}):");
foreach (var c in kingdom.Citizens)
    Console.WriteLine($"  - {c.Name}: {c.Job}");

Console.WriteLine("Resources:");
foreach (var (resource, count) in kingdom.Resources)
    Console.WriteLine($"  {resource}: {count}");
```

Rode:

```powershell
dotnet run
```

Você deve ver Eldoria impresso — dois prédios, um cidadão, quatro recursos. Tudo fica na memória do computador, e desaparece no momento em que o programa termina. Salvar no disco vem na Fase 2.

## Mexa um pouco

Adicione um terceiro prédio em `Program.cs` e rode de novo. Depois chame `kingdom.Buildings[0].Upgrade()` antes do loop de impressão — o nível do primeiro prédio deve aparecer agora como 2.

Tente escrever `kingdom.Buildings[0].Name = "New Name";`. O compilador vai recusar — `Name` não tem setter. Bom. É a encapsulation funcionando: um reino não pode renomear um de seus próprios prédios por acidente.

Adicione um método no `Kingdom` chamado `HireCitizen` que recebe um nome, cria um `Citizen` e o adiciona à lista. Use-o de `Program.cs` em vez da versão longa. O reino faz o trabalho; o programa só pede por isso.

## O que você acabou de fazer

Você escreveu quatro classes e viu elas se conectarem — um `Kingdom` que possui listas de `Building` e `Citizen` e um dicionário de `Resource`. Você conheceu as peças de uma classe que vai usar todo dia daqui para frente: properties com `get` e `private set`, um constructor que configura as coisas, e a diferença entre uma classe (o modelo) e um objeto (a coisa que você recebeu de `new`). Você também viu a encapsulation em ação quando o compilador recusou deixar o código de fora reescrever o nome de um prédio. Cinco arquivos de C# de verdade, e um reino imprime no terminal — tudo vivendo na memória pelos poucos segundos que o programa roda.

**Conceitos que você já sabe nomear:**

- **class vs object** — modelo versus a coisa construída a partir dele
- **property** — valor com nome com `get` e `set` opcional
- **constructor** — o método especial que roda no `new`
- **encapsulation** — a classe controla quem pode mudar o quê
- **enum** — conjunto fixo de valores com nome, verificado pelo compilador

## Por sua conta

Hora de fechar o livro. Não role de volta para os passos — prove para você mesmo, da sua própria cabeça, que a única grande ideia pegou: escrever uma classe. Ninguém corrige isto — é só para você. É o jeito mais fácil de descobrir o que *não* pegou ainda, enquanto ainda é simples de consertar. Travar aqui é totalmente normal — é exatamente para isso que serve.

Abra um arquivo novo e vazio. Sem olhar, escreva uma classe pequena chamada `Wall`:

1. Uma property `Name` que pode ser lida mas não mudada de fora (só `get`).
2. Uma property `Height` que começa em `1` e só pode ser mudada de dentro da classe (um `private set`).
3. Um constructor que recebe um nome e define `Name`.
4. Um método `Raise()` que adiciona `1` a `Height`.
5. Depois em `Program.cs`: crie um `Wall` com `new`, chame `Raise()` duas vezes e imprima seu nome e altura. Rode.

<details><summary>Travou? Abra aqui para conferir.</summary>

```csharp
public class Wall
{
    public string Name { get; }
    public int Height { get; private set; } = 1;

    public Wall(string name)
    {
        Name = name;
    }

    public void Raise()
    {
        Height++;
    }
}
```

```csharp
var wall = new Wall("North Wall");
wall.Raise();
wall.Raise();
Console.WriteLine($"{wall.Name} is at height {wall.Height}");
```

Height deve imprimir como `3`. Se você tentar `wall.Name = "South Wall";` a build falha — `Name` só tem `get`. Isso é encapsulation: a classe decide o que o código de fora pode mudar.

</details>

## Fechamento

1. **Quiz** — abra o `quiz.md`, anote suas respostas no `journal/quiz-notes.md`.
2. **Progresso** — uma linha no `journal/progress.md`: `Module 1.1 — Title — DATE — short build summary. Learnt: one sentence.`
3. **Commit e push** — prepare os dois arquivos, mensagem de commit `Module 1.1 done`, Sync.
4. **Poste no `#wins`** — uma linha sobre hoje, mais a URL do commit.

O Módulo 0.1 cobre o porquê e os passos do painel/CLI se você precisar relembrar. Leve as respostas do quiz das quais você tiver menos certeza para a próxima conversa semanal.

## Próximo

Por enquanto tudo está em um projeto só. Tudo bem por uma aula. O Módulo 1.2 divide isso em partes: as regras do reino (Building, Citizen, Resource, Kingdom) se movem para a própria *class library*, e o programa se torna uma camada fina por cima. Essa mudança é de onde vem o nome do resto do curso.
